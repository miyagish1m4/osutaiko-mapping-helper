using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils.Helper;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Properties;

namespace osu_taiko_Mapping_Helper.Views
{
    public partial class SettingForm : Form
    {
        #region クラス変数
        private Config config;
        private NotesPosition notesPosition = new NotesPosition();
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">コンフィグ</param>
        internal SettingForm(Config config)
        {
            InitializeComponent();
            this.config = config;
            InitializeLabelText();
            SetChkAdvanceMode(config.advanceMode);
            SetChkUnicodeSupport(config.unicodeSupport);
            SetNotesPosition();
        }
        /// <summary>
        /// ラベルテキストの初期化設定
        /// </summary>
        private void InitializeLabelText()
        {
            Common.SetLabelText(lblLanguage, "LBL_SETTINGS_LANGUAGE");
            Common.SetLabelText(lblMaxBackupCount, "LBL_SETTINGS_BACKUP_LIMIT");
            Common.SetLabelText(lblSeparateSetting, "LBL_SETTINGS_NOTES_POSITION");
            Common.SetLabelText(lblUnicodeSupport, "LBL_SETTINGS_UNICODE_SUPPORT");
            Common.SetLabelText(btnSave, "LBL_SETTINGS_SAVE");
        }
        /// <summary>
        /// コントロール初期化設定
        /// </summary>
        private void InitializeControls()
        {
            cmbLanguage.Items.AddRange(Constants.LANGUAGES);
            cmbLanguage.Text = config.language;
            txtMaxBackupCount.Text = config.maxBackupCount.ToString();
            rdoDon.Checked = true;
            this.MinimizeBox = false;
            this.MaximizeBox = false;
        }
        /// <summary>
        /// ノーツ座標の初期化設定<br/>
        /// </summary>
        private void SetNotesPosition()
        {
            notesPosition.donX = config.donX.ToString();
            notesPosition.donY = config.donY.ToString();
            notesPosition.katX = config.katX.ToString();
            notesPosition.katY = config.katY.ToString();
            notesPosition.finisherDonX = config.finisherDonX.ToString();
            notesPosition.finisherDonY = config.finisherDonY.ToString();
            notesPosition.finisherKatX = config.finisherKatX.ToString();
            notesPosition.finisherKatY = config.finisherKatY.ToString();
        }
        #endregion
        #region イベントハンドラ
        private void SettingForm_Load(object sender, EventArgs e)
        {
            // コントロールの初期化
            InitializeControls();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            // app.configに設定値をセットする
            if (SettingHelper.SetConfig(cmbLanguage.Text,
                                        txtMaxBackupCount.Text,
                                        chkAdvanceMode.Checked,
                                        chkUnicodeSupport.Checked,
                                        notesPosition,
                                        config))
            {
                Common.LoadConfig(config);
                // 設定値に応じてバックアップの保持数を変更する
                if (SettingHelper.ResetBackupFile(config))
                {
                    // 成功した場合はメッセージダイアログを表示する
                    Common.ShowMessageDialog("I_A-P-3");
                    this.Close();
                }
                else
                {
                    // 失敗した場合はエラーダイアログを表示する
                    Common.ShowMessageDialog("E_A-P-2");
                }
            }
        }
        private void chkAdvanceMode_CheckedChanged(object? sender, EventArgs? e)
        {
            chkAdvanceMode.Text = chkAdvanceMode.Checked ? "✔" : "";
        }
        private void chkUnicodeSupport_CheckedChanged(object sender, EventArgs e)
        {
            chkUnicodeSupport.Text = chkUnicodeSupport.Checked ? "✔" : "";
        }
        private void SetChkAdvanceMode(int isAdvanceMode)
        {
            chkAdvanceMode.Checked = isAdvanceMode == 1 ? true : false;
        }
        private void SetChkUnicodeSupport(int isUnicodeSupport)
        {
            chkUnicodeSupport.Checked = isUnicodeSupport == 1 ? true : false;
        }
        private void rdoDon_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoDon.Checked)
            {
                txtPisitionX.Text = notesPosition.donX;
                txtPositionY.Text = notesPosition.donY;
            }
        }
        private void rdoKat_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoKat.Checked)
            {
                txtPisitionX.Text = notesPosition.katX;
                txtPositionY.Text = notesPosition.katY;
            }
        }
        private void rdoFinisherDon_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFinisherDon.Checked)
            {
                txtPisitionX.Text = notesPosition.finisherDonX;
                txtPositionY.Text = notesPosition.finisherDonY;
            }
        }
        private void rdoFinisherKat_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoFinisherKat.Checked)
            {
                txtPisitionX.Text = notesPosition.finisherKatX;
                txtPositionY.Text = notesPosition.finisherKatY;
            }
        }
        private void txtPisitionX_TextChanged(object sender, EventArgs e)
        {
            if (rdoDon.Checked)
            {
                notesPosition.donX = txtPisitionX.Text;
            }
            else if (rdoKat.Checked)
            {
                notesPosition.katX = txtPisitionX.Text;
            }
            else if (rdoFinisherDon.Checked)
            {
                notesPosition.finisherDonX = txtPisitionX.Text;
            }
            else if (rdoFinisherKat.Checked)
            {
                notesPosition.finisherKatX = txtPisitionX.Text;
            }
        }
        private void txtPositionY_TextChanged(object sender, EventArgs e)
        {
            if (rdoDon.Checked)
            {
                notesPosition.donY = txtPositionY.Text;
            }
            else if (rdoKat.Checked)
            {
                notesPosition.katY = txtPositionY.Text;
            }
            else if (rdoFinisherDon.Checked)
            {
                notesPosition.finisherDonY = txtPositionY.Text;
            }
            else if (rdoFinisherKat.Checked)
            {
                notesPosition.finisherKatY = txtPositionY.Text;
            }
        }
        #endregion

    }
}
