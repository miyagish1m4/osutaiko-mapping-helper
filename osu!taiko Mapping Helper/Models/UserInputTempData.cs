namespace osu_taiko_Mapping_Helper.Models
{
    /// <summary>
    /// ユーザーがフォーム上で入力したデータをまとめたクラス
    /// </summary>
    [Serializable]
    public class UserInputTempData

    {
        #region 必須入力値
        // タイミング(開始位置)
        internal string timingFrom { set; get; } = string.Empty;
        // タイミング(終了位置)
        internal string timingTo { set; get; } = string.Empty;
        // 計算方法
        internal bool isArithmetic { set; get; } = true;
        internal bool isGeometric { set; get; } = false;
        #endregion
        #region 任意入力値
        // SV有効化設定
        internal bool isSv { set; get; } = true;
        // SV(開始)
        internal string svFrom { set; get; } = string.Empty;
        // SV(終了)
        internal string svTo { set; get; } = string.Empty;
        // 音量有効化設定
        internal bool isVolume { set; get; } = true;
        // 音量(開始)
        internal string volumeFrom { set; get; } = string.Empty;
        // 音量(終了)
        internal string volumeTo { set; get; } = string.Empty;
        // 始点有効化設定
        internal bool isEnableFrom { set; get; } = true;
        // 終点有効化設定
        internal bool isEnableTo { set; get; } = true;
        // 相対速度変化オプション有効化設定
        internal bool isRelative { set; get; } = false;
        // 相対速度オプションで"乗算"選択時のフラグ
        internal bool isRelativeMultiply { set; get; } = true;
        // 相対速度オプションで"乗算"選択時の基準SV
        internal string relativeMultiplyBaseSv { set; get; } = "0";
        // 相対速度オプションで"乗算"選択時のSV(開始)
        internal string relativeMultiplySvFrom { set; get; } = string.Empty;
        // 相対速度オプションで"乗算"選択時のSV(終了)
        internal string relativeMultiplySvTo { set; get; } = string.Empty;
        // 相対速度オプションで"加算"選択時のフラグ
        internal bool isRelativeSum { set; get; } = false;
        // 相対速度オプションで"加算"選択時のSV(開始)
        internal string relativeSumSvFrom { set; get; } = string.Empty;
        // 相対速度オプションで"加算"選択時のSV(終了)
        internal string relativeSumSvTo { set; get; } = string.Empty;
        // 相対速度オプションで終点のSV有効化設定
        internal bool isEnableRelativeEnd { set; get; } = false;
        // オフセットモード
        // 0 : 1/16
        // 1 : ms指定
        internal int offsetMode { set; get; }
        // オフセット有効化設定
        internal bool isOffset { set; get; } = true;
        // オフセット値
        internal string offset { set; get; } = string.Empty;
        // 1/12offset有効化設定
        internal bool isDuoOffset { set; get; }
        internal SetOption setOption = new();
        internal SetObjectOption setObjectOption = new();
        internal SetBeatSnapOption setBeatSnapOption = new();
        internal struct SetOption
        {
            // オブジェクト
            internal bool isSetObjects { set; get; } = true;
            // ビートスナップ
            internal bool isSetBeatSnap { set; get; } = false;
            // 緑線
            internal bool isSetGreenLine { set; get; } = false;
            public SetOption()
            {
            }
        }
        internal struct SetObjectOption
        {
            // 全てのヒットオブジェクト有効化フラグ
            internal bool isAllHitObjects { set; get; } = true;
            // 小節線オプション有効化フラグ
            internal bool isOnlyBarlines { set; get; } = false;
            // 小節線のみ
            internal bool isOnBarlines { set; get; } = true;
            // 小節線以外
            internal bool isOffBarlines { set; get; } = false;
            // ブックマークオプション有効化フラグ
            internal bool isOnlyBookmarks { set; get; } = false;
            // ブックマークのみ
            internal bool isOnBookmarks { set; get; } = true;
            // ブックマーク以外
            internal bool isOffBookmarks { set; get; } = false;
            // 特定のヒットオブジェクトオプション有効化フラグ
            internal bool isOnlyHitObjects { set; get; } = false;
            // d
            internal bool isOnlyNormalDong { set; get; } = false;
            // k
            internal bool isOnlyNormalKa { set; get; } = false;
            // slider
            internal bool isOnlyNormalSlider { set; get; } = false;
            // spinner
            internal bool isOnlySpinner { set; get; } = false;
            // finisher d
            internal bool isOnlyFinisherDong { set; get; } = false;
            // finisher k
            internal bool isOnlyFinisherKa { set; get; } = false;
            // finisher slider
            internal bool isOnlyFinisherSlider { set; get; } = false;
            public SetObjectOption()
            {
            }
        }
        internal struct SetBeatSnapOption
        {
            // ビートスナップ有効化フラグ
            internal bool isBeatSnap { set; get; } = false;
            // ビートスナップ間隔
            internal string beatSnap { set; get; } = string.Empty;
            public SetBeatSnapOption()
            {
            }
        }
        #endregion
        /// <summary>
        /// コンストラクタ
        /// シリアライズ時に使用する
        /// </summary>
        public UserInputTempData()
        {
        }
    }
}
