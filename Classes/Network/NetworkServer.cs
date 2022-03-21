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
    public class NetworkServer : IDisposable
    {
        private TcpListener _server;
        private IPAddress _adresaIP;
        private Socket _socketListening;
        private System.Timers.Timer _timerCitireDate;

        private string _mesajDeconectare;
        private bool _disposed;
        private int _port;
        private bool _conectat;
        private string _buffer;

        private NetworkStream _streamClient;
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
        }

        public NetworkServer(IPAddress adresaIP,int port)
        {
            _adresaIP = adresaIP;
            _port = port;
            _disposed = false;
            _conectat = false;
            _mesajDeconectare = "0";
            try
            {
                _server = new TcpListener(adresaIP, port);
                _server.Start();
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie Constructor: " + exceptie);
            }
        }
        ~NetworkServer() => Dispose();

        public void AcceptaConexiuneaUrmatoare()
        {
            try
            {
                _server.BeginAcceptSocket( new AsyncCallback(AcceptaConexiuneSocket), _server);
            }
            catch(Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie AcceptaConexiune: " + exceptie);
            }
        }

        public void Dispose()
        {
            if (_disposed == false)
            {
                _disposed = true;
                InchideServer();
                _server = null;
                _adresaIP = null;
                _socketListening.Dispose();
                  _port = 0;

                _streamClient.Dispose();
                _streamCitire.Dispose();
                _streamScriere.Dispose();
                _timerCitireDate.Stop();
                _timerCitireDate.Dispose();
                Debug.WriteLine("NetworkServer sters!");
            }
            else
            {
                Debug.WriteLine("NetworkServer a fost deja sters!");
            }


        }

        public void TrimiteDate(String date)
        {
            if (_socketListening == null)
            {
                Debug.WriteLine("Streamurile serverului nu sunt initializate! ");
            }
            else
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
                if(date == _mesajDeconectare)
                {
                    Debug.WriteLine("Clientul s-a deconectat de la server");
                }
                Debug.WriteLine("Date Primite Server: " + date);
                _buffer = date;
                return date;
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie TrimiteDate: " + exceptie);
            }
            return null;
        }

        private void AcceptaConexiuneSocket(IAsyncResult rezultatAsincron)
        {
            _socketListening = _server.EndAcceptSocket(rezultatAsincron);
            Debug.WriteLine("Serverul a primit conexiunea clientului");
            _conectat = true;
            InitializeazaStreamuri();
            AscultaPentruDate();
        }

        private void AscultaPentruDate()
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
            if(_streamClient != null)
                PrimesteDate();
        }

        private void InitializeazaStreamuri()
        {
            try
            {
                _streamClient = new NetworkStream(_socketListening, true);
                if (_streamClient != null)
                {
                    _streamCitire = new StreamReader(_streamClient);
                    _streamScriere = new StreamWriter(_streamClient);
                    _streamScriere.AutoFlush = true;
                }
            }
            catch(Exception exceptie)
            {
                Debug.Write("Exceptie la InitializeazaStreamuri: " + exceptie);
            }

        }

        private void InchideStreamuri()
        {
            if (_streamClient != null)
            {
                _streamClient.Close();
                _streamCitire.Close();
                _streamScriere.Close();
            }
        }

        private void InchideSocket()
        {
            if (_socketListening != null)
            {
                _socketListening.Close();
            }
        }

        private void InchideServer()
        {
            try
            {
                if (_server.Server.IsBound == true && _server != null)
                {
                    _streamScriere.WriteLine(_mesajDeconectare);
                    _server.Stop();
                    InchideSocket();
                    InchideStreamuri();
                }
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie InchideServer: " + exceptie);
            }
        }
    }
}
