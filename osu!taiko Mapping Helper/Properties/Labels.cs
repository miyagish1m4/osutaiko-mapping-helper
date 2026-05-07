namespace osu_taiko_Mapping_Helper.Properties
{
    internal class Labels
    {
        internal static readonly Dictionary<string, string> LabelsJp = new()
        {
            // MainForm(共通)
            { "LBL_BACKUPS", "バックアップ" },
            { "LBL_SETTINGS", "設定" },
            // MainForm(SVEditor)
            { "LBL_APPLY_TIMING_FROM", "始点に適用" },
            { "LBL_APPLY_TIMING_TO", "終点に適用" },
            { "LBL_SV_CALCULATION", "計算方法" },
            { "LBL_LINEAR", "等差" },
            { "LBL_GEOMETRIC", "等比" },
            { "LBL_RELATIVE_MODE", "相対速度変化" },
            { "LBL_RELATIVE_MULTIPLY", "乗算" },
            { "LBL_RELATIVE_SUM", "加算" },
            { "LBL_RELATIVE_BASESV", "基準SV" },
            { "LBL_RELATIVE_ENABLE_TIMING_TO", "終点の有効化" },
            { "LBL_TAB_APPLY", "適用" },
            { "LBL_TAB_DELETE", "削除" },
            { "LBL_APPLY_OFFSET", "オフセット" },
            { "LBL_APPLY_HEXA_OFFSET", "1/16オフセット"},
            { "LBL_APPLY_DUO_OFFSET", "1/12を含める )"},
            { "LBL_EXECUTE", "実行"},
            { "LBL_APPLY_TAB_OBJECTS", "Objectsのみ" },
            { "LBL_APPLY_TAB_BEATSNAPS", "BeatSnaps" },
            { "LBL_APPLY_TAB_INHERITED_POINTS", "緑線" },
            { "LBL_OBJECTS_ALL_HITOBJECTS", "すべてのHitObject" },
            { "LBL_OBJECTS_BARLINES", "小節線" },
            { "LBL_OBJECTS_ON_BARLINES", "小節線上のみ" },
            { "LBL_OBJECTS_OFF_BARLINES", "小節線上以外" },
            { "LBL_OBJECTS_BOOKMARKS", "BookMark" },
            { "LBL_OBJECTS_ON_BOOKMARKS", "BookMarkのみ" },
            { "LBL_OBJECTS_OFF_BOOKMARKS", "BookMark以外" },
            { "LBL_OBJECTS_SPECIFIC_HITOBJECTS", "特定のオブジェクトのみ" },
            { "LBL_OBJECTS_NORMAL", "通常" },
            { "LBL_OBJECTS_FINISHER", "大音符" },
            { "LBL_BEATSNAPS_DIVISOR", "ビートスナップ間隔" },
            { "LBL_DELETE_OFFSET", "始点のオフセット" },
            // MainForm(Utility)
            { "LBL_OFFSET_SHIFTER", "オフセット変更 (all diffs)" },
            { "LBL_HITSOUND_OPTION", "ヒットサウンド設定" },
            { "LBL_NOTES_POSITION", "ノーツ座標設定" },
            { "LBL_TAG_EDITOR", "タグエディタ (all diffs)" },
            { "LBL_SETTING_COPIER", "設定コピー (all diffs)" },
            { "LBL_RESNAP", "再スナップ" },
            { "LBL_BEAT_SNAP_DIVISOR", "ビートスナップ間隔" },
            { "LBL_UTILITY_APPLY", "実行" },
            { "LBL_OPTIONAL_ALL_DIFFS", "(all diffs)" },
            // SettingForm
            { "LBL_SETTINGS_LANGUAGE", "言語設定" },
            { "LBL_SETTINGS_BACKUP_LIMIT", "バックアップ保持上限" },
            { "LBL_SETTINGS_NOTES_POSITION", "ノーツ配置設定" },
            { "LBL_SETTINGS_UNICODE_SUPPORT", "Unicode対応" },
            { "LBL_SETTINGS_SAVE", "保存" },
            // DebugForm
            { "LBL_DEBUG_SCROLL_SPEED", "見た目BPM" },
            // DialogForm
            { "LBL_DIALOG_OK", "OK" },
            { "LBL_DIALOG_CANCEL", "キャンセル" },
        };
        internal static readonly Dictionary<string, string> LabelsEn = new()
        {
            // MainForm(Common)
            { "LBL_BACKUPS", "Backups" },
            { "LBL_SETTINGS", "Settings" },
            // MainForm(SVEditor)
            { "LBL_APPLY_TIMING_FROM", "Apply Start Timing" },
            { "LBL_APPLY_TIMING_TO", "Apply End Timing" },
            { "LBL_SV_CALCULATION", "Calculation" },
            { "LBL_LINEAR", "Linear" },
            { "LBL_GEOMETRIC", "Geometric" },
            { "LBL_RELATIVE_MODE", "Relative Mode" },
            { "LBL_RELATIVE_MULTIPLY", "Multiply" },
            { "LBL_RELATIVE_SUM", "Sum" },
            { "LBL_RELATIVE_BASESV", "BaseSV" },
            { "LBL_RELATIVE_ENABLE_TIMING_TO", "Enable End Timing" },
            { "LBL_TAB_APPLY", "Apply" },
            { "LBL_TAB_DELETE", "Delete" },
            { "LBL_APPLY_OFFSET", "Offset" },
            { "LBL_APPLY_HEXA_OFFSET", "1/16 Offset"},
            { "LBL_APPLY_DUO_OFFSET", "1/12 Inclusive )"},
            { "LBL_EXECUTE", "Execute"},
            { "LBL_APPLY_TAB_OBJECTS", "Only Objects" },
            { "LBL_APPLY_TAB_BEATSNAPS", "BeatSnaps" },
            { "LBL_APPLY_TAB_INHERITED_POINTS", "Inherited Points" },
            { "LBL_OBJECTS_ALL_HITOBJECTS", "All Objects" },
            { "LBL_OBJECTS_BARLINES", "Barlines" },
            { "LBL_OBJECTS_ON_BARLINES", "On Barlines" },
            { "LBL_OBJECTS_OFF_BARLINES", "Except Barlines" },
            { "LBL_OBJECTS_BOOKMARKS", "BookMarks" },
            { "LBL_OBJECTS_ON_BOOKMARKS", "On BookMarks" },
            { "LBL_OBJECTS_OFF_BOOKMARKS", "Except BookMarks" },
            { "LBL_OBJECTS_SPECIFIC_HITOBJECTS", "Specific Objects" },
            { "LBL_OBJECTS_NORMAL", "Normal" },
            { "LBL_OBJECTS_FINISHER", "Finisher" },
            { "LBL_BEATSNAPS_DIVISOR", "Beat Snap Divisor" },
            { "LBL_DELETE_OFFSET", "Offset" },
            // MainForm(Utility)
            { "LBL_OFFSET_SHIFTER", "Offset Shifter (all diffs)" },
            { "LBL_HITSOUND_OPTION", "Hitsound Option" },
            { "LBL_NOTES_POSITION", "Notes Position" },
            { "LBL_TAG_EDITOR", "Tag Editor (all diffs)" },
            { "LBL_SETTING_COPIER", "Setting Copier (all diffs)" },
            { "LBL_RESNAP", "Resnap" },
            { "LBL_BEAT_SNAP_DIVISOR", "Beat Snap Divisor" },
            { "LBL_UTILITY_APPLY", "Apply" },
            { "LBL_OPTIONAL_ALL_DIFFS", "(all diffs)" },
            // SettingForm
            { "LBL_SETTINGS_LANGUAGE", "Language" },
            { "LBL_SETTINGS_BACKUP_LIMIT", "Backup Limit" },
            { "LBL_SETTINGS_NOTES_POSITION", "Notes Position" },
            { "LBL_SETTINGS_UNICODE_SUPPORT", "Unicode Support" },
            { "LBL_SETTINGS_SAVE", "Save" },
            // DebugForm
            { "LBL_DEBUG_SCROLL_SPEED", "Scroll Speed(BPM)" },
            // DialogForm
            { "LBL_DIALOG_OK", "OK" },
            { "LBL_DIALOG_CANCEL", "Cancel" },
        };
    }
}
