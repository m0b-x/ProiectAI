using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Timers;

namespace ProiectVolovici
{
    public class NetworkClient : IDisposable
    {
        public static String BufferGol = System.String.Empty;
        public static readonly int TimeoutConexiune = 5000;

        private TcpClient _client;
        private IPAddress _adresaIP;
        private System.Timers.Timer _timerCitireDate;

        private int _port;
        private int _intervalTimeoutConexiune;
        private bool _disposed;
        private string _mesajDeconectare;
        private string _mesajConectare;

        private bool _conectat;
        private String _buffer;

        private NetworkStream _streamPrimireClient;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;

        public bool Disposed
        {
            get { return _disposed; }
        }

        public TcpClient Client
        {
            get { return _client; }
        }

        public System.Timers.Timer TimerCitireDate
        {
            get { return _timerCitireDate; }
            set { _timerCitireDate = value; }
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

        public String Buffer
        {
            get { return _buffer; }
            set { _buffer = value; }
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
            _intervalTimeoutConexiune = TimeoutConexiune;
            _buffer = BufferGol;
        }

        ~NetworkClient() => Dispose();

        //sursa : https://stackoverflow.com/questions/18486585/tcpclient-connectasync-get-status
        public async Task PornesteCerereaDeConectare()
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

                var taskTimeout = Task.Delay(_intervalTimeoutConexiune).
                    ContinueWith<TcpClient>(task => null, TaskContinuationOptions.ExecuteSynchronously);

                var rezultatConexiune = Task.WhenAny(taskConexiune, taskTimeout).Unwrap();

                await rezultatConexiune;
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
                Debug.WriteLine("Exceptie ConecteazaClientLaServer: {0}", exceptie.ToString());
            }
        }

        public void AscultaPentruDate()
        {
            if (_timerCitireDate == null)
            {
                _timerCitireDate = new();
                _timerCitireDate.Interval = 25;
                _timerCitireDate.AutoReset = true;
                _timerCitireDate.Enabled = true;
                _timerCitireDate.Elapsed += new ElapsedEventHandler(AscultaDate_Tick);
                _timerCitireDate.Start();
            }
        }

        private void AscultaDate_Tick(object source, ElapsedEventArgs e)
        {
            if (_streamPrimireClient != null)
            {
                PrimesteDate();
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

                _streamPrimireClient.Dispose();
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
                Debug.WriteLine("Date trimise de catre client:" + date);
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie client TrimiteDate: {0}", exceptie.ToString());
            }
        }

        public String PrimesteDate()
        {
            try
            {
                String date;
                date = _streamCitire.ReadLine();
                _buffer = date;
                if (date.Equals(_mesajDeconectare))
                {
                    Debug.WriteLine("Serverul s-a deconectat de la client`");
                }
                Debug.WriteLine("Date Primite de Client: " + date);
                return date;
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie networkclient TrimiteDate: {0}", exceptie.ToString());
                return BufferGol;
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