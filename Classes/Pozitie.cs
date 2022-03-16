using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class Pozitie
    {
        private int _linie;
        private int _coloana;

        public int Linie
        {
            get { return _linie; }
            set { _linie = value; }
        }

        public int Coloana
        {
            get { return _coloana; }
            set { _coloana = value; }
        }
        
        public Pozitie(int linie,int coloana)
        {
            _linie = linie;
            _coloana = coloana;
        }

        public static bool operator ==(Pozitie pozitieStanga, Pozitie pozitieDreapta)
        {
            if (pozitieStanga is null)
            {
                if (pozitieDreapta is null)
                {
                    return true;
                }
                return false;
            }
            return (pozitieStanga.Linie == pozitieDreapta.Linie && pozitieStanga.Coloana == pozitieDreapta.Coloana);
        }
        public static bool operator !=(Pozitie pozitieStanga, Pozitie pozitieDreapta) => !(pozitieStanga == pozitieDreapta);


    }
}
