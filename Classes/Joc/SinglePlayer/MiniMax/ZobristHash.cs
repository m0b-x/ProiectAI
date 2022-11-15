using ProiectVolovici;
using System;
using static System.Windows.Forms.LinkLabel;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProiectVolovici
{
    public static class ZobristHash
    {

        static int NrPiese = Enum.GetNames(typeof(CodPiesa)).Length;
        static int MarimeTabla = ConstantaTabla.NrColoane * ConstantaTabla.NrLinii;

        private static readonly Int64[,] tabeltranspozitie;

        static ZobristHash()
        {
            Random rnd;

            rnd = new Random();
            tabeltranspozitie = new long[MarimeTabla, NrPiese];
            for (int i = 0; i < MarimeTabla; i++)
            {
                for(int j =0;j<NrPiese; j++)
                    tabeltranspozitie[i,j] = rnd.NextInt64();
            }
        }

        public static long Hash(int[,] tabla)
        {
            long retVal = 0;

            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    if (tabla[linie,coloana] != (int)CodPiesa.Gol)
                    {
                        var piesa = tabla[linie, coloana];
                        retVal ^= tabeltranspozitie[linie * 10 + coloana, piesa];
                    }
                }
            }
           return retVal;
        }

        public static long UpdateazaHash(long hashInitial,
                                  int linieInitiala,int coloanaInitiala,
                                  int piesaInitiala,
                                  int linieFinala, int coloanaFinala,
                                  int piesaFinala)
        {
            long hashFinal = hashInitial;
            hashFinal ^= tabeltranspozitie[linieInitiala * 10 + coloanaInitiala, (int)CodPiesa.Gol];
            hashFinal ^= tabeltranspozitie[linieFinala * 10 + coloanaFinala, piesaFinala];

            return hashFinal;
        }
    } 
} 
