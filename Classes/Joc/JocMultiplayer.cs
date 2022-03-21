using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class JocMultiplayer : EngineJoc
    {
        public JocMultiplayer(Form parentForm) : base(parentForm)
        {
        }

        public JocMultiplayer(Form parentForm, int[,] matriceTabla) : base(parentForm, matriceTabla)
        {
        }

        public void AdaugaPiesaAsta(ref Piesa piesa,Pozitie pozitie)
        {
            AdaugaPiesa(ref piesa, pozitie);
        }

    }
}
