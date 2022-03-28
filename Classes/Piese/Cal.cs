using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Cal : Piesa
    {

        public Cal(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
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
       public override void ArataMutariPosibile(EngineJoc joc)
       {
            List<Pozitie> pozitii = new List<Pozitie>();

            const int primaLinie = 0;
            const int primaColoana = 0;
            int ultimaLinie = ConstantaTabla.MarimeOrizontala - 1;
            int ultimaColoana = ConstantaTabla.MarimeVerticala - 1;

            if (_pozitiePiesa.Linie < ultimaLinie - 1)
            {
                if (_pozitiePiesa.Coloana < ultimaColoana)
                {
                    if (joc.MatriceCodPiese[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana] == (int)CodPiesa.Gol)
                    {
                        if (joc.MatriceCodPiese[_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 1] == (int)CodPiesa.Gol)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 1));
                        }
                        else if (joc.GetPiesaCuPozitia(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 1)).CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 1));
                        }
                    }
                }
                else if (_pozitiePiesa.Coloana > primaColoana)
                {
                    if (joc.MatriceCodPiese[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana] == (int)CodPiesa.Gol)
                    {
                        if (joc.MatriceCodPiese[_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 1] == (int)CodPiesa.Gol)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 1));
                        }
                        else if (joc.GetPiesaCuPozitia(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 1)).CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 1));
                        }
                    }
                }
            }
            else if(_pozitiePiesa.Linie > primaLinie + 1)
            {
                if (_pozitiePiesa.Coloana < ultimaColoana)
                {
                    if (joc.MatriceCodPiese[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana] == (int)CodPiesa.Gol)
                    {
                        if (joc.MatriceCodPiese[_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 1] == (int)CodPiesa.Gol)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 1));
                        }
                        else if (joc.GetPiesaCuPozitia(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 1)).CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 1));
                        }
                    }
                }
                else if (_pozitiePiesa.Coloana > primaColoana)
                {
                    if (joc.MatriceCodPiese[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana] == (int)CodPiesa.Gol)
                    {
                        if (joc.MatriceCodPiese[_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 1] == (int)CodPiesa.Gol)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 1));
                        }
                        else if (joc.GetPiesaCuPozitia(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 1)).CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 1));
                        }
                    }
                }
            }

            if (_pozitiePiesa.Coloana < ultimaColoana - 1)
            {
                if (_pozitiePiesa.Linie < ultimaLinie)
                {
                    if (joc.MatriceCodPiese[_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1] == (int)CodPiesa.Gol)
                    {
                        if (joc.MatriceCodPiese[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 2] == (int)CodPiesa.Gol)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 2));
                        }
                        else if (joc.GetPiesaCuPozitia(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 2)).CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana + 2));
                        }
                    }
                }
                else if (_pozitiePiesa.Linie > primaLinie)
                {
                    if (joc.MatriceCodPiese[_pozitiePiesa.Linie, _pozitiePiesa.Coloana + 1] == (int)CodPiesa.Gol)
                    {
                        if (joc.MatriceCodPiese[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 2] == (int)CodPiesa.Gol)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 2));
                        }
                        else if (joc.GetPiesaCuPozitia(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 2)).CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana + 2));
                        }
                    }
                }
            }
            else if (_pozitiePiesa.Coloana > primaColoana + 1)
            {
                if (_pozitiePiesa.Linie > primaLinie)
                {
                    if (joc.MatriceCodPiese[_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1] == (int)CodPiesa.Gol)
                    {
                        if (joc.MatriceCodPiese[_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 2] == (int)CodPiesa.Gol)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 2));
                        }
                        else if (joc.GetPiesaCuPozitia(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 2)).CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie + 1, _pozitiePiesa.Coloana - 2));
                        }
                    }
                }

                else if (_pozitiePiesa.Linie < ultimaLinie)
                {
                    if (joc.MatriceCodPiese[_pozitiePiesa.Linie, _pozitiePiesa.Coloana - 1] == (int)CodPiesa.Gol)
                    {
                        if (joc.MatriceCodPiese[_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 2] == (int)CodPiesa.Gol)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 2));
                        }
                        else if (joc.GetPiesaCuPozitia(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 2)).CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(_pozitiePiesa.Linie - 1, _pozitiePiesa.Coloana - 2));
                        }
                    }
                }
            }
            joc.ColoreazaMutariPosibile(pozitii: pozitii);
        }
    }
}
