using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Rege : Piesa
    {
        public Rege(CuloareJoc culoare)
        {
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
            int sfarsitLinie = joc.MarimeVerticala - 1;
            int inceputLinie = 0;
            List<Pozitie> mutariRege = new List<Pozitie>();
            List<Pozitie> pozitii = new List<Pozitie>();

            mutariRege.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana));
            mutariRege.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana));
            mutariRege.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
            mutariRege.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));

            foreach (Pozitie pozitie in mutariRege)
            {
                if (joc.TablaJoc.PozitiiPalat.Contains(pozitie))
                {
                    if (joc.MatriceCodPiese[pozitie.Linie, pozitie.Coloana] == (int)CodPiesa.Gol)
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
                                    pozitii.Add(pozitie);
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
                                    pozitii.Add(pozitie);
                                }
                            }
                        }
                    }
                }

            }
            joc.ColoreazaMutariPosibile(pozitii: pozitii);
        }
    }
}
