using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class Tabla
    {
        private Form _parentForm;
        private Cadran[,] _arrayCadrane;
        private int[,] _matriceTabla;
        private List<Piesa> _listaPiese;
        private List<Pozitie> _pozitiiMutariPosibile;

        private int _pragRau;
        private int _marimeRau;

        private int _marimeTablaOrizontala;
        private int _marimeTablaVerticala;

        private Color _culoareCadranPar;
        private Color _culoareCadranImpar;
        private Color _culoareCadranSelectat;
        private Color _culoareCadranMutari;

        private Piesa _piesaSelectata;

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

        public int MarimeTablaOrizontala
        {
            get { return _marimeTablaOrizontala; }
            private set { _marimeTablaOrizontala = value; }
        }
        public int MarimeTablaVerticala
        {
            get { return _marimeTablaVerticala; }
            private set { _marimeTablaVerticala = value; }
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

        public Tabla(Form parentForm)
        {
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>();
            _pozitiiMutariPosibile = new List<Pozitie>();

            _marimeTablaOrizontala = ConstantaTabla.MarimeTablaOrizontala;
            _marimeTablaVerticala = ConstantaTabla.MarimeTablaVerticala;

            _pragRau = ConstantaTabla.PragRau;
            _marimeRau = ConstantaTabla.MarimeRau;

            _culoareCadranImpar = ConstantaTabla.CuloareCadranImpar;
            _culoareCadranPar = ConstantaTabla.CuloareCadranPar;
            _culoareCadranMutari = ConstantaTabla.CuloareCadranMutari;
            _culoareCadranSelectat = ConstantaTabla.CuloareCadranSelectat;

            _arrayCadrane = new Cadran[ConstantaTabla.MarimeTablaOrizontala, ConstantaTabla.MarimeTablaVerticala];

            _matriceTabla = new int[ConstantaTabla.MarimeTablaOrizontala, ConstantaTabla.MarimeTablaVerticala];

            for (int linie = 0; linie < ConstantaTabla.MarimeTablaOrizontala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeTablaVerticala; coloana++)
                {
                   
                    _arrayCadrane[linie, coloana] = new Cadran( this, new Pozitie(linie, coloana), DecideCuloareaCadranului(linie,coloana));
                    _arrayCadrane[linie, coloana].AddClickEventHandler(OnCadranClick);
                }
            }
        }

        public Color DecideCuloareaCadranului(int linie,int coloana)
        {
            if (linie % 2 == 0)
            {
                if (coloana % 2 == 1)
                {
                    return _culoareCadranImpar;
                }
                else
                {
                    return _culoareCadranPar;
                }
            }
            else
            {
                if (coloana % 2 == 1)
                {
                    return _culoareCadranPar;
                }
                else
                {
                    return _culoareCadranImpar;
                }
            }
        }

        public void AdaugaPiesa(Piesa piesa, Pozitie pozitie)
        {
            if (pozitie.Linie > MarimeTablaOrizontala || pozitie.Coloana > MarimeTablaVerticala || pozitie.Linie < 0 || pozitie.Coloana < 0)
            {
                Debug.WriteLine("Linie sau coloana invalida! Linie:" + pozitie.Linie + ", Coloana:" + pozitie.Coloana);
            }
            else
            {
                if (piesa.PusaPeTabla == true)
                {
                    Debug.Write("Eroare:Piesa selectata este deja pusa pe tabla!");
                }
                else
                {
                    if (_matriceTabla[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                    {
                        Debug.WriteLine("Eroare:Nu se poate adauga piesa una peste alta!");
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

        public bool ArePiesaMutariPosibile()
        {
            if (_pozitiiMutariPosibile.Count == 0 || _pozitiiMutariPosibile == null)
                return true;
            return false;
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

        public void MutaPiesa(Piesa piesa, Pozitie pozitie)
        {
            if (pozitie.Linie > MarimeTablaOrizontala || pozitie.Coloana > MarimeTablaVerticala || pozitie.Linie < 0 || pozitie.Coloana < 0)
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
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(null);

                        piesa.Pozitie = pozitie;
                        _matriceTabla[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);
                    }
                    else
                    {
                        _matriceTabla[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                        _matriceTabla[pozitie.Linie, pozitie.Coloana] = (int)piesa.Cod;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(null);

                        piesa.Pozitie = pozitie;
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);
                    }
                    DecoloreazaMutariPosibile(_pozitiiMutariPosibile);
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
            for (int linie = 0; linie < _marimeTablaOrizontala; linie++)
            {
                for (int coloana = 0; coloana < _marimeTablaVerticala; coloana++)
                {
                    _arrayCadrane[linie, coloana].setPiesa(null);
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
                    if (ArePiesaMutariPosibile() == true)
                    {
                        Piesa piesa = GetPiesaCuPozitia(pozitie);
                        Debug.WriteLine("Click Piesa:" + piesa.Cod + "->[linie:" + piesa.Pozitie.Linie + ",coloana:" + piesa.Pozitie.Coloana + "]");
                        _piesaSelectata = piesa;
                        ArataPiesaSelectata(piesa);
                        piesa.ArataMutariPosibile(this);
                    }
                    else 
                    {
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
                    _piesaSelectata = ConstantaTabla.PiesaNula;
                    _pozitiiMutariPosibile = new List<Pozitie>();
                }
                else
                {
                    return;
                }
            }
        }

    }
}
