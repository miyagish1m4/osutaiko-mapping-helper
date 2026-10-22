using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Services;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// ユーティリティのテスト (TEST_PATTERNS.md の 7)
    /// </summary>
    /// <remarks>
    /// 7-3・7-22・7-23 (MainForm の中の処理) は MainFormFlowTests、
    /// 7-7 は InputValidationTests (リスナップの範囲)、7-15〜7-21 は ResnapTests にある
    /// </remarks>
    public class UtilityTests : TestEnvironment
    {
        /// <summary>
        /// 譜面を作って読み込み、ユーティリティで書き出して、書き出した行を返す
        /// </summary>
        private List<string> Export(OsuFileBuilder builder, UserInputUtilityData data, BeatmapMetadata? info = null, List<TimingPoint>? timingPoints = null)
        {
            string path = WriteOsuFile("utility.osu", builder.Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            Assert.NotEqual(string.Empty, beatmap.version);
            Assert.True(BeatmapHelper.ExportToOsuFile(beatmap, path, data, info ?? new BeatmapMetadata(), timingPoints));
            return [.. File.ReadAllLines(path)];
        }

        private static List<string> Section(List<string> lines, string header) =>
            [.. lines.SkipWhile(l => l != header).Skip(1).TakeWhile(l => !l.StartsWith('[')).Where(l => l.Trim() != "")];

        private static UserInputUtilityData Offset(int offset) => new() { utilityCode = Constants.UTILITY_OFFSET, offset = offset };

        /// <summary>
        /// オフセットで、線・ノーツ・スピナーの終わり・ブックマーク・休憩・動画がずれる
        /// </summary>
        [Fact]
        public void Offset_ShiftsLinesNotesSpinnerEndBookmarksBreaksAndVideo()
        {
            // 7-1 (PreviewTime は選択中の diff の値 + オフセット。BG の行はずらさない)
            var builder = new OsuFileBuilder
            {
                Editor = ["Bookmarks: 1500,2500"],
                Events = ["//Background and Video events", "0,0,\"bg.jpg\",0,0", "Video,500,\"v.mp4\"", "//Break Periods", "2,4500,6000"],
                TimingPoints = ["0,500,4,1,0,100,1,0", "2000,-50,4,1,0,80,0,0"],
                HitObjects = ["256,192,1000,1,0,0:0:0:0:", "256,192,3000,12,0,4000,0:0:0:0:"],
            };

            var lines = Export(builder, Offset(100), new BeatmapMetadata { previewTime = 5000 });

            Assert.Contains("PreviewTime: 5100", lines);
            Assert.Contains("Bookmarks: 1600,2600", lines);
            Assert.Equal(["0,0,\"bg.jpg\",0,0", "Video,600,\"v.mp4\"", "2,4600,6100"], Section(lines, "[Events]").Where(l => !l.StartsWith("//")));
            Assert.Equal(["100,500,4,1,0,100,1,0", "2100,-50,4,1,0,80,0,0"], Section(lines, "[TimingPoints]"));
            Assert.Equal(["256,192,1100,1,0,0:0:0:0:", "256,192,3100,12,0,4100,0:0:0:0:"], Section(lines, "[HitObjects]"));
        }

        /// <summary>
        /// オフセットは、ブックマークと PreviewTime の書き方によらずずれる
        /// </summary>
        [Theory]
        [InlineData("Bookmarks:1500,2500", "PreviewTime:1000", "Bookmarks: 1600,2600", "PreviewTime: 5100")]   // 後ろに空白なし
        [InlineData("Bookmarks: ", "PreviewTime: 1000", "Bookmarks: ", "PreviewTime: 5100")]                      // ブックマークの値が空
        public void Offset_ShiftsRegardlessOfBookmarkAndPreviewFormat(string bookmarks, string preview, string expectedBookmarks, string expectedPreview)
        {
            // 7-2 (L7)
            var builder = new OsuFileBuilder { Editor = [bookmarks], General = ["AudioFilename: audio.mp3", preview, "Mode: 1"] };

            var lines = Export(builder, Offset(100), new BeatmapMetadata { previewTime = 5000 });

            Assert.Contains(expectedBookmarks, lines);
            Assert.Contains(expectedPreview, lines);
        }

        /// <summary>
        /// オフセットで 0 より前になる時刻は、マイナスのまま書く
        /// </summary>
        [Fact]
        public void Offset_NegativeTime_WrittenAsIs()
        {
            // 7-14 今の動きを固定する
            var builder = new OsuFileBuilder { HitObjects = ["256,192,1000,1,0,0:0:0:0:"], TimingPoints = ["0,500,4,1,0,100,1,0"] };

            var lines = Export(builder, Offset(-2000), new BeatmapMetadata { previewTime = 1000 });

            Assert.Equal(["-2000,500,4,1,0,100,1,0"], Section(lines, "[TimingPoints]"));
            Assert.Equal(["256,192,-1000,1,0,0:0:0:0:"], Section(lines, "[HitObjects]"));
        }

        /// <summary>
        /// オフセットで PreviewTime が -1 なら、-1 のままにする
        /// </summary>
        [Fact]
        public void Offset_PreviewTimeMinusOne_Kept()
        {
            // 7-14
            var lines = Export(new OsuFileBuilder(), Offset(100), new BeatmapMetadata { previewTime = -1 });

            Assert.Contains("PreviewTime: -1", lines);
        }

        /// <summary>
        /// タグ編集では、Tags の行だけを書き換える
        /// </summary>
        [Fact]
        public void TagEdit_RewritesOnlyTagsLine()
        {
            // 7-4 (L7。Source の値に "Tags:" が入っていても Source 行はそのまま)
            var builder = new OsuFileBuilder();
            builder.Metadata = [.. builder.Metadata.Select(l => l.StartsWith("Source:") ? "Source:Tags:y" : l)];

            var lines = Export(builder, new UserInputUtilityData { utilityCode = Constants.UTILITY_TAG_EDIT, tags = "a b" });

            Assert.Contains("Source:Tags:y", lines);
            Assert.Contains("Tags:a b", lines);
            Assert.Single(lines, l => l.StartsWith("Tags:"));
        }

        /// <summary>
        /// 設定コピーのメタデータでは選択中の diff の値を書き、値に Creator を含む Tags の行は書き換えない
        /// </summary>
        [Fact]
        public void SettingCopierMetadata_WritesSelectedValuesAndKeepsTags()
        {
            // 7-4・7-5
            var builder = new OsuFileBuilder();
            builder.Metadata = [.. builder.Metadata.Select(l => l.StartsWith("Tags:") ? "Tags:Creator:foo Title:bar" : l)];
            var info = new BeatmapMetadata { title = "T", titleUnicode = "TU", artist = "A", artistUnicode = "AU", creator = "C", source = "S" };

            var lines = Export(builder, new UserInputUtilityData { utilityCode = Constants.UTILITY_SETTING_COPIER, settingCopierCode = Constants.SETTING_COPIER_METADATA }, info);

            Assert.Equal(["Title:T", "TitleUnicode:TU", "Artist:A", "ArtistUnicode:AU", "Creator:C", "Version:Oni", "Source:S", "Tags:Creator:foo Title:bar"], Section(lines, "[Metadata]"));
        }

        /// <summary>
        /// 設定コピーの BG では、選択中の diff の BG の行を書く
        /// </summary>
        [Fact]
        public void SettingCopierBackground_WritesSelectedBackground()
        {
            // 7-5
            var info = new BeatmapMetadata { background = "0,0,\"other.jpg\",0,40" };

            var lines = Export(new OsuFileBuilder(), new UserInputUtilityData { utilityCode = Constants.UTILITY_SETTING_COPIER, settingCopierCode = Constants.SETTING_COPIER_BG }, info);

            Assert.Contains("0,0,\"other.jpg\",0,40", lines);
            Assert.DoesNotContain("0,0,\"bg.jpg\",0,0", lines);
        }

        /// <summary>
        /// 設定コピーのプレビューでは、選択中の diff の PreviewTime を書く
        /// </summary>
        [Fact]
        public void SettingCopierPreview_WritesSelectedPreviewTime()
        {
            // 7-5
            var lines = Export(new OsuFileBuilder(), new UserInputUtilityData { utilityCode = Constants.UTILITY_SETTING_COPIER, settingCopierCode = Constants.SETTING_COPIER_PREVIEW }, new BeatmapMetadata { previewTime = 7777 });

            Assert.Contains("PreviewTime: 7777", lines);
        }

        /// <summary>
        /// 設定コピーのタイミングでは赤線を置き換え、緑線はそのまま残す
        /// </summary>
        [Fact]
        public void SettingCopierTiming_ReplacesRedLinesAndKeepsGreenLines()
        {
            // 7-5・7-13
            var builder = new OsuFileBuilder { TimingPoints = ["0,500,4,1,0,100,1,0", "2000,-50,4,1,0,80,0,0"] };
            List<TimingPoint> selectedRedLines = [new TimingPoint("100,400,4,1,0,90,1,0")];

            var lines = Export(builder, new UserInputUtilityData { utilityCode = Constants.UTILITY_SETTING_COPIER, settingCopierCode = Constants.SETTING_COPIER_TIMING_POINTS }, null, selectedRedLines);

            Assert.Equal(["100,400,4,1,0,90,1,0", "2000,-50,4,1,0,80,0,0"], Section(lines, "[TimingPoints]"));
        }

        /// <summary>
        /// ヒットサウンドでは、カツだけを指定の音にする
        /// </summary>
        [Theory]
        [InlineData(Constants.HITSOUND_CLAP, new[] { "0", "8", "8", "12", "0", "0" })]
        [InlineData(Constants.HITSOUND_WHISTLE, new[] { "0", "2", "2", "6", "0", "0" })]
        public void Hitsound_ChangesOnlyKats(int hitSoundCode, string[] expected)
        {
            // 7-6・7-8・7-11 (ドン / カツ(whistle) / カツ(whistle+clap) / 大カツ / スライダー / スピナー)
            var builder = new OsuFileBuilder
            {
                HitObjects = ["256,192,1000,1,0,0:0:0:0:", "256,192,1250,1,2,0:0:0:0:", "256,192,1500,1,10,0:0:0:0:", "256,192,1750,1,12,0:0:0:0:", "256,192,2000,2,0,L|300:192,1,70", "256,192,3000,12,0,4000,0:0:0:0:"],
            };

            var lines = Export(builder, new UserInputUtilityData { utilityCode = Constants.UTILITY_HITSOUND, hitSoundCode = hitSoundCode });

            Assert.Equal(expected, Section(lines, "[HitObjects]").Select(l => l.Split(',')[4]));
        }

        /// <summary>
        /// ノーツ位置の入力値 (ドン 64,64 / カツ 448,64 / 大ドン 64,320 / 大カツ 448,320)
        /// </summary>
        private static UserInputUtilityData NotesPosition(int code) => new()
        {
            utilityCode = Constants.UTILITY_NOTES_POSITION,
            notesPositionCode = code,
            donX = 64, donY = 64, katX = 448, katY = 64, finisherDonX = 64, finisherDonY = 320, finisherKatX = 448, finisherKatY = 320,
        };

        private static readonly List<string> PositionNotes =
        [
            "100,100,1000,1,0,0:0:0:0:",       // ドン
            "100,100,1250,1,8,0:0:0:0:",       // カツ
            "100,100,1500,1,4,0:0:0:0:",       // 大ドン
            "100,100,1750,1,12,0:0:0:0:",      // 大カツ
            "100,100,2000,2,0,L|150:100,1,50", // スライダー
            "100,100,3000,12,0,4000,0:0:0:0:", // スピナー
        ];

        /// <summary>
        /// ノーツ位置の中央では全部中央に動き、スライダーは曲線ごと動く
        /// </summary>
        [Fact]
        public void NotesPositionCenter_MovesAllAndSliderCurves()
        {
            // 7-9・7-12 (スピナーは動かさない)
            var lines = Export(new OsuFileBuilder { HitObjects = PositionNotes }, NotesPosition(Constants.NOTES_POSITION_CENTER));

            var hitObjects = Section(lines, "[HitObjects]");
            Assert.All(hitObjects.Take(4), l => Assert.StartsWith("256,192,", l));
            Assert.Equal("256,192,2000,2,0,L|306:192,1,50", hitObjects[4]);
            Assert.StartsWith("100,100,3000,12", hitObjects[5]);
        }

        /// <summary>
        /// ノーツ位置を分けると、ドン・カツ・大ドン・大カツが設定の座標になり、スライダーは中央になる
        /// </summary>
        [Fact]
        public void NotesPositionSeparate_UsesConfiguredPositionsAndCentersSliders()
        {
            // 7-9・7-12
            var lines = Export(new OsuFileBuilder { HitObjects = PositionNotes }, NotesPosition(Constants.NOTES_POSITION_SEPARATE));

            var positions = Section(lines, "[HitObjects]").Select(l => string.Join(",", l.Split(',').Take(2))).ToList();
            Assert.Equal(["64,64", "448,64", "64,320", "448,320", "256,192", "100,100"], positions);
        }

        /// <summary>
        /// ノーツ位置のランダムでは画面の範囲内に動き、スピナーは動かない
        /// </summary>
        [Fact]
        public void NotesPositionRandom_StaysOnScreenAndKeepsSpinners()
        {
            // 7-12
            var lines = Export(new OsuFileBuilder { HitObjects = PositionNotes }, NotesPosition(Constants.NOTES_POSITION_RANDOM));

            var hitObjects = Section(lines, "[HitObjects]");
            foreach (var line in hitObjects.Take(5))
            {
                var parts = line.Split(',');
                Assert.InRange(int.Parse(parts[0]), 0, 511);
                Assert.InRange(int.Parse(parts[1]), 0, 383);
            }
            Assert.StartsWith("100,100,3000,12", hitObjects[5]);
        }

        /// <summary>
        /// リスナップは、指定したスナップの一番近い位置に合う
        /// </summary>
        [Theory]
        [InlineData(12, 1043, 1041)]   // 1/12 (BPM120: 41.67ms)
        [InlineData(16, 1033, 1031)]   // 1/16 (31.25ms)
        public void Resnap_SnapsToNearestPositionOfGivenSnap(int beatSnap, int time, int expected)
        {
            // 7-10
            string path = WriteOsuFile("resnap.osu", new OsuFileBuilder { Editor = [], TimingPoints = ["0,500,4,1,0,100,1,0"], HitObjects = [$"256,192,{time},1,0,0:0:0:0:"] }.Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            var data = new UserInputUtilityData { utilityCode = Constants.UTILITY_RESNAP, beatSnap = beatSnap, isAllNotes = true };

            Assert.True(ResnapService.Apply(data, beatmap));

            Assert.Equal(expected, beatmap.hitObjects.Single(h => h.noteType == Constants.NoteType.CIRCLE).time);
        }
    }
}
