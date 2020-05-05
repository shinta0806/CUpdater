// ============================================================================
// 
// メインフォーム
// 
// ============================================================================

// ----------------------------------------------------------------------------
// デバッグ用コマンドライン引数
// /id Test /Name "テスト" /UpdateRSS http://www2u.biglobe.ne.jp/~shinta/test/TestCUpdater_AutoUpdate.xml /CurrentVer "Ver 1.00 β" /Wait 2
// ----------------------------------------------------------------------------

using Shinta;

using System;
using System.Diagnostics;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

using Updater.Misc;

namespace Updater
{
	public partial class FormUpdater : Form
	{
		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public FormUpdater()
		{
			InitializeComponent();
		}

		// ====================================================================
		// protected メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// ウィンドウプロシージャー
		// --------------------------------------------------------------------
		protected override void WndProc(ref Message oMsg)
		{
			switch ((UpdaterCommand)oMsg.Msg)
			{
				case UpdaterCommand.CloseMainFormRequested:
					CloseMainFormRequested(Convert.ToBoolean((Int32)oMsg.WParam));
					break;
				case UpdaterCommand.ShowMainFormRequested:
					ShowMainFormRequested();
					break;
				case UpdaterCommand.WorkerThreadNotifyLogMessage:
					WorkerThreadNotifyLogMessage((Int32)oMsg.WParam);
					break;
				case UpdaterCommand.WorkerThreadNotifyProgress:
					WorkerThreadNotifyProgress((Int32)oMsg.WParam);
					break;
				case UpdaterCommand.WorkerThreadNotifySubCaption:
					WorkerThreadNotifySubCaption((Int32)oMsg.WParam);
					break;
				case UpdaterCommand.WorkerThreadStatusChangedCleaning:
					WorkerThreadStatusChangedCleaning();
					break;
				case UpdaterCommand.WorkerThreadStatusChangedInstalling:
					WorkerThreadStatusChangedInstalling();
					break;
				case UpdaterCommand.WorkerThreadStatusChangedWaiting:
					WorkerThreadStatusChangedWaiting();
					break;
			}

			// 自前で処理したときも base の呼出が必要なのかどうか不明なので一応呼んでおく
			base.WndProc(ref oMsg);
		}


		// ====================================================================
		// private 定数
		// ====================================================================

		private const String MUTEX_UPDATER_PREFIX = "SHINTA_Updater_Mutex_";

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// 多重起動防止用
		private Mutex mMutex;

		// ログ出力用
		private LogWriter mLogWriter;

		// 本来 UpdaterLauncher は起動用だが、ここでは引数管理用として使用
		private UpdaterLauncher mParams = new UpdaterLauncher();

		// 作業用スレッド
		WorkerThread mWorkerThread;

		// アプリケーション終了時タスク安全中断用
		private CancellationTokenSource mAppCancellationTokenSource = new CancellationTokenSource();

		// ====================================================================
		// private メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// コマンドライン引数の解析
		// --------------------------------------------------------------------
		private void AnalyzeParams()
		{
			Int32 aInt32;
			String[] aParams = Environment.GetCommandLineArgs();
			String aParam;
			String aOpt;

			for (Int32 i = 1; i < aParams.Length; i++)
			{
				// 本当は、パラメーターによってはインデックスをさらに 1 つ進める必要があるが、面倒くさいので進めない
				aParam = aParams[i];
				if (i == aParams.Length - 1)
				{
					aOpt = String.Empty;
				}
				else
				{
					aOpt = aParams[i + 1];
				}
				//mTraceSource.TraceEvent(TraceEventType.Verbose, 0, "AnalyzeParams() param[" + i.ToString() + "]: " + aParam + " = " + aOpt);

				// 共通オプション
				if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_ID, true) == 0)
				{
					mParams.ID = aOpt;
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_NAME, true) == 0)
				{
					mParams.Name = aOpt;
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_WAIT, true) == 0)
				{
					if (Int32.TryParse(aOpt, out aInt32))
					{
						mParams.Wait = aInt32;
					}
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_FORCE_SHOW, true) == 0)
				{
					mParams.ForceShow = true;
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_NOTIFY_HWND, true) == 0)
				{
					if (Int32.TryParse(aOpt, out aInt32))
					{
						mParams.NotifyHWnd = (IntPtr)aInt32;
					}
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_SELF_LAUNCH, true) == 0)
				{
					mParams.SelfLaunch = true;
				}
				// 共通オプション（オンリー系）
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_VERBOSE, true) == 0)
				{
					mParams.Verbose = true;
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_DELETE_OLD, true) == 0)
				{
					mParams.DeleteOld = true;
				}
				// 最新情報確認用オプション
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_LATEST_RSS, true) == 0)
				{
					mParams.LatestRss = aOpt;
				}
				// 更新（自動アップデート）用オプション
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_UPDATE_RSS, true) == 0)
				{
					mParams.UpdateRss = aOpt;
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_CURRENT_VER, true) == 0)
				{
					mParams.CurrentVer = aOpt;
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_PID, true) == 0)
				{
					if (Int32.TryParse(aOpt, out aInt32))
					{
						mParams.PID = aInt32;
					}
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_RELAUNCH, true) == 0)
				{
					mParams.Relaunch = aOpt;
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_CLEAR_UPDATE_CACHE, true) == 0)
				{
					mParams.ClearUpdateCache = true;
				}
				else if (String.Compare(aParam, UpdaterLauncher.PARAM_STR_FORCE_INSTALL, true) == 0)
				{
					mParams.ForceInstall = true;
				}
			}
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "AnalyzeParams() ID: " + mParams.ID);
		}

		// --------------------------------------------------------------------
		// スレッドを安全に削除
		// --------------------------------------------------------------------
		private void DeleteWorkerThread()
		{
			if (mWorkerThread != null)
			{
				mWorkerThread.WaitForTerminate();
				mWorkerThread = null;
			}
		}

		// --------------------------------------------------------------------
		// 各種初期化
		// --------------------------------------------------------------------
		private void Init()
		{
			// ログ初期化
			mLogWriter = new LogWriter(UpdaterConstants.APP_ID);
			mLogWriter.ApplicationQuitToken = mAppCancellationTokenSource.Token;
			mLogWriter.TextBoxDisplay = TextBoxLog;
			mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "起動しました：" + UpdaterConstants.APP_NAME_J + " "
					+ UpdaterConstants.APP_VER + " ====================");
