using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;

namespace ProiectVolovici
{
	public class MiniMaxAI : Jucator
	{
		private static TabelTranspozitie TabelTranspozitie = new(300);
        private static Dictionary<long, (Pozitie, Pozitie)> CaceDeschideri = new();
        private static Random GeneratorRandom = new();

		private EngineMiniMax _engine;
		private int _adancime;
		private Stopwatch _cronometruAI = new Stopwatch();

		private Stopwatch _cronometru= new();

		public Stopwatch CronometruAI
		{
			get { return _cronometru; }
			set { _cronometru = value;  }
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


        public MiniMaxAI(Culoare culoare, EngineMiniMax engine, int adancime = ConstantaTabla.Adancime) : base(culoare)
		{
			_engine = engine;
			_culoare = culoare;
			_adancime = adancime;
			AdaugaOpeningsInCache();
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

		public static SortedList<double, Tuple<Pozitie, Pozitie>> CalculeazaCapturiPosibile(int[][] matrice, (int, int)[] pozitiiPieseDeEvaluat)
		{
			SortedList<double, Tuple<Pozitie, Pozitie>> capPos;

			capPos = new(18, new DuplicateKeyComparerDesc<double>());
			foreach (var poz in pozitiiPieseDeEvaluat)
			{
				if (poz.Item1 != -1)
				{
					_pieseVirtuale[matrice[poz.Item1][poz.Item2]].Pozitie = new Pozitie(poz.Item1, poz.Item2);
					List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Item1][poz.Item2]].ReturneazaMutariPosibile(matrice);
					foreach (Pozitie mut in mutari)
					{
						int piesaLuata = matrice[mut.Linie][mut.Coloana];
						int piesaCareIa = matrice[poz.Item1][poz.Item2];

						if(piesaLuata != 0)
						{
							capPos.Add(-EngineJoc.ReturneazaScorPiesa(piesaCareIa)
								+ -EngineJoc.ReturneazaScorPiesa(piesaLuata)
								,
								new(new(poz.Item1, poz.Item2), mut));
						}
					}
				}
			}
			return capPos;
		}


