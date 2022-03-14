using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Cal : Piesa
    {
        public Cal(Tabla tabla, Culoare culoare, int linie, int coloana)
        {
            this.Linie = linie;
            this.Coloana = coloana;
            this.CuloarePiesa = culoare;
            if (culoare == Culoare.ALBASTRU)
            {
                this.Imagine = Properties.Resources.bhorse;
                this.Cod = CodPiesa.CalAbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.whorse;
                this.Cod = CodPiesa.CalAlb;
            }
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
        }
    }
}