#if DEBUG
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "デバッグモード：" + Common.DEBUG_ENABLED_MARK);
#endif
			mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "プロセス動作モード：" + (Environment.Is64BitProcess ? "64" : "32"));

			// その他
			AnalyzeParams();

#if DEBUG
			Text = "［デバッグ］" + Text;
#endif
		}

		// --------------------------------------------------------------------
		// ラベルとログ
		// --------------------------------------------------------------------
		private void SetCaptionMessage(String oMsg)
		{
			LabelCaption.Text = oMsg;
			mLogWriter.LogMessage(TraceEventType.Information, oMsg);
		}

		// ====================================================================
		// private イベントハンドラ
		// ====================================================================

		// --------------------------------------------------------------------
		// スレッドからのイベント：フォームを閉じる
		// --------------------------------------------------------------------
		private void CloseMainFormRequested(Boolean oResult)
		{
			if (oResult)
			{
				SetCaptionMessage("完了しました。");
				mLogWriter.LogMessage(TraceEventType.Information, "作業を完了しました。");
			}
			else
			{
				SetCaptionMessage("中止またはエラーです。");
				mLogWriter.LogMessage(TraceEventType.Error, "作業を中止またはエラーが発生しました。");
			}
			DeleteWorkerThread();
			Close();
		}

		// --------------------------------------------------------------------
		// スレッドからのイベント：フォームを表示する
		// --------------------------------------------------------------------
		private void ShowMainFormRequested()
		{
			Opacity = 100;
			ShowInTaskbar = true;

			// ShowInTaskbar をいじるとハンドルが変わるのでスレッドに教える
			// 保留：厳密には、ShowInTaskbar と MainFormHandle 更新はアトミックに行う必要があるが、
			// 最悪でも 1 つくらいメッセージを取りこぼすくらいだろうから、とりあえずこのままにしておく
			mWorkerThread.MainFormHandle = Handle;
		}

		// --------------------------------------------------------------------
		// スレッドからのイベント：ログに記録したメッセージを画面にも表示する
		// --------------------------------------------------------------------
		private void WorkerThreadNotifyLogMessage(Int32 oIndex)
		{
			// テキストボックスに表示するのみ（ログファイルへの記録はスレッド側で実施）
			TextBoxLog.AppendText(mWorkerThread.MessageString(oIndex) + "\r\n");
		}

		// --------------------------------------------------------------------
		// スレッドからのイベント：進捗率を表示する
		// --------------------------------------------------------------------
		private void WorkerThreadNotifyProgress(Int32 oPercent)
		{
			ProgressBarProgress.Value = oPercent;
		}

		// --------------------------------------------------------------------
		// スレッドからのイベント：サブキャプション（追加情報）を表示する
		// --------------------------------------------------------------------
		private void WorkerThreadNotifySubCaption(Int32 oIndex)
		{
			String aMsg = mWorkerThread.MessageString(oIndex);
			LabelSubCaption.Text = aMsg;
			mLogWriter.LogMessage(TraceEventType.Information, aMsg);
		}

		// --------------------------------------------------------------------
		// スレッドからのイベント：ステータス更新：整理中
		// --------------------------------------------------------------------
		private void WorkerThreadStatusChangedCleaning()
		{
			SetCaptionMessage("整理中...");
			LabelSubCaption.Text = String.Empty;
		}

		// --------------------------------------------------------------------
		// スレッドからのイベント：ステータス更新：インストール中
		// --------------------------------------------------------------------
		private void WorkerThreadStatusChangedInstalling()
		{
			SetCaptionMessage("インストール中...");
			LabelSubCaption.Text = String.Empty;
		}

		// --------------------------------------------------------------------
		// スレッドからのイベント：ステータス更新：更新対象アプリ終了待ち
		// --------------------------------------------------------------------
		private void WorkerThreadStatusChangedWaiting()
		{
			SetCaptionMessage("アプリケーションの終了を待っています...");
			LabelSubCaption.Text = "アプリケーションを終了させて下さい。終了後、自動的に更新が開始されます。";
			TextBoxLog.AppendText("［" + mLogWriter.TraceEventTypeToCaption(TraceEventType.Information) + "］アプリケーションの終了を待っています...\r\n");
		}

		// ====================================================================
		// IDE 生成イベントハンドラ
		// ====================================================================

		private void FormUpdater_Load(object sender, EventArgs e)
		{
			// メンバ変数の初期化
			Init();
#if DEBUGz
			MessageBox.Show("FormUpdater_Load()");
#endif
		}

		private void FormUpdater_Shown(object sender, EventArgs e)
		{
			Boolean aShowErrMsg = mParams.ForceShow;
			Boolean aResult;
			String aErrMsg = String.Empty;

			try
			{
				// 呼びだし元アプリが背面に行くのを防止できるように配慮
				if (mParams.NotifyHWnd != IntPtr.Zero)
				{
					WindowsApi.PostMessage(mParams.NotifyHWnd, UpdaterLauncher.WM_UPDATER_LAUNCHED, (IntPtr)0, (IntPtr)0);
				}

				if (!mParams.IsRequiredValid())
				{
					// ユーザーが間違って起動した可能性が高いので、ユーザーにメッセージを表示する
					aShowErrMsg = true;
					aErrMsg = "動作に必要なパラメーターが設定されていません。";
#if DEBUGz
					aErrMsg = "［デバッグ］ " + UpdaterConstants.APP_VER + "\n" + aErrMsg;
#endif
					throw new Exception(aErrMsg);
				}

				// オンリー系の動作（動作後に終了）
				if (mParams.DeleteOld)
				{
					// 未実装
					throw new Exception("パラメーターが不正です。");
				}
				else if (mParams.Verbose)
				{
					UpdaterCommon.NotifyDisplayedIfNeeded(mParams);
					using (FormAbout aAbout = new FormAbout(mLogWriter))
					{
						aAbout.ShowDialog();
					}
					throw new Exception(String.Empty);
				}

				// セルフ再起動
				// .NET Core アプリから起動された場合、自身が呼びだし元のファイルをロックしている状態になっていることがある
				// セルフ再起動することにより、呼び出し元と自身のアプリの関連性が切れ、ロックが解除されるようだ
				if (!mParams.SelfLaunch)
				{
					// 何らかのバグにより再起動を繰り返す事態になった場合に利用者がプロセスを殺す余地ができるように少し待機
					Thread.Sleep(1000);

					mParams.SelfLaunch = true;
					mParams.Launch(mParams.ForceShow);
					throw new Exception("セルフ再起動したため終了します。");
				}

				// 同じパスでの多重起動防止
				bool aCreated;
				Mutex aMutex = new Mutex(true, MUTEX_UPDATER_PREFIX + Application.ExecutablePath.Replace("\\", "/"), out aCreated);
#if DEBUG
				//String DB = MUTEX_UPDATER_PREFIX + Assembly.GetEntryAssembly().Location.Replace("\\", "/");
				String DB = MUTEX_UPDATER_PREFIX + Application.ExecutablePath.Replace("\\", "/");
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, DB);
#endif
				if (aCreated)
				{
					mMutex = aMutex;
				}
				else
				{
					throw new Exception("多重起動のため終了します。");
				}

				// オンリー系ではないので先に進む
				mWorkerThread = new WorkerThread(mParams, mLogWriter);
				mWorkerThread.MainFormHandle = Handle;
				mWorkerThread.Start();

				aResult = true;

			}
			catch (Exception oExcep)
			{
				if (!String.IsNullOrEmpty(oExcep.Message))
				{
					aErrMsg = oExcep.Message;
				}
				aResult = false;
			}

			if (!aResult)
			{
				if (!String.IsNullOrEmpty(aErrMsg))
				{
					if (aShowErrMsg)
					{
						UpdaterCommon.NotifyDisplayedIfNeeded(mParams);
						mLogWriter.ShowLogMessage(TraceEventType.Error, aErrMsg);
					}
					else
					{
						mLogWriter.LogMessage(TraceEventType.Error, aErrMsg);
					}
				}
				Close();
			}

			// 正常終了時はウィンドウを閉じない（スレッドに閉じてもらう）
		}

		private void FormUpdater_MouseDown(object sender, MouseEventArgs e)
		{
#if DEBUGz
			MessageBox.Show("closing: " + Handle.ToString());
			Common.PostMessage(Handle, 0x0010 /* WM_CLOSE */, (IntPtr)0, (IntPtr)0);	// 動作した
#endif
		}

		private void FormUpdater_FormClosed(object sender, FormClosedEventArgs e)
		{
			DeleteWorkerThread();

			//ミューテックスを解放する
			if (mMutex != null)
			{
				mMutex.ReleaseMutex();
			}

			mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "終了しました：" + UpdaterConstants.APP_NAME_J + " "
					+ UpdaterConstants.APP_VER + " --------------------");

		}
	}
}
