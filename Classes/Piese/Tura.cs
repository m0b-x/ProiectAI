using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Tura : Piesa
    {
        public Tura(Culoare culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == Culoare.Albastru)
            {
                this.Imagine = Properties.Resources.brook;
                this.Cod = CodPiesa.TuraAlbastra;
            }
            else
            {
                this.Imagine = Properties.Resources.wrook;
                this.Cod = CodPiesa.TuraAlba;
            }
        }
    }
}
