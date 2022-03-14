using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class Tabla
    {

        private Cadran[,] _arrayCadrane;
        private int[,] _matriceTabla;

        private int _marimeTablaOrizontala;
        private int _marimeTablaVerticala;

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

        public Tabla(Form parentForm, int marimeTablaOrizontala,int marimeTablaVerticala)
        {
            this._marimeTablaOrizontala = marimeTablaOrizontala;
            this._marimeTablaVerticala = marimeTablaVerticala;

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

        public void setCadranBackground(int linie, int coloana, System.Drawing.Image imagine)
        {
            _arrayCadrane[linie, coloana].BackgroundImage = imagine;
        }

        public void stergeTabla()
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
