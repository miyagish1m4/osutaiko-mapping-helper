using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// MainForm の中にある処理の流れのテスト (TEST_PATTERNS.md の 7-3・7-22・7-23)
    /// </summary>
    /// <remarks>
    /// MainForm はコンストラクタで osu! の監視スレッドを動かすため、コンストラクタを通さずに作り、
    /// 処理で使うフィールド(選択中の譜面・入力値・テキストボックス)だけを設定してボタンの処理を呼ぶ
    /// </remarks>
    public class MainFormFlowTests : TestEnvironment
    {
        private string SetDirectory => Path.Combine(WorkDirectory, "Songs", "1 test");

        /// <summary>
        /// セットのフォルダに diff を作る
        /// </summary>
        private string CreateDiff(string version, OsuFileBuilder? builder = null)
        {
            Directory.CreateDirectory(SetDirectory);
            builder ??= new OsuFileBuilder();
            builder.Metadata = [.. builder.Metadata.Select(l => l.StartsWith("Version:") ? $"Version:{version}" : l)];
            string path = Path.Combine(SetDirectory, $"a - t (m) [{version}].osu");
            File.WriteAllText(path, string.Join("\r\n", builder.Build()) + "\r\n");
            return path;
        }

        /// <summary>
        /// 処理で使うフィールドだけを持つ MainForm を作る (osu! で beatmapPath の譜面を選んでいる状態)
        /// </summary>
        private static MainForm CreateForm(string beatmapPath)
        {
            var form = TestReflection.CreateWithoutConstructor<MainForm>();
            TestReflection.SetField(form, "beatmapInfo", new BeatmapMetadata { beatmapPath = beatmapPath });
            TestReflection.SetField(form, "targetBeatmapInfo", new BeatmapMetadata());
            TestReflection.SetField(form, "targetBackupDirectoryName", string.Empty);
            TestReflection.SetField(form, "userInputData", new UserInputData());
            TestReflection.SetField(form, "userInputTempData", new UserInputTempData());
            TestReflection.SetField(form, "userInputUtilityData", new UserInputUtilityData());
            TestReflection.SetField(form, "timingPoints", new List<TimingPoint>());
            TestReflection.SetField(form, "config", new Config { maxBackupCount = 5 });
            TestReflection.SetField(form, "txtTags", new TextBox { Multiline = true });
            TestReflection.SetField(form, "txtTimingOffset", new TextBox());
            return form;
        }

        private static void Click(MainForm form, string handler) => TestReflection.Invoke(form, handler, form, EventArgs.Empty);

        private bool HasBackup => Directory.Exists(Path.Combine(WorkDirectory, "Backup")) &&
                                  Directory.EnumerateFiles(Path.Combine(WorkDirectory, "Backup"), "*", SearchOption.AllDirectories).Any();

        #region 7-3 タグ編集
        /// <summary>
        /// タグの改行は除いて、1 行で書く
        /// </summary>
        [Theory]
        [InlineData("a b\r\nc", "Tags:a bc")]
        [InlineData("a\nb\r", "Tags:ab")]
        [InlineData("\r\n", "Tags:")]
        public void ApplyTags_RemovesLineBreaks(string text, string expected)
        {
            // 7-3 (U12。同じセットの diff も同じタグになる)
            string oni = CreateDiff("Oni");
            string futsuu = CreateDiff("Futsuu");

            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(oni);
                ((TextBox)TestReflection.GetField(form, "txtTags")!).Text = text;
                Click(form, "btnApplyTags_Click");
            });

            Assert.Contains(expected, File.ReadAllLines(oni));
            Assert.Contains(expected, File.ReadAllLines(futsuu));
            Assert.Single(File.ReadAllLines(oni), l => l.StartsWith("Tags:"));
            Assert.Equal(["I_A-P-1"], MessageCodes);
        }
        #endregion

        #region 7-23 何もしない条件
        /// <summary>
        /// タグ編集で譜面を選んでいなければ、メッセージを出さずに何もしない
        /// </summary>
        [Fact]
        public void ApplyTags_NoBeatmapSelected_DoesNothingSilently()
        {
            // 7-23 今の動きを固定する
            string oni = CreateDiff("Oni");
            byte[] before = File.ReadAllBytes(oni);

            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(string.Empty);
                ((TextBox)TestReflection.GetField(form, "txtTags")!).Text = "a";
                Click(form, "btnApplyTags_Click");
            });

            Assert.Equal(before, File.ReadAllBytes(oni));
            Assert.Empty(MessageCodes);
            Assert.False(HasBackup);
        }

        /// <summary>
        /// オフセットで譜面を選んでいなければ、譜面情報の取得に失敗したメッセージを出す
        /// </summary>
        [Fact]
        public void ApplyOffset_NoBeatmapSelected_ShowsLoadError()
        {
            // 7-23 今の動きを固定する (ボタンから実行すると、先に譜面の読み込みで止まる)
            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(string.Empty);
                ((TextBox)TestReflection.GetField(form, "txtTimingOffset")!).Text = "100";
                Click(form, "btnApplyOffset_Click");
            });

            Assert.Equal(["E_A-D-1"], MessageCodes);
            Assert.False(HasBackup);
        }

        /// <summary>
        /// オフセットの処理は、譜面のパスが無ければメッセージを出さずに何もしない
        /// </summary>
        [Fact]
        public void ExecuteUtilityProcess_OffsetWithoutPath_DoesNothingSilently()
        {
            // 7-23 今の動きを固定する (ユーティリティの処理そのもの)
            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(string.Empty);
                ((UserInputUtilityData)TestReflection.GetField(form, "userInputUtilityData")!).utilityCode = Constants.UTILITY_OFFSET;
                ((TextBox)TestReflection.GetField(form, "txtTimingOffset")!).Text = "100";
                TestReflection.Invoke(form, "ExecuteUtilityProcess");
            });

            Assert.Empty(MessageCodes);
            Assert.False(HasBackup);
        }

        /// <summary>
        /// 設定コピーで選択中の diff が読めなければ、何も書き換えない
        /// </summary>
        [Fact]
        public void ApplySettingCopier_UnreadableSelectedDiff_ChangesNothing()
        {
            // 7-23 今の動きを固定する (ボタンから実行すると、譜面情報の取得に失敗したメッセージを出す)
            string broken = CreateDiff("Broken", new OsuFileBuilder { HitObjects = ["480,80,5371,1,6"] });
            string oni = CreateDiff("Oni");
            byte[] before = File.ReadAllBytes(oni);

            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(broken);
                ((UserInputUtilityData)TestReflection.GetField(form, "userInputUtilityData")!).settingCopierCode = Constants.SETTING_COPIER_METADATA;
                Click(form, "btnApplySettingCopier_Click");
            });

            Assert.Equal(before, File.ReadAllBytes(oni));
            Assert.Equal(["E_A-D-1"], MessageCodes);
            Assert.False(HasBackup);
        }

        /// <summary>
        /// 設定コピーの処理は、選択中の diff の情報が無ければメッセージを出さずに何もしない
        /// </summary>
        [Fact]
        public void ExecuteUtilityProcess_SettingCopierWithoutBeatmap_DoesNothingSilently()
        {
            // 7-23 今の動きを固定する (ユーティリティの処理そのもの)
            string oni = CreateDiff("Oni");
            string muzukashii = CreateDiff("Muzukashii");
            byte[] before = File.ReadAllBytes(muzukashii);

            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(oni);
                TestReflection.Invoke(form, "SetTargetBeatmap");
                var data = (UserInputUtilityData)TestReflection.GetField(form, "userInputUtilityData")!;
                data.utilityCode = Constants.UTILITY_SETTING_COPIER;
                data.settingCopierCode = Constants.SETTING_COPIER_METADATA;
                TestReflection.Invoke(form, "ExecuteUtilityProcess");
            });

            Assert.Equal(before, File.ReadAllBytes(muzukashii));
            Assert.Empty(MessageCodes);
            Assert.False(HasBackup);
        }
        #endregion

        #region 7-22 処理の順番
        /// <summary>
        /// エラーにならない入力 (オブジェクトに置く、1000〜2000ms、SV 1、音量 50、オフセットなし)
        /// </summary>
        private static UserInputTempData ValidInput() => new()
        {
            timingFrom = "1000",
            timingTo = "2000",
            svFrom = "1",
            svTo = "1",
            volumeFrom = "50",
            volumeTo = "50",
            isOffset = false,
        };

        /// <summary>
        /// 計算後の値 (オブジェクトに置く・全てのヒットオブジェクト・SV 2.0・始点と終点を含む)
        /// </summary>
        private static UserInputData CalculatedInput(int from, int to, double sv)
        {
            UserInputData data = new()
            {
                timingFrom = from,
                timingTo = to,
                applySetCode = 0x1,
                isSv = true,
                svFrom = sv,
                svTo = sv,
                calculationCode = Constants.CALCULATION_ARITHMETIC,
                relativeCode = Constants.RELATIVE_DISABLE,
            };
            data.setObjectOption.setObjectsCode = 0x37f;
            data.setObjectOption.isTimingStart = true;
            data.setObjectOption.isTimingEnd = true;
            return data;
        }

        /// <summary>
        /// 譜面を読み込んでから、計算以降の処理を実行する (btnApply_Click の入力チェックの後と同じ)
        /// </summary>
        private static void LoadAndExecute(string beatmapPath, UserInputData data)
        {
            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(beatmapPath);
                Assert.True((bool)TestReflection.Invoke(form, "GetBeatmap")!);
                TestReflection.SetField(form, "userInputData", data);
                TestReflection.Invoke(form, "ExecuteSvEditProcess", Constants.EXECUTE_APPLY);
            });
        }

        /// <summary>
        /// 成功したときは、バックアップを作ってから書き出し、最後に完了メッセージを出す
        /// </summary>
        [Fact]
        public void Apply_Success_BacksUpThenExportsThenShowsCompletion()
        {
            // 7-22 (バックアップは書き換える前の中身)
            string oni = CreateDiff("Oni");
            byte[] before = File.ReadAllBytes(oni);

            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(oni);
                TestReflection.SetField(form, "userInputTempData", ValidInput());
                Click(form, "btnApply_Click");
            });

            Assert.NotEqual(before, File.ReadAllBytes(oni));
            var backups = BeatmapHelper.GetBackupFiles(BeatmapHelper.GetBackupDirectoryPath(Path.GetFileNameWithoutExtension(oni)));
            Assert.Equal(before, File.ReadAllBytes(Assert.Single(backups)));
            Assert.Equal(["I_A-P-1"], MessageCodes);
        }

        /// <summary>
        /// 入力チェックで止まったときは、譜面を変えない
        /// </summary>
        [Fact]
        public void Apply_InvalidInput_KeepsBeatmap()
        {
            // 7-22
            string oni = CreateDiff("Oni");
            byte[] before = File.ReadAllBytes(oni);
            var input = ValidInput();
            input.timingFrom = string.Empty;

            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(oni);
                TestReflection.SetField(form, "userInputTempData", input);
                Click(form, "btnApply_Click");
            });

            Assert.Equal(before, File.ReadAllBytes(oni));
            Assert.False(HasBackup);
            Assert.Equal(["E_V-EM-4"], MessageCodes);
        }

        /// <summary>
        /// 譜面の読み込みで止まったときは、譜面を変えない
        /// </summary>
        [Fact]
        public void Apply_UnreadableBeatmap_KeepsBeatmap()
        {
            // 7-22
            string broken = CreateDiff("Broken", new OsuFileBuilder { HitObjects = ["480,80,5371,1,6"] });
            byte[] before = File.ReadAllBytes(broken);

            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(broken);
                TestReflection.SetField(form, "userInputTempData", ValidInput());
                Click(form, "btnApply_Click");
            });

            Assert.Equal(before, File.ReadAllBytes(broken));
            Assert.False(HasBackup);
            Assert.Equal(["E_A-D-1"], MessageCodes);
        }

        /// <summary>
        /// 計算で SV が 0 以下になるときは、譜面を変えずに中止する
        /// </summary>
        [Fact]
        public void Apply_SvNotPositive_AbortsWithoutChanges()
        {
            // 7-22・5-7 (S6。相対速度の加算で SV0.5 の区間に -1 を足す)
            var builder = new OsuFileBuilder { TimingPoints = ["0,500,4,1,0,50,1,0", "1000,-66.6666666666667,4,1,0,50,0,0", "2000,-200,4,1,0,50,0,0"] };
            string oni = CreateDiff("Oni", builder);
            byte[] before = File.ReadAllBytes(oni);
            var data = CalculatedInput(2000, 2500, -1.0);
            data.relativeCode = Constants.RELATIVE_SUM;

            LoadAndExecute(oni, data);

            Assert.Equal(before, File.ReadAllBytes(oni));
            Assert.False(HasBackup);
            Assert.Equal(["E_V-C-7"], MessageCodes);
        }

        /// <summary>
        /// バックアップを作れないときは、譜面を変えない
        /// </summary>
        [Fact]
        public void Apply_BackupFailure_KeepsBeatmap()
        {
            // 7-22 (バックアップのフォルダと同じ名前のファイルを置いて、フォルダを作れなくする)
            string oni = CreateDiff("Oni");
            byte[] before = File.ReadAllBytes(oni);
            string backupDirectory = BeatmapHelper.GetBackupDirectoryPath(Path.GetFileNameWithoutExtension(oni));
            Directory.CreateDirectory(Path.GetDirectoryName(backupDirectory)!);
            File.WriteAllText(backupDirectory, string.Empty);

            LoadAndExecute(oni, CalculatedInput(1000, 2000, 2.0));

            Assert.Equal(before, File.ReadAllBytes(oni));
            Assert.Equal(["E_A-P-1"], MessageCodes);
        }

        /// <summary>
        /// 書き出しに失敗したときは、完了メッセージを出さない
        /// </summary>
        [Fact]
        public void Apply_ExportFailure_DoesNotShowCompletion()
        {
            // 7-22 (読み取り専用にして書き出せなくする)
            string oni = CreateDiff("Oni");
            byte[] before = File.ReadAllBytes(oni);
            File.SetAttributes(oni, FileAttributes.ReadOnly);
            try
            {
                LoadAndExecute(oni, CalculatedInput(1000, 2000, 2.0));

                Assert.Equal(before, File.ReadAllBytes(oni));
                Assert.Equal(["E_A-P-1"], MessageCodes);
            }
            finally
            {
                File.SetAttributes(oni, FileAttributes.Normal);
            }
        }
        #endregion

        #region 15 SV を BPM で指定
        /// <summary>
        /// BPM で指定のチェックまわりのコントロールを持つ MainForm を作る
        /// </summary>
        /// <param name="executeTabIndex">適用(0)・削除(1)のタブ</param>
        /// <param name="setTypeTabIndex">処理項目のタブ (0:オブジェクト 1:ビートスナップ 2:緑線 3:赤線)</param>
        private static MainForm CreateSvBpmForm(int executeTabIndex = 0, int setTypeTabIndex = 0, string beatmapPath = "")
        {
            var form = CreateForm(beatmapPath);
            var executeTab = new TabControl();
            executeTab.TabPages.AddRange([new TabPage(), new TabPage()]);
            executeTab.SelectedIndex = executeTabIndex;
            var setTypeTab = new TabControl();
            setTypeTab.TabPages.AddRange([new TabPage(), new TabPage(), new TabPage(), new TabPage()]);
            setTypeTab.SelectedIndex = setTypeTabIndex;
            TestReflection.SetField(form, "tabExecuteType", executeTab);
            TestReflection.SetField(form, "tabSetType", setTypeTab);
            TestReflection.SetField(form, "chkEnableSv", new CheckBox { Checked = true });
            TestReflection.SetField(form, "chkSvBpm", new CheckBox());
            TestReflection.SetField(form, "chkRelative", new CheckBox());
            TestReflection.SetField(form, "txtSvFrom", new TextBox());
            TestReflection.SetField(form, "txtSvTo", new TextBox());
            TestReflection.SetField(form, "btnSetSvFrom", new Button());
            TestReflection.SetField(form, "btnSetSvTo", new Button());
            return form;
        }

        private static T GetControl<T>(MainForm form, string name) => (T)TestReflection.GetField(form, name)!;

        private static UserInputTempData TempData(MainForm form) => GetControl<UserInputTempData>(form, "userInputTempData");

        /// <summary>
        /// BPM で指定を ON にすると、相対速度変化のチェックが外れて選べなくなり、入力欄は BPM の値に、ボタンは「Set Start BPM」「Set End BPM」になる。OFF に戻すと SV に戻る
        /// </summary>
        [Fact]
        public void SvBpm_Toggle_DisablesRelativeAndSwitchesValues()
        {
            // 15-8
            TestReflection.RunInSta(() =>
            {
                var form = CreateSvBpmForm();
                var temp = TempData(form);
                temp.svFrom = "1";
                temp.svTo = "2";
                temp.svBpmFrom = "240";
                temp.svBpmTo = "300";
                var chkRelative = GetControl<CheckBox>(form, "chkRelative");
                chkRelative.Checked = true;

                GetControl<CheckBox>(form, "chkSvBpm").Checked = true;
                Click(form, "chkSvBpm_CheckedChanged");

                Assert.True(temp.isSvBpm);
                Assert.False(chkRelative.Checked);
                Assert.False(chkRelative.Enabled);
                Assert.Equal("240", GetControl<TextBox>(form, "txtSvFrom").Text);
                Assert.Equal("300", GetControl<TextBox>(form, "txtSvTo").Text);
                Assert.Equal("Set Start BPM", GetControl<Button>(form, "btnSetSvFrom").Text);
                Assert.Equal("Set End BPM", GetControl<Button>(form, "btnSetSvTo").Text);

                GetControl<CheckBox>(form, "chkSvBpm").Checked = false;
                Click(form, "chkSvBpm_CheckedChanged");

                Assert.False(temp.isSvBpm);
                Assert.True(chkRelative.Enabled);
                Assert.Equal("1", GetControl<TextBox>(form, "txtSvFrom").Text);
                Assert.Equal("2", GetControl<TextBox>(form, "txtSvTo").Text);
                Assert.Equal("Set Start SV", GetControl<Button>(form, "btnSetSvFrom").Text);
                Assert.Equal("Set End SV", GetControl<Button>(form, "btnSetSvTo").Text);
            });
        }

        /// <summary>
        /// BPM で指定しているときに入力した値は、SV とは別に覚える
        /// </summary>
        [Fact]
        public void SvBpm_TextChanged_StoresSeparatelyFromSv()
        {
            // 15-8
            TestReflection.RunInSta(() =>
            {
                var form = CreateSvBpmForm();
                var temp = TempData(form);
                temp.svFrom = "1";
                temp.svTo = "2";
                temp.isSvBpm = true;

                GetControl<TextBox>(form, "txtSvFrom").Text = "200";
                Click(form, "txtSvFrom_TextChanged");
                GetControl<TextBox>(form, "txtSvTo").Text = "250";
                Click(form, "txtSvTo_TextChanged");

                Assert.Equal("200", temp.svBpmFrom);
                Assert.Equal("250", temp.svBpmTo);
                Assert.Equal("1", temp.svFrom);
                Assert.Equal("2", temp.svTo);
            });
        }

        /// <summary>
        /// BPM で指定のチェックは、適用タブで処理項目が赤線以外、かつ SV が有効なときだけ表示する
        /// </summary>
        [Theory]
        [InlineData(0, 0, true, true)]    // 適用・オブジェクト
        [InlineData(0, 1, true, true)]    // 適用・ビートスナップ
        [InlineData(0, 2, true, true)]    // 適用・緑線
        [InlineData(0, 3, true, false)]   // 適用・赤線
        [InlineData(1, 0, true, false)]   // 削除
        [InlineData(0, 0, false, false)]  // SV のチェックを外している
        public void SvBpm_ShownOnlyForApplyTabsExceptRedLineWithSv(int executeTabIndex, int setTypeTabIndex, bool isSv, bool expected)
        {
            // 15-9 (表示(Visible)は親の画面が表示されていないと正しく読めないため、見えない画面にチェックを置いて確かめる)
            TestReflection.RunInSta(() =>
            {
                var form = CreateSvBpmForm(executeTabIndex, setTypeTabIndex);
                GetControl<CheckBox>(form, "chkEnableSv").Checked = isSv;
                using var host = new Form { ShowInTaskbar = false, Opacity = 0, StartPosition = FormStartPosition.Manual, Location = new System.Drawing.Point(-2000, -2000) };
                var chkSvBpm = GetControl<CheckBox>(form, "chkSvBpm");
                host.Controls.Add(chkSvBpm);
                host.Show();

                TestReflection.Invoke(form, "UpdateSvBpmControls");

                Assert.Equal(expected, chkSvBpm.Visible);
            });
        }

        /// <summary>
        /// BPM で指定しているときの Set Start SV・Set End SV は、現地点の見た目 BPM を入れる。SV のときは SV を入れる
        /// </summary>
        [Theory]
        [InlineData(true, "SliderMultiplier:1.4", "240")]
        [InlineData(true, "SliderMultiplier:1.8", "308.57143")]
        [InlineData(false, "SliderMultiplier:1.8", "2")]
        public void SetSvButtons_SvBpm_SetsVisualBpm(bool isSvBpm, string sliderMultiplier, string expected)
        {
            // 15-7 (2500ms は BPM120・SV2)
            var builder = new OsuFileBuilder();
            builder.Difficulty = [.. builder.Difficulty.Where(l => !l.StartsWith("SliderMultiplier:")), sliderMultiplier];
            string oni = CreateDiff("Oni", builder);

            TestReflection.RunInSta(() =>
            {
                var form = CreateSvBpmForm(beatmapPath: oni);
                TempData(form).isSvBpm = isSvBpm;
                TestReflection.SetField(form, "currentTime", 2500);

                Click(form, "btnSetSvFrom_Click");
                Click(form, "btnSetSvTo_Click");

                Assert.Equal(expected, GetControl<TextBox>(form, "txtSvFrom").Text);
                Assert.Equal(expected, GetControl<TextBox>(form, "txtSvTo").Text);
            });
        }
        #endregion

        #region 8-5・8-10 Ctrl+Z
        /// <summary>
        /// Ctrl+Z を押したときの処理を呼ぶ
        /// </summary>
        /// <remarks>
        /// Ctrl+Z は SV Editor の画面を表示しているときだけ動く。表示(Visible)は親の画面が表示されていないと正しく読めないため、
        /// 見えない画面に SV Editor のパネルを置いて決める
        /// </remarks>
        private static void PressCtrlZ(MainForm form, bool isSvEditorVisible = true)
        {
            using var host = new Form { ShowInTaskbar = false, Opacity = 0, StartPosition = FormStartPosition.Manual, Location = new System.Drawing.Point(-2000, -2000) };
            var panel = new Panel { Visible = isSvEditorVisible };
            host.Controls.Add(panel);
            host.Show();
            TestReflection.SetField(form, "pnlGroupSvEditor", panel);
            TestReflection.Invoke(form, "osu_taiko_Mapping_Helper_KeyDown", form, new KeyEventArgs(Keys.Z | Keys.Control));
        }

        /// <summary>
        /// Ctrl+Z を押すと、バックアップに戻して戻したメッセージを出す
        /// </summary>
        [Fact]
        public void CtrlZ_RestoresBackupAndShowsMessage()
        {
            // 8-5・14-17
            string oni = CreateDiff("Oni");
            byte[] before = File.ReadAllBytes(oni);
            Assert.True(BeatmapHelper.CreateBackup(oni, Path.GetFileNameWithoutExtension(oni)));
            File.WriteAllText(oni, "changed");

            TestReflection.RunInSta(() => PressCtrlZ(CreateForm(oni)));

            Assert.Equal(before, File.ReadAllBytes(oni));
            Assert.Equal(["I_A-P-2"], MessageCodes);
        }

        /// <summary>
        /// Ctrl+Z で、バックアップのフォルダにバックアップが無ければ、失敗のメッセージを出す
        /// </summary>
        [Fact]
        public void CtrlZ_WithoutBackupFiles_ShowsError()
        {
            // 8-10
            string oni = CreateDiff("Oni");
            Directory.CreateDirectory(BeatmapHelper.GetBackupDirectoryPath(Path.GetFileNameWithoutExtension(oni)));
            File.WriteAllText(oni, "changed");

            TestReflection.RunInSta(() => PressCtrlZ(CreateForm(oni)));

            Assert.Equal("changed", File.ReadAllText(oni));
            Assert.Equal(["E_A-P-1"], MessageCodes);
        }

        /// <summary>
        /// Ctrl+Z で、バックアップのフォルダが無ければ、メッセージを出さずに何もしない
        /// </summary>
        [Fact]
        public void CtrlZ_WithoutBackupFolder_DoesNothingSilently()
        {
            // 8-10
            string oni = CreateDiff("Oni");
            File.WriteAllText(oni, "changed");

            TestReflection.RunInSta(() => PressCtrlZ(CreateForm(oni)));

            Assert.Equal("changed", File.ReadAllText(oni));
            Assert.Empty(MessageCodes);
        }

        /// <summary>
        /// Ctrl+Z は、SV Editor の画面を表示していなければ何もしない
        /// </summary>
        [Fact]
        public void CtrlZ_SvEditorHidden_DoesNothing()
        {
            // 13 (手動にしていた「Ctrl+Z は SV Editor のときだけ動く」。ユーティリティの画面では、入力欄の取り消しとして使えるようにする)
            string oni = CreateDiff("Oni");
            Assert.True(BeatmapHelper.CreateBackup(oni, Path.GetFileNameWithoutExtension(oni)));
            File.WriteAllText(oni, "changed");

            TestReflection.RunInSta(() => PressCtrlZ(CreateForm(oni), isSvEditorVisible: false));

            Assert.Equal("changed", File.ReadAllText(oni));
            Assert.Empty(MessageCodes);
        }

        /// <summary>
        /// Ctrl+Z で、譜面を選んでいなければ、譜面情報の取得に失敗したメッセージを出す
        /// </summary>
        [Fact]
        public void CtrlZ_NoBeatmapSelected_ShowsLoadError()
        {
            // 8-10
            TestReflection.RunInSta(() => PressCtrlZ(CreateForm(string.Empty)));

            Assert.Equal(["E_A-D-1"], MessageCodes);
        }
        #endregion

        #region 10 BG 設定のメニュー
        /// <summary>
        /// BG の画像か BG の行が取れていなければ、BG 設定の画面を開かずに、BG の取得に失敗したメッセージを出す
        /// </summary>
        [Theory]
        [InlineData("", "0,0,\"bg.jpg\",0,0")]   // BG の画像が取れていない
        [InlineData("C:\\bg.jpg", "")]           // BG の行が取れていない
        public void BgSetterMenu_WithoutBackground_ShowsError(string backgroundPath, string background)
        {
            // 10-4・14-17
            TestReflection.RunInSta(() =>
            {
                var form = CreateForm(string.Empty);
                var info = (BeatmapMetadata)TestReflection.GetField(form, "beatmapInfo")!;
                info.backgroundPath = backgroundPath;
                info.background = background;

                Click(form, "bGSetterToolStripMenuItem_Click");
            });

            Assert.Equal(["E_A-D-2"], MessageCodes);
        }
        #endregion
    }
}
