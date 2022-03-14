using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    abstract class Piesa
    {
        protected Culoare _culoarePiesa;
        protected int _liniePiesa;
        protected int _coloanaPiesa;
        protected Image _imaginePiesa;

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

        public void mutaPiesa(Tabla tabla,int linie,int coloana)
        {
            if(linie > tabla.)

            tabla.ArrayCadrane[this.Linie, this.Coloana].BackgroundImage = null;
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
        }

    }
}
