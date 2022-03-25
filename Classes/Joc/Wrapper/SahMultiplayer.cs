﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class SahMultiplayer : JocMultiplayer,IDisposable
    {        
        Label _labelConexiuneLocala;
        Label _labelConexiuneSocket;
        Form _parentForm;
        System.Timers.Timer timerHost;
        System.Timers.Timer timerClient;

        private delegate void DelegatProprietateCrossThread( Control control,
                                                             string propertyName,
                                                             object propertyValue);

        public SahMultiplayer(Form parentForm, ref Tuple<Om, Om> jucatori) : base(parentForm, ref jucatori)
        {
            _parentForm = parentForm;
            _parentForm.FormClosing += new FormClosingEventHandler(FormClosing_Event);
        }
        public SahMultiplayer(Form parentForm, int[,] matriceTabla, ref Tuple<Om, Om> jucatori) : base(parentForm, matriceTabla, ref jucatori)
        {
            _parentForm = parentForm;
            _parentForm.FormClosing += new FormClosingEventHandler(FormClosing_Event);
        }
        ~SahMultiplayer() => Dispose();

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
            base.Dispose();
        }

        private void FormClosing_Event(object sender, FormClosingEventArgs e)
        {
            Dispose();
        }

        public override void HosteazaJoc(int port)
        {
            InitializeazaLabeleHost();
            base.HosteazaJoc(port);
            timerHost = new System.Timers.Timer();
            timerHost.Enabled = true;
            timerHost.Interval = 100;
            timerHost.Elapsed += new System.Timers.ElapsedEventHandler(VerificaPrimireServer);
            timerHost.AutoReset = true;
        }

        public override void ConecteazateLaJoc(IPAddress adresaIP, int port)
        {
            InitializeazaLabeleClient();
            base.ConecteazateLaJoc(adresaIP, port);
            timerClient = new System.Timers.Timer();
            timerClient.Enabled = true;
            timerClient.Interval = 100;
            timerClient.Elapsed += new System.Timers.ElapsedEventHandler(VerificaPrimireClient);
            timerClient.AutoReset = true;
        }
        public void VerificaPrimireServer(object source, System.Timers.ElapsedEventArgs e)
        { 
            if(_esteHost == true)
            {
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.Green);
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Client primit");
                timerHost.Dispose();
            }
        }
        public void VerificaPrimireClient(object source, System.Timers.ElapsedEventArgs e)
        {
            if (_esteClient == true)
            {
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "BackColor", Color.Green);
                SeteazaProprietateaDinAltThread(_labelConexiuneSocket, "Text", "Server primit");
                timerClient.Dispose();
            }
        }


        public void InitializeazaLabeleHost()
        {
            _labelConexiuneLocala = new System.Windows.Forms.Label();
            _parentForm.Controls.Add(_labelConexiuneLocala);
            _labelConexiuneLocala.Parent = this._parentForm;
            _labelConexiuneLocala.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneLocala.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneLocala.Location = new System.Drawing.Point(225, 602);
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
            _labelConexiuneSocket.Location = new System.Drawing.Point(225, 675);
            _labelConexiuneSocket.Name = "labelClientConectat";
            _labelConexiuneSocket.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneSocket.TabIndex = 1;
            _labelConexiuneSocket.Text = "Client Oprit";
            _labelConexiuneSocket.BackColor = Color.DarkRed;
            _labelConexiuneSocket.Refresh();
        }

        public void InitializeazaLabeleClient()
        {
            _labelConexiuneLocala = new System.Windows.Forms.Label();
            _parentForm.Controls.Add(_labelConexiuneLocala);
            _labelConexiuneLocala.Parent = this._parentForm;
            _labelConexiuneLocala.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _labelConexiuneLocala.Font = new System.Drawing.Font("Segoe UI", 17F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            _labelConexiuneLocala.Location = new System.Drawing.Point(225, 675);
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
            _labelConexiuneSocket.Location = new System.Drawing.Point(225, 602);
            _labelConexiuneSocket.Name = "labelClientConectat";
            _labelConexiuneSocket.Size = new System.Drawing.Size(147, 40);
            _labelConexiuneSocket.TabIndex = 1;
            _labelConexiuneSocket.BackColor = Color.DarkRed;
            _labelConexiuneSocket.Text = "Se Conecteaza";
            _labelConexiuneSocket.Refresh();
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