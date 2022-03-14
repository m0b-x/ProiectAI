using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Rege : Piesa
    {
        public Rege(Tabla tabla, Culoare culoare, int linie, int coloana)
        {
            this.Linie = linie;
            this.Coloana = coloana;
            this.CuloarePiesa = culoare;
            if (culoare == Culoare.ALBASTRU)
            {
                this.Imagine = Properties.Resources.bking;
                this.Cod = CodPiesa.RegeAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.wking;
                this.Cod = CodPiesa.RegeAlb;
            }
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
        }
    }
}
