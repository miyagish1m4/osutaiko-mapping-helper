using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Models;
using System.Globalization;

namespace osu_taiko_Mapping_Helper
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            // .osuの数値はOSの地域設定に関係なく小数点が"."のため、数値・日付の書式を地域設定によらない形にする
            // (画面の表示言語(CurrentUICulture)は変更しない)
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;
            // ログやバックアップなどはカレントディレクトリを基準に保存するため、起動方法(ショートカットの作業フォルダ、
            // アップデート後の再起動など)に関係なくexeのフォルダを基準にする
            Directory.SetCurrentDirectory(AppContext.BaseDirectory);

            var config = new Config();
            config.ConfigLoad();
            Common.LoadConfig(config);

            Application.Run(new MainForm());
        }
    }
}
