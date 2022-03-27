using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class Masina : Jucator
    {
        public Masina(CuloareJoc culoare) : base(culoare)
        {
            _culoare = culoare;
        }
    }
}
