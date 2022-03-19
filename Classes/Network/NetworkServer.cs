using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;

namespace ProiectVolovici
{
    public class NetworkServer
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

        public NetworkServer(System.Net.IPAddress adresaIP, uint port)
        {
            _server = new TcpListener(adresaIP, 3000);
        }

        public void StartServer()
        {
            DeschideThread();
            _thread = new Thread(new ThreadStart(CitireServer));
            _server.Start();
            _thread.Start();
        }

        public void ScriereServer(String mesaj)
        {
            _scriereServer.WriteLine(mesaj);
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
            Debug.WriteLine("GAY");
            string dateServer = _citireServer.ReadLine();
            if (dateServer == null)
                return;

            if (dateServer == _mesajInchidereServer)
            {
                InchideThread();
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
                    Debug.Write("DA");
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
