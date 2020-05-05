// ============================================================================
// 
// 自動更新の状態をファイルに保存・読み込むためのクラス
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 設定が変更される度にすみやかに保存されるべき
// ----------------------------------------------------------------------------

using Shinta;

using System;
using System.Configuration;

namespace Updater.Misc
{
	[SettingsProvider(typeof(VariableSettingsProvider))]
	public class AutoUpdateStates : ApplicationSettingsBase
	{
		// ====================================================================
		// public 定数
		// ====================================================================

		// ====================================================================
		// public プロパティ
		// ====================================================================

		// ダウンロード・インストールしないバージョン（既にやったか、またはユーザーに不要と言われた）
		private const String KEY_NAME_SKIP_VER = "SkipVer";
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public String SkipVer
		{
			get
			{
				return (String)this[KEY_NAME_SKIP_VER];
			}
			set
			{
				this[KEY_NAME_SKIP_VER] = value;
			}
		}

		// ダウンロードを試行している、もしくは完了したバージョン
		private const String KEY_NAME_DOWNLOAD_VER = "DownloadVer";
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public String DownloadVer
		{
			get
			{
				return (String)this[KEY_NAME_DOWNLOAD_VER];
			}
			set
			{
				this[KEY_NAME_DOWNLOAD_VER] = value;
			}
		}

		// ダウンロードを試行した回数
		private const String KEY_NAME_DOWNLOAD_TRY = "DownloadTry";
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public Int32 DownloadTry
		{
			get
			{
				return (Int32)this[KEY_NAME_DOWNLOAD_TRY];
			}
			set
			{
				this[KEY_NAME_DOWNLOAD_TRY] = value;
			}
		}

		// ダウンロードしたファイルのあるべき MD5
		private const String KEY_NAME_DOWNLOAD_MD5 = "DownloadMD5";
		[UserScopedSetting]
		[DefaultSettingValue("")]
		public String DownloadMD5
		{
			get
			{
				return (String)this[KEY_NAME_DOWNLOAD_MD5];
			}
			set
			{
				this[KEY_NAME_DOWNLOAD_MD5] = value;
			}
		}
	}
	// public class AutoUpdateStates ___END___
}
// namespace Updater.Misc
