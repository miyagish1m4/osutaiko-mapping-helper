using System.Drawing;
using System.Drawing.Imaging;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils.Helper;
using osu_taiko_Mapping_Helper.Views;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// BG 設定のテスト (TEST_PATTERNS.md の 10)
    /// </summary>
    public class BgSetterTests : TestEnvironment
    {
        private string SetDirectory => Path.Combine(WorkDirectory, "Songs", "1 test");

        /// <summary>
        /// 指定したサイズの画像を作る
        /// </summary>
        private string CreateImage(string fileName, int width, int height)
        {
            Directory.CreateDirectory(SetDirectory);
            string path = Path.Combine(SetDirectory, fileName);
            using var bitmap = new Bitmap(width, height);
            bitmap.Save(path, ImageFormat.Png);
            return path;
        }

        /// <summary>
        /// セットのフォルダに diff を作る
        /// </summary>
        private string CreateDiff(string version, string backgroundLine, int mode = 1, List<string>? hitObjects = null)
        {
            Directory.CreateDirectory(SetDirectory);
            var builder = new OsuFileBuilder
            {
                General = ["AudioFilename: audio.mp3", $"Mode: {mode}"],
                Metadata = ["Title:t", $"Version:{version}"],
                Events = ["//Background and Video events", backgroundLine, "//Break Periods"],
            };
            if (hitObjects != null) builder.HitObjects = hitObjects;
            string path = Path.Combine(SetDirectory, $"a - t (m) [{version}].osu");
            File.WriteAllText(path, string.Join("\r\n", builder.Build()) + "\r\n");
            return path;
        }

        private static string BackgroundLine(string path) =>
            File.ReadAllLines(path).SkipWhile(l => l != "//Background and Video events").Skip(1).First();

        /// <summary>
        /// BG 設定の画面を作り、y 座標を指定して適用する
        /// </summary>
        private void ApplyBackground(string beatmapPath, string imagePath, string background, int y)
        {
            TestReflection.RunInSta(() =>
            {
                using var form = new BGSetterForm(new Config { maxBackupCount = 5 }, beatmapPath, imagePath, background, Path.GetFileNameWithoutExtension(beatmapPath));
                TestReflection.SetField(form, "retY", y);
                TestReflection.Invoke(form, "btnApply_Click", form, EventArgs.Empty);
            });
        }

        /// <summary>
        /// 同じ BG を使っている taiko の diff の y 座標がそろう
        /// </summary>
        [Fact]
        public void Apply_SyncsYToTaikoDiffsWithSameBackground()
        {
            // 10-1 (大文字違い・「"」なし・座標なしの diff も同じ BG。x 座標と表示開始時間はその diff のまま)
            string image = CreateImage("bg.jpg", 1920, 1080);
            string oni = CreateDiff("Oni", "0,0,\"bg.jpg\",0,0");
            string muzukashii = CreateDiff("Muzukashii", "0,0,\"BG.JPG\",20,-10");
            string futsuu = CreateDiff("Futsuu", "0,0,\"bg.jpg\"");
            string kantan = CreateDiff("Kantan", "0,0,bg.jpg,0,0");
            string other = CreateDiff("Other", "0,0,\"other.jpg\",0,0");
            string standard = CreateDiff("Std", "0,0,\"bg.jpg\",0,0", mode: 0);

            ApplyBackground(oni, image, "0,0,\"bg.jpg\",0,0", 40);

            Assert.Equal("0,0,\"bg.jpg\",0,40", BackgroundLine(oni));
            Assert.Equal("0,0,\"BG.JPG\",20,40", BackgroundLine(muzukashii));
            Assert.Equal("0,0,\"bg.jpg\",0,40", BackgroundLine(futsuu));
            Assert.Equal("0,0,bg.jpg,0,40", BackgroundLine(kantan));
            Assert.Equal("0,0,\"other.jpg\",0,0", BackgroundLine(other));
            Assert.Equal("0,0,\"bg.jpg\",0,0", BackgroundLine(standard));
            Assert.Contains("I_A-P-1", MessageCodes);
        }

        /// <summary>
        /// 同じ BG の diff は、それぞれバックアップを作る
        /// </summary>
        [Fact]
        public void Apply_CreatesBackupForEachDiffWithSameBackground()
        {
            // 10-1
            string image = CreateImage("bg.jpg", 1920, 1080);
            string oni = CreateDiff("Oni", "0,0,\"bg.jpg\",0,0");
            string muzukashii = CreateDiff("Muzukashii", "0,0,\"bg.jpg\",0,0");

            ApplyBackground(oni, image, "0,0,\"bg.jpg\",0,0", 40);

            Assert.Single(BeatmapHelper.GetBackupFiles(BeatmapHelper.GetBackupDirectoryPath(Path.GetFileNameWithoutExtension(oni))));
            Assert.Single(BeatmapHelper.GetBackupFiles(BeatmapHelper.GetBackupDirectoryPath(Path.GetFileNameWithoutExtension(muzukashii))));
        }

        /// <summary>
        /// 読めない taiko の diff があると、何も書き換えない
        /// </summary>
        [Fact]
        public void Apply_UnreadableTaikoDiff_ChangesNothing()
        {
            // 10-2
            string image = CreateImage("bg.jpg", 1920, 1080);
            string oni = CreateDiff("Oni", "0,0,\"bg.jpg\",0,0");
            string broken = CreateDiff("Broken", "0,0,\"bg.jpg\",0,0", hitObjects: ["480,80,5371,1,6"]);
            byte[] oniBefore = File.ReadAllBytes(oni);

            ApplyBackground(oni, image, "0,0,\"bg.jpg\",0,0", 40);

            Assert.Equal(oniBefore, File.ReadAllBytes(oni));
            Assert.Equal("0,0,\"bg.jpg\",0,0", BackgroundLine(broken));
            Assert.Equal(["E_A-D-1"], MessageCodes);
        }

        /// <summary>
        /// y 座標は、Mapset Verifier の警告が出ない範囲で動かせる
        /// </summary>
        [Theory]
        [InlineData(1920, 1080, -135, 114)]   // 16:9
        [InlineData(1366, 768, -135, 113)]
        [InlineData(2560, 1080, -76, 50)]     // 21:9
        [InlineData(1024, 768, -215, 197)]    // 4:3 (上はバーの下を覆う範囲の方が狭い)
        [InlineData(1000, 1000, -320, 304)]   // 1:1
        [InlineData(1080, 1920, -648, 636)]   // 縦長
        public void Drag_LimitsYToMapsetVerifierRange(int width, int height, int expectedMin, int expectedMax)
        {
            // 10-3
            string image = CreateImage($"bg_{width}x{height}.png", width, height);
            string oni = CreateDiff("Oni", "0,0,\"bg.png\",0,0");
            int min = int.MaxValue, max = int.MinValue;

            TestReflection.RunInSta(() =>
            {
                using var form = new BGSetterForm(new Config(), oni, image, "0,0,\"bg.png\",0,0", "Oni");
                var picture = (PictureBox)TestReflection.GetField(form, "picDisplayBg")!;
                // 上下に 1px ずつ大きく動かして、届く y の最小・最大を調べる
                foreach (int direction in new[] { -1, 1 })
                {
                    for (int i = 0; i < 1500; i++)
                    {
                        TestReflection.Invoke(form, "picDisplayBg_MouseMove", picture, new MouseEventArgs(MouseButtons.Left, 1, 0, direction, 0));
                        int y = (int)TestReflection.GetField(form, "retY")!;
                        min = Math.Min(min, y);
                        max = Math.Max(max, y);
                    }
                }
            });

            Assert.Equal(expectedMin, min);
            Assert.Equal(expectedMax, max);
        }

        /// <summary>
        /// 画像のサイズが取れなければ、y 座標の範囲を制限しない
        /// </summary>
        [Fact]
        public void Drag_ImageSizeUnavailable_NoLimit()
        {
            // 10-3 (Mapset Verifier と同じくチェックしない)
            Directory.CreateDirectory(SetDirectory);
            string notImage = Path.Combine(SetDirectory, "broken.png");
            File.WriteAllText(notImage, "not an image");
            string oni = CreateDiff("Oni", "0,0,\"broken.png\",0,0");

            TestReflection.RunInSta(() =>
            {
                using var form = new BGSetterForm(new Config(), oni, notImage, "0,0,\"broken.png\",0,0", "Oni");

                Assert.Equal(int.MinValue, (int)TestReflection.GetField(form, "minY")!);
                Assert.Equal(int.MaxValue, (int)TestReflection.GetField(form, "maxY")!);
            });
        }

        /// <summary>
        /// 拡張子が大文字の BG でも、BG 設定の画面を開ける
        /// </summary>
        [Fact]
        public void Open_UppercaseExtension_LoadsImage()
        {
            // 10-4 (U9)
            string image = CreateImage("BG.JPG", 1920, 1080);
            string oni = CreateDiff("Oni", "0,0,\"BG.JPG\",0,0");

            TestReflection.RunInSta(() =>
            {
                using var form = new BGSetterForm(new Config(), oni, image, "0,0,\"BG.JPG\",0,0", "Oni");
                Assert.NotNull(((PictureBox)TestReflection.GetField(form, "picDisplayBg")!).Image);
            });
        }

        /// <summary>
        /// BG 設定の画面に表示する画像の大きさ
        /// </summary>
        [Theory]
        [InlineData(1920, 1080, 854, 480)]    // ちょうど 16:9
        [InlineData(2560, 1080, 854, 480)]    // 横長は中央を切り出す
        [InlineData(1000, 1000, 854, 854)]    // 縦に長い画像は縦に長いまま
        [InlineData(1080, 1920, 854, 1518)]
        public void SetBgOnForm_ImageSize(int width, int height, int expectedWidth, int expectedHeight)
        {
            // 10-5
            string image = CreateImage($"view_{width}x{height}.png", width, height);

            using var bitmap = BGHelper.SetBgOnForm(image, new Point(854, 480), 0.7);

            Assert.Equal(expectedWidth, bitmap.Width);
            Assert.Equal(expectedHeight, bitmap.Height);
        }
    }
}
