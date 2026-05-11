using osu_taiko_Mapping_Helper.Utils;

namespace osu_taiko_Mapping_Helper.Views
{
    public partial class DialogForm : Form
    {
        #region クラス変数
        private int newLineCount = 1;
        const double CHAR_SIZE = 8.5;
        const int BUTTON_GAP = 11;
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        public DialogForm(string messageCode, MessageBoxButtons dialogButtonCode = MessageBoxButtons.OK)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ControlBox = false;
            FormUtils.SetMessage(picSystemIcon, lblMessage, this, messageCode);
            InitializeControls(dialogButtonCode);
            InitializeLabelText();

        }
        /// <summary>
        /// ラベルテキストを初期化する
        /// </summary>
        private void InitializeLabelText()
        {
            Common.SetLabelText(btnOk, "LBL_DIALOG_OK");
            Common.SetLabelText(btnCancel, "LBL_DIALOG_CANCEL");
        }
        /// <summary>
        /// コントロールの初期化設定
        /// </summary>
        private void InitializeControls(MessageBoxButtons dialogButtonCode)
        {
            newLineCount = lblMessage.Text.Length - lblMessage.Text.Replace("\n", "").Length;
            this.ClientSize = new Size(90 + lblMessage.Size.Width, this.ClientSize.Height - 15 + lblMessage.Size.Height);
            picSystemIcon.Location = new Point(picSystemIcon.Location.X, picSystemIcon.Location.Y + (int)(CHAR_SIZE * newLineCount));
            pnlBg.Location = new Point(pnlBg.Location.X, pnlBg.Location.Y - 15 + lblMessage.Size.Height);
            switch (dialogButtonCode)
            {
                case MessageBoxButtons.OK:
                    btnOk.Location = new Point(this.ClientSize.Width - 90, btnOk.Location.Y - 15 + lblMessage.Size.Height);
                    this.ActiveControl = this.btnOk;
                    break;
                case MessageBoxButtons.OKCancel:
                    btnCancel.Location = new Point(this.ClientSize.Width - 90, btnOk.Location.Y - 15 + lblMessage.Size.Height);
                    btnOk.Location = new Point(this.ClientSize.Width - btnCancel.Width - BUTTON_GAP - 90, btnOk.Location.Y - 15 + lblMessage.Size.Height);
                    this.ActiveControl = this.btnCancel;
                    break;
            }
        }
        #endregion
        #region イベントハンドラ
        private void btnOk_Click(object sender, EventArgs e)
        {
            Common.isDialogResult = true;
            this.Close();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            Common.isDialogResult = false;
            this.Close();
        }
        #endregion
    }
}
