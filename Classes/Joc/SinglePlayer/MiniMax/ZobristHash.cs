using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax
{
    internal class ZobristHash
    {
        static int NrPiese = 14;
        static int NrPozitiiTabla = 90;
        static Random GeneratorRandom = new Random();

        int[,] _tabel = new int[NrPozitiiTabla, NrPiese];

        public void InitHash()
        {
            for(int i=0; i < NrPozitiiTabla; i++)
                for(int j =0; j<NrPiese; j++)
                {
                    _tabel[i, j] = GeneratorRandom.Next(0,int.MaxValue);
                }
        }


    }
}
