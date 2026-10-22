using System.Collections;
using System.Configuration;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Text.Json;
using osu_taiko_Mapping_Helper.Utils;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// アップデーターのテスト (TEST_PATTERNS.md の 12)
    /// </summary>
    /// <remarks>
    /// GitHub に問い合わせる処理は、テストの中で立てた HTTP サーバーの応答で確かめる。
    /// 本物の GitHub に問い合わせるテストは、環境変数 OSU_HELPER_GITHUB_TEST に 1 を指定したときだけ動く。
    /// 最新版が見つかったときの流れ(ダイアログ → ダウンロード → 更新バッチの実行)は、アプリの入れ替えと再起動が起きるため手動で確認する
    /// </remarks>
    public class UpdaterTests : TestEnvironment
    {
        private const BindingFlags Flags = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private static readonly Type Updater = typeof(UpdaterUtils);
        private static Type Nested(string name) => Updater.GetNestedType(name, BindingFlags.NonPublic)!;

        /// <summary>
        /// バージョンの新しさを比べる
        /// </summary>
        [Theory]
        [InlineData("v1.2.6", "v1.2.5", true)]
        [InlineData("v1.2.6", "v1.2.6", false)]
        [InlineData("v1.2.5", "v1.2.6", false)]
        [InlineData("v1.10.0", "v1.9.9", true)]     // 数字として比べる
        [InlineData("v1.2.6", "v1.2.6-beta", true)] // プレリリースより正式版が新しい
        [InlineData("v1.2.6-beta", "v1.2.6", false)]
        [InlineData("v2.0.0", "v1.2.6", true)]
        public void IsNewerVersion_ComparesVersions(string latest, string current, bool expected)
        {
            // 12-1
            Assert.Equal(expected, (bool)TestReflection.InvokeStatic(Updater, "IsNewerVersion", latest, current)!);
        }

        /// <summary>
        /// タグ名からバージョンを取り出す
        /// </summary>
        [Theory]
        [InlineData("v1.2.6", 1, 2, 6, "")]
        [InlineData("v1.2.6-beta", 1, 2, 6, "beta")]
        [InlineData("1.2.6", 1, 2, 6, "")]
        [InlineData("v1.2", 1, 2, 0, "")]
        [InlineData("release-v1.2.6", 1, 2, 6, "")]   // 前に文字が付いていても、最初の数字から読む
        public void AppVersionTryParse_ParsesTagName(string tagName, int major, int minor, int patch, string preRelease)
        {
            // 12-9
            var appVersion = Nested("AppVersion");
            object?[] args = [tagName, null];

            bool isSuccess = (bool)appVersion.GetMethod("TryParse", Flags)!.Invoke(null, args)!;

            Assert.True(isSuccess);
            Assert.Equal(major, appVersion.GetProperty("Major")!.GetValue(args[1]));
            Assert.Equal(minor, appVersion.GetProperty("Minor")!.GetValue(args[1]));
            Assert.Equal(patch, appVersion.GetProperty("Patch")!.GetValue(args[1]));
            Assert.Equal(preRelease, appVersion.GetProperty("PreRelease")!.GetValue(args[1]));
        }

        /// <summary>
        /// 数字の無いタグ名は、バージョンとして読めない
        /// </summary>
        [Fact]
        public void AppVersionTryParse_WithoutNumber_Fails()
        {
            // 12-9
            object?[] args = ["latest", null];

            Assert.False((bool)Nested("AppVersion").GetMethod("TryParse", Flags)!.Invoke(null, args)!);
        }

        /// <summary>
        /// リリースの情報から、プレリリースかどうかを読む
        /// </summary>
        [Theory]
        [InlineData("""{"tag_name":"v1.3.0","prerelease":true,"draft":false}""", true)]
        [InlineData("""{"tag_name":"v1.3.0","prerelease":false,"draft":false}""", false)]
        public void GithubRelease_ReadsPrerelease(string json, bool expected)
        {
            // 12-2 (M6。プレリリースを除く判定そのものは GitHub への問い合わせの中にあるため、その前提を確かめる)
            var options = (JsonSerializerOptions)Updater.GetField("JsonOptions", Flags)!.GetValue(null)!;

            var release = JsonSerializer.Deserialize(json, Nested("GithubRelease"), options)!;

            Assert.Equal(expected, release.GetType().GetProperty("Prerelease")!.GetValue(release));
        }

        /// <summary>
        /// ダウンロードするファイルの候補を作る
        /// </summary>
        private static IList Assets(params (string name, string url)[] assets)
        {
            var assetType = Nested("GithubReleaseAsset");
            var list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(assetType))!;
            foreach (var (name, url) in assets)
            {
                var asset = Activator.CreateInstance(assetType, true)!;
                assetType.GetProperty("Name")!.SetValue(asset, name);
                assetType.GetProperty("DownloadUrl")!.SetValue(asset, url);
                list.Add(asset);
            }
            return list;
        }

        private static string? SelectedName(IList assets)
        {
            var selected = TestReflection.InvokeStatic(Updater, "SelectApplicationAsset", assets);
            return (string?)selected?.GetType().GetProperty("Name")!.GetValue(selected);
        }

        /// <summary>
        /// zip と exe があれば、zip を選ぶ
        /// </summary>
        [Fact]
        public void SelectApplicationAsset_PrefersZip()
        {
            // 12-5・12-10
            Assert.Equal("app.zip", SelectedName(Assets(("app.exe", "https://x/app.exe"), ("app.zip", "https://x/app.zip"))));
        }

        /// <summary>
        /// exe だけなら、exe を選ぶ
        /// </summary>
        [Fact]
        public void SelectApplicationAsset_OnlyExe_SelectsExe()
        {
            // 12-10
            Assert.Equal("app.EXE", SelectedName(Assets(("readme.txt", "https://x/readme.txt"), ("app.EXE", "https://x/app.EXE"))));
        }

        /// <summary>
        /// zip も exe も無ければ、選ばない
        /// </summary>
        [Fact]
        public void SelectApplicationAsset_NoZipOrExe_ReturnsNull()
        {
            // 12-5・12-10 (MainForm は W_U-A-1 を出す)
            Assert.Null(SelectedName(Assets(("readme.txt", "https://x/readme.txt"), ("source.tar.gz", "https://x/source.tar.gz"))));
        }

        /// <summary>
        /// ダウンロードの URL が空のファイルは、選ばない
        /// </summary>
        [Fact]
        public void SelectApplicationAsset_EmptyUrl_Skipped()
        {
            // 12-10
            Assert.Equal("app.exe", SelectedName(Assets(("app.zip", ""), ("app.exe", "https://x/app.exe"))));
        }

        /// <summary>
        /// 更新バッチのパスは、空白やシングルクォートを含んでも壊れない
        /// </summary>
        [Theory]
        [InlineData(@"C:\Users\name\osu", @"'C:\Users\name\osu'")]
        [InlineData(@"C:\it's here\osu", @"'C:\it''s here\osu'")]
        public void ToPowerShellSingleQuotedString_EscapesSpacesAndQuotes(string path, string expected)
        {
            // 12-6
            Assert.Equal(expected, TestReflection.InvokeStatic(Updater, "ToPowerShellSingleQuotedString", path));
        }

        /// <summary>
        /// 後片付けは、更新用のフォルダの中だけを消してよい
        /// </summary>
        [Fact]
        public void IsSafeCleanupDirectory_AllowsOnlyInsideUpdateFolder()
        {
            // 12-4 (H4)
            string root = (string)TestReflection.InvokeStatic(Updater, "GetUpdateRootDirectory")!;
            string baseDirectory = (string)TestReflection.InvokeStatic(Updater, "GetUpdateBaseDirectory")!;

            Assert.True((bool)TestReflection.InvokeStatic(Updater, "IsSafeCleanupDirectory", Path.Combine(root, "v1.2.6"))!);
            Assert.False((bool)TestReflection.InvokeStatic(Updater, "IsSafeCleanupDirectory", root)!);
            Assert.False((bool)TestReflection.InvokeStatic(Updater, "IsSafeCleanupDirectory", Path.GetTempPath())!);
            Assert.False((bool)TestReflection.InvokeStatic(Updater, "IsSafeCleanupDirectory", root + "_other")!);
            Assert.True((bool)TestReflection.InvokeStatic(Updater, "IsSafeCleanupRootDirectory", baseDirectory)!);
            Assert.False((bool)TestReflection.InvokeStatic(Updater, "IsSafeCleanupRootDirectory", Path.GetTempPath())!);
        }

        /// <summary>
        /// 更新用のフォルダは、前回の残りを消してから作る
        /// </summary>
        [Fact]
        public void PrepareUpdateDirectory_DeletesPreviousAndCreates()
        {
            // 12-7 (実際の一時フォルダ(%TEMP%\osu-taiko-mapping-helper\update)を使うため、すでにある場合は消さないようにテストしない)
            string root = (string)TestReflection.InvokeStatic(Updater, "GetUpdateRootDirectory")!;
            if (Directory.Exists(root)) return;
            try
            {
                Directory.CreateDirectory(Path.Combine(root, "old"));

                string directory = (string)TestReflection.InvokeStatic(Updater, "PrepareUpdateDirectory", "v1.2.6")!;

                Assert.Equal(Path.Combine(root, "v1.2.6"), directory);
                Assert.False(Directory.Exists(Path.Combine(root, "old")));
                Assert.True(Directory.Exists(directory));
            }
            finally
            {
                if (Directory.Exists(root)) Directory.Delete(root, true);
            }
        }

        /// <summary>
        /// GitHub への問い合わせには、User-Agent を付ける
        /// </summary>
        [Fact]
        public void CreateHttpClient_SetsUserAgent()
        {
            // 12-8
            using var client = (HttpClient)TestReflection.InvokeStatic(Updater, "CreateHttpClient")!;

            Assert.NotEmpty(client.DefaultRequestHeaders.UserAgent);
        }

        /// <summary>
        /// 設定ファイルのマージでは今の値を移し、新しい項目は初期値のままにする
        /// </summary>
        [Fact]
        public void MergeApplicationConfig_CopiesCurrentValuesAndKeepsNewDefaults()
        {
            // 12-3 (今の設定ファイルは testhost の設定ファイル)
            string currentPath = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None).FilePath;
            string currentBackup = File.ReadAllText(currentPath);
            try
            {
                var current = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                current.AppSettings.Settings["maxBackupCount"].Value = "77";
                current.AppSettings.Settings.Add("removedKey", "old");   // 新しい版では無くなった項目
                current.Save();

                string extracted = Path.Combine(WorkDirectory, "extracted");
                Directory.CreateDirectory(extracted);
                string newConfig = Path.Combine(extracted, Path.GetFileName(currentPath));
                File.WriteAllText(newConfig, """
                    <?xml version="1.0" encoding="utf-8" ?>
                    <configuration>
                      <appSettings>
                        <add key="maxBackupCount" value="10"/>
                        <add key="newKey" value="default"/>
                      </appSettings>
                    </configuration>
                    """);

                TestReflection.InvokeStatic(Updater, "MergeApplicationConfig", extracted);

                var merged = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap { ExeConfigFilename = newConfig }, ConfigurationUserLevel.None);
                Assert.Equal("77", merged.AppSettings.Settings["maxBackupCount"].Value);
                Assert.Equal("default", merged.AppSettings.Settings["newKey"].Value);
                Assert.Null(merged.AppSettings.Settings["removedKey"]);
            }
            finally
            {
                File.WriteAllText(currentPath, currentBackup);
            }
        }

        #region GitHub への問い合わせ
        /// <summary>
        /// 決まった応答を返すだけの HTTP サーバー (テストの間だけ、ループバックの空いているポートで動かす)
        /// </summary>
        private sealed class LocalHttpServer : IDisposable
        {
            private readonly TcpListener listener = new(IPAddress.Loopback, 0);
            private readonly CancellationTokenSource cancellation = new();
            internal string Url { get; }

            internal LocalHttpServer(int statusCode, byte[] body, string contentType = "application/json")
            {
                listener.Start();
                Url = $"http://127.0.0.1:{((IPEndPoint)listener.LocalEndpoint).Port}/";
                _ = Task.Run(async () =>
                {
                    while (!cancellation.IsCancellationRequested)
                    {
                        TcpClient client;
                        try
                        {
                            client = await listener.AcceptTcpClientAsync(cancellation.Token);
                        }
                        catch
                        {
                            return;
                        }
                        using (client)
                        {
                            var stream = client.GetStream();
                            // リクエストの見出しの終わりまで読む
                            var received = new StringBuilder();
                            var buffer = new byte[4096];
                            while (!received.ToString().Contains("\r\n\r\n"))
                            {
                                int length = await stream.ReadAsync(buffer);
                                if (length == 0) break;
                                received.Append(Encoding.ASCII.GetString(buffer, 0, length));
                            }
                            string header = $"HTTP/1.1 {statusCode} Test\r\nContent-Type: {contentType}\r\nContent-Length: {body.Length}\r\nConnection: close\r\n\r\n";
                            await stream.WriteAsync(Encoding.ASCII.GetBytes(header));
                            await stream.WriteAsync(body);
                        }
                    }
                });
            }

            internal LocalHttpServer(int statusCode, string body) : this(statusCode, Encoding.UTF8.GetBytes(body)) { }

            public void Dispose()
            {
                cancellation.Cancel();
                listener.Stop();
            }
        }

        /// <summary>
        /// HttpClient の既定のプロキシを差し替えて処理を実行する (テストの後で元に戻す)
        /// </summary>
        /// <remarks>
        /// 更新の処理は HttpClient を中で作るため、プロキシを差し替えて通信先を切り替える。
        /// 中身の無い WebProxy は、プロキシを通さずに直接つなぐ (PC のプロキシ設定に左右されないようにする)
        /// </remarks>
        private static async Task WithDefaultProxy(IWebProxy proxy, Func<Task> action)
        {
            var previous = HttpClient.DefaultProxy;
            HttpClient.DefaultProxy = proxy;
            try
            {
                await action();
            }
            finally
            {
                HttpClient.DefaultProxy = previous;
            }
        }

        /// <summary>
        /// private の async メソッドを呼んで、終わるのを待って結果を返す
        /// </summary>
        private static async Task<object?> InvokeAsync(string name, params object?[] args)
        {
            var task = (Task)TestReflection.InvokeStatic(Updater, name, args)!;
            await task;
            return task.GetType().GetProperty("Result")?.GetValue(task);
        }

        private static string? TagName(object? release) => (string?)release?.GetType().GetProperty("TagName")!.GetValue(release);

        /// <summary>
        /// GitHub の API の応答から、リリースの情報を読む
        /// </summary>
        [Fact]
        public async Task GetReleaseAsync_ReadsRelease()
        {
            // 12-2・12-10
            using var server = new LocalHttpServer(200, """{"tag_name":"v1.3.0","prerelease":false,"draft":false,"assets":[{"name":"app.zip","browser_download_url":"https://x/app.zip"}]}""");
            object? release = null;

            await WithDefaultProxy(new WebProxy(), async () =>
            {
                using var client = (HttpClient)TestReflection.InvokeStatic(Updater, "CreateHttpClient")!;
                release = await InvokeAsync("GetReleaseAsync", client, server.Url);
            });

            Assert.Equal("v1.3.0", TagName(release));
            Assert.Equal("app.zip", SelectedName((IList)release!.GetType().GetProperty("Assets")!.GetValue(release)!));
        }

        /// <summary>
        /// GitHub の API が失敗を返したら、リリースは無しとする
        /// </summary>
        [Theory]
        [InlineData(403)]   // 回数の制限
        [InlineData(404)]   // リリースが無い
        [InlineData(500)]
        public async Task GetReleaseAsync_ErrorResponse_ReturnsNull(int statusCode)
        {
            // 14-15
            using var server = new LocalHttpServer(statusCode, """{"message":"API rate limit exceeded"}""");
            object? release = "not null";

            await WithDefaultProxy(new WebProxy(), async () =>
            {
                using var client = (HttpClient)TestReflection.InvokeStatic(Updater, "CreateHttpClient")!;
                release = await InvokeAsync("GetReleaseAsync", client, server.Url);
            });

            Assert.Null(release);
        }

        /// <summary>
        /// ダウンロードしたファイルは、応答の中身と同じになる
        /// </summary>
        [Fact]
        public async Task DownloadFileAsync_SavesResponseBody()
        {
            // 12-5
            byte[] body = [.. Enumerable.Range(0, 100000).Select(i => (byte)(i % 251))];
            using var server = new LocalHttpServer(200, body, "application/octet-stream");
            string destination = Path.Combine(WorkDirectory, "app.zip");

            await WithDefaultProxy(new WebProxy(), () => InvokeAsync("DownloadFileAsync", server.Url + "app.zip", destination));

            Assert.Equal(body, File.ReadAllBytes(destination));
        }

        /// <summary>
        /// GitHub につながらなければ、アップデートの確認は通常の起動を続ける
        /// </summary>
        [Fact]
        public async Task CheckUpdateAsync_Offline_ContinuesStartup()
        {
            // 14-15 (つながらないプロキシを通して、オフラインと同じ状態にする)
            bool isUpdate = true;
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();

            await WithDefaultProxy(new WebProxy("http://127.0.0.1:9"), async () =>
            {
                isUpdate = await UpdaterUtils.CheckUpdateAsync("v1.0.0");
            });

            Assert.False(isUpdate);
            Assert.Empty(MessageCodes);
            Assert.True(stopwatch.Elapsed < TimeSpan.FromSeconds(30), $"{stopwatch.Elapsed.TotalSeconds:F1} 秒かかった");
        }

        /// <summary>
        /// 本物の GitHub から、最新版の情報を取れる
        /// </summary>
        [Fact]
        public async Task GetLatestReleaseAsync_RealGitHub_ReturnsRelease()
        {
            // 12-2・12-5 (環境変数 OSU_HELPER_GITHUB_TEST に 1 を指定したときだけ動く。リポジトリが公開されていないと取れない)
            if (Environment.GetEnvironmentVariable("OSU_HELPER_GITHUB_TEST") != "1") return;

            var latest = await InvokeAsync("GetLatestReleaseAsync", "v0.0.0");
            var none = await InvokeAsync("GetLatestReleaseAsync", "v999.0.0");

            Assert.NotNull(latest);
            Assert.False((bool)latest.GetType().GetProperty("Prerelease")!.GetValue(latest)!);
            Assert.NotNull(SelectedName((IList)latest.GetType().GetProperty("Assets")!.GetValue(latest)!));
            Assert.Null(none);
        }
        #endregion

        #region 更新ファイルの展開とバッチ
        /// <summary>
        /// 指定したファイルを入れた zip を作る
        /// </summary>
        private string CreateZip(params string[] entries)
        {
            string path = Path.Combine(WorkDirectory, "update.zip");
            using var archive = ZipFile.Open(path, ZipArchiveMode.Create);
            foreach (var entry in entries)
            {
                using var writer = new StreamWriter(archive.CreateEntry(entry).Open());
                writer.Write(entry);
            }
            return path;
        }

        /// <summary>
        /// zip の中が 1 つのフォルダなら、そのフォルダをアプリのファイルとする
        /// </summary>
        [Fact]
        public void ExtractApplicationArchive_SingleFolder_ReturnsThatFolder()
        {
            // 12-5
            string zip = CreateZip("osu!taiko Mapping Helper/app.exe", "osu!taiko Mapping Helper/app.dll.config");

            string directory = (string)TestReflection.InvokeStatic(Updater, "ExtractApplicationArchive", zip, WorkDirectory)!;

            Assert.Equal(Path.Combine(WorkDirectory, "extracted", "osu!taiko Mapping Helper"), directory);
            Assert.True(File.Exists(Path.Combine(directory, "app.exe")));
        }

        /// <summary>
        /// zip の中にファイルが直接あれば、展開したフォルダをアプリのファイルとする
        /// </summary>
        [Fact]
        public void ExtractApplicationArchive_FilesAtRoot_ReturnsExtractedFolder()
        {
            // 12-5
            string zip = CreateZip("app.exe", "app.dll.config", "lib/a.dll");

            string directory = (string)TestReflection.InvokeStatic(Updater, "ExtractApplicationArchive", zip, WorkDirectory)!;

            Assert.Equal(Path.Combine(WorkDirectory, "extracted"), directory);
        }

        /// <summary>
        /// 展開したファイルから、新しい版の設定ファイルを探す
        /// </summary>
        [Theory]
        [InlineData(new[] { "app.dll.config", "other.config" }, "app.dll.config")]   // 今の設定ファイルと同じ名前を選ぶ
        [InlineData(new[] { "other.config" }, "other.config")]                       // 無ければほかの設定ファイル
        [InlineData(new[] { "readme.txt" }, "")]                                     // 設定ファイルが無い
        public void GetExtractedConfigPath_FindsNewConfig(string[] files, string expected)
        {
            // 12-3
            string extracted = Path.Combine(WorkDirectory, "extracted");
            Directory.CreateDirectory(extracted);
            foreach (var file in files) File.WriteAllText(Path.Combine(extracted, file), string.Empty);

            string path = (string)TestReflection.InvokeStatic(Updater, "GetExtractedConfigPath", extracted, Path.Combine(WorkDirectory, "app.dll.config"))!;

            Assert.Equal(expected == string.Empty ? string.Empty : Path.Combine(extracted, expected), path);
        }

        /// <summary>
        /// 更新バッチは、アプリの終了を待ってからファイルを入れ替えて起動し直す
        /// </summary>
        [Fact]
        public void CreateUpdateBatch_WaitsCopiesCleansUpAndRestarts()
        {
            // 12-4・12-6 (バッチは実行しない。中の PowerShell のスクリプトを取り出して確かめる。
            // 実際の一時フォルダ(%TEMP%\osu-taiko-mapping-helper-apply-update.bat)を使うため、すでにある場合はテストしない)
            string batchPath = Path.Combine(Path.GetTempPath(), "osu-taiko-mapping-helper-apply-update.bat");
            if (File.Exists(batchPath)) return;
            var method = Updater.GetMethods(Flags).Single(m => m.Name == "CreateUpdateBatch" && m.GetParameters().Length == 6);
            try
            {
                string path = (string)method.Invoke(null, [@"C:\update\it's new", @"C:\update", @"C:\base", @"C:\app dir", @"C:\app dir\app.exe", 1234])!;

                Assert.Equal(batchPath, path);
                var lines = File.ReadAllLines(path);
                Assert.Equal("@echo off", lines[0]);
                Assert.Contains("del \"%~f0\"", lines[^1]);
                string script = Encoding.Unicode.GetString(Convert.FromBase64String(lines[1].Split(' ')[^1]));
                Assert.Contains("$pidToWait = 1234", script);
                Assert.Contains(@"$sourceDirectory = 'C:\update\it''s new'", script);
                Assert.Contains(@"$applicationDirectory = 'C:\app dir'", script);
                Assert.Contains(@"$executablePath = 'C:\app dir\app.exe'", script);
                // 終了を待つ → コピー → 後片付け → 起動 の順
                int wait = script.IndexOf("Get-Process -Id $pidToWait");
                int copy = script.IndexOf("Copy-Item");
                int cleanup = script.IndexOf("Remove-Item -LiteralPath $cleanupRootDirectory");
                int start = script.IndexOf("Start-Process");
                Assert.True(wait < copy && copy < cleanup && cleanup < start);
            }
            finally
            {
                if (File.Exists(batchPath)) File.Delete(batchPath);
            }
        }

        /// <summary>
        /// 更新用のフォルダの外を後片付けするバッチは作らない
        /// </summary>
        [Fact]
        public void CreateUpdateBatch_CleanupOutsideUpdateFolder_Throws()
        {
            // 12-4 (H4)
            var method = Updater.GetMethods(Flags).Single(m => m.Name == "CreateUpdateBatch" && m.GetParameters().Length == 2);

            var error = Assert.Throws<TargetInvocationException>(() => method.Invoke(null, [WorkDirectory, Path.GetTempPath()]));
            Assert.IsType<InvalidOperationException>(error.InnerException);
        }
        #endregion
    }
}
