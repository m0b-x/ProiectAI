using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public abstract class Piesa
    {
        protected Culoare _culoarePiesa;
        protected int _liniePiesa;
        protected int _coloanaPiesa;
        protected Image _imaginePiesa;
        protected CodPiesa _codPiesa;
        protected bool _pusaPeTabla;

        public Culoare CuloarePiesa
        {
            get { return _culoarePiesa; }
            set { _culoarePiesa = value; }
        }
        public int Linie
        {
            get { return _liniePiesa; }
            set { _liniePiesa = value; }
        }

        public int Coloana
        {
            get { return _coloanaPiesa; }
            set { _coloanaPiesa = value; }
        }
        public Image Imagine
        {
            get { return _imaginePiesa; }
            set { _imaginePiesa = value; }
        }

        public CodPiesa Cod
        {
            get { return _codPiesa; }
            set { _codPiesa = value; }
        }

        public bool PusaPeTabla
        {
            get { return _pusaPeTabla; }
            set { _pusaPeTabla = value; }
        }

    }
}
