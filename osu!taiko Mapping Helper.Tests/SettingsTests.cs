using System.Configuration;
using System.Xml.Linq;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// 設定・環境のテスト (TEST_PATTERNS.md の 9-5・9-14・11)
    /// </summary>
    /// <remarks>
    /// 設定ファイルは testhost の設定ファイル(Helper の App.config をコピーしたもの)を使う。
    /// テストで書き換えた設定ファイルは、テストの最後に元に戻す
    /// </remarks>
    public class SettingsTests : TestEnvironment, IDisposable
    {
        private readonly string configPath;
        private readonly string configBackup;

        public SettingsTests()
        {
            configPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
            configBackup = File.ReadAllText(configPath);
        }

        void IDisposable.Dispose()
        {
            File.WriteAllText(configPath, configBackup);
            Dispose();
        }

        private static NotesPosition ValidPositions() => new()
        {
            donX = "64", donY = "64", katX = "448", katY = "64", finisherDonX = "64", finisherDonY = "320", finisherKatX = "448", finisherKatY = "320",
        };

        private static Config LoadedConfig()
        {
            var config = new Config();
            config.ConfigLoad();
            return config;
        }

        // 9-5 設定画面の入力チェック
        /// <summary>
        /// バックアップ保持上限の入力エラー
        /// </summary>
        [Theory]
        [InlineData("", "E_V-EM-1")]
        [InlineData("abc", "E_V-T-2")]
        [InlineData("1.5", "E_V-T-1")]
        [InlineData("0", "E_V-C-1")]
        [InlineData("101", "E_V-C-1")]
        public void Settings_InvalidMaxBackupCount_ShowsError(string maxBackupCount, string expectedCode)
        {
            var config = LoadedConfig();

            Assert.False(SettingHelper.SetConfig("日本語", maxBackupCount, 0, false, false, ValidPositions(), config));

            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// ノーツ位置の入力エラー
        /// </summary>
        [Theory]
        [InlineData("donX", "", "E_V-EM-2")]
        [InlineData("donY", "", "E_V-EM-3")]
        [InlineData("katX", "abc", "E_V-T-4")]
        [InlineData("katY", "abc", "E_V-T-6")]
        [InlineData("finisherDonX", "1.5", "E_V-T-3")]
        [InlineData("finisherDonY", "1.5", "E_V-T-5")]
        [InlineData("finisherKatX", "513", "E_V-C-2")]
        [InlineData("finisherKatY", "385", "E_V-C-3")]
        [InlineData("donX", "-1", "E_V-C-2")]
        public void Settings_InvalidNotesPosition_ShowsError(string field, string value, string expectedCode)
        {
            var positions = ValidPositions();
            typeof(NotesPosition).GetProperty(field, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)!.SetValue(positions, value);

            Assert.False(SettingHelper.SetConfig("日本語", "10", 0, false, false, positions, LoadedConfig()));

            Assert.Equal([expectedCode], MessageCodes);
        }

        /// <summary>
        /// ノーツ位置は、画面の端まで入力できる
        /// </summary>
        [Theory]
        [InlineData("0", "0")]
        [InlineData("512", "384")]
        public void Settings_NotesPositionAcceptsScreenEdges(string x, string y)
        {
            var positions = ValidPositions();
            positions.donX = x;
            positions.donY = y;

            Assert.True(SettingHelper.SetConfig("日本語", "10", 0, false, false, positions, LoadedConfig()));
        }

        /// <summary>
        /// 設定画面のどれか 1 つが範囲外なら、どの設定も保存しない
        /// </summary>
        [Fact]
        public void Settings_AnyOutOfRange_SavesNothing()
        {
            // 9-14
            var config = LoadedConfig();
            int before = config.maxBackupCount;
            var positions = ValidPositions();
            positions.katY = "999";

            Assert.False(SettingHelper.SetConfig("English", "55", 1, true, true, positions, config));

            Assert.Equal(before, config.maxBackupCount);
            Assert.Equal(before, LoadedConfig().maxBackupCount);
        }

        /// <summary>
        /// 設定画面の保存で、全部の値が保存される
        /// </summary>
        [Fact]
        public void Settings_Save_SavesAllValues()
        {
            var config = LoadedConfig();

            Assert.True(SettingHelper.SetConfig("English", "55", 1, true, true, ValidPositions(), config));

            var reloaded = LoadedConfig();
            Assert.Equal("English", reloaded.language);
            Assert.Equal(55, reloaded.maxBackupCount);
            Assert.Equal(1, reloaded.offsetMode);
            Assert.Equal(1, reloaded.advanceMode);
            Assert.Equal(1, reloaded.unicodeSupport);
            Assert.Equal(448, reloaded.finisherKatX);
        }

        // 11-1・11-2 Songs フォルダの場所
        private string OsuDirectory => Path.Combine(WorkDirectory, "osu");

        /// <summary>
        /// osu! のユーザーの設定ファイル (osu! と同じく、ユーザー名からファイル名に使えない文字と "." を除いた名前)
        /// </summary>
        private string UserConfigPath()
        {
            string userName = Environment.UserName;
            foreach (char c in Path.GetInvalidFileNameChars()) userName = userName.Replace(c.ToString(), string.Empty);
            userName = userName.Replace(".", string.Empty);
            return Path.Combine(OsuDirectory, $"osu!.{userName}.cfg");
        }

        /// <summary>
        /// Songs フォルダの場所を、osu! の設定どおりに求める
        /// </summary>
        [Theory]
        [InlineData(null, "osu\\Songs")]                              // cfg なし
        [InlineData("BeatmapDirectory = Songs", "osu\\Songs")]
        [InlineData("BeatmapDirectory = Maps", "osu\\Maps")]          // 相対パス
        [InlineData("BeatmapDirectory = ..\\Other Songs", "Other Songs")]
        [InlineData("BeatmapDirectory = Nothing", "osu\\Songs")]      // 存在しないフォルダ
        [InlineData("BeatmapDirectory =", "osu\\Songs")]              // 値が空
        [InlineData("#BeatmapDirectory = Maps", "osu\\Songs")]        // コメント
        [InlineData("BeatmapDirectoryX = Maps", "osu\\Songs")]        // 似た名前のキー
        public void GetSongsFolderLocation_FollowsOsuConfig(string? line, string expected)
        {
            // 11-1・11-2 (L4)
            Directory.CreateDirectory(Path.Combine(OsuDirectory, "Songs"));
            Directory.CreateDirectory(Path.Combine(OsuDirectory, "Maps"));
            Directory.CreateDirectory(Path.Combine(WorkDirectory, "Other Songs"));
            if (line != null) File.WriteAllText(UserConfigPath(), "# osu! configuration\r\n" + line + "\r\n");

            string result = Common.GetSongsFolderLocation(OsuDirectory);

            Assert.Equal(Path.GetFullPath(Path.Combine(WorkDirectory, expected)), Path.GetFullPath(result));
        }

        /// <summary>
        /// Songs フォルダは、絶対パスでも指定できる
        /// </summary>
        [Theory]
        [InlineData(false)]
        [InlineData(true)]   // 末尾に \
        public void GetSongsFolderLocation_AcceptsAbsolutePath(bool trailingSeparator)
        {
            // 11-1
            string songs = Path.Combine(WorkDirectory, "Absolute Songs");
            Directory.CreateDirectory(songs);
            Directory.CreateDirectory(OsuDirectory);
            File.WriteAllText(UserConfigPath(), $"BeatmapDirectory = {songs}{(trailingSeparator ? "\\" : "")}\r\n");

            Assert.Equal(songs, Common.GetSongsFolderLocation(OsuDirectory));
        }

        /// <summary>
        /// Songs フォルダのパスにイコールが入っていても、途中で切れない
        /// </summary>
        [Fact]
        public void GetSongsFolderLocation_PathWithEquals_NotTruncated()
        {
            // 11-1
            string songs = Path.Combine(WorkDirectory, "a=b");
            Directory.CreateDirectory(songs);
            Directory.CreateDirectory(OsuDirectory);
            File.WriteAllText(UserConfigPath(), $"BeatmapDirectory = {songs}\r\n");

            Assert.Equal(songs, Common.GetSongsFolderLocation(OsuDirectory));
        }

        // 11-3 計算方法の保存と復元
        /// <summary>
        /// 起動時に、保存されている計算方法に戻す
        /// </summary>
        [Theory]
        [InlineData(Constants.CALCULATION_ARITHMETIC, true, false)]
        [InlineData(Constants.CALCULATION_GEOMETRIC, false, true)]
        [InlineData(0, true, false)]   // 想定外の値は等差
        public void InitializeCalculationControls_RestoresSavedCalculation(int calculationCode, bool expectedArithmetic, bool expectedGeometric)
        {
            bool isArithmetic = false, isGeometric = false;
            TestReflection.RunInSta(() =>
            {
                var (form, arithmetic, geometric, input) = CreateMainFormWithCalculationControls(new Config { calculationCode = calculationCode });

                TestReflection.Invoke(form, "InitializeCalculationControls");

                isArithmetic = arithmetic.Checked;
                isGeometric = geometric.Checked;
                Assert.Equal(arithmetic.Checked, input.isArithmetic);
                Assert.Equal(geometric.Checked, input.isGeometric);
            });

            Assert.Equal(expectedArithmetic, isArithmetic);
            Assert.Equal(expectedGeometric, isGeometric);
        }

        /// <summary>
        /// 計算方法を選ぶと、その場で設定ファイルに保存する
        /// </summary>
        [Fact]
        public void CalculationRadio_SavesImmediately()
        {
            // 11-3
            TestReflection.RunInSta(() =>
            {
                var config = LoadedConfig();
                var (form, arithmetic, geometric, _) = CreateMainFormWithCalculationControls(config);
                TestReflection.Invoke(form, "InitializeCalculationControls");

                geometric.Checked = true;
                Assert.Equal(Constants.CALCULATION_GEOMETRIC, LoadedConfig().calculationCode);

                arithmetic.Checked = true;
                Assert.Equal(Constants.CALCULATION_ARITHMETIC, LoadedConfig().calculationCode);
            });
        }

        /// <summary>
        /// 計算方法のラジオボタンだけを持つ MainForm を作る (osu! の監視スレッドは動かさない)
        /// </summary>
        private static (MainForm form, RadioButton arithmetic, RadioButton geometric, UserInputTempData input) CreateMainFormWithCalculationControls(Config config)
        {
            var form = TestReflection.CreateWithoutConstructor<MainForm>();
            var host = new Form();
            var panel = new Panel();
            host.Controls.Add(panel);
            var arithmetic = new RadioButton { Checked = true };
            var geometric = new RadioButton();
            panel.Controls.Add(arithmetic);
            panel.Controls.Add(geometric);
            var input = new UserInputTempData();
            TestReflection.SetField(form, "rdoArithmetic", arithmetic);
            TestReflection.SetField(form, "rdoGeometric", geometric);
            TestReflection.SetField(form, "config", config);
            TestReflection.SetField(form, "userInputTempData", input);
            // デザイナーと同じくイベントをつなぐ (InitializeCalculationControls が外してつなぎ直せるよう、MainForm のメソッドそのものをつなぐ)
            var flags = System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic;
            arithmetic.CheckedChanged += (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), form, typeof(MainForm).GetMethod("rdoArithmetic_CheckedChanged", flags)!);
            geometric.CheckedChanged += (EventHandler)Delegate.CreateDelegate(typeof(EventHandler), form, typeof(MainForm).GetMethod("rdoGeometric_CheckedChanged", flags)!);
            return (form, arithmetic, geometric, input);
        }

        /// <summary>
        /// ログのフォルダを、作業フォルダに作る
        /// </summary>
        [Fact]
        public void InitializeDirectoryAndFiles_CreatesLogFoldersInWorkDirectory()
        {
            // 11-4 (H4。Program.cs で作業フォルダを exe のフォルダにしている前提)
            Assert.True(Directory.Exists(WorkDirectory + Constants.INFO_LOG_DIRECTORY));
            Assert.True(Directory.Exists(WorkDirectory + Constants.WARNING_LOG_DIRECTORY));
            Assert.True(Directory.Exists(WorkDirectory + Constants.ERROR_LOG_DIRECTORY));
        }

        /// <summary>
        /// 設定ファイルに行が無いと、読み込みで落ちる
        /// </summary>
        [Fact]
        public void ConfigLoad_MissingKey_Throws()
        {
            // 11-5 今の動きを固定する (新しい設定項目を足すときは、設定ファイルに行があるか注意する)
            var document = XDocument.Load(configPath);
            document.Descendants("add").First(x => (string?)x.Attribute("key") == "calculationCode").Remove();
            document.Save(configPath);

            Assert.ThrowsAny<Exception>(() => new Config().ConfigLoad());
        }

        /// <summary>
        /// 設定ファイルの値が数字でないと、読み込みで落ちる
        /// </summary>
        [Fact]
        public void ConfigLoad_NonNumericValue_Throws()
        {
            // 11-5 今の動きを固定する
            var document = XDocument.Load(configPath);
            document.Descendants("add").First(x => (string?)x.Attribute("key") == "maxBackupCount").SetAttributeValue("value", "abc");
            document.Save(configPath);

            Assert.ThrowsAny<Exception>(() => new Config().ConfigLoad());
        }

        /// <summary>
        /// 言語が想定外の値なら、メッセージは空になる
        /// </summary>
        [Fact]
        public void WriteDialogMessage_UnknownLanguage_ReturnsEmpty()
        {
            // 11-6 今の動きを固定する
            Common.LoadConfig(new Config { language = "Deutsch" });

            Assert.Equal(string.Empty, Common.WriteDialogMessage("E_A-P-1"));
        }

        /// <summary>
        /// 終了時の保存で、オフセットと計算方法の値を消さない
        /// </summary>
        [Fact]
        public void ConfigSave_KeepsOffsetAndCalculationValues()
        {
            // 11-7 (オフセットと計算方法は変更したときに別に保存している)
            var config = LoadedConfig();
            config.offsetHexa = 1;
            config.SaveOffsetHexa();
            config.offsetMsValue = "-15";
            config.SaveOffsetMsValue();
            config.calculationCode = Constants.CALCULATION_GEOMETRIC;
            config.SaveCalculationCode();

            var another = LoadedConfig();
            another.maxBackupCount = 33;
            another.ConfigSave();

            var reloaded = LoadedConfig();
            Assert.Equal(33, reloaded.maxBackupCount);
            Assert.Equal(1, reloaded.offsetHexa);
            Assert.Equal("-15", reloaded.offsetMsValue);
            Assert.Equal(Constants.CALCULATION_GEOMETRIC, reloaded.calculationCode);
        }

        /// <summary>
        /// 設定画面の保存ボタンで設定を保存し、保存したメッセージを出す
        /// </summary>
        [Fact]
        public void SettingForm_Save_SavesAndShowsMessage()
        {
            // 9-5・14-17
            TestReflection.RunInSta(() =>
            {
                using var form = new Views.SettingForm(LoadedConfig());
                TestReflection.Invoke(form, "InitializeControls");
                ((TextBox)TestReflection.GetField(form, "txtMaxBackupCount")!).Text = "20";

                TestReflection.Invoke(form, "btnSave_Click", form, EventArgs.Empty);
            });

            Assert.Equal(["I_A-P-3"], MessageCodes);
            Assert.Equal(20, LoadedConfig().maxBackupCount);
        }

        /// <summary>
        /// 設定画面の保存ボタンで入力エラーがあれば、保存せず、保存したメッセージも出さない
        /// </summary>
        [Fact]
        public void SettingForm_SaveWithInvalidInput_DoesNotSave()
        {
            // 9-5
            int before = LoadedConfig().maxBackupCount;
            TestReflection.RunInSta(() =>
            {
                using var form = new Views.SettingForm(LoadedConfig());
                TestReflection.Invoke(form, "InitializeControls");
                ((TextBox)TestReflection.GetField(form, "txtMaxBackupCount")!).Text = string.Empty;

                TestReflection.Invoke(form, "btnSave_Click", form, EventArgs.Empty);
            });

            Assert.Equal(["E_V-EM-1"], MessageCodes);
            Assert.Equal(before, LoadedConfig().maxBackupCount);
        }
    }
}
