using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class IntrareTabel : IEquatable<IntrareTabel>
    {
        public double Item1;
        public int Item2;
        public int Item3;


        public IntrareTabel(double item1, int item2, int item3)
        {
            Item1 = item1;
            Item2 = item2;
            Item3 = item3;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is IntrareTabel)) return false;
            IntrareTabel other = (IntrareTabel)obj;
            return Item1 == other.Item1 && Item2 == other.Item2;
        }

        public bool Equals(IntrareTabel other)
        {
            return other is not null &&
                   Item1 == other.Item1 &&
                   Item2 == other.Item2 &&
                   Item3 == other.Item3;
        }

        // Note: Not quite FNV!
        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = (int)2166136261;

                hash = (hash * 16777619) ^ Item1.GetHashCode();
                hash = (hash * 16777619) ^ Item2.GetHashCode();
                hash = (hash * 16777619) ^ Item3.GetHashCode();
                return hash;
            }
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
