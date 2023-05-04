using System.Collections.Generic;

namespace ProiectVolovici
{
    public struct Pozitie
    {
        public int Linie;
        public int Coloana;

        public static Pozitie PozitieInvalida = new Pozitie(-1, -1);
        public Pozitie(int linie, int coloana)
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
            return unchecked(Linie ^ Coloana);
        }
        static Pozitie[][] ListaPozitii =
        {
            new Pozitie[] { new Pozitie(0, 0),new Pozitie(0, 1),new Pozitie(0, 2),new Pozitie(0, 3),new Pozitie(0, 4),new Pozitie(0, 5),new Pozitie(0, 6),new Pozitie(0, 7),new Pozitie(0, 8) },
            new Pozitie[] { new Pozitie(1, 0),new Pozitie(1, 1),new Pozitie(1, 2),new Pozitie(1, 3),new Pozitie(1, 4),new Pozitie(1, 5),new Pozitie(1, 6),new Pozitie(1, 7),new Pozitie(1, 8) },
            new Pozitie[] { new Pozitie(2, 0),new Pozitie(2, 1),new Pozitie(2, 2),new Pozitie(2, 3),new Pozitie(2, 4),new Pozitie(2, 5),new Pozitie(2, 6),new Pozitie(2, 7),new Pozitie(2, 8) },
            new Pozitie[] { new Pozitie(3, 0),new Pozitie(3, 1),new Pozitie(3, 2),new Pozitie(3, 3),new Pozitie(3, 4),new Pozitie(3, 5),new Pozitie(3, 6),new Pozitie(3, 7),new Pozitie(3, 8) },
            new Pozitie[] { new Pozitie(4, 0),new Pozitie(4, 1),new Pozitie(4, 2),new Pozitie(4, 3),new Pozitie(4, 4),new Pozitie(4, 5),new Pozitie(4, 6),new Pozitie(4, 7),new Pozitie(4, 8) },
            new Pozitie[] { new Pozitie(5, 0),new Pozitie(5, 1),new Pozitie(5, 2),new Pozitie(5, 3),new Pozitie(5, 4),new Pozitie(5, 5),new Pozitie(5, 6),new Pozitie(5, 7),new Pozitie(5, 8) },
            new Pozitie[] { new Pozitie(6, 0),new Pozitie(6, 1),new Pozitie(6, 2),new Pozitie(6, 3),new Pozitie(6, 4),new Pozitie(6, 5),new Pozitie(6, 6),new Pozitie(6, 7),new Pozitie(6, 8) },
            new Pozitie[] { new Pozitie(7, 0),new Pozitie(7, 1),new Pozitie(7, 2),new Pozitie(7, 3),new Pozitie(7, 4),new Pozitie(7, 5),new Pozitie(7, 6),new Pozitie(7, 7),new Pozitie(7, 8) },
            new Pozitie[] { new Pozitie(8, 0),new Pozitie(8, 1),new Pozitie(8, 2),new Pozitie(8, 3),new Pozitie(8, 4),new Pozitie(8, 5),new Pozitie(8, 6),new Pozitie(8, 7),new Pozitie(8, 8) },
            new Pozitie[] { new Pozitie(9, 0),new Pozitie(9, 1),new Pozitie(9, 2),new Pozitie(9, 3),new Pozitie(9, 4),new Pozitie(9, 5),new Pozitie(9, 6),new Pozitie(9, 7),new Pozitie(9, 8) }
        };

        public static Pozitie AcceseazaElementStatic(int linie, int coloana)
        {
            return ListaPozitii[linie][coloana];
        }
        public static Pozitie AcceseazaElementStaticSafe(int linie, int coloana)
        {
            if (linie < 0 || linie > 9
                || coloana < 0 || coloana > 8)
                return Pozitie.PozitieInvalida;

            return ListaPozitii[linie][coloana];
        }


        public static bool EstePozitieValida(int linie, int coloana)
        {
            if (0 <= linie && linie <= 9 &&
                0 <= coloana && coloana <= 8)
            {
                return true;
            }
            return false;
        }

        public override string ToString()
        {
            return $"({Linie},{Coloana})";
        }
    }
}