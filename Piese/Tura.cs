using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Tura : Piesa
    {
        public Tura(Tabla tabla, Culoare culoare, int linie, int coloana)
        {
            this.Linie = linie;
            this.Coloana = coloana;
            this.CuloarePiesa = culoare;
            if (culoare == Culoare.ALBASTRU)
            {
                this.Imagine = Properties.Resources.brook;
                this.Cod = CodPiesa.TuraAlbastra;
            }
            else
            {
                this.Imagine = Properties.Resources.wrook;
                this.Cod = CodPiesa.TuraAlba;
            }
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
            tabla.MatriceTabla[linie, coloana] = (int)this.Cod;
        }
    }
}
