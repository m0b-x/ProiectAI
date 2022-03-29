using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Tun : Piesa
    {
        public Tun(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
            {
                this.Imagine = Properties.Resources.bcannon;
                this.Cod = CodPiesa.TunAlbastru;
            }
            else
            {
                this.Imagine = Properties.Resources.wcannon;
                this.Cod = CodPiesa.TunAlb;
            }
        }
        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> pozitii = new List<Pozitie>();

            int liniePozitiiPosibile = this.Pozitie.Linie + 1;
            while (liniePozitiiPosibile < ConstantaTabla.MarimeVerticala)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
                {
                    pozitii.Add(pozitiePosibila);
                }
                else
                {
                    if (!joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                    {
                        break;
                    }
                }
                liniePozitiiPosibile++;
            }

            liniePozitiiPosibile = this.Pozitie.Linie - 1;
            while (liniePozitiiPosibile >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
                {
                    pozitii.Add(pozitiePosibila);
                }
                else
                {
                    if (!joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                    {
                        break;
                    }
                }
                liniePozitiiPosibile--;
            }

            int colonaPozitiePosibila = this.Pozitie.Coloana + 1;
            while (colonaPozitiePosibila < ConstantaTabla.MarimeOrizontala)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
                {
                    pozitii.Add(pozitiePosibila);
                }
                else
                {
                    if (!joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                    {
                        break;
                    }
                }
                colonaPozitiePosibila++;
            }

            colonaPozitiePosibila = this.Pozitie.Coloana - 1;
            while (colonaPozitiePosibila >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
                {
                    pozitii.Add(pozitiePosibila);
                }
                else
                {
                    if (!joc.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                    {
                        break;
                    }
                }
                colonaPozitiePosibila--;
            }
            joc.ColoreazaMutariPosibile(pozitii: pozitii);
        }
    }
}
