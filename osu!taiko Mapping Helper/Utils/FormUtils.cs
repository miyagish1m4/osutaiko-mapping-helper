using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Views;

namespace osu_taiko_Mapping_Helper.Utils
{
    internal class FormUtils
    {
        #region MainForm
        /// <summary>
        /// SV始点テキストボックスの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="userInputTempData">ユーザー入力の一時データ</param>
        /// <param name="txtSvFrom">SV始点テキストボックス</param>
        internal static void SetTxtSvFromEnabledState(bool isEnable,
                                                      UserInputTempData userInputTempData,
                                                      TextBox txtSvFrom)
        {
            if (isEnable)
            {
                if (userInputTempData.isRelative)
                {
                    if (userInputTempData.isRelativeMultiply)
                    {
                        txtSvFrom.Text = userInputTempData.relativeMultiplySvFrom;
                    }
                    else if (userInputTempData.isRelativeSum)
                    {
                        txtSvFrom.Text = userInputTempData.relativeSumSvFrom;
                    }
                    else
                    {
                        txtSvFrom.Text = userInputTempData.svFrom;
                    }
                }
                else
                {
                    txtSvFrom.Text = userInputTempData.svFrom;
                }
                txtSvFrom.Enabled = true;
                txtSvFrom.BackColor = SystemColors.Window;
                txtSvFrom.ForeColor = SystemColors.WindowText;
            }
            else
            {
                txtSvFrom.Enabled = false;
                txtSvFrom.BackColor = SystemColors.WindowFrame;
                txtSvFrom.ForeColor = txtSvFrom.BackColor;
            }
        }
        /// <summary>
        /// SV終点テキストボックスの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="txtSvTo">SV終点テキストボックス</param>
        internal static void SetTxtSvToEnabledState(bool isEnable,
                                                    TextBox txtSvTo)
        {
            if (isEnable)
            {
                txtSvTo.Enabled = true;
                txtSvTo.ForeColor = SystemColors.WindowText;
                txtSvTo.BackColor = SystemColors.Window;
            }
            else
            {
                txtSvTo.Enabled = false;
                txtSvTo.BackColor = SystemColors.WindowFrame;
                txtSvTo.ForeColor = txtSvTo.BackColor;
            }
        }
        /// <summary>
        /// SV入れ替えボタンの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="btnSwapSv">SV入れ替えボタン</param>
        internal static void SetBtnSwapSvEnabledState(bool isEnable,
                                                      Button btnSwapSv)
        {
            if (isEnable)
            {
                btnSwapSv.Enabled = true;
                btnSwapSv.ForeColor = Color.Cyan;
                btnSwapSv.FlatAppearance.BorderColor = Color.Cyan;
            }
            else
            {
                btnSwapSv.Enabled = false;
                btnSwapSv.ForeColor = SystemColors.WindowFrame;
                btnSwapSv.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// 始点SV設定ボタンの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="btnSetSvFrom">始点SV設定ボタン</param>
        internal static void SetBtnSetSvFromEnabledState(bool isEnable,
                                                         Button btnSetSvFrom)
        {
            if (isEnable)
            {
                btnSetSvFrom.Enabled = true;
                btnSetSvFrom.ForeColor = SystemColors.Control;
                btnSetSvFrom.FlatAppearance.BorderColor = Color.Cyan;
            }
            else
            {
                btnSetSvFrom.Enabled = false;
                btnSetSvFrom.ForeColor = SystemColors.WindowFrame;
                btnSetSvFrom.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// 終点SV設定ボタンの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="btnSetSvTo">終点SV設定ボタン</param>
        internal static void SetBtnSetSvToEnabledState(bool isEnable,
                                                       Button btnSetSvTo)
        {
            if (isEnable)
            {
                btnSetSvTo.Enabled = true;
                btnSetSvTo.ForeColor = SystemColors.Control;
                btnSetSvTo.FlatAppearance.BorderColor = Color.Cyan;
            }
            else
            {
                btnSetSvTo.Enabled = false;
                btnSetSvTo.ForeColor = SystemColors.WindowFrame;
                btnSetSvTo.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// Volume始点テキストボックスの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="txtVolumeFrom">Volume始点テキストボックス</param>
        internal static void SetTxtVolumeFromEnabledState(bool isEnable,
                                                          TextBox txtVolumeFrom)
        {
            if (isEnable)
            {
                txtVolumeFrom.Enabled = true;
                txtVolumeFrom.ForeColor = SystemColors.WindowText;
                txtVolumeFrom.BackColor = SystemColors.Window;
            }
            else
            {
                txtVolumeFrom.Enabled = false;
                txtVolumeFrom.ForeColor = SystemColors.WindowFrame;
                txtVolumeFrom.BackColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// Volume終点テキストボックスの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="txtVolumeTo">Volume終点テキストボックス</param>
        internal static void SetTxtVolumeToEnabledState(bool isEnable,
                                                        TextBox txtVolumeTo)
        {
            if (isEnable)
            {
                txtVolumeTo.Enabled = true;
                txtVolumeTo.ForeColor = SystemColors.WindowText;
                txtVolumeTo.BackColor = SystemColors.Window;
            }
            else
            {
                txtVolumeTo.Enabled = false;
                txtVolumeTo.ForeColor = SystemColors.WindowFrame;
                txtVolumeTo.BackColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// Volume入れ替えボタンの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="btnSwapVolume">Volume入れ替えボタン</param>
        internal static void SetBtnSwapVolumeEnabledState(bool isEnable,
                                                          Button btnSwapVolume)
        {
            if (isEnable)
            {
                btnSwapVolume.Enabled = true;
                btnSwapVolume.ForeColor = Color.Cyan;
                btnSwapVolume.FlatAppearance.BorderColor = Color.Cyan;
            }
            else
            {
                btnSwapVolume.Enabled = false;
                btnSwapVolume.ForeColor = SystemColors.WindowFrame;
                btnSwapVolume.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// 始点Volume設定ボタンの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="btnSetVolumeFrom">始点Volume設定ボタン</param>
        internal static void SetBtnSetVolumeFromEnabledState(bool isEnable,
                                                             Button btnSetVolumeFrom)
        {
            if (isEnable)
            {
                btnSetVolumeFrom.Enabled = true;
                btnSetVolumeFrom.ForeColor = SystemColors.Control;
                btnSetVolumeFrom.FlatAppearance.BorderColor = Color.Cyan;
            }
            else
            {
                btnSetVolumeFrom.Enabled = false;
                btnSetVolumeFrom.ForeColor = SystemColors.WindowFrame;
                btnSetVolumeFrom.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// 終点Volume設定ボタンの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="btnSetVolumeTo">終点Volume設定ボタン</param>
        internal static void SetBtnSetVolumeToEnabledState(bool isEnable,
                                                           Button btnSetVolumeTo)
        {
            if (isEnable)
            {
                btnSetVolumeTo.Enabled = true;
                btnSetVolumeTo.ForeColor = SystemColors.Control;
                btnSetVolumeTo.FlatAppearance.BorderColor = Color.Cyan;
            }
            else
            {
                btnSetVolumeTo.Enabled = false;
                btnSetVolumeTo.ForeColor = SystemColors.WindowFrame;
                btnSetVolumeTo.FlatAppearance.BorderColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// 相対速度変化オプションの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="userInputTempData">ユーザー入力の一時データ</param>
        /// <param name="pnlRelativeSvGroup">相対速度変化オプションのグループパネル</param>
        /// <param name="chkEnableSvTo">SV終点設定チェックボックス</param>
        /// <param name="chkEnableSv">SV設定チェックボックス</param>
        /// <param name="rdoArithmetic">等差ラジオボタン</param>
        /// <param name="rdoGeometric">等比ラジオボタン</param>
        /// <param name="txtSvFrom">SV始点テキストボックス</param>
        /// <param name="txtSvTo">SV終点テキストボックス</param>
        internal static void SetRelativeEnabledState(bool isEnable,
                                                     UserInputTempData userInputTempData,
                                                     Panel pnlRelativeSvGroup,
                                                     CheckBox chkEnableSvTo,
                                                     CheckBox chkEnableSv,
                                                     RadioButton rdoArithmetic,
                                                     RadioButton rdoGeometric,
                                                     TextBox txtSvFrom,
                                                     TextBox txtSvTo)
        {
            if (isEnable)
            {
                pnlRelativeSvGroup.Visible = true;
                chkEnableSvTo.Visible = true;
                chkEnableSv.Enabled = false;
                rdoArithmetic.Enabled = false;
                rdoGeometric.Enabled = false;
                if (userInputTempData.isRelativeMultiply)
                {
                    txtSvFrom.Text = userInputTempData.relativeMultiplySvFrom;
                    txtSvTo.Text = userInputTempData.relativeMultiplySvTo;
                }
                else if (userInputTempData.isRelativeSum)
                {
                    txtSvFrom.Text = userInputTempData.relativeSumSvFrom;
                    txtSvTo.Text = userInputTempData.relativeSumSvTo;
                }
            }
            else
            {
                pnlRelativeSvGroup.Visible = false;
                chkEnableSvTo.Visible = false;
                chkEnableSv.Enabled = true;
                rdoArithmetic.Enabled = true;
                rdoGeometric.Enabled = true;
                txtSvFrom.Text = userInputTempData.svFrom;
                txtSvTo.Text = userInputTempData.svTo;
            }
        }
        /// <summary>
        /// 相対速度変化オプション(乗算)の有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="userInputTempData">ユーザー入力の一時データ</param>
        /// <param name="txtRelativeBaseSv">相対基準SVテキストボックス</param>
        /// <param name="txtSvFrom">SV始点テキストボックス</param>
        /// <param name="txtSvTo">SV終点テキストボックス</param>
        internal static void SetRelativeMultiplyEnableState(bool isEnable,
                                                            UserInputTempData userInputTempData,
                                                            TextBox txtRelativeBaseSv,
                                                            TextBox txtSvFrom,
                                                            TextBox txtSvTo)
        {
            if (isEnable)
            {
                txtRelativeBaseSv.Enabled = true;
                txtRelativeBaseSv.ForeColor = SystemColors.WindowText;
                txtRelativeBaseSv.BackColor = SystemColors.Window;
                txtSvFrom.Text = userInputTempData.relativeMultiplySvFrom;
                txtSvTo.Text = userInputTempData.relativeMultiplySvTo;
            }
            else
            {
                txtRelativeBaseSv.Enabled = false;
                txtRelativeBaseSv.ForeColor = SystemColors.WindowFrame;
                txtRelativeBaseSv.BackColor = SystemColors.WindowFrame;
            }
        }
        /// <summary>
        /// Offset有効化チェックボックス(適用)の有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="txtOffset">Offsetテキストボックス</param>
        internal static void SetChkEnableOffset(bool isEnable,
                                                TextBox txtOffset)
        {
            if (isEnable)
            {
                txtOffset.ForeColor = SystemColors.WindowText;
                txtOffset.BackColor = SystemColors.Window;
                txtOffset.Enabled = true;
            }
            else
            {
                txtOffset.ForeColor = SystemColors.WindowFrame;
                txtOffset.BackColor = SystemColors.WindowFrame;
                txtOffset.Enabled = false;
            }
        }
        /// <summary>
        /// 処理項目タブが"適用"の時のコントロールの初期化処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        /// <param name="userInputTempData">ユーザー入力の一時データ</param>
        /// <param name="config">設定データ</param>
        /// <param name="chkEnableSv">SV有効化チェックボックス</param>
        /// <param name="chkEnableVolume">Volume有効化チェックボックス</param>
        /// <param name="chkApplyStartObject">開始オブジェクト適用チェックボックス</param>
        /// <param name="chkApplyEndObject">終了オブジェクト適用チェックボックス</param>
        /// <param name="chkRelative">相対チェックボックス</param>
        /// <param name="pnlRelativeSvGroup">相対SVグループパネル</param>
        /// <param name="chkEnableSvTo">SV終点有効化チェックボックス</param>
        /// <param name="txtSvFrom">SV始点テキストボックス</param>
        /// <param name="txtSvTo">SV終点テキストボックス</param>
        /// <param name="btnSwapSv">SV入れ替えボタン</param>
        /// <param name="btnSetSvFrom">SV始点設定ボタン</param>
        /// <param name="btnSetSvTo">SV終点設定ボタン</param>
        /// <param name="txtVolumeFrom">Volume始点テキストボックス</param>
        /// <param name="txtVolumeTo">Volume終点テキストボックス</param>
        /// <param name="btnSwapVolume">Volume入れ替えボタン</param>
        /// <param name="btnSetVolumeFrom">Volume始点設定ボタン</param>
        /// <param name="btnSetVolumeTo">Volume終点設定ボタン</param>
        internal static void SetApplyControls(bool isEnable,
                                              UserInputTempData userInputTempData,
                                              Config config,
                                              CheckBox chkEnableSv,
                                              CheckBox chkEnableVolume,
                                              CheckBox chkApplyStartObject,
                                              CheckBox chkApplyEndObject,
                                              CheckBox chkRelative,
                                              Panel pnlRelativeSvGroup,
                                              CheckBox chkEnableSvTo,
                                              TextBox txtSvFrom,
                                              TextBox txtSvTo,
                                              Button btnSwapSv,
                                              Button btnSetSvFrom,
                                              Button btnSetSvTo,
                                              TextBox txtVolumeFrom,
                                              TextBox txtVolumeTo,
                                              Button btnSwapVolume,
                                              Button btnSetVolumeFrom,
                                              Button btnSetVolumeTo)
        {
            bool isAdvanceMode = (config.advanceMode == 1 && isEnable);
            chkEnableSv.Enabled = isEnable;
            chkEnableVolume.Enabled = isEnable;
            chkApplyStartObject.Enabled = isEnable;
            chkApplyEndObject.Enabled = isEnable;
            chkRelative.Visible = isAdvanceMode;
            if (isEnable)
            {
                pnlRelativeSvGroup.Visible = userInputTempData.isRelative;
                chkEnableSvTo.Visible = userInputTempData.isRelative;
            }
            else
            {
                pnlRelativeSvGroup.Visible = false;
                chkEnableSvTo.Visible = false;
            }
            if (isEnable && !userInputTempData.isSv)
            {
                SetTxtSvFromEnabledState(userInputTempData.isSv, userInputTempData, txtSvFrom);
                SetTxtSvToEnabledState(userInputTempData.isSv, txtSvTo);
                SetBtnSwapSvEnabledState(userInputTempData.isSv, btnSwapSv);
                SetBtnSetSvFromEnabledState(userInputTempData.isSv, btnSetSvFrom);
                SetBtnSetSvToEnabledState(userInputTempData.isSv, btnSetSvTo);
            }
            else
            {
                if (userInputTempData.isRelative)
                {
                    chkEnableSv.Enabled = false;
                    SetTxtSvToEnabledState(isEnable ? userInputTempData.isEnableRelativeEnd : isEnable, txtSvTo);
                    SetBtnSwapSvEnabledState(isEnable ? userInputTempData.isEnableRelativeEnd : isEnable, btnSwapSv);
                    SetBtnSetSvToEnabledState(isEnable ? userInputTempData.isEnableRelativeEnd : isEnable, btnSetSvTo);
                }
                else
                {
                    SetTxtSvToEnabledState(isEnable, txtSvTo);
                    SetBtnSwapSvEnabledState(isEnable, btnSwapSv);
                    SetBtnSetSvToEnabledState(isEnable, btnSetSvTo);
                }
                SetTxtSvFromEnabledState(isEnable, userInputTempData, txtSvFrom);
                SetBtnSetSvFromEnabledState(isEnable, btnSetSvFrom);
            }
            if (isEnable && !userInputTempData.isVolume)
            {
                SetTxtVolumeFromEnabledState(userInputTempData.isVolume, txtVolumeFrom);
                SetTxtVolumeToEnabledState(userInputTempData.isVolume, txtVolumeTo);
                SetBtnSwapVolumeEnabledState(userInputTempData.isVolume, btnSwapVolume);
                SetBtnSetVolumeFromEnabledState(userInputTempData.isVolume, btnSetVolumeFrom);
                SetBtnSetVolumeToEnabledState(userInputTempData.isVolume, btnSetVolumeTo);
            }
            else
            {
                SetTxtVolumeFromEnabledState(isEnable, txtVolumeFrom);
                SetTxtVolumeToEnabledState(isEnable, txtVolumeTo);
                SetBtnSwapVolumeEnabledState(isEnable, btnSwapVolume);
                SetBtnSetVolumeFromEnabledState(isEnable, btnSetVolumeFrom);
                SetBtnSetVolumeToEnabledState(isEnable, btnSetVolumeTo);
            }

        }
        #endregion
        #region SettingsForm
        #endregion
        #region DialogForm
        /// <summary>
        /// メッセージの設定
        /// </summary>
        /// <param name="messageCode"></param>
        internal static void SetMessage(PictureBox picSystemIcon, 
                                        Label lblMessage,
                                        DialogForm form,
                                        string messageCode)
        {
            string messageLevel = messageCode[..1];
            switch (messageLevel)
            {
                // Informationメッセージの場合
                case "I":
                    picSystemIcon.Image = Properties.Resources.systemicon_info;
                    lblMessage.Text = Common.WriteDialogMessage(messageCode);
                    form.Text = "Information";
                    break;
                // Warningメッセージの場合
                case "W":
                    picSystemIcon.Image = Properties.Resources.systemicon_warn;
                    lblMessage.Text = Common.WriteDialogMessage(messageCode);
                    form.Text = "Warning";
                    break;
                // Errorメッセージの場合
                case "E":
                    picSystemIcon.Image = Properties.Resources.systemicon_err;
                    lblMessage.Text = Common.WriteDialogMessage(messageCode);
                    form.Text = "Error";
                    break;
                // 上記以外の場合
                default:
                    picSystemIcon.Image = Properties.Resources.systemicon_info;
                    lblMessage.Text = messageCode;
                    form.Text = "Information";
                    break;
            }
        }
        #endregion
        #region DebugForm
        #endregion
        #region BGSetterForm
        #endregion
    }
}
