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
        /// ユーザーが入力したデータをXML形式でシリアライズする関数
        /// </summary>
        /// <param name="userInputData">入力データ</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool SerializeUserInputData(UserInputData userInputData)
        {
            try
            {
                DateTime date = DateTime.Now;
                // シリアライザーの作成
                XmlSerializer serializer = new(userInputData.GetType());
                using (var sw = new StreamWriter(Directory.GetCurrentDirectory() + "\\" +
                                                 Properties.Constants.HISTORY_DIRECTORY + "\\" +
                                                 "\\history_" +
                                                 userInputData.createDate.ToString("yyyyMMddHHmmssfff") +
                                                 Properties.Constants.XML_EXTENSION,
                                                 false,
                                                 new System.Text.UTF8Encoding(false))) // BOMなしUTF-8
                {
                    // historyディレクトリにシリアライズしたファイルを作成する
                    serializer.Serialize(sw, userInputData);
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-SERIALIZE-XML");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
        /// <summary>
        /// ユーザーが入力したデータをXML形式からデシリアライズする関数
        /// </summary>
        /// <param name="userInputData">格納先のデータ</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static bool DeserializeUserInputData(ref List<UserInputData> userInputData)
        {
            try
            {
                // 履歴ファイルを探す
                string[] files = Directory.GetFiles(Directory.GetCurrentDirectory() + "\\" +
                                                    Properties.Constants.HISTORY_DIRECTORY, "history_*.xml");
                DateTime date = DateTime.Now;
                // シリアライザーの作成
                XmlSerializer serializer = new(typeof(UserInputData));
                // 履歴ファイルが見つからない場合は
                if (files.Length == 0)
                {
                    return false;
                }
                foreach (var file in files)
                {
                    // 履歴をデシリアライズし、入力値リストに格納する
                    using (var sw = new StreamReader(file,
                                                     new System.Text.UTF8Encoding(false)))
                    {
                        UserInputData? tempUserInputData = (UserInputData?)serializer?.Deserialize(sw);
                        if (tempUserInputData != null)
                        {
                            userInputData?.Add(tempUserInputData);
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-DESERIALIZE-XML");
                Common.WriteExceptionMessage(ex);
                return false;
            }
        }
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
                decimal retBaseSv = -1m;
                if (userInputTempData.isRelative)
                {
                    if (userInputTempData.isRelativeMultiply)
                    {
                        userInputData.relativeCode = Constants.RELATIVE_MULTIPLY;
                        userInputData.relativeBaseSv = decimal.Parse(userInputTempData.relativeMultiplyBaseSv);
                        if (userInputTempData.relativeMultiplyBaseSv == string.Empty)
                        {
                            //基準SVの入力がない
                            Common.ShowMessageDialog("E_V-EM-002");
                            return false;
                        }
                        if (!decimal.TryParse(userInputTempData.relativeMultiplyBaseSv, out retBaseSv))
                        {
                            //SVのフォーマットが間違えている
                            Common.ShowMessageDialog("E_V-T-001");
                            return false;
                        }
                        if (retBaseSv < 0)
                        {
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
                if (!userInputTempData.isArithmetic && !userInputTempData.isGeometric)
                {
                    //計算方法が指定されていない
                    Common.ShowMessageDialog("E_V-EM-005");
                    return false;
                }
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
                if ((userInputData.calculationCode == 0) && userInputTempData.isSv)
                {
                    //計算方法が指定されていない
                    Common.ShowMessageDialog("E_V-EM-005");
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
                    userInputData.setObjectOption.setObjectsCode = 0x0000017f;
                }
                else if (userInputTempData.setObjectOption.isOnlyBarlines)
                {
                    if (userInputTempData.setObjectOption.isOnBarlines)
                    {
                        userInputData.setObjectOption.setObjectsCode = 0x00000100;
                    }
                    else if (userInputTempData.setObjectOption.isOffBarlines)
                    {
                        userInputData.setObjectOption.setObjectsCode = 0x00000080;
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
                        userInputData.setObjectOption.setObjectsCode = 0x00000400;
                    }
                    else if (userInputTempData.setObjectOption.isOffBarlines)
                    {
                        userInputData.setObjectOption.setObjectsCode = 0x00000200;
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
                        userInputData.setObjectOption.setObjectsCode += 0x0000007f;
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
                    //タイミングの入力がない
                    Common.ShowMessageDialog("E_V-EM-001");
                    return false;
                }
                if (!Common.ConvertMsTiming(userInputTempData.timingFrom, ref retTimingFrom) ||
                    !Common.ConvertMsTiming(userInputTempData.timingTo, ref retTimingTo))
                {
                    //タイミングのフォーマットが間違えている
                    Common.ShowMessageDialog("E_V-C-001");
                    return false;
                }
                if (retTimingFrom > retTimingTo)
                {
                    //タイミングの開始位置が終了位置より大きい
                    Common.ShowMessageDialog("E_V-C-002");
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
                decimal retSvFrom = -1m;
                decimal retSvTo = -1m;
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
                    //SVの入力がない
                    Common.ShowMessageDialog("E_V-EM-002");
                    return false;
                }
                if (!decimal.TryParse(tempSvFrom, out retSvFrom) ||
                    !decimal.TryParse(tempSvTo, out retSvTo))
                {
                    //SVのフォーマットが間違えている
                    Common.ShowMessageDialog("E_V-T-001");
                    return false;
                }
                switch (userInputData.relativeCode)
                {
                    case Constants.RELATIVE_DISABLE:
                        // 相対速度変化オプションが無効の場合は
                        // SVがosu側で指定できる範囲内かチェックする
                        if (((retSvFrom < 0.01m) || (retSvFrom > 10m) || (retSvTo < 0.01m) || (retSvTo > 10m)))
                        {
                            retSvFrom = -1m;
                            retSvTo = -1m;
                            // SVがosu側で指定できる範囲外の値
                            Common.ShowMessageDialog("E_V-C-003");
                            return false;
                        }
                        break;
                    case Constants.RELATIVE_SUM:
                        // 相対速度変化オプションが加算の場合は
                        // SVがosu側で指定できる範囲内かチェックする
                        if (((retSvFrom <= -10m) || (retSvFrom >= 10m) || (retSvTo <= -10m) || (retSvTo >= 10m)))
                        {
                            retSvFrom = -1m;
                            retSvTo = -1m;
                            // SVがosu側で指定できる範囲外の値
                            Common.ShowMessageDialog("E_V-C-003");
                            return false;
                        }
                        break;
                    case Constants.RELATIVE_MULTIPLY:
                        // 相対速度変化オプションが乗算の場合は
                        // 基準SVが0かつ倍率が0かチェックする
                        if (((retSvFrom == 0m) || (retSvTo == 0m)) &&
                            userInputData.relativeBaseSv == 0m)
                        {
                            retSvFrom = -1m;
                            retSvTo = -1m;
                            // SVがosu側で指定できる範囲外の値
                            Common.ShowMessageDialog("E_V-C-003");
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
                    //Volumeの入力がない
                    Common.ShowMessageDialog("E_V-EM-003");
                    return false;
                }
                if (!int.TryParse(userInputTempData.volumeFrom, out retVolumeFrom) ||
                    !int.TryParse(userInputTempData.volumeTo, out retVolumeTo))
                {
                    //Volumeのフォーマットが間違えている
                    Common.ShowMessageDialog("E_V-T-002");
                    return false;
                }
                if ((retVolumeFrom < 5) || (retVolumeFrom > 100) ||
                    (retVolumeTo < 5) || (retVolumeTo > 100))
                {
                    retVolumeFrom = -1;
                    retVolumeTo = -1;
                    //Volumeがosu側で指定できる範囲外の値
                    Common.ShowMessageDialog("E_V-C-006");
                    return false;
                }
                userInputData.volumeFrom = retVolumeFrom;
                userInputData.volumeTo = retVolumeTo;
                // osu側で指定できるVolumeの範囲内の場合はバリデーションチェックを完了する
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
                if (!int.TryParse(userInputTempData.offset, out retOffset))
                {
                    //Offsetのフォーマットが間違えている
                    Common.ShowMessageDialog("E_V-T-003");
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
                userInputData.setBeatSnapOption.isBeatSnap = userInputTempData.setBeatSnapOption.isBeatSnap;
                if (!userInputData.setBeatSnapOption.isBeatSnap)
                {
                    retBeatSnap = -1;
                    return true;
                }
                if (userInputTempData.setBeatSnapOption.beatSnap == string.Empty)
                {
                    //ビートスナップ間隔が空欄
                    Common.ShowMessageDialog("E_V-EM-004");
                    return false;
                }
                if (!int.TryParse(userInputTempData.setBeatSnapOption.beatSnap, out retBeatSnap))
                {
                    //ビートスナップ間隔が整数ではない
                    Common.ShowMessageDialog("E_V-T-004");
                    return false;
                }
                if (retBeatSnap <= 0)
                {
                    //ビートスナップ間隔が0以下の整数
                    Common.ShowMessageDialog("E_V-T-004");
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
        internal static string SetCurrentSv(Beatmap? beatmap, int currentTime)
        {
            TimingPoint timingPointBuff = new();
            decimal retSv = 0;
            string retSvStr = string.Empty;
            try
            {
                if (beatmap == null)
                {
                    throw new Exception();
                }
                // 現在のタイミングのSVを取得する (小数点第15位まで)
                retSv = Math.Round(beatmap.timingPoints.LastOrDefault(tp => tp.time <= currentTime)?.sv ?? -1, 15, MidpointRounding.AwayFromZero);
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
