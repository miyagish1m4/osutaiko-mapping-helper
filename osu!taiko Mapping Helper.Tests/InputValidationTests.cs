using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// 入力チェックのテスト (TEST_PATTERNS.md の 9)
    /// </summary>
    public class InputValidationTests : TestEnvironment
    {
        /// <summary>
        /// エラーにならない入力 (オブジェクトに置く、1000〜2000ms、SV 1、音量 50、オフセットなし)
        /// </summary>
        private static UserInputTempData ValidInput()
        {
            UserInputTempData input = new()
            {
                timingFrom = "1000",
                timingTo = "2000",
                svFrom = "1",
                svTo = "1",
                volumeFrom = "50",
                volumeTo = "50",
                isOffset = false,
            };
            return input;
        }

        /// <summary>
        /// 入力をチェックして、結果と変換後の入力値を返す
        /// </summary>
        private static (bool isSuccess, UserInputData data) Validate(UserInputTempData input, int executeCode = Constants.EXECUTE_APPLY)
        {
            UserInputData data = new();
            bool isSuccess = UserInputDataHelper.SetUserInputData(input, executeCode, ref data);
            return (isSuccess, data);
        }

        /// <summary>
        /// エラーにならない入力は、そのまま変換される
        /// </summary>
        [Fact]
        public void SetUserInputData_ValidInput_Converted()
        {
            var (isSuccess, data) = Validate(ValidInput());

            Assert.True(isSuccess);
            Assert.Empty(MessageCodes);
            Assert.Equal(1000, data.timingFrom);
            Assert.Equal(2000, data.timingTo);
            Assert.Equal(1.0, data.svFrom);
            Assert.Equal(50, data.volumeFrom);
            Assert.Equal(Constants.CALCULATION_ARITHMETIC, data.calculationCode);
            Assert.Equal(Constants.RELATIVE_DISABLE, data.relativeCode);
        }

        // 9-1 Timing
        /// <summary>
        /// Timing の入力エラー
        /// </summary>
        [Theory]
        [InlineData("", "2000", "E_V-EM-4")]           // 空
        [InlineData("1000", "", "E_V-EM-4")]
        [InlineData("abc", "2000", "E_V-C-4")]         // 形式が違う
        [InlineData("1:2:3", "2000", "E_V-C-4")]       // 9-8 ミリ秒が 3 桁ない
        [InlineData("2000", "1000", "E_V-C-5")]        // 始点 > 終点
        public void SetUserInputData_InvalidTiming_ShowsError(string timingFrom, string timingTo, string expectedCode)
        {
            var input = ValidInput();
            input.timingFrom = timingFrom;
            input.timingTo = timingTo;

            var (isSuccess, _) = Validate(input);

            Assert.False(isSuccess);
            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// Timing は、時刻とミリ秒の両方で入力できる
        /// </summary>
        [Theory]
        [InlineData("00:01:000", 1000)]
        [InlineData("01:23:456", 83456)]
        [InlineData("01:23:456 (1,2,3) - ", 83456)]   // 9-7 osu!のエディタからコピーした時刻
        [InlineData("1500", 1500)]
        public void SetUserInputData_TimingAcceptsTimeAndMilliseconds(string timing, int expected)
        {
            var input = ValidInput();
            input.timingFrom = timing;
            input.timingTo = "99:00:000";

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.Equal(expected, data.timingFrom);
        }

        // 9-2 SV
        /// <summary>
        /// SV の入力エラー
        /// </summary>
        [Theory]
        [InlineData("", "1", "E_V-EM-5")]
        [InlineData("abc", "1", "E_V-T-7")]
        [InlineData("0", "1", "E_V-T-8")]
        [InlineData("1", "-1", "E_V-T-8")]
        public void SetUserInputData_InvalidSv_ShowsError(string svFrom, string svTo, string expectedCode)
        {
            var input = ValidInput();
            input.svFrom = svFrom;
            input.svTo = svTo;

            var (isSuccess, _) = Validate(input);

            Assert.False(isSuccess);
            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// SV のチェックを外すと、SV の入力は見ない
        /// </summary>
        [Fact]
        public void SetUserInputData_SvUnchecked_IgnoresSv()
        {
            var input = ValidInput();
            input.isSv = false;
            input.svFrom = "";

            var (isSuccess, _) = Validate(input);

            Assert.True(isSuccess);
        }

        /// <summary>
        /// SV にカンマを入れると、桁区切りとして読まれる
        /// </summary>
        [Fact]
        public void SetUserInputData_SvWithComma_ReadAsThousandsSeparator()
        {
            // 9-10 今の動きを固定する (osu!側でもエラーになるため対応しない)
            var input = ValidInput();
            input.svFrom = "1,5";
            input.svTo = "1,5";

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.Equal(15.0, data.svFrom);
        }

        // 9-3・9-11 音量
        /// <summary>
        /// 音量の入力エラー
        /// </summary>
        [Theory]
        [InlineData("", "E_V-EM-6")]
        [InlineData("abc", "E_V-T-9")]
        [InlineData("50.5", "E_V-T-10")]
        [InlineData("50.0", "E_V-T-10")]
        [InlineData("4", "E_V-C-6")]
        [InlineData("101", "E_V-C-6")]
        public void SetUserInputData_InvalidVolume_ShowsError(string volume, string expectedCode)
        {
            var input = ValidInput();
            input.volumeFrom = volume;

            var (isSuccess, _) = Validate(input);

            Assert.False(isSuccess);
            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// 音量は 5 から 100 まで入力できる
        /// </summary>
        [Theory]
        [InlineData("5", 5)]
        [InlineData("100", 100)]
        [InlineData("+50", 50)]
        public void SetUserInputData_VolumeAcceptsFiveToHundred(string volume, int expected)
        {
            var input = ValidInput();
            input.volumeFrom = volume;

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.Equal(expected, data.volumeFrom);
        }

        // 9-4・9-11 ビートスナップ間隔
        /// <summary>
        /// ビートスナップ間隔の入力エラー
        /// </summary>
        [Theory]
        [InlineData("", "E_V-EM-8")]
        [InlineData("abc", "E_V-T-13")]
        [InlineData("0", "E_V-T-14")]
        [InlineData("-1", "E_V-T-14")]
        [InlineData("1.5", "E_V-T-14")]
        public void SetUserInputData_InvalidBeatSnap_ShowsError(string beatSnap, string expectedCode)
        {
            var input = ValidInput();
            input.setOption.isSetObjects = false;
            input.setOption.isSetBeatSnap = true;
            input.setBeatSnapOption.beatSnap = beatSnap;

            var (isSuccess, _) = Validate(input);

            Assert.False(isSuccess);
            Assert.Equal([expectedCode], MessageCodes);
        }

        // 9-4・9-11 オフセット (ms)
        /// <summary>
        /// ms オフセットの入力エラー
        /// </summary>
        [Theory]
        [InlineData("abc", "E_V-T-15")]
        [InlineData("1.5", "E_V-T-16")]
        public void SetUserInputData_InvalidOffset_ShowsError(string offset, string expectedCode)
        {
            var input = ValidInput();
            input.isOffset = true;
            input.offsetMode = 1;
            input.offset = offset;

            var (isSuccess, _) = Validate(input);

            Assert.False(isSuccess);
            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// ms オフセットは、空なら 0 になる
        /// </summary>
        [Theory]
        [InlineData("", 0)]
        [InlineData("-20", -20)]
        [InlineData("15", 15)]
        public void SetUserInputData_EmptyOffset_BecomesZero(string offset, int expected)
        {
            var input = ValidInput();
            input.isOffset = true;
            input.offsetMode = 1;
            input.offset = offset;

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.Equal(expected, data.offset);
        }

        /// <summary>
        /// ブックマークのみを選ぶと、オフセットを使わない
        /// </summary>
        [Fact]
        public void SetUserInputData_OnlyBookmarks_DisablesOffset()
        {
            // U5
            var input = ValidInput();
            input.isOffset = true;
            input.offsetMode = 1;
            input.offset = "-20";
            input.setObjectOption.isAllHitObjects = false;
            input.setObjectOption.isOnlyBookmarks = true;

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.False(data.isOffset);
        }

        // 9-4・9-11 見た目BPM (赤線に置く)
        /// <summary>
        /// 見た目 BPM の入力エラー
        /// </summary>
        [Theory]
        [InlineData("", "E_V-EM-10")]
        [InlineData("1e5", "E_V-T-17")]
        [InlineData("0", "E_V-T-18")]
        [InlineData("-1", "E_V-T-18")]
        public void SetUserInputData_InvalidBpm_ShowsError(string bpm, string expectedCode)
        {
            var input = ValidInput();
            input.setOption.isSetObjects = false;
            input.setOption.isSetRedLine = true;
            input.bpm = bpm;

            var (isSuccess, _) = Validate(input);

            Assert.False(isSuccess);
            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// 赤線に置くときは、SV の入力を見ない
        /// </summary>
        [Fact]
        public void SetUserInputData_RedLineMode_IgnoresSv()
        {
            // 5-31
            var input = ValidInput();
            input.setOption.isSetObjects = false;
            input.setOption.isSetRedLine = true;
            input.bpm = "200";
            input.svFrom = "";

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.Equal(200.0, data.bpm);
        }

        // 9-12 相対速度の乗算の基準SV
        /// <summary>
        /// 相対速度の乗算の入力エラー
        /// </summary>
        [Theory]
        [InlineData("", "1", "E_V-EM-7")]
        [InlineData("abc", "1", "E_V-T-11")]
        [InlineData("-1", "1", "E_V-T-12")]
        [InlineData("0", "0", "E_V-T-8")]     // 基準SVが0で倍率も0
        public void SetUserInputData_InvalidRelativeMultiply_ShowsError(string baseSv, string ratio, string expectedCode)
        {
            var input = ValidInput();
            input.isRelative = true;
            input.isRelativeMultiply = true;
            input.relativeMultiplyBaseSv = baseSv;
            input.relativeMultiplySvFrom = ratio;

            var (isSuccess, _) = Validate(input);

            Assert.False(isSuccess);
            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// 相対速度で終点 SV を使わない場合は、終点が始点と同じになる
        /// </summary>
        [Fact]
        public void SetUserInputData_RelativeWithoutSvTo_UsesSvFrom()
        {
            // 9-13
            var input = ValidInput();
            input.isRelative = true;
            input.isRelativeMultiply = false;
            input.isRelativeSum = true;
            input.relativeSumSvFrom = "0.5";
            input.relativeSumSvTo = "9";
            input.isEnableRelativeEnd = false;

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.Equal(Constants.RELATIVE_SUM, data.relativeCode);
            Assert.Equal(0.5, data.svFrom);
            Assert.Equal(0.5, data.svTo);
        }

        /// <summary>
        /// 相対速度では、計算方法が等差になる
        /// </summary>
        [Fact]
        public void SetUserInputData_Relative_UsesArithmetic()
        {
            var input = ValidInput();
            input.isArithmetic = false;
            input.isGeometric = true;
            input.isRelative = true;
            input.isRelativeMultiply = false;
            input.isRelativeSum = true;
            input.relativeSumSvFrom = "0.5";

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.Equal(Constants.CALCULATION_ARITHMETIC, data.calculationCode);
        }

        /// <summary>
        /// 削除では、SV と音量の入力を見ない
        /// </summary>
        [Fact]
        public void SetUserInputData_Remove_IgnoresSvAndVolume()
        {
            // 9-9
            var input = ValidInput();
            input.svFrom = "";
            input.volumeFrom = "";

            var (isSuccess, data) = Validate(input, Constants.EXECUTE_REMOVE);

            Assert.True(isSuccess);
            Assert.Empty(MessageCodes);
            Assert.False(data.shouldApplyRedlines);
        }

        // 9-6 時刻の変換の往復
        /// <summary>
        /// ミリ秒と時刻の変換は、往復で同じ値になる
        /// </summary>
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(59999)]
        [InlineData(83456)]
        [InlineData(3599999)]
        public void ConvertTiming_RoundTrip_ReturnsSameValue(int ms)
        {
            string formatted = Common.ConvertFormatTiming(ms);
            int result = 0;

            Assert.True(Common.ConvertMsTiming(formatted, ref result));
            Assert.Equal(ms, result);
        }

        // 7-19 リスナップの範囲
        /// <summary>
        /// リスナップの範囲が両方空なら、OK で全体を対象にする
        /// </summary>
        [Fact]
        public void SetResnapTimingData_BothEmptyAndOk_TargetsWholeBeatmap()
        {
            UserInputUtilityData data = new();
            Common.testDialogResult = true;

            int result = UserInputUtilityDataHelper.SetResnapTimingData(data, "", "");

            Assert.Equal(1, result);
            Assert.True(data.isAllNotes);
            Assert.Equal(["I_A-P-4"], MessageCodes);
        }

        /// <summary>
        /// リスナップの範囲が両方空で、キャンセルなら何もしない
        /// </summary>
        [Fact]
        public void SetResnapTimingData_BothEmptyAndCancel_DoesNothing()
        {
            UserInputUtilityData data = new();
            Common.testDialogResult = false;

            int result = UserInputUtilityDataHelper.SetResnapTimingData(data, "", "");

            Assert.Equal(0, result);
        }

        /// <summary>
        /// リスナップの範囲を指定すると、前回の全体指定が残らない
        /// </summary>
        [Fact]
        public void SetResnapTimingData_Range_ClearsPreviousWholeTarget()
        {
            // U13
            UserInputUtilityData data = new() { isAllNotes = true };

            int result = UserInputUtilityDataHelper.SetResnapTimingData(data, "1000", "2000");

            Assert.Equal(1, result);
            Assert.False(data.isAllNotes);
            Assert.Equal(1000, data.resnapTimingFrom);
            Assert.Equal(2000, data.resnapTimingTo);
        }

        /// <summary>
        /// リスナップの範囲が片方だけ空ならエラー
        /// </summary>
        [Fact]
        public void SetResnapTimingData_OneSideEmpty_ShowsError()
        {
            UserInputUtilityData data = new();

            int result = UserInputUtilityDataHelper.SetResnapTimingData(data, "1000", "");

            Assert.Equal(-1, result);
            Assert.Equal(["E_V-EM-4"], MessageCodes);
        }

        // 15 SV を BPM で指定
        /// <summary>
        /// BPM で指定すると、入力した BPM を SV の始点・終点に入れ、相対速度変化は使わない
        /// </summary>
        [Fact]
        public void SetUserInputData_SvBpm_StoresBpmAndDisablesRelative()
        {
            // 15-1 (SV の欄の値は見ない)
            var input = ValidInput();
            input.isSvBpm = true;
            input.svBpmFrom = "240";
            input.svBpmTo = "180.5";
            input.svFrom = "abc";
            input.isRelative = true;
            input.isRelativeSum = true;
            input.relativeSumSvFrom = "0.5";
            input.relativeSumSvTo = "0.5";

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.Empty(MessageCodes);
            Assert.True(data.isSvBpm);
            Assert.Equal(240.0, data.svFrom);
            Assert.Equal(180.5, data.svTo);
            Assert.Equal(Constants.RELATIVE_DISABLE, data.relativeCode);
        }

        /// <summary>
        /// BPM で指定するときの入力エラー (メッセージは「赤線に置く」の見た目 BPM と同じ)
        /// </summary>
        [Theory]
        [InlineData("", "180", "E_V-EM-10")]      // 空
        [InlineData("240", "", "E_V-EM-10")]
        [InlineData("abc", "180", "E_V-T-17")]    // 数値でない
        [InlineData("1,000", "180", "E_V-T-17")]  // カンマ
        [InlineData("0", "180", "E_V-T-18")]      // 0
        [InlineData("240", "-1", "E_V-T-18")]     // 負の値
        public void SetUserInputData_InvalidSvBpm_ShowsError(string bpmFrom, string bpmTo, string expectedCode)
        {
            // 15-2
            var input = ValidInput();
            input.isSvBpm = true;
            input.svBpmFrom = bpmFrom;
            input.svBpmTo = bpmTo;

            var (isSuccess, _) = Validate(input);

            Assert.False(isSuccess);
            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// SV のチェックを外しているときは、BPM で指定のチェックがあっても BPM の入力は見ない
        /// </summary>
        [Fact]
        public void SetUserInputData_SvBpmWithSvUnchecked_IgnoresBpm()
        {
            // 15-2
            var input = ValidInput();
            input.isSv = false;
            input.isSvBpm = true;
            input.svBpmFrom = string.Empty;

            var (isSuccess, data) = Validate(input);

            Assert.True(isSuccess);
            Assert.False(data.isSvBpm);
        }

        // 7-1 ユーティリティのオフセットの欄
        /// <summary>
        /// ユーティリティのオフセットの入力エラー
        /// </summary>
        [Theory]
        [InlineData("", "E_V-EM-9")]     // 空
        [InlineData("abc", "E_V-T-15")]  // 数値でない
        [InlineData("1.5", "E_V-T-16")]  // 整数でない
        public void SetOffsetData_InvalidOffset_ShowsError(string offset, string expectedCode)
        {
            // 7-1・14-17
            var data = new UserInputUtilityData();

            Assert.False(UserInputUtilityDataHelper.SetOffsetData(data, offset));

            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// ユーティリティのオフセットは、整数ならそのまま入る
        /// </summary>
        [Theory]
        [InlineData("100", 100)]
        [InlineData("-20", -20)]
        [InlineData("+15", 15)]
        public void SetOffsetData_ValidOffset_Stored(string offset, int expected)
        {
            // 7-1
            var data = new UserInputUtilityData();

            Assert.True(UserInputUtilityDataHelper.SetOffsetData(data, offset));

            Assert.Empty(MessageCodes);
            Assert.Equal(expected, data.offset);
        }
    }
}
