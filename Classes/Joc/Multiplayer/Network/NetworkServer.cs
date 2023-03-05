using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Timers;

namespace ProiectVolovici
{
	public class NetworkServer : IDisposable
	{
		public static String BufferGol = System.String.Empty;

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

		public bool Disposed
		{
			get { return _disposed; }
		}

		public System.Timers.Timer TimerCitireDate
		{
			get { return _timerCitireDate; }
			set { _timerCitireDate = value; }
		}

		public TcpListener Server
		{
			get { return _server; }
		}

		public bool ClientPrimit
		{
			get { return _clientPrimit; }
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

		public NetworkServer(IPAddress adresaIP, int port)
		{
			_adresaIP = adresaIP;
			_port = port;

			_disposed = false;
			_clientPrimit = false;

			_buffer = BufferGol;

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

		public void AcceptaUrmatorulClient()
		{
			try
			{
				_server.BeginAcceptTcpClient(new AsyncCallback(AcceptaConexiuneClient), _server);
			}
			catch (Exception exceptie)
			{
				Debug.WriteLine("Exceptie functie AcceptaConexiune: {0}", exceptie.ToString());
			}
		}

		public void Dispose()
		{
			GC.SuppressFinalize(this);
			if (_disposed == false)
			{
				_disposed = true;
				InchideServer();

				_timerCitireDate.Stop();
				_streamClient.Dispose();
				_streamCitire.Dispose();
				_streamScriere.Dispose();
				_timerCitireDate.Dispose();
				_client.Dispose();
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
					Debug.WriteLine("Date trimise catre client: {0}", date);
					_streamScriere.WriteLine(date);
				}
				catch (Exception exceptie)
				{
					Debug.WriteLine("Exceptie functie server TrimiteDate: {0}", exceptie.ToString());
				}
		}

		public String PrimesteDate()
		{
			try
			{
				String date;
				date = _streamCitire.ReadLine();
				_buffer = date;
				if (date == _mesajDeconectare)
				{
					Debug.WriteLine("Clientul s-a deconectat de la server");
				}
				Debug.WriteLine("Date Primite Server: {0}", date);
				return date;
			}
			catch (Exception exceptie)
			{
				Debug.WriteLine("Exceptie functie networkserver TrimiteDate: {0}", exceptie.ToString());
				return BufferGol;
			}
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
				_timerCitireDate.Interval = 25;
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
				Debug.WriteLine("Exceptie functie InchideServer: {0}", exceptie.ToString());
			}
		}
	}
}