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
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(int[,] matrice)
        {
            int sfarsitLinie = ConstantaTabla.MarimeVerticala - 1;

            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariPrimaFiltrare = new List<Pozitie>();
            List<Pozitie> mutariADouaFiltrare = new List<Pozitie>();

            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));
            const int primaLinie = 0;
            const int primaColoana = 0;

            int ultimaLinie = ConstantaTabla.MarimeVerticala - 1;
            int ultimaColoana = ConstantaTabla.MarimeOrizontala - 1;

            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (pozitie.Linie <= ultimaLinie &&
                     pozitie.Linie >= primaLinie &&
                     pozitie.Coloana <= ultimaColoana &&
                     pozitie.Coloana >= primaColoana &&
                     ConstantaTabla.PozitiiPalat.Contains(pozitie))
                {
                    if (matrice[pozitie.Linie, pozitie.Coloana] == (int)CodPiesa.Gol)
                    {
                        mutariPrimaFiltrare.Add(pozitie);
                    }
                    else if (matrice[pozitie.Linie, pozitie.Coloana] % 2 != (int)this.Cod % 2)
                    {
                        mutariPrimaFiltrare.Add(pozitie);
                    }
                }
            }
            foreach (Pozitie pozitie in mutariPrimaFiltrare)
            {
                if(this.CuloarePiesa == CuloareJoc.Alb)
                {
                    for(int linie = this.Pozitie.Linie-1;linie>= 0;linie--)
                    {
                        if (matrice[linie,this.Pozitie.Coloana] != (int)CodPiesa.Gol)
                        {
                            if(matrice[linie,this.Pozitie.Coloana] != (int)CodPiesa.RegeAlbastru)
                            {
                                mutariADouaFiltrare.Add(pozitie);
                            }
                            break;
                        }
                    }
                }
                else
                {
                    for (int linie = this.Pozitie.Linie +1; linie <= 9; linie++)
                    {
                        if (matrice[linie, this.Pozitie.Coloana] != (int)CodPiesa.Gol)
                        {
                            if (matrice[linie, this.Pozitie.Coloana] != (int)CodPiesa.RegeAlb)
                            {
                                mutariADouaFiltrare.Add(pozitie);
                            }
                            break;
                        }
                    }
                }
            }
            return mutariADouaFiltrare;
        }
    }
}