using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Piesa pion = new Pion(Culoare.ALB, 4, 4);
            Piesa pion2 = new Pion(Culoare.ALBASTRU, 6, 6);
            Tabla tabla = new Tabla(this, 9, 10);
            tabla.AdaugaPiesa(pion2, 1, 1);
            tabla.AdaugaPiesa(pion, 5, 5);
            tabla.MutaPiesa(pion, 6, 6);
            
        }
    }
}
