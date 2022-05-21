using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Gardian : Piesa
    {
        public Gardian(CuloareJoc culoare)
        {
            this.ValoarePiesa = ConstantaPiese.ValoareGardian;
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
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariFiltrate = new List<Pozitie>();

            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 1));

            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (joc.TablaJoc.PozitiiPalat.Contains(pozitie))
                {
                    if (joc.MatriceCoduriPiese[pozitie.Linie, pozitie.Coloana] == (int)CodPiesa.Gol ||
                        joc.MatriceCoduriPiese[pozitie.Linie, pozitie.Coloana] % 2 != (int)this._codPiesa % 2)
                    {
                        mutariFiltrate.Add(pozitie);
                    }
                }
            }

            return mutariFiltrate;
        }
    }
}