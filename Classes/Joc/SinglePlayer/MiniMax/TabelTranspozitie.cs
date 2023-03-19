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

    public IntrareTabel Lookup(long hash)
    {
        return _tabel[hash];
    }

    public void AdaugaIntrare(long hash, double score, int depth, int flag)
    {
        _tabel[hash] = new IntrareTabel(score, depth, flag);
    }
}
