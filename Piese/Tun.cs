using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Tun : Piesa
    {
        public Tun(Tabla tabla, Culoare culoare, int linie, int coloana)
        {
            this.Linie = linie;
            this.Coloana = coloana;
            this.CuloarePiesa = culoare;
            if (culoare == Culoare.ALBASTRU)
            {
                this.Imagine = Properties.Resources.bcannon;
            }
            else
            {
                this.Imagine = Properties.Resources.wcannon;
            }
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
        }
    }
}
