using System;
using System.Collections.Generic;
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

        private int _marimeTablaOrizontala;
        private int _marimeTablaVerticala;

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

        public Tabla(Form parentForm, int marimeTablaOrizontala,int marimeTablaVerticala)
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
                    _arrayCadrane[linie, coloana] = new Cadran(parentForm, linie, coloana);
                }
            }
        }

        public void AdaugaPiesa(Piesa piesa,int linie = -1,int coloana = -1)
        {
            if (linie > MarimeTablaOrizontala || coloana > MarimeTablaVerticala || linie < 0 || coloana < 0)
            {
                Console.WriteLine("Linie sau coloana invalida! Linie:" + linie + ", Coloana:" + coloana);
            }
            else
            {
                if (piesa.PusaPeTabla == true)
                {
                    Console.Write("Eroare:Piesa selectata este deja pusa pe tabla!");
                }
                else
                { 
                    if (linie == -1)
                    {
                        linie = piesa.Linie;
                    }
                    else
                    {
                        piesa.Linie = linie;
                    }

                    if (coloana == -1)
                    {
                        coloana = piesa.Coloana;
                    }
                    else
                    {
                        piesa.Coloana = coloana;
                    }
                    piesa.PusaPeTabla = true;

                    this.ArrayCadrane[piesa.Linie, piesa.Coloana].BackgroundImage = piesa.Imagine;
                    this.MatriceTabla[piesa.Linie, piesa.Coloana] = (int)piesa.Cod;
                    this.ListaPiese.Add(piesa);
                }
            }
           
        }


        public void MutaPiesa(Piesa piesa,int linie,int coloana)
        {
            if (linie > MarimeTablaOrizontala || coloana > MarimeTablaVerticala || linie < 0 || coloana < 0)
            {
                Console.WriteLine("Linie sau coloana invalida! Linie:" + linie + ", Coloana:" + coloana);
            }
            else
            {
                if (piesa.PusaPeTabla == false)
                {
                    Console.WriteLine("Eroare:Piesa nu este pusa pe tabla!");
                }
                else
                {
                    if (_matriceTabla[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        _listaPiese.Remove(PiesaCuPozitia(linie, coloana));
                        _matriceTabla[piesa.Linie, piesa.Coloana] = (int)CodPiesa.Gol;
                        ArrayCadrane[piesa.Linie, piesa.Coloana].BackgroundImage = null;

                        _matriceTabla[linie, coloana] = (int)piesa.Cod;
                        ArrayCadrane[linie, coloana].BackgroundImage = piesa.Imagine;
                        piesa.Linie = linie;
                        piesa.Coloana = coloana;
                    }
                    else
                    {
                        _matriceTabla[piesa.Linie, piesa.Coloana] = (int)CodPiesa.Gol;
                        _matriceTabla[linie, coloana] = (int)piesa.Cod;
                        ArrayCadrane[piesa.Linie, piesa.Coloana].BackgroundImage = null;

                        ArrayCadrane[linie, coloana].BackgroundImage = piesa.Imagine;
                        piesa.Linie = linie;
                        piesa.Coloana = coloana;
                    }
                }
            }
        }

        public Piesa PiesaCuPozitia(int linie,int coloana)
        {
            foreach(Piesa piesa in _listaPiese)
            {
                if (piesa.Coloana == coloana && piesa.Linie == linie)
                    return piesa;
            }
            return null;
        }
 


        public void GetPiesaDinCoordonate(Tabla tabla, int liniePiesa, int coloanaPiesa)
        {
            for (int linie = 0; linie < _marimeTablaOrizontala; linie++)
            {
                for (int coloana = 0; coloana < _marimeTablaVerticala; coloana++)
                {
                    _arrayCadrane[linie, coloana].BackgroundImage = null;
                }
            }
        }

        public void SetCadranBackground(int linie, int coloana, System.Drawing.Image imagine)
        {
            _arrayCadrane[linie, coloana].BackgroundImage = imagine;
        }

        public void CurataTabla()
        {
            for (int linie = 0; linie < _marimeTablaOrizontala; linie++)
            {
                for (int coloana = 0; coloana < _marimeTablaVerticala; coloana++)
                {
                    _arrayCadrane[linie, coloana].BackgroundImage = null;
                }
            }
        }


    }
}
