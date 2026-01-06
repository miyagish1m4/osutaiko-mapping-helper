using osuTaikoSvTool.Utils;

namespace osuTaikoSvTool.Views
{
    public partial class ExecuteResultForm : Form
    {
        #region クラス変数
        private int newLineCount = 1;
        const decimal CHAR_SIZE = 8.5m;
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="messageCode">メッセージコード</param>
        public ExecuteResultForm(string messageCode)
        {
            InitializeComponent();
            this.ShowInTaskbar = false;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ControlBox = false;
            SetMessage(messageCode);
            InitializeControls();
        }
        /// <summary>
        /// コントロールの初期化設定
        /// </summary>
        private void InitializeControls()
        {
            newLineCount = lblMessage.Text.Length - lblMessage.Text.Replace("\n", "").Length;
            this.ClientSize = new Size(90 + lblMessage.Size.Width, this.ClientSize.Height - 15 + lblMessage.Size.Height);
            picSystemIcon.Location = new Point(picSystemIcon.Location.X, picSystemIcon.Location.Y + (int)(CHAR_SIZE * newLineCount));
            pnlBg.Location = new Point(pnlBg.Location.X, pnlBg.Location.Y - 15 + lblMessage.Size.Height);
            btnOk.Location = new Point(this.ClientSize.Width - 90, btnOk.Location.Y - 15 + lblMessage.Size.Height);
            btnOk.Focus();
        }
        /// <summary>
        /// メッセージの設定
        /// </summary>
        /// <param name="messageCode"></param>
        private void SetMessage(string messageCode)
        {
            string messageLevel = messageCode[..1];
            switch (messageLevel)
            {
                // Informationメッセージの場合
                case "I":
                    picSystemIcon.Image = Properties.Resources.systemicon_info;
                    lblMessage.Text = Common.WriteDialogMessage(messageCode);
                    this.Text = "Information";
                    break;
                // Warningメッセージの場合
                case "W":
                    picSystemIcon.Image = Properties.Resources.systemicon_warn;
                    lblMessage.Text = Common.WriteDialogMessage(messageCode);
                    this.Text = "Warning";
                    break;
                // Errorメッセージの場合
                case "E":
                    picSystemIcon.Image = Properties.Resources.systemicon_err;
                    lblMessage.Text = Common.WriteDialogMessage(messageCode);
                    this.Text = "Error";
                    break;
                // 上記以外の場合
                default:
                    picSystemIcon.Image = Properties.Resources.systemicon_info;
                    lblMessage.Text = messageCode;
                    this.Text = "Information";
                    break;
            }
        }
        #endregion
        #region イベントハンドラ
        private void btnOk_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion
    }
}
