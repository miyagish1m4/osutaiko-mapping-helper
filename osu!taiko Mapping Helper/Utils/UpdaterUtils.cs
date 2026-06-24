using System.Diagnostics;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Text.Json;
using System.Text.Json.Serialization;
using osu_taiko_Mapping_Helper.Properties;

namespace osu_taiko_Mapping_Helper.Utils
{
    /// <summary>
    /// GitHub Releasesを利用したアップデート確認と取得を行う
    /// </summary>
    internal static class UpdaterUtils
    {
        private const string Owner = "miyagish1m4";
        private const string Repository = "osutaiko-mapping-helper";
        private const string ReleasesApiUrl = $"https://api.github.com/repos/{Owner}/{Repository}/releases";
        private const string LatestReleaseApiUrl = $"https://api.github.com/repos/{Owner}/{Repository}/releases/latest";
        private const string UserAgent = "osu-taiko-mapping-helper-updater";
        private const string UpdateMarkerFileName = ".osu-taiko-mapping-helper-update";
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        /// <summary>
        /// 最新リリースを確認し、必要に応じてアップデート処理を開始する
        /// </summary>
        /// <param name="currentVersion">現在のアプリケーションバージョン</param>
        /// <returns>アップデート処理を開始した場合はtrue、通常起動を継続する場合はfalse</returns>
        internal static async Task<bool> CheckUpdateAsync(string currentVersion)
        {
            try
            {
                var latestRelease = await GetLatestReleaseAsync(currentVersion);
                if (latestRelease == null || !IsNewerVersion(latestRelease.TagName, currentVersion))
                {
                    return false;
                }

                if (!Common.ShowMessageDialog(
                    "I_U-C-1",
                    Constants.DIALOG_OPTION_YESNO,
                    currentVersion,
                    latestRelease.TagName))
                {
                    return false;
                }

                return await DownloadAndRunUpdaterAsync(latestRelease);
            }
            catch (Exception ex)
            {
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }

        /// <summary>
        /// GitHub Releases APIから最新リリース情報を取得する
        /// </summary>
        /// <returns>最新リリース情報。取得できない場合はnull</returns>
        private static async Task<GithubRelease?> GetLatestReleaseAsync(string currentVersion)
        {
            using var client = CreateHttpClient();
            var latestRelease = await GetReleaseAsync(client, LatestReleaseApiUrl);
            if (latestRelease != null &&
                !latestRelease.Draft &&
                IsNewerVersion(latestRelease.TagName, currentVersion))
            {
                return latestRelease;
            }

            using var response = await client.GetAsync(ReleasesApiUrl);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            var releases = await JsonSerializer.DeserializeAsync<List<GithubRelease>>(stream, JsonOptions);
            if (releases == null || releases.Count == 0)
            {
                return null;
            }

            return releases
                .Where(release => !release.Draft)
                .Where(release => AppVersion.TryParse(release.TagName, out _))
                .Where(release => IsNewerVersion(release.TagName, currentVersion))
                .OrderByDescending(release => AppVersion.Parse(release.TagName))
                .FirstOrDefault();
        }

        /// <summary>
        /// GitHub Releases APIから単一リリース情報を取得する
        /// </summary>
        /// <param name="client">GitHub API用HTTPクライアント</param>
        /// <param name="url">取得対象URL</param>
        /// <returns>リリース情報。取得できない場合はnull</returns>
        private static async Task<GithubRelease?> GetReleaseAsync(HttpClient client, string url)
        {
            using var response = await client.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            await using var stream = await response.Content.ReadAsStreamAsync();
            return await JsonSerializer.DeserializeAsync<GithubRelease>(stream, JsonOptions);
        }

        /// <summary>
        /// リリースassetを取得し、更新用バッチまたはインストーラーを起動する
        /// </summary>
        /// <param name="release">取得対象のリリース情報</param>
        /// <returns>アップデート処理を開始できた場合はtrue、開始できなかった場合はfalse</returns>
        private static async Task<bool> DownloadAndRunUpdaterAsync(GithubRelease release)
        {
            var asset = SelectApplicationAsset(release.Assets);
            if (asset == null)
            {
                Common.ShowMessageDialog("W_U-A-1");
                return false;
            }

            var updateDirectory = PrepareUpdateDirectory(release.TagName);
            var assetPath = Path.Combine(updateDirectory, asset.Name);

            await DownloadFileAsync(asset.DownloadUrl, assetPath);

            if (Path.GetExtension(assetPath).Equals(".exe", StringComparison.OrdinalIgnoreCase))
            {
                Process.Start(new ProcessStartInfo(assetPath) { UseShellExecute = true });
                return true;
            }

            if (!Path.GetExtension(assetPath).Equals(".zip", StringComparison.OrdinalIgnoreCase))
            {
                Common.ShowMessageDialog("I_U-D-1", 0, assetPath);
                return false;
            }

            var extractedDirectory = ExtractApplicationArchive(assetPath, updateDirectory);
            var batchPath = CreateUpdateBatch(extractedDirectory, updateDirectory);
            Process.Start(new ProcessStartInfo("cmd.exe", $"/c \"{batchPath}\"")
            {
                CreateNoWindow = true,
                UseShellExecute = false,
                WindowStyle = ProcessWindowStyle.Hidden
            });

            return true;
        }

        /// <summary>
        /// GitHub API用のHTTPクライアントを作成する
        /// </summary>
        /// <returns>GitHub API用にヘッダーを設定したHTTPクライアント</returns>
        private static HttpClient CreateHttpClient()
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            client.DefaultRequestHeaders.UserAgent.ParseAdd(UserAgent);
            client.DefaultRequestHeaders.Accept.ParseAdd("application/vnd.github+json");
            client.DefaultRequestHeaders.Add("X-GitHub-Api-Version", "2022-11-28");
            return client;
        }

