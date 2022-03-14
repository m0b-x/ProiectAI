using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class Cadran : UserControl
    {
        private int _linie;
        private int _coloana;

        private int _pragRau = 4;
        private int _marimeCadran = 50;
        private int _offsetCadran = 50;
        private int _offsetRau = 25;


         public static Color _culoareCadranPar = Color.BlanchedAlmond;
         public static Color _culoareCadranImpar = Color.DarkGreen;
         public static ImageLayout _layoutCadran = ImageLayout.Center;
         public static BorderStyle _borderCadran = BorderStyle.FixedSingle; 

        public Cadran()
        {
            InitializeComponent();
        }

        public Cadran(Form parentForm, int linie, int coloana)
        {
            this.Parent = parentForm;
            if (linie > _pragRau)
                this.Location = new Point(coloana * _marimeCadran + _offsetCadran, linie * _marimeCadran + _offsetCadran + _offsetRau);
            else
                this.Location = new Point(coloana * _marimeCadran + _offsetCadran, linie * _marimeCadran + _offsetCadran);

            this._linie = coloana;
            this._coloana = linie;
            this.Size = new Size(_marimeCadran, _marimeCadran);
            this.BorderStyle = _borderCadran;
            this.BackgroundImageLayout = _layoutCadran;

            if (linie % 2 == 0)
            {
                if (coloana % 2 == 1)
                {
                    this.BackColor = _culoareCadranImpar;
                }
                else
                {
                    this.BackColor = _culoareCadranPar;
                }
            }
            else
            {
                if (coloana % 2 == 1)
                {
                    this.BackColor = _culoareCadranPar;
                }
                else
                {
                    this.BackColor = _culoareCadranImpar;
                }
            }
        }



        private void cadran_Load(object sender, EventArgs e)
        {

        }

    }
}
