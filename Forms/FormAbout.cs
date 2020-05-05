// ============================================================================
// 
// バージョン情報フォーム
// 
// ============================================================================

// ----------------------------------------------------------------------------
// バージョン情報フォームを他のプロジェクトで使い回すやり方
// ・FormAbout.cs、FormAbout.Designer.cs、FormAbout.resx の 3 つを新プロジェクトのフォルダにコピー
// ・FormAbout.cs、FormAbout.Designer.cs の namespace を新プロジェクトのものに変更
// ・先頭の namespace 以外にも namespace 使っている箇所があるかもしれないので注意
//   （FormAbout.Designer.cs でアイコンを読み込むところは使っているので削除）
// ・既存の項目追加で、プロジェクトに 3 ファイルを追加するのだが、これがややこしい
//   FormAbout.cs 配下に FormAboutDesigner.cs、FormAbout.resx、FormAbout があるツリーになるようにする
//   resx を追加して、ダブルクリックでエディタ出してから、一旦プロジェクトから除外
//   次に Designer.cs を追加してから、一旦プロジェクトから除外
//   最後に FormAbout.cs を追加、でいけるかな？
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

using Updater.Misc;

namespace Updater
{
	public partial class FormAbout : Form
	{
		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public FormAbout(LogWriter logWriter)
		{
			InitializeComponent();

			mLogWriter = logWriter;
		}

		// ====================================================================
		// private 定数
		// ====================================================================

		private const String FILE_NAME_HISTORY = "Updater_History_JPN.txt";

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// ログ
		private LogWriter mLogWriter;

		// ====================================================================
		// private メンバー関数
		// ====================================================================

		// ====================================================================
		// private イベントハンドラ
		// ====================================================================

		private void ButtonHistory_Click(object sender, EventArgs e)
		{
			try
			{
				Process.Start(Path.GetDirectoryName(Application.ExecutablePath) + "\\" + FILE_NAME_HISTORY);
			}
			catch (Exception)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "改訂履歴を表示できませんでした。\n" + FILE_NAME_HISTORY);
			}
		}

		private void FormAbout_Load(object sender, EventArgs e)
		{
			// 表示
			Text = UpdaterConstants.APP_NAME_J + "のバージョン情報";
#if DEBUG
			Text = "［デバッグ］" + Text;
#endif
			LabelAppName.Text = UpdaterConstants.APP_NAME_J;
			LabelAppVer.Text = UpdaterConstants.APP_VER;
			LabelCopyright.Text = UpdaterConstants.COPYRIGHT_J;

			// コントロール
			ActiveControl = ButtonOK;
		}

		// --------------------------------------------------------------------
		// LinkLabel のクリックを集約
		// --------------------------------------------------------------------
		private void LinkLabels_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string aLink = String.Empty;

			try
			{
				// MSDN を見ると e.Link.LinkData がリンク先のように読めなくもないが、実際には
				// 値が入っていないので sender をキャストしてリンク先を取得する
				e.Link.Visited = true;
				aLink = ((LinkLabel)sender).Text;
				Process.Start(aLink);
			}
			catch (Exception)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "リンク先を表示できませんでした。\n" + aLink);
			}

		}





	}
}