        /// <summary>
        /// リリースassetからアプリケーション更新に利用するファイルを選択する
        /// </summary>
        /// <param name="assets">リリースに含まれるasset一覧</param>
        /// <returns>更新に利用するasset。見つからない場合はnull</returns>
        private static GithubReleaseAsset? SelectApplicationAsset(List<GithubReleaseAsset> assets)
        {
            return assets
                .Where(asset => !string.IsNullOrWhiteSpace(asset.DownloadUrl))
                .OrderBy(asset => GetAssetPriority(asset.Name))
                .FirstOrDefault(asset => GetAssetPriority(asset.Name) < int.MaxValue);
        }

        /// <summary>
        /// asset名から更新対象としての優先度を取得する
        /// </summary>
        /// <param name="assetName">判定対象のasset名</param>
        /// <returns>優先度。小さい値ほど優先される</returns>
        private static int GetAssetPriority(string assetName)
        {
            var extension = Path.GetExtension(assetName);
            if (extension.Equals(".zip", StringComparison.OrdinalIgnoreCase)) return 0;
            if (extension.Equals(".exe", StringComparison.OrdinalIgnoreCase)) return 1;
            return int.MaxValue;
        }

        /// <summary>
        /// 更新ファイルを配置する一時フォルダを作成する
        /// </summary>
        /// <param name="tagName">対象リリースのタグ名</param>
        /// <returns>作成した一時フォルダパス</returns>
        private static string PrepareUpdateDirectory(string tagName)
        {
            var safeTagName = string.Join("_", tagName.Split(Path.GetInvalidFileNameChars()));
            var updateRootDirectory = GetUpdateRootDirectory();
            var updateDirectory = Path.Combine(updateRootDirectory, safeTagName);

            if (Directory.Exists(updateRootDirectory))
            {
                Directory.Delete(updateRootDirectory, true);
            }

            Directory.CreateDirectory(updateDirectory);
            File.WriteAllText(Path.Combine(updateDirectory, UpdateMarkerFileName), string.Empty);
            return updateDirectory;
        }

