using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Models;

namespace osu_taiko_Mapping_Helper.Utils.Helper
{
    internal class UserInputUtilityDataHelper
    {

        internal static bool SetOffsetData(UserInputUtilityData userInputUtilityData, string offset)
        {
            try
            {
                int retOffset = 0;
                if (offset == string.Empty)
                {
                    //オフセットの入力がない
                    offset = "0";
                }
                if (!int.TryParse(offset, out retOffset))
                {
                    //オフセットのフォーマットが間違えている
                    Common.ShowMessageDialog("E_V-T-003");
                    return false;
                }
                // ユーザー入力データにオフセット情報をセットする
                userInputUtilityData.offset = retOffset;
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
        /// リスナップタイミングの入力値をユーザー入力データにセットする処理<br/>
        /// </summary>
        /// <param name="userInputUtilityData">入力データ</param>
        /// <param name="timingFrom">始点</param>
        /// <param name="timingTo">終点</param>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        internal static int SetResnapTimingData(UserInputUtilityData userInputUtilityData,
                                                string timingFrom,
                                                string timingTo)
        {
            try
            {
                int retTimingFrom = 0;
                int retTimingTo = 0;
                if ((timingFrom == string.Empty) && (timingTo == string.Empty))
                {
                    //タイミングの入力がない
                    if (Common.ShowMessageDialog("I_A-P-005", Constants.DIALOG_OPTION_OKCANCEL))
                    {
                        userInputUtilityData.isAllNotes = true;
                        return 1;
                    }
                    else
                    {
                        return 0;
                    }
                }
                if ((timingFrom == string.Empty) || (timingTo == string.Empty))
                {
                    //タイミングの入力がない
                    Common.ShowMessageDialog("E_V-EM-001");
                    return -1;
                }
                if (!Common.ConvertMsTiming(timingFrom, ref retTimingFrom) ||
                    !Common.ConvertMsTiming(timingTo, ref retTimingTo))
                {
                    //タイミングのフォーマットが間違えている
                    Common.ShowMessageDialog("E_V-C-001");
                    return -1;
                }
                if (retTimingFrom > retTimingTo)
                {
                    //タイミングの開始位置が終了位置より大きい
                    Common.ShowMessageDialog("E_V-C-002");
                    return -1;
                }
                // ユーザー入力データにタイミング情報をセットする
                userInputUtilityData.resnapTimingFrom = retTimingFrom;
                userInputUtilityData.resnapTimingTo = retTimingTo;
                return 1;
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return -1;
            }
        }
    }
}
