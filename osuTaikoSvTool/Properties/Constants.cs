namespace osuTaikoSvTool.Properties
{
    /// <summary>
    /// 定数クラス
    /// </summary>
    internal class Constants
    {
        #region ファイル名
        internal const string APP_NAME = "osuTaikoSvTool";
        internal const string APP_VERSION = "0.5.3";
        #endregion
        #region 言語設定
        internal static readonly string[] LANGUAGES = ["日本語", 
                                                       "English"];
        #endregion
        #region ディレクトリ
        // バックアップディレクトリ
        internal const string BACKUP_DIRECTORY = "\\BackUp";
        // 作業ディレクトリ
        internal const string WORK_DIRECTORY = "\\Work";
        // Infoログディレクトリ
        internal const string LOG_DIRECTORY = "\\Log";
        // Infoログディレクトリ
        internal const string INFO_LOG_DIRECTORY = "\\Log\\Info";
        // Warningログディレクトリ
        internal const string WARNING_LOG_DIRECTORY = "\\Log\\Warning";
        // Errorログディレクトリ
        internal const string ERROR_LOG_DIRECTORY = "\\Log\\Error";
        // 履歴ディレクトリ
        internal const string HISTORY_DIRECTORY = "\\History";
        // エスケープ文字
        internal const string ESCAPE_CHARACTER = @"[\\,/,:,*,?,"",<,>,|]";
        #endregion
        #region 拡張子
        // osuファイル拡張子
        internal const string OSU_EXTENSION = ".osu";
        // ログファイル拡張子
        internal const string LOG_EXTENSION = ".log";
        // xmlファイル拡張子
        internal const string XML_EXTENSION = ".xml";
        // csvファイル拡張子
        internal const string CSV_EXTENSION = ".csv";
        #endregion
        #region ログレベル
        internal const string LOG_LEVEL_INFO = "INFO";
        internal const string LOG_LEVEL_WARNING = "WARNING";
        internal const string LOG_LEVEL_ERROR = "ERROR";
        #endregion
        #region osuファイルのセクション
        internal const string GENERAL = "[General]";
        internal const string EDITOR = "[Editor]";
        internal const string METADATA = "[Metadata]";
        internal const string DIFFICULTY = "[Difficulty]";
        internal const string EVENTS = "[Events]";
        internal const string TIMING_POINTS = "[TimingPoints]";
        internal const string COLOURS = "[Colours]";
        internal const string HIT_OBJECTS = "[HitObjects]";
        internal const int VERSION_CODE = 0;
        internal const int GENERAL_CODE = 1;
        internal const int EDITOR_CODE = 2;
        internal const int METADATA_CODE = 3;
        internal const int DIFFICULTY_CODE = 4;
        internal const int EVENTS_CODE = 5;
        internal const int TIMING_POINTS_CODE = 6;
        internal const int COLOURS_CODE = 7;
        internal const int HIT_OBJECTS_CODE = 8;
        internal const string BOOKMARKS = "Bookmarks";
        #endregion
        #region セットコード
        internal const int SET_TIMING = 0;
        internal const int SET_SV = 1;
        internal const int SET_VOLUME = 2;
        #endregion
        #region 実行コード
        internal const int EXECUTE_APPLY = 0;
        internal const int EXECUTE_REMOVE = 1;
        #endregion
        #region 計算コード
        internal const int CALCULATION_ARITHMETIC = 1;  // 等差
        internal const int CALCULATION_GEOMETRIC = 2;   // 等比
        #endregion
        #region 相対速度オプション
        internal const int RELATIVE_DISABLE = -1;   // 無効
        internal const int RELATIVE_SUM = 1;        // 加算
        internal const int RELATIVE_MULTIPLY = 2;   // 乗算
        #endregion
        // ヒットオブジェクトの種類
        internal enum NoteType
        {
            BARLINE,
            BOOKMARK,
            CIRCLE,
            SLIDER,
            SPINNER
        }
    }
}