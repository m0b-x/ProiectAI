using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class EngineJoc : IDisposable
    {
        private Form _parentForm;
        private Tabla _tabla;

        private int[,] _matriceCodPiese;

        private List<Piesa> _listaPiese;
        private List<Pozitie> _pozitiiMutariPosibile;

        private Label[] _labelColoane;
        private Label[] _labelLinii;

        private Piesa _piesaSelectata;

        private Tuple<Pozitie, Pozitie> _ultimaMutare;

        public List<Pozitie> PozitiiMutariPosibile
        {
            get { return _pozitiiMutariPosibile; }
            set { _pozitiiMutariPosibile = value; }
        }

        public List<Piesa> ListaPiese
        {
            get { return _listaPiese; }
            set { _listaPiese = value; }
        }

        public Form ParentForm
        {
            get { return _parentForm; }
            set { _parentForm = value; }
        }

        public Tabla TablaJoc
        {
            get { return _tabla; }
        }

        public Cadran[,] ArrayCadrane
        {
            get { return _tabla.ArrayCadrane; }
            set { _tabla.ArrayCadrane = value; }
        }

        public int[,] MatriceCodPiese
        {
            get { return _matriceCodPiese; }
            set { _matriceCodPiese = value; }
        }

        public int MarimeVerticala
        {
            get { return _tabla.MarimeVerticala; }
        }

        public int MarimeOrizontala
        {
            get { return _tabla.MarimeOrizontala; }
        }

        public int PragRau
        {
            get { return _tabla.PragRau; }
        }

        public int MarimeRau
        {
            get { return _tabla.MarimeRau; }
        }

        public Color CuloareCadranPar
        {
            get { return _tabla.CuloareCadranPar; }
        }

        public Color CuloareCadranImpar
        {
            get { return _tabla.CuloareCadranImpar; }
        }

        public Color CuloareCadranSelectat
        {
            get { return _tabla.CuloareCadranSelectat; }
            set { _tabla.CuloareCadranSelectat = value; }
        }

        public Color CuloareCadranMutari
        {
            get { return _tabla.CuloareCadranMutari; }
            set { _tabla.CuloareCadranMutari = value; }
        }

        protected Piesa PiesaSelectata
        {
            get { return _piesaSelectata; }
            set { _piesaSelectata = value; }
        }

        public Tuple<Pozitie, Pozitie> UltimaMutare
        {
            get { return _ultimaMutare; }
        }

        public EngineJoc(Form parentForm)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>();
            _pozitiiMutariPosibile = new List<Pozitie>();

            _tabla = new Tabla();
            _ultimaMutare = new Tuple<Pozitie, Pozitie>(new Pozitie(0, 0), new Pozitie(0, 0));

            _matriceCodPiese = new int[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];

            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    ArrayCadrane[linie, coloana] = new Cadran(this, new Pozitie(linie, coloana), _tabla.DecideCuloareaCadranului(linie, coloana));
                }
            }
            _labelLinii = new Label[ConstantaTabla.MarimeVerticala];
            _labelColoane = new Label[ConstantaTabla.MarimeOrizontala];
            CreeazaLabeluriLinii(_labelColoane);
            CreeazaLabeluriColoane(_labelLinii);
        }

        public EngineJoc(Form parentForm, int[,] matriceTabla)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>();
            _pozitiiMutariPosibile = new List<Pozitie>();

            _tabla = new Tabla();
            _ultimaMutare = new Tuple<Pozitie, Pozitie>(new Pozitie(0, 0), new Pozitie(0, 0));

            _matriceCodPiese = new int[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];

            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    ArrayCadrane[linie, coloana] = new Cadran(this, new Pozitie(linie, coloana), _tabla.DecideCuloareaCadranului(linie, coloana));
                    if (matriceTabla[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)Enum.ToObject(typeof(CodPiesa), matriceTabla[linie, coloana]));
                        AdaugaPiesa(piesa, new Pozitie(linie, coloana));
                    }
                }
            }
        }

        public virtual void Dispose()
        {
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                _labelLinii[linie].Dispose();
            }
            for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
            {
                _labelColoane[coloana].Dispose();
            }
            _tabla.Dispose();
            GC.SuppressFinalize(this);
        }

        private void CreeazaLabeluriColoane(Label[] __labelLinii)
        {
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                __labelLinii[linie] = new Label();
                __labelLinii[linie].Text = linie.ToString();
                __labelLinii[linie].Font = new Font(ConstantaTabla.FontPrincipal, ConstantaTabla.MarimeFont);
                __labelLinii[linie].AutoSize = false;
                __labelLinii[linie].Size = new Size(ConstantaCadran.MarimeCadran, ConstantaCadran.MarimeCadran);
                __labelLinii[linie].Location = new Point(ArrayCadrane[linie, MarimeOrizontala - 1].Location.X + ConstantaCadran.MarimeCadran,
                                                        ArrayCadrane[linie, MarimeOrizontala - 1].Location.Y + ConstantaCadran.MarimeCadran / 3);
                _parentForm.Controls.Add(__labelLinii[linie]);
            }
        }

        private void CreeazaLabeluriLinii(Label[] __labelColoane)
        {
            char literaLinie = 'A';
            for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
            {
                __labelColoane[coloana] = new Label();
                __labelColoane[coloana].Text = literaLinie++.ToString();
                __labelColoane[coloana].Font = new Font(ConstantaTabla.FontPrincipal, ConstantaTabla.MarimeFont);
                __labelColoane[coloana].AutoSize = false;
                __labelColoane[coloana].Size = new Size(ConstantaCadran.MarimeCadran, ConstantaCadran.MarimeCadran);
                __labelColoane[coloana].Location = new Point(ArrayCadrane[MarimeVerticala - 1, coloana].Location.X + ConstantaCadran.MarimeCadran / 3,
                                                            ArrayCadrane[MarimeVerticala - 1, coloana].Location.Y + ConstantaCadran.MarimeCadran);
                _parentForm.Controls.Add(__labelColoane[coloana]);
            }
        }

        public void AdaugaPieselePrestabilite()
        {
            //piese albastre

            AdaugaPiesa(new Pion(CuloareJoc.Albastru), new Pozitie(3, 0));
            AdaugaPiesa(new Pion(CuloareJoc.Albastru), new Pozitie(3, 2));
            AdaugaPiesa(new Pion(CuloareJoc.Albastru), new Pozitie(3, 4));
            AdaugaPiesa(new Pion(CuloareJoc.Albastru), new Pozitie(3, 6));
            AdaugaPiesa(new Pion(CuloareJoc.Albastru), new Pozitie(3, 8));

            AdaugaPiesa(new Tun(CuloareJoc.Albastru), new Pozitie(2, 1));
            AdaugaPiesa(new Tun(CuloareJoc.Albastru), new Pozitie(2, 7));

            AdaugaPiesa(new Tura(CuloareJoc.Albastru), new Pozitie(0, 0));
            AdaugaPiesa(new Tura(CuloareJoc.Albastru), new Pozitie(0, 8));

            AdaugaPiesa(new Cal(CuloareJoc.Albastru), new Pozitie(0, 1));
            AdaugaPiesa(new Cal(CuloareJoc.Albastru), new Pozitie(0, 7));

            AdaugaPiesa(new Elefant(CuloareJoc.Albastru), new Pozitie(0, 2));
            AdaugaPiesa(new Elefant(CuloareJoc.Albastru), new Pozitie(0, 6));

            AdaugaPiesa(new General(CuloareJoc.Albastru), new Pozitie(0, 3));
            AdaugaPiesa(new General(CuloareJoc.Albastru), new Pozitie(0, 5));

            AdaugaPiesa(new Rege(CuloareJoc.Albastru), new Pozitie(0, 4));

            //piese albe

            AdaugaPiesa(new Pion(CuloareJoc.Alb), new Pozitie(6, 0));
            AdaugaPiesa(new Pion(CuloareJoc.Alb), new Pozitie(6, 2));
            AdaugaPiesa(new Pion(CuloareJoc.Alb), new Pozitie(6, 4));
            AdaugaPiesa(new Pion(CuloareJoc.Alb), new Pozitie(6, 6));
            AdaugaPiesa(new Pion(CuloareJoc.Alb), new Pozitie(6, 8));

            AdaugaPiesa(new Tun(CuloareJoc.Alb), new Pozitie(7, 1));
            AdaugaPiesa(new Tun(CuloareJoc.Alb), new Pozitie(7, 7));

            AdaugaPiesa(new Tura(CuloareJoc.Alb), new Pozitie(9, 0));
            AdaugaPiesa(new Tura(CuloareJoc.Alb), new Pozitie(9, 8));

            AdaugaPiesa(new Cal(CuloareJoc.Alb), new Pozitie(9, 1));
            AdaugaPiesa(new Cal(CuloareJoc.Alb), new Pozitie(9, 7));

            AdaugaPiesa(new Elefant(CuloareJoc.Alb), new Pozitie(9, 2));
            AdaugaPiesa(new Elefant(CuloareJoc.Alb), new Pozitie(9, 6));

            AdaugaPiesa(new General(CuloareJoc.Alb), new Pozitie(9, 3));
            AdaugaPiesa(new General(CuloareJoc.Alb), new Pozitie(9, 5));

            AdaugaPiesa(new Rege(CuloareJoc.Alb), new Pozitie(9, 4));
        }

        public void ActualizeazaIntreagaTabla(int[,] matriceTabla)
        {
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)Enum.ToObject(typeof(CodPiesa), matriceTabla[linie, coloana]));
                    if (piesa != ConstantaTabla.PiesaNula)
                    {
                        AdaugaPiesa(piesa, new Pozitie(linie, coloana));
                    }
                }
            }
        }

        public static Piesa ConvertesteCodPiesaInObiect(CodPiesa codPiesa)
        {
            switch (codPiesa)
            {
                case CodPiesa.PionAlb: return new Pion(CuloareJoc.Alb);
                case CodPiesa.PionAlbastru: return new Pion(CuloareJoc.Albastru);
                case CodPiesa.TuraAlba: return new Tura(CuloareJoc.Alb);
                case CodPiesa.TuraAlbastra: return new Tura(CuloareJoc.Albastru);
                case CodPiesa.TunAlb: return new Tun(CuloareJoc.Alb);
                case CodPiesa.TunAlbastru: return new Tun(CuloareJoc.Albastru);
                case CodPiesa.GardianAlb: return new General(CuloareJoc.Alb);
                case CodPiesa.GardianAlbastru: return new General(CuloareJoc.Albastru);
                case CodPiesa.ElefantAlb: return new Elefant(CuloareJoc.Alb);
                case CodPiesa.ElefantAlbastru: return new Elefant(CuloareJoc.Albastru); ;
                case CodPiesa.CalAlb: return new Cal(CuloareJoc.Alb);
                case CodPiesa.CalAbastru: return new Cal(CuloareJoc.Albastru);
                case CodPiesa.RegeAlb: return new Rege(CuloareJoc.Alb);
                case CodPiesa.RegeAlbastru: return new Rege(CuloareJoc.Albastru);
                default: return ConstantaTabla.PiesaNula;
            }
        }

        public void AdaugaPiesa(Piesa piesa, Pozitie pozitie)
        {
            if (pozitie.Linie > MarimeVerticala || pozitie.Coloana > MarimeOrizontala || pozitie.Linie < 0 || pozitie.Coloana < 0)
            {
                Debug.WriteLine("Linie sau coloana invalida! Linie: {0}, Coloana {1}", pozitie.Linie, pozitie.Coloana);
            }
            else if (piesa == ConstantaTabla.PiesaNula)
            {
                Debug.WriteLine("Piesa invalida! Linie: {0}, Coloana {1}", pozitie.Linie, pozitie.Coloana);
            }
            else
            {
                if (piesa.PusaPeTabla == true)
                {
                    Debug.WriteLine("Eroare:Piesa selectata este deja pusa pe tabla!");
                }
                else
                {
                    if (_matriceCodPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                    {
                        Debug.WriteLine("Eroare:adugare piesa peste alta! Linie:{0},Coloana:{1},PiesaAdaugata:{2}", pozitie.Linie, pozitie.Coloana, piesa.Cod);
                    }
                    piesa.Pozitie = pozitie;
                    piesa.PusaPeTabla = true;
                    SeteazaPiesaCadranului(piesa.Pozitie, piesa);
                }
            }
        }

        public void ArataPiesaSelectata(Piesa piesa)
        {
            ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].BackColor = CuloareCadranSelectat;
        }

        public void ArataPozitieBlocata(Pozitie pozitie)
        {
            if (ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor != ConstantaTabla.CuloarePozitieBlocata)
            {
                Color culoareCadranPrecedenta = ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor;
                ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = ConstantaTabla.CuloarePozitieBlocata;
                var timer = new System.Threading.Timer(
                             e =>
                                {
                                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = culoareCadranPrecedenta;
                                    PiesaSelectata = ConstantaTabla.PiesaNula;
                                },
                             null,
                             TimeSpan.Zero,
                             TimeSpan.FromMilliseconds(100));
            }
        }

        public void AscundePiesaSelectata(Piesa piesa)
        {
            ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].BackColor = _tabla.DecideCuloareaCadranului(piesa.Pozitie.Linie, piesa.Pozitie.Coloana);
        }

        public void DecoloreazaMutariPosibile(List<Pozitie> pozitii)
        {
            if (_pozitiiMutariPosibile != null)
            {
                foreach (Pozitie pozitie in _pozitiiMutariPosibile)
                {
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = _tabla.DecideCuloareaCadranului(pozitie.Linie, pozitie.Coloana);
                }
            }
        }

        public bool ExistaMutariPosibile()
        {
            if (_pozitiiMutariPosibile == null || _pozitiiMutariPosibile.Count == 0)
                return false;
            return true;
        }

        public void ColoreazaMutariPosibile(List<Pozitie> pozitii)
        {
            if (pozitii != null)
            {
                foreach (Pozitie pozitie in pozitii)
                {
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = CuloareCadranMutari;
                    _pozitiiMutariPosibile.Add(new Pozitie(pozitie.Linie, pozitie.Coloana));
                }
            }
        }

        public bool EsteMutareaPosibila(Pozitie pozitie)
        {
            if (ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor == CuloareCadranMutari)
                return true;
            else
                return false;
        }

        protected void ActualizeazaUltimaMutare(Pozitie pozitieInitiala, Pozitie pozitieFinala)
        {
            _ultimaMutare = new Tuple<Pozitie, Pozitie>(pozitieInitiala, pozitieFinala);
        }

        public void SeteazaPiesaCadranului(Pozitie pozitie, Piesa piesa)
        {
            if (ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
            {
                if (piesa != ConstantaTabla.PiesaNula)
                {
                    piesa.Pozitie = pozitie;
                    ListaPiese.Remove(ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran);
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran.Dispose();
                    ListaPiese.Add(piesa);
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran = piesa;
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].PozitieCadran = pozitie;
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackgroundImage = piesa.Imagine;
                    _matriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                }
                else
                {
                    ListaPiese.Remove(ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran);
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran = ConstantaTabla.PiesaNula;
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackgroundImage = null;
                    _matriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)CodPiesa.Gol;
                }
            }
            else
            {
                if (piesa != ConstantaTabla.PiesaNula)
                {
                    piesa.Pozitie = pozitie;
                    ListaPiese.Add(piesa);
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran = piesa;
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].PozitieCadran = pozitie;
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackgroundImage = piesa.Imagine;
                    _matriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                }
                else
                {
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackgroundImage = null;
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran = ConstantaTabla.PiesaNula;
                    _matriceCodPiese[pozitie.Linie, pozitie.Coloana] = (int)CodPiesa.Gol;
                }
            }
        }

        protected virtual void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie)
        {
            if (piesa == null || pozitie == null)
            {
                return;
            }
            if (piesa.GetType().Equals(typeof(Tun)))
            {
                TerminaMutareaTunului(piesa, pozitie);
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

        public void TerminaMutareaTunului(Piesa piesa, Pozitie pozitiaFinala)
        {
            Pozitie pozitieInitiala = piesa.Pozitie;

            if (pozitieInitiala.Linie != pozitiaFinala.Linie)
            {
                int linieInitiala = (pozitieInitiala.Linie > pozitiaFinala.Linie) ? pozitiaFinala.Linie : pozitieInitiala.Linie;
                int linieFinala = (pozitieInitiala.Linie < pozitiaFinala.Linie) ? pozitiaFinala.Linie : pozitieInitiala.Linie;
                for (int linie = linieInitiala; linie < linieFinala; linie++)
                {
                    if (ArrayCadrane[linie, piesa.Pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                    {
                        if (ArrayCadrane[linie, piesa.Pozitie.Coloana].PiesaCadran.CuloarePiesa != piesa.CuloarePiesa)
                        {
                            SeteazaPiesaCadranului(new Pozitie(linie, piesa.Pozitie.Coloana), ConstantaTabla.PiesaNula);
                        }
                    }
                }
            }
            else if (pozitieInitiala.Coloana != pozitiaFinala.Coloana)
            {
                int coloanaInitiala = (pozitieInitiala.Coloana > pozitiaFinala.Coloana) ? pozitiaFinala.Coloana : pozitieInitiala.Coloana;
                int coloanaFinala = (pozitieInitiala.Coloana < pozitiaFinala.Coloana) ? pozitiaFinala.Coloana : pozitieInitiala.Coloana;
                for (int coloana = coloanaInitiala; coloana < coloanaFinala; coloana++)
                {
                    if (ArrayCadrane[piesa.Pozitie.Linie, coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                    {
                        if (ArrayCadrane[piesa.Pozitie.Linie, coloana].PiesaCadran.CuloarePiesa != piesa.CuloarePiesa)
                        {
                            SeteazaPiesaCadranului(new Pozitie(piesa.Pozitie.Linie, coloana), ConstantaTabla.PiesaNula);
                        }
                    }
                }
            }
        }

        public Piesa GetPiesaCuPozitia(Pozitie pozitie)
        {
            if (_listaPiese != null)
            {
                foreach (Piesa piesa in _listaPiese)
                {
                    if (piesa.Pozitie == pozitie)
                    {
                        return piesa;
                    }
                }
            }
            return null;
        }

        public void CurataTabla()
        {
            for (int linie = 0; linie < _tabla.MarimeOrizontala; linie++)
            {
                for (int coloana = 0; coloana < _tabla.MarimeVerticala; coloana++)
                {
                    SeteazaPiesaCadranului(new Pozitie(linie, coloana), ConstantaTabla.PiesaNula);
                }
            }
        }

        public void ActiveazaTimerRepetitiv(ref System.Timers.Timer timer, uint interval, System.Timers.ElapsedEventHandler functie)
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = interval;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(functie);
            timer.Enabled = true;
        }
    }
}