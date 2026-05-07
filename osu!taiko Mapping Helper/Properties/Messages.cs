namespace osu_taiko_Mapping_Helper.Properties
{
    /// <summary>
    /// メッセージクラス<br/>
    /// ログ、ダイアログに出力するメッセージ
    /// </summary>
    class Messages
    {
        // ダイアログメッセージ
        internal static readonly Dictionary<string, string> DialogMessagesJp = new()
        {
            { "I_A-P-1", "処理に成功しました。\nCtrl+Lを押して譜面を更新してください。" },
            { "I_A-P-2", "譜面を実行前の状態に戻しました。\nCtrl+Lを押して譜面を更新してください。" },
            { "I_A-P-3", "設定を保存しました。" },
            { "I_A-P-4", "全ノーツをリスナップしますが、よろしいですか？" },
            { "E_A-P-1", "処理に失敗しました。" },
            { "E_A-P-2", "設定の保存に失敗しました。" },
            { "E_A-D-1", "譜面情報の取得に失敗しました。" },
            { "E_A-D-2", "BGの取得に失敗しました。" },
            { "E_V-EM-1", "バックアップ保持上限を指定してください。" },
            { "E_V-EM-2", "ノーツのx座標を指定してください。" },
            { "E_V-EM-3", "ノーツのy座標を指定してください。" },
            { "E_V-EM-4", "Timingを指定してください。" },
            { "E_V-EM-5", "SVを指定してください。" },
            { "E_V-EM-6", "Volumeを指定してください。" },
            { "E_V-EM-7", "基準SVを指定してください。" },
            { "E_V-EM-8", "ビートスナップ間隔を指定してください。" },
            { "E_V-EM-9", "オフセットを指定してください。" },
            { "E_V-C-1", "バックアップ保持上限は1～100の範囲で指定してください。" },
            { "E_V-C-2", "ノーツのx座標は0～512の範囲で指定してください。" },
            { "E_V-C-3", "ノーツのy座標は0～384の範囲で指定してください。" },
            { "E_V-C-4", "Timingは「mm:ss:fff」形式または、ミリ秒形式で入力してください。" },
            { "E_V-C-5", "Timingの始点が終点の値を上回っています。" },
            { "E_V-C-6", "Volumeは0～100の範囲で指定してください。" },
            { "E_V-T-1", "バックアップ保持上限は整数で指定してください。" },
            { "E_V-T-2", "バックアップ保持上限は数値で指定してください。" },
            { "E_V-T-3", "ノーツのx座標は整数で指定してください。" },
            { "E_V-T-4", "ノーツのx座標は数値で指定してください。" },
            { "E_V-T-5", "ノーツのy座標は整数で指定してください。" },
            { "E_V-T-6", "ノーツのy座標は数値で指定してください。" },
            { "E_V-T-7", "SVは数値で指定してください。" },
            { "E_V-T-8", "SVは0より大きい値を指定してください。" },
            { "E_V-T-9", "Volumeは数値で指定してください。" },
            { "E_V-T-10", "Volumeは整数で指定してください。" },
            { "E_V-T-11", "基準SVは数値で指定してください。" },
            { "E_V-T-12", "基準SVは正の値を指定してください。" },
            { "E_V-T-13", "ビートスナップ間隔は数値で指定してください。" },
            { "E_V-T-14", "ビートスナップ間隔は自然数を指定してください。" },
            { "E_V-T-15", "オフセットは数値で指定してください。" },
            { "E_V-T-16", "オフセットは整数で指定してください。" },
        };
        // ダイアログメッセージ
        internal static readonly Dictionary<string, string> DialogMessagesEn = new()
        {
            { "I_A-P-1", "The operation has been completed successfully.\nPress Ctrl+L to reload the map." },
            { "I_A-P-2", "The map has been restored to its version before execution.\nPress Ctrl+L to reload it." },
            { "I_A-P-3", "Settings have been saved." },
            { "I_A-P-4", "All notes will be resnapped. Are you sure?" },
            { "E_A-P-1", "The operation failed." },
            { "E_A-P-2", "Settings failed." },
            { "E_A-D-1", "Getting the map information failed." },
            { "E_A-D-2", "Getting the background failed." },
            { "E_V-EM-1", "Please specify the backup limit." },
            { "E_V-EM-2", "Please specify the note X position." },
            { "E_V-EM-3", "Please specify the note Y position." },
            { "E_V-EM-4", "Please specify the timing." },
            { "E_V-EM-5", "Please specify the SV." },
            { "E_V-EM-6", "Please specify the volume." },
            { "E_V-EM-7", "Please specify the base SV." },
            { "E_V-EM-8", "Please specify the beat snap divisor." },
            { "E_V-EM-9", "Please specify the offset." },
            { "E_V-C-1", "Please specify the backup limit between 1 and 100." },
            { "E_V-C-2", "Please specify the note X position between 0 and 512." },
            { "E_V-C-3", "Please specify the note Y position between 0 and 384." },
            { "E_V-C-4", "Please enter the timing in \"mm:ss:fff\" format or in milliseconds." },
            { "E_V-C-5", "The start timing exceeds the end timing." },
            { "E_V-C-6", "Please specify the volume between 1 and 100." },
            { "E_V-T-1", "Please specify the backup limit as an integer." },
            { "E_V-T-2", "Please specify the backup limit as a number." },
            { "E_V-T-3", "Please specify the note X position as an integer." },
            { "E_V-T-4", "Please specify the note X position as a number." },
            { "E_V-T-5", "Please specify the note Y position as an integer." },
            { "E_V-T-6", "Please specify the note Y position as a number." },
            { "E_V-T-7", "Please specify the SV as a number." },
            { "E_V-T-8", "Please specify the SV greater than 0." },
            { "E_V-T-9", "Please specify the volume as a number." },
            { "E_V-T-10", "Please specify the volume as an integer." },
            { "E_V-T-11", "Please specify the base SV as a number." },
            { "E_V-T-12", "Please specify the base SV as a positive value." },
            { "E_V-T-13", "Please specify the beat snap divisor as a number." },
            { "E_V-T-14", "Please specify the beat snap divisor as a natural number." },
            { "E_V-T-15", "Please specify the offset as a number." },
            { "E_V-T-16", "Please specify the offset as a integer." }        };
        // ログメッセージ
        internal static readonly Dictionary<string, string> LogMessages = new()
        {
            { "LOG_E-EXPORT-OSU", "osuへエクスポートに失敗しました。" },
            { "LOG_E-GET-BEATMAP", "譜面情報の取得に失敗しました。" },
            { "LOG_E-GET-INPUT", "入力情報の取得に失敗しました。" },
            { "LOG_E-ADD-LINES", "緑線の追加に失敗しました。" },
            { "LOG_E-REMOVE-LINES", "緑線の削除に失敗しました。" },
            { "LOG_E-MODIFY-LINES", "緑線の変更に失敗しました。" },
            { "LOG_E-CALCULATE-LINES", "緑線の算出に失敗しました。" },
            { "LOG_E-CREATE-BACKUP", "バックアップの作成に失敗しました。" },
            { "LOG_E-EXCEPTION", "例外エラーが発生しました。" },
            { "LOG_W-GET-BG", "BGの取得に失敗しました。" },
            { "LOG_I-START", "アプリケーションを開始します。" },
            { "LOG_I-END", "アプリケーションを終了します。" },
        };
    }
}
