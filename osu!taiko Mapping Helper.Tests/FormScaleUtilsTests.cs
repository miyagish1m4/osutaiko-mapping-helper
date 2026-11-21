using System.Drawing;
using osu_taiko_Mapping_Helper.Utils;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// 画面の大きさを Windows の拡大率とモニターの大きさに合わせる処理のテスト (TEST_PATTERNS.md の 16)
    /// </summary>
    /// <remarks>
    /// メイン画面そのものは osu! を読む処理などが動くため、ここでは同じ種類の部品を持つ見本の画面で確かめる
    /// (メイン画面の見た目は手動で確かめる)
    /// </remarks>
    public class FormScaleUtilsTests : TestEnvironment
    {
        /// <summary>
        /// 縮める割合は、入りきる場合は 1、入りきらない場合は中身が入りきる割合になる
        /// </summary>
        [Theory]
        [InlineData(1920, 1032, 1.0)]                        // FullHD・100%: 入りきる
        [InlineData(1366, 728, (728.0 - 39) / 963)]          // 1366×768: 高さで決まる
        [InlineData(1600, 860, (860.0 - 39) / 963)]          // 1600×900
        [InlineData(300, 1032, (300.0 - 16) / 408)]          // 幅が足りない: 幅で決まる
        [InlineData(300, 728, (300.0 - 16) / 408)]           // 幅も高さも足りない: 小さい方
        public void GetFitScale_ReturnsScaleToFitWorkingArea(int areaWidth, int areaHeight, double expected)
        {
            // 16-1 (メイン画面と同じ大きさ: 枠込み 424×1002、中身 408×963。枠は縮まないので中身で計算する)
            float scale = FormScaleUtils.GetFitScale(new Size(424, 1002), new Size(408, 963), new Rectangle(0, 0, areaWidth, areaHeight));

            Assert.Equal(expected, scale, 4);
        }

        /// <summary>
        /// 中身の大きさが取れない場合や、作業領域が極端に小さい場合でも、画面が無くならない
        /// </summary>
        [Theory]
        [InlineData(0, 0, 100, 100, 1.0)]     // 中身の大きさが 0
        [InlineData(408, 963, 10, 10, 0.1)]    // 下限 (0.1)
        public void GetFitScale_InvalidSizes_ReturnsSafeScale(int clientWidth, int clientHeight, int areaWidth, int areaHeight, double expected)
        {
            // 16-1
            float scale = FormScaleUtils.GetFitScale(new Size(clientWidth + 16, clientHeight + 39), new Size(clientWidth, clientHeight), new Rectangle(0, 0, areaWidth, areaHeight));

            Assert.Equal(expected, scale, 4);
        }

        /// <summary>
        /// 見本の画面 (メイン画面と同じ種類の部品を持つ)
        /// </summary>
        private sealed class SampleForm : Form
        {
            internal Panel Panel { get; } = new() { Location = new Point(10, 20), Size = new Size(200, 100) };
            internal Button Button { get; } = new() { Location = new Point(10, 10), Size = new Size(80, 30), Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold) };
            internal Label InheritedLabel { get; } = new() { Location = new Point(100, 10), Size = new Size(60, 20) };
            internal CheckBox CheckBox { get; } = new() { Location = new Point(10, 200), Size = new Size(100, 24), Font = new Font("Yu Gothic UI", 8F) };
            internal TabControl TabControl { get; } = new() { Location = new Point(10, 240), Size = new Size(200, 100), SizeMode = TabSizeMode.Fixed, ItemSize = new Size(98, 40) };
            internal PictureBox PictureBox { get; } = new() { Location = new Point(220, 20), Size = new Size(100, 60) };
            internal MenuStrip MenuStrip { get; } = new();
            internal ToolStripMenuItem MenuItem { get; } = new("Menu") { Font = new Font("Yu Gothic UI", 10F) };

            internal SampleForm()
            {
                // メイン画面と同じく、WinForms の自動の拡大は使わない
                AutoScaleMode = AutoScaleMode.None;
                ClientSize = new Size(400, 400);
                Panel.Controls.Add(Button);
                Panel.Controls.Add(InheritedLabel);
                TabControl.TabPages.AddRange([new TabPage("A"), new TabPage("B")]);
                MenuStrip.Items.Add(MenuItem);
                Controls.AddRange([Panel, CheckBox, TabControl, PictureBox, MenuStrip]);
                // Helper は起動時に文字の描き方の初期値を GDI にしている (ApplicationConfiguration.Initialize) ため、同じ状態にする
                Button.UseCompatibleTextRendering = false;
                CheckBox.UseCompatibleTextRendering = false;
            }
        }

        /// <summary>
        /// 画面を縮めると、部品の位置・大きさ・文字・タブの見出し・画像・ボタンの描き方が縮めた割合に合わせて変わる
        /// </summary>
        [Fact]
        public void ScaleForm_ScalesBoundsFontsTabsPicturesAndButtons()
        {
            // 16-2
            TestReflection.RunInSta(() =>
            {
                using var form = new SampleForm();
                float formFont = form.Font.SizeInPoints;
                float labelFont = form.InheritedLabel.Font.SizeInPoints;

                FormScaleUtils.ScaleForm(form, 0.5f);

                // 位置と大きさ
                Assert.Equal(new Size(200, 200), form.ClientSize);
                Assert.Equal(new Rectangle(5, 10, 100, 50), form.Panel.Bounds);
                Assert.Equal(new Rectangle(5, 5, 40, 15), form.Button.Bounds);
                // 文字は部品より少し強めに縮める (部品の割合 × fontScaleRatio)
                float fontScale = 0.5f * FormScaleUtils.fontScaleRatio;
                Assert.Equal(10F * fontScale, form.Button.Font.SizeInPoints, 2);
                Assert.Equal(FontStyle.Bold, form.Button.Font.Style);
                Assert.Equal(8F * fontScale, form.CheckBox.Font.SizeInPoints, 2);
                Assert.Equal(10F * fontScale, form.MenuItem.Font.SizeInPoints, 2);
                // 親の文字をそのまま使っている部品も、二重に縮まない
                Assert.Equal(labelFont * fontScale, form.InheritedLabel.Font.SizeInPoints, 2);
                // 画面そのものの文字は変えない (変えると、部品の大きさが自動で変わるため)
                Assert.Equal(formFont, form.Font.SizeInPoints, 2);
                // タブの見出しは、切り捨ててから幅だけ 1 ピクセル余裕を持たせる
                Assert.Equal(new Size(48, 20), form.TabControl.ItemSize);
                // 画像は枠に合わせて縮めて表示する
                Assert.Equal(PictureBoxSizeMode.Zoom, form.PictureBox.SizeMode);
                // ボタン・チェックボックスは、文字の下が切れない描き方にする
                Assert.True(form.Button.UseCompatibleTextRendering);
                Assert.True(form.CheckBox.UseCompatibleTextRendering);
            });
        }

        /// <summary>
        /// 作業領域に入りきる場合は、画面を変えない
        /// </summary>
        [Fact]
        public void FitToWorkingArea_Fits_DoesNothing()
        {
            // 16-3 (FullHD・100% では今の見た目のまま)
            TestReflection.RunInSta(() =>
            {
                using var form = new SampleForm();
                var before = form.Size;
                float buttonFont = form.Button.Font.SizeInPoints;

                float scale = FormScaleUtils.FitToWorkingArea(form, new Rectangle(0, 0, 1920, 1032));

                Assert.Equal(1f, scale);
                Assert.Equal(before, form.Size);
                Assert.Equal(buttonFont, form.Button.Font.SizeInPoints);
                Assert.False(form.Button.UseCompatibleTextRendering);
                Assert.Equal(PictureBoxSizeMode.Normal, form.PictureBox.SizeMode);
            });
        }

        /// <summary>
        /// 作業領域に入りきらない場合は、画面が作業領域に入りきる大きさに縮み、部品も画面の中に収まる
        /// </summary>
        [Fact]
        public void FitToWorkingArea_DoesNotFit_ShrinksIntoWorkingArea()
        {
            // 16-3
            TestReflection.RunInSta(() =>
            {
                using var form = new SampleForm();
                var area = new Rectangle(0, 0, 1366, 300);

                float scale = FormScaleUtils.FitToWorkingArea(form, area);

                Assert.True(scale < 1f);
                Assert.True(form.Height <= area.Height, $"画面の高さ {form.Height} が作業領域 {area.Height} を超えている");
                Assert.All(form.Controls.Cast<Control>(), control =>
                    Assert.True(control.Right <= form.ClientSize.Width && control.Bottom <= form.ClientSize.Height, $"{control.GetType().Name} が画面からはみ出している: {control.Bounds}"));
            });
        }

        /// <summary>
        /// 拡大率が 100% でなく、拡大しても入りきる場合は、部品を拡大率の分だけ大きくし、文字は変えない
        /// </summary>
        [Fact]
        public void ScaleForDisplay_HighDpiFits_EnlargesBoundsOnly()
        {
            // 16-5 (4K・150% など。文字は Windows が拡大率の分だけ大きく描くため、文字の大きさは変えない)
            TestReflection.RunInSta(() =>
            {
                using var form = new SampleForm();
                float buttonFont = form.Button.Font.SizeInPoints;

                float scale = FormScaleUtils.ScaleForDisplay(form, 1.5f, new Rectangle(0, 0, 3840, 2100));

                Assert.Equal(1.5f, scale);
                Assert.Equal(new Size(600, 600), form.ClientSize);
                Assert.Equal(new Rectangle(15, 30, 300, 150), form.Panel.Bounds);
                Assert.Equal(buttonFont, form.Button.Font.SizeInPoints, 2);
                // タブの見出しも、部品と同じ割合で大きくする (WinForms は大きくしない)
                Assert.Equal(new Size(146, 60), form.TabControl.ItemSize);
                // 大きくするだけなので、ボタンの文字の描き方は変えない
                Assert.False(form.Button.UseCompatibleTextRendering);
            });
        }

        /// <summary>
        /// 拡大率が 100% でなく、拡大すると入りきらない場合は、部品は「拡大率 × 入りきる割合」、文字は「入りきる割合」で変わる
        /// </summary>
        [Fact]
        public void ScaleForDisplay_HighDpiDoesNotFit_ScalesBoundsAndShrinksFonts()
        {
            // 16-5 (FullHD・150% など)
            TestReflection.RunInSta(() =>
            {
                using var form = new SampleForm();
                var area = new Rectangle(0, 0, 1920, 400);
                int frameHeight = form.Height - form.ClientSize.Height;
                // 150% の中身は 600×600。入りきる割合は (400 − 枠の高さ) ÷ 600
                float fit = (400f - frameHeight) / 600f;

                float scale = FormScaleUtils.ScaleForDisplay(form, 1.5f, area);

                Assert.Equal(1.5f * fit, scale, 3);
                Assert.True(form.Height <= area.Height, $"画面の高さ {form.Height} が作業領域 {area.Height} を超えている");
                Assert.Equal(10F * fit * FormScaleUtils.fontScaleRatio, form.Button.Font.SizeInPoints, 2);
                Assert.True(form.Button.UseCompatibleTextRendering);
            });
        }
    }
}
