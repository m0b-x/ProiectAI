using System.Collections.Generic;
using System.Diagnostics;

namespace ProiectVolovici
{
    internal class Cal : Piesa
    {
        public Cal(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
            {
                this.Imagine = Properties.Resources.bhorse;
                this.Cod = CodPiesa.CalAbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.whorse;
                this.Cod = CodPiesa.CalAlb;
            }
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc);

            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(EngineJoc joc)
        {
            const int primaLinie = 0;
            const int primaColoana = 0;
            
            int ultimaLinie = ConstantaTabla.MarimeVerticala - 1;
            int ultimaColoana = ConstantaTabla.MarimeOrizontala - 1;

            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariFiltruTabla = new List<Pozitie>();
            List<Pozitie> mutariFiltruFinal = new List<Pozitie>();

            Pozitie dreaptaSusVertical = new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 1);
            Pozitie dreaptaJosVertical = new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 1);
            Pozitie stangaSusVertical = new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 1);
            Pozitie stangaJosVertical = new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 1);

            Pozitie dreaptaSusOrizontal = new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 2);
            Pozitie dreaptaJosOrizontal = new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 2);
            Pozitie stangaSusOrizontal = new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 2);
            Pozitie stangaJosOrizontal = new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 2);

            mutariNefiltrate.Add(dreaptaSusVertical);
            mutariNefiltrate.Add(dreaptaJosVertical);
            mutariNefiltrate.Add(stangaSusVertical);
            mutariNefiltrate.Add(stangaJosVertical);

            mutariNefiltrate.Add(dreaptaSusOrizontal);
            mutariNefiltrate.Add(dreaptaJosOrizontal);
            mutariNefiltrate.Add(stangaSusOrizontal);
            mutariNefiltrate.Add(stangaJosOrizontal);

            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (pozitie.Linie <= ultimaLinie &&
                     pozitie.Linie >= primaLinie &&
                     pozitie.Coloana <= ultimaColoana &&
                     pozitie.Coloana >= primaColoana)
                {
                    if (joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                    {
                        mutariFiltruTabla.Add(pozitie);
                    }
                    else if (joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran.CuloarePiesa != this.CuloarePiesa)
                    {
                        mutariFiltruTabla.Add(pozitie);
                    }
                }
            }

            if (mutariFiltruTabla.Contains(dreaptaSusVertical))
            {
                if (joc.MatriceCodPiese[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(dreaptaSusVertical);
                }
            }
            if (mutariFiltruTabla.Contains(stangaSusVertical))
            {
                if (joc.MatriceCodPiese[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(stangaSusVertical);
                }
            }
            if (mutariFiltruTabla.Contains(dreaptaJosVertical))
            {
                if (joc.MatriceCodPiese[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(dreaptaJosVertical);
                }
            }
            if (mutariFiltruTabla.Contains(stangaJosVertical))
            {
                if (joc.MatriceCodPiese[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana] == (int)CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(stangaJosVertical);
                }
            }

            if (mutariFiltruTabla.Contains(dreaptaSusOrizontal))
            {
                if (joc.MatriceCodPiese[_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1] == (int)CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(dreaptaSusOrizontal);
                }
            }
            if (mutariFiltruTabla.Contains(dreaptaJosOrizontal))
            {
                if (joc.MatriceCodPiese[_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1] == (int)CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(dreaptaJosOrizontal);
                }
            }
            if (mutariFiltruTabla.Contains(stangaSusOrizontal))
            {
                if (joc.MatriceCodPiese[_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1] == (int)CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(stangaSusOrizontal);
                }
            }
            if (mutariFiltruTabla.Contains(stangaJosOrizontal))
            {
                if (joc.MatriceCodPiese[_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1] == (int)CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(stangaJosOrizontal);
                }
            }

            return mutariFiltruFinal;
        }
    }
}