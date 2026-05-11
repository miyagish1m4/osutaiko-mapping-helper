using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;
using static System.Windows.Forms.Design.AxImporter;

namespace osu_taiko_Mapping_Helper.Services
{
    internal class SVCalculatorService
    {
        /// <summary>
        /// 適用ボタン押下時の処理
        /// </summary>
        /// <param name="userInputData">ユーザー入力データ</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">追加する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool Apply(UserInputData userInputData, Beatmap beatmap, ref List<TimingPoint> outTimingPoints)
        {
            List<TimingPoint> timingPointsBuff = [];
            try
            {
                if (userInputData.setOption.isSetObjects)
                {
                    if (userInputData.offsetMode == 0 && userInputData.isOffset)
                    {
                        if (!BeatmapHelper.GetSnapsOnHitObjects(ref beatmap))
                        {
                            throw new Exception();
                        }
                    }
                    // objectに緑線を置く場合
                    if (!ApplyOnObjects(userInputData, beatmap, ref timingPointsBuff))
                    {
                        throw new Exception();
                    }
                }
                else if (userInputData.setOption.isSetBeatSnap)
                {
                    // beatSnap間隔で緑線を置く場合
                    if (!ApplyOnBeatSnaps(userInputData, beatmap, ref timingPointsBuff))
                    {
                        throw new Exception();
                    }

                }
                else if (userInputData.setOption.isSetGreenLine)
                {
                    // 緑線の更新のみ行う場合
                    if (!ApplyGreenLine(userInputData, beatmap, ref timingPointsBuff))
                    {
                        throw new Exception();
                    }
                }
                else
                {
                    throw new Exception();
                }
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

        }
        /// <summary>
        /// 削除ボタン押下時の処理
        /// </summary>
        /// <param name="userInputData">ユーザー入力データ</param>
        /// <param name="beatmap">譜面情報</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool Remove(UserInputData userInputData, Beatmap beatmap)
        {
            try
            {
                // 緑線の削除を行う
                if (!RemoveInferitedPoint(userInputData, beatmap))
                {
                    throw new Exception();
                }
                // スライダーの長さの調整
                if (!AdjustSliderLengthAfterExecute(userInputData, beatmap, beatmap.timingPoints, Constants.EXECUTE_REMOVE))
                {
                    throw new Exception();
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteWarningMessage("LOG_E-ADD-LINES");
                Common.WriteExceptionMessage(ex);
                return false;
            }

        }
        /// <summary>
        /// 緑線を削除する処理
        /// </summary>
        /// <param name="userInputData">ユーザー入力データ</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">削除後のタイミングポイントを格納するリスト</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool RemoveInferitedPoint(UserInputData userInputData, Beatmap beatmap, List<TimingPoint>? outTimingPoints = null)
        {
            try
            {
                double baseSv = 1;
                // 削除後の適用されるSVと音量を求める
                var applyInheritedPoint = beatmap.timingPoints.LastOrDefault(tp => tp.time < userInputData.timingFrom);
                if (applyInheritedPoint != null)
                {
                    baseSv = applyInheritedPoint.sv;
                }
                else
                {
                    // 最初のタイミングポイントと範囲内の最初のノーツが同じタイミングの場合に該当する
                    baseSv = beatmap.timingPoints[0].sv;
                }
                // 指定範囲内にスライダーがある場合はsliderLengthを調整する
                for (global::System.Int32 i = 0; i < beatmap.hitObjects.Count; i++)
                {
                    if ((beatmap.hitObjects[i].time >= userInputData.timingFrom) &&
                        (beatmap.hitObjects[i].time <= userInputData.timingTo) &&
                        (beatmap.hitObjects[i].noteType == Constants.NoteType.SLIDER))
                    {
                        // 途中に赤線が設置されている場合はbaseSvを1に変更する
                        for (global::System.Int32 j = (beatmap.timingPoints.Count) - (1); j >= 0; j--)
                        {
                            if ((beatmap.timingPoints[j].time >= userInputData.timingFrom) &&
                                (beatmap.timingPoints[j].time <= beatmap.hitObjects[i].time))
                            {
                                baseSv = 1;
                            }
                        }
                        beatmap.hitObjects[i].sliderLength = beatmap.hitObjects[i].sliderLength *
                                                     (baseSv / beatmap.hitObjects[i].sv);
                    }
                }
                // 指定範囲内の緑線を削除する
                // 指定範囲内に赤線がある場合は適用させる音量を設定する
                for (global::System.Int32 i = (beatmap.timingPoints.Count) - (1); i >= 0; i--)
                {
                    if (beatmap.timingPoints[i].time > userInputData.timingTo)
                    {
                        continue;
                    }
                    if (beatmap.timingPoints[i].time < userInputData.timingFrom)
                    {
                        break;
                    }
                    if (!beatmap.timingPoints[i].isRedLine)
                    {
                        if (outTimingPoints != null)
                        {
                            var ho = beatmap.hitObjects.LastOrDefault(ho => ho.time == beatmap.timingPoints[i].time);
                            var otp = outTimingPoints.LastOrDefault(otp => otp.time == beatmap.timingPoints[i].time);
                            if (ho == null && otp == null)
                            {
                                var tp = beatmap.timingPoints.LastOrDefault(tp => tp.time < beatmap.timingPoints[i].time && !tp.isRedLine);
                                if (tp != null && (beatmap.timingPoints[i].effect & 1) != (tp.effect & 1))
                                {
                                    beatmap.timingPoints[i].sv = tp.sv;
                                    beatmap.timingPoints[i].volume = tp.volume;
                                    continue;
                                }
                            }
                        }
                        beatmap.timingPoints.RemoveAt(i);
                    }
                    //if (beatmap.timingPoints[i].isRedLine)
                    //{
                    //    beatmap.timingPoints[i].volume = baseVolume;
                    //}
                }

                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-REMOVE-LINES");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// Objectに緑線を適用する
        /// </summary>
        /// <param name="userInputData">ユーザー入力値</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">適用する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyOnObjects(UserInputData userInputData, Beatmap beatmap, ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                // HitObjectsに緑線適用処理
                if (!ApplyOnHitObjects(userInputData, beatmap, ref outTimingPoints))
                {
                    throw new Exception();
                }
                // TimingPointsに緑線適用処理
                if (!ApplyOnTimingPoints(userInputData, beatmap, ref outTimingPoints))
                {
                    throw new Exception();
                }
                // ユーザーが指定した範囲外のスライダーの長さの調整
                if (!AdjustSliderLengthAfterExecute(userInputData, beatmap, outTimingPoints, Constants.EXECUTE_APPLY))
                {
                    throw new Exception();
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
        /// HitObjectsに緑線を適用する
        /// </summary>
        /// <param name="userInputData">ユーザー入力値</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">適用する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyOnHitObjects(UserInputData userInputData, Beatmap beatmap, ref List<TimingPoint> outTimingPoints)
        {
            double baseBpm = 120;
            double svPerMs = 0;
            double volumePerMs = 0;
            bool isIgnoreObject = false;
            int offset = (userInputData.isOffset && userInputData.offsetMode == 1) ? userInputData.offset : 0;
            List<int> removeList = [];
            try
            {
                // 始点のBPMを求める
                var baseTimingPoint = beatmap.timingPoints.LastOrDefault(tp => (tp.time <= userInputData.timingFrom) && tp.isRedLine);
                baseBpm = baseTimingPoint != null ? baseTimingPoint.bpm : 120;
                if (userInputData.isSv)
                {
                    // 1msあたりのSVの変化量を求める
                    svPerMs = GetSvPerMs(userInputData.svFrom,
                                         userInputData.svTo,
                                         userInputData.timingFrom,
                                         userInputData.timingTo,
                                         userInputData.calculationCode);
                }
                if (userInputData.isVolume)
                {
                    // 1msあたりのVolumeの変化量を求める
                    volumePerMs = (double)(userInputData.volumeTo - userInputData.volumeFrom) /
                                  (double)(userInputData.timingTo - userInputData.timingFrom);
                }
                for (global::System.Int32 i = 0; i < beatmap.hitObjects.Count; i++)
                {
                    // ユーザーが入力した始点より前の場合は何も処理をしない
                    if (beatmap.hitObjects[i].time < userInputData.timingFrom)
                    {
                        continue;
                    }
                    // ユーザーが入力した始点より後ろの場合は処理を抜ける
                    if (beatmap.hitObjects[i].time > userInputData.timingTo)
                    {
                        break;
                    }
                    double sv;
                    int volume;
                    // 始点に適用が無効化されている場合は何も処理をしない
                    if (((userInputData.setObjectOption.setObjectsCode & 0x0000037f) != 0) &&
                        !userInputData.setObjectOption.isTimingStart &&
                        (beatmap.hitObjects[i].time == userInputData.timingFrom))
                    {
                        continue;
                    }
                    // 終点に適用が無効化されている場合は何も処理をしない
                    if (((userInputData.setObjectOption.setObjectsCode & 0x0000037f) != 0) &&
                        !userInputData.setObjectOption.isTimingEnd &&
                        (beatmap.hitObjects[i].time == userInputData.timingTo))
                    {
                        continue;
                    }
                    // 直前のTimingPointを探す
                    var applyInheritedPoint = beatmap.timingPoints.LastOrDefault(tp => tp.time <= beatmap.hitObjects[i].svApplyTime) ??
                                              throw new Exception();
                    var applyInheritedPointIndex = beatmap.timingPoints.FindLastIndex(tp => tp.time <= beatmap.hitObjects[i].svApplyTime);
                    int effect = userInputData.isKiai ? 1 : applyInheritedPoint.effect;
                    // オブジェクトコードを比較し、一致するものがある場合に緑線の追加を行う
                    if ((beatmap.hitObjects[i].hitObjectCode & userInputData.setObjectOption.setObjectsCode) != 0)
                    {
                        // 直前のTimingPointのインデックスを算出する
                        var applyInheritedPointIndexes = i == 0 ?
                                                         beatmap.timingPoints.Select((tp, index) => new { tp, index })
                                                                             .Where(x => (x.tp.time <= beatmap.hitObjects[i].svApplyTime) && !x.tp.isRedLine)
                                                                             .Select(x => x.index)
                                                                             .ToList() :
                                                         beatmap.timingPoints.Select((tp, index) => new { tp, index })
                                                                             .Where(x => (x.tp.time <= beatmap.hitObjects[i].svApplyTime && x.tp.time > beatmap.hitObjects[i - 1].time) && !x.tp.isRedLine)
                                                                             .Select(x => x.index)
                                                                             .ToList();
                        if (beatmap.hitObjects[i].time == beatmap.timingPoints[applyInheritedPointIndex].time && offset < 0)
                        {
                            var tp = beatmap.timingPoints.LastOrDefault(tp => tp.time < beatmap.timingPoints[applyInheritedPointIndex].time && !tp.isRedLine);
                            if (tp == null)
                            {
                                if ((beatmap.timingPoints[applyInheritedPointIndex].effect & 1) >= 1)
                                {
                                    effect = 0;
                                }
                            }
                            else
                            {
                                if ((beatmap.timingPoints[applyInheritedPointIndex].effect & 1) == (tp.effect & 1))
                                {
                                    effect = beatmap.timingPoints[applyInheritedPointIndex].effect;
                                }
                                else
                                {
                                    effect = tp.effect;
                                }
                            }

                        }
                        // 直前の赤線を探す
                        var applyTimingPoint = beatmap.timingPoints.LastOrDefault(tp => (tp.time <= beatmap.hitObjects[i].svApplyTime) && tp.isRedLine);
                        if ((userInputData.timingFrom + offset <= applyInheritedPoint?.time) &&
                            !applyInheritedPoint.isRedLine)
                        {
                            if (i == 0)
                            {
                                // 全体の1ノーツ目で、緑線がユーザーの指定範囲内にある場合は削除対象にする
                                removeList.AddRange(applyInheritedPointIndexes);
                            }
                            else
                            {
                                // 直前のTimingPointが前のノーツより後にあるかつ、
                                // ユーザーの指定範囲内の場合は削除対象にする
                                if (beatmap.hitObjects[i - 1].time < applyInheritedPoint?.time)
                                {
                                    removeList.AddRange(applyInheritedPointIndexes);
                                }
                            }
                        }
                        //////
                        int time;
                        if (userInputData.isOffset)
                        {
                            if (applyTimingPoint != null &&
                                beatmap.timingPoints[0].time == beatmap.hitObjects[i].time)
                            {
                                time = beatmap.hitObjects[i].time;
                            }
                            else
                            {
                                switch (userInputData.offsetMode)
                                {
                                    case 0:
                                        if (i != 0 && applyTimingPoint != null)
                                        {
                                            HitObject? prevHo = (i == 0) ? null : beatmap.hitObjects[i - 1];
                                            HitObject? nextHo = (i == beatmap.hitObjects.Count - 1) ? null : beatmap.hitObjects[i + 1];
                                            offset = -(int)GetOffsetTiming(userInputData,
                                                                           applyTimingPoint,
                                                                           beatmap.hitObjects[i],
                                                                           prevHo,
                                                                           nextHo);
                                        }
                                        time = (int)(beatmap.hitObjects[i].rawTime + offset);
                                        break;
                                    case 1:
                                        time = beatmap.hitObjects[i].time + (int)offset;
                                        break;
                                    default:
                                        throw new Exception();
                                }
                            }
                        }
                        else
                        {
                            time = beatmap.hitObjects[i].time;
                        }
                        if (userInputData.isSv)
                        {
                            // 直前のSVを取得する
                            double baseSv = applyInheritedPoint != null ? applyInheritedPoint.sv : 1;
                            // SVを求める
                            sv = CalculateSv(userInputData.svFrom,
                                             svPerMs,
                                             userInputData.timingFrom,
                                             beatmap.hitObjects[i].time,
                                             baseSv,
                                             userInputData.calculationCode,
                                             userInputData.relativeCode,
                                             userInputData.relativeBaseSv) *
                                             (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / (applyTimingPoint != null ? applyTimingPoint.bpm : 120)) : 1);
                        }
                        else
                        {
                            // 直前のSVを参照する
                            sv = applyInheritedPoint != null ? applyInheritedPoint.sv : 1;
                        }
                        if (userInputData.isVolume)
                        {
                            // 音量を求める
                            volume = applyTimingPoint != null ? (int)(CalculateVolume(userInputData.volumeFrom,
                                                                                      volumePerMs,
                                                                                      userInputData.timingFrom,
                                                                                      beatmap.hitObjects[i].time) + 0.5) : 100;
                        }
                        else
                        {
                            // 直前の音量を参照する
                            volume = applyInheritedPoint != null ? applyInheritedPoint.volume : 100;
                        }
                        if (beatmap.hitObjects[i].noteType == Constants.NoteType.SLIDER)
                        {
                            // スライダーの長さを調整する
                            beatmap.hitObjects[i].sliderLength = beatmap.hitObjects[i].sliderLength *
                                                                 (sv / beatmap.hitObjects[i].sv);
                        }
                        // 緑線を追加する
                        outTimingPoints.Add(new TimingPoint(time,
                                                            0,//bpmは緑線には不要な情報の為、0を設定する
                                                            sv,
                                                            0,//barLengthは緑線には不要な情報の為、0を設定する
                                                            applyInheritedPoint != null ? applyInheritedPoint.meter : 4,
                                                            applyInheritedPoint != null ? applyInheritedPoint.sampleSet : 1,
                                                            applyInheritedPoint != null ? applyInheritedPoint.sampleIndex : 0,
                                                            volume,
                                                            false,
                                                            effect));
                        isIgnoreObject = false;
                    }
                    else
                    {
                        // 前回のノーツがSV,Volumeの計算の対象外だった場合は何もしない
                        if (isIgnoreObject)
                        {
                            continue;
                        }
                        // 全体の1ノーツ目だった場合は最低でも赤線が適用されているため、何も処理をしない
                        if (i == 0)
                        {
                            continue;
                        }
                        // 直前のTimingPointを探す
                        if (applyInheritedPoint == null)
                        {
                            throw new Exception();
                        }
                        // 直前のTimingPointのインデックスを算出する
                        var applyInheritedPointIndexes = beatmap.timingPoints.Select((tp, index) => new { tp, index })
                                                                             .Where(x => x.tp.time <= beatmap.hitObjects[i].svApplyTime)
                                                                             .Select(x => x.index)
                                                                             .ToList();
                        if (beatmap.hitObjects[i].time == beatmap.timingPoints[applyInheritedPointIndex].time && offset < 0)
                        {
                            var tp = beatmap.timingPoints.LastOrDefault(tp => tp.time < beatmap.timingPoints[applyInheritedPointIndex].time && !tp.isRedLine);
                            if (tp == null)
                            {
                                if ((beatmap.timingPoints[applyInheritedPointIndex].effect & 1) >= 1)
                                {
                                    effect = 0;
                                }
                            }
                            else
                            {
                                if ((beatmap.timingPoints[applyInheritedPointIndex].effect & 1) == (tp.effect & 1))
                                {
                                    effect = beatmap.timingPoints[applyInheritedPointIndex].effect;
                                }
                                else
                                {
                                    effect = tp.effect;
                                }
                            }

                        }
                        if (beatmap.hitObjects[i].noteType != Constants.NoteType.BARLINE || offset != 0)
                        {
                            isIgnoreObject = true;
                        }
                        // 直前のTimingPointの情報を元に緑線を作成する
                        outTimingPoints.Add(new TimingPoint(beatmap.hitObjects[i].time + offset,
                                                            applyInheritedPoint.bpm,
                                                            applyInheritedPoint.sv,
                                                            applyInheritedPoint.barLength,
                                                            applyInheritedPoint.meter,
                                                            applyInheritedPoint.sampleSet,
                                                            applyInheritedPoint.sampleIndex,
                                                            applyInheritedPoint.volume,
                                                            false,
                                                            userInputData.isKiai ? 1 : effect));
                        foreach (var ipIndex in applyInheritedPointIndexes)
                        {
                            if ((beatmap.timingPoints[ipIndex].time > beatmap.hitObjects[i - 1].time) &&
                                !beatmap.timingPoints[ipIndex].isRedLine)
                            {
                                // 直前のTimingPointが前のノーツより後にある場合は削除対象にする
                                removeList.Add(ipIndex);
                            }
                        }
                    }
                }
                // 削除をする際に順番が変にならないようにソートする
                removeList.Sort();
                // 前から削除を行うと要素を詰めてしまうため、後ろから削除を行う
                for (global::System.Int32 i = (removeList.Count) - (1); i >= 0; i--)
                {
                    var ho = beatmap.hitObjects.LastOrDefault(ho => ho.time == beatmap.timingPoints[removeList[i]].time);
                    if (offset != 0 || ho == null)
                    {
                        var tp = beatmap.timingPoints.LastOrDefault(tp => tp.time < beatmap.timingPoints[removeList[i]].time && !tp.isRedLine);
                        if (tp != null && (tp.effect & 1) != (beatmap.timingPoints[removeList[i]].effect & 1))
                        {
                            var otp = outTimingPoints.LastOrDefault(otp => otp.time <= (beatmap.timingPoints[removeList[i]].time + offset));
                            if (otp != null)
                            {
                                beatmap.timingPoints[removeList[i]].sv = otp.sv;
                                beatmap.timingPoints[removeList[i]].volume = otp.volume;
                            }
                            continue;
                        }
                    }
                    beatmap.timingPoints.RemoveAt(removeList[i]);
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
        /// 緑線のSVを更新する処理
        /// </summary>
        /// <param name="userInputData"></param>
        /// <param name="beatmap"></param>
        /// <param name="outTimingPoints"></param>
        /// <returns></returns>
        private static bool ApplyGreenLine(UserInputData userInputData, Beatmap beatmap, ref List<TimingPoint> outTimingPoints)
        {
            try
            {
                double baseBpm = 120;
                double svPerMs = 0;
                double volumePerMs = 0;
                List<int> removeList = [];
                // 始点のBPMを求める
                var baseTimingPoint = beatmap.timingPoints.LastOrDefault(tp => (tp.time <= userInputData.timingFrom) && tp.isRedLine);
                baseBpm = baseTimingPoint != null ? baseTimingPoint.bpm : 120;
                if (userInputData.isSv)
                {
                    // 1msあたりのSVの変化量を求める
                    svPerMs = GetSvPerMs(userInputData.svFrom,
                                         userInputData.svTo,
                                         userInputData.timingFrom,
                                         userInputData.timingTo,
                                         userInputData.calculationCode);
                }
                if (userInputData.isVolume)
                {
                    // 1msあたりのVolumeの変化量を求める
                    volumePerMs = (double)(userInputData.volumeTo - userInputData.volumeFrom) /
                                  (double)(userInputData.timingTo - userInputData.timingFrom);
                }
                for (global::System.Int32 i = 0; i < beatmap.timingPoints.Count; i++)
                {
                    // ユーザーが入力した始点より前の場合は何も処理をしない
                    if (beatmap.timingPoints[i].time < userInputData.timingFrom)
                    {
                        continue;
                    }
                    // ユーザーが入力した終点より後ろの場合は処理を抜ける
                    if (beatmap.timingPoints[i].time > userInputData.timingTo)
                    {
                        break;
                    }
                    // 赤線の場合は何も処理をしない
                    if (beatmap.timingPoints[i].isRedLine)
                    {
                        continue;
                    }
                    double sv;
                    int volume;
                    var applyTimingPoint = beatmap.timingPoints.LastOrDefault(tp => (tp.time <= beatmap.timingPoints[i].time) && tp.isRedLine);
                    if (userInputData.isSv)
                    {
                        // 直前のSVを取得する
                        double baseSv = beatmap.timingPoints[i].sv;
                        // SVを求める
                        sv = CalculateSv(userInputData.svFrom,
                                         svPerMs,
                                         userInputData.timingFrom,
                                         beatmap.timingPoints[i].time,
                                         baseSv,
                                         userInputData.calculationCode,
                                         userInputData.relativeCode,
                                         userInputData.relativeBaseSv) *
                                         (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / (applyTimingPoint != null ? applyTimingPoint.bpm : 120)) : 1);
                    }
                    else
                    {
                        // 直前のSVを参照する
                        sv = beatmap.timingPoints[i].sv;
                    }
                    if (userInputData.isVolume)
                    {
                        // 音量を求める
                        volume = applyTimingPoint != null ? (int)(CalculateVolume(userInputData.volumeFrom,
                                                                                  volumePerMs,
                                                                                  userInputData.timingFrom,
                                                                                  beatmap.timingPoints[i].time) + 0.5) : 100;
                    }
                    else
                    {
                        // 直前の音量を参照する
                        volume = beatmap.timingPoints[i].volume;
                    }
                    // 直前のTimingPointの情報を元に緑線を作成する
                    outTimingPoints.Add(new TimingPoint(beatmap.timingPoints[i].time,
                                                        beatmap.timingPoints[i].bpm,
                                                        sv,
                                                        beatmap.timingPoints[i].barLength,
                                                        beatmap.timingPoints[i].meter,
                                                        beatmap.timingPoints[i].sampleSet,
                                                        beatmap.timingPoints[i].sampleIndex,
                                                        volume,
                                                        false,
                                                        userInputData.isKiai ? 1 : beatmap.timingPoints[i].effect));
                    removeList.Add(i);
                }
                // 削除をする際に順番が変にならないようにソートする
                removeList.Sort();
                // 前から削除を行うと要素を詰めてしまうため、後ろから削除を行う
                for (global::System.Int32 i = (removeList.Count) - (1); i >= 0; i--)
                {
                    beatmap.timingPoints.RemoveAt(removeList[i]);
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
        /// 赤線に緑線を適用する
        /// </summary>
        /// <param name="userInputData">ユーザー入力値</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">適用する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyOnTimingPoints(UserInputData userInputData, Beatmap beatmap, ref List<TimingPoint> outTimingPoints)
        {
            double baseBpm = 120;
            double svPerMs = 0;
            double volumePerMs = 0;
            int offset = (userInputData.isOffset && userInputData.offsetMode == 1) ? userInputData.offset : 0;
            List<TimingPoint> timingPointsBuff = [];
            List<int> removeList = [];
            try
            {
                // 始点のBPMを求める
                var baseTimingPoint = beatmap.timingPoints.LastOrDefault(tp => (tp.time <= userInputData.timingFrom) && tp.isRedLine);
                baseBpm = baseTimingPoint != null ? baseTimingPoint.bpm : 120;
                if (userInputData.isSv)
                {
                    // 1msあたりのSVの変化量を求める
                    svPerMs = GetSvPerMs(userInputData.svFrom,
                                         userInputData.svTo,
                                         userInputData.timingFrom,
                                         userInputData.timingTo,
                                         userInputData.calculationCode);
                }
                if (userInputData.isVolume)
                {
                    // 1msあたりのVolumeの変化量を求める
                    volumePerMs = (double)(userInputData.volumeTo - userInputData.volumeFrom) /
                                  (double)(userInputData.timingTo - userInputData.timingFrom);
                }
                for (int i = 0; i < beatmap.hitObjects.Count; i++)
                {
                    // 緑線設定フラグ
                    bool isSetInheritedPoint = false;
                    if (beatmap.hitObjects[i].time < userInputData.timingFrom)
                    {
                        continue;
                    }
                    if (beatmap.hitObjects[i].time > userInputData.timingTo)
                    {
                        break;
                    }
                    // 始点に適用が無効化されている場合は何も処理をしない
                    if ((userInputData.setObjectOption.setObjectsCode == 0x0000037f) &&
                        !userInputData.setObjectOption.isTimingStart &&
                        (beatmap.hitObjects[i].time == userInputData.timingFrom))
                    {
                        continue;
                    }
                    // 終点に適用が無効化されている場合は何も処理をしない
                    if ((userInputData.setObjectOption.setObjectsCode == 0x0000037f) &&
                        !userInputData.setObjectOption.isTimingEnd &&
                        (beatmap.hitObjects[i].time == userInputData.timingTo))
                    {
                        continue;
                    }
                    var applyTimingPoint = beatmap.timingPoints.LastOrDefault(tp => (tp.time == beatmap.hitObjects[i].time) && tp.isRedLine);
                    double sv = 1;
                    int volume = 100;
                    TimingPoint? applyOutputInheritedPoint = new();
                    // ノーツと同じタイミングに赤線がある場合
                    if (applyTimingPoint != null)
                    {
                        // オフセット値が0の場合
                        if (offset == 0)
                        {

                            applyOutputInheritedPoint = outTimingPoints.LastOrDefault(tp => (tp.time == beatmap.hitObjects[i].time) && !tp.isRedLine);
                            // ノーツと同じタイミングに緑線がない場合
                            if (applyOutputInheritedPoint == null)
                            {
                                // 緑線設定フラグを有効にする
                                isSetInheritedPoint = true;
                            }
                            // ある場合は(同じタイミングに赤線,緑線,ノーツがある)
                            else
                            {
                                // 何もしない
                                continue;
                            }
                        }
                        // オフセット値が0以外の場合
                        else
                        {
                            // 緑線設定フラグを有効にする
                            isSetInheritedPoint = true;
                        }
                    }
                    // 緑線設定フラグが有効の場合
                    if (isSetInheritedPoint)
                    {
                        // 直前のTimingPointを探す
                        var applyInheritedPoint = beatmap.timingPoints.LastOrDefault(tp => tp.time <= beatmap.hitObjects[i].svApplyTime);
                        // 直前のTimingPointのインデックスを算出する
                        var applyInheritedPointIndex = beatmap.timingPoints.FindLastIndex(tp => tp.time <= beatmap.hitObjects[i].svApplyTime);
                        // オブジェクトコードを比較し、一致するものがある場合にSV,Volumeの計算を行う
                        if ((beatmap.hitObjects[i].hitObjectCode & userInputData.setObjectOption.setObjectsCode) != 0)
                        {
                            if (userInputData.isSv)
                            {
                                // 直前のSVを取得する
                                double baseSv = applyInheritedPoint != null ? applyInheritedPoint.sv : 1;
                                // SVを求める
                                sv = CalculateSv(userInputData.svFrom,
                                                 svPerMs,
                                                 userInputData.timingFrom,
                                                 beatmap.hitObjects[i].time,
                                                 baseSv,
                                                 userInputData.calculationCode,
                                                 userInputData.relativeCode,
                                                 userInputData.relativeBaseSv) *
                                                 (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / (applyTimingPoint != null ? applyTimingPoint.bpm : 120)) : 1);
                            }
                            else
                            {
                                // 直前のSVを参照する
                                sv = applyInheritedPoint != null ? applyInheritedPoint.sv : 1;
                            }
                        }
                        else
                        {
                            // 直前のSVを参照する
                            sv = applyInheritedPoint != null ? applyInheritedPoint.sv : 1;

                        }
                        if (userInputData.isVolume)
                        {
                            // 音量を求める
                            volume = applyTimingPoint != null ? (int)(CalculateVolume(userInputData.volumeFrom,
                                                                                      volumePerMs,
                                                                                      userInputData.timingFrom,
                                                                                      beatmap.hitObjects[i].time) + 0.5) : 100;
                        }
                        else
                        {
                            // 直前の音量を参照する
                            volume = applyInheritedPoint != null ? applyInheritedPoint.volume : 100;
                        }
                        if (applyTimingPoint != null)
                        {
                            outTimingPoints.Add(new TimingPoint(beatmap.hitObjects[i].time,
                                                                0,
                                                                sv,
                                                                0,
                                                                applyTimingPoint.meter,
                                                                applyTimingPoint.sampleSet,
                                                                applyTimingPoint.sampleIndex,
                                                                volume,
                                                                false,
                                                                userInputData.isKiai ? 1 : applyTimingPoint.effect));
                        }
                    }
                    else
                    {
                        continue;
                    }
                    var applyInheritedPointIndexes = beatmap.timingPoints.Select((tp, index) => new { tp, index }).
                                                                          Where(x => (x.tp.time <= beatmap.hitObjects[i].svApplyTime) && !x.tp.isRedLine).
                                                                          Select(x => x.index).
                                                                          ToList();
                    if (applyInheritedPointIndexes.Count != 0)
                    {
                        for (global::System.Int32 j = (applyInheritedPointIndexes.Count - 1); j >= 0; j--)
                        {
                            if (beatmap.timingPoints[applyInheritedPointIndexes[j]].time > beatmap.hitObjects[i - 1].time)
                            {
                                beatmap.timingPoints.RemoveAt(applyInheritedPointIndexes[j]);
                            }
                        }
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
        /// ユーザーが指定した範囲より後にあるスライダーの長さ調整をする
        /// </summary>
        /// <param name="userInputData">ユーザー入力値</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">適用された緑線</param>
        /// <param name="executeCode">実行コード</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool AdjustSliderLengthAfterExecute(UserInputData userInputData, Beatmap beatmap, List<TimingPoint> outTimingPoints, int executeCode)
        {
            // 削除処理をすると上手くスライダーの長さが調整されない
            try
            {
                // 指定範囲の直前のタイミングポイント
                TimingPoint? timingPointBeforeTimingFrom = new();
                // 終点の次のタイミングポイント
                TimingPoint? timingTo = new();
                // 指定範囲の始点
                int timingFrom = executeCode == Constants.EXECUTE_APPLY ?
                                 userInputData.timingTo : userInputData.timingFrom;
                // 順番を揃える為ソートする
                outTimingPoints = [.. outTimingPoints.OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0)];
                // 指定範囲の直前のタイミングポイントの取得
                timingPointBeforeTimingFrom = outTimingPoints.LastOrDefault(otp => otp.time <= timingFrom);
                // 終点の次のタイミングポイントの取得
                timingTo = beatmap.timingPoints.FirstOrDefault(tp => tp.time > userInputData.timingTo);

                if (timingPointBeforeTimingFrom == null)
                {
                    throw new Exception("");
                }
                for (global::System.Int32 i = 0; i < beatmap.hitObjects.Count; i++)
                {
                    // ノーツのタイミングが指定範囲より前だった場合は何も処理をしない
                    if (beatmap.hitObjects[i].time < timingFrom)
                    {
                        continue;
                    }
                    // ノーツのタイミングが指定範囲以降だった場合は処理を抜ける
                    if (timingTo != null && beatmap.hitObjects[i].time >= timingTo.time)
                    {
                        break;
                    }
                    // 直前のTimingPointを探す
                    var applyInheritedPoint = outTimingPoints.LastOrDefault(otp => otp.time <= beatmap.hitObjects[i].time);
                    if (applyInheritedPoint != null && beatmap.hitObjects[i].noteType == Constants.NoteType.SLIDER)
                    {
                        // ノーツがスライダーだった場合、スライダーの長さを調整する
                        beatmap.hitObjects[i].sliderLength = beatmap.hitObjects[i].sliderLength *
                                                             (applyInheritedPoint.sv / beatmap.hitObjects[i].sv);
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
        /// beatSnap間隔に緑線を適用する
        /// </summary>
        /// <param name="userInputData">ユーザー入力値</param>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="outTimingPoints">適用する緑線の格納先</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ApplyOnBeatSnaps(UserInputData userInputData, Beatmap beatmap, ref List<TimingPoint> outTimingPoints)
        {
            double svPerMs = 0;
            double volumePerMs = 0;
            double baseBpm = 120;
            try
            {
                List<TimingPoint> redLineList = beatmap.timingPoints.FindAll(tp => tp.isRedLine);
                double rawTimingFrom = BeatmapHelper.GetRawTiming(beatmap.timingPoints, userInputData.timingFrom);
                double timingOffset = 0;
                if (rawTimingFrom == double.MinValue)
                {
                    throw new Exception();
                }
                var applyTimingPoint = beatmap.timingPoints.LastOrDefault(tp => (tp.time <= rawTimingFrom) && tp.isRedLine) ?? throw new Exception();
                double currentTiming = applyTimingPoint.time;
                baseBpm = applyTimingPoint != null ? applyTimingPoint.bpm : 120;
                double beatSnapTiming = Constants.ONE_MINUTE / baseBpm / userInputData.setBeatSnapOption.beatSnap;
                // timingが指定されたビートスナップ間隔からどれぐらいズレてるか算出する
                while (true)
                {
                    if (currentTiming >= rawTimingFrom)
                    {
                        timingOffset = currentTiming - rawTimingFrom;
                        break;
                    }
                    currentTiming += beatSnapTiming;
                }
                if (userInputData.isSv)
                {
                    // 1msあたりのSVの変化量を求める
                    svPerMs = GetSvPerMs(userInputData.svFrom,
                                         userInputData.svTo,
                                         userInputData.timingFrom,
                                         userInputData.timingTo,
                                         userInputData.calculationCode);
                }
                if (userInputData.isVolume)
                {
                    // 1msあたりのVolumeの変化量を求める
                    volumePerMs = (double)(userInputData.volumeTo - userInputData.volumeFrom) /
                                  (double)(userInputData.timingTo - userInputData.timingFrom);
                }
                currentTiming = (applyTimingPoint != null ? applyTimingPoint.time : 120) + timingOffset;
                for (global::System.Int32 i = 0; i < redLineList.Count; i++)
                {
                    if (redLineList[i].time < applyTimingPoint?.time)
                    {
                        continue;
                    }
                    if (redLineList[i].time > userInputData.timingTo)
                    {
                        break;
                    }
                    for (int j = 0; ; j++)
                    {
                        // 始点に適用が無効化されている場合は何も処理をしない
                        if (!userInputData.setObjectOption.isTimingStart &&
                            ((int)currentTiming == userInputData.timingFrom))
                        {
                            currentTiming = redLineList[i].time + beatSnapTiming * (j + 1);
                            continue;
                        }
                        // 終点に適用が無効化されている場合は何も処理をしない
                        if (!userInputData.setObjectOption.isTimingEnd &&
                            ((int)currentTiming == userInputData.timingTo))
                        {
                            currentTiming = redLineList[i].time + beatSnapTiming * (j + 1);
                            break;
                        }
                        if ((int)currentTiming < userInputData.timingFrom)
                        {
                            currentTiming = redLineList[i].time + beatSnapTiming * (j + 1);
                            continue;
                        }
                        if (userInputData.timingTo < (int)currentTiming)
                        {
                            break;
                        }
                        double sv = 1;
                        int volume = 100;
                        var applyInheritedPoint = beatmap.timingPoints.LastOrDefault(tp => tp.time <= (int)currentTiming) ?? throw new Exception();
                        if (userInputData.isSv)
                        {
                            // 直前のSVを取得する
                            double baseSv = applyInheritedPoint != null ? applyInheritedPoint.sv : 1;
                            // SVを求める
                            sv = CalculateSv(userInputData.svFrom,
                                             svPerMs,
                                             userInputData.timingFrom,
                                             (int)currentTiming,
                                             baseSv,
                                             userInputData.calculationCode,
                                             userInputData.relativeCode,
                                             userInputData.relativeBaseSv) *
                                             (userInputData.relativeCode == Constants.RELATIVE_DISABLE ? (baseBpm / redLineList[i].bpm) : 1);
                        }
                        else
                        {
                            // 直前のSVを参照する
                            sv = applyInheritedPoint != null ? applyInheritedPoint.sv : 1;
                        }
                        if (userInputData.isVolume)
                        {
                            // 音量を求める
                            volume = applyTimingPoint != null ? (int)(CalculateVolume(userInputData.volumeFrom,
                                                                                      volumePerMs,
                                                                                      userInputData.timingFrom,
                                                                                      (int)currentTiming) + 0.5) : 100;
                        }
                        else
                        {
                            // 直前の音量を参照する
                            volume = applyInheritedPoint != null ? applyInheritedPoint.volume : 100;
                        }
                        if (applyInheritedPoint == null)
                        {
                            return false;
                        }
                        outTimingPoints.Add(new TimingPoint((int)currentTiming,
                                                            applyInheritedPoint.bpm,
                                                            sv,
                                                            applyInheritedPoint.barLength,
                                                            applyInheritedPoint.meter,
                                                            applyInheritedPoint.sampleSet,
                                                            applyInheritedPoint.sampleIndex,
                                                            volume,
                                                            false,
                                                            userInputData.isKiai ? 1 : applyInheritedPoint.effect));
                        currentTiming = redLineList[i].time + beatSnapTiming * (j + 1);
                        if (i != redLineList.Count - 1)
                        {
                            var currentIndex = 0;
                            if (currentTiming + 1 > redLineList[i + 1].time)
                            {
                                currentIndex = i;
                                if (i != redLineList.Count - 2)
                                {
                                    if (redLineList[i + 2].time - redLineList[i + 1].time < 10)
                                    {
                                        i++;
                                    }
                                }
                                if (applyTimingPoint != null)
                                {
                                    currentTiming = redLineList[i + 1].time +
                                                    timingOffset *
                                                    (applyTimingPoint.bpm /
                                                     redLineList[i + 1].bpm);
                                    beatSnapTiming = Constants.ONE_MINUTE / redLineList[i + 1].bpm / userInputData.setBeatSnapOption.beatSnap;
                                }
                                break;
                            }
                        }
                    }
                }
                // 元から設定されている緑線の削除
                if (!RemoveInferitedPoint(userInputData, beatmap, outTimingPoints))
                {
                    throw new Exception();
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
        /// 計算コードに応じて1msあたりのSVを計算する
        /// </summary>
        /// <param name="svFrom">SV(始点)</param>
        /// <param name="svTo">SV(終点)</param>
        /// <param name="timingFrom">Timing(始点)</param>
        /// <param name="timingTo">Timing(終点)</param>
        /// <param name="calculationCode">計算コード</param>
        /// <returns>1msあたりのSVを返す</returns>
        /// <exception cref="ArgumentException">計算コードが不正</exception>
        private static double GetSvPerMs(double svFrom,
                                         double svTo,
                                         int timingFrom,
                                         int timingTo,
                                         int calculationCode)
        {
            switch (calculationCode)
            {
                case Constants.CALCULATION_ARITHMETIC:
                    return (svTo - svFrom) / (timingTo - timingFrom);
                case Constants.CALCULATION_GEOMETRIC:
                    return Math.Pow(svTo / svFrom, 1.0 / (timingTo - timingFrom));
                default:
                    throw new ArgumentException("Invalid calculation code");
            }
        }
        /// <summary>
        /// 算出した1msあたりのSVを元に、指定されたタイミングのSVを計算する
        /// </summary>
        /// <param name="svFrom">SV(始点)</param>
        /// <param name="svPerMs">1msあたりのSV</param>
        /// <param name="timingFrom">Timing(始点)</param>
        /// <param name="currentTiming">現地点のTiming</param>
        /// <param name="calculationCode">計算コード</param>
        /// <returns>算出したSV</returns>
        /// <exception cref="ArgumentException">計算コードが不正</exception>
        private static double CalculateSv(double svFrom,
                                          double svPerMs,
                                          int timingFrom,
                                          int currentTiming,
                                          double baseSv,
                                          int calculationCode,
                                          int relativeCode,
                                          double relativeBaseSv)
        {
            double currentSv = 0;
            switch (calculationCode)
            {
                case Constants.CALCULATION_ARITHMETIC:
                    currentSv = svFrom + (svPerMs * (currentTiming - timingFrom));
                    break;
                case Constants.CALCULATION_GEOMETRIC:
                    currentSv = svFrom * Math.Pow(svPerMs, (double)(currentTiming - timingFrom));
                    break;
                default:
                    throw new ArgumentException("Invalid calculation code");
            }
            switch (relativeCode)
            {
                case Constants.RELATIVE_DISABLE:
                    return currentSv;
                case Constants.RELATIVE_MULTIPLY:
                    return (baseSv - relativeBaseSv) * currentSv + relativeBaseSv;
                case Constants.RELATIVE_SUM:
                    return baseSv + currentSv;
                default:
                    throw new ArgumentException("Invalid relative code");
            }
        }
        /// <summary>
        /// 算出した1msあたりのVolumeを元に、指定されたタイミングのVolumeを計算する
        /// </summary>
        /// <param name="volumeFrom">Volume(始点)</param>
        /// <param name="volumePerMs">1msあたりのVolume</param>
        /// <param name="timingFrom">Timing(始点)</param>
        /// <param name="currentTiming">現地点のTiming</param>
        /// <returns>算出したVolume</returns>
        private static double CalculateVolume(double volumeFrom,
                                              double volumePerMs,
                                              int timingFrom,
                                              int currentTiming)
        {
            return volumeFrom + (volumePerMs * (currentTiming - timingFrom));
        }
        private static double GetOffsetTiming(UserInputData userInputData,
                                              TimingPoint timingPoint,
                                              HitObject hitObject,
                                              HitObject? prevHitObject,
                                              HitObject? nextHitObject)
        {
            double hexaOffset = Constants.ONE_MINUTE / timingPoint.bpm / Constants.HEXA_SNAP;
            double duoOffset = Constants.ONE_MINUTE / timingPoint.bpm / Constants.DUO_SNAP;
            if (userInputData.isDuoOffset)
            {
                if (hitObject.snap != Constants.DUO_SNAP)
                {
                    double intervalTri = Constants.ONE_MINUTE / timingPoint.bpm / Constants.TRI_SNAP;

                    bool isOddAdjacent = (prevHitObject != null && prevHitObject.time >= hitObject.time - intervalTri - Constants.MILLISECOND_TOLERANCE && prevHitObject.snap == Constants.DUO_SNAP)
                                       || (nextHitObject != null && nextHitObject.time <= hitObject.time + intervalTri + Constants.MILLISECOND_TOLERANCE && nextHitObject.snap == Constants.DUO_SNAP);

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
    }
}