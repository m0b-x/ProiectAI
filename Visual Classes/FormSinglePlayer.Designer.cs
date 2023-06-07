namespace ProiectVolovici
{
	partial class FormSinglePlayer
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSinglePlayer));
			buttonReverse = new System.Windows.Forms.Button();
			SuspendLayout();
			// 
			// buttonReverse
			// 
			buttonReverse.Location = new System.Drawing.Point(555, 566);
			buttonReverse.Name = "buttonReverse";
			buttonReverse.Size = new System.Drawing.Size(118, 23);
			buttonReverse.TabIndex = 0;
			buttonReverse.Text = "Inverseaza Mutarea";
			buttonReverse.UseVisualStyleBackColor = true;
			buttonReverse.Click += buttonReverse_Click;
			// 
			// FormSinglePlayer
			// 
			AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
			AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			ClientSize = new System.Drawing.Size(726, 706);
			Controls.Add(buttonReverse);
			Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			Name = "FormSinglePlayer";
			Text = "FormSinglePlayer";
			Load += FormSinglePlayer_Load;
			ResumeLayout(false);
		}

		#endregion

		private System.Windows.Forms.Button buttonReverse;
	}
}