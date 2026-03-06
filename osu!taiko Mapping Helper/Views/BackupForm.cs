using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Views
{
    public partial class BackupForm : Form
    {
        #region クラス変数
        private int totalPage = 0;
        private int currentPage = 1;
        private List<string> backupFiles = [];
        private string beatmapPath = string.Empty;
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="songName">曲名</param>
        /// <param name="beatmapPath">譜面のパス</param>
        public BackupForm(string songName, string beatmapPath)
        {
            InitializeComponent();
            string backupPath = Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY + "\\" + songName;
            backupFiles.AddRange(Directory.GetFiles(backupPath, "*.osu"));
            backupFiles.Reverse();
            this.beatmapPath = beatmapPath;
            totalPage = (backupFiles.Count + 9) / 10;
            lblCurrentPage.Text = currentPage.ToString();
            SetBackupFileList();
        }
        /// <summary>
        /// ラベルテキストの初期化設定
        /// </summary>
        private void InitializeLabelText()
        {
            Common.SetLabelText(lblFileName, "LBL_BACKUP_FILE");
            Common.SetLabelText(lblDate, "LBL_BACKUP_DATE");
        }
        /// <summary>
        /// バックアップファイル名を取得する
        /// </summary>
        /// <param name="index">ボタンのインデックス</param>
        private void GetBackupFile(int index)
        {

            if (this.Controls.Find("lblFileName" + index, true).FirstOrDefault() is not Label fileNameLabel)
            {
                return;
            }
            if (fileNameLabel.Text == string.Empty)
            {
                return;
            }
            int fileIndex = (currentPage - 1) * 10 + (index - 1);
            string? backupDirectory = Path.GetDirectoryName(backupFiles[fileIndex]);
            if (BeatmapHelper.ExportToOsuFileFromBackup(beatmapPath, backupFiles[fileIndex]))
            {
                Common.ShowMessageDialog("I_A-P-004");
                this.Close();
            }
            else
            {
                Common.ShowMessageDialog("E_A-P-001");
            }
            return;
        }
        /// <summary>
        /// バックアップファイルリストを設定する(1ページあたり10件)
        /// </summary>
        private void SetBackupFileList()
        {
            int startIndex = (currentPage - 1) * 10;
            for (int i = 0; i < 10; i++)
            {
                int fileIndex = startIndex + i;
                if (this.Controls.Find("lblFileName" + (i + 1), true).FirstOrDefault() is not Label fileNameLabel ||
                    this.Controls.Find("lblDate" + (i + 1), true).FirstOrDefault() is not Label dateLabel)
                {
                    return;
                }
                if (fileIndex < backupFiles.Count)
                {
                    string filePath = backupFiles[fileIndex];
                    fileNameLabel.Text = Path.GetFileName(filePath);
                    dateLabel.Text = File.GetLastWriteTime(filePath).ToString("yyyy-MM-dd HH:mm:ss");
                }
                else
                {
                    fileNameLabel.Text = "";
                    dateLabel.Text = "";
                }
            }
        }
        #endregion
        #region イベントハンドラ
        private void BackupForm_Load(object sender, EventArgs e)
        {
            this.MinimizeBox = false;
            this.MaximizeBox = false;
            lblCurrentPage.Text = currentPage.ToString();
            lblTotalPage.Text = totalPage.ToString();
            btnPreviousPage.Enabled = false;
            if (totalPage == 1)
            {
                btnNextPage.Enabled = false;
            }
            InitializeLabelText();
        }
        private void lblFileName1_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(1);
        }
        private void lblDate1_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(1);
        }
        private void lblFileName2_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(2);
        }
        private void lblDate2_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(2);
        }
        private void lblFileName3_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(3);
        }
        private void lblDate3_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(3);
        }
        private void lblFileName4_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(4);
        }
        private void lblDate4_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(4);
        }
        private void lblFileName5_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(5);
        }
        private void lblDate5_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(5);
        }
        private void lblFileName6_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(6);
        }
        private void lblDate6_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(6);
        }
        private void lblFileName7_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(7);
        }
        private void lblDate7_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(7);
        }
        private void lblFileName8_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(8);
        }
        private void lblDate8_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(8);
        }
        private void lblFileName9_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(9);
        }
        private void lblDate9_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(9);
        }
        private void lblFileName10_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(10);
        }
        private void lblDate10_DoubleClick(object sender, EventArgs e)
        {
            GetBackupFile(10);
        }
        private void btnNextPage_Click(object sender, EventArgs e)
        {
            btnPreviousPage.Enabled = true;
            currentPage++;
            lblCurrentPage.Text = currentPage.ToString();
            SetBackupFileList();
            if (currentPage == totalPage)
            {
                btnNextPage.Enabled = false;
            }
        }
        private void btnPreviousPage_Click(object sender, EventArgs e)
        {
            btnNextPage.Enabled = true;
            currentPage--;
            lblCurrentPage.Text = currentPage.ToString();
            SetBackupFileList();
            if (currentPage == 1)
            {
                btnPreviousPage.Enabled = false;
            }
        }
        #endregion
    }
}
