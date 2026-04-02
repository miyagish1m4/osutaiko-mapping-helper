namespace osu_taiko_Mapping_Helper.Models
{
    /// <summary>
    /// osuファイルから取得したTimingPointsデータをまとめたクラス
    /// </summary>
    internal class TimingPoint
    {
        // タイミングポイント
        internal int time { set; get; }
        // BPM
        internal double bpm { set; get; }
        // SV
        internal double sv { set; get; }
        // 1小節の長さ(ms)
        internal double barLength { set; get; }
        // 拍子
        internal int meter { set; get; }
        // サンプルセット(Normal,Soft,Drum 等)
        internal int sampleSet { set; get; }
        // サンプルインデックス
        internal int sampleIndex { set; get; }
        // 音量
        internal int volume { set; get; }
        // タイミングポイントの種類
        internal bool isRedLine { set; get; }
        // エフェクト(kiai有無,小節線有無 等)
        internal int effect { set; get; }
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="time">timing</param>
        /// <param name="bpm">BPM</param>
        /// <param name="sv">SV</param>
        /// <param name="barLength">1小節あたりの時間(ms)</param>
        /// <param name="meter">拍子(meter/4)</param>
        /// <param name="sampleSet">ヒットサウンド</param>
        /// <param name="sampleIndex">ヒットサウンドカスタム時のインデックス</param>
        /// <param name="volume">音量</param>
        /// <param name="isRedLine">赤線判定フラグ</param>
        /// <param name="effect">エフェクト(kiai有無,omit有無など)</param>
        internal TimingPoint(int time,
                             double bpm,
                             double sv,
                             double barLength,
                             int meter,
                             int sampleSet,
                             int sampleIndex,
                             int volume,
                             bool isRedLine,
                             int effect)
        {
            this.time = time;
            this.bpm = bpm;
            this.sv = sv;
            this.barLength = barLength;
            this.meter = meter;
            this.sampleSet = sampleSet;
            this.sampleIndex = sampleIndex;
            this.volume = volume;
            this.isRedLine = isRedLine;
            this.effect = effect;
        }
        /// <summary>
        /// コンストラクタ
        /// osuファイルから取得した1行のデータをクラス変数に格納する
        /// </summary>
        /// <param name="line">osuファイルから取得した1行のデータ</param>
        internal TimingPoint(string line)
        {
            string[] buff = line.Split(",");
            // タイミングポイントが小数だった場合は切り捨て
            if (buff[0].IndexOf(".") != -1)
            {
                buff[0] = buff[0].Substring(0, buff[0].IndexOf("."));
            }
            time = int.Parse(buff[0]);          //タイミング
            meter = int.Parse(buff[2]);         //拍子
            sampleSet = int.Parse(buff[3]);     //サンプルセット(Normal,Soft,Drum 等)
            sampleIndex = int.Parse(buff[4]);   //サンプルインデックス?
            volume = int.Parse(buff[5]);        //音量
            effect = int.Parse(buff[7]);        //エフェクト(kiai有無,小節線有無 等)
            //赤線か緑線か判定する
            if (int.Parse(buff[6]) == 1)
            {
                isRedLine = true;
                barLength = double.Parse(buff[1]) * meter;
                bpm = 60000 / double.Parse(buff[1]);
                sv = 1;
            }
            else
            {
                isRedLine = false;
                sv = -100 / double.Parse(buff[1]);
            }

        }
        internal TimingPoint()
        {
        }
    }
}
