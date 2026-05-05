namespace osu_taiko_Mapping_Helper.Properties
{
    /// <summary>
    /// 定数クラス
    /// </summary>
    internal class Constants
    {
        #region ファイル名
        internal const string APP_NAME = "osu!Taiko Mapping Helper";
        internal const string APP_VERSION = "1.1.0-beta";
        #endregion
        #region 言語設定
        internal static readonly string[] LANGUAGES = ["English",
                                                       "日本語"];
        #endregion
        #region ディレクトリ
        // バックアップディレクトリ
        internal const string BACKUP_DIRECTORY = "\\Backup";
        // Infoログディレクトリ
        internal const string LOG_DIRECTORY = "\\Log";
        // Infoログディレクトリ
        internal const string INFO_LOG_DIRECTORY = "\\Log\\Info";
        // Warningログディレクトリ
        internal const string WARNING_LOG_DIRECTORY = "\\Log\\Warning";
        // Errorログディレクトリ
        internal const string ERROR_LOG_DIRECTORY = "\\Log\\Error";
        // 履歴ディレクトリ
        internal const string DEBUG_DIRECTORY = "\\Debug";
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
        // jpgファイル拡張子
        internal const string JPG_EXTENSION = ".jpg";
        // jpegファイル拡張子
        internal const string JPEG_EXTENSION = ".jpeg";
        // pngファイル拡張子
        internal const string PNG_EXTENSION = ".png";
        // webpファイル拡張子
        internal const string WEBP_EXTENSION = ".webp";
        #endregion
        #region ログレベル
        internal const string LOG_LEVEL_INFO = "INFO";
        internal const string LOG_LEVEL_WARNING = "WARNING";
        internal const string LOG_LEVEL_ERROR = "ERROR";
        #endregion
        #region osuファイルのセクション
        internal const string GENERAL = "[General]";
        internal const string PREVIEW = "PreviewTime: ";
        internal const string EDITOR = "[Editor]";
        internal const string METADATA = "[Metadata]";
        internal const string DIFFICULTY = "[Difficulty]";
        internal const string EVENTS = "[Events]";
        internal const string BG_AND_VIDEO = "//Background and Video events";
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
        internal const string TITLE = "Title:";
        internal const string TITLE_UNICODE = "TitleUnicode:";
        internal const string ARTIST = "Artist:";
        internal const string ARTIST_UNICODE = "ArtistUnicode:";
        internal const string CREATOR = "Creator:";
        internal const string SOURCE = "Source:";
        internal const string TAGS = "Tags:";
        #endregion
        #region セットコード
        internal const int SET_TIMING = 0;
        internal const int SET_BPM = 1;
        internal const int SET_SV = 2;
        internal const int SET_VOLUME = 3;
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
        #region ユーティリティ機能コード
        internal const int UTILITY_OFFSET = 0;
        internal const int UTILITY_HITSOUND = 1;
        internal const int UTILITY_NOTES_POSITION = 2;
        internal const int UTILITY_TAG_EDIT = 3;
        internal const int UTILITY_SETTING_COPIER = 4;
        internal const int UTILITY_RESNAP = 5;
        #endregion
        #region ヒットサウンドコード
        internal const int HITSOUND_CLAP = 0;
        internal const int HITSOUND_WHISTLE = 1;
        #endregion
        #region ノーツポジションコード
        internal const int NOTES_POSITION_CENTER = 0;
        internal const int NOTES_POSITION_SEPARATE = 1;
        internal const int NOTES_POSITION_RANDOM = 2;
        // ノーツ座標
        internal static readonly Point CENTER_NOTES = new(256, 192);
        #endregion
        #region メタデータ設定コード
        internal const int SETTING_COPIER_METADATA = 0;
        internal const int SETTING_COPIER_BG = 1;
        internal const int SETTING_COPIER_PREVIEW = 2;
        internal const int SETTING_COPIER_TIMING_POINTS = 3;
        #endregion
        #region ビートスナップ間隔
        internal static readonly int[] BEAT_SNAP_INTERVALS = [24, 36, 48, 60];
        #endregion
        #region ダイアログのボタンの種類
        internal const int DIALOG_BUTTON_OK = 0;
        internal const int DIALOG_BUTTON_OKCANCEL = 1;
        #endregion
        #region ダイアログオプションコード
        // ビットで判定を行う
        // index : 0 -> モードレスダイアログ
        // index : 1 -> OK/キャンセルの選択肢
        internal const int DIALOG_OPTION_MODELESS = 0x00000001;
        internal const int DIALOG_OPTION_OKCANCEL = 0x00000002;
        #endregion
        internal const double BASE_SLIDER_MULTIPLIER = 1.4;
        internal static string ConvertNoteType(NoteType noteType)
        {
            switch (noteType)
            {
                case NoteType.BARLINE:
                    return "Barline";
                case NoteType.BOOKMARK:
                    return "Bookmark";
                case NoteType.CIRCLE:
                    return "Circle";
                case NoteType.SLIDER:
                    return "Slider";
                case NoteType.SPINNER:
                    return "Spinner";
                case NoteType.SPINNER_END:
                    return "SpinnerEnd";
                default:
                    return "Unknown";
            }
        }
        // ヒットオブジェクトの種類
        internal enum NoteType
        {
            BARLINE,
            BOOKMARK,
            CIRCLE,
            SLIDER,
            SPINNER,
            SPINNER_END,
        }
    }
}