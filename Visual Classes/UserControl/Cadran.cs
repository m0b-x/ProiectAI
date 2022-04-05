using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class Cadran : UserControl
    {
        private Pozitie _pozitieCadran;

        private Piesa _piesaCadran;

        private int _marimeCadran = ConstantaCadran.MarimeCadran;
        private int _offsetCadran = ConstantaCadran.OffsetCadran;

        private static ImageLayout _layoutCadran = ConstantaCadran.LayoutCadran;
        private static BorderStyle _borderCadran = ConstantaCadran.BorderCadran;

        public Piesa PiesaCadran
        {
            get { return _piesaCadran; }
            set { _piesaCadran = value; }
        }

        public Pozitie PozitieCadran
        {
            get { return _pozitieCadran; }
            set { _pozitieCadran = value; }
        }

        public Cadran()
        {
            InitializeComponent();
        }

        public Cadran(EngineJoc joc, Pozitie pozitie, Color culoare)
        {
            Parent = joc.ParentForm;
            if (pozitie.Linie > joc.PragRau)
                Location = new Point(pozitie.Coloana * _marimeCadran + _offsetCadran, pozitie.Linie * _marimeCadran + _offsetCadran + joc.MarimeRau);
            else
                Location = new Point(pozitie.Coloana * _marimeCadran + _offsetCadran, pozitie.Linie * _marimeCadran + _offsetCadran);

            _pozitieCadran = pozitie;
            Size = new Size(_marimeCadran, _marimeCadran);
            BorderStyle = _borderCadran;
            BackColor = culoare;
            BackgroundImageLayout = _layoutCadran;
            _piesaCadran = ConstantaTabla.PiesaNula;
        }

        public void ArataPalatul()
        {
        }

        public bool CadranEsteGol()
        {
            if (_piesaCadran == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool EsteAdversar(CuloareJoc culoare)
        {
            if (this.CadranEsteGol())
            {
                return true;
            }
            if (this._piesaCadran.CuloarePiesa == culoare)
            {
                return false;
            }
            return true;
        }

        private void cadran_Load(object sender, EventArgs e)
        {
        }
    }
}