using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class ParserTabla
    {
        int _liniiDecodificate;
        int _coloaneDecodificate;
        int _lungimeMesajInitial;
        
        public ParserTabla(int liniiDecodificate,int coloaneDecodificate, int lungimeMesajInitial)
        {
            _liniiDecodificate = liniiDecodificate;
            _coloaneDecodificate = coloaneDecodificate;
            _lungimeMesajInitial = lungimeMesajInitial;
        }
        
        public String CodificareMesaj(int[,] matriceTabla)
        {
            var str = string.Join("", matriceTabla.OfType<int>()
                                    .Select((value, index) => new { value, index })
                                    .GroupBy(linie => linie.index / matriceTabla.GetLength(1))
                                    .Select(linie => $"{{{string.Join(" ", linie.Select(linie => linie.value))}}}"));

            return str;
        }

        public int[,] DecodificaMesaj(String stringPrimit)
        {

            stringPrimit = stringPrimit.Replace("}{", " ");

            stringPrimit = stringPrimit.Substring(1, stringPrimit.Length - 2);


            int[] mapaAuxiliara = stringPrimit.Split(' ').Select(int.Parse).ToArray();

            int [,]arrayReturnat = new int[_liniiDecodificate, _coloaneDecodificate];


            for (int linie = 0; linie < _liniiDecodificate; linie++)
                for (int coloana = 0; coloana < _coloaneDecodificate; coloana++)
                    arrayReturnat[linie, coloana] = mapaAuxiliara[(coloana * _coloaneDecodificate) + linie];

            //rotire
            for (int linie = 0; linie < _liniiDecodificate; ++linie)
            {
                for (int coloana = 0; coloana < _coloaneDecodificate; ++coloana)
                {
                    arrayReturnat[linie, coloana] = arrayReturnat[_liniiDecodificate - coloana - 1, linie];
                }
            }

            //mirroring
            for (int linie = 0; linie < _coloaneDecodificate; linie++)
            {
                for (int coloana = 0; coloana < _coloaneDecodificate / 2; coloana++)
                {
                    int auxiliar = arrayReturnat[linie, _coloaneDecodificate - coloana - 1];
                    arrayReturnat[linie, _coloaneDecodificate - coloana - 1] = arrayReturnat[linie, coloana];
                    arrayReturnat[linie, coloana] = auxiliar;

                }
            }
            return arrayReturnat;

        }
    }
}
