﻿using System;
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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Piesa pion = new Pion(Culoare.ALB);
            Piesa pion2 = new Pion(Culoare.ALBASTRU);

            const int lungimeOrizontala = 10;
            const int latimeTabla = 9;
            Tabla tabla = new Tabla(this, latimeTabla, lungimeOrizontala);

            tabla.AdaugaPiesa(pion2, new Pozitie(5,6));
            tabla.AdaugaPiesa(pion, new Pozitie(5, 5));
            tabla.MutaPiesa(pion, new Pozitie(6, 6));

            Pozitie pozitie1 = new Pozitie(6, 9);
            Pozitie pozitie2 = new Pozitie(6, 6);
            pozitie2 = pozitie1;
            Debug.WriteLine(pozitie2.Linie+pozitie2.Coloana);
        }
    }
}
