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
                this.Cod = CodPiesa.TunAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.wcannon;
                this.Cod = CodPiesa.TunAlb;
            }
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
            tabla.MatriceTabla[linie, coloana] = (int)this.Cod;
        }
    }
}
