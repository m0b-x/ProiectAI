using System;
using System.Windows.Forms;
using System.Net;

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

        FormJocClient formClient;
        Form formJoc;
        private void FormJocHost_Load(object sender, EventArgs e)
        {
            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(), new Om());

            formJoc = this;
            jocSah = new SahMultiplayer(formJoc, ref jucatori);
            jocSah.AdaugaPieselePrestabilite();
            jocSah.HosteazaJoc(port);

            formClient = new FormJocClient();
            formClient.FormClosing += new FormClosingEventHandler(FormJocClient_Closed);
            jocSahForm2 = new SahMultiplayer(formClient, ref jucatori);
            jocSahForm2.ConecteazateLaJoc(IPAddress.Parse("127.0.0.1"), port);
            formClient.Show();
        }
        private void FormJocClient_Closed(object sender, FormClosingEventArgs e)
        {
        }
        private void FormJocHost_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
