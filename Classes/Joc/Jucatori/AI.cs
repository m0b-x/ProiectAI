using System;
using System.Collections.Generic;

namespace ProiectVolovici
{
    public class AI : Jucator
    {
        EngineJocSinglePlayer _engine;

        List<Tuple<Tuple<Pozitie, Pozitie>, int[,]>> _matriciMutariPosibile = new();

        public AI(CuloareJoc culoare,EngineJocSinglePlayer _engine) : base(culoare)
        {
            _culoare = culoare;
        }
    }
}