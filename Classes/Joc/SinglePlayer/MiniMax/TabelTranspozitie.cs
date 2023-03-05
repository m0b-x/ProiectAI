using System.Collections.Generic;

namespace ProiectVolovici
{
	public class TabelTranspozitie
	{
		private Dictionary<long, IntrareTabelTranspozitie> _tabel = new();

		public Dictionary<long, IntrareTabelTranspozitie> Tabel
		{
			get { return _tabel; }
		}

		public TabelTranspozitie(Dictionary<long, IntrareTabelTranspozitie> tabel)
		{
			_tabel = tabel;
		}

		public TabelTranspozitie()
		{
		}

		public void AdaugaIntrare(long cheie, double alpha, FlagIntrare flag, int adancime)
		{
			if (_tabel.ContainsKey(cheie))
			{
				var item = Tabel[cheie];
				if (adancime >= item.Adancime)
					_tabel[cheie] = new IntrareTabelTranspozitie(adancime, alpha, flag);
			}
			else
			{
				_tabel.Add(cheie, new IntrareTabelTranspozitie(adancime, alpha, flag));
			}
		}

		public void ReseteazaTabel()
		{
			_tabel = new();
		}
	}
}