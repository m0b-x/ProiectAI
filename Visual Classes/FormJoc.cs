using System;
using System.Windows.Forms;
using System.Net;

namespace ProiectVolovici
{
    public partial class FormJoc : Form
    {
        public FormJoc()
        {
            InitializeComponent();
        }

        HostSah jocSah;
        ClientSah jocSahForm2;

        IPAddress adresa = IPAddress.Parse("127.0.0.1");
        int port = 3000;

        Form formClient;
        Form formHost;
        private void FormJocHost_Load(object sender, EventArgs e)
        {
            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(), new Om());

            formHost = this;
            jocSah = new HostSah(formHost,jucatori.Item1);
            jocSah.AdaugaPieselePrestabilite();
            jocSah.HosteazaJoc(port);

            formClient = new Form();
            formClient.Size = formHost.Size;
            formClient.Text = "Form Client";
            formClient.FormClosing += new FormClosingEventHandler(FormJocClient_Closed);
            jocSahForm2 = new ClientSah(formClient,jucatori.Item2);
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
