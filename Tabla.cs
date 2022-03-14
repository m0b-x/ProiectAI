using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class Tabla
    {

        private cadran[,] arrayCadrane;
        private int[,] matriceTabla;

        private int marimeTablaOrizontala;
        private int marimeTablaVerticala;

        public Tabla(Form parentForm, int marimeTablaOrizontala,int marimeTablaVerticala)
        {
            this.marimeTablaOrizontala = marimeTablaOrizontala;
            this.marimeTablaVerticala = marimeTablaVerticala;

            arrayCadrane = new cadran[marimeTablaOrizontala, marimeTablaVerticala];

            matriceTabla = new int[marimeTablaOrizontala, marimeTablaVerticala];

            for (int linie = 0; linie < marimeTablaOrizontala; linie++)
            {
                for (int coloana = 0; coloana < marimeTablaVerticala; coloana++)
                {
                    arrayCadrane[linie, coloana] = new cadran(parentForm, linie, coloana);
                }
            }
        }

        public void setCadranBackground(int linie, int coloana, System.Drawing.Image imagine)
        {
            arrayCadrane[linie, coloana].BackgroundImage = imagine;
        }

        public void stergeTabla()
        {
            for (int linie = 0; linie < marimeTablaOrizontala; linie++)
            {
                for (int coloana = 0; coloana < marimeTablaVerticala; coloana++)
                {
                    arrayCadrane[linie, coloana].BackgroundImage = null;
                }
            }
        }

    }
}
