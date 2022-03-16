using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici.Classes
{
    abstract class Jucator
    {
        private String _nume;
        private List<Mutare> _istoricMutari;


        public String Nume
        {
            get { return _nume; }
            set { _nume = value; }
        }
        public List<Mutare> IstoricMutari
        {
            get { return _istoricMutari; }
            set { _istoricMutari = value; }
        }
        public void adaugaMutare(Mutare mutare)
        {
            _istoricMutari.Add(mutare);
        }

    }
}
