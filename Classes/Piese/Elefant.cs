﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    class Elefant : Piesa
    {
        public Elefant(CuloareJoc culoare)
        {
            this.CuloarePiesa = culoare;
            this.PusaPeTabla = false;
            this.Selectata = false;
            if (culoare == CuloareJoc.Albastru)
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
        public override void ArataMutariPosibile(EngineJoc joc) 
        {
            const int primaLinie = 0;
            const int primaColoana = 0;
            int ultimaLinie = ConstantaTabla.MarimeVerticala - 1;
            int ultimaColoana = ConstantaTabla.MarimeOrizontala - 1;

            List<Pozitie> mutariNefiltrate = new List<Pozitie>();
            List<Pozitie> mutariFiltruTabla = new List<Pozitie>();
            List<Pozitie> mutariFiltruFinal = new List<Pozitie>();

            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana + 2));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie + 2, _pozitiePiesa.Coloana - 2));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana + 2));
            mutariNefiltrate.Add(new Pozitie(_pozitiePiesa.Linie - 2, _pozitiePiesa.Coloana - 2));

            foreach (Pozitie pozitie in mutariNefiltrate)
            {
                if ( pozitie.Linie <= ultimaLinie     &&
                     pozitie.Linie >= primaLinie      &&
                     pozitie.Coloana <= ultimaColoana &&
                     pozitie.Coloana >= primaColoana  )
                {
                    if(_culoarePiesa == CuloareJoc.Alb)
                    {
                        if (pozitie.Linie > joc.PragRau)
                        {
                            if (joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                            {
                                mutariFiltruTabla.Add(pozitie);
                            }
                            else if(joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran.CuloarePiesa != CuloareJoc.Alb)
                            {
                                mutariFiltruTabla.Add(pozitie);
                            }
                        }
                    }
                    else
                    {
                        if (pozitie.Linie <= joc.PragRau)
                        {
                            if (joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                            {
                                mutariFiltruTabla.Add(pozitie);
                            }
                            else if (joc.ArrayCadrane[pozitie.Linie, pozitie.Coloana].PiesaCadran.CuloarePiesa != CuloareJoc.Albastru)
                            {
                                mutariFiltruTabla.Add(pozitie);
                            }
                        }
                    }
                }

            }
            foreach (Pozitie pozitie in mutariFiltruTabla)
            {
                Pozitie pozitieElementMijloc = new Pozitie(0,0);
                if (pozitie.Linie > _pozitiePiesa.Linie)
                {
                    pozitieElementMijloc.Linie = pozitie.Linie - 1;
                }
                else
                {
                    pozitieElementMijloc.Linie = _pozitiePiesa.Linie - 1;
                }
                if (pozitie.Coloana > _pozitiePiesa.Coloana)
                {
                    pozitieElementMijloc.Coloana = pozitie.Coloana - 1;
                }
                else
                {
                    pozitieElementMijloc.Coloana = _pozitiePiesa.Coloana - 1;
                }
                if (joc.ArrayCadrane[pozitieElementMijloc.Linie, pozitieElementMijloc.Coloana].PiesaCadran == ConstantaTabla.PiesaNula)
                {
                    mutariFiltruFinal.Add(pozitie);
                }

            }
            joc.ColoreazaMutariPosibile(pozitii: mutariFiltruFinal);
        }
    }
}
