using ProiectVolovici;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProiectVolovici
{
    public static class ZobristHash
    {

        private static readonly Int64[] tabeltranspozitie;

        static int MarimeTabel = ConstantaTabla.NrColoane * ConstantaTabla.NrLinii * Enum.GetNames(typeof(CodPiesa)).Length;
        static int MarimeTabla = ConstantaTabla.NrColoane * ConstantaTabla.NrLinii;

        static ZobristHash()
        {
            Random rnd;

            rnd = new Random(0);
            tabeltranspozitie = new long[MarimeTabel];
            for (int i = 0; i < MarimeTabel; i++)
            {
                tabeltranspozitie[i] = rnd.NextInt64();
            }
        }

        public static long UpdateazaHash(long zobristKey, int pos, int piesaluata, int piesaCareIa)
        {
            int baseIndex;

            baseIndex = pos << 4;
            zobristKey ^= tabeltranspozitie[baseIndex + (piesaluata)] ^
                          tabeltranspozitie[baseIndex + (piesaCareIa)];
            return zobristKey;
        }

        public static long UpdateazaHash(long zobristKey,
                                            int pos1,
                                            int oldPiece1,
                                            int newPiece1,
                                            int pos2,
                                            int oldPiece2,
                                            int newPiece2)
        {
            int baseIndex1;
            int baseIndex2;

            baseIndex1 = pos1 << 4;
            baseIndex2 = pos2 << 4;
            zobristKey ^= tabeltranspozitie[baseIndex1 + (oldPiece1)] ^
                          tabeltranspozitie[baseIndex1 + (newPiece1)] ^
                          tabeltranspozitie[baseIndex2 + (oldPiece2)] ^
                          tabeltranspozitie[baseIndex2 + (newPiece2)];
            return zobristKey;
        }

        public static long Hash(int[,] tabla)
        {
            long retVal = 0;

            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    retVal ^= tabeltranspozitie[(linie+coloana << 4) + tabla[linie, coloana]];
                }
            }
           return retVal;
        }
    } 
} 
