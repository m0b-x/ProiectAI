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
            this.butonExit = new System.Windows.Forms.Button();
            this.butonMultiPlayer = new System.Windows.Forms.Button();
            this.butonSinglePlayer = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // labelTitlu
            // 
            this.labelTitlu.AutoSize = true;
            this.labelTitlu.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelTitlu.Location = new System.Drawing.Point(24, 31);
            this.labelTitlu.Name = "labelTitlu";
            this.labelTitlu.Size = new System.Drawing.Size(143, 37);
            this.labelTitlu.TabIndex = 8;
            this.labelTitlu.Text = "Xiangqi";
            // 
            // butonExit
            // 
            this.butonExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butonExit.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butonExit.Location = new System.Drawing.Point(41, 181);
            this.butonExit.Name = "butonExit";
            this.butonExit.Size = new System.Drawing.Size(104, 39);
            this.butonExit.TabIndex = 7;
            this.butonExit.Text = "Exit";
            this.butonExit.UseVisualStyleBackColor = true;
            this.butonExit.Click += new System.EventHandler(this.butonExit_Click);
            // 
            // butonMultiPlayer
            // 
            this.butonMultiPlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butonMultiPlayer.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butonMultiPlayer.Location = new System.Drawing.Point(40, 136);
            this.butonMultiPlayer.Name = "butonMultiPlayer";
            this.butonMultiPlayer.Size = new System.Drawing.Size(105, 39);
            this.butonMultiPlayer.TabIndex = 6;
            this.butonMultiPlayer.Text = "MultiPlayer";
            this.butonMultiPlayer.UseVisualStyleBackColor = true;
            this.butonMultiPlayer.Click += new System.EventHandler(this.butonMultiPlayer_Click);
            // 
            // butonSinglePlayer
            // 
            this.butonSinglePlayer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.butonSinglePlayer.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.butonSinglePlayer.Location = new System.Drawing.Point(41, 91);
            this.butonSinglePlayer.Name = "butonSinglePlayer";
            this.butonSinglePlayer.Size = new System.Drawing.Size(104, 39);
            this.butonSinglePlayer.TabIndex = 5;
            this.butonSinglePlayer.Text = "SinglePlayer";
            this.butonSinglePlayer.UseVisualStyleBackColor = true;
            this.butonSinglePlayer.Click += new System.EventHandler(this.butonSinglePlayer_Click);
            // 
            // FormMeniu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::ProiectVolovici.Properties.Resources.background;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(442, 442);
            this.Controls.Add(this.labelTitlu);
            this.Controls.Add(this.butonExit);
            this.Controls.Add(this.butonMultiPlayer);
            this.Controls.Add(this.butonSinglePlayer);
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
        private System.Windows.Forms.Button butonExit;
        private System.Windows.Forms.Button butonMultiPlayer;
        private System.Windows.Forms.Button butonSinglePlayer;
    }
}