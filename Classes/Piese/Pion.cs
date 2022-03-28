using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Pion : Piesa
    {
        
        public Pion(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
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

        public override void ArataMutariPosibile(EngineJoc joc)
        {
            List<Pozitie> pozitii = new List<Pozitie>();
            if(CuloarePiesa == CuloareJoc.Albastru)
            {
                int sfarsitLinie = joc.MarimeVerticala - 1;
                if (this.Pozitie.Linie != sfarsitLinie)
                {
                    if (joc.ArrayCadrane[this.Pozitie.Linie + 1, this.Pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                    {
                        if (joc.ArrayCadrane[this.Pozitie.Linie + 1, this.Pozitie.Coloana].PiesaCadran.CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(this.Pozitie.Linie + 1, this.Pozitie.Coloana));
                        }
                    }
                    else
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie + 1, this.Pozitie.Coloana));
                    }
                }
                if(this.Pozitie.Linie > joc.PragRau )
                {
                    int sfarsitColoana = joc.MarimeOrizontala - 1;
                    const int inceputColoana = 0;

                    if (this.Pozitie.Coloana != sfarsitColoana)
                    {
                        if (joc.ArrayCadrane[this.Pozitie.Linie, this.Pozitie.Coloana + 1].PiesaCadran != ConstantaTabla.PiesaNula)
                        {
                            if (joc.ArrayCadrane[this.Pozitie.Linie, this.Pozitie.Coloana + 1].PiesaCadran.CuloarePiesa != this.CuloarePiesa)
                            {
                                pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana + 1));
                            }
                        }
                        else
                        {
                            pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana + 1));
                        }

                    }
                    if (this.Pozitie.Coloana != inceputColoana)
                    {
                        if (joc.ArrayCadrane[this.Pozitie.Linie, this.Pozitie.Coloana - 1].PiesaCadran != ConstantaTabla.PiesaNula)
                        {
                            if (joc.ArrayCadrane[this.Pozitie.Linie, this.Pozitie.Coloana - 1].PiesaCadran.CuloarePiesa != this.CuloarePiesa)
                            {
                                pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                            }
                        }
                        else
                        {
                            pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                        }
                    }
                }
            }
            else if (CuloarePiesa == CuloareJoc.Alb)
            {
                int inceputLinie = 0;
                if (this.Pozitie.Linie != inceputLinie)
                {
                    if (joc.ArrayCadrane[this.Pozitie.Linie - 1, this.Pozitie.Coloana].PiesaCadran != ConstantaTabla.PiesaNula)
                    {
                        if (joc.ArrayCadrane[this.Pozitie.Linie - 1, this.Pozitie.Coloana].PiesaCadran.CuloarePiesa != this.CuloarePiesa)
                        {
                            pozitii.Add(new Pozitie(this.Pozitie.Linie - 1, this.Pozitie.Coloana));
                        }
                    }
                    else
                    {
                        pozitii.Add(new Pozitie(this.Pozitie.Linie - 1, this.Pozitie.Coloana));
                    }
                }
                if (this.Pozitie.Linie <= joc.PragRau)
                {
                    int sfarsitColoana = joc.MarimeOrizontala - 1;
                    const int inceputColoana = 0;
                    
                    if (this.Pozitie.Coloana != sfarsitColoana)
                    {
                        if (joc.ArrayCadrane[this.Pozitie.Linie, this.Pozitie.Coloana + 1].PiesaCadran != ConstantaTabla.PiesaNula)
                        {
                            if (joc.ArrayCadrane[this.Pozitie.Linie, this.Pozitie.Coloana + 1].PiesaCadran.CuloarePiesa != this.CuloarePiesa)
                            {
                                pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana + 1));
                            }
                        }
                        else
                        {
                            pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana + 1));
                        }
                    }
                    if (this.Pozitie.Coloana != inceputColoana)
                    {
                        if (joc.ArrayCadrane[this.Pozitie.Linie, this.Pozitie.Coloana - 1].PiesaCadran != ConstantaTabla.PiesaNula)
                        {
                            if (joc.ArrayCadrane[this.Pozitie.Linie, this.Pozitie.Coloana - 1].PiesaCadran.CuloarePiesa != this.CuloarePiesa)
                            {
                                pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                            }
                        }
                        else
                        {
                            pozitii.Add(new Pozitie(this.Pozitie.Linie, this.Pozitie.Coloana - 1));
                        }
                    }
                }
            }
            joc.ColoreazaMutariPosibile(pozitii);
        }
    }
}
