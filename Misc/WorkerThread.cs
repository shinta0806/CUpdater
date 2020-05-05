// ============================================================================
// 
// 最新情報の確認、更新版のダウンロード・インストールを実行する
// 
// ============================================================================

// ----------------------------------------------------------------------------
// ToDo: _Common 配下の WorkerThread.cs を継承する
// 
// ----------------------------------------------------------------------------

using Shinta;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Threading;
using System.Windows.Forms;

namespace Updater.Misc
{
	public class WorkerThread
	{
		// ====================================================================
		// public プロパティ
		// ====================================================================

		// メインフォームハンドル
		public IntPtr MainFormHandle { get; set; }

#if false
		// 終了要求制御
		public CancellationToken CancellationToken { get; set; }
#endif

#if false
		// スレッド中止制御
		public Boolean TerminateRequested { get; set; }
#endif

		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public WorkerThread(UpdaterLauncher launcherParams, LogWriter logWriter)
		{
			mParams = launcherParams;
			mLogWriter = logWriter;
			mCancellationTokenSource = new CancellationTokenSource();

			// 表示名の設定
			mDisplayName = "「" + (String.IsNullOrEmpty(mParams.Name) ? mParams.ID : mParams.Name) + "」";
		}

		// --------------------------------------------------------------------
		// 送信したメッセージ
		// --------------------------------------------------------------------
		public String MessageString(Int32 oIndex)
		{
			String aResult;
			lock (mMessageStrings)
			{
				aResult = mMessageStrings[oIndex];
			}
			return aResult;
		}

		// --------------------------------------------------------------------
		// スレッドを作成して実行
		// --------------------------------------------------------------------
		public void Start()
		{
			mThread = new Thread(new ThreadStart(ThreadMethod));
			mThread.Start();
		}

		// --------------------------------------------------------------------
		// スレッドの終了を待つ
		// --------------------------------------------------------------------
		public void WaitForTerminate()
		{
			mCancellationTokenSource.Cancel();
			mThread.Join();
		}

		// ====================================================================
		// private 定数
		// ====================================================================

		// 設定保存ファイルのサフィックス
		private const String FILE_NAME_DUMMY_SUFFIX = "_Dummy";
		private const String FILE_NAME_LATEST_SUFFIX = "_Latest";
		private const String FILE_NAME_UPDATE_SUFFIX = "_Update";

		// ファイル名
		private const String FILE_NAME_DOWNLOAD_ZIP = "Download.zip";

		// フォルダ名
		private const String FOLDER_NAME_NEW_ARCHIVE = "NewArchive\\";
		private const String FOLDER_NAME_NEW_EXTRACT = "NewExtract\\";
		private const String FOLDER_NAME_OLD = "Old\\";
		private const String FOLDER_NAME_UPDATE = "Update\\";


		// 標準のユーザーエージェントに付加するちょちょいと自動更新独自のユーザーエージェント
		private const String USER_AGENT_PART = " CUpdater/";

		// 自動更新用ファイルをダウンロードする回数（プログラムや RSS の不具合で永遠にダウンロードするのを防ぐ）
		private const Int32 DOWNLOAD_TRY_MAX = 5;

		// MainFormHandle が変更されるのを待つ回数の最大値（万が一変わらなかった場合に無限ループになるのを防ぐ）
		private const Int32 WAIT_MAIN_FORM_HANDLE_CHANGE_MAX = 10;

		// ====================================================================
		// private メンバー変数
		// ====================================================================

		// コマンドライン引数
		private UpdaterLauncher mParams;

		// 表示名
		private String mDisplayName;

		// 最新情報
		private List<RssItem> mNewItems = new List<RssItem>();

		// 更新情報
		private List<RssItem> mUpdateItems = new List<RssItem>();

		// 更新制御情報
		private AutoUpdateStates mAutoUpdateStates = new AutoUpdateStates();

