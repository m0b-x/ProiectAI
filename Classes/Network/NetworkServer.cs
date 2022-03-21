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

        private NetworkStream _streamClient;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;

        private int _port;

        private String _mesajInchidere = "_mesajInitial";

        public NetworkServer(IPAddress adresaIP,int port)
        {
            _adresaIP = adresaIP;
            _port = port;

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
            InchideServer();
            _server = null;
            _adresaIP = null;
            _socketListening.Dispose();

            _streamClient.Dispose();
            _streamCitire.Dispose();
            _streamScriere.Dispose();

            _port = 0;


            Debug.WriteLine("NetworkServer Disposed!");
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
                Debug.WriteLine("Date Primite Server: " + date);
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
