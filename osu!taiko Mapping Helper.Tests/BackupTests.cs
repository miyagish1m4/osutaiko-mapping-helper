using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// バックアップと Ctrl+Z のテスト (TEST_PATTERNS.md の 8)
    /// </summary>
    /// <remarks>
    /// バックアップの保存先は作業フォルダ(テストでは一時フォルダ)の Backup フォルダ。
    /// フォルダ名を .osu のファイル名にする処理(8-1)は MainForm の中にあるため、ここでは保存先のパスの作り方を確かめる
    /// </remarks>
    public class BackupTests : TestEnvironment
    {
        private string BackupRoot => Path.Combine(WorkDirectory, "Backup");

        /// <summary>
        /// バックアップの形式の名前 (yyyy_MM_dd_HH_mm_ss_fff.osu) のファイルを作り、更新日時を設定する
        /// </summary>
        private static string CreateBackupFile(string directory, DateTime time, string content = "")
        {
            Directory.CreateDirectory(directory);
            string path = Path.Combine(directory, $"{time:yyyy_MM_dd_HH_mm_ss_fff}.osu");
            File.WriteAllText(path, content);
            File.SetLastWriteTime(path, time);
            return path;
        }

        /// <summary>
        /// バックアップは、作業フォルダの Backup フォルダの中に、指定した名前のフォルダで作る
        /// </summary>
        [Fact]
        public void CreateBackup_CreatesNamedFolderInsideBackupFolder()
        {
            // 8-1
            string osu = WriteOsuFile("a - t (m) [Oni].osu", new OsuFileBuilder().Build());

            Assert.True(BeatmapHelper.CreateBackup(osu, "a - t (m) [Oni]"));

            string directory = Path.Combine(BackupRoot, "a - t (m) [Oni]");
            Assert.Equal(directory, BeatmapHelper.GetBackupDirectoryPath("a - t (m) [Oni]"));
            var file = Assert.Single(BeatmapHelper.GetBackupFiles(directory));
            Assert.Equal(File.ReadAllText(osu), File.ReadAllText(file));
        }

        /// <summary>
        /// フォルダ名に使えない記号は空白に置き換え、バックアップの作成と Ctrl+Z で同じ場所を使う
        /// </summary>
        [Fact]
        public void BackupDirectoryPath_ReplacesInvalidCharactersAndSharesLocationWithUndo()
        {
            // 8-2 (M7)
            string osu = WriteOsuFile("symbols.osu", new OsuFileBuilder().Build());
            string name = "a: b, c? d* e\" f< g> h| i/ j\\ k";

            Assert.True(BeatmapHelper.CreateBackup(osu, name));
            File.WriteAllText(osu, "changed");
            Assert.True(BeatmapHelper.ExportToPreviousOsuFile(osu, name));

            Assert.Equal(Path.Combine(BackupRoot, "a  b  c  d  e  f  g  h  i  j  k"), BeatmapHelper.GetBackupDirectoryPath(name));
            Assert.NotEqual("changed", File.ReadAllText(osu));
        }

        /// <summary>
        /// バックアップの形式でないファイルは、バックアップとして数えない
        /// </summary>
        [Fact]
        public void GetBackupFiles_IgnoresFilesNotInBackupFormat()
        {
            // 8-3 (M3)
            string directory = Path.Combine(BackupRoot, "map");
            var newer = CreateBackupFile(directory, new DateTime(2026, 1, 2));
            var older = CreateBackupFile(directory, new DateTime(2026, 1, 1));
            File.WriteAllText(Path.Combine(directory, "copy.osu"), "");
            File.WriteAllText(Path.Combine(directory, "2026_01_01.osu"), "");

            Assert.Equal([newer, older], BeatmapHelper.GetBackupFiles(directory));
        }

        /// <summary>
        /// 保持数を超えたバックアップは古い順に消え、形式の違うファイルは消さない
        /// </summary>
        [Fact]
        public void ResetBackupFile_DeletesOldestOverLimitAndKeepsOtherFiles()
        {
            // 8-3・8-4・8-9 (M3。どの譜面のフォルダでも整理する)
            string mapA = Path.Combine(BackupRoot, "A");
            string mapB = Path.Combine(BackupRoot, "B");
            var a3 = CreateBackupFile(mapA, new DateTime(2026, 1, 3));
            var a2 = CreateBackupFile(mapA, new DateTime(2026, 1, 2));
            var a1 = CreateBackupFile(mapA, new DateTime(2026, 1, 1));
            var b2 = CreateBackupFile(mapB, new DateTime(2026, 2, 2));
            var b1 = CreateBackupFile(mapB, new DateTime(2026, 2, 1));
            var b0 = CreateBackupFile(mapB, new DateTime(2026, 1, 31));
            string other = Path.Combine(mapA, "copy.osu");
            File.WriteAllText(other, "");

            Assert.True(SettingHelper.ResetBackupFile(new Config { maxBackupCount = 2 }));

            Assert.True(File.Exists(a3));
            Assert.True(File.Exists(a2));
            Assert.False(File.Exists(a1));
            Assert.True(File.Exists(b2));
            Assert.True(File.Exists(b1));
            Assert.False(File.Exists(b0));
            Assert.True(File.Exists(other));
        }

        /// <summary>
        /// 更新日時が同じバックアップは、名前の新しい順に並ぶ
        /// </summary>
        [Fact]
        public void GetBackupFiles_SameWriteTime_OrdersByNameDescending()
        {
            // 8-4・14-12
            string directory = Path.Combine(BackupRoot, "map");
            var time = new DateTime(2026, 1, 1, 12, 0, 0);
            string first = CreateBackupFile(directory, new DateTime(2026, 1, 1, 12, 0, 0, 100));
            string second = CreateBackupFile(directory, new DateTime(2026, 1, 1, 12, 0, 0, 200));
            File.SetLastWriteTime(first, time);
            File.SetLastWriteTime(second, time);

            Assert.Equal([second, first], BeatmapHelper.GetBackupFiles(directory));
        }

        /// <summary>
        /// 時計を戻した後に作ったバックアップも、更新日時の新しい順に並ぶ
        /// </summary>
        [Fact]
        public void GetBackupFiles_AfterClockSetBack_OrdersByWriteTimeDescending()
        {
            // 14-12 (名前の日時より、ファイルの更新日時を優先する)
            string directory = Path.Combine(BackupRoot, "map");
            string namedLater = CreateBackupFile(directory, new DateTime(2026, 5, 1));
            string namedEarlier = CreateBackupFile(directory, new DateTime(2026, 4, 1));
            File.SetLastWriteTime(namedLater, new DateTime(2026, 1, 1));
            File.SetLastWriteTime(namedEarlier, new DateTime(2026, 6, 1));

            Assert.Equal([namedEarlier, namedLater], BeatmapHelper.GetBackupFiles(directory));
        }

        /// <summary>
        /// Ctrl+Z では一番新しいバックアップに戻り、2 回続けても同じバックアップに戻る
        /// </summary>
        [Fact]
        public void Undo_RestoresNewestBackupEvenWhenRepeated()
        {
            // 8-5・8-10
            string osu = WriteOsuFile("map.osu", ["current"]);
            string directory = BeatmapHelper.GetBackupDirectoryPath("map");
            CreateBackupFile(directory, new DateTime(2026, 1, 1), "older");
            CreateBackupFile(directory, new DateTime(2026, 1, 2), "newest");

            Assert.True(BeatmapHelper.ExportToPreviousOsuFile(osu, "map"));
            Assert.Equal("newest", File.ReadAllText(osu));

            File.WriteAllText(osu, "current");
            Assert.True(BeatmapHelper.ExportToPreviousOsuFile(osu, "map"));
            Assert.Equal("newest", File.ReadAllText(osu));
        }

        /// <summary>
        /// Ctrl+Z でバックアップのフォルダが無ければ失敗し、譜面は変わらない
        /// </summary>
        [Fact]
        public void Undo_WithoutBackupFolder_FailsAndKeepsBeatmap()
        {
            // 8-10 (MainForm はフォルダが無いときは何もしない)
            string osu = WriteOsuFile("map.osu", ["current"]);

            Assert.False(BeatmapHelper.ExportToPreviousOsuFile(osu, "map"));
            Assert.Equal("current\r\n", File.ReadAllText(osu));
        }

        /// <summary>
        /// Ctrl+Z でフォルダにバックアップが無ければ失敗する
        /// </summary>
        [Fact]
        public void Undo_WithoutBackupFiles_Fails()
        {
            // 8-10 (MainForm は E_A-P-1 を出す)
            string osu = WriteOsuFile("map.osu", ["current"]);
            string directory = BeatmapHelper.GetBackupDirectoryPath("map");
            Directory.CreateDirectory(directory);
            File.WriteAllText(Path.Combine(directory, "copy.osu"), "not backup");

            Assert.False(BeatmapHelper.ExportToPreviousOsuFile(osu, "map"));
            Assert.Equal("current\r\n", File.ReadAllText(osu));
        }

        /// <summary>
        /// パスが 260 文字を超えても、バックアップの作成・復元・整理ができる
        /// </summary>
        [Fact]
        public void Backup_LongPathOver260_CreatesRestoresAndCleansUp()
        {
            // 8-6
            string name = new('a', 240);
            string osu = WriteOsuFile("long.osu", new OsuFileBuilder().Build());
            Assert.True(BeatmapHelper.GetBackupDirectoryPath(name).Length > 260);

            Assert.True(BeatmapHelper.CreateBackup(osu, name));
            File.WriteAllText(osu, "changed");
            Assert.True(BeatmapHelper.ExportToPreviousOsuFile(osu, name));
            Assert.True(SettingHelper.ResetBackupFile(new Config { maxBackupCount = 1 }));

            Assert.NotEqual("changed", File.ReadAllText(osu));
        }

        /// <summary>
        /// temp ファイルの後片付けでは、そのファイルだけを消す
        /// </summary>
        [Fact]
        public void DeleteTemporaryFile_DeletesOnlyThatFile()
        {
            // 8-7 (H4。同時に起動している別の Helper の temp ファイルを消さない)
            string temp = Path.Combine(WorkDirectory, "temp");
            Directory.CreateDirectory(temp);
            string mine = Path.Combine(temp, "mine.tmp");
            string others = Path.Combine(temp, "others.tmp");
            File.WriteAllText(mine, "");
            File.WriteAllText(others, "");

            TestReflection.InvokeStatic(typeof(BeatmapHelper), "DeleteTemporaryFile", mine);

            Assert.False(File.Exists(mine));
            Assert.True(File.Exists(others));
            Assert.True(Directory.Exists(temp));
        }

        /// <summary>
        /// バックアップのフォルダを作れなければ失敗する
        /// </summary>
        [Fact]
        public void CreateBackup_CannotCreateFolder_Fails()
        {
            // 8-8 (同じ名前のファイルがあってフォルダを作れない)
            string osu = WriteOsuFile("map.osu", new OsuFileBuilder().Build());
            Directory.CreateDirectory(BackupRoot);
            File.WriteAllText(Path.Combine(BackupRoot, "map"), "this is a file");

            Assert.False(BeatmapHelper.CreateBackup(osu, "map"));
        }

        /// <summary>
        /// 前の版の名前のバックアップフォルダが残っていても影響しない
        /// </summary>
        [Fact]
        public void Backup_OldVersionFolderRemaining_DoesNotAffect()
        {
            // 14-13 (M2 より前はメタデータから作った名前のフォルダだった)
            string osu = WriteOsuFile("a - t (m) [Oni].osu", ["current"]);
            CreateBackupFile(Path.Combine(BackupRoot, "artist - title (creator) [Oni]"), new DateTime(2026, 1, 1), "old version folder");

            Assert.False(BeatmapHelper.ExportToPreviousOsuFile(osu, "a - t (m) [Oni]"));
            Assert.Equal("current\r\n", File.ReadAllText(osu));
        }
    }
}
