using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Models;
using System.Text;

namespace osu_taiko_Mapping_Helper.Utils.Helper
{
    /// <summary>
    /// デバッグ用のメソッドを集約したクラス
    /// </summary>
    internal class Debug
    {
        /// <summary>
        /// TimingPointsのCSVファイル出力処理
        /// </summary>
        /// <param name="beatmap">譜面データ</param>
        /// <param name="backupDirectory">バックアップフォルダ</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool ExportToCsvFile(Beatmap beatmap, string backupDirectory)
        {
            string path = Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY + "\\" + backupDirectory;
            DateTime now = DateTime.Now;
            string backupFileName = $"{now:yyyy_MM_dd_HH_mm_ss_fff}" + Constants.CSV_EXTENSION;
            // バックアップフォルダがない場合は作成する
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            StreamWriter file = new(path + "\\" + backupFileName, false, Encoding.GetEncoding("utf-8"));
            beatmap.timingPoints = [.. beatmap.timingPoints.OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0)];
            string Header = "time,bpm,sv,barLength,meter,sampleSet,sampleIndex,volume,isRedLine,effect";
            // ヘッダーを書き込む
            file.WriteLine(Header);
            try
            {
                // データを書き込む
                foreach (var timingPoint in beatmap.timingPoints)
                {

                    string timingPointLine = timingPoint.time + "," +
                                             timingPoint.bpm + "," +
                                             timingPoint.sv + "," +
                                             timingPoint.barLength + "," +
                                             timingPoint.meter + "," +
                                             timingPoint.sampleSet + "," +
                                             timingPoint.sampleIndex + "," +
                                             timingPoint.volume + "," +
                                             (timingPoint.isRedLine ? "1" : "0") + "," +
                                             timingPoint.effect;
                    file.WriteLine(timingPointLine);
                }
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXPORT-OSU");
                Common.WriteExceptionMessage(ex);
                return false;
            }
            finally
            {
                // いかなる場合でもファイルを閉じる
                file.Close();
            }
            return true;
        }
        /// <summary>
        /// 入力値のCSVファイル出力処理
        /// </summary>
        /// <param name="userInputData">入力値データ</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool ExportToUserInputData(UserInputData userInputData)
        {
            string path = Directory.GetCurrentDirectory() + Constants.HISTORY_DIRECTORY;
            DateTime now = DateTime.Now;
            string backupFileName = $"history" + Constants.CSV_EXTENSION;
            try
            {
                if(!File.Exists(path + "\\" + backupFileName))
                {
                    File.Create(path + "\\" + backupFileName).Close();
                }
                StreamReader srFile = new(path + "\\" + backupFileName, Encoding.GetEncoding("utf-8"));
                string line = srFile.ReadToEnd();
                srFile.Close();
                StreamWriter swFile = new(path + "\\" + backupFileName, true, Encoding.GetEncoding("utf-8"));
                if (string.IsNullOrEmpty(line) || line.Length == 0)
                {
                    string Header = "timingFrom,timingTo,isSv,svFrom,svTo,isVolume,volumeFrom,volumeTo,calculationCode,isKiai,relativeCode,relativeBaseSv,isOffset,offset,setOption.isSetObjects,setOption.isSetBeatSnap,setOption.isSetGreenLine,setObjectOption.setObjectsCode,setObjectOption.isTimingStart,setObjectOption.isTimingEnd,setBeatSnapOption.beatSnap,setBeatSnapOption.isBeatSnap,createDate";
                    // ヘッダーを書き込む
                    swFile.WriteLine(Header);
                }
                // データを書き込む
                string timingPointLine = userInputData.timingFrom + "," +
                                         userInputData.timingTo + "," +
                                         userInputData.isSv + "," +
                                         userInputData.svFrom + "," +
                                         userInputData.svTo + "," +
                                         userInputData.isVolume + "," +
                                         userInputData.volumeFrom + "," +
                                         userInputData.volumeTo + "," +
                                         userInputData.calculationCode + "," +
                                         userInputData.isKiai + "," +
                                         userInputData.relativeCode + "," +
                                         userInputData.relativeBaseSv + "," +
                                         userInputData.isOffset + "," +
                                         userInputData.offset + "," +
                                         userInputData.setOption.isSetObjects + "," +
                                         userInputData.setOption.isSetBeatSnap + "," +
                                         userInputData.setOption.isSetGreenLine + "," +
                                         userInputData.setObjectOption.setObjectsCode + "," +
                                         //userInputData.setObjectOption.isKiaiStart + "," +
                                         //userInputData.setObjectOption.isKiaiEnd + "," +
                                         userInputData.setObjectOption.isTimingStart + "," +
                                         userInputData.setObjectOption.isTimingEnd + "," +
                                         userInputData.setBeatSnapOption.beatSnap + "," +
                                         userInputData.setBeatSnapOption.isBeatSnap + "," +
                                         userInputData.createDate;
                swFile.WriteLine(timingPointLine);
                // いかなる場合でもファイルを閉じる
                swFile.Close();
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXPORT-OSU");
                Common.WriteExceptionMessage(ex);
                return false;
            }
            finally
            {
            }
            return true;
        }
    }
}
