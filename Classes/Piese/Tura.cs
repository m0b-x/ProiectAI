namespace ProiectVolovici
{
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// Defines the <see cref="Tura" />.
    /// </summary>
    internal class Tura : Piesa
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Tura"/> class.
        /// </summary>
        /// <param name="culoare">The culoare<see cref="Culoare"/>.</param>
        public Tura(Culoare culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == Culoare.Albastru)
            {
                this.Imagine = Properties.Resources.brook;
                this.Cod = CodPiesa.TuraAlbastra;
            }
            else
            {
                this.Imagine = Properties.Resources.wrook;
                this.Cod = CodPiesa.TuraAlba;
            }
        }

        /// <summary>
        /// The ArataMutariPosibile.
        /// </summary>
        /// <param name="tabla">The tabla<see cref="Tabla"/>.</param>
        public override void ArataMutariPosibile(Tabla tabla)
        {
            Debug.WriteLine("ArataMutariPosibileTura " + this._codPiesa);
            List<Pozitie> pozitii = new List<Pozitie>();

            int liniePozitiiPosibile = this.Pozitie.Linie + 1;
            while (liniePozitiiPosibile < ConstantaTabla.MarimeVerticala)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (tabla.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
                {
                    pozitii.Add(pozitiePosibila);
                }
                else
                {
                    if(tabla.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                    {
                        pozitii.Add(pozitiePosibila);
                    }
                    break;
                }
                liniePozitiiPosibile++;
            }

            liniePozitiiPosibile = this.Pozitie.Linie - 1;
            while (liniePozitiiPosibile >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: liniePozitiiPosibile, coloana: this.Pozitie.Coloana);
                if (tabla.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
                {
                    pozitii.Add(pozitiePosibila);
                }
                else
                {
                    if (tabla.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                    {
                        pozitii.Add(pozitiePosibila);
                    }
                    break;
                }
                liniePozitiiPosibile--;
            }

            int colonaPozitiePosibila = this.Pozitie.Coloana + 1;
            while (colonaPozitiePosibila < ConstantaTabla.MarimeOrizontala)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (tabla.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
                {
                    pozitii.Add(pozitiePosibila);
                }
                else
                {
                    if (tabla.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                    {
                        pozitii.Add(pozitiePosibila);
                    }
                    break;
                }
                colonaPozitiePosibila++;
            }

            colonaPozitiePosibila = this.Pozitie.Coloana - 1;
            while (colonaPozitiePosibila >= 0)
            {
                Pozitie pozitiePosibila = new Pozitie(linie: this.Pozitie.Linie, coloana: colonaPozitiePosibila);
                if (tabla.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].CadranEsteGol())
                {
                    pozitii.Add(pozitiePosibila);
                }
                else
                {
                    if (tabla.ArrayCadrane[pozitiePosibila.Linie, pozitiePosibila.Coloana].EsteAdversar(this.CuloarePiesa))
                    {
                        pozitii.Add(pozitiePosibila);
                    }
                    break;
                }
                colonaPozitiePosibila--;
            }
            tabla.ColoreazaMutariPosibile(pozitii: pozitii);
        }
    }
}
