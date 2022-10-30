using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public MiniMaxAI(CuloareJoc culoare,int adancime, EngineMiniMax engine) : base(culoare)
        {
            this._engine = engine;
            _culoare = culoare;
            _adancime = adancime;
        }

        public class ComparatorMax<TKey>
                        : IComparer<double>
        {
            #region IComparer<TKey> Members

            public int Compare(double x, double y)
            {
                int result = x.CompareTo(y);

                if (result == 0)
                    return 1; 
                else          
                    return result;
            }

            #endregion
        }
        public class ComparatorMin<TKey>
                        : IComparer<double>
        {
            #region IComparer<TKey> Members

            public int Compare(double x, double y)
            {
                int result = y.CompareTo(x);

                if (result == 0)
                    return 1;
                else
                    return result;
            }

            #endregion
        }

        public void CalculeazaMutareaAI(ref Tuple<Pozitie, Pozitie> mutareaOptima,
                                                ref double scorulMutariiOptime, List<Tuple<Tuple<Pozitie, Pozitie>,
                                                int[,]>> tupluMutariSiMatriciPosibile)
        {
            if (_engine.NrMutari > 0)
            {
                for (int mutareSiMatrice = 0; mutareSiMatrice < tupluMutariSiMatriciPosibile.Count; mutareSiMatrice++)
                {
                    double scorMutare = MiniMaxNeoptimizat(tupluMutariSiMatriciPosibile[mutareSiMatrice].Item2, double.NegativeInfinity, double.PositiveInfinity, 
                        _adancime, CuloareJoc.Albastru);
                    if (scorMutare >= scorulMutariiOptime)
                    {
                        mutareaOptima = tupluMutariSiMatriciPosibile[mutareSiMatrice].Item1;
                        scorulMutariiOptime = scorMutare;
                    }
                }
            }
            else
            {
                int index = (int)GeneratorRandom.Next(tupluMutariSiMatriciPosibile.Count);
                mutareaOptima = tupluMutariSiMatriciPosibile[index].Item1;
            }
        }

        public List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> CalculeazaMutariPosibileAI()
        {
            List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> tupluMutariSiMatriciPosibile = new();
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; ++coloana)
                {
                    if (_engine.MatriceCoduriPiese[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        if (_engine.EstePiesaAlbastra(_engine.MatriceCoduriPiese[linie, coloana]))
                        {
                            Piesa piesaAI = EngineJoc.ConvertesteCodPiesaInObiect((CodPiesa)_engine.MatriceCoduriPiese[linie, coloana]);

                            piesaAI.Pozitie = new Pozitie(linie, coloana);
                            tupluMutariSiMatriciPosibile.AddRange(_engine.ReturneazaMatriciMutariPosibile(piesaAI, _engine.MatriceCoduriPiese));
                        }
                    }
                }
            }

            return tupluMutariSiMatriciPosibile;
        }
        public double EvalueazaMatricea(int[,] matrice)
        {
            double scor = 0;

            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; ++coloana)
                {
                    CodPiesa codPiesa = (CodPiesa)matrice[linie, coloana];
                    if (_engine.EstePiesaAlba(matrice[linie, coloana]))
                    {
                        scor -= _engine.ReturneazaScorPiese(codPiesa);
                    }
                    else
                    {
                        scor += _engine.ReturneazaScorPiese(codPiesa);
                    }
                }
            }
            return scor;
        }
        public double MiniMaxNeoptimizat(int[,] matrice, double alpha, double beta, int adancime, CuloareJoc culoare)
        {
            _engine.AfiseazaMatriceDebug(matrice,adancime);
            if (adancime == 0)
            {
                double evaluare = EvalueazaMatricea(matrice);
                return evaluare;
            }

            if (culoare == CuloareJoc.Albastru)
            {
                double newBeta = beta;
                for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                    {
                        if (matrice[linie, coloana] != (int)CodPiesa.Gol)
                        {
                            if (_engine.ReturneazaCuloareDupaCodulPiesei((CodPiesa)matrice[linie, coloana]) == culoare)
                            {
                                Piesa piesa = EngineJoc.ConvertesteCodPiesaInObiect((CodPiesa)matrice[linie, coloana]);
                                piesa.Pozitie = new Pozitie(linie, coloana);
                                var mutariPosibile = piesa.ReturneazaMutariPosibile(matrice);
                                foreach (var mutarePosibila in mutariPosibile)
                                {
                                    var matriceSuccesor = matrice.Clone() as int[,];

                                    matriceSuccesor[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                                    matriceSuccesor[mutarePosibila.Linie, mutarePosibila.Coloana] = (int)piesa.Cod;

                                    newBeta = Math.Min(newBeta, MiniMaxNeoptimizat(matriceSuccesor, alpha, beta, adancime - 1, CuloareJoc.Alb)); //think about how to change moves

                                    if (newBeta <= alpha)
                                        break;
                                }
                            }
                        }
                    }
                }
                return newBeta;
            }
            else
            {
                double newAlpha = alpha;
                for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                    {
                        if (matrice[linie, coloana] != (int)CodPiesa.Gol)
                        {
                            if (_engine.ReturneazaCuloareDupaCodulPiesei((CodPiesa)matrice[linie, coloana]) == culoare)
                            {
                                Piesa piesa = EngineJoc.ConvertesteCodPiesaInObiect((CodPiesa)matrice[linie, coloana]);
                                piesa.Pozitie = new Pozitie(linie, coloana);
                                var mutariPosibile = piesa.ReturneazaMutariPosibile(matrice);
                                foreach (var mutarePosibila in mutariPosibile)
                                {
                                    var matriceSuccesor = matrice.Clone() as int[,];

                                    matriceSuccesor[piesa.Pozitie.Linie, piesa.Pozitie.Coloana] = (int)CodPiesa.Gol;
                                    matriceSuccesor[mutarePosibila.Linie, mutarePosibila.Coloana] = (int)piesa.Cod;

                                    newAlpha = Math.Max(newAlpha, MiniMaxNeoptimizat(matriceSuccesor, alpha, beta, adancime - 1, CuloareJoc.Albastru));

                                    if (beta <= newAlpha)
                                        break;
                                }
                            }
                        }
                    }
                }
                return newAlpha;
            }
        }
    }
}
