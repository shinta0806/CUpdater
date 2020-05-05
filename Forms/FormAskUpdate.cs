// ============================================================================
// 
// 更新版を適用するかどうかユーザーに確認するためのフォーム
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// 
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Updater
{
	public partial class FormAskUpdate : Form
	{
		// ====================================================================
		// public プロパティ
		// ====================================================================

		// コマンドライン引数
		public UpdaterLauncher Params { get; set; }

		// 表示名
		public String DisplayName { get; set; }

		// 見つかった更新版のバージョン
		public String NewVer { get; set; }

		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public FormAskUpdate()
		{
			InitializeComponent();
		}

		private void FormAskUpdate_Shown(object sender, EventArgs e)
		{
			// 表示設定
			Text = DisplayName + "の自動更新";
			LabelAsk.Text = DisplayName + "の更新版が公開されています。インストールしますか？\n"
			+ "現在のバージョン：" + Params.CurrentVer + "\n"
			+ "新しいバージョン：" + NewVer;
			//Activate();
		}

		private void ButtonYes_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Yes;
		}

		private void ButtonNo_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show(NewVer + " が自動インストールされなくなりますが、よろしいですか？", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) 
					!= DialogResult.Yes)
			{
				return;
			}

			DialogResult = DialogResult.No;
		}

		private void ButtonLater_Click(object sender, EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
		}

		private void FormAskUpdate_Load(object sender, EventArgs e)
		{

		}
	}
}
