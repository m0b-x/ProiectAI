using ProiectVolovici.Classes.Joc.Multiplayer;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace ProiectVolovici
{
	public class EngineClient : EngineJoc
	{
		public static int IntervalTimerPrimireDate = 50;

		protected Om _jucatorClient;

		protected NetworkClient _client;
		protected ParserTabla _parserTabla;

		private Tuple<Pozitie, Pozitie> _ultimaMutarePrimitaClient;

		protected System.Timers.Timer _timerJocClient;

		protected bool _randulClientului;

		protected bool _timerJocClientDisposed;

		private String _ultimulMesajPrimitClient = NetworkClient.BufferGol;

		public Om Jucator
		{
			get { return _jucatorClient; }
		}

		public bool RandulTau
		{
			get { return _randulClientului; }
		}

		public EngineClient(Form parentForm, Om jucator) : base(parentForm)
		{
			AdaugaEvenimentCadrane();
			_jucatorClient = jucator;
			_jucatorClient.Culoare = Culoare.AlbMin;

			_randulClientului = false;

			_ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

			_parserTabla = new ParserTabla(ConstantaTabla.NrLinii, ConstantaTabla.NrColoane);
		}

		public EngineClient(Form parentForm, int[][] matriceTabla, Om jucator) : base(parentForm, matriceTabla)
		{
			AdaugaEvenimentCadrane();
			_jucatorClient = jucator;
			_jucatorClient.Culoare = Culoare.AlbMin;
			_randulClientului = false;

			_ultimaMutarePrimitaClient = new Tuple<Pozitie, Pozitie>(new Pozitie(1, 1), new Pozitie(1, 1));

			_parserTabla = new ParserTabla(ConstantaTabla.NrLinii, ConstantaTabla.NrColoane);
		}

		public void AdaugaEvenimentCadrane()
		{
			for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
			{
				for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
				{
					ArrayCadrane[linie][coloana].Click += OnCadranClick;
				}
			}
		}

		private void RealizeazaMutareaOnline(Piesa piesa, Pozitie pozitie)
		{
			if (pozitie.Linie > MarimeVerticala || pozitie.Coloana > MarimeOrizontala || pozitie.Linie < 0 || pozitie.Coloana < 0)
			{
				Debug.WriteLine("Linie sau coloana invalida! Linie: {0}, Coloana {1}", pozitie.Linie, pozitie.Coloana);
			}
			else
			{
				_client.TrimiteDate(_parserTabla.CodificareMutare(piesa.Pozitie, pozitie));
				NuEsteRandulTau();
				RealizeazaMutareaLocal(piesa, pozitie);
			}
		}

		protected virtual void EsteRandulTau()
		{
			_randulClientului = true;
		}

		protected virtual void NuEsteRandulTau()
		{
			_randulClientului = false;
		}

		public override void Dispose()
		{
			if (_timerJocClient != null)
				_timerJocClient.Dispose();
			GC.SuppressFinalize(this);
			Debug.WriteLine("Dispose JocMultiplayer");
			_timerJocClientDisposed = true;
			_client.TrimiteDate(_client.MesajDeconectare);
		}

		~EngineClient() => Dispose();

		public virtual async void ConecteazateLaJoc(IPAddress adresaIP, int port)
		{
			_client = new NetworkClient(adresaIP, port);
			await _client.PornesteCerereaDeConectare();
			await PrimesteTablaAsincron();
			_client.TimerCitireDate.Stop();
			ActiveazaTimerRepetitiv(ref _timerJocClient, (uint)IntervalTimerPrimireDate, SincronizeazaClient);
			_client.Buffer = NetworkClient.BufferGol;
			NuEsteRandulTau();
		}

		protected virtual async Task PrimesteTablaAsincron()
		{
			while (_client.Buffer.Equals(NetworkClient.BufferGol))
			{
				await Task.Delay(50);
			}

			var matrice = _parserTabla.DecodificareTablaSiAspect(_client.Buffer, out _aspectJoc);
			if (_aspectJoc == Aspect.Invers)
				_aspectJoc = Aspect.Normal;
			else
				_aspectJoc = Aspect.Invers;
			ActualizeazaIntreagaTabla(matrice);
		}

		public void SincronizeazaClient(object source, ElapsedEventArgs e)
		{
			if (_timerJocClientDisposed == false)
			{
				if (_ultimulMesajPrimitClient.Equals(_client.Buffer))
				{
					_client.PrimesteDate();
				}
				if (_client.Buffer != NetworkClient.BufferGol)
				{
					_ultimulMesajPrimitClient = _client.Buffer;
					if (!_ultimulMesajPrimitClient.Equals(_client.MesajDeconectare))
					{
						_ultimaMutarePrimitaClient = _parserTabla.DecodificareMutare(_ultimulMesajPrimitClient);
						VerificaSahul(_ultimaMutarePrimitaClient.Item2);
						RealizeazaMutareaLocal(GetPiesaCuPozitia(_ultimaMutarePrimitaClient.Item1), _ultimaMutarePrimitaClient.Item2);
						EsteRandulTau();
						VerificaSahurile();
					}
					else
					{
						_timerJocClientDisposed = true;
						_timerJocClient.Stop();
						MessageBox.Show("Server Deconectat(Cod 3)", "Server s-a deconectat");
						if (_esteGataMeciul == false)
						{
							VerificaSahurile();
						}
					}
				}
			}
		}

		private int VerificaTentativaDeSah()
		{
			for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
			{
				for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
				{
					if (ArrayCadrane[linie][coloana].PiesaCadran != ConstantaTabla.PiesaNula)
					{
						List<Pozitie> mutari = ArrayCadrane[linie][coloana].PiesaCadran.ReturneazaMutariPosibile(this.MatriceCoduriPiese);
						foreach (var mutare in mutari)
						{
							if (MatriceCoduriPiese[mutare.Linie][mutare.Coloana] == (int)CodPiesa.RegeAlb)
								return ConstantaTabla.SahLaRegeAlb;
							if (MatriceCoduriPiese[mutare.Linie][mutare.Coloana] == (int)CodPiesa.RegeAlbastru)
								return ConstantaTabla.SahLaRegerAlbastru;
						}
					}
				}
			}
			return ConstantaTabla.NuEsteSah;
		}

		private void VerificaSahul(Pozitie pozitie)
		{
			Piesa piesa = GetPiesaCuPozitia(pozitie);
			if (piesa != null)
			{
				if (piesa.Cod == CodPiesa.RegeAlbastru)
				{
					MessageBox.Show("Ai pierdut");
					TerminaMeciul();
				}
				else if (piesa.Cod == CodPiesa.RegeAlbastru)
				{
					MessageBox.Show("Ai castigat");
					TerminaMeciul();
				}
			}
		}

		public void TerminaMeciul()
		{
			_esteGataMeciul = true;
			StergeEvenimenteleCadranelor();
			_timerJocClient.Stop();
			_client.TrimiteDate(_client.MesajDeconectare);
		}

		private void StergeEvenimenteleCadranelor()
		{
			for (int linie = 0; linie < ConstantaTabla.NrLinii; linie++)
			{
				for (int coloana = 0; coloana < ConstantaTabla.NrColoane; coloana++)
				{
					ArrayCadrane[linie][coloana].Click -= OnCadranClick;
				}
			}
		}

		public void OnCadranClick(object sender, EventArgs e)
		{
			if (_randulClientului)
			{
				if (PiesaSelectata == ConstantaTabla.PiesaNula)
				{
					Pozitie pozitie = (sender as Cadran).PozitieCadran;

					if (ArrayCadrane[pozitie.Linie][pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
					{
						Piesa piesa = GetPiesaCuPozitia(pozitie);

						if (piesa != null)
						{
							if (piesa.Culoare != _jucatorClient.Culoare)
							{
								return;
							}
							PiesaSelectata = piesa;
							piesa.ArataMutariPosibile(this);
							if (ExistaMutariPosibile() == true)
							{
								ArataPiesaSelectata(piesa);
							}
							else
							{
								ArataPozitieBlocata(pozitie);
							}
						}
					}
				}
				else
				{
					Pozitie pozitie = (sender as Cadran).PozitieCadran;

					if (PiesaSelectata.Pozitie != pozitie)
					{
						if (EsteMutareaPosibila(pozitie))
						{
							AscundePiesaSelectata(PiesaSelectata);
							if (MatriceCoduriPiese[pozitie.Linie][pozitie.Coloana] != (int)CodPiesa.Gol)
							{
								ConstantaSunet.SunetPiesaLuata.Play();
							}
							else
							{
								ConstantaSunet.SunetPiesaMutata.Play();
							}
							VerificaSahul(pozitie);
							RealizeazaMutareaOnline(PiesaSelectata, pozitie);
						}
					}
				}
			}
		}
	}
}