﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Pion : Piesa
    {
        
        public Pion(Tabla tabla,Culoare culoare,int linie,int coloana)
        {
            this.Linie = linie;
            this.Coloana = coloana;
            this.CuloarePiesa = culoare;
            if (culoare == Culoare.ALB)
            {
                this.Imagine = Properties.Resources.bpawn;
            }
            else
            {
                this.Imagine = Properties.Resources.wpawn;
            }
            tabla.ArrayCadrane[linie, coloana].BackgroundImage = this.Imagine;
        }
    }
}
