using System;
using System.Collections.Generic;

namespace ProiectVolovici
{
    public struct Mutare : IEquatable<Mutare>
    {
        public Pozitie PozitieInitiala;
        public Pozitie PozitieFinala;

        public Mutare(Pozitie item1, Pozitie item2)
        {
            PozitieInitiala = item1;
            PozitieFinala = item2;
        }

        public void Reverse()
        {
            var aux = PozitieFinala;
            PozitieFinala = PozitieInitiala;
            PozitieInitiala = aux;
        }
        public override bool Equals(object obj)
        {
            if (!(obj is Mutare)) return false;
            Mutare other = (Mutare)obj;
            return PozitieInitiala == other.PozitieInitiala && PozitieFinala == other.PozitieFinala;
        }

        public bool Equals(Mutare other)
        {
            return EqualityComparer<Pozitie>.Default.Equals(PozitieInitiala, other.PozitieInitiala) &&
                   EqualityComparer<Pozitie>.Default.Equals(PozitieFinala, other.PozitieFinala);
        }

        public override int GetHashCode()
        {
            unchecked 
            {
                int hash = (int)2166136261;

                hash = (hash * 16777619) ^ PozitieInitiala.GetHashCode();
                hash = (hash * 16777619) ^ PozitieFinala.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return $"{PozitieInitiala.Linie},{PozitieInitiala.Coloana} -> {PozitieFinala.Linie},{PozitieFinala.Coloana}";
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