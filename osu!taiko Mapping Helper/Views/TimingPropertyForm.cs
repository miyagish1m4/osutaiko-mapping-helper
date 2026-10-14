using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;
using System.ComponentModel;

namespace osu_taiko_Mapping_Helper.Views
{
    public partial class TimingPropertyForm : Form
    {
        #region クラス変数
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MainForm parentForm { get; set; }
        private string beatmapPath = string.Empty;
        private Beatmap? beatmapData;
        private int currentTime;
        // 最後に読み込みに成功した譜面のパスと、読み込む前に取得した更新日時
        private string loadedPath = string.Empty;
        private DateTime loadedLastWriteTime;
        private readonly object beatmapLock = new();
        delegate void DelegateProcess();//delegateを宣言
        private int updateInterval = 15;
        private bool isClose = false;
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
#pragma warning disable CS8618 // null 非許容のフィールドには、コンストラクターの終了時に null 以外の値が入っていなければなりません。'required' 修飾子を追加するか、Null 許容として宣言することを検討してください。
        public TimingPropertyForm()
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
                    // 譜面が変わったか、更新されていた場合は読み込み直す
                    ReloadBeatmapIfChanged();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
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
        /// <param name="beatmapPath">osu!で選択している譜面のパス(osu!が起動していない場合は空)</param>
        /// <param name="currentTime">現在の時間</param>
        /// <remarks>
        /// 譜面の読み込みはUpdateLabelTextで行うため、ここではパスと時間のみ受け取る
        /// </remarks>
        internal void SetOsuData(string? beatmapPath, int currentTime)
        {
            this.beatmapPath = beatmapPath ?? string.Empty;
            this.currentTime = currentTime;
        }
        /// <summary>
        /// 譜面情報を取得する
        /// </summary>
        /// <returns>取得に成功した場合はtrue、失敗した場合はfalseを返す</returns>
        internal bool GetBeatmap()
        {
            lock (beatmapLock)
            {
                return LoadBeatmap(beatmapPath);
            }
        }
        /// <summary>
        /// 譜面のパスか更新日時が前回読み込んだ時と違う場合、譜面を読み込み直す
        /// </summary>
        /// <remarks>
        /// 読み込みに失敗した場合は前回読み込んだ時の値が変わらないため、次の周回で読み込み直す
        /// </remarks>
        private void ReloadBeatmapIfChanged()
        {
            lock (beatmapLock)
            {
                string path = beatmapPath;
                // (譜面を選択していない場合、loadedPathは空で表示も消えている)
                if (path == loadedPath &&
                    (path == string.Empty || File.GetLastWriteTime(path) == loadedLastWriteTime))
                {
                    return;
                }
                LoadBeatmap(path);
            }
        }
        /// <summary>
        /// 譜面を読み込む (beatmapLockをかけてから呼ぶ)
        /// </summary>
        /// <param name="path">譜面のパス</param>
        /// <returns>読み込みに成功した場合はtrue、失敗した場合はfalseを返す</returns>
        private bool LoadBeatmap(string path)
        {
            if (path == string.Empty || !File.Exists(path))
            {
                // 譜面を選択していない場合は表示を消す
                beatmapData = null;
                loadedPath = string.Empty;
                return false;
            }
            // 読み込み中に更新された場合に次の周回で読み込み直せるよう、読み込む前に更新日時を取得する
            DateTime lastWriteTime = File.GetLastWriteTime(path);
            var tempBeatmapData = BeatmapHelper.GetTimingPointsData(path);
            if (tempBeatmapData == null)
            {
                // 別の譜面に切り替わっている場合は、前の譜面の情報を表示しないよう表示を消す
                // (同じ譜面の場合は、読み込めるまで前回読み込んだ内容を表示する)
                if (path != loadedPath)
                {
                    beatmapData = null;
                    loadedPath = string.Empty;
                }
                return false;
            }
            beatmapData = tempBeatmapData;
            loadedPath = path;
            loadedLastWriteTime = lastWriteTime;
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
                        control.Text = beatmapPath == string.Empty ? string.Empty : Common.ConvertFormatTiming(currentTime);
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
            //parentForm.updateInterval = 15;
        }
        private void DebugForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //parentForm.updateInterval = 15;
        }
        private void DebugForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            isClose = true;
        }
        #endregion
    }
}
