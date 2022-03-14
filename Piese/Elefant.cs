using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Elefant : Piesa
    {
        public Elefant(Tabla tabla, Culoare culoare, int linie, int coloana)
        {
            this.Linie = linie;
            this.Coloana = coloana;
            this.CuloarePiesa = culoare;
            if (culoare == Culoare.ALBASTRU)
            {
                this.Imagine = Properties.Resources.belephantrev2;
                this.Cod = CodPiesa.ElefantAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.welephantrev2;
                this.Cod = CodPiesa.ElefantAlb;
            }
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
            tabla.MatriceTabla[linie, coloana] = (int)this.Cod;
        }
    }
}
