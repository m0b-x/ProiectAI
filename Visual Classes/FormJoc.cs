using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class FormJoc : Form
    {
        public FormJoc()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Piesa pion = new Pion(Culoare.Albastru);
            Piesa pion2 = new Pion(Culoare.Albastru);

            const int lungimeOrizontala = 10;
            const int lungimeVerticala = 9;
            Tabla tabla = new Tabla(this, lungimeOrizontala, lungimeVerticala);

            tabla.AdaugaPiesa(pion2, new Pozitie(1,1));
            tabla.AdaugaPiesa(pion, new Pozitie(7, 7));
            tabla.MutaPiesa(pion, new Pozitie(6, 6));

            Pozitie pozitie1 = new Pozitie(6, 9);
            Pozitie pozitie2 = new Pozitie(6, 6);

        }
    }
}
