using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// 小節線・擬似オブジェクトとスナップのテスト (TEST_PATTERNS.md の 3・4)
    /// </summary>
    public class BarlineAndSnapTests : TestEnvironment
    {
        private Beatmap Read(List<string> timingPoints, List<string> hitObjects, List<string>? editor = null)
        {
            var builder = new OsuFileBuilder { TimingPoints = timingPoints, HitObjects = hitObjects, Editor = editor ?? [] };
            string path = WriteOsuFile("barline.osu", builder.Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            Assert.NotEqual(string.Empty, beatmap.version);
            return beatmap;
        }

        private static List<HitObject> At(Beatmap beatmap, int time) => [.. beatmap.hitObjects.Where(h => h.time == time)];

        /// <summary>
        /// ブックマークだけの時刻に、擬似オブジェクトができる
        /// </summary>
        [Fact]
        public void Bookmark_WithoutNote_CreatesPseudoObject()
        {
            // 3-3
            var beatmap = Read(["0,500,4,1,0,100,1,0"], ["256,192,1000,1,0,0:0:0:0:"], ["Bookmarks: 1250"]);

            var bookmark = At(beatmap, 1250).Single();
            Assert.Equal(Constants.NoteType.BOOKMARK, bookmark.noteType);
            Assert.Equal(0x800, bookmark.hitObjectCode & 0xC00);   // ブックマーク上だけ
        }

        /// <summary>
        /// ノーツと同じ時刻のブックマークは、ノーツに印が付く
        /// </summary>
        [Fact]
        public void Bookmark_OnNote_MarksNote()
        {
            // 3-3
            var beatmap = Read(["0,500,4,1,0,100,1,0"], ["256,192,1000,1,0,0:0:0:0:"], ["Bookmarks: 1000"]);

            var note = At(beatmap, 1000).Single();
            Assert.Equal(Constants.NoteType.CIRCLE, note.noteType);
            Assert.Equal(0x800, note.hitObjectCode & 0xC00);
        }

        /// <summary>
        /// 同じ時刻のブックマークが 2 つあっても、印は 1 つ
        /// </summary>
        [Fact]
        public void Bookmark_DuplicatedAtSameTime_MarksOnce()
        {
            // 3-3 (L1)
            var beatmap = Read(["0,500,4,1,0,100,1,0"], ["256,192,1000,1,0,0:0:0:0:"], ["Bookmarks: 1000,1000,1250,1250"]);

            Assert.Equal(0x800, At(beatmap, 1000).Single().hitObjectCode & 0xC00);
            var bookmark = At(beatmap, 1250).Single();
            Assert.Equal(0x800, bookmark.hitObjectCode & 0xC00);
        }

        /// <summary>
        /// ブックマーク以外のノーツには、ブックマーク以外の印が付く
        /// </summary>
        [Fact]
        public void Note_NotOnBookmark_MarkedAsOffBookmark()
        {
            var beatmap = Read(["0,500,4,1,0,100,1,0"], ["256,192,1000,1,0,0:0:0:0:"]);

            Assert.Equal(0x400, At(beatmap, 1000).Single().hitObjectCode & 0xC00);
        }

        /// <summary>
        /// スピナーの終わりに、擬似オブジェクトができる
        /// </summary>
        [Fact]
        public void Spinner_CreatesPseudoObjectAtEnd()
        {
            // 3-4
            var beatmap = Read(["0,500,4,1,0,100,1,0"], ["256,192,1000,12,0,1700,0:0:0:0:"]);

            var spinnerEnd = At(beatmap, 1700).Single();
            Assert.Equal(Constants.NoteType.SPINNER_END, spinnerEnd.noteType);
            Assert.Equal(0x80, spinnerEnd.hitObjectCode);   // 小節線以外・ブックマーク以外の印は付かない
        }

        /// <summary>
        /// 小節線以外のノーツには、小節線以外の印だけが付く
        /// </summary>
        [Fact]
        public void Note_NotOnBarline_MarkedOnlyAsOffBarline()
        {
            // 3-1
            var beatmap = Read(["0,500,4,1,0,100,1,0"], ["256,192,1000,1,0,0:0:0:0:"]);

            Assert.Equal(0x100, At(beatmap, 1000).Single().hitObjectCode & 0x300);
        }

        /// <summary>
        /// 小節線を省略する赤線では、その赤線より前の小節線を作らず、赤線の位置には作る
        /// </summary>
        [Fact]
        public void Barline_OmitFirstBarline_SkipsBeforeAndCreatesAtRedLine()
        {
            // 3-5 (最初の赤線が 3000ms、1 小節 2000ms。省略しなければ 1000ms にも小節線ができる)
            var omitted = Read(["3000,500,4,1,0,100,1,8"], ["256,192,3500,1,0,0:0:0:0:"]);
            var normal = Read(["3000,500,4,1,0,100,1,0"], ["256,192,3500,1,0,0:0:0:0:"]);

            Assert.DoesNotContain(omitted.hitObjects, h => h.time == 1000 && h.noteType == Constants.NoteType.BARLINE);
            Assert.Contains(omitted.hitObjects, h => h.time == 3000 && h.noteType == Constants.NoteType.BARLINE);
            Assert.Contains(normal.hitObjects, h => h.time == 1000 && h.noteType == Constants.NoteType.BARLINE);
        }

        /// <summary>
        /// 最後のノーツより後ろの赤線の位置にも、小節線ができる
        /// </summary>
        [Fact]
        public void Barline_RedLineAfterLastNote_CreatesBarline()
        {
            // 3-7
            var beatmap = Read(["0,500,4,1,0,100,1,0", "5300,500,4,1,0,100,1,0"], ["256,192,1000,1,0,0:0:0:0:"]);

            Assert.Contains(beatmap.hitObjects, h => h.time == 5300 && h.noteType == Constants.NoteType.BARLINE);
        }

        /// <summary>
        /// 赤線が無い譜面は小節線を作らず、ノーツに小節線以外の印が付く
        /// </summary>
        [Fact]
        public void Barline_NoRedLine_MarksNotesAsOffBarline()
        {
            // 3-8
            var beatmap = Read(["0,-100,4,1,0,100,0,0"], ["256,192,1000,1,0,0:0:0:0:"]);

            Assert.DoesNotContain(beatmap.hitObjects, h => h.noteType == Constants.NoteType.BARLINE);
            Assert.Equal(0x100, At(beatmap, 1000).Single().hitObjectCode & 0x300);
        }

        /// <summary>
        /// ノーツが無い譜面は、小節線を作らない
        /// </summary>
        [Fact]
        public void Barline_NoNotes_CreatesNone()
        {
            // 3-8
            var beatmap = Read(["0,500,4,1,0,100,1,0"], []);

            Assert.Empty(beatmap.hitObjects);
        }

        /// <summary>
        /// ノーツのスナップと、正確な時刻を求める
        /// </summary>
        [Theory]
        [InlineData(1000, 16, 1000.0)]          // 1/1
        [InlineData(1062, 16, 1062.5)]          // 1/8
        [InlineData(1031, 16, 1031.25)]         // 1/16
        [InlineData(1166, 12, 1166.666667)]     // 1/3
        [InlineData(1083, 12, 1083.333333)]     // 1/6
        [InlineData(1041, 12, 1041.666667)]     // 1/12
        [InlineData(1020, 16, 1020.0)]          // どちらでもない → 1/16 扱いで時刻そのまま
        public void GetSnapsOnHitObjects_CalculatesSnapAndRawTime(int time, int expectedSnap, double expectedRawTime)
        {
            // 4-1 (BPM120: 1/16 = 31.25ms、1/12 = 41.67ms)
            var beatmap = Read(["0,500,4,1,0,100,1,0"], [$"256,192,{time},1,0,0:0:0:0:"]);

            Assert.True(BeatmapHelper.GetSnapsOnHitObjects(ref beatmap));

            var note = beatmap.hitObjects.Single(h => h.noteType == Constants.NoteType.CIRCLE);
            Assert.Equal(expectedSnap, note.snap);
            Assert.Equal(expectedRawTime, note.rawTime, 5);
        }

        /// <summary>
        /// 赤線をまたぐノーツは、次の赤線から数え直す
        /// </summary>
        [Theory]
        [InlineData(2020, 2020.833333)]   // 次の赤線 (BPM180、1/16 = 20.83ms) から数える
        [InlineData(1999, 2000.0)]        // 次の赤線の 2ms 以内は次の赤線で数える
        public void GetSnapsOnHitObjects_AcrossRedLine_CountsFromNextRedLine(int time, double expectedRawTime)
        {
            // 4-2
            var beatmap = Read(["0,500,4,1,0,100,1,0", "2000,333.333333333333,4,1,0,100,1,0"], ["256,192,1000,1,0,0:0:0:0:", $"256,192,{time},1,0,0:0:0:0:"]);

            Assert.True(BeatmapHelper.GetSnapsOnHitObjects(ref beatmap));

            var note = beatmap.hitObjects.Single(h => h.noteType == Constants.NoteType.CIRCLE && h.time == time);
            Assert.Equal(expectedRawTime, note.rawTime, 5);
        }

        /// <summary>
        /// 正確な時刻を、BPM の単位 60000 で求める
        /// </summary>
        [Fact]
        public void GetRawTiming_UsesOneMinuteInMilliseconds()
        {
            // 4-4 (S3。BPM120 の 1/3 は 166.67ms)
            List<TimingPoint> timingPoints = [new TimingPoint("0,500,4,1,0,100,1,0")];

            double rawTime = BeatmapHelper.GetRawTiming(timingPoints, 166);

            Assert.Equal(166.666667, rawTime, 5);
        }
    }
}
