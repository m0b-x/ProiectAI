using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Pion : Piesa
    {
        int _paritatePiesa;
        public Pion(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
            this.Culoare = culoare;

            this.Selectata = false;

            if (aspect == Aspect.Normal)
            {
                if (culoare == Culoare.AlbastruMax)
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
            else
            {
                if (culoare != Culoare.AlbastruMax)
                {
                    this.Imagine = Properties.Resources.bpawn;
                    this.Cod = CodPiesa.PionAlb;
                }
                else
                {
                    this.Imagine = Properties.Resources.wpawn;
                    this.Cod = CodPiesa.PionAlbastru;
                }
            }
            _paritatePiesa = (int)Cod % 2;
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaPozitiiPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(mutariPosibile);
        }

        public override List<Pozitie> ReturneazaPozitiiPosibile(int[][] matrice)
        {
            List<Pozitie> pozitii = new(3);
            if (this.Culoare == Culoare.AlbastruMax)
            {
                if (this.Pozitie.Linie > 4)
                {
                    pozitii.Add(new Pozitie(this.Pozitie.Linie + 1, this.Pozitie.Coloana));
                    pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana + 1));
                    pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                }
                else
                {
                    pozitii.Add(new Pozitie(this.Pozitie.Linie + 1, this.Pozitie.Coloana));
                }
            }
            else
            {
                if (this.Pozitie.Linie < 5)
                {
                    pozitii.Add(new Pozitie(this.Pozitie.Linie - 1, this.Pozitie.Coloana));
                    pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana + 1));
                    pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                }
                else
                {
                    pozitii.Add(new Pozitie(this.Pozitie.Linie - 1, this.Pozitie.Coloana));
                }
            }

            for (int i = pozitii.Count - 1; i >= 0; i--)
            {
                if (0 > pozitii[i].Linie || pozitii[i].Linie > 9 ||
                    0 > pozitii[i].Coloana || pozitii[i].Coloana > 8 ||
					matrice[pozitii[i].Linie][pozitii[i].Coloana] != 0 &&
						matrice[pozitii[i].Linie][pozitii[i].Coloana] % 2 == _paritatePiesa)
                {
                    pozitii.RemoveAt(i);
                }

            }
            return pozitii;
        }
    }
}