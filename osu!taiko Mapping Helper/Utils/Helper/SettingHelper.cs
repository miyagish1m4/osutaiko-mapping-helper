using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;

namespace osu_taiko_Mapping_Helper.Utils.Helper
{
    internal class SettingHelper
    {
        /// <summary>
        /// 設定画面で指定された値をconfigファイルに設定する
        /// </summary>
        /// <param name="language">言語</param>
        /// <param name="maxBackupCount">バックアップの最大保持数</param>
        /// <param name="maxHistoryCount">入力履歴ファイルの最大保持数</param>
        /// <param name="isAdvanceMode">アドバンスモード有効化フラグ</param>
        /// <param name="notesPosition">ノーツ座標</param>
        /// <param name="config">コンフィグクラス</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool SetConfig(string language,
                                       string maxBackupCount,
                                       bool isAdvanceMode,
                                       bool isUnicodeSupport,
                                       NotesPosition notesPosition,
                                       Config config)
        {
            try
            {
                int retMaxBackupCount = 0;
                int retDonX = 0;
                int retDonY = 0;
                int retKatX = 0;
                int retKatY = 0;
                int retFinisherDonX = 0;
                int retFinisherDonY = 0;
                int retFinisherKatX = 0;
                int retFinisherKatY = 0;
                int advanceMode = isAdvanceMode ? 1 : 0;
                int unicodeSupport = isUnicodeSupport ? 1 : 0;
                // 入力された値のバリデーションチェックをする
                if (!ValidateMaxBackupCount(maxBackupCount, ref retMaxBackupCount) ||
                    !ValidatePosition(notesPosition.donX, ref retDonX, 0) ||
                    !ValidatePosition(notesPosition.donY, ref retDonY, 1) ||
                    !ValidatePosition(notesPosition.katX, ref retKatX, 0) ||
                    !ValidatePosition(notesPosition.katY, ref retKatY, 1) ||
                    !ValidatePosition(notesPosition.finisherDonX, ref retFinisherDonX, 0) ||
                    !ValidatePosition(notesPosition.finisherDonY, ref retFinisherDonY, 1) ||
                    !ValidatePosition(notesPosition.finisherKatX, ref retFinisherKatX, 0) ||
                    !ValidatePosition(notesPosition.finisherKatY, ref retFinisherKatY, 1))
                {

                    return false;
                }
                config.language = language;
                config.maxBackupCount = retMaxBackupCount;
                config.unicodeSupport = unicodeSupport;
                config.advanceMode = advanceMode;
                config.donX = retDonX;
                config.donY = retDonY;
                config.katX = retKatX;
                config.katY = retKatY;
                config.finisherDonX = retFinisherDonX;
                config.finisherDonY = retFinisherDonY;
                config.finisherKatX = retFinisherKatX;
                config.finisherKatY = retFinisherKatY;
                config.ConfigSave();
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// バックアップの最大保持数のバリデーションチェックをする関数
        /// </summary>
        /// <param name="maxBackupCount">バックアップの最大保持数</param>
        /// <param name="retMaxBackupCount">チェック後のバックアップの最大保持数</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ValidateMaxBackupCount(string maxBackupCount, ref int retMaxBackupCount)
        {
            try
            {
                if (maxBackupCount == string.Empty)
                {
                    //バックアップの保持上限の入力がない
                    Common.ShowMessageDialog("E_V-EM-1");
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(maxBackupCount, @"^[-+]?[0-9]*\.?[0-9]+$"))
                {
                    //バックアップの保持上限が数値で指定されていない
                    Common.ShowMessageDialog("E_V-T-2");
                    return false;
                }
                if (!int.TryParse(maxBackupCount, out retMaxBackupCount))
                {
                    //バックアップの保持上限が整数で指定されていない
                    Common.ShowMessageDialog("E_V-T-1");
                    return false;
                }
                if ((retMaxBackupCount < 1) || (retMaxBackupCount > 100))
                {
                    //バックアップの最大保持数が1～100以内ではない
                    Common.ShowMessageDialog("E_V-C-1");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// ノーツ座標のバリデーションチェックをする関数
        /// </summary>
        /// <param name="position">ノーツ座標</param>
        /// <param name="retPosition">チェック後のノーツ座標</param>
        /// <param name="dimension">次元(x,y)</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ValidatePosition(string position, ref int retPosition, int dimension)
        {
            try
            {
                int maxPosition = dimension == 0 ? 512 : 384;
                if (position == string.Empty)
                {
                    //ノーツの座標が指定されていない
                    Common.ShowMessageDialog(dimension == 0 ? "E_V-EM-2" : "E_V-EM-3");
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(position, @"^[-+]?[0-9]*\.?[0-9]+$"))
                {
                    //ノーツの座標が数値で指定されていない
                    Common.ShowMessageDialog(dimension == 0 ? "E_V-T-4" : "E_V-T-6");
                    return false;
                }
                if (!int.TryParse(position, out retPosition))
                {
                    //ノーツの座標が整数で指定されていない
                    Common.ShowMessageDialog(dimension == 0 ? "E_V-T-3" : "E_V-T-5");
                    return false;
                }
                if ((retPosition < 0) || (retPosition > maxPosition))
                {
                    //ノーツの座標が指定の範囲内で指定されていない
                    Common.ShowMessageDialog(dimension == 0 ? "E_V-C-2" : "E_V-C-3");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// バックアップの最大保持数分新しいバックアップファイルを残す
        /// </summary>
        /// <param name="config">コンフィグクラス</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool ResetBackupFile(Config config)
        {
            try
            {
                string backupPath = Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY + "\\";
                string[] songPath = Directory.GetDirectories(backupPath);
                // バックアップを作成した譜面の数分ループ
                for (global::System.Int32 i = 0; i < songPath.Length; i++)
                {
                    // バックアップフォルダ内にあるosuファイルを取得
                    string[] backupFiles = Directory.GetFiles(songPath[i], "*" + Constants.OSU_EXTENSION);
                    List<long> fileDate = [];
                    // ファイル名の日付のみを取得し、数値にする
                    foreach (var file in backupFiles)
                    {
                        string date = file.Replace(songPath[i] + "\\", "")
                                          .Replace(Constants.OSU_EXTENSION, "")
                                          .Replace("_", "");
                        fileDate.Add(Convert.ToInt64(date));
                    }
                    // 数値を降順にソートする
                    fileDate.Sort();
                    fileDate.Reverse();
                    // バックアップの最大保持数分新しいファイルのみ残す
                    for (global::System.Int32 j = (fileDate.Count) - (1); j >= config.maxBackupCount; j--)
                    {
                        string targetFileName = fileDate[j].ToString("0000_00_00_00_00_00_000");
                        targetFileName = Path.Combine(songPath[i], targetFileName + Constants.OSU_EXTENSION);
                        File.Delete(targetFileName);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
    }
}
