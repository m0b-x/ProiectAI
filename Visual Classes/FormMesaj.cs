using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici.Visual_Classes
{
    public partial class FormMesaj : Form
    {
        public static Color CuloareAlb = Color.SandyBrown;
        public static Color CuloareAlbastru = Color.FromArgb(89, 132, 189);
        public static Color CuloareNeutra = System.Drawing.SystemColors.ControlDarkDark;

        public FormMesaj(Form parentForm, TipCastig tipMesaj, string mesajStare, string mesajMotiv)
        {
            InitializeComponent();
            switch (tipMesaj)
            {
                case TipCastig.NoCotest:
                    panelDocked.BackColor = CuloareNeutra;
                    buttonOk.BackColor = CuloareNeutra;
                    labelMotivCastig.ForeColor = CuloareNeutra;
                    labelStareCastig.ForeColor = CuloareNeutra;
                    break;

                case TipCastig.CastigAlb:
                    panelDocked.BackColor = CuloareAlb;
                    buttonOk.BackColor = CuloareAlb;
                    labelMotivCastig.ForeColor = CuloareAlb;
                    labelStareCastig.ForeColor = CuloareAlb;
                    break;

                case TipCastig.CastigAlbastru:
                    panelDocked.BackColor = CuloareAlbastru;
                    buttonOk.BackColor = CuloareAlbastru;
                    labelMotivCastig.ForeColor = CuloareAlbastru;
                    labelStareCastig.ForeColor = CuloareAlbastru;
                    break;
            }
            labelStareCastig.Text = mesajStare;
            labelMotivCastig.Text = mesajMotiv;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Restart();
        }
    }
}