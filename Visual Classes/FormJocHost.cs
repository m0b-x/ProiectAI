using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Threading;
using System.IO;

namespace ProiectVolovici
{
    public partial class FormJocHost : Form
    {
        public FormJocHost()
        {
            InitializeComponent();
        }

        JocMultiplayer jocSah;
        JocMultiplayer jocSahForm2;

        IPAddress adresa = IPAddress.Parse("127.0.0.1");
        int port = 3000;

        private async void FormJocHost_Load(object sender, EventArgs e)
        {
            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(), new Om());

            Form formJoc = this;
            jocSah = new JocMultiplayer(formJoc, ref jucatori);
            jocSah.AdaugaPieselePrestabilite();
            jocSah.HosteazaJoc(port);

            FormJocClient formClient = new FormJocClient();
            jocSahForm2 = new JocMultiplayer(formClient, ref jucatori);
            jocSahForm2.ConecteazateLaJoc(IPAddress.Parse("127.0.0.1"), port);
            formClient.Show();
        }

        private void FormJocHost_FormClosing(object sender, FormClosingEventArgs e)
        {;
        }

    }
}
