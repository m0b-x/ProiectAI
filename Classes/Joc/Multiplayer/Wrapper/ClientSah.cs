using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class ClientSah : EngineClient, IDisposable
    {
        public static uint TimpTimerVizual = 50;

        private Label _labelConexiuneLocala;
        private Label _labelConexiuneSocket;
        private Form _parentForm;
        private Label _labelRand;
        private RichTextBox _textBoxMutariAlb;
        private RichTextBox _textBoxMutariAlbastru;
        private Label _labelMutariAlbastru;
        private Label _labelMutariAlb;

        private System.Timers.Timer _timerClient;
        private System.Timers.Timer _timerStatusServer;

        public RichTextBox MutariAlb
        {
            get { return _textBoxMutariAlb; }
            set { _textBoxMutariAlb = value; }
        }

        public RichTextBox MutariAlbastru
        {
            get { return _textBoxMutariAlbastru; }
            set { _textBoxMutariAlbastru = value; }
        }

        public ClientSah(Form parentForm, Om jucator) : base(parentForm, jucator)
        {
            _parentForm = parentForm;
            _parentForm.FormClosing += new FormClosingEventHandler(FormClosing_Event);
        }

        public ClientSah(Form parentForm, int[][] matriceTabla, Om jucator) : base(parentForm, matriceTabla, jucator)
        {
            _parentForm = parentForm;
            _parentForm.FormClosing += new FormClosingEventHandler(FormClosing_Event);
        }

        ~ClientSah() => Dispose();

        public override void Dispose()
        {
            Debug.WriteLine("Dispose SahMultiplayer");
            GC.SuppressFinalize(this);
            _labelConexiuneSocket.Dispose();
            _labelConexiuneLocala.Dispose();
            _timerStatusServer.Dispose();
            _textBoxMutariAlb.Dispose();
            _textBoxMutariAlbastru.Dispose();
            base.Dispose();
        }

        private void FormClosing_Event(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        public override void ConecteazateLaJoc(IPAddress adresaIP, int port)
        {
            InitializeazaInterfataVizuala();
            NuEsteRandulTau();
            ActiveazaTimerRepetitiv( _timerClient, (uint)EngineClient.IntervalTimerPrimireDate, VerificareConexiuneCuHostul);
            ActiveazaTimerRepetitiv( _timerStatusServer, TimpTimerVizual, DeconecteazaServerulVizual);
            base.ConecteazateLaJoc(adresaIP, port);
        }

        public void DeconecteazaServerulVizual(object source, System.Timers.ElapsedEventArgs e)
        {
            if (_timerJocClientDisposed == true)
            {
                _timerStatusServer.Dispose();
                UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.DarkRed);
                UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Server Deconectat");
                UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Size", new System.Drawing.Size(200, 40));
            }
        }

        protected override void EsteRandulTau()
        {
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelRand, "BackColor", Color.Green);
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelRand, "Text", "Mutarea Ta");
            base.EsteRandulTau();
        }

        protected override void NuEsteRandulTau()
        {
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelRand, "BackColor", Color.DarkRed);
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelRand, "Text", "Mutarea Lui");
            base.NuEsteRandulTau();
        }

        protected override void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie, bool logMove = true)
        {
            base.RealizeazaMutareaLocal(piesa, pozitie);
            if (RandulTau == true)
            {
                ScrieUltimaMutareInTextBox(MutariAlb);
            }
            else
            {
                ScrieUltimaMutareInTextBox(MutariAlbastru);
            }
        }

        private void ScrieUltimaMutareInTextBox(RichTextBox textBox)
        {
            String ultimaMutareString = String.Format("    ({0},{1}) -> ({2},{3})", UltimaMutare.PozitieInitiala.Linie, (char)('A' + UltimaMutare.PozitieInitiala.Coloana), UltimaMutare.PozitieFinala.Linie, (char)('A' + UltimaMutare.PozitieFinala.Coloana));
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(textBox, "Text", $"{UtilitatiCrossThread.PrimesteTextulDinAltThread(textBox)}{Environment.NewLine}{ultimaMutareString}");
        }

        public void VerificareConexiuneCuHostul(object source, System.Timers.ElapsedEventArgs e)
        {
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.Green);
            UtilitatiCrossThread.SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Server primit");
            _timerClient.Dispose();
        }

        public void InitializeazaInterfataVizuala()
        {
            _labelConexiuneLocala = new System.Windows.Forms.Label();
            _labelConexiuneLocala.Parent = this._parentForm;
            _labelConexiuneLocala.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneLocala.Font = new System.Drawing.Font(ConstantaTabla.FontSecundar, 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneLocala.Location = new System.Drawing.Point(200, 675);
            _labelConexiuneLocala.Name = "labelJocConectat";
            _labelConexiuneLocala.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneLocala.TabIndex = 0;
            _labelConexiuneLocala.Text = "Client Pornit";
            _labelConexiuneLocala.BackColor = Color.Green;

            _labelConexiuneSocket = new System.Windows.Forms.Label();
            _labelConexiuneSocket.Parent = this._parentForm;
            _labelConexiuneSocket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneSocket.Font = new System.Drawing.Font(ConstantaTabla.FontSecundar, 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneSocket.Location = new System.Drawing.Point(200, 610);
            _labelConexiuneSocket.Name = "labelClientConectat";
            _labelConexiuneSocket.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneSocket.TabIndex = 1;
            _labelConexiuneSocket.BackColor = Color.DarkRed;
            _labelConexiuneSocket.Text = "Se Conecteaza";

            _labelRand = new System.Windows.Forms.Label();
            _labelRand.Parent = this._parentForm;
            _labelRand.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelRand.Font = new System.Drawing.Font(ConstantaTabla.FontSecundar, 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelRand.Location = new System.Drawing.Point(200, 10);
            _labelRand.Name = "labelMutare";
            _labelRand.Size = new System.Drawing.Size(140, 35);
            _labelRand.TabIndex = 1;
            _labelRand.Text = "Mutarea Ta";
            _labelRand.BackColor = Color.Green;

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

            _labelMutariAlbastru = new Label();
            _labelMutariAlbastru.AutoSize = true;
            _labelMutariAlbastru.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelMutariAlbastru.Location = new System.Drawing.Point(550, 58);
            _labelMutariAlbastru.Name = "richTextBoxMutariAlbastru";
            _labelMutariAlbastru.Size = new System.Drawing.Size(134, 21);
            _labelMutariAlbastru.TabIndex = 2;
            _labelMutariAlbastru.Text = "Mutari Albastru:";

            _labelMutariAlb = new Label();
            _labelMutariAlb.AutoSize = true;
            _labelMutariAlb.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelMutariAlb.Location = new System.Drawing.Point(550, 320);
            _labelMutariAlb.Name = "richTextBoxMutariAlb";
            _labelMutariAlb.Size = new System.Drawing.Size(134, 21);
            _labelMutariAlb.TabIndex = 3;
            _labelMutariAlb.Text = "Mutari Alb:";

            _parentForm.Controls.Add(_labelMutariAlb);
            _parentForm.Controls.Add(_labelMutariAlbastru);
            _parentForm.Controls.Add(_textBoxMutariAlb);
            _parentForm.Controls.Add(_textBoxMutariAlbastru);
            _parentForm.Controls.Add(_labelRand);
            _parentForm.Controls.Add(_labelConexiuneSocket);
            _parentForm.Controls.Add(_labelConexiuneLocala);
        }
    }
}