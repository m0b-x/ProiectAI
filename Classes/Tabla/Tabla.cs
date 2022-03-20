using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Media;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class Tabla
    {
        private static List<Pozitie> _pozitiiPalat=new List<Pozitie>();

        private Form _parentForm;
        private Cadran[,] _arrayCadrane;
        private int[,] _matriceTabla;
        private List<Piesa> _listaPiese;
        private List<Pozitie> _pozitiiMutariPosibile;

        private int _pragRau;
        private int _marimeRau;

        private int _marimeVerticala;
        private int _marimeOrizontala;

        private Color _culoareCadranPar;
        private Color _culoareCadranImpar;
        private Color _culoareCadranSelectat;
        private Color _culoareCadranMutari;

        private Piesa _piesaSelectata;

        private Pozitie[] _ultimaMutare;

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

        public Cadran[,] ArrayCadrane
        {
            get { return _arrayCadrane; }
            set { _arrayCadrane = value; }
        }

        public int[,] MatriceTabla
        {
            get { return _matriceTabla; }
            set { _matriceTabla = value; }
        }

        public int MarimeVerticala
        {
            get { return _marimeVerticala; }
            private set { _marimeVerticala = value; }
        }
        public int MarimeOrizontala
        {
            get { return _marimeOrizontala; }
            private set { _marimeOrizontala = value; }
        }
        public int PragRau
        {
            get { return _pragRau; }
            private set { _pragRau = value; }
        }
        public int MarimeRau
        {
            get { return _marimeRau; }
            private set { _marimeRau = value; }
        }
        public Color CuloareCadranPar
        {
            get { return _culoareCadranPar; }
            set { _culoareCadranPar = value; }
        }

        public Color CuloareCadranImpar
        {
            get { return _culoareCadranImpar; }
            set { _culoareCadranImpar = value; }
        }

        public Color CuloareCadranSelectat
        {
            get { return _culoareCadranSelectat; }
            set { _culoareCadranSelectat = value; }
        }

        public Color CuloareCadranMutari
        {
            get { return _culoareCadranMutari; }
            set { _culoareCadranMutari = value; }
        }
        public Piesa PiesaSelectata
        {
            get { return _piesaSelectata; }
            set { _piesaSelectata = value; }
        }

        public Pozitie[] UltimaMutare
        {
            get { return _ultimaMutare };
        }

        public Tabla(Form parentForm)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>();
            _pozitiiMutariPosibile = new List<Pozitie>();

            _marimeVerticala = ConstantaTabla.MarimeVerticala;
            _marimeOrizontala = ConstantaTabla.MarimeOrizontala;

            _pragRau = ConstantaTabla.PragRau;
            _marimeRau = ConstantaTabla.MarimeRau;

            _culoareCadranImpar = ConstantaTabla.CuloareCadranImpar;
            _culoareCadranPar = ConstantaTabla.CuloareCadranPar;
            _culoareCadranMutari = ConstantaTabla.CuloareCadranMutari;
            _culoareCadranSelectat = ConstantaTabla.CuloareCadranSelectat;

            _arrayCadrane = new Cadran[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];

            _matriceTabla = new int[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];


            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    _arrayCadrane[linie, coloana] = new Cadran(this, new Pozitie(linie, coloana), DecideCuloareaCadranului(linie, coloana));
                    _arrayCadrane[linie, coloana].AddClickEventHandler(OnCadranClick);
                }
            }
        }
        public Tabla(Form parentForm,int[,] matriceTabla)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>();
            _pozitiiMutariPosibile = new List<Pozitie>();

            _marimeVerticala = ConstantaTabla.MarimeVerticala;
            _marimeOrizontala = ConstantaTabla.MarimeOrizontala;

            _pragRau = ConstantaTabla.PragRau;
            _marimeRau = ConstantaTabla.MarimeRau;

            _culoareCadranImpar = ConstantaTabla.CuloareCadranImpar;
            _culoareCadranPar = ConstantaTabla.CuloareCadranPar;
            _culoareCadranMutari = ConstantaTabla.CuloareCadranMutari;
            _culoareCadranSelectat = ConstantaTabla.CuloareCadranSelectat;

            _arrayCadrane = new Cadran[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];

            _matriceTabla = new int[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];

            _ultimaMutare = new Pozitie[2];

            ConstantaTabla.InitializeazaPolitiiPalat(ref _pozitiiPalat);

            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    _arrayCadrane[linie, coloana] = new Cadran( this, new Pozitie(linie, coloana), DecideCuloareaCadranului(linie,coloana));
                    _arrayCadrane[linie, coloana].AddClickEventHandler(OnCadranClick);
                    if(matriceTabla[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)Enum.ToObject(typeof(CodPiesa), matriceTabla[linie, coloana]));
                        AdaugaPiesa(ref piesa, new Pozitie(linie, coloana));
                    }
                }
            }
        }

        private Piesa ConvertesteCodPiesaInObiect(CodPiesa codPiesa)
        {
            switch (codPiesa)
            {
                case CodPiesa.PionAlb: return new Pion(Culoare.Alb);
                case CodPiesa.PionAlbastru: return new Pion(Culoare.Albastru);
                case CodPiesa.TuraAlba: return new Tura(Culoare.Alb);
                case CodPiesa.TuraAlbastra: return new Tura(Culoare.Albastru);
                case CodPiesa.TunAlb: return new Tun(Culoare.Alb);
                case CodPiesa.TunAlbastru: return new Tun(Culoare.Albastru);
                case CodPiesa.GardianAlb: return new Gardian(Culoare.Alb);
                case CodPiesa.GardianAlbastru: return new Gardian(Culoare.Albastru);
                case CodPiesa.ElefantAlb: return new Elefant(Culoare.Alb);
                case CodPiesa.ElefantAlbastru: return new Elefant(Culoare.Albastru); ;
                case CodPiesa.CalAlb: return new Cal(Culoare.Alb);
                case CodPiesa.CalAbastru: return new Cal(Culoare.Albastru);
                case CodPiesa.RegeAlb: return new Rege(Culoare.Alb);
                case CodPiesa.RegeAlbastru: return new Rege(Culoare.Albastru);
                default: return null;
            }

        }

        public Color DecideCuloareaCadranului(int linie,int coloana)
        {
            if (_pozitiiPalat.Contains(new Pozitie(linie, coloana)))
            {
                if (linie % 2 == 0)
                {
                    if (coloana % 2 == 1)
                    {
                        return ConstantaTabla.CuloarePalatImpar;
                    }
                    else
                    {
                       return ConstantaTabla.CuloarePalatPar;
                    }
                }
                else
                {
                    if (coloana % 2 == 1)
                    {
                        return ConstantaTabla.CuloarePalatPar;
                    }
                    else
                    {
                        return ConstantaTabla.CuloarePalatImpar;
                    }
                }
            }
            else
            {
                if (linie % 2 == 0)
                {
                    if (coloana % 2 == 1)
                    {
                        return ConstantaTabla.CuloareCadranImpar;
                    }
                    else
                    {
                        return ConstantaTabla.CuloareCadranPar;
                    }
                }
                else
                {
                    if (coloana % 2 == 1)
                    {
                        return ConstantaTabla.CuloareCadranPar;
                    }
                    else
                    {
                        return ConstantaTabla.CuloareCadranImpar;
                    }
                }
            }
        }

        public void AdaugaPiesa(ref Piesa piesa, Pozitie pozitie)
        {
            if (pozitie.Linie > MarimeVerticala || pozitie.Coloana > MarimeOrizontala || pozitie.Linie < 0 || pozitie.Coloana < 0)
            {
                Debug.WriteLine("Linie sau coloana invalida! Linie:" + pozitie.Linie + ", Coloana:" + pozitie.Coloana);
            }
            else if(piesa == ConstantaTabla.PiesaNula)
            {
                Debug.WriteLine("Piesa invalida! Linie:" + pozitie.Linie + ", Coloana:" + pozitie.Coloana);
            }
            else
            {
                if (piesa.PusaPeTabla == true)
                {
                    Debug.WriteLine("Eroare:Piesa selectata este deja pusa pe tabla!");
                }
                else
                {
                    if (_matriceTabla[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                    {
                        Debug.WriteLine("Eroare:adugare piesa peste alta! Linie:{0},Coloana:{1},PiesaAdaugata:{2}",pozitie.Linie,pozitie.Coloana,piesa.Cod);
                    }
                    else
                    {
                        piesa.Pozitie = pozitie;

                        piesa.PusaPeTabla = true;

                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);
                        this.MatriceTabla[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)piesa.Cod;
                        this.ListaPiese.Add(piesa);
                    }
                }
            }
        }

        public void ArataPiesaSelectata(Piesa piesa)
        {
            ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].BackColor = CuloareCadranSelectat;
        }
        public void ArataPiesaBlocata(Pozitie pozitie)
        {
            ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = ConstantaTabla.CuloarePiesaBlocata;
            System.Timers.Timer timerPiesaBlocata = new System.Timers.Timer();
            timerPiesaBlocata.Elapsed += new ElapsedEventHandler((sender, e) => DecoloreazaPiesaBlocata(sender, e, pozitie, timerPiesaBlocata));
            timerPiesaBlocata.Interval = 400;
            timerPiesaBlocata.Enabled = true;
            ConstantaSunet.SunetMutareGresita.Play();
        }
        private void DecoloreazaPiesaBlocata(object source, ElapsedEventArgs e,Pozitie pozitie, System.Timers.Timer timer)
        {
            ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = DecideCuloareaCadranului(pozitie.Linie, pozitie.Coloana);
            timer.Stop();
        }

        public void AscundePiesaSelectata(Piesa piesa)
        {
            ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].BackColor = DecideCuloareaCadranului(piesa.Pozitie.Linie,piesa.Pozitie.Coloana);
        }


        public void DecoloreazaMutariPosibile(List<Pozitie> pozitii)
        {
            if (_pozitiiMutariPosibile != null)
            {
                foreach (Pozitie pozitie in _pozitiiMutariPosibile)
                {
                    ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = DecideCuloareaCadranului(pozitie.Linie, pozitie.Coloana);
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

        public void ActualizeazaUltimaMutare(Pozitie pozitieInitiala,Pozitie pozitiFinala)
        {
            _ultimaMutare[0] = pozitieInitiala;
            _ultimaMutare[1] = pozitiFinala;
        }

        public void MutaPiesa(Piesa piesa, Pozitie pozitie)
        {
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
                    if (_matriceTabla[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                    {
                        _listaPiese.Remove(GetPiesaCuPozitia(pozitie));
                        _matriceTabla[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(ConstantaTabla.PiesaNula);

                        ActualizeazaUltimaMutare(new Pozitie(piesa.Pozitie.Linie, piesa.Pozitie.Coloana), new Pozitie(pozitie.Linie, pozitie.Coloana);

                        piesa.Pozitie = pozitie;
                        _matriceTabla[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);

                        ConstantaSunet.SunetPiesaLuata.Play();
                    }
                    else
                    {
                        _matriceTabla[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                        _matriceTabla[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(ConstantaTabla.PiesaNula);

                        piesa.Pozitie = pozitie;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);

                        ConstantaSunet.SunetPiesaMutata.Play();
                    }
                    DecoloreazaMutariPosibile(_pozitiiMutariPosibile);
                    _piesaSelectata = ConstantaTabla.PiesaNula;
                    _pozitiiMutariPosibile.Clear();
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
                        return piesa;
                }
            }
            return null;
        }

        public void CurataTabla()
        {
            for (int linie = 0; linie < _marimeOrizontala; linie++)
            {
                for (int coloana = 0; coloana < _marimeVerticala; coloana++)
                {
                    _arrayCadrane[linie, coloana].setPiesa(ConstantaTabla.PiesaNula);
                }
            }
        }
        public void OnCadranClick(object sender, EventArgs e)
        {
            if (_piesaSelectata == ConstantaTabla.PiesaNula)
            {
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;


                if (_arrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                {
                    Piesa piesa = GetPiesaCuPozitia(pozitie);
                    piesa.ArataMutariPosibile(this);
                    if (ExistaMutariPosibile() == true)
                    {
                        ArataPiesaSelectata(piesa);
                        Debug.WriteLine("Click Piesa:" + piesa.Cod + "->[linie:" + piesa.Pozitie.Linie + ",coloana:" + piesa.Pozitie.Coloana + "]");
                        _piesaSelectata = piesa;
                    }
                    else 
                    {
                        ArataPiesaBlocata(pozitie);
                        return;
                    }
                }
                else
                {
                    Debug.WriteLine("Click Piesa:Gol->[linie:" + pozitie.Linie + ",coloana:" + pozitie.Coloana + "]");
                }
            }
            else
            {
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;

                if (_piesaSelectata.Pozitie == pozitie)
                {
                    return;
                }
                if (EsteMutareaPosibila(pozitie))
                {
                    AscundePiesaSelectata(_piesaSelectata);
                    MutaPiesa(_piesaSelectata, pozitie);
                }
                else
                {
                    return;
                }
            }
        }

    }
}
