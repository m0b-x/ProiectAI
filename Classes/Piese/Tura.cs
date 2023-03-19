namespace ProiectVolovici
{
    using System.Collections.Generic;

    internal class Tura : Piesa
    {
        int _paritatePiesa;
        public Tura(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
            this.Culoare = culoare;

            this.Selectata = false;

            if (aspect == Aspect.Normal)
            {
                if (culoare == Culoare.AlbastruMax)
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
            else
            {
                if (culoare != Culoare.AlbastruMax)
                {
                    this.Imagine = Properties.Resources.brook;
                    this.Cod = CodPiesa.TuraAlba;
                }
                else
                {
                    this.Imagine = Properties.Resources.wrook;
                    this.Cod = CodPiesa.TuraAlbastra;
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
            //10 linii + 9 coloane - 1
            List<Pozitie> mutariLegale = new List<Pozitie>(18);

            //linie in jos
            for (int linie = this.Pozitie.Linie + 1; linie < 10; linie++)
            {
                if (matrice[linie][this.Pozitie.Coloana] == 0)
                {
                    mutariLegale.Add(new Pozitie(linie, this.Pozitie.Coloana));
                }
                else
                {
                    if (matrice[linie][this.Pozitie.Coloana] % 2 != _paritatePiesa)
                    {
                        mutariLegale.Add(new Pozitie(linie, this.Pozitie.Coloana));
                    }
                    break;
                }
            }
            //linie in sus
            for (int linie = this.Pozitie.Linie - 1; linie >= 0; linie--)
            {
                if (matrice[linie][this.Pozitie.Coloana] == 0)
                {
                    mutariLegale.Add(new Pozitie(linie, this.Pozitie.Coloana));
                }
                else
                {
                    if (matrice[linie][this.Pozitie.Coloana] % 2 != _paritatePiesa)
                    {
                        mutariLegale.Add(new Pozitie(linie, this.Pozitie.Coloana));
                    }
                    break;
                }
            }
            //coloana in sus
            for (int coloana = this.Pozitie.Coloana + 1; coloana < 9; coloana++)
            {
                if (matrice[this.Pozitie.Linie][coloana] == 0)
                {
                    mutariLegale.Add(new Pozitie(this.Pozitie.Linie, coloana));
                }
                else
                {
                    if (matrice[this.Pozitie.Linie][coloana] % 2 != _paritatePiesa)
                    {
                        mutariLegale.Add(new Pozitie(this.Pozitie.Linie, coloana));
                    }
                    break;
                }
            }

            //coloana in jos

            for (int coloana = this.Pozitie.Coloana - 1; coloana >= 0; coloana--)
            {
                if (matrice[this.Pozitie.Linie][coloana] == 0)
                {
                    mutariLegale.Add(new Pozitie(this.Pozitie.Linie, coloana));
                }
                else
                {
                    if (matrice[this.Pozitie.Linie][coloana] % 2 != _paritatePiesa)
                    {
                        mutariLegale.Add(new Pozitie(this.Pozitie.Linie, coloana));
                    }
                    break;
                }
            }

            //returnare valori
            return mutariLegale;
        }
    }
}