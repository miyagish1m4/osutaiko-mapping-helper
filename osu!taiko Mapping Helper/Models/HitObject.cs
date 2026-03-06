using osu_taiko_Mapping_Helper.Properties;

namespace osu_taiko_Mapping_Helper.Models
{
    /// <summary>
    /// osuから取得したHitObjectsデータをまとめたクラス
    /// </summary>
    internal class HitObject
    {
        // BPM
        public decimal bpm { set; get; }
        // SV
        public decimal sv { set; get; }
        // SVが掛かるタイミング
        public int svApplyTime { set; get; }
        // ヒットオブジェクトの種類
        public Constants.NoteType noteType;
        // ノーツの種類
        // ビットで判定を行う
        // 0b0000 0000 0000 0000 0000 0000 0000 0001 面
        // 0b0000 0000 0000 0000 0000 0000 0000 0010 面(大音符)
        // 0b0000 0000 0000 0000 0000 0000 0000 0100 縁
        // 0b0000 0000 0000 0000 0000 0000 0000 1000 縁(大音符)
        // 0b0000 0000 0000 0000 0000 0000 0001 0000 スライダー
        // 0b0000 0000 0000 0000 0000 0000 0010 0000 スライダー(大音符)
        // 0b0000 0000 0000 0000 0000 0000 0100 0000 スピナー
        // 0b0000 0000 0000 0000 0000 0000 1000 0000 小節線以外
        // 0b0000 0000 0000 0000 0000 0001 0000 0000 小節線
        // 0b0000 0000 0000 0000 0000 0010 0000 0000 Bookmark以外
        // 0b0000 0000 0000 0000 0000 0100 0000 0000 Bookmark
        public int hitObjectCode { set; get; }
        // NewComboの判定
        public bool isNewCombo { set; get; }
        // 小節線の判定
        public bool isOnBarline { set; get; }
        #region 全ヒットオブジェクト共通変数
        // ヒットオブジェクトのx座標
        public int positionX { set; get; }
        // ヒットオブジェクトのy座標
        public int positionY { set; get; }
        // タイミング(ミリ秒)
        public int time { set; get; }
        // ヒットオブジェクトの種類(.osuファイルのフォーマット)
        public string type { set; get; }
        // ノーツの種類(.osuファイルのフォーマット)
        public string hitSound { set; get; }
        // ヒットサンプルの種類
        public string? hitSample { set; get; } = null;
        #endregion
        #region slider専用変数
        // sliderのカーブ設定を表す文字列
        public string? curveSetting { set; get; } = null;
        // sliderの折り返し回数
        public decimal slides { set; get; }
        // sliderの長さ
        public decimal sliderLength { set; get; }
        // sliderの折り返し時のヒットサウンドの種類
        public string? edgeSounds { set; get; } = null;
        // sliderの折り返し時のヒットサンプルの種類
        public string? edgeSets { set; get; } = null;
        #endregion
        #region spinner専用変数
        // spinnerの終了時間
        public int endTime { set; get; }
        #endregion
        /// <summary>
        /// コンストラクタ
        /// osuファイルから取得した1行のデータをクラス変数に格納する
        /// </summary>
        /// <param name="line">osuファイルから取得1行のデータ</param>
        internal HitObject(string line)
        {
            string[] buff = line.Split(",");

            positionX = int.Parse(buff[0]);
            positionY = int.Parse(buff[1]);
            time = int.Parse(buff[2]);
            type = buff[3];
            hitSound = buff[4];
            if ((int.Parse(buff[3]) & 0b00000001) != 0)
            {
                // ノーツの場合
                noteType = Constants.NoteType.CIRCLE;
                hitSample = buff[5];
            }
            else if ((int.Parse(buff[3]) & 0b00000010) != 0)
            {
                // スライダーの場合
                noteType = Constants.NoteType.SLIDER;
                curveSetting = buff[5];
                slides = decimal.Parse(buff[6]);
                sliderLength = decimal.Parse(buff[7]);
                if (buff.Length > 8)
                {
                    edgeSounds = buff[8];
                    edgeSets = buff[9];
                    hitSample = buff[10];
                }
            }
            else if ((int.Parse(buff[3]) & 0b00001000) != 0)
            {
                // スピナーの場合
                noteType = Constants.NoteType.SPINNER;
                endTime = int.Parse(buff[5]);
                hitSample = buff[6];
            }
            if ((int.Parse(buff[3]) & 0b00000100) != 0)
            {
                // NewComboが有効の場合
                isNewCombo = true;
            }
            isOnBarline = false;
            SetHitObjectCode(buff);
        }
        /// <summary>
        /// 小節線,Bookmark算出処理時に使用されるコンストラクタ
        /// </summary>
        /// <param name="time">timing</param>
        /// <param name="code">ノーツ種別
        /// 　　　　　　　　　 0:小節線
        /// 　　　　　　　　　 1:Bookmark</param>
        internal HitObject(int time, int code)
        {
            // 小節線の場合
            this.time = time;
            this.svApplyTime = time;
            positionX = 0;   // 未使用
            positionY = 0;   // 未使用
            hitSound = "0";  // 未使用
            type = "0";      // 未使用
            isNewCombo = false;
            isOnBarline = true;
            switch (code)
            {
                case 0:
                    noteType = Constants.NoteType.BARLINE;
                    hitObjectCode = 0x00000100;
                    break;
                case 1:
                    noteType = Constants.NoteType.BOOKMARK;
                    hitObjectCode = 0x00000400;
                    break;
            }
        }
        /// <summary>
        /// ヒットサウンドの種類をヒットオブジェクトコードに変換
        /// オブジェクト判定に使用
        /// </summary>
        /// <param name="buff">osuから取得した1行のデータ</param>
        private void SetHitObjectCode(string[] buff)
        {
            // サークルの場合
            if ((int.Parse(buff[3]) & 0b00000001) != 0)
            {
                // 縁の場合
                if (((int.Parse(buff[4]) & 0b0010) != 0 || (int.Parse(buff[4]) & 0b1000) != 0))
                {
                    // フィニッシャーの場合
                    if ((int.Parse(buff[4]) & 0b0100) != 0)
                    {
                        hitObjectCode = 0b00001000;
                    }
                    // フィニッシャーではない場合
                    else
                    {
                        hitObjectCode = 0b00000100;
                    }
                }
                // 面の場合
                else
                {
                    // フィニッシャーの場合
                    if ((int.Parse(buff[4]) & 0b0100) != 0)
                    {
                        hitObjectCode = 0b00000010;
                    }
                    // フィニッシャーではない場合
                    else
                    {
                        hitObjectCode = 0b00000001;
                    }
                }
            }
            // スライダーの場合
            else if ((int.Parse(buff[3]) & 0b00000010) != 0)
            {
                // フィニッシャーではない場合
                if ((int.Parse(buff[4]) & 0b0100) != 0b0100)
                {
                    hitObjectCode = 0b00010000;
                }
                // フィニッシャーの場合
                else
                {
                    hitObjectCode = 0b00100000;
                }
            }
            // スピナーの場合
            else if ((int.Parse(buff[3]) & 0b00001000) != 0)
            {
                hitObjectCode = 0b01000000;
            }
        }
    }
}
