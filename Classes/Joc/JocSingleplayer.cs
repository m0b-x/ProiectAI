using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class JocSingleplayer : EngineJoc
    {
        public JocSingleplayer(Form parentForm) : base(parentForm)
        {
        }
        public JocSingleplayer(Form parentForm, int[,] matriceTabla) : base(parentForm, matriceTabla)
        {
        }

    }
}
