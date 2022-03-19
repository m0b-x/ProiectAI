using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ProiectVolovici
{
    class Server
    {
        private TcpListener _server;
        public Socket _socketServer;
        private String _dateServer;
        private Thread _thread;
        private StreamWriter _scriereServer;
        private StreamReader _citireServer;
        NetworkStream _streamServerScriere;
        NetworkStream _streamServerCitire;

        private String _mesajInchidereServer;
        private bool _threadDeschis;

        public Server(System.Net.IPAddress adresaIP, uint port)
        {
            _server = new TcpListener(adresaIP, 3000);
        }

        public void Start()
        {
            _thread = new Thread(new ThreadStart(CitireServer));
            _server.Start();
            _thread.Start();
            DeschideThread();
        }

        public void ScriereServer(String mesaj)
        {
            _streamServerScriere.Write(mesaj);
        }

        public void InchidereStreamuri()
        {
            _streamServerScriere.Close();
            _streamServerCitire.Close();
        }

        public void DeschideThread()
        {
            _threadDeschis = true;
        }

        public void InchideThread()
        {
            _threadDeschis = false;
        }

        public void InitializareStreamuri()
        {
            _streamServerCitire = new NetworkStream(_socketServer);
            _citireServer = new StreamReader(_streamServerCitire);
            _streamServerScriere = new NetworkStream(_socketServer);
            _scriereServer = new StreamWriter(_streamServerCitire);
            _scriereServer.AutoFlush = true;
        }

        public void PrelucreazaDatelePrimite()
        {
            string dateServer = _citireServer.ReadLine();
            if (dateServer == null)
                return;

            if (dateServer == _mesajInchidereServer)
            {
                InchideThread()
            }
            else
            {
                //fa ceva cu dateServer
            }
        }

        public void CitireServer()
        {

            while (_threadDeschis)
            {
                try
                {
                    _socketServer = _server.AcceptSocket();
                }
                catch(System.Exception eroare)
                {
                    Debug.WriteLine("Eroare socket: "+eroare);
                }
                try
                {
                    InitializareStreamuri();
                    while (_threadDeschis)
                    {
                        PrelucreazaDatelePrimite();
                    }
                    InchidereStreamuri();
                }
                catch (Exception eroare)
                {
                    Debug.WriteLine("Eroare thread: " + eroare);
                }
                finally
                {
                    _threadDeschis = false;
                }
            }

        }


    }
}
