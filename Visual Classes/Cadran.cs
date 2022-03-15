using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class Cadran : UserControl
    {
        private int _linie;
        private int _coloana;

        private int _marimeCadran = 50;
        private int _offsetCadran = 50;

        public static Color _culoareCadranPar = Color.BlanchedAlmond;
        public static Color _culoareCadranImpar = Color.DarkGreen;
        public static Color _culoareCadranSelectat = Color.DeepSkyBlue;
        public static Color _culoareCadranMutari = Color.DodgerBlue;
        public static ImageLayout _layoutCadran = ImageLayout.Center;
        public static BorderStyle _borderCadran = BorderStyle.FixedSingle; 
         
         /*
        public Color CuloareCadranPar
        {
           get { return _culoareCadranPar; }
           set { _culoareCadranPar = value; }
        }
        
        public Color CuloareCadranImpar
        {
            get { return _culoareCadranImpar; }
            set { _culoareCadranImpar = value; }
         }
        
        public Color CuloareCadranSelectat
        {
            get { return _culoareCadranSelectat; }
            set { _culoareCadranSelectat = value; }
        }
        
        public Color CuloareCadranMutari
        {
            get { return _culoareCadranMutari; }
            set { _culoareCadranMutari = value; }
        }
        */

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
