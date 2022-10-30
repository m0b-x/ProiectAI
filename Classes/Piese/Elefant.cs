﻿using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Elefant : Piesa
    {
        public Elefant(CuloareJoc culoare)
        {
            this.ValoarePiesa = ConstantaPiese.ValoareElefant;
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
            {
                this.Imagine = Properties.Resources.belephantrev2;
                this.Cod = CodPiesa.ElefantAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.welephantrev2;
                this.Cod = CodPiesa.ElefantAlb;
            }
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(int[,] matrice)
        {
            const int primaLinie = 0;
            const int primaColoana = 0;

            int ultimaLinie = ConstantaTabla.MarimeVerticala - 1;
            int ultimaColoana = ConstantaTabla.MarimeOrizontala - 1;

            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariFiltruTabla = new List<Pozitie>();
            List<Pozitie> mutariFiltruFinal = new List<Pozitie>();

            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 2));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 2));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 2));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 2));

            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (pozitie.Linie <= ultimaLinie &&
                     pozitie.Linie >= primaLinie &&
                     pozitie.Coloana <= ultimaColoana &&
                     pozitie.Coloana >= primaColoana)
                {
                if (pozitie.Linie > ConstantaTabla.PragRau && this.CuloarePiesa == CuloareJoc.Alb
                        || pozitie.Linie <= ConstantaTabla.PragRau && this.CuloarePiesa == CuloareJoc.Albastru)
                {
                    if (matrice[pozitie.Linie, pozitie.Coloana] == (int)CodPiesa.Gol)
                    {
                        mutariFiltruTabla.Add(pozitie);
                    }
                    else if (matrice[pozitie.Linie, pozitie.Coloana] %2 != (int)Cod%2)
                    {
                        mutariFiltruTabla.Add(pozitie);
                    }
                }
                }
            }
            foreach (Pozitie pozitie in mutariFiltruTabla)
            {
                Pozitie pozitieElementMijloc = new Pozitie(0, 0);
                if (pozitie.Linie > _pozitiePiesa.Linie)
                {
                    pozitieElementMijloc.Linie = pozitie.Linie - 1;
                }
                else
                {
                    pozitieElementMijloc.Linie = _pozitiePiesa.Linie - 1;
                }
                if (pozitie.Coloana > _pozitiePiesa.Coloana)
                {
                    pozitieElementMijloc.Coloana = pozitie.Coloana - 1;
                }
                else
                {
                    pozitieElementMijloc.Coloana = _pozitiePiesa.Coloana - 1;
                }
                if (matrice[pozitieElementMijloc.Linie, pozitieElementMijloc.Coloana] == (int) CodPiesa.Gol)
                {
                    mutariFiltruFinal.Add(pozitie);
                }
                else if(matrice[pozitie.Linie, pozitie.Coloana] % 2 != (int)Cod % 2)
                {
                    mutariFiltruFinal.Add(pozitie);
                }
            }

            return mutariFiltruFinal;
        }
    }
}