using ProiectVolovici.Properties;
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

namespace ProiectVolovici.Visual_Classes
{
    public partial class FormMesaj : Form
    {
        public static Color CuloareAlb = Color.Beige;
        public static Color CuloareAlbastru = Color.FromArgb(89, 132, 189);
        public static Color CuloareNeutra = System.Drawing.SystemColors.ControlDarkDark;

        public static Image ImagineCastigAlb = Resources.winemoji;
        public static Image ImagineAlbastru = Resources.loseemoji;
        public static Image ImagineNeutra = Resources.neutralemoji;
        public FormMesaj(Form parentForm,TipCastig tipMesaj, string mesajStare, string mesajMotiv)
        {
            InitializeComponent();
            switch(tipMesaj)
            {
                case TipCastig.NoCotest:
                    panelDocked.BackColor = CuloareNeutra;
                    buttonOk.BackColor = CuloareNeutra;
                    labelMotivCastig.ForeColor = CuloareNeutra;
                    labelStareCastig.ForeColor = CuloareNeutra;
                    pictureBoxEmote.Image = ImagineNeutra;
                    break;

                case TipCastig.CastigAlb:
                    panelDocked.BackColor = CuloareAlb;
                    buttonOk.BackColor = CuloareAlb;
                    labelMotivCastig.ForeColor = CuloareAlb;
                    labelStareCastig.ForeColor = CuloareAlb;
                    pictureBoxEmote.Image = ImagineCastigAlb;
                    break;

                case TipCastig.CastigAlbastru:
                    panelDocked.BackColor = CuloareAlbastru;
                    buttonOk.BackColor = CuloareAlbastru;
                    labelMotivCastig.ForeColor = CuloareAlbastru;
                    labelStareCastig.ForeColor = CuloareAlbastru;
                    pictureBoxEmote.Image = ImagineAlbastru;
                    break;
            }
            labelStareCastig.Text = mesajStare;
            labelMotivCastig.Text = mesajMotiv;
        }

        private void buttonOk_Click(object sender, EventArgs e)
        {
            this.Close(); 
            Application.Restart();
            Environment.Exit(0);
        }
    }
}
