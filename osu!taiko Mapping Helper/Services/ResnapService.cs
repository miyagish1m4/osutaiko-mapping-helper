using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils;

namespace osu_taiko_Mapping_Helper.Services
{
    internal class ResnapService
    {
        // 別のロジック考案中
        //internal static bool Apply(UserInputUtilityData userInputUtilityData, Beatmap beatmap)
        //{
        //    double baseBpm = 120;
        //    double beatSnapLength = -1L;
        //    try
        //    {
        //        var baseTimingPoint = userInputUtilityData.resnapTimingFrom < beatmap.timingPoints[0].time ?
        //            beatmap.timingPoints[0] : beatmap.timingPoints.LastOrDefault(tp => (tp.time <= userInputUtilityData.resnapTimingFrom) && tp.isRedLine);
        //        var baseTimingPointIndex = userInputUtilityData.resnapTimingFrom < beatmap.timingPoints[0].time ?
        //            0 : beatmap.timingPoints.FindLastIndex(tp => (tp.time <= userInputUtilityData.resnapTimingFrom) && tp.isRedLine);
        //        var timingPointIndexes = beatmap.timingPoints.Select((tp, index) => new { tp, Index = index })
        //                                                     .Where(x => x.tp.isRedLine)
        //                                                     .Select(x => x.Index)
        //                                                     .ToList();
        //        baseBpm = baseTimingPoint != null ? (double)baseTimingPoint.bpm : 120;
        //        if (baseTimingPoint == null) throw new Exception("No valid base timing point found for the specified resnap timing.");
        //        double currentTime = -1;
        //        double baseTime = baseTimingPoint.time;
        //        int currentIndex = 0;

        //        foreach (var timingPoint in timingPointIndexes)
        //        {
        //            if (timingPoint == baseTimingPointIndex)
        //            {
        //                break;
        //            }
        //            currentIndex++;
        //        }
        //        if (timingPointIndexes[currentIndex] == 0 && beatmap.hitObjects[0].time < beatmap.timingPoints[timingPointIndexes[currentIndex]].time)
        //        {
        //            beatSnapLength = (double)beatmap.timingPoints[timingPointIndexes[currentIndex]].barLength / userInputUtilityData.beatSnap / 4;
        //            var diffTime = beatmap.timingPoints[timingPointIndexes[currentIndex]].time - beatmap.hitObjects[0].time;
        //            var snapCount = (int)Math.Floor(diffTime / beatSnapLength);
        //            currentTime = beatmap.timingPoints[timingPointIndexes[currentIndex]].time - beatSnapLength * snapCount;
        //            for (int i = 0; ; i++)
        //            {
        //                if ((int)(currentTime + beatSnapLength / 2) >= beatmap.timingPoints[timingPointIndexes[currentIndex]].time)
        //                {
        //                    break;
        //                }
        //                var notesInCurrentSnapIndexes = beatmap.hitObjects.Select((ho, index) => new { ho, Index = index })
        //                                                                  .Where(x => x.ho.time >= (int)(currentTime - beatSnapLength / 2) && x.ho.time < (int)(currentTime + beatSnapLength / 2))
        //                                                                  .Select(x => x.Index)
        //                                                                  .ToList();
        //                if (notesInCurrentSnapIndexes.Count() == 0)
        //                {
        //                    currentTime = baseTime - beatSnapLength * (snapCount - i - 1);
        //                    continue;
        //                }
        //                for (int k = 0; k < notesInCurrentSnapIndexes.Count(); k++)
        //                {
        //                    beatmap.hitObjects[notesInCurrentSnapIndexes[k]].time = (int)currentTime;
        //                }
        //                currentTime = baseTime - beatSnapLength * (snapCount - i - 1);
        //            }
        //        }
        //        beatSnapLength = (double)beatmap.timingPoints[timingPointIndexes[currentIndex]].barLength / userInputUtilityData.beatSnap / 4;
        //        baseTime = beatmap.timingPoints[timingPointIndexes[currentIndex]].time;
        //        currentTime = beatmap.timingPoints[timingPointIndexes[currentIndex]].time;
        //        for (int i = 0,j = currentIndex; i < beatmap.hitObjects.Count; i++)
        //        {
        //            if ((int)(beatmap.hitObjects[i].time + beatSnapLength / 2) <= userInputUtilityData.resnapTimingFrom)
        //            {
        //                currentTime = baseTime + beatSnapLength * (j + 1);
        //                continue;
        //            }
        //            if ((int)(beatmap.hitObjects[i].time + beatSnapLength / 2) > userInputUtilityData.resnapTimingTo)
        //            {
        //                return true;
        //            }
        //            List<int> betweenTimingPointIndex = [];
        //            var diffTime = beatmap.hitObjects[i].time - beatmap.timingPoints[timingPointIndexes[currentIndex]].time;
        //            var snapCount = (int)Math.Round(diffTime / beatSnapLength, MidpointRounding.AwayFromZero);
        //            var snappedTime = beatmap.timingPoints[timingPointIndexes[currentIndex]].time + beatSnapLength * snapCount;
        //            if (i != beatmap.hitObjects.Count - 1)
        //            {
        //                betweenTimingPointIndex = [.. beatmap.timingPoints.Select((tp, index) => new { tp, Index = index })
        //                                                                  .Where(x => x.tp.time > beatmap.hitObjects[i].time && x.tp.time <= beatmap.hitObjects[i + 1].time && x.tp.isRedLine)
        //                                                                  .Select(x => x.Index)];

