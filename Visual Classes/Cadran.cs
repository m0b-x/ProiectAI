using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class Cadran : UserControl
    {
        Pozitie _pozitieCadran;

        private Piesa _piesaCadran;

        private int _marimeCadran = 50;
        private int _offsetCadran = 50;

        private static ImageLayout _layoutCadran = ImageLayout.Center;
        private static BorderStyle _borderCadran = BorderStyle.FixedSingle;

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

        public Cadran(Tabla tabla, Pozitie pozitie,Color culoare)
        {
            Parent = tabla.ParentForm;
            if (pozitie.Linie > tabla.PragRau)
                Location = new Point(pozitie.Coloana * _marimeCadran + _offsetCadran, pozitie.Linie * _marimeCadran + _offsetCadran + tabla.MarimeRau);
            else
                Location = new Point(pozitie.Coloana * _marimeCadran + _offsetCadran, pozitie.Linie * _marimeCadran + _offsetCadran);

            this._pozitieCadran = pozitie;
            Size = new Size(_marimeCadran, _marimeCadran);
            BorderStyle = _borderCadran;
            BackColor = culoare;
            BackgroundImageLayout = _layoutCadran;
            _piesaCadran = null;

        }
        public void setPiesa(Piesa piesa)
        {
            if (piesa != null)
            {
                _piesaCadran = piesa;
                BackgroundImage = piesa.Imagine;
            }
            else
            {
                _piesaCadran = null;
                BackgroundImage = null;
            }
        }

        public void SetCadranBackground(System.Drawing.Image imagine)
        {
           this.BackgroundImage = imagine;
        }

        public void AddClickEventHandler(EventHandler eventHandler)
        {
            this.Click += eventHandler;
        }

        public void AddDoubleClickEventHandler(EventHandler eventHandler)
        {
            this.DoubleClick += eventHandler;
        }

        private void cadran_Load(object sender, EventArgs e)
        {
        }

    }
}
