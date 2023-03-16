using System;

namespace ProiectVolovici
{
	public class Pozitie
	{
		private int _linie;
		private int _coloana;

		public int Linie
		{
			get { return _linie; }
			set { _linie = value; }
		}

		public int Coloana
		{
			get { return _coloana; }
			set { _coloana = value; }
		}

        public int Item1
        {
            get { return _linie; }
            set { _linie = value; }
        }

        public int Item2
        {
            get { return _coloana; }
            set { _coloana = value; }
        }

        public Pozitie(int linie, int coloana)
		{
			_linie = linie;
			_coloana = coloana;
		}

		public override bool Equals(Object obj)
		{
			if ((obj == null) || !this.GetType().Equals(obj.GetType()))
			{
				return false;
			}
			else
			{
				Pozitie cealaltaPozitie = (Pozitie)obj;
				return (Linie == cealaltaPozitie.Linie) && (Coloana == cealaltaPozitie.Coloana);
			}
		}

		public override string ToString()
		{
			return String.Format("(Linie:{0}, Coloana:{1})", _linie, _coloana);
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(_linie, _coloana);
		}

		public static bool operator ==(Pozitie pozitieStanga, Pozitie pozitieDreapta)
		{
			if (pozitieStanga is null)
			{
				if (pozitieDreapta is null)
				{
					return true;
				}
				return false;
			}
			if (pozitieDreapta is null)
			{
				if (pozitieStanga is null)
				{
					return true;
				}
				return false;
			}
			return (pozitieStanga.Linie == pozitieDreapta.Linie && pozitieStanga.Coloana == pozitieDreapta.Coloana);
		}

		public static bool operator !=(Pozitie pozitieStanga, Pozitie pozitieDreapta) => !(pozitieStanga == pozitieDreapta);
	}
}