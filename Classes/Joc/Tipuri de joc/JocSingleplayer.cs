using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public class JocSingleplayer : EngineJoc
    {
        private Om _jucatorUman;
        private Masina _jucatorMasina;

        public Om JucatorOm
        {
            get { return _jucatorUman; }
        }

        public Masina JucatorMasina
        {
            get { return _jucatorMasina; }
        }

        public JocSingleplayer(Form parentForm,ref Tuple<Om,Masina> jucatori) : base(parentForm)
        {
            _jucatorUman = jucatori.Item1;
            _jucatorMasina = jucatori.Item2;
        }
        public JocSingleplayer(Form parentForm, int[,] matriceTabla, ref Tuple<Om, Masina> jucatori) : base(parentForm, matriceTabla)
        {
            _jucatorUman = jucatori.Item1;
            _jucatorMasina = jucatori.Item2;
        }

    }
}
