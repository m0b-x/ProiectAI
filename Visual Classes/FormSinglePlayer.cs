using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class FormSinglePlayer : Form
    {
        public FormSinglePlayer(Form form)
        {
            InitializeComponent();
        }

        private EngineMiniMax jocSah;
        private Form formPrincipal;

        private void FormSinglePlayer_Load(object sender, EventArgs e)
        {
            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(Culoare.Alb), new Om(Culoare.Albastru));

            formPrincipal = this;
            jocSah = new EngineMiniMax(formPrincipal, jucatori.Item1, Aspect.Normal);
            jocSah.AdaugaPieselePrestabilite();
            jocSah.DeschideJocul();
        }
    }
}