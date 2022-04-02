using System;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class HostSah : EngineHost
    {
        public static uint IntervalTimerVizual = 50;

        private Label _labelConexiuneLocala;
        private Label _labelConexiuneSocket;
        private Label _labelRand;
        Label _labelMutariAlbastru;
        Label _labelMutariAlb;

        RichTextBox _textBoxMutariAlb;
        RichTextBox _textBoxMutariAlbastru;

        private Form _parentForm;

        private System.Timers.Timer _timerHost;
        private System.Timers.Timer _timerStatusClient;

        private delegate void DelegatProprietateCrossThread(Control control,
                                                             string propertyName,
                                                             object propertyValue);

        public HostSah(Form parentForm, Om jucator) : base(parentForm, jucator)
        {
            _parentForm = parentForm;
            _parentForm.FormClosing += new FormClosingEventHandler(FormClosing_Event);
        }

        public HostSah(Form parentForm, int[,] matriceTabla, Om jucator) : base(parentForm, matriceTabla, jucator)
        {
            _parentForm = parentForm;
            _parentForm.FormClosing += new FormClosingEventHandler(FormClosing_Event);
        }
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
        ~HostSah() => Dispose();

        public override void Dispose()
        {
            Debug.WriteLine("Dispose SahMultiplayer");
            GC.SuppressFinalize(this);
            _labelConexiuneSocket.Dispose();
            _labelConexiuneLocala.Dispose();
            _textBoxMutariAlb.Dispose();
            _textBoxMutariAlbastru.Dispose();
            _labelRand.Dispose();
            _timerStatusClient.Dispose();
            base.Dispose();
        }

        private void FormClosing_Event(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        public override void HosteazaJoc(int port)
        {
            InitializeazaInterfataVizuala();
            EsteRandulTau();
            ActiveazaTimerRepetitiv(ref _timerHost, (uint)EngineHost.IntervalTimerPrimireDate, VerificaPrimireHost);
            ActiveazaTimerRepetitiv(ref _timerStatusClient, IntervalTimerVizual, DeconecteazaClientulVizual);
            base.HosteazaJoc(port);
        }

        public void DeconecteazaClientulVizual(object source, System.Timers.ElapsedEventArgs e)
        {
            if (_timerJocHostDisposed == true)
            {
                _timerStatusClient.Dispose();
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.DarkRed);
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Client Deconectat");
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Size", new System.Drawing.Size(200, 40));
            }
        }

        protected override void NuEsteRandulTau()
        {
            SeteazaProprietateaDinAltThread(_labelRand, "BackColor", Color.DarkRed);
            SeteazaProprietateaDinAltThread(_labelRand, "Text", "Mutarea Lui");
            base.NuEsteRandulTau();
        }

        protected override void RealizeazaMutareaLocal(Piesa piesa, Pozitie pozitie)
        {
            base.RealizeazaMutareaLocal(piesa, pozitie);
            if (RandulTau == true)
            {
                ScrieUltimaMutareInTextBox(MutariAlbastru);
            }
            else
            {
                ScrieUltimaMutareInTextBox(MutariAlb);
            }
        }
        private void ScrieUltimaMutareInTextBox(RichTextBox textBox)
        {
            String ultimaMutareString = String.Format("     ({0},{1})->({2},{3})", UltimaMutare.Item1.Linie, UltimaMutare.Item1.Coloana, UltimaMutare.Item2.Linie, UltimaMutare.Item2.Linie);
            SeteazaProprietateaDinAltThread(textBox, "Text",ultimaMutareString);
        }

        protected override void EsteRandulTau()
        {
            SeteazaProprietateaDinAltThread(_labelRand, "BackColor", Color.Green);
            SeteazaProprietateaDinAltThread(_labelRand, "Text", "Mutarea Ta");
            base.EsteRandulTau();
        }

        public void VerificaPrimireHost(object source, System.Timers.ElapsedEventArgs e)
        {
            SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.Green);
            SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Client primit");
            _timerHost.Dispose();
            _timerStatusClient.Start();
        }

        public static void SeteazaProprietateaDinAltThread(Control control,
                                                    string propertyName,
                                                    object propertyValue)
        {
            if (control.IsDisposed == false)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new DelegatProprietateCrossThread(SeteazaProprietateaDinAltThread),
                                                new object[] { control, propertyName, propertyValue });
                }
                else
                {
                    control.GetType().InvokeMember(
                        propertyName,
                        BindingFlags.SetProperty,
                        null,
                        control,
                        new object[] { propertyValue });
                }
            }
        }

        public void InitializeazaInterfataVizuala()
        {
            _labelConexiuneLocala = new System.Windows.Forms.Label();
            _labelConexiuneLocala.Parent = this._parentForm;
            _labelConexiuneLocala.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneLocala.Font = new System.Drawing.Font(ConstantaTabla.FontSecundar, 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneLocala.Location = new System.Drawing.Point(200, 602);
            _labelConexiuneLocala.Name = "labelJocConectat";
            _labelConexiuneLocala.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneLocala.TabIndex = 0;
            _labelConexiuneLocala.Text = "Server pornit";
            _labelConexiuneLocala.BackColor = Color.Green;

            _labelConexiuneSocket = new System.Windows.Forms.Label();
            _labelConexiuneSocket.Parent = this._parentForm;
            _labelConexiuneSocket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneSocket.Font = new System.Drawing.Font(ConstantaTabla.FontSecundar, 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneSocket.Location = new System.Drawing.Point(200, 675);
            _labelConexiuneSocket.Name = "labelClientConectat";
            _labelConexiuneSocket.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneSocket.TabIndex = 1;
            _labelConexiuneSocket.Text = "Client Oprit";
            _labelConexiuneSocket.BackColor = Color.DarkRed;

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
            _textBoxMutariAlb.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlb.Location = new System.Drawing.Point(536, 82);
            _textBoxMutariAlb.Name = "textBoxMutariAlb";
            _textBoxMutariAlb.Size = new System.Drawing.Size(200, 200);
            _textBoxMutariAlb.TabIndex = 0;
            _textBoxMutariAlb.Text = "";

            _textBoxMutariAlbastru = new RichTextBox();
            _textBoxMutariAlbastru.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlbastru.Location = new System.Drawing.Point(536, 344);
            _textBoxMutariAlbastru.Name = "textBoxMutariAlbastru";
            _textBoxMutariAlbastru.Size = new System.Drawing.Size(200, 200);
            _textBoxMutariAlbastru.TabIndex = 1;
            _textBoxMutariAlbastru.Text = "";

            _labelMutariAlbastru = new Label();
            _labelMutariAlbastru.AutoSize = true;
            _labelMutariAlbastru.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelMutariAlbastru.Location = new System.Drawing.Point(566, 58);
            _labelMutariAlbastru.Name = "richTextBoxMutariAlbastru";
            _labelMutariAlbastru.Size = new System.Drawing.Size(134, 21);
            _labelMutariAlbastru.TabIndex = 2;
            _labelMutariAlbastru.Text = "Mutari Albastru:";

            _labelMutariAlb = new Label();
            _labelMutariAlb.AutoSize = true;
            _labelMutariAlb.Font = new System.Drawing.Font(ConstantaTabla.FontPrincipal, 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelMutariAlb.Location = new System.Drawing.Point(585, 320);
            _labelMutariAlb.Name = "richTextBoxMutariAlb";
            _labelMutariAlb.Size = new System.Drawing.Size(94, 21);
            _labelMutariAlb.TabIndex = 3;
            _labelMutariAlb.Text = "Mutari Alb:";

            _parentForm.Controls.Add(_labelMutariAlb);
            _parentForm.Controls.Add(_labelMutariAlbastru);
            _parentForm.Controls.Add(_textBoxMutariAlb);
            _parentForm.Controls.Add(_textBoxMutariAlbastru);
            _parentForm.Controls.Add(_labelRand);
            _parentForm.Controls.Add(_labelConexiuneSocket);
            _parentForm.Controls.Add(_labelConexiuneLocala);
            _parentForm.Controls.Add(_labelConexiuneLocala);
        }
    }
}