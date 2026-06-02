using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Services
{
    internal class SVCalculatorService
    {
        private static UserInputData userInputData = new();
        private static Beatmap beatmap = new();
        /// <summary>
        /// クラス変数設定処理
        /// </summary>
        /// <param name="userInputData">ユーザー入力データ</param>
        /// <param name="beatmap">譜面情報</param>
        private static void SetClasses(UserInputData userInputData, Beatmap beatmap)
        {
            SVCalculatorService.userInputData = userInputData;
            SVCalculatorService.beatmap = beatmap;
        }
        /// <summary>
        /// クラス変数クリア処理
        /// </summary>
        private static void ClearClasses()
        {
            SVCalculatorService.userInputData = new();
            SVCalculatorService.beatmap = new();
        }
        #region 適用処理関数
        /// <summary>
        /// 適用処理
        /// </summary>
        /// <param name="userInputData">ユーザー入力データ</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns></returns>
        internal static bool Apply(UserInputData userInputData, Beatmap beatmap, ref List<TimingPoint> outTimingPoints)
        {
            List<TimingPoint> timingPointsBuff = [];
            try
            {
                SetClasses(userInputData, beatmap);
                if ((userInputData.applySetCode & 0x00000001) != 0)
                {
                    // objectに緑線を置く場合
                    if (!ApplyOnObjects(ref timingPointsBuff)) throw new Exception("Failed to apply on objects.");
                }
                else if ((userInputData.applySetCode & 0x00000002) != 0)
                {
                    // beatSnap間隔で緑線を置く場合
                    if (!ApplyOnBeatSnaps(ref timingPointsBuff)) throw new Exception("Failed to apply on beat snaps.");
                }
                else if ((userInputData.applySetCode & 0x00000004) != 0)
                {
                    if (!ApplyGreenLines()) throw new Exception("Failed to change green lines.");
                }
                else if ((userInputData.applySetCode & 0x00000008) != 0)
                {
                    if (!ApplyOnRedLines(ref timingPointsBuff)) throw new Exception("Failed to apply on red lines.");
                }
                if (userInputData.isVolume && userInputData.shouldApplyRedlines)
                {
                    if (!ApplyVolumeOnTimingPoints(ref timingPointsBuff)) throw new Exception("Failed to apply volume on timing points.");
                }

                // ユーザーが指定した範囲外のスライダーの長さの調整
                if (!AdjustSliderLengthAfterExecute(timingPointsBuff, Constants.EXECUTE_APPLY)) throw new Exception("Failed to adjust slider length.");

                if (!DeleteTimingPoints()) throw new Exception("Failed to delete timing points.");
                // 処理で取得した緑線を情報を格納する
                outTimingPoints.AddRange(timingPointsBuff);
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteWarningMessage("LOG_E-ADD-LINES");
                Common.WriteExceptionMessage(ex);
                return false;
            }
            finally
            {
                ClearClasses();
            }
        }
        /// <summary>
        /// 適用処理 (objectsのみ)
        /// </summary>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyOnObjects(ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                // オフセットモードが1/16 (1/12) offsetかつ、オフセットが有効な場合は、
                // HitObjectのタイミングにオフセットを適用した位置に緑線を置くための情報を取得する
                if (userInputData.offsetMode == 0 && userInputData.isOffset)
                {
                    if (!BeatmapHelper.GetSnapsOnHitObjects(ref beatmap))
                    {
                        throw new Exception("Failed to get snaps.");
                    }
                }
                if (!AddGreenLinesOnObjects(ref outTimingPoints))
                {
                    throw new Exception("Failed to add green lines on objects.");
                }
                // TimingPointsに緑線適用処理
                if (!AddGreenLinesOnTimingPoints(ref outTimingPoints))
                {
                    throw new Exception("Failed to add green lines on timing points.");
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
        /// 適用処理 (beatsnap間隔)
        /// </summary>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyOnBeatSnaps(ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                if (!AddGreenLinesOnBeatSnaps(ref outTimingPoints))
                {
                    throw new Exception("Failed to add green lines on beatsnaps.");
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
        /// 適用処理 (緑線)
        /// </summary>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyGreenLines()
        {
            try
            {
                if (!ChangeGreenLines())
                {
                    throw new Exception("Failed to change green lines");
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
        /// 適用処理 (赤線)
        /// </summary>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyOnRedLines(ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                if (!AddGreenLinesOnRedLines(ref outTimingPoints))
                {
                    throw new Exception("Failed to change green lines");
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
        /// 緑線追加処理 (objects)
        /// </summary>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool AddGreenLinesOnObjects(ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                double baseBpm = beatmap.timingPoints.LastOrDefault(tp => tp.time < userInputData.timingFrom && tp.isRedLine)?.bpm ?? 120;
                double svPerMs = userInputData.isSv ? GetSvPerMs() : 0;
                double volumePerMs = userInputData.isVolume ? GetVolumePerMs() : 0;
                bool isIgnoreObject = false;
                bool isFirstNotes = false;

                for (global::System.Int32 i = 0; i < beatmap.hitObjects.Count; i++)
                {
                    int loopCode = GetLoopCode(i);
                    if (loopCode == Constants.STATE_CONTINUE) continue;
                    if (loopCode == Constants.STATE_BREAK) break;
                    // 直前の緑線のIndexを探す
                    var greenLineIndex = beatmap.timingPoints.FindLastIndex(tp => tp.time <= beatmap.hitObjects[i].svApplyTime);
                    // 直前の赤線のIndexを探す
                    var redLineIndex = beatmap.timingPoints.FindLastIndex(tp => (tp.time <= beatmap.hitObjects[i].svApplyTime) && tp.isRedLine);
                    //////
                    double offset = 0;
                    int time = i == 0 ? beatmap.hitObjects[i].time : GetOffsetTiming(i, greenLineIndex, redLineIndex, out offset);
                    int effect = GetEffect(i, greenLineIndex, offset);
                    var greenLineIndexes = beatmap.timingPoints.Select((tp, index) => new { tp, index }).
                                                                Where(x => (x.tp.time <= beatmap.hitObjects[i].svApplyTime && x.tp.time > (beatmap.hitObjects.SafeGetIndex(i - 1)?.time ?? int.MinValue)) && !x.tp.isRedLine).
                                                                Select(x => x.index).
                                                                ToList();
                    // 削除対象となる緑線のdeleteFlagを有効にする
                    SetDeleteFlag(greenLineIndexes);
                    if ((beatmap.hitObjects[i].hitObjectCode & userInputData.setObjectOption.setObjectsCode) != 0)
                    {
                        double bpm = time >= beatmap.timingPoints[redLineIndex].time ?
                            beatmap.timingPoints[redLineIndex].bpm :
                            (beatmap.timingPoints.LastOrDefault(tp => (tp.time <= time) && tp.isRedLine)?.bpm ?? 120);
                        double sv = CalculateSv(i, greenLineIndex, svPerMs) *
                            (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / bpm) : 1);
                        int volume = CalculateVolume(i, greenLineIndex, volumePerMs);
                        if (beatmap.hitObjects[i].noteType == Constants.NoteType.SLIDER)
                        {
                            beatmap.hitObjects[i].sliderLength = GetAdjustedSliderLength(i, sv);
                        }
                        // SVの最適化 必要かな？
                        //if (outTimingPoints.Count != 0 &&
                        //    outTimingPoints[^1].sv == sv &&
                        //    outTimingPoints[^1].volume == volume &&
                        //    outTimingPoints[^1].effect == effect) continue;
                        outTimingPoints.Add(new TimingPoint
                        {
                            time = time,
                            bpm = 0,
                            sv = sv,
                            barLength = 0,
                            meter = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.meter ?? 4,
                            sampleSet = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sampleSet ?? 1,
                            sampleIndex = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sampleIndex ?? 0,
                            volume = volume,
                            isRedLine = false,
                            effect = effect
                        });
                        if (CheckKiai(greenLineIndex))
                        {
                            beatmap.timingPoints[greenLineIndex].sv = CalculateSv(i, greenLineIndex, svPerMs) *
                            (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / beatmap.timingPoints[redLineIndex].bpm) : 1);
                            beatmap.timingPoints[greenLineIndex].volume = volume;
                        }
                        isIgnoreObject = false;
                        isFirstNotes = true;
                    }
                    else
                    {
                        // 前回のノーツがSV,Volumeの計算の対象外だった場合、または全体の1ノーツ目だった場合は最低でも赤線が適用されているため、何も処理をしない
                        if (isIgnoreObject || i == 0) continue;
                        // 直前のTimingPointを探す
                        if (greenLineIndex < 0) throw new Exception();
                        // miyag 20260601 条件の見直し
                        if (!isFirstNotes && (beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.time <= userInputData.timingFrom) && !beatmap.timingPoints[greenLineIndex].isRedLine)
                        {
                            isIgnoreObject = true;
                            continue;
                        }
                        if (beatmap.hitObjects[i].noteType == Constants.NoteType.SPINNER_END)
                        {
                            continue;
                        }
                        if (beatmap.hitObjects[i].noteType != Constants.NoteType.BARLINE || offset != 0)
                        {
                            isIgnoreObject = true;
                        }
                        outTimingPoints.Add(new TimingPoint
                        {
                            time = time,
                            bpm = 0,
                            sv = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sv ?? 1,
                            barLength = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.barLength ?? 2000,
                            meter = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.meter ?? 4,
                            sampleSet = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sampleSet ?? 1,
                            sampleIndex = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sampleIndex ?? 0,
                            volume = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.volume ?? 100,
                            isRedLine = false,
                            effect = effect
                        });
                        CheckKiai(greenLineIndex);
                    }
                    foreach (var index in greenLineIndexes)
                    {
                        CheckKiai(index);
                    }
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 緑線追加処理 (timingPoints)
        /// </summary>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool AddGreenLinesOnTimingPoints(ref List<TimingPoint> outTimingPoints)
        {
            double baseBpm = beatmap.timingPoints.LastOrDefault(tp => tp.time < userInputData.timingFrom && tp.isRedLine)?.bpm ?? 120;
            double svPerMs = userInputData.isSv ? GetSvPerMs() : 0;
            double volumePerMs = userInputData.isVolume ? GetVolumePerMs() : 0;
            List<TimingPoint> timingPointsBuff = [];
            List<int> removeList = [];
            try
            {
                for (int i = 0; i < beatmap.hitObjects.Count; i++)
                {
                    int loopCode = GetLoopCode(i);
                    if (loopCode == Constants.STATE_CONTINUE) continue;
                    if (loopCode == Constants.STATE_BREAK) break;

                    // 直前の赤線のIndexを探す
                    var redLineIndex = beatmap.timingPoints.FindLastIndex(tp => (tp.time == beatmap.hitObjects[i].time) && tp.isRedLine);
                    // 直前のTimingPointのインデックスを算出する
                    var greenLineIndex = beatmap.timingPoints.FindLastIndex(tp => tp.time == beatmap.hitObjects[i].time);
                    if (redLineIndex == -1)
                    {
                        continue;
                    }
                    int time = GetOffsetTiming(i, greenLineIndex, redLineIndex, out double offset);
                    int effect = GetEffect(i, greenLineIndex, offset);
                    // 緑線設定フラグが有効の場合
                    if (IsSetInheritedPoint(i, offset, outTimingPoints))
                    {
                        // オブジェクトコードを比較し、一致するものがある場合にSV,Volumeの計算を行う
                        if ((beatmap.hitObjects[i].hitObjectCode & userInputData.setObjectOption.setObjectsCode) != 0)
                        {
                            double sv = CalculateSv(i, greenLineIndex, svPerMs) *
                                (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / beatmap.timingPoints[redLineIndex].bpm) : 1);
                            int volume = CalculateVolume(i, greenLineIndex, volumePerMs);
                            outTimingPoints.Add(new TimingPoint(beatmap.hitObjects[i].time,
                                                                0,
                                                                sv,
                                                                0,
                                                                beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.meter ?? 4,
                                                                beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sampleSet ?? 1,
                                                                beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sampleIndex ?? 0,
                                                                volume,
                                                                false,
                                                                beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.effect ?? 0));
                        }
                        SetDeleteFlag(beatmap.timingPoints.Select((tp, index) => new { tp, index }).
                                                           Where(x => (x.tp.time == beatmap.hitObjects[i].time && !x.tp.isRedLine)).
                                                           Select(x => x.index).
                                                           ToList());
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
        /// 緑線追加処理 (beatsnap間隔)
        /// </summary>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool AddGreenLinesOnBeatSnaps(ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                double svPerMs = userInputData.isSv ? GetSvPerMs() : 0;
                double volumePerMs = userInputData.isVolume ? GetVolumePerMs() : 0;
                var redLineIndexes = beatmap.timingPoints.Select((tp, index) => new { tp, index })
                                                         .Where(x => x.tp.isRedLine)
                                                         .Select(x => x.index)
                                                         .ToList();
                double rawTimingFrom = BeatmapHelper.GetRawTiming(beatmap.timingPoints, userInputData.timingFrom);
                if (rawTimingFrom == double.MinValue) throw new Exception();
                // 直前の赤線のIndexを探す
                var redLineIndex = beatmap.timingPoints.FindLastIndex(tp => (tp.time <= rawTimingFrom) && tp.isRedLine);
                double currentTiming = beatmap.timingPoints.SafeGetIndex(redLineIndex)?.time ?? beatmap.timingPoints[0].time;
                double baseBpm = beatmap.timingPoints.SafeGetIndex(redLineIndex)?.bpm ?? 120;
                double beatSnapTiming = Constants.ONE_MINUTE / baseBpm / userInputData.setBeatSnapOption.beatSnap;
                // timingが指定されたビートスナップ間隔からどれぐらいズレてるか算出する
                double timingOffset = GetBeatSnapOffset(currentTiming, beatSnapTiming, rawTimingFrom);
                currentTiming += timingOffset;
                for (int i = 0; i < redLineIndexes.Count; i++)
                {
                    int loopCode = GetLoopCode(redLineIndexes[i], true);
                    if (loopCode == Constants.STATE_CONTINUE) continue;
                    if (loopCode == Constants.STATE_BREAK) break;
                    for (int j = 0; ; j++)
                    {
                        loopCode = GetLoopCode(redLineIndexes[i], j, ref beatSnapTiming, ref currentTiming, redLineIndexes.SafeGetIndex(i + 1));
                        if (loopCode == Constants.STATE_CONTINUE) continue;
                        if (loopCode == Constants.STATE_BREAK) break;
                        // 直前の緑線のIndexを探す
                        var greenLineIndex = beatmap.timingPoints.FindLastIndex(tp => tp.time <= currentTiming);
                        double sv = CalculateSv(j, greenLineIndex, svPerMs) *
                            (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / beatmap.timingPoints[redLineIndexes[i]].bpm) : 1);
                        int volume = CalculateVolume(currentTiming, volumePerMs);
                        outTimingPoints.Add(new TimingPoint
                        {
                            time = (int)currentTiming,
                            bpm = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.bpm ?? 120,
                            sv = sv,
                            barLength = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.barLength ?? 2000,
                            meter = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.meter ?? 4,
                            sampleSet = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sampleSet ?? 1,
                            sampleIndex = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sampleIndex ?? 0,
                            volume = volume,
                            isRedLine = false,
                            effect = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.effect ?? 0
                        });
                        currentTiming = beatmap.timingPoints[redLineIndexes[i]].time + beatSnapTiming * (j + 1);
                    }
                }
                var greenLineIndexes = beatmap.timingPoints.Select((tp, index) => new { tp, index }).
                                                                   Where(x => (x.tp.time >= userInputData.timingFrom) && (x.tp.time <= userInputData.timingTo) && !x.tp.isRedLine).
                                                                   Select(x => x.index).
                                                                   ToList();
                SetDeleteFlag(greenLineIndexes);
                foreach (var index in greenLineIndexes)
                {
                    CheckKiai(index);
                }
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 緑線変更処理 (緑線)
        /// </summary>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ChangeGreenLines()
        {
            try
            {
                double baseBpm = beatmap.timingPoints.LastOrDefault(tp => tp.time < userInputData.timingFrom && tp.isRedLine)?.bpm ?? 120;
                double svPerMs = userInputData.isSv ? GetSvPerMs() : 0;
                double volumePerMs = userInputData.isVolume ? GetVolumePerMs() : 0;
                for (int i = 0; i < beatmap.timingPoints.Count; i++)
                {
                    if (beatmap.timingPoints[i].isRedLine) continue;
                    int loopCode = GetLoopCode(i, true);
                    if (loopCode == Constants.STATE_CONTINUE) continue;
                    if (loopCode == Constants.STATE_BREAK) break;
                    var redLineIndex = beatmap.timingPoints.FindLastIndex(tp => (tp.time <= beatmap.timingPoints[i].time) && tp.isRedLine);
                    beatmap.timingPoints[i].sv = CalculateSv(i, svPerMs) *
                        (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / beatmap.timingPoints[redLineIndex].bpm) : 1);
                    beatmap.timingPoints[i].volume = CalculateVolume(i, volumePerMs);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 緑線追加処理 (赤線)
        /// </summary>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool AddGreenLinesOnRedLines(ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                double svPerMs = userInputData.isSv ? GetSvPerMs() : 0;
                double volumePerMs = userInputData.isVolume ? GetVolumePerMs() : 0;
                var redLineList = beatmap.timingPoints.FindAll(tp => tp.isRedLine);
                List<int> greenLineIndexes = [];
                for (int i = 0; i < redLineList.Count; i++)
                {
                    int loopCode = GetLoopCode(i, true);
                    if (loopCode == Constants.STATE_CONTINUE) continue;
                    if (loopCode == Constants.STATE_BREAK) break;
                    // 直前の緑線のIndexを探す
                    var greenLineIndex = beatmap.timingPoints.FindLastIndex(tp => tp.time == redLineList[i].time);
                    double sv = userInputData.bpm / redLineList[i].bpm;
                    int volume = CalculateVolume(i, volumePerMs);

                    var afterGreenLineIndex = beatmap.timingPoints.FindIndex(tp => tp.time > redLineList[i].time);
                    var hitObjectIndexes = beatmap.hitObjects.Select((ho, index) => new { ho, index }).
                                                              Where(x => (x.ho.time < (beatmap.timingPoints.SafeGetIndex(afterGreenLineIndex)?.time ?? int.MaxValue)) &&
                                                                    x.ho.time >= redLineList[i].time).
                                                              Select(x => x.index).
                                                              ToList();
                    foreach (var index in hitObjectIndexes)
                    {
                        if (beatmap.hitObjects[index].noteType != Constants.NoteType.SLIDER) continue;
                        beatmap.hitObjects[index].sliderLength = GetAdjustedSliderLength(index, sv);
                    }
                    outTimingPoints.Add(new TimingPoint
                    {
                        time = redLineList[i].time,
                        bpm = 0,
                        sv = sv,
                        barLength = redLineList[i].barLength,
                        meter = redLineList[i].meter,
                        sampleSet = redLineList[i].sampleSet,
                        sampleIndex = redLineList[i].sampleIndex,
                        volume = volume,
                        isRedLine = false,
                        effect = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.effect ?? redLineList[i].effect
                    });
                    greenLineIndexes.AddRange(beatmap.timingPoints.Select((tp, index) => new { tp, index }).
                                                                   Where(x => (x.tp.time == redLineList[i].time) && !x.tp.isRedLine).
                                                                   Select(x => x.index).
                                                                   ToList());
                }
                SetDeleteFlag(greenLineIndexes);
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 赤線音量適用処理
        /// </summary>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyVolumeOnTimingPoints(ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                for (int i = 0; i < beatmap.timingPoints.Count; i++)
                {
                    if (!beatmap.timingPoints[i].isRedLine) continue;
                    int loopExcuteCode = GetLoopCode(i, true);
                    if (loopExcuteCode == Constants.STATE_CONTINUE) continue;
                    if (loopExcuteCode == Constants.STATE_BREAK) break;
                    var applyInheritedPoint = outTimingPoints.LastOrDefault(tp => tp.time == beatmap.timingPoints[i].time && !tp.isRedLine);
                    if (applyInheritedPoint == null)
                    {
                        applyInheritedPoint = beatmap.timingPoints.LastOrDefault(tp => tp.time == beatmap.timingPoints[i].time && !tp.isRedLine);
                        if (applyInheritedPoint == null) return false;
                    }
                    beatmap.timingPoints[i].volume = applyInheritedPoint.volume;
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
        /// ループコード取得処理
        /// </summary>
        /// <param name="redLineIndex">赤線のインデックス</param>
        /// <param name="snapIndex">スナップのインデックス</param>
        /// <param name="beatSnapTiming">beatsnapのタイミング</param>
        /// <param name="currentTiming">現在のタイミング</param>
        /// <param name="redLineIndex">次の赤線のインデックス</param>
        /// <returns>-1 ; break<br/>0 : 続行<br/>1 : continue</returns>
        private static int GetLoopCode(int redLineIndex, int snapIndex, ref double beatSnapTiming, ref double currentTiming, int nextRedLineIndex = 0)
        {
            // 始点に適用が無効化されている場合は何も処理をしない
            if (!userInputData.setObjectOption.isTimingStart &&
                ((int)currentTiming == userInputData.timingFrom))
            {
                currentTiming = beatmap.timingPoints[redLineIndex].time + beatSnapTiming * (snapIndex + 1);
                return Constants.STATE_CONTINUE;
            }
            // 終点に適用が無効化されている場合は何も処理をしない
            if (!userInputData.setObjectOption.isTimingEnd &&
                ((int)currentTiming == userInputData.timingTo))
            {
                currentTiming = beatmap.timingPoints[redLineIndex].time + beatSnapTiming * (snapIndex + 1);
                return Constants.STATE_BREAK;
            }
            if ((int)currentTiming < userInputData.timingFrom)
            {
                currentTiming = beatmap.timingPoints[redLineIndex].time + beatSnapTiming * (snapIndex + 1);
                return Constants.STATE_CONTINUE;
            }
            if (userInputData.timingTo < (int)currentTiming) return Constants.STATE_BREAK;
            if (nextRedLineIndex > 0)
            {
                if ((int)currentTiming >= beatmap.timingPoints[nextRedLineIndex].time)
                {
                    currentTiming = beatmap.timingPoints[nextRedLineIndex].time;
                    beatSnapTiming = Constants.ONE_MINUTE / beatmap.timingPoints[nextRedLineIndex].bpm / userInputData.setBeatSnapOption.beatSnap;
                    return Constants.STATE_BREAK;
                }
            }
            return Constants.STATE_NONE;
        }
        /// <summary>
        /// 計算コードに応じて1msあたりのSVを計算する
        /// </summary>
        /// <returns>1msあたりのSVを返す</returns>
        /// <exception cref="ArgumentException">計算コードが不正</exception>
        private static double GetSvPerMs()
        {
            switch (userInputData.calculationCode)
            {
                case Constants.CALCULATION_ARITHMETIC:
                    return (userInputData.svTo - userInputData.svFrom) / (userInputData.timingTo - userInputData.timingFrom);
                case Constants.CALCULATION_GEOMETRIC:
                    return Math.Pow(userInputData.svTo / userInputData.svFrom, 1.0 / (userInputData.timingTo - userInputData.timingFrom));
                default:
                    throw new ArgumentException("Invalid calculation code");
            }
        }
        /// <summary>
        /// 1msあたりのVolumeを計算する
        /// </summary>
        /// <returns>1msあたりのVolumeを返す</returns>
        private static double GetVolumePerMs() => (double)(userInputData.volumeTo - userInputData.volumeFrom) / (double)(userInputData.timingTo - userInputData.timingFrom);
        /// <summary>
        /// 算出した1msあたりのSVを元に、指定されたタイミングのSVを計算する
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="greenLineIndex">赤線/緑線のインデックス</param>
        /// <param name="svPerMs">1msあたりのSV</param>
        /// <returns>算出したSV</returns>
        /// <exception cref="ArgumentException">計算コードが不正</exception>
        private static double CalculateSv(int hitObjectIndex, int greenLineIndex, double svPerMs)
        {
            double baseSv = beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.sv ?? 1.0;
            if (userInputData.isSv)
            {
                double currentSv;
                switch (userInputData.calculationCode)
                {
                    case Constants.CALCULATION_ARITHMETIC:
                        currentSv = userInputData.svFrom + (svPerMs * (beatmap.hitObjects[hitObjectIndex].time - userInputData.timingFrom));
                        break;
                    case Constants.CALCULATION_GEOMETRIC:
                        currentSv = userInputData.svFrom * Math.Pow(svPerMs, (double)(beatmap.hitObjects[hitObjectIndex].time - userInputData.timingFrom));
                        break;
                    default:
                        throw new ArgumentException("Invalid calculation code");
                }
                switch (userInputData.relativeCode)
                {
                    case Constants.RELATIVE_DISABLE:
                        return currentSv;
                    case Constants.RELATIVE_MULTIPLY:
                        return (baseSv - userInputData.relativeBaseSv) * currentSv + userInputData.relativeBaseSv;
                    case Constants.RELATIVE_SUM:
                        return baseSv + currentSv;
                    default:
                        throw new ArgumentException("Invalid relative code");
                }
            }
            return baseSv;
        }
        /// <summary>
        /// 算出した1msあたりのSVを元に、指定されたタイミングのSVを計算する
        /// </summary>
        /// <param name="timingPointIndex">タイミングのインデックス</param>
        /// <param name="svPerMs">1msあたりのSV</param>
        /// <returns>算出したSV</returns>
        /// <exception cref="ArgumentException">計算コードが不正</exception>
        private static double CalculateSv(int timingPointIndex, double svPerMs)
        {
            double baseSv = beatmap.timingPoints.LastOrDefault(tp => tp.time <= beatmap.timingPoints[timingPointIndex].time)?.sv ?? 1.0;
            if (userInputData.isSv)
            {
                double currentSv;
                switch (userInputData.calculationCode)
                {
                    case Constants.CALCULATION_ARITHMETIC:
                        currentSv = userInputData.svFrom + (svPerMs * (beatmap.timingPoints[timingPointIndex].time - userInputData.timingFrom));
                        break;
                    case Constants.CALCULATION_GEOMETRIC:
                        currentSv = userInputData.svFrom * Math.Pow(svPerMs, (double)(beatmap.timingPoints[timingPointIndex].time - userInputData.timingFrom));
                        break;
                    default:
                        throw new ArgumentException("Invalid calculation code");
                }
                switch (userInputData.relativeCode)
                {
                    case Constants.RELATIVE_DISABLE:
                        return currentSv;
                    case Constants.RELATIVE_MULTIPLY:
                        return (baseSv - userInputData.relativeBaseSv) * currentSv + userInputData.relativeBaseSv;
                    case Constants.RELATIVE_SUM:
                        return baseSv + currentSv;
                    default:
                        throw new ArgumentException("Invalid relative code");
                }
            }
            return baseSv;
        }
        /// <summary>
        /// 算出した1msあたりのVolumeを元に、指定されたタイミングのVolumeを計算する
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="greenLineIndex">赤線/緑線のインデックス</param>
        /// <param name="volumePerMs">1msあたりのVolume</param>
        /// <returns>算出したVolume</returns>
        private static int CalculateVolume(int hitObjectIndex, int greenLineIndex, double volumePerMs)
        {

            if (userInputData.isVolume)
            {
                return (int)(userInputData.volumeFrom + (volumePerMs * (beatmap.hitObjects[hitObjectIndex].time - userInputData.timingFrom)) + 0.5);
            }
            return beatmap.timingPoints.SafeGetIndex(greenLineIndex)?.volume ?? 100;
        }
        /// <summary>
        /// 算出した1msあたりのVolumeを元に、指定されたタイミングのVolumeを計算する
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="greenLineIndex">赤線/緑線のインデックス</param>
        /// <param name="volumePerMs">1msあたりのVolume</param>
        /// <returns>算出したVolume</returns>
        private static int CalculateVolume(int timingPointIndex, double volumePerMs)
        {

            if (userInputData.isVolume)
            {
                return (int)(userInputData.volumeFrom + (volumePerMs * (beatmap.timingPoints[timingPointIndex].time - userInputData.timingFrom)) + 0.5);
            }
            return beatmap.timingPoints.SafeGetIndex(timingPointIndex)?.volume ?? 100;
        }
        /// <summary>
        /// 算出した1msあたりのVolumeを元に、指定されたタイミングのVolumeを計算する
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="greenLineIndex">赤線/緑線のインデックス</param>
        /// <param name="volumePerMs">1msあたりのVolume</param>
        /// <returns>算出したVolume</returns>
        private static int CalculateVolume(double currentTiming, double volumePerMs)
        {

            if (userInputData.isVolume)
            {
                return (int)(userInputData.volumeFrom + (volumePerMs * (currentTiming - userInputData.timingFrom)) + 0.5);
            }
            
            return beatmap.timingPoints.LastOrDefault(tp => tp.time < currentTiming)?.volume ?? 100;
        }
        /// <summary>
        /// オフセット取得処理 (1/16(1/12)オフセット時)
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="redLineIndex">赤線のインデックス</param>
        /// <returns>オフセットの値</returns>
        private static double GetOffsetTiming(int hitObjectIndex, int redLineIndex)
        {
            double hexaOffset = Constants.ONE_MINUTE / beatmap.timingPoints[redLineIndex].bpm / Constants.HEXA_SNAP;
            double duoOffset = Constants.ONE_MINUTE / beatmap.timingPoints[redLineIndex].bpm / Constants.DUO_SNAP;
            HitObject? prevHitObject = beatmap.hitObjects.SafeGetIndex(hitObjectIndex - 1);
            HitObject? nextHitObject = beatmap.hitObjects.SafeGetIndex(hitObjectIndex + 1);
            if (userInputData.isDuoOffset)
            {
                if (beatmap.hitObjects[hitObjectIndex].snap != Constants.DUO_SNAP)
                {
                    double intervalTri = Constants.ONE_MINUTE / beatmap.timingPoints[redLineIndex].bpm / Constants.TRI_SNAP;

                    bool isOddAdjacent = (prevHitObject != null && prevHitObject.time >= beatmap.hitObjects[hitObjectIndex].time - intervalTri - Constants.MILLISECOND_TOLERANCE && prevHitObject.snap == Constants.DUO_SNAP)
                                      || (nextHitObject != null && nextHitObject.time <= beatmap.hitObjects[hitObjectIndex].time + intervalTri + Constants.MILLISECOND_TOLERANCE && nextHitObject.snap == Constants.DUO_SNAP);
                    if (isOddAdjacent)
                    {
                        return duoOffset;
                    }
                    else
                    {
                        return hexaOffset;
                    }
                }
                else
                {
                    return duoOffset;
                }
            }
            else
            {
                return hexaOffset;
            }
        }
        /// <summary>
        /// オフセット適用後のタイミング取得処理
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="greenLineIndex">赤線/緑線のインデックス</param>
        /// <param name="redLineIndex">赤線のインデックス</param>
        /// <param name="offset">オフセット値</param>
        /// <returns>オフセット適用後のタイミング</returns>
        private static int GetOffsetTiming(int hitObjectIndex, int greenLineIndex, int redLineIndex, out double offset)
        {
            offset = userInputData.offset;
            if (userInputData.isOffset)
            {
                if (beatmap.timingPoints[0].time == beatmap.hitObjects[hitObjectIndex].time)
                {
                    return beatmap.hitObjects[hitObjectIndex].time;
                }
                else
                {
                    switch (userInputData.offsetMode)
                    {
                        case 0:
                            offset = -GetOffsetTiming(hitObjectIndex, redLineIndex);
                            return (int)(beatmap.hitObjects[hitObjectIndex].rawTime + offset);
                        case 1:
                            return beatmap.hitObjects[hitObjectIndex].time + (int)offset;
                        default:
                            throw new Exception();
                    }
                }
            }
            else
            {
                return beatmap.hitObjects[hitObjectIndex].time;
            }
        }
        /// <summary>
        /// エフェクト取得処理
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="greenLineIndex">赤線/緑線のインデックス</param>
        /// <param name="offset">オフセット値</param>
        /// <returns>エフェクトの値</returns>
        private static int GetEffect(int hitObjectIndex, int greenLineIndex, double offset)
        {
            try
            {
                if (beatmap.timingPoints.SafeGetIndex(greenLineIndex) == null) return 0;
                if (beatmap.hitObjects[hitObjectIndex].time == beatmap.timingPoints[greenLineIndex].time && offset < 0)
                {
                    var tp = beatmap.timingPoints.LastOrDefault(tp => tp.time < beatmap.timingPoints[greenLineIndex].time && !tp.isRedLine);
                    if (tp == null)
                    {
                        if ((beatmap.timingPoints[greenLineIndex].effect & 1) >= 1) return 0;
                    }
                    else
                    {
                        if ((beatmap.timingPoints[greenLineIndex].effect & 1) == (tp.effect & 1))
                        {
                            return beatmap.timingPoints[greenLineIndex].effect;
                        }
                        else
                        {
                            return tp.effect;
                        }
                    }

                }
                return beatmap.timingPoints[greenLineIndex].effect;
            }
            catch
            {
                return 0;
            }
        }
        /// <summary>
        /// kiaiの切り替えのチェック
        /// </summary>
        /// <param name="index">緑線のインデックス</param>
        /// <returns>kiaiの切り替え用の緑線だった場合はtrue、それ以外の場合はfalse</returns>
        private static bool CheckKiai(int index)
        {
            try
            {
                if (beatmap.timingPoints.SafeGetIndex(index) == null || !beatmap.timingPoints[index].isDelete) return false;
                int kiai = beatmap.timingPoints[index].effect & 1;
                int prevKiai = (beatmap.timingPoints.SafeGetIndex(index - 1)?.effect ?? ~kiai) & 1;
                var objectOnGreenLine = beatmap.hitObjects.FirstOrDefault(ho => ho.time == beatmap.timingPoints[index].time);
                if (kiai == prevKiai) return false;
                if (objectOnGreenLine == null || userInputData.isOffset)
                {
                    beatmap.timingPoints[index].isDelete = false;
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// 始点からbeatsnapまでのoffsetを求める
        /// </summary>
        /// <param name="currentTiming">現在のタイミング</param>
        /// <param name="beatSnapTiming">beatsnapのタイミング</param>
        /// <param name="rawTimingFrom">厳密な始点のタイミング</param>
        /// <returns></returns>
        private static double GetBeatSnapOffset(double currentTiming, double beatSnapTiming, double rawTimingFrom)
        {
            double baseTiming = currentTiming;
            for (int i = 0; ; i++)
            {
                if (currentTiming >= rawTimingFrom) return currentTiming - rawTimingFrom;
                currentTiming = baseTiming + beatSnapTiming * (i + 1);
            }
        }
        /// <summary>
        /// 緑線設置判定処理
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="offset">オフセット値</param>
        /// <param name="outTimingPoints">適用された緑線</param>
        /// <returns>true : 緑線設置する<br/>false : 緑線設置しない</returns>
        private static bool IsSetInheritedPoint(int hitObjectIndex, double offset, List<TimingPoint> outTimingPoints)
        {
            TimingPoint? applyOutputInheritedPoint = new();
            // オフセット値が0の場合
            if (offset == 0)
            {
                applyOutputInheritedPoint = outTimingPoints.LastOrDefault(tp => (tp.time == beatmap.hitObjects[hitObjectIndex].time) && !tp.isRedLine);
                // ノーツと同じタイミングに緑線がない場合
                if (applyOutputInheritedPoint == null)
                {
                    // 緑線設定フラグを有効にする
                    return true;
                }
                // ある場合は(同じタイミングに赤線,緑線,ノーツがある)
                else
                {
                    // 何もしない
                    return false;
                }
            }
            // オフセット値が0以外の場合
            else
            {
                // 緑線設定フラグを有効にする
                return true;
            }
        }
        #endregion
        #region 削除処理
        /// <summary>
        /// 適用処理
        /// </summary>
        /// <param name="userInputData">ユーザー入力データ</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns></returns>
        internal static bool Remove(UserInputData userInputData, Beatmap beatmap)
        {
            try
            {
                SetClasses(userInputData, beatmap);
                for (int i = 0; i < beatmap.timingPoints.Count; i++)
                {
                    if (beatmap.timingPoints[i].isRedLine) continue;
                    int loopCode = GetLoopCode(i, true);
                    if (loopCode == Constants.STATE_CONTINUE) continue;
                    if (loopCode == Constants.STATE_BREAK) break;
                    if ((beatmap.timingPoints[i].time == userInputData.timingFrom && !userInputData.setObjectOption.isTimingStart) ||
                        (beatmap.timingPoints[i].time == userInputData.timingTo && !userInputData.setObjectOption.isTimingEnd)) continue;
                    beatmap.timingPoints[i].isDelete = true;
                }
                if (!SetDeleteFlags()) throw new Exception("Failed to set delete flags.");
                // ユーザーが指定した範囲外のスライダーの長さの調整
                if (!AdjustSliderLengthAfterExecute(beatmap.timingPoints, Constants.EXECUTE_REMOVE)) throw new Exception("Failed to adjust slider length.");
                if (!DeleteTimingPoints()) throw new Exception("Failed to delete timing points.");
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteWarningMessage("LOG_E-ADD-LINES");
                Common.WriteExceptionMessage(ex);
                return false;
            }
            finally
            {
                ClearClasses();
            }
        }
        private static bool SetDeleteFlags()
        {
            try
            {
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion
        /// <summary>
        /// ループコード取得処理
        /// </summary>
        /// <param name="index">インデックス</param>
        /// <param name="isTimingPoints">タイミングポイントフラグ</param>
        /// <returns>-1 ; break<br/>0 : 続行<br/>1 : continue</returns>
        private static int GetLoopCode(int index, bool isTimingPoints = false)
        {
            if (isTimingPoints)
            {
                // ユーザーが入力した始点より後ろの場合は処理を抜ける
                if (beatmap.timingPoints[index].time > userInputData.timingTo) return Constants.STATE_BREAK;
                // ユーザーが入力した始点より前の場合は何も処理をしない
                if (beatmap.timingPoints[index].time < userInputData.timingFrom) return Constants.STATE_CONTINUE;
            }
            else
            {
                // ユーザーが入力した始点より後ろの場合は処理を抜ける
                if (beatmap.hitObjects[index].time > userInputData.timingTo) return Constants.STATE_BREAK;
                // ユーザーが入力した始点より前の場合は何も処理をしない
                if (beatmap.hitObjects[index].time < userInputData.timingFrom) return Constants.STATE_CONTINUE;
                // 始点に適用が無効化されている場合は何も処理をしない
                if (((userInputData.setObjectOption.setObjectsCode & 0x0000037f) != 0) &&
                    !userInputData.setObjectOption.isTimingStart &&
                    (beatmap.hitObjects[index].time == userInputData.timingFrom)) return Constants.STATE_CONTINUE;
                // 終点に適用が無効化されている場合は何も処理をしない
                if (((userInputData.setObjectOption.setObjectsCode & 0x0000037f) != 0) &&
                    !userInputData.setObjectOption.isTimingEnd &&
                    (beatmap.hitObjects[index].time == userInputData.timingTo)) return Constants.STATE_CONTINUE;
            }
            return Constants.STATE_NONE;
        }
        /// <summary>
        /// ユーザーが指定した範囲より後にあるスライダーの長さ調整をする
        /// </summary>
        /// <param name="outTimingPoints">適用された緑線</param>
        /// <param name="executeCode">実行コード</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool AdjustSliderLengthAfterExecute(List<TimingPoint> outTimingPoints, int executeCode)
        {
            // 削除処理をすると上手くスライダーの長さが調整されない
            try
            {
                // 指定範囲の始点
                int timingFrom = executeCode == Constants.EXECUTE_APPLY ?
                                 userInputData.timingTo : userInputData.timingFrom;
                // 終点の次のタイミングポイント
                int timingTo = beatmap.timingPoints.FirstOrDefault(tp => tp.time > userInputData.timingTo)?.time ?? int.MaxValue; ;
                // 順番を揃える為ソートする
                outTimingPoints = [.. outTimingPoints.OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0)];
                for (global::System.Int32 i = 0; i < beatmap.hitObjects.Count; i++)
                {
                    // ノーツのタイミングが指定範囲外だった場合は何も処理をしない
                    if (!beatmap.hitObjects[i].time.IsWithin(timingFrom, timingTo)) continue;
                    // 直前のTimingPointを探す
                    var greenLineIndex = outTimingPoints.FindLastIndex(tp => tp.time <= beatmap.hitObjects[i].time);
                    if (greenLineIndex < 0 && beatmap.hitObjects[i].noteType == Constants.NoteType.SLIDER)
                    {
                        // ノーツがスライダーだった場合、スライダーの長さを調整する
                        beatmap.hitObjects[i].sliderLength = GetAdjustedSliderLength(i, beatmap.timingPoints[greenLineIndex].sv);
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
        /// 削除対象赤線/緑線削除処理
        /// </summary>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool DeleteTimingPoints()
        {
            try
            {
                beatmap.timingPoints.RemoveAll(tp => tp.isDelete);
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
        /// 削除対象の赤線/緑線にdeleteFlagを設定する処理
        /// </summary>
        /// <param name="applyInheritedPointIndexes">削除対象のインデックスリスト</param>
        private static void SetDeleteFlag(List<int> applyInheritedPointIndexes)
        {
            foreach (var index in applyInheritedPointIndexes)
            {
                if (beatmap.timingPoints.SafeGetIndex(index) == null)
                {
                    continue;
                }
                beatmap.timingPoints[index].isDelete = true;
            }
        }
        /// <summary>
        /// 調整済みのsliderLengthを取得する
        /// </summary>
        /// <param name="hitObjectIndex">ヒットオブジェクトのインデックス</param>
        /// <param name="sv">適用後SV</param>
        /// <returns>調整済みのsliderLength</returns>
        private static double GetAdjustedSliderLength(int hitObjectIndex, double sv) => beatmap.hitObjects[hitObjectIndex].sliderLength * (sv / beatmap.hitObjects[hitObjectIndex].sv);
    }
}