namespace ProiectVolovici
{
    using System.Collections.Generic;

    internal class Tura : Piesa
    {
        public Tura(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
            this.ValoarePiesa = ConstantaPiese.ValoareTura;
            this.Culoare = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;

            if (aspect == Aspect.Normal)
            {
                if (culoare == Culoare.Albastru)
                {
                    this.Imagine = Properties.Resources.brook;
                    this.Cod = CodPiesa.TuraAlbastra;
                }
                else
                {
                    this.Imagine = Properties.Resources.wrook;
                    this.Cod = CodPiesa.TuraAlba;
                }
            }
            else
            {
                if (culoare != Culoare.Albastru)
                {
                    this.Imagine = Properties.Resources.brook;
                    this.Cod = CodPiesa.TuraAlba;
                }
                else
                {
                    this.Imagine = Properties.Resources.wrook;
                    this.Cod = CodPiesa.TuraAlbastra;
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
            List<Pozitie> mutariLegale = new List<Pozitie>();

            int liniePoz = this.Pozitie.Linie + 1;
            while (liniePoz < ConstantaTabla.NrLinii)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePoz, coloana: this.Pozitie.Coloana);
                if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                    {
                        mutariLegale.Add(pozitiePosibila);
                    }
                    break;
                }
                liniePoz++;
            }

            liniePoz = this.Pozitie.Linie - 1;
            while (liniePoz >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePoz, coloana: this.Pozitie.Coloana);
                if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                    {
                        mutariLegale.Add(pozitiePosibila);
                    }
                    break;
                }
                liniePoz--;
            }

            int colonaPozitiePosibila = this.Pozitie.Coloana + 1;
            while (colonaPozitiePosibila < ConstantaTabla.NrColoane)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                    {
                        mutariLegale.Add(pozitiePosibila);
                    }
                    break;
                }
                colonaPozitiePosibila++;
            }

            colonaPozitiePosibila = this.Pozitie.Coloana - 1;
            while (colonaPozitiePosibila >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                    {
                        mutariLegale.Add(pozitiePosibila);
                    }
                    break;
                }
                colonaPozitiePosibila--;
            }
            return mutariLegale;
        }
    }
}