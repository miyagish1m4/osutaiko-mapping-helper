using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// 譜面の読み込みのテスト (TEST_PATTERNS.md の 1)
    /// </summary>
    public class BeatmapReadTests : TestEnvironment
    {
        /// <summary>
        /// 譜面を作って読み込む
        /// </summary>
        private Beatmap Read(OsuFileBuilder builder, bool isThread = false)
        {
            string path = WriteOsuFile("test.osu", builder.Build());
            return BeatmapHelper.GetBeatmapData(path, isThread);
        }

        /// <summary>
        /// 擬似オブジェクト(小節線・ブックマーク・スピナーの終点)を除いたノーツ
        /// </summary>
        private static List<HitObject> Notes(Beatmap beatmap) =>
            [.. beatmap.hitObjects.Where(h => h.noteType is Constants.NoteType.CIRCLE or Constants.NoteType.SLIDER or Constants.NoteType.SPINNER)];

        /// <summary>
        /// 普通の譜面を読める
        /// </summary>
        [Fact]
        public void GetBeatmapData_ReadsNormalBeatmap()
        {
            // 1-1
            var beatmap = Read(new OsuFileBuilder());

            Assert.Equal("osu file format v14", beatmap.version);
            Assert.Equal(2, beatmap.timingPoints.Count);
            Assert.Equal(4, Notes(beatmap).Count);
            Assert.Equal([1500, 2500], beatmap.bookmarks.Select(b => b.time));
            Assert.Contains("SliderMultiplier:1.4", beatmap.difficulty);
        }

        /// <summary>
        /// 空のファイルは、読み込み失敗になる
        /// </summary>
        [Fact]
        public void GetBeatmapData_EmptyFile_Fails()
        {
            // 1-2
            string path = WriteOsuFile("empty.osu", []);
            File.WriteAllText(path, "");

            var beatmap = BeatmapHelper.GetBeatmapData(path);

            Assert.Equal(string.Empty, beatmap.version);
        }

        /// <summary>
        /// 空白だけの行は、空行として飛ばす
        /// </summary>
        [Fact]
        public void GetBeatmapData_WhitespaceLine_SkippedAsEmpty()
        {
            // 1-4
            var builder = new OsuFileBuilder();
            builder.TimingPoints.Add("   ");
            builder.HitObjects.Insert(1, "\t");

            var beatmap = Read(builder);

            Assert.NotEqual(string.Empty, beatmap.version);
            Assert.Equal(2, beatmap.timingPoints.Count);
            Assert.Equal(4, Notes(beatmap).Count);
        }

        /// <summary>
        /// ブックマークの書き方ごとの読み込み
        /// </summary>
        [Theory]
        [InlineData("Bookmarks: 1500,2500", new[] { 1500, 2500 })]
        [InlineData("Bookmarks:1500,2500", new[] { 1500, 2500 })]   // 後ろに空白なし
        [InlineData("Bookmarks: ", new int[0])]                      // 値が空
        [InlineData("Bookmarks: 1500,abc,2500", new[] { 1500 })]     // 数字以外でそこで打ち切り
        public void GetBeatmapData_BookmarkFormats(string line, int[] expected)
        {
            // 1-5
            var builder = new OsuFileBuilder { Editor = [line] };

            var beatmap = Read(builder);

            Assert.NotEqual(string.Empty, beatmap.version);
            Assert.Equal(expected, beatmap.bookmarks.Select(b => b.time));
        }

        /// <summary>
        /// 見出しのコメントが無い Events も、行の先頭の値で読める
        /// </summary>
        [Fact]
        public void Events_WithoutHeaderComments_ReadByFirstValue()
        {
            // 1-6 (lazerで書き出した譜面)
            var builder = new OsuFileBuilder { Events = ["0,0,\"bg.png\",0,0", "2,180257,191875", "Video,0,\"video.mp4\""] };

            var beatmap = Read(builder);

            var bg = beatmap.events.Single(e => !e.isComment && e.eventCode == Constants.BG_AND_VIDEO_CODE && !e.isVideo);
            var video = beatmap.events.Single(e => e.isVideo);
            var breakPeriod = beatmap.events.Single(e => e.eventCode == Constants.BREAK_PERIODS_CODE);
            Assert.Equal("\"bg.png\"", bg.fileName);
            Assert.Equal("\"video.mp4\"", video.fileName);
            Assert.Equal(180257, breakPeriod.startTime);
            Assert.Equal(191875, breakPeriod.endTime);
        }

        /// <summary>
        /// 見出しの後ろに空白がある Events も読める
        /// </summary>
        [Fact]
        public void Events_HeaderCommentWithTrailingSpace_Reads()
        {
            // 1-6
            var builder = new OsuFileBuilder { Events = ["// Background and Video events", "0,0,\"bg.jpg\",0,0", "// Break Periods", "2,152618,153707"] };

            var beatmap = Read(builder);

            var breakPeriod = beatmap.events.Single(e => e.eventCode == Constants.BREAK_PERIODS_CODE && !e.isComment);
            Assert.False(breakPeriod.isVideo);
            Assert.Equal(153707, breakPeriod.endTime);
        }

        /// <summary>
        /// 動画の下のストーリーボードの命令は、元の行のまま読む
        /// </summary>
        [Fact]
        public void Events_StoryboardCommandUnderVideo_KeepsRawLine()
        {
            // 1-7
            var builder = new OsuFileBuilder { Events = ["//Background and Video events", "0,0,\"bg.jpg\",0,0", "Video,0,\"video.mp4\"", " F,0,0,,0,1", "_M,0,0,,0,127", "//Break Periods"] };

            var beatmap = Read(builder);

            Assert.NotEqual(string.Empty, beatmap.version);
            Assert.Contains(beatmap.events, e => e.isComment && e.comment == " F,0,0,,0,1");
            Assert.Contains(beatmap.events, e => e.isComment && e.comment == "_M,0,0,,0,127");
        }

        /// <summary>
        /// BG の行の座標が無ければ 0 として読む
        /// </summary>
        [Theory]
        [InlineData("0,0,\"bg.jpg\"", 0, 0)]          // 座標なし
        [InlineData("0,0,\"bg.jpg\",10", 10, 0)]      // x だけ
        [InlineData("0,0,\"bg.jpg\",10,-20", 10, -20)]
        public void Events_BackgroundWithoutOffset_ReadsAsZero(string line, int expectedX, int expectedY)
        {
            // 1-8 (M4)
            var builder = new OsuFileBuilder { Events = ["//Background and Video events", line, "//Break Periods"] };

            var beatmap = Read(builder);

            var bg = beatmap.events.Single(e => !e.isComment && !e.isVideo && e.eventCode == Constants.BG_AND_VIDEO_CODE);
            Assert.Equal(expectedX, bg.xOffset == int.MinValue ? 0 : bg.xOffset);
            Assert.Equal(expectedY, bg.yOffset == int.MinValue ? 0 : bg.yOffset);
        }

        /// <summary>
        /// 動画と休憩の行は、数字と名前のどちらでも読める
        /// </summary>
        [Theory]
        [InlineData("1,0,\"video.mp4\"", true, false)]
        [InlineData("Video,0,\"video.mp4\"", true, false)]
        [InlineData("2,1000,2000", false, true)]
        [InlineData("Break,1000,2000", false, true)]
        public void Events_VideoAndBreak_ReadByNumberOrName(string line, bool isVideo, bool isBreak)
        {
            // 1-9
            var builder = new OsuFileBuilder { Events = ["//Background and Video events", "0,0,\"bg.jpg\",0,0", line] };

            var beatmap = Read(builder);

            var ev = beatmap.events.Last();
            Assert.Equal(isVideo, ev.isVideo);
            Assert.Equal(isBreak, ev.eventCode == Constants.BREAK_PERIODS_CODE);
        }

        /// <summary>
        /// 古い背景色の行は、元の行のまま読む
        /// </summary>
        [Fact]
        public void Events_OldBackgroundColour_KeepsRawLine()
        {
            // 1-9
            var builder = new OsuFileBuilder { Events = ["//Background and Video events", "0,0,\"bg.jpg\",0,0", "3,100,163,162,255"] };

            var beatmap = Read(builder);

            var ev = beatmap.events.Last();
            Assert.True(ev.isComment);
            Assert.False(ev.isVideo);
            Assert.Equal("3,100,163,162,255", ev.comment);
        }

        /// <summary>
        /// 古い形式のノーツの行は、読み込み失敗になる
        /// </summary>
        [Fact]
        public void GetBeatmapData_OldFormatHitObject_Fails()
        {
            // 1-10 対応しない仕様を固定する (TimingPropertyは別の読み込みで読める)
            var builder = new OsuFileBuilder { HitObjects = ["480,80,5371,1,6"] };

            var beatmap = Read(builder);

            Assert.Equal(string.Empty, beatmap.version);
        }

        /// <summary>
        /// 線の小数の時刻は、切り捨てて読む
        /// </summary>
        [Fact]
        public void TimingPoint_DecimalTime_Truncated()
        {
            // 1-11 対応しない仕様を固定する
            var builder = new OsuFileBuilder { TimingPoints = ["1517.52667247697,500,4,1,0,100,1,0"] };

            var beatmap = Read(builder);

            Assert.Equal(1517, beatmap.timingPoints[0].time);
        }

        /// <summary>
        /// LF だけの改行のファイルと、BOM 付きのファイルも読める
        /// </summary>
        [Fact]
        public void GetBeatmapData_LfAndBom_Reads()
        {
            // 1-14
            string path = Path.Combine(WorkDirectory, "lf.osu");
            File.WriteAllText(path, string.Join("\n", new OsuFileBuilder().Build()) + "\n", new System.Text.UTF8Encoding(true));

            var beatmap = BeatmapHelper.GetBeatmapData(path);

            Assert.Equal("osu file format v14", beatmap.version);
            Assert.Equal(4, Notes(beatmap).Count);
        }

        /// <summary>
        /// モードを取得できる
        /// </summary>
        [Theory]
        [InlineData("Mode: 1", 1)]
        [InlineData("Mode: 0", 0)]
        [InlineData(null, 0)]   // 行が無い
        public void GetMode_ReturnsMode(string? modeLine, int expected)
        {
            // 1-16
            var builder = new OsuFileBuilder();
            builder.General.RemoveAll(l => l.StartsWith("Mode"));
            if (modeLine != null) builder.General.Add(modeLine);

            var beatmap = Read(builder);

            Assert.Equal(expected, BeatmapHelper.GetMode(beatmap));
        }

        /// <summary>
        /// ノーツが無い譜面も読める
        /// </summary>
        [Fact]
        public void GetBeatmapData_NoHitObjects_Reads()
        {
            // 1-18
            var builder = new OsuFileBuilder { HitObjects = [] };

            var beatmap = Read(builder);

            Assert.NotEqual(string.Empty, beatmap.version);
            Assert.Empty(Notes(beatmap));
        }

        /// <summary>
        /// ノーツの種類を判定できる
        /// </summary>
        [Theory]
        [InlineData("256,192,1000,1,0,0:0:0:0:", 0x001)]    // ドン
        [InlineData("256,192,1000,1,4,0:0:0:0:", 0x002)]    // 大ドン
        [InlineData("256,192,1000,1,2,0:0:0:0:", 0x004)]    // カツ (whistle)
        [InlineData("256,192,1000,1,8,0:0:0:0:", 0x004)]    // カツ (clap)
        [InlineData("256,192,1000,1,10,0:0:0:0:", 0x004)]   // カツ (whistle+clap)
        [InlineData("256,192,1000,1,12,0:0:0:0:", 0x008)]   // 大カツ
        [InlineData("256,192,1000,5,0,0:0:0:0:", 0x001)]    // New Combo 付きのドン
        [InlineData("256,192,1000,2,0,L|300:192,1,70", 0x010)]   // スライダー
        [InlineData("256,192,1000,2,4,L|300:192,1,70", 0x020)]   // 大スライダー
        [InlineData("256,192,1000,12,0,2000,0:0:0:0:", 0x040)]   // スピナー
        public void GetBeatmapData_DetectsNoteType(string line, int expectedCode)
        {
            // 1-23
            var builder = new OsuFileBuilder { HitObjects = [line] };

            var beatmap = Read(builder);

            // 0x7f: ノーツの種類のビットだけを見る (小節線・ブックマークのビットは除く)
            Assert.Equal(expectedCode, Notes(beatmap).Single().hitObjectCode & 0x7f);
        }

        /// <summary>
        /// 小節線とノーツが同じ時刻なら、小節線の印が付く
        /// </summary>
        [Fact]
        public void Note_OnBarline_MarkedAsOnBarline()
        {
            // 3-1・3-2 (L1)
            var builder = new OsuFileBuilder { HitObjects = ["256,192,2000,1,0,0:0:0:0:"], TimingPoints = ["0,500,4,1,0,100,1,0", "2000,500,4,1,0,100,1,0"] };

            var beatmap = Read(builder);

            var note = Notes(beatmap).Single();
            Assert.Equal(0x200, note.hitObjectCode & 0x300);   // 小節線上だけ (小節線が 2 回重なっても桁上がりしない)
        }

        /// <summary>
        /// 先頭のファイル形式の行が無い譜面は、読み込み失敗になる
        /// </summary>
        [Fact]
        public void GetBeatmapData_WithoutFormatVersion_Fails()
        {
            // 1-3 今の動きを固定する
            var lines = new OsuFileBuilder().Build();
            lines.RemoveRange(0, 2);   // "osu file format v14" と空行
            string path = WriteOsuFile("noversion.osu", lines);

            var beatmap = BeatmapHelper.GetBeatmapData(path);

            Assert.Equal(string.Empty, beatmap.version);
        }

        /// <summary>
        /// 小数の値は、小数点がピリオドとして読まれる
        /// </summary>
        [Fact]
        public void GetBeatmapData_DecimalPoint_ReadAsPeriod()
        {
            // 1-12 (Program.cs で InvariantCulture にしている前提。TestEnvironment も同じ設定にしている)
            var builder = new OsuFileBuilder
            {
                TimingPoints = ["0,333.333333333333,4,1,0,100,1,0", "1000,-66.6666666666667,4,1,0,100,0,0"],
                HitObjects = ["256,192,1000,2,0,L|300:192,1,52.5"],
            };

            var beatmap = Read(builder);

            Assert.Equal(180.0, beatmap.timingPoints[0].bpm, 6);
            Assert.Equal(1.5, beatmap.timingPoints[1].sv, 6);
            Assert.Equal(52.5, Notes(beatmap).Single().sliderLength);
        }

        /// <summary>
        /// 別のプログラムが書き込み用に開いていても読める
        /// </summary>
        [Fact]
        public void GetBeatmapData_OpenedForWriting_Reads()
        {
            // 1-13
            string path = WriteOsuFile("shared.osu", new OsuFileBuilder().Build());
            using var writer = new FileStream(path, FileMode.Open, FileAccess.Write, FileShare.ReadWrite);

            var beatmap = BeatmapHelper.GetBeatmapData(path);

            Assert.NotEqual(string.Empty, beatmap.version);
        }

        /// <summary>
        /// 別のプログラムが読み書きを止めていると、読み込み失敗になる
        /// </summary>
        [Fact]
        public void GetBeatmapData_LockedExclusively_Fails()
        {
            // 1-13
            string path = WriteOsuFile("locked.osu", new OsuFileBuilder().Build());
            using var locker = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            var beatmap = BeatmapHelper.GetBeatmapData(path);

            Assert.Equal(string.Empty, beatmap.version);
        }

        /// <summary>
        /// セクションの見出しの後ろに空白があると、見出しとして扱わない
        /// </summary>
        [Fact]
        public void GetBeatmapData_SectionHeaderWithTrailingSpace_NotHeader()
        {
            // 1-15 今の動きを固定する ([Events] の行は前のセクション([Difficulty])の行として読まれる)
            var lines = new OsuFileBuilder().Build();
            int index = lines.IndexOf("[Events]");
            lines[index] = "[Events] ";
            string path = WriteOsuFile("space.osu", lines);

            var beatmap = BeatmapHelper.GetBeatmapData(path);

            Assert.Empty(beatmap.events);
            Assert.Contains("[Events] ", beatmap.difficulty);
        }

        /// <summary>
        /// BG の行が 2 つあると、両方を BG として読む
        /// </summary>
        [Fact]
        public void Events_TwoBackgrounds_BothRead()
        {
            // 1-17
            var builder = new OsuFileBuilder { Events = ["//Background and Video events", "0,0,\"bg1.jpg\",0,0", "0,0,\"bg2.jpg\",0,0"] };

            var beatmap = Read(builder);

            Assert.Equal(2, beatmap.events.Count(e => !e.isComment && !e.isVideo && e.eventCode == Constants.BG_AND_VIDEO_CODE));
        }

        /// <summary>
        /// ノーツに BPM と SV が割り当てられる
        /// </summary>
        [Theory]
        [InlineData(500, 120.0, 1.0, 0)]      // 最初の赤線より前 (今は最初の線の値になる)
        [InlineData(1000, 120.0, 1.0, 1000)]  // 赤線ちょうど
        [InlineData(1500, 120.0, 1.0, 1000)]  // 赤線の後ろ
        [InlineData(2000, 120.0, 2.0, 2000)]  // 緑線ちょうど
        [InlineData(2500, 120.0, 2.0, 2000)]  // 緑線の後ろ
        [InlineData(3000, 240.0, 1.5, 3000)]  // 同じ時刻に赤線と緑線 (緑線の SV)
        [InlineData(4000, 240.0, 0.5, 4000)]  // 同じ時刻に緑線が 2 本 (今は先にある方の SV)
        public void GetBeatmapData_AssignsBpmAndSvToNotes(int time, double expectedBpm, double expectedSv, int expectedSvApplyTime)
        {
            // 1-19・1-24
            var builder = new OsuFileBuilder
            {
                TimingPoints = ["1000,500,4,1,0,100,1,0", "2000,-50,4,1,0,100,0,0", "3000,250,4,1,0,100,1,0", "3000,-66.6666666666667,4,1,0,100,0,0", "4000,-200,4,1,0,100,0,0", "4000,-25,4,1,0,100,0,0"],
                HitObjects = [$"256,192,{time},1,0,0:0:0:0:"],
                Editor = [],
            };

            var beatmap = Read(builder);

            var note = Notes(beatmap).Single();
            Assert.Equal(expectedBpm, note.bpm, 6);
            Assert.Equal(expectedSv, note.sv, 6);
            if (time >= 1000) Assert.Equal(expectedSvApplyTime, note.svApplyTime);
        }

        /// <summary>
        /// 表示用の読み込みは、普通の譜面を読める
        /// </summary>
        [Fact]
        public void TryDecodeBeatmap_ReadsNormalBeatmap()
        {
            // 1-20 (メイン画面の表示に使う OsuParsers)
            string path = WriteOsuFile("parsers.osu", new OsuFileBuilder().Build());

            var beatmap = Common.TryDecodeBeatmap(path);

            Assert.NotNull(beatmap);
            Assert.Equal("title", beatmap.MetadataSection.Title);
            Assert.Equal("Oni", beatmap.MetadataSection.Version);
        }

        /// <summary>
        /// 表示用の読み込みは、開けないファイルなら null を返す
        /// </summary>
        [Fact]
        public void TryDecodeBeatmap_UnreadableFile_ReturnsNull()
        {
            // 1-20 (書き込み中などで開けない場合は再試行して、だめなら null)
            string path = WriteOsuFile("parsers_locked.osu", new OsuFileBuilder().Build());
            using var locker = new FileStream(path, FileMode.Open, FileAccess.ReadWrite, FileShare.None);

            var beatmap = Common.TryDecodeBeatmap(path);

            Assert.Null(beatmap);
        }

        /// <summary>
        /// BG の行を取得できる
        /// </summary>
        [Theory]
        [InlineData(new[] { "//Background and Video events", "0,0,\"bg.jpg\",0,0" }, "0,0,\"bg.jpg\",0,0")]
        [InlineData(new[] { "//Background and Video events", "0,0,\"BG.JPG\",0,0" }, "0,0,\"BG.JPG\",0,0")]               // 拡張子が大文字 (U9)
        [InlineData(new[] { "//Background and Video events", "Video,0,\"v.mp4\"", "0,0,\"bg.png\",0,0" }, "0,0,\"bg.png\",0,0")]   // 動画の行が先
        [InlineData(new[] { "//Background and Video events", "Video,0,\"v.mp4\"" }, "")]                                // BG が無い
        [InlineData(new[] { "0,0,\"bg.png\",0,0", "2,180257,191875" }, "0,0,\"bg.png\",0,0")]                          // 見出しが無い (1-22)
        [InlineData(new[] { "// Background and Video events", "0,0,\"bg.jpg\",0,0" }, "0,0,\"bg.jpg\",0,0")]             // 見出しに空白 (1-22)
        public void GetBackground_ReturnsBackgroundLine(string[] events, string expectedBackground)
        {
            // 1-21・1-22
            var builder = new OsuFileBuilder { Events = [.. events] };
            string path = WriteOsuFile("bg.osu", builder.Build());
            BeatmapMetadata info = new() { beatmapPath = path };

            bool isSuccess = BeatmapHelper.GetBackground(ref info);

            Assert.True(isSuccess);
            Assert.Equal(expectedBackground, info.background);
        }

        /// <summary>
        /// Timing Property 用の読み込みは、古い形式の譜面でも線を読める
        /// </summary>
        [Fact]
        public void GetTimingPointsData_OldFormat_ReadsTimingPoints()
        {
            // 2-2
            var builder = new OsuFileBuilder { HitObjects = ["480,80,5371,1,6"], Events = ["0,0,\"bg.jpg\"", " F,0,0,,0,1"] };
            string path = WriteOsuFile("old.osu", builder.Build());

            var beatmap = BeatmapHelper.GetTimingPointsData(path);

            Assert.NotNull(beatmap);
            Assert.Equal(2, beatmap.timingPoints.Count);
            Assert.Contains("SliderMultiplier:1.4", beatmap.difficulty);
        }
    }
}
