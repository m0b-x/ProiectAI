﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public ParserTabla(int liniiDecodificate, int coloaneDecodificate)
        {
            _liniiDecodificate = liniiDecodificate;
            _coloaneDecodificate = coloaneDecodificate;
        }


        public String CodificareMutare(Pozitie pozitieInitiala,Pozitie pozitieFinala)
        {
            Debug.WriteLine("CallBack CodificareMutare");
            string mutareString =  "{" +
                                    pozitieInitiala.Linie + "," +
                                    pozitieInitiala.Coloana + "," +
                                    pozitieFinala.Linie + "," +
                                    pozitieFinala.Coloana +
                                   "}";
            Debug.WriteLine(mutareString);
            return mutareString;
        }
        public Tuple<Pozitie,Pozitie> DecodificareMutare(String mutare)
        {
            Debug.WriteLine("Decodificare" + mutare);
            mutare = mutare.Replace("{", " ");
            mutare = mutare.Replace("}", " ");
            int[] vectorPozitiiInt = mutare.Split(',').Select(int.Parse).ToArray();


            return new Tuple<Pozitie, Pozitie>(new Pozitie(vectorPozitiiInt[0], vectorPozitiiInt[1]),
                                               new Pozitie(vectorPozitiiInt[2], vectorPozitiiInt[3]));
            }
        public String CodificareTabla(int[,] matriceTabla)
        {
            var str = /*"mesajInceput" + */ string.Join("", matriceTabla.OfType<int>()
                                            .Select((value, index) => new { value, index })
                                            .GroupBy(linie => linie.index / matriceTabla.GetLength(1))
                                            .Select(linie => $"{{{string.Join(" ", linie.Select(linie => linie.value))}}}"));

            return str;
        }

        public int[,] DecodificareTabla(String stringPrimit)
        {

            stringPrimit = stringPrimit.Replace("}{", " ");

            stringPrimit = stringPrimit.Substring(1, stringPrimit.Length - 2);


            int[] vectorAuxiliar = stringPrimit.Split(' ').Select(int.Parse).ToArray();

            int[,] matriceReturnata = new int[_liniiDecodificate, _coloaneDecodificate];

            //din vector in matrice
            int contorElemente = 0;
            for (int linie = 0; linie < _liniiDecodificate; linie++)
            {
                for (int coloana = 0; coloana < _coloaneDecodificate; coloana++)
                {
                    matriceReturnata[linie, coloana] = vectorAuxiliar[contorElemente++];
                }
            }
            return matriceReturnata;

        }
    }
}