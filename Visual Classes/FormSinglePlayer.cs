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
using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;

namespace ProiectVolovici
{
    public partial class FormSinglePlayer : Form
    {
        public FormSinglePlayer()
        {
            InitializeComponent();
        }
        EngineMiniMax jocSah;
        Form formPrincipal;
        private void FormSinglePlayer_Load(object sender, EventArgs e)
        {

            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(CuloareJoc.Alb), new Om(CuloareJoc.Albastru));

            formPrincipal = this;
            jocSah = new EngineMiniMax(formPrincipal, jucatori.Item1);
            jocSah.AdaugaPieselePrestabilite();
        }
    }
}
