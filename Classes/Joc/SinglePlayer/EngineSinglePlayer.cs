using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class EngineJocSinglePlayer : EngineJoc
    {
        public static int IntervalTimerPrimireDate = 50;

        protected Om _jucatorHost;

        protected bool _randulOmului;

        private String _ultimulMesajPrimitHost = NetworkServer.BufferGol;

        public Dictionary<Piesa,int> _dictionarValoriPiese = new();

        public Om Jucator
        {
            get { return _jucatorHost; }
        }
        public bool RandulTau
        {
            get { return _randulOmului; }
        }

        public EngineJocSinglePlayer(Form parentForm, Om jucator) : base(parentForm)
        {
            AdaugaEvenimentCadrane();
            _jucatorHost = jucator;

            _randulOmului = false;

            AdaugaEvenimentCadrane();
        }

        public EngineJocSinglePlayer(Form parentForm, int[,] matriceTabla, Om jucator) : base(parentForm, matriceTabla)
        {
            AdaugaEvenimentCadrane();
            _jucatorHost = jucator;

            _randulOmului = false;
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

        protected virtual void NuEsteRandulTau()
        {
            _randulOmului = false;
        }

        protected virtual void EsteRandulTau()
        {
            _randulOmului = true;
        }


        public void OnCadranClick(object sender, EventArgs e)
        {
            if (_randulOmului)
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
                            NuEsteRandulTau();
                            RealizeazaMutareaLocal(PiesaSelectata, pozitie);

                            RealizeazaMutareaAI();
                        }
                    }
                }
            }
        }

        private void RealizeazaMutareaAI()
        {
            Tuple<Pozitie, Pozitie> pozitieReturnata = ReturneazaMutareaAI();
            Piesa piesa = GetPiesaCuPozitia(pozitieReturnata.Item1);
            Task.Delay(ConstantaTabla.TimpAsteptariAI);
            RealizeazaMutareaLocal(piesa, pozitieReturnata.Item2);
            EsteRandulTau();
        }

        public Tuple<Pozitie, Pozitie> ReturneazaMutareaAI()
        {
            Tuple<Pozitie, Pozitie> pozitieReturnata = Tuple.Create(new Pozitie(0, 0), new Pozitie(0, 0));

            return pozitieReturnata;
        }
    }
}