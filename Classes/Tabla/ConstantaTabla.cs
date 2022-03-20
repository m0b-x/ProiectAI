using System;
using System.Collections.Generic;
using System.Drawing;

namespace ProiectVolovici
{
    public static class ConstantaTabla
    {
        public static readonly int MarimeOrizontala = 9;
        public static readonly int MarimeVerticala = 10;
        public static readonly int PragRau = 4;
        public static readonly int MarimeRau = 10;

        public static Color CuloareCadranPar = Color.BlanchedAlmond;
        public static Color CuloareCadranImpar = Color.DarkGreen;
        public static Color CuloareCadranSelectat = Color.DeepSkyBlue;
        public static Color CuloareCadranMutari = Color.DodgerBlue;

        public static Color CuloarePalatPar = Color.OldLace;
        public static Color CuloarePalatImpar = Color.Olive;

        public static Color CuloarePiesaBlocata = Color.DarkRed;

        public static readonly Piesa PiesaNula = null;


        public static void InitializeazaPalat(ref List<Pozitie> _pozitiiPalat)
        {
            _pozitiiPalat.Add(new Pozitie(0, 3));
            _pozitiiPalat.Add(new Pozitie(0, 4));
            _pozitiiPalat.Add(new Pozitie(0, 5));

            _pozitiiPalat.Add(new Pozitie(1, 3));
            _pozitiiPalat.Add(new Pozitie(1, 4));
            _pozitiiPalat.Add(new Pozitie(1, 5));

            _pozitiiPalat.Add(new Pozitie(2, 3));
            _pozitiiPalat.Add(new Pozitie(2, 4));
            _pozitiiPalat.Add(new Pozitie(2, 5));

            _pozitiiPalat.Add(new Pozitie(7, 3));
            _pozitiiPalat.Add(new Pozitie(7, 4));
            _pozitiiPalat.Add(new Pozitie(7, 5));

            _pozitiiPalat.Add(new Pozitie(8, 3));
            _pozitiiPalat.Add(new Pozitie(8, 4));
            _pozitiiPalat.Add(new Pozitie(8, 5));

            _pozitiiPalat.Add(new Pozitie(9, 3));
            _pozitiiPalat.Add(new Pozitie(9, 4));
            _pozitiiPalat.Add(new Pozitie(9, 5));
        }

    }
}
