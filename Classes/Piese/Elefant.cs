using System.Collections.Generic;

namespace ProiectVolovici
{
	internal class Elefant : Piesa
	{
		int _paritatePiesa;
		public Elefant(Culoare culoare, Aspect aspect = Aspect.Normal)
		{
			this.Culoare = culoare;

			this.Selectata = false;

			if (aspect == Aspect.Normal)
			{
				if (culoare == Culoare.AlbastruMax)
				{
					this.Imagine = Properties.Resources.belephantrev2;
					this.Cod = CodPiesa.ElefantAlbastru;
				}
				else
				{
					this.Imagine = Properties.Resources.welephantrev2;
					this.Cod = CodPiesa.ElefantAlb;
				}
			}
			else
			{
				if (culoare != Culoare.AlbastruMax)
				{
					this.Imagine = Properties.Resources.belephantrev2;
					this.Cod = CodPiesa.ElefantAlb;
				}
				else
				{
					this.Imagine = Properties.Resources.welephantrev2;
					this.Cod = CodPiesa.ElefantAlbastru;
				}
			}
			_paritatePiesa = (int)Cod % 2;
		}

		public override void ArataMutariPosibile(EngineJoc joc)
		{
			List<Pozitie> mutariPosibile = ReturneazaMutariPosibile(joc.MatriceCoduriPiese);
			joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
		}

		public override List<Pozitie> ReturneazaMutariPosibile(int[][] matrice)
		{
			List<Pozitie> pozitii = new List<Pozitie>(4);

			var poz1 = new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 2);
			var poz2 = new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 2);
			var poz3 = new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 2);
			var poz4 = new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 2);

            if (0 <= poz1.Linie && poz1.Linie <= 9 &&
                0 <= poz1.Coloana && poz1.Coloana <= 8)
				if(
				matrice[this.Pozitie.Linie+1][this.Pozitie.Coloana+1] == 0)
                pozitii.Add(poz1);

            if (0 <= poz2.Linie && poz2.Linie <= 9 &&
                0 <= poz2.Coloana && poz2.Coloana <= 8)
                if (
                matrice[this.Pozitie.Linie + 1][this.Pozitie.Coloana - 1] == 0)
                pozitii.Add(poz2);


            if (0 <= poz3.Linie && poz3.Linie <= 9 &&
                0 <= poz3.Coloana && poz3.Coloana <= 8)
                if (
                matrice[this.Pozitie.Linie - 1][this.Pozitie.Coloana + 1] == 0)
                pozitii.Add(poz3);


            if (0 <= poz4.Linie && poz4.Linie <= 9 &&
                0 <= poz4.Coloana && poz4.Coloana <= 8)
                if (
                matrice[this.Pozitie.Linie - 1][this.Pozitie.Coloana - 1] == 0)
                pozitii.Add(poz4);


			if (Culoare == Culoare.AlbMin)
			{
				for (int i = pozitii.Count - 1; i >= 0; i--)
				{
					if (matrice[pozitii[i].Linie][pozitii[i].Coloana] % 2 == _paritatePiesa ||
						pozitii[i].Linie < 5)
					{
						pozitii.RemoveAt(i);
					}
				}
			}
			else
            {
				for (int i = pozitii.Count - 1; i >= 0; i--)
				{
					if (matrice[pozitii[i].Linie][pozitii[i].Coloana] % 2 == _paritatePiesa ||
						pozitii[i].Linie > 4)
					{
						pozitii.RemoveAt(i);
					}
				}
            }
            return pozitii;
		}
	}
}