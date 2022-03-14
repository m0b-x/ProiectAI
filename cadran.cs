using System;
using System.Drawing;
using System.Windows.Forms;

namespace ProiectVolovici
{
    public partial class cadran : UserControl
    {
        private int linie { get; set; }
        private int coloana { get; set; }

        private int pragRau { get; set; } = 4;
        private int marimeCadran { get; set; } = 50;
        private int offsetCadran { get; set; } = 50;
        private int offsetRau { get; set; } = 25;


         private Color culoareCadranPar = Color.BlanchedAlmond;
         private Color culoareCadranImpar = Color.DarkGreen;
         private ImageLayout layoutCadran = ImageLayout.Center;
         private BorderStyle borderCadran = BorderStyle.FixedSingle; 

        public cadran()
        {
            InitializeComponent();
        }

        public cadran(Form parentForm, int linie, int coloana)
        {
            this.Parent = parentForm;
            if (linie > pragRau)
                this.Location = new Point(coloana * marimeCadran + offsetCadran, linie * marimeCadran + offsetCadran + offsetRau);
            else
                this.Location = new Point(coloana * marimeCadran + offsetCadran, linie * marimeCadran + offsetCadran);

            this.linie = coloana;
            this.coloana = linie;
            this.Size = new Size(marimeCadran, marimeCadran);
            this.BorderStyle = borderCadran;
            this.BackgroundImageLayout = layoutCadran;

            if (linie % 2 == 0)
            {
                if (coloana % 2 == 1)
                {
                    this.BackColor = culoareCadranImpar;
                }
                else
                {
                    this.BackColor = culoareCadranPar;
                }
            }
            else
            {
                if (coloana % 2 == 1)
                {
                    this.BackColor = culoareCadranPar;
                }
                else
                {
                    this.BackColor = culoareCadranImpar;
                }
            }
        }



        private void cadran_Load(object sender, EventArgs e)
        {

        }

    }
}
