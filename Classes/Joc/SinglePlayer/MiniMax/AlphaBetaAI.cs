using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;

namespace ProiectVolovici
{
    public class AlphaBetaAI : AI
    {
        private static TabelTranspozitie TabelTranspozitie = new(300);
        private static Dictionary<ulong, (Pozitie, Pozitie)> CaceDeschideri = new();
        private static double MarimeFereastraAspiratie = ConstantaPiese.ValoarePion / 4;
        private static double ValoareMaxima = 50000;
        private static bool FerestreAspiratie = true;
        private static Dictionary<(int, Pozitie), double> HistoryTable = new(14 * 90);
        private static Mutare[][] KillerMoves;
        private static int MarimeKillerMoves = 64;
        private static int NoduriEvaluate = 0;

        private static double OffsetMVVLVA = 200;
        private static double OffsetKillerMoves = 400;
        private static double OffsetHistoryTable = 200;
        const int FullDepthMoves = 4;
        const int ReductionLimit = 3;


        private static double ProcentajMaterial = 0.95;
        private static double ProcentajPST = 0.0;
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
		*/
        private static void InitializeazaTabelCapturiPiese()
        {
            for (int piesaCareIa = 1; piesaCareIa <= 14; piesaCareIa++)
                for (int piesaCapturata = 1; piesaCapturata <= 14; piesaCapturata++)
                {
                    TabelCapturiPiese[piesaCareIa][piesaCapturata] = OffsetMVVLVA + ConstantaPiese.ValoareRege
                        + EngineSinglePlayer.ReturneazaScorPiesa(piesaCapturata)
                        - EngineSinglePlayer.ReturneazaScorPiesa(piesaCareIa);
                    //explicatie: 500 - o valoare ca sa puna capturile peste tabelele de pst
                    //valoarerege ca sa nulifice in caz ca regele ia ceva
                }
        }
        private static double[][] TabelNul = new double[][] {
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0}
        };


