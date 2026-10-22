using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Services;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// SV計算の削除のテスト (TEST_PATTERNS.md の 5-12・5-19・5-40・5-41・5-43)
    /// </summary>
    /// <remarks>
    /// 譜面: 赤線 0ms (BPM120・音量50)、緑線 1000ms (SV1.5・音量60)・2000ms (SV1.5・音量70・kiai開始)・
    /// 3000ms (SV0.8・音量80・kiaiのまま)・4000ms (SV0.8・音量90・kiai終了)、
    /// ノーツ 500・1500・2500(スライダー)・3500・4500
    /// </remarks>
    public class SvRemoveTests : TestEnvironment
    {
        private static readonly List<string> TimingPoints =
        [
            "0,500,4,1,0,50,1,0",
            "1000,-66.6666666666667,4,1,0,60,0,0",
            "2000,-66.6666666666667,4,1,0,70,0,1",
            "3000,-125,4,1,0,80,0,1",
            "4000,-125,4,1,0,90,0,0",
        ];
        private static readonly List<string> HitObjects =
        [
            "256,192,500,1,0,0:0:0:0:",
            "256,192,1500,1,0,0:0:0:0:",
            "256,192,2500,2,0,L|300:192,1,105",
            "256,192,3500,1,0,0:0:0:0:",
            "256,192,4500,1,0,0:0:0:0:",
        ];

        /// <summary>
        /// 緑線を削除して、削除後の譜面を返す
        /// </summary>
        private Beatmap Remove(int from, int to, bool isTimingStart = true, bool isTimingEnd = true,
                               bool isOffset = false, int offsetMode = 0, int offset = 0, bool isDuoOffset = false, int setObjectsCode = 0x37f)
        {
            var builder = new OsuFileBuilder { TimingPoints = TimingPoints, HitObjects = HitObjects };
            string path = WriteOsuFile("remove.osu", builder.Build());
            var beatmap = BeatmapHelper.GetBeatmapData(path);
            Assert.NotEqual(string.Empty, beatmap.version);

            UserInputData data = new()
            {
                timingFrom = from,
                timingTo = to,
                isOffset = isOffset,
                offsetMode = offsetMode,
                offset = offset,
                isDuoOffset = isDuoOffset,
            };
            data.setObjectOption.setObjectsCode = setObjectsCode;
            data.setObjectOption.isTimingStart = isTimingStart;
            data.setObjectOption.isTimingEnd = isTimingEnd;
            Assert.True(SVCalculatorService.Remove(data, beatmap));
            return beatmap;
        }

        /// <summary>
        /// 残った緑線を「時刻(SV,音量,kiai)」の形にする
        /// </summary>
        private static List<string> GreenLines(Beatmap beatmap) =>
            [.. beatmap.timingPoints.Where(tp => !tp.isRedLine)
                                    .Select(tp => $"{tp.time}({Math.Round(tp.sv, 2)},{tp.volume}{((tp.effect & 1) == 1 ? ",kiai" : "")})")];

        /// <summary>
        /// kiai の切り替わりが無い範囲の緑線は、全部消える
        /// </summary>
        [Fact]
        public void Remove_NoKiaiChange_RemovesAllGreenLines()
        {
            // 5-12
            var beatmap = Remove(500, 1500);

            Assert.Equal(["2000(1.5,70,kiai)", "3000(0.8,80,kiai)", "4000(0.8,90)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 削除は、選んだオブジェクトの種類によらず範囲内の緑線を消す
        /// </summary>
        [Theory]
        [InlineData(0x800)]   // ブックマークのみ
        [InlineData(0x4)]     // カツのみ
        public void Remove_IgnoresSelectedObjectTypes(int setObjectsCode)
        {
            // 5-19 今の動きを固定する (削除はオブジェクトの種類を見ないため、元の音量に戻す緑線も置かない。
            // 全てのヒットオブジェクトを選んだときと同じ結果になる)
            var selected = Remove(500, 1500, setObjectsCode: setObjectsCode);
            var all = Remove(500, 1500);

            Assert.Equal(GreenLines(all), GreenLines(selected));
            Assert.Equal(["2000(1.5,70,kiai)", "3000(0.8,80,kiai)", "4000(0.8,90)"], GreenLines(selected));
        }

        /// <summary>
        /// kiai の開始の緑線は残り、始点より前の線の SV と音量になる
        /// </summary>
        [Fact]
        public void Remove_KiaiStartKeptWithValuesBeforeStart()
        {
            // 5-40
            var beatmap = Remove(1500, 2500);

            Assert.Equal(["1000(1.5,60)", "2000(1.5,60,kiai)", "3000(0.8,80,kiai)", "4000(0.8,90)"], GreenLines(beatmap));
        }

        /// <summary>
        /// kiai の終了の緑線は残り、始点より前の線の SV と音量になる
        /// </summary>
        [Fact]
        public void Remove_KiaiEndKeptWithValuesBeforeStart()
        {
            // 5-40
            var beatmap = Remove(3500, 4500);

            Assert.Equal(["1000(1.5,60)", "2000(1.5,70,kiai)", "3000(0.8,80,kiai)", "4000(0.8,80)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 範囲が広いと、kiai の開始と終了だけが残る
        /// </summary>
        [Fact]
        public void Remove_WideRange_KeepsOnlyKiaiStartAndEnd()
        {
            // 5-40 (始点より前は赤線なので SV1・音量50)
            var beatmap = Remove(500, 4500);

            Assert.Equal(["2000(1,50,kiai)", "4000(1,50)"], GreenLines(beatmap));
        }

        /// <summary>
        /// 残す kiai の線の値は、オフセットを含めた始点より前の線から取る
        /// </summary>
        [Theory]
        [InlineData(false, 0, 0, false, 1020, "2000(1.5,60,kiai)")]   // オフセットなし: 1020ms より前 → 1000ms の緑線
        [InlineData(true, 0, 0, false, 1020, "2000(1,50,kiai)")]      // 1/16 (31.25ms): 988ms より前 → 赤線
        [InlineData(true, 0, 0, true, 1040, "2000(1,50,kiai)")]       // 1/12 (41.67ms): 998ms より前 → 赤線
        [InlineData(true, 1, -600, false, 1500, "2000(1,50,kiai)")]   // ms -600: 900ms より前 → 赤線
        public void Remove_KiaiValuesTakenBeforeStartWithOffset(bool isOffset, int offsetMode, int offset, bool isDuoOffset, int from, string expectedKiaiLine)
        {
            // 5-40
            var beatmap = Remove(from, 2500, isOffset: isOffset, offsetMode: offsetMode, offset: offset, isDuoOffset: isDuoOffset);

            Assert.Contains(expectedKiaiLine, GreenLines(beatmap));
        }

        /// <summary>
        /// 始点と終点ちょうどの緑線は、含まない設定なら消えない
        /// </summary>
        [Theory]
        [InlineData(true, true, new[] { "2000(1,50,kiai)", "4000(1,50)" })]
        [InlineData(false, true, new[] { "1000(1.5,60)", "2000(1,50,kiai)", "4000(1,50)" })]   // 残す kiai の線の値は始点(1000ms)より前 = 赤線
        [InlineData(true, false, new[] { "2000(1,50,kiai)", "4000(0.8,90)" })]
        public void Remove_StartAndEndLines_KeptWhenExcluded(bool isTimingStart, bool isTimingEnd, string[] expected)
        {
            // 5-41
            var beatmap = Remove(1000, 4000, isTimingStart, isTimingEnd);

            Assert.Equal(expected, GreenLines(beatmap));
        }

        /// <summary>
        /// 残した kiai の線の SV が変わると、後ろのスライダーの長さが補正される
        /// </summary>
        [Fact]
        public void Remove_KeptKiaiSvChange_AdjustsSliderLength()
        {
            // 5-43 (2500ms のスライダーは SV1.5 で長さ 105。kiai の線の SV が 1 になるので、長さ(時間)を保つために 70 になる)
            var beatmap = Remove(500, 4500);

            var slider = beatmap.hitObjects.Single(h => h.noteType == Constants.NoteType.SLIDER);
            Assert.Equal(70, slider.sliderLength, 6);
        }
    }
}
