using System;
using System.Net;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class FormMultiPlayer : Form
    {
        public FormMultiPlayer()
        {
            InitializeComponent();
        }

        private HostSah jocSah;
        private ClientSah jocSahForm2;

        private IPAddress adresa = IPAddress.Parse("127.0.0.1");
        private int port = 3000;

        private Form formClient;
        private Form formHost;

        private void FormJocHost_Load(object sender, EventArgs e)
        {
            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(CuloareJoc.Alb), new Om(CuloareJoc.Albastru));

            formHost = this;
            jocSah = new HostSah(formHost, jucatori.Item1);
            jocSah.AdaugaPieselePrestabilite();
            jocSah.HosteazaJoc(port);

            formClient = new Form();
            formClient.Size = formHost.Size;
            formClient.Text = "Form Client";
            formClient.FormClosing += new FormClosingEventHandler(FormJocClient_Closed);
            jocSahForm2 = new ClientSah(formClient, jucatori.Item2);
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