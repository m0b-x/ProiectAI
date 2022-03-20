﻿using System;
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
        protected Pozitie _pozitiePiesa;
        protected Image _imaginePiesa;
        protected CodPiesa _codPiesa;
        protected bool _pusaPeTabla = false;
        protected bool _selectata;

        public Culoare CuloarePiesa
        {
            get { return _culoarePiesa; }
            set { _culoarePiesa = value; }
        }

        public Pozitie Pozitie
        {
            get { return _pozitiePiesa; }
            set { _pozitiePiesa = value; }
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

        public bool Selectata
        {
            get { return _selectata; }
            set { _selectata = value; }
        }

        public abstract void ArataMutariPosibile(Tabla tabla);

    }
}
