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
    public partial class FormJoc : Form
    {
        public FormJoc()
        {
            InitializeComponent();
        }

        NetworkServer server;
        NetworkClient client;

        private void Form1_Load(object sender, EventArgs e)
        {

            Piesa pion = new Pion(Culoare.Albastru);
            Piesa pion2 = new Pion(Culoare.Albastru);
            Piesa turaAlbastra = new Tura(Culoare.Albastru);
            Piesa turaAlba = new Tura(Culoare.Alb);
            Piesa rege = new Rege(Culoare.Alb);

            Tabla tabla = new Tabla(this);

            tabla.AdaugaPiesa(pion2, new Pozitie(2,1));
            tabla.AdaugaPiesa(pion, new Pozitie(1, 1));
            tabla.AdaugaPiesa(rege, new Pozitie(8, 7));
            tabla.AdaugaPiesa(turaAlbastra, new Pozitie(0, 0));
            tabla.AdaugaPiesa(turaAlba, new Pozitie(8, 8));

            server = new NetworkServer(IPAddress.Any,3000);
            client = new NetworkClient(IPAddress.Parse("127.0.0.1"), 3000);

            client.ConecteazaClientLaServer();
            server.AcceptaConexiuni();

            server.TrimiteDate("GAY");
            Debug.WriteLine("DatePrimite de la client:" + client.PrimesteDate());


        }

        private void FormJoc_FormClosing(object sender, FormClosingEventArgs e)
        {
            server.Dispose();
            client.Dispose();
        }

    }
}
