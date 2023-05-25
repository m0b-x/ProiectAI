using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class NetworkClient : IDisposable
    {
        public static readonly int TimeoutConexiune = 15000;

        private TcpClient _client;
        private IPAddress _adresaIP;

        private int _port;
        private bool _disposed;
        private string _mesajDeconectare;
        private string _mesajConectare;

        private bool _conectat;

        private NetworkStream _streamPrimireClient;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;

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

        public TcpClient Client
        {
            get { return _client; }
        }


        public bool Conectat
        {
            get { return _conectat; }
        }

        public String MesajDeconectare
        {
            get { return _mesajDeconectare; }
        }

        public String MesajConectare
        {
            get { return _mesajConectare; }
        }


        public NetworkClient(IPAddress adresaIP, int port)
        {
            _client = new TcpClient();
            _adresaIP = adresaIP;
            _port = port;

            _disposed = false;
            _conectat = false;

            _mesajDeconectare = "{8,8,8,8}";
            _mesajConectare = "1";
        }

        ~NetworkClient() => Dispose();

        public async Task PornesteCerereaDeConectare()
        {
            String adresaIPString = _adresaIP.ToString();
            try
            {
                Task<TcpClient> taskConexiune = _client.
                    ConnectAsync(_adresaIP, _port).ContinueWith(task =>
                    {
                        return task.IsFaulted ? null : _client;
                    },
                    TaskContinuationOptions.ExecuteSynchronously);

                Task<TcpClient> taskTimeout = Task.Delay(TimeoutConexiune).
                    ContinueWith<TcpClient>(task => null, TaskContinuationOptions.ExecuteSynchronously);

                Task<TcpClient> rezultatConexiune = Task.WhenAny(taskConexiune, taskTimeout).Unwrap();

                await rezultatConexiune;
                TcpClient resultTcpClient = rezultatConexiune.Result;

                if (resultTcpClient != null)
                {
                    InitializareStreamuri();
                    Debug.WriteLine("Clientul a fost conectat la server cu success!");
                    _conectat = true;
                }
                else
                {
                    Debug.WriteLine("Client nu a fost conectat! Motiv:Timeout ");
                    _conectat = false;
                }
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie ConecteazaClientLaServer: {0}", exceptie.ToString());
                _conectat = false;
            }
        }

        private void InchideStreamuri()
        {
            if (_streamPrimireClient != null)
            {
                _streamPrimireClient.Close();
                _streamCitire.Close();
                _streamScriere.Close();
            }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            if (_disposed == false)
            {
                _disposed = true;
                InchidereClient();
                _client.Dispose();

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
                Debug.WriteLine("Date trimise de catre client:" + date);
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie client TrimiteDate: {0}", exceptie.ToString());
            }
        }

        private void InchidereClient()
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
                Debug.WriteLine("Exceptie functie InchidereClient: {0}", exceptie.ToString());
            }
        }

        private void InitializareStreamuri()
        {
            _streamPrimireClient = _client.GetStream();
            _streamCitire = new StreamReader(_streamPrimireClient);
            _streamScriere = new StreamWriter(_streamPrimireClient);
            _streamScriere.AutoFlush = true;
        }
    }
}