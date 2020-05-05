// ============================================================================
// 
// ちょちょいと自動更新全体で使う定数
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using Shinta;

using System;

namespace Updater.Misc
{
	// ====================================================================
	// public 列挙子
	// ====================================================================

	// --------------------------------------------------------------------
	// 通信用メッセージ定数
	// --------------------------------------------------------------------
	public enum UpdaterCommand : uint
	{
		// メインフォームのクローズ要求（WPARAM: 終了ステータス、LPARAM: 無し）
		CloseMainFormRequested = WindowsApi.WM_APP,
		// メインフォームの表示要求（引数無し）
		ShowMainFormRequested,
		// タスクバーへのアイコン表示要求（引数無し）→使わなくなった（フォーム表示と同時に実施）
		//ShowTaskBarRequested,
		// ワーカースレッドの状態が更新された：更新対象アプリ終了待ち（引数無し）
		WorkerThreadStatusChangedWaiting,
		// ワーカースレッドの状態が更新された：インストール中（引数無し）
		WorkerThreadStatusChangedInstalling,
		// ワーカースレッドの状態が更新された：整理中（引数無し）
		WorkerThreadStatusChangedCleaning,
		// サブキャプションに表示するメッセージを通知（WPARAM: メッセージ番号、LPARAM: 無し）
		WorkerThreadNotifySubCaption,
		// 表示用ログメッセージを通知（WPARAM: メッセージ番号、LPARAM: 無し）
		WorkerThreadNotifyLogMessage,
		// 進捗率（WPARAM: トータル進捗率［パーミル］、LPARAM: 無し）
		WorkerThreadNotifyProgress,
	}


	public class UpdaterConstants
	{
		// ====================================================================
		// public 定数
		// ====================================================================

		// --------------------------------------------------------------------
		// アプリの基本情報
		// --------------------------------------------------------------------
		public const String APP_ID = "Updater";
		public const String APP_NAME_J = "ちょちょいと自動更新";
		public const String APP_VER = "Ver 3.15";
		public const String COPYRIGHT_J = "Copyright (C) 2014-2020 by SHINTA";

	}
	// public class UpdaterConstants ___END___
}
// namespace Updater.Misc ___END___
