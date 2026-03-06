namespace osu_taiko_Mapping_Helper.Models
{
    /// <summary>
    /// osuファイルから取得したビートマップ情報
    /// </summary>
    internal class Beatmap
    {
        // バージョン
        internal string version { set; get; }
        // General
        internal List<string> general { set; get; }
        // Editor
        internal List<string> editor { set; get; }
        // Metadata
        internal List<string> metadata { set; get; }
        // Difficulty
        internal List<string> difficulty { set; get; }
        // イベント
        internal List<string> events { set; get; }
        // Colours
        internal List<string> colours { set; get; }
        // タイミングポイント
        internal List<TimingPoint> timingPoints { set; get; }
        // ヒットオブジェクト
        internal List<HitObject> hitObjects { set; get; }
        // ブックマーク
        internal List<Bookmark> bookmarks { set; get; }
        /// <summary>
        /// コンストラクタ
        /// 取得した値をクラスの変数に格納する
        /// </summary>
        /// <param name="version">バージョン</param>
        /// <param name="general">Generalセクション</param>
        /// <param name="editor">Editorセクション</param>
        /// <param name="metadata">Metadataセクション</param>
        /// <param name="difficulty">Difficultyセクション</param>
        /// <param name="events">Eventsセクション</param>
        /// <param name="timingPoints">TimingPointsセクション</param>
        /// <param name="colours">Coloursセクション</param>
        /// <param name="hitObject">HitObjectsセクション</param>
        /// <param name="bookmarks">bookmarkのリスト</param>
        internal Beatmap(string version,
                         List<string> general,
                         List<string> editor,
                         List<string> metadata,
                         List<string> difficulty,
                         List<string> events,
                         List<TimingPoint> timingPoints,
                         List<string> colours,
                         List<HitObject> hitObject,
                         List<Bookmark> bookmarks)
        {
            this.version = version;
            this.general = general;
            this.editor = editor;
            this.metadata = metadata;
            this.difficulty = difficulty;
            this.events = events;
            this.timingPoints = timingPoints;
            this.colours = colours;
            this.hitObjects = hitObject;
            this.bookmarks = bookmarks;
        }
    }
}
