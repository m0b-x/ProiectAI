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


        private int _pragRau = 4;
        private int _offsetRau = 10;

        private int _marimeTablaOrizontala;
        private int _marimeTablaVerticala;

        private Color _culoareCadranPar = Color.BlanchedAlmond;
        private Color _culoareCadranImpar = Color.DarkGreen;
        private Color _culoareCadranSelectat = Color.DeepSkyBlue;
        private Color _culoareCadranMutari = Color.DodgerBlue;

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
        public int OffsetRau
        {
            get { return _offsetRau; }
            private set { _offsetRau = value; }
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

        public Tabla(Form parentForm, int marimeTablaOrizontala, int marimeTablaVerticala)
        {
            _marimeTablaOrizontala = marimeTablaOrizontala;
            _marimeTablaVerticala = marimeTablaVerticala;
            _parentForm = parentForm;
            _listaPiese = new List<Piesa>();

            _arrayCadrane = new Cadran[marimeTablaOrizontala, marimeTablaVerticala];

            _matriceTabla = new int[marimeTablaOrizontala, marimeTablaVerticala];

            for (int linie = 0; linie < marimeTablaOrizontala; linie++)
            {
                for (int coloana = 0; coloana < marimeTablaVerticala; coloana++)
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

        
        public void ColoreazaMutariPosibile(List<Pozitie> pozitii)
        {
            foreach(Pozitie pozitie in pozitii)
            {
                ArrayCadrane[pozitie.Linie, pozitie.Coloana].BackColor = CuloareCadranMutari;
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
                        Debug.WriteLine(pozitie.Linie + " " + pozitie.Coloana);
                        this.ArrayCadrane[piesa.Pozitie.Linie, piesa.Pozitie.Coloana].setPiesa(piesa);
                    }
                }
            }
        }

        public Piesa GetPiesaCuPozitia(Pozitie pozitie)
        {
            foreach (Piesa piesa in _listaPiese)
            {
                if (piesa.Pozitie == pozitie)
                    return piesa;
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
            if (_piesaSelectata == null)
            {
                Pozitie pozitie = new Pozitie(0, 0);
                pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;


                if (_arrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran != null)
                {
                    Piesa piesa = GetPiesaCuPozitia(pozitie);
                    Debug.WriteLine("Click Piesa:" + piesa.Cod + "->[linie:" + piesa.Pozitie.Linie + ",coloana:" + piesa.Pozitie.Coloana + "]");
                    _piesaSelectata = piesa;
                    ArataPiesaSelectata(piesa);
                    piesa.ArataMutariPosibile(this);
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

                Debug.WriteLine("Dublu Click->[linie:" + pozitie.Linie + ",coloana:" + pozitie.Coloana + "]");
                AscundePiesaSelectata(_piesaSelectata);
                MutaPiesa(_piesaSelectata, pozitie);
                _piesaSelectata = null;
            }
        }

    }
}
