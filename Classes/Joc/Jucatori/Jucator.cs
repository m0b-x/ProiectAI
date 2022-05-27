using System;
using System.Collections.Generic;

namespace ProiectVolovici
{
    public abstract class Jucator
    {
        protected String _nume;
        protected CuloareJoc _culoare;
        protected List<Tuple<Pozitie, Pozitie>> _istoricMutari;
        protected Pozitie _ultimaPozitie;
        public Jucator(CuloareJoc culoare)
        {
            _culoare = culoare;
            _ultimaPozitie = new(-1,-1);
        }

        public Pozitie UltimaPozitie
        {
            get { return _ultimaPozitie; }
            set { _ultimaPozitie = value; }
        }
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

        public List<Tuple<Pozitie, Pozitie>> IstoricMutari
        {
            get { return _istoricMutari; }
            set { _istoricMutari = value; }
        }
    }
}