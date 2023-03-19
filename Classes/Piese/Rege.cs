using System.Collections.Generic;

namespace ProiectVolovici
{
	internal class Rege : Piesa
	{
		int _paritatePiesa;
		public Rege(Culoare culoare, Aspect aspect = Aspect.Normal)
		{
			this.Culoare = culoare;

			this.Selectata = false;

			if (aspect == Aspect.Normal)
			{
				if (culoare == Culoare.AlbastruMax)
				{
					this.Imagine = Properties.Resources.bking;
					this.Cod = CodPiesa.RegeAlbastru;
				}
				else
				{
					this.Imagine = Properties.Resources.wking;
					this.Cod = CodPiesa.RegeAlb;
				}
			}
			else
			{
				if (culoare != Culoare.AlbastruMax)
				{
					this.Imagine = Properties.Resources.bking;
					this.Cod = CodPiesa.RegeAlb;
				}
				else
				{
					this.Imagine = Properties.Resources.wking;
					this.Cod = CodPiesa.RegeAlbastru;
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

			pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana));
			pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana));
			pozitii.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1));
			pozitii.Add(new Pozitie(_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1));


			int ultimaLinie = ConstantaTabla.NrLinii - 1;
			int ultimaColoana = ConstantaTabla.NrColoane - 1;

            for (int i = pozitii.Count - 1; i >= 0; i--)
            {
				if (ConstantaTabla.PozitiiPalat.Contains(pozitii[i]))
				{
					if (matrice[pozitii[i].Linie][pozitii[i].Coloana] != 0)
					{
						if(matrice[pozitii[i].Linie][pozitii[i].Coloana] % 2 ==  _paritatePiesa)
							pozitii.RemoveAt(i);
					}
				}
				else
                {
                    pozitii.RemoveAt(i);
                }
            }


            for (int i = pozitii.Count - 1; i >= 0; i--)
            {
				if (this.Culoare == Culoare.AlbastruMax)
				{
					for (int linie = this.Pozitie.Linie+1; linie <= ultimaLinie; linie++)
					{
						if (matrice[linie][this.Pozitie.Coloana] != 0)
						{
							if (matrice[linie][this.Pozitie.Coloana] == (int)CodPiesa.RegeAlb)
							{
								pozitii.RemoveAt(i);
							}
							else
							{
								break;
							}
						}
					}
				}
				else
				{
                    for (int linie = this.Pozitie.Linie-1; linie >=0; linie--)
                    {
                        if (matrice[linie][this.Pozitie.Coloana] == (int)CodPiesa.RegeAlbastru)
                        {
                            pozitii.RemoveAt(i);
                        }
                        else
                        {
							break;
                        }
                    }
                }
            }


            return pozitii;
		}
	}
}