		// メインフォームに届けたい文字列（要排他制御）
		private List<String> mMessageStrings = new List<String>();

		// 実行するスレッド
		private Thread mThread;

		// 終了要求制御
		CancellationTokenSource mCancellationTokenSource;

		// ログ
		private LogWriter mLogWriter;

		// ====================================================================
		// private メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// 最新情報の確認
		// --------------------------------------------------------------------
		private void AnalyzeUpdateRss()
		{
			// アップデートファイル情報のバージョンを揃える（不揃いのを切り捨てる）
			Int32 aIndex = 1;
			while (aIndex < mUpdateItems.Count)
			{
				if (mUpdateItems[aIndex].Elements[RssManager.NODE_NAME_TITLE] != mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE])
				{
					mUpdateItems.RemoveAt(aIndex);
					mLogWriter.ShowLogMessage(TraceEventType.Verbose, "AnalyzeRSS() バージョン不揃い: " + aIndex.ToString());
				}
				else
				{
					aIndex++;
				}
			}

			// 更新が必要かどうかの判定
			// 強制インストールの場合は常に必要判定
			if (mParams.ForceInstall)
			{
				return;
			}

			// RSS に記載のバージョンが現在のバージョン以下なら不要
			if (StringUtils.StrAndNumCmp(mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE], mParams.CurrentVer, true) <= 0)
			{
				throw new Exception("更新の必要はありません：更新版がありません（現行：" + mParams.CurrentVer
						+ "、最新版：" + mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE] + "）");
			}

			// RSS に記載のバージョンが SkipVer 以下なら不要
			if (StringUtils.StrAndNumCmp(mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE], mAutoUpdateStates.SkipVer, true) <= 0)
			{
				throw new Exception("更新の必要はありません：ユーザーに不要と指定されたバージョンです（現行："
						+ mParams.CurrentVer + "、不要版：" + mAutoUpdateStates.SkipVer + "）");
			}
		}

		// --------------------------------------------------------------------
		// 最新情報の確認
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void AskDisplayLatest()
		{
			UpdaterCommon.NotifyDisplayedIfNeeded(mParams);
			if (MessageBox.Show(mDisplayName + "の最新情報が " + mNewItems.Count.ToString() + " 件見つかりました。\n表示しますか？",
					"質問", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
			{
				throw new Exception("最新情報の表示を中止しました。");
			}
		}

		// --------------------------------------------------------------------
		// 更新するかユーザーに尋ねる
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void AskUpdate()
		{
			UpdaterCommon.NotifyDisplayedIfNeeded(mParams);
			using (FormAskUpdate aAsk = new FormAskUpdate())
			{
				aAsk.Params = mParams;
				aAsk.DisplayName = mDisplayName;
				aAsk.NewVer = mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE];
				switch (aAsk.ShowDialog())
				{
					case DialogResult.Yes:
						ShowInstallMessage();
						break;
					case DialogResult.No:
						mAutoUpdateStates.SkipVer = mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE];
						mAutoUpdateStates.Save();
						throw new Exception("更新版（" + mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE] + "）はインストールしません。");
					default:
						throw new Exception("更新版（" + mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE] + "）は後でインストールします。");
				}
			}
		}

		// --------------------------------------------------------------------
		// 最新情報の確認
		// --------------------------------------------------------------------
		private Boolean CheckLatestInfo(out String oErr)
		{
			Boolean aResult = true;
			oErr = String.Empty;

			try
			{
				PrepareLatest();
				AskDisplayLatest();
				DisplayLatest();

				// 最新情報を正しく表示できたら、それがメッセージ代わりなので、別途のメッセージ表示はしない
			}
			catch (Exception oExcep)
			{
				oErr = "【最新情報の確認】\n" + oExcep.Message;
				aResult = false;
			}

			return aResult;
		}

		// --------------------------------------------------------------------
		// 自動更新の確認
		// --------------------------------------------------------------------
		private Boolean CheckUpdate(out String oErr)
		{
			Boolean aResult = false;
			String aOKMessage = String.Empty;
			oErr = String.Empty;

			try
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "CheckUpdate() relaunch path: " + mParams.Relaunch);
				PrepareUpdate();

				if (mParams.ForceInstall)
				{
					ShowInstallMessage();
				}
				else
				{
					AskUpdate();
				}
				mParams.ForceShow = true;
				IntPtr aOldMainFormHandle = MainFormHandle;
				PostCommand(UpdaterCommand.ShowMainFormRequested);

				// ShowMainFormRequested により MainFormHandle が更新されるはずなので、それを待つ
				for (Int32 i = 0; i < WAIT_MAIN_FORM_HANDLE_CHANGE_MAX; i++)
				{
					if (MainFormHandle != aOldMainFormHandle)
					{
						mLogWriter.ShowLogMessage(TraceEventType.Verbose, "CheckUpdate() #" + i.ToString() + " で脱出");
						break;
					}
					Thread.Sleep(Common.GENERAL_SLEEP_TIME);
				}

				WaitTargetExit();
				InstallUpdate();

				aOKMessage = "更新版のインストールが完了しました。";
				if (!String.IsNullOrEmpty(mParams.Relaunch))
				{
					aOKMessage += "\n" + mDisplayName + "を再起動します。";
				}
				LogAndSendAndShowMessage(TraceEventType.Information, aOKMessage, true);
				aResult = true;
			}
			catch (Exception oExcep)
			{
				oErr = "【更新版の確認】\n" + oExcep.Message;
			}

			// 再起動
			if (aResult && !String.IsNullOrEmpty(mParams.Relaunch))
			{
				try
				{
					Process.Start(mParams.Relaunch);
				}
				catch
				{
					LogAndSendAndShowMessage(TraceEventType.Error, mDisplayName + "を再起動できませんでした。", true);
					oErr = mDisplayName + "を再起動できませんでした。";
					aResult = false;
				}
			}
			return aResult;
		}

		// --------------------------------------------------------------------
		// 最新情報の確認
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void DisplayLatest()
		{
			Int32 aNumErrors = 0;

			foreach (RssItem aNewItem in mNewItems)
			{
				try
				{
					Process.Start(aNewItem.Elements[RssManager.NODE_NAME_LINK]);
				}
				catch
				{
					// エラーでもとりあえずは続行
					aNumErrors++;
				}
			}
			if (aNumErrors == 0)
			{
				// 正常終了
				mLogWriter.LogMessage(TraceEventType.Information, mNewItems.Count.ToString() + " 件の最新情報を表示完了。");
			}
			else if (aNumErrors < mNewItems.Count)
			{
				throw new Exception("一部の最新情報を表示できませんでした。");
			}
			else
			{
				throw new Exception("最新情報を全く表示できませんでした。");
			}
		}

		// --------------------------------------------------------------------
		// 更新版をダウンロード
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void DownloadUpdateArchive()
		{
			LogAndSendAndShowMessage(TraceEventType.Information, "更新版をダウンロードします。", false);

			// 試行回数チェック
			if (mAutoUpdateStates.DownloadVer == mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE]
				&& mAutoUpdateStates.DownloadTry >= DOWNLOAD_TRY_MAX
				&& !mParams.ForceInstall)
			{
				throw new Exception("ダウンロード回数超過のため自動更新を中止しました。");
			}

			// 試行情報更新
			if (mAutoUpdateStates.DownloadVer == mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE])
			{
				mAutoUpdateStates.DownloadTry++;
			}
			else
			{
				mAutoUpdateStates.DownloadVer = mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE];
				mAutoUpdateStates.DownloadTry = 1;
			}
			mAutoUpdateStates.DownloadMD5 = mUpdateItems[0].Elements[RssManager.NODE_NAME_LINK + RssItem.RSS_ITEM_NAME_DELIMITER + RssManager.ATTRIBUTE_NAME_MD5];
			mAutoUpdateStates.Save();

			// ダウンロード保存用フォルダの初期化
			try
			{
				Directory.Delete(Path.GetDirectoryName(UpdateArchivePath()), true);
			}
			catch
			{

			}
			// 念のための（自分自身との）アクセス競合回避
			Thread.Sleep(Common.GENERAL_SLEEP_TIME);
			// 作成
			Directory.CreateDirectory(Path.GetDirectoryName(UpdateArchivePath()));

			// ミラー選択
			Int32 aMirrorIndex = new Random().Next(mUpdateItems.Count);
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "DownloadUpdate() mirror index: " + aMirrorIndex.ToString());
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "DownloadUpdate() URL: " + mUpdateItems[aMirrorIndex].Elements[RssManager.NODE_NAME_LINK]);

			// ダウンロード
			Downloader aDownloader = new Downloader();
			aDownloader.CancellationToken = mCancellationTokenSource.Token;
			aDownloader.UserAgent += USER_AGENT_PART + UpdaterConstants.APP_VER;
			using (FileStream aFS = new FileStream(UpdateArchivePath(), FileMode.Create, FileAccess.ReadWrite))
			{
				aDownloader.Download(mUpdateItems[aMirrorIndex].Elements[RssManager.NODE_NAME_LINK], aFS);
			}
			if (!IsUpdateArchiveMD5Valid())
			{
				throw new Exception("正常にダウンロードが完了しませんでした（内容にエラーがあります）。");
			}
			LogAndSendAndShowMessage(TraceEventType.Information, "更新版のダウンロードが完了しました。", false);
		}

		// --------------------------------------------------------------------
		// Download.zip 展開後のルートパス
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private String ExtractBasePath()
		{
			String[] aFiles = Directory.GetFiles(ExtractPath(), "*");
			String[] aFolders = Directory.GetDirectories(ExtractPath(), "*");
			if (aFiles.Length == 0 && aFolders.Length == 0)
			{
				throw new Exception("ダウンロードした更新版の内容が空です。");
			}

			// アーカイブの中身（直下）が ID 名で始まるフォルダのみであれば、そのフォルダがルートパス
			if (aFiles.Length == 0 && aFolders.Length == 1
					&& String.Compare(Path.GetFileName(aFolders[0]), 0, mParams.ID, 0, mParams.ID.Length, true) == 0)
			{
				return aFolders[0] + "\\";
			}

			// それ以外なら、ExtractPath() 自体がルートパス
			return ExtractPath();
		}

		// --------------------------------------------------------------------
		// Download.zip をインストールする
		// --------------------------------------------------------------------
		private String ExtractPath()
		{
			return Path.GetDirectoryName(Application.ExecutablePath) + "\\" + FOLDER_NAME_UPDATE + FOLDER_NAME_NEW_EXTRACT;
		}

		// --------------------------------------------------------------------
		// ファイル 1 つをインストールする
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void InstallMove(String targetFile, String extractBasePath)
		{
			String aMiddleName = targetFile.Substring(extractBasePath.Length);
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "InstallUpdate() aMiddleName: " + aMiddleName);
			PostCommand(UpdaterCommand.WorkerThreadNotifySubCaption, aMiddleName);
			String aDestFile = Path.GetDirectoryName(Application.ExecutablePath) + "\\" + aMiddleName;
			Directory.CreateDirectory(Path.GetDirectoryName(aDestFile));
			try
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "InstallUpdate() deleting aDestFile: " + aDestFile);
				File.Delete(aDestFile);
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "InstallUpdate() moving: " + targetFile);
				File.Move(targetFile, aDestFile);
			}
			catch (Exception excep)
			{
				mLogWriter.ShowLogMessage(Common.TRACE_EVENT_TYPE_STATUS, "　スタックトレース：\n" + excep.StackTrace);
				throw new Exception("ファイルのインストールができませんでした：\n" + aMiddleName + "\nファイルが実行中または使用中でないか確認して下さい。");
			}

		}

		// --------------------------------------------------------------------
		// Download.zip をインストールする
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void InstallUpdate()
		{
			PostCommand(UpdaterCommand.WorkerThreadStatusChangedInstalling);

			// 現在のファイル群をバックアップするフォルダ
			try
			{
				Directory.Delete(OldPath(), true);
			}
			catch
			{

			}
			// ロックに対する安全マージン
			Thread.Sleep(Common.GENERAL_SLEEP_TIME);
			Directory.CreateDirectory(OldPath());

			// アーカイブ展開
			try
			{
				Directory.Delete(ExtractPath(), true);
			}
			catch
			{

			}
			// C++Builder 時代は zip 展開時にアクセス違反になることがあったので、スリープしてみていた。今は不要かも？
			Thread.Sleep(Common.GENERAL_SLEEP_TIME);
			Directory.CreateDirectory(ExtractPath());
			try
			{
				ZipFile.ExtractToDirectory(UpdateArchivePath(), ExtractPath());
			}
			catch (Exception oExcep)
			{
				throw new Exception("ダウンロードしたアーカイブを解凍できませんでした：" + oExcep.Message);
			}
			Thread.Sleep(Common.GENERAL_SLEEP_TIME);

			// 展開後のベースフォルダと全ファイル取得
			String aExtractBasePath = ExtractBasePath();
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "InstallUpdate() extract base folder: " + aExtractBasePath);
			String[] aExtractFiles = Directory.GetFiles(aExtractBasePath, "*", SearchOption.AllDirectories);

			// アーカイブを移動
			String self = null;
			Int32 aCount = 0;
			foreach (String aFile in aExtractFiles)
			{
				if (String.Compare(Path.GetFileName(aFile), Path.GetFileName(Application.ExecutablePath), true) == 0)
				{
					self = aFile;
					mLogWriter.ShowLogMessage(TraceEventType.Verbose, "InstallUpdate() セルフスキップ");
				}
				else
				{
					InstallMove(aFile, aExtractBasePath);
					aCount++;
				}
				PostCommand(UpdaterCommand.WorkerThreadNotifyProgress, aCount * 1000 / aExtractFiles.Length);
			}

			// 自分自身が上書きされる場合は Old に退避
			if (!String.IsNullOrEmpty(self))
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "InstallUpdate() 自分自身を退避");
				try
				{
					File.Move(Application.ExecutablePath, OldPath() + Path.GetFileName(Application.ExecutablePath));
				}
				catch
				{
					throw new Exception("現行ファイルの退避ができませんでした。");
				}
				InstallMove(self, aExtractBasePath);
			}

		}

		// --------------------------------------------------------------------
		// ダウンロード完了している自動更新用アーカイブの MD5 は有効か
		// --------------------------------------------------------------------
		private Boolean IsUpdateArchiveMD5Valid()
		{
			// MD5 ハッシュ値の取得
			Byte[] aHashBytes;
			using (FileStream aFS = new FileStream(UpdateArchivePath(), FileMode.Open, FileAccess.Read, FileShare.Read))
			{
				using (MD5 aMD5 = MD5.Create())
				{
					aHashBytes = aMD5.ComputeHash(aFS);

				}
			}

			// ハッシュ値を文字列に変換
			String aHashStr = BitConverter.ToString(aHashBytes).Replace("-", "");

			// 確認
			if (String.Compare(mAutoUpdateStates.DownloadMD5, aHashStr, true) != 0)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "IsUpdateArchiveMD5Valid() アーカイブ情報の MD5 が異なる: mAutoUpdateStates.DownloadMD5: " + mAutoUpdateStates.DownloadMD5);
				return false;
			}
			return true;
		}

		// --------------------------------------------------------------------
		// 更新版が既にダウンロード完了しているか
		// --------------------------------------------------------------------
		private Boolean IsUpdateDownloaded()
		{
			// アーカイブがあるか
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "IsUpdateDownloaded() path: " + UpdateArchivePath());
			if (!File.Exists(UpdateArchivePath()))
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "IsUpdateDownloaded() アーカイブが無い");
				return false;
			}

			// アーカイブは有効か
			if (mAutoUpdateStates.DownloadVer != mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE])
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "IsUpdateDownloaded() アーカイブ情報のバージョンが異なる");
				return false;
			}
			if (mAutoUpdateStates.DownloadMD5 != mUpdateItems[0].Elements[RssManager.NODE_NAME_LINK + RssItem.RSS_ITEM_NAME_DELIMITER + RssManager.ATTRIBUTE_NAME_MD5])
			{
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "IsUpdateDownloaded() アーカイブ情報の MD5 が異なる");
				return false;
			}
			if (!IsUpdateArchiveMD5Valid())
			{
				return false;
			}

			LogAndSendAndShowMessage(TraceEventType.Information, "新しいバージョンをすでにダウンロード済です。", false);
			return true;

		}

		// --------------------------------------------------------------------
		// ログへの記録とユーザーへの表示
		// --------------------------------------------------------------------
		private Boolean LogAndSendAndShowMessage(TraceEventType oEventType, String oMsg, Boolean oShowIfForceShow)
		{
			// デバッグ用メッセージを無視
#if !DEBUG
			if (oEventType == TraceEventType.Verbose)
			{
				return true;
			}
#endif

			if (String.IsNullOrEmpty(oMsg))
			{
				return false;
			}

			PostCommand(UpdaterCommand.WorkerThreadNotifyLogMessage, "［" + mLogWriter.TraceEventTypeToCaption(oEventType) + "］" + oMsg);

			if (oShowIfForceShow && mParams.ForceShow)
			{
				UpdaterCommon.NotifyDisplayedIfNeeded(mParams);
				mLogWriter.ShowLogMessage(oEventType, oMsg);
			}
			else
			{
				mLogWriter.LogMessage(oEventType, oMsg);
			}
			return true;
		}

		// --------------------------------------------------------------------
		// 現行ファイルのバックアップ先パス
		// --------------------------------------------------------------------
		private String OldPath()
		{
			return Path.GetDirectoryName(Application.ExecutablePath) + "\\" + FOLDER_NAME_UPDATE + FOLDER_NAME_OLD;
		}

		// --------------------------------------------------------------------
		// メインウィンドウにコマンドを送る
		// --------------------------------------------------------------------
		private void PostCommand(UpdaterCommand oCmd, Int32 oWParam = 0, Int32 oLParam = 0)
		{
			WindowsApi.PostMessage(MainFormHandle, (UInt32)oCmd, (IntPtr)oWParam, (IntPtr)oLParam);
		}

		// --------------------------------------------------------------------
		// メインウィンドウにコマンド（文字列付き）を送る
		// WPARAM は送りたい文字列の番号となる
		// --------------------------------------------------------------------
		private void PostCommand(UpdaterCommand oCmd, String oMsg, Int32 oLParam = 0)
		{
			Int32 aIndex;
			lock (mMessageStrings)
			{
				mMessageStrings.Add(oMsg);
				aIndex = mMessageStrings.Count - 1;
			}
			PostCommand(oCmd, aIndex, oLParam);
		}

		// --------------------------------------------------------------------
		// 最新情報の確認と表示準備
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void PrepareLatest()
		{
			LogAndSendAndShowMessage(TraceEventType.Information, mDisplayName + "の最新情報を確認中...", false);

			// RSS チェック
			RssManager aRssManager = new RssManager();
			SetRssManager(aRssManager, FILE_NAME_LATEST_SUFFIX);
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, "PrepareLatest() location: " + mParams.LatestRss);
			String aErr;
			if (!aRssManager.ReadLatestRss(mParams.LatestRss, out aErr))
			{
				throw new Exception(aErr);
			}
			aRssManager.GetNewItems(out mNewItems);

			// 更新
			aRssManager.UpdatePastRss();
			aRssManager.Save();

			// 分析
			if (mNewItems.Count == 0)
			{
				throw new Exception("最新情報はありませんでした。");
			}
			LogAndSendAndShowMessage(TraceEventType.Information, mDisplayName + "の最新情報が "
					+ mNewItems.Count.ToString() + " 件見つかりました。", false);
		}

		// --------------------------------------------------------------------
		// 更新版の確認とインストール準備
		// ＜例外＞ Exception
		// --------------------------------------------------------------------
		private void PrepareUpdate()
		{
			LogAndSendAndShowMessage(TraceEventType.Information, mDisplayName + "の更新版を確認中...", false);

			// 更新制御情報
			VariableSettingsProvider aProvider = (VariableSettingsProvider)mAutoUpdateStates.Providers[VariableSettingsProvider.PROVIDER_NAME_VARIABLE_SETTINGS];
			aProvider.FileName = Path.GetDirectoryName(Application.UserAppDataPath) + "\\" + mParams.ID + FILE_NAME_UPDATE_SUFFIX + Common.FILE_EXT_CONFIG;
			if (mParams.ClearUpdateCache)
			{
				LogAndSendAndShowMessage(TraceEventType.Information, "更新制御情報をクリアします。", false);
				// 空の情報で保存
				mAutoUpdateStates.Reset();
				mAutoUpdateStates.Save();
				// ダウンロード済みファイルも削除
				try
				{
					File.Delete(UpdateArchivePath());
				}
				catch
				{
				}
			}
			else
			{
				mAutoUpdateStates.Reload();

			}

			// RSS チェック
			RssManager aRssManager = new RssManager();
			SetRssManager(aRssManager, FILE_NAME_DUMMY_SUFFIX);
			String aErr;
			if (!aRssManager.ReadLatestRss(mParams.UpdateRss, out aErr))
			{
				throw new Exception(aErr);
			}

			// 全件取得
			aRssManager.GetAllItems(out mUpdateItems);

			// 分析
			if (mUpdateItems.Count == 0)
			{
				throw new Exception("自動更新用 RSS に情報がありません。");
			}
			AnalyzeUpdateRss();
			LogAndSendAndShowMessage(TraceEventType.Information, "新しいバージョン「" + mUpdateItems[0].Elements[RssManager.NODE_NAME_TITLE]
					+ "」が見つかりました。", false);

			// ダウンロード
			if (!IsUpdateDownloaded())
			{
				DownloadUpdateArchive();
			}
		}

		// --------------------------------------------------------------------
		// RSS マネージャーの設定を行う
		// --------------------------------------------------------------------
		private void SetRssManager(RssManager oRssManager, String oConfigFileSuffix)
		{
			// 既存設定の読込
			VariableSettingsProvider aProvider = (VariableSettingsProvider)oRssManager.Providers[VariableSettingsProvider.PROVIDER_NAME_VARIABLE_SETTINGS];
			aProvider.FileName = Path.GetDirectoryName(Application.UserAppDataPath) + "\\" + mParams.ID + oConfigFileSuffix + Common.FILE_EXT_CONFIG;
			oRssManager.Reload();

			// スレッド制御
			oRssManager.CancellationToken = mCancellationTokenSource.Token;

			// UA
			oRssManager.UserAgent += USER_AGENT_PART + UpdaterConstants.APP_VER;
			//mTraceSource.TraceEvent(TraceEventType.Verbose, 0, "SetRssManager() UA: " + oRssManager.UserAgent);

#if DEBUG
			String aGuids = "SetRssManager() PastRssGuids:\n";
			foreach (String aGuid in oRssManager.PastRssGuids)
			{
				aGuids += aGuid + "\n";
			}
			mLogWriter.ShowLogMessage(TraceEventType.Verbose, aGuids);
#endif
		}

		// --------------------------------------------------------------------
		// インストールを開始する旨のメッセージを表示
		// --------------------------------------------------------------------
		private void ShowInstallMessage()
		{
			UpdaterCommon.NotifyDisplayedIfNeeded(mParams);
			mLogWriter.ShowLogMessage(TraceEventType.Information, mDisplayName + "の更新版をインストールします。\n"
					+ mDisplayName + "が起動している場合は終了してから、OK ボタンをクリックして下さい。");
		}

		// --------------------------------------------------------------------
		// 別スレッドで実行させる関数
		// --------------------------------------------------------------------
		private void ThreadMethod()
		{
			Boolean aLatestResult = false;
			Boolean aUpdateResult = false;
			String aLatestErr = String.Empty;
			String aUpdateErr = String.Empty;
			Boolean aTotalResult = true;

			try
			{
				// ASSERT
				Debug.Assert(mParams != null, "WorkerThread.ThreadMethod(): bad Param");

				// 待機
				if (mParams.Wait > 0)
				{
					LogAndSendAndShowMessage(TraceEventType.Information, mParams.Wait.ToString() + " 秒待機します...", false);
					Thread.Sleep(mParams.Wait * 1000);
				}

				// 最新情報確認
				if (mParams.IsLatestMode())
				{
					aLatestResult = CheckLatestInfo(out aLatestErr);
				}

				// 自動更新
				if (mParams.IsUpdateMode())
				{
					aUpdateResult = CheckUpdate(out aUpdateErr);
				}

				if (!aLatestResult && !aUpdateResult)
				{
					// 片方でも正常に終了していればそこでメッセージが表示される
					// どちらも正常に終了していない場合のみメッセージを表示する
					LogAndSendAndShowMessage(TraceEventType.Error, aLatestErr + "\n\n" + aUpdateErr, true);
					aTotalResult = false;
				}

			}
			catch (Exception oExcep)
			{
				mLogWriter.ShowLogMessage(TraceEventType.Error, "スレッドの予期しないエラー：" + oExcep.Message);
				aTotalResult = false;
			}
			finally
			{
				UpdaterCommon.NotifyDisplayedIfNeeded(mParams);
				PostCommand(UpdaterCommand.CloseMainFormRequested, Convert.ToInt32(aTotalResult));
			}
		}

		// --------------------------------------------------------------------
		// 更新版のアーカイブをダウンロードしたフルパス
		// --------------------------------------------------------------------
		private String UpdateArchivePath()
		{
			return Path.GetDirectoryName(Application.ExecutablePath) + "\\" + FOLDER_NAME_UPDATE + FOLDER_NAME_NEW_ARCHIVE + FILE_NAME_DOWNLOAD_ZIP;
		}

		// --------------------------------------------------------------------
		// 更新版のアーカイブをダウンロードしたフルパス
		// --------------------------------------------------------------------
		private void WaitTargetExit()
		{

			if (mParams.PID == 0)
			{
				// 更新対象アプリが起動していることを知らされていない
				mLogWriter.ShowLogMessage(TraceEventType.Verbose, "WaitTargetExit() PID が 0 なので待機しない");
				return;
			}
			PostCommand(UpdaterCommand.WorkerThreadStatusChangedWaiting);

			// プロセスの終了を待機
			Process aTargetProcess;
			try
			{
				aTargetProcess = Process.GetProcessById(mParams.PID);
			}
			catch
			{
				LogAndSendAndShowMessage(TraceEventType.Information, "終了検知ができません。終了を待たずに続行します。", false);
				return;
			}
			aTargetProcess.WaitForExit();
			aTargetProcess.Dispose();
			LogAndSendAndShowMessage(TraceEventType.Information, "終了を検知しました。続行します。", false);
		}

	}
	// public class WorkerThread ___END___
}
// namespace Updater.Misc ___END___

