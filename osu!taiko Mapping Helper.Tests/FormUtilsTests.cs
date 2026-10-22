using System.Drawing;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// 画面の有効・無効のテスト (TEST_PATTERNS.md の 12.5)
    /// </summary>
    /// <remarks>
    /// コントロールの表示(Visible)は、親の画面が表示されていないと正しく読めないため、
    /// 見えない画面(透明・タスクバーに出さない)にコントロールを置いて確かめる
    /// </remarks>
    public class FormUtilsTests : TestEnvironment
    {
        /// <summary>
        /// SV の入力欄まわりのコントロール
        /// </summary>
        private sealed class SvControls : IDisposable
        {
            internal Form Host { get; } = new() { ShowInTaskbar = false, Opacity = 0, StartPosition = FormStartPosition.Manual, Location = new Point(-2000, -2000) };
            internal CheckBox ChkEnableSv { get; } = new() { Checked = true };
            internal CheckBox ChkEnableVolume { get; } = new() { Checked = true };
            internal CheckBox ChkApplyStartObject { get; } = new();
            internal CheckBox ChkApplyEndObject { get; } = new();
            internal CheckBox ChkApplyRedLines { get; } = new();
            internal CheckBox ChkRelative { get; } = new();
            internal CheckBox ChkEnableSvTo { get; } = new();
            internal Panel PnlRelativeSvGroup { get; } = new();
            internal TextBox TxtSvFrom { get; } = new();
            internal TextBox TxtSvTo { get; } = new();
            internal TextBox TxtVolumeFrom { get; } = new();
            internal TextBox TxtVolumeTo { get; } = new();
            internal TextBox TxtRelativeBaseSv { get; } = new();
            internal TextBox TxtOffset { get; } = new();
            internal Button BtnSwapSv { get; } = new();
            internal Button BtnSetSvFrom { get; } = new();
            internal Button BtnSetSvTo { get; } = new();
            internal Button BtnSwapVolume { get; } = new();
            internal Button BtnSetVolumeFrom { get; } = new();
            internal Button BtnSetVolumeTo { get; } = new();
            internal RadioButton RdoArithmetic { get; } = new() { Checked = true };
            internal RadioButton RdoGeometric { get; } = new();

            internal SvControls()
            {
                Host.Controls.AddRange([ChkEnableSv, ChkEnableVolume, ChkApplyStartObject, ChkApplyEndObject, ChkApplyRedLines, ChkRelative, ChkEnableSvTo, PnlRelativeSvGroup,
                                        TxtSvFrom, TxtSvTo, TxtVolumeFrom, TxtVolumeTo, TxtRelativeBaseSv, TxtOffset, BtnSwapSv, BtnSetSvFrom, BtnSetSvTo, BtnSwapVolume, BtnSetVolumeFrom, BtnSetVolumeTo]);
                var radioPanel = new Panel();
                radioPanel.Controls.AddRange([RdoArithmetic, RdoGeometric]);
                Host.Controls.Add(radioPanel);
                Host.Show();
            }

            /// <summary>
            /// 処理項目のタブを切り替えたときの処理 (SetApplyControls)
            /// </summary>
            internal void ApplyControls(bool isEnable, UserInputTempData input, Config config, int tabIndex) =>
                FormUtils.SetApplyControls(isEnable, input, config, tabIndex, ChkEnableSv, ChkEnableVolume, ChkApplyStartObject, ChkApplyEndObject, ChkApplyRedLines,
                                           ChkRelative, PnlRelativeSvGroup, ChkEnableSvTo, TxtSvFrom, TxtSvTo, BtnSwapSv, BtnSetSvFrom, BtnSetSvTo,
                                           TxtVolumeFrom, TxtVolumeTo, BtnSwapVolume, BtnSetVolumeFrom, BtnSetVolumeTo);

            /// <summary>
            /// SV のチェック・相対速度を切り替えたときの処理 (SetApplyContols)
            /// </summary>
            internal void ApplyContols(bool isEnable) =>
                FormUtils.SetApplyContols(isEnable, ChkEnableSv, TxtSvFrom, TxtSvTo, BtnSetSvFrom, BtnSetSvTo, BtnSwapSv, ChkRelative, ChkEnableSvTo);

            public void Dispose() => Host.Dispose();
        }

        /// <summary>
        /// 相対速度と終点 SV を使うかどうかで、SV の入力欄が切り替わる
        /// </summary>
        [Theory]
        [InlineData(false, false, true, true, true)]    // 相対速度 OFF: SV のチェックを外せる、終点 SV も入力できる
        [InlineData(true, false, false, true, false)]   // 相対速度 ON・終点 SV を使わない: SV のチェックは外せず、終点 SV は入力できない
        [InlineData(true, true, false, true, true)]     // 相対速度 ON・終点 SV を使う
        public void SvControls_SwitchByRelativeAndSvTo(bool isRelative, bool isEnableSvTo, bool expectedChkEnableSv, bool expectedSvFrom, bool expectedSvTo)
        {
            // 12.5-1 (U2・U3)
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();
                controls.ChkRelative.Checked = isRelative;
                controls.ChkEnableSvTo.Checked = isEnableSvTo;

                controls.ApplyContols(true);

                Assert.Equal(expectedChkEnableSv, controls.ChkEnableSv.Enabled);
                Assert.Equal(expectedSvFrom, controls.TxtSvFrom.Enabled);
                Assert.Equal(expectedSvTo, controls.TxtSvTo.Enabled);
                Assert.Equal(expectedSvTo, controls.BtnSwapSv.Enabled);
                Assert.Equal(expectedSvTo, controls.BtnSetSvTo.Enabled);
            });
        }

        /// <summary>
        /// SV のチェックを外すと、SV の入力欄は全部入力できない
        /// </summary>
        [Fact]
        public void SvControls_SvUnchecked_AllDisabled()
        {
            // 12.5-1
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();
                controls.ChkEnableSv.Checked = false;

                controls.ApplyContols(true);

                Assert.True(controls.ChkEnableSv.Enabled);
                Assert.False(controls.TxtSvFrom.Enabled);
                Assert.False(controls.TxtSvTo.Enabled);
                Assert.False(controls.BtnSetSvFrom.Enabled);
            });
        }

        /// <summary>
        /// 相対速度を ON にすると、相対速度の設定欄が出て、計算方法と SV のチェックは選べない
        /// </summary>
        [Fact]
        public void SvControls_RelativeOn_ShowsRelativeSettingsAndLocksOthers()
        {
            // 12.5-1
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();
                var input = new UserInputTempData { isRelativeMultiply = true, relativeMultiplySvFrom = "0.5", relativeMultiplySvTo = "0.8", svFrom = "1", svTo = "2" };

                FormUtils.SetRelativeEnabledState(true, input, controls.PnlRelativeSvGroup, controls.ChkEnableSvTo, controls.ChkEnableSv, controls.RdoArithmetic, controls.RdoGeometric, controls.TxtSvFrom, controls.TxtSvTo);

                Assert.True(controls.PnlRelativeSvGroup.Visible);
                Assert.True(controls.ChkEnableSvTo.Visible);
                Assert.False(controls.ChkEnableSv.Enabled);
                Assert.False(controls.RdoArithmetic.Enabled);
                Assert.False(controls.RdoGeometric.Enabled);
                Assert.Equal("0.5", controls.TxtSvFrom.Text);
                Assert.Equal("0.8", controls.TxtSvTo.Text);

                FormUtils.SetRelativeEnabledState(false, input, controls.PnlRelativeSvGroup, controls.ChkEnableSvTo, controls.ChkEnableSv, controls.RdoArithmetic, controls.RdoGeometric, controls.TxtSvFrom, controls.TxtSvTo);

                Assert.False(controls.PnlRelativeSvGroup.Visible);
                Assert.True(controls.ChkEnableSv.Enabled);
                Assert.True(controls.RdoArithmetic.Enabled);
                Assert.Equal("1", controls.TxtSvFrom.Text);
                Assert.Equal("2", controls.TxtSvTo.Text);
            });
        }

        /// <summary>
        /// 処理項目のタブで、SV の入力欄が切り替わる
        /// </summary>
        [Theory]
        [InlineData(0, true)]    // オブジェクト
        [InlineData(1, true)]    // ビートスナップ
        [InlineData(2, true)]    // 緑線
        [InlineData(3, false)]   // 赤線 (SV は見た目 BPM で決まる)
        public void SvControls_SwitchByTab(int tabIndex, bool expectedSv)
        {
            // 12.5-2 (U3・U4)
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();

                controls.ApplyControls(true, new UserInputTempData(), new Config(), tabIndex);

                Assert.Equal(expectedSv, controls.ChkEnableSv.Enabled);
                Assert.Equal(expectedSv, controls.TxtSvFrom.Enabled);
                Assert.Equal(expectedSv, controls.TxtSvTo.Enabled);
                Assert.True(controls.TxtVolumeFrom.Enabled);
            });
        }

        /// <summary>
        /// 削除のタブでは、SV と音量の入力欄が全部入力できない
        /// </summary>
        [Fact]
        public void RemoveTab_DisablesSvAndVolumeControls()
        {
            // 12.5-2 (U4)
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();

                controls.ApplyControls(false, new UserInputTempData(), new Config(), 0);

                Assert.False(controls.ChkEnableSv.Enabled);
                Assert.False(controls.TxtSvFrom.Enabled);
                Assert.False(controls.TxtSvTo.Enabled);
                Assert.False(controls.ChkEnableVolume.Enabled);
                Assert.False(controls.TxtVolumeFrom.Enabled);
                Assert.False(controls.PnlRelativeSvGroup.Visible);
            });
        }

        /// <summary>
        /// 「赤線にも音量を適用」のチェックは、音量が有効なときだけ出る
        /// </summary>
        [Theory]
        [InlineData(true, true, true)]
        [InlineData(false, true, false)]
        [InlineData(true, false, false)]
        public void ApplyRedLinesCheckBox_ShownOnlyWhenVolumeEnabled(bool isVolume, bool isEnable, bool expectedVisible)
        {
            // 12.5-2
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();

                controls.ApplyControls(isEnable, new UserInputTempData { isVolume = isVolume }, new Config(), 0);

                Assert.Equal(expectedVisible, controls.ChkApplyRedLines.Visible);
            });
        }

        /// <summary>
        /// 相対速度のチェックは、詳細モードで赤線以外のタブのときだけ出る
        /// </summary>
        [Theory]
        [InlineData(1, 0, true)]
        [InlineData(0, 0, false)]   // 詳細モードでない
        [InlineData(1, 3, false)]   // 赤線のタブ
        public void RelativeCheckBox_ShownOnlyInAdvanceModeExceptRedLineTab(int advanceMode, int tabIndex, bool expectedVisible)
        {
            // 12.5-2
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();

                controls.ApplyControls(true, new UserInputTempData(), new Config { advanceMode = advanceMode }, tabIndex);

                Assert.Equal(expectedVisible, controls.ChkRelative.Visible);
            });
        }

        /// <summary>
        /// ms オフセットの入力欄の有効・無効と色
        /// </summary>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void OffsetTextBox_EnabledStateAndColor(bool isEnable)
        {
            // 12.5-3 (U6)
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();

                FormUtils.SetChkEnableOffset(isEnable, controls.TxtOffset);

                Assert.Equal(isEnable, controls.TxtOffset.Enabled);
                Assert.Equal(isEnable ? SystemColors.Window : SystemColors.WindowFrame, controls.TxtOffset.BackColor);
            });
        }

        /// <summary>
        /// SV の入れ替えボタンの文字色
        /// </summary>
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void SwapSvButton_ForeColor(bool isEnable)
        {
            // 12.5-4 (U7)
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();

                FormUtils.SetBtnSwapSvEnabledState(isEnable, controls.BtnSwapSv);

                Assert.Equal(isEnable, controls.BtnSwapSv.Enabled);
                Assert.Equal(isEnable ? Color.Cyan : SystemColors.WindowFrame, controls.BtnSwapSv.ForeColor);
            });
        }

        /// <summary>
        /// 乗算モードでは基準 SV の欄が入力でき、乗算の値が入る
        /// </summary>
        [Fact]
        public void MultiplyMode_EnablesBaseSvAndFillsValues()
        {
            // 12.5-5
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();
                var input = new UserInputTempData { relativeMultiplySvFrom = "0.5", relativeMultiplySvTo = "0.8" };

                FormUtils.SetRelativeMultiplyEnableState(true, input, controls.TxtRelativeBaseSv, controls.TxtSvFrom, controls.TxtSvTo);
                Assert.True(controls.TxtRelativeBaseSv.Enabled);
                Assert.Equal("0.5", controls.TxtSvFrom.Text);
                Assert.Equal("0.8", controls.TxtSvTo.Text);

                FormUtils.SetRelativeMultiplyEnableState(false, input, controls.TxtRelativeBaseSv, controls.TxtSvFrom, controls.TxtSvTo);
                Assert.False(controls.TxtRelativeBaseSv.Enabled);
            });
        }

        /// <summary>
        /// BPM で指定する場合は、SV の入力欄に BPM で入力した値を入れる
        /// </summary>
        [Fact]
        public void SvBpm_FillsBpmValuesIntoSvTextBoxes()
        {
            // 15-8
            TestReflection.RunInSta(() =>
            {
                using var controls = new SvControls();
                var input = new UserInputTempData { isSvBpm = true, svBpmFrom = "240", svBpmTo = "300", svFrom = "1", svTo = "2" };

                FormUtils.SetTxtSvFromEnabledState(true, input, controls.TxtSvFrom);
                Assert.Equal("240", controls.TxtSvFrom.Text);

                // 相対速度変化のチェックが外れたときも、BPM の値に戻す
                controls.TxtSvFrom.Text = string.Empty;
                FormUtils.SetRelativeEnabledState(false, input, controls.PnlRelativeSvGroup, controls.ChkEnableSvTo, controls.ChkEnableSv, controls.RdoArithmetic, controls.RdoGeometric, controls.TxtSvFrom, controls.TxtSvTo);
                Assert.Equal("240", controls.TxtSvFrom.Text);
                Assert.Equal("300", controls.TxtSvTo.Text);
            });
        }
    }
}
