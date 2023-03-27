using System;

namespace ProiectVolovici
{
    public static class ZobristHash
    {
        private static int NrPiese = Enum.GetNames(typeof(CodPiesa)).Length;
        private static int MarimeTabla = ConstantaTabla.NrColoane * ConstantaTabla.NrLinii;

        private static readonly long[][] tabeltranspozitie;

        static ZobristHash()
        {
            Random rnd;

            rnd = new Random(0);
            tabeltranspozitie = new long[MarimeTabla][];
            for (int i = 0; i < MarimeTabla; i++)
            {
                tabeltranspozitie[i] = new long[NrPiese];
            }
            for (int i = 0; i < MarimeTabla; i++)
            {
                for (int j = 1; j < NrPiese; j++)
                {
                    long valRandom = rnd.NextInt64();
                    tabeltranspozitie[i][j] = valRandom;
                }
            }
        }

        public static long HashuiesteTabla(int[][] tabla)
        {
            long retVal = 0;
            unchecked
            {
                for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                    {
                        if (tabla[linie][coloana] != (int)CodPiesa.Gol)
                        {
                            int piesa = tabla[linie][coloana];
                            retVal ^= tabeltranspozitie[linie * (ConstantaTabla.NrLinii - 1) + coloana][piesa];
                        }
                    }
                }
            }
            return retVal;
        }

        public static long UpdateazaHash(long hashInitial,
                                  int linieInitiala, int coloanaInitiala,
                                  int piesaLuata,
                                  int linieFinala, int coloanaFinala,
                                  int piesaCareIa)
        {
            long hashFinal = hashInitial;
            unchecked
            {
                //Debug.WriteLine(linieInitiala * (ConstantaTabla.NrLinii - 1));
                //Debug.WriteLine(linieFinala * (ConstantaTabla.NrLinii - 1));
                hashFinal ^= tabeltranspozitie[linieInitiala * (ConstantaTabla.NrLinii - 1) + coloanaInitiala][piesaCareIa];
                hashFinal ^= tabeltranspozitie[linieFinala * (ConstantaTabla.NrLinii - 1) + coloanaFinala][piesaLuata];
                hashFinal ^= tabeltranspozitie[linieFinala * (ConstantaTabla.NrLinii - 1) + coloanaFinala][piesaCareIa];
            }
            return hashFinal;
        }
    }
}