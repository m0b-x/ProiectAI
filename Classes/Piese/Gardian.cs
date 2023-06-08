using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Gardian : Piesa
    {
        int _paritatePiesa;
        public Gardian(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
            this.Culoare = culoare;

            this.Selectata = false;

            if (aspect == Aspect.Normal)
            {
                if (culoare == Culoare.AlbastruMax)
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
            else
            {
                if (culoare != Culoare.AlbastruMax)
                {
                    this.Imagine = Properties.Resources.benvoy;
                    this.Cod = CodPiesa.GardianAlb;
                }
                else
                {
                    this.Imagine = Properties.Resources.wenvoy;
                    this.Cod = CodPiesa.GardianAlbastru;
                }
            }
            _paritatePiesa = (int)this.Cod % 2;
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaPozitiiPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }




        public static Dictionary<Pozitie, List<Pozitie>> dictionarMutariGardianAlb = new()
        {
            {
                Pozitie.AcceseazaElementStatic(7,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(8, 4)
                }
            },
            {
                Pozitie.AcceseazaElementStatic(7,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(8, 4)
                }
            },
            {
                Pozitie.AcceseazaElementStatic(8,4),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(9, 3),
                    Pozitie.AcceseazaElementStatic(9, 5),
                    Pozitie.AcceseazaElementStatic(7, 3),
                    Pozitie.AcceseazaElementStatic(7, 5),
                }
            },
            {
                Pozitie.AcceseazaElementStatic(9,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(8, 4),
                }
            },
            {
                Pozitie.AcceseazaElementStatic(9,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(8, 4),
                }
            },
        };

        public static Dictionary<Pozitie, List<Pozitie>> dictionarMutariGardianAlbastru = new()
        {
            {
                Pozitie.AcceseazaElementStatic(2,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(1, 4)
                }
            },
            {
                Pozitie.AcceseazaElementStatic(2,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(1, 4)
                }
            },
            {
                Pozitie.AcceseazaElementStatic(1,4),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(0, 3),
                    Pozitie.AcceseazaElementStatic(0, 5),
                    Pozitie.AcceseazaElementStatic(2, 3),
                    Pozitie.AcceseazaElementStatic(2, 5),
                }
            },
            {
                Pozitie.AcceseazaElementStatic(0,3),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(1, 4),
                }
            },
            {
                Pozitie.AcceseazaElementStatic(0,5),
                new List<Pozitie>()
                {
                    Pozitie.AcceseazaElementStatic(1, 4),
                }
            },
        };


        public override List<Pozitie> ReturneazaPozitiiPosibile(int[][] matrice)
        {

            if (Culoare == Culoare.AlbMin)
            {
                List<Pozitie> pozitii = dictionarMutariGardianAlb[Pozitie];
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
                List<Pozitie> pozitii = dictionarMutariGardianAlbastru[Pozitie];
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