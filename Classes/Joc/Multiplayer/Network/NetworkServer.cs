using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class NetworkServer : IDisposable
    {
        private TcpListener _server;
        private IPAddress _adresaIP;
        private TcpClient _client;

        private string _mesajDeconectare;
        private bool _disposed;
        private int _port;
        private bool _clientPrimit;

        private NetworkStream _streamClient;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;

        private ManualResetEvent _clientPrimitEvent = new ManualResetEvent(false);

        public StreamReader StreamCitire
        {
            get { return _streamCitire; }
        }
        public StreamWriter StreamScriere
        {
            get { return _streamScriere; }
        }
        public bool Disposed
        {
            get { return _disposed; }
        }

        public TcpListener Server
        {
            get { return _server; }
        }

        public ManualResetEvent ClientPrimitEvent
        {
            get { return _clientPrimitEvent; }
        }
        public bool ClientPrimit
        {
            get { return _clientPrimit; }
        }

        public String MesajDeconectare
        {
            get { return _mesajDeconectare; }
        }

        public int Port
        {
            get { return _port; }
        }

        public NetworkServer(IPAddress adresaIP, int port)
        {
            _adresaIP = adresaIP;
            _port = port;

            _disposed = false;
            _clientPrimit = false;


            _mesajDeconectare = "{8,8,8,8}";
            try
            {
                _server = new TcpListener(adresaIP, port);
                _server.Start();
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie Constructor: {0}", exceptie.ToString());
            }
        }

        ~NetworkServer() => Dispose();


        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (_disposed == false)
            {
                _disposed = true;
                InchideServer();
                Debug.WriteLine("NetworkServer inchis!");
            }
            else
            {
                Debug.WriteLine("NetworkServer a fost deja inchis!");
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
                _streamScriere.WriteLine(date);
                Debug.WriteLine($"Date trimise catre client: {date}");
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine($"Exceptie functie server TrimiteDate: {exceptie.ToString()}");
            }
        }
        public async Task PrimesteClientAsync()
        {
            try
            {
                _client = await _server.AcceptTcpClientAsync();

                Debug.WriteLine("Serverul a primit conexiunea clientului");

                _clientPrimit = true;
                ClientPrimitEvent.Set();
                InitializeazaStreamuri();
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine($"Exceptie functie AcceptaConexiune: {exceptie.ToString()}");
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
            catch (Exception exceptie)
            {
                Debug.Write("Exceptie la InitializeazaStreamuri: {0}", exceptie.ToString());
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
            _client?.Close();
        }

        private void InchideServer()
        {
            try
            {
                if (_server.Server.IsBound == true && _server != null)
                {
                    _streamScriere.WriteLine(_mesajDeconectare);
                    InchideStreamuri();
                    _server.Stop();
                    InchideSocket();
                }
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie InchideServer: {0}", exceptie.ToString());
            }
        }
    }
}