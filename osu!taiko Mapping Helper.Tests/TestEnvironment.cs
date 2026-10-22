using System.Globalization;
using System.Text;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils;

// SVCalculatorServiceなどがstaticで状態を持つため、テストを同時に実行しない
[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace osu_taiko_Mapping_Helper.Tests
{
    /// <summary>
    /// テストの共通の準備と後片付け
    /// </summary>
    /// <remarks>
    /// ・作業フォルダを一時フォルダに切り替える (バックアップ・temp・ログはカレントディレクトリに作られるため、本物のフォルダを汚さない)<br/>
    /// ・ダイアログを表示せず、表示しようとしたメッセージコードを記録する<br/>
    /// ・Helperの起動時(Program.cs)と同じく、数値の読み書きをInvariantCultureにする
    /// </remarks>
    public abstract class TestEnvironment : IDisposable
    {
        private readonly string previousDirectory;
        private readonly CultureInfo previousCulture;
        // テストごとの作業フォルダ
        protected string WorkDirectory { get; }

        protected TestEnvironment()
        {
            previousDirectory = Directory.GetCurrentDirectory();
            previousCulture = CultureInfo.CurrentCulture;
            WorkDirectory = Path.Combine(Path.GetTempPath(), "osu-taiko-mapping-helper-tests", Guid.NewGuid().ToString("N"));
            Directory.CreateDirectory(WorkDirectory);
            Directory.SetCurrentDirectory(WorkDirectory);
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            // ログの書き出し先を作る
            Common.InitializeDirectoryAndFiles();
            Common.isTestMode = true;
            Common.testMessageCodes.Clear();
            Common.testDialogResult = true;
            // 画面の文字(ラベル)は設定の言語で決まるため、言語を日本語にする
            // (言語を変えるテストがあっても次のテストに残らないよう、テストごとに設定し直す)
            Common.LoadConfig(new Config { language = "日本語" });
        }

        public void Dispose()
        {
            Common.isTestMode = false;
            Common.testMessageCodes.Clear();
            Directory.SetCurrentDirectory(previousDirectory);
            CultureInfo.CurrentCulture = previousCulture;
            try
            {
                Directory.Delete(WorkDirectory, true);
            }
            catch
            {
                // 後片付けに失敗しても、テストの結果には影響させない
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// テスト中に表示しようとしたメッセージコード
        /// </summary>
        protected static List<string> MessageCodes => Common.testMessageCodes;

        /// <summary>
        /// 作業フォルダに.osuファイルを作る (BOMなしのUTF-8、改行はCRLF)
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="lines">ファイルの中身 (1行ずつ)</param>
        /// <returns>作ったファイルのパス</returns>
        protected string WriteOsuFile(string fileName, IEnumerable<string> lines)
        {
            string path = Path.Combine(WorkDirectory, fileName);
            File.WriteAllText(path, string.Join("\r\n", lines) + "\r\n", new UTF8Encoding(false));
            return path;
        }
    }
}
