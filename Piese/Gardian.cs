using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Gardian : Piesa
    {
        public Gardian(Tabla tabla, Culoare culoare, int linie, int coloana)
        {
            this.Linie = linie;
            this.Coloana = coloana;
            this.CuloarePiesa = culoare;
            if (culoare == Culoare.ALBASTRU)
            {
                this.Imagine = Properties.Resources.benvoy;
            }
            else
            {
                this.Imagine = Properties.Resources.wenvoy;
            }
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
        }

    }
}
