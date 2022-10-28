using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax
{
    public class EngineMiniMax : EngineJoc
    {
        private int _nrMutari = 0;

        protected Om _jucatorOm;
        protected MiniMaxAI _miniMaxAI;

        protected bool _randulOmului;

        private string _ultimulMesajPrimitHost = NetworkServer.BufferGol;

        public Dictionary<Piesa, int> _dictionarValoriPiese = new Dictionary<Piesa, int>();

        List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> _matriciMutariPosibile = new ();

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
        public int NrMutari
        {
            get { return _nrMutari; }
            set { _nrMutari = value; }
        }

        public EngineMiniMax(Form parentForm, Om jucator, int adancime = ConstantaTabla.Adancime) : base(parentForm)
        {
            InitializeazaInterfataVizuala();
            AdaugaEvenimentCadrane();
            _jucatorOm = jucator;
            _miniMaxAI = new MiniMaxAI(CuloareJoc.Albastru, adancime, this);

            _randulOmului = true;

            EsteRandulTau();
        }

        public EngineMiniMax(Form parentForm, int[,] matriceTabla, Om jucator, int adancime = 0) : base(parentForm, matriceTabla)
        {
            InitializeazaInterfataVizuala();
            AdaugaEvenimentCadrane();
            _jucatorOm = jucator;
            _miniMaxAI = new MiniMaxAI(CuloareJoc.Albastru, adancime, this);

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
            _textBoxMutariAlb.RightToLeft = RightToLeft.No;
            _textBoxMutariAlb.Text = string.Empty;

            _textBoxMutariAlbastru = new RichTextBox();
            _textBoxMutariAlbastru.ReadOnly = true;
            _textBoxMutariAlbastru.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlbastru.Location = new System.Drawing.Point(536, 344);
            _textBoxMutariAlbastru.Name = "textBoxMutariAlbastru";
            _textBoxMutariAlbastru.Size = new System.Drawing.Size(155, 210);
            _textBoxMutariAlbastru.TabIndex = 1;
            _textBoxMutariAlbastru.RightToLeft = RightToLeft.No;
            _textBoxMutariAlbastru.Text = string.Empty;

            ParentForm.Controls.Add(_textBoxMutariAlb);
            ParentForm.Controls.Add(_textBoxMutariAlbastru);
        }
        private void ScrieUltimaMutareInTextBox(RichTextBox textBox)
        {
            string ultimaMutareString = string.Format("    ({0},{1}) -> ({2},{3})", UltimaMutare.Item1.Linie, (char)('A' + UltimaMutare.Item1.Coloana), UltimaMutare.Item2.Linie, (char)('A' + UltimaMutare.Item2.Coloana));
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(textBox, "Text", $"{UtilitatiCrossThread.PrimesteTextulDinAltThread(textBox)}{Environment.NewLine}{ultimaMutareString}");
        }

        protected override void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie)
        {
            _nrMutari++;
            base.RealizeazaMutareaLocal(piesa, pozitie);
        }

        public async void OnCadranClick(object sender, EventArgs e)
        {
            if (_randulOmului)
            {
                if (PiesaSelectata == ConstantaTabla.PiesaNula)
                {
                    Pozitie pozitie = new(0, 0);
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
                    Pozitie pozitie = new(0, 0);
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
                                ScrieUltimaMutareInTextBox(_textBoxMutariAlbastru);
                                await Task.Run(() =>
                                {
                                    RealizeazaMutareaAI();
                                });
                            }
                        }
                    }
                }
            }
        }

        public void RealizeazaMutareaAI()
        {
            Stopwatch cronometru = new();
            cronometru.Start();
            Tuple<Pozitie, Pozitie> mutareaOptima = new(new Pozitie(0, 0), new Pozitie(0, 0));
            double scorulMutariiOptime = 0;

            List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> tupluMutariSiMatriciPosibile = CalculeazaMutariPosibile();
            mutareaOptima = tupluMutariSiMatriciPosibile[0].Item1;
            scorulMutariiOptime = _miniMaxAI.EvalueazaPozitiaCurenta(tupluMutariSiMatriciPosibile[0].Item2, double.NegativeInfinity, double.PositiveInfinity, _miniMaxAI.Adancime, CuloareJoc.Albastru);
            _miniMaxAI.CalculeazaMutareaOptima(ref mutareaOptima, ref scorulMutariiOptime, tupluMutariSiMatriciPosibile);
            VerificaSahulLaJucator(scorulMutariiOptime);
            Piesa piesa = GetPiesaCuPozitia(mutareaOptima.Item1);
            Pozitie pozitie = mutareaOptima.Item2;
            //aici
            RealizeazaMutareaLocal(piesa, pozitie);
            _miniMaxAI.UltimaPozitie = pozitie;
            EsteRandulTau();
            Debug.WriteLine(cronometru.Elapsed);
            cronometru.Stop();
        }


        private List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> CalculeazaMutariPosibile()
        {
            List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> tupluMutariSiMatriciPosibile = new();
            for (int linie = 0; linie < ConstantaTabla.MarimeVerticala; ++linie)
            {
                for (int coloana = 0; coloana < ConstantaTabla.MarimeOrizontala; ++coloana)
                {
                    if (MatriceCoduriPiese[linie, coloana] != (int)CodPiesa.Gol)
                    {
                        if (EstePiesaAlbastra(MatriceCoduriPiese[linie, coloana]))
                        {
                            Piesa piesaAI = ConvertesteCodPiesaInObiect((CodPiesa)MatriceCoduriPiese[linie, coloana]);

                            piesaAI.Pozitie = new Pozitie(linie, coloana);
                            tupluMutariSiMatriciPosibile.AddRange(ReturneazaMatriciMutariPosibile(piesaAI));
                        }
                    }
                }
            }

            return tupluMutariSiMatriciPosibile;
        }

        private int VerificaTentativaDeSah()
        {
            foreach (Cadran cadran in ArrayCadrane)
            {
                if (cadran.PiesaCadran != ConstantaTabla.PiesaNula)
                {
                    List<Pozitie> mutari = cadran.PiesaCadran.ReturneazaMutariPosibile(this);
                    foreach (Pozitie mutare in mutari)
                    {
                        if (MatriceCoduriPiese[mutare.Linie, mutare.Coloana] == (int)CodPiesa.RegeAlb)
                        {
                            return ConstantaTabla.SahLaRegeAlb;
                        }

                        if (MatriceCoduriPiese[mutare.Linie, mutare.Coloana] == (int)CodPiesa.RegeAlbastru)
                        {
                            return ConstantaTabla.SahLaRegerAlbastru;
                        }
                    }
                }
            }
            return ConstantaTabla.NuEsteSah;
        }

        public void VerificaSahulPersistent()
        {
            int tentativaSah = VerificaTentativaDeSah();

            if (tentativaSah == ConstantaTabla.SahLaRegerAlbastru)
            {
                _nrSahuriLaAlbastru++;
            }
            else
            {
                _nrSahuriLaAlbastru = 0;
            }

            if (tentativaSah == ConstantaTabla.SahLaRegerAlbastru)
            {
                _nrSahuriLaAlbastru++;
            }
            else
            {
                _nrSahuriLaAlb = 0;
            }

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
            StergeEvenimenteleCadranelor();
        }

        private void StergeEvenimenteleCadranelor()
        {
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
            if (scorulMutariiOptime > ConstantaPiese.PragSahLaAlbastru)
            {
                MessageBox.Show("Ai pierdut");
                TerminaMeciul();
            }
            else
            {
                if (scorulMutariiOptime < ConstantaPiese.PragSahLaAlb)
                {
                    MessageBox.Show("Ai castigat");
                    TerminaMeciul();
                }
            }
        }

    }
}