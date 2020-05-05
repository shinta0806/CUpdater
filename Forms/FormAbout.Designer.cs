namespace Updater
{
	partial class FormAbout
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAbout));
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.LabelAppName = new System.Windows.Forms.Label();
			this.LabelAppVer = new System.Windows.Forms.Label();
			this.LabelCopyright = new System.Windows.Forms.Label();
			this.GroupBoxAuthorInfo = new System.Windows.Forms.GroupBox();
			this.LinkLabelTwitter = new System.Windows.Forms.LinkLabel();
			this.label3 = new System.Windows.Forms.Label();
			this.LinkLabelWeb = new System.Windows.Forms.LinkLabel();
			this.label2 = new System.Windows.Forms.Label();
			this.LinkLabelEMail = new System.Windows.Forms.LinkLabel();
			this.label1 = new System.Windows.Forms.Label();
			this.GroupBoxApplication = new System.Windows.Forms.GroupBox();
			this.LinkLabelSupportWeb = new System.Windows.Forms.LinkLabel();
			this.label5 = new System.Windows.Forms.Label();
			this.LinkLabelDistWeb = new System.Windows.Forms.LinkLabel();
			this.label6 = new System.Windows.Forms.Label();
			this.ButtonHistory = new System.Windows.Forms.Button();
			this.ButtonOK = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.GroupBoxAuthorInfo.SuspendLayout();
			this.GroupBoxApplication.SuspendLayout();
			this.SuspendLayout();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::Updater.Properties.Resources.アイコン;
			this.pictureBox1.InitialImage = null;
			this.pictureBox1.Location = new System.Drawing.Point(16, 16);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(121, 105);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// LabelAppName
			// 
			this.LabelAppName.Font = new System.Drawing.Font("MS UI Gothic", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.LabelAppName.Location = new System.Drawing.Point(152, 16);
			this.LabelAppName.Name = "LabelAppName";
			this.LabelAppName.Size = new System.Drawing.Size(321, 29);
			this.LabelAppName.TabIndex = 0;
			this.LabelAppName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelAppVer
			// 
			this.LabelAppVer.Font = new System.Drawing.Font("MS UI Gothic", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.LabelAppVer.Location = new System.Drawing.Point(152, 59);
			this.LabelAppVer.Name = "LabelAppVer";
			this.LabelAppVer.Size = new System.Drawing.Size(321, 18);
			this.LabelAppVer.TabIndex = 1;
			this.LabelAppVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// LabelCopyright
			// 
			this.LabelCopyright.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
			this.LabelCopyright.Location = new System.Drawing.Point(152, 94);
			this.LabelCopyright.Name = "LabelCopyright";
			this.LabelCopyright.Size = new System.Drawing.Size(321, 18);
			this.LabelCopyright.TabIndex = 2;
			this.LabelCopyright.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			// 
			// GroupBoxAuthorInfo
			// 
			this.GroupBoxAuthorInfo.Controls.Add(this.LinkLabelTwitter);
			this.GroupBoxAuthorInfo.Controls.Add(this.label3);
			this.GroupBoxAuthorInfo.Controls.Add(this.LinkLabelWeb);
			this.GroupBoxAuthorInfo.Controls.Add(this.label2);
			this.GroupBoxAuthorInfo.Controls.Add(this.LinkLabelEMail);
			this.GroupBoxAuthorInfo.Controls.Add(this.label1);
			this.GroupBoxAuthorInfo.Location = new System.Drawing.Point(16, 144);
			this.GroupBoxAuthorInfo.Name = "GroupBoxAuthorInfo";
			this.GroupBoxAuthorInfo.Size = new System.Drawing.Size(457, 112);
			this.GroupBoxAuthorInfo.TabIndex = 3;
			this.GroupBoxAuthorInfo.TabStop = false;
			this.GroupBoxAuthorInfo.Text = "作者情報";
			// 
			// LinkLabelTwitter
			// 
			this.LinkLabelTwitter.Location = new System.Drawing.Point(102, 75);
			this.LinkLabelTwitter.Name = "LinkLabelTwitter";
			this.LinkLabelTwitter.Size = new System.Drawing.Size(336, 16);
			this.LinkLabelTwitter.TabIndex = 5;
			this.LinkLabelTwitter.TabStop = true;
			this.LinkLabelTwitter.Text = "https://twitter.com/shinta0806";
			this.LinkLabelTwitter.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.LinkLabelTwitter.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabels_LinkClicked);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 75);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(80, 16);
			this.label3.TabIndex = 4;
			this.label3.Text = "Twitter：";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LinkLabelWeb
			// 
			this.LinkLabelWeb.Location = new System.Drawing.Point(102, 49);
			this.LinkLabelWeb.Name = "LinkLabelWeb";
			this.LinkLabelWeb.Size = new System.Drawing.Size(336, 16);
			this.LinkLabelWeb.TabIndex = 3;
			this.LinkLabelWeb.TabStop = true;
			this.LinkLabelWeb.Text = "http://shinta.coresv.com/";
			this.LinkLabelWeb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.LinkLabelWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabels_LinkClicked);
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 49);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(80, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "ホームページ：";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LinkLabelEMail
			// 
			this.LinkLabelEMail.Location = new System.Drawing.Point(102, 24);
			this.LinkLabelEMail.Name = "LinkLabelEMail";
			this.LinkLabelEMail.Size = new System.Drawing.Size(336, 16);
			this.LinkLabelEMail.TabIndex = 1;
			this.LinkLabelEMail.TabStop = true;
			this.LinkLabelEMail.Text = "shinta.0806@gmail.com";
			this.LinkLabelEMail.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.LinkLabelEMail.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabels_LinkClicked);
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(80, 16);
			this.label1.TabIndex = 0;
			this.label1.Text = "E メール：";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// GroupBoxApplication
			// 
			this.GroupBoxApplication.Controls.Add(this.LinkLabelSupportWeb);
			this.GroupBoxApplication.Controls.Add(this.label5);
			this.GroupBoxApplication.Controls.Add(this.LinkLabelDistWeb);
			this.GroupBoxApplication.Controls.Add(this.label6);
			this.GroupBoxApplication.Location = new System.Drawing.Point(16, 272);
			this.GroupBoxApplication.Name = "GroupBoxApplication";
			this.GroupBoxApplication.Size = new System.Drawing.Size(457, 80);
			this.GroupBoxApplication.TabIndex = 4;
			this.GroupBoxApplication.TabStop = false;
			this.GroupBoxApplication.Text = "アプリケーション情報";
			// 
			// LinkLabelSupportWeb
			// 
			this.LinkLabelSupportWeb.AutoEllipsis = true;
			this.LinkLabelSupportWeb.Location = new System.Drawing.Point(102, 49);
			this.LinkLabelSupportWeb.Name = "LinkLabelSupportWeb";
			this.LinkLabelSupportWeb.Size = new System.Drawing.Size(336, 16);
			this.LinkLabelSupportWeb.TabIndex = 3;
			this.LinkLabelSupportWeb.TabStop = true;
			this.LinkLabelSupportWeb.Text = "http://shinta.coresv.com/software_dev/cupdater_jpn/#Support";
			this.LinkLabelSupportWeb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.LinkLabelSupportWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabels_LinkClicked);
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 49);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 2;
			this.label5.Text = "サポートページ：";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// LinkLabelDistWeb
			// 
			this.LinkLabelDistWeb.AutoEllipsis = true;
			this.LinkLabelDistWeb.Location = new System.Drawing.Point(102, 24);
			this.LinkLabelDistWeb.Name = "LinkLabelDistWeb";
			this.LinkLabelDistWeb.Size = new System.Drawing.Size(336, 16);
			this.LinkLabelDistWeb.TabIndex = 1;
			this.LinkLabelDistWeb.TabStop = true;
			this.LinkLabelDistWeb.Text = "http://shinta.coresv.com/software_dev/cupdater_jpn/";
			this.LinkLabelDistWeb.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.LinkLabelDistWeb.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabels_LinkClicked);
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(16, 24);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(80, 16);
			this.label6.TabIndex = 0;
			this.label6.Text = "配布ページ：";
			this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// ButtonHistory
			// 
			this.ButtonHistory.Location = new System.Drawing.Point(376, 368);
			this.ButtonHistory.Name = "ButtonHistory";
			this.ButtonHistory.Size = new System.Drawing.Size(96, 28);
			this.ButtonHistory.TabIndex = 6;
			this.ButtonHistory.Text = "改訂履歴 (&H)";
			this.ButtonHistory.UseVisualStyleBackColor = true;
			this.ButtonHistory.Click += new System.EventHandler(this.ButtonHistory_Click);
			// 
			// ButtonOK
			// 
			this.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.ButtonOK.Location = new System.Drawing.Point(168, 368);
			this.ButtonOK.Name = "ButtonOK";
			this.ButtonOK.Size = new System.Drawing.Size(192, 28);
			this.ButtonOK.TabIndex = 5;
			this.ButtonOK.Text = "OK";
			this.ButtonOK.UseVisualStyleBackColor = true;
			// 
			// FormAbout
			// 
			this.AcceptButton = this.ButtonOK;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(492, 412);
			this.Controls.Add(this.ButtonOK);
			this.Controls.Add(this.ButtonHistory);
			this.Controls.Add(this.GroupBoxApplication);
			this.Controls.Add(this.GroupBoxAuthorInfo);
			this.Controls.Add(this.LabelCopyright);
			this.Controls.Add(this.LabelAppVer);
			this.Controls.Add(this.LabelAppName);
			this.Controls.Add(this.pictureBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "FormAbout";
			this.Load += new System.EventHandler(this.FormAbout_Load);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.GroupBoxAuthorInfo.ResumeLayout(false);
			this.GroupBoxApplication.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.Label LabelAppName;
		private System.Windows.Forms.Label LabelAppVer;
		private System.Windows.Forms.Label LabelCopyright;
		private System.Windows.Forms.GroupBox GroupBoxAuthorInfo;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.LinkLabel LinkLabelTwitter;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.LinkLabel LinkLabelWeb;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.LinkLabel LinkLabelEMail;
		private System.Windows.Forms.GroupBox GroupBoxApplication;
		private System.Windows.Forms.LinkLabel LinkLabelSupportWeb;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.LinkLabel LinkLabelDistWeb;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button ButtonHistory;
		private System.Windows.Forms.Button ButtonOK;
	}
}