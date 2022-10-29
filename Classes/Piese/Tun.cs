using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Tun : Piesa
    {
        public Tun(CuloareJoc culoare)
        {
            this.ValoarePiesa = ConstantaPiese.ValoareTun;
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

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(int[,] matrice)
        {
            List<Pozitie> mutariLegale = new List<Pozitie>();

            int liniePozitiiPosibile = this.Pozitie.Linie + 1;
            while (liniePozitiiPosibile < ConstantaTabla.MarimeVerticala)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] == (int) CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    liniePozitiiPosibile++;
                    {
                        while (liniePozitiiPosibile < ConstantaTabla.MarimeVerticala)
                        {
                            pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                            if (matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] != (int) CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] %2 != (int) this.Cod %2)
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto functie_linie;
                            }
                            liniePozitiiPosibile++;
                        }
                    }
                }
                liniePozitiiPosibile++;
            }
            functie_linie:
            liniePozitiiPosibile = this.Pozitie.Linie - 1;
            while (liniePozitiiPosibile >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] == (int) CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    liniePozitiiPosibile--;
                    {
                        while (liniePozitiiPosibile >= 0)
                        {
                            pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                            if (matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] != (int) CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] %2 != (int) this.Cod %2)
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto functie_coloane;
                            }
                            liniePozitiiPosibile--;
                        }
                    }
                }
                liniePozitiiPosibile--;
            }
            functie_coloane:
            int colonaPozitiePosibila = this.Pozitie.Coloana + 1;
            while (colonaPozitiePosibila < ConstantaTabla.MarimeOrizontala)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] == (int) CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    colonaPozitiePosibila++;
                    {
                        while (colonaPozitiePosibila < ConstantaTabla.MarimeOrizontala)
                        {
                            pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                            if (matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] != (int) CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] %2 != (int) this.Cod %2)
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto functie_coloane2;
                            }
                            colonaPozitiePosibila++;
                        }
                    }
                }
                colonaPozitiePosibila++;
            }
            functie_coloane2:
            colonaPozitiePosibila = this.Pozitie.Coloana - 1;
            while (colonaPozitiePosibila >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] == (int) CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    colonaPozitiePosibila--;
                    {
                        while (colonaPozitiePosibila >= 0)
                        {
                            pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                            if (matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] != (int) CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie, pozitiePosibila.Coloana] %2 != (int) this.Cod %2)
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto sfarsit_functie;
                            }
                            colonaPozitiePosibila--;
                        }
                    }
                }
                colonaPozitiePosibila--;
            }
           sfarsit_functie:
            return mutariLegale;
        }
    }
}