
using System;

namespace ProiectVolovici
{
    public abstract class AI : Jucator
    {
        protected AI(Culoare culoare) : base(culoare)
        {
        }
        public abstract Tuple<Mutare, double> ReturneazaMutareaOptima();

        public abstract bool IsThinking();
    }
}
