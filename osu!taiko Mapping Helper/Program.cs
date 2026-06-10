using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Models;

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

            var config = new Config();
            config.ConfigLoad();
            Common.LoadConfig(config);

            if (UpdaterUtils.CheckUpdate("v" + Constants.APP_VERSION)) return;
            Application.Run(new MainForm());
        }
    }
}
