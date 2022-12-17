using System;

namespace ProiectVolovici
{
    public struct IntrareTabelTranspozitie : IEquatable<IntrareTabelTranspozitie>
    {
        private int _adancime;
        private double _valuare;
        private FlagIntrare _flagIntrare;

        public FlagIntrare FlagIntrare
        {
            get { return _flagIntrare; }
            set { _flagIntrare = value; }
        }
        public int Adancime
        {
            get { return _adancime; }
            set { _adancime = value; }
        }

        public double Valuare
        {
            get { return _valuare; }
            set { _valuare = value; }
        }
        public override bool Equals(object obj)
        {
            return obj is IntrareTabelTranspozitie transpozitie && Equals(transpozitie);
        }

        public IntrareTabelTranspozitie(int adancime, double val, FlagIntrare flag) : this()
        {
            _adancime = adancime;
            _valuare = val;
            _flagIntrare = flag;
        }

        public bool Equals(IntrareTabelTranspozitie other)
        {
            return  _adancime == other._adancime && _flagIntrare == other._flagIntrare;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine( _adancime, _flagIntrare);
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