using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Gardian : Piesa
    {
        public Gardian(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
            this.ValoarePiesa = ConstantaPiese.ValoareGardian;
            this.Culoare = culoare;
            
            this.Selectata = false;

            if (aspect == Aspect.Normal)
            {
                if (culoare == Culoare.AlbastruMax)
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
            else
            {
                if (culoare != Culoare.AlbastruMax)
                {
                    this.Imagine = Properties.Resources.benvoy;
                    this.Cod = CodPiesa.GardianAlb;
                }
                else
                {
                    this.Imagine = Properties.Resources.wenvoy;
                    this.Cod = CodPiesa.GardianAlbastru;
                }
            }
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(int[][] matrice)
        {
            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariFiltrate = new List<Pozitie>();

            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 1));

            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (ConstantaTabla.PozitiiPalat.Contains(pozitie))
                {
                    if (matrice[pozitie.Linie][pozitie.Coloana] == (int)CodPiesa.Gol)
                    {
                        mutariFiltrate.Add(pozitie);
                    }
                    else if (matrice[pozitie.Linie][pozitie.Coloana] % 2 != (int)this._codPiesa % 2)
                    {
                        mutariFiltrate.Add(pozitie);
                    }
                }
            }

            return mutariFiltrate;
        }
    }
}