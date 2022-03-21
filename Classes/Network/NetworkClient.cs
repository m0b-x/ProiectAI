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
        private int _port;
        private int _timpTimeoutConexiune;
        private System.Timers.Timer _timerCitireDate;

        private NetworkStream _streamServer;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;

        public NetworkClient(IPAddress adresaIP, int port)
        {
            _adresaIP = adresaIP;
            _port = port;
            _client = new TcpClient();
            _timpTimeoutConexiune = 5000;
        }
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
            InchidereClient();
            _client.Dispose();
            _adresaIP = null;
            _port = 0;

            _streamServer.Dispose();
            _streamCitire.Dispose();
            _streamScriere.Dispose();

            Debug.WriteLine("NetworkClient Disposed!");
        }

        public void TrimiteDate(String date)
        {
            try
            {
                _streamScriere.WriteLine(date);
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
                Debug.WriteLine("Date Primite Client: " + date);
                return date;
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie TrimiteDate: " + exceptie);
            }
            return null;
        }

        private void AscultaDate_Tick(object source, ElapsedEventArgs e)
        {
            if (_streamServer != null)
                PrimesteDate();
        }

        private void InchidereClient ()
        {
            try
            { 
                if (_client != null)
                {
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
