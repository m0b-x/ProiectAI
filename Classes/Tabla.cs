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

        private int _pragRau = 4;
        private int _offsetRau = 25;

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
        public int PragRau
        {
            get { return _pragRau; }
            private set { _pragRau = value; }
        }
        public int OffsetRau
        {
            get { return _pragRau; }
            private set { _pragRau = value; }
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
                    _arrayCadrane[linie, coloana] = new Cadran(parentForm, this, linie, coloana);
                    _arrayCadrane[linie, coloana].AddEventHandler(OnCadranClick);
                }
            }
        }

        public void AdaugaPiesa(Piesa piesa, int linie, int coloana)
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
                    if (_matriceTabla[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        Console.WriteLine("Eroare:Nu se poate adauga piesa una peste alta!");
                    }
                    else
                    {
                        piesa.Linie = linie;
                        piesa.Coloana = coloana;

                        piesa.PusaPeTabla = true;

                        this.ArrayCadrane[piesa.Linie, piesa.Coloana].setPiesa(piesa);
                        this.MatriceTabla[piesa.Linie, piesa.Coloana] = (int)piesa.Cod;
                        this.ListaPiese.Add(piesa);
                    }
                }
            }

        }


        public void MutaPiesa(Piesa piesa, int linie, int coloana)
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
                        _listaPiese.Remove(GetPiesaCuPozitia(linie, coloana));
                        _matriceTabla[piesa.Linie, piesa.Coloana] = (int)CodPiesa.Gol;
                        this.ArrayCadrane[piesa.Linie, piesa.Coloana].setPiesa(null);

                        piesa.Linie = linie;
                        piesa.Coloana = coloana;
                        _matriceTabla[linie, coloana] = (int)piesa.Cod;
                        this.ArrayCadrane[piesa.Linie, piesa.Coloana].setPiesa(piesa);
                    }
                    else
                    {
                        _matriceTabla[piesa.Linie, piesa.Coloana] = (int)CodPiesa.Gol;
                        _matriceTabla[linie, coloana] = (int)piesa.Cod;
                        this.ArrayCadrane[piesa.Linie, piesa.Coloana].setPiesa(null);

                        piesa.Linie = linie;
                        piesa.Coloana = coloana;
                        this.ArrayCadrane[piesa.Linie, piesa.Coloana].setPiesa(piesa);
                    }
                }
            }
        }

        public Piesa GetPiesaCuPozitia(int linie, int coloana)
        {
            foreach (Piesa piesa in _listaPiese)
            {
                if (piesa.Coloana == coloana && piesa.Linie == linie)
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
            int linie, coloana;
            linie = (sender as Cadran).Linie;
            coloana = (sender as Cadran).Coloana;
            if(_arrayCadrane[linie, coloana].BackgroundImage != null)
            {
                Piesa piesa = GetPiesaCuPozitia(linie, coloana);
                Console.WriteLine("Piesa:" + piesa.Cod);
            }
            else
            {
                Console.WriteLine("Piesa:Gol");
            }
        }

    }
}
