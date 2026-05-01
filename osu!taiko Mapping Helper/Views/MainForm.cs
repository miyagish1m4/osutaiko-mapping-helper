using System.Diagnostics;
using osu_taiko_Mapping_Helper.Models;
using osu_taiko_Mapping_Helper.Properties;
using osu_taiko_Mapping_Helper.Services;
using osu_taiko_Mapping_Helper.Utils;
using osu_taiko_Mapping_Helper.Utils.Helper;
using osu_taiko_Mapping_Helper.Views;
using OsuMemoryDataProvider;
using OsuMemoryDataProvider.OsuMemoryModels;
using OsuParsers.Decoders;

namespace osu_taiko_Mapping_Helper
{
    public partial class MainForm : Form
    {
        #region クラス変数
        private readonly StructuredOsuMemoryReader sreader = StructuredOsuMemoryReader.GetInstance(new("osu!"));
        private readonly OsuBaseAddresses baseAddresses = new();
        private BeatmapMetadata beatmapInfo = new();
        private BeatmapMetadata preBeatmapInfo = new();
        private Beatmap? beatmapData;
        private UserInputData userInputData = new();
        private UserInputTempData userInputTempData = new();
        private UserInputUtilityData userInputUtilityData = new();
        private Config config = new();
        private int preUnicodeSupport;
        private List<TimingPoint> timingPoints = [];
        private string osuDirectory = string.Empty;
        private string songsPath = string.Empty;
        private int currentTime;
        private bool isDirectoryLoaded = false;
        private bool isUpdate = true;
        private string backupDirectoryName = string.Empty;
        private DebugForm? DebugForm = null;
        public int updateInterval { get; set; } = 100;
        #endregion
        #region メソッド
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Thread getMemoryDataThread = new(UpdateMemoryData) { IsBackground = true };
            getMemoryDataThread.Start();
            UpdateBeatmapInfo();

        }
        /// <summary>
        /// osu.exeのメモリデータ取得処理
        /// </summary>
        private void UpdateMemoryData()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(updateInterval);
                    // osu.exeを探す
                    var processes = Process.GetProcessesByName("osu!");
                    if (processes.Length == 0)
                    {
                        if (isDirectoryLoaded)
                        {
                            beatmapInfo = new();
                        }
                        isDirectoryLoaded = false;
                        throw new Exception("osu!が起動されていません。");
                    }
                    // osuフォルダ取得処理
                    if (!isDirectoryLoaded)
                    {
                        Process process = processes[0];
                        string? fileName = process.MainModule?.FileName;
                        osuDirectory = Path.GetDirectoryName(fileName) ?? string.Empty;
                        if (osuDirectory != string.Empty)
                        {
                            if (!Directory.Exists(osuDirectory))
                            {
                                return;
                            }
                            songsPath = Common.GetSongsFolderLocation(osuDirectory);
                            isDirectoryLoaded = !isDirectoryLoaded;
                        }
                    }
                    // メモリから譜面の情報を取得する
                    sreader.TryRead(baseAddresses.Beatmap);
                    sreader.TryRead(baseAddresses.GeneralData);
                    currentTime = baseAddresses.GeneralData.AudioTime;
                    // 譜面のフォルダを取得
                    string osuBeatmapPath = Path.Combine(songsPath ?? "",
                                                         baseAddresses.Beatmap.FolderName ?? "",
                                                         baseAddresses.Beatmap.OsuFileName ?? "");
                    // 譜面のデータを取得
                    OsuParsers.Beatmaps.Beatmap beatmapData = BeatmapDecoder.Decode(osuBeatmapPath);

