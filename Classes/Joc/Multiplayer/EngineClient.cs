using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class EngineClient : EngineJoc
    {
        public static int IntervalTimerPrimireDate = 50;

        protected Om _jucatorClient;

        protected NetworkClient _client;
        protected ParserTabla _parserTabla;

        private Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaClient;

        protected System.Timers.Timer _timerJocClient;

        protected bool _esteRandulClientului;

        protected bool _timerJocClientDisposed;

        private String _ultimulMesajPrimitClient = NetworkClient.BufferGol;

        public Om Jucator
        {
            get { return _jucatorClient; }
        }

        public EngineClient(Form parentForm, Om jucator) : base(parentForm)
        {
            AdaugaEvenimentCadrane();
            _jucatorClient = jucator;

            _esteRandulClientului = false;

            _ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala);
        }

        public EngineClient(Form parentForm, int[,] matriceTabla, Om jucator) : base(parentForm, matriceTabla)
        {
            AdaugaEvenimentCadrane();
            _jucatorClient = jucator;

            _esteRandulClientului = false;

            _ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala);
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
                    _client.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                    RealizeazaMutareaLocal(piesa, pozitie);
                    EsteRandulHostului();
                }
            }
        }

        public void EsteRandulClientului()
        {
            _esteRandulClientului = true;
        }

        public void EsteRandulHostului()
        {
            _esteRandulClientului = false;
        }

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            Debug.WriteLine("Dispose JocMultiplayer");
            _timerJocClientDisposed = true;
            _timerJocClient.Stop();
            _client.TrimiteDate(_client.MesajDeconectare);
            _timerJocClient.Dispose();
        }

        ~EngineClient() => Dispose();

        public virtual async void ConecteazateLaJoc(IPAddress adresaIP, int port)
        {
            _client = new NetworkClient(adresaIP, port);
            await _client.PornesteCerereaDeConectare();
            await PrimesteTablaAsincron();
            _client.TimerCitireDate.Stop();
            ActiveazaTimerRepetitiv(ref _timerJocClient, (uint)IntervalTimerPrimireDate, SincronizeazaClient);
            _client.Buffer = NetworkClient.BufferGol;
            EsteRandulHostului();
        }

        protected virtual async Task PrimesteTablaAsincron()
        {
            while (_client.Buffer.Equals(NetworkClient.BufferGol))
            {
                await Task.Delay(50);
            }
            ActualizeazaIntreagaTabla(_parserTabla.DecodificareTabla(_client.Buffer));
        }

        public void SincronizeazaClient(object source, ElapsedEventArgs e)
        {
            if (_timerJocClientDisposed == false)
            {
                if (_ultimulMesajPrimitClient.Equals(_client.Buffer))
                {
                    _client.PrimesteDate();
                }
                if (_client.Buffer != NetworkClient.BufferGol)
                {
                    _ultimulMesajPrimitClient = _client.Buffer;
                    if (!_ultimulMesajPrimitClient.Equals(_client.MesajDeconectare))
                    {
                        _ultimaMutarePrimitaClient = _parserTabla.DecodificareMutare(_ultimulMesajPrimitClient);
                        RealizeazaMutareaLocal(GetPiesaCuPozitia(_ultimaMutarePrimitaClient.Item1), _ultimaMutarePrimitaClient.Item2);
                        EsteRandulClientului();
                    }
                    else
                    {
                        _timerJocClientDisposed = true;
                        _timerJocClient.Stop();
                        MessageBox.Show("Server Deconectat(Cod 3)", "Server s-a deconectat");
                    }
                }
            }
        }

        public void OnCadranClick(object sender, EventArgs e)
        {
            if (_esteRandulClientului)
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
                            if (piesa.CuloarePiesa != _jucatorClient.Culoare)
                            {
                                return;
                            }
                            PiesaSelectata = piesa;
                            piesa.ArataMutariPosibile(this);
                            if (ExistaMutariPosibile() == true)
                            {
                                ArataPiesaSelectata(piesa);
                            }
                            else
                            {
                                ArataPozitieBlocata(pozitie);
                            }
                        }
                    }
                }
                else
                {
                    Pozitie pozitie = new Pozitie(0, 0);
                    pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                    pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;

                    if (PiesaSelectata.Pozitie != pozitie)
                    {
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
}