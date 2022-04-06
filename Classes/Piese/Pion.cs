using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Pion : Piesa
    {
        public Pion(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
            {
                this.Imagine = Properties.Resources.bpawn;
                this.Cod = CodPiesa.PionAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.wpawn;
                this.Cod = CodPiesa.PionAlb;
            }
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc);
            joc.ColoreazaMutariPosibile(mutariPosibile);
        }

        public override List<Pozitie> ReturneazaMutariPosibile(EngineJoc joc)
        {
            const int primaLinie = 0;
            const int primaColoana = 0;
            int ultimaLinie = ConstantaTabla.MarimeVerticala - 1;
            int ultimaColoana = ConstantaTabla.MarimeOrizontala - 1;

            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariFiltruTabla = new List<Pozitie>();

            if (this.CuloarePiesa == CuloareJoc.Albastru)
            {
                mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana));
                if (_pozitiePiesa.Linie > joc.PragRau)
                {
                    mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
                    mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));
                }
            }
            else
            {
                mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana));
                if (_pozitiePiesa.Linie <= joc.PragRau)
                {
                    mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
                    mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));
                }
            }
            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if (pozitie.Linie <= ultimaLinie &&
                     pozitie.Linie >= primaLinie &&
                     pozitie.Coloana <= ultimaColoana &&
                     pozitie.Coloana >= primaColoana)
                {
                    if (joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                    {
                        mutariFiltruTabla.Add(pozitie);
                    }
                    else if (joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran.CuloarePiesa != this.CuloarePiesa)
                    {
                        mutariFiltruTabla.Add(pozitie);
                    }
                }
            }

            return mutariFiltruTabla;
        }
    }
}