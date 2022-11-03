﻿using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.LinkLabel;

namespace ProiectVolovici
{
    public class MiniMaxAI : Jucator
    {
        private static Random GeneratorRandom = new Random();

        private EngineMiniMax _engine;
        private int _adancime;

        public int Adancime
        {
            get { return _adancime; }
        }
        List<Piesa> _pieseVirtuale = new();
        public MiniMaxAI(Culoare culoare, EngineMiniMax engine, int adancime = ConstantaTabla.Adancime) : base(culoare)
        {
            this._engine = engine;
            _culoare = culoare;
            _adancime = adancime;
            InitializeazaPiseleVirtuale();
        }

        private void InitializeazaPiseleVirtuale()
        {
            _pieseVirtuale.Add(null);
            _pieseVirtuale.Add(new Pion(Culoare.Alb));
            _pieseVirtuale.Add(new Pion(Culoare.Albastru));
            _pieseVirtuale.Add(new Tura(Culoare.Alb));
            _pieseVirtuale.Add(new Tura(Culoare.Albastru));
            _pieseVirtuale.Add(new Tun(Culoare.Alb));
            _pieseVirtuale.Add(new Tun(Culoare.Albastru));
            _pieseVirtuale.Add(new Gardian(Culoare.Alb));
            _pieseVirtuale.Add(new Gardian(Culoare.Albastru));
            _pieseVirtuale.Add(new Elefant(Culoare.Alb));
            _pieseVirtuale.Add(new Elefant(Culoare.Albastru));
            _pieseVirtuale.Add(new Cal(Culoare.Alb));
            _pieseVirtuale.Add(new Cal(Culoare.Albastru));
            _pieseVirtuale.Add(new Rege(Culoare.Alb));
            _pieseVirtuale.Add(new Rege(Culoare.Albastru));
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
                    return 1;
                else         
                    return result;
            }

