using ProiectVolovici.Visual_Classes;
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
        protected Tabla _tabla;

        protected int[][] _matriceCodPiese;
        protected Aspect _aspectJoc;
        private List<Piesa> _listaPiese;
        private List<Piesa> _listaPieseAlbe;
        private List<Piesa> _listaPieseAlbastre;
        protected List<Pozitie> _pozitiiMutariPosibile;

        private Label[] _labelColoane;
        private Label[] _labelLinii;
        private Label _labelAsteptare;

        private Piesa _piesaSelectata;

        private Tuple<Pozitie, Pozitie> _ultimaMutare;

        protected bool _esteGataMeciul;
        protected int _nrSahuriLaAlb = 0;
        protected int _nrSahuriLaAlbastru = 0;
        protected bool _sahPersistentLaAlb = false;
        protected bool _sahPersistentLaAlbastru = false;

        public Aspect AspectJoc
        {
            get { return _aspectJoc; }
            set { _aspectJoc = value; }
        }
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

        public Tuple<Pozitie, Pozitie> UltimaMutare
        {
            get { return _ultimaMutare; }
        }

        public EngineJoc(Form parentForm, Aspect aspect = Aspect.Normal)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>(32);
            _listaPieseAlbe = new List<Piesa>(16);
            _listaPieseAlbastre = new List<Piesa>(16);
            _pozitiiMutariPosibile = new List<Pozitie>(200);
            _aspectJoc = aspect;

            _tabla = new Tabla();
            _ultimaMutare = new Tuple<Pozitie, Pozitie>(new Pozitie(0, 0), new Pozitie(0, 0));

            _matriceCodPiese = new int[ConstantaTabla.NrLinii][];
            for (int i = 0; i < ConstantaTabla.NrLinii; i++)
                _matriceCodPiese[i] = new int[ConstantaTabla.NrColoane];

            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie][coloana] = new Cadran(this, new Pozitie(linie, coloana), _tabla.DecideCuloareaCadranului(linie, coloana));
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
            _pozitiiMutariPosibile = new List<Pozitie>(200);
            _aspectJoc = aspect;

            _tabla = new Tabla();
            _ultimaMutare = new Tuple<Pozitie, Pozitie>(new Pozitie(0, 0), new Pozitie(0, 0));

            _matriceCodPiese = new int[ConstantaTabla.NrLinii][];
            for (int i = 0; i < ConstantaTabla.NrLinii; i++)
                _matriceCodPiese[i] = new int[ConstantaTabla.NrColoane];

            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie][coloana] = new Cadran(this, new Pozitie(linie, coloana), _tabla.DecideCuloareaCadranului(linie, coloana));
                    if (matriceTabla[linie][coloana] != (int)CodPiesa.Gol)
                    {
                        Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)Enum.ToObject(typeof(CodPiesa), matriceTabla[linie][coloana]));
                        AdaugaPiesa(piesa, new Pozitie(linie, coloana));
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

            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), new Pozitie(3, 0));
            
            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), new Pozitie(3, 2));
            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), new Pozitie(3, 4));
            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), new Pozitie(3, 6));
            AdaugaPiesa(new Pion(Culoare.AlbastruMax, _aspectJoc), new Pozitie(3, 8));

            AdaugaPiesa(new Tun(Culoare.AlbastruMax, _aspectJoc), new Pozitie(2, 1));
            AdaugaPiesa(new Tun(Culoare.AlbastruMax, _aspectJoc), new Pozitie(2, 7));

            AdaugaPiesa(new Tura(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 0));
            AdaugaPiesa(new Tura(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 8));

            AdaugaPiesa(new Cal(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 1));
            AdaugaPiesa(new Cal(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 7));

            AdaugaPiesa(new Elefant(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 2));
            AdaugaPiesa(new Elefant(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 6));

            AdaugaPiesa(new Gardian(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 3));
            AdaugaPiesa(new Gardian(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 5));
            
            AdaugaPiesa(new Rege(Culoare.AlbastruMax, _aspectJoc), new Pozitie(0, 4));

            //piese albe

            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), new Pozitie(6, 0));
            
            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), new Pozitie(6, 2));
            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), new Pozitie(6, 4));
            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), new Pozitie(6, 6));
            AdaugaPiesa(new Pion(Culoare.AlbMin, _aspectJoc), new Pozitie(6, 8));

            AdaugaPiesa(new Tun(Culoare.AlbMin, _aspectJoc), new Pozitie(7, 1));
            AdaugaPiesa(new Tun(Culoare.AlbMin, _aspectJoc), new Pozitie(7, 7));

            AdaugaPiesa(new Tura(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 0));
            AdaugaPiesa(new Tura(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 8));

            AdaugaPiesa(new Cal(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 1));
            AdaugaPiesa(new Cal(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 7));

            AdaugaPiesa(new Elefant(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 2));
            AdaugaPiesa(new Elefant(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 6));

            AdaugaPiesa(new Gardian(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 3));
            AdaugaPiesa(new Gardian(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 5));
           
            AdaugaPiesa(new Rege(Culoare.AlbMin, _aspectJoc), new Pozitie(9, 4));
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
                        AdaugaPiesa(piesa, new Pozitie(linie, coloana));
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
                SeteazaPiesaCadranului(piesa.Pozitie,piesa);
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

        public void DecoloreazaMutariPosibile(List<Pozitie> pozitii)
        {
            if (_pozitiiMutariPosibile != null)
            {
                foreach (Pozitie pozitie in _pozitiiMutariPosibile)
                {
                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor = _tabla.DecideCuloareaCadranului(pozitie.Linie, pozitie.Coloana);
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
                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackColor = CuloareCadranMutari;
                    _pozitiiMutariPosibile.Add(new Pozitie(pozitie.Linie, pozitie.Coloana));
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

        protected void ActualizeazaUltimaaMutare(Pozitie pozitieInitiala, Pozitie pozitieFinala)
        {
            _ultimaMutare = new Tuple<Pozitie, Pozitie>(pozitieInitiala, pozitieFinala);
        }

        public void StergePiesaDinListaDupaPoz(ref List<Piesa> lista, Pozitie poz)
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

        public void SeteazaPiesaCadranului(Pozitie pozitie,Piesa piesa)
        {
            if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
            {
                //se pune piesa peste piesa => adaugam piesa noua,stergem piesa veche
                if (piesa != ConstantaTabla.PiesaNula)
                {
                    //stergem piesa veche
                    StergePiesaDinListaDupaPoz(ref _listaPiese, pozitie);
                    if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran.Culoare == Culoare.AlbMin)
                    {
                        StergePiesaDinListaDupaPoz(ref _listaPieseAlbe, pozitie);
                    }
                    else
                    {
                        StergePiesaDinListaDupaPoz(ref _listaPieseAlbastre, pozitie);
                    }
                    //adaugam piesa noua
                    piesa.Pozitie = pozitie;
                    _listaPiese.Add(piesa);
                    if (piesa.Culoare == Culoare.AlbMin)
                    {
                        _listaPieseAlbe.Add(piesa);
                    }
                    else
                    {
                        _listaPieseAlbastre.Add(piesa);
                    }

                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran = piesa;
                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackgroundImage = piesa.Imagine;
                    _matriceCodPiese[pozitie.Linie][pozitie.Coloana] = (int)piesa.Cod;
                }
                //se pune gol peste piesa=>stergem piesa veche
                else
                {
                    if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran.Culoare == Culoare.AlbMin)
                    {
                        StergePiesaDinListaDupaPoz(ref _listaPieseAlbe, pozitie);
                    }
                    else if(ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran.Culoare == Culoare.AlbastruMax)
                    {
                        StergePiesaDinListaDupaPoz(ref _listaPieseAlbastre, pozitie);
                    }
                    StergePiesaDinListaDupaPoz(ref _listaPiese, pozitie);

                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran = ConstantaTabla.PiesaNula;
                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackgroundImage = null;
                    _matriceCodPiese[pozitie.Linie][pozitie.Coloana] = (int)CodPiesa.Gol;
                }
            }
            else
            {
                //piesa peste gol => adaugam doar
                if (piesa != ConstantaTabla.PiesaNula)
                {
                    piesa.Pozitie = pozitie;
                    if (piesa.Culoare == Culoare.AlbMin)
                    {
                        _listaPieseAlbe.Add(piesa);
                    }
                    else
                    {
                        _listaPieseAlbastre.Add(piesa);
                    }
                    _listaPiese.Add(piesa);
                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran = piesa;
                    ArrayCadrane[pozitie.Linie][pozitie.Coloana].BackgroundImage = piesa.Imagine;
                    _matriceCodPiese[pozitie.Linie][pozitie.Coloana] = (int)piesa.Cod;
                }
                //gol peste gol =>redundant
                else
                {
                }
            }
        }

        protected virtual void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitiaFinala)
        {
            Pozitie pozitieInitiala = piesa.Pozitie;
            AscundePiesaSelectata(piesa);
            SeteazaPiesaCadranului(pozitieInitiala, ConstantaTabla.PiesaNula);
            SeteazaPiesaCadranului(pozitiaFinala, piesa);

            DecoloreazaMutariPosibile(PozitiiMutariPosibile);
            ActualizeazaUltimaMutare(pozitieInitiala, pozitiaFinala);

            PiesaSelectata = ConstantaTabla.PiesaNula;
            PozitiiMutariPosibile.Clear();

        }
		protected void ActualizeazaUltimaMutare(Pozitie pozitieInitiala, Pozitie pozitieFinala)
		{
			_ultimaMutare = new Tuple<Pozitie, Pozitie>(pozitieInitiala, pozitieFinala);
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

        public void StergePozitiaDinLista(ref List<Pozitie> pozitii, Pozitie pozitieDeSters)
        {
            if (pozitii.Contains(pozitieDeSters))
            {
                pozitii.Remove(pozitieDeSters);
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

        public void BlocheazaComenzi(int milisecunde)
        {
            var timerAsteptare = new System.Windows.Forms.Timer();
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

        public Culoare ReturneazaCuloareDupaCodulPiesei(CodPiesa codPiesa)
        {
            if ((int)codPiesa % 2 == 0)
                return Culoare.AlbMin;
            else
                return Culoare.AlbastruMax;
        }

        public bool EstePiesaAlba(int codPiesa)
        {
            return codPiesa % 2 == 1 ? true : false;
        }

        public bool EstePiesaAlbastra(int codPiesa)
        {
            return codPiesa % 2 == 0 ? true : false;
        }

        private double[] _arrayScorPiese = new double[]
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

        public double ReturneazaScorPiesa(int codPiesa, int linie, int coloana)
        {
            if(codPiesa == (int)CodPiesa.PionAlb)
            {
                if (linie <= ConstantaTabla.PragRau)
                    return ConstantaPiese.ValoarePionDupaRau;
            }
            if (codPiesa == (int)CodPiesa.PionAlbastru)
            {
                if (linie > ConstantaTabla.PragRau)
                    return ConstantaPiese.ValoarePionDupaRau;
            }
            return _arrayScorPiese[(int)codPiesa];
        }

        public virtual void TerminaMeciul(TipSah tipSah = TipSah.Nespecificat)
        {
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
            _esteGataMeciul = true;
        }

        protected void TerminaMeciulDacaEsteSahDirect(TipSah tipSah, int piesaLuata)
        {
            if (piesaLuata == (int)CodPiesa.RegeAlb)
            {
                TerminaMeciul(TipSah.RegeAlbLuat);
            }
            else if (piesaLuata == (int)CodPiesa.RegeAlbastru)
            {
                TerminaMeciul(TipSah.RegeAlbastruLuat);
            }
            if (tipSah == TipSah.SahPersistentLaAlb)
            {
              //  TerminaMeciul(TipSah.SahPersistentLaAlb);
            }
            else if (tipSah == TipSah.SahPersistentLaAlbastru)
            {
               // TerminaMeciul(TipSah.SahPersistentLaAlbastru);
            }
        }

        protected TipSah VerificaSahulPersistent()
        {
            int codRegeAlb = (int)CodPiesa.RegeAlb;
            int codRegeAlbastru = (int)CodPiesa.RegeAlbastru;
            bool esteSah = false;
            int contorMutariAlb = 0;
            foreach (var piesa in _listaPieseAlbe)
            {
                var mutariPosibile = piesa.ReturneazaMutariPosibile(_matriceCodPiese);
                contorMutariAlb += mutariPosibile.Count;
                foreach (var mutare in mutariPosibile)
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
            foreach (var piesa in _listaPieseAlbastre)
            {
                var mutariPosibile = piesa.ReturneazaMutariPosibile(_matriceCodPiese);
                contorMutariAlbastru += mutariPosibile.Count;
                foreach (var mutare in mutariPosibile)
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

        public void AfiseazaMatriceDebug(int[][] matrice, int adancime, double eval)
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