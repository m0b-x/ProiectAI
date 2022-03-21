using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public abstract class Jucator
    {
        private String _nume;
        private CuloareJoc _culoare;
        private List<Tuple<Piesa,Piesa>> _istoricMutari;

        public String Nume
        {
            get { return _nume; }
            set { _nume = value; }
        }
        public CuloareJoc Culoare
        {
            get { return _culoare; }
            set { _culoare = value; }
        }

        public List<Tuple<Piesa, Piesa>> IstoricMutari
        {
            get { return _istoricMutari; }
            set { _istoricMutari = value; }
        }

    }
}
