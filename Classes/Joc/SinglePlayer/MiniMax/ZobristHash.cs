using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax
{
    internal class ZobristHash
    {
        static int NrPozitiiTabla = 90;
        static int NrPiese = Enum.GetNames(typeof(CodPiesa)).Length;
        static Random GeneratorRandom = new Random();

        int[,] _tabel = new int[NrPozitiiTabla, NrPiese];

        //inspirat https://www.geeksforgeeks.org/minimax-algorithm-in-game-theory-set-5-zobrist-hashing/
        public void InitHash()
        {
            for(int i=0; i < NrPozitiiTabla; i++)
                for(int j =0; j<NrPiese; j++)
                {
                    _tabel[i, j] = GeneratorRandom.Next(0,int.MaxValue);
                }
        }

        public long Hash(int[,] matrice)
        {
            long h = 0;
            for (int i = 0; i < NrPozitiiTabla; i++)
                for (int j = 0; j < NrPiese; j++)
                {
                    if (matrice[i,j] != 0)
                    {
                        int piesa = matrice[i, j];
                        h ^= _tabel[i+j, piesa];
                    }
                }
            return h;
        }


    }
}
