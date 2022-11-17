using System;

namespace ProiectVolovici
{
    public struct IntrareTabelTranspozitie : IEquatable<IntrareTabelTranspozitie>
    {
        private long _hashCode;
        private int _adancime;
        private double _alpha;
        private double _beta;

        public int Adancime
        {
            get { return _adancime; }
            set { _adancime = value; }
        }

        public double Alpha
        {
            get { return _alpha; }
            set { _alpha = value; }
        }

        public double Beta
        {
            get { return _beta; }
            set { _beta = value; }
        }

        public override bool Equals(object obj)
        {
            return obj is IntrareTabelTranspozitie transpozitie && Equals(transpozitie);
        }

        public IntrareTabelTranspozitie(int adancime, double alpha, double beta) : this()
        {
            _adancime = adancime;
            _alpha = alpha;
            _beta = beta;
        }

        public bool Equals(IntrareTabelTranspozitie other)
        {
            return _hashCode == other._hashCode &&
                   _adancime == other._adancime;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_hashCode, _adancime);
        }

        public static bool operator ==(IntrareTabelTranspozitie left, IntrareTabelTranspozitie right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntrareTabelTranspozitie left, IntrareTabelTranspozitie right)
        {
            return !(left == right);
        }
    }
}