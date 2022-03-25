using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class HostJoc : EngineJoc
    {
        private static int _intervalTimere = 150;

        protected NetworkServer _host;
        protected ParserTabla _parserTabla;
        protected Om _jucatorHost;

        private Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaHost;
        protected System.Timers.Timer _timerJocHost;

        protected int _timpTimere;
        protected bool _esteRandulHostului;
        protected bool _timerJocClientDisposed;
        protected bool _timerJocHostDisposed;
        public Om JucatorOm
        {
            get { return _jucatorHost; }
        }
        public HostJoc(Form parentForm, Om jucator) : base(parentForm)
        {
            AdaugaEvenimentCadrane();
            _jucatorHost = jucator;

            _esteRandulHostului = false;

            _timpTimere = _intervalTimere;

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, ConstantaTabla.LungimeMesajDiferential);
        
            AdaugaEvenimentCadrane();
        }

        public HostJoc(Form parentForm, int[,] matriceTabla,Om jucator) : base(parentForm, matriceTabla)
        {
            AdaugaEvenimentCadrane();
            _jucatorHost = jucator;

            _esteRandulHostului = false;

            _timpTimere = _intervalTimere;

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

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
                    _host.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                    EsteRandulClientului();
                    RealizeazaMutareaLocal(piesa, pozitie);
                }
            }
        }
        public void EsteRandulClientului()
        {
            _esteRandulHostului = false;
        }

        public void EsteRandulHostului()
        {
            _esteRandulHostului = true;
        }
        public void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie)
        {
            if (piesa == null || pozitie == null)
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

        ~HostJoc() => Dispose();

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            Debug.WriteLine("Dispose JocMultiplayer");

            _timerJocHostDisposed = true;
            _timerJocHost.Stop();
            _host.TrimiteDate(_host.MesajDeconectare);
            _timerJocHost.Dispose();
        }
        public virtual async void HosteazaJoc(int port)
        {
            _host = new NetworkServer(IPAddress.Any, port);
            _host.AcceptaUrmatorulClient();
            await AsteaptaComunicareaCuClientul();
            EsteRandulHostului();
        }
        protected virtual async Task AsteaptaComunicareaCuClientul()
        {
            while (_host.ClientPrimit == false)
            {
                await Task.Delay(50);
            }
            _host.TrimiteDate(_parserTabla.CodificareTabla(this.MatriceCodPiese));
            _host.TimerCitireDate.Stop();
            PornesteTimerHost();
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
        public void PrimesteDateleHost(object source, ElapsedEventArgs e)
        {
            try
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
                                    _timerJocHostDisposed = true;
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
                                MessageBox.Show("Client Deconectat(Cod 3)", "Clientul s-a deconectat");
                            }
                        }
                    }
                }
            }
            catch { _timerJocHostDisposed = true; _timerJocHost.Stop(); }
        }

        public void OnCadranClick(object sender, EventArgs e)
        {
            if (PiesaSelectata == ConstantaTabla.PiesaNula)
            {
                if (!_esteRandulHostului)
                {
                    return;
                }
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;

                if (ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                {

                    Piesa piesa = GetPiesaCuPozitia(pozitie);

                    if (piesa != null)
                    {
                        if (_esteRandulHostului)
                        {
                            if (piesa.CuloarePiesa == CuloareJoc.Albastru)
                                return;
                        }
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