            #endregion
        }
        public List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> CalculeazaPrimeleMutariAI()
        {
            SortedList<double, Tuple<Tuple<Pozitie, Pozitie>, int[,]>> tupluMutariSiMatriciPosibile = new(new DuplicateKeyComparerDesc<double>());
            double evaluareInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if (_engine.MatriceCoduriPiese[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        if (_engine.MatriceCoduriPiese[linie, coloana] % 2 == 0)
                        {
                            _pieseVirtuale[_engine.MatriceCoduriPiese[linie, coloana]].Pozitie = new Pozitie(linie, coloana);
                            var matriciMutari = _engine.ReturneazaMatriciMutariPosibile(_pieseVirtuale[_engine.MatriceCoduriPiese[linie, coloana]], _engine.MatriceCoduriPiese);
                            foreach (var matMut in matriciMutari)
                            {
                                tupluMutariSiMatriciPosibile.Add(evaluareInitiala + _engine.ReturneazaScorPiese((CodPiesa)_engine.MatriceCoduriPiese[matMut.Item1.Item2.Linie, matMut.Item1.Item2.Coloana]), matMut);
                            }
                        }
                    }
                }
            }
            return tupluMutariSiMatriciPosibile.Values.ToList();
        }
        //TODO:adauga pozitia initiala
        public Tuple<Tuple<Pozitie, Pozitie>, double> EvalueazaMutarileAI( List<Tuple<Tuple<Pozitie, Pozitie>,int[,]>> tupluMutariSiMatriciPosibile)
        {
            double evaluareMatriceInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
            Tuple<Pozitie, Pozitie> mutareOptima = tupluMutariSiMatriciPosibile[0].Item1;


            int codPiesaLuata = _engine.MatriceCoduriPiese[
                tupluMutariSiMatriciPosibile[0].Item1.Item2.Linie,
                tupluMutariSiMatriciPosibile[0].Item1.Item2.Coloana];

            double scorMutareOptima = Minimax_Alb(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiese((CodPiesa)codPiesaLuata),
                    tupluMutariSiMatriciPosibile[0].Item2, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime);

            for (int i = 1; i < tupluMutariSiMatriciPosibile.Count; i++)
            {
                codPiesaLuata = _engine.MatriceCoduriPiese[
                    tupluMutariSiMatriciPosibile[i].Item1.Item2.Linie,
                    tupluMutariSiMatriciPosibile[i].Item1.Item2.Coloana];

                double scorMutare = Minimax_Alb(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiese((CodPiesa)codPiesaLuata),
                    tupluMutariSiMatriciPosibile[i].Item2, double.NegativeInfinity, double.PositiveInfinity
                    ,_adancime);
                //Debug.WriteLine(tupluMutariSiMatriciPosibile[i].Item1.Item1.Linie + "," + tupluMutariSiMatriciPosibile[i].Item1.Item1.Coloana + "->"+
                //    tupluMutariSiMatriciPosibile[i].Item1.Item2.Linie + "," + tupluMutariSiMatriciPosibile[i].Item1.Item2.Coloana+" "+ scorMutare);

                if (scorMutare >= scorMutareOptima)
                {
                    mutareOptima = tupluMutariSiMatriciPosibile[i].Item1;
                    scorMutareOptima = scorMutare;
                }
                Debug.WriteLine(scorMutareOptima);
            }
            return new(mutareOptima, scorMutareOptima);
        }
        public double EvalueazaMatricea(int[,] matrice)
        {
            double scor = 0;

            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if (matrice[linie, coloana] != 0)
                    {
                        if ((matrice[linie, coloana]-1) % 2 != 0)
                        {
                            scor -= _engine.ReturneazaScorPiese((CodPiesa)matrice[linie, coloana]);
                        }
                        else
                        {
                            scor += _engine.ReturneazaScorPiese((CodPiesa)matrice[linie, coloana]);
                        }
                    }
                }
            }
            return scor;
        }
        public int[,] CopiazaMatrice(int[,] matriceInitiala, int n, int m)
        {
            int[,] matriceCopiata = new int[n, m];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < m; j++)
                    matriceCopiata[i, j] = matriceInitiala[i, j];
            return matriceCopiata;
        }
        public double Minimax_Albastru(double eval, int[,] matrice, double alpha, double beta, int adancime)
        {
            //_engine.AfiseazaMatriceDebug(matrice,adancime,eval);
            if (adancime == 0)
            {
                return eval;
            }
            else
            {
                double newAlpha = double.MinValue;
                for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                    {
                        if (EstePiesaAlbastra(matrice, linie, coloana))
                        {
                            int piesaCareIa = matrice[linie, coloana];
                            _pieseVirtuale[piesaCareIa].Pozitie = new Pozitie(linie, coloana);

                            var mutariPosibile = _pieseVirtuale[piesaCareIa].ReturneazaMutariPosibile(matrice);
                            foreach (var mutarePosibila in mutariPosibile)
                            {
                                //Debug.WriteLine(linie + "," + coloana + "->"+mutarePosibila.Linie + "," + mutarePosibila.Coloana);

                                var piesaLuata = matrice[mutarePosibila.Linie, mutarePosibila.Coloana];
                                matrice[linie, coloana] = (int)CodPiesa.Gol;
                                matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = matrice[linie, coloana];

                                newAlpha = Math.Max(newAlpha, Minimax_Alb(eval + _engine.ReturneazaScorPiese((CodPiesa)piesaLuata),
                                    matrice, alpha, beta, adancime - 1));
                                alpha = Math.Max(newAlpha, alpha);

                                matrice[linie, coloana] = piesaCareIa;
                                matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                    break;
                            }
                        }
                    }
                }
                return alpha;
            }
        }

        private static bool EstePiesaAlbastra(int[,] matrice, int linie, int coloana)
        {
            return (matrice[linie, coloana] - 1) % 2 == 1;
        }

        public double Minimax_Alb(double eval,int[,] matrice, double alpha, double beta, int adancime)
        {
            //_engine.AfiseazaMatriceDebug(matrice,adancime,eval);
            if (adancime == 0)
            {
                return eval;
            }
            else
            {
                double newBeta = double.MaxValue;
                for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                    {
                        if (EstePiesaAlba(matrice, linie, coloana))
                        {
                            int piesaCareIa = matrice[linie, coloana];
                            _pieseVirtuale[piesaCareIa].Pozitie = new Pozitie(linie, coloana);

                            var mutariPosibile = _pieseVirtuale[piesaCareIa].ReturneazaMutariPosibile(matrice);

                            foreach (var mutarePosibila in mutariPosibile)
                            {
                                //Debug.WriteLine(linie + "," + coloana + "->"+mutarePosibila.Linie + "," + mutarePosibila.Coloana);
                                var piesaLuata = matrice[mutarePosibila.Linie, mutarePosibila.Coloana];

                                matrice[linie, coloana] = (int)CodPiesa.Gol;
                                matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = matrice[linie, coloana];

                                newBeta = Math.Min(newBeta, Minimax_Albastru(eval - _engine.ReturneazaScorPiese((CodPiesa)piesaLuata),
                                    matrice, alpha, beta, adancime - 1));
                                beta = Math.Min(newBeta, beta);

                                matrice[linie, coloana] = piesaCareIa;
                                matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                    break;
                            }
                        }
                    }
                }
                return beta;
            }
        }

        private static bool EstePiesaAlba(int[,] matrice, int linie, int coloana)
        {
            return (matrice[linie, coloana] - 1) % 2 == 0;
        }
    }
}
