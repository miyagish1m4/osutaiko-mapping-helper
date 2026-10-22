using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils.Helper;
using osu_taiko_Mapping_Helper.Views;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// Timing Property のテスト (TEST_PATTERNS.md の 2)
    /// </summary>
    public class TimingPropertyTests : TestEnvironment
    {
        private static readonly List<string> TimingPoints =
        [
            "1000,500,4,1,0,60,1,0",               // BPM120
            "2000,-50,4,1,0,70,0,0",               // SV2
            "3000,250,4,1,0,80,1,1",               // BPM240
            "3000,-66.6666666666667,4,1,0,90,0,1", // SV1.5
        ];

        /// <summary>
        /// 線と Difficulty だけを読む
        /// </summary>
        [Fact]
        public void GetTimingPointsData_ReadsOnlyTimingPointsAndDifficulty()
        {
            // 2-1
            string path = WriteOsuFile("tp.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());

            var beatmap = BeatmapHelper.GetTimingPointsData(path);

            Assert.NotNull(beatmap);
            Assert.Equal(4, beatmap.timingPoints.Count);
            Assert.Contains("SliderMultiplier:1.4", beatmap.difficulty);
            Assert.Empty(beatmap.metadata);
            Assert.Empty(beatmap.events);
            Assert.Empty(beatmap.hitObjects);
        }

        /// <summary>
        /// 線のセクションが無ければ、空のまま成功する
        /// </summary>
        [Fact]
        public void GetTimingPointsData_NoSection_SucceedsEmpty()
        {
            // 2-3
            var lines = new OsuFileBuilder().Build();
            int start = lines.IndexOf("[TimingPoints]");
            lines.RemoveRange(start, 4);   // 見出し・線 2 本・空行
            string path = WriteOsuFile("notp.osu", lines);

            var beatmap = BeatmapHelper.GetTimingPointsData(path);

            Assert.NotNull(beatmap);
            Assert.Empty(beatmap.timingPoints);
        }

        /// <summary>
        /// 線の行が壊れていれば、null を返す
        /// </summary>
        [Fact]
        public void GetTimingPointsData_BrokenLine_ReturnsNull()
        {
            // 2-4
            string path = WriteOsuFile("broken.osu", new OsuFileBuilder { TimingPoints = ["1000,abc"] }.Build());

            Assert.Null(BeatmapHelper.GetTimingPointsData(path));
        }

        /// <summary>
        /// 表示の値は、ほかの機能の読み込みと同じになる
        /// </summary>
        [Theory]
        [InlineData(999)]
        [InlineData(1000)]
        [InlineData(1001)]
        [InlineData(1999)]
        [InlineData(2000)]
        [InlineData(2500)]
        [InlineData(3000)]
        [InlineData(99999)]
        public void DisplayValues_SameAsFullRead(int time)
        {
            // 2-5
            string path = WriteOsuFile("tp.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());
            var timingOnly = BeatmapHelper.GetTimingPointsData(path);
            var full = BeatmapHelper.GetBeatmapData(path, true);

            Assert.Equal(Debug.SetCurrentBpm(full, time), Debug.SetCurrentBpm(timingOnly, time));
            Assert.Equal(UserInputDataHelper.SetCurrentSv(full, time, true), UserInputDataHelper.SetCurrentSv(timingOnly, time, true));
            Assert.Equal(UserInputDataHelper.SetCurrentVolume(full, time), UserInputDataHelper.SetCurrentVolume(timingOnly, time));
        }

        /// <summary>
        /// 表示の値が、線のとおりになる
        /// </summary>
        [Theory]
        [InlineData(1500, "120", "1", "60")]
        [InlineData(2500, "120", "2", "70")]
        [InlineData(3500, "240", "1.5", "90")]
        public void DisplayValues_MatchTimingPoints(int time, string bpm, string sv, string volume)
        {
            // 2-5
            string path = WriteOsuFile("tp.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());
            var beatmap = BeatmapHelper.GetTimingPointsData(path);

            Assert.Equal(bpm, Debug.SetCurrentBpm(beatmap, time));
            Assert.Equal(sv, UserInputDataHelper.SetCurrentSv(beatmap, time, true));
            Assert.Equal(volume, UserInputDataHelper.SetCurrentVolume(beatmap, time));
        }

        /// <summary>
        /// 最初の線より前は、空欄になる
        /// </summary>
        [Fact]
        public void DisplayValues_BeforeFirstLine_Empty()
        {
            // 2-6
            string path = WriteOsuFile("tp.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());
            var beatmap = BeatmapHelper.GetTimingPointsData(path);

            Assert.Equal(string.Empty, Debug.SetCurrentBpm(beatmap, 500));
            Assert.Equal(string.Empty, UserInputDataHelper.SetCurrentSv(beatmap, 500, true));
            Assert.Equal(string.Empty, UserInputDataHelper.SetCurrentVolume(beatmap, 500));
        }

        /// <summary>
        /// 読み込みのスレッドを動かさずに Timing Property の画面を作る
        /// </summary>
        private static TimingPropertyForm CreateForm()
        {
            var form = TestReflection.CreateWithoutConstructor<TimingPropertyForm>();
            TestReflection.SetField(form, "beatmapLock", new object());
            TestReflection.SetField(form, "beatmapPath", string.Empty);
            TestReflection.SetField(form, "loadedPath", string.Empty);
            return form;
        }

        private static Beatmap? LoadedBeatmap(TimingPropertyForm form) => (Beatmap?)TestReflection.GetField(form, "beatmapData");

        private static void Reload(TimingPropertyForm form) => TestReflection.Invoke(form, "ReloadBeatmapIfChanged");

        /// <summary>
        /// 譜面を書き換えて、更新日時を進める (同じ秒の中で書き換えても更新を見分けられるようにする)
        /// </summary>
        private static void Rewrite(string path, List<string> timingPoints)
        {
            DateTime before = File.GetLastWriteTime(path);
            File.WriteAllText(path, string.Join("\r\n", new OsuFileBuilder { TimingPoints = timingPoints }.Build()));
            File.SetLastWriteTime(path, before.AddSeconds(5));
        }

        /// <summary>
        /// 同じ譜面が更新されたら、読み直す
        /// </summary>
        [Fact]
        public void Reload_SameBeatmapUpdated_Reloads()
        {
            // 2-7 (T2)
            string path = WriteOsuFile("a.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());
            var form = CreateForm();
            form.SetOsuData(path, 0);
            Assert.True(form.GetBeatmap());

            Rewrite(path, ["1000,500,4,1,0,60,1,0"]);
            Reload(form);

            Assert.Single(LoadedBeatmap(form)!.timingPoints);
        }

        /// <summary>
        /// 譜面が切り替わったら、読み直す
        /// </summary>
        [Fact]
        public void Reload_BeatmapChanged_Reloads()
        {
            // 2-7 (T3)
            string pathA = WriteOsuFile("a.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());
            string pathB = WriteOsuFile("b.osu", new OsuFileBuilder { TimingPoints = ["0,300,4,1,0,100,1,0"] }.Build());
            var form = CreateForm();
            form.SetOsuData(pathA, 0);
            form.GetBeatmap();

            form.SetOsuData(pathB, 0);
            Reload(form);

            Assert.Equal(200.0, LoadedBeatmap(form)!.timingPoints.Single().bpm, 6);
        }

        /// <summary>
        /// osu! が終了したら、表示を消す
        /// </summary>
        [Fact]
        public void Reload_OsuClosed_ClearsDisplay()
        {
            // 2-7
            string path = WriteOsuFile("a.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());
            var form = CreateForm();
            form.SetOsuData(path, 0);
            form.GetBeatmap();

            form.SetOsuData(string.Empty, 0);
            Reload(form);

            Assert.Null(LoadedBeatmap(form));
        }

        /// <summary>
        /// 同じ譜面の読み直しに失敗したら、前の表示を残して次の周回で読み直す
        /// </summary>
        [Fact]
        public void Reload_SameBeatmapFails_KeepsPreviousAndRetries()
        {
            // 2-8 (T1)
            string path = WriteOsuFile("a.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());
            var form = CreateForm();
            form.SetOsuData(path, 0);
            form.GetBeatmap();

            Rewrite(path, ["1000,abc"]);   // 読めない
            Reload(form);
            Assert.Equal(4, LoadedBeatmap(form)!.timingPoints.Count);

            // 更新日時が同じでも、前回失敗しているので読み直す
            DateTime brokenTime = File.GetLastWriteTime(path);
            File.WriteAllText(path, string.Join("\r\n", new OsuFileBuilder { TimingPoints = ["1000,500,4,1,0,60,1,0"] }.Build()));
            File.SetLastWriteTime(path, brokenTime);
            Reload(form);
            Assert.Single(LoadedBeatmap(form)!.timingPoints);
        }

        /// <summary>
        /// 別の譜面の読み込みに失敗したら、表示を消す
        /// </summary>
        [Fact]
        public void Reload_OtherBeatmapFails_ClearsDisplay()
        {
            // 2-8
            string pathA = WriteOsuFile("a.osu", new OsuFileBuilder { TimingPoints = TimingPoints }.Build());
            string pathB = WriteOsuFile("b.osu", new OsuFileBuilder { TimingPoints = ["1000,abc"] }.Build());
            var form = CreateForm();
            form.SetOsuData(pathA, 0);
            form.GetBeatmap();

            form.SetOsuData(pathB, 0);
            Reload(form);

            Assert.Null(LoadedBeatmap(form));
        }

        /// <summary>
        /// 本物の Timing Property の画面で、見た目 BPM の表示を読む
        /// </summary>
        /// <remarks>
        /// 見た目 BPM の計算は画面の更新処理(UpdateLabelText)の中にあるため、コンストラクタで動く更新のスレッドをそのまま使う。
        /// 画面は表示せずにハンドルだけを作り、表示が変わるまでメッセージを処理して待つ
        /// </remarks>
        private static string ReadVisualizeBpm(string path, int time, string expected)
        {
            string text = "(読めなかった)";
            TestReflection.RunInSta(() =>
            {
                var form = new TimingPropertyForm();
                try
                {
                    _ = form.Handle;
                    var label = (Label)TestReflection.GetField(form, "lblVisualizeBpm")!;
                    // 待っている間に前の値が残らないよう、期待と違う値にしておく
                    label.Text = expected == string.Empty ? "-" : string.Empty;
                    form.SetOsuData(path, time);
                    var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                    while (label.Text != expected && stopwatch.Elapsed < TimeSpan.FromSeconds(5))
                    {
                        Application.DoEvents();
                        Thread.Sleep(5);
                    }
                    text = label.Text;
                }
                finally
                {
                    TestReflection.SetField(form, "isClose", true);
                    form.Dispose();
                }
            });
            return text;
        }

        /// <summary>
        /// 見た目 BPM は、BPM・SV・SliderMultiplier から計算する
        /// </summary>
        [Theory]
        [InlineData("SliderMultiplier:1.4", 2500, "240")]        // BPM120・SV2
        [InlineData("SliderMultiplier:1.4", 3500, "360")]        // BPM240・SV1.5
        [InlineData("SliderMultiplier:1.8", 2500, "308.57143")]  // 120 × 2 × 1.8 ÷ 1.4 を小数 5 桁に丸める
        [InlineData("SliderMultiplier:2", 1500, "171.42857")]    // 120 × 1 × 2 ÷ 1.4
        [InlineData("SliderMultiplier:1.4", 500, "")]            // 最初の線より前
        public void VisualizeBpm_CalculatedFromBpmSvAndSliderMultiplier(string sliderMultiplier, int time, string expected)
        {
            // 2-9
            var builder = new OsuFileBuilder { TimingPoints = TimingPoints };
            builder.Difficulty = [.. builder.Difficulty.Where(l => !l.StartsWith("SliderMultiplier:")), sliderMultiplier];
            string path = WriteOsuFile("visual.osu", builder.Build());

            Assert.Equal(expected, ReadVisualizeBpm(path, time, expected));
        }

        /// <summary>
        /// SliderMultiplier の行が無ければ、見た目 BPM は空欄になる
        /// </summary>
        [Fact]
        public void VisualizeBpm_WithoutSliderMultiplier_Empty()
        {
            // 2-9
            var builder = new OsuFileBuilder { TimingPoints = TimingPoints };
            builder.Difficulty = [.. builder.Difficulty.Where(l => !l.StartsWith("SliderMultiplier:"))];
            string path = WriteOsuFile("visual.osu", builder.Build());

            Assert.Equal(string.Empty, ReadVisualizeBpm(path, 2500, string.Empty));
        }

        /// <summary>
        /// 現地点の見た目 BPM は、BPM・SV・SliderMultiplier から計算する (SV を BPM で指定するときの Set Start SV・Set End SV)
        /// </summary>
        [Theory]
        [InlineData("SliderMultiplier:1.4", 2500, "240")]
        [InlineData("SliderMultiplier:1.8", 2500, "308.57143")]
        [InlineData("SliderMultiplier:2", 1500, "171.42857")]
        [InlineData("SliderMultiplier:1.4", 3500, "360")]
        [InlineData("SliderMultiplier:1.4", 500, "")]   // 最初の赤線より前
        [InlineData(null, 2500, "240")]                 // SliderMultiplier の行が無ければ 1.4 とする (Timing Property は空欄)
        public void SetCurrentVisualBpm_CalculatesFromBpmSvAndSliderMultiplier(string? sliderMultiplier, int time, string expected)
        {
            // 15-7
            var builder = new OsuFileBuilder { TimingPoints = TimingPoints };
            builder.Difficulty = [.. builder.Difficulty.Where(l => !l.StartsWith("SliderMultiplier:"))];
            if (sliderMultiplier != null) builder.Difficulty.Add(sliderMultiplier);
            string path = WriteOsuFile("visual.osu", builder.Build());

            Assert.Equal(expected, UserInputDataHelper.SetCurrentVisualBpm(BeatmapHelper.GetBeatmapData(path), time));
        }
    }
}
