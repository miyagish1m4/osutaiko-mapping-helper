using System.Reflection.Metadata.Ecma335;
using System.Xml.Serialization;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;

namespace osu_taiko_Mapping_Helper.Utils.Helper
{
    /// <summary>
    /// ユーザーが入力したデータを扱うクラス
    /// </summary>
    class UserInputDataHelper
    {
        /// <summary>
        /// 相対速度変化コードを設定する関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool SetRelativeOption(UserInputTempData userInputTempData,
                                              ref UserInputData userInputData)
        {
            try
            {
                double retBaseSv = -1;
                if (userInputTempData.isRelative)
                {
                    if (userInputTempData.isRelativeMultiply)
                    {
                        userInputData.relativeCode = Constants.RELATIVE_MULTIPLY;
                        if (userInputTempData.relativeMultiplyBaseSv == string.Empty)
                        {
                            //基準SVが指定されていない
                            Common.ShowMessageDialog("E_V-EM-7");
                            return false;
                        }
                        if (!double.TryParse(userInputTempData.relativeMultiplyBaseSv, out retBaseSv))
                        {
                            //基準SVが数値で指定されていない
                            Common.ShowMessageDialog("E_V-T-11");
                            return false;
                        }
                        if (retBaseSv < 0)
                        {
                            //基準SVが負の値で指定されている
                            Common.ShowMessageDialog("E_V-T-12");
                            return false;
                        }
                        userInputData.relativeBaseSv = retBaseSv;
                    }
                    else if (userInputTempData.isRelativeSum)
                    {
                        userInputData.relativeCode = Constants.RELATIVE_SUM;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    userInputData.relativeCode = Constants.RELATIVE_DISABLE;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 計算コードを設定する関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool SetCalculationCode(UserInputTempData userInputTempData,
                                               ref UserInputData userInputData)
        {
            try
            {
                if (userInputTempData.isRelative)
                {
                    userInputData.calculationCode = Constants.CALCULATION_ARITHMETIC;
                }
                else
                {
                    if (userInputTempData.isArithmetic)
                    {
                        userInputData.calculationCode = Constants.CALCULATION_ARITHMETIC;
                    }
                    else if (userInputTempData.isGeometric)
                    {
                        userInputData.calculationCode = Constants.CALCULATION_GEOMETRIC;
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
        /// kiai設定を設定する関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool SetKiaiOption(UserInputTempData userInputTempData,
                                          ref UserInputData userInputData)
        {
            try
            {
                userInputData.isKiai = userInputTempData.isKiai;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 適用設定を設定する関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool SetApplyOption(UserInputTempData userInputTempData,
                                           ref UserInputData userInputData)
        {
            try
            {
                userInputData.setOption.isSetObjects = userInputTempData.setOption.isSetObjects;
                userInputData.setOption.isSetBeatSnap = userInputTempData.setOption.isSetBeatSnap;
                userInputData.setOption.isSetGreenLine = userInputTempData.setOption.isSetGreenLine;
                userInputData.setObjectOption.isTimingStart = userInputTempData.isEnableFrom;
                userInputData.setObjectOption.isTimingEnd = userInputTempData.isEnableTo;
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 適用対象となるオブジェクトコードを設定する関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool SetObjectCode(UserInputTempData userInputTempData,
                                          ref UserInputData userInputData)
        {
            try
            {
                if (userInputTempData.setObjectOption.isAllHitObjects)
                {
                    userInputData.setObjectOption.setObjectsCode = 0x0000037f;
                }
                else if (userInputTempData.setObjectOption.isOnlyBarlines)
                {
                    if (userInputTempData.setObjectOption.isOnBarlines)
                    {
                        userInputData.setObjectOption.setObjectsCode = 0x00000200;
                    }
                    else if (userInputTempData.setObjectOption.isOffBarlines)
                    {
                        userInputData.setObjectOption.setObjectsCode = 0x00000100;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (userInputTempData.setObjectOption.isOnlyBookmarks)
                {
                    if (userInputTempData.setObjectOption.isOnBarlines)
                    {
                        userInputData.setObjectOption.setObjectsCode = 0x00000800;
                    }
                    else if (userInputTempData.setObjectOption.isOffBarlines)
                    {
                        userInputData.setObjectOption.setObjectsCode = 0x00000400;
                    }
                    else
                    {
                        return false;
                    }
                }
                else if (userInputTempData.setObjectOption.isOnlyHitObjects)
                {
                    if (userInputTempData.setObjectOption.isOnlyNormalDong)
                    {
                        userInputData.setObjectOption.setObjectsCode += 0x00000001;
                    }
                    if (userInputTempData.setObjectOption.isOnlyFinisherDong)
                    {
                        userInputData.setObjectOption.setObjectsCode += 0x00000002;
                    }
                    if (userInputTempData.setObjectOption.isOnlyNormalKa)
                    {
                        userInputData.setObjectOption.setObjectsCode += 0x00000004;
                    }
                    if (userInputTempData.setObjectOption.isOnlyFinisherKa)
                    {
                        userInputData.setObjectOption.setObjectsCode += 0x00000008;
                    }
                    if (userInputTempData.setObjectOption.isOnlyNormalSlider)
                    {
                        userInputData.setObjectOption.setObjectsCode += 0x00000010;
                    }
                    if (userInputTempData.setObjectOption.isOnlyFinisherSlider)
                    {
                        userInputData.setObjectOption.setObjectsCode += 0x00000020;
                    }
                    if (userInputTempData.setObjectOption.isOnlySpinner)
                    {
                        userInputData.setObjectOption.setObjectsCode += 0x00000040;
                    }
                    if (userInputData.setObjectOption.setObjectsCode == 0x00000000)
                    {
                        userInputData.setObjectOption.setObjectsCode += 0x0000037f;
                    }
                    return true;
                }
                else
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// Timingのバリデーションチェックをする関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ValidateTiming(UserInputTempData userInputTempData,
                                           ref UserInputData userInputData)
        {
            try
            {
                int retTimingFrom = 0;
                int retTimingTo = 0;
                if ((userInputTempData.timingFrom == string.Empty) || (userInputTempData.timingTo == string.Empty))
                {
                    //タイミングが指定されていない
                    Common.ShowMessageDialog("E_V-EM-4");
                    return false;
                }
                if (!Common.ConvertMsTiming(userInputTempData.timingFrom, ref retTimingFrom) ||
                    !Common.ConvertMsTiming(userInputTempData.timingTo, ref retTimingTo))
                {
                    //タイミングのフォーマットが間違えている
                    Common.ShowMessageDialog("E_V-C-4");
                    return false;
                }
                if (retTimingFrom > retTimingTo)
                {
                    //タイミングの開始位置が終了位置より大きい
                    Common.ShowMessageDialog("E_V-C-5");
                    return false;
                }
                // ユーザー入力データにタイミング情報をセットする
                userInputData.timingFrom = retTimingFrom;
                userInputData.timingTo = retTimingTo;
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
        /// SVのバリデーションチェックをする関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ValidateSv(UserInputTempData userInputTempData,
                                       ref UserInputData userInputData)
        {
            try
            {
                string tempSvFrom = string.Empty;
                string tempSvTo = string.Empty;
                double retSvFrom = -1;
                double retSvTo = -1;
                userInputData.isSv = userInputTempData.isSv;
                if (!userInputData.isSv)
                {
                    //SV有効化フラグが有効ではない
                    retSvFrom = -1;
                    retSvTo = -1;
                    return true;
                }
                if (userInputTempData.isRelative)
                {
                    switch (userInputData.relativeCode)
                    {
                        case Constants.RELATIVE_MULTIPLY:
                            tempSvFrom = userInputTempData.relativeMultiplySvFrom;
                            tempSvTo = userInputTempData.relativeMultiplySvTo;
                            break;
                        case Constants.RELATIVE_SUM:
                            tempSvFrom = userInputTempData.relativeSumSvFrom;
                            tempSvTo = userInputTempData.relativeSumSvTo;
                            break;
                        default:
                            // 相対速度変化オプションが無効の場合はここに来ないはず
                            return false;
                    }
                    if (!userInputTempData.isEnableRelativeEnd)
                    {
                        tempSvTo = tempSvFrom;
                    }
                }
                else
                {
                    tempSvFrom = userInputTempData.svFrom;
                    tempSvTo = userInputTempData.svTo;
                }
                if (((tempSvFrom == string.Empty) || (tempSvTo == string.Empty)))
                {
                    //SVが指定されていない
                    Common.ShowMessageDialog("E_V-EM-5");
                    return false;
                }
                if (!double.TryParse(tempSvFrom, out retSvFrom) ||
                    !double.TryParse(tempSvTo, out retSvTo))
                {
                    //SVが数値で指定されていない
                    Common.ShowMessageDialog("E_V-T-7");
                    return false;
                }
                switch (userInputData.relativeCode)
                {
                    case Constants.RELATIVE_DISABLE:
                        if ((retSvFrom <= 0) || (retSvTo <= 0))
                        {
                            //SVが0が指定されている
                            Common.ShowMessageDialog("E_V-T-8");
                            return false;
                        }
                        break;
                    case Constants.RELATIVE_SUM:
                        break;
                    case Constants.RELATIVE_MULTIPLY:
                        // 相対速度変化オプションが乗算の場合は
                        // 基準SVが0かつ倍率が0かチェックする
                        if (((retSvFrom == 0) || (retSvTo == 0)) &&
                            userInputData.relativeBaseSv == 0)
                        {
                            retSvFrom = -1;
                            retSvTo = -1;
                            // SVが0が指定されている
                            Common.ShowMessageDialog("E_V-T-8");
                            return false;
                        }
                        break;
                }
                userInputData.svFrom = retSvFrom;
                userInputData.svTo = retSvTo;
                return true;
            }
            catch
            {
                return false;
            }

        }
        /// <summary>
        /// Volumeのバリデーションチェックをする関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ValidateVolume(UserInputTempData userInputTempData,
                                           ref UserInputData userInputData)
        {
            try
            {
                int retVolumeFrom = -1;
                int retVolumeTo = -1;
                userInputData.isVolume = userInputTempData.isVolume;
                if (!userInputData.isVolume)
                {
                    //Volume有効化フラグが有効ではない
                    retVolumeFrom = -1;
                    retVolumeTo = -1;
                    return true;
                }
                if ((userInputTempData.volumeFrom == string.Empty) ||
                    (userInputTempData.volumeTo == string.Empty))
                {
                    //Volumeが指定されていない
                    Common.ShowMessageDialog("E_V-EM-6");
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(userInputTempData.volumeFrom, @"^[-+]?[0-9]*\.?[0-9]+$") ||
                    !System.Text.RegularExpressions.Regex.IsMatch(userInputTempData.volumeTo, @"^[-+]?[0-9]*\.?[0-9]+$"))
                {
                    //Volumeが数値で指定されていない
                    Common.ShowMessageDialog("E_V-T-9");
                    return false;
                }
                if (!int.TryParse(userInputTempData.volumeFrom, out retVolumeFrom) ||
                    !int.TryParse(userInputTempData.volumeTo, out retVolumeTo))
                {
                    //Volumeが整数で指定されていない
                    Common.ShowMessageDialog("E_V-T-10");
                    return false;
                }
                if ((retVolumeFrom < 0) || (retVolumeFrom > 100) ||
                    (retVolumeTo < 0) || (retVolumeTo > 100))
                {
                    retVolumeFrom = -1;
                    retVolumeTo = -1;
                    //Volumeが0から100の範囲で指定されていない
                    Common.ShowMessageDialog("E_V-C-6");
                    return false;
                }
                userInputData.volumeFrom = retVolumeFrom;
                userInputData.volumeTo = retVolumeTo;
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
        /// Offsetのバリデーションチェックをする関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ValidateOffset(UserInputTempData userInputTempData,
                                           ref UserInputData userInputData)
        {
            try
            {
                int retOffset = 0;
                userInputData.isOffset = userInputTempData.isOffset;
                if (!userInputData.isOffset)
                {
                    //Offset有効化フラグが有効ではない
                    retOffset = 0;
                    return true;
                }
                if (userInputTempData.offset == string.Empty)
                {
                    //Offsetが指定されていない
                    retOffset = 0;
                    return true;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(userInputTempData.offset, @"^[-+]?[0-9]*\.?[0-9]+$"))
                {
                    //Offsetが数値で指定されていない
                    Common.ShowMessageDialog("E_V-T-15");
                    return false;
                }
                if (!int.TryParse(userInputTempData.offset, out retOffset))
                {
                    //Offsetが整数で指定されていない
                    Common.ShowMessageDialog("E_V-T-16");
                    return false;
                }
                userInputData.offset = retOffset;
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
        /// BeatSnapのバリデーションチェックをする関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private static bool ValidateBeatSnap(UserInputTempData userInputTempData,
                                             ref UserInputData userInputData)
        {
            try
            {
                if (!userInputData.setOption.isSetBeatSnap)
                {
                    return true;
                }
                int retBeatSnap = -1;
                if (userInputTempData.setBeatSnapOption.beatSnap == string.Empty)
                {
                    //ビートスナップ間隔が指定されていない
                    Common.ShowMessageDialog("E_V-EM-8");
                    return false;
                }
                if (!System.Text.RegularExpressions.Regex.IsMatch(userInputTempData.setBeatSnapOption.beatSnap, @"^[-+]?[0-9]*\.?[0-9]+$"))
                {
                    //ビートスナップ間隔が数値で指定されていない
                    Common.ShowMessageDialog("E_V-T-13");
                    return false;
                }
                if (!int.TryParse(userInputTempData.setBeatSnapOption.beatSnap, out retBeatSnap) || retBeatSnap <= 0)
                {
                    //ビートスナップ間隔が自然数で指定されていない
                    Common.ShowMessageDialog("E_V-T-14");
                    return false;
                }
                userInputData.setBeatSnapOption.beatSnap = retBeatSnap;
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
        /// 入力値を検証し、UserInputDataに格納する関数
        /// </summary>
        /// <param name="userInputTempData">入力値(一時保存用)</param>
        /// <param name="executeCode">実行コード</param>
        /// <param name="userInputData">入力値</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool SetUserInputData(UserInputTempData userInputTempData,
                                              int executeCode,
                                              ref UserInputData userInputData)
        {
            try
            {
                if (!ValidateTiming(userInputTempData, ref userInputData))
                {
                    throw new Exception("タイミング情報の取得に失敗しました。");
                }
                if (!ValidateOffset(userInputTempData, ref userInputData))
                {
                    throw new Exception("Offsetの取得に失敗しました。");
                }
                switch (executeCode)
                {
                    case Properties.Constants.EXECUTE_APPLY:
                        //適用時
                        if (!SetApplyOption(userInputTempData, ref userInputData))
                        {
                            throw new Exception("適用オプションの設定に失敗しました。");
                        }
                        if (!SetCalculationCode(userInputTempData, ref userInputData))
                        {
                            throw new Exception("計算方法の設定に失敗しました。");
                        }
                        if (!SetRelativeOption(userInputTempData, ref userInputData))
                        {
                            throw new Exception("相対速度変化オプションの設定に失敗しました。");
                        }
                        if (!SetObjectCode(userInputTempData, ref userInputData))
                        {
                            throw new Exception("オブジェクトコードの設定に失敗しました。");
                        }
                        if (!ValidateSv(userInputTempData, ref userInputData))
                        {
                            throw new Exception("SVの取得に失敗しました。");
                        }
                        if (!ValidateVolume(userInputTempData, ref userInputData))
                        {
                            throw new Exception("Volumeの取得に失敗しました。");
                        }
                        if (!ValidateBeatSnap(userInputTempData, ref userInputData))
                        {
                            throw new Exception("BeatSnap間隔の取得に失敗しました。");
                        }
                        if (!SetKiaiOption(userInputTempData, ref userInputData))
                        {
                            throw new Exception("Kiai設定の取得に失敗しました。");
                        }
                        break;
                    case Properties.Constants.EXECUTE_REMOVE:
                        break;
                    default:
                        throw new Exception("不明な実行コードが指定されました。");
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
        /// 現地点のSVを取得する関数
        /// </summary>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="currentTime">タイミング</param>
        /// <returns>SVの値</returns>
        internal static string SetCurrentSv(Beatmap? beatmap, int currentTime, bool isDebug = false)
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
                // 現在のタイミングのSVを取得する (小数点第15位まで)
                retSv = Math.Round(beatmap.timingPoints.LastOrDefault(tp => tp.time <= currentTime)?.sv ?? -1, isDebug ? 8 : 12, MidpointRounding.AwayFromZero);
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
        /// <summary>
        /// 現地点のVolumeを取得する関数
        /// </summary>
        /// <param name="beatmap">譜面情報</param>
        /// <param name="currentTime">タイミング</param>
        /// <returns>Volumeの値</returns>
        internal static string SetCurrentVolume(Beatmap? beatmap, int currentTime)
        {
            TimingPoint timingPointBuff = new();
            int retVolume = 0;
            try
            {
                if (beatmap == null)
                {
                    throw new Exception();
                }
                // 現在のタイミングのVolumeを取得する
                retVolume = beatmap.timingPoints.LastOrDefault(tp => tp.time <= currentTime)?.volume ?? -1;
                if (retVolume == -1)
                {
                    throw new Exception();
                }
                return retVolume.ToString();
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
