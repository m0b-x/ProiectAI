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
            labelTitlu = new System.Windows.Forms.Label();
            butonOptiune3 = new System.Windows.Forms.Button();
            butonOptiune2 = new System.Windows.Forms.Button();
            butonOptiune1 = new System.Windows.Forms.Button();
            butonOptiune4 = new System.Windows.Forms.Button();
            textBoxIP = new System.Windows.Forms.TextBox();
            labelIP = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // labelTitlu
            // 
            labelTitlu.AutoSize = true;
            labelTitlu.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            labelTitlu.Location = new System.Drawing.Point(8, 45);
            labelTitlu.Name = "labelTitlu";
            labelTitlu.Size = new System.Drawing.Size(197, 37);
            labelTitlu.TabIndex = 8;
            labelTitlu.Text = "Chū xuézhě";
            // 
            // butonOptiune3
            // 
            butonOptiune3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            butonOptiune3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            butonOptiune3.Location = new System.Drawing.Point(41, 184);
            butonOptiune3.Name = "butonOptiune3";
            butonOptiune3.Size = new System.Drawing.Size(104, 39);
            butonOptiune3.TabIndex = 7;
            butonOptiune3.Text = "Exit";
            butonOptiune3.UseVisualStyleBackColor = true;
            butonOptiune3.Click += butonExit_Click;
            // 
            // butonOptiune2
            // 
            butonOptiune2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            butonOptiune2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            butonOptiune2.Location = new System.Drawing.Point(41, 139);
            butonOptiune2.Name = "butonOptiune2";
            butonOptiune2.Size = new System.Drawing.Size(105, 39);
            butonOptiune2.TabIndex = 6;
            butonOptiune2.Text = "MultiPlayer";
            butonOptiune2.UseVisualStyleBackColor = true;
            butonOptiune2.Click += butonMultiPlayer_Click;
            // 
            // butonOptiune1
            // 
            butonOptiune1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            butonOptiune1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            butonOptiune1.Location = new System.Drawing.Point(42, 94);
            butonOptiune1.Name = "butonOptiune1";
            butonOptiune1.Size = new System.Drawing.Size(104, 39);
            butonOptiune1.TabIndex = 5;
            butonOptiune1.Text = "SinglePlayer";
            butonOptiune1.UseVisualStyleBackColor = true;
            butonOptiune1.Click += butonSinglePlayer_Click;
            // 
            // butonOptiune4
            // 
            butonOptiune4.Enabled = false;
            butonOptiune4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            butonOptiune4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            butonOptiune4.Location = new System.Drawing.Point(40, 229);
            butonOptiune4.Name = "butonOptiune4";
            butonOptiune4.Size = new System.Drawing.Size(104, 39);
            butonOptiune4.TabIndex = 9;
            butonOptiune4.Text = "Inapoi";
            butonOptiune4.UseVisualStyleBackColor = true;
            butonOptiune4.Visible = false;
            // 
            // textBoxIP
            // 
            textBoxIP.Enabled = false;
            textBoxIP.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            textBoxIP.Location = new System.Drawing.Point(42, 148);
            textBoxIP.Name = "textBoxIP";
            textBoxIP.Size = new System.Drawing.Size(104, 23);
            textBoxIP.TabIndex = 10;
            textBoxIP.Text = "127.0.0.1";
            textBoxIP.Visible = false;
            // 
            // labelIP
            // 
            labelIP.AutoSize = true;
            labelIP.Enabled = false;
            labelIP.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            labelIP.Location = new System.Drawing.Point(8, 148);
            labelIP.Name = "labelIP";
            labelIP.Size = new System.Drawing.Size(27, 19);
            labelIP.TabIndex = 11;
            labelIP.Text = "IP";
            labelIP.Visible = false;
            // 
            // FormMeniu
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackgroundImage = Properties.Resources.background;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(442, 442);
            Controls.Add(labelIP);
            Controls.Add(textBoxIP);
            Controls.Add(butonOptiune4);
            Controls.Add(labelTitlu);
            Controls.Add(butonOptiune3);
            Controls.Add(butonOptiune2);
            Controls.Add(butonOptiune1);
            DoubleBuffered = true;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MaximumSize = new System.Drawing.Size(458, 481);
            MinimizeBox = false;
            MinimumSize = new System.Drawing.Size(458, 481);
            Name = "FormMeniu";
            ShowIcon = false;
            Text = "Meniu Principal";
            Load += FormMeniu_Load;
            ResumeLayout(false);
            PerformLayout();
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