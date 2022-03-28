using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Gardian : Piesa
    {
        public Gardian(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
            {
                this.Imagine = Properties.Resources.benvoy;
                this.Cod = CodPiesa.GardianAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.wenvoy;
                this.Cod = CodPiesa.GardianAlb;
            }
        }
        public override void ArataMutariPosibile(EngineJoc joc) 
        {
            List<Pozitie> mutariGardian = new List<Pozitie>();
            List<Pozitie> pozitii = new List<Pozitie>();

            mutariGardian.Add(new Pozitie(_pozitiePiesa.Linie + 1,_pozitiePiesa.Coloana + 1));
            mutariGardian.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 1));
            mutariGardian.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 1));
            mutariGardian.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 1));

            foreach (Pozitie pozitie in mutariGardian)
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
