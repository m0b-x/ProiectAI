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

        private Dictionary<(long, int), double> _dictionarMutariAlbastru = new();
        private Dictionary<(long, int), double> _dictionarMutariAlb = new();
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
        public SortedList<double, Tuple<Pozitie, Pozitie>> CalculeazaPrimeleMutariAI()
        {
            SortedList<double, Tuple<Pozitie, Pozitie>> tupluMutariSiMatriciPosibile = new(new DuplicateKeyComparerDesc<double>());
            double evaluareInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
            var copieMatrice = CopiazaMatrice( _engine.MatriceCoduriPiese, ConstantaTabla.NrLinii, ConstantaTabla.NrColoane);
            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if ((_engine.MatriceCoduriPiese[linie, coloana]-1) % 2 == 1)
                    {
                        _pieseVirtuale[_engine.MatriceCoduriPiese[linie, coloana]].Pozitie = new Pozitie(linie, coloana);
                        var mutari = _pieseVirtuale[_engine.MatriceCoduriPiese[linie, coloana]].ReturneazaMutariPosibile(_engine.MatriceCoduriPiese);
                        foreach (var mut in mutari)
                        {
                            int piesaLuata = copieMatrice[mut.Linie, mut.Coloana];
                            int piesaCareIa = _engine.MatriceCoduriPiese[linie, coloana];

                            copieMatrice[mut.Linie, mut.Coloana] = piesaCareIa;
                            copieMatrice[linie, coloana] = (int)CodPiesa.Gol;

                            tupluMutariSiMatriciPosibile.Add(evaluareInitiala + _engine.ReturneazaScorPiese((CodPiesa)piesaLuata)
                                , new(new(linie, coloana),mut));

                            copieMatrice[mut.Linie, mut.Coloana] =  piesaLuata;
                            copieMatrice[linie, coloana] = piesaCareIa;

                        }
                    }
                }
            }
            return tupluMutariSiMatriciPosibile;
        }
        //TODO:adauga pozitia initiala
        public Tuple<Tuple<Pozitie, Pozitie>, double> IncepeEvaluareaMiniMax( SortedList<double, Tuple<Pozitie, Pozitie>> tupluMutariSiMatriciPosibile)
        {
            double evaluareMatriceInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
            Tuple<Pozitie, Pozitie> mutareOptima = tupluMutariSiMatriciPosibile.Values[0];

            var matriceInitiala = CopiazaMatrice(_engine.MatriceCoduriPiese, ConstantaTabla.NrLinii, ConstantaTabla.NrColoane);

            long cheieZobristInitiala = ZobristHash.Hash(matriceInitiala);

            int codPiesaLuata = _engine.MatriceCoduriPiese[
                tupluMutariSiMatriciPosibile.Values[0].Item2.Linie,
                tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana];

            if(codPiesaLuata == (int) CodPiesa.RegeAlb)
                return new(mutareOptima, double.MaxValue);

            int codPiesaCareIa = _engine.MatriceCoduriPiese[
                tupluMutariSiMatriciPosibile.Values[0].Item1.Linie,
                tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana];

            matriceInitiala[
                tupluMutariSiMatriciPosibile.Values[0].Item1.Linie,
                tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana] = 0;

            matriceInitiala[
                tupluMutariSiMatriciPosibile.Values[0].Item2.Linie,
                tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana] = codPiesaCareIa;

            long cheieZobristUpdatata = ZobristHash.UpdateazaHash(cheieZobristInitiala,
                                            tupluMutariSiMatriciPosibile.Values[0].Item1.Linie+
                                            tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana,
                                            codPiesaCareIa,
                                            0,
                                            tupluMutariSiMatriciPosibile.Values[0].Item2.Linie+
                                            tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana,
                                            codPiesaLuata,
                                            codPiesaCareIa);

            double scorMutareOptima = Minimax_Alb(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiese((CodPiesa)codPiesaLuata),
                    matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime, codPiesaLuata, cheieZobristUpdatata);

            matriceInitiala[
                tupluMutariSiMatriciPosibile.Values[0].Item1.Linie,
                tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana] = codPiesaCareIa;

            matriceInitiala[
                tupluMutariSiMatriciPosibile.Values[0].Item2.Linie,
                tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana] = codPiesaLuata;


            for (int i = 1; i < tupluMutariSiMatriciPosibile.Count; i++)
            {
                codPiesaLuata = _engine.MatriceCoduriPiese[
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Linie,
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Coloana];

                if (codPiesaLuata == (int)CodPiesa.RegeAlb)
                    return new(mutareOptima, double.MaxValue);

                codPiesaCareIa = _engine.MatriceCoduriPiese[
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Linie,
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Coloana];


                matriceInitiala[
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Linie,
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Coloana] = 0;

                matriceInitiala[
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Linie,
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Coloana] = codPiesaCareIa;

                cheieZobristUpdatata = ZobristHash.UpdateazaHash(cheieZobristInitiala,
                                            tupluMutariSiMatriciPosibile.Values[0].Item1.Linie +
                                            tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana,
                                            codPiesaCareIa,
                                            0,
                                            tupluMutariSiMatriciPosibile.Values[0].Item2.Linie +
                                            tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana,
                                            codPiesaLuata,
                                            codPiesaCareIa);

                double scorMutare = Minimax_Alb(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiese((CodPiesa)codPiesaLuata),
                    matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime, codPiesaLuata, cheieZobristUpdatata);

                matriceInitiala[
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Linie,
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Coloana] = codPiesaCareIa;

                matriceInitiala[
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Linie,
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Coloana] = codPiesaLuata;

                //Debug.WriteLine(tupluMutariSiMatriciPosibile[i].Item1.Item1.Linie + "," + tupluMutariSiMatriciPosibile[i].Item1.Item1.Coloana + "->"+
                //    tupluMutariSiMatriciPosibile[i].Item1.Item2.Linie + "," + tupluMutariSiMatriciPosibile[i].Item1.Item2.Coloana+" "+ scorMutare);

                if (scorMutare >= scorMutareOptima)
                {
                    mutareOptima = tupluMutariSiMatriciPosibile.Values[i];
                    scorMutareOptima = scorMutare;
                }
                //Debug.WriteLine(scorMutareOptima);
            }
            Debug.WriteLine(scorMutareOptima);
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
        public double Minimax_Albastru(double eval, int[,] matrice, double alpha, double beta, int adancime, int piesaCapturata, long cheie)
        {
            //_engine.AfiseazaMatriceDebug(matrice,adancime,eval);
            if (piesaCapturata == (int)CodPiesa.RegeAlb || adancime == 0)
            {
                return eval;
            }

            else
            {
                if (_dictionarMutariAlbastru.ContainsKey((cheie, adancime)))
                {
                    return _dictionarMutariAlbastru[(cheie, adancime)];
                }
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


                                long cheieZobristUpdatata = ZobristHash.UpdateazaHash(cheie,
                                                            linie + coloana,
                                                            piesaCareIa,
                                                            0,
                                                            mutarePosibila.Linie + mutarePosibila.Coloana,
                                                            piesaLuata,
                                                            piesaCareIa);

                                double rezultatMinimax;
                                if (_dictionarMutariAlb.ContainsKey(new(cheieZobristUpdatata, adancime - 1)))
                                {
                                    rezultatMinimax = _dictionarMutariAlb[new(cheieZobristUpdatata, adancime - 1)];
                                }
                                else
                                {
                                    matrice[linie, coloana] = (int)CodPiesa.Gol;
                                    matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = matrice[linie, coloana];


                                    rezultatMinimax = Minimax_Alb(eval + _engine.ReturneazaScorPiese((CodPiesa)piesaLuata),
                                        matrice, alpha, beta, adancime - 1, piesaLuata, cheieZobristUpdatata);
                                
                                    matrice[linie, coloana] = piesaCareIa;
                                    matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = piesaLuata;

                                    _dictionarMutariAlb.Add((cheieZobristUpdatata, adancime - 1), rezultatMinimax);
                                }
                                newAlpha = Math.Max(newAlpha, rezultatMinimax);

                                alpha = Math.Max(newAlpha, alpha);

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

        public double Minimax_Alb(double eval,int[,] matrice, double alpha, double beta, int adancime, int piesaCapturata, long cheie)
        {
                //_engine.AfiseazaMatriceDebug(matrice,adancime,eval);
            if (piesaCapturata == (int)CodPiesa.RegeAlbastru || adancime == 0)
            {
                return eval;
            }
            else
            {
                if (_dictionarMutariAlb.ContainsKey(new(cheie,adancime)))
                {
                    return _dictionarMutariAlb[new(cheie,adancime)];
                }
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


                                long cheieZobristUpdatata = ZobristHash.UpdateazaHash(cheie,
                                                            linie + coloana,
                                                            piesaCareIa,
                                                            0,
                                                            mutarePosibila.Linie + mutarePosibila.Coloana,
                                                            piesaLuata,
                                                            piesaCareIa);
                                double rezultatMinimax;
                                if (_dictionarMutariAlbastru.ContainsKey(new(cheieZobristUpdatata, adancime - 1)))
                                {
                                    rezultatMinimax = _dictionarMutariAlbastru[new(cheieZobristUpdatata, adancime - 1)];
                                }
                                else
                                {
                                    matrice[linie, coloana] = (int)CodPiesa.Gol;
                                    matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = matrice[linie, coloana];

                                    rezultatMinimax = Minimax_Albastru(eval - _engine.ReturneazaScorPiese((CodPiesa)piesaLuata),
                                        matrice, alpha, beta, adancime - 1, piesaLuata, cheieZobristUpdatata);

                                    matrice[linie, coloana] = piesaCareIa;
                                    matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = piesaLuata;

                                    _dictionarMutariAlbastru.Add((cheieZobristUpdatata, adancime - 1), rezultatMinimax);
                                }
                                newBeta = Math.Min(newBeta, rezultatMinimax);
                                beta = Math.Min(newBeta, beta);
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