        //            }
        //            if (betweenTimingPointIndex.Count != 0)
        //            {
        //                j += betweenTimingPointIndex.Count;
        //                beatSnapLength = (double)beatmap.timingPoints[timingPointIndexes[j]].barLength / userInputUtilityData.beatSnap / 4;
        //                baseTime = beatmap.timingPoints[timingPointIndexes[j]].time;
        //                currentTime = beatmap.timingPoints[timingPointIndexes[j]].time;
        //            }

        //        }
        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error in Apply method: {ex.Message}");
        //        return false;
        //    }
        //}

        /// <summary>
        /// リスナップ機能を実行する
        /// </summary>
        /// <param name="userInputUtilityData">入力データ</param>
        /// <param name="beatmap">譜面情報</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool Apply(UserInputUtilityData userInputUtilityData, Beatmap beatmap)
        {
            double baseBpm = 120;
            double beatSnapLength = -1L;
            try
            {
                // もし全範囲指定(タイミングの入力なし)だった場合1ノーツ目と最終ノーツを始点と終点にする
                if (userInputUtilityData.isAllNotes)
                {
                    userInputUtilityData.resnapTimingFrom = beatmap.hitObjects[0].time;
                    userInputUtilityData.resnapTimingTo = beatmap.hitObjects[beatmap.hitObjects.Count - 1].time + 20;
                }
                // 始点の直前のタイミングポイントを取得
                // もしなかった場合は、最初のタイミングポイントを取得する
                var baseTimingPoint = userInputUtilityData.resnapTimingFrom < beatmap.timingPoints[0].time ?
                    beatmap.timingPoints[0] : beatmap.timingPoints.LastOrDefault(tp => (tp.time <= userInputUtilityData.resnapTimingFrom) && tp.isRedLine);
                // 始点の直前のタイミングポイントのインデックスを取得
                var baseTimingPointIndex = userInputUtilityData.resnapTimingFrom < beatmap.timingPoints[0].time ?
                    0 : beatmap.timingPoints.FindLastIndex(tp => (tp.time <= userInputUtilityData.resnapTimingFrom) && tp.isRedLine);
                // 赤線のタイミングポイントのインデックスをリストで取得
                var timingPointIndexes = beatmap.timingPoints.Select((tp, index) => new { tp, Index = index })
                                                             .Where(x => x.tp.isRedLine)
                                                             .Select(x => x.Index)
                                                             .ToList();
                // 始点のBPMを取得
                baseBpm = baseTimingPoint != null ? baseTimingPoint.bpm : 120;
                if (baseTimingPoint == null) throw new Exception("No valid base timing point found for the specified resnap timing.");
                double currentTime = -1;
                double baseTime = baseTimingPoint.time;
                int currentIndex = 0;

                // 始点のインデックスを取得
                foreach (var timingPoint in timingPointIndexes)
                {
                    if (timingPoint == baseTimingPointIndex)
                    {
                        break;
                    }
                    currentIndex++;
                }

                // 始点の直前のタイミングポイントより前にノーツがある場合は、最初のタイミングポイントを基準にリスナップする
                if (timingPointIndexes[currentIndex] == 0 && beatmap.hitObjects[0].time < beatmap.timingPoints[timingPointIndexes[currentIndex]].time)
                {
                    beatSnapLength = beatmap.timingPoints[timingPointIndexes[currentIndex]].barLength / userInputUtilityData.beatSnap / 4;
                    var diffTime = beatmap.timingPoints[timingPointIndexes[currentIndex]].time - beatmap.hitObjects[0].time;
                    var snapCount = (int)Math.Floor(diffTime / beatSnapLength);
                    currentTime = beatmap.timingPoints[timingPointIndexes[currentIndex]].time - beatSnapLength * snapCount;
                    for (int i = 0; ; i++)
                    {
                        // もし現在のスナップ位置のノーツが、始点の直前のタイミングポイントの時間を超えている場合は、ループを抜ける
                        if ((int)(currentTime + beatSnapLength / 2) >= beatmap.timingPoints[timingPointIndexes[currentIndex]].time)
                        {
                            break;
                        }
                        var notesInCurrentSnapIndexes = beatmap.hitObjects.Select((ho, index) => new { ho, Index = index })
                                                                          .Where(x => x.ho.time >= (int)(currentTime - beatSnapLength / 2) && x.ho.time < (int)(currentTime + beatSnapLength / 2))
                                                                          .Select(x => x.Index)
                                                                          .ToList();
                        // もし現在のスナップの範囲にノーツがない場合は、次のスナップ位置に移動する
                        if (notesInCurrentSnapIndexes.Count() == 0)
                        {
                            currentTime = baseTime - beatSnapLength * (snapCount - i - 1);
                            continue;
                        }
                        // もし現在のスナップの範囲にノーツ複数あった場合に備えfor文で対応する
                        for (int k = 0; k < notesInCurrentSnapIndexes.Count(); k++)
                        {
                            beatmap.hitObjects[notesInCurrentSnapIndexes[k]].time = (int)currentTime;
                        }
                        currentTime = baseTime - beatSnapLength * (snapCount - i - 1);
                    }
                }
                // 始点以降のノーツをリスナップする
                for (int i = currentIndex; i < timingPointIndexes.Count; i++)
                {
                    var nextTiming = i == timingPointIndexes.Count - 1 ? beatmap.hitObjects[^1].time + 1 : beatmap.timingPoints[timingPointIndexes[i + 1]].time;
                    beatSnapLength = beatmap.timingPoints[timingPointIndexes[i]].barLength / userInputUtilityData.beatSnap / 4;
                    baseTime = beatmap.timingPoints[timingPointIndexes[i]].time;
                    currentTime = beatmap.timingPoints[timingPointIndexes[i]].time;
                    for (int j = 0; ; j++)
                    {
                        // もし現在のスナップ位置が始点より前の場合は、次のスナップ位置に移動する
                        if ((int)(currentTime + beatSnapLength / 2) <= userInputUtilityData.resnapTimingFrom)
                        {
                            currentTime = baseTime + beatSnapLength * (j + 1);
                            continue;
                        }
                        // もし現在のスナップ位置が次のタイミングポイントを超えている場合は、ループを抜ける
                        if ((int)currentTime >= nextTiming)
                        {
                            break;
                        }
                        // もし現在のスナップ位置が終点を超えている場合は、このメソッドを終了する
                        if ((int)(currentTime + beatSnapLength / 2) > userInputUtilityData.resnapTimingTo)
                        {
                            return true;
                        }
                        var notesInCurrentSnapIndexes = beatmap.hitObjects.Select((ho, index) => new { ho, Index = index })
                                                                          .Where(x => x.ho.time >= (int)(currentTime - beatSnapLength / 2) && x.ho.time < (int)(currentTime + beatSnapLength / 2))
                                                                          .Select(x => x.Index)
                                                                          .ToList();
                        // もし現在のスナップの範囲にノーツがない場合は、次のスナップ位置に移動する
                        if (notesInCurrentSnapIndexes.Count() == 0)
                        {
                            currentTime = baseTime + beatSnapLength * (j + 1);
                            continue;
                        }
                        // もし現在のスナップの範囲にノーツ複数あった場合に備えfor文で対応する
                        for (int k = 0; k < notesInCurrentSnapIndexes.Count(); k++)
                        {
                            beatmap.hitObjects[notesInCurrentSnapIndexes[k]].time = (int)currentTime;
                        }
                        currentTime = baseTime + beatSnapLength * (j + 1);
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
