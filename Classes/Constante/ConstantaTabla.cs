﻿using System.Collections.Generic;
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
            new Pozitie(0, 3),
            new Pozitie(0, 4),
            new Pozitie(0, 5),

            new Pozitie(1, 3),
            new Pozitie(1, 4),
            new Pozitie(1, 5),

            new Pozitie(2, 3),
            new Pozitie(2, 4),
            new Pozitie(2, 5),

            new Pozitie(7, 3),
            new Pozitie(7, 4),
            new Pozitie(7, 5),

            new Pozitie(8, 3),
            new Pozitie(8, 4),
            new Pozitie(8, 5),

            new Pozitie(9, 3),
            new Pozitie(9, 4),
            new Pozitie(9, 5)
        };

        public static void InitializeazaPolitiiPalat(ref List<Pozitie> _pozitiiPalat)
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