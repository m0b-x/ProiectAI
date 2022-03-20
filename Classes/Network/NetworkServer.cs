using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class NetworkServer : IDisposable
    {
        private TcpListener _server;
        private IPAddress _adresaIP;
        private Socket _socketClient;

        private NetworkStream _streamClient;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;

        private int _port;

        private String _mesajInitial = "_mesajInitial";

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

        public void AcceptaConexiuniExistente()
        {
            try
            {
                _socketClient = _server.AcceptSocket();
                _streamClient = new NetworkStream(_socketClient,true);
                Debug.WriteLine("Client conectat(ServerSide): "+_socketClient.Connected);
                InitializeazaStreamuri();
            }
            catch(Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie AcceptaConexiune: " + exceptie);
            }
        }

        public void InitializeazaStreamuri()
        {
            if (_streamClient != null)
            {
                _streamCitire = new StreamReader(_streamClient);
                _streamScriere = new StreamWriter(_streamClient);
                _streamScriere.AutoFlush = true;
            }
        }

        public void InchideStreamuri()
        {
            if (_streamClient != null)
            {
                _streamClient.Close();
                _streamCitire.Close();
                _streamScriere.Close();
            }
        }

        public void InchideSocket()
        {
            if (_socketClient != null)
            {
                _socketClient.Close();
            }
        }

        public void InchideServer()
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

        public void Dispose()
        {
            InchideServer();
            _server = null;
            _adresaIP = null;
            _socketClient = null;

            _streamClient = null;
            _streamCitire = null;
            _streamScriere = null;

            _port = 0;


            Debug.WriteLine("NetworkServer Disposed!");
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
                date =_streamCitire.ReadLine();
                Debug.WriteLine("Date Primite Server: " + date);
                return date;
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie functie TrimiteDate: " + exceptie);
            }
            return null;
        }
    }
}
