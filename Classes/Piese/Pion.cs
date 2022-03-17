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

        public override void ArataMutariPosibile(Tabla tabla)
        {
            List<Pozitie> pozitii = new List<Pozitie>();
            if(CuloarePiesa == Culoare.Albastru)
            {
                if (this.Pozitie.Coloana < tabla.PragRau)
                {
                    pozitii.Add(new Pozitie(this.Pozitie.Linie+1, this.Pozitie.Coloana ));
                }
                else if(this.Pozitie.Coloana >= tabla.PragRau)
                {
                    int sfarsitColoana = tabla.MarimeTablaVerticala - 1;
                    int sfarsitLinie = tabla.MarimeTablaOrizontala - 1;
                    const int inceputLinie = 0;

                    if (this.Pozitie.Coloana != sfarsitColoana)
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie + 1, this.Pozitie.Coloana ));
                    }
                    if (this.Pozitie.Linie != inceputLinie)
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                    }
                    if (this.Pozitie.Linie != sfarsitLinie)
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana +1));
                    }
                }
            }
            else
            {
                if (this.Pozitie.Coloana >= tabla.PragRau)
                {
                    pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                }
                else if (this.Pozitie.Coloana >= tabla.PragRau)
                {
                    int sfarsitLinie = tabla.MarimeTablaOrizontala - 1;
                    const int inceputLinie = 0;
                    const int inceputColoana = 0;

                    if (this.Pozitie.Coloana != inceputColoana)
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                    }
                    if (this.Pozitie.Linie != inceputLinie)
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie - 1, this.Pozitie.Coloana));
                    }
                    if (this.Pozitie.Linie != sfarsitLinie)
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie + 1, this.Pozitie.Coloana));
                    }
                }

            }
            tabla.ColoreazaMutariPosibile(pozitii);
        }
    }
}
