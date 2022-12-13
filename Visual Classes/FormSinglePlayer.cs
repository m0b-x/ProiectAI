using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class FormSinglePlayer : Form
    {
        private int _adancime;
        public FormSinglePlayer(Form form, int adancime)
        {
            _adancime = adancime;
            InitializeComponent();
        }

        private EngineMiniMax jocSah;
        private Form formPrincipal;

        private void FormSinglePlayer_Load(object sender, EventArgs e)
        {
            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(Culoare.Alb), new Om(Culoare.Albastru));

            formPrincipal = this;
            jocSah = new EngineMiniMax(formPrincipal, jucatori.Item1, Aspect.Normal, _adancime , incepeOmul: true);
            jocSah.AdaugaPieselePrestabilite();
            jocSah.DeschideJocul();
        }
    }
}