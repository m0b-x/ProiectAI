using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Elefant : Piesa
    {
        public Elefant(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
            {
                this.Imagine = Properties.Resources.belephantrev2;
                this.Cod = CodPiesa.ElefantAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.welephantrev2;
                this.Cod = CodPiesa.ElefantAlb;
            }
        }
        public override void ArataMutariPosibile(EngineJoc tabla) { Console.WriteLine("Fa corpul metodei"); }
    }
}
