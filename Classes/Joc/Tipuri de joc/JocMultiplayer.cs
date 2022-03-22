using System;
using System.Diagnostics;
using System.Net;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class JocMultiplayer : EngineJoc, IDisposable
    {
        private Om _jucatorServer;
        private Om _jucatorClient;

        private NetworkServer _server;
        private NetworkClient _client;
        private ParserTabla _parserTabla;

        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaHost;
        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaClient;

        private int _timpTimere;
        private bool _esteHost;
        private bool _esteClient;

        private bool _esteRandulHostului;
        private bool _esteRandulClientului;

        private string _ultimulMesajPrimitClient;
        private string _ultimulMesajPrimitServer;

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

            _esteRandulClientului = false;
            _esteRandulHostului = false;

            _esteClient = false;
            _esteHost = false;
            _timpTimere = 100;

            _ultimulMesajPrimitClient = "";
            _ultimulMesajPrimitServer = "";

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1),new Pozitie(1, 1));
            _ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, ConstantaTabla.LungimeMesajDiferential);
        }
        public JocMultiplayer(Form parentForm, int[,] matriceTabla, ref Tuple<Om, Om> jucatori) : base(parentForm, matriceTabla)
        {
            _jucatorServer = jucatori.Item1;
            _jucatorClient = jucatori.Item2;

            _esteRandulClientului = false;
            _esteRandulHostului = false;

            _esteClient = false;
            _esteHost = false;
            _timpTimere = 100;

            _ultimulMesajPrimitClient = "";
            _ultimulMesajPrimitServer = "";

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));
            _ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

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

        private void RealizeazaMutareaOnline(Piesa piesa, Pozitie pozitie)
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
                        EsteRandulClientului();
                    }
                    if (_esteClient)
                    {
                        _client.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                        EsteRandulHostului();
                    }
                    RealizeazaMutarea(piesa, pozitie);
                }
            }
        }

        public void EsteRandulClientului()
        {
            _esteRandulHostului = false;
            _esteRandulClientului = true;
        }

        public void EsteRandulHostului()
        {
            _esteRandulClientului = false;
            _esteRandulHostului = true;
        }
        public void RealizeazaMutarea(Piesa piesa, Pozitie pozitie)
        {
            if (MatriceCodPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
            {
                DecoloreazaMutariPosibile(PozitiiMutariPosibile);
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
                DecoloreazaMutariPosibile(PozitiiMutariPosibile);
                ActualizeazaUltimaMutare(piesa.Pozitie, pozitie);
                MatriceCodPiese[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                MatriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(ConstantaTabla.PiesaNula);

                piesa.Pozitie = pozitie;
                this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);

                ConstantaSunet.SunetPiesaMutata.Play();
            }
            PiesaSelectata = ConstantaTabla.PiesaNula;
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
            if (_server != null && _server.ClientPrimit == true)
                _server.TrimiteDate(date);
        }

        public void HosteazaJoc(int port)
        {
            _server = new NetworkServer(IPAddress.Any, port);
            _server.AcceptaUrmatorulClient();
            AsteaptaComunicareaCuClientul();
            _esteHost = true;
            _esteRandulHostului = true;
            _esteRandulClientului = false;
        }

        public async void ConecteazateLaJoc(IPAddress adresaIP, int port)
        {
            _client = new NetworkClient(adresaIP, port);
            await _client.PornesteCerereaDeConectare();
            AsteaptaComunicareaCuServerul();
            _esteClient = true;
            _esteRandulHostului = true;
            _esteRandulClientului = false;
        }


        private void AsteaptaComunicareaCuClientul()
        {
            if (_timerClientAcceptat == null)
            {
                _timerClientAcceptat = new();
                _timerClientAcceptat.Interval = _timpTimere;
                _timerClientAcceptat.AutoReset = true;
                _timerClientAcceptat.Enabled = true;
                _timerClientAcceptat.Elapsed += new ElapsedEventHandler(TrimiteTabla);
                _timerClientAcceptat.Start();
            }
        }
        private void TrimiteTabla(object source, ElapsedEventArgs e)
        {
            if (_server.ClientPrimit == true)
            {
                _timerClientAcceptat.Dispose();
                _server.TrimiteDate(_parserTabla.CodificareTabla(this.MatriceCodPiese));
                PornesteTimerJocServerSide();
            }
        }
        private void PrimesteTabla(object source, ElapsedEventArgs e)
        {
            if (_client.Conectat == true && (!_client.Buffer.Equals(NetworkClient.BufferGol)))
            {
                _timerClientConectat.Dispose();
                EsteRandulClientului();
                ActualizeazaIntreagaTabla(_parserTabla.DecodificareTabla(_client.Buffer));
                PornesteTimerJocClientSide();
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
                _timerClientConectat.Elapsed += new ElapsedEventHandler(PrimesteTabla);
                _timerClientConectat.Start();
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
                _timerJocServer.Elapsed += new ElapsedEventHandler(PrimesteDateleHost);
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
                _timerJocClient.Elapsed += new ElapsedEventHandler(PrimesteDateClient);
                _timerJocClient.Start();
            }
        }

        public void PrimesteDateClient(object source, ElapsedEventArgs e)
        {
            if (_client.Conectat == true)
            {
                if (!(_ultimulMesajPrimitClient.Equals(_client.Buffer)))
                {
                    if (!_client.Buffer.Equals(_client.MesajDeconectare))
                    {
                        if (_client.Buffer != NetworkClient.BufferGol && _client.Buffer.Length <= ConstantaTabla.LungimeMesajDiferential)
                        {
                            if (!_ultimaMutarePrimitaClient.Equals(UltimaMutare))
                            {
                                _ultimaMutarePrimitaClient = _parserTabla.DecodificareMutare(_client.Buffer);
                                if (!UltimaMutare.Equals(_ultimaMutarePrimitaClient))
                                {
                                    Piesa ultimaPiesa = GetPiesaCuPozitia(_ultimaMutarePrimitaClient.Item1);
                                    if (ultimaPiesa != ConstantaTabla.PiesaNula)
                                    {
                                        Debug.WriteLine("Sincronizeaza jocul Client: " + _client.Buffer);
                                        _ultimulMesajPrimitClient = _client.Buffer;
                                        RealizeazaMutareaOnline(ultimaPiesa, _ultimaMutarePrimitaClient.Item2);
                                        EsteRandulClientului();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    //todo:deconecteaza-te
                }
            }
        }

        public void PrimesteDateleHost(object source, ElapsedEventArgs e)
        {
            if (_server.ClientPrimit == true)
            {
                if (!_ultimulMesajPrimitServer.Equals(_server.Buffer))
                {
                    if (!_server.Buffer.Equals(_server.MesajDeconectare))
                    {
                        if (_server.Buffer != NetworkServer.BufferGol && _server.Buffer.Length <= ConstantaTabla.LungimeMesajDiferential)
                        {
                            if (!_ultimaMutarePrimitaHost.Item2.Equals(UltimaMutare.Item2))
                            {
                                _ultimaMutarePrimitaHost = _parserTabla.DecodificareMutare(_server.Buffer);
                                if (!UltimaMutare.Equals(_ultimaMutarePrimitaHost))
                                {
                                    Debug.WriteLine("Sincronizeaza jocul Host: " + _server.Buffer);
                                    Piesa ultimaPiesa = GetPiesaCuPozitia(_ultimaMutarePrimitaHost.Item1);
                                    if (ultimaPiesa != ConstantaTabla.PiesaNula)
                                    {
                                        _ultimulMesajPrimitServer = _server.Buffer;
                                        RealizeazaMutareaOnline(ultimaPiesa, _ultimaMutarePrimitaHost.Item2);
                                        EsteRandulHostului();
                                    }
                                }
                            }
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
            if (_server != null && _server.ClientPrimit == true)
                _server.Dispose();
        }

        public void InchideClientul()
        {
            if (_client != null && _client.Conectat == true)
                _client.Dispose();
        }
        public async void OnCadranClick(object sender, EventArgs e)
        {
            if (_esteClient && _esteRandulClientului || _esteHost && _esteRandulHostului)
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
                            PiesaSelectata = piesa;
                        }
                        else
                        {
                            await ArataPozitieBlocata(pozitie);
                        }
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
}