        /// <summary>
        /// 指定URLからファイルをダウンロードする
        /// </summary>
        /// <param name="url">ダウンロード元URL</param>
        /// <param name="destinationPath">保存先ファイルパス</param>
        private static async Task DownloadFileAsync(string url, string destinationPath)
        {
            using var client = CreateHttpClient();
            await using var releaseStream = await client.GetStreamAsync(url);
            await using var fileStream = File.Create(destinationPath);
            await releaseStream.CopyToAsync(fileStream);
        }

        /// <summary>
        /// zip形式の更新ファイルを展開する
        /// </summary>
        /// <param name="archivePath">展開対象のzipファイルパス</param>
        /// <param name="updateDirectory">展開先の親フォルダパス</param>
        /// <returns>アプリケーションファイルが格納されている展開先フォルダパス</returns>
        private static string ExtractApplicationArchive(string archivePath, string updateDirectory)
        {
            var extractedDirectory = Path.Combine(updateDirectory, "extracted");
            ZipFile.ExtractToDirectory(archivePath, extractedDirectory, true);

            var childDirectories = Directory.GetDirectories(extractedDirectory);
            if (childDirectories.Length == 1 && Directory.GetFiles(extractedDirectory).Length == 0)
            {
                return childDirectories[0];
            }

            return extractedDirectory;
        }

        /// <summary>
        /// 更新用の一時フォルダを配置する親フォルダパスを取得する
        /// </summary>
        /// <returns>更新用一時フォルダの親フォルダパス</returns>
        private static string GetUpdateRootDirectory()
        {
            return Path.Combine(Path.GetTempPath(), "osu-taiko-mapping-helper", "update");
        }

