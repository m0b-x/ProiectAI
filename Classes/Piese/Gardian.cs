using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Gardian : Piesa
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

            mutariGardian.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 1));
            mutariGardian.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 1));
            mutariGardian.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 1));
            mutariGardian.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 1));

            foreach (Pozitie pozitie in mutariGardian)
            {
                if (joc.TablaJoc.PozitiiPalat.Contains(pozitie))
                {
                    if (joc.MatriceCodPiese[pozitie.Linie, pozitie.Coloana] == (int)CodPiesa.Gol ||
                        joc.MatriceCodPiese[pozitie.Linie, pozitie.Coloana] % 2 != (int)this._codPiesa % 2)
                    {
                        pozitii.Add(pozitie);
                    }
                }
                joc.ColoreazaMutariPosibile(pozitii: pozitii);
            }
        }
    }
}