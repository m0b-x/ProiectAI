﻿using System.Collections.Generic;

namespace ProiectVolovici
{
	internal class Cal : Piesa
	{
		int _paritatePiesa;

        public Cal(Culoare culoare, Aspect aspect = Aspect.Normal)
		{
			this.ValoarePiesa = ConstantaPiese.ValoareCal;
			this.Culoare = culoare;

			this.Selectata = false;

			if (aspect == Aspect.Normal)
			{
				if (culoare == Culoare.AlbastruMax)
				{
					this.Imagine = Properties.Resources.bhorse;
					this.Cod = CodPiesa.CalAbastru;
				}
				else
				{
					this.Imagine = Properties.Resources.whorse;
					this.Cod = CodPiesa.CalAlb;
				}
			}
			else
			{
				if (culoare != Culoare.AlbastruMax)
				{
					this.Imagine = Properties.Resources.bhorse;
					this.Cod = CodPiesa.CalAlb;
				}
				else
				{
					this.Imagine = Properties.Resources.whorse;
					this.Cod = CodPiesa.CalAbastru;
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

            var poz1 = new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 1);
            var poz2 = new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 1);
            var poz3 = new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 1);
            var poz4 = new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 1);
            var poz5 = new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 2);
            var poz6 = new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 2);
            var poz7 = new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 2);
            var poz8 = new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 2);

			if (0 <= poz1.Linie && poz1.Linie <= 9 &&
				0 <= poz1.Coloana && poz1.Coloana <= 8)
			{
				if (matrice[this.Pozitie.Linie + 1][this.Pozitie.Coloana] == 0)
				{
					pozitii.Add(poz1);
				}
			}

            if (0 <= poz2.Linie && poz2.Linie <= 9 &&
                0 <= poz2.Coloana && poz2.Coloana <= 8)
            {
                if (matrice[this.Pozitie.Linie + 1][this.Pozitie.Coloana] == 0)
                {
                    pozitii.Add(poz2);
                }
            }
			if (0 <= poz3.Linie && poz3.Linie <= 9 &&
                0 <= poz3.Coloana && poz3.Coloana <= 8)
			{
				if (matrice[this.Pozitie.Linie - 1][this.Pozitie.Coloana] == 0)
				{
					pozitii.Add(poz3);
				}
			}

            if (0 <= poz4.Linie && poz4.Linie <= 9 &&
                0 <= poz4.Coloana && poz4.Coloana <= 8)
            {
                if (matrice[this.Pozitie.Linie - 1][this.Pozitie.Coloana] == 0)
                {
                    pozitii.Add(poz4);
                }
            }

			if (0 <= poz5.Linie && poz5.Linie <= 9 &&
                0 <= poz5.Coloana && poz5.Coloana <= 8)
			{
				if (matrice[this.Pozitie.Linie][this.Pozitie.Coloana + 1] == 0)
				{
					pozitii.Add(poz5);
				}
			}
            if (0 <= poz6.Linie && poz6.Linie <= 9 &&
                0 <= poz6.Coloana && poz6.Coloana <= 8)
            {
                if (matrice[this.Pozitie.Linie][this.Pozitie.Coloana + 1] == 0)
                {
                    pozitii.Add(poz6);
                }
            }


			if (0 <= poz7.Linie && poz7.Linie <= 9 &&
                0 <= poz7.Coloana && poz7.Coloana <= 8)
			{
				if (matrice[this.Pozitie.Linie][this.Pozitie.Coloana - 1] == 0)
				{
					pozitii.Add(poz7);
				}
			}

            if (0 <= poz8.Linie && poz8.Linie <= 9 &&
                0 <= poz8.Coloana && poz8.Coloana <= 8)
            {
                if (matrice[this.Pozitie.Linie][this.Pozitie.Coloana - 1] == 0)
                {
                    pozitii.Add(poz8);
                }
            }

            for (int i = pozitii.Count - 1; i >= 0; i--)
            {
                if (matrice[pozitii[i].Linie][pozitii[i].Coloana] % 2 == _paritatePiesa)
                {
                    pozitii.RemoveAt(i);
                }
            }

            return pozitii;
        }
	}
}