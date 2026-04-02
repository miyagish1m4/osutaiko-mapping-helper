using System.Diagnostics;
using OsuMemoryDataProvider;
using OsuMemoryDataProvider.OsuMemoryModels;
using OsuParsers.Decoders;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Services;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;

namespace osu_taiko_Mapping_Helper.Views
{
    public partial class DebugForm : Form
    {
        #region クラス変数
        private BeatmapMetadata beatmapInfo = new();
        private Beatmap? beatmapData;
        private int currentTime;
        delegate void DelegateProcess();//delegateを宣言
        private int updateInterval = 15;
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public DebugForm()
        {
            InitializeComponent();
            Thread getMemoryDataThread = new Thread(UpdateBeatmap) { IsBackground = true };
            getMemoryDataThread.Start();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }

        private void UpdateBeatmap()
        {
            while (true)
            {
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
                            double.TryParse(lblCurrentSv.Text, out double sv))
                        {
                            double retSv = Math.Round(bpm * sv, 5, MidpointRounding.AwayFromZero);
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
        internal void UpdateMemoryData(BeatmapMetadata beatmapInfo, int currentTime)
        {
            if (beatmapInfo.beatmapPath == null || beatmapInfo.beatmapPath == string.Empty)
            {
                return;
            }
            // 譜面の内容を取得する
            this.beatmapInfo = beatmapInfo;
            this.currentTime = currentTime;
        }
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

        private void DebugForm_Load(object sender, EventArgs e)
        {

        }
    }
}
#endregion