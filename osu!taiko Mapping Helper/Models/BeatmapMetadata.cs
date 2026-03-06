namespace osu_taiko_Mapping_Helper.Models
{
    /// <summary>
    /// 選曲中の譜面から取得したメタデータ
    /// </summary>
    internal class BeatmapMetadata
    {
        // タイトル
        internal string title { set; get; }
        internal string titleUnicode { set; get; }
        // アーティスト
        internal string artist { set; get; }
        internal string artistUnicode { set; get; }
        // マッパー名
        internal string creator { set; get; }
        // Diff名
        internal string version { set; get; }
        // ソース
        internal string source { set; get; }
        // BGのパス
        internal string backgroundPath { set; get; }
        // BGの座標
        internal string background { set; get; }
        // 譜面のパス
        internal string beatmapPath { set; get; }
        // タグ
        internal string[] tags { set; get; }
        /// <summary>
        /// コンストラクタ
        /// クラス変数に空文字を格納する
        /// </summary>
        public BeatmapMetadata()
        {
            this.title = string.Empty;
            this.titleUnicode = string.Empty;
            this.artist = string.Empty;
            this.artistUnicode = string.Empty;
            this.creator = string.Empty;
            this.version = string.Empty;
            this.source = string.Empty;
            this.backgroundPath = string.Empty;
            this.beatmapPath = string.Empty;
            this.background = string.Empty;
            this.tags = Array.Empty<string>();
        }
    }
}
