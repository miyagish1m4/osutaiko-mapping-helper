using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Services;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// SV計算の適用のテスト (TEST_PATTERNS.md の 5)
    /// </summary>
    /// <remarks>
    /// 5-24・5-29・5-42 は期待結果が決まっていないため、今の動き(コードから読んだもの)を固定している<br/>
    /// 基本の譜面: 赤線 0ms (BPM120・音量50)、緑線なし、ブックマーク 1500・2250、
    /// ノーツ 1000 ドン / 1500 カツ / 2000 ドン(小節線上) / 2500 大ドン / 3000 スライダー / 3500〜3800 スピナー
    /// </remarks>
    public class SvApplyTests : TestEnvironment
    {
        private static OsuFileBuilder BaseMap() => new()
        {
            Editor = ["Bookmarks: 1500,2250"],
            TimingPoints = ["0,500,4,1,0,50,1,0"],
            HitObjects =
            [
                "256,192,1000,1,0,0:0:0:0:",
                "256,192,1500,1,8,0:0:0:0:",
                "256,192,2000,1,0,0:0:0:0:",
                "256,192,2500,1,4,0:0:0:0:",
                "256,192,3000,2,0,L|300:192,1,70",
                "256,192,3500,12,0,3800,0:0:0:0:",
            ],
        };

        /// <summary>
        /// 入力値 (オブジェクトに置く・全てのヒットオブジェクト・SV 2.0 で一定・音量は変えない・始点と終点を含む)
        /// </summary>
        private static UserInputData Input(int from, int to, double svFrom = 2.0, double? svTo = null)
        {
            UserInputData data = new()
            {
                timingFrom = from,
                timingTo = to,
                applySetCode = 0x1,
                isSv = true,
                svFrom = svFrom,
                svTo = svTo ?? svFrom,
                isVolume = false,
                calculationCode = Constants.CALCULATION_ARITHMETIC,
                relativeCode = Constants.RELATIVE_DISABLE,
            };
            data.setObjectOption.setObjectsCode = 0x37f;
            data.setObjectOption.isTimingStart = true;
            data.setObjectOption.isTimingEnd = true;
            return data;
        }

        /// <summary>
        /// 譜面を読み込んで適用し、MainForm と同じく元の線と追加した線をまとめた譜面を返す
        /// </summary>
        private (bool isSuccess, Beatmap beatmap, List<TimingPoint> added) Apply(UserInputData data, OsuFileBuilder? builder = null)
        {
            string path = WriteOsuFile("apply.osu", (builder ?? BaseMap()).Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            Assert.NotEqual(string.Empty, beatmap.version);
            List<TimingPoint> added = [];
            bool isSuccess = SVCalculatorService.Apply(data, beatmap, ref added);
            beatmap.timingPoints = [.. beatmap.timingPoints.Concat(added).OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0)];
            return (isSuccess, beatmap, added);
        }

        /// <summary>
        /// 緑線を「時刻(SV,音量)」の形にする (kiai の線は「,kiai」を付ける)
        /// </summary>
        private static List<string> GreenLines(Beatmap beatmap) =>
            [.. beatmap.timingPoints.Where(tp => !tp.isRedLine)
                                    .Select(tp => $"{tp.time}({Math.Round(tp.sv, 3)},{tp.volume}{((tp.effect & 1) == 1 ? ",kiai" : "")})")];

        // 5-1 オブジェクトに置く × 対象の種類
        /// <summary>
        /// 全てのヒットオブジェクトでは、ブックマークだけの位置も含めて範囲内の全部に置く
        /// </summary>
        [Fact]
        public void ApplyOnObjects_AllHitObjects_PlacesOnAllInRangeIncludingBookmarks()
        {
            var (isSuccess, beatmap, _) = Apply(Input(1000, 3500));

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50)", "2000(2,50)", "2250(2,50)", "2500(2,50)", "3000(2,50)", "3500(2,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// カツだけを選ぶとカツに置き、前後には元の SV に戻す線を置く
        /// </summary>
        [Fact]
        public void ApplyOnObjects_OnlyKat_PlacesOnKatAndRestoresAround()
        {
            var data = Input(1000, 3500);
            data.setObjectOption.setObjectsCode = 0x4;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(["1000(1,50)", "1500(2,50)", "2000(1,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 小節線上を選ぶと、小節線上のノーツだけに置く
        /// </summary>
        [Fact]
        public void ApplyOnObjects_OnBarline_PlacesOnlyOnBarlineNotes()
        {
            var data = Input(1000, 3500);
            data.setObjectOption.setObjectsCode = 0x200;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(["1000(1,50)", "2000(2,50)", "2250(1,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 小節線以外を選ぶと、小節線上のノーツには置かない
        /// </summary>
        [Fact]
        public void ApplyOnObjects_OffBarline_SkipsBarlineNotes()
        {
            var data = Input(1000, 3500);
            data.setObjectOption.setObjectsCode = 0x100;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50)", "2000(1,50)", "2250(2,50)", "2500(2,50)", "3000(2,50)", "3500(2,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// ブックマーク上を選ぶと、ブックマークの位置だけに置く
        /// </summary>
        [Fact]
        public void ApplyOnObjects_OnBookmark_PlacesOnlyOnBookmarks()
        {
            var data = Input(1000, 3500);
            data.setObjectOption.setObjectsCode = 0x800;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(["1000(1,50)", "1500(2,50)", "2000(1,50)", "2250(2,50)", "2500(1,50)"], GreenLines(beatmap));
        }

        // 5-2・5-38 ビートスナップ間隔に置く
        /// <summary>
        /// ビートスナップ間隔に置く
        /// </summary>
        [Theory]
        [InlineData(true, true, new[] { 1000, 1500, 2000, 2500, 3000 })]
        [InlineData(false, true, new[] { 1500, 2000, 2500, 3000 })]
        [InlineData(true, false, new[] { 1000, 1500, 2000, 2500 })]
        public void ApplyOnBeatSnaps_PlacesAtInterval(bool isTimingStart, bool isTimingEnd, int[] expectedTimes)
        {
            var data = Input(1000, 3000);
            data.applySetCode = 0x2;
            data.setBeatSnapOption.beatSnap = 1;   // BPM120 の 1/1 = 500ms
            data.setObjectOption.isTimingStart = isTimingStart;
            data.setObjectOption.isTimingEnd = isTimingEnd;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(expectedTimes, beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.time));
            Assert.All(beatmap.timingPoints.Where(tp => !tp.isRedLine), tp => Assert.Equal(2.0, tp.sv, 6));
        }

        /// <summary>
        /// ビートスナップ間隔の範囲の中に赤線があると、赤線から数え直して BPM で補正する
        /// </summary>
        [Fact]
        public void ApplyOnBeatSnaps_RedLineInRange_RecountsAndAdjustsBpm()
        {
            // 5-25 (2000ms から BPM240、1/1 = 250ms。SV は 120/240 倍)
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "2000,250,4,1,0,50,1,0"];
            var data = Input(1000, 3000);
            data.applySetCode = 0x2;
            data.setBeatSnapOption.beatSnap = 1;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50)", "2000(1,50)", "2250(1,50)", "2500(1,50)", "2750(1,50)", "3000(1,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 緑線を変えると、範囲内の緑線だけが変わる
        /// </summary>
        [Fact]
        public void ApplyGreenLines_ChangesOnlyGreenLinesInRange()
        {
            // 5-3
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "500,-66.6666666666667,4,1,0,50,0,0", "1500,-125,4,1,0,50,0,0", "2500,-83.3333333333333,4,1,0,50,0,0", "3500,-100,4,1,0,50,0,0"];
            var data = Input(1000, 3000);
            data.applySetCode = 0x4;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            Assert.Equal(["500(1.5,50)", "1500(2,50)", "2500(2,50)", "3500(1,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 赤線に置くと、見た目の BPM になる SV の線を赤線の位置に置く
        /// </summary>
        [Fact]
        public void ApplyOnRedLines_PlacesSvForVisualBpm()
        {
            // 5-4 (見た目 BPM 240: BPM120 の赤線は SV2、BPM240 の赤線は SV1)
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "2000,250,4,1,0,60,1,0"];
            var data = Input(0, 3000);
            data.applySetCode = 0x8;
            data.bpm = 240;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            Assert.Equal(["0(2,50)", "2000(1,60)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 赤線にも音量を適用するとき、赤線の音量が変わる
        /// </summary>
        [Theory]
        [InlineData(true, 50)]    // 2000ms での音量 = 30 + (70 - 30) × 2000 / 4000
        [InlineData(false, 90)]   // 元のまま
        public void ApplyRedLines_ChangesRedLineVolume(bool shouldApplyRedlines, int expectedVolume)
        {
            // 5-15 (S5)
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "2000,500,4,1,0,90,1,0"];
            var data = Input(0, 4000);
            data.isVolume = true;
            data.volumeFrom = 30;
            data.volumeTo = 70;
            data.shouldApplyRedlines = shouldApplyRedlines;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            Assert.Equal(expectedVolume, beatmap.timingPoints.Single(tp => tp.isRedLine && tp.time == 2000).volume);
        }

        /// <summary>
        /// 等差では、SV が時刻に応じて直線で変わる
        /// </summary>
        [Fact]
        public void Arithmetic_SvChangesLinearly()
        {
            // 5-5 (1000ms で 1、3000ms で 3)
            var (isSuccess, beatmap, _) = Apply(Input(1000, 3000, 1.0, 3.0));

            Assert.True(isSuccess);
            Assert.Equal(["1000(1,50)", "1500(1.5,50)", "2000(2,50)", "2250(2.25,50)", "2500(2.5,50)", "3000(3,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 等比では、SV が時刻に応じて指数で変わる
        /// </summary>
        [Fact]
        public void Geometric_SvChangesExponentially()
        {
            // 5-5 (1000ms で 1、3000ms で 4。2000ms で 2、1500ms で √2)
            var data = Input(1000, 3000, 1.0, 4.0);
            data.calculationCode = Constants.CALCULATION_GEOMETRIC;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            var lines = beatmap.timingPoints.Where(tp => !tp.isRedLine).ToDictionary(tp => tp.time, tp => tp.sv);
            Assert.Equal(1.0, lines[1000], 6);
            Assert.Equal(Math.Sqrt(2), lines[1500], 6);
            Assert.Equal(2.0, lines[2000], 6);
            Assert.Equal(4.0, lines[3000], 6);
        }

        /// <summary>
        /// 始点と終点の SV が同じなら、全部同じ SV になる
        /// </summary>
        [Theory]
        [InlineData(Constants.CALCULATION_ARITHMETIC)]
        [InlineData(Constants.CALCULATION_GEOMETRIC)]
        public void SameSvFromAndTo_AllSame(int calculationCode)
        {
            // 5-18
            var data = Input(1000, 3000, 2.0, 2.0);
            data.calculationCode = calculationCode;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.All(beatmap.timingPoints.Where(tp => !tp.isRedLine), tp => Assert.Equal(2.0, tp.sv, 6));
        }

        /// <summary>
        /// 範囲の始点と終点が同じ時刻なら、始点の SV が入る
        /// </summary>
        [Theory]
        [InlineData(Constants.CALCULATION_ARITHMETIC)]
        [InlineData(Constants.CALCULATION_GEOMETRIC)]
        public void SameTimingFromAndTo_UsesSvFrom(int calculationCode)
        {
            // 5-32 (0 で割らない・NaN にならない)
            var data = Input(1000, 1000, 1.5, 3.0);
            data.calculationCode = calculationCode;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(["1000(1.5,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 相対速度のテスト用の譜面 (1000ms に SV1.5、2000ms に SV0.5 の緑線)
        /// </summary>
        private static OsuFileBuilder RelativeMap()
        {
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "1000,-66.6666666666667,4,1,0,50,0,0", "2000,-200,4,1,0,50,0,0"];
            return builder;
        }

        /// <summary>
        /// 相対速度の加算では、元の SV に足す
        /// </summary>
        [Fact]
        public void RelativeSum_AddsToOriginalSv()
        {
            // 5-6・5-33 (元の線が無い 3000ms 以降は無く、範囲は 1000〜2500)
            var data = Input(1000, 2500, 0.5);
            data.relativeCode = Constants.RELATIVE_SUM;

            var (isSuccess, beatmap, _) = Apply(data, RelativeMap());

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50)", "2000(1,50)", "2250(1,50)", "2500(1,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 相対速度の乗算では、元の SV と基準 SV の差に倍率を掛ける
        /// </summary>
        [Fact]
        public void RelativeMultiply_ScalesDifferenceFromBaseSv()
        {
            // 5-6・5-33 ((元の SV − 1) × 0.5 + 1)
            var data = Input(1000, 2500, 0.5);
            data.relativeCode = Constants.RELATIVE_MULTIPLY;
            data.relativeBaseSv = 1.0;

            var (isSuccess, beatmap, _) = Apply(data, RelativeMap());

            Assert.True(isSuccess);
            Assert.Equal(["1000(1.25,50)", "1500(1.25,50)", "2000(0.75,50)", "2250(0.75,50)", "2500(0.75,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 元の線が無い位置の相対速度は、1 を元にする
        /// </summary>
        [Fact]
        public void Relative_WithoutOriginalLine_UsesOne()
        {
            // 5-33 (赤線だけの区間)
            var data = Input(1000, 1500, 0.5);
            data.relativeCode = Constants.RELATIVE_SUM;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(["1000(1.5,50)", "1500(1.5,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 相対速度の加算で、SV が 0 以下になる線ができる
        /// </summary>
        [Fact]
        public void RelativeSum_CanProduceNonPositiveSv()
        {
            // 5-7 (S6。この結果を見て MainForm が E_V-C-7 を出して中止する。MainForm の判定は画面なしでは呼べないため、その前提だけを確かめる)
            var data = Input(2000, 2500, -1.0);
            data.relativeCode = Constants.RELATIVE_SUM;

            var (isSuccess, _, added) = Apply(data, RelativeMap());

            Assert.True(isSuccess);
            Assert.Contains(added, tp => !(tp.sv > 0));
        }

        /// <summary>
        /// 音量だけを変えると、SV は元のまま
        /// </summary>
        [Fact]
        public void VolumeOnly_KeepsSv()
        {
            // 5-8
            var data = Input(1000, 1500);
            data.isSv = false;
            data.isVolume = true;
            data.volumeFrom = 30;
            data.volumeTo = 30;

            var (isSuccess, beatmap, _) = Apply(data, RelativeMap());

            Assert.True(isSuccess);
            Assert.Equal(["1000(1.5,30)", "1500(1.5,30)", "2000(0.5,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// SV だけを変えると、音量は元のまま
        /// </summary>
        [Fact]
        public void SvOnly_KeepsVolume()
        {
            // 5-8
            var builder = RelativeMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "1000,-66.6666666666667,4,1,0,70,0,0"];

            var (isSuccess, beatmap, _) = Apply(Input(1000, 1500), builder);

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,70)", "1500(2,70)"], GreenLines(beatmap));
        }

        /// <summary>
        /// kiai をまたぐ範囲でも、kiai の有無は元のまま
        /// </summary>
        [Fact]
        public void RangeAcrossKiai_KeepsKiai()
        {
            // 5-9 (1750ms に kiai 開始の緑線)
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "1750,-100,4,1,0,50,0,1"];

            var (isSuccess, beatmap, _) = Apply(Input(1000, 2500), builder);

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50)", "1750(2,50,kiai)", "2000(2,50,kiai)", "2250(2,50,kiai)", "2500(2,50,kiai)"], GreenLines(beatmap));
        }

        /// <summary>
        /// ms オフセットの分だけ線がずれる
        /// </summary>
        [Theory]
        [InlineData(-10, new[] { 990, 1490 })]
        [InlineData(15, new[] { 1015, 1515 })]
        [InlineData(0, new[] { 1000, 1500 })]
        public void MillisecondOffset_ShiftsLines(int offset, int[] expectedTimes)
        {
            // 5-10・5-37
            var data = Input(1000, 1500);
            data.isOffset = true;
            data.offsetMode = 1;
            data.offset = offset;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(expectedTimes, beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.time));
        }

        /// <summary>
        /// 1/16 のオフセットでは、1/16 拍前に置く
        /// </summary>
        [Fact]
        public void HexaOffset_PlacesSixteenthBeatEarlier()
        {
            // 5-10 (BPM120 の 1/16 = 31.25ms)
            var data = Input(1000, 1500);
            data.isOffset = true;
            data.offsetMode = 0;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal([968, 1468], beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.time));
        }

        /// <summary>
        /// 1/12 のオフセットでは、1/12 のノーツと隣のノーツを 1/12 拍前にし、それ以外は 1/16 拍前にする
        /// </summary>
        [Fact]
        public void DuoOffset_UsesTwelfthNearTwelfthNotesOtherwiseSixteenth()
        {
            // 5-35 (1041・1083 は 1/12。1000 は 1/12 のノーツと 1/3 拍以内で隣り合う。1500 は離れている)
            var builder = BaseMap();
            builder.Editor = [];
            builder.HitObjects = ["256,192,1000,1,0,0:0:0:0:", "256,192,1041,1,0,0:0:0:0:", "256,192,1083,1,0,0:0:0:0:", "256,192,1500,1,0,0:0:0:0:"];
            var data = Input(1000, 1500);
            data.isOffset = true;
            data.offsetMode = 0;
            data.isDuoOffset = true;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            // (正確な時刻 - オフセット) を整数に切り捨てるため、小数の誤差で 1ms ずれることがある
            int[] expected = [958, 1000, 1041, 1468];
            var times = beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.time).ToList();
            Assert.Equal(expected.Length, times.Count);
            for (int i = 0; i < expected.Length; i++) Assert.InRange(times[i], expected[i] - 1, expected[i]);
        }

        /// <summary>
        /// 最初の線と同じ時刻のノーツには、オフセットを付けない
        /// </summary>
        [Fact]
        public void Offset_NoteAtFirstTimingPoint_NotShifted()
        {
            // 5-36
            var builder = BaseMap();
            builder.Editor = [];
            builder.HitObjects = ["256,192,0,1,0,0:0:0:0:", "256,192,500,1,0,0:0:0:0:"];
            var data = Input(0, 500);
            data.isOffset = true;
            data.offsetMode = 1;
            data.offset = -10;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            Assert.Equal([0, 490], beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.time));
        }

        /// <summary>
        /// SV を変えても、スライダーの長さ（時間）が変わらない
        /// </summary>
        [Fact]
        public void Apply_KeepsSliderDuration()
        {
            // 5-11 (S1。3000ms のスライダーは SV1 で長さ 70 → SV2 では 140)
            var (isSuccess, beatmap, _) = Apply(Input(1000, 3500));

            Assert.True(isSuccess);
            var sliders = beatmap.hitObjects.Where(h => h.noteType == Constants.NoteType.SLIDER).ToDictionary(h => h.time, h => h.sliderLength);
            Assert.Equal(140, sliders[3000], 6);
        }

        /// <summary>
        /// 範囲に何も無ければ、何も変えずに成功する
        /// </summary>
        [Fact]
        public void Apply_EmptyRange_SucceedsWithoutChanges()
        {
            // 5-13
            var (isSuccess, beatmap, added) = Apply(Input(5000, 6000));

            Assert.True(isSuccess);
            Assert.Empty(added);
            Assert.Empty(GreenLines(beatmap));
        }

        /// <summary>
        /// 範囲の始点と終点ちょうどのノーツには、含む設定のときだけ置く
        /// </summary>
        [Theory]
        [InlineData(true, true, new[] { 1000, 1500, 2000 })]
        [InlineData(false, true, new[] { 1500, 2000 })]
        [InlineData(true, false, new[] { 1000, 1500 })]
        public void Apply_StartAndEndNotes_PlacedOnlyWhenIncluded(bool isTimingStart, bool isTimingEnd, int[] expectedTimes)
        {
            // 5-14
            var data = Input(1000, 2000);
            data.setObjectOption.isTimingStart = isTimingStart;
            data.setObjectOption.isTimingEnd = isTimingEnd;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(expectedTimes, beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.time));
        }

        /// <summary>
        /// 小節線とノーツが 1ms ずれている場合は、線を 1ms 前に置く
        /// </summary>
        [Fact]
        public void Apply_SeparateBarline_PlacesOneMillisecondEarlier()
        {
            // 5-16 (1 小節が 1999.99999995ms の赤線。小節線は stable では 1999ms、lazer では 2000ms になり、2000ms のノーツと分かれる)
            var builder = BaseMap();
            builder.Editor = [];
            builder.TimingPoints = ["0,499.9999999875,4,1,0,50,1,0"];
            builder.HitObjects = ["256,192,2000,1,0,0:0:0:0:"];

            var (isSuccess, beatmap, _) = Apply(Input(2000, 2000), builder);

            Assert.True(isSuccess);
            Assert.Equal([1999], beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.time));
        }

        /// <summary>
        /// 範囲の中で BPM が変わると、基準の BPM に合わせて SV を補正する
        /// </summary>
        [Fact]
        public void Apply_BpmChangeInRange_AdjustsSvToBaseBpm()
        {
            // 5-17・5-26・5-39 (2000ms から BPM240。2000ms のノーツと同じ時刻にあった SV0.5 の緑線は消えて 1 本だけになる)
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "2000,250,4,1,0,50,1,0", "2000,-200,4,1,0,50,0,0"];

            var (isSuccess, beatmap, _) = Apply(Input(1000, 2500), builder);

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50)", "2000(1,50)", "2250(1,50)", "2500(1,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 相対速度では、BPM の補正をしない
        /// </summary>
        [Fact]
        public void Relative_DoesNotAdjustBpm()
        {
            // 5-17
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "2000,250,4,1,0,50,1,0"];
            var data = Input(1000, 2500, 0.5);
            data.relativeCode = Constants.RELATIVE_SUM;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            Assert.All(beatmap.timingPoints.Where(tp => !tp.isRedLine), tp => Assert.Equal(1.5, tp.sv, 6));
        }

        /// <summary>
        /// 同じ操作を 2 回しても、線が二重にならない
        /// </summary>
        [Fact]
        public void Apply_Twice_DoesNotDuplicateLines()
        {
            // 5-20
            var (_, first, _) = Apply(Input(1000, 3500));
            string path = Path.Combine(WorkDirectory, "apply.osu");
            Assert.True(BeatmapHelper.ExportToOsuFile(first, path));
            var reloaded = BeatmapHelper.GetBeatmapData(path);
            List<TimingPoint> added = [];

            Assert.True(SVCalculatorService.Apply(Input(1000, 3500), reloaded, ref added));
            reloaded.timingPoints = [.. reloaded.timingPoints.Concat(added).OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0)];

            Assert.Equal(GreenLines(first), GreenLines(reloaded));
        }

        /// <summary>
        /// 適用してから同じ範囲を削除すると、元の線とスライダーの長さに戻る
        /// </summary>
        [Fact]
        public void ApplyThenRemove_RestoresLinesAndSliderLength()
        {
            // 5-20
            var (_, applied, _) = Apply(Input(1000, 3500));
            string path = Path.Combine(WorkDirectory, "apply.osu");
            Assert.True(BeatmapHelper.ExportToOsuFile(applied, path));
            var reloaded = BeatmapHelper.GetBeatmapData(path);
            UserInputData remove = new() { timingFrom = 1000, timingTo = 3500 };
            remove.setObjectOption.isTimingStart = true;
            remove.setObjectOption.isTimingEnd = true;

            Assert.True(SVCalculatorService.Remove(remove, reloaded));

            Assert.Empty(GreenLines(reloaded));
            Assert.Equal(70, reloaded.hitObjects.Single(h => h.noteType == Constants.NoteType.SLIDER).sliderLength, 6);
        }

        /// <summary>
        /// 入力を変えて続けて実行しても、前回の状態が残らない
        /// </summary>
        [Fact]
        public void Apply_Consecutive_DoesNotKeepPreviousState()
        {
            // 5-21
            var kaOnly = Input(1000, 3500);
            kaOnly.setObjectOption.setObjectsCode = 0x4;
            Apply(kaOnly);

            var (_, second, _) = Apply(Input(1000, 3500));

            Assert.Equal(["1000(2,50)", "1500(2,50)", "2000(2,50)", "2250(2,50)", "2500(2,50)", "3000(2,50)", "3500(2,50)"], GreenLines(second));
        }

        /// <summary>
        /// 新しい緑線は、直前の線のサンプルセットとサンプル番号を引き継ぐ
        /// </summary>
        [Fact]
        public void Apply_NewLinesInheritSampleSetAndIndex()
        {
            // 5-22
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "500,-100,3,2,3,50,0,0"];

            var (isSuccess, beatmap, _) = Apply(Input(1000, 1500), builder);

            Assert.True(isSuccess);
            Assert.All(beatmap.timingPoints.Where(tp => !tp.isRedLine && tp.time >= 1000), tp =>
            {
                Assert.Equal(3, tp.meter);
                Assert.Equal(2, tp.sampleSet);
                Assert.Equal(3, tp.sampleIndex);
            });
        }

        /// <summary>
        /// オフセットで前に置いた線では、kiai の開始の位置がずれない
        /// </summary>
        [Fact]
        public void Offset_KeepsKiaiStartPosition()
        {
            // 5-23 (2000ms のノーツの位置で kiai 開始。-10ms の線は kiai なし、2000ms の kiai の線は残る)
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "2000,-100,4,1,0,50,0,1"];
            var data = Input(2000, 2000);
            data.isOffset = true;
            data.offsetMode = 1;
            data.offset = -10;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            var lines = beatmap.timingPoints.Where(tp => !tp.isRedLine).ToDictionary(tp => tp.time, tp => tp.effect & 1);
            Assert.Equal(0, lines[1990]);
            Assert.Equal(1, lines[2000]);
        }

        /// <summary>
        /// 音量だけを変えて範囲の端にノーツが無ければ、端に線を置く
        /// </summary>
        [Fact]
        public void VolumeOnly_PlacesLinesAtRangeEdgesWithoutNotes()
        {
            // 5-27 (1200ms で 30、2700ms で 60)
            var data = Input(1200, 2700);
            data.isSv = false;
            data.isVolume = true;
            data.volumeFrom = 30;
            data.volumeTo = 60;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(["1200(1,30)", "1500(1,36)", "2000(1,46)", "2250(1,51)", "2500(1,56)", "2700(1,60)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 同じ時刻に置く線が重なると、後から足した線だけが残る
        /// </summary>
        [Fact]
        public void Apply_SameTimeLines_KeepsLastAdded()
        {
            // 5-28 (1000ms のノーツの -10ms の線と、始点 990ms の音量の線が重なる。後から足した始点の線が残る)
            var data = Input(990, 1500);
            data.isSv = false;
            data.isVolume = true;
            data.volumeFrom = 30;
            data.volumeTo = 30;
            data.isOffset = true;
            data.offsetMode = 1;
            data.offset = -10;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Single(beatmap.timingPoints, tp => !tp.isRedLine && tp.time == 990);
        }

        /// <summary>
        /// 音量のグラデーションは、四捨五入される
        /// </summary>
        [Theory]
        [InlineData(50, 51, 51)]   // 50.5 は四捨五入して 51
        [InlineData(80, 20, 50)]   // 下げる向き
        public void VolumeGradient_RoundsHalfUp(int volumeFrom, int volumeTo, int expectedAt1500)
        {
            // 5-34 (1000〜2000ms の真ん中の 1500ms)
            var data = Input(1000, 2000);
            data.isSv = false;
            data.isVolume = true;
            data.volumeFrom = volumeFrom;
            data.volumeTo = volumeTo;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(expectedAt1500, beatmap.timingPoints.Single(tp => !tp.isRedLine && tp.time == 1500).volume);
        }

        /// <summary>
        /// 入力チェックを通った音量のグラデーションは、5 から 100 の範囲に収まる
        /// </summary>
        [Theory]
        [InlineData(0x1)]   // オブジェクトに置く
        [InlineData(0x2)]   // ビートスナップ間隔
        [InlineData(0x4)]   // 緑線
        public void VolumeGradient_ValidInput_StaysWithinFiveToHundred(int applySetCode)
        {
            // 5-24 (音量は範囲内の時刻だけで計算するため、始点と終点の音量の間に収まる)
            var data = Input(1000, 3000);
            data.applySetCode = applySetCode;
            data.setBeatSnapOption.beatSnap = 4;
            data.isSv = false;
            data.isVolume = true;
            data.volumeFrom = 5;
            data.volumeTo = 100;

            var (isSuccess, beatmap, _) = Apply(data, RelativeMap());

            Assert.True(isSuccess);
            Assert.All(beatmap.timingPoints, tp => Assert.InRange(tp.volume, 5, 100));
        }

        /// <summary>
        /// 範囲外の音量が渡されたら、そのまま書く
        /// </summary>
        [Fact]
        public void Volume_OutOfRange_WrittenAsIs()
        {
            // 5-24 今の動きを固定する (入力チェックを通らない値。計算では範囲に収めない)
            var data = Input(1000, 2000);
            data.isSv = false;
            data.isVolume = true;
            data.volumeFrom = 0;
            data.volumeTo = 150;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal([0, 75, 150], beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.volume));
        }

        /// <summary>
        /// 時刻が小数になるビートスナップ間隔では、線の時刻を切り捨てる
        /// </summary>
        [Theory]
        [InlineData(7, new[] { 0, 71, 142, 214, 285, 357, 428 })]   // 71.43ms ずつ
        [InlineData(5, new[] { 0, 100, 200, 300, 400 })]            // 100ms ずつ (割り切れる)
        [InlineData(3, new[] { 0, 166, 333 })]                      // 166.67ms ずつ
        public void ApplyOnBeatSnaps_FractionalTime_Truncates(int beatSnap, int[] expectedTimes)
        {
            // 5-29 今の動きを固定する (osu! のスナップの位置と合うかは、比べ方が決まっていない。
            // 四捨五入なら 1/7 は 143・286・429、1/3 は 167 になる)
            var data = Input(0, 450);
            data.applySetCode = 0x2;
            data.setBeatSnapOption.beatSnap = beatSnap;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(expectedTimes, beatmap.timingPoints.Where(tp => !tp.isRedLine).Select(tp => tp.time));
        }

        /// <summary>
        /// kiai の切り替わりの緑線がある譜面 (1500ms: カツと同じ位置で kiai 開始、2750ms: ノーツの無い位置で kiai 終了。どちらも SV1)
        /// </summary>
        private static OsuFileBuilder KiaiMap()
        {
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "1500,-100,4,1,0,50,0,1", "2750,-100,4,1,0,50,0,0"];
            return builder;
        }

        /// <summary>
        /// オブジェクトに置くと、範囲内の kiai の線は残り、SV は新しい値になる
        /// </summary>
        [Fact]
        public void ApplyOnObjects_KiaiLinesKeptWithNewSv()
        {
            // 5-42 (1500ms の kiai 開始はノーツに置く線が引き継ぐ。2750ms の kiai 終了は残って SV2 になる)
            var (isSuccess, beatmap, _) = Apply(Input(1000, 3000), KiaiMap());

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50,kiai)", "2000(2,50,kiai)", "2250(2,50,kiai)", "2500(2,50,kiai)", "2750(2,50)", "3000(2,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// ビートスナップ間隔では、ずれた位置の kiai の線が元の SV のまま残る
        /// </summary>
        [Fact]
        public void ApplyOnBeatSnaps_OffSnapKiaiLineKeepsOldSv()
        {
            // 5-42 今の動きを固定する (期待結果は未決定。2750ms の線が SV1 のまま残るため、2750〜3000ms だけ SV が戻る)
            var data = Input(1000, 3000);
            data.applySetCode = 0x2;
            data.setBeatSnapOption.beatSnap = 1;

            var (isSuccess, beatmap, _) = Apply(data, KiaiMap());

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50,kiai)", "2000(2,50,kiai)", "2500(2,50,kiai)", "2750(1,50)", "3000(2,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 緑線に適用すると、kiai の線も SV が新しい値になる
        /// </summary>
        [Fact]
        public void ApplyGreenLines_KiaiLinesGetNewSv()
        {
            // 5-42
            var data = Input(1000, 3000);
            data.applySetCode = 0x4;

            var (isSuccess, beatmap, _) = Apply(data, KiaiMap());

            Assert.True(isSuccess);
            Assert.Equal(["1500(2,50,kiai)", "2750(2,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 赤線に置くときは、kiai の線を変えない
        /// </summary>
        [Fact]
        public void ApplyOnRedLines_KeepsKiaiLines()
        {
            // 5-42 (赤線は 0ms だけ。kiai の線は SV1 のまま)
            var data = Input(0, 3000);
            data.applySetCode = 0x8;
            data.bpm = 240;

            var (isSuccess, beatmap, _) = Apply(data, KiaiMap());

            Assert.True(isSuccess);
            Assert.Equal(["0(2,50)", "1500(1,50,kiai)", "2750(1,50)"], GreenLines(beatmap));
        }

        #region 15 SV を BPM で指定
        /// <summary>
        /// BPM で指定する入力値 (Input と同じで、svFrom・svTo に BPM を入れる)
        /// </summary>
        private static UserInputData BpmInput(int from, int to, double bpmFrom, double? bpmTo = null)
        {
            var data = Input(from, to, bpmFrom, bpmTo);
            data.isSvBpm = true;
            return data;
        }

        /// <summary>
        /// SliderMultiplier を差し替えた基本の譜面 (null なら行を消す)
        /// </summary>
        private static OsuFileBuilder MapWithSliderMultiplier(string? sliderMultiplier)
        {
            var builder = BaseMap();
            builder.Difficulty = [.. builder.Difficulty.Where(l => !l.StartsWith("SliderMultiplier:"))];
            if (sliderMultiplier != null) builder.Difficulty.Add($"SliderMultiplier:{sliderMultiplier}");
            return builder;
        }

        /// <summary>
        /// BPM で指定すると、SliderMultiplier 1.4 の譜面では SV = BPM ÷ 赤線の BPM になる
        /// </summary>
        [Fact]
        public void SvBpm_ConvertsBpmToSv()
        {
            // 15-3 (BPM120 の譜面で 240 → SV2。SV で 2 を指定したときと同じ線になる)
            var (_, bySv, _) = Apply(Input(1000, 3500, 2.0));
            var (isSuccess, byBpm, _) = Apply(BpmInput(1000, 3500, 240));

            Assert.True(isSuccess);
            Assert.Equal(GreenLines(bySv), GreenLines(byBpm));
        }

        /// <summary>
        /// BPM で指定すると、SliderMultiplier も換算に入れる (行が無ければ 1.4 とする)
        /// </summary>
        [Theory]
        [InlineData("1.8", 240.0 / 120 * 1.4 / 1.8)]
        [InlineData("0.7", 240.0 / 120 * 1.4 / 0.7)]
        [InlineData(null, 2.0)]
        public void SvBpm_UsesSliderMultiplier(string? sliderMultiplier, double expectedSv)
        {
            // 15-3 (Timing Property の見た目 BPM が 240 になる SV)
            var (isSuccess, beatmap, _) = Apply(BpmInput(1000, 2000, 240), MapWithSliderMultiplier(sliderMultiplier));

            Assert.True(isSuccess);
            Assert.All(beatmap.timingPoints.Where(tp => !tp.isRedLine), tp => Assert.Equal(expectedSv, tp.sv, 9));
        }

        /// <summary>
        /// BPM で指定したとき、範囲の中で BPM が変わっても同じ見た目 BPM になる
        /// </summary>
        [Fact]
        public void SvBpm_BpmChangeInRange_KeepsVisualBpm()
        {
            // 15-4 (2000ms から BPM240。見た目 BPM 240 は、BPM120 では SV2、BPM240 では SV1)
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "2000,250,4,1,0,50,1,0"];

            var (isSuccess, beatmap, _) = Apply(BpmInput(1000, 3000, 240), builder);

            Assert.True(isSuccess);
            Assert.Equal(["1000(2,50)", "1500(2,50)", "2000(1,50)", "2250(1,50)", "2500(1,50)", "3000(1,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// BPM で指定したグラデーションは、BPM で等差・等比に変わる
        /// </summary>
        [Theory]
        [InlineData(Constants.CALCULATION_ARITHMETIC, new[] { "1000(1,50)", "1500(1.5,50)", "2000(2,50)" })]   // 120 → 180 → 240
        [InlineData(Constants.CALCULATION_GEOMETRIC, new[] { "1000(1,50)", "1500(1.414,50)", "2000(2,50)" })]  // 120 → 169.7 → 240
        public void SvBpm_Gradient_ChangesByBpm(int calculationCode, string[] expected)
        {
            // 15-5
            var data = BpmInput(1000, 2000, 120, 240);
            data.calculationCode = calculationCode;

            var (isSuccess, beatmap, _) = Apply(data);

            Assert.True(isSuccess);
            Assert.Equal(expected, GreenLines(beatmap));
        }

        /// <summary>
        /// BPM で指定すると、ビートスナップ間隔・緑線に置くときも SV に換算する
        /// </summary>
        [Theory]
        [InlineData(0x2)]
        [InlineData(0x4)]
        public void SvBpm_BeatSnapsAndGreenLines_ConvertBpmToSv(int applySetCode)
        {
            // 15-3
            var data = BpmInput(1000, 3000, 240);
            data.applySetCode = applySetCode;
            data.setBeatSnapOption.beatSnap = 1;

            var (isSuccess, beatmap, _) = Apply(data, RelativeMap());

            Assert.True(isSuccess);
            Assert.All(beatmap.timingPoints.Where(tp => !tp.isRedLine && tp.time >= 1000 && tp.time <= 3000), tp => Assert.Equal(2.0, tp.sv, 9));
        }

        /// <summary>
        /// 赤線に置くときは、BPM で指定の設定があっても見た目 BPM の欄の値で計算する
        /// </summary>
        [Fact]
        public void SvBpm_RedLineMode_Ignored()
        {
            // 15-3
            var builder = BaseMap();
            builder.TimingPoints = ["0,500,4,1,0,50,1,0", "2000,250,4,1,0,60,1,0"];
            var data = BpmInput(0, 3000, 999);
            data.applySetCode = 0x8;
            data.bpm = 240;

            var (isSuccess, beatmap, _) = Apply(data, builder);

            Assert.True(isSuccess);
            Assert.Equal(["0(2,50)", "2000(1,60)"], GreenLines(beatmap));
        }

        /// <summary>
        /// BPM で指定して適用した後も、入力値は BPM のまま (SV に換算した値で書き換えない)
        /// </summary>
        [Fact]
        public void SvBpm_KeepsInputValues()
        {
            // 15-6
            var data = BpmInput(1000, 2000, 240, 180);

            Apply(data);

            Assert.Equal(240.0, data.svFrom);
            Assert.Equal(180.0, data.svTo);
        }
        #endregion
    }
}
