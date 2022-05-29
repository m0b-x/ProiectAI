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
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariLegale = new List<Pozitie>();

            int liniePozitiiPosibile = this.Pozitie.Linie + 1;
            while (liniePozitiiPosibile < ConstantaTabla.MarimeVerticala)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
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
                            if (!joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol() &&
                                joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto functie_coloane;
                            }
                            liniePozitiiPosibile++;
                        }
                    }
                }
                liniePozitiiPosibile++;
            }
            liniePozitiiPosibile = this.Pozitie.Linie - 1;
            while (liniePozitiiPosibile >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
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
                            if (!joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol() &&
                                joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
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
                if (joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
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
                            if (!joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol() &&
                                joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                            {
                                mutariLegale.Add(pozitiePosibila);
                                goto sfarsit_functie;
                            }
                            colonaPozitiePosibila++;
                        }
                    }
                }
                colonaPozitiePosibila++;
            }

            colonaPozitiePosibila = this.Pozitie.Coloana - 1;
            while (colonaPozitiePosibila >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
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
                            if (!joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol() &&
                                joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
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