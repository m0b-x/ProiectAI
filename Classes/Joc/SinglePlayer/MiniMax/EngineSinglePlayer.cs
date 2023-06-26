using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax
{
    public class EngineSinglePlayer : EngineJoc
    {
        private int _nrMutari = 0;
        protected Om _jucatorOm;
        protected AI _jucatorAI;

        protected bool _randulOmului;

        private RichTextBox _textBoxMutariAlbastru;
        private RichTextBox _textBoxMutariAlb;

        protected System.Timers.Timer _timerAsteptareAI;
        protected short _valoareTimerAsteptare;

        public Om Jucator
        {
            get { return _jucatorOm; }
        }

        public bool RandulOmului
        {
            get { return _randulOmului; }
        }

        public int NrMutari
        {
            get { return _nrMutari; }
            set { _nrMutari = value; }
        }

        public EngineSinglePlayer(Form parentForm,
            Om jucator,
            Aspect aspect,
            TipAI tip,
            int adancime,
            bool incepeOmul = true
            ) : base(parentForm, aspect)
        {
            InitializeazaInterfataVizuala();
            AdaugaEvenimentCadrane();
            _jucatorOm = jucator;
            InitializeazaAI(tip, adancime);

            _randulOmului = incepeOmul;
            InitializeazaTimerAsteptare();
            EsteRandulTau();
        }

        public EngineSinglePlayer(Form parentForm,
            int[][] matriceTabla,
            Om jucator,
            Aspect aspect,
            TipAI tip,
            int adancime,
            bool incepeOmul = true) : base(parentForm, matriceTabla, aspect)
        {
            InitializeazaInterfataVizuala();
            AdaugaEvenimentCadrane();
            _jucatorOm = jucator;
            InitializeazaAI(tip, adancime);
            _randulOmului = incepeOmul;
            InitializeazaTimerAsteptare();
        }

        private void InitializeazaAI(TipAI tip, int adancime)
        {
            if (tip == TipAI.MiniMax)
            {
                _jucatorAI = new AlphaBetaAI(Culoare.AlbastruMax, this, adancime, MTDF : false);
            }
            else if (tip == TipAI.AlphaBeta)
            {
                _jucatorAI = new AlphaBetaAI(Culoare.AlbastruMax, this, adancime, MTDF: false);
            }
            else
            {
                _jucatorAI = new AlphaBetaAI(Culoare.AlbastruMax, this, adancime, MTDF: true);
            }
        }

        public void InitializeazaTimerAsteptare()
        {
            _valoareTimerAsteptare = 0;
            _timerAsteptareAI = new();
            _timerAsteptareAI.Elapsed += new ElapsedEventHandler(SchimbaTextAsteptare);
            _timerAsteptareAI.Interval = 300;
            _timerAsteptareAI.Enabled = false;
        }

        public void SchimbaTextAsteptare(object source, ElapsedEventArgs e)
        {
            switch (_valoareTimerAsteptare)
            {
                case 0:
                    {
                        UtilitatiCrossThread.SeteazaProprietateaDinAltThread(LabelAsteptare, "Text", "In Asteptare AI.");
                        _valoareTimerAsteptare++;
                        break;
                    }
                case 1:
                    {
                        UtilitatiCrossThread.SeteazaProprietateaDinAltThread(LabelAsteptare, "Text", "In Asteptare AI..");
                        _valoareTimerAsteptare++;
                        break;
                    }
                case 2:
                    {
                        UtilitatiCrossThread.SeteazaProprietateaDinAltThread(LabelAsteptare, "Text", "In Asteptare AI...");
                        _valoareTimerAsteptare = 0;
                        break;
                    }
            }
        }

        public void PornesteTimerAsteptareAI()
        {
            _valoareTimerAsteptare = 0;
            _timerAsteptareAI.Start();
        }

        public void OpresteTimerAsteptareAI()
        {
            _valoareTimerAsteptare = 0;
            _timerAsteptareAI.Stop();
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(LabelAsteptare, "Text", "Este randul tau(alb)");
        }

        public void AdaugaEvenimentCadrane()
        {
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie][coloana].Click += OnCadranClick;
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

        public override void StergeUltimaMutare()
		{
			StergeUltimaLinieDinRichTextBox();
			NrMutari-= 2;
			base.StergeUltimaMutare();
		}

		private void StergeUltimaLinieDinRichTextBox()
		{
            if (NrMutari > 0)
            {
                if (NrMutari % 2 == 0)
                {
                    string textInitial = _textBoxMutariAlb.Text;
                    string[] linii = textInitial.Split('\n');
                    Array.Resize(ref linii, linii.Length - 1);
                    string textModificat = string.Join("\n", linii);
                    _textBoxMutariAlb.Text = textModificat;
                }
                else
                {
                    string textInitial = _textBoxMutariAlbastru.Text;
                    string[] linii = textInitial.Split('\n');
                    Array.Resize(ref linii, linii.Length - 1);
                    string textModificat = string.Join("\n", linii);
                    _textBoxMutariAlbastru.Text = textModificat;
                }
            }
		}

		public void InitializeazaInterfataVizuala()
        {
            _textBoxMutariAlbastru = new RichTextBox();
            _textBoxMutariAlbastru.ReadOnly = true;
            _textBoxMutariAlbastru.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlbastru.Location = new System.Drawing.Point(536, 82);
            _textBoxMutariAlbastru.Name = "textBoxMutariAlb";
            _textBoxMutariAlbastru.Size = new System.Drawing.Size(155, 210);
            _textBoxMutariAlbastru.TabIndex = 0;
            _textBoxMutariAlbastru.RightToLeft = RightToLeft.No;
            _textBoxMutariAlbastru.Text = "Mutari AI:\n";

            _textBoxMutariAlb = new RichTextBox();
            _textBoxMutariAlb.ReadOnly = true;
            _textBoxMutariAlb.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlb.Location = new System.Drawing.Point(536, 344);
            _textBoxMutariAlb.Name = "textBoxMutariAlbastru";
            _textBoxMutariAlb.Size = new System.Drawing.Size(155, 210);
            _textBoxMutariAlb.TabIndex = 1;
            _textBoxMutariAlb.RightToLeft = RightToLeft.No;
            _textBoxMutariAlb.Text = "Mutari Jucator:\n";

            ParentForm.Controls.Add(_textBoxMutariAlbastru);
            ParentForm.Controls.Add(_textBoxMutariAlb);
        }

        private void ScrieUltimaMutareInTextBox(RichTextBox textBox)
        {
            string ultimaMutareString = string.Format("    ({0},{1}) -> ({2},{3})", UltimaMutare.PozitieInitiala.Linie, (char)('A' + UltimaMutare.PozitieInitiala.Coloana), UltimaMutare.PozitieFinala.Linie, (char)('A' + UltimaMutare.PozitieFinala.Coloana));
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(textBox, "Text", $"{UtilitatiCrossThread.PrimesteTextulDinAltThread(textBox)}{Environment.NewLine}{ultimaMutareString}");
        }

        protected override void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie, bool logMove = true)
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
                    Pozitie pozitie = (sender as Cadran).PozitieCadran;

                    if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                    {
                        Piesa piesa = GetPiesaCuPozitia(pozitie);

                        if (piesa != null)
                        {
                            if (piesa.Culoare != _jucatorOm.Culoare)
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
                    Pozitie pozitie = (sender as Cadran).PozitieCadran;

                    if (PiesaSelectata.Pozitie != pozitie)
                    {
                        if (EsteMutareaPosibila(pozitie))
                        {
                            if (MatriceCoduriPiese[pozitie.Linie][pozitie.Coloana] != (int)CodPiesa.Gol)
                            {
                                ConstantaSunet.SunetPiesaLuata.Play();
                            }
                            else
                            {
                                ConstantaSunet.SunetPiesaMutata.Play();
                            }
                            NuEsteRandulTau();
                            PornesteTimerAsteptareAI();
                            RealizeazaMutareaLocal(PiesaSelectata, pozitie);
                            ScrieUltimaMutareInTextBox(_textBoxMutariAlb);

                            VerificaSahul();
                            _jucatorOm.UltimaPozitie = pozitie;
                            if (_esteGataMeciul == false)
                            {
                                await Task.Run(() =>
                                {
                                    RealizeazaMutareaAI(moveOrdering: true);
                                });
                                VerificaSahul();
                                VerificaSahFaraMutariAlb();
                            }
                            ScrieUltimaMutareInTextBox(_textBoxMutariAlbastru);
                        }
                    }
                    else
                    {
                        AscundePiesaSelectata(PiesaSelectata);
                        Pozitie pozitieInitiala = PiesaSelectata.Pozitie;
                        DecoloreazaMutariPosibile();
                        PiesaSelectata = ConstantaTabla.PiesaNula;
                        PozitiiMutariPosibile.Clear();
                    }
                }
            }
        }

        private void VerificaSahul()
        {
            TipSah tipSah = base.VerificaSahurile();
            if (tipSah != TipSah.NuEsteSah)
            {
                TerminaMeciul(tipSah);
            }
        }

        public void DeschideJocul(int ms = 1000)
        {
            if (_randulOmului == false)
            {
                System.Timers.Timer _timerMutare = new()
                {
                    Interval = ms,
                    Enabled = false,
                    AutoReset = false
                };
                _timerMutare.Elapsed += new ElapsedEventHandler(DeschideCuMutareaAI);
                _timerMutare.Start();
            }
            else
            {
                UtilitatiCrossThread.SeteazaProprietateaDinAltThread(LabelAsteptare, "Text", "Este randul tau.");
            }
        }

        private void DeschideCuMutareaAI(object source, ElapsedEventArgs e)
        {
            VerificaSahFaraMutariAlb();
            RealizeazaMutareaAI(moveOrdering: true);
        }

        public void RealizeazaMutareaAI(bool moveOrdering = true)
        {
            Stopwatch cronometru = new();
            cronometru.Start();

            //evaluarea minimax primeste mutarile ai-ului ca si primul parametru
            unchecked
            {
                var mutareaOptima = _jucatorAI.ReturneazaMutareaOptima();
                if (mutareaOptima == null)
                    return;
                Piesa piesa = GetPiesaCuPozitia(mutareaOptima.Item1.PozitieInitiala);
                Pozitie pozitie = mutareaOptima.Item1.PozitieFinala;
                RealizeazaMutareaLocal(piesa, pozitie);

                OpresteTimerAsteptareAI();
                _jucatorAI.UltimaPozitie = pozitie;
                EsteRandulTau();
            }
            Debug.WriteLine(cronometru.Elapsed);
            cronometru.Stop();
        }


        public override void TerminaMeciul(TipSah tipSah = TipSah.Nespecificat)
        {
            StergeEvenimenteleCadranelor();
            _esteGataMeciul = true;
            base.TerminaMeciul(tipSah);
        }

        private void StergeEvenimenteleCadranelor()
        {
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie][coloana].Click -= OnCadranClick;
                }
            }
        }
    }
}