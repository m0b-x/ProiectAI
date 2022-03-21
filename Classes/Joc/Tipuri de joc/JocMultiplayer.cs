using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;
using System.Diagnostics;
using System.Net.Sockets;

namespace ProiectVolovici
{
    public class JocMultiplayer : EngineJoc, IDisposable
    {
        private Om _jucatorServer;
        private Om _jucatorClient;

        private NetworkServer _server;
        private NetworkClient _client;
        private ParserTabla _parserTabla;

        private int _timpTimere;
        private bool _esteHost;
        private bool _esteClient;

        private System.Timers.Timer _timerClientAcceptat;
        private System.Timers.Timer _timerClientConectat;
        private System.Timers.Timer _timerJocClient;
        private System.Timers.Timer _timerJocServer;

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
            AdaugaEvenimentCadrane();
            _jucatorServer = jucatori.Item1;
            _jucatorClient = jucatori.Item2;
            _timpTimere = 20;
            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, ConstantaTabla.LungimeMesajDiferential);
        }
        public JocMultiplayer(Form parentForm, int[,] matriceTabla, ref Tuple<Om, Om> jucatori) : base(parentForm, matriceTabla)
        {
            _jucatorServer = jucatori.Item1;
            _jucatorClient = jucatori.Item2;
            _timpTimere = 20;
            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, ConstantaTabla.LungimeMesajDiferential);
        }

        public void AdaugaEvenimentCadrane()
        {
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    ArrayCadrane[linie, coloana].Click += OnCadranClick;
                }
            }
        }

        private void RealizeazaMutareaOnline(Piesa piesa,Pozitie pozitie)
        {
            Debug.WriteLine("MutaPiesaOnline: " + piesa.Pozitie.Linie + " " + piesa.Pozitie.Coloana + "->" + pozitie.Linie + " " + pozitie.Coloana);   

            if (pozitie.Linie > MarimeVerticala || pozitie.Coloana > MarimeOrizontala || pozitie.Linie < 0 || pozitie.Coloana < 0)
            {
                Debug.WriteLine("Linie sau coloana invalida! Linie:" + pozitie.Linie + ", Coloana:" + pozitie.Coloana);
            }
            else
            {
                if (piesa.PusaPeTabla == false)
                {
                    Debug.WriteLine("Eroare:Piesa nu este pusa pe tabla!");
                }
                else
                {           
                    if (_esteHost)
                    {
                        _server.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                    }
                    if (_esteClient)
                    {
                        _client.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                    }
                    RealizeazaMutarea(piesa, pozitie);
                }
            }
        }

        public void RealizeazaMutarea(Piesa piesa,Pozitie pozitie)
        {
            Debug.WriteLine("Mutare: " + pozitie.Linie + " " + pozitie.Coloana + "<-" + piesa.Pozitie.Linie + " " + pozitie.Coloana);
            if (MatriceCodPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
            {
                ActualizeazaUltimaMutare(piesa.Pozitie, pozitie);
                MatriceCodPiese[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(ConstantaTabla.PiesaNula);

                piesa.Pozitie = pozitie;
                MatriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);

                ListaPiese.Remove(GetPiesaCuPozitia(pozitie));
                ConstantaSunet.SunetPiesaLuata.Play();
            }
            else
            {
                ActualizeazaUltimaMutare(piesa.Pozitie, pozitie);
                MatriceCodPiese[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                MatriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(ConstantaTabla.PiesaNula);

                piesa.Pozitie = pozitie;
                this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);

                ConstantaSunet.SunetPiesaMutata.Play();
            }
            PiesaSelectata = ConstantaTabla.PiesaNula;
            DecoloreazaMutariPosibile(PozitiiMutariPosibile);
            PozitiiMutariPosibile.Clear();
        }

        public void Dispose()
        {
            _jucatorServer = null;
            _jucatorClient = null;

            _server.Dispose();
            _client.Dispose();
            _parserTabla = null;

            _timpTimere = 0;

            _timerClientAcceptat.Dispose();
            _timerClientConectat.Dispose();
            _timerJocClient.Dispose();
            _timerJocServer.Dispose();

            _esteClient = false;
            _esteHost = false;
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
            _esteHost = true;
        }

        public void ConecteazateLaJoc(IPAddress adresaIP,int port)
        {
            _client = new NetworkClient(adresaIP,port);
            _client.PornesteCerereaDeConectare();
            AsteaptaComunicareaCuServerul();
            _esteClient = true;
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
                _timerClientAcceptat.Interval = _timpTimere;
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
                trimiteTablaCatreClient();
                PornesteTimerJocServerSide();
                _timerClientAcceptat.Stop();
            }
        }

        private void AsteaptaComunicareaCuServerul()
        {
            if (_timerClientConectat == null)
            {
                _timerClientConectat = new();
                _timerClientConectat.Interval = _timpTimere;
                _timerClientConectat.AutoReset = true;
                _timerClientConectat.Enabled = true;
                _timerClientConectat.Elapsed += new ElapsedEventHandler(PregatestePrimireaTablei);
                _timerClientConectat.Start();
            }
        }
        private void PregatestePrimireaTablei(object source, ElapsedEventArgs e)
        {
            if (_client.Conectat == true && _client.Buffer != null)
            {
                ActualizeazaIntreagaTabla(_parserTabla.DecodificareTabla(_client.Buffer));
                PornesteTimerJocClientSide();
                _timerClientConectat.Stop();
            }
        }
        private void PornesteTimerJocServerSide()
        {
            if (_timerJocServer == null)
            {
                _timerJocServer = new();
                _timerJocServer.Interval = _timpTimere;
                _timerJocServer.AutoReset = true;
                _timerJocServer.Enabled = true;
                _timerJocServer.Elapsed += new ElapsedEventHandler(sincronizeazaJoculServerSide);
                _timerJocServer.Start();
            }
        }

        private void PornesteTimerJocClientSide()
        {
            if (_timerJocClient == null)
            {
                _timerJocClient = new();
                _timerJocClient.Interval = _timpTimere;
                _timerJocClient.AutoReset = true;
                _timerJocClient.Enabled = true;
                _timerJocClient.Elapsed += new ElapsedEventHandler(sincronizeazaJoculClientSide);
                _timerJocClient.Start();
            }
        }

        public void sincronizeazaJoculClientSide(object source, ElapsedEventArgs e)
        {
            if (_client != null && _client.Conectat == true)
            {
                Debug.WriteLine("Sincronizeaza Jocul Client:" + _client.Buffer);
                if (!_client.Buffer.Equals(_client.MesajDeconectare) && _client.Buffer.Length <= ConstantaTabla.LungimeMesajDiferential)
                {
                    Debug.WriteLine("Sincronizeaza jocul buffer server: " + _client.Buffer);
                    Tuple<Pozitie, Pozitie> ultimaMutarePrimita = UltimaMutare;
                    Tuple<Pozitie, Pozitie> ultimaMutareActuala = _parserTabla.DecodificareMutare(_client.Buffer);
                    if (!ultimaMutarePrimita.Equals(ultimaMutareActuala))
                    {
                        Piesa ultimaPiesa = GetPiesaCuPozitia(ultimaMutareActuala.Item1);
                        if (ultimaPiesa != null)
                        {
                            Debug.WriteLine("Sincronizeaza Client");
                            RealizeazaMutareaOnline(ultimaPiesa, ultimaMutareActuala.Item2);
                        }
                    }
                }
            }
            else
            {
                //todo:deconecteaza-te
            }
        }

        public void sincronizeazaJoculServerSide(object source, ElapsedEventArgs e)
        {
            if (_server != null && _server.Conectat == true)
            {
                Debug.WriteLine("Sincronizeaza Jocul Client:" + _server.Buffer);
                if (_server.Buffer != null &&!_server.Buffer.Equals(_server.MesajDeconectare) &&_server.Buffer.Length <= ConstantaTabla.LungimeMesajDiferential)
                {
                    Debug.WriteLine("Sincronizeaza jocul buffer server: " + _server.Buffer);
                    Tuple<Pozitie, Pozitie> ultimaMutarePrimita = UltimaMutare;
                    Tuple<Pozitie, Pozitie> ultimaMutareActuala = _parserTabla.DecodificareMutare(_server.Buffer);
                    if (!ultimaMutarePrimita.Equals(ultimaMutareActuala))
                    {
                        Piesa ultimaPiesa = GetPiesaCuPozitia(ultimaMutareActuala.Item1);
                        if (ultimaPiesa != null)
                        {
                            Debug.WriteLine("Sincronizeaza Client");
                            RealizeazaMutareaOnline(ultimaPiesa, ultimaMutareActuala.Item2);
                        }
                    }
                }
                else
                {
                    //todo:deconecteaza-te
                }
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
        public void OnCadranClick(object sender, EventArgs e)
        {
            if (PiesaSelectata == ConstantaTabla.PiesaNula)
            {
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;


                if (ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                {
                    Piesa piesa = GetPiesaCuPozitia(pozitie);
                    piesa.ArataMutariPosibile(this);
                    if (ExistaMutariPosibile() == true)
                    {
                        ArataPiesaSelectata(piesa);
                        Debug.WriteLine("Click Piesa:" + piesa.Cod + "->[linie:" + piesa.Pozitie.Linie + ",coloana:" + piesa.Pozitie.Coloana + "]");
                        PiesaSelectata = piesa;
                    }
                    else
                    {
                        ArataPiesaBlocata(pozitie);
                        return;
                    }
                }
                else
                {
                    Debug.WriteLine("Click Piesa:Gol->[linie:" + pozitie.Linie + ",coloana:" + pozitie.Coloana + "]");
                }
            }
            else
            {
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;

                if (PiesaSelectata.Pozitie == pozitie)
                {
                    return;
                }
                if (EsteMutareaPosibila(pozitie))
                {
                    AscundePiesaSelectata(PiesaSelectata);
                    Debug.WriteLine("Cadran Click");
                    RealizeazaMutareaOnline(PiesaSelectata, pozitie);
                }
                else
                {
                    return;
                }
            }
        }

    }
}
