using System.Configuration;

namespace osu_taiko_Mapping_Helper.Models
{
    /// <summary>
    /// 設定情報クラス
    /// </summary>
    internal class Config
    {
        // バックアップの最大保持数
        internal int maxBackupCount { get; set; }
        // 言語設定
        internal string? language { get; set; }
        // オフセットモード設定
        internal int offsetMode { get; set; }
        // Unicode対応の有無
        internal int unicodeSupport { get; set; }
        // AdvanceMode設定
        internal int advanceMode { get; set; }
        internal int donX { get; set; }
        internal int donY { get; set; }
        internal int katX { get; set; }
        internal int katY { get; set; }
        internal int finisherDonX { get; set; }
        internal int finisherDonY { get; set; }
        internal int finisherKatX { get; set; }
        internal int finisherKatY { get; set; }
        /// <summary>
        /// configファイルの読み込み処理
        /// </summary>
        internal void ConfigLoad()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // config読み込み
            maxBackupCount = Convert.ToInt32(config.AppSettings.Settings["maxBackupCount"].Value);
            language = config.AppSettings.Settings["language"].Value;
            offsetMode = Convert.ToInt32(config.AppSettings.Settings["offsetMode"].Value);
            unicodeSupport = Convert.ToInt32(config.AppSettings.Settings["unicodeSupport"].Value);
            advanceMode = Convert.ToInt32(config.AppSettings.Settings["advanceMode"].Value);
            donX = Convert.ToInt32(config.AppSettings.Settings["donX"].Value);
            donY = Convert.ToInt32(config.AppSettings.Settings["donY"].Value);
            katX = Convert.ToInt32(config.AppSettings.Settings["katX"].Value);
            katY = Convert.ToInt32(config.AppSettings.Settings["katY"].Value);
            finisherDonX = Convert.ToInt32(config.AppSettings.Settings["finisherDonX"].Value);
            finisherDonY = Convert.ToInt32(config.AppSettings.Settings["finisherDonY"].Value);
            finisherKatX = Convert.ToInt32(config.AppSettings.Settings["finisherKatX"].Value);
            finisherKatY = Convert.ToInt32(config.AppSettings.Settings["finisherKatY"].Value);
        }
        /// <summary>
        /// configファイルの書き込み処理
        /// </summary>
        internal void ConfigSave()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

            // config書き込み
            config.AppSettings.Settings["maxBackupCount"].Value = maxBackupCount.ToString();
            config.AppSettings.Settings["language"].Value = language;
            config.AppSettings.Settings["offsetMode"].Value = offsetMode.ToString();
            config.AppSettings.Settings["unicodeSupport"].Value = unicodeSupport.ToString();
            config.AppSettings.Settings["advanceMode"].Value = advanceMode.ToString();
            config.AppSettings.Settings["donX"].Value = donX.ToString();
            config.AppSettings.Settings["donY"].Value = donY.ToString();
            config.AppSettings.Settings["katX"].Value = katX.ToString();
            config.AppSettings.Settings["katY"].Value = katY.ToString();
            config.AppSettings.Settings["finisherDonX"].Value = finisherDonX.ToString();
            config.AppSettings.Settings["finisherDonY"].Value = finisherDonY.ToString();
            config.AppSettings.Settings["finisherKatX"].Value = finisherKatX.ToString();
            config.AppSettings.Settings["finisherKatY"].Value = finisherKatY.ToString();
            config.Save();
        }
    }
}
