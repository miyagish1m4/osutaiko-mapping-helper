namespace osu_taiko_Mapping_Helper.Models
{
    /// <summary>
    /// ブックマークのデータをまとめたクラス
    /// </summary>
    /// <remarks>
    /// SV計算時にSVやBPM情報が必要になるためTimingPointsクラスを継承し、使用する
    /// </remarks>
    internal class Bookmark : TimingPoint
    {
        /// <summary>
        /// コンストラクタ
        /// 取得した時間を格納し、SVを-1で初期化
        /// </summary>
        /// <param name="time">timing</param>
        internal Bookmark(int time)
        {
            this.time = time;
            this.sv = -1;
        }
    }
}
