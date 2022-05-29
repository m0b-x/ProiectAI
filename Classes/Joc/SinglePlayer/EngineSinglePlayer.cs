using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class EngineJocSinglePlayer : EngineJoc
    {
        private static readonly int Adancime = 2;
        private int _nrMutari = 0;

        protected Om _jucatorOm;
        protected AI _jucatorAi;

        protected bool _randulOmului;

        private String _ultimulMesajPrimitHost = NetworkServer.BufferGol;

        public Dictionary<Piesa,int> _dictionarValoriPiese = new();

        List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> _matriciMutariPosibile = new();

        private RichTextBox _textBoxMutariAlb;
        private RichTextBox _textBoxMutariAlbastru;

        public Om Jucator
        {
            get { return _jucatorOm; }
        }
        public bool RandulTau
        {
            get { return _randulOmului; }
        }

        public EngineJocSinglePlayer(Form parentForm, Om jucator) : base(parentForm)
        {
            InitializeazaInterfataVizuala();
            AdaugaEvenimentCadrane();
            _jucatorOm = jucator;
            _jucatorAi = new AI(CuloareJoc.Albastru);

            _randulOmului = true;

            EsteRandulTau();
        }

        public EngineJocSinglePlayer(Form parentForm, int[,] matriceTabla, Om jucator) : base(parentForm, matriceTabla)
        {
            InitializeazaInterfataVizuala();
            AdaugaEvenimentCadrane();
            _jucatorOm = jucator;
            _jucatorAi = new AI(CuloareJoc.Albastru);

            _randulOmului = false;
        }

        public void AdaugaEvenimentCadrane()
        {
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    ArrayCadrane[linie, coloana].Click += OnCadranClick;
                }
            }
        }

        protected virtual void NuEsteRandulTau()
        {
            _randulOmului = false;
        }

        protected virtual void EsteRandulTau()
        {
            _randulOmului = true;
        }

        public void InitializeazaInterfataVizuala()
        {
            _textBoxMutariAlb = new RichTextBox();
            _textBoxMutariAlb.ReadOnly = true;
            _textBoxMutariAlb.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlb.Location = new System.Drawing.Point(536, 82);
            _textBoxMutariAlb.Name = "textBoxMutariAlb";
            _textBoxMutariAlb.Size = new System.Drawing.Size(155, 210);
            _textBoxMutariAlb.TabIndex = 0;
            _textBoxMutariAlb.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _textBoxMutariAlb.Text = System.String.Empty;

            _textBoxMutariAlbastru = new RichTextBox();
            _textBoxMutariAlbastru.ReadOnly = true;
            _textBoxMutariAlbastru.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlbastru.Location = new System.Drawing.Point(536, 344);
            _textBoxMutariAlbastru.Name = "textBoxMutariAlbastru";
            _textBoxMutariAlbastru.Size = new System.Drawing.Size(155, 210);
            _textBoxMutariAlbastru.TabIndex = 1;
            _textBoxMutariAlbastru.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _textBoxMutariAlbastru.Text = System.String.Empty;

            ParentForm.Controls.Add(_textBoxMutariAlb);
            ParentForm.Controls.Add(_textBoxMutariAlbastru);
        }
        private void ScrieUltimaMutareInTextBox(RichTextBox textBox)
        {
            String ultimaMutareString = String.Format("    ({0},{1}) -> ({2},{3})", UltimaMutare.Item1.Linie, (char)('A' + UltimaMutare.Item1.Coloana), UltimaMutare.Item2.Linie, (char)('A' + UltimaMutare.Item2.Coloana));
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(textBox, "Text", $"{UtilitatiCrossThread.PrimesteTextulDinAltThread(textBox)}{Environment.NewLine}{ultimaMutareString}");
        }

        protected override void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie)
        {
            _nrMutari++;
            base.RealizeazaMutareaLocal(piesa, pozitie);
        }

        public void OnCadranClick(object sender, EventArgs e)
        {
            if (_randulOmului)
            {
                if (PiesaSelectata == ConstantaTabla.PiesaNula)
                {
                    Pozitie pozitie = new Pozitie(0, 0);
                    pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                    pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;

                    if (ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                    {
                        Piesa piesa = GetPiesaCuPozitia(pozitie);

                        if (piesa != null)
                        {
                            if (piesa.CuloarePiesa != _jucatorOm.Culoare)
                            {
                                return;
                            }
                            PiesaSelectata = piesa;
                            piesa.ArataMutariPosibile(this);
                            if (ExistaMutariPosibile() == true)
                            {
                                ArataPiesaSelectata(piesa);
                            }
                            else
                            {
                                ArataPozitieBlocata(pozitie);
                            }
                        }
                    }
                }
                else
                {
                    Pozitie pozitie = new Pozitie(0, 0);
                    pozitie.Linie = (sender as Cadran).PozitieCadran.Linie;
                    pozitie.Coloana = (sender as Cadran).PozitieCadran.Coloana;

                    if (PiesaSelectata.Pozitie != pozitie)
                    {
                        if (EsteMutareaPosibila(pozitie))
                        {
                            if (MatriceCoduriPiese[pozitie.Linie, pozitie.Coloana] != (int)CodPiesa.Gol)
                            {
                                ConstantaSunet.SunetPiesaLuata.Play();
                            }
                            else
                            {
                                ConstantaSunet.SunetPiesaMutata.Play();
                            }
                            NuEsteRandulTau(); 
                            VerificaSahulLaAi(pozitie);
                            RealizeazaMutareaLocal(PiesaSelectata, pozitie);
                            ScrieUltimaMutareInTextBox(_textBoxMutariAlb);
                            _jucatorOm.UltimaPozitie = pozitie;
                            if (_esteGataMeciul == false)
                            {
                                RealizeazaMutareaAI();
                                ScrieUltimaMutareInTextBox(_textBoxMutariAlbastru);
                            }
                        }
                    }
                }
            }
        }

        private double EvalueazaMatricea(int[,] matrice)
        {

            double scorAlb = 0;
            double scorAlbastru = 0;


            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; ++coloana)
                {
                    CodPiesa codPiesa = (CodPiesa)matrice[linie, coloana];
                    if (codPiesa != CodPiesa.Gol)
                    {
                        if (EstePiesaAlba(matrice[linie, coloana]))
                        {
                            scorAlb += ReturneazaScorPiese(codPiesa);
                        }
                        else
                        {
                            scorAlbastru += ReturneazaScorPiese(codPiesa);
                        }

                    }
                }
            }
            return scorAlbastru-scorAlb;
        }

        public void RealizeazaMutareaAI()
        {
            Stopwatch cronometru = new();
            cronometru.Start();
            Tuple<Pozitie, Pozitie> mutareaOptima = new(new Pozitie(0, 0), new Pozitie(0, 0));
            double scorulMutariiOptime = 0;

            List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> tupluMutariSiMatriciPosibile = new();
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; ++coloana)
                {
                    if (MatriceCoduriPiese[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        Piesa piesaAI = ConvertesteCodPiesaInObiect((CodPiesa)MatriceCoduriPiese[linie, coloana]);

                        piesaAI.Pozitie = new Pozitie(linie, coloana);
                        if (piesaAI.CuloarePiesa == CuloareJoc.Albastru)
                        {
                            _matriciMutariPosibile = ReturneazaMatriciMutariPosibile(piesaAI);
                            foreach (var matrice in _matriciMutariPosibile)
                            {
                                tupluMutariSiMatriciPosibile.Add(matrice);
                            }
                        }
                    }
                }
            }
            mutareaOptima = tupluMutariSiMatriciPosibile[0].Item1;
            scorulMutariiOptime = EvalueazaPozitia(tupluMutariSiMatriciPosibile[0].Item2, Double.NegativeInfinity, Double.PositiveInfinity, Adancime, CuloareJoc.Albastru);

            if (_nrMutari > 0)
            {
                for (int linie = 1; linie < tupluMutariSiMatriciPosibile.Count; linie++)
                {
                    double coloana = EvalueazaPozitia(tupluMutariSiMatriciPosibile[linie].Item2, Double.NegativeInfinity, Double.PositiveInfinity, Adancime, CuloareJoc.Alb);
                    if (coloana >= scorulMutariiOptime)
                    {
                        mutareaOptima = tupluMutariSiMatriciPosibile[linie].Item1;
                        scorulMutariiOptime = coloana;
                    }
                }
            }
            else
            {
                Random generatorRandom = new Random();
                int index = (int)generatorRandom.NextInt64(tupluMutariSiMatriciPosibile.Count);
                mutareaOptima = tupluMutariSiMatriciPosibile[index].Item1;
            }
            VerificaSahulLaJucator(scorulMutariiOptime);
            Piesa piesa = GetPiesaCuPozitia(mutareaOptima.Item1);
            Pozitie pozitie = mutareaOptima.Item2;
            RealizeazaMutareaLocal(piesa, pozitie);
            _jucatorAi.UltimaPozitie = pozitie;
            EsteRandulTau();
            Debug.WriteLine(cronometru.Elapsed);
            cronometru.Stop();
        }


        private int VerificaTentativaDeSah()
        {
            foreach (Cadran cadran in ArrayCadrane)
            {
                if (cadran.PiesaCadran != ConstantaTabla.PiesaNula)
                {
                    List<Pozitie> mutari = cadran.PiesaCadran.ReturneazaMutariPosibile(this);
                    foreach (var mutare in mutari)
                    {
                        if (MatriceCoduriPiese[mutare.Linie, mutare.Coloana] == (int)CodPiesa.RegeAlb)
                            return ConstantaTabla.SahLaRegeAlb;
                        if (MatriceCoduriPiese[mutare.Linie, mutare.Coloana] == (int)CodPiesa.RegeAlbastru)
                            return ConstantaTabla.SahLaRegerAlbastru;
                    }
                }
            }
            return ConstantaTabla.NuEsteSah;
        }

        public void VerificaSahulPersistent()
        {
            int tentativaSah = VerificaTentativaDeSah();

            if (tentativaSah == ConstantaTabla.SahLaRegerAlbastru)
                _nrSahuriLaAlbastru++;
            else
                _nrSahuriLaAlbastru = 0;


            if (tentativaSah == ConstantaTabla.SahLaRegerAlbastru)
                _nrSahuriLaAlbastru++;
            else
                _nrSahuriLaAlb = 0;

            if (_nrSahuriLaAlbastru >= ConstantaTabla.NrMaximSahuri)
            {
                MessageBox.Show("Ai castigat");
                TerminaMeciul();
            }
            if (_nrSahuriLaAlb >= ConstantaTabla.NrMaximSahuri)
            {
                MessageBox.Show("Ai pierdut");
                TerminaMeciul();
            }
        }

        public void TerminaMeciul()
        {
            _esteGataMeciul = true;
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    ArrayCadrane[linie, coloana].Click -= OnCadranClick;
                }
            }
        }
        private void VerificaSahulLaAi(Pozitie pozitie)
        {

            VerificaSahulPersistent();
            Piesa piesa = GetPiesaCuPozitia(pozitie);
            if (piesa != null)
            {
                if (piesa.Cod == CodPiesa.RegeAlbastru)
                {
                    MessageBox.Show("Ai castigat");
                    TerminaMeciul();
                }
            }
        }
        private void VerificaSahulLaJucator(double scorulMutariiOptime)
        {

            VerificaSahulPersistent();
            if (scorulMutariiOptime > ConstantaTabla.PragSahLaAlbastru)
            {
                MessageBox.Show("Ai pierdut");
                TerminaMeciul();
            }
            else
            {
                if (scorulMutariiOptime < ConstantaTabla.PragSahLaAlb)
                {
                    MessageBox.Show("Ai castigat");
                    TerminaMeciul();
                }
            }
        }

        public double EvalueazaPozitia(int[,] matrice, double alpha, double beta, int adancime, CuloareJoc culoare)
        {
            if (adancime == 0)
            {
                double evaluare = EvalueazaMatricea(matrice);
                return evaluare;
            }

            if (culoare == CuloareJoc.Albastru)
            { 
                List<Tuple<Pozitie,Pozitie>> mutari = new (); 
                for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
                {
                    for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                    {
                        if (matrice[linie,coloana] != (int)CodPiesa.Gol)
                        {
                            if (ReturneazaCuloareDupaCodulPiesei((CodPiesa)matrice[linie,coloana]) == culoare)
                            {
                                Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)matrice[linie, coloana]);
                                piesa.Pozitie = new Pozitie(linie, coloana);
                                var mutariPosibile = piesa.ReturneazaMutariPosibile(this);
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
                    matriceSuccesor[mutare.Item2.Linie, mutare.Item2.Coloana] =  codPiesa;

                    newBeta = Math.Min(newBeta, EvalueazaPozitia(matriceSuccesor, alpha, beta, adancime - 1, (culoare == CuloareJoc.Alb) ? CuloareJoc.Albastru : CuloareJoc.Alb)); //think about how to change moves
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
                            if (ReturneazaCuloareDupaCodulPiesei((CodPiesa)matrice[linie, coloana]) == culoare)
                            {
                                Piesa piesa = ConvertesteCodPiesaInObiect((CodPiesa)matrice[linie, coloana]);
                                piesa.Pozitie = new Pozitie(linie, coloana);
                                var mutariPosibile = piesa.ReturneazaMutariPosibile(this);
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

                    newAlpha = Math.Max(newAlpha, EvalueazaPozitia(matriceSuccesor, alpha, beta, adancime - 1, (culoare == CuloareJoc.Alb) ? CuloareJoc.Albastru : CuloareJoc.Alb)); 
                    if (beta <= newAlpha) 
                        break;
                }
                return newAlpha; 
            }
        }
        public CuloareJoc ReturneazaCuloareDupaCodulPiesei(CodPiesa codPiesa)
        {
            if ((int)codPiesa % 2 == 0)
                return CuloareJoc.Alb;
            else
                return CuloareJoc.Albastru;
        }
        public bool EstePiesaAlba(int codPiesa)
        {
            return (codPiesa % 2 == 1) ? true : false; 
        }

        public bool EstePiesaAlbastra(int codPiesa)
        {
            return (codPiesa % 2 == 0) ? true : false;
        }

        public double ReturneazaScorPiese(CodPiesa codPiesa)
        {
            switch (codPiesa)
            {
                case CodPiesa.CalAlb: return ConstantaPiese.ValoareCal;
                case CodPiesa.CalAbastru: return ConstantaPiese.ValoareCal;

                case CodPiesa.ElefantAlb: return ConstantaPiese.ValoareElefant;
                case CodPiesa.ElefantAlbastru: return ConstantaPiese.ValoareElefant;

                case CodPiesa.GardianAlb: return ConstantaPiese.ValoareGardian;
                case CodPiesa.GardianAlbastru: return ConstantaPiese.ValoareGardian;

                case CodPiesa.PionAlb: return ConstantaPiese.ValoarePion;
                case CodPiesa.PionAlbastru: return ConstantaPiese.ValoarePion;

                case CodPiesa.RegeAlb: return ConstantaPiese.ValoareRege;
                case CodPiesa.RegeAlbastru: return ConstantaPiese.ValoareRege;

                case CodPiesa.TuraAlba: return ConstantaPiese.ValoareTura;
                case CodPiesa.TuraAlbastra: return ConstantaPiese.ValoareTura;

                case CodPiesa.TunAlb: return ConstantaPiese.ValoareTun;
                case CodPiesa.TunAlbastru: return ConstantaPiese.ValoareTun;

                default: return 0;
            }

        }
        public void AfisareMatriciDebug(int[,] matrice)
        {
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; coloana++)
                {
                    Debug.Write(matrice[linie, coloana] + " ");
                }
                Debug.WriteLine("");
            }
            Debug.WriteLine(EvalueazaMatricea(matrice));
            Debug.WriteLine("------------------------------------------");
        }
        public Tuple<Pozitie, Pozitie> ReturneazaMutareaAI()
        {
            Tuple<Pozitie, Pozitie> pozitieReturnata = Tuple.Create(new Pozitie(0, 0), new Pozitie(0, 0));

            return pozitieReturnata;
        }

    }
}