                    // タイトル
                    beatmapInfo.title = beatmapData.MetadataSection.Title;
                    beatmapInfo.titleUnicode = beatmapData.Version <= 9 ? beatmapData.MetadataSection.Title : beatmapData.MetadataSection.TitleUnicode;
                    // アーティスト
                    beatmapInfo.artist = beatmapData.MetadataSection.Artist;
                    beatmapInfo.artistUnicode = beatmapData.Version <= 9 ? beatmapData.MetadataSection.Artist : beatmapData.MetadataSection.ArtistUnicode;
                    // マッパー名
                    beatmapInfo.creator = beatmapData.MetadataSection.Creator;
                    // Diff名
                    beatmapInfo.version = beatmapData.MetadataSection.Version;
                    beatmapInfo.source = beatmapData.MetadataSection.Source;
                    beatmapInfo.tags = beatmapData.MetadataSection.Tags;
                    beatmapInfo.previewTime = beatmapData.GeneralSection.PreviewTime;
                    // 譜面のパス
                    beatmapInfo.beatmapPath = osuBeatmapPath;
                    // BGのパス
                    string backgroundPath = Path.Combine(songsPath ?? "",
                                                         baseAddresses.Beatmap.FolderName ?? "",
                                                         beatmapData.EventsSection.BackgroundImage ?? "");
                    beatmapInfo.backgroundPath = backgroundPath ?? "";
                    beatmapInfo.lastUpdate = File.GetLastWriteTime(beatmapInfo.beatmapPath).ToString("yyyy-MM-dd HH:mm:ss");
                    try
                    {
                        DebugForm?.UpdateMemoryData(beatmapInfo, currentTime);

                    }
                    catch
                    {

                    }
                    if (beatmapInfo.backgroundPath != "")
                    {
                        if (!BeatmapHelper.GetBackground(ref beatmapInfo))
                        {
                            continue;
                        }
                    }
                    if (isUpdate)
                    {
                        isUpdate = false;
                    }
                }
                catch (Exception ex)
                {
                    // このExceptionはErrorログに書き込むと容量が膨大になってしまうため、
                    // Errorログには書かずConsoleログに出力
                    Console.WriteLine(ex);
                }
            }
        }
        /// <summary>
        /// UpdateMemoryDataで取得したosuのメモリデータを使用し、
        /// 譜面のBGやMetadataをコントロールに設定する
        /// </summary>
        private async void UpdateBeatmapInfo()
        {
            while (true)
            {
                await Task.Delay(15);

                try
                {
                    if (isUpdate)
                    {
                        continue;
                    }
                    // 前回取得したデータと同じ場合は処理を行わない
                    if (preBeatmapInfo.version == beatmapInfo.version &&
                        preBeatmapInfo.beatmapPath == beatmapInfo.beatmapPath &&
                        preBeatmapInfo.lastUpdate == beatmapInfo.lastUpdate)
                    {
                        continue;
                    }
                    preBeatmapInfo.backgroundPath = beatmapInfo.backgroundPath ?? "";
                    preBeatmapInfo.artistUnicode = beatmapInfo.artistUnicode;
                    preBeatmapInfo.titleUnicode = beatmapInfo.titleUnicode;
                    preBeatmapInfo.creator = beatmapInfo.creator;
                    // テキストラベルに譜面の情報を書き込む
                    lblFileName.Text = config.unicodeSupport == 1 ?
                                       (beatmapInfo.artistUnicode.Replace("&", "&&") +
                                       (beatmapInfo.artistUnicode == string.Empty ? "" : " - ") +
                                       beatmapInfo.titleUnicode.Replace("&", "&&") +
                                       (beatmapInfo.titleUnicode == string.Empty ? "" : " [") +
                                       beatmapInfo.version.Replace("&", "&&") +
                                       (beatmapInfo.version == string.Empty ? "" : "]")) :
                                       (beatmapInfo.artist.Replace("&", "&&") +
                                       (beatmapInfo.artist == string.Empty ? "" : " - ") +
                                       beatmapInfo.title.Replace("&", "&&") +
                                       (beatmapInfo.title == string.Empty ? "" : " [") +
                                       beatmapInfo.version.Replace("&", "&&") +
                                       (beatmapInfo.version == string.Empty ? "" : "]"));
                    preBeatmapInfo.tags = beatmapInfo.tags == null ? [""] : beatmapInfo.tags;
                    txtTags.Text = beatmapInfo.tags == null ? "" : string.Join(" ", beatmapInfo.tags);
                    // バックアップフォルダ名を設定する
                    backupDirectoryName = beatmapInfo.artist + " - " +
                                          beatmapInfo.title + " (" +
                                          beatmapInfo.creator + ") [" +
                                          beatmapInfo.version + "]";
                    try
                    {
                        DebugForm?.GetBeatmap();
                    }
                    catch
                    {
                    }
                    preBeatmapInfo.version = beatmapInfo.version;
                    preBeatmapInfo.previewTime = beatmapInfo.previewTime;
                    preBeatmapInfo.beatmapPath = beatmapInfo.beatmapPath;
                    preBeatmapInfo.lastUpdate = beatmapInfo.lastUpdate;
                    // BGのパスが取得できている場合はBGをフォームに表示する
                    if (beatmapInfo.backgroundPath == null || beatmapInfo.backgroundPath == string.Empty)
                    {
                        picDisplayBg.Image = null;
                        Common.WriteWarningMessage("LOG_W-GET-BG");
                    }
                    else
                    {
                        picDisplayBg.Image = BeatmapHelper.SetBgOnForm(beatmapInfo.backgroundPath);
                    }
                }
                catch (Exception ex)
                {
                    // このExceptionはErrorログに書き込むと容量が膨大になってしまうため、
                    // Errorログには書かずConsoleログに出力
                    Console.WriteLine(ex);
                }
            }
        }
        /// <summary>
        /// コントロールの初期化処理
        /// </summary>
        private void InitializeControls()
        {
            cmbHitsoundTool.SelectedIndex = 0;
            cmbNotesPosition.SelectedIndex = 0;
            cmbSettingCopier.SelectedIndex = 0;
            cmbResnapBeatSnap.SelectedIndex = 0;
            picSpecificNormalDong.Image = Properties.Resources.d;
            picSpecificNormalKa.Image = Properties.Resources.k;
            picSpecificNormalSlider.Image = Properties.Resources.slider;
            picSpecificNormalSpinner.Image = Properties.Resources.spinner;
            picSpecificFinisherDong.Image = Properties.Resources.d;
            picSpecificFinisherKa.Image = Properties.Resources.k;
            picSpecificFinisherSlider.Image = Properties.Resources.slider;
            pnlGroupSvEditor.Visible = true;
            pnlGroupUtility.Visible = false;
            picSpecificNormalDong.Visible = false;
            picSpecificFinisherDong.Visible = false;
            picSpecificNormalKa.Visible = false;
            picSpecificFinisherKa.Visible = false;
            picSpecificNormalSlider.Visible = false;
            picSpecificFinisherSlider.Visible = false;
            picSpecificNormalSpinner.Visible = false;
            lblSpecificNormal.Visible = false;
            lblSpecificFinisher.Visible = false;
            lblSpecificGridLine.Visible = false;
            lblSpecificGridLine2.Visible = false;
            rdoAllHitObjects.Checked = true;
            if (config.advanceMode == 1)
            {
                chkRelative.Visible = true;
                menuStrip1.Items.Clear();
                menuStrip1.Items.AddRange([sVEditorToolStripMenuItem, utilityToolStripMenuItem, timingPropertyToolStripMenuItem]);
            }
            else
            {
                chkRelative.Visible = false;
                menuStrip1.Items.Clear();
                menuStrip1.Items.AddRange([sVEditorToolStripMenuItem, utilityToolStripMenuItem]);
            }
        }
        /// <summary>
        /// 処理項目タブが"Objectsのみ"の時のコントロールの初期化処理
        /// </summary>
        private void InitializeHitObjectsControls()
        {
            rdoAllHitObjects.Checked = userInputTempData.setObjectOption.isAllHitObjects;
            rdoOnlyBarline.Checked = userInputTempData.setObjectOption.isOnlyBarlines;
            rdoOnlyBookMark.Checked = userInputTempData.setObjectOption.isOnlyBookmarks;
            rdoOnlySpecificHitObject.Checked = userInputTempData.setObjectOption.isOnlyHitObjects;
            chkEnableOffset.Enabled = true;
            txtOffset.Enabled = true;
            txtOffset.BackColor = SystemColors.Window;
            txtOffset.ForeColor = SystemColors.WindowText;
            Common.SetLabelText(chkEnableOffset, "LBL_APPLY_OFFSET");
        }
        /// <summary>
        /// 処理項目タブが"ビートスナップ間隔"の時のコントロールの初期化処理
        /// </summary>
        private void InitializeBeatSnapControls()
        {
            chkEnableOffset.Enabled = false;
            txtOffset.Enabled = false;
            txtOffset.BackColor = SystemColors.WindowFrame;
            txtOffset.ForeColor = txtSvFrom.BackColor;
        }
        /// <summary>
        /// 処理項目タブが"緑線"の時のコントロールの初期化処理
        /// </summary>
        private void InitializeGreenLineControls()
        {
            chkEnableOffset.Enabled = false;
            txtOffset.Enabled = false;
            txtOffset.BackColor = SystemColors.WindowFrame;
            txtOffset.ForeColor = txtSvFrom.BackColor;
        }
        /// <summary>
        /// メイン処理(SV Editor)
        /// </summary>
        /// <param name="executeCode">実行コード</param>
        private void ExecuteSvEditProcess(int executeCode)
        {
            bool isSvCalculation = false;
            // 入力値と譜面情報が取得できていない場合はエラーダイアログを表示する
            if (userInputData == null || beatmapData == null)
            {
                Common.ShowMessageDialog("E_A-D-1");
                return;
            }
            // 実行コードにSV処理を行う
            switch (executeCode)
            {
                // 適用
                case Constants.EXECUTE_APPLY:
                    isSvCalculation = SVCalculatorService.Apply(userInputData, beatmapData, ref timingPoints);
                    beatmapData.timingPoints.AddRange(timingPoints);
                    beatmapData.timingPoints = beatmapData.timingPoints.OrderBy(a => a.time).ThenByDescending(b => b.isRedLine ? 1 : 0).ToList();
                    timingPoints.Clear();
                    break;
                // 削除
                case Constants.EXECUTE_REMOVE:
                    isSvCalculation = SVCalculatorService.Remove(userInputData, beatmapData);
                    break;
            }
            if (!isSvCalculation)
            {
                // 失敗した場合はエラーダイアログを表示する
                Common.ShowMessageDialog("E_A-P-1");
                return;
            }
            // バックアップを作成する
            if (BeatmapHelper.CreateBackup(this.beatmapInfo.beatmapPath, this.backupDirectoryName))
            {
                if (!SettingHelper.ResetBackupFile(config))
                {
                    // 失敗した場合はエラーダイアログを表示する
                    Common.ShowMessageDialog("E_A-P-1");
                    return;
                }
            }
            // デバッグ用CSV出力
            // 内容確認などに使ってね
            //if (!Utils.Helper.Debug.ExportToCsvFile(beatmapData, this.backupDirectoryName))
            // osuファイルに上書きする
            if (!BeatmapHelper.ExportToOsuFile(beatmapData, this.beatmapInfo.beatmapPath))
            {
                // 失敗した場合はエラーダイアログを表示する
                Common.ShowMessageDialog("E_A-P-1");
                return;
            }
#if DEBUG
            // デバッグ用ユーザー入力データ出力
            if (!Utils.Helper.Debug.ExportToUserInputData(userInputData))
            {
                // 失敗した場合はエラーダイアログを表示する
                Common.ShowMessageDialog("E_A-P-1");
                return;
            }
            DebugForm?.GetBeatmap();
#endif
            // 成功した場合はメッセージダイアログを表示する
            Common.ShowMessageDialog("I_A-P-1", Constants.DIALOG_OPTION_MODELESS);
            this.Activate();
            return;
        }
        /// <summary>
        /// メイン処理(Utility)
        /// </summary>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private void ExecuteUtilityProcess()
        {
            // 譜面情報がリアルタイムで取得できていない場合はエラーダイアログを表示する
            if (this.beatmapInfo.beatmapPath == null || this.beatmapInfo.beatmapPath == string.Empty)
            {
                return;
            }
            string beatmapDirectory = Path.GetDirectoryName(this.beatmapInfo.beatmapPath) ?? string.Empty;
            string[] beatmapsPath = Directory.GetFiles(beatmapDirectory, "*" + Constants.OSU_EXTENSION, SearchOption.TopDirectoryOnly);
            switch (userInputUtilityData.utilityCode)
            {
                case Constants.UTILITY_OFFSET:
                    if (!UserInputUtilityDataHelper.SetOffsetData(userInputUtilityData,
                                                                  txtTimingOffset.Text))
                    {
                        return;
                    }
                    if (string.IsNullOrEmpty(beatmapDirectory))
                    {
                        return;
                    }
                    foreach (var beatmapPath in beatmapsPath)
                    {
                        // マップセットの内容を取得する
                        var tempBeatmap = BeatmapHelper.GetBeatmapData(beatmapPath);

                        // 出力
                        if (!BeatmapHelper.ExportToOsuFile(tempBeatmap, beatmapPath, userInputUtilityData, beatmapInfo))
                        {
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                        }
                    }
                    break;
                case Constants.UTILITY_TAG_EDIT:
                case Constants.UTILITY_SETTING_COPIER:
                    var tags = txtTags.Text.Replace("\n", "");
                    if (string.IsNullOrEmpty(beatmapDirectory) ||
                        (userInputUtilityData.utilityCode == Constants.UTILITY_SETTING_COPIER && beatmapData == null))
                    {
                        return;
                    }
                    var timingPoints = beatmapData?.timingPoints.Where(tp => tp.isRedLine).ToList();
                    foreach (var beatmapPath in beatmapsPath)
                    {
                        // マップセットの内容を取得する
                        var tempBeatmap = BeatmapHelper.GetBeatmapData(beatmapPath);

                        // 出力
                        if (!BeatmapHelper.ExportToOsuFile(tempBeatmap, beatmapPath, userInputUtilityData, beatmapInfo, timingPoints))
                        {
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                        }
                    }
                    break;
                case Constants.UTILITY_HITSOUND:
                    // 譜面情報が取得できていない場合はエラーダイアログを表示する
                    if (beatmapData == null)
                    {
                        Common.ShowMessageDialog("E_A-D-1");
                        return;
                    }
                    // バックアップを作成する
                    if (BeatmapHelper.CreateBackup(this.beatmapInfo.beatmapPath, this.backupDirectoryName))
                    {
                        if (!SettingHelper.ResetBackupFile(config))
                        {
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                        }
                    }
                    if (!BeatmapHelper.ExportToOsuFile(beatmapData, this.beatmapInfo.beatmapPath, userInputUtilityData))
                    {
                        // 失敗した場合はエラーダイアログを表示する
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    break;
                case Constants.UTILITY_NOTES_POSITION:
                    // 譜面情報が取得できていない場合はエラーダイアログを表示する
                    if (beatmapData == null)
                    {
                        Common.ShowMessageDialog("E_A-D-1");
                        return;
                    }
                    userInputUtilityData.notesPositionCode = cmbNotesPosition.SelectedIndex;
                    userInputUtilityData.donX = config.donX;
                    userInputUtilityData.donY = config.donY;
                    userInputUtilityData.katX = config.katX;
                    userInputUtilityData.katY = config.katY;
                    userInputUtilityData.finisherDonX = config.finisherDonX;
                    userInputUtilityData.finisherDonY = config.finisherDonY;
                    userInputUtilityData.finisherKatX = config.finisherKatX;
                    userInputUtilityData.finisherKatY = config.finisherKatY;
                    // バックアップを作成する
                    if (BeatmapHelper.CreateBackup(this.beatmapInfo.beatmapPath, this.backupDirectoryName))
                    {
                        if (!SettingHelper.ResetBackupFile(config))
                        {
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                        }
                    }
                    if (!BeatmapHelper.ExportToOsuFile(beatmapData, this.beatmapInfo.beatmapPath, userInputUtilityData))
                    {
                        // 失敗した場合はエラーダイアログを表示する
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    break;
                case Constants.UTILITY_RESNAP:
                    // 譜面情報が取得できていない場合はエラーダイアログを表示する
                    if (beatmapData == null)
                    {
                        Common.ShowMessageDialog("E_A-D-1");
                        return;
                    }
                    // 入力値をUserInputDataクラスに格納
                    switch (UserInputUtilityDataHelper.SetResnapTimingData(userInputUtilityData,
                                                                           txtResnapTimingFrom.Text,
                                                                           txtResnapTimingTo.Text))
                    {
                        case 1:
                            // 正常終了した場合、先の処理を続行
                            break;
                        default:
                            // 準正常/異常終了した場合は、処理を終了
                            return;
                    }
                    if (!ResnapService.Apply(userInputUtilityData, beatmapData))
                    {
                        // 失敗した場合はエラーダイアログを表示する
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    // バックアップを作成する
                    if (BeatmapHelper.CreateBackup(this.beatmapInfo.beatmapPath, this.backupDirectoryName))
                    {
                        if (!SettingHelper.ResetBackupFile(config))
                        {
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                        }
                    }
                    if (!BeatmapHelper.ExportToOsuFile(beatmapData, this.beatmapInfo.beatmapPath, userInputUtilityData))
                    {
                        // 失敗した場合はエラーダイアログを表示する
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    break;
            }
            try
            {
                DebugForm?.GetBeatmap();
            }
            catch
            {
            }
            // 成功した場合はメッセージダイアログを表示する
            Common.ShowMessageDialog("I_A-P-1", Constants.DIALOG_OPTION_MODELESS);
            this.Activate();
            return;
        }
        /// <summary>
        /// 譜面情報の取得処理
        /// </summary>
        /// <returns>処理が<br/>・正常終了した場合はtrue<br/>・異常終了した場合はfalse</returns>
        private bool GetBeatmap()
        {
            // 譜面情報がリアルタイムで取得できていない場合はエラーダイアログを表示する
            if (this.beatmapInfo.beatmapPath == null || this.beatmapInfo.beatmapPath == string.Empty)
            {
                return false;
            }
            // 譜面の内容を取得する
            beatmapData = BeatmapHelper.GetBeatmapData(this.beatmapInfo.beatmapPath);
#if DEBUG
            Utils.Helper.Debug.ExportHitObejcts(beatmapData);
#endif
            // 取得できなかった場合はエラーダイアログを表示する
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
                if (setCode != Constants.SET_TIMING)
                {
                    if (!GetBeatmap())
                    {
                        throw new Exception();
                    }
                }
                switch (setCode)
                {
                    case Constants.SET_TIMING:
                        control.Text = Common.ConvertFormatTiming(currentTime);
                        break;
                    case Constants.SET_SV:
                        control.Text = UserInputDataHelper.SetCurrentSv(beatmapData, currentTime);
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
            finally
            {
                beatmapData = null;
            }
        }
        /// <summary>
        /// コンフィグの初期化処理
        /// </summary>
        private void InitializeConfig()
        {
            config.ConfigLoad();
            Common.LoadConfig(config);
            preUnicodeSupport = config.unicodeSupport;
            this.Text = Constants.APP_NAME + " ver" + Constants.APP_VERSION;
        }
        /// <summary>
        /// ラベルテキストの初期化処理
        /// </summary>
        private void InitializeLabelText()
        {
            // MainForm(共通)
            Common.SetLabelText(btnBackup, "LBL_BACKUPS");
            Common.SetLabelText(btnViewSetting, "LBL_SETTINGS");
            // MainForm(SVEditor)
            Common.SetLabelText(chkApplyStartObject, "LBL_APPLY_TIMING_FROM");
            Common.SetLabelText(chkApplyEndObject, "LBL_APPLY_TIMING_TO");
            Common.SetLabelText(lblCalculationType, "LBL_SV_CALCULATION");
            Common.SetLabelText(rdoArithmetic, "LBL_LINEAR");
            Common.SetLabelText(rdoGeometric, "LBL_GEOMETRIC");
            Common.SetLabelText(chkRelative, "LBL_RELATIVE_MODE");
            Common.SetLabelText(rdoRelativeMultiply, "LBL_RELATIVE_MULTIPLY");
            Common.SetLabelText(rdoRelativeSum, "LBL_RELATIVE_SUM");
            Common.SetLabelText(lblRelativeBaseSv, "LBL_RELATIVE_BASESV");
            Common.SetLabelText(chkEnableSvTo, "LBL_RELATIVE_ENABLE_TIMING_TO");
            Common.SetLabelText(tabApplyPage, "LBL_TAB_APPLY");
            Common.SetLabelText(tabRemovePage, "LBL_TAB_DELETE");
            Common.SetLabelText(chkEnableOffset, "LBL_APPLY_OFFSET");
            Common.SetLabelText(btnApply, "LBL_EXECUTE");
            Common.SetLabelText(tabHitObjectsPage, "LBL_APPLY_TAB_OBJECTS");
            Common.SetLabelText(tabBeatSnap, "LBL_APPLY_TAB_BEATSNAPS");
            Common.SetLabelText(tabGreenLine, "LBL_APPLY_TAB_INHERITED_POINTS");
            Common.SetLabelText(rdoAllHitObjects, "LBL_OBJECTS_ALL_HITOBJECTS");
            Common.SetLabelText(rdoOnlyBarline, "LBL_OBJECTS_BARLINES");
            Common.SetLabelText(rdoOnlyBookMark, "LBL_OBJECTS_BOOKMARKS");
            if (rdoOnlyBarline.Checked)
            {
                Common.SetLabelText(rdoOnlyOnNotes, "LBL_OBJECTS_ON_BARLINES");
                Common.SetLabelText(rdoOnlyOffNotes, "LBL_OBJECTS_OFF_BARLINES");
            }
            else if (rdoOnlyBookMark.Checked)
            {
                Common.SetLabelText(rdoOnlyOnNotes, "LBL_OBJECTS_ON_BOOKMARKS");
                Common.SetLabelText(rdoOnlyOffNotes, "LBL_OBJECTS_OFF_BOOKMARKS");
            }
            Common.SetLabelText(rdoOnlySpecificHitObject, "LBL_OBJECTS_SPECIFIC_HITOBJECTS");
            Common.SetLabelText(lblSpecificNormal, "LBL_OBJECTS_NORMAL");
            Common.SetLabelText(lblSpecificFinisher, "LBL_OBJECTS_FINISHER");
            Common.SetLabelText(lblBeatSnaps, "LBL_BEATSNAPS_DIVISOR");
            Common.SetLabelText(chkEnableOffset, "LBL_APPLY_OFFSET");
            Common.SetLabelText(chkEnableStartOffset, "LBL_DELETE_OFFSET");
            Common.SetLabelText(btnRemove, "LBL_EXECUTE");
            // MainForm(Utility)
            Common.SetLabelText(lblOffsetShifter, "LBL_OFFSET_SHIFTER");
            Common.SetLabelText(lblHitsoundOption, "LBL_HITSOUND_OPTION");
            Common.SetLabelText(lblNotesPosition, "LBL_NOTES_POSITION");
            Common.SetLabelText(lblTagEditor, "LBL_TAG_EDITOR");
            Common.SetLabelText(lblSettingCopier, "LBL_SETTING_COPIER");
            Common.SetLabelText(lblResnap, "LBL_RESNAP");
            Common.SetLabelText(lblBeatSnapDivisor, "LBL_BEAT_SNAP_DIVISOR");
            Common.SetLabelText(btnApplyOffsetShifter, "LBL_UTILITY_APPLY");
            Common.SetLabelText(btnApplyHitsoundOption, "LBL_UTILITY_APPLY");
            Common.SetLabelText(btnApplyNotesPosition, "LBL_UTILITY_APPLY");
            Common.SetLabelText(btnApplyTagEditor, "LBL_UTILITY_APPLY");
            Common.SetLabelText(btnApplySettingCopier, "LBL_UTILITY_APPLY");
            Common.SetLabelText(btnApplyResnap, "LBL_UTILITY_APPLY");
        }
        /// <summary>
        /// SV始点テキストボックスの有効/無効状態に応じたUI設定処理
        /// </summary>
        /// <param name="isEnable">有効化フラグ</param>
        private void SetTxtSvFromEnabledState(bool isEnable)
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
        private void SetTxtSvToEnabledState(bool isEnable)
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
        private void SetBtnSwapSvEnabledState(bool isEnable)
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
        /// <param name="isEnable"></param>
        private void SetBtnSetSvFromEnabledState(bool isEnable)
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
        /// <param name="isEnable"></param>
        private void SetBtnSetSvToEnabledState(bool isEnable)
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
        private void SetTxtVolumeFromEnabledState(bool isEnable)
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
        private void SetTxtVolumeToEnabledState(bool isEnable)
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
        private void SetBtnSwapVolumeEnabledState(bool isEnable)
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
        /// <param name="isEnable"></param>
        private void SetBtnSetVolumeFromEnabledState(bool isEnable)
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
        /// <param name="isEnable"></param>
        private void SetBtnSetVolumeToEnabledState(bool isEnable)
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
        private void SetRelativeEnabledState(bool isEnable)
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
        private void SetRelativeMultiplyEnableState(bool isEnable)
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
        private void SetChkEnableOffset(bool isEnable)
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
        private void SetApplyControls(bool isEnable)
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
                SetTxtSvFromEnabledState(userInputTempData.isSv);
                SetTxtSvToEnabledState(userInputTempData.isSv);
                SetBtnSwapSvEnabledState(userInputTempData.isSv);
                SetBtnSetSvFromEnabledState(userInputTempData.isSv);
                SetBtnSetSvToEnabledState(userInputTempData.isSv);
            }
            else
            {
                if (userInputTempData.isRelative)
                {
                    chkEnableSv.Enabled = false;
                    SetTxtSvToEnabledState(isEnable ? userInputTempData.isEnableRelativeEnd : isEnable);
                    SetBtnSwapSvEnabledState(isEnable ? userInputTempData.isEnableRelativeEnd : isEnable);
                    SetBtnSetSvToEnabledState(isEnable ? userInputTempData.isEnableRelativeEnd : isEnable);
                }
                else
                {
                    SetTxtSvToEnabledState(isEnable);
                    SetBtnSwapSvEnabledState(isEnable);
                    SetBtnSetSvToEnabledState(isEnable);
                }
                SetTxtSvFromEnabledState(isEnable);
                SetBtnSetSvFromEnabledState(isEnable);
            }
            if (isEnable && !userInputTempData.isVolume)
            {
                SetTxtVolumeFromEnabledState(userInputTempData.isVolume);
                SetTxtVolumeToEnabledState(userInputTempData.isVolume);
                SetBtnSwapVolumeEnabledState(userInputTempData.isVolume);
                SetBtnSetVolumeFromEnabledState(userInputTempData.isVolume);
                SetBtnSetVolumeToEnabledState(userInputTempData.isVolume);
            }
            else
            {
                SetTxtVolumeFromEnabledState(isEnable);
                SetTxtVolumeToEnabledState(isEnable);
                SetBtnSwapVolumeEnabledState(isEnable);
                SetBtnSetVolumeFromEnabledState(isEnable);
                SetBtnSetVolumeToEnabledState(isEnable);
            }

        }
        /// <summary>
        /// Offset有効化チェックボックス(削除)の有効/無効状態に応じたUI設定処理
        /// </summary>
        private void SetChkEnableStartOffset(bool isEnable)
        {
            if (isEnable)
            {
                txtStartOffset.Enabled = true;
                txtStartOffset.BackColor = SystemColors.Window;
                txtStartOffset.ForeColor = SystemColors.WindowText;
            }
            else
            {
                txtStartOffset.Enabled = false;
                txtStartOffset.BackColor = SystemColors.WindowFrame;
                txtStartOffset.ForeColor = SystemColors.WindowFrame;
            }
        }
        #endregion
        #region イベントハンドラ
        #region 共通機能のイベントハンドラ
        private void osu_taiko_Mapping_Helper_Load(object sender, EventArgs e)
        {
            Common.InitializeDirectoryAndFiles();
            Common.WriteInfoMessage("LOG_I-START");
            // ApplicationExitイベントハンドラを追加
            Application.ApplicationExit += new EventHandler(Application_ApplicationExit);
            // Configファイル設定読み込み
            InitializeConfig();
            // それぞれのコントロールの初期化処理
            InitializeControls();
            InitializeLabelText();
            picDisplayBg.Controls.Add(lblFileName);
            pnlRelativeSvGroup.Visible = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
        }
        private void osu_taiko_Mapping_Helper_Shown(object sender, EventArgs e)
        {
            // 画面がタスクバーと被らないように位置の変更をする
            if (this.WindowState == System.Windows.Forms.FormWindowState.Normal)
            {
                var rect = System.Windows.Forms.Screen.GetWorkingArea(this);
                this.Left = (rect.Left + rect.Width > this.Left + this.Width) ? this.Left : rect.Left + rect.Width - this.Width;
                this.Left = (rect.Left < this.Left) ? this.Left : rect.Left;
                this.Top = (rect.Top + rect.Height > this.Top + this.Height) ? this.Top : rect.Top + rect.Height - this.Height;
                this.Top = (rect.Top < this.Top) ? this.Top : rect.Top;
            }
        }
        private void osu_taiko_Mapping_Helper_KeyDown(object sender, KeyEventArgs e)
        {
            string backupPath = Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY + "\\" + this.backupDirectoryName;
            switch (e.KeyData)
            {
                //［Ctrl］+［S］が押されたらSV適用/削除を実行する
                case (Keys.S | Keys.Control):
                    if (pnlGroupSvEditor.Visible)
                    {
                        switch (tabExecuteType.SelectedIndex)
                        {
                            case 0:
                                btnApply_Click(sender, e);
                                break;
                            case 1:
                                btnRemove_Click(sender, e);
                                break;
                            default:
                                break;
                        }
                    }
                    break;
                //［Ctrl］+［Z］が押されたら実行前の譜面にする
                case (Keys.Z | Keys.Control):
                    // 譜面情報がリアルタイムで取得できていない場合はエラーダイアログを表示する
                    if (this.beatmapInfo.beatmapPath == null || this.beatmapInfo.beatmapPath == string.Empty)
                    {
                        Common.ShowMessageDialog("E_A-D-1");
                        return;
                    }
                    // バックアップディレクトリが見つからない場合は何もしない
                    if (!Directory.Exists(backupPath))
                    {
                        break;
                    }
                    if (BeatmapHelper.ExportToPreviousOsuFile(this.beatmapInfo.beatmapPath, this.backupDirectoryName))
                    {
                        // 成功した場合は完了メッセージを表示する
                        Common.ShowMessageDialog("I_A-P-2", Constants.DIALOG_OPTION_MODELESS);
                    }
                    else
                    {
                        // 失敗した場合はエラーダイアログを表示する
                        Common.ShowMessageDialog("E_A-P-1");
                    }
                    break;
            }

        }
        private void Application_ApplicationExit(object? sender, EventArgs e)
        {
            //ApplicationExitイベントハンドラを削除
            Application.ApplicationExit -= new EventHandler(Application_ApplicationExit);
            config.ConfigSave();
            Common.WriteInfoMessage("LOG_I-END");
        }
        private void btnBackup_Click(object sender, EventArgs e)
        {
            // バックアップフォルダをエクスプローラで開く
            System.Diagnostics.Process.Start("EXPLORER.EXE", Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY);
        }
        private void btnViewSetting_Click(object sender, EventArgs e)
        {
            // 設定画面を表示する
            Form settingForm = new SettingForm(config);
            settingForm.ShowDialog();
            InitializeLabelText();
            try
            {
                DebugForm?.InitializeLabelText();
            } catch
            {
            }
            if (preUnicodeSupport != config.unicodeSupport)
            {
                // テキストラベルに譜面の情報を書き込む
                lblFileName.Text = config.unicodeSupport == 1 ?
                                   (beatmapInfo.artistUnicode.Replace("&", "&&") +
                                   (beatmapInfo.artistUnicode == string.Empty ? "" : " - ") +
                                   beatmapInfo.titleUnicode.Replace("&", "&&") +
                                   (beatmapInfo.titleUnicode == string.Empty ? "" : " [") +
                                   beatmapInfo.version.Replace("&", "&&") +
                                   (beatmapInfo.version == string.Empty ? "" : "]"))
                                   :
                                   (beatmapInfo.artist.Replace("&", "&&") +
                                   (beatmapInfo.artist == string.Empty ? "" : " - ") +
                                   beatmapInfo.title.Replace("&", "&&") +
                                   (beatmapInfo.title == string.Empty ? "" : " [") +
                                   beatmapInfo.version.Replace("&", "&&") +
                                   (beatmapInfo.version == string.Empty ? "" : "]"));
            }
            if (config.advanceMode == 1)
            {
                chkRelative.Visible = true;
                menuStrip1.Items.Clear();
                menuStrip1.Items.AddRange(new ToolStripItem[] { sVEditorToolStripMenuItem, utilityToolStripMenuItem, timingPropertyToolStripMenuItem });
            }
            else
            {
                chkRelative.Checked = false;
                chkRelative.Visible = false;
                menuStrip1.Items.Clear();
                menuStrip1.Items.AddRange([sVEditorToolStripMenuItem, utilityToolStripMenuItem]);
            }
            preUnicodeSupport = config.unicodeSupport;
        }
        private void sVEditorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlGroupSvEditor.Visible = true;
            pnlGroupUtility.Visible = false;
        }
        private void utilityToolStripMenuItem_Click(object sender, EventArgs e)
        {
            pnlGroupSvEditor.Visible = false;
            pnlGroupUtility.Visible = true;
        }
        private void timingPropertyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DebugForm?.Close();
            DebugForm?.Dispose();
            DebugForm = new DebugForm()
            {
                parentForm = this
            };
            DebugForm.Show();
            bool isGetBeatmap = false;
            while (!isGetBeatmap)
            {
                isGetBeatmap = DebugForm.GetBeatmap();
            }
            DebugForm.Text = "Timing Property";
        }
        #endregion
        #region SV Editorタブのイベントハンドラ
        private void txtTimingFrom_Enter(object sender, EventArgs e)
        {
            this.txtTimingFrom.SelectAll();
        }
        private void txtTimingTo_Enter(object sender, EventArgs e)
        {
            this.txtTimingTo.SelectAll();
        }
        private void txtTimingFrom_TextChanged(object sender, EventArgs e)
        {
            userInputTempData.timingFrom = txtTimingFrom.Text;
        }
        private void txtTimingTo_TextChanged(object sender, EventArgs e)
        {
            userInputTempData.timingTo = txtTimingTo.Text;
        }
        private void btnSetTimingFrom_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtTimingFrom, Constants.SET_TIMING);
        }
        private void btnSetTimingTo_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtTimingTo, Constants.SET_TIMING);
        }
        private void btnSwapTiming_Click(object sender, EventArgs e)
        {
            // Timingの始点と終点を入れ替える
            string timingBuff = "";
            timingBuff = txtTimingFrom.Text;
            txtTimingFrom.Text = txtTimingTo.Text;
            txtTimingTo.Text = timingBuff;
        }
        private void txtSvFrom_Enter(object sender, EventArgs e)
        {
            this.txtSvFrom.SelectAll();
        }
        private void txtSvTo_Enter(object sender, EventArgs e)
        {
            this.txtSvTo.SelectAll();
        }
        private void txtSvFrom_TextChanged(object? sender, EventArgs e)
        {
            if (userInputTempData.isRelative)
            {
                if (userInputTempData.isRelativeMultiply)
                {
                    userInputTempData.relativeMultiplySvFrom = txtSvFrom.Text;
                }
                else if (userInputTempData.isRelativeSum)
                {
                    userInputTempData.relativeSumSvFrom = txtSvFrom.Text;
                }
                else
                {
                    userInputTempData.svFrom = txtSvFrom.Text;
                }
            }
            else
            {
                userInputTempData.svFrom = txtSvFrom.Text;
            }
        }
        private void txtSvTo_TextChanged(object? sender, EventArgs e)
        {
            if (chkRelative.Checked)
            {
                if (rdoRelativeMultiply.Checked)
                {
                    userInputTempData.relativeMultiplySvTo = txtSvTo.Text;
                }
                else if (rdoRelativeSum.Checked)
                {
                    userInputTempData.relativeSumSvTo = txtSvTo.Text;
                }
                else
                {
                    userInputTempData.svTo = txtSvTo.Text;
                }
            }
            else
            {
                userInputTempData.svTo = txtSvTo.Text;
            }
        }
        private void btnSetSvFrom_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtSvFrom, Constants.SET_SV);
        }
        private void btnSetSvTo_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtSvTo, Constants.SET_SV);
        }
        private void chkEnableSv_CheckedChanged(object? sender, EventArgs e)
        {
            userInputTempData.isSv = chkEnableSv.Checked;
            SetTxtSvFromEnabledState(userInputTempData.isSv);
            SetTxtSvToEnabledState(userInputTempData.isSv);
            SetBtnSwapSvEnabledState(userInputTempData.isSv);
            SetBtnSetSvFromEnabledState(userInputTempData.isSv);
            SetBtnSetSvToEnabledState(userInputTempData.isSv);
            if (userInputTempData.isSv && config.advanceMode == 1)
            {
                chkRelative.Visible = true;
            }
            else
            {
                chkRelative.Visible = false;
                pnlRelativeSvGroup.Visible = false;
                chkEnableSvTo.Visible = false;
            }
        }
        private void btnSwapSv_Click(object sender, EventArgs e)
        {
            // SVの始点と終点を入れ替える
            string SVBuff = "";
            SVBuff = txtSvFrom.Text;
            txtSvFrom.Text = txtSvTo.Text;
            txtSvTo.Text = SVBuff;
        }
        private void txtVolumeFrom_Enter(object sender, EventArgs e)
        {
            this.txtVolumeFrom.SelectAll();
        }
        private void txtVolumeTo_Enter(object sender, EventArgs e)
        {
            this.txtVolumeTo.SelectAll();
        }
        private void txtVolumeFrom_TextChanged(object sender, EventArgs e)
        {
            userInputTempData.volumeFrom = txtVolumeFrom.Text;
        }
        private void txtVolumeTo_TextChanged(object sender, EventArgs e)
        {
            userInputTempData.volumeTo = txtVolumeTo.Text;
        }
        private void btnSetVolumeFrom_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtVolumeFrom, Constants.SET_VOLUME);
        }
        private void btnSetVolumeTo_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtVolumeTo, Constants.SET_VOLUME);
        }
        private void btnSwapVolume_Click(object sender, EventArgs e)
        {
            // Volumeの始点と終点を入れ替える
            string volumeBuff = "";
            volumeBuff = txtVolumeFrom.Text;
            txtVolumeFrom.Text = txtVolumeTo.Text;
            txtVolumeTo.Text = volumeBuff;

        }
        private void chkEnableVolume_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isVolume = chkEnableVolume.Checked;
            SetTxtVolumeFromEnabledState(userInputTempData.isVolume);
            SetTxtVolumeToEnabledState(userInputTempData.isVolume);
            SetBtnSwapVolumeEnabledState(userInputTempData.isVolume);
            SetBtnSetVolumeFromEnabledState(userInputTempData.isVolume);
            SetBtnSetVolumeToEnabledState(userInputTempData.isVolume);
        }
        private void chkApplyStartObject_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isEnableFrom = chkApplyStartObject.Checked;
        }
        private void chkApplyEndObject_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isEnableTo = chkApplyEndObject.Checked;
        }
        private void rdoArithmetic_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isArithmetic = rdoArithmetic.Checked;
        }
        private void rdoGeometric_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isGeometric = rdoGeometric.Checked;
        }
        private void chkRelative_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isRelative = chkRelative.Checked;
            bool isEnable = userInputTempData.isRelative ?
                userInputTempData.isEnableRelativeEnd : userInputTempData.isSv;
            SetRelativeEnabledState(userInputTempData.isRelative);
            SetTxtSvToEnabledState(isEnable);
            SetBtnSwapSvEnabledState(isEnable);
            SetBtnSetSvToEnabledState(isEnable);
        }
        private void rdoRelativeMultiply_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isRelativeMultiply = rdoRelativeMultiply.Checked;
            SetRelativeMultiplyEnableState(userInputTempData.isRelativeMultiply);
        }
        private void txtRelativeBaseSv_TextChanged(object sender, EventArgs e)
        {
            userInputTempData.relativeMultiplyBaseSv = txtRelativeBaseSv.Text;
        }
        private void rdoRelativeSum_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isRelativeSum = rdoRelativeSum.Checked;
            if (userInputTempData.isRelativeSum)
            {
                txtSvFrom.Text = userInputTempData.relativeSumSvFrom;
                txtSvTo.Text = userInputTempData.relativeSumSvTo;
            }
        }
        private void chkEnableSvTo_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isEnableRelativeEnd = chkEnableSvTo.Checked;
            SetTxtSvToEnabledState(userInputTempData.isEnableRelativeEnd);
            SetBtnSwapSvEnabledState(userInputTempData.isEnableRelativeEnd);
            SetBtnSetSvToEnabledState(userInputTempData.isEnableRelativeEnd);
        }
        private void chkEnableOffset_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isOffset = chkEnableOffset.Checked;
            SetChkEnableOffset(userInputTempData.isOffset);
        }
        private void txtOffset_TextChanged(object sender, EventArgs e)
        {
            userInputTempData.offset = txtOffset.Text;
        }
        private void chkEnableKiai_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isKiai = chkEnableKiai.Checked;
        }
        private void tabExecuteType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabExecuteType.SelectedIndex)
            {
                case 0:
                    // Apply
                    SetApplyControls(true);
                    break;
                case 1:
                    // Remove
                    SetApplyControls(false);
                    break;
                default:
                    break;
            }
        }
        private void tabSetType_DrawItem(object sender, DrawItemEventArgs e)
        {
            //対象のTabControlを取得
            TabControl tab = (TabControl)sender;
            //タブページのテキストを取得
            string txt = tab.TabPages[e.Index].Text;

            //タブのテキストと背景を描画するためのブラシを決定する
            Brush foreBrush, backBrush;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                //選択されているタブのテキストを黒、背景をライトグレーとする
                foreBrush = Brushes.Black;
                backBrush = Brushes.LightGray;
            }
            else
            {
                //選択されていないタブのテキストは黒、背景を白とする
                foreBrush = Brushes.Black;
                backBrush = Brushes.White;
            }

            //StringFormatを作成
            StringFormat sf = new StringFormat();
            //中央に表示する
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            //背景の描画
            e.Graphics.FillRectangle(backBrush, e.Bounds);
            //Textの描画
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            e.Graphics.DrawString(txt, e.Font, foreBrush, e.Bounds, sf);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#if DEBUG
            System.Diagnostics.Debug.WriteLine(e.Font);
