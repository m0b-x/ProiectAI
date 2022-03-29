using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Cal : Piesa
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
            const int primaLinie = 0;
            const int primaColoana = 0;
            int ultimaLinie = ConstantaTabla.MarimeOrizontala - 1;
            int ultimaColoana = ConstantaTabla.MarimeVerticala - 1;

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
                    else if (joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran.CuloarePiesa != CuloareJoc.Albastru)
                    {
                        mutariFiltruTabla.Add(pozitie);
                    }
                }
            }

            if(mutariFiltruTabla.Contains(dreaptaSusVertical))
            {
                if (joc.ArrayCadrane[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(dreaptaSusVertical);
                }
                else if (joc.ArrayCadrane[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana].PiesaCadran.CuloarePiesa != _culoarePiesa)
                {
                    mutariFiltruFinal.Add(dreaptaSusVertical);
                }
            }
            if (mutariFiltruTabla.Contains(dreaptaJosVertical))
            {
                if (joc.ArrayCadrane[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(dreaptaJosVertical);
                }
                else if (joc.ArrayCadrane[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana].PiesaCadran.CuloarePiesa != _culoarePiesa)
                {
                    mutariFiltruFinal.Add(dreaptaJosVertical);
                }
            }
            if (mutariFiltruTabla.Contains(stangaSusVertical))
            {
                if (joc.ArrayCadrane[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(stangaSusVertical);
                }
                else if (joc.ArrayCadrane[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana].PiesaCadran.CuloarePiesa != _culoarePiesa)
                {
                    mutariFiltruFinal.Add(stangaSusVertical);
                }
            }
            if (mutariFiltruTabla.Contains(stangaJosVertical))
            {
                if (joc.ArrayCadrane[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(stangaJosVertical);
                }
                else if (joc.ArrayCadrane[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana].PiesaCadran.CuloarePiesa != _culoarePiesa)
                {
                    mutariFiltruFinal.Add(stangaJosVertical);
                }
            }

            if (mutariFiltruTabla.Contains(dreaptaSusOrizontal))
            {
                if (joc.ArrayCadrane[_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(dreaptaSusOrizontal);
                }
                else if (joc.ArrayCadrane[_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1].PiesaCadran.CuloarePiesa != _culoarePiesa)
                {
                    mutariFiltruFinal.Add(dreaptaSusOrizontal);
                }
            }
            if (mutariFiltruTabla.Contains(dreaptaJosOrizontal))
            {
                if (joc.ArrayCadrane[_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(dreaptaJosOrizontal);
                }
                else if (joc.ArrayCadrane[_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1].PiesaCadran.CuloarePiesa != _culoarePiesa)
                {
                    mutariFiltruFinal.Add(dreaptaJosOrizontal);
                }
            }
            if (mutariFiltruTabla.Contains(stangaSusOrizontal))
            {
                if (joc.ArrayCadrane[_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(stangaSusOrizontal);
                }
                else if (joc.ArrayCadrane[_pozitiePiesa.Linie,_pozitiePiesa.Coloana - 1].PiesaCadran.CuloarePiesa != _culoarePiesa)
                {
                    mutariFiltruFinal.Add(stangaSusOrizontal);
                }
            }
            if (mutariFiltruTabla.Contains(stangaJosOrizontal))
            {
                if (joc.ArrayCadrane[_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(stangaJosOrizontal);
                }
                else if (joc.ArrayCadrane[_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1].PiesaCadran.CuloarePiesa != _culoarePiesa)
                {
                    mutariFiltruFinal.Add(stangaJosOrizontal);
                }
            }

            joc.ColoreazaMutariPosibile(pozitii: mutariFiltruFinal);
        }
    }
}
