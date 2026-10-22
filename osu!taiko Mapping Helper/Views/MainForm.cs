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
using System.ComponentModel;

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
        private static Config config = new();
        private int preUnicodeSupport;
        private List<TimingPoint> timingPoints = [];
        private string osuDirectory = string.Empty;
        private string songsPath = string.Empty;
        private int currentTime;
        private bool isDirectoryLoaded = false;
        // 見つけたosu!のプロセス (起動中かどうかの確認に使う)
        private Process? osuProcess = null;
        // 次にosu!のプロセスを探す時刻
        private DateTime nextOsuProcessSearchTime = DateTime.MinValue;
        // osu!のプロセスを探す間隔(ミリ秒)
        private const int OSU_PROCESS_SEARCH_INTERVAL = 1000;
        private bool isUpdate = true;
        // バックアップフォルダ名 (機能によらず、.osuのファイル名で統一する)
        private string backupDirectoryName => Path.GetFileNameWithoutExtension(beatmapInfo.beatmapPath ?? string.Empty);
        // 処理対象の譜面情報とバックアップフォルダ名 (処理の開始時にSetTargetBeatmapで設定する)
        private BeatmapMetadata targetBeatmapInfo = new();
        private string targetBackupDirectoryName = string.Empty;
        private TimingPropertyForm? TimingPropertyForm = null;
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int updateInterval { get; set; } = 15;
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
            Shown += async (_, _) => await CheckForUpdatesAsync();

        }
        private async Task CheckForUpdatesAsync()
        {
            if (ShowPendingUpdateError())
            {
                return;
            }

            if (await UpdaterUtils.CheckUpdateAsync("v" + Constants.APP_VERSION))
            {
                Close();
            }
        }
        private static bool ShowPendingUpdateError()
        {
            const string updateErrorVariable = "OSU_TAIKO_MAPPING_HELPER_UPDATE_ERROR";
            var updateErrorLogPath = Environment.GetEnvironmentVariable(updateErrorVariable);
            Environment.SetEnvironmentVariable(updateErrorVariable, null);

            if (string.IsNullOrWhiteSpace(updateErrorLogPath))
            {
                return false;
            }

            Common.ShowMessageDialog("W_U-A-2", 0, updateErrorLogPath);
            return true;
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
                    Process? process = GetOsuProcess();
                    if (process == null)
                    {
                        if (isDirectoryLoaded)
                        {
                            beatmapInfo = new();
                        }
                        isDirectoryLoaded = false;
                        // 前の譜面の情報を表示し続けないよう、TimingPropertyの表示を消す
                        TimingPropertyForm?.SetOsuData(string.Empty, currentTime);
                        throw new Exception("osu!が起動されていません。");
                    }
                    // osuフォルダ取得処理
                    if (!isDirectoryLoaded)
                    {
                        string? fileName = process.MainModule?.FileName;
                        osuDirectory = Path.GetDirectoryName(fileName) ?? string.Empty;
                        if (osuDirectory != string.Empty)
                        {
                            if (!Directory.Exists(osuDirectory))
                            {
                                continue;
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
                    // 外部ツールが .osu を更新していないか確認するため、最終更新日時を取得する
                    DateTime lastWriteTime = File.GetLastWriteTime(osuBeatmapPath);

                    currentTime = baseAddresses.GeneralData.AudioTime;
                    // TimingPropertyには選択している譜面のパスを渡す (譜面の読み込みはTimingProperty側で行う)
                    TimingPropertyForm?.SetOsuData(osuBeatmapPath, currentTime);

                    // 譜面パスと更新日時が変わっていない場合、.osuのDecodeを行わない
                    // これによりUpdateMemoryDataが短周期で.osuを読み続けることを防ぐ
                    if (beatmapInfo.beatmapPath == osuBeatmapPath &&
                        beatmapInfo.lastUpdate == lastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff"))
                    {
                        continue;
                    }
                    // 譜面のデータを取得
                    var beatmapData = Common.TryDecodeBeatmap(osuBeatmapPath);
                    // 外部ツールが書き込み中の可能性があるため、今回の更新は見送る
                    if (beatmapData == null) continue;
                    //OsuParsers.Beatmaps.Beatmap beatmapData = BeatmapDecoder.Decode(osuBeatmapPath);

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
                    string backgroundPath = beatmapData.EventsSection.BackgroundImage != null ?
                                                         Path.Combine(songsPath ?? "",
                                                         baseAddresses.Beatmap.FolderName ?? "",
                                                         beatmapData.EventsSection.BackgroundImage) : "";
                    beatmapInfo.backgroundPath = backgroundPath ?? "";
                    // 読み込み中に更新された場合に次の周回で読み込み直せるよう、読み込む前に取得した更新日時を使う
                    beatmapInfo.lastUpdate = lastWriteTime.ToString("yyyy-MM-dd HH:mm:ss.fff");
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
        /// 起動中のosu!のプロセスを取得する
        /// </summary>
        /// <returns>osu!のプロセス (起動していない場合はnull)</returns>
        private Process? GetOsuProcess()
        {
            if (osuProcess != null)
            {
                try
                {
                    if (!osuProcess.HasExited)
                    {
                        return osuProcess;
                    }
                }
                catch (Exception ex)
                {
                    // 終了したか確認できない場合は、探し直す
                    Console.WriteLine(ex);
                }
                osuProcess.Dispose();
                osuProcess = null;
                // 終了した直後は、すぐに探し直す
                nextOsuProcessSearchTime = DateTime.MinValue;
            }
            if (DateTime.Now < nextOsuProcessSearchTime)
            {
                return null;
            }
            nextOsuProcessSearchTime = DateTime.Now.AddMilliseconds(OSU_PROCESS_SEARCH_INTERVAL);
            var processes = Process.GetProcessesByName("osu!");
            if (processes.Length == 0)
            {
                return null;
            }
            osuProcess = processes[0];
            // 使わないプロセスは破棄する
            foreach (var other in processes.Skip(1))
            {
                other.Dispose();
            }
            return osuProcess;
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
                    preBeatmapInfo.version = beatmapInfo.version;
                    preBeatmapInfo.previewTime = beatmapInfo.previewTime;
                    preBeatmapInfo.beatmapPath = beatmapInfo.beatmapPath;
                    preBeatmapInfo.lastUpdate = beatmapInfo.lastUpdate;
                    picDisplayBg.Image = BGHelper.SetBgOnForm(beatmapInfo.backgroundPath ?? "", new Point(384, 216));
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
            chkApplyStartObject.Checked = true;
            chkApplyEndObject.Checked = true;
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
            userInputTempData.offsetMode = config.offsetMode;
            tabSetType.Controls.Clear();
            switch (config.offsetMode)
            {
                case 0:
                    pnlMiliSecondOffset.Visible = false;
                    pnlHexaAndDecaOffset.Visible = true;
                    userInputTempData.isOffset = chkEnableHexaOffset.Checked;
                    tabSetType.Controls.Add(tabHitObjectsPage);
                    tabSetType.Controls.Add(tabBeatSnap);
                    break;
                case 1:
                    pnlMiliSecondOffset.Visible = true;
                    pnlHexaAndDecaOffset.Visible = false;
                    userInputTempData.isOffset = chkEnableOffset.Checked;
                    tabSetType.Controls.Add(tabHitObjectsPage);
                    tabSetType.Controls.Add(tabBeatSnap);
                    tabSetType.Controls.Add(tabGreenLine);
                    tabSetType.Controls.Add(tabRedLine);
                    break;
            }
            menuStrip1.Items.Clear();
            tabSetType.Controls.Clear();
            if (config.advanceMode == 1)
            {
                chkRelative.Visible = true;
                menuStrip1.Items.AddRange([sVEditorToolStripMenuItem, utilityToolStripMenuItem, timingPropertyToolStripMenuItem, bGSetterToolStripMenuItem]);
                tabSetType.Controls.Add(tabHitObjectsPage);
                tabSetType.Controls.Add(tabBeatSnap);
                tabSetType.Controls.Add(tabGreenLine);
                tabSetType.Controls.Add(tabRedLine);
            }
            else
            {
                chkRelative.Visible = false;
                menuStrip1.Items.AddRange([sVEditorToolStripMenuItem, utilityToolStripMenuItem, bGSetterToolStripMenuItem]);
                tabSetType.Controls.Add(tabHitObjectsPage);
                tabSetType.Controls.Add(tabBeatSnap);
            }
            InitializeOffsetControls();
            InitializeCalculationControls();
            UpdateSvBpmControls();
            tabSetType.ItemSize = new Size(Constants.TAB_MAX_ITEM_SIZE / tabSetType.Controls.Count, tabSetType.ItemSize.Height);
        }
        /// <summary>
        /// 計算方法(等差・等比)のコントロールの初期化処理
        /// </summary>
        /// <remarks>
        /// 設定ファイルに保存されている前回の状態に戻す
        /// </remarks>
        private void InitializeCalculationControls()
        {
            // 初期化中に設定ファイルへ上書き保存されないよう、イベントを一時的に外す
            rdoArithmetic.CheckedChanged -= rdoArithmetic_CheckedChanged;
            rdoGeometric.CheckedChanged -= rdoGeometric_CheckedChanged;
            // 等比以外の値の場合は等差にする
            bool isGeometric = config.calculationCode == Constants.CALCULATION_GEOMETRIC;
            rdoGeometric.Checked = isGeometric;
            rdoArithmetic.Checked = !isGeometric;
            userInputTempData.isArithmetic = rdoArithmetic.Checked;
            userInputTempData.isGeometric = rdoGeometric.Checked;
            rdoArithmetic.CheckedChanged += rdoArithmetic_CheckedChanged;
            rdoGeometric.CheckedChanged += rdoGeometric_CheckedChanged;
        }
        /// <summary>
        /// オフセット関連のコントロールの初期化処理
        /// </summary>
        /// <remarks>
        /// 一旦すべてのオフセットのチェックを外し、設定画面で選択しているオフセットモードのチェックのみ
        /// 設定ファイルに保存されている前回の状態に戻す
        /// </remarks>
        private void InitializeOffsetControls()
        {
            // 初期化中にCheckedChanged/TextChangedが呼ばれると、選択していないモードの状態まで
            // 設定ファイルに上書き保存されてしまうため、イベントを一時的に外す
            chkEnableHexaOffset.CheckedChanged -= chkEnableHexaOffset_CheckedChanged;
            chkEnableDuoOffset.CheckedChanged -= chkEnableDuoOffset_CheckedChanged;
            chkEnableOffset.CheckedChanged -= chkEnableOffset_CheckedChanged;
            txtOffset.TextChanged -= txtOffset_TextChanged;
            bool isHexaMode = config.offsetMode == 0;
            // 1/16(1/12) offset
            chkEnableHexaOffset.Checked = isHexaMode && config.offsetHexa == 1;
            // 1/12 offsetは1/16 offsetが有効な場合のみ有効にする
            chkEnableDuoOffset.Checked = chkEnableHexaOffset.Checked && config.offsetDuo == 1;
            // ms指定のoffset
            chkEnableOffset.Checked = !isHexaMode && config.offsetMs == 1;
            txtOffset.Text = config.offsetMsValue ?? string.Empty;
            FormUtils.SetChkEnableOffset(chkEnableOffset.Checked, txtOffset);
            // 入力値に反映する (オフセットの有効化設定は選択しているモードのチェックで決める)
            userInputTempData.isOffset = isHexaMode ? chkEnableHexaOffset.Checked : chkEnableOffset.Checked;
            userInputTempData.isDuoOffset = chkEnableDuoOffset.Checked;
            userInputTempData.offset = txtOffset.Text;
            chkEnableHexaOffset.CheckedChanged += chkEnableHexaOffset_CheckedChanged;
            chkEnableDuoOffset.CheckedChanged += chkEnableDuoOffset_CheckedChanged;
            chkEnableOffset.CheckedChanged += chkEnableOffset_CheckedChanged;
            txtOffset.TextChanged += txtOffset_TextChanged;
        }
        /// <summary>
        /// 処理項目タブが"Objectsのみ"の時のコントロールの初期化処理
        /// </summary>
        private void InitializeHitObjectsControls()
        {
            switch (config.offsetMode)
            {
                case 0:
                    pnlHexaAndDecaOffset.Visible = true;
                    pnlMiliSecondOffset.Visible = false;
                    break;
                case 1:
                    pnlHexaAndDecaOffset.Visible = false;
                    pnlMiliSecondOffset.Visible = true;
                    break;
            }
            chkRelative.Visible = config.advanceMode == 1 && chkEnableSv.Checked;
            rdoAllHitObjects.Checked = userInputTempData.setObjectOption.isAllHitObjects;
            rdoOnlyBarline.Checked = userInputTempData.setObjectOption.isOnlyBarlines;
            rdoOnlyBookMark.Checked = userInputTempData.setObjectOption.isOnlyBookmarks;
            rdoOnlySpecificHitObject.Checked = userInputTempData.setObjectOption.isOnlyHitObjects;
            chkEnableOffset.Enabled = true;
            FormUtils.SetChkEnableOffset(chkEnableOffset.Checked, txtOffset);
            Common.SetLabelText(chkEnableOffset, "LBL_APPLY_OFFSET");
            Common.SetLabelText(chkEnableHexaOffset, "LBL_APPLY_HEXA_OFFSET");
            Common.SetLabelText(chkEnableDuoOffset, "LBL_APPLY_DUO_OFFSET");
            FormUtils.SetApplyContols(true,
                                      chkEnableSv,
                                      txtSvFrom,
                                      txtSvTo,
                                      btnSetSvFrom,
                                      btnSetSvTo,
                                      btnSwapSv,
                                      chkRelative,
                                      chkEnableSvTo);
        }
        /// <summary>
        /// 処理項目タブが"ビートスナップ間隔"の時のコントロールの初期化処理
        /// </summary>
        private void InitializeBeatSnapControls()
        {
            pnlHexaAndDecaOffset.Visible = false;
            pnlMiliSecondOffset.Visible = false;
            chkRelative.Visible = config.advanceMode == 1 && chkEnableSv.Checked;
            FormUtils.SetApplyContols(true,
                                      chkEnableSv,
                                      txtSvFrom,
                                      txtSvTo,
                                      btnSetSvFrom,
                                      btnSetSvTo,
                                      btnSwapSv,
                                      chkRelative,
                                      chkEnableSvTo);
        }
        /// <summary>
        /// 処理項目タブが"緑線"の時のコントロールの初期化処理
        /// </summary>
        private void InitializeGreenLineControls()
        {
            pnlHexaAndDecaOffset.Visible = false;
            pnlMiliSecondOffset.Visible = false;
            chkRelative.Visible = config.advanceMode == 1 && chkEnableSv.Checked;
            FormUtils.SetApplyContols(true,
                                      chkEnableSv,
                                      txtSvFrom,
                                      txtSvTo,
                                      btnSetSvFrom,
                                      btnSetSvTo,
                                      btnSwapSv,
                                      chkRelative,
                                      chkEnableSvTo);
        }
        /// <summary>
        /// 処理項目タブが"赤線"の時のコントロールの初期化処理
        /// </summary>
        private void InitializeRedLineControls()
        {
            pnlHexaAndDecaOffset.Visible = false;
            pnlMiliSecondOffset.Visible = false;
            chkRelative.Visible = false;
            chkRelative.Checked = false;
            FormUtils.SetApplyContols(false,
                                      chkEnableSv,
                                      txtSvFrom,
                                      txtSvTo,
                                      btnSetSvFrom,
                                      btnSetSvTo,
                                      btnSwapSv,
                                      chkRelative,
                                      chkEnableSvTo);

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
                    // 計算後のSVが0以下になる緑線(追加する緑線、または変更した範囲内の緑線)がある場合は、譜面を書き換えずに中止する
                    // (相対速度変化の加算などで0以下になると、緑線の値が-∞や正の値になり譜面が壊れるため)
                    if (isSvCalculation &&
                        (timingPoints.Any(tp => !(tp.sv > 0)) ||
                         beatmapData.timingPoints.Any(tp => !tp.isRedLine &&
                                                            tp.time >= userInputData.timingFrom &&
                                                            tp.time <= userInputData.timingTo &&
                                                            !(tp.sv > 0))))
                    {
                        timingPoints.Clear();
                        Common.ShowMessageDialog("E_V-C-7");
                        return;
                    }
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
            if (!BeatmapHelper.CreateBackup(targetBeatmapInfo.beatmapPath, targetBackupDirectoryName))
            {
                Common.ShowMessageDialog("E_A-P-1");
                return;
            }
            else
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
            if (!BeatmapHelper.ExportToOsuFile(beatmapData, targetBeatmapInfo.beatmapPath))
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
            if (targetBeatmapInfo.beatmapPath == null || targetBeatmapInfo.beatmapPath == string.Empty)
            {
                return;
            }
            string beatmapDirectory = Path.GetDirectoryName(targetBeatmapInfo.beatmapPath) ?? string.Empty;
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
                    switch (BeatmapHelper.ExportToAllOsuFiles(beatmapsPath, userInputUtilityData, targetBeatmapInfo))
                    {
                        case 0:
                            // 正常終了した場合、先の処理を続行
                            break;
                        case 1:
                            // 読み込みに失敗したdiffがあった場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-D-1");
                            return;
                        default:
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                    }
                    if (!SettingHelper.ResetBackupFile(config))
                    {
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    break;
                case Constants.UTILITY_TAG_EDIT:
                case Constants.UTILITY_SETTING_COPIER:
                    // タグ内の改行コードを取り除く
                    userInputUtilityData.tags = txtTags.Text.Replace("\r", string.Empty).Replace("\n", string.Empty);
                    if (string.IsNullOrEmpty(beatmapDirectory) ||
                        (userInputUtilityData.utilityCode == Constants.UTILITY_SETTING_COPIER && beatmapData == null))
                    {
                        return;
                    }
                    var timingPoints = beatmapData?.timingPoints.Where(tp => tp.isRedLine).ToList();
                    switch (BeatmapHelper.ExportToAllOsuFiles(beatmapsPath, userInputUtilityData, targetBeatmapInfo, timingPoints))
                    {
                        case 0:
                            // 正常終了した場合、先の処理を続行
                            break;
                        case 1:
                            // 読み込みに失敗したdiffがあった場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-D-1");
                            return;
                        default:
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                    }
                    if (!SettingHelper.ResetBackupFile(config))
                    {
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
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
                    if (!BeatmapHelper.CreateBackup(targetBeatmapInfo.beatmapPath, targetBackupDirectoryName))
                    {
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    else
                    {
                        if (!SettingHelper.ResetBackupFile(config))
                        {
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                        }
                    }
                    if (!BeatmapHelper.ExportToOsuFile(beatmapData, targetBeatmapInfo.beatmapPath, userInputUtilityData))
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
                    if (!BeatmapHelper.CreateBackup(targetBeatmapInfo.beatmapPath, targetBackupDirectoryName))
                    {
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    else
                    {
                        if (!SettingHelper.ResetBackupFile(config))
                        {
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                        }
                    }
                    if (!BeatmapHelper.ExportToOsuFile(beatmapData, targetBeatmapInfo.beatmapPath, userInputUtilityData))
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
                    if (!BeatmapHelper.CreateBackup(targetBeatmapInfo.beatmapPath, targetBackupDirectoryName))
                    {
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    else
                    {
                        if (!SettingHelper.ResetBackupFile(config))
                        {
                            // 失敗した場合はエラーダイアログを表示する
                            Common.ShowMessageDialog("E_A-P-1");
                            return;
                        }
                    }
                    if (!BeatmapHelper.ExportToOsuFile(beatmapData, targetBeatmapInfo.beatmapPath, userInputUtilityData))
                    {
                        // 失敗した場合はエラーダイアログを表示する
                        Common.ShowMessageDialog("E_A-P-1");
                        return;
                    }
                    break;
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
            // 処理対象の譜面を、処理を始めた時点の譜面に固定する
            SetTargetBeatmap();
            // 譜面情報がリアルタイムで取得できていない場合はエラーダイアログを表示する
            if (targetBeatmapInfo.beatmapPath == null || targetBeatmapInfo.beatmapPath == string.Empty)
            {
                return false;
            }
            // 譜面の内容を取得する
            beatmapData = BeatmapHelper.GetBeatmapData(targetBeatmapInfo.beatmapPath);
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
        /// 処理対象の譜面情報を設定する
        /// </summary>
        /// <remarks>
        /// beatmapInfoはosu!の監視スレッドが書き換えるため、処理中にosu!で譜面を切り替えても
        /// 処理を始めた時点の譜面に書き込むよう、処理の開始時にコピーを取る
        /// </remarks>
        private void SetTargetBeatmap()
        {
            var info = beatmapInfo;
            targetBeatmapInfo = new BeatmapMetadata
            {
                title = info.title,
                titleUnicode = info.titleUnicode,
                artist = info.artist,
                artistUnicode = info.artistUnicode,
                creator = info.creator,
                version = info.version,
                source = info.source,
                backgroundPath = info.backgroundPath,
                background = info.background,
                beatmapPath = info.beatmapPath,
                previewTime = info.previewTime,
                tags = info.tags,
                lastUpdate = info.lastUpdate
            };
            // バックアップフォルダ名は、コピーした譜面のパスから求める (パスとずれないようにするため)
            targetBackupDirectoryName = Path.GetFileNameWithoutExtension(info.beatmapPath ?? string.Empty);
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
                control.Text = setCode switch
                {
                    Constants.SET_TIMING => Common.ConvertFormatTiming(currentTime),
                    Constants.SET_SV => UserInputDataHelper.SetCurrentSv(beatmapData, currentTime),
                    Constants.SET_VOLUME => UserInputDataHelper.SetCurrentVolume(beatmapData, currentTime),
                    Constants.SET_BPM => UserInputDataHelper.SetCurrentBpm(beatmapData, currentTime),
                    Constants.SET_VISUAL_BPM => UserInputDataHelper.SetCurrentVisualBpm(beatmapData, currentTime),
                    _ => throw new Exception(),
                };
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
            Common.SetLabelText(chkApplyRedLines, "LBL_APPLY_REDLINES");
            Common.SetLabelText(chkSvBpm, "LBL_SV_BPM");
            Common.SetLabelText(btnSetSvFrom, userInputTempData.isSvBpm ? "LBL_SET_BPM_FROM" : "LBL_SET_SV_FROM");
            Common.SetLabelText(btnSetSvTo, userInputTempData.isSvBpm ? "LBL_SET_BPM_TO" : "LBL_SET_SV_TO");
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
            Common.SetLabelText(chkEnableHexaOffset, "LBL_APPLY_HEXA_OFFSET");
            Common.SetLabelText(chkEnableDuoOffset, "LBL_APPLY_DUO_OFFSET");
            Common.SetLabelText(btnApply, "LBL_EXECUTE");
            Common.SetLabelText(tabHitObjectsPage, "LBL_APPLY_TAB_OBJECTS");
            Common.SetLabelText(tabBeatSnap, "LBL_APPLY_TAB_BEATSNAPS");
            Common.SetLabelText(lblBeatSnapsGuide, "LBL_APPLY_TAB_BEATSNAPS_GUIDE");
            Common.SetLabelText(tabGreenLine, "LBL_APPLY_TAB_GREENLINES");
            Common.SetLabelText(lblGreenLineGuide, "LBL_APPLY_TAB_GREENLINES_GUIDE");
            Common.SetLabelText(tabRedLine, "LBL_APPLY_TAB_REDLINES");
            Common.SetLabelText(lblRedLineGuide, "LBL_APPLY_TAB_REDLINES_GUIDE");
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
            Common.SetLabelText(lblScrollSpeed, "LBL_REDLINES_SCROLL_SPEED");
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
        /// "BPMで指定"のチェックと、相対速度変化オプションのチェックの有効/無効を設定する
        /// </summary>
        private void UpdateSvBpmControls()
        {
            chkSvBpm.Visible = tabExecuteType.SelectedIndex == 0 && tabSetType.SelectedIndex != 3 && chkEnableSv.Checked;
            chkRelative.Enabled = !chkSvBpm.Checked;
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
            string backupPath = BeatmapHelper.GetBackupDirectoryPath(this.backupDirectoryName);
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
                    // SV Editorを表示している場合のみ実行する
                    if (!pnlGroupSvEditor.Visible)
                    {
                        break;
                    }
                    // テキストボックスの入力の取り消し(Ctrl+Z)が一緒に行われないよう、キーを子のコントロールに渡さない
                    e.SuppressKeyPress = true;
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
            // 選択している譜面のバックアップフォルダがある場合はそのフォルダを、
            // 無い場合(譜面を選択していない、まだバックアップを作っていない)はバックアップフォルダ全体をエクスプローラで開く
            string backupPath = Directory.GetCurrentDirectory() + Constants.BACKUP_DIRECTORY;
            if (!string.IsNullOrEmpty(beatmapInfo.beatmapPath))
            {
                string beatmapBackupPath = BeatmapHelper.GetBackupDirectoryPath(backupDirectoryName);
                if (Directory.Exists(beatmapBackupPath))
                {
                    backupPath = beatmapBackupPath;
                }
            }
            System.Diagnostics.Process.Start("EXPLORER.EXE", "\"" + backupPath + "\"");
        }
        private void btnViewSetting_Click(object sender, EventArgs e)
        {
            // 設定画面を表示する
            Form settingForm = new SettingForm(config);
            settingForm.ShowDialog();
            InitializeLabelText();
            try
            {
                TimingPropertyForm?.InitializeLabelText();
            }
            catch
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
            userInputTempData.offsetMode = config.offsetMode;
            // 設定画面でオフセットモードが変更されている場合があるため、オフセットのチェックを初期化する
            // (削除タブを開いている場合はtabExecuteType_SelectedIndexChangedでオフセットが無効になるため、先に呼ぶ)
            InitializeOffsetControls();
            tabSetType_SelectedIndexChanged(sender, e);
            // 処理項目タブを作り直すと先頭のタブに戻るため、作り直す前に選択していたタブを覚えておく
            int selectedSetTypeIndex = tabSetType.SelectedIndex;
            menuStrip1.Items.Clear();
            tabSetType.Controls.Clear();
            if (config.advanceMode == 1)
            {
                chkRelative.Visible = true;
                menuStrip1.Items.AddRange([sVEditorToolStripMenuItem, utilityToolStripMenuItem, timingPropertyToolStripMenuItem, bGSetterToolStripMenuItem]);
                tabSetType.Controls.Add(tabHitObjectsPage);
                tabSetType.Controls.Add(tabBeatSnap);
                tabSetType.Controls.Add(tabGreenLine);
                tabSetType.Controls.Add(tabRedLine);
            }
            else
            {
                chkRelative.Checked = false;
                chkRelative.Visible = false;
                menuStrip1.Items.AddRange([sVEditorToolStripMenuItem, utilityToolStripMenuItem, bGSetterToolStripMenuItem]);
                tabSetType.Controls.Add(tabHitObjectsPage);
                tabSetType.Controls.Add(tabBeatSnap);
            }
            tabSetType.ItemSize = new Size(Constants.TAB_MAX_ITEM_SIZE / tabSetType.Controls.Count, tabSetType.ItemSize.Height);
            // 作り直す前に選択していた処理項目タブに戻す (詳細モードの変更でタブが無くなった場合は先頭のタブにする)
            tabSetType.SelectedIndex = (selectedSetTypeIndex >= 0 && selectedSetTypeIndex < tabSetType.TabCount) ? selectedSetTypeIndex : 0;
            tabSetType_SelectedIndexChanged(sender, e);
            // 処理項目タブの初期化でSVのコントロールが有効になるため、最後に適用/削除タブに合わせて有効/無効を設定し直す
            tabExecuteType_SelectedIndexChanged(sender, e);
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
            TimingPropertyForm?.Close();
            TimingPropertyForm?.Dispose();
            TimingPropertyForm = new TimingPropertyForm()
            {
                parentForm = this
            };
            TimingPropertyForm.SetOsuData(beatmapInfo.beatmapPath, currentTime);
            TimingPropertyForm.Show();
            TimingPropertyForm.Text = "Timing Property";
            if (this.beatmapInfo.beatmapPath == null || this.beatmapInfo.beatmapPath == string.Empty)
            {
                return;
            }
            if (!TimingPropertyForm.GetBeatmap())
            {
                Common.ShowMessageDialog("E_A-D-1");
            }
        }
        private void bGSetterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // BGの画像と、BGの行(座標)の両方が取得できている場合のみBG設定画面を表示する
            if (!string.IsNullOrEmpty(beatmapInfo.backgroundPath) && !string.IsNullOrEmpty(beatmapInfo.background))
            {
                BGSetterForm bgSetterForm = new(config,
                                                beatmapInfo.beatmapPath,
                                                beatmapInfo.backgroundPath,
                                                beatmapInfo.background,
                                                backupDirectoryName);
                bgSetterForm.ShowDialog();
            }
            else
            {
                Common.ShowMessageDialog("E_A-D-2");
            }

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
            if (userInputTempData.isSvBpm)
            {
                // BPMで指定する場合は、SVとは別に覚える
                userInputTempData.svBpmFrom = txtSvFrom.Text;
            }
            else if (userInputTempData.isRelative)
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
            if (userInputTempData.isSvBpm)
            {
                // BPMで指定する場合は、SVとは別に覚える
                userInputTempData.svBpmTo = txtSvTo.Text;
            }
            else if (chkRelative.Checked)
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
            // BPMで指定する場合は、現地点の見た目BPMを入れる
            SetCurrentTimeInfo(txtSvFrom, userInputTempData.isSvBpm ? Constants.SET_VISUAL_BPM : Constants.SET_SV);
        }
        private void btnSetSvTo_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtSvTo, userInputTempData.isSvBpm ? Constants.SET_VISUAL_BPM : Constants.SET_SV);
        }
        private void chkEnableSv_CheckedChanged(object? sender, EventArgs e)
        {
            userInputTempData.isSv = chkEnableSv.Checked;
            FormUtils.SetTxtSvFromEnabledState(userInputTempData.isSv, userInputTempData, txtSvFrom);
            FormUtils.SetTxtSvToEnabledState(userInputTempData.isSv, txtSvTo);
            FormUtils.SetBtnSwapSvEnabledState(userInputTempData.isSv, btnSwapSv);
            FormUtils.SetBtnSetSvFromEnabledState(userInputTempData.isSv, btnSetSvFrom);
            FormUtils.SetBtnSetSvToEnabledState(userInputTempData.isSv, btnSetSvTo);
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
            UpdateSvBpmControls();
        }
        private void chkSvBpm_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isSvBpm = chkSvBpm.Checked;
            if (chkSvBpm.Checked)
            {
                // BPMで指定する場合は相対速度変化オプションを使わないため、チェックを外す
                chkRelative.Checked = false;
            }
            // SVの入力欄を、選んだ指定方法で前回入力した値に戻す
            txtSvFrom.Text = chkSvBpm.Checked ? userInputTempData.svBpmFrom : userInputTempData.svFrom;
            txtSvTo.Text = chkSvBpm.Checked ? userInputTempData.svBpmTo : userInputTempData.svTo;
            Common.SetLabelText(btnSetSvFrom, userInputTempData.isSvBpm ? "LBL_SET_BPM_FROM" : "LBL_SET_SV_FROM");
            Common.SetLabelText(btnSetSvTo, userInputTempData.isSvBpm ? "LBL_SET_BPM_TO" : "LBL_SET_SV_TO");
            UpdateSvBpmControls();
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
            chkApplyRedLines.Visible = userInputTempData.isVolume && (tabExecuteType.SelectedIndex == 0);
            FormUtils.SetTxtVolumeFromEnabledState(userInputTempData.isVolume, txtVolumeFrom);
            FormUtils.SetTxtVolumeToEnabledState(userInputTempData.isVolume, txtVolumeTo);
            FormUtils.SetBtnSwapVolumeEnabledState(userInputTempData.isVolume, btnSwapVolume);
            FormUtils.SetBtnSetVolumeFromEnabledState(userInputTempData.isVolume, btnSetVolumeFrom);
            FormUtils.SetBtnSetVolumeToEnabledState(userInputTempData.isVolume, btnSetVolumeTo);
        }
        private void chkApplyStartObject_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isEnableFrom = chkApplyStartObject.Checked;
        }
        private void chkApplyEndObject_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isEnableTo = chkApplyEndObject.Checked;
        }
        private void chkApplyRedLines_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isEnableRedLines = chkApplyRedLines.Checked;
        }
        private void rdoArithmetic_CheckedChanged(object? sender, EventArgs? e)
        {
            userInputTempData.isArithmetic = rdoArithmetic.Checked;
            // 選択された計算方法を設定ファイルに保存する (選択が外れた方では保存しない)
            if (rdoArithmetic.Checked)
            {
                config.calculationCode = Constants.CALCULATION_ARITHMETIC;
                config.SaveCalculationCode();
            }
        }
        private void rdoGeometric_CheckedChanged(object? sender, EventArgs? e)
        {
            userInputTempData.isGeometric = rdoGeometric.Checked;
            // 選択された計算方法を設定ファイルに保存する (選択が外れた方では保存しない)
            if (rdoGeometric.Checked)
            {
                config.calculationCode = Constants.CALCULATION_GEOMETRIC;
                config.SaveCalculationCode();
            }
        }
        private void chkRelative_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isRelative = chkRelative.Checked;
            bool isEnable = userInputTempData.isRelative ?
                userInputTempData.isEnableRelativeEnd : userInputTempData.isSv;
            FormUtils.SetRelativeEnabledState(userInputTempData.isRelative, userInputTempData, pnlRelativeSvGroup, chkEnableSvTo, chkEnableSv, rdoArithmetic, rdoGeometric, txtSvFrom, txtSvTo);
            FormUtils.SetTxtSvToEnabledState(isEnable, txtSvTo);
            FormUtils.SetBtnSwapSvEnabledState(isEnable, btnSwapSv);
            FormUtils.SetBtnSetSvToEnabledState(isEnable, btnSetSvTo);
        }
        private void rdoRelativeMultiply_CheckedChanged(object sender, EventArgs e)
        {
            userInputTempData.isRelativeMultiply = rdoRelativeMultiply.Checked;
            FormUtils.SetRelativeMultiplyEnableState(userInputTempData.isRelativeMultiply, userInputTempData, txtRelativeBaseSv, txtSvFrom, txtSvTo);
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
            FormUtils.SetTxtSvToEnabledState(userInputTempData.isEnableRelativeEnd, txtSvTo);
            FormUtils.SetBtnSwapSvEnabledState(userInputTempData.isEnableRelativeEnd, btnSwapSv);
            FormUtils.SetBtnSetSvToEnabledState(userInputTempData.isEnableRelativeEnd, btnSetSvTo);
        }
        private void chkEnableOffset_CheckedChanged(object? sender, EventArgs? e)
        {
            userInputTempData.isOffset = chkEnableOffset.Checked;
            config.offsetMs = chkEnableOffset.Checked ? 1 : 0;
            FormUtils.SetChkEnableOffset(userInputTempData.isOffset, txtOffset);
            config.SaveOffsetMs();
        }
        private void txtOffset_TextChanged(object? sender, EventArgs? e)
        {
            userInputTempData.offset = txtOffset.Text;
            config.offsetMsValue = txtOffset.Text;
            config.SaveOffsetMsValue();
        }
        private void chkEnableHexaOffset_CheckedChanged(object? sender, EventArgs? e)
        {
            userInputTempData.isOffset = chkEnableHexaOffset.Checked;
            config.offsetHexa = chkEnableHexaOffset.Checked ? 1 : 0;
            if (!userInputTempData.isOffset)
            {
                chkEnableDuoOffset.Checked = false;
                config.offsetDuo = 0;
            }
            config.SaveOffsetHexa();
        }
        private void chkEnableDuoOffset_CheckedChanged(object? sender, EventArgs? e)
        {
            userInputTempData.isDuoOffset = chkEnableDuoOffset.Checked;
            config.offsetDuo = chkEnableDuoOffset.Checked ? 1 : 0;
            if (!userInputTempData.isOffset && userInputTempData.isDuoOffset)
            {
                chkEnableHexaOffset.Checked = true;
                config.offsetHexa = 1;
            }
            config.SaveOffsetDuo();
        }
        private void tabExecuteType_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tabExecuteType.SelectedIndex)
            {
                case 0:
                    // Apply
                    FormUtils.SetApplyControls(true,
                                               userInputTempData,
                                               config,
                                               tabSetType.SelectedIndex,
                                               chkEnableSv,
                                               chkEnableVolume,
                                               chkApplyStartObject,
                                               chkApplyEndObject,
                                               chkApplyRedLines,
                                               chkRelative,
                                               pnlRelativeSvGroup,
                                               chkEnableSvTo,
                                               txtSvFrom,
                                               txtSvTo,
                                               btnSwapSv,
                                               btnSetSvFrom,
                                               btnSetSvTo,
                                               txtVolumeFrom,
                                               txtVolumeTo,
                                               btnSwapVolume,
                                               btnSetVolumeFrom,
                                               btnSetVolumeTo);
                    userInputTempData.isOffset = config.offsetMode == 0 ? chkEnableHexaOffset.Checked : chkEnableOffset.Checked;
                    break;
                case 1:
                    // Remove
                    FormUtils.SetApplyControls(false,
                                               userInputTempData,
                                               config,
                                               tabSetType.SelectedIndex,
                                               chkEnableSv,
                                               chkEnableVolume,
                                               chkApplyStartObject,
                                               chkApplyEndObject,
                                               chkApplyRedLines,
                                               chkRelative,
                                               pnlRelativeSvGroup,
                                               chkEnableSvTo,
                                               txtSvFrom,
                                               txtSvTo,
                                               btnSwapSv,
                                               btnSetSvFrom,
                                               btnSetSvTo,
                                               txtVolumeFrom,
                                               txtVolumeTo,
                                               btnSwapVolume,
                                               btnSetVolumeFrom,
                                               btnSetVolumeTo);
                    userInputTempData.isOffset = false;
                    break;
                default:
                    break;
            }
            UpdateSvBpmControls();
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
                    userInputTempData.setOption.isSetRedLine = false;
                    InitializeHitObjectsControls();
                    break;
                case 1:
                    // BeatSnap
                    userInputTempData.setOption.isSetObjects = false;
                    userInputTempData.setOption.isSetBeatSnap = true;
                    userInputTempData.setOption.isSetGreenLine = false;
                    userInputTempData.setOption.isSetRedLine = false;
                    InitializeBeatSnapControls();
                    break;
                case 2:
                    // GreenLine
                    userInputTempData.setOption.isSetObjects = false;
                    userInputTempData.setOption.isSetBeatSnap = false;
                    userInputTempData.setOption.isSetGreenLine = true;
                    userInputTempData.setOption.isSetRedLine = false;
                    InitializeGreenLineControls();
                    break;
                case 3:
                    userInputTempData.setOption.isSetObjects = false;
                    userInputTempData.setOption.isSetBeatSnap = false;
                    userInputTempData.setOption.isSetGreenLine = false;
                    userInputTempData.setOption.isSetRedLine = true;
                    InitializeRedLineControls();
                    break;
                default:
                    break;
            }
            UpdateSvBpmControls();
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
                // ブックマークのみの場合はオフセットのコントロールを非表示にする
                chkEnableOffset.Visible = false;
                txtOffset.Visible = false;
                lblMiliSecond.Visible = false;
                chkEnableHexaOffset.Visible = false;
                label1.Visible = false;
                chkEnableDuoOffset.Visible = false;
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
                // オフセットのコントロールを表示する
                chkEnableOffset.Visible = true;
                txtOffset.Visible = true;
                lblMiliSecond.Visible = true;
                chkEnableHexaOffset.Visible = true;
                label1.Visible = true;
                chkEnableDuoOffset.Visible = true;
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
        private void txtScrollSpeed_TextChanged(object sender, EventArgs e)
        {
            userInputTempData.bpm = txtScrollSpeed.Text;
        }
        private void btnSetCurrentBpm_Click(object sender, EventArgs e)
        {
            SetCurrentTimeInfo(txtScrollSpeed, Constants.SET_BPM);
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
            // 処理対象の譜面を、処理を始めた時点の譜面に固定する
            SetTargetBeatmap();
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