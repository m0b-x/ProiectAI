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
    class NetworkClient : IDisposable
    {
        private TcpClient _client;
        private IPAddress _adresaIP;
        private int _port;

        private NetworkStream _streamServer;
        private StreamReader _streamCitire;
        private StreamWriter _streamScriere;

        public NetworkClient(IPAddress adresaIP, int port)
        {
            _adresaIP = adresaIP;
            _port = port;
        }

        public void ConecteazaClientLaServer()
        {
            String adresaIPString = _adresaIP.ToString();
            try
            {
                _client = new TcpClient(adresaIPString, _port);
                InitializareStreamuri();
                Debug.WriteLine("Client Conectat(ClientSide): " + _client.Connected);
            }
            catch (Exception exceptie)
            {
                Debug.WriteLine("Exceptie ConecteazaClientLaServer: " + exceptie);
            }

        }

        public void InchidereClient ()
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

        public void InitializareStreamuri()
        {
            _streamServer = _client.GetStream();
            _streamCitire = new StreamReader(_streamServer);
            _streamScriere = new StreamWriter(_streamServer);
        }

        public void InchideStreamuri()
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
            _client = null;
            _adresaIP = null;
            _port = 0;

            _streamServer = null;
            _streamCitire = null;
            _streamScriere = null;

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
    }
}
