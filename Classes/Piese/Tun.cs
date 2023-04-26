using System.Collections.Generic;

namespace ProiectVolovici
{
    internal class Tun : Piesa
    {
        int _paritatePiesa;
        public Tun(Culoare culoare, Aspect aspect = Aspect.Normal)
        {
            this.Culoare = culoare;

            this.Selectata = false;
            if (aspect == Aspect.Normal)
            {
                if (culoare == Culoare.AlbastruMax)
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
            else
            {
                if (culoare != Culoare.AlbastruMax)
                {
                    this.Imagine = Properties.Resources.bcannon;
                    this.Cod = CodPiesa.TunAlb;
                }
                else
                {
                    this.Imagine = Properties.Resources.wcannon;
                    this.Cod = CodPiesa.TunAlbastru;
                }
            }
            _paritatePiesa = (int)this.Cod % 2;
        }

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> mutariPosibile = ReturneazaPozitiiPosibile(joc.MatriceCoduriPiese);
            joc.ColoreazaMutariPosibile(pozitii: mutariPosibile);
        }

        public override List<Pozitie> ReturneazaPozitiiPosibile(int[][] matrice)
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
                    for (int linieSec = linie + 1; linieSec < 10; linieSec++)
                    {
                        if (matrice[linieSec][this.Pozitie.Coloana] != 0)
                        {
                            if (matrice[linieSec][this.Pozitie.Coloana] % 2 != _paritatePiesa)
                            {
                                mutariLegale.Add(new Pozitie(linieSec, this.Pozitie.Coloana));
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
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
                    for (int linieSec = linie - 1; linieSec >= 0; linieSec--)
                    {
                        if (matrice[linieSec][this.Pozitie.Coloana] != 0)
                        {
                            if (matrice[linieSec][this.Pozitie.Coloana] % 2 != _paritatePiesa)
                            {
                                mutariLegale.Add(new Pozitie(linieSec, this.Pozitie.Coloana));
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
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
                    for (int coloanaSec = coloana + 1; coloanaSec < 9; coloanaSec++)
                    {
                        if (matrice[this.Pozitie.Linie][coloanaSec] != 0)
                        {
                            if (matrice[this.Pozitie.Linie][coloanaSec] % 2 != _paritatePiesa)
                            {
                                mutariLegale.Add(new Pozitie(this.Pozitie.Linie, coloanaSec));
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
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
                    for (int coloanaSec = coloana - 1; coloanaSec >= 0; coloanaSec--)
                    {
                        if (matrice[this.Pozitie.Linie][coloanaSec] != 0)
                        {
                            if (matrice[this.Pozitie.Linie][coloanaSec] % 2 != _paritatePiesa)
                            {
                                mutariLegale.Add(new Pozitie(this.Pozitie.Linie, coloanaSec));
                                break;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    break;
                }
            }

            //returnare valori
            return mutariLegale;

        }
    }
}