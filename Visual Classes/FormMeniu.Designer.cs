namespace ProiectVolovici
{
    partial class FormMeniu
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMeniu));
            this.labelTitlu = new System.Windows.Forms.Label();
            this.butonOptiune3 = new System.Windows.Forms.Button();
            this.butonOptiune2 = new System.Windows.Forms.Button();
            this.butonOptiune1 = new System.Windows.Forms.Button();
            this.butonOptiune4 = new System.Windows.Forms.Button();
            this.textBoxIP = new System.Windows.Forms.TextBox();
            this.labelIP = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // labelTitlu
            // 
            this.labelTitlu.AutoSize = true;
            this.labelTitlu.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTitlu.Location = new System.Drawing.Point(28, 44);
            this.labelTitlu.Name = "labelTitlu";
            this.labelTitlu.Size = new System.Drawing.Size(143, 37);
            this.labelTitlu.TabIndex = 8;
            this.labelTitlu.Text = "Xiangqi";
            // 
            // butonOptiune3
            // 
            this.butonOptiune3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butonOptiune3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butonOptiune3.Location = new System.Drawing.Point(41, 184);
            this.butonOptiune3.Name = "butonOptiune3";
            this.butonOptiune3.Size = new System.Drawing.Size(104, 39);
            this.butonOptiune3.TabIndex = 7;
            this.butonOptiune3.Text = "Exit";
            this.butonOptiune3.UseVisualStyleBackColor = true;
            this.butonOptiune3.Click += new System.EventHandler(this.butonExit_Click);
            // 
            // butonOptiune2
            // 
            this.butonOptiune2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butonOptiune2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butonOptiune2.Location = new System.Drawing.Point(41, 139);
            this.butonOptiune2.Name = "butonOptiune2";
            this.butonOptiune2.Size = new System.Drawing.Size(105, 39);
            this.butonOptiune2.TabIndex = 6;
            this.butonOptiune2.Text = "MultiPlayer";
            this.butonOptiune2.UseVisualStyleBackColor = true;
            this.butonOptiune2.Click += new System.EventHandler(this.butonMultiPlayer_Click);
            // 
            // butonOptiune1
            // 
            this.butonOptiune1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butonOptiune1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butonOptiune1.Location = new System.Drawing.Point(42, 94);
            this.butonOptiune1.Name = "butonOptiune1";
            this.butonOptiune1.Size = new System.Drawing.Size(104, 39);
            this.butonOptiune1.TabIndex = 5;
            this.butonOptiune1.Text = "SinglePlayer";
            this.butonOptiune1.UseVisualStyleBackColor = true;
            this.butonOptiune1.Click += new System.EventHandler(this.butonSinglePlayer_Click);
            // 
            // butonOptiune4
            // 
            this.butonOptiune4.Enabled = false;
            this.butonOptiune4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butonOptiune4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butonOptiune4.Location = new System.Drawing.Point(40, 229);
            this.butonOptiune4.Name = "butonOptiune4";
            this.butonOptiune4.Size = new System.Drawing.Size(104, 39);
            this.butonOptiune4.TabIndex = 9;
            this.butonOptiune4.Text = "Inapoi";
            this.butonOptiune4.UseVisualStyleBackColor = true;
            this.butonOptiune4.Visible = false;
            // 
            // textBoxIP
            // 
            this.textBoxIP.Enabled = false;
            this.textBoxIP.Location = new System.Drawing.Point(42, 148);
            this.textBoxIP.Name = "textBoxIP";
            this.textBoxIP.Size = new System.Drawing.Size(104, 23);
            this.textBoxIP.TabIndex = 10;
            this.textBoxIP.Visible = false;
            // 
            // labelIP
            // 
            this.labelIP.AutoSize = true;
            this.labelIP.Enabled = false;
            this.labelIP.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelIP.Location = new System.Drawing.Point(8, 148);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(27, 19);
            this.labelIP.TabIndex = 11;
            this.labelIP.Text = "IP";
            this.labelIP.Visible = false;
            // 
            // FormMeniu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ProiectVolovici.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(442, 442);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.textBoxIP);
            this.Controls.Add(this.butonOptiune4);
            this.Controls.Add(this.labelTitlu);
            this.Controls.Add(this.butonOptiune3);
            this.Controls.Add(this.butonOptiune2);
            this.Controls.Add(this.butonOptiune1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMeniu";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.FormMeniu_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitlu;
        private System.Windows.Forms.Button butonOptiune3;
        private System.Windows.Forms.Button butonOptiune2;
        private System.Windows.Forms.Button butonOptiune1;
        private System.Windows.Forms.Button butonOptiune4;
        private System.Windows.Forms.TextBox textBoxIP;
        private System.Windows.Forms.Label labelIP;
    }
}