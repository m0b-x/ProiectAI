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
        private TabelTranspozitie _tabelTranspozitie = new();

        public int Adancime
        {
            get { return _adancime; }
        }

        private List<Piesa> _pieseVirtuale = new()
        {
            null,
            new Pion(Culoare.Alb),
            new Pion(Culoare.Albastru),
            new Tura(Culoare.Alb),
            new Tura(Culoare.Albastru),
            new Tun(Culoare.Alb),
            new Tun(Culoare.Albastru),
            new Gardian(Culoare.Alb),
            new Gardian(Culoare.Albastru),
            new Elefant(Culoare.Alb),
            new Elefant(Culoare.Albastru),
            new Cal(Culoare.Alb),
            new Cal(Culoare.Albastru),
            new Rege(Culoare.Alb),
            new Rege(Culoare.Albastru)
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
            /*
            int[][] initiala = new int[10][]
            {
                new int [] { 4, 12, 10, 8, 14, 8, 10, 12, 4},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 6, 0, 0, 0, 0, 0, 6, 0},
                new int[] { 2, 0, 2, 0, 2, 0, 2, 0, 2},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 1, 0, 1, 0, 1, 0, 1, 0, 1},
                new int[] { 0, 5, 0, 0, 0, 0, 0, 5, 0},
                new int[] { 0, 0, 0, 0, 0, 0, 0, 0, 0},
                new int[] { 3, 11, 9, 7, 13, 7, 9, 11, 3}
            };

            hash = ZobristHash.HashuiesteTabla(initiala);

            _cacheDeschideri.Add(hash, (new Pozitie(0, 0), new Pozitie(0, 0)));
             */


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
            //

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
            //

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
            //

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
            //

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


            //

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

            _cacheDeschideri.Add(hash, (new Pozitie(2, 7), new Pozitie(2, 7)));
            //

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

            //

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
            //

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

        public SortedList<double, Tuple<Pozitie, Pozitie>> CalculeazaPrimeleMutariAI()
        {
            SortedList<double, Tuple<Pozitie, Pozitie>> mutPos = new(new DuplicateKeyComparerDesc<double>());
            double evaluareInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if ((_engine.MatriceCoduriPiese[linie][coloana] - 1) % 2 == 1)
                    {
                        _pieseVirtuale[_engine.MatriceCoduriPiese[linie][coloana]].Pozitie = new Pozitie(linie, coloana);
                        List<Pozitie> mutari = _pieseVirtuale[_engine.MatriceCoduriPiese[linie][coloana]].ReturneazaMutariPosibile(_engine.MatriceCoduriPiese);
                        foreach (Pozitie mut in mutari)
                        {
                            int piesaLuata = _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana];
                            int piesaCareIa = _engine.MatriceCoduriPiese[linie][coloana];

                            _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana] = piesaCareIa;
                            _engine.MatriceCoduriPiese[linie][coloana] = (int)CodPiesa.Gol;

                            mutPos.Add(evaluareInitiala + _engine.ReturneazaScorPiesa(piesaLuata, mut.Linie, mut.Coloana)
                                , new(new(linie, coloana), mut));

                            _engine.MatriceCoduriPiese[mut.Linie][mut.Coloana] = piesaLuata;
                            _engine.MatriceCoduriPiese[linie][coloana] = piesaCareIa;
                        }
                    }
                }
            }
            return mutPos;
        }

        //TODO:adauga pozitia initiala
        public Tuple<Tuple<Pozitie, Pozitie>, double> IncepeEvaluareaMiniMax(SortedList<double, Tuple<Pozitie, Pozitie>> mutPos)
        {
            //ADAUGA AICI CACHE OPENINGS
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


            int codPiesaLuata = _engine.MatriceCoduriPiese[
                mutPos.Values[0].Item2.Linie][
                mutPos.Values[0].Item2.Coloana];

            if (codPiesaLuata == (int)CodPiesa.RegeAlb)
                return new(mutareOptima, double.MaxValue);

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
            int nrPieseAlbe = _engine.ListaPieseAlbe.Count;
            int nrPieseAlbastre = _engine.ListaPieseAlbastre.Count;
            if (EstePiesa(codPiesaLuata))
                nrPieseAlbe--;

            double scorMutareOptima = Minimax_PieseAlbe(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiesa(codPiesaLuata, mutPos.Values[0].Item2.Linie, mutPos.Values[0].Item2.Coloana),
                    matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime, codPiesaLuata, hashUpdatat, nrPieseAlbastre, nrPieseAlbe);

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

                if (codPiesaLuata == (int)CodPiesa.RegeAlb)
                {
                    return new(mutareOptima, double.MaxValue);
                }

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

                nrPieseAlbe = _engine.ListaPieseAlbe.Count;
                if (EstePiesa(codPiesaLuata))
                {
                    nrPieseAlbe--;
                }

                double scorMutare = Minimax_PieseAlbe(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiesa(codPiesaLuata, mutPos.Values[i].Item2.Linie, mutPos.Values[i].Item2.Coloana),
                    matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime, codPiesaLuata, hashUpdatat, nrPieseAlbastre, nrPieseAlbe);

                matriceInitiala[
                    mutPos.Values[i].Item1.Linie][
                    mutPos.Values[i].Item1.Coloana] = codPiesaCareIa;

                matriceInitiala[
                    mutPos.Values[i].Item2.Linie][
                    mutPos.Values[i].Item2.Coloana] = codPiesaLuata;

                //Debug.WriteLine(mutPos[i].Item1.Item1.Linie + "," + mutPos[i].Item1.Item1.Coloana + "->"+
                //    mutPos[i].Item1.Item2.Linie + "," + mutPos[i].Item1.Item2.Coloana+" "+ scorMutare);

                if (scorMutare >= scorMutareOptima)
                {
                    mutareOptima = mutPos.Values[i];
                    scorMutareOptima = scorMutare;
                }
                //Debug.WriteLine(scorMutareOptima);
            }
            return new(mutareOptima, scorMutareOptima);
        }

        public double EvalueazaMatricea(int[][] matrice)
        {
            double scor = 0;

            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if (matrice[linie][coloana] != 0)
                    {
                        if ((matrice[linie][coloana] - 1) % 2 != 0)
                        {
                            var valoarePiesaLuata = _engine.ReturneazaScorPiesa(matrice[linie][coloana], linie, coloana);
                            scor -= valoarePiesaLuata; 
                        }
                        else
                        {
                            var valoarePiesaLuata = _engine.ReturneazaScorPiesa(matrice[linie][coloana], linie, coloana);
                            scor += valoarePiesaLuata;
                        }
                    }
                }
            }
            return scor;
        }

        public double Minimax_PieseAlbastre(double eval, int[][] matrice, double alpha,
            double beta, int adancime, int piesaCapturata, long hash,
            int nrPieseAlbastre, int nrPieseAlbe)
        {
            if (_tabelTranspozitie.Tabel.ContainsKey(hash))
            {
                IntrareTabelTranspozitie elementTabel = _tabelTranspozitie.Tabel[hash];
                if (elementTabel.Adancime >= adancime)
                {
                    return elementTabel.Alpha;
                }
            }
            //_engine.AfiseazaMatriceDebug(matrice,adancime,eval);
            if (piesaCapturata == (int)CodPiesa.RegeAlbastru ||
                piesaCapturata == (int)CodPiesa.RegeAlb)
            {
                return eval / adancime;
            }
            if( adancime == 0)
            {
                return eval;
            }
            else
            {
                long hashUpdatat = hash;
                double newAlpha = double.MinValue;
                int ctPieseEvaluate = 0;
                for (int linie = ConstantaTabla.NrLinii - 1; linie >= 0; linie--)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                    {
                        if (EstePiesaAlbastra(matrice[linie][coloana]))
                        {
                            if (ctPieseEvaluate == nrPieseAlbastre)
                            {
                                goto ValoareFinala;
                            }
                            ctPieseEvaluate++;
                            int piesaCareIa = matrice[linie][coloana];
                            _pieseVirtuale[piesaCareIa].Pozitie = new Pozitie(linie, coloana);

                            List<Pozitie> mutariPosibile = _pieseVirtuale[piesaCareIa].ReturneazaMutariPosibile(matrice);
                            foreach (Pozitie mutPos in mutariPosibile)
                            {
                                //Debug.WriteLine(linie + "," + coloana + "->"+mutPos.Linie + "," + mutPos.Coloana);

                                int piesaLuata = matrice[mutPos.Linie][mutPos.Coloana];


                                matrice[linie][coloana] = (int)CodPiesa.Gol;
                                matrice[mutPos.Linie][mutPos.Coloana] = piesaCareIa;

                                hashUpdatat = ZobristHash.UpdateazaHash(
                                    hashInitial: hash,
                                    linieInitiala: linie,
                                    coloanaInitiala: coloana,
                                    piesaLuata: piesaLuata,
                                    linieFinala: mutPos.Linie,
                                    coloanaFinala: mutPos.Coloana,
                                    piesaCareIa: piesaCareIa);

                                if (EstePiesa(piesaLuata))
                                {
                                    nrPieseAlbe--;
                                }


                                var valoarePiesaLuata = _engine.ReturneazaScorPiesa(piesaLuata, mutPos.Linie, mutPos.Coloana);

                                newAlpha = Math.Max(newAlpha, Minimax_PieseAlbe(eval + valoarePiesaLuata,
                                    matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat, nrPieseAlbastre, nrPieseAlbe));
                                alpha = Math.Max(newAlpha, alpha);

                                matrice[linie][coloana] = piesaCareIa;
                                matrice[mutPos.Linie][mutPos.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                {
                                    if (_tabelTranspozitie.Tabel.ContainsKey(hashUpdatat))
                                    {
                                        if (_tabelTranspozitie.Tabel[hashUpdatat].Adancime < adancime)
                                        {
                                            _tabelTranspozitie.Tabel[hashUpdatat] = new IntrareTabelTranspozitie(adancime, alpha, _tabelTranspozitie.Tabel[hashUpdatat].Beta);
                                        }
                                    }
                                    else
                                    {
                                        _tabelTranspozitie.AdaugaIntrare(hashUpdatat, new IntrareTabelTranspozitie(adancime, alpha, beta));
                                    }
                                    return alpha;
                                }
                            }
                        }
                    }
                }
            ValoareFinala:

                if (_tabelTranspozitie.Tabel.ContainsKey(hash))
                {
                    if (_tabelTranspozitie.Tabel[hash].Adancime < adancime)
                    {
                        _tabelTranspozitie.Tabel[hash] = new IntrareTabelTranspozitie(adancime, alpha, alpha);
                    }
                }
                else
                {
                    _tabelTranspozitie.AdaugaIntrare(hash, new IntrareTabelTranspozitie(adancime, alpha, alpha));
                }
                return alpha;
            }
        }

        private static bool EstePiesaAlbastra(int codPiesa)
        {
            return (codPiesa - 1) % 2 == 1;
        }

        public double Minimax_PieseAlbe(double eval, int[][] matrice,
            double alpha, double beta, int adancime, int piesaCapturata,
            long hash, int nrPieseAlbastre, int nrPieseAlbe)
        {
            if (_tabelTranspozitie.Tabel.ContainsKey(hash))
            {
                IntrareTabelTranspozitie elementTabel = _tabelTranspozitie.Tabel[hash];
                if (elementTabel.Adancime >= adancime)
                {
                    return elementTabel.Beta;
                }
            }
            //_engine.AfiseazaMatriceDebug(matrice,adancime,eval);
            if (piesaCapturata == (int)CodPiesa.RegeAlbastru ||
                piesaCapturata == (int)CodPiesa.RegeAlb)
            {
                return eval / adancime;
            }
            if (adancime == 0)
            {
                return eval;
            }
            else
            {
                double newBeta = double.MaxValue;
                long hashUpdatat = hash;

                int ctPieseEvaluate = 0;
                for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                    {
                        if (EstePiesaAlba(matrice[linie][coloana]))
                        {
                            if (ctPieseEvaluate == nrPieseAlbe)
                            {
                                goto ValoareFinala;
                            }
                            ctPieseEvaluate++;

                            int piesaCareIa = matrice[linie][coloana];
                            _pieseVirtuale[piesaCareIa].Pozitie = new Pozitie(linie, coloana);

                            List<Pozitie> mutariPosibile = _pieseVirtuale[piesaCareIa].ReturneazaMutariPosibile(matrice);
                            foreach (Pozitie mutPos in mutariPosibile)
                            {
                                //Debug.WriteLine(linie + "," + coloana + "->"+mutPos.Linie + "," + mutPos.Coloana);
                                int piesaLuata = matrice[mutPos.Linie][mutPos.Coloana];
                                matrice[linie][coloana] = (int)CodPiesa.Gol;
                                matrice[mutPos.Linie][mutPos.Coloana] = piesaCareIa;

                                hashUpdatat = ZobristHash.UpdateazaHash(
                                    hashInitial: hash,
                                    linieInitiala: linie,
                                    coloanaInitiala: coloana,
                                    piesaLuata: piesaLuata,
                                    linieFinala: mutPos.Linie,
                                    coloanaFinala: mutPos.Coloana,
                                    piesaCareIa: piesaCareIa);

                                if (EstePiesa(piesaLuata))
                                {
                                    nrPieseAlbastre--;
                                }

                                var valoarePiesaLuata = _engine.ReturneazaScorPiesa(piesaLuata, mutPos.Linie, mutPos.Coloana);
                                newBeta = Math.Min(newBeta, Minimax_PieseAlbastre(eval - valoarePiesaLuata,
                                    matrice, alpha, beta, adancime - 1,
                                    piesaLuata, hashUpdatat, nrPieseAlbastre, nrPieseAlbe));

                                beta = Math.Min(newBeta, beta);

                                matrice[linie][coloana] = piesaCareIa;
                                matrice[mutPos.Linie][mutPos.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                {
                                    if (_tabelTranspozitie.Tabel.ContainsKey(hashUpdatat))
                                    {
                                        if (_tabelTranspozitie.Tabel[hashUpdatat].Adancime < adancime)
                                        {
                                            _tabelTranspozitie.Tabel[hashUpdatat] = new IntrareTabelTranspozitie(adancime, _tabelTranspozitie.Tabel[hashUpdatat].Alpha, beta);
                                        }
                                    }
                                    else
                                    {
                                        _tabelTranspozitie.AdaugaIntrare(hashUpdatat, new IntrareTabelTranspozitie(adancime, alpha, beta));
                                    }
                                    return beta;
                                }
                            }
                        }
                    }
                }
            ValoareFinala:
                if (_tabelTranspozitie.Tabel.ContainsKey(hash))
                {
                    if (_tabelTranspozitie.Tabel[hash].Adancime < adancime)
                    {
                        _tabelTranspozitie.Tabel[hash] = new IntrareTabelTranspozitie(adancime, beta, beta);
                    }
                }
                else
                {
                    _tabelTranspozitie.AdaugaIntrare(hash, new IntrareTabelTranspozitie(adancime, beta, beta));
                }
                return beta;
            }
        }

        private static bool EstePiesaAlba(int codPiesa)
        {
            return (codPiesa - 1) % 2 == 0;
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