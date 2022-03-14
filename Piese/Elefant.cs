﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Elefant : Piesa
    {
        public Elefant(Culoare culoare, int linie=0, int coloana=0)
        {
            this.Linie = linie;
            this.Coloana = coloana;
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            if (culoare == Culoare.ALBASTRU)
            {
                this.Imagine = Properties.Resources.belephantrev2;
                this.Cod = CodPiesa.ElefantAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.welephantrev2;
                this.Cod = CodPiesa.ElefantAlb;
            }
        }
    }
}
