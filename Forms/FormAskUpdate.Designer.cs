namespace Updater
{
	partial class FormAskUpdate
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAskUpdate));
			this.LabelAsk = new System.Windows.Forms.Label();
			this.ButtonYes = new System.Windows.Forms.Button();
			this.ButtonNo = new System.Windows.Forms.Button();
			this.ButtonLater = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// LabelAsk
			// 
			this.LabelAsk.Location = new System.Drawing.Point(16, 16);
			this.LabelAsk.Name = "LabelAsk";
			this.LabelAsk.Size = new System.Drawing.Size(288, 64);
			this.LabelAsk.TabIndex = 0;
			// 
			// ButtonYes
			// 
			this.ButtonYes.Location = new System.Drawing.Point(16, 96);
			this.ButtonYes.Name = "ButtonYes";
			this.ButtonYes.Size = new System.Drawing.Size(288, 28);
			this.ButtonYes.TabIndex = 1;
			this.ButtonYes.Text = "はい。今すぐ更新します。(&Y)";
			this.ButtonYes.UseVisualStyleBackColor = true;
			this.ButtonYes.Click += new System.EventHandler(this.ButtonYes_Click);
			// 
			// ButtonNo
			// 
			this.ButtonNo.Location = new System.Drawing.Point(16, 136);
			this.ButtonNo.Name = "ButtonNo";
			this.ButtonNo.Size = new System.Drawing.Size(288, 28);
			this.ButtonNo.TabIndex = 2;
			this.ButtonNo.Text = "いいえ。このバージョンはインストールしたくありません。(&N)";
			this.ButtonNo.UseVisualStyleBackColor = true;
			this.ButtonNo.Click += new System.EventHandler(this.ButtonNo_Click);
			// 
			// ButtonLater
			// 
			this.ButtonLater.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.ButtonLater.Location = new System.Drawing.Point(16, 192);
			this.ButtonLater.Name = "ButtonLater";
			this.ButtonLater.Size = new System.Drawing.Size(288, 28);
			this.ButtonLater.TabIndex = 3;
			this.ButtonLater.Text = "後にしたいので、再度確認して下さい。(&L)";
			this.ButtonLater.UseVisualStyleBackColor = true;
			this.ButtonLater.Click += new System.EventHandler(this.ButtonLater_Click);
			// 
			// FormAskUpdate
			// 
			this.AcceptButton = this.ButtonYes;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.ButtonLater;
			this.ClientSize = new System.Drawing.Size(318, 237);
			this.Controls.Add(this.ButtonLater);
			this.Controls.Add(this.ButtonNo);
			this.Controls.Add(this.ButtonYes);
			this.Controls.Add(this.LabelAsk);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAskUpdate";
			this.Load += new System.EventHandler(this.FormAskUpdate_Load);
			this.Shown += new System.EventHandler(this.FormAskUpdate_Shown);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label LabelAsk;
		private System.Windows.Forms.Button ButtonYes;
		private System.Windows.Forms.Button ButtonNo;
		private System.Windows.Forms.Button ButtonLater;
	}
}