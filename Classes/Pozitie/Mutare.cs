using System;
using System.Collections.Generic;

namespace ProiectVolovici
{
    public struct Mutare : IEquatable<Mutare>
    {
        public Pozitie Item1;
        public Pozitie Item2;

        public Mutare(Pozitie item1, Pozitie item2)
        {
            Item1 = item1;
            Item2 = item2;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Mutare)) return false;
            Mutare other = (Mutare)obj;
            return Item1 == other.Item1 && Item2 == other.Item2;
        }

        public bool Equals(Mutare other)
        {
            return EqualityComparer<Pozitie>.Default.Equals(Item1, other.Item1) &&
                   EqualityComparer<Pozitie>.Default.Equals(Item2, other.Item2);
        }

        // Note: Not quite FNV!
        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = (int)2166136261;

                hash = (hash * 16777619) ^ Item1.GetHashCode();
                hash = (hash * 16777619) ^ Item2.GetHashCode();
                return hash;
            }
        }

        public static bool operator ==(Mutare a, Mutare b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Mutare a, Mutare b)
        {
            return !a.Equals(b);
        }

    }
}