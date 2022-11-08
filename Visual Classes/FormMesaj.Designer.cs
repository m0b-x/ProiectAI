namespace ProiectVolovici.Visual_Classes
{
    partial class FormMesaj
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
            this.panelDocked = new System.Windows.Forms.Panel();
            this.pictureBoxEmote = new System.Windows.Forms.PictureBox();
            this.buttonOk = new System.Windows.Forms.Button();
            this.labelStareCastig = new System.Windows.Forms.Label();
            this.labelMotivCastig = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEmote)).BeginInit();
            this.SuspendLayout();
            // 
            // panelDocked
            // 
            this.panelDocked.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.panelDocked.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.panelDocked.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelDocked.Location = new System.Drawing.Point(0, 122);
            this.panelDocked.Name = "panelDocked";
            this.panelDocked.Size = new System.Drawing.Size(359, 13);
            this.panelDocked.TabIndex = 0;
            // 
            // pictureBoxEmote
            // 
            this.pictureBoxEmote.Image = global::ProiectVolovici.Properties.Resources.winemoji;
            this.pictureBoxEmote.Location = new System.Drawing.Point(12, 14);
            this.pictureBoxEmote.Name = "pictureBoxEmote";
            this.pictureBoxEmote.Size = new System.Drawing.Size(100, 97);
            this.pictureBoxEmote.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBoxEmote.TabIndex = 1;
            this.pictureBoxEmote.TabStop = false;
            // 
            // buttonOk
            // 
            this.buttonOk.AutoSize = true;
            this.buttonOk.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonOk.FlatAppearance.BorderSize = 0;
            this.buttonOk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonOk.Font = new System.Drawing.Font("Segoe UI Black", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.buttonOk.ForeColor = System.Drawing.SystemColors.Control;
            this.buttonOk.Location = new System.Drawing.Point(161, 81);
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.Size = new System.Drawing.Size(84, 30);
            this.buttonOk.TabIndex = 2;
            this.buttonOk.Text = "Continua";
            this.buttonOk.UseVisualStyleBackColor = false;
            this.buttonOk.Click += new System.EventHandler(this.buttonOk_Click);
            // 
            // labelStareCastig
            // 
            this.labelStareCastig.AutoSize = true;
            this.labelStareCastig.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelStareCastig.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelStareCastig.Location = new System.Drawing.Point(150, 15);
            this.labelStareCastig.Name = "labelStareCastig";
            this.labelStareCastig.Size = new System.Drawing.Size(109, 25);
            this.labelStareCastig.TabIndex = 3;
            this.labelStareCastig.Text = "Ai castigat!";
            // 
            // labelMotivCastig
            // 
            this.labelMotivCastig.AutoSize = true;
            this.labelMotivCastig.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.labelMotivCastig.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.labelMotivCastig.Location = new System.Drawing.Point(170, 49);
            this.labelMotivCastig.Name = "labelMotivCastig";
            this.labelMotivCastig.Size = new System.Drawing.Size(67, 21);
            this.labelMotivCastig.TabIndex = 4;
            this.labelMotivCastig.Text = "(motiv)";
            // 
            // FormMesaj
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(359, 135);
            this.ControlBox = false;
            this.Controls.Add(this.labelMotivCastig);
            this.Controls.Add(this.labelStareCastig);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.pictureBoxEmote);
            this.Controls.Add(this.panelDocked);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "FormMesaj";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEmote)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelDocked;
        private System.Windows.Forms.PictureBox pictureBoxEmote;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Label labelStareCastig;
        private System.Windows.Forms.Label labelMotivCastig;
    }
}