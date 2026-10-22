using System.Diagnostics;
using System.Text.RegularExpressions;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Services;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;
using Xunit.Abstractions;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// 横断的な観点のテスト (TEST_PATTERNS.md の 14)
    /// </summary>
    /// <remarks>
    /// 14-10〜14-13 は ExportTests・BackupTests、14-11 の一部は 8-8・6-17、14-15 は UpdaterTests にある。
    /// 14-16 (osu! の状態ごとの表示と動き) は osu! を動かす必要があるため、手動で確認する
    /// </remarks>
    public class CrossCuttingTests(ITestOutputHelper output) : TestEnvironment
    {
        /// <summary>
        /// リポジトリのフォルダ (テストの出力先 bin\Debug\net10.0-windows7.0 から 4 つ上)
        /// </summary>
        private static string RepositoryDirectory => Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));

        /// <summary>
        /// 日本語と英語のメッセージとラベルのキーがそろっている
        /// </summary>
        [Fact]
        public void Messages_JapaneseAndEnglishKeysMatch()
        {
            // 14-1
            Assert.Equal(Messages.DialogMessagesJp.Keys.Order(), Messages.DialogMessagesEn.Keys.Order());
            Assert.Equal(Labels.LabelsJp.Keys.Order(), Labels.LabelsEn.Keys.Order());
        }

        /// <summary>
        /// コードで使っているメッセージのコードが辞書にある
        /// </summary>
        [Fact]
        public void Messages_AllCodesUsedInSourceExist()
        {
            // 14-1
            var codes = UsedMessageCodes();

            Assert.NotEmpty(codes);
            Assert.All(codes, code => Assert.True(Messages.DialogMessagesJp.ContainsKey(code), $"辞書に無いメッセージ: {code}"));
        }

        /// <summary>
        /// Helper のコードで使っているメッセージのコード
        /// </summary>
        private static List<string> UsedMessageCodes()
        {
            string source = Path.Combine(RepositoryDirectory, "osu!taiko Mapping Helper");
            return [.. Directory.GetFiles(source, "*.cs", SearchOption.AllDirectories)
                                .Where(f => !f.Contains($"{Path.DirectorySeparatorChar}obj{Path.DirectorySeparatorChar}"))
                                .SelectMany(f => Regex.Matches(File.ReadAllText(f), "ShowMessageDialog\\(\"([^\"]+)\"").Select(m => m.Groups[1].Value))
                                .Distinct()];
        }

        /// <summary>
        /// アップデートのメッセージ (実際にアップデートする流れの中で出るため、手動で確かめる)
        /// </summary>
        private static readonly string[] UpdateMessageCodes = ["I_U-C-1", "I_U-D-1", "W_U-A-1", "W_U-A-2"];

        /// <summary>
        /// コードで使っているメッセージごとに、そのメッセージが出るテストがある (アップデートのメッセージは除く)
        /// </summary>
        [Fact]
        public void Messages_EachCodeHasTest()
        {
            // 14-17 (テストのコードの中に、メッセージのコードが書かれているかで判定する。辞書にあってもコードで使っていないメッセージは対象にしない)
            var testSources = string.Concat(Directory.GetFiles(Path.Combine(RepositoryDirectory, "osu!taiko Mapping Helper.Tests"), "*.cs")
                                                     .Where(f => !f.EndsWith(nameof(CrossCuttingTests) + ".cs"))
                                                     .Select(File.ReadAllText));
            var missing = UsedMessageCodes().Except(UpdateMessageCodes).Where(code => !testSources.Contains($"\"{code}\"")).Order().ToList();

            Assert.True(missing.Count == 0, "テストが無いメッセージ: " + string.Join(", ", missing));
        }

        /// <summary>
        /// テストは一時フォルダの中だけで動く
        /// </summary>
        [Fact]
        public void Tests_RunOnlyInsideTemporaryFolder()
        {
            // 14-2 (バックアップ・ログ・一時ファイルはカレントディレクトリに作られるため、カレントディレクトリが一時フォルダの中にあればよい)
            string temp = Path.GetFullPath(Path.GetTempPath());

            Assert.StartsWith(temp, WorkDirectory, StringComparison.OrdinalIgnoreCase);
            Assert.Equal(WorkDirectory, Directory.GetCurrentDirectory(), ignoreCase: true);
            Assert.StartsWith(WorkDirectory, BeatmapHelper.GetBackupDirectoryPath("x"), StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// REVIEW_NOTES.md の場所 (環境変数 REVIEW_NOTES_PATH、無ければリポジトリと同じ階層の osutaiko-mapping-helper_review)
        /// </summary>
        private static string? ReviewNotesPath()
        {
            string? path = Environment.GetEnvironmentVariable("REVIEW_NOTES_PATH");
            if (string.IsNullOrEmpty(path))
            {
                path = Path.Combine(RepositoryDirectory, "..", "osutaiko-mapping-helper_review", "REVIEW_NOTES.md");
            }
            return File.Exists(path) ? path : null;
        }

        /// <summary>
        /// REVIEW_NOTES.md で直した項目の ID を取り出す
        /// </summary>
        /// <remarks>
        /// 一覧の表 (H・S・T・M・U) は状態が「修正済み」で始まる行、軽微なもの (L) は「- L1:」の後ろの行に「修正」がある項目
        /// </remarks>
        private static List<string> FixedIds(string[] lines)
        {
            List<string> ids = [];
            foreach (var line in lines)
            {
                var cells = line.Split('|').Select(c => c.Trim()).ToArray();
                if (cells.Length >= 5 && Regex.IsMatch(cells[1], "^[HSTMU][0-9]+$") && cells[4].StartsWith("修正済み"))
                {
                    ids.Add(cells[1]);
                }
            }
            string? currentL = null;
            foreach (var line in lines)
            {
                var match = Regex.Match(line, "^- (L[0-9]+|その他):");
                if (match.Success)
                {
                    currentL = match.Groups[1].Value.StartsWith('L') ? match.Groups[1].Value : null;
                    continue;
                }
                if (line.StartsWith('#')) currentL = null;
                if (currentL != null && line.Contains("修正") && !ids.Contains(currentL)) ids.Add(currentL);
            }
            return ids;
        }

        /// <summary>
        /// 直した不具合ごとに、その不具合を確かめるテストがある
        /// </summary>
        [Fact]
        public void ReviewNotes_EachFixedIssueHasTest()
        {
            // 14-7 (テストのコメントに ID が書かれているかで判定する。REVIEW_NOTES.md が見つからなければ何もしない)
            string? notesPath = ReviewNotesPath();
            if (notesPath == null)
            {
                output.WriteLine("REVIEW_NOTES.md が見つからないため、何もしない");
                return;
            }
            var ids = FixedIds(File.ReadAllLines(notesPath));
            var testSources = string.Concat(Directory.GetFiles(Path.Combine(RepositoryDirectory, "osu!taiko Mapping Helper.Tests"), "*.cs")
                                                     .Where(f => !f.EndsWith(nameof(CrossCuttingTests) + ".cs"))
                                                     .Select(File.ReadAllText));

            Assert.NotEmpty(ids);
            var missing = ids.Where(id => !Regex.IsMatch(testSources, $"(?<![A-Za-z0-9]){id}(?![0-9])")).ToList();
            Assert.True(missing.Count == 0, "テストが無い項目: " + string.Join(", ", missing));
        }

        /// <summary>
        /// 大きい譜面でも、SV の適用と書き出しが実用的な時間で終わる
        /// </summary>
        [Fact]
        public void LargeBeatmap_ApplyAndExportFinishInTime()
        {
            // 14-4 (ノーツ 5000 個・緑線 2000 本)
            var builder = new OsuFileBuilder
            {
                Editor = [],
                TimingPoints = ["0,500,4,1,0,100,1,0", .. Enumerable.Range(1, 2000).Select(i => $"{i * 250},-100,4,1,0,100,0,0")],
                HitObjects = [.. Enumerable.Range(1, 5000).Select(i => $"256,192,{i * 100},1,0,0:0:0:0:")],
            };
            string path = WriteOsuFile("large.osu", builder.Build());
            var stopwatch = Stopwatch.StartNew();

            var beatmap = BeatmapHelper.GetBeatmapData(path);
            UserInputData data = new() { timingFrom = 0, timingTo = 500000, applySetCode = 0x1, isSv = true, svFrom = 1, svTo = 2, calculationCode = Constants.CALCULATION_ARITHMETIC, relativeCode = Constants.RELATIVE_DISABLE };
            data.setObjectOption.setObjectsCode = 0x37f;
            data.setObjectOption.isTimingStart = true;
            data.setObjectOption.isTimingEnd = true;
            List<TimingPoint> added = [];
            Assert.True(SVCalculatorService.Apply(data, beatmap, ref added));
            beatmap.timingPoints.AddRange(added);
            beatmap.timingPoints = [.. beatmap.timingPoints.OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0)];
            Assert.True(BeatmapHelper.ExportToOsuFile(beatmap, path));

            stopwatch.Stop();
            output.WriteLine($"{stopwatch.ElapsedMilliseconds}ms");
            Assert.True(stopwatch.Elapsed < TimeSpan.FromSeconds(30), $"{stopwatch.Elapsed.TotalSeconds:F1} 秒かかった");
        }

        /// <summary>
        /// 日本語・空白・記号を含むパスでも、読み込み・書き出し・バックアップができる
        /// </summary>
        [Fact]
        public void SpecialCharacterPath_ReadExportAndBackup()
        {
            // 14-5・14-14
            string directory = Path.Combine(WorkDirectory, "Songs", "123 アーティスト - 曲名 (譜面作成者)");
            Directory.CreateDirectory(directory);
            string path = Path.Combine(directory, "アーティスト - 曲名 (譜面作成者) [鬼 & Inner Oni!].osu");
            File.WriteAllText(path, string.Join("\r\n", new OsuFileBuilder().Build()));
            string backupName = Path.GetFileNameWithoutExtension(path);

            var beatmap = BeatmapHelper.GetBeatmapData(path);
            Assert.NotEqual(string.Empty, beatmap.version);
            Assert.True(BeatmapHelper.CreateBackup(path, backupName));
            Assert.True(BeatmapHelper.ExportToOsuFile(beatmap, path));
            Assert.True(BeatmapHelper.ExportToPreviousOsuFile(path, backupName));
        }

        /// <summary>
        /// 失敗したときは、譜面とバックアップが変わらない
        /// </summary>
        [Fact]
        public void Failure_KeepsBeatmapAndBackup()
        {
            // 14-6 (読み込めない diff があるオフセット)
            string good = WriteOsuFile("good.osu", new OsuFileBuilder().Build());
            string bad = WriteOsuFile("bad.osu", new OsuFileBuilder { HitObjects = ["480,80,5371,1,6"] }.Build());
            byte[] before = File.ReadAllBytes(good);

            int result = BeatmapHelper.ExportToAllOsuFiles([good, bad], new UserInputUtilityData { utilityCode = Constants.UTILITY_OFFSET, offset = 100 }, new BeatmapMetadata());

            Assert.Equal(1, result);
            Assert.Equal(before, File.ReadAllBytes(good));
            Assert.False(Directory.Exists(Path.Combine(WorkDirectory, "Backup")) && Directory.EnumerateFiles(Path.Combine(WorkDirectory, "Backup"), "*", SearchOption.AllDirectories).Any());
        }

        /// <summary>
        /// BG 設定用の書き出しも、BG の行以外はふつうの書き出しと同じになる
        /// </summary>
        [Fact]
        public void BackgroundExport_SameAsNormalExceptBackgroundLine()
        {
            // 14-8 (同じ処理が 2 か所にあるもの)
            var builder = new OsuFileBuilder
            {
                Events = ["//Background and Video events", "0,0,\"bg.jpg\",0,0", "Video,500,\"v.mp4\"", "//Break Periods", "2,4500,6000", " F,0,0,,0,1"],
                HitObjects = ["256,192,1000,5,0,0:0:0:0:", "256,192,2000,2,0,L|300:192,1,70,2|0,0:0|0:0,0:0:0:0:", "256,192,3000,12,0,4000,0:0:0:0:"],
            };
            string normal = WriteOsuFile("normal.osu", builder.Build());
            string bg = WriteOsuFile("bg.osu", builder.Build());

            Assert.True(BeatmapHelper.ExportToOsuFile(BeatmapHelper.GetBeatmapData(normal), normal));
            Assert.True(BeatmapHelper.ExportToOsuFile(BeatmapHelper.GetBeatmapData(bg, true), bg, ["0", "0", "\"bg.jpg\"", "0", "0"]));

            // (空行の数は書き出し方で違うことがあるため、空行は除いて比べる)
            Assert.Equal(File.ReadAllLines(normal).Where(l => l != ""), File.ReadAllLines(bg).Where(l => l != ""));
        }

        /// <summary>
        /// 表示用の読み込みとほかの機能の読み込みで、タイトル・diff 名・BG が同じになる
        /// </summary>
        [Fact]
        public void DisplayAndHelperReaders_AgreeOnTitleVersionAndBackground()
        {
            // 14-9
            string path = WriteOsuFile("compare.osu", new OsuFileBuilder().Build());

            var display = Common.TryDecodeBeatmap(path);
            var helper = BeatmapHelper.GetBeatmapData(path);
            BeatmapMetadata info = new() { beatmapPath = path };
            BeatmapHelper.GetBackground(ref info);

            Assert.NotNull(display);
            Assert.NotEqual(string.Empty, helper.version);
            Assert.Contains($"Title:{display.MetadataSection.Title}", helper.metadata);
            Assert.Contains($"Version:{display.MetadataSection.Version}", helper.metadata);
            Assert.Contains(display.EventsSection.BackgroundImage, info.background);
        }

        /// <summary>
        /// 手元の全譜面の一括テスト (14-3・14-9)。環境変数 OSU_SONGS_PATH に Songs フォルダを指定したときだけ動く
        /// </summary>
        /// <remarks>
        /// 本物の譜面は読むだけで、書き出しは作業フォルダにコピーしてから行う。
        /// 結果(読めない譜面の数、書き出しで [HitObjects] が変わる譜面の数など)は出力に書くので、前回の結果と比べる
        /// </remarks>
        [Fact]
        public void LocalSongs_ReadAndExportAll()
        {
            string? songs = Environment.GetEnvironmentVariable("OSU_SONGS_PATH");
            if (string.IsNullOrEmpty(songs) || !Directory.Exists(songs))
            {
                output.WriteLine("OSU_SONGS_PATH が指定されていないため、何もしない");
                return;
            }
            DateTime startTime = DateTime.Now;
            int total = 0, readFailed = 0, displayFailed = 0, hitObjectsChanged = 0, eventsChanged = 0;
            foreach (var path in Directory.EnumerateFiles(songs, "*.osu", SearchOption.AllDirectories))
            {
                var lines = File.ReadAllLines(path);
                if (!lines.Any(l => l.Trim() == "Mode: 1")) continue;
                total++;
                if (Common.TryDecodeBeatmap(path) == null) displayFailed++;
                string copy = Path.Combine(WorkDirectory, "copy.osu");
                File.Copy(path, copy, true);
                var beatmap = BeatmapHelper.GetBeatmapData(copy);
                if (beatmap.version == string.Empty)
                {
                    readFailed++;
                    continue;
                }
                Assert.True(BeatmapHelper.ExportToOsuFile(beatmap, copy));
                var exported = File.ReadAllLines(copy);
                if (!Section(lines, "[HitObjects]").SequenceEqual(Section(exported, "[HitObjects]"))) hitObjectsChanged++;
                if (!Section(lines, "[Events]").SequenceEqual(Section(exported, "[Events]"))) eventsChanged++;
            }
            output.WriteLine($"太鼓の譜面 {total}、読めない {readFailed}、表示用の読み込みで読めない {displayFailed}、書き出しで [HitObjects] が変わる {hitObjectsChanged}、[Events] が変わる {eventsChanged}");
            // 本物の譜面を書き換えていない
            Assert.DoesNotContain(Directory.EnumerateFiles(songs, "*.osu", SearchOption.AllDirectories), f => File.GetLastWriteTime(f) > startTime);
        }

        private static List<string> Section(IEnumerable<string> lines, string header) =>
            [.. lines.SkipWhile(l => l.Trim() != header).Skip(1).TakeWhile(l => !l.TrimStart().StartsWith('[')).Where(l => l.Trim() != "")];
    }
}
