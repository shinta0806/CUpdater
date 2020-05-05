namespace Updater
{
	partial class FormUpdater
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			this.LabelCaption = new System.Windows.Forms.Label();
			this.LabelSubCaption = new System.Windows.Forms.Label();
			this.ProgressBarProgress = new System.Windows.Forms.ProgressBar();
			this.TextBoxLog = new System.Windows.Forms.TextBox();
			this.SuspendLayout();
			// 
			// LabelCaption
			// 
			this.LabelCaption.Location = new System.Drawing.Point(16, 16);
			this.LabelCaption.Name = "LabelCaption";
			this.LabelCaption.Size = new System.Drawing.Size(585, 16);
			this.LabelCaption.TabIndex = 0;
			// 
			// LabelSubCaption
			// 
			this.LabelSubCaption.Location = new System.Drawing.Point(16, 40);
			this.LabelSubCaption.Name = "LabelSubCaption";
			this.LabelSubCaption.Size = new System.Drawing.Size(585, 16);
			this.LabelSubCaption.TabIndex = 1;
			// 
			// ProgressBarProgress
			// 
			this.ProgressBarProgress.Location = new System.Drawing.Point(16, 72);
			this.ProgressBarProgress.Maximum = 1000;
			this.ProgressBarProgress.Name = "ProgressBarProgress";
			this.ProgressBarProgress.Size = new System.Drawing.Size(584, 23);
			this.ProgressBarProgress.TabIndex = 2;
			// 
			// TextBoxLog
			// 
			this.TextBoxLog.BackColor = System.Drawing.SystemColors.Control;
			this.TextBoxLog.Location = new System.Drawing.Point(16, 112);
			this.TextBoxLog.Multiline = true;
			this.TextBoxLog.Name = "TextBoxLog";
			this.TextBoxLog.ReadOnly = true;
			this.TextBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.TextBoxLog.Size = new System.Drawing.Size(584, 136);
			this.TextBoxLog.TabIndex = 3;
			// 
			// FormUpdater
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(619, 262);
			this.ControlBox = false;
			this.Controls.Add(this.TextBoxLog);
			this.Controls.Add(this.ProgressBarProgress);
			this.Controls.Add(this.LabelSubCaption);
			this.Controls.Add(this.LabelCaption);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Name = "FormUpdater";
			this.Opacity = 0D;
			this.ShowInTaskbar = false;
			this.Text = "自動更新";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormUpdater_FormClosed);
			this.Load += new System.EventHandler(this.FormUpdater_Load);
			this.Shown += new System.EventHandler(this.FormUpdater_Shown);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FormUpdater_MouseDown);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label LabelCaption;
		private System.Windows.Forms.Label LabelSubCaption;
		private System.Windows.Forms.ProgressBar ProgressBarProgress;
		private System.Windows.Forms.TextBox TextBoxLog;
	}
}