#endif
        }
        private void tabExecuteType_DrawItem(object sender, DrawItemEventArgs e)
        {
            //対象のTabControlを取得
            TabControl tab = (TabControl)sender;
            //タブページのテキストを取得
            string txt = tab.TabPages[e.Index].Text;

            //タブのテキストと背景を描画するためのブラシを決定する
            Brush foreBrush, backBrush;
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            {
                //選択されているタブのテキストを黒、背景をライトグレーとする
                foreBrush = Brushes.Black;
                backBrush = Brushes.LightGray;
            }
            else
            {
                //選択されていないタブのテキストは黒、背景を白とする
                foreBrush = Brushes.Black;
                backBrush = Brushes.White;
            }

            //StringFormatを作成
            StringFormat sf = new StringFormat();
            //中央に表示する
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;

            //背景の描画
            e.Graphics.FillRectangle(backBrush, e.Bounds);
#pragma warning disable CS8604 // Null 参照引数の可能性があります。
            e.Graphics.DrawString(txt, e.Font, foreBrush, e.Bounds, sf);
#pragma warning restore CS8604 // Null 参照引数の可能性があります。
#if DEBUG
            System.Diagnostics.Debug.WriteLine(e.Font);
#endif
        }
        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                // 入力値をUserInputDataクラスに格納
                if (!UserInputDataHelper.SetUserInputData(userInputTempData,
                                                          Constants.EXECUTE_APPLY,
                                                          ref userInputData))
                {
                    return;
                }
                // 譜面情報の取得
                if (!GetBeatmap())
                {
                    Common.ShowMessageDialog("E_A-D-1");
                    return;
                }
                // 追加処理の実行
                ExecuteSvEditProcess(Constants.EXECUTE_APPLY);
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return;
            }
            finally
            {
                beatmapData = null;
                userInputData = new();
            }
        }
        private void tabSetType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabSetType.SelectedIndex)
            {
                case 0:
                    // HitObjects
                    userInputTempData.setOption.isSetObjects = true;
                    userInputTempData.setOption.isSetBeatSnap = false;
                    userInputTempData.setOption.isSetGreenLine = false;
                    InitializeHitObjectsControls();
                    break;
                case 1:
                    // BeatSnap
                    userInputTempData.setOption.isSetObjects = false;
                    userInputTempData.setOption.isSetBeatSnap = true;
                    userInputTempData.setOption.isSetGreenLine = false;
                    InitializeBeatSnapControls();
                    break;
                case 2:
                    // GreenLine
                    userInputTempData.setOption.isSetObjects = false;
                    userInputTempData.setOption.isSetBeatSnap = false;
                    userInputTempData.setOption.isSetGreenLine = true;
                    InitializeGreenLineControls();
                    break;
                default:
                    break;
            }
        }
        private void rdoAllHitObjects_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoAllHitObjects.Checked)
            {
                // すべてのobject関連のコントロールを有効化
                rdoOnlyOnNotes.Visible = false;
                rdoOnlyOffNotes.Visible = false;
            }
            userInputTempData.setObjectOption.isAllHitObjects = rdoAllHitObjects.Checked;
        }
        private void rdoOnlyBarline_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOnlyBarline.Checked)
            {
                rdoOnlyOnNotes.Visible = true;
                rdoOnlyOffNotes.Visible = true;
                rdoOnlyOnNotes.Checked = userInputTempData.setObjectOption.isOnBarlines;
                rdoOnlyOffNotes.Checked = userInputTempData.setObjectOption.isOffBarlines;
                rdoOnlyOnNotes.CheckedChanged += rdoOnlyOnNotes_CheckedChanged;
                rdoOnlyOffNotes.CheckedChanged += rdoOnlyOffNotes_CheckedChanged;
                Common.SetLabelText(rdoOnlyOnNotes, "LBL_OBJECTS_ON_BARLINES");
                Common.SetLabelText(rdoOnlyOffNotes, "LBL_OBJECTS_OFF_BARLINES");
            }
            else
            {
                rdoOnlyOnNotes.CheckedChanged -= rdoOnlyOnNotes_CheckedChanged;
                rdoOnlyOffNotes.CheckedChanged -= rdoOnlyOffNotes_CheckedChanged;
            }
            userInputTempData.setObjectOption.isOnlyBarlines = rdoOnlyBarline.Checked;
        }
        private void rdoOnlyBookMark_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOnlyBookMark.Checked)
            {
                // Tabコントロール内のコントロールをすべて無効化
                chkEnableOffset.Visible = false;
                txtOffset.Visible = false;
                lblMiliSecond.Visible = false;
                rdoOnlyOnNotes.Visible = true;
                rdoOnlyOffNotes.Visible = true;
                rdoOnlyOnNotes.Checked = userInputTempData.setObjectOption.isOnBookmarks;
                rdoOnlyOffNotes.Checked = userInputTempData.setObjectOption.isOffBookmarks;
                rdoOnlyOnNotes.CheckedChanged += rdoOnlyOnNotes_CheckedChanged;
                rdoOnlyOffNotes.CheckedChanged += rdoOnlyOffNotes_CheckedChanged;
                Common.SetLabelText(rdoOnlyOnNotes, "LBL_OBJECTS_ON_BOOKMARKS");
                Common.SetLabelText(rdoOnlyOffNotes, "LBL_OBJECTS_OFF_BOOKMARKS");
            }
            else
            {
                // Tabコントロール内のコントロールを有効化
                chkEnableOffset.Visible = true;
                txtOffset.Visible = true;
                lblMiliSecond.Visible = true;
                rdoOnlyOnNotes.CheckedChanged -= rdoOnlyOnNotes_CheckedChanged;
                rdoOnlyOffNotes.CheckedChanged -= rdoOnlyOffNotes_CheckedChanged;
            }
            userInputTempData.setObjectOption.isOnlyBookmarks = rdoOnlyBookMark.Checked;
        }
        private void rdoOnlyOnNotes_CheckedChanged(object? sender, EventArgs e)
        {
            if (rdoOnlyBarline.Checked)
            {
                userInputTempData.setObjectOption.isOnBarlines = rdoOnlyOnNotes.Checked;
            }
            else if (rdoOnlyBookMark.Checked)
            {
                userInputTempData.setObjectOption.isOnBookmarks = rdoOnlyOnNotes.Checked;
            }
        }
        private void rdoOnlyOffNotes_CheckedChanged(object? sender, EventArgs e)
        {
            if (rdoOnlyBarline.Checked)
            {
                userInputTempData.setObjectOption.isOffBarlines = rdoOnlyOffNotes.Checked;
            }
            else if (rdoOnlyBookMark.Checked)
            {
                userInputTempData.setObjectOption.isOffBookmarks = rdoOnlyOffNotes.Checked;
            }
        }
        private void rdoOnlySpecificHitObject_CheckedChanged(object sender, EventArgs e)
        {
            if (rdoOnlySpecificHitObject.Checked)
            {
                // 特定のobject関連のコントロールを有効化
                picSpecificNormalDong.Visible = true;
                picSpecificFinisherDong.Visible = true;
                picSpecificNormalKa.Visible = true;
                picSpecificFinisherKa.Visible = true;
                picSpecificNormalSlider.Visible = true;
                picSpecificFinisherSlider.Visible = true;
                picSpecificNormalSpinner.Visible = true;
                lblSpecificNormal.Visible = true;
                lblSpecificFinisher.Visible = true;
                lblSpecificGridLine.Visible = true;
                lblSpecificGridLine2.Visible = true;
                // pictureBoxに設定されているMouseDownイベントを設定する
                picSpecificNormalDong.MouseDown += picSpecificNormalDong_MouseDown;
                picSpecificFinisherDong.MouseDown += picSpecificFinisherDong_MouseDown;
                picSpecificNormalKa.MouseDown += picSpecificNormalKa_MouseDown;
                picSpecificFinisherKa.MouseDown += picSpecificFinisherKa_MouseDown;
                picSpecificNormalSlider.MouseDown += picSpecificNormalSlider_MouseDown;
                picSpecificFinisherSlider.MouseDown += picSpecificFinisherSlider_MouseDown;
                picSpecificNormalSpinner.MouseDown += picSpecificNormalSpinner_MouseDown;
                rdoOnlyOnNotes.Visible = false;
                rdoOnlyOffNotes.Visible = false;
            }
            else
            {
                // 特定のobject関連のコントロールを無効化
                picSpecificNormalDong.Visible = false;
                picSpecificFinisherDong.Visible = false;
                picSpecificNormalKa.Visible = false;
                picSpecificFinisherKa.Visible = false;
                picSpecificNormalSlider.Visible = false;
                picSpecificFinisherSlider.Visible = false;
                picSpecificNormalSpinner.Visible = false;
                lblSpecificNormal.Visible = false;
                lblSpecificFinisher.Visible = false;
                lblSpecificGridLine.Visible = false;
                lblSpecificGridLine2.Visible = false;
                // pictureBoxに設定されているMouseDownイベントを外す
                picSpecificNormalDong.MouseDown -= picSpecificNormalDong_MouseDown;
                picSpecificFinisherDong.MouseDown -= picSpecificFinisherDong_MouseDown;
                picSpecificNormalKa.MouseDown -= picSpecificNormalKa_MouseDown;
                picSpecificFinisherKa.MouseDown -= picSpecificFinisherKa_MouseDown;
                picSpecificNormalSlider.MouseDown -= picSpecificNormalSlider_MouseDown;
                picSpecificFinisherSlider.MouseDown -= picSpecificFinisherSlider_MouseDown;
                picSpecificNormalSpinner.MouseDown -= picSpecificNormalSpinner_MouseDown;
            }
            userInputTempData.setObjectOption.isOnlyHitObjects = rdoOnlySpecificHitObject.Checked;
        }
        private void picSpecificNormalDong_MouseDown(object? sender, MouseEventArgs e)
        {
            userInputTempData.setObjectOption.isOnlyNormalDong = !userInputTempData.setObjectOption.isOnlyNormalDong;
            picSpecificNormalDong.Image = userInputTempData.setObjectOption.isOnlyNormalDong ?
                          Properties.Resources.d_selected : Properties.Resources.d;
        }
        private void picSpecificFinisherDong_MouseDown(object? sender, MouseEventArgs e)
        {
            userInputTempData.setObjectOption.isOnlyFinisherDong = !userInputTempData.setObjectOption.isOnlyFinisherDong;
            picSpecificFinisherDong.Image = userInputTempData.setObjectOption.isOnlyFinisherDong ?
                        Properties.Resources.d_selected : Properties.Resources.d;

        }
        private void picSpecificNormalKa_MouseDown(object? sender, MouseEventArgs e)
        {
            userInputTempData.setObjectOption.isOnlyNormalKa = !userInputTempData.setObjectOption.isOnlyNormalKa;
            picSpecificNormalKa.Image = userInputTempData.setObjectOption.isOnlyNormalKa ?
                        Properties.Resources.k_selected : Properties.Resources.k;
        }
        private void picSpecificFinisherKa_MouseDown(object? sender, MouseEventArgs e)
        {
            userInputTempData.setObjectOption.isOnlyFinisherKa = !userInputTempData.setObjectOption.isOnlyFinisherKa;
            picSpecificFinisherKa.Image = userInputTempData.setObjectOption.isOnlyFinisherKa ?
                        Properties.Resources.k_selected : Properties.Resources.k;
        }
        private void picSpecificNormalSlider_MouseDown(object? sender, MouseEventArgs e)
        {
            userInputTempData.setObjectOption.isOnlyNormalSlider = !userInputTempData.setObjectOption.isOnlyNormalSlider;
            picSpecificNormalSlider.Image = userInputTempData.setObjectOption.isOnlyNormalSlider ?
                        Properties.Resources.slider_selected : Properties.Resources.slider;
        }
        private void picSpecificFinisherSlider_MouseDown(object? sender, MouseEventArgs e)
        {
            userInputTempData.setObjectOption.isOnlyFinisherSlider = !userInputTempData.setObjectOption.isOnlyFinisherSlider;
            picSpecificFinisherSlider.Image = userInputTempData.setObjectOption.isOnlyFinisherSlider ?
                        Properties.Resources.slider_selected : Properties.Resources.slider;
        }
        private void picSpecificNormalSpinner_MouseDown(object? sender, MouseEventArgs e)
        {
            userInputTempData.setObjectOption.isOnlySpinner = !userInputTempData.setObjectOption.isOnlySpinner;
            picSpecificNormalSpinner.Image = userInputTempData.setObjectOption.isOnlySpinner ?
                        Properties.Resources.spinner_selected : Properties.Resources.spinner;
        }
        private void txtBeatSnap_TextChanged(object? sender, EventArgs e)
        {
            userInputTempData.setBeatSnapOption.beatSnap = txtBeatSnap.Text;
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {
            try
            {
                // 入力値をUserInputDataクラスに格納
                if (!UserInputDataHelper.SetUserInputData(userInputTempData,
                                                          Constants.EXECUTE_REMOVE,
                                                          ref userInputData))
                {
                    return;
                }
                // 譜面情報の取得
                if (!GetBeatmap())
                {
                    Common.ShowMessageDialog("E_A-D-1");
                    return;
                }
                // 削除処理の実行
                ExecuteSvEditProcess(Constants.EXECUTE_REMOVE);
            }
            catch (Exception ex)
            {
                Common.WriteErrorMessage("LOG_E-EXCEPTION");
                Common.WriteExceptionMessage(ex);
                return;
            }
            finally
            {
                beatmapData = null;
                userInputData = new();
            }
        }
        private void txtStartOffset_TextChanged(object sender, EventArgs e)
        {
            userInputTempData.offset = txtStartOffset.Text;
        }
        private void chkEnableStartOffset_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isOffset = chkEnableStartOffset.Checked;
            SetChkEnableStartOffset(userInputTempData.isOffset);
        }
        #endregion
        #region utilityタブのイベントハンドラ
        private void cmbHitsoundTool_SelectedIndexChanged(object sender, EventArgs e)
        {
            userInputUtilityData.hitSoundCode = cmbHitsoundTool.SelectedIndex;
        }
        private void btnApplyOffset_Click(object sender, EventArgs e)
        {
            // 譜面情報の取得
            if (!GetBeatmap())
            {
                Common.ShowMessageDialog("E_A-D-1");
                return;
            }
            userInputUtilityData.utilityCode = Constants.UTILITY_OFFSET;
            ExecuteUtilityProcess();
        }
        private void btnApplyHitsound_Click(object sender, EventArgs e)
        {
            // 譜面情報の取得
            if (!GetBeatmap())
            {
                Common.ShowMessageDialog("E_A-D-1");
                return;
            }
            userInputUtilityData.utilityCode = Constants.UTILITY_HITSOUND;
            ExecuteUtilityProcess();
        }
        private void cmbNotesPosition_SelectedIndexChanged(object sender, EventArgs e)
        {
            userInputUtilityData.notesPositionCode = cmbNotesPosition.SelectedIndex;
        }
        private void btnApplyNotesPosition_Click(object sender, EventArgs e)
        {
            // 譜面情報の取得
            if (!GetBeatmap())
            {
                Common.ShowMessageDialog("E_A-D-1");
                return;
            }
            userInputUtilityData.utilityCode = Constants.UTILITY_NOTES_POSITION;
            ExecuteUtilityProcess();

        }
        private void txtTags_TextChanged(object sender, EventArgs e)
        {
            userInputUtilityData.tags = txtTags.Text;
        }
        private void txtTags_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Enterキーが押された場合
            if (e.KeyChar == (char)Keys.Enter)
            {
                // 入力を無効化する
                e.Handled = true;
            }
        }
        private void btnApplyTags_Click(object sender, EventArgs e)
        {
            userInputUtilityData.utilityCode = Constants.UTILITY_TAG_EDIT;
            ExecuteUtilityProcess();
        }
        private void cmbMetadataSetting_SelectedIndexChanged(object sender, EventArgs e)
        {
            userInputUtilityData.settingCopierCode = cmbSettingCopier.SelectedIndex;
        }
        private void btnApplySettingCopier_Click(object sender, EventArgs e)
        {
            // 譜面情報の取得
            if (!GetBeatmap())
            {
                Common.ShowMessageDialog("E_A-D-1");
                return;
            }
            userInputUtilityData.utilityCode = Constants.UTILITY_SETTING_COPIER;
            ExecuteUtilityProcess();
        }
        private void txtResnapTimingFrom_TextChanged(object sender, EventArgs e)
        {
            int tempTiming = 0;
            if (!Common.ConvertMsTiming(txtResnapTimingFrom.Text, ref tempTiming))
            {
                tempTiming = int.MinValue;
            }
            userInputUtilityData.resnapTimingFrom = tempTiming;
        }
        private void txtResnapTimingTo_TextChanged(object sender, EventArgs e)
        {
            int tempTiming = 0;
            if (!Common.ConvertMsTiming(txtResnapTimingTo.Text, ref tempTiming))
            {
                tempTiming = int.MinValue;
            }
            userInputUtilityData.resnapTimingTo = tempTiming;
        }
        private void btnSetResnapTimingFrom_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtResnapTimingFrom, Constants.SET_TIMING);
        }
        private void btnSetResnapTimingTo_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtResnapTimingTo, Constants.SET_TIMING);
        }
        private void btnSwapResnapTiming_Click(object sender, EventArgs e)
        {
            // Timingの始点と終点を入れ替える
            string timingBuff = "";
            timingBuff = txtResnapTimingFrom.Text;
            txtResnapTimingFrom.Text = txtResnapTimingTo.Text;
            txtResnapTimingTo.Text = timingBuff;

        }
        private void cmbResnapBeatSnap_SelectedIndexChanged(object sender, EventArgs e)
        {
            userInputUtilityData.beatSnap = Constants.BEAT_SNAP_INTERVALS[cmbResnapBeatSnap.SelectedIndex];
        }
        private void btnApplyResnap_Click(object sender, EventArgs e)
        {
            // 譜面情報の取得
            if (!GetBeatmap())
            {
                Common.ShowMessageDialog("E_A-D-1");
                return;
            }
            userInputUtilityData.utilityCode = Constants.UTILITY_RESNAP;
            ExecuteUtilityProcess();
        }
        #endregion
        #endregion
    }
}