﻿using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Rege : Piesa
    {
        public Rege(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
            this.ValoarePiesa = ConstantaPiese.ValoareRege;
            this.Culoare = culoare;
            
            this.Selectata = false;

            if (aspect == Aspect.Normal)
            {
                if (culoare == Culoare.AlbastruMax)
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
            else
            {

                if (culoare != Culoare.AlbastruMax)
                {
                    this.Imagine = Properties.Resources.bking;
                    this.Cod = CodPiesa.RegeAlb;
                }
                else
                {
                    this.Imagine = Properties.Resources.wking;
                    this.Cod = CodPiesa.RegeAlbastru;
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
            List<Pozitie> mutariNefiltrate = new List<Pozitie>(4);
            List<Pozitie> mutariPrimaFiltrare = new List<Pozitie>(4);
            List<Pozitie> mutariADouaFiltrare = new List<Pozitie>(4);

            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));

            const int primaLinie = 0;
            const int primaColoana = 0;

            int ultimaLinie = ConstantaTabla.NrLinii - 1;
            int ultimaColoana = ConstantaTabla.NrColoane - 1;

            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (pozitie.Linie <= ultimaLinie &&
                     pozitie.Linie >= primaLinie &&
                     pozitie.Coloana <= ultimaColoana &&
                     pozitie.Coloana >= primaColoana &&
                     ConstantaTabla.PozitiiPalat.Contains(pozitie))
                {
                    if (matrice[pozitie.Linie][pozitie.Coloana] == (int)CodPiesa.Gol)
                    {
                        mutariPrimaFiltrare.Add(pozitie);
                    }
                    else if (matrice[pozitie.Linie][pozitie.Coloana] % 2 != (int)this.Cod % 2)
                    {
                        mutariPrimaFiltrare.Add(pozitie);
                    }
                }
            }
            foreach (Pozitie pozitie in mutariPrimaFiltrare)
            {
                bool okMutare = true;
                if (this.Culoare == Culoare.AlbMin)
                {
                    for (int linie = this.Pozitie.Linie - 1; linie >= 0; linie--)
                    {
                        if (matrice[linie][pozitie.Coloana] != (int)CodPiesa.Gol)
                        {
                            if (matrice[linie][pozitie.Coloana] != (int)CodPiesa.RegeAlbastru)
                            {
                                mutariADouaFiltrare.Add(pozitie);
                                break;
                            }
                            else
                            {
                                okMutare = false;
                                break;
                            }
                        }
                    }
                    if (okMutare)
                        mutariADouaFiltrare.Add(pozitie);
                }
                else if (this.Culoare == Culoare.AlbastruMax)
                {
                    okMutare = true;
                    for (int linie = this.Pozitie.Linie + 1; linie <= 9; linie++)
                    {
                        if (matrice[linie][pozitie.Coloana] != (int)CodPiesa.Gol)
                        {
                            if (matrice[linie][pozitie.Coloana] != (int)CodPiesa.RegeAlb)
                            {
                                mutariADouaFiltrare.Add(pozitie);
                                break;
                            }
                            else
                            {
                                okMutare = false;
                                break;
                            }
                        }
                    }
                    if (okMutare)
                        mutariADouaFiltrare.Add(pozitie);
                }
            }
            return mutariADouaFiltrare;
        }
    }
}