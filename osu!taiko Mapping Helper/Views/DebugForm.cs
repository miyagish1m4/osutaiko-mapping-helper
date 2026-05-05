using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Views
{
    public partial class DebugForm : Form
    {
        #region クラス変数
        public MainForm parentForm { get; set; }
        private BeatmapMetadata beatmapInfo = new();
        private Beatmap? beatmapData;
        private int currentTime;
        delegate void DelegateProcess();//delegateを宣言
        private int updateInterval = 15;
        private bool isClose = false;
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。'required' 修飾子を追加するか、Null 許容として宣言することを検討してください。
        public DebugForm()
#pragma warning restore CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。'required' 修飾子を追加するか、Null 許容として宣言することを検討してください。
        {
            InitializeComponent();
            Thread getMemoryDataThread = new(UpdateLabelText) { IsBackground = true };
            getMemoryDataThread.Start();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
        /// <summary>
        /// ラベルテキストの初期化設定
        /// </summary>
        internal void InitializeLabelText()
        {
            Common.SetLabelText(lblScrollSpeed, "LBL_DEBUG_SCROLL_SPEED");
            lblScrollSpeed.Text += " : ";
        }
        /// <summary>
        /// ラベルテキストの更新処理
        /// </summary>
        private void UpdateLabelText()
        {
            while (true)
            {
                if (isClose)
                {
                    return;
                }
                //if (!isLoad)
                //{
                //    Thread.Sleep(15);
                //    isLoad = true;
                //}
                Thread.Sleep(updateInterval);
                try
                {
                    this.Invoke(() =>
                    {
                        SetCurrentTimeInfo(lblCurrentTiming, Constants.SET_TIMING);
                        SetCurrentTimeInfo(lblCurrentBpm, Constants.SET_BPM);
                        SetCurrentTimeInfo(lblCurrentSv, Constants.SET_SV);
                        SetCurrentTimeInfo(lblCurrentVolume, Constants.SET_VOLUME);
                        if (double.TryParse(lblCurrentBpm.Text, out double bpm) &&
                            double.TryParse(lblCurrentSv.Text, out double sv) &&
                            double.TryParse(beatmapData?.difficulty.FirstOrDefault(line => line.StartsWith("SliderMultiplier:"))?.Split(':')[1].Trim(), out double sliderMultiplier))
                        {
                            double retSv = Math.Round(bpm * sv * sliderMultiplier / Constants.BASE_SLIDER_MULTIPLIER, 5, MidpointRounding.AwayFromZero);
                            string retVisualizeBpmStr = (retSv).ToString();
                            if (retVisualizeBpmStr.Contains('.'))
                            {
                                // SVが整数だった場合は"0"と"."を削除する
                                retVisualizeBpmStr = retVisualizeBpmStr.TrimEnd('0');
                                retVisualizeBpmStr = retVisualizeBpmStr.TrimEnd('.');
                            }

                            lblVisualizeBpm.Text = retVisualizeBpmStr;
                        }
                        else
                        {
                            lblVisualizeBpm.Text = string.Empty;
                        }
                    });

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }
        /// <summary>
        /// Osu!から譜面情報を受け取る処理
        /// </summary>
        /// <param name="beatmapInfo">更新する譜面情報</param>
        /// <param name="currentTime">現在の時間</param>
        internal void SetOsuData(BeatmapMetadata beatmapInfo, int currentTime)
        {
            if (beatmapInfo.beatmapPath == null || beatmapInfo.beatmapPath == string.Empty)
            {
                return;
            }
            // 譜面の内容を取得する
            this.beatmapInfo = beatmapInfo;
            this.currentTime = currentTime;
        }
        /// <summary>
        /// 譜面情報を取得する
        /// </summary>
        /// <returns>取得に成功した場合はtrue、失敗した場合はfalseを返す</returns>
        /// <returns>取得に成功した場合はtrue、失敗した場合はfalseを返す</returns>
        internal bool GetBeatmap()
        {
            // 譜面情報がリアルタイムで取得できていない場合はエラーダイアログを表示する
            if (this.beatmapInfo.beatmapPath == null || this.beatmapInfo.beatmapPath == string.Empty)
            {
                return false;
            }
            // 譜面の内容を取得する
            beatmapData = BeatmapHelper.GetBeatmapData(this.beatmapInfo.beatmapPath, true);
            if (beatmapData.version == string.Empty)
            {
                return false;
            }
            return true;
        }
        /// <summary>
        /// 現地点の情報を取得する処理
        /// </summary>
        /// <param name="control">設定対象のコントロール</param>
        /// <param name="setCode">セットコード(0:Timing 1:SV 2:Volume)</param>
        private void SetCurrentTimeInfo(Control control, int setCode)
        {
            try
            {
                switch (setCode)
                {
                    case Constants.SET_TIMING:
                        control.Text = Common.ConvertFormatTiming(currentTime);
                        break;
                    case Constants.SET_BPM:
                        control.Text = Utils.Helper.Debug.SetCurrentBpm(beatmapData, currentTime);
                        break;
                    case Constants.SET_SV:
                        control.Text = UserInputDataHelper.SetCurrentSv(beatmapData, currentTime, true);
                        break;
                    case Constants.SET_VOLUME:
                        control.Text = UserInputDataHelper.SetCurrentVolume(beatmapData, currentTime);
                        break;
                    default:
                        throw new Exception();
                }
            }
            catch
            {
                Common.WriteErrorMessage("LOG_E-GET-BEATMAP");
            }
        }
        #endregion
        #region イベント
        private void DebugForm_Load(object sender, EventArgs e)
        {
#if DEBUG
            this.TopMost = true;
#endif
            InitializeLabelText();
            parentForm.updateInterval = 15;
            GetBeatmap();
        }
        private void DebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            parentForm.updateInterval = 100;
        }
        private void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClose = true;
        }
        #endregion
    }
}
