using ProiectVolovici;
using System;
using System.Collections.Generic;

class TabelTranspozitie
{
    Dictionary<ulong, IntrareTabel> _tabel;

    public TabelTranspozitie(int nrIntrariEstimate)
    {
        _tabel = new Dictionary<ulong, IntrareTabel>(nrIntrariEstimate);
    }

    public bool Contine(ulong hash)
    {
        return _tabel.ContainsKey(hash);
    }

    public IntrareTabel ReturneazaIntrarea(ulong hash)
    {
        return _tabel[hash];
    }

    public void AdaugaIntrare(ulong hash, double scor, int adancime, int flag)
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
