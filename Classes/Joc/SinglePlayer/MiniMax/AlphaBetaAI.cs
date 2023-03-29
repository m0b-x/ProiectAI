using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProiectVolovici
{
    public class AlphaBetaAI : AI
    {
        private static TabelTranspozitie TabelTranspozitie = new(300);
        private static Dictionary<long, (Pozitie, Pozitie)> CaceDeschideri = new();
        private static Random GeneratorRandom = new();
        private static double MarimeFereastraAspiratie = 0;//ConstantaPiese.ValoarePion / 2;
        private static double ValoareMaxima = 50000;

        private EngineSinglePlayer _engine;
        private int _adancime;
        private Stopwatch _cronometruAI = new Stopwatch();

        private Stopwatch _cronometru = new();

        public Stopwatch CronometruAI
        {
            get { return _cronometru; }
            set { _cronometru = value; }
        }

        const int pionAlb = (int)CodPiesa.PionAlb;
        const int pionAlbastru = (int)CodPiesa.PionAlbastru;
        const int calAlb = (int)CodPiesa.CalAlb;
        const int calAlbastru = (int)CodPiesa.CalAbastru;
        const int tunAlb = (int)CodPiesa.TunAlb;
        const int tunAlbastru = (int)CodPiesa.TunAlbastru;
        const int turaAlba = (int)CodPiesa.TuraAlba;
        const int turaAlbastra = (int)CodPiesa.TuraAlbastra;
        const int elefantAlb = (int)CodPiesa.ElefantAlb;
        const int elefantAlbastru = (int)CodPiesa.ElefantAlbastru;
        const int gardianAlb = (int)CodPiesa.GardianAlb;
        const int gardianAlbastru = (int)CodPiesa.GardianAlbastru;
        const int regeAlb = (int)CodPiesa.RegeAlb;
        const int regeAlbastru = (int)CodPiesa.RegeAlbastru;

        private static double[][] TabelCapturiPiese = new double[][]
        {
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 },
            new double[] {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0 }
        };
        /*
        PionAlb = 1,
		PionAlbastru = 2,
		TuraAlba = 3,
		TuraAlbastra = 4,
		TunAlb = 5,
		TunAlbastru = 6,
		GardianAlb = 7,
		GardianAlbastru = 8,
		ElefantAlb = 9,
		ElefantAlbastru = 10,
		CalAlb = 11,
		CalAbastru = 12,
		RegeAlb = 13,
		RegeAlbastru = 14
		formula = 200 + piesaCareIa/100 + piesaLuata/10;
		*/
        private static void InitializeazaTabelCapturiPiese()
        {
            for (int i = 1; i <= 14; i++)
                for (int j = 1; j <= 14; j++)
                {
                    TabelCapturiPiese[i][j] = 200 + EngineSinglePlayer.ReturneazaScorPiesa(i) / 100 + EngineSinglePlayer.ReturneazaScorPiesa(j) / 10;
                }
        }

        private static int[][] TabelPionAlbastru = new int[][] {
            new int[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new int[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new int[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new int[] {0,  0, -2,  0,  4,  0, -2,  0,  0},
            new int[] {2,  0,  8,  0,  8,  0,  8,  0,  2},
            new int[] {6,  12, 18, 18, 20, 18, 18, 12, 6},
            new int[] {10, 20, 30, 34, 40, 34, 30, 20, 10},
            new int[] {14, 26, 42, 60, 80, 60, 42, 26, 14},
            new int[] {18, 36, 56, 80, 120, 80, 56, 36, 18},
            new int[] {0,  3,  6,  9,  12,  9,  6,  3,  0}
        };

        private static int[][] TabelPionAlb = new int[][] {

            new int[] {0,  3,  6,  9,  12,  9,  6,  3,  0},
            new int[] {18, 36, 56, 80, 120, 80, 56, 36, 18},
            new int[] {14, 26, 42, 60, 80, 60, 42, 26, 14},
            new int[] {10, 20, 30, 34, 40, 34, 30, 20, 10},
            new int[] {6,  12, 18, 18, 20, 18, 18, 12, 6},
            new int[] {2,  0,  8,  0,  8,  0,  8,  0,  2},
            new int[] {0,  0, -2,  0,  4,  0, -2,  0,  0},
            new int[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new int[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new int[] {0,  0,  0,  0,  0,  0,  0,  0,  0}
			//
        };


        private static int[][] TabelCalAlbastru = new int[][]
        {
            new int[] {0, -4, 0, 0, 0, 0, 0, -4, 0},
            new int[] {0, 2, 4, 4, -2, 4, 4, 2, 0},
            new int[] {4, 2, 8, 8, 4, 8, 8, 2, 4},
            new int[] {2, 6, 8, 6, 10, 6, 8, 6, 2},
            new int[] {4, 12, 16, 14, 12, 14, 16, 12, 4},
            new int[] {6, 16, 14, 18, 16, 18, 14, 16, 6},
            new int[] {8, 24, 18, 24, 20, 24, 18, 24, 8},
            new int[] {12, 14, 16, 20, 18, 20, 16, 14, 12},
            new int[] {4, 10, 28, 16, 8, 16, 28, 10, 4},
            new int[] {4, 8, 16, 12, 4, 12, 16, 8, 4}
        };

        private static int[][] TabelCalAlb = new int[][]
        {
            new int[] {4, 8, 16, 12, 4, 12, 16, 8, 4},
            new int[] {4, 10, 28, 16, 8, 16, 28, 10, 4},
            new int[] {12, 14, 16, 20, 18, 20, 16, 14, 12},
            new int[] {8, 24, 18, 24, 20, 24, 18, 24, 8},
            new int[] {6, 16, 14, 18, 16, 18, 14, 16, 6},
            new int[] {4, 12, 16, 14, 12, 14, 16, 12, 4},
            new int[] {2, 6, 8, 6, 10, 6, 8, 6, 2},
            new int[] {4, 2, 8, 8, 4, 8, 8, 2, 4},
            new int[] {0, 2, 4, 4, -2, 4, 4, 2, 0},
            new int[] {0, -4, 0, 0, 0, 0, 0, -4, 0}
        };

        private static int[][] TabelTunAlbastru = new int[][]
        {
            new int[] {0, 0, 2, 6, 6, 6, 2, 0, 0},
            new int[] {0, 2, 4, 6, 6, 6, 4, 2, 0},
            new int[] {4, 0, 8, 6, 10, 6, 8, 0, 4},
            new int[] {0, 0, 0, 2, 4, 2, 0, 0, 0},
            new int[] {-2, 0, 4, 2, 6, 2, 4, 0, -2},
            new int[] {0, 0, 0, 2, 8, 2, 0, 0, 0},
            new int[] {0, 0, -2, 4, 10, 4, -2, 0, 0},
            new int[] {2, 2, 0, -10, -8, -10, 0, 2, 2},
            new int[] {2, 2, 0, -4, -14, -4, 0, 2, 2},
            new int[] {6, 4, 0, -10, -12, -10, 0, 4, 6}
        };
        private static int[][] TabelTunAlb = new int[][]
        {
            new int[] {6, 4, 0, -10, -12, -10, 0, 4, 6},
            new int[] {2, 2, 0, -4, -14, -4, 0, 2, 2},
            new int[] {2, 2, 0, -10, -8, -10, 0, 2, 2},
            new int[] {0, 0, -2, 4, 10, 4, -2, 0, 0},
            new int[] {0, 0, 0, 2, 8, 2, 0, 0, 0},
            new int[] {-2, 0, 4, 2, 6, 2, 4, 0, -2},
            new int[] {0, 0, 0, 2, 4, 2, 0, 0, 0},
            new int[] {4, 0, 8, 6, 10, 6, 8, 0, 4},
            new int[] {0, 2, 4, 6, 6, 6, 4, 2, 0},
            new int[] {0, 0, 2, 6, 6, 6, 2, 0, 0},
        };

        private static int[][] TabelTuraAlbastra = new int[][]
        {
            new int[] {-2, 10, 6, 14, 12, 14, 6, 10, -2},
            new int[] {8, 4, 8, 16, 8, 16, 8, 4, 8},
            new int[] {4, 8, 6, 14, 12, 14, 6, 8, 4},
            new int[] {6, 10, 8, 14, 14, 14, 8, 10, 6},
            new int[] {12, 16, 14, 20, 20, 20, 14, 16, 12},
            new int[] {12, 14, 12, 18, 18, 18, 12, 14, 12},
            new int[] {12, 18, 16, 22, 22, 22, 16, 18, 12},
            new int[] {12, 12, 12, 18, 18, 18, 12, 12, 12},
            new int[] {16, 20, 18, 24, 26, 24, 18, 20, 16},
            new int[] {14, 14, 12, 18, 16, 18, 12, 14, 14}
        };
        private static int[][] TabelTuraAlba = new int[][]
        {
            new int[] {-14, -14, -16, -18, -20, -18, -14, -14, -14},
            new int[] {14, 12, 18, 18, 18, 12, 14, 14, 14},
            new int[] {12, 18, 24, 26, 24, 18, 16, 20, 16},
            new int[] {12, 12, 12, 18, 18, 18, 12, 12, 12},
            new int[] {12, 18, 16, 22, 22, 22, 16, 18, 12},
            new int[] {12, 14, 12, 18, 18, 18, 12, 14, 12},
            new int[] {12, 16, 14, 20, 20, 20, 14, 16, 12},
            new int[] {4, 8, 6, 14, 12, 14, 6, 8, 4},
            new int[] {-8, -4, -8, -16, -8, -16, -8, -4, -8},
            new int[] {2, -10, -6, -14, -12, -14, -6, -10, 2}
        };
        private static int[][] TabelGardianAlb = new int[][]
        {
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, -1, 0, -1, 0, 0, 0},
            new int[] {0, 0, 0, 0, 3, 0, 0, 0, 0},
            new int[] {0, 0, 0, 1, 0, 1, 0, 0, 0},
        };
        private static int[][] TabelGardianAlbastru = new int[][]
        {
            new int[] {0, 0, 0, 1, 0, 1, 0, 0, 0},
            new int[] {0, 0, 0, 0, 3, 0, 0, 0, 0},
            new int[] {0, 0, 0, -1, 0, -1, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
        };
        //
        private static int[][] TabelRegeAlb = new int[][]
        {
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, -2, -2, -2, 0, 0, 0},
            new int[] {0, 0, 0, -2, -2, -2, 0, 0, 0},
            new int[] {0, 0, 0, -2, 2, -2, 0, 0, 0},
        };
        private static int[][] TabelRegeAlbastru = new int[][]
        {
            new int[] {0, 0, 0, -2, 2, -2, 0, 0, 0},
            new int[] {0, 0, 0, -2, -2, -2, 0, 0, 0},
            new int[] {0, 0, 0, -2, -2, -2, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
        };
        //
        private static int[][] TabelElefantAlb = new int[][]
        {
            new int[] {0, 1, 0, 0, 0, -1, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, -1, 0, 0, 0, -1, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {-2, 0, 0, 0, 3, 0, 0, 0, -2 },
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, -1, 0, 1, 0, 0, 0}
        };
        private static int[][] TabelElefantAlbastru = new int[][]
        {
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, -1, 0, 0, 0, -1, 0, 0},
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {-2, 0, 0, 0, 3, 0, 0, 0, -2 },
            new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new int[] {0, 0, 1, 0, 0, 0, 1, 0, 0}
        };
        private static List<int[][]> ListaTabelePST = new List<int[][]>()
        {
            null,
            TabelPionAlb,
            TabelPionAlbastru,
            TabelTuraAlba,
            TabelTuraAlbastra,
            TabelTunAlb,
            TabelTunAlbastru,
            TabelGardianAlb,
            TabelGardianAlbastru,
            TabelElefantAlb,
            TabelElefantAlbastru,
            TabelCalAlb,
            TabelCalAlbastru,
            TabelRegeAlb,
            TabelRegeAlbastru

        };
        public int Adancime
        {
            get { return _adancime; }
        }

        private static List<Piesa> _pieseVirtuale = new()
        {
            null,
            new Pion(Culoare.AlbMin),
            new Pion(Culoare.AlbastruMax),
            new Tura(Culoare.AlbMin),
            new Tura(Culoare.AlbastruMax),
            new Tun(Culoare.AlbMin),
            new Tun(Culoare.AlbastruMax),
            new Gardian(Culoare.AlbMin),
            new Gardian(Culoare.AlbastruMax),
            new Elefant(Culoare.AlbMin),
            new Elefant(Culoare.AlbastruMax),
            new Cal(Culoare.AlbMin),
            new Cal(Culoare.AlbastruMax),
            new Rege(Culoare.AlbMin),
            new Rege(Culoare.AlbastruMax)
        };


        public AlphaBetaAI(Culoare culoare, EngineSinglePlayer engine, int adancime = ConstantaTabla.Adancime) : base(culoare)
        {
            _engine = engine;
            _culoare = culoare;
            _adancime = adancime;
            AdaugaOpeningsInCache();
            InitializeazaTabelCapturiPiese();

        }
        public class DuplicateKeyComparerAsc<TKey>
                :
             IComparer<TKey> where TKey : IComparable
        {
            #region IComparer<TKey> Members

            public int Compare(TKey x, TKey y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                {
                    return 1;
                }
                else
                {
                    return result;
                }
            }

            #endregion IComparer<TKey> Members
        }
        public class DuplicateKeyComparerDesc<TKey>
                :
             IComparer<TKey> where TKey : IComparable
        {
            #region IComparer<TKey> Members

            public int Compare(TKey x, TKey y)
            {
                int result = y.CompareTo(x);

                if (result == 0)
                {
                    return 1;
                }
                else
                {
                    return result;
                }
            }

            #endregion IComparer<TKey> Members
        }
        //200 + valoarePiesaLuata/10 - valoarePiesaCareCaptureaza/100 totul aproximat prin adaos(exceptie pion

        public static void AdaugaOpeningsInCache()
        {
            long hash;

            int[][] pionAlbMij = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 0, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 2, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(pionAlbMij);

            CaceDeschideri.Add(hash, (new Pozitie(7, 1), new Pozitie(7, 4)));

            int[][] pionAlbastruMij = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 1, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 0, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(pionAlbastruMij);

            CaceDeschideri.Add(hash, (new Pozitie(2, 1), new Pozitie(2, 4)));

            int[][] atacPionAlbastru = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 0, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 2, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(atacPionAlbastru);

            CaceDeschideri.Add(hash, (new Pozitie(7, 1), new Pozitie(7, 2)));

            int[][] atacPionAlbastru1 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 0, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 2, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(atacPionAlbastru1);

            CaceDeschideri.Add(hash, (new Pozitie(7, 1), new Pozitie(7, 2)));

            int[][] atacPionAlbastru2 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 0, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 2, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(atacPionAlbastru2);

            CaceDeschideri.Add(hash, (new Pozitie(7, 7), new Pozitie(7, 6)));

            int[][] atacPionAlbastru3 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(atacPionAlbastru3);

            CaceDeschideri.Add(hash, (new Pozitie(7, 7), new Pozitie(7, 6)));

            int[][] pionAlb1 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 1, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 0, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(pionAlb1);

            CaceDeschideri.Add(hash, (new Pozitie(2, 1), new Pozitie(2, 2)));

            int[][] pionAlb12 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 1, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 0, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(pionAlb12);

            CaceDeschideri.Add(hash, (new Pozitie(2, 7), new Pozitie(2, 6)));

            int[][] pionAlb3 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(pionAlb3);

            CaceDeschideri.Add(hash, (new Pozitie(0, 1), new Pozitie(2, 2)));

            int[][] pionAlb4 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 1},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 0},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(pionAlb4);

            CaceDeschideri.Add(hash, (new Pozitie(2, 7), new Pozitie(2, 8)));

            int[][] tunAlbAtac1 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 5, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(tunAlbAtac1);

            CaceDeschideri.Add(hash, (new Pozitie(0, 1), new Pozitie(2, 2)));

            int[][] tunAlbAtac2 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 5, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(tunAlbAtac2);

            CaceDeschideri.Add(hash, (new Pozitie(0, 7), new Pozitie(2, 6)));

            int[][] tunAlbastruAtac1 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 6, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(tunAlbastruAtac1);

            CaceDeschideri.Add(hash, (new Pozitie(9, 1), new Pozitie(7, 2)));

            int[][] tunAlbastruAtac2 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 6, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(tunAlbastruAtac2);

            CaceDeschideri.Add(hash, (new Pozitie(0, 7), new Pozitie(7, 6)));

            int[][] tunAlbMijloc1 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 0, 0, 0, 5, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(tunAlbMijloc1);

            CaceDeschideri.Add(hash, (new Pozitie(0, 1), new Pozitie(2, 2)));

            int[][] tunAlbMijloc2 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 5, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(tunAlbMijloc2);

            CaceDeschideri.Add(hash, (new Pozitie(0, 7), new Pozitie(2, 6)));

            int[][] tunAlbastruMijloc1 = new int[10][]
{
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 6, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
};

            hash = ZobristHash.HashuiesteTabla(tunAlbastruMijloc1);

            CaceDeschideri.Add(hash, (new Pozitie(9, 7), new Pozitie(7, 6)));

            int[][] tunAlbastruMijloc2 = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 6, 0, 0, 0, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(tunAlbastruMijloc2);

            CaceDeschideri.Add(hash, (new Pozitie(9, 1), new Pozitie(7, 2)));

            int[][] atacTunAlbLaGardianStanga = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 0, 5, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(atacTunAlbLaGardianStanga);

            CaceDeschideri.Add(hash, (new Pozitie(0, 1), new Pozitie(2, 0)));

            int[][] atacTunAlbLaGardianDreapta = new int[10][]
                {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 5, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
                };

            hash = ZobristHash.HashuiesteTabla(atacTunAlbLaGardianDreapta);

            CaceDeschideri.Add(hash, (new Pozitie(0, 7), new Pozitie(2, 8)));

            int[][] atacTunAlbastruLaGardianDreapta = new int[10][]
                {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 6, 0, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
                };

            hash = ZobristHash.HashuiesteTabla(atacTunAlbastruLaGardianDreapta);
            CaceDeschideri.Add(hash, (new Pozitie(0, 8), new Pozitie(2, 8)));

            int[][] atacTunAlbastruLaGardianDr = new int[10][]
                {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 6, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
                };

            hash = ZobristHash.HashuiesteTabla(atacTunAlbastruLaGardianDr);
            CaceDeschideri.Add(hash, (new Pozitie(9, 1), new Pozitie(7, 0)));

            int[][] atacTunAlbDr = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 6, 0, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 6, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(atacTunAlbDr);

            CaceDeschideri.Add(hash, (new Pozitie(9, 7), new Pozitie(7, 9)));
            int[][] atacTunAlbDreapta = new int[10][]
            {
                new int[] { 4, 12, 10, 8, 14, 8, 10, 12, 4 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 6, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1 },
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0 },
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                new int[] { 3, 11, 9, 7, 13, 7, 9, 6, 3 }
            };

            hash = ZobristHash.HashuiesteTabla(atacTunAlbDreapta);

            CaceDeschideri.Add(hash, (new Pozitie(9, 9), new Pozitie(9, 8)));

            int[][] atacTunAlbastruStanga = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 5, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(atacTunAlbastruStanga);

            CaceDeschideri.Add(hash, (new Pozitie(0, 8), new Pozitie(0, 7)));

            int[][] atacTunAlbastruDreapta = new int[10][]
            {
                new int [] { 4, 5, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(atacTunAlbastruDreapta);

            CaceDeschideri.Add(hash, (new Pozitie(0, 0), new Pozitie(0, 1)));
        }

        public static SortedList<double, Mutare> CalculeazaCapturiPosibile(int[][] matrice, Pozitie[] pozitiiPieseDeEvaluat)
        {
            SortedList<double, Mutare> capPos;

            capPos = new(18, new DuplicateKeyComparerDesc<double>());
            foreach (Pozitie poz in pozitiiPieseDeEvaluat)
            {
                if (poz.Linie != -1)
                {
                    _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].Pozitie = new Pozitie(poz.Linie, poz.Coloana);
                    List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].ReturneazaMutariPosibile(matrice);
                    foreach (Pozitie mut in mutari)
                    {
                        int piesaLuata = matrice[mut.Linie][mut.Coloana];
                        int piesaCareIa = matrice[poz.Linie][poz.Coloana];

                        if (piesaLuata != 0)
                        {
                            capPos.Add(-EngineJoc.ReturneazaScorPiesa(piesaCareIa)
                                + -EngineJoc.ReturneazaScorPiesa(piesaLuata)
                                ,
                                new(new(poz.Linie, poz.Coloana), mut));
                        }
                    }
                }
            }
            return capPos;
        }


        public static SortedList<double, Mutare> CalculeazaMutariPosibile(int[][] matrice, Pozitie[] pozitiiPieseDeEvaluat, bool moveOrdering = true)
        {
            SortedList<double, Mutare> mutPos;

            //most valuable victim, least valuable agressor
            //Piece-Square Tables

            if (moveOrdering == true)
            {
                mutPos = new(80, new DuplicateKeyComparerDesc<double>());
                foreach (Pozitie poz in pozitiiPieseDeEvaluat)
                {
                    if (poz.Linie != -1)
                    {
                        _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].Pozitie = new Pozitie(poz.Linie, poz.Coloana);
                        List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].ReturneazaMutariPosibile(matrice);
                        foreach (Pozitie mut in mutari)
                        {
                            int piesaLuata = matrice[mut.Linie][mut.Coloana];
                            int piesaCareIa = matrice[poz.Linie][poz.Coloana];

                            if (piesaLuata == 0)
                            {
                                mutPos.Add(ListaTabelePST[piesaCareIa][mut.Linie][mut.Coloana], new(new(poz.Linie, poz.Coloana), mut));
                            }
                            else
                            {
                                mutPos.Add(TabelCapturiPiese[piesaCareIa][piesaLuata], new(new(poz.Linie, poz.Coloana), mut));
                            }
                        }
                    }
                }
                return mutPos;
            }
            else
            {
                mutPos = new(80, new DuplicateKeyComparerAsc<double>());
                int ct = 0;
                foreach (Pozitie poz in pozitiiPieseDeEvaluat)
                {
                    if (poz.Linie != -1)
                    {
                        _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].Pozitie = new Pozitie(poz.Linie, poz.Coloana);
                        List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].ReturneazaMutariPosibile(matrice);
                        foreach (Pozitie mut in mutari)
                        {
                            mutPos.Add(ct++, new(new(poz.Linie, poz.Coloana), mut));
                        }
                    }
                }
                return mutPos;
            }
        }
        public static void StergePozitieDinVector(int index, Pozitie[] vector)
        {
            vector[index] = Pozitie.PozitieNula;
        }

        public static int StergePozitieDinVector(Pozitie poz, Pozitie[] vector)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i].Linie == poz.Linie && vector[i].Coloana == poz.Coloana)
                {
                    vector[i] = Pozitie.PozitieNula;
                    return i;
                }
            }
            return -1;
        }

        public static int SchimbaPozitiaDinVector(int index,
                                            Pozitie[] vector,
                                            Pozitie pozFinala)
        {
            vector[index] = pozFinala;
            return index;
        }

        public static int SchimbaPozitiaDinVector(Pozitie pozInitiala,
                                            Pozitie[] vector,
                                            Pozitie pozFinala)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i].Linie == pozInitiala.Linie && vector[i].Coloana == pozInitiala.Coloana)
                {
                    vector[i] = pozFinala;
                    return i;
                }
            }
            return -1;
        }

        public static int SchimbaPozitiaDinVector(Pozitie[] vector,
                                            Pozitie pozFinala,
                                            int index)
        {
            vector[index] = pozFinala;
            return index;
        }

        public static int AdaugaPoitieInVector(Pozitie[] vector,
                                            Pozitie pozFinala)
        {
            for (int i = 0; i < vector.Length; i++)
            {
                if (vector[i].Linie == -1)
                {
                    vector[i] = pozFinala;
                    return i;
                }
            }
            return -1;
        }

        public static int AdaugaPozitieInVector(int i, Pozitie[] vector,
                                            Pozitie pozFinala)
        {
            vector[i] = pozFinala;
            return i;
        }

        public override Tuple<Mutare, double> ReturneazaMutareaOptima()
        {
            int[][] matriceClonata = _engine.MatriceCoduriPiese.Clone() as int[][];
            long hashInitial = ZobristHash.HashuiesteTabla(matriceClonata);

            if (_engine.NrMutari <= 1)
            {
                if (CaceDeschideri.ContainsKey(hashInitial))
                {
                    (Pozitie, Pozitie) item = CaceDeschideri[hashInitial];
                    return new(new Mutare(item.Item1, item.Item2), double.MinValue);
                }
            }

            double alpha = -50000;
            double beta = 50000;

            double scorMutare;
            int indexPozLuata,
                indexPozCareIa,
                codPiesaCareIa,
                codPiesaLuata;
            long hashUpdatat;


            _cronometruAI.Start();

            double evaluareMatriceInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);


            Pozitie[] pozitiiAlbastre = _engine.ReturneazaPozitiiAlbastre();
            Pozitie[] pozitiiAlbe = _engine.ReturneazaPozitiiAlbe();

            var mutariPosibile = CalculeazaMutariPosibile(matriceClonata, pozitiiAlbastre, moveOrdering: true);
            var valoriMutariPosibile = mutariPosibile.Values;

            Mutare mutareOptima = valoriMutariPosibile[0];
            double scorMutareOptima = evaluareMatriceInitiala
                        + EngineJoc.ReturneazaScorPiesa(
                        matriceClonata[
                        valoriMutariPosibile[0].MutareFinala.Linie][
                        valoriMutariPosibile[0].MutareFinala.Coloana]
                        );
            int adancimeMutareOptima = 0;


            int index = 0;


            for (int adancimeIterativa = 1; adancimeIterativa <= _adancime &&
                CronometruAI.ElapsedMilliseconds < 8000; adancimeIterativa++)
            {

                index = 0;

                foreach (var mutarePosibila in valoriMutariPosibile)
                {
                    codPiesaLuata = matriceClonata[
                        mutarePosibila.MutareFinala.Linie][
                        mutarePosibila.MutareFinala.Coloana];

                    codPiesaCareIa = matriceClonata[
                        mutarePosibila.MutareInitiala.Linie][
                        mutarePosibila.MutareInitiala.Coloana];

                    if (codPiesaLuata == (int)CodPiesa.RegeAlb)
                    {
                        mutareOptima = mutarePosibila;
                        scorMutareOptima = double.MaxValue;
                        break;
                    }

                    matriceClonata[
                        mutarePosibila.MutareInitiala.Linie][
                        mutarePosibila.MutareInitiala.Coloana] = 0;

                    matriceClonata[
                        mutarePosibila.MutareFinala.Linie][
                        mutarePosibila.MutareFinala.Coloana] = codPiesaCareIa;

                    indexPozCareIa = SchimbaPozitiaDinVector(
                        mutarePosibila.MutareInitiala,
                        pozitiiAlbastre,
                        mutarePosibila.MutareFinala);

                    indexPozLuata = -1;
                    if (EstePiesa(codPiesaLuata))
                    {
                        indexPozLuata = StergePozitieDinVector(mutarePosibila.MutareFinala,
                                                pozitiiAlbe);
                    }

                    hashUpdatat = ZobristHash.UpdateazaHash(
                        hashInitial: hashInitial,
                        linieInitiala: mutarePosibila.MutareInitiala.Linie,
                        coloanaInitiala: mutarePosibila.MutareInitiala.Coloana,
                        piesaLuata: codPiesaLuata,
                        linieFinala: mutarePosibila.MutareFinala.Linie,
                        coloanaFinala: mutarePosibila.MutareFinala.Coloana,
                        piesaCareIa: codPiesaCareIa);



                    scorMutare = AlphaBetaCuMemorie(
                                evaluareMatriceInitiala + EngineJoc.ReturneazaScorPiesa(codPiesaLuata)
                                , matriceClonata
                                , alpha
                                , beta
                                , adancimeIterativa
                                , codPiesaLuata
                                , hashUpdatat
                                , pozitiiAlbe
                                , pozitiiAlbastre
                                , Culoare.AlbMin);




                    //Debug.WriteLine(scorMutare + " " + adancimeTemp + " " + mutarePos);
                    SchimbaPozitiaDinVector(indexPozCareIa,
                        pozitiiAlbastre,
                        mutarePosibila.MutareInitiala);

                    if (EstePiesa(codPiesaLuata))
                    {
                        AdaugaPozitieInVector(indexPozLuata, pozitiiAlbe, mutarePosibila.MutareFinala);
                    }

                    matriceClonata[
                        mutarePosibila.MutareInitiala.Linie][
                        mutarePosibila.MutareInitiala.Coloana] = codPiesaCareIa;

                    matriceClonata[
                        mutarePosibila.MutareFinala.Linie][
                        mutarePosibila.MutareFinala.Coloana] = codPiesaLuata;

                    if (scorMutare >= scorMutareOptima  && adancimeIterativa == adancimeMutareOptima ||
                        adancimeIterativa > adancimeMutareOptima)
                    {
                        mutareOptima = mutarePosibila;
                        scorMutareOptima = scorMutare;
                        adancimeMutareOptima = adancimeIterativa;
                    }
                    Debug.WriteLine($"{mutarePosibila} cu scor:{scorMutare} si adancime:{adancimeIterativa}");
                    index++;
                }
            }
            _cronometruAI.Stop();
            _cronometruAI.Reset();
            Debug.WriteLine(scorMutareOptima + " " + adancimeMutareOptima + " " + mutareOptima);
            return new(mutareOptima, scorMutareOptima);
        }
        public static double EvalueazaMatricea(int[][] matrice)
        {
            double scor = 0;

            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    //albastru
                    if ((matrice[linie][coloana] - 1) % 2 != 0)
                    {
                        double valoarePiesaLuata = EngineJoc.ReturneazaScorPiesa(matrice[linie][coloana]);
                        scor += valoarePiesaLuata;
                    }
                    //alb
                    else
                    {
                        double valoarePiesaLuata = EngineJoc.ReturneazaScorPiesa(matrice[linie][coloana]);
                        scor -= valoarePiesaLuata;
                    }
                }
            }
            return scor;
        }

        public void AfiseazaVectorDebug(Pozitie[] vector)
        {
            foreach (Pozitie v in vector)
            {
                Debug.Write(v + " ");
            }
            Debug.WriteLine("");
        }

        public static double AlphaBetaCuMemorie(double eval, int[][] matrice, double alpha,
            double beta, int adancime, int piesaCapturata, long hash,
            Pozitie[] pozAlbe, Pozitie[] pozAlbastre, Culoare culoare)
        {

            if (TabelTranspozitie.Contine(hash))
            {
                IntrareTabel entry = TabelTranspozitie.ReturneazaIntrarea(hash);

                if (entry.Adancime >= adancime)
                {
                    switch (entry.Flag)
                    {
                        case 0: // Exact value
                           return entry.Scor;
                        case 1: // Lower bound
                            alpha = Math.Max(alpha, entry.Scor);
                            break;
                        case 2: // Upper bound
                            beta = Math.Min(beta, entry.Scor);
                            break;
                    }

                    if (beta <= alpha)
                    {
                        return entry.Scor;
                    }
                }
            }
            if (adancime <= 0)
            {
                return eval;
            }
            if (piesaCapturata == (int)CodPiesa.RegeAlb ||
                piesaCapturata == (int)CodPiesa.RegeAlbastru
                )
            {
                return eval;
            }


            //maximizare => albastru
            if (culoare == Culoare.AlbastruMax)
            {
                var origAlpha = alpha;
                long hashUpdatat;
                int piesaLuata;
                int piesaCareIa;


                SortedList<double, Mutare> mutariSortate = CalculeazaMutariPosibile(matrice, pozAlbastre, moveOrdering: true);

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;

                double val = -ValoareMaxima;
                foreach (Mutare mutPos in mutSortateValues)
                {

                    piesaLuata = matrice[mutPos.MutareFinala.Linie][mutPos.MutareFinala.Coloana];
                    piesaCareIa = matrice[mutPos.MutareInitiala.Linie][mutPos.MutareInitiala.Coloana];

                    matrice[mutPos.MutareInitiala.Linie][mutPos.MutareInitiala.Coloana] = (int)CodPiesa.Gol;
                    matrice[mutPos.MutareFinala.Linie][mutPos.MutareFinala.Coloana] = piesaCareIa;

                    hashUpdatat = ZobristHash.UpdateazaHash(
                        hashInitial: hash,
                        linieInitiala: mutPos.MutareInitiala.Linie,
                        coloanaInitiala: mutPos.MutareInitiala.Coloana,
                        piesaLuata: piesaLuata,
                        linieFinala: mutPos.MutareFinala.Linie,
                        coloanaFinala: mutPos.MutareFinala.Coloana,
                        piesaCareIa: piesaCareIa);

                    int indexPiesaLuata = -1;
                    //albastru ia alb
                    if (EstePiesa(piesaLuata))
                    {
                        indexPiesaLuata = StergePozitieDinVector(mutPos.MutareFinala,
                                    pozAlbe);
                    }
                    int pozitieSchimbata = SchimbaPozitiaDinVector(mutPos.MutareInitiala, pozAlbastre, mutPos.MutareFinala);

                    double valoarePiesaLuata = EngineJoc.ReturneazaScorPiesa(piesaLuata);

                    val = Math.Max(val, AlphaBetaCuMemorie(eval + valoarePiesaLuata,
                                matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbMin));
                    alpha = Math.Max(val, alpha);

                    SchimbaPozitiaDinVector(pozitieSchimbata, pozAlbastre, mutPos.MutareInitiala);

                    if (EstePiesa(piesaLuata))
                    {
                        AdaugaPozitieInVector(indexPiesaLuata, pozAlbe,
                                    mutPos.MutareFinala);
                    }

                    matrice[mutPos.MutareInitiala.Linie][mutPos.MutareInitiala.Coloana] = piesaCareIa;
                    matrice[mutPos.MutareFinala.Linie][mutPos.MutareFinala.Coloana] = piesaLuata;

                    if (val >= beta)
                    {
                        goto ValoareFinala;
                    }
                }
            ValoareFinala:

                int flag = 0; // Exact value
                if (val <= origAlpha)
                {
                    flag = 2; // Upper bound
                }
                else if (val >= beta)
                {
                    flag = 1; // Lower bound
                }
                TabelTranspozitie.AdaugaIntrare(hash, val, adancime, flag);


                return val;
            }
            //minimizare => alb
            else
            {
                var origBeta = beta;
                long hashUpdatat;
                int piesaLuata;
                int piesaCareIa;

                SortedList<double, Mutare> mutariSortate = CalculeazaMutariPosibile(matrice, pozAlbe, moveOrdering: true);

                if (mutariSortate.Count == 0)
                    return eval;

                double val = ValoareMaxima;
                var mutSortateValues = mutariSortate.Values;
                foreach (Mutare mutPos in mutSortateValues)
                {
                    piesaLuata = matrice[mutPos.MutareFinala.Linie][mutPos.MutareFinala.Coloana];
                    piesaCareIa = matrice[mutPos.MutareInitiala.Linie][mutPos.MutareInitiala.Coloana];

                    matrice[mutPos.MutareInitiala.Linie][mutPos.MutareInitiala.Coloana] = (int)CodPiesa.Gol;
                    matrice[mutPos.MutareFinala.Linie][mutPos.MutareFinala.Coloana] = piesaCareIa;

                    hashUpdatat = ZobristHash.UpdateazaHash(
                        hashInitial: hash,
                        linieInitiala: mutPos.MutareInitiala.Linie,
                        coloanaInitiala: mutPos.MutareInitiala.Coloana,
                        piesaLuata: piesaLuata,
                        linieFinala: mutPos.MutareFinala.Linie,
                        coloanaFinala: mutPos.MutareFinala.Coloana,
                        piesaCareIa: piesaCareIa);

                    //alb ia albastru
                    int indexPiesaLuata = -1;
                    if (EstePiesa(piesaLuata))
                    {
                        indexPiesaLuata = StergePozitieDinVector(mutPos.MutareFinala,
                                    pozAlbastre);
                    }
                    int pozitieSchimbata = SchimbaPozitiaDinVector(mutPos.MutareInitiala, pozAlbe, mutPos.MutareFinala);


                    double valoarePiesaLuata = EngineJoc.ReturneazaScorPiesa(piesaLuata);

                    val = Math.Min(val, AlphaBetaCuMemorie(eval - valoarePiesaLuata,
                        matrice, alpha, beta, adancime - 1,
                        piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbastruMax));

                    beta = Math.Min(val, beta);

                    if (EstePiesa(piesaLuata))
                    {
                        AdaugaPozitieInVector(indexPiesaLuata, pozAlbastre,
                                   mutPos.MutareFinala);
                    }

                    SchimbaPozitiaDinVector(pozitieSchimbata, pozAlbe, mutPos.MutareInitiala);

                    matrice[mutPos.MutareInitiala.Linie][mutPos.MutareInitiala.Coloana] = piesaCareIa;
                    matrice[mutPos.MutareFinala.Linie][mutPos.MutareFinala.Coloana] = piesaLuata;

                    if (val <= alpha)
                    {
                        goto ValoareFinala;
                    }
                }
            ValoareFinala:

                int flag = 0; // Exact value
                if (val <= alpha)
                {
                    flag = 2; // Upper bound
                }
                else if (val >= origBeta)
                {
                    flag = 1; // Lower bound
                }
                TabelTranspozitie.AdaugaIntrare(hash, val, adancime, flag);

                return val;
            }
        }

        private static bool EstePiesa(int codPiesa)
        {
            if (codPiesa == 0)
            {
                return false;
            }

            return true;
        }
    }
}