using System;

namespace ProiectVolovici
{
    public struct Pozitie
    {
        public int Linie;
        public int Coloana;

        public static Pozitie PozitieNula = new Pozitie(-1,-1);
        public Pozitie (int linie, int coloana)
        {
            Linie = linie;
            Coloana = coloana;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Pozitie)) return false;
            Pozitie other = (Pozitie)obj;
            return Linie == other.Linie && Coloana == other.Coloana;
        }

        public bool Equals(Pozitie other)
        {
            return Linie == other.Linie && Coloana == other.Coloana;
        }

        public static bool operator ==(Pozitie a, Pozitie b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Pozitie a, Pozitie b)
        {
            return !a.Equals(b);
        }

        public override int GetHashCode()
        {
            int hash = 19;
            hash = hash * 29 + Linie;
            hash = hash * 29 + Coloana;
            return hash;
        }
    }
}