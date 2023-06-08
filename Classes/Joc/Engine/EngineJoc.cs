using ProiectVolovici.Visual_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class EngineJoc : IDisposable
    {
        private Form _parentForm;
        protected Tabla _tabla;

        protected int[][] _matriceCodPiese;
        protected Aspect _aspectJoc;
        private List<Piesa> _listaPiese;
        private List<Piesa> _listaPieseAlbe;
        private List<Piesa> _listaPieseAlbastre;
        protected List<Pozitie> pozitiiMutariColorate;
        private Stack<Mutare> _stivaMutari = new Stack<Mutare>(120);
        private Stack<int> _stivaCodPiese = new Stack<int>(120);

        private Label[] _labelColoane;
        private Label[] _labelLinii;
        private Label _labelAsteptare;

        private Piesa _piesaSelectata;

        protected bool _esteGataMeciul;
        protected int _nrSahuriLaAlb = 0;
        protected int _nrSahuriLaAlbastru = 0;
        protected bool _sahPersistentLaAlb = false;
        protected bool _sahPersistentLaAlbastru = false;

        public Stack<Mutare> StivaMutari
        {
            get {  return _stivaMutari; }
        }
        public Mutare UltimaMutare
        {
            get { return _stivaMutari.Peek(); }
        }

        public Aspect AspectJoc
        {
            get { return _aspectJoc; }
            set { _aspectJoc = value; }
        }

        public List<Pozitie> PozitiiMutariPosibile
        {
            get { return pozitiiMutariColorate; }
            set { pozitiiMutariColorate = value; }
        }

        public List<Piesa> ListaPiese
        {
            get { return _listaPiese; }
            set { _listaPiese = value; }
        }

        public List<Piesa> ListaPieseAlbe
        {
            get { return _listaPieseAlbe; }
            set { _listaPieseAlbe = value; }
        }

        public List<Piesa> ListaPieseAlbastre
        {
            get { return _listaPieseAlbastre; }
            set { _listaPieseAlbastre = value; }
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

        public Cadran[][] ArrayCadrane
        {
            get { return _tabla.ArrayCadrane; }
            set { _tabla.ArrayCadrane = value; }
        }

        public Label LabelAsteptare
        {
            get { return _labelAsteptare; }
            set { _labelAsteptare = value; }
        }

        public int[][] MatriceCoduriPiese
        {
            get
            {
                return _matriceCodPiese;
            }
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


        public EngineJoc(Form parentForm, Aspect aspect = Aspect.Normal)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>(32);
            _listaPieseAlbe = new List<Piesa>(16);
            _listaPieseAlbastre = new List<Piesa>(16);
            pozitiiMutariColorate = new List<Pozitie>(200);
            _aspectJoc = aspect;

            _tabla = new Tabla();

            _matriceCodPiese = new int[ConstantaTabla.NrLinii][];

            for (int i = 0; i < ConstantaTabla.NrLinii; i++)
                _matriceCodPiese[i] = new int[ConstantaTabla.NrColoane];

            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie][coloana] = new Cadran(this, Pozitie.AcceseazaElementStatic(linie, coloana), _tabla.DecideCuloareaCadranului(linie, coloana));
                }
            }
            _labelLinii = new Label[ConstantaTabla.NrLinii];
            _labelColoane = new Label[ConstantaTabla.NrColoane];

            CreeazaLabeluriLinii(_labelColoane);
            CreeazaLabelAsteptare();
            CreeazaLabeluriColoane(_labelLinii);
        }

        public EngineJoc(Form parentForm, int[][] matriceTabla, Aspect aspect = Aspect.Normal)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>(32);
            _listaPieseAlbe = new List<Piesa>(16);
            _listaPieseAlbastre = new List<Piesa>(16);
            pozitiiMutariColorate = new List<Pozitie>(200);
            _aspectJoc = aspect;

            _tabla = new Tabla();

            _matriceCodPiese = new int[ConstantaTabla.NrLinii][];
            for (int i = 0; i < ConstantaTabla.NrLinii; i++)
                _matriceCodPiese[i] = new int[ConstantaTabla.NrColoane];

            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie][coloana] = new Cadran(this, Pozitie.AcceseazaElementStatic(linie, coloana), _tabla.DecideCuloareaCadranului(linie, coloana));
                    if (matriceTabla[linie][coloana] != (int)CodPiesa.Gol)
                    {
                        Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)Enum.ToObject(typeof(CodPiesa), matriceTabla[linie][coloana]));
                        AdaugaPiesa(piesa, Pozitie.AcceseazaElementStatic(linie, coloana));
                    }
                }
            }
        }

        public virtual void Dispose()
        {
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                _labelLinii[linie].Dispose();
            }
            for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
            {
                _labelColoane[coloana].Dispose();
            }
            _tabla.Dispose();
            GC.SuppressFinalize(this);
        }

        private void CreeazaLabeluriColoane(Label[] _labelLinii)
        {
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                _labelLinii[linie] = new Label();
                _labelLinii[linie].Text = linie.ToString();
                _labelLinii[linie].Font = new Font(ConstantaTabla.FontPrincipal, ConstantaTabla.MarimeFont);
                _labelLinii[linie].AutoSize = true;
                _labelLinii[linie].Size = new Size(ConstantaCadran.MarimeCadran, ConstantaCadran.MarimeCadran);
                _labelLinii[linie].Location = new Point(ArrayCadrane[linie][MarimeOrizontala - 1].Location.X + ConstantaCadran.MarimeCadran,
                                                        ArrayCadrane[linie][MarimeOrizontala - 1].Location.Y + ConstantaCadran.MarimeCadran / 3);
                _parentForm.Controls.Add(_labelLinii[linie]);
            }
        }

        protected void CreeazaLabelAsteptare()
        {
            _labelAsteptare = new Label();
            _labelAsteptare.Text = String.Empty;
            _labelAsteptare.Font = new Font(ConstantaTabla.FontPrincipal, ConstantaTabla.MarimeFont);
            _labelAsteptare.AutoSize = true;
            _labelAsteptare.Location = new Point(255, 30);
            _parentForm.Controls.Add(_labelAsteptare);
        }

        private void CreeazaLabeluriLinii(Label[] _labelColoane)
        {
            char literaLinie = 'A';
            for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
            {
                _labelColoane[coloana] = new Label();
                _labelColoane[coloana].Text = literaLinie++.ToString();
                _labelColoane[coloana].Font = new Font(ConstantaTabla.FontPrincipal, ConstantaTabla.MarimeFont);
                _labelColoane[coloana].AutoSize = false;
                _labelColoane[coloana].Size = new Size(ConstantaCadran.MarimeCadran, ConstantaCadran.MarimeCadran);
                _labelColoane[coloana].Location = new Point(ArrayCadrane[MarimeVerticala - 1][coloana].Location.X + ConstantaCadran.MarimeCadran / 3,
                                                            ArrayCadrane[MarimeVerticala - 1][coloana].Location.Y + ConstantaCadran.MarimeCadran);
                _parentForm.Controls.Add(_labelColoane[coloana]);
            }
        }

        public void AdaugaPieselePrestabilite()
        {
            //piese albastre

            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(3, 0));

            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(3, 2));
            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(3, 4));
            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(3, 6));
            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(3, 8));

            AdaugaPiesa(new Tun(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(2, 1));
            AdaugaPiesa(new Tun(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(2, 7));

            AdaugaPiesa(new Tura(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 0));
            AdaugaPiesa(new Tura(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 8));

            AdaugaPiesa(new Cal(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 1));
            AdaugaPiesa(new Cal(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 7));

            AdaugaPiesa(new Elefant(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 2));
            AdaugaPiesa(new Elefant(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 6));

            AdaugaPiesa(new Gardian(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 3));
            AdaugaPiesa(new Gardian(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 5));

            AdaugaPiesa(new Rege(Culoare.AlbastruMax, _aspectJoc), Pozitie.AcceseazaElementStatic(0, 4));

            //piese albe


            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(6, 0));
            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(6, 2));
            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(6, 4));
            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(6, 6));
            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(6, 8));

            AdaugaPiesa(new Tun(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(7, 1));
            AdaugaPiesa(new Tun(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(7, 7));

            AdaugaPiesa(new Tura(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 0));
            AdaugaPiesa(new Tura(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 8));

            AdaugaPiesa(new Cal(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 1));
            AdaugaPiesa(new Cal(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 7));

            AdaugaPiesa(new Elefant(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 2));
            AdaugaPiesa(new Elefant(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 6));

            AdaugaPiesa(new Gardian(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 5));
            AdaugaPiesa(new Gardian(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 3));

            AdaugaPiesa(new Rege(Culoare.AlbMin, _aspectJoc), Pozitie.AcceseazaElementStatic(9, 4));

            _listaPiese = _listaPiese.OrderByDescending(o => ReturneazaScorPiesa((int)o.Cod)).ToList();
            _listaPieseAlbastre = _listaPieseAlbastre.OrderByDescending(o => ReturneazaScorPiesa((int)o.Cod)).ToList();
            _listaPieseAlbe = _listaPieseAlbe.OrderByDescending(o => ReturneazaScorPiesa((int)o.Cod)).ToList();

        }

        public void ActualizeazaIntreagaTabla(int[][] matriceTabla)
        {
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)Enum.ToObject(typeof(CodPiesa), matriceTabla[linie][coloana]));
                    if (piesa != ConstantaTabla.PiesaNula)
                    {
                        AdaugaPiesa(piesa, Pozitie.AcceseazaElementStatic(linie, coloana));
                    }
                }
            }
        }

        public Piesa ConvertesteCodPiesaInObiect(CodPiesa codPiesa)
        {
            switch (codPiesa)
            {
                case CodPiesa.PionAlb: return new Pion(Culoare.AlbMin, _aspectJoc);
                case CodPiesa.PionAlbastru: return new Pion(Culoare.AlbastruMax, _aspectJoc);
                case CodPiesa.TuraAlba: return new Tura(Culoare.AlbMin, _aspectJoc);
                case CodPiesa.TuraAlbastra: return new Tura(Culoare.AlbastruMax, _aspectJoc);
                case CodPiesa.TunAlb: return new Tun(Culoare.AlbMin, _aspectJoc);
                case CodPiesa.TunAlbastru: return new Tun(Culoare.AlbastruMax, _aspectJoc);
                case CodPiesa.GardianAlb: return new Gardian(Culoare.AlbMin, _aspectJoc);
                case CodPiesa.GardianAlbastru: return new Gardian(Culoare.AlbastruMax, _aspectJoc);
                case CodPiesa.ElefantAlb: return new Elefant(Culoare.AlbMin, _aspectJoc);
                case CodPiesa.ElefantAlbastru: return new Elefant(Culoare.AlbastruMax, _aspectJoc);
                case CodPiesa.CalAlb: return new Cal(Culoare.AlbMin, _aspectJoc);
                case CodPiesa.CalAbastru: return new Cal(Culoare.AlbastruMax, _aspectJoc);
                case CodPiesa.RegeAlb: return new Rege(Culoare.AlbMin, _aspectJoc);
                case CodPiesa.RegeAlbastru: return new Rege(Culoare.AlbastruMax, _aspectJoc);
                case CodPiesa.Gol: return ConstantaTabla.PiesaNula;
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
                piesa.Pozitie = pozitie;
                PunePiesaPeTabla(piesa.Pozitie, piesa);
            }
        }

        public void ArataPiesaSelectata(Piesa piesa)
        {
            ArrayCadrane[piesa.Pozitie.Linie][piesa.Pozitie.Coloana].BackColor = CuloareCadranSelectat;
        }

        public void ArataPozitieBlocata(Pozitie pozitie)
        {
            if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor != ConstantaTabla.CuloarePozitieBlocata)
            {
                Color culoareCadranPrecedenta = ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor;
                ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor = ConstantaTabla.CuloarePozitieBlocata;
                BlocheazaComenzi(ConstantaTabla.IntervalPiesaBlocata);
                ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor = culoareCadranPrecedenta;
                PiesaSelectata = ConstantaTabla.PiesaNula;
            }
        }

        public void AscundePiesaSelectata(Piesa piesa)
        {
            ArrayCadrane[piesa.Pozitie.Linie][piesa.Pozitie.Coloana].BackColor = _tabla.DecideCuloareaCadranului(piesa.Pozitie.Linie, piesa.Pozitie.Coloana);
		}
		public void AscundePiesaSelectata(Pozitie pozPiesa)
		{
			ArrayCadrane[pozPiesa.Linie][pozPiesa.Coloana].BackColor = _tabla.DecideCuloareaCadranului(pozPiesa.Linie, pozPiesa.Coloana);
		}

		public void DecoloreazaMutariPosibile()
        {
            if (pozitiiMutariColorate != null)
            {
                foreach (Pozitie pozitie in pozitiiMutariColorate)
                {
                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor = _tabla.DecideCuloareaCadranului(pozitie.Linie, pozitie.Coloana);
                }
            }
        }

        public bool ExistaMutariPosibile()
        {
            if (pozitiiMutariColorate == null || pozitiiMutariColorate.Count == 0)
                return false;
            return true;
        }

        public void ColoreazaMutariPosibile(List<Pozitie> pozitii)
        {
            if (pozitii != null)
            {
                foreach (Pozitie pozitie in pozitii)
                {
                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor = CuloareCadranMutari;
                    pozitiiMutariColorate.Add(Pozitie.AcceseazaElementStatic(pozitie.Linie, pozitie.Coloana));
                }
            }
        }

        public bool EsteMutareaPosibila(Pozitie pozitie)
        {
            if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor == CuloareCadranMutari)
                return true;
            else
                return false;
        }


        public static void StergePiesaDinListaDupaPoz(List<Piesa> lista, Pozitie poz)
        {
            for (int i = lista.Count - 1; i >= 0; i--)
            {
                if (lista[i].Pozitie.Linie == poz.Linie &&
                    lista[i].Pozitie.Coloana == poz.Coloana)
                {
                    lista.RemoveAt(i);
                    break;
                }
            }
        }

        public void PunePiesaPeTabla(Pozitie pozitie, Piesa piesa)
        {
            if (piesa == null)
            {
                if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran != null)
                {
                    if(_matriceCodPiese[pozitie.Linie][pozitie.Coloana] != 0)
                        StergePiesaDupaPozitieDinListe(pozitie, ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran.Culoare);
                    NulificaPiesaCadranului(pozitie);
                }
                else
                {
                }
                _matriceCodPiese[pozitie.Linie][pozitie.Coloana] = 0;
            }
            else
            {
                if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran != null)
                {
                    StergePiesaDupaPozitieDinListe(pozitie, ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran.Culoare);

                    piesa.Pozitie = pozitie;
                    AdaugaPiesaInListe(pozitie, piesa);
                    StergePiesaCadranuluiDupaPozitie(pozitie);
                    SeteazaPiesaCadranului(pozitie, piesa);
                    _matriceCodPiese[pozitie.Linie][pozitie.Coloana] = (int)piesa.Cod;
                }
                else
                {
                    piesa.Pozitie = pozitie;
                    AdaugaPiesaInListe(pozitie, piesa);
                    SeteazaPiesaCadranului(pozitie, piesa);
                    _matriceCodPiese[pozitie.Linie][pozitie.Coloana] = (int)piesa.Cod;
                }
            }
        }

        private void StergePiesaCadranuluiDupaPozitie(Pozitie pozitie)
        {
            ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran.Dispose();
            NulificaPiesaCadranului(pozitie);
        }

        private void NulificaPiesaCadranului(Pozitie pozitie)
        {
            ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran = null;
            ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackgroundImage = null;
        }

        private void SeteazaPiesaCadranului(Pozitie pozitie, Piesa piesa)
        {
            ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran = piesa;
            ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackgroundImage = piesa.Imagine;
        }
        private void StergePiesaDupaPozitieDinListe(Pozitie pozitie,Culoare culoare)
        {
            StergePiesaDinListaDupaPoz(ListaPiese, pozitie);

            if (culoare == Culoare.AlbastruMax)
                StergePiesaDinListaDupaPoz(ListaPieseAlbastre, pozitie);
            else
                StergePiesaDinListaDupaPoz(ListaPieseAlbe, pozitie);

        }

        private void AdaugaPiesaInListe(Pozitie pozitie, Piesa piesa)
        {
            _listaPiese.Add(piesa);

            if (piesa.Culoare == Culoare.AlbastruMax)
                ListaPieseAlbastre.Add(piesa);
            else
                ListaPieseAlbe.Add(piesa);
        }

        protected virtual void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitieFinala, bool logMove = true)
        {

            Pozitie pozitieInitiala = piesa.Pozitie;
            if(logMove == true)
                AdaugaMutareaInListe(pozitieInitiala, pozitieFinala);
            AscundePiesaSelectata(piesa);
            PunePiesaPeTabla(pozitieInitiala, ConstantaTabla.PiesaNula);
            PunePiesaPeTabla(pozitieFinala, piesa);

            DecoloreazaMutariPosibile();

            PiesaSelectata = ConstantaTabla.PiesaNula;
            PozitiiMutariPosibile.Clear();

        }
        public virtual void StergeUltimaMutare()
        {
            if(_stivaMutari.Count() > 0)
            {
                var mutare = _stivaMutari.Pop();
                var codPiesa = _stivaCodPiese.Pop();


                RealizeazaMutareaLocal(GetPiesaCuPozitia(mutare.PozitieFinala), mutare.PozitieInitiala);
                if(codPiesa != 0)
                {
                    AdaugaPiesa(ConvertesteCodPiesaInObiect((CodPiesa)codPiesa), mutare.PozitieFinala);
                }
                _stivaMutari.Pop();
                _stivaCodPiese.Pop();

				AscundePiesaSelectata(mutare.PozitieInitiala);
				DecoloreazaMutariPosibile();
				PozitiiMutariPosibile.Clear();

			}
		}
        private void AdaugaMutareaInListe(Pozitie pozitieInitiala, Pozitie pozitieFinala)
        {
            var mutare = new Mutare(pozitieInitiala, pozitieFinala);
            _stivaMutari.Push(mutare);
            _stivaCodPiese.Push(MatriceCoduriPiese[pozitieFinala.Linie][pozitieFinala.Coloana]);
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
                    PunePiesaPeTabla(Pozitie.AcceseazaElementStatic(linie, coloana), ConstantaTabla.PiesaNula);
                }
            }
        }

        public static void StergePozitiaDinLista(List<Pozitie> pozitii, Pozitie pozitieDeSters)
        {
            if (pozitii.Contains(pozitieDeSters))
            {
                pozitii.Remove(pozitieDeSters);
            }
        }

        public static void ActiveazaTimerRepetitiv(System.Timers.Timer timer, uint interval, System.Timers.ElapsedEventHandler functie)
        {
            timer = new System.Timers.Timer();
            timer.AutoReset = true;
            timer.Interval = interval;
            timer.Elapsed += new System.Timers.ElapsedEventHandler(functie);
            timer.Enabled = true;
        }

        public static void BlocheazaComenzi(int milisecunde)
        {
            Timer timerAsteptare = new System.Windows.Forms.Timer();
            if (milisecunde == 0 || milisecunde < 0) return;

            timerAsteptare.Interval = milisecunde;
            timerAsteptare.Enabled = true;
            timerAsteptare.Start();

            timerAsteptare.Tick += (s, e) =>
            {
                timerAsteptare.Enabled = false;
                timerAsteptare.Stop();
            };

            while (timerAsteptare.Enabled)
            {
                Application.DoEvents();
            }
        }

        public static Culoare ReturneazaCuloareDupaCodulPiesei(CodPiesa codPiesa)
        {
            if ((int)codPiesa % 2 == 0)
                return Culoare.AlbMin;
            else
                return Culoare.AlbastruMax;
        }

        public static bool EstePiesaAlba(int codPiesa)
        {
            return codPiesa % 2 == 1;
        }

        public static bool EstePiesaAlbastra(int codPiesa)
        {
            return codPiesa % 2 == 0;
        }

        private static double[] _arrayScorPiese = new double[]
        {
            0,
            ConstantaPiese.ValoarePion,
            ConstantaPiese.ValoarePion,
            ConstantaPiese.ValoareTura,
            ConstantaPiese.ValoareTura,
            ConstantaPiese.ValoareTun,
            ConstantaPiese.ValoareTun,
            ConstantaPiese.ValoareGardian,
            ConstantaPiese.ValoareGardian,
            ConstantaPiese.ValoareElefant,
            ConstantaPiese.ValoareElefant,
            ConstantaPiese.ValoareCal,
            ConstantaPiese.ValoareCal ,
            ConstantaPiese.ValoareRege,
            ConstantaPiese.ValoareRege
        };


        public Pozitie[] ReturneazaPozitiiAlbe()
        {
            Pozitie[] pozitii = new Pozitie[ListaPieseAlbe.Count];
            int ct = 0;
            foreach (Piesa piesa in ListaPieseAlbe)
            {
                pozitii[ct] = piesa.Pozitie;
                ct++;
            }
            return pozitii;
        }

        public Pozitie[] ReturneazaPozitiiAlbastre()
        {
            Pozitie[] pozitii = new Pozitie[ListaPieseAlbastre.Count];
            int ct = 0;
            foreach (Piesa piesa in ListaPieseAlbastre)
            {
                pozitii[ct] = piesa.Pozitie;
                ct++;
            }
            return pozitii;
        }

        public static double ReturneazaScorPiesa(int codPiesa)
        {
            return _arrayScorPiese[codPiesa];
        }

        public virtual void TerminaMeciul(TipSah tipSah = TipSah.Nespecificat)
        {
            _esteGataMeciul = true;
            switch (tipSah)
            {
                case TipSah.Nespecificat:
                    {
                        FormMesaj formMesaj = new(this.ParentForm, TipCastig.NoCotest, "Meci terminat!", "(nespecificat)");
                        formMesaj.ShowDialog();
                        break;
                    }
                case TipSah.RegeAlbLuat:
                    {
                        FormMesaj formMesaj = new(this.ParentForm, TipCastig.CastigAlbastru, "Meci terminat!", "(Albastru castiga!)");
                        formMesaj.ShowDialog();
                        break;
                    }
                case TipSah.RegeAlbastruLuat:
                    {
                        FormMesaj formMesaj = new(this.ParentForm, TipCastig.CastigAlb, "Meci terminat!", "(Alb castiga!)");
                        formMesaj.ShowDialog();
                        break;
                    }
                case TipSah.SahPersistentLaAlb:
                    {
                        FormMesaj formMesaj = new(this.ParentForm, TipCastig.CastigAlbastru, "Meci terminat!", "(Albastru castiga!)");
                        formMesaj.ShowDialog();
                        break;
                    }
                case TipSah.SahPersistentLaAlbastru:
                    {
                        FormMesaj formMesaj = new(this.ParentForm, TipCastig.CastigAlb, "Meci terminat!", "(Alb castiga!)");
                        formMesaj.ShowDialog();
                        break;
                    }
                case TipSah.FaraMutariAlb:
                    {
                        FormMesaj formMesaj = new(this.ParentForm, TipCastig.CastigAlbastru, "Meci terminat!", "(Albastru castiga!)");
                        formMesaj.ShowDialog();
                        break;
                    }
                case TipSah.FaraMutariAlbastru:
                    {
                        FormMesaj formMesaj = new(this.ParentForm, TipCastig.CastigAlb, "Meci terminat!", "(Alb castiga!)");
                        formMesaj.ShowDialog();
                        break;
                    }
            }
        }

        public Pozitie ReturneazaPozitieRegeAlb()
        {
            foreach(var piesa in _listaPieseAlbe)
            {
                if (piesa.Cod == CodPiesa.RegeAlb)
                    return piesa.Pozitie;
            }
            return Pozitie.AcceseazaElementStatic(-1,-1);
        }
        public Pozitie ReturneazaPozitieRegeAlbastru()
        {
            foreach (var piesa in _listaPieseAlbastre)
            {
                if (piesa.Cod == CodPiesa.RegeAlbastru)
                    return piesa.Pozitie;
            }
            return Pozitie.AcceseazaElementStatic(-1, -1);
        }
        protected TipSah VerificaSahurile()
        {
            int codRegeAlb = (int)CodPiesa.RegeAlb;
            int codRegeAlbastru = (int)CodPiesa.RegeAlbastru;
            bool esteSah = false;
            int contorMutariAlb = 0;

            bool regeAlbPrezent = false;
            foreach (Piesa piesa in _listaPieseAlbe)
            {
                if (piesa.Cod == CodPiesa.RegeAlb)
                {
                    regeAlbPrezent = true;
                }
                List<Pozitie> mutariPosibile = piesa.ReturneazaPozitiiPosibile(_matriceCodPiese);
                contorMutariAlb += mutariPosibile.Count;
                foreach (Pozitie mutare in mutariPosibile)
                {
                    if (_matriceCodPiese[mutare.Linie][mutare.Coloana] == codRegeAlbastru)
                    {
                        _nrSahuriLaAlbastru++;
                        if (_nrSahuriLaAlbastru == 3)
                        {
                            esteSah = true;
                            return TipSah.SahPersistentLaAlbastru;
                        }
                    }
                }
            }
            if (regeAlbPrezent == false)
            {
                return TipSah.RegeAlbLuat;
            }
            if (esteSah == false)
            {
                _nrSahuriLaAlbastru = 0;
            }
            if (contorMutariAlb == 0)
            {
                return TipSah.FaraMutariAlbastru;
            }
            esteSah = false;
            int contorMutariAlbastru = 0;
            bool regeAlbastruPrezent = false;

            foreach (Piesa piesa in _listaPieseAlbastre)
            {
                if (piesa.Cod == CodPiesa.RegeAlbastru)
                {
                    regeAlbastruPrezent = true;
                }
                List<Pozitie> mutariPosibile = piesa.ReturneazaPozitiiPosibile(_matriceCodPiese);
                contorMutariAlbastru += mutariPosibile.Count;
                foreach (Pozitie mutare in mutariPosibile)
                {
                    if (_matriceCodPiese[mutare.Linie][mutare.Coloana] == codRegeAlb)
                    {
                        _nrSahuriLaAlb++;
                        if (_nrSahuriLaAlb == 3)
                        {
                            esteSah = true;
                            return TipSah.SahPersistentLaAlb;
                        }
                    }
                }
            }
            if (regeAlbastruPrezent == false)
            {
                return TipSah.RegeAlbastruLuat;
            }

            if (esteSah == false)
            {
                _nrSahuriLaAlb = 0;
            }
            if (contorMutariAlbastru == 0)
            {
                return TipSah.FaraMutariAlb;
            }
            return TipSah.NuEsteSah;
        }

        public static void AfiseazaMatriceDebug(int[][] matrice, int adancime, double eval)
        {
            Debug.WriteLine("------------------------------------------");
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    Debug.Write(matrice[linie][coloana] + " ");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine("Adancime:" + adancime);
            Debug.WriteLine("Eval:" + eval);
            Debug.WriteLine("------------------------------------------");
        }
    }
}