        private static double[][] TabelPionAlbastru = new double[][] {
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0, -2,  0,  4,  0, -2,  0,  0},
            new double[] {2,  0,  8,  0,  8,  0,  8,  0,  2},
            new double[] {6,  12, 18, 18, 20, 18, 18, 12, 6},
            new double[] {10, 20, 30, 34, 40, 34, 30, 20, 10},
            new double[] {14, 26, 42, 60, 80, 60, 42, 26, 14},
            new double[] {18, 36, 56, 80, 120, 80, 56, 36, 18},
            new double[] {0,  3,  6,  9,  12,  9,  6,  3,  0}
        };

        private static double[][] TabelPionAlb = new double[][] {

            new double[] {0,  3,  6,  9,  12,  9,  6,  3,  0},
            new double[] {18, 36, 56, 80, 120, 80, 56, 36, 18},
            new double[] {14, 26, 42, 60, 80, 60, 42, 26, 14},
            new double[] {10, 20, 30, 34, 40, 34, 30, 20, 10},
            new double[] {6,  12, 18, 18, 20, 18, 18, 12, 6},
            new double[] {2,  0,  8,  0,  8,  0,  8,  0,  2},
            new double[] {0,  0, -2,  0,  4,  0, -2,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0},
            new double[] {0,  0,  0,  0,  0,  0,  0,  0,  0}
			//
        };


        private static double[][] TabelCalAlbastru = new double[][]
        {
            new double[] {0, -4, 0, 0, 0, 0, 0, -4, 0},
            new double[] {0, 2, 4, 4, -2, 4, 4, 2, 0},
            new double[] {4, 2, 8, 8, 4, 8, 8, 2, 4},
            new double[] {2, 6, 8, 6, 10, 6, 8, 6, 2},
            new double[] {4, 12, 16, 14, 12, 14, 16, 12, 4},
            new double[] {6, 16, 14, 18, 16, 18, 14, 16, 6},
            new double[] {8, 24, 18, 24, 20, 24, 18, 24, 8},
            new double[] {12, 14, 16, 20, 18, 20, 16, 14, 12},
            new double[] {4, 10, 28, 16, 8, 16, 28, 10, 4},
            new double[] {4, 8, 16, 12, 4, 12, 16, 8, 4}
        };

        private static double[][] TabelCalAlb = new double[][]
        {
            new double[] {4, 8, 16, 12, 4, 12, 16, 8, 4},
            new double[] {4, 10, 28, 16, 8, 16, 28, 10, 4},
            new double[] {12, 14, 16, 20, 18, 20, 16, 14, 12},
            new double[] {8, 24, 18, 24, 20, 24, 18, 24, 8},
            new double[] {6, 16, 14, 18, 16, 18, 14, 16, 6},
            new double[] {4, 12, 16, 14, 12, 14, 16, 12, 4},
            new double[] {2, 6, 8, 6, 10, 6, 8, 6, 2},
            new double[] {4, 2, 8, 8, 4, 8, 8, 2, 4},
            new double[] {0, 2, 4, 4, -2, 4, 4, 2, 0},
            new double[] {0, -4, 0, 0, 0, 0, 0, -4, 0}
        };

        private static double[][] TabelTunAlbastru = new double[][]
        {
            new double[] {0, 0, 2, 6, 6, 6, 2, 0, 0},
            new double[] {0, 2, 4, 6, 6, 6, 4, 2, 0},
            new double[] {4, 0, 8, 6, 10, 6, 8, 0, 4},
            new double[] {0, 0, 0, 2, 4, 2, 0, 0, 0},
            new double[] {-2, 0, 4, 2, 6, 2, 4, 0, -2},
            new double[] {0, 0, 0, 2, 8, 2, 0, 0, 0},
            new double[] {0, 0, -2, 4, 10, 4, -2, 0, 0},
            new double[] {2, 2, 0, -10, -8, -10, 0, 2, 2},
            new double[] {2, 2, 0, -4, -14, -4, 0, 2, 2},
            new double[] {6, 4, 0, -10, -12, -10, 0, 4, 6}
        };
        private static double[][] TabelTunAlb = new double[][]
        {
            new double[] {6, 4, 0, -10, -12, -10, 0, 4, 6},
            new double[] {2, 2, 0, -4, -14, -4, 0, 2, 2},
            new double[] {2, 2, 0, -10, -8, -10, 0, 2, 2},
            new double[] {0, 0, -2, 4, 10, 4, -2, 0, 0},
            new double[] {0, 0, 0, 2, 8, 2, 0, 0, 0},
            new double[] {-2, 0, 4, 2, 6, 2, 4, 0, -2},
            new double[] {0, 0, 0, 2, 4, 2, 0, 0, 0},
            new double[] {4, 0, 8, 6, 10, 6, 8, 0, 4},
            new double[] {0, 2, 4, 6, 6, 6, 4, 2, 0},
            new double[] {0, 0, 2, 6, 6, 6, 2, 0, 0},
        };

        private static double[][] TabelTuraAlbastra = new double[][]
        {
            new double[] {-2, 10, 6, 14, 12, 14, 6, 10, -2},
            new double[] {8, 4, 8, 16, 8, 16, 8, 4, 8},
            new double[] {4, 8, 6, 14, 12, 14, 6, 8, 4},
            new double[] {6, 10, 8, 14, 14, 14, 8, 10, 6},
            new double[] {12, 16, 14, 20, 20, 20, 14, 16, 12},
            new double[] {12, 14, 12, 18, 18, 18, 12, 14, 12},
            new double[] {12, 18, 16, 22, 22, 22, 16, 18, 12},
            new double[] {12, 12, 12, 18, 18, 18, 12, 12, 12},
            new double[] {16, 20, 18, 24, 26, 24, 18, 20, 16},
            new double[] {14, 14, 12, 18, 16, 18, 12, 14, 14}
        };
        private static double[][] TabelTuraAlba = new double[][]
        {
            new double[] {-14, -14, -16, -18, -20, -18, -14, -14, -14},
            new double[] {14, 12, 18, 18, 18, 12, 14, 14, 14},
            new double[] {12, 18, 24, 26, 24, 18, 16, 20, 16},
            new double[] {12, 12, 12, 18, 18, 18, 12, 12, 12},
            new double[] {12, 18, 16, 22, 22, 22, 16, 18, 12},
            new double[] {12, 14, 12, 18, 18, 18, 12, 14, 12},
            new double[] {12, 16, 14, 20, 20, 20, 14, 16, 12},
            new double[] {4, 8, 6, 14, 12, 14, 6, 8, 4},
            new double[] {-8, -4, -8, -16, -8, -16, -8, -4, -8},
            new double[] {2, -10, -6, -14, -12, -14, -6, -10, 2}
        };
        private static double[][] TabelGardianAlb = new double[][]
        {
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, -1, 0, -1, 0, 0, 0},
            new double[] {0, 0, 0, 0, 3, 0, 0, 0, 0},
            new double[] {0, 0, 0, 1, 0, 1, 0, 0, 0},
        };
        private static double[][] TabelGardianAlbastru = new double[][]
        {
            new double[] {0, 0, 0, 1, 0, 1, 0, 0, 0},
            new double[] {0, 0, 0, 0, 3, 0, 0, 0, 0},
            new double[] {0, 0, 0, -1, 0, -1, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
        };
        //
        private static double[][] TabelRegeAlb = new double[][]
        {
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, -2, -2, -2, 0, 0, 0},
            new double[] {0, 0, 0, -2, -2, -2, 0, 0, 0},
            new double[] {0, 0, 0, -2, 2, -2, 0, 0, 0},
        };
        private static double[][] TabelRegeAlbastru = new double[][]
        {
            new double[] {0, 0, 0, -2, 2, -2, 0, 0, 0},
            new double[] {0, 0, 0, -2, -2, -2, 0, 0, 0},
            new double[] {0, 0, 0, -2, -2, -2, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
        };
        //
        private static double[][] TabelElefantAlb = new double[][]
        {
            new double[] {0, 1, 0, 0, 0, -1, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, -1, 0, 0, 0, -1, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {-2, 0, 0, 0, 3, 0, 0, 0, -2 },
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, -1, 0, 1, 0, 0, 0}
        };
        private static double[][] TabelElefantAlbastru = new double[][]
        {
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, -1, 0, 0, 0, -1, 0, 0},
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {-2, 0, 0, 0, 3, 0, 0, 0, -2 },
            new double[] {0, 0, 0, 0, 0, 0, 0, 0, 0},
            new double[] {0, 0, 1, 0, 0, 0, 1, 0, 0}
        };
        private static List<double[][]> TabelPSTMoveOrdering = new List<double[][]>()
        {
            TabelNul,
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
            InitializeazaKillerMoves();
            InitializeazaHistoryHerusticis();
        }

        private void InitializeazaKillerMoves()
        {
            KillerMoves = new Mutare[MarimeKillerMoves + 1][];
            for (int i = 0; i <= MarimeKillerMoves; i++)
            {
                KillerMoves[i] = new Mutare[2];
            }
        }

        private static void InitializeazaHistoryHerusticis()
        {
            for (int i = 1; i <= 14; i++)
            {
                for (int linie = 0; linie < 10; linie++)
                {
                    for (int coloana = 0; coloana < 9; coloana++)
                    {
                        HistoryTable.Add((i, new Pozitie(linie, coloana)), 0);
                    }
                }
            }
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
        //200 + valoareLuata/10 - valoarePiesaCareCaptureaza/100 totul aproximat prin adaos(exceptie pion

        public static void AdaugaOpeningsInCache()
        {
            ulong hash;

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

        public static SortedList<double, Mutare> GenereazaMutariPosibile(int[][] matrice, Pozitie[] pozitiiPieseDeEvaluat, bool moveOrdering = true, int adancime = 0)
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
                        _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].Pozitie = poz;
                        List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].ReturneazaPozitiiPosibile(matrice);
                        foreach (Pozitie mut in mutari)
                        {
                            int piesaLuata = matrice[mut.Linie][mut.Coloana];
                            int piesaCareIa = matrice[poz.Linie][poz.Coloana];

                            if (piesaLuata == 0)
                            {
                                if (KillerMoves[adancime][0].PozitieInitiala == poz && KillerMoves[adancime][0].PozitieFinala == mut)
                                {
                                    mutPos.Add(OffsetKillerMoves, new(new(poz.Linie, poz.Coloana), mut));
                                }
                                else
                                if (KillerMoves[adancime][1].PozitieInitiala == poz && KillerMoves[adancime][1].PozitieFinala == mut)
                                {
                                    mutPos.Add(OffsetKillerMoves - 1, new(new(poz.Linie, poz.Coloana), mut));
                                }
                                else if (HistoryTable[(piesaCareIa, mut)] > 0)
                                {
                                    mutPos.Add(OffsetHistoryTable + HistoryTable[(piesaCareIa, mut)], new(new(poz.Linie, poz.Coloana), mut));
                                }
                                else
                                {
                                    mutPos.Add(TabelPSTMoveOrdering[piesaCareIa][mut.Linie][mut.Coloana], new(new(poz.Linie, poz.Coloana), mut));
                                }
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
                        List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].ReturneazaPozitiiPosibile(matrice);
                        foreach (Pozitie mut in mutari)
                        {
                            mutPos.Add(ct++, new(new(poz.Linie, poz.Coloana), mut));
                        }
                    }
                }
                return mutPos;
            }
        }



        public static SortedList<double, Mutare> GenereazaCapturiPosibile(int[][] matrice, Pozitie[] pozitiiPieseDeEvaluat)
        {
            SortedList<double, Mutare> mutPos;

            //most valuable victim, least valuable agressor
            //Piece-Square Tables

            mutPos = new(80, new DuplicateKeyComparerDesc<double>());
            foreach (Pozitie poz in pozitiiPieseDeEvaluat)
            {
                if (poz.Linie != -1)
                {
                    _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].Pozitie = poz;
                    List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Linie][poz.Coloana]].ReturneazaPozitiiPosibile(matrice);
                    foreach (Pozitie mut in mutari)
                    {
                        int piesaLuata = matrice[mut.Linie][mut.Coloana];
                        int piesaCareIa = matrice[poz.Linie][poz.Coloana];

                        if (piesaLuata != 0)
                        {
                            mutPos.Add(TabelCapturiPiese[piesaCareIa][piesaLuata], new(new(poz.Linie, poz.Coloana), mut));
                        }
                    }
                }
            }
            return mutPos;
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


        public static T[][] CloneazaMatricea<T>(T[][] arr)
        {
            int linie = arr.Length;
            T[][] arrClona = new T[linie][];

            for (int i = 0; i < linie; i++)
            {
                int coloana = arr[i].Length;
                arrClona[i] = new T[coloana];
                Array.Copy(arr[i], arrClona[i], coloana);
            }

            return arrClona;
        }

        public static bool AreJaggedArraysEqual<T>(T[][] arr1, T[][] arr2)
        {
            if (arr1.Length != arr2.Length)
                return false;

            for (int i = 0; i < arr1.Length; i++)
            {
                if (arr1[i].Length != arr2[i].Length)
                    return false;

                for (int j = 0; j < arr1[i].Length; j++)
                {
                    if (!EqualityComparer<T>.Default.Equals(arr1[i][j], arr2[i][j]))
                        return false;
                }
            }

            return true;
        }



        public static Pozitie ReturneazaPozitieRegeAlbastruInMatrice(int[][] matrice)
        {
            for (int i = 0; i <= 2;i++)
            {
                for(int j = 3;j<=5;j++)
                {
                    if (matrice[i][j] == regeAlbastru)
                        return new Pozitie(i, j);
                }
            }
            return new Pozitie(-1, -1);
        }
        public static Pozitie ReturneazaPozitieRegeAlbInMatrice(int[][] matrice)
        {
            for (int i = 7; i <= 9; i++)
            {
                for (int j = 3; j <= 5; j++)
                {
                    if (matrice[i][j] == regeAlb)
                        return new Pozitie(i, j);
                }
            }
            return new Pozitie(-1, -1);
        }

        public override Tuple<Mutare, double> ReturneazaMutareaOptima()
        {
            int[][] matriceClonata = CloneazaMatricea(_engine.MatriceCoduriPiese);
            ulong hashInitial = ZobristHash.HashuiesteTabla(matriceClonata);

            if (_engine.NrMutari <= 1)
            {
                if (CaceDeschideri.ContainsKey(hashInitial))
                {
                    (Pozitie, Pozitie) item = CaceDeschideri[hashInitial];
                    return new(new Mutare(item.Item1, item.Item2), double.MinValue);
                }
            }

            double scorMutare;
            int indexPiesaLuata,
                piesaCareIa,
                piesaLuata;
            ulong hashUpdatat;


            _cronometruAI.Start();



            Pozitie[] pozAlbastre = _engine.ReturneazaPozitiiAlbastre();
            Pozitie[] pozAlbe = _engine.ReturneazaPozitiiAlbe();

            //Debug.WriteLine("Poz Rege: "+ ReturneazaPozitieRegeAlbastruInMatrice(matriceClonata) + EsteSahLaAlbastru(matriceClonata, ReturneazaPozitieRegeAlbastruInMatrice(matriceClonata)));

            double evaluareMatriceInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese, pozAlbe, pozAlbastre);

            //AFISAREDEBUG EngineJoc.AfiseazaMatriceDebug(matriceClonata, 0, 0);
            //AFISAREDEBUG Debug.WriteLine("Evaluare initiala:"+evaluareMatriceInitiala);
            var mutariPosibile = GenereazaMutariPosibile(matriceClonata, pozAlbastre, moveOrdering: true, adancime: 0);

            if (mutariPosibile.Count == 0)
            {
                _engine.TerminaMeciul(TipSah.SahPersistentLaAlbastru);
            }
            SortedList<double, Mutare> listaAuxiliara = new(mutariPosibile.Count, new DuplicateKeyComparerDesc<double>());


            var valoriMutariPosibile = mutariPosibile.Values.ToList();

            int pozitieSchimbata;
            double valoareMutare;

            Mutare mutareOptima = valoriMutariPosibile[0];
            double scorMutareOptima = -ValoareMaxima;

            int adancimeMutareOptima = 0;
            piesaLuata = matriceClonata[mutareOptima.PozitieFinala.Linie][mutareOptima.PozitieFinala.Coloana];

            if (piesaLuata == (int)CodPiesa.RegeAlb)
            {
                return new(mutareOptima, ValoareMaxima);
            }


            double alpha = -ValoareMaxima;
            double beta = ValoareMaxima;

            NoduriEvaluate = 0;
            for (int adancimeIterativa = 1; adancimeIterativa <= _adancime; adancimeIterativa++)
            {
                SeteazaFereastraDeAspiratie(scorMutareOptima, ref alpha, ref beta, ref adancimeIterativa);

                foreach (var mutPos in valoriMutariPosibile)
                {
                    FaMutareaAlbastru(matriceClonata, hashInitial, pozAlbe, pozAlbastre, out hashUpdatat,
                        out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);

                    //AFISAREDEBUG Debug.WriteLine(EvalueazaMatricea(matriceClonata,pozAlbe,pozAlbastre)+" "+(evaluareMatriceInitiala + valoareMutare));

                    scorMutare = AlphaBetaCuMemorie(
                            evaluareMatriceInitiala + valoareMutare
                            , matriceClonata
                            , alpha
                            , beta
                            , adancimeIterativa
                            , piesaLuata
                            , hashUpdatat
                            , pozAlbe
                            , pozAlbastre
                    , Culoare.AlbMin);
                    listaAuxiliara.Add(scorMutare, mutPos);

                    RefaMutareaAlbastru(matriceClonata, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                    if (scorMutare > scorMutareOptima || adancimeIterativa > adancimeMutareOptima)
                    {
                        mutareOptima = mutPos;
                        scorMutareOptima = scorMutare;
                        adancimeMutareOptima = adancimeIterativa;
                    }
                    //Debug.WriteLine($"{mutPos} cu scor:{scorMutare} si adancime:{adancimeIterativa}");

                }//sf loop miscari
                valoriMutariPosibile.Clear();
                valoriMutariPosibile.AddRange(listaAuxiliara.Values);
                listaAuxiliara.Clear();
                Debug.WriteLine($"Noduri Evaluate: {NoduriEvaluate} la adancimea {adancimeIterativa}");

                adancimeIterativa = ScadeAdancimeaPentruFerestreAspiratie(scorMutareOptima, alpha, beta, adancimeIterativa);
            }
            Debug.WriteLine($"{evaluareMatriceInitiala}\n\n");
            _cronometruAI.Stop();
            _cronometruAI.Reset();
            return new(mutareOptima, scorMutareOptima);
        }

        private static int ScadeAdancimeaPentruFerestreAspiratie(double scorMutareOptima, double alpha, double beta, int adancimeIterativa)
        {
            if (scorMutareOptima > beta || scorMutareOptima < alpha)
            {
                adancimeIterativa--;
            }

            return adancimeIterativa;
        }

        private static void SeteazaFereastraDeAspiratie(double scorMutareOptima, ref double alpha, ref double beta, ref int adancimeIterativa)
        {
            if (FerestreAspiratie && adancimeIterativa >= 3)
            {
                if (scorMutareOptima > beta)
                    if (scorMutareOptima < alpha)
                    {
                    beta = ValoareMaxima;
                    adancimeIterativa--;
                }
                else
                if (scorMutareOptima < alpha)
                {
                    alpha = -ValoareMaxima;
                    adancimeIterativa--;
                }
                else
                {
                    alpha = scorMutareOptima - MarimeFereastraAspiratie;
                    beta = scorMutareOptima + MarimeFereastraAspiratie;
                }
            }
        }

        public static double EvalueazaMatricea(int[][] matrice, Pozitie[] pozAlbe, Pozitie[] pozAlbastre)
        {
            double material = 0;
            double pst = 0;

            //Debug.WriteLine("POZ ALBE");
            foreach (var poz in pozAlbe)
            {
                if (poz.Linie != -1)
                {
                    var piesa = matrice[poz.Linie][poz.Coloana];
                    material -= EngineJoc.ReturneazaScorPiesa(piesa);
                    pst -= TabelPSTMoveOrdering[piesa][poz.Linie][poz.Coloana];
                }
            }
            //Debug.WriteLine("POZ ALBASTRE");
            foreach (var poz in pozAlbastre)
            {
                if (poz.Linie != -1)
                {
                    //AFISAREDEBUG Debug.WriteLine(poz);
                    var piesa = matrice[poz.Linie][poz.Coloana];
                    material += EngineJoc.ReturneazaScorPiesa(piesa);
                    pst += TabelPSTMoveOrdering[piesa][poz.Linie][poz.Coloana];
                }
            }
            return material * ProcentajMaterial + pst * ProcentajPST;
        }

        public void AfiseazaVectorDebug(Pozitie[] vector)
        {
            foreach (Pozitie v in vector)
            {
                Debug.Write(v + " ");
            }
            Debug.WriteLine("");
        }


        public static bool EsteSahLaAlbastru(int[][] matrice, Pozitie pozRegeAlbastru)
        {
            //Sah La Pioni
            if (matrice[pozRegeAlbastru.Linie + 1][pozRegeAlbastru.Coloana] == pionAlb ||
                matrice[pozRegeAlbastru.Linie][pozRegeAlbastru.Coloana - 1] == pionAlb ||
                matrice[pozRegeAlbastru.Linie][pozRegeAlbastru.Coloana + 1] == pionAlb)
            {
                return true;
            }

            //Sah la tun si tura in linie +
            for (int linie = pozRegeAlbastru.Linie + 1; linie <= 9; linie++)
            {
                if (matrice[linie][pozRegeAlbastru.Coloana] == turaAlba)
                {
                    return true;
                }
                else if (matrice[linie][pozRegeAlbastru.Coloana] != 0)
                {
                    for (int linieTun = linie + 1; linieTun <= 9; linieTun++)
                    {
                        if (matrice[linieTun][pozRegeAlbastru.Coloana] != 0)
                        {
                            if (matrice[linieTun][pozRegeAlbastru.Coloana] == tunAlb)
                            {
                                return true;
                            }
                            else
                            {
                                goto SfarsitLinieP;
                            }
                        }
                    }
                }
            }
        SfarsitLinieP:
            //Sah la tun si tura in linie -
            for (int linie = pozRegeAlbastru.Linie - 1; linie >= 0; linie--)
            {
                if (matrice[linie][pozRegeAlbastru.Coloana] == turaAlba)
                {
                    return true;
                }
                else if (matrice[linie][pozRegeAlbastru.Coloana] != 0)
                {
                    for (int linieTun = linie - 1; linieTun >= 0; linieTun--)
                    {
                        if (matrice[linieTun][pozRegeAlbastru.Coloana] != 0)
                        {
                            if (matrice[linieTun][pozRegeAlbastru.Coloana] == tunAlb)
                            {
                                return true;
                            }
                            else
                            {
                                goto SfarsitLinieN;
                            }
                        }
                    }
                }
            }
        SfarsitLinieN:

            //Sah la tun si tura in coloana -
            for (int coloana = pozRegeAlbastru.Coloana - 1; coloana >= 0; coloana--)
            {
                if (matrice[pozRegeAlbastru.Linie][coloana] == turaAlba)
                {
                    return true;
                }
                else if (matrice[pozRegeAlbastru.Linie][coloana] != 0)
                {
                    for (int coloanaTun = coloana - 1; coloanaTun >= 0; coloanaTun--)
                    {
                        if (matrice[pozRegeAlbastru.Linie][coloanaTun] != 0)
                        {
                            if (matrice[pozRegeAlbastru.Linie][coloanaTun] == tunAlb)
                            {
                                return true;
                            }
                            else
                            {
                                goto SfarsitColoanaN;
                            }
                        }
                    }
                }
            }
        SfarsitColoanaN:

            //Sah la tun si tura in coloana +
            for (int coloana = pozRegeAlbastru.Coloana + 1; coloana <= 8; coloana++)
            {
                if (matrice[pozRegeAlbastru.Linie][coloana] == turaAlba)
                {
                    return true;
                }
                else if (matrice[pozRegeAlbastru.Linie][coloana] != 0)
                {
                    for (int coloanaTun = coloana + 1; coloanaTun <= 8;coloanaTun++)
                    {
                        if (matrice[pozRegeAlbastru.Linie][coloanaTun] != 0)
                        {
                            if (matrice[pozRegeAlbastru.Linie][coloanaTun] == tunAlb)
                            {
                                return true;
                            }
                            else
                            {
                                goto SfarsitColoanaP;
                            }
                        }
                    }
                }
            }
        SfarsitColoanaP:

            //Sah La cal
            if (matrice[pozRegeAlbastru.Linie][pozRegeAlbastru.Coloana - 1] == 0)
            {
                if (matrice[pozRegeAlbastru.Linie + 1][pozRegeAlbastru.Coloana - 2] == calAlb)
                {

                }

                if (pozRegeAlbastru.Linie > 1)
                {
                    if (matrice[pozRegeAlbastru.Linie - 1][pozRegeAlbastru.Coloana - 2] == calAlb)
                    {

                    }
                }
            }
            if (matrice[pozRegeAlbastru.Linie][pozRegeAlbastru.Coloana + 1] == 0)
            {
                if (matrice[pozRegeAlbastru.Linie + 1][pozRegeAlbastru.Coloana + 2] == calAlb)
                {

                }

                if (pozRegeAlbastru.Linie > 1)
                {
                    if (matrice[pozRegeAlbastru.Linie - 1][pozRegeAlbastru.Coloana + 2] == calAlb)
                    {

                    }
                }
            }
            if (pozRegeAlbastru.Linie > 2)
            {
                if (matrice[pozRegeAlbastru.Linie - 1][pozRegeAlbastru.Coloana] == 0)
                {
                    if (matrice[pozRegeAlbastru.Linie - 2][pozRegeAlbastru.Coloana + 1] == calAlb)
                    {

                    }
                    if (matrice[pozRegeAlbastru.Linie - 2][pozRegeAlbastru.Coloana - 1] == calAlb)
                    {

                    }
                }
            }
            if (matrice[pozRegeAlbastru.Linie + 1][pozRegeAlbastru.Coloana] == 0)
            {
                if (matrice[pozRegeAlbastru.Linie + 2][pozRegeAlbastru.Coloana + 1] == calAlb)
                {

                }
                if (matrice[pozRegeAlbastru.Linie + 2][pozRegeAlbastru.Coloana - 1] == calAlb)
                {

                }
            }

            return false;
        }
        public static bool EsteSahLaAlb(int[][] matrice, Pozitie pozRegeAlb)
        {
            //Sah La Pioni
            if (matrice[pozRegeAlb.Linie - 1][pozRegeAlb.Coloana] == pionAlbastru ||
                matrice[pozRegeAlb.Linie][pozRegeAlb.Coloana - 1] == pionAlbastru ||
                matrice[pozRegeAlb.Linie][pozRegeAlb.Coloana + 1] == pionAlbastru)
            {
                return true;
            }

            //Sah la tun si tura in linie +
            for (int linie = pozRegeAlb.Linie + 1; linie <= 9; linie++)
            {
                if (matrice[linie][pozRegeAlb.Coloana] == turaAlbastra)
                {
                    return true;
                }
                else if (matrice[linie][pozRegeAlb.Coloana] != 0)
                {
                    for (int linieTun = linie + 1; linieTun <= 9; linieTun++)
                    {
                        if (matrice[linieTun][pozRegeAlb.Coloana] != 0)
                        {
                            if (matrice[linieTun][pozRegeAlb.Coloana] == tunAlbastru)
                            {
                                return true;
                            }
                            else
                            {
                                goto SfarsitLinieP;
                            }
                        }
                    }
                }
            }
        SfarsitLinieP:
            //Sah la tun si tura in linie -
            for (int linie = pozRegeAlb.Linie - 1; linie >= 0; linie--)
            {
                if (matrice[linie][pozRegeAlb.Coloana] == turaAlbastra)
                {
                    return true;
                }
                else if (matrice[linie][pozRegeAlb.Coloana] != 0)
                {
                    for (int linieTun = linie - 1; linieTun >= 0; linieTun--)
                    {
                        if (matrice[linieTun][pozRegeAlb.Coloana] != 0)
                        {
                            if (matrice[linieTun][pozRegeAlb.Coloana] == tunAlbastru)
                            {
                                return true;
                            }
                            else
                            {
                                goto SfarsitLinieN;
                            }
                        }
                    }
                }
            }
        SfarsitLinieN:

            //Sah la tun si tura in coloana -
            for (int coloana = pozRegeAlb.Coloana - 1; coloana >= 0; coloana--)
            { 
                if (matrice[pozRegeAlb.Linie][coloana] == turaAlbastra)
                {
                    return true;
                }
                else if (matrice[pozRegeAlb.Linie][coloana] != 0)
                {
                    for (int coloanaTun = coloana - 1; coloanaTun >= 0; coloanaTun--)
                    {
                        if (matrice[pozRegeAlb.Linie][coloanaTun] != 0)
                        {
                            if (matrice[pozRegeAlb.Linie][coloanaTun] == tunAlbastru)
                            {
                                return true;
                            }
                            else
                            {
                                goto SfarsitColoanaN;
                            }
                        }
                    }
                }
            }
        SfarsitColoanaN:

            //Sah la tun si tura in coloana +
            for (int coloana = pozRegeAlb.Coloana + 1; coloana <= 8; coloana++)
            {
                if (matrice[pozRegeAlb.Linie][coloana] == turaAlbastra)
                {
                    return true;
                }
                else if (matrice[pozRegeAlb.Linie][coloana] != 0)
                {
                    for (int coloanaTun = coloana + 1; coloanaTun <= 8; coloanaTun++)
                    {
                        if (matrice[pozRegeAlb.Linie][coloanaTun] != 0)
                        {
                            if (
                            matrice[pozRegeAlb.Linie][coloanaTun] == tunAlbastru)
                            {
                                return true;
                            }
                            else
                            {
                                goto SfarsitColoanaP;
                            }
                        }
                    }
                }
            }
        SfarsitColoanaP:

            //Sah La cal
            if (matrice[pozRegeAlb.Linie][pozRegeAlb.Coloana - 1] == 0)
            {
                if (pozRegeAlb.Linie < 9)
                {
                    if (matrice[pozRegeAlb.Linie + 1][pozRegeAlb.Coloana - 2] == calAlbastru)
                    {

                    }
                }
                if (matrice[pozRegeAlb.Linie - 1][pozRegeAlb.Coloana - 2] == calAlbastru)
                {

                }
            }
            if (matrice[pozRegeAlb.Linie][pozRegeAlb.Coloana + 1] == 0)
            {
                if (pozRegeAlb.Linie < 9)
                {
                    if (matrice[pozRegeAlb.Linie + 1][pozRegeAlb.Coloana + 2] == calAlbastru)
                    {

                    }
                }
                if (matrice[pozRegeAlb.Linie - 1][pozRegeAlb.Coloana + 2] == calAlbastru)
                {

                }
            }
            if (matrice[pozRegeAlb.Linie - 1][pozRegeAlb.Coloana] == 0)
            {
                if (matrice[pozRegeAlb.Linie - 2][pozRegeAlb.Coloana + 1] == calAlbastru)
                {

                }
                if (matrice[pozRegeAlb.Linie - 2][pozRegeAlb.Coloana - 1] == calAlbastru)
                {

                }
            }
            if (pozRegeAlb.Linie < 9)
            {
                if (matrice[pozRegeAlb.Linie + 1][pozRegeAlb.Coloana] == 0)
                {
                    if (pozRegeAlb.Linie < 8)
                    {
                        if (matrice[pozRegeAlb.Linie + 2][pozRegeAlb.Coloana + 1] == calAlbastru)
                        {

                        }
                        if (matrice[pozRegeAlb.Linie + 2][pozRegeAlb.Coloana - 1] == calAlbastru)
                        {

                        }
                    }
                }
            }

            return false;
        }











        public static double AlphaBetaCuMemorie(double eval, int[][] matrice, double alpha,
            double beta, int adancime, int piesaCapturata, ulong hash,
            Pozitie[] pozAlbe, Pozitie[] pozAlbastre, Culoare culoare)
        {
            // pv search
            bool nodPV = false;
            NoduriEvaluate++;
            //tabel de transpozitie
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
                NoduriEvaluate--;
                //quiescence search
                var val = QSC(eval, matrice, alpha, beta, piesaCapturata, pozAlbe, pozAlbastre, culoare);


                //transposition table
                int flag = 0; // Exact value
                if (val <= alpha)
                {
                    flag = 2; // Upper bound
                }
                else if (val >= beta)
                {
                    flag = 1; // Lower bound
                }
                TabelTranspozitie.AdaugaIntrare(hash, val, 0, flag);
                return val;
            }

            if (piesaCapturata == regeAlbastru ||
                piesaCapturata == regeAlb)
            {
                //nod final
                return eval;
            }

            //maximizare => albastru
            if (culoare == Culoare.AlbastruMax)
            {

                bool esteSahLaAlbastru = EsteSahLaAlbastru(matrice, ReturneazaPozitieRegeAlbastruInMatrice(matrice));

                if (esteSahLaAlbastru)
                {
                    //check extension
                    adancime++;
                }
                else
                {
                    //null move pruning
                    if (adancime >= 3  && eval >= beta)
                    {
                        var evalCurenta = AlphaBetaCuMemorie(eval,
                                    matrice, eval, beta, adancime - 3,
                                    0, hash, pozAlbe, pozAlbastre, Culoare.AlbMin);
                        if (evalCurenta >= beta)
                            return evalCurenta;
                    }
                }

                var origAlpha = alpha;
                ulong hashUpdatat;
                int piesaLuata;
                int piesaCareIa;

                double val = -ValoareMaxima;


                SortedList<double, Mutare> mutariSortate = GenereazaMutariPosibile(matrice, pozAlbastre, moveOrdering: true, adancime: adancime);

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;

                foreach (Mutare mutPos in mutSortateValues)
                {
                    int indexPiesaLuata, pozitieSchimbata;
                    double valoareMutare;

                    FaMutareaAlbastru(matrice, hash, pozAlbe, pozAlbastre, out hashUpdatat, out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);

                    //principal variation search
                    if (nodPV == true)
                    {
                        val = Math.Max(val, AlphaBetaCuMemorie(eval + valoareMutare,
                                    matrice, alpha, alpha + 1, adancime - 1, piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbMin));

                        //outside bounds check
                        if (val > alpha && val < beta)
                        {
                            val = -ValoareMaxima;
                            val = Math.Max(val, AlphaBetaCuMemorie(eval + valoareMutare,
                                        matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbMin));
                        }
                    }
                    else
                    {
                        val = Math.Max(val, AlphaBetaCuMemorie(eval + valoareMutare,
                                    matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbMin));
                    }
                    //
                    alpha = Math.Max(val, alpha);
                    RefaMutareaAlbastru(matrice, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                    if (val >= beta)
                    {
                        if (piesaLuata == 0)
                        {
                            //killer moves
                            KillerMoves[adancime][1] = KillerMoves[adancime][0];
                            KillerMoves[adancime][0] = mutPos;
                            //hiistory heuristics
                            HistoryTable[(piesaCareIa, mutPos.PozitieFinala)] += (double) adancime * adancime;
                        }
                        goto ValoareFinala;
                    }
                    //principal variation check
                    if (eval > alpha && eval < beta)
                    {
                        nodPV = true;
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
            else// if (culoare == Culoare.AlbMin)
            {
                bool esteSahLaAlb = EsteSahLaAlb(matrice, ReturneazaPozitieRegeAlbInMatrice(matrice));

                if (esteSahLaAlb)
                {
                    //check extension
                    adancime++;
                }
                else
                {
                    //null move pruning
                    if(adancime >= 3 && alpha >= eval)
                    {
                        var evalCurenta = AlphaBetaCuMemorie(eval,
                                matrice, alpha, eval, adancime - 3,
                                0, hash, pozAlbe, pozAlbastre, Culoare.AlbastruMax);
                        if (alpha >= evalCurenta)
                            return evalCurenta;
                    }
                }

                var origBeta = beta;
                ulong hashUpdatat;
                int piesaLuata;
                int piesaCareIa;

                double val = ValoareMaxima;

                SortedList<double, Mutare> mutariSortate = GenereazaMutariPosibile(matrice, pozAlbe, moveOrdering: true, adancime: adancime);

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;
                foreach (Mutare mutPos in mutSortateValues)
                {
                    int indexPiesaLuata, pozitieSchimbata;
                    double valoareMutare;

                    FaMutareaAlb(matrice, hash, pozAlbe, pozAlbastre, out hashUpdatat, out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);
                    
                    //principal variation search
                    if (nodPV == true)
                    {
                        val = Math.Min(val, AlphaBetaCuMemorie(eval - valoareMutare,
                            matrice, beta - 1, beta, adancime - 1,
                            piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbastruMax));

                        if (val > alpha && val < beta)
                        {
                            val = ValoareMaxima;
                            val = Math.Min(val, AlphaBetaCuMemorie(eval - valoareMutare,
                                matrice, alpha, beta, adancime - 1,
                                piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbastruMax));
                        }
                    }
                    else
                    {
                        val = Math.Min(val, AlphaBetaCuMemorie(eval - valoareMutare,
                            matrice, alpha, beta, adancime - 1,
                            piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbastruMax));

                    }
                    //
                    beta = Math.Min(val, beta);
                    RefaMutareaAlb(matrice, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                    if (val <= alpha)
                    {
                        if (piesaLuata == 0)
                        {
                            //killer moves
                            KillerMoves[adancime][1] = KillerMoves[adancime][0];
                            KillerMoves[adancime][0] = mutPos;
                            //history heuristics
                            HistoryTable[(piesaCareIa, mutPos.PozitieFinala)] = (double)adancime * adancime ;
                        }
                        goto ValoareFinala;
                    }
                    //principal variation check
                    if (val > alpha && val < origBeta)
                    {
                        nodPV = true;
                    }
                }
            ValoareFinala:

                //transposition table
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
















        public static double QSC(double eval, int[][] matrice, double alpha,
            double beta, int piesaCapturata,
            Pozitie[] pozAlbe, Pozitie[] pozAlbastre, Culoare culoare)
        {
            NoduriEvaluate++;
            if (piesaCapturata == (int)CodPiesa.RegeAlb ||
                piesaCapturata == (int)CodPiesa.RegeAlbastru
                )
            {
                return eval;
            }
            if (culoare == Culoare.AlbastruMax)
            {
                bool esteSahLaAlbastru = EsteSahLaAlbastru(matrice, ReturneazaPozitieRegeAlbastruInMatrice(matrice));

                if (eval >= beta && !esteSahLaAlbastru)
                    return eval;
                //delta pruning
                if (eval + ConstantaPiese.ValoareTura + 300 < alpha)
                {
                    return eval;
                }


                var origAlpha = alpha;
                int piesaLuata;
                int piesaCareIa;

                double val = -ValoareMaxima;



                SortedList<double, Mutare> mutariSortate;
                if (esteSahLaAlbastru)
                {
                    mutariSortate = GenereazaMutariPosibile(matrice, pozAlbastre);
                }
                else
                {
                    mutariSortate = GenereazaCapturiPosibile(matrice, pozAlbastre);
                }

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;

                foreach (Mutare mutPos in mutSortateValues)
                {
                    int indexPiesaLuata, pozitieSchimbata;
                    double valoareMutare;

                    FaMutareaAlbastruQSC(matrice, pozAlbe, pozAlbastre, out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);

                    val = Math.Max(val, QSC(eval + valoareMutare,
                                matrice, alpha, beta, piesaLuata, pozAlbe, pozAlbastre, Culoare.AlbMin));
                    alpha = Math.Max(val, alpha);

                    RefaMutareaAlbastru(matrice, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                    if (val >= beta)
                    {
                        goto ValoareFinala;
                    }
                }
            ValoareFinala:

                return val;
            }
            else// if (culoare == Culoare.AlbMin)
            {
                bool esteSahLaAlb = EsteSahLaAlb(matrice, ReturneazaPozitieRegeAlbInMatrice(matrice));

                if (alpha >= eval && !esteSahLaAlb)
                    return eval;
                //delta pruning
                if (eval - ConstantaPiese.ValoareTura - 300 > beta)
                {
                    return eval;
                }

                var origBeta = beta;
                int piesaLuata;
                int piesaCareIa;

                double val = ValoareMaxima;



                SortedList<double, Mutare> mutariSortate;
                if (esteSahLaAlb)
                {
                    mutariSortate = GenereazaMutariPosibile(matrice, pozAlbe);
                }
                else
                {
                    mutariSortate = GenereazaCapturiPosibile(matrice, pozAlbe);
                }

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;
                foreach (Mutare mutPos in mutSortateValues)
                {
                    int indexPiesaLuata, pozitieSchimbata;
                    double valoareMutare;

                    FaMutareaAlbQSC(matrice, pozAlbe, pozAlbastre, out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);

                    val = Math.Min(val, QSC(eval - valoareMutare,
                        matrice, alpha, beta,
                        piesaLuata, pozAlbe, pozAlbastre, Culoare.AlbastruMax));

                    beta = Math.Min(val, beta);

                    RefaMutareaAlb(matrice, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                    if (val <= alpha)
                    {
                        goto ValoareFinala;
                    }
                }
            ValoareFinala:
                return val;
            }
        }





        private static void FaMutareaAlbQSC(int[][] matrice, Pozitie[] pozAlbe, Pozitie[] pozAlbastre, out int piesaLuata, out int piesaCareIa, Mutare mutPos, out int indexPiesaLuata, out int pozitieSchimbata, out double valoareMutare)
        {
            piesaLuata = matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana];
            piesaCareIa = matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana];

            matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana] = (int)CodPiesa.Gol;
            matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana] = piesaCareIa;

            //alb ia albastru
            indexPiesaLuata = -1;
            if (EstePiesa(piesaLuata))
            {
                indexPiesaLuata = StergePozitieDinVector(mutPos.PozitieFinala,
                            pozAlbastre);
            }
            pozitieSchimbata = SchimbaPozitiaDinVector(mutPos.PozitieInitiala, pozAlbe, mutPos.PozitieFinala);


            valoareMutare = EngineJoc.ReturneazaScorPiesa(piesaLuata) * ProcentajMaterial
                + ProcentajPST * (
                    -TabelPSTMoveOrdering[piesaLuata][mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana]
                    - TabelPSTMoveOrdering[piesaCareIa][mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana]
                    + TabelPSTMoveOrdering[piesaCareIa][mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana]);
        }


        private static void FaMutareaAlbastruQSC(int[][] matrice, Pozitie[] pozAlbe, Pozitie[] pozAlbastre, out int piesaLuata, out int piesaCareIa, Mutare mutPos, out int indexPiesaLuata, out int pozitieSchimbata, out double valoareMutare)
        {
            piesaLuata = matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana];
            piesaCareIa = matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana];

            matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana] = (int)CodPiesa.Gol;
            matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana] = piesaCareIa;

            indexPiesaLuata = -1;
            //albastru ia alb
            if (EstePiesa(piesaLuata))
            {
                indexPiesaLuata = StergePozitieDinVector(mutPos.PozitieFinala,
                            pozAlbe);
            }
            pozitieSchimbata = SchimbaPozitiaDinVector(mutPos.PozitieInitiala, pozAlbastre, mutPos.PozitieFinala);


            valoareMutare = EngineJoc.ReturneazaScorPiesa(piesaLuata) * ProcentajMaterial
                + ProcentajPST * (
                    -TabelPSTMoveOrdering[piesaLuata][mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana]
                    - TabelPSTMoveOrdering[piesaCareIa][mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana]
                    + TabelPSTMoveOrdering[piesaCareIa][mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana]);
        }

        private static void FaMutareaAlb(int[][] matrice, ulong hash, Pozitie[] pozAlbe, Pozitie[] pozAlbastre, out ulong hashUpdatat, out int piesaLuata, out int piesaCareIa, Mutare mutPos, out int indexPiesaLuata, out int pozitieSchimbata, out double valoareMutare)
        {
            piesaLuata = matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana];
            piesaCareIa = matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana];

            matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana] = (int)CodPiesa.Gol;
            matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana] = piesaCareIa;

            hashUpdatat = ZobristHash.UpdateazaHash(
                hashInitial: hash,
                linieInitiala: mutPos.PozitieInitiala.Linie,
                coloanaInitiala: mutPos.PozitieInitiala.Coloana,
                piesaLuata: piesaLuata,
                linieFinala: mutPos.PozitieFinala.Linie,
                coloanaFinala: mutPos.PozitieFinala.Coloana,
                piesaCareIa: piesaCareIa);

            //alb ia albastru
            indexPiesaLuata = -1;
            if (EstePiesa(piesaLuata))
            {
                indexPiesaLuata = StergePozitieDinVector(mutPos.PozitieFinala,
                            pozAlbastre);
            }
            pozitieSchimbata = SchimbaPozitiaDinVector(mutPos.PozitieInitiala, pozAlbe, mutPos.PozitieFinala);


            valoareMutare = EngineJoc.ReturneazaScorPiesa(piesaLuata) * ProcentajMaterial
                + ProcentajPST * (
                    -TabelPSTMoveOrdering[piesaLuata][mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana]
                    - TabelPSTMoveOrdering[piesaCareIa][mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana]
                    + TabelPSTMoveOrdering[piesaCareIa][mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana]);
        }


        private static void FaMutareaAlbastru(int[][] matrice, ulong hash, Pozitie[] pozAlbe, Pozitie[] pozAlbastre, out ulong hashUpdatat, out int piesaLuata, out int piesaCareIa, Mutare mutPos, out int indexPiesaLuata, out int pozitieSchimbata, out double valoareMutare)
        {
            piesaLuata = matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana];
            piesaCareIa = matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana];

            matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana] = (int)CodPiesa.Gol;
            matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana] = piesaCareIa;

            hashUpdatat = ZobristHash.UpdateazaHash(
                hashInitial: hash,
                linieInitiala: mutPos.PozitieInitiala.Linie,
                coloanaInitiala: mutPos.PozitieInitiala.Coloana,
                piesaLuata: piesaLuata,
                linieFinala: mutPos.PozitieFinala.Linie,
                coloanaFinala: mutPos.PozitieFinala.Coloana,
                piesaCareIa: piesaCareIa);

            indexPiesaLuata = -1;
            //albastru ia alb
            if (EstePiesa(piesaLuata))
            {
                indexPiesaLuata = StergePozitieDinVector(mutPos.PozitieFinala,
                            pozAlbe);
            }
            pozitieSchimbata = SchimbaPozitiaDinVector(mutPos.PozitieInitiala, pozAlbastre, mutPos.PozitieFinala);


            valoareMutare = EngineJoc.ReturneazaScorPiesa(piesaLuata) * ProcentajMaterial
                + ProcentajPST * (
                    -TabelPSTMoveOrdering[piesaLuata][mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana]
                    - TabelPSTMoveOrdering[piesaCareIa][mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana]
                    + TabelPSTMoveOrdering[piesaCareIa][mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana]);
        }



        private static void RefaMutareaAlb(int[][] matrice, Pozitie[] pozAlbe, Pozitie[] pozAlbastre, int piesaLuata, int piesaCareIa, Mutare mutPos, int indexPiesaLuata, int pozitieSchimbata)
        {
            if (EstePiesa(piesaLuata))
            {
                AdaugaPozitieInVector(indexPiesaLuata, pozAlbastre,
                           mutPos.PozitieFinala);
            }

            SchimbaPozitiaDinVector(pozitieSchimbata, pozAlbe, mutPos.PozitieInitiala);

            matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana] = piesaCareIa;
            matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana] = piesaLuata;
        }

        private static void RefaMutareaAlbastru(int[][] matrice, Pozitie[] pozAlbe, Pozitie[] pozAlbastre, int piesaLuata, int piesaCareIa, Mutare mutPos, int indexPiesaLuata, int pozitieSchimbata)
        {
            SchimbaPozitiaDinVector(pozitieSchimbata, pozAlbastre, mutPos.PozitieInitiala);

            if (EstePiesa(piesaLuata))
            {
                AdaugaPozitieInVector(indexPiesaLuata, pozAlbe,
                            mutPos.PozitieFinala);
            }

            matrice[mutPos.PozitieInitiala.Linie][mutPos.PozitieInitiala.Coloana] = piesaCareIa;
            matrice[mutPos.PozitieFinala.Linie][mutPos.PozitieFinala.Coloana] = piesaLuata;
        }

        private static bool EstePiesa(int codPiesa)
        {
            if (codPiesa == 0)
            {
                return false;
            }

            return true;
        }

















        public static double MiniMax(double eval, int[][] matrice, int adancime, int piesaCapturata, ulong hash,
            Pozitie[] pozAlbe, Pozitie[] pozAlbastre, Culoare culoare)
        {
            // pv search

            NoduriEvaluate++;

            if (adancime <= 0)
            {
                NoduriEvaluate--;
                //quiescence search
                return eval;
            }

            if (piesaCapturata == regeAlbastru ||
                piesaCapturata == regeAlb)
            {
                //nod final
                return eval;
            }

            //maximizare => albastru
            if (culoare == Culoare.AlbastruMax)
            {
                ulong hashUpdatat;
                int piesaLuata;
                int piesaCareIa;

                double val = -ValoareMaxima;


                SortedList<double, Mutare> mutariSortate = GenereazaMutariPosibile(matrice, pozAlbastre, moveOrdering: true, adancime: adancime);

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;

                foreach (Mutare mutPos in mutSortateValues)
                {
                    int indexPiesaLuata, pozitieSchimbata;
                    double valoareMutare;

                    FaMutareaAlbastru(matrice, hash, pozAlbe, pozAlbastre, out hashUpdatat, out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);

                    val = Math.Max(val, MiniMax(eval + valoareMutare,
                                matrice, adancime - 1, piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbMin));

                    //
                    RefaMutareaAlbastru(matrice, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                }

                return val;
            }
            else// if (culoare == Culoare.AlbMin)
            {

                ulong hashUpdatat;
                int piesaLuata;
                int piesaCareIa;

                double val = ValoareMaxima;

                SortedList<double, Mutare> mutariSortate = GenereazaMutariPosibile(matrice, pozAlbe, moveOrdering: true, adancime: adancime);

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;
                foreach (Mutare mutPos in mutSortateValues)
                {
                    int indexPiesaLuata, pozitieSchimbata;
                    double valoareMutare;

                    FaMutareaAlb(matrice, hash, pozAlbe, pozAlbastre, out hashUpdatat, out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);

                    val = Math.Min(val, MiniMax(eval - valoareMutare,
                        matrice, adancime - 1,
                        piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbastruMax));


                    //
                    RefaMutareaAlb(matrice, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                }
                return val;
            }
        }



        public static double AlphaBeta(double eval, int[][] matrice, double alpha,
            double beta, int adancime, int piesaCapturata, ulong hash,
            Pozitie[] pozAlbe, Pozitie[] pozAlbastre, Culoare culoare)
        {
            // pv search

            NoduriEvaluate++;

            if (adancime <= 0)
            {
                NoduriEvaluate--;
                //quiescence search
                return eval;
            }

            if (piesaCapturata == regeAlbastru ||
                piesaCapturata == regeAlb)
            {
                //nod final
                return eval;
            }

            //maximizare => albastru
            if (culoare == Culoare.AlbastruMax)
            {


                var origAlpha = alpha;
                ulong hashUpdatat;
                int piesaLuata;
                int piesaCareIa;

                double val = -ValoareMaxima;


                SortedList<double, Mutare> mutariSortate = GenereazaMutariPosibile(matrice, pozAlbastre, moveOrdering: true, adancime: adancime);

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;

                foreach (Mutare mutPos in mutSortateValues)
                {
                    int indexPiesaLuata, pozitieSchimbata;
                    double valoareMutare;

                    FaMutareaAlbastru(matrice, hash, pozAlbe, pozAlbastre, out hashUpdatat, out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);

                    val = Math.Max(val, AlphaBeta(eval + valoareMutare,
                                matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbMin));

                    //
                    alpha = Math.Max(val, alpha);
                    RefaMutareaAlbastru(matrice, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                    if (val >= beta)
                    {
                        goto ValoareFinala;
                    }
                }
            ValoareFinala:


                return val;
            }
            else// if (culoare == Culoare.AlbMin)
            {

                var origBeta = beta;
                ulong hashUpdatat;
                int piesaLuata;
                int piesaCareIa;

                double val = ValoareMaxima;

                SortedList<double, Mutare> mutariSortate = GenereazaMutariPosibile(matrice, pozAlbe, moveOrdering: true, adancime: adancime);

                if (mutariSortate.Count == 0)
                    return eval;

                var mutSortateValues = mutariSortate.Values;
                foreach (Mutare mutPos in mutSortateValues)
                {
                    int indexPiesaLuata, pozitieSchimbata;
                    double valoareMutare;

                    FaMutareaAlb(matrice, hash, pozAlbe, pozAlbastre, out hashUpdatat, out piesaLuata, out piesaCareIa, mutPos, out indexPiesaLuata, out pozitieSchimbata, out valoareMutare);

                    val = Math.Min(val, AlphaBeta(eval - valoareMutare,
                        matrice, alpha, beta, adancime - 1,
                        piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbastruMax));

                    
                    //
                    beta = Math.Min(val, beta);
                    RefaMutareaAlb(matrice, pozAlbe, pozAlbastre, piesaLuata, piesaCareIa, mutPos, indexPiesaLuata, pozitieSchimbata);

                    if (val <= alpha)
                    {
                        goto ValoareFinala;
                    }
                }
            ValoareFinala:
                return val;
            }
        }






    }
}




















