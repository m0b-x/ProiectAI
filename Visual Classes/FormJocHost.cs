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

        NetworkServer server;
        NetworkClient client;

        private void FormJocHost_Load(object sender, EventArgs e)
        {
            Piesa pion = new Pion(Culoare.Albastru);
            Piesa pion2 = new Pion(Culoare.Albastru);
            Piesa turaAlbastra = new Tura(Culoare.Albastru);
            Piesa turaAlba = new Tura(Culoare.Alb);
            Piesa rege = new Rege(Culoare.Alb);

            JocDeSah tablaServer = new JocDeSah(this);

            tablaServer.AdaugaPiesa(ref pion2, new Pozitie(2,1));
            tablaServer.AdaugaPiesa(ref pion, new Pozitie(1, 1));
            tablaServer.AdaugaPiesa(ref rege, new Pozitie(8, 7));
            tablaServer.AdaugaPiesa(ref turaAlbastra, new Pozitie(0, 0));
            tablaServer.AdaugaPiesa(ref turaAlba, new Pozitie(8, 8));

            FormJocClient formJocClient = new FormJocClient();
            formJocClient.Show();

            server = new NetworkServer(IPAddress.Any, 3000);
            client = new NetworkClient(IPAddress.Parse("127.0.0.1"), 3000);

            server.AcceptaConexiuneaUrmatoare();
            client.PornesteCerereaDeConectare();

            server.TrimiteDate("CHUNGAAAAA");
            Debug.WriteLine("DatePrimite de la server: " + client.PrimesteDate());

            ParserTabla parserTabla = new ParserTabla(ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala, 5);

            server.TrimiteDate   (parserTabla.CodificareTabla(tablaServer.MatriceCodPiese));

            JocDeSah tablaClient = new JocDeSah(formJocClient, parserTabla.DecodificareTabla(client.PrimesteDate()));

        }

        private void FormJocHost_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Dispose();
            client.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            client.TrimiteDate("CHUNGA");
        }
    }
}
