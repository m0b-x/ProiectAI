using System;
using System.Collections.Generic;

namespace ProiectVolovici
{
    public struct Mutare : IEquatable<Mutare>
    {
        public Pozitie MutareInitiala;
        public Pozitie MutareFinala;

        public Mutare(Pozitie item1, Pozitie item2)
        {
            MutareInitiala = item1;
            MutareFinala = item2;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Mutare)) return false;
            Mutare other = (Mutare)obj;
            return MutareInitiala == other.MutareInitiala && MutareFinala == other.MutareFinala;
        }

        public bool Equals(Mutare other)
        {
            return EqualityComparer<Pozitie>.Default.Equals(MutareInitiala, other.MutareInitiala) &&
                   EqualityComparer<Pozitie>.Default.Equals(MutareFinala, other.MutareFinala);
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = (int)2166136261;

                hash = (hash * 16777619) ^ MutareInitiala.GetHashCode();
                hash = (hash * 16777619) ^ MutareFinala.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{MutareInitiala.Linie},{MutareInitiala.Coloana} -> {MutareFinala.Linie},{MutareFinala.Coloana}";
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