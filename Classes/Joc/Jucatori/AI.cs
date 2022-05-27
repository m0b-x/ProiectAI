using System;
using System.Collections.Generic;

namespace ProiectVolovici
{
    public class AI : Jucator
    {
        public AI(CuloareJoc culoare) : base(culoare)
        {
            _culoare = culoare;
        }
    }
}