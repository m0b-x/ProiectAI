using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProiectVolovici
{
    public abstract class Piesa : IDisposable
    {
        protected Culoare _culoarePiesa;
        protected Aspect _aspectPiesa;
        protected Pozitie _pozitiePiesa;
        protected Image _imaginePiesa;

        protected CodPiesa _codPiesa;

        protected double _valoarePiesa;
        protected bool _selectata;

        public Aspect Aspect
        {
            get { return _aspectPiesa; }
            set { _aspectPiesa = value; }
        }
        public Culoare Culoare
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

        public abstract List<Pozitie> ReturneazaMutariPosibile(int[][] matrice);

        public override bool Equals(object obj)
        {
            return obj is Piesa piesa &&
                   _culoarePiesa == piesa._culoarePiesa &&
                   EqualityComparer<Pozitie>.Default.Equals(_pozitiePiesa, piesa._pozitiePiesa) &&
                   _codPiesa == piesa._codPiesa;
        }
    }
}