using System.Text;
using System.Text.RegularExpressions;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;

namespace osu_taiko_Mapping_Helper.Utils.Helper
{
    /// <summary>
    /// 譜面の情報を取得し、加工するクラス
    /// </summary>
    class BeatmapHelper
    {
        /// <summary>
        /// BGを取得して、フォームの背景に設定する
        /// </summary>
        /// <param name="path">ファイルパス</param>
        /// <returns>加工したBGデータ</returns>
        internal static Bitmap SetBgOnForm(string path)
        {
            const double DARKNESS_FACTOR = 0.5;
            Bitmap canvas = new(384, 216);
            try
            {
                if (path == "")
                {
                    throw new Exception("背景画像が見つかりませんでした。");
                }
                Bitmap image = new(path);
                float width = 0;
                float height = 0;
                float diffWidth = 0;
                float diffHeight = 0;
                float zoomRatio = 0;
                // 横長画像の場合
                if (((float)image.Height / (float)image.Width) < 0.5625f)
                {
                    // 縦の長さから拡大率を求める
                    zoomRatio = ((float)image.Height / 216f);
                    // 縦の長さからアスペクト比が16:9の場合の横の長さを求める
                    height = image.Height;
                    float heightRatio = (float)image.Height / 9f;
                    width = heightRatio * 16;
                    // 中央部を表示したいため、表示する際の横の座標を求める
                    diffWidth = ((float)image.Width - width) / 2;
                }
                else if (((float)image.Height / (float)image.Width) > 0.5625f)
                {
                    // 横の長さから拡大率を求める
                    zoomRatio = ((float)image.Width / 384f);
                    // 横の長さからアスペクト比が16:9の場合の縦の長さを求める
                    width = image.Width;
                    float widthRatio = (float)image.Width / 16f;
                    height = widthRatio * 9;
                    // 中央部を表示したいため、表示する際の縦の座標を求める
                    diffHeight = ((float)image.Height - height) / 2;
                }
                else
                {
                    // 拡大率を求める
                    zoomRatio = ((float)image.Width / 384f);
                    width = image.Width;
                    height = image.Height;
                }
                RectangleF destinationRect = new(0, 0, width / zoomRatio, height / zoomRatio);
                RectangleF sourceRect = new(diffWidth, diffHeight, width, height);
                Graphics graphic = Graphics.FromImage(canvas);
                // 補間方法を高品質双三次補間に設定
                graphic.InterpolationMode =
                    System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                // 表示する
                graphic.DrawImage(image, destinationRect, sourceRect, GraphicsUnit.Pixel);
                image.Dispose();
                graphic.Dispose();
                // BGの明度を下げる
                for (int y = 0; y < 216; y++)
                {
                    for (int x = 0; x < 384; x++)
                    {
                        Color pixel = canvas.GetPixel(x, y);
                        int r = (int)(pixel.R * DARKNESS_FACTOR);
                        int g = (int)(pixel.G * DARKNESS_FACTOR);
                        int b = (int)(pixel.B * DARKNESS_FACTOR);
                        Color darkPixel = Color.FromArgb(r, g, b);
                        canvas.SetPixel(x, y, darkPixel);
                    }
                }

                return canvas;
            }
            catch
            {
                Common.WriteWarningMessage("LOG_W-GET-BG");
                return canvas;
            }
        }
        /// <summary>
        /// beatmapのデータを取得する
        /// </summary>
        /// <param name="path">beatmapのパス</param>
        /// <returns>取得したデータ</returns>
        internal static Beatmap GetBeatmapData(string path, bool isThread = false)
        {
            var version = string.Empty;
            var generalList = new List<string>();
            var editorList = new List<string>();
            var metadataList = new List<string>();
            var difficultyList = new List<string>();
            var eventsList = new List<string>();
            var timingPointList = new List<TimingPoint>();
            var coloursList = new List<string>();
            var uninheritedTimingPointList = new List<TimingPoint>();
            var hitObjectList = new List<HitObject>();
            List<Bookmark> bookmarkList = [];
            try
            {
                var lines = File.ReadAllLines(path);
                // 譜面情報をセクションに区切り全取得
                if (!GetBeatmapInfoBySection(lines,
                                             ref version,
                                             ref generalList,
                                             ref editorList,
                                             ref metadataList,
                                             ref difficultyList,
                                             ref eventsList,
                                             ref timingPointList,
                                             ref coloursList,
                                             ref hitObjectList,
                                             ref bookmarkList))
                {
                    throw new Exception();
                }
                // 赤線と小節線を取得する
                if (!GetOtherObject(ref timingPointList,
                                    ref hitObjectList,
                                    ref uninheritedTimingPointList,
                                    ref bookmarkList))
                {
                    throw new Exception();
                }
                // HitObject に SVとBPMを適用(ソート後)
                if (!SetSvAndBpmOnHitObjects(timingPointList,
                                             ref hitObjectList))
                {
                    throw new Exception();
                }
                // BeatmapInfo に渡す
                return new Beatmap(version,
                                   generalList,
                                   editorList,
                                   metadataList,
                                   difficultyList,
                                   eventsList,
                                   timingPointList,
                                   coloursList,
                                   hitObjectList,
                                   bookmarkList);
            }
            catch (Exception ex)
            {
                if (!isThread)
                {
                    Common.WriteErrorMessage("LOG_E-GET-BEATMAP");
                    Common.WriteExceptionMessage(ex);
                }
                Console.WriteLine(ex);
                return new Beatmap("",
                                   generalList,
                                   editorList,
                                   metadataList,
                                   difficultyList,
                                   eventsList,
                                   timingPointList,
                                   coloursList,
                                   hitObjectList,
                                   bookmarkList);
            }
        }
        /// <summary>
        /// 譜面情報をセクションに区切りに変換する
        /// </summary>
        /// <param name="lines">osuファイルの中身</param>
        /// <param name="generalList">Generalの格納先</param>
        /// <param name="editorList">Editorの格納先</param>
        /// <param name="metadataList">Metadataの格納先</param>
        /// <param name="difficultyList">Difficultyの格納先</param>
        /// <param name="eventsList">EventsListの格納先</param>
        /// <param name="timingPointList">TimingPointの格納先</param>
        /// <param name="coloursList">Coloursの格納先</param>
        /// <param name="hitObjectList">HitObjectの格納先</param>
        /// <param name="bookmarks">Bookmarkの格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool GetBeatmapInfoBySection(string[] lines,
                                                    ref string version,
                                                    ref List<string> generalList,
                                                    ref List<string> editorList,
                                                    ref List<string> metadataList,
                                                    ref List<string> difficultyList,
                                                    ref List<string> eventsList,
                                                    ref List<TimingPoint> timingPointList,
                                                    ref List<string> coloursList,
                                                    ref List<HitObject> hitObjectList,
                                                    ref List<Bookmark> bookmarks)
        {
            int structureCode = Constants.VERSION_CODE;
            try
            {
                foreach (var line in lines)
                {
                    // 空白行は何もしない
                    if (line == "")
                    {
                        continue;
                    }
                    // bookmarkがある場合はbookmarksリストにインスタンスを作成する
                    if (line.Length >= 9)
                    {
                        if (line[..Constants.BOOKMARKS.Length] == Constants.BOOKMARKS)
                        {
                            string[] bookmarkParts = line.Split(':');
                            List<string> stringBookmarks = [.. bookmarkParts[1].Replace(" ", "").Split(",")];
                            foreach (var timing in stringBookmarks)
                            {
                                int outTiming;
                                if (!int.TryParse(timing, out outTiming))
                                {
                                    break;
                                }
                                bookmarks.Add(new Bookmark(outTiming));
                            }
                        }
                    }
                    // 構成コードを設定する
                    switch (line)
                    {
                        case Constants.GENERAL:
                            structureCode = Constants.GENERAL_CODE;
                            continue;
                        case Constants.EDITOR:
                            structureCode = Constants.EDITOR_CODE;
                            continue;
                        case Constants.METADATA:
                            structureCode = Constants.METADATA_CODE;
                            continue;
                        case Constants.DIFFICULTY:
                            structureCode = Constants.DIFFICULTY_CODE;
                            continue;
                        case Constants.EVENTS:
                            structureCode = Constants.EVENTS_CODE;
                            continue;
                        case Constants.TIMING_POINTS:
                            structureCode = Constants.TIMING_POINTS_CODE;
                            continue;
                        case Constants.COLOURS:
                            structureCode = Constants.COLOURS_CODE;
                            continue;
                        case Constants.HIT_OBJECTS:
                            structureCode = Constants.HIT_OBJECTS_CODE;
                            continue;
                        default:
                            break;
                    }
                    // 構成コードに応じて1行のデータをそれぞれのリストに格納する
                    switch (structureCode)
                    {
                        case Constants.VERSION_CODE:
                            version = line;
                            break;
                        case Constants.GENERAL_CODE:
                            generalList.Add(line);
                            break;
                        case Constants.EDITOR_CODE:
                            editorList.Add(line);
                            break;
                        case Constants.METADATA_CODE:
                            metadataList.Add(line);
                            break;
                        case Constants.DIFFICULTY_CODE:
                            difficultyList.Add(line);
                            break;
                        case Constants.EVENTS_CODE:
                            eventsList.Add(line);
                            break;
                        case Constants.TIMING_POINTS_CODE:
                            timingPointList.Add(new TimingPoint(line));
                            break;
                        case Constants.COLOURS_CODE:
                            coloursList.Add(line);
                            break;
                        case Constants.HIT_OBJECTS_CODE:
                            hitObjectList.Add(new HitObject(line));
                            break;
                        default:
                            break;
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
        /// <summary>
        /// その他のオブジェクト(スピナー終点,赤線,小節線,ブックマーク)を取得する
        /// </summary>
        /// <param name="timingPointList">赤線のリスト</param>
        /// <param name="hitObjectList">スピナーの終点,小節線の格納先(小節線はHitObjectとしてカウントする)</param>
        /// <param name="uninheritedTimingPointList">赤線の格納先</param>
        /// <param name="bookmarkList">ブックマークの格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool GetOtherObject(ref List<TimingPoint> timingPointList,
                                           ref List<HitObject> hitObjectList,
                                           ref List<TimingPoint> uninheritedTimingPointList,
                                           ref List<Bookmark> bookmarkList)
        {
            try
            {
                List<HitObject> tempHitObjects = [];

                hitObjectList = [.. hitObjectList.OrderBy(a => a.time)];
                foreach (var hitObject in hitObjectList)
                {
                    if (hitObject.noteType == Constants.NoteType.SPINNER)
                    {
                        tempHitObjects.Add(new HitObject(hitObject.endTime, 0));
                    }
                }
                // bookmarksをhitObjectに含める
                for (int i = 0; i < bookmarkList.Count; i++)
                {
                    int currentTime = bookmarkList[i].time;
                    // すでに同じ time の HitObject が存在するかをチェック
                    var hitObjectOnBookmark = hitObjectList.FirstOrDefault(h => h.time == currentTime);
                    if (hitObjectOnBookmark == null)
                    {
                        // ない場合はBookmarkをHitObjectとして追加
                        hitObjectList.Add(new HitObject(currentTime, 2));
                    }
                    else
                    {
                        // ある場合はオブジェクトコードにBookmarkを追加
                        hitObjectOnBookmark.hitObjectCode += 0x00000800;
                    }
                }
                foreach (var hitObject in hitObjectList)
                {

                    if ((hitObject.hitObjectCode & 0x00000800) == 0)
                    {
                        // オブジェクトコードにbookmarkがない場合は
                        // オブジェクトコードにbookmark以外を追加
                        hitObject.hitObjectCode += unchecked((int)0x00000400);
                    }
                    if ((hitObject.hitObjectCode & 0x00000200) == 0)
                    {
                        // オブジェクトコードに小節線がない場合は
                        // オブジェクトコードに小節線以外を追加
                        hitObject.hitObjectCode += 0x00000100;
                    }
                }
                hitObjectList.AddRange(tempHitObjects);
                hitObjectList = [.. hitObjectList.OrderBy(a => a.time)];

                // 最終ノーツを取る(これ以上は見ない)
                hitObjectList.Sort((a, b) => a.time.CompareTo(b.time));
                HitObject lastHitObject = hitObjectList.Last();

                foreach (var timingPoint in timingPointList)
                {
                    if (timingPoint.isRedLine)
                    {
                        // 赤線を追加
                        uninheritedTimingPointList.Add(timingPoint);
                        uninheritedTimingPointList.Last().sv = 1;
                    }
                }
                for (int i = 0; i < uninheritedTimingPointList.Count; i++)
                {
                    double startTime = uninheritedTimingPointList[i].time;
                    double currentTime = startTime;
                    int timeEnd = lastHitObject.time;

                    if (i + 1 < uninheritedTimingPointList.Count) timeEnd = uninheritedTimingPointList[i + 1].time;
                    double timeBar = uninheritedTimingPointList[i].barLength;
                    for (int j = 0; currentTime < timeEnd; j++)
                    {
                        int timeBarline = (int)Math.Floor(currentTime);

                        // すでに同じ time の HitObject が存在するかをチェック
                        var hitObjectOnBarLine = hitObjectList.FirstOrDefault(h => h.time == timeBarline);
                        if (hitObjectOnBarLine == null)
                        {
                            // 赤線を HitObject として追加
                            hitObjectList.Add(new HitObject(timeBarline, 1));
                        }
                        else
                        {
                            // オブジェクトコードに小節線を追加
                            hitObjectOnBarLine.hitObjectCode += unchecked((int)0x00000200);
                        }
                        currentTime = startTime + timeBar * (j + 1);
                    }
                }
                // ソートする
                timingPointList = [.. timingPointList.OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0)];
                hitObjectList = [.. hitObjectList.OrderBy(a => a.time)];
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
        /// HitObjectにSVとBPMを適用
        /// </summary>
        /// <param name="timingPointList">赤線のリスト</param>
        /// <param name="hitObjectList">適用対象のHitObject</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool SetSvAndBpmOnHitObjects(List<TimingPoint> timingPointList,
                                                    ref List<HitObject> hitObjectList)
        {
            int timingIndex = 0;
            double currentBpm = 0;
            double currentSv = 1.0;
            int svApplyTime = 0;
            try
            {
                foreach (var hitObject in hitObjectList)
                {
                    // timingPoint を進める
                    while (timingIndex + 1 < timingPointList.Count &&
                           timingPointList[timingIndex + 1].time <= hitObject.time)
                    {
                        timingIndex++;
                    }

                    // 赤線と緑線が同じ time に複数ある可能性があるため、
                    // その time の中で緑線があれば優先的に使う
                    var timeGroup = timingPointList
                                    .Where(tp => tp.time == timingPointList[timingIndex].time)
                                    .ToList();

                    var green = timeGroup.FirstOrDefault(tp => !tp.isRedLine);
                    var red = timeGroup.FirstOrDefault(tp => tp.isRedLine);

                    if (red != null)
                    {
                        currentBpm = red.bpm;
                    }
                    if (green != null)
                    {
                        currentSv = green.sv;
                        svApplyTime = green.time;
                    }
                    else if (red != null)
                    {
                        // 赤線のみある場合、SVは1.0
                        currentSv = 1.0;
                        svApplyTime = red.time;
                    }

                    hitObject.bpm = currentBpm;
                    hitObject.sv = currentSv;
                    hitObject.svApplyTime = svApplyTime;
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
        /// バックアップフォルダを作成する
        /// </summary>
        /// <param name="path">osuファイルが格納されているフォルダ</param>
        /// <param name="backupFile">バックアップの出力先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool CreateBackup(string path, string backupFile)
        {
            try
            {
                string escapedBackupFile = Regex.Replace(backupFile, Constants.ESCAPE_CHARACTER, " ");
                string backupPath = Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY + "\\" + escapedBackupFile;
                DateTime now = DateTime.Now;
                string backupFileName = $"{now:yyyy_MM_dd_HH_mm_ss_fff}" + Constants.OSU_EXTENSION;
                // バックアップフォルダがない場合は作成する
                if (!Directory.Exists(backupPath))
                {
                    Directory.CreateDirectory(backupPath);
                }
                // コピーの作成
                File.Copy(path, Path.Combine(backupPath, backupFileName), true);
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteWarningMessage("LOG_E-CREATE-BACKUP");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// osuファイルを作成する
        /// </summary>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="beatmapPath">ファイル名</param>
        /// <param name="userInputUtilityData">ユーティリティタブの入力データ</param>
        /// <param name="beatmapInfo">選択している譜面情報</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool ExportToOsuFile(Beatmap beatmap,
                                             string beatmapPath,
                                             UserInputUtilityData? userInputUtilityData = null,
                                             BeatmapMetadata? beatmapInfo = null,
                                             List<TimingPoint>? timingPoints = null)
        {
            StreamWriter? file = null;
            try
            {
                file = new(beatmapPath, false, Encoding.GetEncoding("utf-8"));
                // 設定されている譜面データをすべて出力する
                file.WriteLine(beatmap.version);
                file.WriteLine("");
                file.WriteLine(Constants.GENERAL);
                foreach (var line in beatmap.general)
                {
                    if (line.Contains(Constants.PREVIEW) && userInputUtilityData != null)
                    {
                        if (userInputUtilityData?.utilityCode == Constants.UTILITY_SETTING_COPIER &&
                            userInputUtilityData?.settingCopierCode == Constants.SETTING_COPIER_PREVIEW)
                        {
                            file.WriteLine(Constants.PREVIEW + beatmapInfo?.previewTime);
                        }
                        else
                        {
                            file.WriteLine(line);
                        }
                    }
                    else
                    {
                        file.WriteLine(line);
                    }
                }
                file.WriteLine("");
                file.WriteLine(Constants.EDITOR);
                beatmap.editor.ForEach(line => file.WriteLine(line));
                file.WriteLine("");
                file.WriteLine(Constants.METADATA);
                for (int i = 0; i < beatmap.metadata.Count; i++)
                {
                    // タグ編集ユーティリティが選択されている場合は入力されているタグを上書きする
                    if (beatmap.metadata[i].Contains(Constants.TAGS) && userInputUtilityData?.utilityCode == Constants.UTILITY_TAG_EDIT)
                    {
                        file.WriteLine(Constants.TAGS + userInputUtilityData?.tags);
                    }
                    // メタデータ設定ユーティリティが選択されている場合は選択している譜面のメタデータをコピーする
                    else if (userInputUtilityData?.utilityCode == Constants.UTILITY_SETTING_COPIER &&
                             userInputUtilityData?.settingCopierCode == Constants.SETTING_COPIER_METADATA)
                    {
                        if (beatmap.metadata[i].Contains(Constants.TITLE))
                        {
                            file.WriteLine(Constants.TITLE + beatmapInfo?.title);
                        }
                        else if (beatmap.metadata[i].Contains(Constants.TITLE_UNICODE))
                        {
                            file.WriteLine(Constants.TITLE_UNICODE + beatmapInfo?.titleUnicode);
                        }
                        else if (beatmap.metadata[i].Contains(Constants.ARTIST))
                        {
                            file.WriteLine(Constants.ARTIST + beatmapInfo?.artist);
                        }
                        else if (beatmap.metadata[i].Contains(Constants.ARTIST_UNICODE))
                        {
                            file.WriteLine(Constants.ARTIST_UNICODE + beatmapInfo?.artistUnicode);
                        }
                        else if (beatmap.metadata[i].Contains(Constants.CREATOR))
                        {
                            file.WriteLine(Constants.CREATOR + beatmapInfo?.creator);
                        }
                        else if (beatmap.metadata[i].Contains(Constants.SOURCE))
                        {
                            file.WriteLine(Constants.SOURCE + beatmapInfo?.source);
                        }
                        else
                        {
                            file.WriteLine(beatmap.metadata[i]);
                        }
                    }
                    // それら以外の場合はそのまま出力する
                    else
                    {
                        file.WriteLine(beatmap.metadata[i]);
                    }
                }
                file.WriteLine("");
                file.WriteLine(Constants.DIFFICULTY);
                beatmap.difficulty.ForEach(line => file.WriteLine(line));
                file.WriteLine("");
                file.WriteLine(Constants.EVENTS);
                for (global::System.Int32 i = 0; i < beatmap.events.Count; i++)
                {
                    file.WriteLine(beatmap.events[i]);
                    // メタデータ設定ユーティリティが選択されている場合は選択している譜面のBGをコピーする
                    if (userInputUtilityData?.utilityCode == Constants.UTILITY_SETTING_COPIER &&
                        userInputUtilityData?.settingCopierCode == Constants.SETTING_COPIER_BG &&
                        beatmap.events[i].Contains(Constants.BG_AND_VIDEO) &&
                        beatmapInfo?.background != string.Empty)
                    {
                        file.WriteLine(beatmapInfo?.background);
                        if (beatmap.events[i + 1].Contains(Constants.JPG_EXTENSION) ||
                            beatmap.events[i + 1].Contains(Constants.JPEG_EXTENSION) ||
                            beatmap.events[i + 1].Contains(Constants.PNG_EXTENSION) ||
                            beatmap.events[i + 1].Contains(Constants.WEBP_EXTENSION))
                        {
                            i++;
                        }
                    }
                }
                file.WriteLine("");
                file.WriteLine(Constants.TIMING_POINTS);
                var inheritedPoints = beatmap.timingPoints.Where(tp => !tp.isRedLine).ToList();
                List<TimingPoint> tempTimingPoints = new List<TimingPoint>();
                if (userInputUtilityData?.utilityCode == Constants.UTILITY_SETTING_COPIER &&
                    userInputUtilityData?.settingCopierCode == Constants.SETTING_COPIER_TIMING_POINTS &&
                    timingPoints != null)
                {
                    tempTimingPoints.AddRange(timingPoints);
                } else
                {
                    tempTimingPoints = beatmap.timingPoints.Where(tp => tp.isRedLine).ToList();
                }
                if (tempTimingPoints == null)
                {
                    return false;
                }
                tempTimingPoints.AddRange(inheritedPoints);
                // ソートする
                tempTimingPoints = [.. tempTimingPoints.OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0)];
                foreach (var timingPoint in tempTimingPoints)
                {
                    // beatLengthは桁数を指定して求める
                    string beatLength = (timingPoint.isRedLine ?
                                        Math.Round(60000 / timingPoint.bpm, 12, MidpointRounding.AwayFromZero).ToString() :
                                        Math.Round(-100 / timingPoint.sv, 12, MidpointRounding.AwayFromZero).ToString());
                    if (beatLength.Contains('.'))
                    {
                        // beatLengthが整数だった場合は"0"と"."を削除する
                        beatLength = beatLength.TrimEnd('0');
                        beatLength = beatLength.TrimEnd('.');
                    }
                    int time = userInputUtilityData?.utilityCode == Constants.UTILITY_OFFSET ?
                               (timingPoint.time + userInputUtilityData.offset) : timingPoint.time;
                    string timingPointLine = time + "," +
                                             beatLength + "," +
                                             timingPoint.meter + "," +
                                             timingPoint.sampleSet + "," +
                                             timingPoint.sampleIndex + "," +
                                             timingPoint.volume + "," +
                                             (timingPoint.isRedLine ? "1" : "0") + "," +
                                             timingPoint.effect;
                    file.WriteLine(timingPointLine);
                }
                file.WriteLine("");
                if (beatmap.colours.Count != 0)
                {
                    file.WriteLine(Constants.COLOURS);
                    beatmap.colours.ForEach(line => file.WriteLine(line));
                }
                file.WriteLine("");
                file.WriteLine(Constants.HIT_OBJECTS);
                for (global::System.Int32 i = 0; i < beatmap.hitObjects.Count; i++)
                {
                    int endTime = int.MinValue;
                    if (beatmap.hitObjects[i].noteType == Constants.NoteType.SPINNER)
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
                    if (beatmap.hitObjects[i].noteType != Constants.NoteType.BARLINE &&
                        beatmap.hitObjects[i].noteType != Constants.NoteType.BOOKMARK &&
                        beatmap.hitObjects[i].noteType != Constants.NoteType.SPINNER_END)
                    {
                        string hitObjectLine = CreateHitObjectLine(beatmap.hitObjects[i],
                                                                   userInputUtilityData,
                                                                   endTime);
                        file.WriteLine(hitObjectLine);
                    }
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
                if (file != null)
                {
                    file.Close();
                }
            }
            return true;
        }
        /// <summary>
        /// 前に実行したファイルをコピーする処理
        /// </summary>
        /// <param name="path">osuファイルが格納されているフォルダ</param>
        /// <param name="backupDirectory">バックアップディレクトリ</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool ExportToPreviousOsuFile(string path, string backupDirectory)
        {
            try
            {
                string backupPath = Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY + "\\" + backupDirectory;
                // バックアップディレクトリが見つからない場合はfalseで返す
                if (!Directory.Exists(backupPath))
                {
                    return false;
                }
                // バックアップファイルを探す
                string[] files = Directory.GetFiles(backupPath, "*" + Constants.OSU_EXTENSION);
                // バックアップファイルが見つからない場合はfalseで返す
                if (files.Length == 0)
                {
                    return false;
                }
                List<long> fileDate = [];
                // ファイル名の_を消し、数値にする
                foreach (var file in files)
                {
                    string deletedPath = file.Replace(backupPath + "\\", "");
                    string deletedExtension = deletedPath.Replace(Constants.OSU_EXTENSION, "");
                    string deletedUnderScore = deletedExtension.Replace("_", "");
                    fileDate.Add(Convert.ToInt64(deletedUnderScore));
                }
                // 数値を降順にソートする
                fileDate.Sort();
                fileDate.Reverse();
                // 最後の実行で作成されたバックアップファイルを探す
                long currentDate = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddHHmmssfff"));
                var targetFile = fileDate.FirstOrDefault(f => f <= currentDate);
                if (targetFile == 0)
                {
                    return false;
                }
                string targetFileString = targetFile.ToString("0000_00_00_00_00_00_000");
                targetFileString = Path.Combine(backupPath, targetFileString + Constants.OSU_EXTENSION);
                // Songsフォルダにコピーの作成
                File.Copy(targetFileString, path, true);
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXPORT-OSU");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// 指定したバックアップファイルをコピーする処理
        /// </summary>
        /// <param name="beatmapPath">osuファイルが格納されているフォルダ</param>
        /// <param name="backupFile">バックアップファイル</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool ExportToOsuFileFromBackup(string beatmapPath, string backupFile)
        {
            try
            {
                // Songsフォルダにコピーの作成
                File.Copy(backupFile, beatmapPath, true);
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXPORT-OSU");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// osuファイルのヒットオブジェクトの行を作成する
        /// </summary>
        /// <param name="hitObject">ヒットオブジェクトデータ</param>
        /// <param name="userInputUtilityData">ユーティリティタブの入力データ</param>
        /// <returns>ヒットオブジェクトの行</returns>
        private static string CreateHitObjectLine(HitObject hitObject, UserInputUtilityData? userInputUtilityData, int endTime)
        {
            int positionX = hitObject.positionX;
            int positionY = hitObject.positionY;
            int time = userInputUtilityData?.utilityCode == Constants.UTILITY_OFFSET ?
                       (hitObject.time + userInputUtilityData.offset) : hitObject.time;
            string hitSound = hitObject.hitSound;
            if (hitObject.noteType != Constants.NoteType.SPINNER)
            {
                switch (userInputUtilityData?.utilityCode)
                {
                    case Constants.UTILITY_HITSOUND:
                        // ヒットサウンド編集ユーティリティが選択されている場合は入力されているヒットサウンドを上書きする
                        switch (userInputUtilityData?.hitSoundCode)
                        {
                            case Constants.HITSOUND_CLAP:
                                if ((hitObject.hitObjectCode & 0b00001100) == 0)
                                {
                                    break;
                                }
                                if ((hitObject.hitObjectCode & 0b00001000) != 0)
                                {
                                    hitSound = "12";
                                }
                                else
                                {
                                    hitSound = "8";
                                }
                                break;
                            case Constants.HITSOUND_WHISTLE:
                                if ((hitObject.hitObjectCode & 0b00001100) == 0)
                                {
                                    break;
                                }
                                if ((hitObject.hitObjectCode & 0b00001000) != 0)
                                {
                                    hitSound = "6";
                                }
                                else
                                {
                                    hitSound = "2";
                                }
                                break;
                        }
                        break;
                    // ノーツ位置編集ユーティリティが選択されている場合は入力されているノーツ位置を上書きする
                    case Constants.UTILITY_NOTES_POSITION:
                        switch (userInputUtilityData?.notesPositionCode)
                        {
                            case Constants.NOTES_POSITION_CENTER:
                                positionX = Constants.CENTER_NOTES.X;
                                positionY = Constants.CENTER_NOTES.Y;
                                break;
                            case Constants.NOTES_POSITION_SEPARATE:
                                if (hitObject.noteType == Constants.NoteType.CIRCLE)
                                {
                                    positionX = SetNotesPosition(hitObject, userInputUtilityData, 0);
                                    positionY = SetNotesPosition(hitObject, userInputUtilityData, 1);
                                }
                                else
                                {
                                    positionX = Constants.CENTER_NOTES.X;
                                    positionY = Constants.CENTER_NOTES.Y;
                                }
                                break;
                            case Constants.NOTES_POSITION_RANDOM:
                                Random random = new();
                                int randomX = random.Next(0, 512);
                                int randomY = random.Next(0, 384);
                                positionX = randomX;
                                positionY = randomY;
                                break;
                        }
                        break;
                    // それ以外の場合はosuファイルの位置をそのまま出力する
                    default:
                        positionX = hitObject.positionX;
                        positionY = hitObject.positionY;
                        break;
                }
            }
            else
            {
            }
            StringBuilder sb = new();
            sb.Append(positionX + ",");
            sb.Append(positionY + ",");
            sb.Append(time + ",");
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
                sb.Append(hitObject.curveSetting + ",");
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
        /// 
        /// </summary>
        /// <param name="hitObject">ヒットオブジェクト情報</param>
        /// <param name="userInputUtilityData">ユーティリティタブの入力データ</param>
        /// <param name="dimension">次元</param>
        /// <returns>座標の値</returns>
        private static int SetNotesPosition(HitObject hitObject, UserInputUtilityData userInputUtilityData, int dimension)
        {
            bool isKat = (hitObject.hitObjectCode & 0b00001100) != 0;
            bool isFinisher = (hitObject.hitObjectCode & 0b00001010) != 0;
            if (dimension == 0)
            {
                if (!isKat)
                {
                    if (!isFinisher)
                    {
                        // d
                        return userInputUtilityData.donX;
                    }
                    else
                    {
                        // D
                        return userInputUtilityData.finisherDonX;
                    }
                }
                else
                {
                    if (!isFinisher)
                    {
                        // k
                        return userInputUtilityData.katX;
                    }
                    else
                    {
                        // K
                        return userInputUtilityData.finisherKatX;
                    }
                }
            }
            else
            {
                if (!isKat)
                {
                    if (!isFinisher)
                    {
                        // d
                        return userInputUtilityData.donY;
                    }
                    else
                    {
                        // D
                        return userInputUtilityData.finisherDonY;
                    }
                }
                else
                {
                    if (!isFinisher)
                    {
                        // k
                        return userInputUtilityData.katY;
                    }
                    else
                    {
                        // K
                        return userInputUtilityData.finisherKatY;
                    }
                }
            }
        }
        /// <summary>
        /// 選択されている譜面のBGの情報を取得する
        /// </summary>
        /// <param name="beatmapInfo">選択している譜面情報</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool GetBackground(ref BeatmapMetadata beatmapInfo)
        {
            try
            {
                var lines = File.ReadAllLines(beatmapInfo.beatmapPath);
                for (global::System.Int32 i = 0; i < lines.Length; i++)
                {
                    if (lines[i] == Constants.BG_AND_VIDEO &&
                        (lines[i + 1].Contains(Constants.JPG_EXTENSION) ||
                         lines[i + 1].Contains(Constants.JPEG_EXTENSION) ||
                         lines[i + 1].Contains(Constants.PNG_EXTENSION) ||
                         lines[i + 1].Contains(Constants.WEBP_EXTENSION)))
                    {
                        beatmapInfo.background = lines[i + 1];
                        i++;
                        return true;
                    }
                }
                beatmapInfo.background = string.Empty;
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
        /// osu側で丸められたTimingの正確な値を算出する
        /// </summary>
        /// <param name="timingPoints">譜面のTimingPoint</param>
        /// <param name="timing">指定されたタイミング</param>
        /// <returns>
        /// 指定されたタイミングがosuの動作対象内の場合 : 算出された正確なタイミング<br/>
        /// 指定されたタイミングがosuの動作対象外の場合 : 引数で指定されたタイミング<br/>
        /// 処理が異常終了した場合 : double型の最小値</returns>
        internal static double GetRawTiming(List<TimingPoint> timingPoints, int timing)
        {
            // snapPerMs[0] 1/16のノーツ間隔(ms) -> 1/1,1/2,1/4,1/8,1/16 に対応可
            // snapPerMs[1] 1/12のノーツ間隔(ms) ->         1/3,1/6,1/12 に対応可
            // snapPerMs[2] 1/9のノーツ間隔(ms)  ->                  1/9 に対応可
            // snapPerMs[3] 1/7のノーツ間隔(ms)  ->                  1/7 に対応可
            // snapPerMs[4] 1/5のノーツ間隔(ms)  ->                  1/5 に対応可
            double[] snapPerMs = new double[5];
            try
            {
                // 手前のTimingPointを算出する
                var applyTimingPoint = timingPoints.LastOrDefault(tp => (tp.time <= timing) && tp.isRedLine) ?? throw new Exception();
                // それぞれのノーツ間隔を算出する
                snapPerMs[0] = 60 / applyTimingPoint.bpm / 16;
                snapPerMs[1] = 60 / applyTimingPoint.bpm / 12;
                snapPerMs[2] = 60 / applyTimingPoint.bpm / 9;
                snapPerMs[3] = 60 / applyTimingPoint.bpm / 7;
                snapPerMs[4] = 60 / applyTimingPoint.bpm / 5;

                // 細かいスナップ間隔から指定されたタイミングの正確なタイミングを算出する
                foreach (var snap in snapPerMs)
                {
                    double currentTime = applyTimingPoint.time;
                    while (true)
                    {
                        // もし現時点を丸めた値が指定されたタイミングと一致した場合
                        // 正確かは怪しい
                        if (Math.Floor(currentTime) == timing)
                        {
                            return currentTime;
                        }
                        // もし現時点が指定されたタイミングより大きい値になった場合は処理を抜ける
                        if (currentTime > timing)
                        {
                            break;
                        }
                        currentTime += snap;
                    }
                }
                // 指定されたTimingの位置が動作対象外
                return (double)timing;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return double.MinValue;
            }
        }
    }
}

