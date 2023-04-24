using System;

namespace ProiectVolovici
{
    public static class ZobristHash
    {
        private static int NrPiese = Enum.GetNames(typeof(CodPiesa)).Length;
        private static int MarimeTabla = ConstantaTabla.NrColoane * ConstantaTabla.NrLinii;
        private static ulong ULongMin = 0;
        private static ulong ULongMax = ulong.MaxValue;

        private static Random randomGen = new Random(DateTime.Now.Ticks.GetHashCode());

        private static readonly ulong[][] tabeltranspozitie;

        static ZobristHash()
        {
            tabeltranspozitie = new ulong[MarimeTabla][];
            for (int i = 0; i < MarimeTabla; i++)
            {
                tabeltranspozitie[i] = new ulong[NrPiese];
            }
            for (int i = 0; i < MarimeTabla; i++)
            {
                for (int j = 1; j < NrPiese; j++)
                {
                    ulong valRandom = NextRandomUlong(randomGen);
                    tabeltranspozitie[i][j] = valRandom;
                }
            }
        }

        private static ulong NextRandomUlong(Random random)
        {

            byte[] buffer = new byte[sizeof(ulong)];
            random.NextBytes(buffer);
            ulong value = BitConverter.ToUInt64(buffer, 0);

            value = value % (ULongMax - ULongMin) + ULongMin;
            return value;
        }

        public static ulong HashuiesteTabla(int[][] tabla)
        {
            ulong retVal = 0;
            unchecked
            {
                for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                    {
                        if (tabla[linie][coloana] != 0)
                        {
                            int piesa = tabla[linie][coloana];
                            retVal ^= tabeltranspozitie[linie * (ConstantaTabla.NrLinii - 1) + coloana][piesa];
                        }
                    }
                }
            }
            return retVal;
        }

        public static ulong UpdateazaHash(ulong hashInitial,
                                  int linieInitiala, int coloanaInitiala,
                                  int piesaLuata,
                                  int linieFinala, int coloanaFinala,
                                  int piesaCareIa)
        {
            ulong hashFinal = hashInitial;
            unchecked
            {
                hashFinal ^= tabeltranspozitie[linieInitiala * (ConstantaTabla.NrLinii - 1) + coloanaInitiala][piesaCareIa];
                hashFinal ^= tabeltranspozitie[linieFinala * (ConstantaTabla.NrLinii - 1) + coloanaFinala][piesaLuata];
                hashFinal ^= tabeltranspozitie[linieFinala * (ConstantaTabla.NrLinii - 1) + coloanaFinala][piesaCareIa];
            }
            return hashFinal;
        }
    }
}