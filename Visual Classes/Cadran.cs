using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class Cadran : UserControl
    {
        private int _linie;
        private int _coloana;

        private Piesa _piesaCadran;

        private int _marimeCadran = 50;
        private int _offsetCadran = 50;

        private static Color _culoareCadranPar = Color.BlanchedAlmond;
        private static Color _culoareCadranImpar = Color.DarkGreen;
        private static Color _culoareCadranSelectat = Color.DeepSkyBlue;
        private static Color _culoareCadranMutari = Color.DodgerBlue;

        private static ImageLayout _layoutCadran = ImageLayout.Center;
        private static BorderStyle _borderCadran = BorderStyle.FixedSingle; 
         

        public int Linie
        {
            get { return _linie; }
            set { _linie = value; }
        }
        public int Coloana
        {
            get { return _coloana; }
            set { _coloana = value; }
        }

        public Piesa PiesaCadran
        {
            get { return _piesaCadran; }
            set { _piesaCadran = value; }
        }
        public static Color CuloareCadranPar
        {
           get { return _culoareCadranPar; }
           set { _culoareCadranPar = value; }
        }
        
        public static Color CuloareCadranImpar
        {
            get { return _culoareCadranImpar; }
            set { _culoareCadranImpar = value; }
         }
        
        public static Color CuloareCadranSelectat
        {
            get { return _culoareCadranSelectat; }
            set { _culoareCadranSelectat = value; }
        }
        
        public static Color CuloareCadranMutari
        {
            get { return _culoareCadranMutari; }
            set { _culoareCadranMutari = value; }
        }
        public Cadran()
        {
            InitializeComponent();
        }

        public Cadran(Form parentForm,Tabla tabla, int linie, int coloana)
        {
            Parent = parentForm;
            if (linie > tabla.PragRau)
                Location = new Point(coloana * _marimeCadran + _offsetCadran, linie * _marimeCadran + _offsetCadran + tabla.OffsetRau);
            else
                Location = new Point(coloana * _marimeCadran + _offsetCadran, linie * _marimeCadran + _offsetCadran);

            _linie = coloana;
            _coloana = linie;
            Size = new Size(_marimeCadran, _marimeCadran);
            BorderStyle = _borderCadran;
            BackgroundImageLayout = _layoutCadran;
            _piesaCadran = null;

            if (linie % 2 == 0)
            {
                if (coloana % 2 == 1)
                {
                    BackColor = _culoareCadranImpar;
                }
                else
                {
                    BackColor = _culoareCadranPar;
                }
            }
            else
            {
                if (coloana % 2 == 1)
                {
                    BackColor = _culoareCadranPar;
                }
                else
                {
                    BackColor = _culoareCadranImpar;
                }
            }
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

        public void SetCadranBackground(int linie, int coloana, System.Drawing.Image imagine)
        {
           this.BackgroundImage = imagine;
        }

        public void AddEventHandler(EventHandler eventHandler)
        {
            this.Click += eventHandler;
        }

        private void cadran_Load(object sender, EventArgs e)
        {

        }

    }
}
