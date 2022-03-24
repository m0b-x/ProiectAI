using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class JocMultiplayer : EngineJoc, IDisposable
    {
        private static int _intervalTimere = 50;

        private Om _jucatorServer;
        private Om _jucatorClient;

        private NetworkServer _host;
        private NetworkClient _client;
        private ParserTabla _parserTabla;

        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaHost;
        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaClient;

        private int _timpTimere;
        private bool _esteHost;
        private bool _esteClient;

        private bool _esteRandulHostului;
        private bool _esteRandulClientului;

        private System.Timers.Timer _timerJocClient;
        private System.Timers.Timer _timerJocHost;

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

        public new void Dispose()
        {
            if (_esteHost)
            {
                _host.TrimiteDate(_host.MesajDeconectare);
                _host.Dispose();
                _timerJocHost.Dispose();
            }
            else if (_esteClient)
            {
                _client.TrimiteDate(_client.MesajDeconectare);
                _client.Dispose();
                _timerJocClient.Dispose();
            }
        }

        ~JocMultiplayer() => Dispose();

        public async void HosteazaJoc(int port)
        {
            _host = new NetworkServer(IPAddress.Any, port);
            _host.AcceptaUrmatorulClient();
            await AsteaptaComunicareaCuClientul();
            _esteHost = true;
            EsteRandulHostului();
        }

        public async void ConecteazateLaJoc(IPAddress adresaIP, int port)
        {
            _client = new NetworkClient(adresaIP, port);
            await _client.PornesteCerereaDeConectare();
            await PrimesteTablaAsync();
            _client.TimerCitireDate.Stop();
            PornesteTimerClient();
            _esteClient = true;
            EsteRandulHostului();
        }


        private async Task AsteaptaComunicareaCuClientul()
        { 
            while(_host.ClientPrimit == false)
            {
                await Task.Delay(50);
            }
            _host.TrimiteDate(_parserTabla.CodificareTabla(this.MatriceCodPiese));
            _host.TimerCitireDate.Stop();
            PornesteTimerHost();
        }
        private async Task PrimesteTablaAsync()
        {
            while (_client.Buffer.Equals(NetworkClient.BufferGol))
            {
                await Task.Delay(50);
            }
            ActualizeazaIntreagaTabla(_parserTabla.DecodificareTabla(_client.Buffer));
        }

        private void PornesteTimerHost()
        {
            if (_timerJocHost == null)
            {
                _timerJocHost = new();
                _timerJocHost.Interval = _timpTimere;
                _timerJocHost.AutoReset = true;
                _timerJocHost.Enabled = true;
                _timerJocHost.Elapsed += new ElapsedEventHandler(PrimesteDateleHost);
                _timerJocHost.Start();
            }
        }

        private void PornesteTimerClient()
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
                            else
                            {
                                MessageBox.Show("Mesaj Server S", "Serverul s-a deconectat");
                                _timerJocClient.Stop();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mesaj Server", "Serverul s-a deconectat");
                        _timerJocClient.Stop();
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
                            else
                            {
                                MessageBox.Show("Client Deconectat S", "Clientul s-a deconectat");
                                _timerJocHost.Stop();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Client Deconectat", "Clientul s-a deconectat");
                        _timerJocHost.Stop();
                    }
                }
            }
            else
            {
                _timerJocHost.Stop();
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
                        piesa.ArataMutariPosibile(this);
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
                    else
                    {
                        return;
                    }
                }
            }
        }

    }
}
