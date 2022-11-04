using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
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

        protected System.Timers.Timer _timerAsteptareAI;
        protected short _valoareTimerAsteptare;

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

        public EngineMiniMax(Form parentForm, Om jucator) : base(parentForm)
        {
            InitializeazaInterfataVizuala();
            AdaugaEvenimentCadrane();
            _jucatorOm = jucator;
            _miniMaxAI = new MiniMaxAI(Culoare.Albastru, this);

            _randulOmului = true;
            InitializeazaTimerAsteptare();
            EsteRandulTau();
        }

        public EngineMiniMax(Form parentForm, int[,] matriceTabla, Om jucator) : base(parentForm, matriceTabla)
        {
            InitializeazaInterfataVizuala();
            AdaugaEvenimentCadrane();
            _jucatorOm = jucator;
            _miniMaxAI = new MiniMaxAI(Culoare.Albastru, this);
            _randulOmului = false;
            InitializeazaTimerAsteptare();
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
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(LabelAsteptare, "Text", String.Empty);
        }
        public void AdaugaEvenimentCadrane()
        {

            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
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
            _textBoxMutariAlb.Text = "Mutari Jucator:\n";

            _textBoxMutariAlbastru = new RichTextBox();
            _textBoxMutariAlbastru.ReadOnly = true;
            _textBoxMutariAlbastru.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlbastru.Location = new System.Drawing.Point(536, 344);
            _textBoxMutariAlbastru.Name = "textBoxMutariAlbastru";
            _textBoxMutariAlbastru.Size = new System.Drawing.Size(155, 210);
            _textBoxMutariAlbastru.TabIndex = 1;
            _textBoxMutariAlbastru.RightToLeft = RightToLeft.No;
            _textBoxMutariAlbastru.Text = "Mutari AI:\n";

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
            PornesteTimerAsteptareAI();
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
                            VerificaSahulPersistent();
                            RealizeazaMutareaLocal(PiesaSelectata, pozitie);
                            ScrieUltimaMutareInTextBox(_textBoxMutariAlb);
                            _jucatorOm.UltimaPozitie = pozitie;
                            if (_esteGataMeciul == false)
                            {
                                await Task.Run(() =>
                                {
                                    RealizeazaMutareaAI();
                                    VerificaSahulPersistent();
                                });
                            }
                        }
                    }
                    else
                    {
                        AscundePiesaSelectata(PiesaSelectata);
                        Pozitie pozitieInitiala = PiesaSelectata.Pozitie;
                        DecoloreazaMutariPosibile(PozitiiMutariPosibile);
                        PiesaSelectata = ConstantaTabla.PiesaNula;
                        PozitiiMutariPosibile.Clear();
                    }
                }
            }
        }

        public void RealizeazaMutareaAI()
        {
            Stopwatch cronometru = new();
            cronometru.Start();

            var tupluMutariSiMatriciPosibile = _miniMaxAI.CalculeazaPrimeleMutariAI();
            var mutareaOptima = _miniMaxAI.EvalueazaMutarileAI(tupluMutariSiMatriciPosibile);
            
            Piesa piesa = GetPiesaCuPozitia(mutareaOptima.Item1.Item1);
            Pozitie pozitie = mutareaOptima.Item1.Item2;
            Piesa piesaLuata = GetPiesaCuPozitia(mutareaOptima.Item1.Item2);
            if (piesaLuata != ConstantaTabla.PiesaNula)
            {
                if (piesaLuata.Cod == CodPiesa.RegeAlb)
                {
                    TerminaMeciul();
                }
            }
            RealizeazaMutareaLocal(piesa, pozitie);
            _miniMaxAI.UltimaPozitie = pozitie;
            EsteRandulTau();
            Debug.WriteLine(cronometru.Elapsed);
            cronometru.Stop();
            OpresteTimerAsteptareAI();
            ScrieUltimaMutareInTextBox(_textBoxMutariAlbastru);
        }


        private int VerificaTentativaDeSah()
        {
            foreach (Cadran cadran in ArrayCadrane)
            {
                if (cadran.PiesaCadran != ConstantaTabla.PiesaNula)
                {
                    List<Pozitie> mutari = cadran.PiesaCadran.ReturneazaMutariPosibile(this.MatriceCoduriPiese);
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


        public void TerminaMeciul(TipSah tipSah = TipSah.Nespecificat)
        {
            switch(tipSah)
            {
                case TipSah.Nespecificat:
                    {
                        MessageBox.Show("1");
                        break;
                    }
                case TipSah.RegeAlbLuat:
                    {
                        MessageBox.Show("2");
                        break;
                    }
                case TipSah.RegeAlbastruLuat:
                    {
                        MessageBox.Show("1");
                        break;
                    }
                case TipSah.SahPersistentAlb:
                    {
                        MessageBox.Show("1");
                        break;
                    }
                case TipSah.SahPersistentAlbastru:
                    {
                        MessageBox.Show("1");
                        break;
                    }
                case TipSah.FaraMutariAlb:
                    {
                        MessageBox.Show("1");
                        break;
                    }
                case TipSah.FaraMutariAlbastru:
                    {
                        MessageBox.Show("1");
                        break;
                    }
                case TipSah.AbandonAlb:
                    {
                        MessageBox.Show("1");
                        break;
                    }
                case TipSah.AbandonAlbastru:
                    {
                        MessageBox.Show("1");
                        break;
                    }
            }
            _esteGataMeciul = true;
            StergeEvenimenteleCadranelor();
        }

        private void StergeEvenimenteleCadranelor()
        {
            for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
            {
                for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
                {
                    ArrayCadrane[linie, coloana].Click -= OnCadranClick;
                }
            }
        }
        private void VerificaSahulPersistent()
        {
            int codRegeAlb = (int)CodPiesa.RegeAlb;
            int codRegeAlbastru = (int)CodPiesa.RegeAlbastru;
            bool esteSah = false;
            foreach (var piesa in ListaPieseAlbe)
            {
                var mutariPosibile = piesa.ReturneazaMutariPosibile(_matriceCodPiese);
                foreach(var mutare in mutariPosibile)
                {
                    if (_matriceCodPiese[mutare.Linie, mutare.Coloana] == codRegeAlbastru)
                    {
                        _nrSahuriLaAlbastru++;
                        if (_nrSahuriLaAlbastru == 3)
                        {
                            MessageBox.Show("SEX§");
                            TerminaMeciul();
                            esteSah = true;
                            break;
                        }
                    }
                }
            }
            if(esteSah == false)
            {
                _nrSahuriLaAlbastru = 0;
            }
            esteSah = false;
            foreach (var piesa in ListaPieseAlbastre)
            {
                var mutariPosibile = piesa.ReturneazaMutariPosibile(_matriceCodPiese);
                foreach (var mutare in mutariPosibile)
                {
                    if (_matriceCodPiese[mutare.Linie, mutare.Coloana] == codRegeAlb)
                    {
                        _nrSahuriLaAlb++;
                        if (_nrSahuriLaAlb == 3)
                        {
                            MessageBox.Show("SEX§2");
                            TerminaMeciul();
                            esteSah = true;
                            break;
                        }
                    }
                }
            }
            if (esteSah == false)
            {
                _nrSahuriLaAlb = 0;
            }
        }

    }
}