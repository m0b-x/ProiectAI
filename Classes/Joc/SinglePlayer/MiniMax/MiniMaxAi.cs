using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections.Generic;
using System.Security.Policy;

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

        private List<Piesa> _pieseVirtuale = new();

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

            #endregion IComparer<TKey> Members
        }

        public SortedList<double, Tuple<Pozitie, Pozitie>> CalculeazaPrimeleMutariAI()
        {
            SortedList<double, Tuple<Pozitie, Pozitie>> tupluMutariSiMatriciPosibile = new(new DuplicateKeyComparerDesc<double>());
            double evaluareInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
            var copieMatrice = CopiazaMatrice(_engine.MatriceCoduriPiese, ConstantaTabla.NrLinii, ConstantaTabla.NrColoane);
            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if ((_engine.MatriceCoduriPiese[linie, coloana] - 1) % 2 == 1)
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
                                , new(new(linie, coloana), mut));

                            copieMatrice[mut.Linie, mut.Coloana] = piesaLuata;
                            copieMatrice[linie, coloana] = piesaCareIa;
                        }
                    }
                }
            }
            return tupluMutariSiMatriciPosibile;
        }

        //TODO:adauga pozitia initiala
        public Tuple<Tuple<Pozitie, Pozitie>, double> IncepeEvaluareaMiniMax(SortedList<double, Tuple<Pozitie, Pozitie>> tupluMutariSiMatriciPosibile)
        {
            double evaluareMatriceInitiala = EvalueazaMatricea(_engine.MatriceCoduriPiese);
            Tuple<Pozitie, Pozitie> mutareOptima = tupluMutariSiMatriciPosibile.Values[0];

            var matriceInitiala = CopiazaMatrice(_engine.MatriceCoduriPiese, ConstantaTabla.NrLinii, ConstantaTabla.NrColoane);
            long hashInitial = ZobristHash.HashuiesteTabla(matriceInitiala);

            int codPiesaLuata = _engine.MatriceCoduriPiese[
                tupluMutariSiMatriciPosibile.Values[0].Item2.Linie,
                tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana];

            if (codPiesaLuata == (int)CodPiesa.RegeAlb)
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

            long hashUpdatat = ZobristHash.UpdateazaHash(
                hashInitial: hashInitial,
                linieInitiala: tupluMutariSiMatriciPosibile.Values[0].Item1.Linie,
                coloanaInitiala: tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana,
                piesaLuata: codPiesaLuata,
                linieFinala: tupluMutariSiMatriciPosibile.Values[0].Item2.Linie,
                coloanaFinala: tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana,
                piesaCareIa: codPiesaCareIa);

            double scorMutareOptima = Minimax_Alb(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiese((CodPiesa)codPiesaLuata),
                    matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime, codPiesaLuata, hashUpdatat);

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

                hashUpdatat = ZobristHash.UpdateazaHash(
                    hashInitial: hashInitial,
                    linieInitiala: tupluMutariSiMatriciPosibile.Values[0].Item1.Linie,
                    coloanaInitiala: tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana,
                    piesaLuata: codPiesaLuata,
                    linieFinala: tupluMutariSiMatriciPosibile.Values[0].Item2.Linie,
                    coloanaFinala: tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana,
                    piesaCareIa: codPiesaCareIa);

                double scorMutare = Minimax_Alb(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiese((CodPiesa)codPiesaLuata),
                    matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime, codPiesaLuata, hashUpdatat);

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
                        if ((matrice[linie, coloana] - 1) % 2 != 0)
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

        public double Minimax_Albastru(double eval, int[,] matrice, double alpha, double beta, int adancime, int piesaCapturata, long hash)
        {
            if (_tabelTranspozitie.Tabel.ContainsKey(hash))
            {
                var elementTabel = _tabelTranspozitie.Tabel[hash];

                if (elementTabel.Alpha >= alpha)
                {
                    if (elementTabel.Adancime >= adancime)
                    {
                        return elementTabel.Alpha;
                    }
                    alpha = Math.Max(alpha, elementTabel.Alpha);
                }
            }
            //_engine.AfiseazaMatriceDebug(matrice,adancime,eval);
            if (piesaCapturata == (int)CodPiesa.RegeAlb || adancime == 0)
            {
                return eval;
            }
            else
            {
                long hashUpdatat = -1;
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

                                hashUpdatat = ZobristHash.UpdateazaHash(
                                    hashInitial: hash,
                                    linieInitiala: linie,
                                    coloanaInitiala: coloana,
                                    piesaLuata: piesaLuata,
                                    linieFinala: mutarePosibila.Linie,
                                    coloanaFinala: mutarePosibila.Coloana,
                                    piesaCareIa: piesaCareIa);

                                newAlpha = Math.Max(newAlpha, Minimax_Alb(eval + _engine.ReturneazaScorPiese((CodPiesa)piesaLuata),
                                    matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat));
                                alpha = Math.Max(newAlpha, alpha);

                                matrice[linie, coloana] = piesaCareIa;
                                matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                {
                                    if (_tabelTranspozitie.Tabel.ContainsKey(hashUpdatat))
                                    {
                                        var elementTabel = _tabelTranspozitie.Tabel[hashUpdatat];
                                        if (elementTabel.Alpha < alpha)
                                        {
                                            elementTabel.Alpha = alpha;
                                        }
                                    }
                                    else
                                    {
                                        _tabelTranspozitie.AdaugaIntrare(hashUpdatat, new IntrareTabelTranspozitie(adancime, alpha, beta));
                                    }
                                    return alpha;
                                }
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

        private TabelTranspozitie _tabelTranspozitie = new TabelTranspozitie();

        public double Minimax_Alb(double eval, int[,] matrice, double alpha, double beta, int adancime, int piesaCapturata, long hash)
        {
            if (_tabelTranspozitie.Tabel.ContainsKey(hash))
            {
                var elementTabel = _tabelTranspozitie.Tabel[hash];
                if (elementTabel.Beta <= beta)
                {
                    if (elementTabel.Adancime >= adancime)
                    {
                        return elementTabel.Beta;
                    }
                }
                beta = Math.Min(beta, elementTabel.Beta);
            }
            //_engine.AfiseazaMatriceDebug(matrice,adancime,eval);
            if (piesaCapturata == (int)CodPiesa.RegeAlbastru || adancime == 0)
            {
                return eval;
            }
            else
            {
                double newBeta = double.MaxValue;
                long hashUpdatat = -1;
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

                                hashUpdatat = ZobristHash.UpdateazaHash(
                                    hashInitial: hash,
                                    linieInitiala: linie,
                                    coloanaInitiala: coloana,
                                    piesaLuata: piesaLuata,
                                    linieFinala: mutarePosibila.Linie,
                                    coloanaFinala: mutarePosibila.Coloana,
                                    piesaCareIa: piesaCareIa);

                                newBeta = Math.Min(newBeta, Minimax_Albastru(eval - _engine.ReturneazaScorPiese((CodPiesa)piesaLuata),
                                    matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat));

                                beta = Math.Min(newBeta, beta);

                                matrice[linie, coloana] = piesaCareIa;
                                matrice[mutarePosibila.Linie, mutarePosibila.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                {
                                    if (_tabelTranspozitie.Tabel.ContainsKey(hashUpdatat))
                                    {
                                        var elementTabel = _tabelTranspozitie.Tabel[hashUpdatat];
                                        if (elementTabel.Beta > beta )
                                        {
                                            elementTabel.Beta = beta;
                                        }
                                    }
                                    else
                                    {
                                        _tabelTranspozitie.AdaugaIntrare(hashUpdatat, new IntrareTabelTranspozitie(adancime, alpha, beta));
                                    }
                                    return beta;
                                }
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