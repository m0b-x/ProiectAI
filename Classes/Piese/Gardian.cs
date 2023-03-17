using System.Collections.Generic;

namespace ProiectVolovici
{
	internal class Gardian : Piesa
	{
		int _paritatePiesa;
		public Gardian(Culoare culoare, Aspect aspect = Aspect.Normal)
		{
			this.ValoarePiesa = ConstantaPiese.ValoareGardian;
			this.Culoare = culoare;

			this.Selectata = false;

			if (aspect == Aspect.Normal)
			{
				if (culoare == Culoare.AlbastruMax)
				{
					this.Imagine = Properties.Resources.benvoy;
					this.Cod = CodPiesa.GardianAlbastru;
				}
				else
				{
					this.Imagine = Properties.Resources.wenvoy;
					this.Cod = CodPiesa.GardianAlb;
				}
			}
			else
			{
				if (culoare != Culoare.AlbastruMax)
				{
					this.Imagine = Properties.Resources.benvoy;
					this.Cod = CodPiesa.GardianAlb;
				}
				else
				{
					this.Imagine = Properties.Resources.wenvoy;
					this.Cod = CodPiesa.GardianAlbastru;
				}
			}
			_paritatePiesa = (int)this.Cod % 2;
		}

		public override void ArataMutariPosibile(EngineJoc joc)
		{
			List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc.MatriceCoduriPiese);
			joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
		}

		public override List<Pozitie> ReturneazaMutariPosibile(int[][] matrice)
		{
			List<Pozitie> pozitii = new List<Pozitie>(4);

			pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 1));
			pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 1));
			pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 1));
			pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 1));

            for (int i = pozitii.Count - 1; i >= 0; i--)
            {
                if (ConstantaTabla.PozitiiPalat.Contains(pozitii[i]))
                {
                    if (matrice[pozitii[i].Linie][pozitii[i].Coloana] != 0)
                    {
                        if (matrice[pozitii[i].Linie][pozitii[i].Coloana] % 2 == _paritatePiesa)
                            pozitii.RemoveAt(i);
                    }
                }
                else
                {
                    pozitii.RemoveAt(i);
                }
            }

            return pozitii;
		}
	}
}