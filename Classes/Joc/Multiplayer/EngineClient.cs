using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class EngineClient : EngineJoc
    {
        private static readonly int _intervalTimere = 100;

        protected Om _jucatorClient;

        protected NetworkClient _client;
        protected ParserTabla _parserTabla;

        Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaClient;

        protected System.Timers.Timer _timerJocClient;

        protected int _timpTimere;

        protected bool _esteRandulClientului;

        protected bool _timerJocClientDisposed;

        private String _ultimulMesajPrimitClient = NetworkClient.BufferGol;

        public Om JucatorOm
        {
            get { return _jucatorClient; }
        }
        public EngineClient(Form parentForm, Om jucator) : base(parentForm)
        {
            AdaugaEvenimentCadrane();
            _jucatorClient = jucator;

            _esteRandulClientului = false;

            _timpTimere = _intervalTimere;

            _ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala);
        }
        public EngineClient(Form parentForm, int[,] matriceTabla, Om jucator) : base(parentForm, matriceTabla)
        {
            AdaugaEvenimentCadrane();
            _jucatorClient = jucator;

            _esteRandulClientului = false;

            _timpTimere = _intervalTimere;

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
            PornesteTimerClient();
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
        private void PornesteTimerClient()
        {
            _timerJocClientDisposed = false;
            _timerJocClient = new();
            _timerJocClient.Interval = _timpTimere;
            _timerJocClient.AutoReset = true;
            _timerJocClient.Enabled = true;
            _timerJocClient.Elapsed += new ElapsedEventHandler(SincronizeazaClient);
            _timerJocClient.Start();
        }
        public void SincronizeazaClient(object source, ElapsedEventArgs e)
        {
            if (_timerJocClientDisposed == false)
            {
                if (_ultimulMesajPrimitClient.Equals(_client.Buffer))
                {
                    _client.PrimesteDate();
                }
                _ultimulMesajPrimitClient = _client.Buffer;
                if (_ultimulMesajPrimitClient != NetworkClient.BufferGol)
                {
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
                            if (piesa.CuloarePiesa == CuloareJoc.Alb)
                                return;
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
                            RealizeazaMutareaOnline(PiesaSelectata, pozitie);
                            if (MatriceCodPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                            {
                                ConstantaSunet.SunetPiesaLuata.Play();
                            }
                            else
                            {
                                ConstantaSunet.SunetPiesaMutata.Play();
                            }
                        }
                    }
                }
            }
        }
    }
}
