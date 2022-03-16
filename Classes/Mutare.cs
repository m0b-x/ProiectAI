using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici.Classes
{
    public class Mutare
    {
        private int _linieInitiala;
        private int _coloanaInitiala;

        private int _linieFinala;
        private int _coloanaFinala;

        private Piesa _piesaMutata;
        private Piesa _piesaLuata;

        public Mutare()
        {
        }

        public void SetPozitieInitiala(int linie,int coloana)
        {
            _linieInitiala = linie;
            _coloanaInitiala = coloana;
        }

        public void SetPozitieFinala(int linie,int coloana)
        {
            _linieFinala = linie;
            _coloanaFinala = coloana;
        }

        public void SetPiese(Piesa piesaMutata,Piesa piesaLuata)
        {
            _piesaMutata = piesaMutata;
            _piesaLuata = piesaLuata;
        }

        public int LinieInitiala
        {
            get { return _linieInitiala; }
            set { _linieInitiala = value; }
        }

        public int ColoanaInitiala
        {
            get { return _coloanaInitiala; }
            set { _coloanaInitiala = value; }
        }

        public int LinieFinala
        {
            get { return _linieFinala; }
            set { _linieFinala = value; }
        }

        public int ColoanaFinala
        {
            get { return _coloanaFinala; }
            set { _coloanaFinala = value; }
        }
        
        public Piesa PiesaMutata
        {
            get { return _piesaMutata; }
            set { _piesaMutata = value; }
        }

        public Piesa PiesaLuata
        {
            get { return _piesaLuata; }
            set { _piesaLuata = value; }
        }

    }
}
