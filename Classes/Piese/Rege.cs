using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Rege : Piesa
    {
        public Rege(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
            {
                this.Imagine = Properties.Resources.bking;
                this.Cod = CodPiesa.RegeAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.wking;
                this.Cod = CodPiesa.RegeAlb;
            }
        }
        public override void ArataMutariPosibile(EngineJoc joc) 
        {
            List<Pozitie> mutariRege = new List<Pozitie>();
            List<Pozitie> pozitii = new List<Pozitie>();

            mutariRege.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana));
            mutariRege.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana));
            mutariRege.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
            mutariRege.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));

            foreach (Pozitie pozitie in mutariRege)
            {
                if (joc.TablaJoc.PozitiiPalat.Contains(pozitie))
                {
                    if (joc.MatriceCodPiese[pozitie.Linie, pozitie.Coloana] == (int)CodPiesa.Gol)
                    {
                        pozitii.Add(pozitie);
                    }
                }

            }
            joc.ColoreazaMutariPosibile(pozitii: pozitii);
        }
    }
}
