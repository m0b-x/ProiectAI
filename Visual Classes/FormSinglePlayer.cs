using ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax;
using System;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class FormSinglePlayer : Form
    {
        private int _adancime;
        private TipAI _tipAI;

        public FormSinglePlayer(Form form, int adancime, TipAI tipAI)
        {
            _adancime = adancime;
            InitializeComponent();
            _tipAI = tipAI;
        }

        private EngineSinglePlayer jocSah;
        private Form formPrincipal;

        private void FormSinglePlayer_Load(object sender, EventArgs e)
        {
            Tuple<Om, Om> jucatori = new Tuple<Om, Om>(new Om(Culoare.AlbMin), new Om(Culoare.AlbastruMax));

            formPrincipal = this;
            jocSah = new EngineSinglePlayer(formPrincipal, jucatori.Item1, Aspect.Normal, _tipAI, _adancime, incepeOmul: true);
            jocSah.AdaugaPieselePrestabilite();
            jocSah.DeschideJocul();
        }

        private void buttonReverse_Click(object sender, EventArgs e)
        {
            jocSah.StergeUltimaMutare();
        }

        private void button1_Click(object sender, EventArgs e)
        {
        }
    }
}