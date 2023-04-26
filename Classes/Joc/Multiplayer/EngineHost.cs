using ProiectVolovici.Classes.Joc.Multiplayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class EngineHost : EngineJoc
    {
        public static int IntervalTimerPrimireDate = 50;

        protected NetworkServer _host;
        protected ParserTabla _parserTabla;
        protected Om _jucatorHost;

        private Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaHost;
        protected System.Timers.Timer _timerJocHost;

        protected bool _randulHostului;
        protected bool _timerJocHostDisposed;

        private String _ultimulMesajPrimitHost = NetworkServer.BufferGol;

        public Om Jucator
        {
            get { return _jucatorHost; }
        }

        public bool RandulTau
        {
            get { return _randulHostului; }
        }

        public EngineHost(Form parentForm, Om jucator) : base(parentForm)
        {
            AdaugaEvenimentCadrane();
            _jucatorHost = jucator;

            _randulHostului = false;

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _parserTabla = new ParserTabla(ConstantaTabla.NrLinii, ConstantaTabla.NrColoane);
        }

        public EngineHost(Form parentForm, int[][] matriceTabla, Om jucator) : base(parentForm, matriceTabla)
        {
            AdaugaEvenimentCadrane();
            _jucatorHost = jucator;

            _randulHostului = false;

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _parserTabla = new ParserTabla(ConstantaTabla.NrLinii, ConstantaTabla.NrColoane);
        }

        public void AdaugaEvenimentCadrane()
        {
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie][coloana].Click += OnCadranClick;
                }
            }
        }

        private void RealizeazaMutareaOnline(Piesa piesa, Pozitie pozitie)
        {
            if (pozitie.Linie > MarimeVerticala || pozitie.Coloana > MarimeOrizontala || pozitie.Linie < 0 || pozitie.Coloana < 0)
            {
                Debug.WriteLine("Linie sau coloana invalida! Linie: {0}, Coloana {1}", pozitie.Linie, pozitie.Coloana);
            }
            else
            {
                NuEsteRandulTau();
                _host.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                RealizeazaMutareaLocal(piesa, pozitie);
            }
        }

        protected virtual void NuEsteRandulTau()
        {
            _randulHostului = false;
        }

        protected virtual void EsteRandulTau()
        {
            _randulHostului = true;
        }

        ~EngineHost() => Dispose();

        public override void Dispose()
        {
            GC.SuppressFinalize(this);
            Debug.WriteLine("Dispose JocMultiplayer");

            _timerJocHostDisposed = true;
            _host.TrimiteDate(_host.MesajDeconectare);
            _timerJocHost?.Dispose();
        }

        public virtual async void HosteazaJoc(int port)
        {
            _host = new NetworkServer(IPAddress.Any, port);
            _host.AcceptaUrmatorulClient();
            await AsteaptaComunicareaCuClientul();
        }

        protected virtual async Task AsteaptaComunicareaCuClientul()
        {
            while (_host.ClientPrimit == false)
            {
                await Task.Delay(50);
            }
            _host.TrimiteDate(_parserTabla.CodificareTablaSiAspect(this.MatriceCoduriPiese, this.AspectJoc));
            _host.TimerCitireDate.Stop();
            ActiveazaTimerRepetitiv(_timerJocHost, (uint)IntervalTimerPrimireDate, SincronizeazaHost);
            EsteRandulTau();
        }

        public void SincronizeazaHost(object source, ElapsedEventArgs e)
        {
            if (_timerJocHostDisposed == false)
            {
                if (_ultimulMesajPrimitHost.Equals(_host.Buffer))
                {
                    _host.PrimesteDate();
                }
                if (_host.Buffer != NetworkServer.BufferGol)
                {
                    _ultimulMesajPrimitHost = _host.Buffer;
                    if (!_ultimulMesajPrimitHost.Equals(_host.MesajDeconectare))
                    {
                        _ultimaMutarePrimitaHost = _parserTabla.DecodificareMutare(_ultimulMesajPrimitHost);
                        VerificaSahul(_ultimaMutarePrimitaHost.Item2);
                        RealizeazaMutareaLocal(GetPiesaCuPozitia(_ultimaMutarePrimitaHost.Item1), _ultimaMutarePrimitaHost.Item2);
                        EsteRandulTau();
                        VerificaSahurile();
                    }
                    else
                    {
                        _timerJocHostDisposed = true;
                        _timerJocHost.Stop();
                        MessageBox.Show("Client Deconectat(Cod 3)", "Clientul s-a deconectat");
                        if (_esteGataMeciul == false)
                        {
                            VerificaSahurile();
                        }
                    }
                }
            }
        }

        private int VerificaTentativaDeSah()
        {
            for (int i = 0; i < ConstantaTabla.NrLinii; i++)
            {
                for (int j = 0; j < ConstantaTabla.NrLinii; j++)
                {
                    if (ArrayCadrane[i][j].PiesaCadran != ConstantaTabla.PiesaNula)
                    {
                        List<Pozitie> mutari = ArrayCadrane[i][j].PiesaCadran.ReturneazaPozitiiPosibile(this.MatriceCoduriPiese);
                        foreach (Pozitie mutare in mutari)
                        {
                            if (MatriceCoduriPiese[mutare.Linie][mutare.Coloana] == (int)CodPiesa.RegeAlb)
                                return ConstantaTabla.SahLaRegeAlb;
                            if (MatriceCoduriPiese[mutare.Linie][mutare.Coloana] == (int)CodPiesa.RegeAlbastru)
                                return ConstantaTabla.SahLaRegerAlbastru;
                        }
                    }
                }
            }
            return ConstantaTabla.NuEsteSah;
        }

        private void VerificaSahul(Pozitie pozitie)
        {
            Piesa piesa = GetPiesaCuPozitia(pozitie);
            if (piesa != null)
            {
                if (piesa.Cod == CodPiesa.RegeAlbastru)
                {
                    MessageBox.Show("Ai castigat");
                    TerminaMeciul();
                }
                else if (piesa.Cod == CodPiesa.RegeAlbastru)
                {
                    MessageBox.Show("Ai pierdut");
                    TerminaMeciul();
                }
            }
        }

        public void TerminaMeciul()
        {
            _esteGataMeciul = true;
            StergeEvenimenteleCadranelor();
            _timerJocHost.Stop();
            _host.TrimiteDate(_host.MesajDeconectare);
        }

        private void StergeEvenimenteleCadranelor()
        {
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie][coloana].Click -= OnCadranClick;
                }
            }
        }

        public void OnCadranClick(object sender, EventArgs e)
        {
            if (_randulHostului)
            {
                if (PiesaSelectata == ConstantaTabla.PiesaNula)
                {
                    Pozitie pozitie = (sender as Cadran).PozitieCadran;

                    if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                    {
                        Piesa piesa = GetPiesaCuPozitia(pozitie);

                        if (piesa != null)
                        {
                            if (piesa.Culoare != _jucatorHost.Culoare)
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
                    Pozitie pozitie = (sender as Cadran).PozitieCadran;

                    if (PiesaSelectata.Pozitie != pozitie)
                    {
                        if (EsteMutareaPosibila(pozitie))
                        {
                            VerificaSahul(pozitie);
                            AscundePiesaSelectata(PiesaSelectata);
                            if (MatriceCoduriPiese[pozitie.Linie][pozitie.Coloana] != (int)CodPiesa.Gol)
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