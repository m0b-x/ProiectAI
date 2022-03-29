﻿using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class EngineHost : EngineJoc
    {
        private static int _intervalTimere = 50;

        protected NetworkServer _host;
        protected ParserTabla _parserTabla;
        protected Om _jucatorHost;

        private Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaHost;
        protected System.Timers.Timer _timerJocHost;

        protected int _timpTimere;
        protected bool _esteRandulHostului;
        protected bool _timerJocHostDisposed;

        private String _ultimulMesajPrimitHost = NetworkServer.BufferGol;
        public Om Jucator
        {
            get { return _jucatorHost; }
        }
        public EngineHost(Form parentForm, Om jucator) : base(parentForm)
        {
            AdaugaEvenimentCadrane();
            _jucatorHost = jucator;

            _esteRandulHostului = false;

            _timpTimere = _intervalTimere;

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

            _parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala);
        
            AdaugaEvenimentCadrane();
        }

        public EngineHost(Form parentForm, int[,] matriceTabla,Om jucator) : base(parentForm, matriceTabla)
        {
            AdaugaEvenimentCadrane();
            _jucatorHost = jucator;

            _esteRandulHostului = false;

            _timpTimere = _intervalTimere;

            _ultimaMutarePrimitaHost = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

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
                    _host.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
                    RealizeazaMutareaLocal(piesa, pozitie);
                    EsteRandulClientului();
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

        ~EngineHost() => Dispose();

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
            EsteRandulHostului();
        }

        private void PornesteTimerHost()
        {
            _timerJocHostDisposed = false;
            _timerJocHost = new();
            _timerJocHost.Interval = _timpTimere;
            _timerJocHost.AutoReset = true;
            _timerJocHost.Enabled = true;
            _timerJocHost.Elapsed += new ElapsedEventHandler(SincronizeazaHost);
            _timerJocHost.Start();
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
                        RealizeazaMutareaLocal(GetPiesaCuPozitia(_ultimaMutarePrimitaHost.Item1), _ultimaMutarePrimitaHost.Item2);
                        EsteRandulHostului();
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

        public void OnCadranClick(object sender, EventArgs e)
        {
            if (_esteRandulHostului)
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
                            if (piesa.CuloarePiesa != _jucatorHost.Culoare)
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
