using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;

namespace ProiectVolovici
{
    public class JocMultiplayer : EngineJoc, IDisposable
    {
        private Om _jucatorServer;
        private Om _jucatorClient;

        private NetworkServer _server;
        private NetworkClient _client;
        private ParserTabla _parserTabla;

        private int _timpTimeout;

        private System.Timers.Timer _timerClientAcceptat;
        private System.Timers.Timer _timerClientConectat;

        public Om JucatorOm
        {
            get { return _jucatorServer; }
        }

        public Om JucatorMasina
        {
            get { return _jucatorClient; }
        }

        public JocMultiplayer(Form parentForm, ref Tuple<Om, Om> jucatori) : base(parentForm)
        {
            _jucatorServer = jucatori.Item1;
            _jucatorClient = jucatori.Item2;
            _timpTimeout = 200;
            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, 5);
        }
        public JocMultiplayer(Form parentForm, int[,] matriceTabla, ref Tuple<Om, Om> jucatori) : base(parentForm, matriceTabla)
        {
            _jucatorServer = jucatori.Item1;
            _jucatorClient = jucatori.Item2;
            _timpTimeout = 200;
            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, 5);
        }

        public void Dispose()
        {
                _jucatorServer = null;
                _jucatorClient = null;

            _server.Dispose();
            _client.Dispose();
            _parserTabla = null;

            _timpTimeout = 0;

            _timerClientAcceptat.Dispose();
            _timerClientConectat.Dispose();
        }

         ~JocMultiplayer() => Dispose();

        public void trimiteDate(String date)
        {
            if(_server != null)
                _server.TrimiteDate(date);
        }

        public void HosteazaJoc(int port)
        {
            _server = new NetworkServer(IPAddress.Any, port);
            _server.AcceptaConexiuneaUrmatoare();
            AsteaptaComunicareaCuClientul();
        }

        public void ConecteazateLaJoc(IPAddress adresaIP,int port)
        {
            _client = new NetworkClient(adresaIP,port);
            _client.PornesteCerereaDeConectare();
            AsteaptaComunicareaCuServerul();
        }

        public void trimiteTablaCatreClient()
        {
            _server.TrimiteDate(_parserTabla.CodificareTabla(this.MatriceCodPiese));
        }


        private void AsteaptaComunicareaCuClientul()
        {
            if (_timerClientAcceptat == null)
            {
                _timerClientAcceptat = new();
                _timerClientAcceptat.Interval = _timpTimeout;
                _timerClientAcceptat.AutoReset = true;
                _timerClientAcceptat.Enabled = true;
                _timerClientAcceptat.Elapsed += new ElapsedEventHandler(PregatesteTrimitereaTablei);
                _timerClientAcceptat.Start();
            }
        }
        private void PregatesteTrimitereaTablei(object source, ElapsedEventArgs e)
        {
            if (_server.Conectat == true)
            {
                _timerClientAcceptat.Stop();
                trimiteTablaCatreClient();
            }
        }

        private void AsteaptaComunicareaCuServerul()
        {
            if (_timerClientConectat == null)
            {
                _timerClientConectat = new();
                _timerClientConectat.Interval = _timpTimeout;
                _timerClientConectat.AutoReset = true;
                _timerClientConectat.Enabled = true;
                _timerClientConectat.Elapsed += new ElapsedEventHandler(PregatestePrimireaTablei);
                _timerClientConectat.Start();
            }
        }
        private void PregatestePrimireaTablei(object source, ElapsedEventArgs e)
        {
            if (_client.Conectat == true)
            {
                _timerClientConectat.Stop();
                ActualizeazaIntreagaTabla(_parserTabla.DecodificareTabla(_client.Buffer));
            }
        }

        public void InchideServerul()
        {
            if(_server != null)
                _server.Dispose();
        }

        public void InchideClientul()
        {
            if(_client != null)
                _client.Dispose();
        }

    }
}
