using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Pion : Piesa
    {
        
        public Pion(Culoare culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == Culoare.Albastru)
            {
                this.Imagine = Properties.Resources.bpawn;
                this.Cod = CodPiesa.PionAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.wpawn;
                this.Cod = CodPiesa.PionAlb;
            }
        }

        public void ArataMutarilePosibile(Tabla tabla)
        {
            List<Pozitie> pozitii = new List<Pozitie>();
            if(CuloarePiesa == Culoare.Albastru)
            {
                if (this.Pozitie.Coloana < tabla.PragRau)
                {
                    if(tabla.MatriceTabla[this.Pozitie.Linie,this.Pozitie.Coloana+1] != (int)CodPiesa.Gol )
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana + 1));
                        tabla.ArataMutarilePosibile(pozitii);

                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }
    }
}
