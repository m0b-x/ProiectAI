using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProiectVolovici
{
    public class TabelTranspozitie
    {
        private Dictionary<long, IntrareTabelTranspozitie> _tabel = new();

        public Dictionary<long, IntrareTabelTranspozitie> Tabel
        {
            get { return _tabel; }
        }

        public TabelTranspozitie(Dictionary<long, IntrareTabelTranspozitie> tabel)
        {
            _tabel = tabel;
        }
        public TabelTranspozitie()
        {
        }
        public void AdaugaIntrare(long cheie, IntrareTabelTranspozitie intrare)
        {
                _tabel.TryAdd(cheie, intrare);
        }

        public void ReseteazaTabel()
        {
            _tabel = new();
        }
    }
}
