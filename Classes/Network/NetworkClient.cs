using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ProiectVolovici
{
    class NetworkClient : IDisposable
    {
        private TcpClient _client;
        private IPAddress _adresaIP;
        private System.Timers.Timer _timerCitireDate;

        private int _port;
        private int _timpTimeoutConexiune;
        private bool _disposed;
        private string _mesajDeconectare;
        private bool _conectat;
        private String _buffer;

        private NetworkStream _streamServer;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;
        public bool Conectat
        {
            get { return _conectat; }
        }
        public String MesajDeconectare
        {
            get { return _mesajDeconectare; }
        }

        public String Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
        }

        public NetworkClient(IPAddress adresaIP, int port)
        {
            _adresaIP = adresaIP;
            _port = port;
            _client = new TcpClient();
            _timpTimeoutConexiune = 5000;
            _disposed = false;
            _conectat = false;
            _mesajDeconectare = "0";
        }

        ~NetworkClient() => Dispose();

        //sursa : https://stackoverflow.com/questions/18486585/tcpclient-connectasync-get-status
        public void PornesteCerereaDeConectare()
        {
            String adresaIPString = _adresaIP.ToString();
            try
            {
                var taskConexiune = _client.
                    ConnectAsync(_adresaIP, _port).ContinueWith(task => 
                    {
                        return task.IsFaulted ? null : _client;
                    },
                    TaskContinuationOptions.ExecuteSynchronously);

                var taskTimeout = Task.Delay(_timpTimeoutConexiune).
                    ContinueWith<TcpClient>(task => null, TaskContinuationOptions.ExecuteSynchronously);

                var rezultatConexiune = Task.WhenAny(taskConexiune, taskTimeout).Unwrap();

                rezultatConexiune.Wait();
                var resultTcpClient = rezultatConexiune.Result;

                if (resultTcpClient != null)
                {
                    InitializareStreamuri();
                    AscultaPentruDate();
                    Debug.WriteLine("Clientul a fost conectat la server cu success!");
                    _conectat = true;
                }
                else
                {
                    Debug.WriteLine("Client nu a fost conectat! Motiv:Timeout ");
                }
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie ConecteazaClientLaServer: " + exceptie);
            }
        }
        public void AscultaPentruDate()
        {
            if (_timerCitireDate == null)
            {
                _timerCitireDate = new();
                _timerCitireDate.Interval = 100;
                _timerCitireDate.AutoReset = true;
                _timerCitireDate.Enabled = true;
                _timerCitireDate.Elapsed += new ElapsedEventHandler(AscultaDate_Tick);
                _timerCitireDate.Start();
            }
        }

        private void AscultaDate_Tick(object source, ElapsedEventArgs e)
        {
            if (_streamServer != null)
            {
                PrimesteDate();
            }
        }

        private void InchideStreamuri()
        {
            if (_streamServer != null)
            {
                _streamServer.Close();
                _streamCitire.Close();
                _streamScriere.Close();
            }
        }

        public void Dispose()
        {
            if (_disposed == false)
            {
                _disposed = true;
                InchidereClient();
                _client.Dispose();
                _adresaIP = null;
                _port = 0;

                _streamServer.Dispose();
                _streamCitire.Dispose();
                _streamScriere.Dispose();
                _timerCitireDate.Dispose();
                Debug.WriteLine("NetworkClient sters!");
            }
            else
            {
                Debug.WriteLine("NetworkClient a fost deja sters!");
            }
        }

        public void TrimiteDate(String date)
        {
            try
            {
                _streamScriere.WriteLine(date);
                _streamServer.Flush();
                Debug.WriteLine("Date trimise de catre client:" + date);
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie TrimiteDate: " + exceptie);
            }
        }

        public String PrimesteDate()
        {
            try
            {
                String date;
                date = _streamCitire.ReadLine();
                if (date == _mesajDeconectare)
                {
                    Debug.WriteLine("Serverul s-a deconectat de la client`");
                }
                Debug.WriteLine("Date Primite de Client: " + date);
                _buffer = date;
                return date;
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie TrimiteDate: " + exceptie);
            }
            return null;
        }

        private void InchidereClient ()
        {
            try
            { 
                if (_client != null)
                {
                    _streamScriere.WriteLine(_mesajDeconectare);
                    _client.Close();
                    InchideStreamuri();
                }
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie InchidereClient: " + exceptie);
            }
        }

        private void InitializareStreamuri()
        {
            _streamServer = _client.GetStream();
            _streamCitire = new StreamReader(_streamServer);
            _streamScriere = new StreamWriter(_streamServer);
            _streamScriere.AutoFlush = true;
        }
    }
}
