using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class Tabla
    {
        private Cadran[,] _arrayCadrane;
        private List<Pozitie> _pozitiiPalat;

        private int _pragRau;
        private int _marimeRau;

        private int _marimeVerticala;
        private int _marimeOrizontala;

        private Color _culoareCadranPar;
        private Color _culoareCadranImpar;
        private Color _culoareCadranSelectat;
        private Color _culoareCadranMutari;

        public List<Pozitie> PozitiiPalat
        {
            get { return _pozitiiPalat; }
            set { _pozitiiPalat = value; }
        }
        public int MarimeVerticala
        {
            get { return _marimeVerticala; }
        }
        public int MarimeOrizontala
        {
            get { return _marimeOrizontala; }
        }
        public int PragRau
        {
            get { return _pragRau; }
        }
        public int MarimeRau
        {
            get { return _marimeRau; }
        }
        public Color CuloareCadranPar
        {
            get { return _culoareCadranPar; }
        }

        public Color CuloareCadranImpar
        {
            get { return _culoareCadranImpar; }
        }

        public Color CuloareCadranSelectat
        {
            get { return _culoareCadranSelectat; }
            set { _culoareCadranSelectat = value; }
        }

        public Color CuloareCadranMutari
        {
            get { return _culoareCadranMutari; }
            set { _culoareCadranMutari = value; }
        }
        public Cadran[,] ArrayCadrane
        {
            get { return _arrayCadrane; }
            set { _arrayCadrane = value; }
        }
        public Tabla()
        {
            _marimeVerticala = ConstantaTabla.MarimeVerticala;
            _marimeOrizontala = ConstantaTabla.MarimeOrizontala;

            _pragRau = ConstantaTabla.PragRau;
            _marimeRau = ConstantaTabla.MarimeRau;

            _culoareCadranImpar = ConstantaTabla.CuloareCadranImpar;
            _culoareCadranPar = ConstantaTabla.CuloareCadranPar;
            _culoareCadranMutari = ConstantaTabla.CuloareCadranMutari;
            _culoareCadranSelectat = ConstantaTabla.CuloareCadranSelectat;

            _pozitiiPalat = new List<Pozitie>();
            ConstantaTabla.InitializeazaPolitiiPalat(ref _pozitiiPalat);

            _arrayCadrane = new Cadran[ConstantaTabla.MarimeVerticala, ConstantaTabla.MarimeOrizontala];
        }
        public Color DecideCuloareaCadranului(int linie, int coloana)
        {
            if (_pozitiiPalat.Contains(new Pozitie(linie, coloana)))
            {
                if (linie % 2 == 0)
                {
                    if (coloana % 2 == 1)
                    {
                        return ConstantaTabla.CuloarePalatImpar;
                    }
                    else
                    {
                        return ConstantaTabla.CuloarePalatPar;
                    }
                }
                else
                {
                    if (coloana % 2 == 1)
                    {
                        return ConstantaTabla.CuloarePalatPar;
                    }
                    else
                    {
                        return ConstantaTabla.CuloarePalatImpar;
                    }
                }
            }
            else
            {
                if (linie % 2 == 0)
                {
                    if (coloana % 2 == 1)
                    {
                        return ConstantaTabla.CuloareCadranImpar;
                    }
                    else
                    {
                        return ConstantaTabla.CuloareCadranPar;
                    }
                }
                else
                {
                    if (coloana % 2 == 1)
                    {
                        return ConstantaTabla.CuloareCadranPar;
                    }
                    else
                    {
                        return ConstantaTabla.CuloareCadranImpar;
                    }
                }
            }
        }
    }
}
