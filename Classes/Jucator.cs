using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici.Classes
{
    abstract class Jucator
    {
        private String _nume;


        public String Nume
        {
            get { return _nume; }
            set { _nume = value; }
        }

    }
}