        public static SortedList<double, Tuple<Pozitie, Pozitie>> CalculeazaMutariPosibile(int[][] matrice, (int, int)[] pozitiiPieseDeEvaluat, bool moveOrdering = false)
        {
            SortedList<double, Tuple<Pozitie, Pozitie>> mutPos;

			//most valuable victim, least valuable agressor
			//Piece-Square Tables

            if (moveOrdering == true)
            {
                mutPos = new(80, new DuplicateKeyComparerDesc<double>());
				foreach (var poz in pozitiiPieseDeEvaluat)
				{
					if (poz.Item1 != -1)
					{
						_pieseVirtuale[matrice[poz.Item1][poz.Item2]].Pozitie = new Pozitie(poz.Item1, poz.Item2);
						List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Item1][poz.Item2]].ReturneazaMutariPosibile(matrice);
						foreach (Pozitie mut in mutari)
						{
							int piesaLuata = matrice[mut.Linie][mut.Coloana];
							int piesaCareIa = matrice[poz.Item1][poz.Item2];

							if (piesaLuata == 0)
							{
								switch (piesaCareIa)
								{
                                    case pionAlb: 
										{
                                            mutPos.Add(TabelPionAlb[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break; 
										}
                                    case pionAlbastru:
                                        {
                                            mutPos.Add(TabelPionAlbastru[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case tunAlb:
                                        {
                                            mutPos.Add(TabelTunAlb[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case tunAlbastru:
                                        {
                                            mutPos.Add(TabelTunAlbastru[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case turaAlba:
                                        {
                                            mutPos.Add(TabelTuraAlba[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case turaAlbastra:
                                        {
                                            mutPos.Add(TabelTuraAlbastra[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case calAlb:
                                        {
                                            mutPos.Add(TabelCalAlb[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case calAlbastru:
                                        {
                                            mutPos.Add(TabelCalAlbastru[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case gardianAlb:
                                        {
                                            mutPos.Add(TabelGardianAlb[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case gardianAlbastru:
                                        {
                                            mutPos.Add(TabelGardianAlbastru[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case elefantAlb:
                                        {
                                            mutPos.Add(TabelElefantAlb[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case elefantAlbastru:
                                        {
                                            mutPos.Add(TabelElefantAlbastru[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case regeAlb:
                                        {
                                            mutPos.Add(TabelRegeAlb[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                    case regeAlbastru:
                                        {
                                            mutPos.Add(TabelRegeAlbastru[mut.Linie][mut.Coloana], new(new(poz.Item1, poz.Item2), mut));
                                            break;
                                        }
                                }
							}
							else
							{
								mutPos.Add(-EngineJoc.ReturneazaScorPiesa(piesaCareIa)
									+ -EngineJoc.ReturneazaScorPiesa(piesaLuata)
                                    ,
									new(new(poz.Item1, poz.Item2), mut));
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
                foreach (var poz in pozitiiPieseDeEvaluat)
                {
                    if (poz.Item1 != -1)
                    {
                        _pieseVirtuale[matrice[poz.Item1][poz.Item2]].Pozitie = new Pozitie(poz.Item1, poz.Item2);
                        List<Pozitie> mutari = _pieseVirtuale[matrice[poz.Item1][poz.Item2]].ReturneazaMutariPosibile(matrice);
                        foreach (Pozitie mut in mutari)
                        {
                            mutPos.Add(ct++, new(new(poz.Item1, poz.Item2), mut));
                        }
                    }
                }
                return mutPos;
            }
        }

        public (int, int)[] ReturneazaPozitiiAlbe()
		{
			(int, int)[] pozitii = new (int, int)[_engine.ListaPieseAlbe.Count];
			int ct = 0;
			foreach (var piesa in _engine.ListaPieseAlbe)
			{
				pozitii[ct] = (piesa.Pozitie.Linie, piesa.Pozitie.Coloana);
				ct++;
			}
			return pozitii;
		}

		public (int, int)[] ReturneazaPozitiiAlbastre()
		{
			(int, int)[] pozitii = new (int, int)[_engine.ListaPieseAlbastre.Count];
			int ct = 0;
			foreach (var piesa in _engine.ListaPieseAlbastre)
			{
				pozitii[ct] = (piesa.Pozitie.Linie, piesa.Pozitie.Coloana);
				ct++;
			}
			return pozitii;
		}

		public static void StergePozitieDinVector(int index, (int, int)[] vector)
		{
			vector[index] = (-1, -1);
		}

		public static int StergePozitieDinVector((int, int) poz, (int, int)[] vector)
		{
			for (int i = 0; i < vector.Length; i++)
			{
				if (vector[i].Item1 == poz.Item1 && vector[i].Item2 == poz.Item2)
				{
					vector[i] = (-1, -1);
					return i;
				}
			}
			return -1;
		}

		public static int SchimbaPozitiaDinVector(int index,
											(int, int)[] vector,
											(int, int) pozFinala)
		{
			vector[index] = pozFinala;
			return index;
		}

		public static int SchimbaPozitiaDinVector((int, int) pozInitiala,
											(int, int)[] vector,
											(int, int) pozFinala)
		{
			for (int i = 0; i < vector.Length; i++)
			{
				if (vector[i].Item1 == pozInitiala.Item1 && vector[i].Item2 == pozInitiala.Item2)
				{
					vector[i] = pozFinala;
					return i;
				}
			}
			return -1;
		}

		public static int SchimbaPozitiaDinVector((int, int)[] vector,
											(int, int) pozFinala,
											int index)
		{
			vector[index] = pozFinala;
			return index;
		}

		public static int AdaugaPoitieInVector((int, int)[] vector,
											(int, int) pozFinala)
		{
			for (int i = 0; i < vector.Length; i++)
			{
				if (vector[i].Item1 == -1)
				{
					vector[i] = pozFinala;
					return i;
				}
			}
			return -1;
		}

		public static int AdaugaPoitieInVector(int i, (int, int)[] vector,
											(int, int) pozFinala)
		{
			vector[i] = pozFinala;
			return i;
		}
        public static double MTDF(int[][] matrice, double f, int depth, double eval, int codPiesaLuata,
			long hashUpdatat, (int, int)[] pozitiiAlbe, (int, int)[] pozitiiAlbastre, Culoare culoare)
        {
            var g = f;
            double upperBound = double.MaxValue;
            double lowerBound = double.MinValue;

            while (lowerBound < upperBound)
            {
                double beta = (g == lowerBound) ? g + 1 : g;


                g = AlphaBetaCuMemorie(
                        eval,
                        matrice,
						beta-1, 
						beta, 
						depth,
						codPiesaLuata, 
						hashUpdatat,
						pozitiiAlbe, 
						pozitiiAlbastre,
						culoare);


                if (g < beta)
                    upperBound = g;
                else
                    lowerBound = g;
            }
            return g;
        }


        public Tuple<Tuple<Pozitie, Pozitie>, double> EvalueazaPozitile(SortedList<double, Tuple<Pozitie, Pozitie>> mutPos, int adancimeCeruta)
		{
			_cronometruAI.Start();

            int[][] matriceInitiala = _engine.MatriceCoduriPiese.Clone() as int[][];
			long hashInitial = ZobristHash.HashuiesteTabla(matriceInitiala);

			if (_engine.NrMutari <= 1)
			{
				if (CaceDeschideri.ContainsKey(hashInitial))
				{
					(Pozitie, Pozitie) item = CaceDeschideri[hashInitial];
					return new(new Tuple<Pozitie, Pozitie>(item.Item1, item.Item2), double.MinValue);
				}
			}

            double evaluareMatriceInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
			Tuple<Pozitie, Pozitie> mutareOptima = mutPos.Values[0];

            int codPiesaLuata = _engine.MatriceCoduriPiese[
                mutPos.Values[0].Item2.Linie][
                mutPos.Values[0].Item2.Coloana];

            double scorMutareOptima = evaluareMatriceInitiala + EngineJoc.ReturneazaScorPiesa(codPiesaLuata) ;
            double scorMutare = evaluareMatriceInitiala + EngineJoc.ReturneazaScorPiesa(codPiesaLuata);


			int indexPozLuata,
                indexPozCareIa,
                codPiesaCareIa;

			long hashUpdatat;

			var pozitiiAlbastre = ReturneazaPozitiiAlbastre();
			var pozitiiAlbe = ReturneazaPozitiiAlbe();


			for (int adancimeTemp = 1; adancimeTemp <= adancimeCeruta &&
				CronometruAI.ElapsedMilliseconds < 6000; adancimeTemp++)
			{
				for (int i = 0; i < mutPos.Count; i++)
				{
					codPiesaLuata = _engine.MatriceCoduriPiese[
						mutPos.Values[i].Item2.Linie][
						mutPos.Values[i].Item2.Coloana];

					codPiesaCareIa = _engine.MatriceCoduriPiese[
						mutPos.Values[i].Item1.Linie][
						mutPos.Values[i].Item1.Coloana];

					matriceInitiala[
						mutPos.Values[i].Item1.Linie][
						mutPos.Values[i].Item1.Coloana] = 0;

					matriceInitiala[
						mutPos.Values[i].Item2.Linie][
						mutPos.Values[i].Item2.Coloana] = codPiesaCareIa;

					hashUpdatat = ZobristHash.UpdateazaHash(
						hashInitial: hashInitial,
						linieInitiala: mutPos.Values[0].Item1.Linie,
						coloanaInitiala: mutPos.Values[0].Item1.Coloana,
						piesaLuata: codPiesaLuata,
						linieFinala: mutPos.Values[0].Item2.Linie,
						coloanaFinala: mutPos.Values[0].Item2.Coloana,
						piesaCareIa: codPiesaCareIa);

					indexPozCareIa = SchimbaPozitiaDinVector(
						(mutPos.Values[i].Item1.Linie, mutPos.Values[i].Item1.Coloana),
						pozitiiAlbastre,
						(mutPos.Values[i].Item2.Linie, mutPos.Values[i].Item2.Coloana));

					indexPozLuata = -1;
					if (EstePiesa(codPiesaLuata))
					{
						indexPozLuata = StergePozitieDinVector((mutPos.Values[i].Item2.Linie, mutPos.Values[i].Item2.Coloana),
												pozitiiAlbe);
					}

					//piesa albastra ia alba => maximizare
					scorMutare = MTDF(matriceInitiala,
                        0,
						adancimeTemp,
						evaluareMatriceInitiala + EngineJoc.ReturneazaScorPiesa(codPiesaLuata)
                        , codPiesaLuata
						, hashUpdatat
                        , pozitiiAlbe
						, pozitiiAlbastre
						, Culoare.AlbMin);


					if (scorMutare > scorMutareOptima)
					{
						mutareOptima = mutPos.Values[i];
						scorMutareOptima = scorMutare;
					}


					SchimbaPozitiaDinVector(indexPozCareIa,
						pozitiiAlbastre,
						(mutPos.Values[i].Item1.Linie, mutPos.Values[i].Item1.Coloana));

					if (EstePiesa(codPiesaLuata))
					{
						AdaugaPoitieInVector(indexPozLuata, pozitiiAlbe, (mutPos.Values[i].Item2.Linie, mutPos.Values[i].Item2.Coloana));
					}

					matriceInitiala[
						mutPos.Values[i].Item1.Linie][
						mutPos.Values[i].Item1.Coloana] = codPiesaCareIa;

					matriceInitiala[
						mutPos.Values[i].Item2.Linie][
						mutPos.Values[i].Item2.Coloana] = codPiesaLuata;
				}
			}
			_cronometruAI.Stop();
			_cronometruAI.Reset();
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
						var valoarePiesaLuata = EngineJoc.ReturneazaScorPiesa(matrice[linie][coloana]);
						scor += valoarePiesaLuata;
					}
					//alb
					else
					{
						var valoarePiesaLuata = EngineJoc.ReturneazaScorPiesa(matrice[linie][coloana]);
						scor -= valoarePiesaLuata;
					}
				}
			}
			return scor;
		}

		public void AfiseazaVectorDebug((int, int)[] vector)
		{
			foreach (var v in vector)
			{
				Debug.Write(v + " ");
			}
			Debug.WriteLine("");
		}


		public static double AlphaBetaCuMemorie(double eval, int[][] matrice, double alpha,
			double beta, int adancime, int piesaCapturata, long hash,
			(int, int)[] pozAlbe, (int,int)[] pozAlbastre, Culoare culoare)
		{

            if ( adancime == 0 )
			{
				return eval;
			}
			if(piesaCapturata == (int)CodPiesa.RegeAlbastru ||
                piesaCapturata == (int)CodPiesa.RegeAlb
                )
            {
				//var capturiAlbe = CalculeazaCapturiPosibile(matrice, pozAlbastre);
                //1	var capturiAlbastre = CalculeazaCapturiPosibile(matrice, pozAlbe);

				return eval;
            }

            if (TabelTranspozitie.Contine(hash))
            {
                var entry = TabelTranspozitie.Lookup(hash);

                if (entry.Item2 >= adancime)
                {
                    switch (entry.Item3)
                    {
                        case 0: // Exact value
                            return entry.Item1;
                        case 1: // Lower bound
                            alpha = Math.Max(alpha, entry.Item1);
                            break;
                        case 2: // Upper bound
                            beta = Math.Min(beta, entry.Item1);
                            break;
                    }

                    if (alpha >= beta)
                    {
                        return entry.Item1;
                    }
                }
            }

            //maximizare => albastru
            if (culoare == Culoare.AlbastruMax)
			{
				double origAlpha = alpha;
				double origBeta = beta;

				long hashUpdatat = hash;
				int piesaLuata;
				int piesaCareIa;


				var mutariSortate = CalculeazaMutariPosibile(matrice, pozAlbastre,moveOrdering: true);


                foreach (var mutPos in mutariSortate.Values)
                {
					double newAlpha = double.MinValue;
                    piesaLuata = matrice[mutPos.Item2.Linie][mutPos.Item2.Coloana];
					piesaCareIa = matrice[mutPos.Item1.Linie][mutPos.Item1.Coloana];

                    matrice[mutPos.Item1.Linie][mutPos.Item1.Coloana] = (int)CodPiesa.Gol;
                    matrice[mutPos.Item2.Linie][mutPos.Item2.Coloana] = piesaCareIa;

                    hashUpdatat = ZobristHash.UpdateazaHash(
                        hashInitial: hash,
                        linieInitiala: mutPos.Item1.Linie,
                        coloanaInitiala: mutPos.Item1.Coloana,
                        piesaLuata: piesaLuata,
                        linieFinala: mutPos.Item2.Linie,
                        coloanaFinala: mutPos.Item2.Coloana,
                        piesaCareIa: piesaCareIa);

                    int indexPiesaLuata = -1;
					//albastru ia alb
                    if (EstePiesa(piesaLuata))
                    {
                        indexPiesaLuata = StergePozitieDinVector((mutPos.Item2.Linie, mutPos.Item2.Coloana),
                                    pozAlbe);
                    }
                    int pozitieSchimbata = SchimbaPozitiaDinVector((mutPos.Item1.Linie, mutPos.Item1.Coloana), pozAlbastre, (mutPos.Item2.Linie, mutPos.Item2.Coloana));

                    var valoarePiesaLuata = EngineJoc.ReturneazaScorPiesa(piesaLuata);

                    newAlpha = Math.Max(newAlpha, AlphaBetaCuMemorie(eval + valoarePiesaLuata,
                                matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbMin));
                    alpha = Math.Max(newAlpha, alpha);

                    SchimbaPozitiaDinVector(pozitieSchimbata, pozAlbastre, (mutPos.Item1.Linie, mutPos.Item1.Coloana));

                    if (EstePiesa(piesaLuata))
                    {
                        AdaugaPoitieInVector(indexPiesaLuata, pozAlbe,
                                    (mutPos.Item2.Linie, mutPos.Item2.Coloana));
                    }

                    matrice[mutPos.Item1.Linie][mutPos.Item1.Coloana] = piesaCareIa;
                    matrice[mutPos.Item2.Linie][mutPos.Item2.Coloana] = piesaLuata;

                    if (beta <= alpha)
                    {
                        goto ValoareFinala;
                    }
                }
            ValoareFinala:

                int flag = 0; // Exact value
                if (alpha <= origAlpha)
                {
                    flag = 2; // Upper bound
                }
                else if (alpha >= origBeta)
                {
                    flag = 1; // Lower bound
                }

                TabelTranspozitie.AdaugaIntrare(hash, alpha, adancime, flag);


                return alpha;
			}
			//minimizare => alb
			else
			{
                double origAlpha = alpha;
                double origBeta = beta;
                long hashUpdatat = hash;
                int piesaLuata;
                int piesaCareIa;
                var mutariSortate = CalculeazaMutariPosibile(matrice, pozAlbe, moveOrdering: true);

                foreach (var mutPos in mutariSortate.Values)
                {

                    double newBeta = double.MaxValue;

                    piesaLuata = matrice[mutPos.Item2.Linie][mutPos.Item2.Coloana];
                    piesaCareIa = matrice[mutPos.Item1.Linie][mutPos.Item1.Coloana];

                    matrice[mutPos.Item1.Linie][mutPos.Item1.Coloana] = (int)CodPiesa.Gol;
                    matrice[mutPos.Item2.Linie][mutPos.Item2.Coloana] = piesaCareIa;

                    hashUpdatat = ZobristHash.UpdateazaHash(
                        hashInitial: hash,
                        linieInitiala: mutPos.Item1.Linie,
                        coloanaInitiala: mutPos.Item1.Coloana,
                        piesaLuata: piesaLuata,
                        linieFinala: mutPos.Item2.Linie,
                        coloanaFinala: mutPos.Item2.Coloana,
                        piesaCareIa: piesaCareIa);

					//alb ia albastru
                    int indexPiesaLuata = -1;
                    if (EstePiesa(piesaLuata))
                    {
                        indexPiesaLuata = StergePozitieDinVector((mutPos.Item2.Linie, mutPos.Item2.Coloana),
                                    pozAlbastre);
                    }
                    int pozitieSchimbata = SchimbaPozitiaDinVector((mutPos.Item1.Linie, mutPos.Item1.Coloana), pozAlbe, (mutPos.Item2.Linie, mutPos.Item2.Coloana));


                    var valoarePiesaLuata = EngineJoc.ReturneazaScorPiesa(piesaLuata);

                    newBeta = Math.Min(newBeta, AlphaBetaCuMemorie(eval - valoarePiesaLuata,
                        matrice, alpha, beta, adancime - 1,
                        piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbastruMax));

                    beta = Math.Min(newBeta, beta);

                    if (EstePiesa(piesaLuata))
                    {
                        AdaugaPoitieInVector(indexPiesaLuata, pozAlbastre,
                                    (mutPos.Item2.Linie, mutPos.Item2.Coloana));
                    }

                    SchimbaPozitiaDinVector(pozitieSchimbata, pozAlbe, (mutPos.Item1.Linie, mutPos.Item1.Coloana));

                    matrice[mutPos.Item1.Linie][mutPos.Item1.Coloana] = piesaCareIa;
                    matrice[mutPos.Item2.Linie][mutPos.Item2.Coloana] = piesaLuata;

                    if (beta <= alpha)
                    {
                        goto ValoareFinala;
                    }
                }
            ValoareFinala:

                int flag = 0; // Exact value
                if (beta <= origAlpha)
                {
                    flag = 2; // Upper bound
                }
                else if (beta >= origBeta)
                {
                    flag = 1; // Lower bound
                }

                TabelTranspozitie.AdaugaIntrare(hash, beta, adancime, flag);

                return beta;
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