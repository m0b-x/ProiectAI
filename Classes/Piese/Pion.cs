using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Pion : Piesa
    {
        public Pion(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
            this.ValoarePiesa = ConstantaPiese.ValoarePion;
            this.Culoare = culoare;
            
            this.Selectata = false;

            if (aspect == Aspect.Normal)
            {
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
            else
            {
                if (culoare != Culoare.Albastru)
                {
                    this.Imagine = Properties.Resources.bpawn;
                    this.Cod = CodPiesa.PionAlb;
                }
                else
                {
                    this.Imagine = Properties.Resources.wpawn;
                    this.Cod = CodPiesa.PionAlbastru;
                }
            }
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(int[][] matrice)
        {
            const int primaLinie = 0;
            const int primaColoana = 0;

            int ultimaLinie = ConstantaTabla.NrLinii - 1;
            int ultimaColoana = ConstantaTabla.NrColoane - 1;

            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariFiltrate = new List<Pozitie>();

            if (this.Culoare == Culoare.Albastru)
            {
                mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana));
                if (_pozitiePiesa.Linie > ConstantaTabla.PragRau)
                {
                    mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
                    mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));
                }
            }
            else
            {
                mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana));
                if (_pozitiePiesa.Linie <= ConstantaTabla.PragRau)
                {
                    mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
                    mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));
                }
            }
            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (pozitie.Linie <= ultimaLinie &&
                     pozitie.Linie >= primaLinie &&
                     pozitie.Coloana <= ultimaColoana &&
                     pozitie.Coloana >= primaColoana)
                {
                    if (matrice[pozitie.Linie][pozitie.Coloana] == (int)CodPiesa.Gol)
                    {
                        mutariFiltrate.Add(pozitie);
                    }
                    else if (matrice[pozitie.Linie][pozitie.Coloana] % 2 != (int)this.Cod % 2)
                    {
                        mutariFiltrate.Add(pozitie);
                    }
                }
            }

            return mutariFiltrate;
        }
    }
}