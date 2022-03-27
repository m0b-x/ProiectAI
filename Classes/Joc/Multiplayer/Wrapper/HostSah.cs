using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class HostSah : EngineHost
    {
        private Label _labelConexiuneLocala;
        private Label _labelConexiuneSocket;
        private Label _labelMutare;

        private Form _parentForm;

        private System.Timers.Timer _timerHost;
        private System.Timers.Timer _timerMutare;

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
            if (_labelConexiuneSocket != null)
            {
                _labelConexiuneSocket.Dispose();
            }
            if (_labelConexiuneLocala != null)
            {
                _labelConexiuneLocala.Dispose();
            }
            if (_labelMutare != null)
            {
                _labelMutare.Dispose();
            }
            _timerMutare.Dispose();
            base.Dispose();
        }

        private void FormClosing_Event(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        public override void HosteazaJoc(int port)
        {
            InitializeazaLabeleHost();
            _timerHost = new System.Timers.Timer();
            _timerHost.Enabled = true;
            _timerHost.Interval = 100;
            _timerHost.Elapsed += new System.Timers.ElapsedEventHandler(VerificaPrimireHost);
            _timerHost.AutoReset = true;

            _timerMutare = new System.Timers.Timer();
            _timerMutare.Enabled = true;
            _timerMutare.Interval = 100;
            _timerMutare.Elapsed += new System.Timers.ElapsedEventHandler(VerificaMutare);
            _timerMutare.AutoReset = true;
            base.HosteazaJoc(port);
        }
        public void VerificaMutare(object source, System.Timers.ElapsedEventArgs e)
        {
            if (_esteRandulHostului)
            {
                if (_labelMutare.Text == "Mutarea Lui")
                {
                    SeteazaProprietateaDinAltThread(_labelMutare, "BackColor", Color.Green);
                    SeteazaProprietateaDinAltThread(_labelMutare, "Text", "Mutarea Ta");
                    _labelMutare.Text = "Mutarea Ta";
                }
            }
            else
            {
                if (_labelMutare.Text == "Mutarea Ta")
                {
                    SeteazaProprietateaDinAltThread(_labelMutare, "BackColor", Color.DarkRed);
                    SeteazaProprietateaDinAltThread(_labelMutare, "Text", "Mutarea Lui");
                    _labelMutare.Text = "Mutarea Lui";
                }
            }
            if(_timerJocHostDisposed == true)
            {
                _timerMutare.Dispose();
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.DarkRed);
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Client Deconectat");
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Size", new System.Drawing.Size(200, 40));
            }
        }

        public void VerificaPrimireHost(object source, System.Timers.ElapsedEventArgs e)
        {
            SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.Green);
            SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Client primit");
            _timerHost.Dispose();
            _timerMutare.Start();
        }


        public void InitializeazaLabeleHost()
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
            _labelConexiuneLocala.Refresh();

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
    }
}
