using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Collections.Generic;
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
        public void CalculeazaMutareaOptima(ref Tuple<Pozitie, Pozitie> mutareaOptima, ref double scorulMutariiOptime, List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> tupluMutariSiMatriciPosibile)
        {
            if (_engine.NrMutari > 0)
            {
                for (int linie = 0; linie < tupluMutariSiMatriciPosibile.Count; linie++)
                {
                    double coloana = EvalueazaPozitiaCurenta(tupluMutariSiMatriciPosibile[linie].Item2, double.NegativeInfinity, double.PositiveInfinity, 
                        _adancime, CuloareJoc.Alb);
                    if (coloana >= scorulMutariiOptime)
                    {
                        mutareaOptima = tupluMutariSiMatriciPosibile[linie].Item1;
                        scorulMutariiOptime = coloana;
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

            double scorAlb = 0;
            double scorAlbastru = 0;


            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; ++coloana)
                {
                    CodPiesa codPiesa = (CodPiesa)matrice[linie, coloana];
                    if (_engine.EstePiesaAlba(matrice[linie, coloana]))
                    {
                        scorAlb += _engine.ReturneazaScorPiese(codPiesa);
                    }
                    else
                    {
                        scorAlbastru += _engine.ReturneazaScorPiese(codPiesa);
                    }
                }
            }
            return scorAlbastru - scorAlb;
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
                List<Tuple<Pozitie, Pozitie>> mutari = new();
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
                                var mutariPosibile = piesa.ReturneazaMutariPosibile(_engine);
                                foreach (var mutarePosibila in mutariPosibile)
                                {
                                    mutari.Add(new(piesa.Pozitie, mutarePosibila));
                                }
                            }
                        }
                    }
                }
                double newBeta = beta;
                foreach (var mutare in mutari)
                {
                    int[,] matriceSuccesor = (int[,])matrice.Clone();
                    int codPiesa = matriceSuccesor[mutare.Item1.Linie, mutare.Item1.Coloana];
                    matriceSuccesor[mutare.Item1.Linie, mutare.Item1.Coloana] = (int)CodPiesa.Gol;
                    matriceSuccesor[mutare.Item2.Linie, mutare.Item2.Coloana] = codPiesa;

                    newBeta = Math.Min(newBeta, EvalueazaPozitiaCurenta(matriceSuccesor, alpha, beta, adancime - 1, culoare == CuloareJoc.Alb ? CuloareJoc.Albastru : CuloareJoc.Alb)); //think about how to change moves
                    if (newBeta <= alpha)
                        break;
                }
                return newBeta;
            }
            else
            {
                List<Tuple<Pozitie, Pozitie>> mutari = new();
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
                                var mutariPosibile = piesa.ReturneazaMutariPosibile(_engine);
                                foreach (var mutarePosibila in mutariPosibile)
                                {
                                    mutari.Add(new(piesa.Pozitie, mutarePosibila));
                                }
                            }
                        }
                    }
                }
                double newAlpha = alpha;
                foreach (var mutare in mutari)
                {
                    int[,] matriceSuccesor = (int[,])matrice.Clone();
                    int codPiesa = matriceSuccesor[mutare.Item1.Linie, mutare.Item1.Coloana];
                    matriceSuccesor[mutare.Item1.Linie, mutare.Item1.Coloana] = (int)CodPiesa.Gol;
                    matriceSuccesor[mutare.Item2.Linie, mutare.Item2.Coloana] = codPiesa;

                    newAlpha = Math.Max(newAlpha, EvalueazaPozitiaCurenta(matriceSuccesor, alpha, beta, adancime - 1, culoare == CuloareJoc.Alb ? CuloareJoc.Albastru : CuloareJoc.Alb));
                    if (beta <= newAlpha)
                        break;
                }
                return newAlpha;
            }
        }
    }
}
