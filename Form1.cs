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
            int marimeTablaOrizontala = 9;
            int marimeTablaVerticala = 10;
            Tabla tabla = new Tabla(this, marimeTablaOrizontala, marimeTablaVerticala);
            Piesa pion = new Pion(tabla, Culoare.ALBASTRU, 1, 1);
            pion.mutaPiesa(tabla, 3, 3);
        }
    }
}
