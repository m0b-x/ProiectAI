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
        private Label _labelMutare;

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

        ~HostSah() => Dispose();

        public override void Dispose()
        {
            Debug.WriteLine("Dispose SahMultiplayer");
            GC.SuppressFinalize(this);
            _labelConexiuneSocket.Dispose();
            _labelConexiuneLocala.Dispose();
            _textBoxMutariAlb.Dispose();
            _textBoxMutariAlbastru.Dispose();
            _labelMutare.Dispose();
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
            SeteazaProprietateaDinAltThread(_labelMutare, "BackColor", Color.DarkRed);
            SeteazaProprietateaDinAltThread(_labelMutare, "Text", "Mutarea Lui");
            base.NuEsteRandulTau();
        }

        protected override void EsteRandulTau()
        {
            SeteazaProprietateaDinAltThread(_labelMutare, "BackColor", Color.Green);
            SeteazaProprietateaDinAltThread(_labelMutare, "Text", "Mutarea Ta");
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
            _parentForm.Controls.Add(_labelConexiuneLocala);
            _labelConexiuneLocala.Parent = this._parentForm;
            _labelConexiuneLocala.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneLocala.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneLocala.Location = new System.Drawing.Point(200, 602);
            _labelConexiuneLocala.Name = "labelJocConectat";
            _labelConexiuneLocala.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneLocala.TabIndex = 0;
            _labelConexiuneLocala.Text = "Server pornit";
            _labelConexiuneLocala.BackColor = Color.Green;

            _labelConexiuneSocket = new System.Windows.Forms.Label();
            _parentForm.Controls.Add(_labelConexiuneSocket);
            _labelConexiuneSocket.Parent = this._parentForm;
            _labelConexiuneSocket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneSocket.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneSocket.Location = new System.Drawing.Point(200, 675);
            _labelConexiuneSocket.Name = "labelClientConectat";
            _labelConexiuneSocket.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneSocket.TabIndex = 1;
            _labelConexiuneSocket.Text = "Client Oprit";
            _labelConexiuneSocket.BackColor = Color.DarkRed;

            _labelMutare = new System.Windows.Forms.Label();
            _parentForm.Controls.Add(_labelMutare);
            _labelMutare.Parent = this._parentForm;
            _labelMutare.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelMutare.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelMutare.Location = new System.Drawing.Point(200, 10);
            _labelMutare.Name = "labelMutare";
            _labelMutare.Size = new System.Drawing.Size(140, 35);
            _labelMutare.TabIndex = 1;
            _labelMutare.Text = "Mutarea Ta";
            _labelMutare.BackColor = Color.Green;

            _textBoxMutariAlb = new RichTextBox();
            _textBoxMutariAlb.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlb.Location = new System.Drawing.Point(536, 82);
            _textBoxMutariAlb.Name = "textBoxMutariAlb";
            _textBoxMutariAlb.Size = new System.Drawing.Size(200, 200);
            _textBoxMutariAlb.TabIndex = 0;
            _textBoxMutariAlb.Text = "";

            _textBoxMutariAlbastru = new RichTextBox();
            _textBoxMutariAlbastru.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _textBoxMutariAlbastru.Location = new System.Drawing.Point(536, 344);
            _textBoxMutariAlbastru.Name = "textBoxMutariAlbastru";
            _textBoxMutariAlbastru.Size = new System.Drawing.Size(200, 200);
            _textBoxMutariAlbastru.TabIndex = 1;
            _textBoxMutariAlbastru.Text = "";
        }

    }
}