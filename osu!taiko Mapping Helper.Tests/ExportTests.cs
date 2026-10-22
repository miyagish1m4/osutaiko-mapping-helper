using System.Globalization;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Services;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// 書き出しのテスト (TEST_PATTERNS.md の 6)
    /// </summary>
    public class ExportTests : TestEnvironment
    {
        /// <summary>
        /// 譜面を作って読み込み、何も変えずに書き出して、書き出した行を返す
        /// </summary>
        private List<string> RoundTrip(OsuFileBuilder builder, string fileName = "export.osu")
        {
            string path = WriteOsuFile(fileName, builder.Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            Assert.NotEqual(string.Empty, beatmap.version);
            Assert.True(BeatmapHelper.ExportToOsuFile(beatmap, path));
            return [.. File.ReadAllLines(path)];
        }

        private static List<string> Section(List<string> lines, string header) =>
            [.. lines.SkipWhile(l => l != header).Skip(1).TakeWhile(l => !l.StartsWith('[')).Where(l => l.Trim() != "")];

        /// <summary>
        /// 何も変えずに書き出すと、線・ノーツ・Events が元と同じになる
        /// </summary>
        [Fact]
        public void Export_Unchanged_KeepsTimingPointsHitObjectsAndEvents()
        {
            // 6-1 (線の値は小数の誤差の範囲で同じ)
            var builder = new OsuFileBuilder
            {
                TimingPoints = ["0,333.333333333333,4,1,0,100,1,0", "1000,-83.3333333333335,4,2,1,70,0,1"],
                HitObjects = ["256,192,1000,5,0,0:0:0:0:", "256,192,1500,1,8,1:2:0:0:", "256,192,2000,2,0,L|300:192,1,70,2|0,0:0|0:0,0:0:0:0:", "256,192,3000,12,0,4000,0:0:0:0:"],
            };
            var original = builder.Build();

            var exported = RoundTrip(builder);

            Assert.Equal(Section(original, "[HitObjects]"), Section(exported, "[HitObjects]"));
            Assert.Equal(Section(original, "[Events]"), Section(exported, "[Events]"));
            var originalLines = Section(original, "[TimingPoints]");
            var exportedLines = Section(exported, "[TimingPoints]");
            Assert.Equal(originalLines.Count, exportedLines.Count);
            for (int i = 0; i < originalLines.Count; i++)
            {
                var a = originalLines[i].Split(',');
                var b = exportedLines[i].Split(',');
                Assert.Equal(double.Parse(a[1], CultureInfo.InvariantCulture), double.Parse(b[1], CultureInfo.InvariantCulture), 9);
                Assert.Equal(a.Where((_, k) => k != 1), b.Where((_, k) => k != 1));
            }
        }

        /// <summary>
        /// BOM を付けずに書き出す
        /// </summary>
        [Fact]
        public void Export_WritesWithoutBom()
        {
            // 6-2 (H1)
            string path = WriteOsuFile("bom.osu", new OsuFileBuilder().Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);

            Assert.True(BeatmapHelper.ExportToOsuFile(beatmap, path));

            var bytes = File.ReadAllBytes(path);
            Assert.False(bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF);
        }

        /// <summary>
        /// 書き出しの途中で失敗しても、元のファイルは変わらない
        /// </summary>
        [Fact]
        public void Export_FailureMidway_KeepsOriginalFile()
        {
            // 6-4 (H1。temp に書いてから置き換える)
            string path = WriteOsuFile("fail.osu", new OsuFileBuilder().Build());
            byte[] before = File.ReadAllBytes(path);
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            beatmap.hitObjects.Add(null!);   // ノーツの行を作るところで失敗させる

            Assert.False(BeatmapHelper.ExportToOsuFile(beatmap, path));

            Assert.Equal(before, File.ReadAllBytes(path));
            Assert.Empty(Directory.GetFiles(Path.Combine(WorkDirectory, "temp")));
        }

        /// <summary>
        /// タグ編集のユーティリティの入力値
        /// </summary>
        private static UserInputUtilityData TagEdit(string tags) => new() { utilityCode = Constants.UTILITY_TAG_EDIT, tags = tags };

        /// <summary>
        /// 全 diff の書き出しでは、taiko 以外の diff を書き換えない
        /// </summary>
        [Fact]
        public void ExportToAllOsuFiles_SkipsNonTaikoDiffs()
        {
            // 6-5
            string taiko = WriteOsuFile("taiko.osu", new OsuFileBuilder().Build());
            var std = new OsuFileBuilder();
            std.General = ["AudioFilename: audio.mp3", "Mode: 0"];
            string standard = WriteOsuFile("std.osu", std.Build());
            byte[] stdBefore = File.ReadAllBytes(standard);

            int result = BeatmapHelper.ExportToAllOsuFiles([taiko, standard], TagEdit("new"), new BeatmapMetadata());

            Assert.Equal(0, result);
            Assert.Contains("Tags:new", File.ReadAllLines(taiko));
            Assert.Equal(stdBefore, File.ReadAllBytes(standard));
        }

        /// <summary>
        /// 全 diff の書き出しで読めない taiko の diff があれば、何も書き換えない
        /// </summary>
        [Fact]
        public void ExportToAllOsuFiles_UnreadableTaikoDiff_ChangesNothing()
        {
            // 6-6 (H1)
            string good = WriteOsuFile("good.osu", new OsuFileBuilder().Build());
            string bad = WriteOsuFile("bad.osu", new OsuFileBuilder { HitObjects = ["480,80,5371,1,6"] }.Build());
            byte[] goodBefore = File.ReadAllBytes(good);

            int result = BeatmapHelper.ExportToAllOsuFiles([good, bad], TagEdit("new"), new BeatmapMetadata());

            Assert.Equal(1, result);
            Assert.Equal(goodBefore, File.ReadAllBytes(good));
        }

        /// <summary>
        /// 全 diff の書き出しの途中で 1 つ失敗すると、それより前の diff は書き換わったままになる
        /// </summary>
        [Fact]
        public void ExportToAllOsuFiles_FailureMidway_KeepsEarlierDiffsChanged()
        {
            // 6-13 今の動きを固定する (対応しない範囲)
            string first = WriteOsuFile("first.osu", new OsuFileBuilder().Build());
            string second = WriteOsuFile("second.osu", new OsuFileBuilder().Build());
            File.SetAttributes(second, FileAttributes.ReadOnly);
            try
            {
                int result = BeatmapHelper.ExportToAllOsuFiles([first, second], TagEdit("new"), new BeatmapMetadata());

                Assert.Equal(2, result);
                Assert.Contains("Tags:new", File.ReadAllLines(first));
                Assert.Contains("Tags:tag", File.ReadAllLines(second));
            }
            finally
            {
                File.SetAttributes(second, FileAttributes.Normal);
            }
        }

        /// <summary>
        /// 小数点はピリオドで書き出す
        /// </summary>
        [Fact]
        public void Export_WritesPeriodAsDecimalPoint()
        {
            // 6-7 (H3。Program.cs で InvariantCulture にしている前提。TestEnvironment も同じ設定にしている)
            var exported = RoundTrip(new OsuFileBuilder
            {
                TimingPoints = ["0,333.333333333333,4,1,0,100,1,0"],
                HitObjects = ["256,192,1000,2,0,L|300:192,1,52.5"],
            });

            Assert.Contains("0,333.333333333333,4,1,0,100,1,0", exported);
            Assert.Contains("256,192,1000,2,0,L|300:192,1,52.5", exported);
        }

        /// <summary>
        /// 同じ時刻の赤線と緑線は、赤線が先に並ぶ
        /// </summary>
        [Fact]
        public void Export_SameTimeRedAndGreenLines_RedLineFirst()
        {
            // 6-8
            var exported = RoundTrip(new OsuFileBuilder { TimingPoints = ["1000,-50,4,1,0,100,0,0", "1000,500,4,1,0,100,1,0"] });

            Assert.Equal(["1000,500,4,1,0,100,1,0", "1000,-50,4,1,0,100,0,0"], Section(exported, "[TimingPoints]"));
        }

        /// <summary>
        /// 削除した線は書き出さない
        /// </summary>
        [Fact]
        public void Export_AfterRemove_DoesNotWriteRemovedLines()
        {
            // 6-9 (線の削除は SV 計算の中で、書き出しの前に行う。書き出しは削除の印を見ない)
            string path = WriteOsuFile("delete.osu", new OsuFileBuilder().Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            UserInputData remove = new() { timingFrom = 1000, timingTo = 3000 };
            remove.setObjectOption.isTimingStart = true;
            remove.setObjectOption.isTimingEnd = true;
            Assert.True(SVCalculatorService.Remove(remove, beatmap));

            Assert.True(BeatmapHelper.ExportToOsuFile(beatmap, path));

            Assert.Equal(["0,500,4,1,0,100,1,0"], Section([.. File.ReadAllLines(path)], "[TimingPoints]"));
        }

        /// <summary>
        /// スライダーの長さは、末尾の 0 と小数点を消して書く
        /// </summary>
        [Theory]
        [InlineData("52.5", "52.5")]
        [InlineData("70", "70")]
        [InlineData("70.0", "70")]
        [InlineData("123.4567890123", "123.4567890123")]
        public void Export_SliderLength_TrimsTrailingZerosAndPoint(string length, string expected)
        {
            // 6-10
            var exported = RoundTrip(new OsuFileBuilder { HitObjects = [$"256,192,1000,2,0,L|300:192,1,{length}"] });

            Assert.Equal($"256,192,1000,2,0,L|300:192,1,{expected}", Section(exported, "[HitObjects]").Single());
        }

        /// <summary>
        /// スライダーの行は、元と同じ形で書く
        /// </summary>
        [Theory]
        [InlineData("256,192,1000,2,0,L|300:192,1,70")]
        [InlineData("256,192,1000,2,0,L|300:192,1,70,2|0,0:0|0:0,0:0:0:0:")]
        public void Export_SliderLine_KeepsOriginalFormat(string line)
        {
            // 6-11
            var exported = RoundTrip(new OsuFileBuilder { HitObjects = [line] });

            Assert.Equal(line, Section(exported, "[HitObjects]").Single());
        }

        /// <summary>
        /// Colours が無い譜面には、Colours を書かない
        /// </summary>
        [Fact]
        public void Export_WithoutColours_DoesNotWriteColours()
        {
            // 6-12
            var without = RoundTrip(new OsuFileBuilder());
            var with = RoundTrip(new OsuFileBuilder { Colours = ["Combo1 : 255,0,0"] }, "colours.osu");

            Assert.DoesNotContain("[Colours]", without);
            Assert.Contains("[Colours]", with);
            Assert.Contains("Combo1 : 255,0,0", with);
        }

        /// <summary>
        /// ノーツの type の値は、元のまま書く
        /// </summary>
        [Theory]
        [InlineData("256,192,1000,5,0,0:0:0:0:")]     // New Combo
        [InlineData("256,192,1000,21,0,0:0:0:0:")]    // New Combo + コンボ色のスキップ
        [InlineData("256,192,1000,12,0,2000,0:0:0:0:")]
        public void Export_KeepsHitObjectType(string line)
        {
            // 6-14
            var exported = RoundTrip(new OsuFileBuilder { HitObjects = [line] });

            Assert.Equal(line, Section(exported, "[HitObjects]").Single());
        }

        /// <summary>
        /// ファイル形式の行は、元のまま書く
        /// </summary>
        [Theory]
        [InlineData("osu file format v14")]
        [InlineData("osu file format v128")]
        public void Export_KeepsFormatVersionLine(string version)
        {
            // 6-15
            var exported = RoundTrip(new OsuFileBuilder { Version = version });

            Assert.Equal(version, exported[0]);
        }

        /// <summary>
        /// 書き出した譜面を、もう一度読み込める
        /// </summary>
        [Fact]
        public void Export_ResultCanBeReadAgain()
        {
            // 6-16
            string path = WriteOsuFile("reload.osu", new OsuFileBuilder().Build());
            var first = BeatmapHelper.GetBeatmapData(path);
            Assert.True(BeatmapHelper.ExportToOsuFile(first, path));

            var second = BeatmapHelper.GetBeatmapData(path);

            Assert.NotEqual(string.Empty, second.version);
            Assert.Equal(first.timingPoints.Count, second.timingPoints.Count);
            Assert.Equal(first.hitObjects.Select(h => (h.time, h.noteType)), second.hitObjects.Select(h => (h.time, h.noteType)));
            Assert.Equal(first.bookmarks.Select(b => b.time), second.bookmarks.Select(b => b.time));
        }

        /// <summary>
        /// 書き出し先が読み取り専用なら失敗し、temp ファイルを残さない
        /// </summary>
        [Fact]
        public void Export_ReadOnlyTarget_FailsWithoutLeavingTemporaryFile()
        {
            // 6-17
            string path = WriteOsuFile("readonly.osu", new OsuFileBuilder().Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            File.SetAttributes(path, FileAttributes.ReadOnly);
            try
            {
                Assert.False(BeatmapHelper.ExportToOsuFile(beatmap, path));

                Assert.Empty(Directory.GetFiles(Path.Combine(WorkDirectory, "temp")));
            }
            finally
            {
                File.SetAttributes(path, FileAttributes.Normal);
            }
        }

        /// <summary>
        /// 同じ譜面の temp ファイルは、毎回別の名前になる
        /// </summary>
        [Fact]
        public void GetTemporaryBeatmapPath_ReturnsUniqueName()
        {
            // 6-18・14-10
            string path = Path.Combine(WorkDirectory, "same.osu");

            var first = (string)TestReflection.InvokeStatic(typeof(BeatmapHelper), "GetTemporaryBeatmapPath", path)!;
            var second = (string)TestReflection.InvokeStatic(typeof(BeatmapHelper), "GetTemporaryBeatmapPath", path)!;

            Assert.NotEqual(first, second);
        }
    }
}