        /// <summary>
        /// 更新後に削除する一時フォルダがアプリの更新用フォルダ配下にあるか判定する
        /// </summary>
        /// <param name="cleanupDirectory">削除対象の一時フォルダパス</param>
        /// <returns>削除してよい更新用一時フォルダの場合はtrue、それ以外はfalse</returns>
        private static bool IsSafeCleanupDirectory(string cleanupDirectory)
        {
            var updateRootDirectory = Path.GetFullPath(GetUpdateRootDirectory())
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            var cleanupFullPath = Path.GetFullPath(cleanupDirectory)
                .TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            if (string.Equals(cleanupFullPath, updateRootDirectory, StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            return cleanupFullPath.StartsWith(
                updateRootDirectory + Path.DirectorySeparatorChar,
                StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 実行中アプリ終了後に更新ファイルを配置するバッチファイルを作成する
        /// </summary>
        /// <param name="sourceDirectory">更新ファイルの展開先フォルダパス</param>
        /// <param name="cleanupDirectory">更新適用後に削除する一時フォルダパス</param>
        /// <returns>作成したバッチファイルパス</returns>
        private static string CreateUpdateBatch(string sourceDirectory, string cleanupDirectory)
        {
            if (!IsSafeCleanupDirectory(cleanupDirectory))
            {
                throw new InvalidOperationException($"Invalid cleanup directory: {cleanupDirectory}");
            }

            var currentProcess = Process.GetCurrentProcess();
            var executablePath = Environment.ProcessPath ?? Application.ExecutablePath;
            var applicationDirectory = AppContext.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar);
            var batchPath = Path.Combine(Path.GetTempPath(), "osu-taiko-mapping-helper", "update", "apply-update.bat");

            Directory.CreateDirectory(Path.GetDirectoryName(batchPath)!);

            var batchLines = new[]
            {
                "@echo off",
                "setlocal",
                $"set \"PID={currentProcess.Id}\"",
                $"set \"SRC={sourceDirectory}\"",
                $"set \"CLEANUP={cleanupDirectory}\"",
                $"set \"DST={applicationDirectory}\"",
                $"set \"EXE={executablePath}\"",
                ":wait",
                "tasklist /FI \"PID eq %PID%\" | find \"%PID%\" >nul",
                "if not errorlevel 1 (",
                "  timeout /t 1 /nobreak >nul",
                "  goto wait",
                ")",
                "xcopy \"%SRC%\\*\" \"%DST%\\\" /E /I /Y >nul",
                "if errorlevel 1 (",
                "  echo Update failed. Failed to copy files from \"%SRC%\" to \"%DST%\". > \"%CLEANUP%\\update-error.log\"",
                "  set \"OSU_TAIKO_MAPPING_HELPER_UPDATE_ERROR=%CLEANUP%\\update-error.log\"",
                "  start \"\" \"%EXE%\"",
                "  endlocal",
                "  del \"%~f0\"",
                "  exit /b 1",
                ")",
                $"if exist \"%CLEANUP%\\{UpdateMarkerFileName}\" rmdir /S /Q \"%CLEANUP%\" >nul 2>nul",
                "start \"\" \"%EXE%\"",
                "endlocal",
                "del \"%~f0\""
            };

            File.WriteAllLines(batchPath, batchLines);
            return batchPath;
        }

        /// <summary>
        /// 最新バージョンが現在のバージョンより新しいか判定する
        /// </summary>
        /// <param name="latestVersion">GitHub Releaseの最新バージョン</param>
        /// <param name="currentVersion">現在のアプリケーションバージョン</param>
        /// <returns>最新バージョンの方が新しい場合はtrue、それ以外はfalse</returns>
        private static bool IsNewerVersion(string latestVersion, string currentVersion)
        {
            if (!AppVersion.TryParse(latestVersion, out var latestAppVersion)) return false;
            if (!AppVersion.TryParse(currentVersion, out var currentAppVersion)) return false;

            return latestAppVersion.CompareTo(currentAppVersion) > 0;
        }

        private sealed class GithubRelease
        {
            [JsonPropertyName("tag_name")]
            public string TagName { get; init; } = string.Empty;

            [JsonPropertyName("draft")]
            public bool Draft { get; init; }

            [JsonPropertyName("assets")]
            public List<GithubReleaseAsset> Assets { get; init; } = [];
        }

        private sealed class GithubReleaseAsset
        {
            [JsonPropertyName("name")]
            public string Name { get; init; } = string.Empty;

            [JsonPropertyName("browser_download_url")]
            public string DownloadUrl { get; init; } = string.Empty;
        }

        private readonly record struct AppVersion(int Major, int Minor, int Patch, string PreRelease)
            : IComparable<AppVersion>
        {
            private static readonly Regex VersionPattern = new(
                @"(?<major>\d+)(?:\.(?<minor>\d+))?(?:\.(?<patch>\d+))?(?:[-_](?<prerelease>[0-9A-Za-z][0-9A-Za-z.-]*))?",
                RegexOptions.Compiled);

            public static AppVersion Parse(string version)
            {
                return TryParse(version, out var appVersion)
                    ? appVersion
                    : new AppVersion(0, 0, 0, string.Empty);
            }

            public static bool TryParse(string version, out AppVersion appVersion)
            {
                var match = VersionPattern.Match(version.Trim());
                if (!match.Success)
                {
                    appVersion = new AppVersion(0, 0, 0, string.Empty);
                    return false;
                }

                appVersion = new AppVersion(
                    GetVersionNumber(match.Groups["major"].Value),
                    GetVersionNumber(match.Groups["minor"].Value),
                    GetVersionNumber(match.Groups["patch"].Value),
                    match.Groups["prerelease"].Value);
                return true;
            }

            public int CompareTo(AppVersion other)
            {
                var majorComparison = Major.CompareTo(other.Major);
                if (majorComparison != 0) return majorComparison;

                var minorComparison = Minor.CompareTo(other.Minor);
                if (minorComparison != 0) return minorComparison;

                var patchComparison = Patch.CompareTo(other.Patch);
                if (patchComparison != 0) return patchComparison;

                if (PreRelease == other.PreRelease) return 0;
                if (string.IsNullOrEmpty(PreRelease)) return 1;
                if (string.IsNullOrEmpty(other.PreRelease)) return -1;

                return string.Compare(PreRelease, other.PreRelease, StringComparison.OrdinalIgnoreCase);
            }

            private static int GetVersionNumber(string versionPart)
            {
                return int.TryParse(versionPart, out var versionNumber) ? versionNumber : 0;
            }
        }
    }
}
