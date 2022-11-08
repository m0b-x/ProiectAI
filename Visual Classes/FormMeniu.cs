using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class FormMeniu : Form
    {
        public FormMeniu()
        {
            InitializeComponent();
        }

        private void FormMeniu_Load(object sender, EventArgs e)
        {

        }

        private void butonSinglePlayer_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form formSinglePlayer = new FormSinglePlayer(this);
            formSinglePlayer.Closed += (s, args) => this.Close();
            formSinglePlayer.Show();
        }

        private void butonMultiPlayer_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form formMultiPlayer = new FormMultiPlayer();
            formMultiPlayer.Closed += (s, args) => this.Close();
            formMultiPlayer.Show();
        }

        private void butonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
