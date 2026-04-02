namespace osu_taiko_Mapping_Helper.Models
{
    /// <summary>
    /// ユーザーがフォーム上で入力したデータをまとめたクラス
    /// </summary>
    [Serializable]
    public class UserInputData

    {
        #region 必須入力値
        // タイミング(開始位置)
        internal int timingFrom { set; get; }
        // タイミング(終了位置)
        internal int timingTo { set; get; }
        // 計算方法
        internal int calculationCode { set; get; }
        #endregion
        #region 任意入力値
        // SV有効化設定
        internal bool isSv { set; get; }
        // SV(開始)
        internal double svFrom { set; get; }
        // SV(終了)
        internal double svTo { set; get; }
        // 音量有効化設定
        internal bool isVolume { set; get; }
        // 音量(開始)
        internal int volumeFrom { set; get; }
        // 音量(終了)
        internal int volumeTo { set; get; }
        // オフセット有効化設定
        internal bool isOffset { set; get; }
        // オフセット値
        internal int offset { set; get; }
        // kiai有効化設定
        internal bool isKiai { set; get; }
        // 相対速度オプション
        // -1 : 無効
        // 0  : 乗算
        // 1  : 加算
        internal int relativeCode { set; get; }
        // 相対速度オプションで"乗算"選択時の基準SV
        internal double relativeBaseSv { set; get; }
        internal SetOption setOption = new();
        internal SetObjectOption setObjectOption = new();
        internal SetBeatSnapOption setBeatSnapOption = new();
        //作成日時
        internal DateTime createDate { set; get; }
        // 更新日時
        // internal DateTime updateDate { set; get; }
        // 対象オブジェクト設定
        internal struct SetOption
        {
            // オブジェクト
            internal bool isSetObjects { set; get; }
            // ビートスナップ
            internal bool isSetBeatSnap { set; get; }
            // 緑線
            internal bool isSetGreenLine { set; get; }
        }
        internal struct SetObjectOption
        {
            // 設定対象オブジェクトコード
            // 0b0000 0000 0000 0000 0000 0000 0000 0001 面
            // 0b0000 0000 0000 0000 0000 0000 0000 0010 面(大音符)
            // 0b0000 0000 0000 0000 0000 0000 0000 0100 縁
            // 0b0000 0000 0000 0000 0000 0000 0000 1000 縁(大音符)
            // 0b0000 0000 0000 0000 0000 0000 0001 0000 スライダー
            // 0b0000 0000 0000 0000 0000 0000 0010 0000 スライダー(大音符)
            // 0b0000 0000 0000 0000 0000 0000 0100 0000 スピナー
            // 0b0000 0000 0000 0000 0000 0000 1000 0000 スピナー終点
            // 0b0000 0000 0000 0000 0000 0001 0000 0000 小節線以外
            // 0b0000 0000 0000 0000 0000 0010 0000 0000 小節線
            // 0b0000 0000 0000 0000 0000 0100 0000 0000 Bookmark以外
            // 0b0000 0000 0000 0000 0000 1000 0000 0000 Bookmark
            internal int setObjectsCode { set; get; }
            // 始点緑線有効化設定
            internal bool isTimingStart { set; get; }
            // 終点緑線有効化設定
            internal bool isTimingEnd { set; get; }

        }
        internal struct SetBeatSnapOption
        {
            internal bool isBeatSnap { set; get; }
            // ビートスナップ間隔
            internal int beatSnap { set; get; }
        }
        #endregion
        /// <summary>
        /// コンストラクタ
        /// シリアライズ時に使用する
        /// </summary>
        public UserInputData()
        {
        }
    }
}
