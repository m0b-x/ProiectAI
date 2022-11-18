using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        private List<Piesa> _pieseVirtuale = new()
        {
            null,
            new Pion(Culoare.Alb),
            new Pion(Culoare.Albastru),
            new Tura(Culoare.Alb),
            new Tura(Culoare.Albastru),
            new Tun(Culoare.Alb),
            new Tun(Culoare.Albastru),
            new Gardian(Culoare.Alb),
            new Gardian(Culoare.Albastru),
            new Elefant(Culoare.Alb),
            new Elefant(Culoare.Albastru),
            new Cal(Culoare.Alb),
            new Cal(Culoare.Albastru),
            new Rege(Culoare.Alb),
            new Rege(Culoare.Albastru)
        };

        public MiniMaxAI(Culoare culoare, EngineMiniMax engine, int adancime = ConstantaTabla.Adancime) : base(culoare)
        {
            this._engine = engine;
            _culoare = culoare;
            _adancime = adancime;
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
            double evaluareInitiala = EvalueazaMatricea(_engine.MatriceJaggedCoduriPiese);
            var copieMatrice = _engine.MatriceJaggedCoduriPiese;
            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if ((_engine.MatriceJaggedCoduriPiese[linie][coloana] - 1) % 2 == 1)
                    {
                        _pieseVirtuale[_engine.MatriceJaggedCoduriPiese[linie][coloana]].Pozitie = new Pozitie(linie, coloana);
                        var mutari = _pieseVirtuale[_engine.MatriceJaggedCoduriPiese[linie][coloana]].ReturneazaMutariPosibile(_engine.MatriceJaggedCoduriPiese);
                        foreach (var mut in mutari)
                        {
                            int piesaLuata = copieMatrice[mut.Linie][mut.Coloana];
                            int piesaCareIa = _engine.MatriceJaggedCoduriPiese[linie][coloana];

                            copieMatrice[mut.Linie][mut.Coloana] = piesaCareIa;
                            copieMatrice[linie][coloana] = (int)CodPiesa.Gol;

                            tupluMutariSiMatriciPosibile.Add(evaluareInitiala + _engine.ReturneazaScorPiese((CodPiesa)piesaLuata)
                                , new(new(linie, coloana), mut));

                            copieMatrice[mut.Linie][mut.Coloana] = piesaLuata;
                            copieMatrice[linie][coloana] = piesaCareIa;
                        }
                    }
                }
            }
            return tupluMutariSiMatriciPosibile;
        }

        //TODO:adauga pozitia initiala
        public Tuple<Tuple<Pozitie, Pozitie>, double> IncepeEvaluareaMiniMax(SortedList<double, Tuple<Pozitie, Pozitie>> tupluMutariSiMatriciPosibile)
        {
            double evaluareMatriceInitiala = EvalueazaMatricea(_engine.MatriceJaggedCoduriPiese);
            Tuple<Pozitie, Pozitie> mutareOptima = tupluMutariSiMatriciPosibile.Values[0];

            var matriceInitiala = _engine.MatriceJaggedCoduriPiese;
            long hashInitial = ZobristHash.HashuiesteTabla(matriceInitiala);

            int codPiesaLuata = _engine.MatriceJaggedCoduriPiese[
                tupluMutariSiMatriciPosibile.Values[0].Item2.Linie][
                tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana];

            if (codPiesaLuata == (int)CodPiesa.RegeAlb)
                return new(mutareOptima, double.MaxValue);

            int codPiesaCareIa = _engine.MatriceJaggedCoduriPiese[
                tupluMutariSiMatriciPosibile.Values[0].Item1.Linie][
                tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana];

            matriceInitiala[
                tupluMutariSiMatriciPosibile.Values[0].Item1.Linie][
                tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana] = 0;

            matriceInitiala[
                tupluMutariSiMatriciPosibile.Values[0].Item2.Linie][
                tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana] = codPiesaCareIa;

            long hashUpdatat = ZobristHash.UpdateazaHash(
                hashInitial: hashInitial,
                linieInitiala: tupluMutariSiMatriciPosibile.Values[0].Item1.Linie,
                coloanaInitiala: tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana,
                piesaLuata: codPiesaLuata,
                linieFinala: tupluMutariSiMatriciPosibile.Values[0].Item2.Linie,
                coloanaFinala: tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana,
                piesaCareIa: codPiesaCareIa);
            int nrPieseAlbe = _engine.ListaPieseAlbe.Count;
            int nrPieseAlbastre = _engine.ListaPieseAlbastre.Count;
            if (EstePiesa(codPiesaLuata))
                nrPieseAlbe--;

            double scorMutareOptima = Minimax_PieseAlbe(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiese((CodPiesa)codPiesaLuata),
                    matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime, codPiesaLuata, hashUpdatat, nrPieseAlbastre, nrPieseAlbe);

            matriceInitiala[
                tupluMutariSiMatriciPosibile.Values[0].Item1.Linie][
                tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana] = codPiesaCareIa;

            matriceInitiala[
                tupluMutariSiMatriciPosibile.Values[0].Item2.Linie][
                tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana] = codPiesaLuata;

            for (int i = 1; i < tupluMutariSiMatriciPosibile.Count; i++)
            {
                codPiesaLuata = _engine.MatriceJaggedCoduriPiese[
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Linie][
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Coloana];

                if (codPiesaLuata == (int)CodPiesa.RegeAlb)
                    return new(mutareOptima, double.MaxValue);

                codPiesaCareIa = _engine.MatriceJaggedCoduriPiese[
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Linie][
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Coloana];

                matriceInitiala[
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Linie][
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Coloana] = 0;

                matriceInitiala[
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Linie][
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Coloana] = codPiesaCareIa;

                hashUpdatat = ZobristHash.UpdateazaHash(
                    hashInitial: hashInitial,
                    linieInitiala: tupluMutariSiMatriciPosibile.Values[0].Item1.Linie,
                    coloanaInitiala: tupluMutariSiMatriciPosibile.Values[0].Item1.Coloana,
                    piesaLuata: codPiesaLuata,
                    linieFinala: tupluMutariSiMatriciPosibile.Values[0].Item2.Linie,
                    coloanaFinala: tupluMutariSiMatriciPosibile.Values[0].Item2.Coloana,
                    piesaCareIa: codPiesaCareIa);

                nrPieseAlbe = _engine.ListaPieseAlbe.Count;
                if (EstePiesa(codPiesaLuata))
                    nrPieseAlbe--;

                double scorMutare = Minimax_PieseAlbe(
                    evaluareMatriceInitiala + _engine.ReturneazaScorPiese((CodPiesa)codPiesaLuata),
                    matriceInitiala, double.NegativeInfinity, double.PositiveInfinity
                    , _adancime, codPiesaLuata, hashUpdatat, nrPieseAlbastre, nrPieseAlbe);

                matriceInitiala[
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Linie][
                    tupluMutariSiMatriciPosibile.Values[i].Item1.Coloana] = codPiesaCareIa;

                matriceInitiala[
                    tupluMutariSiMatriciPosibile.Values[i].Item2.Linie][
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

        public double EvalueazaMatricea(int[][] matrice)
        {
            double scor = 0;

            for (int linie = 0; linie < ConstantaTabla.NrLinii; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; ++coloana)
                {
                    if (matrice[linie][coloana] != 0)
                    {
                        if ((matrice[linie][coloana] - 1) % 2 != 0)
                        {
                            scor -= _engine.ReturneazaScorPiese((CodPiesa)matrice[linie][coloana]);
                        }
                        else
                        {
                            scor += _engine.ReturneazaScorPiese((CodPiesa)matrice[linie][coloana]);
                        }
                    }
                }
            }
            return scor;
        }

        public double Minimax_PieseAlbastre(double eval, int[][] matrice, double alpha, 
            double beta, int adancime, int piesaCapturata, long hash,
            int nrPieseAlbastre, int nrPieseAlbe)
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
                int ctPieseEvaluate = 0;
                for (int linie = ConstantaTabla.NrLinii -1; linie >= 0; linie--)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                    {
                        if (EstePiesaAlbastra(matrice[linie][coloana]))
                        {
                            if(ctPieseEvaluate == nrPieseAlbastre)
                            {
                                goto ValoareFinala;
                            }
                            ctPieseEvaluate++;
                            int piesaCareIa = matrice[linie][coloana];
                            _pieseVirtuale[piesaCareIa].Pozitie = new Pozitie(linie, coloana);

                            var mutariPosibile = _pieseVirtuale[piesaCareIa].ReturneazaMutariPosibile(matrice);
                            foreach (var mutarePosibila in mutariPosibile)
                            {
                                //Debug.WriteLine(linie + "," + coloana + "->"+mutarePosibila.Linie + "," + mutarePosibila.Coloana);

                                var piesaLuata = matrice[mutarePosibila.Linie][mutarePosibila.Coloana];
                                matrice[linie][coloana] = (int)CodPiesa.Gol;
                                matrice[mutarePosibila.Linie][mutarePosibila.Coloana] = matrice[linie][coloana];

                                hashUpdatat = ZobristHash.UpdateazaHash(
                                    hashInitial: hash,
                                    linieInitiala: linie,
                                    coloanaInitiala: coloana,
                                    piesaLuata: piesaLuata,
                                    linieFinala: mutarePosibila.Linie,
                                    coloanaFinala: mutarePosibila.Coloana,
                                    piesaCareIa: piesaCareIa);

                                if (EstePiesa(piesaLuata))
                                    nrPieseAlbe--;

                                newAlpha = Math.Max(newAlpha, Minimax_PieseAlbe(eval + _engine.ReturneazaScorPiese((CodPiesa)piesaLuata),
                                    matrice, alpha, beta, adancime - 1, piesaLuata, hashUpdatat, nrPieseAlbastre, nrPieseAlbe));
                                alpha = Math.Max(newAlpha, alpha);

                                matrice[linie][coloana] = piesaCareIa;
                                matrice[mutarePosibila.Linie][mutarePosibila.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                {
                                    if (_tabelTranspozitie.Tabel.ContainsKey(hashUpdatat))
                                    {
                                        _tabelTranspozitie.Tabel[hashUpdatat] = new IntrareTabelTranspozitie(adancime, alpha, _tabelTranspozitie.Tabel[hashUpdatat].Beta);
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
            ValoareFinala:
                return alpha;
            }
        }

        private static bool EstePiesaAlbastra(int codPiesa)
        {
            return (codPiesa - 1) % 2 == 1;
        }

        private TabelTranspozitie _tabelTranspozitie = new TabelTranspozitie();

        public double Minimax_PieseAlbe(double eval, int[][] matrice,
            double alpha, double beta, int adancime, int piesaCapturata,
            long hash, int nrPieseAlbastre, int nrPieseAlbe)
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

                int ctPieseEvaluate = 0;
                for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                    {
                        if (EstePiesaAlba(matrice[linie][coloana]))
                        {
                            if (ctPieseEvaluate == nrPieseAlbe)
                            {
                                goto ValoareFinala;
                            }
                            ctPieseEvaluate++;
                            int piesaCareIa = matrice[linie][coloana];
                            _pieseVirtuale[piesaCareIa].Pozitie = new Pozitie(linie, coloana);

                            var mutariPosibile = _pieseVirtuale[piesaCareIa].ReturneazaMutariPosibile(matrice);
                            foreach (var mutarePosibila in mutariPosibile)
                            {
                                //Debug.WriteLine(linie + "," + coloana + "->"+mutarePosibila.Linie + "," + mutarePosibila.Coloana);
                                var piesaLuata = matrice[mutarePosibila.Linie][mutarePosibila.Coloana];

                                matrice[linie][coloana] = (int)CodPiesa.Gol;
                                matrice[mutarePosibila.Linie][mutarePosibila.Coloana] = matrice[linie][coloana];

                                hashUpdatat = ZobristHash.UpdateazaHash(
                                    hashInitial: hash,
                                    linieInitiala: linie,
                                    coloanaInitiala: coloana,
                                    piesaLuata: piesaLuata,
                                    linieFinala: mutarePosibila.Linie,
                                    coloanaFinala: mutarePosibila.Coloana,
                                    piesaCareIa: piesaCareIa);

                                if (EstePiesa(piesaLuata))
                                    nrPieseAlbastre--;

                                newBeta = Math.Min(newBeta, Minimax_PieseAlbastre(eval - _engine.ReturneazaScorPiese((CodPiesa)piesaLuata),
                                    matrice, alpha, beta, adancime - 1,
                                    piesaLuata, hashUpdatat,nrPieseAlbastre, nrPieseAlbe));

                                beta = Math.Min(newBeta, beta);

                                matrice[linie][coloana] = piesaCareIa;
                                matrice[mutarePosibila.Linie][mutarePosibila.Coloana] = piesaLuata;

                                if (beta <= alpha)
                                {
                                    if (_tabelTranspozitie.Tabel.ContainsKey(hashUpdatat))
                                    {
                                        _tabelTranspozitie.Tabel[hashUpdatat] = new IntrareTabelTranspozitie(adancime, _tabelTranspozitie.Tabel[hashUpdatat].Alpha, beta);
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
            ValoareFinala:
                return beta;
            }
        }

        private static bool EstePiesaAlba(int codPiesa)
        {
            return (codPiesa - 1) % 2 == 0;
        }

        private static bool EstePiesa(int codPiesa)
        {
            if (codPiesa == 0)
                return false;
            return true;
        }
    }
}