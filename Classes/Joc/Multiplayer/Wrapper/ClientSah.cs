using System;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Reflection;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class ClientSah : EngineClient, IDisposable
    {
        public static uint TimpTimerVizual = 50;

        private Label _labelConexiuneLocala;
        private Label _labelConexiuneSocket;
        private Form _parentForm;
        private Label _labelMutare;
        RichTextBox _textBoxMutariAlb;
        RichTextBox _textBoxMutariAlbastru;

        private System.Timers.Timer _timerClient;
        private System.Timers.Timer _timerStatusServer;

        private delegate void _DelegatCrossThread(Control control,
                                                    string propertyName,
                                                    object propertyValue);

        public ClientSah(Form parentForm, Om jucator) : base(parentForm, jucator)
        {
            _parentForm = parentForm;
            _parentForm.FormClosing += new FormClosingEventHandler(FormClosing_Event);
        }

        public ClientSah(Form parentForm, int[,] matriceTabla, Om jucator) : base(parentForm, matriceTabla, jucator)
        {
            _parentForm = parentForm;
            _parentForm.FormClosing += new FormClosingEventHandler(FormClosing_Event);
        }

        ~ClientSah() => Dispose();

        public override void Dispose()
        {
            Debug.WriteLine("Dispose SahMultiplayer");
            GC.SuppressFinalize(this);
            if (_labelConexiuneSocket != null)
            {
                _labelConexiuneSocket.Dispose();
            }
            if (_labelConexiuneLocala != null)
            {
                _labelConexiuneLocala.Dispose();
            }
            _timerStatusServer.Dispose();
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
            ActiveazaTimerRepetitiv(ref _timerClient, (uint)EngineClient.IntervalTimerPrimireDate, VerificareConexiuneCuHostul);
            ActiveazaTimerRepetitiv(ref _timerStatusServer, TimpTimerVizual, DeconecteazaServerulVizual);
            base.ConecteazateLaJoc(adresaIP, port);
        }

        public void DeconecteazaServerulVizual(object source, System.Timers.ElapsedEventArgs e)
        {
            if (_timerJocClientDisposed == true)
            {
                _timerStatusServer.Dispose();
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.DarkRed);
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Server Deconectat");
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Size", new System.Drawing.Size(200, 40));
            }
        }

        protected override void EsteRandulTau()
        {
            SeteazaProprietateaDinAltThread(_labelMutare, "BackColor", Color.Green);
            SeteazaProprietateaDinAltThread(_labelMutare, "Text", "Mutarea Ta");
            base.EsteRandulTau();
        }

        protected override void NuEsteRandulTau()
        {
            SeteazaProprietateaDinAltThread(_labelMutare, "BackColor", Color.DarkRed);
            SeteazaProprietateaDinAltThread(_labelMutare, "Text", "Mutarea Lui");
            base.NuEsteRandulTau();
        }

        public void VerificareConexiuneCuHostul(object source, System.Timers.ElapsedEventArgs e)
        {
            SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.Green);
            SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Server primit");
            _timerClient.Dispose();
        }

        public void InitializeazaInterfataVizuala()
        {
            _labelConexiuneLocala = new System.Windows.Forms.Label();
            _parentForm.Controls.Add(_labelConexiuneLocala);
            _labelConexiuneLocala.Parent = this._parentForm;
            _labelConexiuneLocala.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneLocala.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneLocala.Location = new System.Drawing.Point(200, 675);
            _labelConexiuneLocala.Name = "labelJocConectat";
            _labelConexiuneLocala.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneLocala.TabIndex = 0;
            _labelConexiuneLocala.Text = "Client Pornit";
            _labelConexiuneLocala.BackColor = Color.Green;
            _labelConexiuneLocala.Refresh();

            _labelConexiuneSocket = new System.Windows.Forms.Label();
            _parentForm.Controls.Add(_labelConexiuneSocket);
            _labelConexiuneSocket.Parent = this._parentForm;
            _labelConexiuneSocket.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneSocket.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneSocket.Location = new System.Drawing.Point(200, 602);
            _labelConexiuneSocket.Name = "labelClientConectat";
            _labelConexiuneSocket.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneSocket.TabIndex = 1;
            _labelConexiuneSocket.BackColor = Color.DarkRed;
            _labelConexiuneSocket.Text = "Se Conecteaza";
            _labelConexiuneSocket.Refresh();

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
            _labelMutare.Refresh();

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

        public static void SeteazaProprietateaDinAltThread(Control control,
                                                    string propertyName,
                                                    object propertyValue)
        {
            if (control.IsDisposed == false)
            {
                if (control.InvokeRequired)
                {
                    control.Invoke(new _DelegatCrossThread(SeteazaProprietateaDinAltThread),
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
    }
}