﻿using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ProiectVolovici
{
    public class MiniMaxAI : Jucator
    {
        private static Random GeneratorRandom = new();

        private EngineMiniMax _engine;
        private int _adancime;
        private TimeSpan _timpAsteptareMaxim = TimeSpan.FromSeconds(3);
        private TabelTranspozitie _tabelTranspozitie = new();
        private const double MarimeFereastraAspiratie = 1;
        public int Adancime
        {
            get { return _adancime; }
        }

        public TimeSpan TimpAsteptareMaxima
        {
            get { return _timpAsteptareMaxim; }
            set { _timpAsteptareMaxim = value; }
        }

        private List<Piesa> _pieseVirtuale = new()
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

        private Dictionary<long, (Pozitie, Pozitie)> _cacheDeschideri = new();

        public void AdaugaOpeningsInCache()
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

            _cacheDeschideri.Add(hash, (new Pozitie(7, 1), new Pozitie(7, 4)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(2, 1), new Pozitie(2, 4)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(7, 1), new Pozitie(7, 2)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(7, 1), new Pozitie(7, 2)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(7, 7), new Pozitie(7, 6)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(7, 7), new Pozitie(7, 6)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(2, 1), new Pozitie(2, 2)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(2, 7), new Pozitie(2, 6)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 1), new Pozitie(2, 2)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(2, 8), new Pozitie(2, 7)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 1), new Pozitie(2, 2)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 7), new Pozitie(2, 6)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(9, 1), new Pozitie(7, 2)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 7), new Pozitie(7, 6)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 1), new Pozitie(2, 2)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 7), new Pozitie(2, 6)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(9, 7), new Pozitie(7, 6)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(9, 1), new Pozitie(7, 2)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 1), new Pozitie(2, 0)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 7), new Pozitie(2, 8)));

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
            _cacheDeschideri.Add(hash, (new Pozitie(0, 8), new Pozitie(2, 8)));

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
            _cacheDeschideri.Add(hash, (new Pozitie(9, 1), new Pozitie(7, 0)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(9, 7), new Pozitie(7, 9)));
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

            _cacheDeschideri.Add(hash, (new Pozitie(9, 9), new Pozitie(9, 8)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 8), new Pozitie(0, 7)));

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

            _cacheDeschideri.Add(hash, (new Pozitie(0, 0), new Pozitie(0, 1)));
        }

        public SortedList<double, Tuple<Pozitie, Pozitie>> CalculeazaPrimeleMutariAI(bool moveOrdering = false)
        {
            SortedList<double, Tuple<Pozitie, Pozitie>> mutPos;

            //most valuable victim, least valuable agressor

            var pozAlbastre = ReturneazaPozitiiAlbastre();
            if (moveOrdering == true)
            {
                mutPos = new(new DuplicateKeyComparerDesc<double>());
                double evaluareInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
                foreach (var poz in pozAlbastre)
                {
                    {
                        _pieseVirtuale[_engine.MatriceCoduriPiese[poz.Item1][poz.Item2]].Pozitie = new Pozitie(poz.Item1, poz.Item2);
                        List<Pozitie> mutari = _pieseVirtuale[_engine.MatriceCoduriPiese[poz.Item1][poz.Item2]].ReturneazaMutariPosibile(_engine.MatriceCoduriPiese);
                        foreach (Pozitie mut in mutari)
                        {
                            int piesaLuata = _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana];
                            int piesaCareIa = _engine.MatriceCoduriPiese[poz.Item1][poz.Item2];

                            _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana] = piesaCareIa;
                            _engine.MatriceCoduriPiese[poz.Item1][poz.Item2] = (int)CodPiesa.Gol;

                            mutPos.Add(evaluareInitiala + _engine.ReturneazaScorPiesa(piesaLuata, mut.Linie, mut.Coloana)
                                                        - _engine.ReturneazaScorPiesa(piesaCareIa, mut.Linie, mut.Coloana)
                                , new(new(poz.Item1, poz.Item2), mut));

                            _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana] = piesaLuata;
                            _engine.MatriceCoduriPiese[poz.Item1][poz.Item2] = piesaCareIa;
                        }
                    }
                }
                return mutPos;
            }
            else
            {
                mutPos = new(new DuplicateKeyComparerDesc<double>());
                int ct = 0;
                foreach (var poz in pozAlbastre)
                {
                        List<Pozitie> mutari = _pieseVirtuale[_engine.MatriceCoduriPiese[poz.Item1][poz.Item2]].ReturneazaMutariPosibile(_engine.MatriceCoduriPiese);
                        foreach (Pozitie mut in mutari)
                        {
                            int piesaLuata = _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana];
                            int piesaCareIa = _engine.MatriceCoduriPiese[poz.Item1][poz.Item2];

                            _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana] = piesaCareIa;
                            _engine.MatriceCoduriPiese[poz.Item1][poz.Item2] = (int)CodPiesa.Gol;

                            mutPos.Add(ct
                                , new(new(poz.Item1, poz.Item2), mut));
                            ct++;

                            _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana] = piesaLuata;
                            _engine.MatriceCoduriPiese[poz.Item1][poz.Item2] = piesaCareIa;
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

        public void StergePozitieDinVector(int index, (int, int)[] vector)
        {
            vector[index] = (-1, -1);
        }

        public int StergePozitieDinVector((int, int) poz, (int, int)[] vector)
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

        public int SchimbaPozitiaDinVector(int index,
                                            (int, int)[] vector,
                                            (int, int) pozFinala)
        {
            vector[index] = pozFinala;
            return index;
        }

        public int SchimbaPozitiaDinVector((int, int) pozInitiala,
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

        public int SchimbaPozitiaDinVector((int, int)[] vector,
                                            (int, int) pozFinala,
                                            int index)
        {
            vector[index] = pozFinala;
            return index;
        }

        public int AdaugaPoitieInVector((int, int)[] vector,
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

        public int AdaugaPoitieInVector(int i, (int, int)[] vector,
                                            (int, int) pozFinala)
        {
            vector[i] = pozFinala;
            return i;
        }

        public Tuple<Tuple<Pozitie, Pozitie>, double> IncepeEvaluareaMiniMax(SortedList<double, Tuple<Pozitie, Pozitie>> mutPos, int adancimeCeruta)
        {

            double evaluareMatriceInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
            Tuple<Pozitie, Pozitie> mutareOptima = mutPos.Values[0];

            int[][] matriceInitiala = _engine.MatriceCoduriPiese;
            long hashInitial = ZobristHash.HashuiesteTabla(matriceInitiala);

            if (_engine.NrMutari <= 1)
            {
                if (_cacheDeschideri.ContainsKey(hashInitial))
                {
                    (Pozitie, Pozitie) item = _cacheDeschideri[hashInitial];
                    return new(new Tuple<Pozitie, Pozitie>(item.Item1, item.Item2), double.MaxValue);
                }
            }

            Stopwatch cronometru = new();
            cronometru.Start();


            int indexPozCareIa;
            int codPiesaLuata = _engine.MatriceCoduriPiese[
                mutPos.Values[0].Item2.Linie][
                mutPos.Values[0].Item2.Coloana];


			if (codPiesaLuata == (int)CodPiesa.RegeAlb)
			{
				cronometru.Reset();
				return new(mutareOptima, double.MaxValue);
			}

            (int, int)[] pozitiiAlbastre = ReturneazaPozitiiAlbastre();
            (int, int)[] pozitiiAlbe = ReturneazaPozitiiAlbe();

			int codPiesaCareIa = _engine.MatriceCoduriPiese[
                mutPos.Values[0].Item1.Linie][
                mutPos.Values[0].Item1.Coloana];

            matriceInitiala[
                mutPos.Values[0].Item1.Linie][
                mutPos.Values[0].Item1.Coloana] = 0;

            matriceInitiala[
                mutPos.Values[0].Item2.Linie][
                mutPos.Values[0].Item2.Coloana] = codPiesaCareIa;

            long hashUpdatat = ZobristHash.UpdateazaHash(
                hashInitial: hashInitial,
                linieInitiala: mutPos.Values[0].Item1.Linie,
                coloanaInitiala: mutPos.Values[0].Item1.Coloana,
                piesaLuata: codPiesaLuata,
                linieFinala: mutPos.Values[0].Item2.Linie,
                coloanaFinala: mutPos.Values[0].Item2.Coloana,
                piesaCareIa: codPiesaCareIa);

            indexPozCareIa = SchimbaPozitiaDinVector(
                (mutPos.Values[0].Item1.Linie, mutPos.Values[0].Item1.Coloana),
                pozitiiAlbastre,
                (mutPos.Values[0].Item2.Linie, mutPos.Values[0].Item2.Coloana));

            int indexPozLuata = -1;
            if (EstePiesa(codPiesaLuata))
            {
                indexPozLuata = StergePozitieDinVector((mutPos.Values[0].Item2.Linie, mutPos.Values[0].Item2.Coloana),
                                        pozitiiAlbe);
            }

			double scorMutareOptima = double.MinValue,
                    scorMutare = MiniMax(
                        evaluareMatriceInitiala + _engine.ReturneazaScorPiesa(codPiesaLuata, mutPos.Values[0].Item2.Linie, mutPos.Values[0].Item2.Coloana),
                        matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                        , adancime: 0, codPiesaLuata, hashUpdatat, pozitiiAlbe, pozitiiAlbastre, Culoare.AlbMin); ;

            double alpha = double.NegativeInfinity;
            double beta = double.PositiveInfinity;
            for (int adancimeTemp = 1; adancimeTemp <= adancimeCeruta; adancimeTemp++)
            {
                if (cronometru.ElapsedMilliseconds > 3000)
                {
                    goto sfarsitFunctie;
                }
                scorMutare = MiniMax(
                        evaluareMatriceInitiala + _engine.ReturneazaScorPiesa(codPiesaLuata, mutPos.Values[0].Item2.Linie, mutPos.Values[0].Item2.Coloana),
                        matriceInitiala, alpha, beta
                        , adancimeTemp, codPiesaLuata, hashUpdatat, pozitiiAlbe, pozitiiAlbastre, Culoare.AlbMin);

                if (scorMutare <= alpha || scorMutare >= beta)
                {
                    alpha = double.NegativeInfinity;
                    beta = double.PositiveInfinity;
                    adancimeTemp--;
                }
                else
                {
                    alpha = scorMutare - MarimeFereastraAspiratie;
                    beta = scorMutare + MarimeFereastraAspiratie;
                }
            }
            if (scorMutare >= scorMutareOptima)
            {
                mutareOptima = mutPos.Values[0];
                scorMutareOptima = scorMutare;
            }
            SchimbaPozitiaDinVector(indexPozCareIa,
                pozitiiAlbastre,
                (mutPos.Values[0].Item1.Linie, mutPos.Values[0].Item1.Coloana));
			if (EstePiesa(codPiesaLuata))
			{
                AdaugaPoitieInVector(indexPozLuata, pozitiiAlbe, (mutPos.Values[0].Item2.Linie, mutPos.Values[0].Item2.Coloana));
            }
            matriceInitiala[
                mutPos.Values[0].Item1.Linie][
                mutPos.Values[0].Item1.Coloana] = codPiesaCareIa;

            matriceInitiala[
                mutPos.Values[0].Item2.Linie][
                mutPos.Values[0].Item2.Coloana] = codPiesaLuata;

            for (int i = 1; i < mutPos.Count; i++)
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


				if (codPiesaLuata == (int)CodPiesa.RegeAlb)
				{
					cronometru.Reset();
					return new(mutareOptima, double.MaxValue);
				}

				alpha = double.NegativeInfinity;
                beta = double.PositiveInfinity;

                for (int adancimeTemp = 0; adancimeTemp <= adancimeCeruta; adancimeTemp++)
                {

                    if (cronometru.ElapsedMilliseconds > 3000)
                    {
                        goto sfarsitFunctie;
                    }
                    scorMutare = MiniMax(
                        evaluareMatriceInitiala + _engine.ReturneazaScorPiesa(codPiesaLuata, mutPos.Values[i].Item2.Linie, mutPos.Values[i].Item2.Coloana),
                        matriceInitiala, alpha, beta
                        , adancimeTemp, codPiesaLuata, hashUpdatat, pozitiiAlbe, pozitiiAlbastre, Culoare.AlbMin);
                    if (scorMutare <= alpha || scorMutare >= beta)
                    {
                        alpha = double.NegativeInfinity;
                        beta = double.PositiveInfinity;
                        adancimeTemp--;
                    }
                    else
                    {
                        alpha = scorMutare - MarimeFereastraAspiratie;
                        beta = scorMutare + MarimeFereastraAspiratie;
                    }
                }

                if (scorMutare >= scorMutareOptima)
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
        sfarsitFunctie:
            cronometru.Reset();
            return new(mutareOptima, scorMutareOptima);
        }

        public double EvalueazaMatricea(int[][] matrice)
        {
            double scor = 0;

            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if ((matrice[linie][coloana] - 1) % 2 != 0)
                    {
                        var valoarePiesaLuata = _engine.ReturneazaScorPiesa(matrice[linie][coloana], linie, coloana);
                        scor += valoarePiesaLuata;
                    }
                    else
                    {
                        var valoarePiesaLuata = _engine.ReturneazaScorPiesa(matrice[linie][coloana], linie, coloana);
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

        public double MiniMax(double eval, int[][] matrice, double alpha,
            double beta, int adancime, int piesaCapturata, long hash,
            (int, int)[] pozAlbe, (int, int)[] pozAlbastre, Culoare culoare)
        {
            if (_tabelTranspozitie.Tabel.ContainsKey(hash))
            {
                var intrare = _tabelTranspozitie.Tabel[hash];
                if (intrare.Adancime >= adancime)
                {
                    if (intrare.FlagIntrare == FlagIntrare.Exact)
                        return intrare.Valuare;
                    if (intrare.FlagIntrare == FlagIntrare.LowerBound && intrare.Valuare > alpha)
                        alpha = intrare.Valuare;
                    if (intrare.FlagIntrare == FlagIntrare.UpperBound && intrare.Valuare < beta)
                        beta = intrare.Valuare;
                    if (alpha >= beta)
                        return intrare.Valuare;
                }
            }
            if (piesaCapturata >= (int)CodPiesa.RegeAlb ||
                adancime == 0)
            {
                return eval;
            }
            else
            {
                //maximizare
                if (culoare == Culoare.AlbastruMax)
                {
                    double origAlpha = alpha;
                    long hashUpdatat = hash;
                    double newAlpha = double.MinValue;
                    for (int i = 0; i < pozAlbastre.Length; i++)
                    {
                        if (pozAlbastre[i].Item1 != -1)
                        {
                            int piesaCareIa = matrice[pozAlbastre[i].Item1][pozAlbastre[i].Item2];
                            _pieseVirtuale[piesaCareIa].Pozitie = new Pozitie(pozAlbastre[i].Item1, pozAlbastre[i].Item2);

                            (int, int) pozInitiala = pozAlbastre[i];
                            List<Pozitie> mutariPosibile = _pieseVirtuale[piesaCareIa].ReturneazaMutariPosibile(matrice);
                            foreach (Pozitie mutPos in mutariPosibile)
                            {
                                int piesaLuata = matrice[mutPos.Linie][mutPos.Coloana];

                                matrice[pozAlbastre[i].Item1][pozAlbastre[i].Item2] = (int)CodPiesa.Gol;
                                matrice[mutPos.Linie][mutPos.Coloana] = piesaCareIa;

                                hashUpdatat = ZobristHash.UpdateazaHash(
                                    hashInitial: hash,
                                    linieInitiala: pozAlbastre[i].Item1,
                                    coloanaInitiala: pozAlbastre[i].Item2,
                                    piesaLuata: piesaLuata,
                                    linieFinala: mutPos.Linie,
                                    coloanaFinala: mutPos.Coloana,
                                    piesaCareIa: piesaCareIa);

                                int indexPiesaLuata = -1;
                                if (EstePiesa(piesaLuata))
                                {
                                    indexPiesaLuata = StergePozitieDinVector((mutPos.Linie, mutPos.Coloana),
                                                pozAlbe);
                                }
                                SchimbaPozitiaDinVector(i, pozAlbastre, (mutPos.Linie, mutPos.Coloana));

                                var valoarePiesaLuata = _engine.ReturneazaScorPiesa(piesaLuata, mutPos.Linie, mutPos.Coloana);
                                newAlpha = Math.Max(newAlpha, MiniMax(eval + valoarePiesaLuata,
                                    matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbMin));
                                alpha = Math.Max(newAlpha, alpha);

                                SchimbaPozitiaDinVector(i, pozAlbastre, pozInitiala);

                                if (EstePiesa(piesaLuata))
                                {
                                    AdaugaPoitieInVector(indexPiesaLuata, pozAlbe,
                                                (mutPos.Linie, mutPos.Coloana));
                                }

                                matrice[pozInitiala.Item1][pozInitiala.Item2] = piesaCareIa;
                                matrice[mutPos.Linie][mutPos.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                {
                                    goto ValoareFinala;
                                }
                            }
                        }
                    }
                ValoareFinala:
                    if (alpha <= origAlpha) // a lowerbound value
                        _tabelTranspozitie.AdaugaIntrare(hash, alpha, FlagIntrare.LowerBound, adancime);
                    else if (alpha >= beta) // an upperbound value
                        _tabelTranspozitie.AdaugaIntrare(hash, alpha, FlagIntrare.UpperBound, adancime);
                    else // a true minimax value
                        _tabelTranspozitie.AdaugaIntrare(hash, alpha, FlagIntrare.Exact, adancime);
                    return alpha;
                }
                //minimizare => alb
                else
                {
                    double origBeta = beta;
                    double newBeta = double.MaxValue;
                    long hashUpdatat = hash;

                    for (int i = 0; i < pozAlbe.Length; i++)
                    {
                        if (pozAlbe[i].Item1 != -1)
                        {
                            int piesaCareIa = matrice[pozAlbe[i].Item1][pozAlbe[i].Item2];
                            _pieseVirtuale[piesaCareIa].Pozitie = new Pozitie(pozAlbe[i].Item1, pozAlbe[i].Item2);

                            (int, int) pozInitiala = pozAlbe[i];
                            List<Pozitie> mutariPosibile = _pieseVirtuale[piesaCareIa].ReturneazaMutariPosibile(matrice);
                            foreach (Pozitie mutPos in mutariPosibile)
                            {
                                int piesaLuata = matrice[mutPos.Linie][mutPos.Coloana];
                                matrice[pozAlbe[i].Item1][pozAlbe[i].Item2] = (int)CodPiesa.Gol;
                                matrice[mutPos.Linie][mutPos.Coloana] = piesaCareIa;

                                hashUpdatat = ZobristHash.UpdateazaHash(
                                    hashInitial: hash,
                                    linieInitiala: pozAlbe[i].Item1,
                                    coloanaInitiala: pozAlbe[i].Item2,
                                    piesaLuata: piesaLuata,
                                    linieFinala: mutPos.Linie,
                                    coloanaFinala: mutPos.Coloana,
                                    piesaCareIa: piesaCareIa);

                                int indexPiesaLuata = -1;
                                if (EstePiesa(piesaLuata))
                                {
                                    indexPiesaLuata = StergePozitieDinVector((mutPos.Linie, mutPos.Coloana),
                                                pozAlbastre);
                                }
                                SchimbaPozitiaDinVector(i, pozAlbe, (mutPos.Linie, mutPos.Coloana));

                                var valoarePiesaLuata = _engine.ReturneazaScorPiesa(piesaLuata, mutPos.Linie, mutPos.Coloana);
                                newBeta = Math.Min(newBeta, MiniMax(eval - valoarePiesaLuata,
                                    matrice, alpha, beta, adancime - 1,
                                    piesaLuata, hashUpdatat, pozAlbe, pozAlbastre, Culoare.AlbastruMax));

                                beta = Math.Min(newBeta, beta);

                                SchimbaPozitiaDinVector(i, pozAlbe, pozInitiala);

                                if(EstePiesa(piesaLuata))
                                {
                                    AdaugaPoitieInVector(indexPiesaLuata, pozAlbastre,
                                                (mutPos.Linie, mutPos.Coloana));
                                }

                                matrice[pozInitiala.Item1][pozInitiala.Item2] = piesaCareIa;
                                matrice[mutPos.Linie][mutPos.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                {
                                    goto ValoareFinala;
                                }
                            }
                        }
                    }
                ValoareFinala:
                    if (beta <= alpha) // a lowerbound value
                        _tabelTranspozitie.AdaugaIntrare(hash, beta, FlagIntrare.LowerBound, adancime);
                    else if (beta >= origBeta) // an upperbound value
                        _tabelTranspozitie.AdaugaIntrare(hash, beta, FlagIntrare.UpperBound, adancime);
                    else // a true minimax value
                        _tabelTranspozitie.AdaugaIntrare(hash, beta, FlagIntrare.Exact, adancime);
                    return beta;
                }
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