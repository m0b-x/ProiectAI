using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Rege : Piesa
    {
        public Rege(CuloareJoc culoare)
        {
            this.ValoarePiesa = ConstantaPiese.ValoareRege;
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
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(EngineJoc joc)
        {
            int sfarsitLinie = joc.MarimeVerticala - 1;
            int inceputLinie = 0;

            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariFiltrate = new List<Pozitie>();

            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));

            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (joc.TablaJoc.PozitiiPalat.Contains(pozitie))
                {
                    if (joc.MatriceCoduriPiese[pozitie.Linie, pozitie.Coloana] == (int)CodPiesa.Gol)
                    {
                        {
                            if (this.CuloarePiesa == CuloareJoc.Alb)
                            {
                                bool estePosibila = true;
                                for (int linie = pozitie.Linie; linie >= inceputLinie; linie--)
                                {
                                    if (joc.ArrayCadrane[linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                                    {
                                        if (joc.ArrayCadrane[linie, pozitie.Coloana].PiesaCadran.GetType() == new Rege(CuloareJoc.Albastru).GetType())
                                        {
                                            estePosibila = false;
                                            break;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (estePosibila == true)
                                {
                                    mutariFiltrate.Add(pozitie);
                                }
                            }
                            else if (this.CuloarePiesa == CuloareJoc.Albastru)
                            {
                                bool estePosibila = true;
                                for (int linie = pozitie.Linie; linie <= sfarsitLinie; linie++)
                                {
                                    if (joc.ArrayCadrane[linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                                    {
                                        if (joc.ArrayCadrane[linie, pozitie.Coloana].PiesaCadran.GetType() == new Rege(CuloareJoc.Alb).GetType())
                                        {
                                            estePosibila = false;
                                            break;
                                        }
                                        else
                                        {
                                            break;
                                        }
                                    }
                                }
                                if (estePosibila == true)
                                {
                                    mutariFiltrate.Add(pozitie);
                                }
                            }
                        }
                    }
                }
            }
            return mutariFiltrate;
        }
    }
}