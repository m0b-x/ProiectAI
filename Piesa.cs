using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public abstract class Piesa
    {
        protected Culoare _culoarePiesa;
        protected int _liniePiesa;
        protected int _coloanaPiesa;
        protected Image _imaginePiesa;
        protected CodPiesa _codPiesa;

        public Culoare CuloarePiesa
        {
            get { return _culoarePiesa; }
            set { _culoarePiesa = value; }
        }
        public int Linie
        {
            get { return _liniePiesa; }
            set { _liniePiesa = value; }
        }

        public int Coloana
        {
            get { return _coloanaPiesa; }
            set { _coloanaPiesa = value; }
        }
        public Image Imagine
        {
            get { return _imaginePiesa; }
            set { _imaginePiesa = value; }
        }

        public CodPiesa Cod
        {
            get { return _codPiesa; }
            set { _codPiesa = value; }
        }

        public void mutaPiesa(Tabla tabla,int linie,int coloana)
        {
            if (linie > tabla.MarimeTablaOrizontala ||
               coloana > tabla.MarimeTablaVerticala ||
               linie < 0 ||
               coloana < 0)
            {
                Console.WriteLine("Linie sau coloana invalida! Linie:" + linie + ", Coloana:" + coloana);
            }
            else
            {
                tabla.ArrayCadrane[this.Linie, this.Coloana].BackgroundImage = null;
                tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
                this.Linie = linie;
                this.Coloana = coloana;
            }
        }

    }
}
