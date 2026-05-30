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
        /// 現地点のSVを取得する関数
        /// </summary>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="currentTime">タイミング</param>
        /// <returns>SVの値</returns>
        internal static string SetCurrentBpm(Beatmap? beatmap, int currentTime)
        {
            TimingPoint timingPointBuff = new();
            double retSv = 0;
            string retSvStr = string.Empty;
            try
            {
                if (beatmap == null)
                {
                    throw new Exception();
                }
                // 現在のタイミングのBPMを取得する (小数点第15位まで)
                retSv = Math.Round(beatmap.timingPoints.LastOrDefault(tp => tp.isRedLine && tp.time <= currentTime)?.bpm ?? -1, 5, MidpointRounding.AwayFromZero);
                if (retSv == -1)
                {
                    throw new Exception();
                }
                retSvStr = retSv.ToString();
                if (retSvStr.Contains('.'))
                {
                    // SVが整数だった場合は"0"と"."を削除する
                    retSvStr = retSvStr.TrimEnd('0');
                    retSvStr = retSvStr.TrimEnd('.');
                }
                return retSvStr;
            }
            catch
            {
                return string.Empty;
            }
        }
        internal static void ExportHitObejcts(Beatmap beatmap)
        {
            string path = Directory.GetCurrentDirectory() + Constants.DEBUG_DIRECTORY;
            DateTime now = DateTime.Now;
            string csvFileName = $"{now:yyyy_MM_dd_HH_mm_ss_fff}" + Constants.CSV_EXTENSION;
            if (!File.Exists(path + "\\" + csvFileName))
            {
                File.Create(path + "\\" + csvFileName).Close();
            }
            StreamWriter? file = null;
            file = new(path + "\\" + csvFileName, false, Encoding.GetEncoding("utf-8"));
            file.WriteLine("");
            file.WriteLine(Constants.HIT_OBJECTS);
            for (global::System.Int32 i = 0; i < beatmap.hitObjects.Count; i++)
            {
                int endTime = int.MinValue;
                if (i != beatmap.hitObjects.Count - 1 && beatmap.hitObjects[i].noteType == Constants.NoteType.SPINNER)
                {
                    for (int j = i; j < beatmap.hitObjects.Count; j++)
                    {
                        if (beatmap.hitObjects[j].noteType == Constants.NoteType.SPINNER_END)
                        {
                            endTime = beatmap.hitObjects[j].time;
                            break;
                        }
                    }
                }
                // HitObjectsの1行のデータを作成する
                string hitObjectLine = CreateHitObjectLine(beatmap.hitObjects[i],
                                                           endTime);
                file.WriteLine(hitObjectLine);
            }
            file.Close();
        }
        private static string CreateHitObjectLine(HitObject hitObject, int endTime)
        {
            int positionX = hitObject.positionX;
            int positionY = hitObject.positionY;
            string hitSound = hitObject.hitSound;
            positionX = hitObject.positionX;
            positionY = hitObject.positionY;
            StringBuilder sb = new();
            sb.Append(Constants.ConvertNoteType(hitObject.noteType) + ",");
            sb.Append(positionX + ",");
            sb.Append(positionY + ",");
            sb.Append(hitObject.time + ",");
            sb.Append(hitObject.type + ",");
            sb.Append(hitSound + ",");
            if (hitObject.noteType == Constants.NoteType.SLIDER)
            {
                // スライダーの場合はsliderLengthを13桁に指定して
                // curveSetting,
                // slides,
                // sliderLength,
                // (edgeSounds),
                // (edgeSets),
                // (hitSample)を設定する
                string sliderLength = hitObject.sliderLength.ToString($"F13").TrimEnd('0');
                if (sliderLength.Substring(sliderLength.Length - 1, 1) == ".")
                {
                    sliderLength = sliderLength.TrimEnd('.');
                }
                sb.Append(hitObject.curveType + ",");
                sb.Append(hitObject.slides + ",");
                sb.Append(sliderLength);
                if ((hitObject.edgeSounds != null) && (hitObject.edgeSets) != null && (hitObject.hitSample != null))
                {
                    sb.Append(",");
                    sb.Append(hitObject.edgeSounds + ",");
                    sb.Append(hitObject.edgeSets + ",");
                    sb.Append(hitObject.hitSample);
                }
                else
                {
                    sb.Append("");
                }
            }
            else if (hitObject.noteType == Constants.NoteType.SPINNER)
            {
                // スピナーの場合はendTimeとhitSampleを設定する
                sb.Append(endTime + ",");
                sb.Append(hitObject.hitSample);
            }
            else
            {
                // 通常ノーツの場合はhitSampleを設定する
                sb.Append(hitObject.hitSample);
            }
            return sb.ToString();
        }
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
            string path = Directory.GetCurrentDirectory() + Constants.DEBUG_DIRECTORY;
            DateTime now = DateTime.Now;
            string backupFileName = $"history" + Constants.CSV_EXTENSION;
            try
            {
                if (!File.Exists(path + "\\" + backupFileName))
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
