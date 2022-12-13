using System;
using System.Diagnostics;
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
            butonOptiune1.Text = "AlfaBeta";
            butonOptiune1.Click -= butonSinglePlayer_Click;
            butonOptiune1.Click += butonAlfaBeta_Click;

            butonOptiune2.Text = "MonteCarlo";
            butonOptiune2.Click -= butonMultiPlayer_Click;
            butonOptiune2.Click += butonMCTS_Click;

            butonOptiune3.Text = "Inapoi";
            butonOptiune3.Click -= butonExit_Click;
            butonOptiune3.Click += butonInapoiDeLaSinglePlayer_Click;

            /*
            Form formSinglePlayer = new FormSinglePlayer(this);
            formSinglePlayer.Closed += (s, args) => this.Close();
            formSinglePlayer.Show();
            */
        }


        private void butonInapoiDeLaSinglePlayer_Click(object sender, EventArgs e)
        {
            butonOptiune1.Text = "SinglePlayer";
            butonOptiune1.Click -= butonAlfaBeta_Click;
            butonOptiune1.Click += butonSinglePlayer_Click;

            butonOptiune2.Text = "MultiPlayer";
            butonOptiune2.Click -= butonMCTS_Click;
            butonOptiune2.Click += butonMultiPlayer_Click;

            butonOptiune3.Text = "Exit";
            butonOptiune3.Click -= butonInapoiDeLaSinglePlayer_Click;
            butonOptiune3.Click += butonExit_Click;

            /*
            Form formSinglePlayer = new FormSinglePlayer(this);
            formSinglePlayer.Closed += (s, args) => this.Close();
            formSinglePlayer.Show();
            */
        }

        private void butonInapoiDeLaDificultateMCTS_Click(object sender, EventArgs e)
        {
            butonOptiune1.Text = "AlfaBeta";
            butonOptiune1.Click -= butonMCTS_Usor_Click;
            butonOptiune1.Click += butonAlfaBeta_Click;

            butonOptiune2.Text = "MonteCarlo";
            butonOptiune2.Click -= butonMCTS_Mediu_Click;
            butonOptiune2.Click += butonMCTS_Click;

            butonOptiune3.Text = "Inapoi";
            butonOptiune3.Click -= butonMCTS_Greu_Click;
            butonOptiune3.Click += butonInapoiDeLaSinglePlayer_Click;


            butonOptiune4.Visible = false;
            butonOptiune4.Enabled = false;
            butonOptiune4.Click -= butonInapoiDeLaDificultateMCTS_Click;

        }
        private void butonInapoiDeLaDificultateAlphaBeta_Click(object sender, EventArgs e)
        {
            butonOptiune1.Text = "AlfaBeta";
            butonOptiune1.Click -= butonAlphaBeta_Usor_Click;
            butonOptiune1.Click += butonAlfaBeta_Click;

            butonOptiune2.Text = "MonteCarlo";
            butonOptiune2.Click -= butonAlphaBeta_Mediu_Click;
            butonOptiune2.Click += butonMCTS_Click;

            butonOptiune3.Text = "Inapoi";
            butonOptiune3.Click -= butonAlphaBeta_Greu_Click;
            butonOptiune3.Click += butonInapoiDeLaSinglePlayer_Click;


            butonOptiune4.Visible = false;
            butonOptiune4.Enabled = false;
            butonOptiune4.Click -= butonInapoiDeLaDificultateAlphaBeta_Click;

        }
        private void butonInapoiDeLaAlphaBeta_Click(object sender, EventArgs e)
        {
            butonOptiune1.Text = "SinglePlayer";
            butonOptiune1.Click -= butonMCTS_Usor_Click;
            butonOptiune1.Click += butonSinglePlayer_Click;

            butonOptiune2.Text = "MultiPlayer";
            butonOptiune2.Click -= butonMCTS_Mediu_Click;
            butonOptiune2.Click += butonMultiPlayer_Click;

            butonOptiune3.Text = "Exit";
            butonOptiune3.Click -= butonMCTS_Greu_Click;
            butonOptiune3.Click += butonExit_Click;

            butonOptiune4.Visible = false;
            butonOptiune4.Enabled = false;
            butonOptiune4.Click -= butonInapoiDeLaAlphaBeta_Click;

        }

        private void butonInapoiDeLaMCTS_Click(object sender, EventArgs e)
        {
            butonOptiune1.Text = "SinglePlayer";
            butonOptiune1.Click -= butonMCTS_Usor_Click;
            butonOptiune1.Click += butonSinglePlayer_Click;

            butonOptiune2.Text = "MultiPlayer";
            butonOptiune2.Click -= butonMCTS_Mediu_Click;
            butonOptiune2.Click += butonMultiPlayer_Click;

            butonOptiune3.Text = "Exit";
            butonOptiune3.Click -= butonMCTS_Greu_Click;
            butonOptiune3.Click += butonExit_Click;

            butonOptiune4.Visible = false;
            butonOptiune4.Enabled = false;
            butonOptiune4.Click -= butonInapoiDeLaMCTS_Click;
        }

        private void butonAlfaBeta_Click(object sender, EventArgs e)
        {
            butonOptiune1.Text = "Usor";
            butonOptiune1.Click -= butonAlfaBeta_Click;
            butonOptiune1.Click += butonAlphaBeta_Usor_Click;

            butonOptiune2.Text = "Mediu";
            butonOptiune2.Click -= butonMCTS_Click;
            butonOptiune2.Click += butonAlphaBeta_Mediu_Click;

            butonOptiune3.Text = "Greu";
            butonOptiune3.Click -= butonInapoiDeLaSinglePlayer_Click;
            butonOptiune3.Click += butonAlphaBeta_Greu_Click;

            butonOptiune4.Text = "Inapoi";
            butonOptiune4.Visible = true;
            butonOptiune4.Enabled = true;
            butonOptiune4.Click += butonInapoiDeLaDificultateAlphaBeta_Click;
        }


        private void butonMCTS_Click(object sender, EventArgs e)
        {
            butonOptiune1.Text = "Usor";
            butonOptiune1.Click -= butonAlfaBeta_Click;
            butonOptiune1.Click += butonMCTS_Usor_Click;

            butonOptiune2.Text = "Mediu";
            butonOptiune2.Click -= butonMCTS_Click;
            butonOptiune2.Click += butonMCTS_Mediu_Click;

            butonOptiune3.Text = "Greu";
            butonOptiune3.Click += butonMCTS_Greu_Click;
            butonOptiune3.Click -= butonInapoiDeLaSinglePlayer_Click;

            butonOptiune4.Text = "Inapoi";
            butonOptiune4.Visible = true;
            butonOptiune4.Enabled = true;
            butonOptiune4.Click += butonInapoiDeLaDificultateMCTS_Click;
        }
        private void butonAlphaBeta_Usor_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Usor");
            this.Hide();
            Form formSinglePlayer = new FormSinglePlayer(this, adancime:0);
            formSinglePlayer.Closed += (s, args) => this.Close();
            formSinglePlayer.Show();
        }
        private void butonAlphaBeta_Mediu_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Mediu");
            this.Hide();
            Form formSinglePlayer = new FormSinglePlayer(this, adancime: 2);
            formSinglePlayer.Closed += (s, args) => this.Close();
            formSinglePlayer.Show();
        }

        private void butonAlphaBeta_Greu_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("Greuu");
            this.Hide();
            Form formSinglePlayer = new FormSinglePlayer(this, adancime: 4);
            formSinglePlayer.Closed += (s, args) => this.Close();
            formSinglePlayer.Show();
        }

        private void butonMCTS_Usor_Click(object sender, EventArgs e)
        {
        }
        private void butonMCTS_Mediu_Click(object sender, EventArgs e)
        {
        }

        private void butonMCTS_Greu_Click(object sender, EventArgs e)
        {
        }


        private void butonMultiPlayer_Click(object sender, EventArgs e)
        {
            Debug.WriteLine("DA");
            /*
            this.Hide();
            Form formMultiPlayer = new FormMultiPlayer();
            formMultiPlayer.Closed += (s, args) => this.Close();
            formMultiPlayer.Show();
            */
        }

        private void butonExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}