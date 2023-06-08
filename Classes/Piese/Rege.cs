using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Rege : Piesa
    {
        int _paritatePiesa;
        public Rege(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
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
            _paritatePiesa = (int)this.Cod % 2;
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaPozitiiPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }




        public static Dictionary<Pozitie, List<Pozitie>> dictionarMutariRegeAlbastru = new()
        {
            {
                Pozitie.AcceseazaElementStatic(0,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(0, 4),
                    Pozitie.AcceseazaElementStatic(1, 3),
                }
            },

            {
                Pozitie.AcceseazaElementStatic(0,4),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(0, 3),
                    Pozitie.AcceseazaElementStatic(0, 5),
                    Pozitie.AcceseazaElementStatic(1, 4),
                }
            },

            {
                Pozitie.AcceseazaElementStatic(0,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(0, 4),
                    Pozitie.AcceseazaElementStatic(1, 5)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(1,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(0, 3),
                    Pozitie.AcceseazaElementStatic(2, 3),
                    Pozitie.AcceseazaElementStatic(1, 4),
                }
            },

            {
                Pozitie.AcceseazaElementStatic(1,4),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(1, 5),
                    Pozitie.AcceseazaElementStatic(1, 3),
                    Pozitie.AcceseazaElementStatic(0, 4),
                    Pozitie.AcceseazaElementStatic(2, 4)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(1,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(1, 4),
                    Pozitie.AcceseazaElementStatic(0, 5),
                    Pozitie.AcceseazaElementStatic(2, 5)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(2,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(2, 4),
                    Pozitie.AcceseazaElementStatic(1, 3)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(2,4),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(2, 3),
                    Pozitie.AcceseazaElementStatic(2, 5),
                    Pozitie.AcceseazaElementStatic(1, 4)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(2,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(2, 4),
                    Pozitie.AcceseazaElementStatic(1, 5)
                }
            }
        };




        public static Dictionary<Pozitie, List<Pozitie>> dictionarMutariRegeAlb = new()
        {
            {
                Pozitie.AcceseazaElementStatic(9,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(9, 4),
                    Pozitie.AcceseazaElementStatic(8, 3),
                }
            },

            {
                Pozitie.AcceseazaElementStatic(9,4),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(9, 3),
                    Pozitie.AcceseazaElementStatic(9, 5),
                    Pozitie.AcceseazaElementStatic(8, 4),
                }
            },

            {
                Pozitie.AcceseazaElementStatic(9,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(9, 4),
                    Pozitie.AcceseazaElementStatic(8, 5)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(8,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(9, 3),
                    Pozitie.AcceseazaElementStatic(7, 3),
                    Pozitie.AcceseazaElementStatic(8, 4),
                }
            },

            {
                Pozitie.AcceseazaElementStatic(8,4),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(8, 5),
                    Pozitie.AcceseazaElementStatic(8, 3),
                    Pozitie.AcceseazaElementStatic(9, 4),
                    Pozitie.AcceseazaElementStatic(7, 4)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(8,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(7, 4),
                    Pozitie.AcceseazaElementStatic(9, 5),
                    Pozitie.AcceseazaElementStatic(8, 5)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(7,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(7, 4),
                    Pozitie.AcceseazaElementStatic(8, 3)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(7,4),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(7, 3),
                    Pozitie.AcceseazaElementStatic(7, 5),
                    Pozitie.AcceseazaElementStatic(8, 4)
                }
            },

            {
                Pozitie.AcceseazaElementStatic(7,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(7, 4),
                    Pozitie.AcceseazaElementStatic(8, 5)
                }
            }
        };

        public override List<Pozitie> ReturneazaPozitiiPosibile(int[][] matrice)
        {

            if (Culoare == Culoare.AlbMin)
            {
                List<Pozitie> pozitii = dictionarMutariRegeAlb[Pozitie];
                List<Pozitie> pozitiiReturnate = new(4);

                for (int i = pozitii.Count - 1; i >= 0; i--)
                {
                    if (matrice[pozitii[i].Linie][pozitii[i].Coloana] == 0 ||
                    (
                    matrice[pozitii[i].Linie][pozitii[i].Coloana] != 0 &&
                    matrice[pozitii[i].Linie][pozitii[i].Coloana] % 2 != _paritatePiesa)
                    )
                    {
                        pozitiiReturnate.Add(pozitii[i]);
                    }
                }
                return pozitiiReturnate;
            }
            else
            {
                List<Pozitie> pozitii = dictionarMutariRegeAlbastru[Pozitie];
                List<Pozitie> pozitiiReturnate = new(4);

                for (int i = pozitii.Count - 1; i >= 0; i--)
                {
                    if (matrice[pozitii[i].Linie][pozitii[i].Coloana] == 0 ||
                    (
                    matrice[pozitii[i].Linie][pozitii[i].Coloana] != 0 &&
                    matrice[pozitii[i].Linie][pozitii[i].Coloana] % 2 != _paritatePiesa)
                    )
                    {
                        pozitiiReturnate.Add(pozitii[i]);
                    }
                }
                return pozitiiReturnate;
            }
        
        }
    }
}