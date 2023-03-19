using System.Collections.Generic;
using System.Drawing;

namespace ProiectVolovici
{
    public static class ConstantaTabla
    {
        public const int Adancime = 4;

        public static int NuEsteSah = 0;
        public static int SahLaRegeAlb = 1;
        public static int SahLaRegerAlbastru = 2;
        public static int NrSahuriPermise = 3;
        public static int CompensareNrSahuri = 4;
        public static int NrMaximSahuri = NrSahuriPermise * CompensareNrSahuri;

        public static readonly int NrColoane = 9;
        public static readonly int NrLinii = 10;
        public static readonly int PragRau = 4;
        public static readonly int MarimeRau = 10;

        public static Color CuloareCadranPar = Color.BlanchedAlmond;
        public static Color CuloareCadranImpar = Color.DarkGreen;
        public static Color CuloareCadranSelectat = Color.DeepSkyBlue;
        public static Color CuloareCadranMutari = Color.DodgerBlue;

        public static Color CuloarePalatPar = Color.OldLace;
        public static Color CuloarePalatImpar = Color.Olive;

        public static Color CuloarePozitieBlocata = Color.DarkRed;

        public static readonly Piesa PiesaNula = null;

        public static readonly int IntervalPiesaBlocata = 300;

        public static readonly int MarimeFont = 15;

        public static readonly string FontPrincipal = "Arial";
        public static readonly string FontSecundar = "Segoe UI";

        public static readonly int TimpAsteptariAI = 600;

        public static List<Pozitie> PozitiiPalat = new()
        {
            Pozitie.AcceseazaElementStatic(0, 3),
            Pozitie.AcceseazaElementStatic(0, 4),
            Pozitie.AcceseazaElementStatic(0, 5),

            Pozitie.AcceseazaElementStatic(1, 3),
            Pozitie.AcceseazaElementStatic(1, 4),
            Pozitie.AcceseazaElementStatic(1, 5),

            Pozitie.AcceseazaElementStatic(2, 3),
            Pozitie.AcceseazaElementStatic(2, 4),
            Pozitie.AcceseazaElementStatic(2, 5),

            Pozitie.AcceseazaElementStatic(7, 3),
            Pozitie.AcceseazaElementStatic(7, 4),
            Pozitie.AcceseazaElementStatic(7, 5),

            Pozitie.AcceseazaElementStatic(8, 3),
            Pozitie.AcceseazaElementStatic(8, 4),
            Pozitie.AcceseazaElementStatic(8, 5),

            Pozitie.AcceseazaElementStatic(9, 3),
            Pozitie.AcceseazaElementStatic(9, 4),
            Pozitie.AcceseazaElementStatic(9, 5)
        };

        public static void InitializeazaPolitiiPalat(ref List<Pozitie> _pozitiiPalat)
        {
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(0, 3));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(0, 4));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(0, 5));

            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(1, 3));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(1, 4));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(1, 5));

            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(2, 3));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(2, 4));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(2, 5));

            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(7, 3));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(7, 4));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(7, 5));

            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(8, 3));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(8, 4));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(8, 5));

            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(9, 3));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(9, 4));
            _pozitiiPalat.Add(Pozitie.AcceseazaElementStatic(9, 5));
        }
    }
}