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

        public void CalculeazaMutareaOptima(ref Tuple<Pozitie, Pozitie> mutareaOptima,
                                                ref double scorulMutariiOptime, List<Tuple<Tuple<Pozitie, Pozitie>,
                                                int[,]>> tupluMutariSiMatriciPosibile)
        {
            if (_engine.NrMutari > 0)
            {
                for (int mutareSiMatrice = 0; mutareSiMatrice < tupluMutariSiMatriciPosibile.Count; mutareSiMatrice++)
                {
                    double scorMutare = EvalueazaPozitiaCurenta(tupluMutariSiMatriciPosibile[mutareSiMatrice].Item2, double.NegativeInfinity, double.PositiveInfinity, 
                        _adancime, CuloareJoc.Alb);
                    if (scorMutare >= scorulMutariiOptime)
                    {
                        mutareaOptima = tupluMutariSiMatriciPosibile[mutareSiMatrice].Item1;
                        scorulMutariiOptime = scorMutare;
                    }
                }
            }
            else
            {
                int index = (int)GeneratorRandom.NextInt64(tupluMutariSiMatriciPosibile.Count);
                mutareaOptima = tupluMutariSiMatriciPosibile[index].Item1;
            }
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
        public double EvalueazaPozitiaCurenta(int[,] matrice, double alpha, double beta, int adancime, CuloareJoc culoare)
        {
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
                                    var matriceSuccesor = (int[,])matrice.Clone();
                                    Tuple<Pozitie, Pozitie> mutare = new(piesa.Pozitie, mutarePosibila);

                                    int codPiesaMutata = matriceSuccesor[mutare.Item1.Linie, mutare.Item1.Coloana];
                                    int codPiesaLuata = matriceSuccesor[mutare.Item2.Linie, mutare.Item2.Coloana];

                                    matriceSuccesor[mutare.Item1.Linie, mutare.Item1.Coloana] = (int)CodPiesa.Gol;
                                    matriceSuccesor[mutare.Item2.Linie, mutare.Item2.Coloana] = codPiesaMutata;

                                    newBeta = Math.Min(newBeta, EvalueazaPozitiaCurenta(matriceSuccesor, alpha, beta, adancime - 1, (culoare == CuloareJoc.Alb) ? CuloareJoc.Albastru : CuloareJoc.Alb)); //think about how to change moves

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
                                    var matriceSuccesor = (int[,])matrice.Clone();
                                    Tuple<Pozitie, Pozitie> mutare = new(piesa.Pozitie, mutarePosibila);

                                    int codPiesaMutata = matriceSuccesor[mutare.Item1.Linie, mutare.Item1.Coloana];
                                    int codPiesaLuata = matriceSuccesor[mutare.Item2.Linie, mutare.Item2.Coloana];

                                    matriceSuccesor[mutare.Item1.Linie, mutare.Item1.Coloana] = (int)CodPiesa.Gol;
                                    matriceSuccesor[mutare.Item2.Linie, mutare.Item2.Coloana] = codPiesaLuata;

                                    newAlpha = Math.Max(newAlpha, EvalueazaPozitiaCurenta(matriceSuccesor, alpha, beta, adancime - 1, (culoare == CuloareJoc.Alb) ? CuloareJoc.Albastru : CuloareJoc.Alb));

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
