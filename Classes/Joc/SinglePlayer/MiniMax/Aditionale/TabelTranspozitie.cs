using ProiectVolovici;
using System;
using System.Collections.Generic;

class TabelTranspozitie
{
    Dictionary<long, IntrareTabel> _tabel;

    public TabelTranspozitie(int nrIntrariEstimate)
    {
        _tabel = new Dictionary<long, IntrareTabel>(nrIntrariEstimate);
    }

    public bool Contine(long hash)
    {
        return _tabel.ContainsKey(hash);
    }

    public IntrareTabel ReturneazaIntrarea(long hash)
    {
        return _tabel[hash];
    }

    public void AdaugaIntrare(long hash, double scor, int adancime, int flag)
    {
        if (_tabel.ContainsKey(hash))
        {
            var item = _tabel[hash];
            if (item.Adancime < adancime)
            {
                _tabel[hash] = new IntrareTabel(scor, adancime, flag);
            }
        }
        else
        {
            _tabel.Add(hash, new IntrareTabel(scor, adancime, flag));
        }
    }
}
