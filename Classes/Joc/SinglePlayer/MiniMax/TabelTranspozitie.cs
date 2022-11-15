using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici.Classes.Joc.SinglePlayer.MiniMax
{
    struct TabelTranspozitie : IEquatable<TabelTranspozitie>
    {
        long hashCode;
        double evaluare;
        TipIntrare tipIntrare;
        int adancime;
        int generatie;

        public TabelTranspozitie(long hashCode, double evaluare, TipIntrare tipIntrare, int adancime, int generatie)
        {
            this.hashCode = hashCode;
            this.evaluare = evaluare;
            this.tipIntrare = tipIntrare;
            this.adancime = adancime;
            this.generatie = generatie;
        }

        public override bool Equals(object obj)
        {
            return obj is TabelTranspozitie transpozitie && Equals(transpozitie);
        }

        public bool Equals(TabelTranspozitie other)
        {
            return hashCode == other.hashCode &&
                   evaluare == other.evaluare &&
                   tipIntrare == other.tipIntrare &&
                   adancime == other.adancime &&
                   generatie == other.generatie;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(hashCode, evaluare, tipIntrare, adancime, generatie);
        }
    }
}
