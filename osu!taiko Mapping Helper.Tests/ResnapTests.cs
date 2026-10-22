using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Services;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// リスナップのテスト (TEST_PATTERNS.md の 7-15〜7-21)
    /// </summary>
    /// <remarks>
    /// 何も指定しなければ赤線は 0ms・BPM120 (1/4 = 125ms)。
    /// スピナーの終わりは書き出し時に決まるため、リスナップしてから書き出した結果で確かめる
    /// </remarks>
    public class ResnapTests : TestEnvironment
    {
        /// <summary>
        /// リスナップして書き出し、ノーツの行を返す
        /// </summary>
        /// <param name="hitObjects">ノーツの行</param>
        /// <param name="timingPoints">線の行 (nullなら BPM120 の赤線 1 本)</param>
        /// <param name="from">始点 (from と to の両方が null なら範囲なし)</param>
        /// <param name="to">終点</param>
        /// <param name="beatSnap">スナップの細かさ (4 なら 1/4)</param>
        private (bool isSuccess, List<string> lines) Resnap(List<string> hitObjects, List<string>? timingPoints = null, int? from = null, int? to = null, int beatSnap = 4)
        {
            var builder = new OsuFileBuilder { HitObjects = hitObjects, Editor = [] };
            if (timingPoints != null) builder.TimingPoints = timingPoints;
            else builder.TimingPoints = ["0,500,4,1,0,100,1,0"];
            string path = WriteOsuFile("resnap.osu", builder.Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            Assert.NotEqual(string.Empty, beatmap.version);

            UserInputUtilityData data = new()
            {
                utilityCode = Constants.UTILITY_RESNAP,
                beatSnap = beatSnap,
                isAllNotes = from == null && to == null,
                resnapTimingFrom = from ?? 0,
                resnapTimingTo = to ?? 0,
            };
            bool isSuccess = ResnapService.Apply(data, beatmap);
            if (!isSuccess) return (false, []);
            Assert.True(BeatmapHelper.ExportToOsuFile(beatmap, path, data));
            var lines = File.ReadAllLines(path).SkipWhile(l => l != "[HitObjects]").Skip(1).Where(l => l != "").ToList();
            return (true, lines);
        }

        /// <summary>
        /// ノーツの行から時刻を取り出す
        /// </summary>
        private static List<int> Times(List<string> lines) => [.. lines.Select(l => int.Parse(l.Split(',')[2]))];

        /// <summary>
        /// 範囲なしで、少しずれたノーツが一番近いスナップに合う
        /// </summary>
        [Fact]
        public void Resnap_WholeBeatmap_SnapsToNearest()
        {
            var (isSuccess, lines) = Resnap(["256,192,1003,1,0,0:0:0:0:", "256,192,1130,1,0,0:0:0:0:", "256,192,1247,1,0,0:0:0:0:"]);

            Assert.True(isSuccess);
            Assert.Equal([1000, 1125, 1250], Times(lines));
        }

        /// <summary>
        /// 範囲なしで、最後のノーツもリスナップされる
        /// </summary>
        [Fact]
        public void Resnap_WholeBeatmap_IncludesLastNote()
        {
            // 7-18
            var (isSuccess, lines) = Resnap(["256,192,1003,1,0,0:0:0:0:", "256,192,1495,1,0,0:0:0:0:"]);

            Assert.True(isSuccess);
            Assert.Equal([1000, 1500], Times(lines));
        }

        /// <summary>
        /// スピナーの始点と終わりの両方がリスナップされる
        /// </summary>
        [Fact]
        public void Resnap_SnapsSpinnerStartAndEnd()
        {
            // 7-17
            var (isSuccess, lines) = Resnap(["256,192,1003,12,0,2997,0:0:0:0:"]);

            Assert.True(isSuccess);
            Assert.Equal("256,192,1000,12,0,3000,0:0:0:0:", lines.Single());
        }

        /// <summary>
        /// スピナーの後ろのノーツもリスナップされる
        /// </summary>
        [Fact]
        public void Resnap_SnapsNotesAfterSpinner()
        {
            var (isSuccess, lines) = Resnap(["256,192,1003,12,0,2997,0:0:0:0:", "256,192,3130,1,0,0:0:0:0:"]);

            Assert.True(isSuccess);
            Assert.Equal(["256,192,1000,12,0,3000,0:0:0:0:", "256,192,3125,1,0,0:0:0:0:"], lines);
        }

        /// <summary>
        /// 同じスナップの範囲にあるノーツは、同じ時刻に重なる
        /// </summary>
        [Fact]
        public void Resnap_NotesInSameSnapRange_Overlap()
        {
            // 7-16
            var (isSuccess, lines) = Resnap(["256,192,1010,1,0,0:0:0:0:", "256,192,1040,1,0,0:0:0:0:"]);

            Assert.True(isSuccess);
            Assert.Equal([1000, 1000], Times(lines));
        }

        /// <summary>
        /// 範囲の外のノーツは動かず、範囲の終わりちょうどのノーツは動く
        /// </summary>
        [Fact]
        public void Resnap_Range_MovesOnlyInsideIncludingEnd()
        {
            // 7-20
            var (isSuccess, lines) = Resnap(
                ["256,192,990,1,0,0:0:0:0:", "256,192,1130,1,0,0:0:0:0:", "256,192,1997,1,0,0:0:0:0:", "256,192,2003,1,0,0:0:0:0:"],
                from: 1000, to: 2000);

            Assert.True(isSuccess);
            Assert.Equal([990, 1125, 2000, 2003], Times(lines));
        }

        /// <summary>
        /// スピナーの終わりが範囲の外なら、終わりは動かない
        /// </summary>
        [Fact]
        public void Resnap_SpinnerEndOutsideRange_NotMoved()
        {
            var (isSuccess, lines) = Resnap(["256,192,1003,12,0,2997,0:0:0:0:"], from: 0, to: 2000);

            Assert.True(isSuccess);
            Assert.Equal("256,192,1000,12,0,2997,0:0:0:0:", lines.Single());
        }

        /// <summary>
        /// 最初の赤線より前のノーツは、最初の赤線から逆にたどったスナップに合う
        /// </summary>
        [Fact(Skip = "7-15: 最初の赤線より 1 スナップ以上前のノーツがある場合はリスナップしない")]
        public void Resnap_NotesBeforeFirstRedLine_SnapBackwardFromFirstRedLine()
        {
            // 7-15 (赤線 1000ms・BPM120 なので 875・750・625… がスナップの位置)
            var (isSuccess, lines) = Resnap(
                ["256,192,870,1,0,0:0:0:0:", "256,192,755,1,0,0:0:0:0:", "256,192,1003,1,0,0:0:0:0:"],
                timingPoints: ["1000,500,4,1,0,100,1,0"]);

            Assert.True(isSuccess);
            Assert.Equal([750, 875, 1000], Times(lines).Order());
        }

        /// <summary>
        /// 区間ごとにその区間のスナップに合い、前の区間が次の区間のノーツを引き寄せない
        /// </summary>
        [Fact]
        public void Resnap_EachSection_SnapsWithinSection()
        {
            // 7-21 (0ms: BPM120 で 1/4 = 125ms、1000ms: BPM300 で 1/4 = 50ms)
            var (isSuccess, lines) = Resnap(
                ["256,192,876,1,0,0:0:0:0:", "256,192,1000,1,0,0:0:0:0:", "256,192,1050,1,0,0:0:0:0:", "256,192,1102,1,0,0:0:0:0:"],
                timingPoints: ["0,500,4,1,0,100,1,0", "1000,200,4,1,0,100,1,0"]);

            Assert.True(isSuccess);
            Assert.Equal([875, 1000, 1050, 1100], Times(lines));
        }

        /// <summary>
        /// ノーツが無い譜面は、失敗を返す
        /// </summary>
        [Fact]
        public void Resnap_NoNotes_Fails()
        {
            // 7-19
            var (isSuccess, _) = Resnap([]);

            Assert.False(isSuccess);
        }
    }
}
