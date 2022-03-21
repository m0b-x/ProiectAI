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

        private void FormJocHost_Load(object sender, EventArgs e)
        {
            Piesa pion = new Pion(CuloareJoc.Albastru);
            Piesa pion2 = new Pion(CuloareJoc.Albastru);
            Piesa turaAlbastra = new Tura(CuloareJoc.Albastru);
            Piesa turaAlba = new Tura(CuloareJoc.Alb);
            Piesa rege = new Rege(CuloareJoc.Alb);

            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(), new Om());

            jocSah = new JocMultiplayer(this, ref jucatori);

            FormJocClient form = new FormJocClient();
            form.Show();

            jocSahForm2 = new JocMultiplayer(form, ref jucatori);

            jocSah.AdaugaPiesa(ref pion2, new Pozitie(2, 1));
            jocSah.AdaugaPiesa(ref pion, new Pozitie(1, 1));
            jocSah.AdaugaPiesa(ref rege, new Pozitie(8, 7));
            jocSah.AdaugaPiesa(ref turaAlbastra, new Pozitie(0, 0));
            jocSah.AdaugaPiesa(ref turaAlba, new Pozitie(8, 8));

            jocSah.HosteazaJoc(3000);
            jocSahForm2.ConecteazateLaJoc(IPAddress.Parse("127.0.0.1"), 3000);
        }

        private void FormJocHost_FormClosing(object sender, FormClosingEventArgs e)
        {;
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }
    }
}
