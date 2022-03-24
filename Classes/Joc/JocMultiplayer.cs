using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class JocMultiplayer : EngineJoc, IDisposable
    {
        private static int _intervalTimere = 150;

        protected Om _jucatorServer;
        protected Om _jucatorClient;

        protected NetworkServer _host;
        protected NetworkClient _client;
        protected ParserTabla _parserTabla;

        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaHost;
        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaClient;

        protected int _timpTimere;
        protected bool _esteHost;
        protected bool _esteClient;

        protected bool _esteRandulHostului;
        protected bool _esteRandulClientului;

        protected System.Timers.Timer _timerJocClient;
        protected System.Timers.Timer _timerJocHost;

        protected bool _timerJocClientDisposed;
        protected bool _timerJocHostDisposed;

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
            _timpTimere = _intervalTimere;

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
            _timpTimere = _intervalTimere;

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
                        _host.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                        EsteRandulClientului();
                    }
                    if (_esteClient)
                    {
                        _client.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                        EsteRandulHostului();
                    }
                    RealizeazaMutareaLocal(piesa, pozitie);
                }
            }
        }

        public void EsteRandulClientului()
        {
            _esteRandulClientului = true;
            _esteRandulHostului = false;
        }

        public void EsteRandulHostului()
        {
            _esteRandulHostului = true;
            _esteRandulClientului = false;
        }
        public void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie)
        {
            if(piesa == null || pozitie == null)
            {
                return;
            }
            Pozitie pozitieInitiala = piesa.Pozitie;
            DecoloreazaMutariPosibile(PozitiiMutariPosibile);
            ActualizeazaUltimaMutare(pozitieInitiala, pozitie);
            SeteazaPiesaCadranului(pozitie, piesa);
            piesa.Pozitie = pozitie;
            SeteazaPiesaCadranului(pozitieInitiala, ConstantaTabla.PiesaNula);

            PiesaSelectata = ConstantaTabla.PiesaNula;
            PozitiiMutariPosibile.Clear();
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            Debug.WriteLine("Dispose JocMultiplayer");
            if (_esteHost)
            {
                _timerJocHostDisposed = true;
                _host.TrimiteDate(_host.MesajDeconectare);
                if(_host.Disposed == true)
                    _host.Dispose();
            }
            if (_esteClient)
            {
                _timerJocClientDisposed = true;
                _client.TrimiteDate(_client.MesajDeconectare);
                if(_client.Disposed == true)
                    _client.Dispose();
            }
        }

        ~JocMultiplayer() => Dispose();

        public virtual async void HosteazaJoc(int port)
        {
            _host = new NetworkServer(IPAddress.Any, port);
            _host.AcceptaUrmatorulClient();
            await AsteaptaComunicareaCuClientul();
            _esteHost = true;
            EsteRandulHostului();
        }

        public virtual async void ConecteazateLaJoc(IPAddress adresaIP, int port)
        {
            _client = new NetworkClient(adresaIP, port);
            await _client.PornesteCerereaDeConectare();
            await PrimesteTablaAsync();
            _client.TimerCitireDate.Stop();
            PornesteTimerClient();
            _esteClient = true;
            EsteRandulHostului();
        }


        protected virtual async Task AsteaptaComunicareaCuClientul()
        { 
            while(_host.ClientPrimit == false)
            {
                await Task.Delay(50);
            }
            _host.TrimiteDate(_parserTabla.CodificareTabla(this.MatriceCodPiese));
            _host.TimerCitireDate.Stop();
            PornesteTimerHost();
        }
        protected virtual async Task PrimesteTablaAsync()
        {
            while (_client.Buffer.Equals(NetworkClient.BufferGol))
            {
                await Task.Delay(50);
            }
            ActualizeazaIntreagaTabla(_parserTabla.DecodificareTabla(_client.Buffer));
        }

        private void PornesteTimerHost()
        {
            _timerJocHostDisposed = false;
            _timerJocHost = new();
            _timerJocHost.Interval = _timpTimere;
            _timerJocHost.AutoReset = true;
            _timerJocHost.Enabled = true;
            _timerJocHost.Elapsed += new ElapsedEventHandler(PrimesteDateleHost);
            _timerJocHost.Start();
        }

        private void PornesteTimerClient()
        {
            _timerJocClientDisposed = false;
            _timerJocClient = new();
            _timerJocClient.Interval = _timpTimere;
            _timerJocClient.AutoReset = true;
            _timerJocClient.Enabled = true;
            _timerJocClient.Elapsed += new ElapsedEventHandler(PrimesteDateClient);
            _timerJocClient.Start();
        }

        public void PrimesteDateClient(object source, ElapsedEventArgs e)
        {
            if (_timerJocClientDisposed == false)
            {
                if (_client.Buffer != null)
                {
                    if (_client.Buffer != NetworkClient.BufferGol && _client.Buffer.Length <= ConstantaTabla.LungimeMesajDiferential)
                    {
                        if (!_client.Buffer.Equals(_client.MesajDeconectare))
                        {
                            String ultimulMesajPrimitClient = _parserTabla.CodificareMutare(_ultimaMutarePrimitaClient.Item1, _ultimaMutarePrimitaClient.Item2);
                            while (ultimulMesajPrimitClient.Equals(_client.Buffer))
                            {
                                _client.PrimesteDate();
                            }
                            if (_client.Buffer != null)
                            {
                                _ultimaMutarePrimitaClient = _parserTabla.DecodificareMutare(_client.Buffer);
                                if (_ultimaMutarePrimitaClient != null)
                                {
                                    Piesa ultimaPiesa = GetPiesaCuPozitia(_ultimaMutarePrimitaClient.Item1);
                                    Debug.WriteLine("Sincronizeaza jocul Client: " + _client);
                                    RealizeazaMutareaLocal(ultimaPiesa, _ultimaMutarePrimitaClient.Item2);
                                    EsteRandulClientului();
                                    _client.PrimesteDate();
                                }
                            }
                        }
                        else
                        {
                            _timerJocClientDisposed = true;
                            _timerJocHost.Stop();
                            _client.Dispose();
                            MessageBox.Show("Server Deconectat(Cod 3)", "Server s-a deconectat");
                        }
                    }
                }
            }
            else
            {
                _timerJocClient.Stop();
            }
        }

        public void PrimesteDateleHost(object source, ElapsedEventArgs e)
        {
            if (_timerJocHostDisposed == false)
            {
                if (_host.Buffer != null)
                {
                    if (_host.Buffer != NetworkServer.BufferGol && _host.Buffer.Length <= ConstantaTabla.LungimeMesajDiferential)
                    {
                        if (!_host.Buffer.Equals(_host.MesajDeconectare))
                        {
                            String ultimulMesajPrimitHost = _parserTabla.CodificareMutare(_ultimaMutarePrimitaHost.Item1, _ultimaMutarePrimitaHost.Item2);
                            while (ultimulMesajPrimitHost.Equals(_host.Buffer))
                            {
                                _host.PrimesteDate();
                            }
                            if (_host.Buffer != null)
                            {
                                _ultimaMutarePrimitaHost = _parserTabla.DecodificareMutare(_host.Buffer);
                                if (_ultimaMutarePrimitaHost != null)
                                {
                                    Debug.WriteLine("Sincronizeaza jocul Host: " + _host.Buffer);
                                    Piesa ultimaPiesa = GetPiesaCuPozitia(_ultimaMutarePrimitaHost.Item1);
                                    RealizeazaMutareaLocal(ultimaPiesa, _ultimaMutarePrimitaHost.Item2);
                                    EsteRandulHostului();
                                    _host.PrimesteDate();
                                }
                            }
                        }
                        else
                        {
                            _timerJocHostDisposed = true;
                            _timerJocHost.Stop();
                            _host.Dispose();
                            MessageBox.Show("Client Deconectat(Cod 3)", "Clientul s-a deconectat");
                        }
                    }
                }
            }
        }

        public void OnCadranClick(object sender, EventArgs e)
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
                        if (piesa != null)
                        {
                            piesa.ArataMutariPosibile(this);
                        }
                        if (ExistaMutariPosibile() == true)
                        {
                            ArataPiesaSelectata(piesa);
                            PiesaSelectata = piesa;
                        }
                        else
                        {
                            ArataPozitieBlocata(pozitie);
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
                        if (MatriceCodPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                        {
                            ConstantaSunet.SunetPiesaLuata.Play();
                        }
                        else
                        {
                            ConstantaSunet.SunetPiesaMutata.Play();
                        }
                        RealizeazaMutareaOnline(PiesaSelectata, pozitie);
                    }
                }
            }
        }

    }
}
