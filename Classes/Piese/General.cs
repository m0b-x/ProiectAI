using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class General : Piesa
    {
        public General(CuloareJoc culoare)
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
            int sfarsitLinie = joc.MarimeVerticala - 1;
            int inceputLinie = 0;
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
                        joc.MatriceCodPiese[pozitie.Linie, pozitie.Coloana] %2 != (int) this._codPiesa % 2)
                    {
                        if (this.CuloarePiesa == CuloareJoc.Alb)
                        {
                            bool estePosibila = true;
                            for (int linie = pozitie.Linie; linie >= inceputLinie; linie--)
                            {
                                Debug.WriteLine("linie General:" + linie);
                                if (joc.ArrayCadrane[linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                                {
                                    if (joc.ArrayCadrane[linie, pozitie.Coloana].PiesaCadran.GetType() == new General(CuloareJoc.Albastru).GetType())
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
                                Debug.WriteLine("linie General:" + linie);
                                if (joc.ArrayCadrane[linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                                {
                                    if (joc.ArrayCadrane[linie, pozitie.Coloana].PiesaCadran.GetType() == new General(CuloareJoc.Alb).GetType())
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
                joc.ColoreazaMutariPosibile(pozitii: pozitii);
            }

        }
    }
}
