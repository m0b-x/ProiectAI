using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class Tabla
    {
        private Form _parentForm;
        private Cadran[,] _arrayCadrane;
        private int[,] _matriceTabla;

        private int _marimeTablaOrizontala;
        private int _marimeTablaVerticala;

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

        public void getPiesaDinCoordonate(Tabla tabla, int liniePiesa, int coloanaPiesa)
        {
            for (int linie = 0; linie < _marimeTablaOrizontala; linie++)
            {
                for (int coloana = 0; coloana < _marimeTablaVerticala; coloana++)
                {
                    _arrayCadrane[linie, coloana].BackgroundImage = null;
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
