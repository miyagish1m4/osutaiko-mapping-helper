using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;
using osu_taiko_Mapping_Helper.Properties;

namespace osu_taiko_Mapping_Helper.Views
{
    public partial class BGSetterForm : Form
    {
        #region クラス変数
        private readonly Config config;
        private readonly string beatmapPath;
        private readonly string backgroundPath;
        private readonly string backupDirectoryName;
        private readonly Size INITIAL_SIZE = new(854, 480);
        private const int INITIAL_BG_Y = 135;
        private Point clickPoint;
        private string[] backgrounds = new string[5];
        private Beatmap? beatmapData;
        private int retY = 0;
        private int offsetY = 0;
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="config">設定情報クラス</param>
        /// <param name="beatmapPath">編集対象のビートマップファイルのパス</param>
        /// <param name="backgroundPath">新しい背景画像ファイルのパス</param>
        /// <param name="background">背景画像設定文字列</param>
        /// <param name="backupDirectoryName">バックアップファイルを保存するディレクトリ</param>
        internal BGSetterForm(Config config, string beatmapPath, string backgroundPath, string background, string backupDirectoryName)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.picDisplayBg.Size = INITIAL_SIZE;
            this.config = config;
            this.beatmapPath = beatmapPath;
            this.backgroundPath = backgroundPath;
            this.backupDirectoryName = backupDirectoryName;
            InitializeBG(background);
            InitializeLabelText();
        }
        /// <summary>
        /// 背景画像を初期化する
        /// </summary>
        /// <param name="background">背景画像設定文字列</param>
        private void InitializeBG(string background)
        {
            backgrounds = background.Split(',', 5);
            // 背景画像を表示する
            picDisplayBg.Image = BGHelper.SetBgOnForm(backgroundPath,
                                                      new Point(INITIAL_SIZE.Width, INITIAL_SIZE.Height),
                                                      0.7);
            // 画像の位置をosuで表示されている位置に調整する
            picDisplayBg.Size = new Size(picDisplayBg.Image.Width, picDisplayBg.Image.Height);
            offsetY = (int)BGHelper.ConvertPixelToOsuPixel((double)(picDisplayBg.Size.Height - INITIAL_SIZE.Height) / 2,
                                                           (double)INITIAL_SIZE.Height);
            int posY = (int)BGHelper.ConvertOsuPixelToPixel((double)(int.Parse(backgrounds[4]) - offsetY),
                                                            (double)INITIAL_SIZE.Height);
            picDisplayBg.Location = new Point(picDisplayBg.Location.X, INITIAL_BG_Y + posY);
            // osuで設定されている初期位置を表示する
            retY = int.Parse(backgrounds[4]);
            lblCurrentPosY.Text = "y : " + retY;
        }
        /// <summary>
        /// ラベルテキストを初期化する
        /// </summary>
        internal void InitializeLabelText()
        {
            Common.SetLabelText(btnApply, "LBL_EXECUTE");
        }
        /// <summary>
        /// 譜面情報を取得する
        /// </summary>
        /// <returns>取得に成功した場合はtrue、失敗した場合はfalseを返す</returns>
        internal bool GetBeatmap()
        {
            // 譜面情報がリアルタイムで取得できていない場合はエラーダイアログを表示する
            if (this.beatmapPath == null || this.beatmapPath == string.Empty)
            {
                return false;
            }
            // 譜面の内容を取得する
            beatmapData = BeatmapHelper.GetBeatmapData(this.beatmapPath, true);
            // 取得できなかった場合はエラーダイアログを表示する
            if (beatmapData.version == string.Empty)
            {
                return false;
            }
            return true;
        }
        #endregion
        #region イベント
        private void BGSetterForm_KeyDown(object sender, KeyEventArgs e)
        {
            string backupPath = Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY + "\\" + this.backupDirectoryName;
            switch (e.KeyData)
            {
                //［Ctrl］+［S］が押されたらBG変更を実行する
                case (Keys.Control | Keys.S):
                    btnApply_Click(sender, e);
                    break;
                //case Keys.D0:
                //    lblCurrentPosY.Text += "0";
                //    break;
                //case Keys.D1:
                //    lblCurrentPosY.Text += "1";
                //    break;
                //case Keys.D2:
                //    lblCurrentPosY.Text += "2";
                //    break;
                //case Keys.D3:
                //    lblCurrentPosY.Text += "3";
                //    break;
                //case Keys.D4:
                //    lblCurrentPosY.Text += "4";
                //    break;
                //case Keys.D5:
                //    lblCurrentPosY.Text += "5";
                //    break;
                //case Keys.D6:
                //    lblCurrentPosY.Text += "6";
                //    break;
                //case Keys.D7:
                //    lblCurrentPosY.Text += "7";
                //    break;
                //case Keys.D8:
                //    lblCurrentPosY.Text += "8";
                //    break;
                //case Keys.D9:
                //    lblCurrentPosY.Text += "9";
                //    break;
                //case Keys.NumPad0:
                //    lblCurrentPosY.Text += "0";
                //    break;
                //case Keys.NumPad1:
                //    lblCurrentPosY.Text += "1";
                //    break;
                //case Keys.NumPad2:
                //    lblCurrentPosY.Text += "2";
                //    break;
                //case Keys.NumPad3:
                //    lblCurrentPosY.Text += "3";
                //    break;
                //case Keys.NumPad4:
                //    lblCurrentPosY.Text += "4";
                //    break;
                //case Keys.NumPad5:
                //    lblCurrentPosY.Text += "5";
                //    break;
                //case Keys.NumPad6:
                //    lblCurrentPosY.Text += "6";
                //    break;
                //case Keys.NumPad7:
                //    lblCurrentPosY.Text += "7";
                //    break;
                //case Keys.NumPad8:
                //    lblCurrentPosY.Text += "8";
                //    break;
                //case Keys.NumPad9:
                //    lblCurrentPosY.Text += "9";
                //    break;
                //case Keys.Back:
                //    if (lblCurrentPosY.Text.Length > 4)
                //    {
                //        lblCurrentPosY.Text = lblCurrentPosY.Text.Substring(0, lblCurrentPosY.Text.Length - 1);
                //    }
                //    break;
            }
        }
        private void picDisplayBg_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                clickPoint = e.Location;
            }
        }
        private void picDisplayBg_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int diffY = e.Location.Y - clickPoint.Y;
                if (picDisplayBg.Location.Y + diffY + picDisplayBg.Size.Height < INITIAL_SIZE.Height || picDisplayBg.Location.Y + diffY > picTaikoBar.Height)
                {
                    return;
                }
                picDisplayBg.Location = new Point(picDisplayBg.Location.X, picDisplayBg.Location.Y + diffY);
                retY = (int)(BGHelper.ConvertPixelToOsuPixel((double)(picDisplayBg.Location.Y - INITIAL_BG_Y),
                                                             (double)INITIAL_SIZE.Height) +
                             offsetY);
                lblCurrentPosY.Text = $"y : {retY}";
            }
        }
        private void picDisplayBg_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                clickPoint = Point.Empty;
            }
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            GetBeatmap();
            // 入力値と譜面情報が取得できていない場合はエラーダイアログを表示する
            if (beatmapData == null)
            {
                Common.ShowMessageDialog("E_A-D-1");
                return;
            }
            // バックアップを作成する
            if (BeatmapHelper.CreateBackup(this.beatmapPath, this.backupDirectoryName))
            {
                if (!SettingHelper.ResetBackupFile(config))
                {
                    // 失敗した場合はエラーダイアログを表示する
                    Common.ShowMessageDialog("E_A-P-1");
                    return;
                }
            }
            backgrounds[4] = retY.ToString();
            if (!BeatmapHelper.ExportToOsuFile(beatmapData, this.beatmapPath, backgrounds))
            {
                // 失敗した場合はエラーダイアログを表示する
                Common.ShowMessageDialog("E_A-P-1");
                return;
            }
            // 成功した場合は成功ダイアログを表示する
            Common.ShowMessageDialog("I_A-P-1");
            this.Close();
        }
        #endregion

    }
}

