using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public struct IntrareTabel : IEquatable<IntrareTabel>
    {
        public double Scor;
        public int Adancime;
        public int Flag;


        public IntrareTabel(double scor, int adancime, int flag)
        {
            Scor = scor;
            Adancime = adancime;
            Flag = flag;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is IntrareTabel)) return false;
            IntrareTabel other = (IntrareTabel)obj;
            return Scor == other.Scor && Adancime == other.Adancime;
        }

        public bool Equals(IntrareTabel other)
        {
            return Scor == other.Scor &&
                   Adancime == other.Adancime &&
                   Flag == other.Flag;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Scor, Adancime, Flag);
        }

        public static bool operator ==(IntrareTabel left, IntrareTabel right)
        {
            return EqualityComparer<IntrareTabel>.Default.Equals(left, right);
        }

        public static bool operator !=(IntrareTabel left, IntrareTabel right)
        {
            return !(left == right);
        }
    }
}
