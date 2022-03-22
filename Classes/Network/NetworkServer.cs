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
        public static String BufferGol = "";

        private TcpListener _server;
        private IPAddress _adresaIP;
        private TcpClient _client;
        private System.Timers.Timer _timerCitireDate;

        private string _mesajDeconectare;
        private bool _disposed;
        private int _port;
        private bool _clientPrimit;
        private string _buffer;

        private NetworkStream _streamClient;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;

        public TcpListener Server
        {
            get { return _server; }
        }

        public bool ClientPrimit
        {
            get {  return _clientPrimit; }
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

        public int Port
        {
            get { return _port; }
        }

        public NetworkServer(IPAddress adresaIP,int port)
        {
            _adresaIP = adresaIP;
            _port = port;

            _disposed = false;
            _clientPrimit = false;

            _buffer = BufferGol;

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

        public void AcceptaUrmatorulClient()
        {
            try
            {
                _server.BeginAcceptTcpClient( new AsyncCallback(AcceptaConexiuneClient), _server);
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
                _client.Dispose();

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
            if (_client == null)
            {
                Debug.WriteLine("Streamurile serverului nu sunt initializate! ");
            }
            else
                try
                {
                    Debug.WriteLine("Date trimise catre client:" + date); 
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
                if (_buffer != date)
                {
                    if (date == _mesajDeconectare)
                    {
                        Debug.WriteLine("Clientul s-a deconectat de la server");
                    }
                    Debug.WriteLine("Date Primite Server: " + date);
                    _buffer = date;
                    return date;
                }
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie TrimiteDate: " + exceptie);
            }
            return null;
        }

        private void AcceptaConexiuneClient(IAsyncResult rezultatAsincron)
        {
            TcpListener _server = (TcpListener)rezultatAsincron.AsyncState;
            _client = _server.EndAcceptTcpClient(rezultatAsincron);

            Debug.WriteLine("Serverul a primit conexiunea clientului");

            _clientPrimit = true;
            InitializeazaStreamuri();
            AscultaPentruDate();
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
            if (_streamClient != null)
            {
                PrimesteDate();
            }
        }

        private void InitializeazaStreamuri()
        {
            try
            {
                _streamClient = _client.GetStream();
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
            if (_client != null)
            {
                _client.Close();
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
