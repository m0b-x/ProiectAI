using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Tun : Piesa
    {
        public Tun(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
            {
                this.Imagine = Properties.Resources.bcannon;
                this.Cod = CodPiesa.TunAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.wcannon;
                this.Cod = CodPiesa.TunAlb;
            }
        }
        public override void ArataMutariPosibile(EngineJoc tabla) { Console.WriteLine("Fa corpul metodei"); }
    }
}
