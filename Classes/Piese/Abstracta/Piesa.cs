using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProiectVolovici
{
    public abstract class Piesa : IDisposable
    {
        protected CuloareJoc _culoarePiesa;
        protected Pozitie _pozitiePiesa;
        protected Image _imaginePiesa;

        protected CodPiesa _codPiesa;

        protected double _valoarePiesa;
        protected bool _pusaPeTabla = false;
        protected bool _selectata;

        public CuloareJoc CuloarePiesa
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

        public double ValoarePiesa
        {
            get { return _valoarePiesa; }
            set { _valoarePiesa = value; }
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _imaginePiesa.Dispose();
        }

        public abstract void ArataMutariPosibile(EngineJoc joc);
        public abstract List<Pozitie> ReturneazaMutariPosibile(EngineJoc joc);


    }
}