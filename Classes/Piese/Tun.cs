using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Tun : Piesa
    {
        public Tun(Culoare culoare,Aspect aspect = Aspect.Normal)
        {
            this.ValoarePiesa = ConstantaPiese.ValoareTun;
            this.Culoare = culoare;
            
            this.Selectata = false;
            if (aspect == Aspect.Normal)
            {
                if (culoare == Culoare.Albastru)
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
            else
            {
                if (culoare != Culoare.Albastru)
                {
                    this.Imagine = Properties.Resources.bcannon;
                    this.Cod = CodPiesa.TunAlb;
                }
                else
                {
                    this.Imagine = Properties.Resources.wcannon;
                    this.Cod = CodPiesa.TunAlbastru;
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

            int liniePozitiiPosibile = this.Pozitie.Linie + 1;
            while (liniePozitiiPosibile < ConstantaTabla.NrLinii)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    liniePozitiiPosibile++;
                    {
                        while (liniePozitiiPosibile < ConstantaTabla.NrLinii)
                        {
                            pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                            if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] != (int)CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto functie_linie;
                            }
                            else

                            if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] != (int)CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                            {
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
                if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] == (int)CodPiesa.Gol)
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
                            if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] != (int)CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto functie_coloane;
                            }
                            else

                            if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] != (int)CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 == (int)this.Cod % 2)
                            {
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
            while (colonaPozitiePosibila < ConstantaTabla.NrColoane)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariLegale.Add(pozitiePosibila);
                }
                else
                {
                    colonaPozitiePosibila++;
                    {
                        while (colonaPozitiePosibila < ConstantaTabla.NrColoane)
                        {
                            pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                            if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] != (int)CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto functie_coloane2;
                            }
                            else

                            if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] != (int)CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 == (int)this.Cod % 2)
                            {
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
                if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] == (int)CodPiesa.Gol)
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
                            if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] != (int)CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 != (int)this.Cod % 2)
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto sfarsit_functie;
                            }
                            else

                            if (matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] != (int)CodPiesa.Gol &&
                                matrice[pozitiePosibila.Linie][pozitiePosibila.Coloana] % 2 == (int)this.Cod % 2)
                            {
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