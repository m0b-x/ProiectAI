using System;
using System.Collections.Generic;

class TabelTranspozitie
{
    Dictionary<long, Tuple<double, int, int>> table;

    public TabelTranspozitie(int nrIntrariEstimate)
    {
        table = new Dictionary<long, Tuple<double, int, int>>(nrIntrariEstimate);
    }

    public bool Contine(long hash)
    {
        return table.ContainsKey(hash);
    }

    public Tuple<double, int, int> Lookup(long hash)
    {
        return table[hash];
    }

    public void AdaugaIntrare(long hash, double score, int depth, int flag)
    {
        table[hash] = new Tuple<double, int, int>(score, depth, flag);
    }
}
