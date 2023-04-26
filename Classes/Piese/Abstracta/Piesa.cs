using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProiectVolovici
{
    public abstract class Piesa : IDisposable, IEquatable<Piesa>
    {
        protected Culoare _culoarePiesa;
        protected Aspect _aspectPiesa;
        protected Pozitie _pozitiePiesa;
        protected Image _imaginePiesa;

        protected CodPiesa _codPiesa;
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


        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _imaginePiesa.Dispose();
        }

        public abstract void ArataMutariPosibile(EngineJoc joc);

        public abstract List<Pozitie> ReturneazaPozitiiPosibile(int[][] matrice);

        public override bool Equals(object obj)
        {
            return Equals(obj as Piesa);
        }

        public bool Equals(Piesa other)
        {
            return other is not null &&
                   EqualityComparer<Pozitie>.Default.Equals(_pozitiePiesa, other._pozitiePiesa) &&
                   _codPiesa == other._codPiesa;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(_pozitiePiesa, _codPiesa);
        }

        public static bool operator ==(Piesa left, Piesa right)
        {
            return EqualityComparer<Piesa>.Default.Equals(left, right);
        }

        public static bool operator !=(Piesa left, Piesa right)
        {
            return !(left == right);
        }

        //
    }
}