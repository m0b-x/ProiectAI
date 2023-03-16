using System;
using System.Diagnostics;
using System.Net;
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
			Form formSinglePlayer = new FormSinglePlayer(this, adancime: 2);
			formSinglePlayer.Closed += (s, args) => this.Close();
			formSinglePlayer.Show();
		}

		private void butonAlphaBeta_Mediu_Click(object sender, EventArgs e)
		{
			Debug.WriteLine("Mediu");
			this.Hide();
			Form formSinglePlayer = new FormSinglePlayer(this, adancime: 3);
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
			butonOptiune1.Text = "Host";
			butonOptiune1.Click -= butonSinglePlayer_Click;
			butonOptiune1.Click += butonHost_Click;

			butonOptiune2.Text = "Connect";
			butonOptiune2.Click -= butonMultiPlayer_Click;
			butonOptiune2.Click += butonConnect_Click;

			butonOptiune3.Text = "Inapoi";
			butonOptiune3.Click -= butonExit_Click;
			butonOptiune3.Click += butonInapoiDeLaMultiPlayer_Click;
			/*
            this.Hide();
            Form formMultiPlay er = new FormMultiPlayer();
            formMultiPlayer.Closed += (s, args) => this.Close();
            formMultiPlayer.Show();
            */
		}

		private void butonStartHost_Click(object sender, EventArgs e)
		{
			int port = 3000;
			try
			{
				AscundeElementeleVizuale();
				var jucatorMP = new Om(Culoare.AlbMin);

				HostSah jocSah = new HostSah(this, jucatorMP);
				jocSah.AdaugaPieselePrestabilite();
				jocSah.HosteazaJoc(port);

				this.Size = new System.Drawing.Size(742, 787);
				this.Text = "Host";
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		private void butonStartConnect_Click(object sender, EventArgs e)
		{
			int port = 3000;
			try
			{
				var adresaIP = IPAddress.Parse(textBoxIP.Text);
				AscundeElementeleVizuale();

				var jucatorMP = new Om(Culoare.AlbastruMax);
				ClientSah jocSahForm2;
				jocSahForm2 = new ClientSah(this, jucatorMP);
				jocSahForm2.ConecteazateLaJoc(adresaIP, port);

				this.Size = new System.Drawing.Size(742, 787);
				this.Text = "Client";
			}
			catch (Exception ex)
			{
				MessageBox.Show("Eroare Ip");
			}
		}

		private void AscundeElementeleVizuale()
		{
			butonOptiune1.Visible = false;
			butonOptiune2.Enabled = false;

			butonOptiune2.Visible = false;
			butonOptiune2.Enabled = false;

			butonOptiune3.Visible = false;
			butonOptiune3.Enabled = false;

			butonOptiune4.Visible = false;
			butonOptiune4.Enabled = false;

			textBoxIP.Enabled = false;
			textBoxIP.Visible = false;

			labelIP.Enabled = false;
			labelIP.Visible = false;

			labelTitlu.Enabled = false;
			labelTitlu.Visible = false;

			this.BackgroundImage = null;
		}

		private void butonHost_Click(object sender, EventArgs e)
		{
			butonOptiune1.Text = "Start";
			butonOptiune1.Click -= butonHost_Click;
			butonOptiune1.Click += butonStartHost_Click;

			butonOptiune2.Enabled = false;
			butonOptiune2.Visible = false;
			// textBoxIP.Visible = true;
			//textBoxIP.Enabled = true;

			butonOptiune3.Text = "Inapoi";
			butonOptiune3.Click -= butonInapoiDeLaMultiPlayer_Click;
			butonOptiune3.Click += butonInapoiDeLaHost_Click;
			/*
            this.Hide();
            Form formMultiPlayer = new FormMultiPlayer();
            formMultiPlayer.Closed += (s, args) => this.Close();
            formMultiPlayer.Show();
            */
		}

		private void butonInapoiDeLaConnect_Click(object sender, EventArgs e)
		{
			butonOptiune1.Text = "Host";
			butonOptiune1.Click -= butonStartConnect_Click;
			butonOptiune1.Click += butonConnect_Click;

			butonOptiune2.Enabled = true;
			butonOptiune2.Visible = true;
			textBoxIP.Enabled = false;
			textBoxIP.Visible = false;

			butonOptiune3.Text = "Inapoi";
			butonOptiune3.Click += butonInapoiDeLaMultiPlayer_Click;
			butonOptiune3.Click -= butonInapoiDeLaConnect_Click;
		}

		private void butonInapoiDeLaHost_Click(object sender, EventArgs e)
		{
			butonOptiune1.Text = "Host";
			butonOptiune1.Click -= butonStartHost_Click;
			butonOptiune1.Click += butonHost_Click;

			butonOptiune2.Enabled = true;
			butonOptiune2.Visible = true;
			textBoxIP.Enabled = false;
			textBoxIP.Visible = false;

			butonOptiune3.Text = "Inapoi";
			butonOptiune3.Click += butonInapoiDeLaMultiPlayer_Click;
			butonOptiune3.Click -= butonInapoiDeLaHost_Click;
		}

		private void butonConnect_Click(object sender, EventArgs e)
		{
			butonOptiune1.Text = "Connect";
			butonOptiune1.Click -= butonHost_Click;
			butonOptiune1.Click += butonStartConnect_Click;

			butonOptiune2.Enabled = false;
			butonOptiune2.Visible = false;
			textBoxIP.Enabled = true;
			textBoxIP.Visible = true;

			butonOptiune3.Text = "Inapoi";
			butonOptiune3.Click -= butonInapoiDeLaMultiPlayer_Click;
			butonOptiune3.Click += butonInapoiDeLaConnect_Click;

			/*
            this.Hide();
            Form formMultiPlayer = new FormMultiPlayer();
            formMultiPlayer.Closed += (s, args) => this.Close();
            formMultiPlayer.Show();
            */
		}

		private void butonInapoiDeLaMultiPlayer_Click(object sender, EventArgs e)
		{
			butonOptiune1.Text = "SinglePlayer";
			butonOptiune1.Click += butonSinglePlayer_Click;
			butonOptiune1.Click -= butonHost_Click;

			butonOptiune2.Text = "MultiPlayer";
			butonOptiune2.Click += butonMultiPlayer_Click;
			butonOptiune2.Click -= butonConnect_Click;

			butonOptiune3.Text = "Exit";
			butonOptiune3.Click -= butonInapoiDeLaMultiPlayer_Click;
			butonOptiune3.Click += butonExit_Click;

			/*
            Form formSinglePlayer = new FormSinglePlayer(this);
            formSinglePlayer.Closed += (s, args) => this.Close();
            formSinglePlayer.Show();
            */
		}

		private void butonExit_Click(object sender, EventArgs e)
		{
			this.Close();
		}
	}
}