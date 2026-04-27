using System.Windows.Forms;

namespace osu_taiko_Mapping_Helper
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            pnlBeatmapInfoGroup = new Panel();
            lblFileName = new Label();
            picDisplayBg = new PictureBox();
            txtTimingFrom = new TextBox();
            txtTimingTo = new TextBox();
            txtSvFrom = new TextBox();
            txtSvTo = new TextBox();
            txtVolumeFrom = new TextBox();
            txtVolumeTo = new TextBox();
            lblTiming = new Label();
            chkEnableSv = new CheckBox();
            rdoArithmetic = new RadioButton();
            rdoGeometric = new RadioButton();
            chkEnableVolume = new CheckBox();
            pnlCalcurationTypeGroup = new Panel();
            btnApply = new Button();
            chkEnableOffset = new CheckBox();
            txtOffset = new TextBox();
            lblMiliSecond = new Label();
            btnSwapTiming = new Button();
            btnSwapSv = new Button();
            btnSwapVolume = new Button();
            btnRemove = new Button();
            txtBeatSnap = new TextBox();
            lblBeatSnap = new Label();
            picSpecificNormalDong = new PictureBox();
            picSpecificNormalKa = new PictureBox();
            picSpecificFinisherDong = new PictureBox();
            picSpecificFinisherKa = new PictureBox();
            lblSpecificGridLine = new Label();
            lblSpecificFinisher = new Label();
            lblSpecificNormal = new Label();
            picSpecificNormalSlider = new PictureBox();
            picSpecificFinisherSlider = new PictureBox();
            lblSpecificGridLine2 = new Label();
            picSpecificNormalSpinner = new PictureBox();
            tabExecuteType = new TabControl();
            tabApplyPage = new TabPage();
            chkEnableKiai = new CheckBox();
            tabSetType = new TabControl();
            tabHitObjectsPage = new TabPage();
            rdoOnlyOffNotes = new RadioButton();
            rdoOnlyOnNotes = new RadioButton();
            pnlSpecificGroup = new Panel();
            rdoAllHitObjects = new RadioButton();
            rdoOnlyBarline = new RadioButton();
            rdoOnlyBookMark = new RadioButton();
            rdoOnlySpecificHitObject = new RadioButton();
            tabBeatSnap = new TabPage();
            lblBeatSnaps = new Label();
            tabGreenLine = new TabPage();
            tabRemovePage = new TabPage();
            chkEnableStartOffset = new CheckBox();
            txtStartOffset = new TextBox();
            lbllblMiliSecondRemove = new Label();
            chkApplyEndObject = new CheckBox();
            chkApplyStartObject = new CheckBox();
            lblCalculationType = new Label();
            btnSetTimingFrom = new Button();
            btnSetTimingTo = new Button();
            pnlRelativeSvGroup = new Panel();
            txtRelativeBaseSv = new TextBox();
            lblRelativeBaseSv = new Label();
            rdoRelativeMultiply = new RadioButton();
            rdoRelativeSum = new RadioButton();
            chkRelative = new CheckBox();
            btnViewSetting = new Button();
            chkEnableSvTo = new CheckBox();
            btnSetSvTo = new Button();
            btnSetSvFrom = new Button();
            btnSetVolumeTo = new Button();
            btnSetVolumeFrom = new Button();
            menuStrip1 = new MenuStrip();
            sVEditorToolStripMenuItem = new ToolStripMenuItem();
            utilityToolStripMenuItem = new ToolStripMenuItem();
            timingPropertyToolStripMenuItem = new ToolStripMenuItem();
            pnlGroupSvEditor = new Panel();
            pnlGroupUtility = new Panel();
            label8 = new Label();
            txtTimingOffset = new TextBox();
            btnApplyOffsetShifter = new Button();
            lblOffsetShifter = new Label();
            btnApplyResnap = new Button();
            cmbResnapBeatSnap = new ComboBox();
            lblBeatSnapDivisor = new Label();
            lblResnap = new Label();
            btnApplySettingCopier = new Button();
            lblSettingCopier = new Label();
            cmbSettingCopier = new ComboBox();
            txtTags = new TextBox();
            btnApplyTagEditor = new Button();
            lblTagEditor = new Label();
            btnApplyNotesPosition = new Button();
            lblNotesPosition = new Label();
            cmbNotesPosition = new ComboBox();
            btnApplyHitsoundOption = new Button();
            lblHitsoundOption = new Label();
            cmbHitsoundTool = new ComboBox();
            btnSetResnapTimingTo = new Button();
            btnSetResnapTimingFrom = new Button();
            btnSwapResnapTiming = new Button();
            lblResnapTiming = new Label();
            txtResnapTimingFrom = new TextBox();
            txtResnapTimingTo = new TextBox();
            btnBackup = new Button();
            pnlBeatmapInfoGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picDisplayBg).BeginInit();
            pnlCalcurationTypeGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSpecificNormalDong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificNormalKa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificFinisherDong).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificFinisherKa).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificNormalSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificFinisherSlider).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificNormalSpinner).BeginInit();
            tabExecuteType.SuspendLayout();
            tabApplyPage.SuspendLayout();
            tabSetType.SuspendLayout();
            tabHitObjectsPage.SuspendLayout();
            pnlSpecificGroup.SuspendLayout();
            tabBeatSnap.SuspendLayout();
            tabRemovePage.SuspendLayout();
            pnlRelativeSvGroup.SuspendLayout();
            menuStrip1.SuspendLayout();
            pnlGroupSvEditor.SuspendLayout();
            pnlGroupUtility.SuspendLayout();
            SuspendLayout();
            // 
            // pnlBeatmapInfoGroup
            // 
            pnlBeatmapInfoGroup.BackColor = SystemColors.ControlDarkDark;
            pnlBeatmapInfoGroup.Controls.Add(lblFileName);
            pnlBeatmapInfoGroup.Controls.Add(picDisplayBg);
            pnlBeatmapInfoGroup.Location = new Point(12, 31);
            pnlBeatmapInfoGroup.Name = "pnlBeatmapInfoGroup";
            pnlBeatmapInfoGroup.Size = new Size(384, 216);
            pnlBeatmapInfoGroup.TabIndex = 12;
            // 
            // lblFileName
            // 
            lblFileName.AllowDrop = true;
            lblFileName.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblFileName.BackColor = Color.Transparent;
            lblFileName.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblFileName.ForeColor = Color.White;
            lblFileName.Location = new Point(1, 63);
            lblFileName.Name = "lblFileName";
            lblFileName.Size = new Size(382, 90);
            lblFileName.TabIndex = 0;
            lblFileName.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picDisplayBg
            // 
            picDisplayBg.BackColor = SystemColors.GrayText;
            picDisplayBg.Location = new Point(0, 0);
            picDisplayBg.Name = "picDisplayBg";
            picDisplayBg.Size = new Size(384, 216);
            picDisplayBg.TabIndex = 1;
            picDisplayBg.TabStop = false;
            // 
            // txtTimingFrom
            // 
            txtTimingFrom.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtTimingFrom.BorderStyle = BorderStyle.FixedSingle;
            txtTimingFrom.ForeColor = SystemColors.WindowText;
            txtTimingFrom.Location = new Point(88, 10);
            txtTimingFrom.Name = "txtTimingFrom";
            txtTimingFrom.Size = new Size(120, 23);
            txtTimingFrom.TabIndex = 1;
            txtTimingFrom.TextAlign = HorizontalAlignment.Center;
            txtTimingFrom.TextChanged += txtTimingFrom_TextChanged;
            txtTimingFrom.Enter += txtTimingFrom_Enter;
            // 
            // txtTimingTo
            // 
            txtTimingTo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtTimingTo.BorderStyle = BorderStyle.FixedSingle;
            txtTimingTo.Location = new Point(271, 10);
            txtTimingTo.Name = "txtTimingTo";
            txtTimingTo.Size = new Size(120, 23);
            txtTimingTo.TabIndex = 2;
            txtTimingTo.TextAlign = HorizontalAlignment.Center;
            txtTimingTo.TextChanged += txtTimingTo_TextChanged;
            txtTimingTo.Enter += txtTimingTo_Enter;
            // 
            // txtSvFrom
            // 
            txtSvFrom.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSvFrom.BackColor = SystemColors.Window;
            txtSvFrom.BorderStyle = BorderStyle.FixedSingle;
            txtSvFrom.Location = new Point(88, 62);
            txtSvFrom.Name = "txtSvFrom";
            txtSvFrom.Size = new Size(120, 23);
            txtSvFrom.TabIndex = 3;
            txtSvFrom.TextAlign = HorizontalAlignment.Center;
            txtSvFrom.TextChanged += txtSvFrom_TextChanged;
            txtSvFrom.Enter += txtSvFrom_Enter;
            // 
            // txtSvTo
            // 
            txtSvTo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtSvTo.BorderStyle = BorderStyle.FixedSingle;
            txtSvTo.Location = new Point(271, 62);
            txtSvTo.Name = "txtSvTo";
            txtSvTo.Size = new Size(120, 23);
            txtSvTo.TabIndex = 4;
            txtSvTo.TextAlign = HorizontalAlignment.Center;
            txtSvTo.TextChanged += txtSvTo_TextChanged;
            txtSvTo.Enter += txtSvTo_Enter;
            // 
            // txtVolumeFrom
            // 
            txtVolumeFrom.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtVolumeFrom.BorderStyle = BorderStyle.FixedSingle;
            txtVolumeFrom.Location = new Point(88, 113);
            txtVolumeFrom.Name = "txtVolumeFrom";
            txtVolumeFrom.Size = new Size(120, 23);
            txtVolumeFrom.TabIndex = 5;
            txtVolumeFrom.TextAlign = HorizontalAlignment.Center;
            txtVolumeFrom.TextChanged += txtVolumeFrom_TextChanged;
            txtVolumeFrom.Enter += txtVolumeFrom_Enter;
            // 
            // txtVolumeTo
            // 
            txtVolumeTo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtVolumeTo.BorderStyle = BorderStyle.FixedSingle;
            txtVolumeTo.Location = new Point(271, 113);
            txtVolumeTo.Name = "txtVolumeTo";
            txtVolumeTo.Size = new Size(120, 23);
            txtVolumeTo.TabIndex = 6;
            txtVolumeTo.TextAlign = HorizontalAlignment.Center;
            txtVolumeTo.TextChanged += txtVolumeTo_TextChanged;
            txtVolumeTo.Enter += txtVolumeTo_Enter;
            // 
            // lblTiming
            // 
            lblTiming.AutoSize = true;
            lblTiming.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblTiming.ForeColor = Color.White;
            lblTiming.Location = new Point(24, 10);
            lblTiming.Name = "lblTiming";
            lblTiming.Size = new Size(56, 20);
            lblTiming.TabIndex = 12;
            lblTiming.Text = "Timing";
            // 
            // chkEnableSv
            // 
            chkEnableSv.Checked = true;
            chkEnableSv.CheckState = CheckState.Checked;
            chkEnableSv.Font = new Font("Yu Gothic UI", 11F, FontStyle.Bold);
            chkEnableSv.ForeColor = Color.White;
            chkEnableSv.Location = new Point(9, 61);
            chkEnableSv.Name = "chkEnableSv";
            chkEnableSv.Size = new Size(80, 24);
            chkEnableSv.TabIndex = 11;
            chkEnableSv.TabStop = false;
            chkEnableSv.Text = "SV";
            chkEnableSv.UseVisualStyleBackColor = true;
            chkEnableSv.CheckedChanged += chkEnableSv_CheckedChanged;
            // 
            // rdoArithmetic
            // 
            rdoArithmetic.Checked = true;
            rdoArithmetic.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            rdoArithmetic.ForeColor = Color.White;
            rdoArithmetic.Location = new Point(14, 5);
            rdoArithmetic.Name = "rdoArithmetic";
            rdoArithmetic.Size = new Size(90, 24);
            rdoArithmetic.TabIndex = 0;
            rdoArithmetic.TabStop = true;
            rdoArithmetic.Text = "等差";
            rdoArithmetic.UseVisualStyleBackColor = true;
            rdoArithmetic.CheckedChanged += rdoArithmetic_CheckedChanged;
            // 
            // rdoGeometric
            // 
            rdoGeometric.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            rdoGeometric.ForeColor = Color.White;
            rdoGeometric.Location = new Point(14, 31);
            rdoGeometric.Name = "rdoGeometric";
            rdoGeometric.Size = new Size(90, 24);
            rdoGeometric.TabIndex = 1;
            rdoGeometric.Text = "等比";
            rdoGeometric.UseVisualStyleBackColor = true;
            rdoGeometric.CheckedChanged += rdoGeometric_CheckedChanged;
            // 
            // chkEnableVolume
            // 
            chkEnableVolume.Checked = true;
            chkEnableVolume.CheckState = CheckState.Checked;
            chkEnableVolume.Font = new Font("Yu Gothic UI", 10F, FontStyle.Bold);
            chkEnableVolume.ForeColor = Color.White;
            chkEnableVolume.Location = new Point(9, 113);
            chkEnableVolume.Name = "chkEnableVolume";
            chkEnableVolume.Size = new Size(80, 24);
            chkEnableVolume.TabIndex = 10;
            chkEnableVolume.TabStop = false;
            chkEnableVolume.Text = "Volume";
            chkEnableVolume.UseVisualStyleBackColor = true;
            chkEnableVolume.CheckedChanged += chkEnableVolume_CheckedChanged;
            // 
            // pnlCalcurationTypeGroup
            // 
            pnlCalcurationTypeGroup.BorderStyle = BorderStyle.Fixed3D;
            pnlCalcurationTypeGroup.Controls.Add(rdoArithmetic);
            pnlCalcurationTypeGroup.Controls.Add(rdoGeometric);
            pnlCalcurationTypeGroup.Location = new Point(9, 212);
            pnlCalcurationTypeGroup.Name = "pnlCalcurationTypeGroup";
            pnlCalcurationTypeGroup.Size = new Size(113, 66);
            pnlCalcurationTypeGroup.TabIndex = 9;
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.DarkCyan;
            btnApply.FlatAppearance.BorderColor = Color.Cyan;
            btnApply.FlatAppearance.BorderSize = 2;
            btnApply.FlatStyle = FlatStyle.Flat;
            btnApply.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApply.ForeColor = SystemColors.ControlLightLight;
            btnApply.Location = new Point(102, 232);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(181, 39);
            btnApply.TabIndex = 0;
            btnApply.TabStop = false;
            btnApply.Text = "実行";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // chkEnableOffset
            // 
            chkEnableOffset.AutoSize = true;
            chkEnableOffset.Checked = true;
            chkEnableOffset.CheckState = CheckState.Checked;
            chkEnableOffset.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            chkEnableOffset.ForeColor = Color.White;
            chkEnableOffset.Location = new Point(184, 14);
            chkEnableOffset.Name = "chkEnableOffset";
            chkEnableOffset.Size = new Size(75, 21);
            chkEnableOffset.TabIndex = 3;
            chkEnableOffset.TabStop = false;
            chkEnableOffset.Text = "オフセット";
            chkEnableOffset.UseVisualStyleBackColor = true;
            chkEnableOffset.CheckedChanged += chkEnableOffset_CheckedChanged;
            // 
            // txtOffset
            // 
            txtOffset.BackColor = SystemColors.Window;
            txtOffset.BorderStyle = BorderStyle.FixedSingle;
            txtOffset.Location = new Point(295, 12);
            txtOffset.Name = "txtOffset";
            txtOffset.Size = new Size(40, 27);
            txtOffset.TabIndex = 4;
            txtOffset.TabStop = false;
            txtOffset.TextChanged += txtOffset_TextChanged;
            // 
            // lblMiliSecond
            // 
            lblMiliSecond.AutoSize = true;
            lblMiliSecond.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            lblMiliSecond.ForeColor = Color.White;
            lblMiliSecond.Location = new Point(340, 17);
            lblMiliSecond.Name = "lblMiliSecond";
            lblMiliSecond.Size = new Size(23, 15);
            lblMiliSecond.TabIndex = 5;
            lblMiliSecond.Text = "ms";
            // 
            // btnSwapTiming
            // 
            btnSwapTiming.FlatAppearance.BorderColor = Color.Cyan;
            btnSwapTiming.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSwapTiming.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSwapTiming.FlatStyle = FlatStyle.Flat;
            btnSwapTiming.ForeColor = Color.Cyan;
            btnSwapTiming.Location = new Point(207, 10);
            btnSwapTiming.Name = "btnSwapTiming";
            btnSwapTiming.Size = new Size(65, 23);
            btnSwapTiming.TabIndex = 7;
            btnSwapTiming.TabStop = false;
            btnSwapTiming.Text = "⇔";
            btnSwapTiming.UseVisualStyleBackColor = true;
            btnSwapTiming.Click += btnSwapTiming_Click;
            // 
            // btnSwapSv
            // 
            btnSwapSv.FlatAppearance.BorderColor = Color.Cyan;
            btnSwapSv.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSwapSv.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSwapSv.FlatStyle = FlatStyle.Flat;
            btnSwapSv.ForeColor = Color.Cyan;
            btnSwapSv.Location = new Point(207, 62);
            btnSwapSv.Name = "btnSwapSv";
            btnSwapSv.Size = new Size(65, 23);
            btnSwapSv.TabIndex = 6;
            btnSwapSv.TabStop = false;
            btnSwapSv.Text = "⇔";
            btnSwapSv.UseVisualStyleBackColor = true;
            btnSwapSv.Click += btnSwapSv_Click;
            // 
            // btnSwapVolume
            // 
            btnSwapVolume.FlatAppearance.BorderColor = Color.Cyan;
            btnSwapVolume.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSwapVolume.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSwapVolume.FlatStyle = FlatStyle.Flat;
            btnSwapVolume.ForeColor = Color.Cyan;
            btnSwapVolume.Location = new Point(207, 113);
            btnSwapVolume.Name = "btnSwapVolume";
            btnSwapVolume.Size = new Size(65, 23);
            btnSwapVolume.TabIndex = 5;
            btnSwapVolume.TabStop = false;
            btnSwapVolume.Text = "⇔";
            btnSwapVolume.UseVisualStyleBackColor = true;
            btnSwapVolume.Click += btnSwapVolume_Click;
            // 
            // btnRemove
            // 
            btnRemove.BackColor = Color.DarkCyan;
            btnRemove.FlatAppearance.BorderColor = Color.Cyan;
            btnRemove.FlatAppearance.BorderSize = 2;
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnRemove.ForeColor = SystemColors.ControlLightLight;
            btnRemove.Location = new Point(102, 232);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(181, 39);
            btnRemove.TabIndex = 10;
            btnRemove.TabStop = false;
            btnRemove.Text = "実行";
            btnRemove.UseVisualStyleBackColor = false;
            btnRemove.Click += btnRemove_Click;
            // 
            // txtBeatSnap
            // 
            txtBeatSnap.BackColor = SystemColors.Window;
            txtBeatSnap.BorderStyle = BorderStyle.FixedSingle;
            txtBeatSnap.Location = new Point(131, 14);
            txtBeatSnap.Name = "txtBeatSnap";
            txtBeatSnap.Size = new Size(40, 25);
            txtBeatSnap.TabIndex = 2;
            txtBeatSnap.TabStop = false;
            txtBeatSnap.TextChanged += txtBeatSnap_TextChanged;
            // 
            // lblBeatSnap
            // 
            lblBeatSnap.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblBeatSnap.ForeColor = Color.White;
            lblBeatSnap.Location = new Point(114, 17);
            lblBeatSnap.Name = "lblBeatSnap";
            lblBeatSnap.Size = new Size(17, 20);
            lblBeatSnap.TabIndex = 1;
            lblBeatSnap.Text = "1/";
            // 
            // picSpecificNormalDong
            // 
            picSpecificNormalDong.Location = new Point(186, 8);
            picSpecificNormalDong.Name = "picSpecificNormalDong";
            picSpecificNormalDong.Size = new Size(42, 42);
            picSpecificNormalDong.TabIndex = 16;
            picSpecificNormalDong.TabStop = false;
            // 
            // picSpecificNormalKa
            // 
            picSpecificNormalKa.Location = new Point(229, 8);
            picSpecificNormalKa.Name = "picSpecificNormalKa";
            picSpecificNormalKa.Size = new Size(42, 42);
            picSpecificNormalKa.TabIndex = 15;
            picSpecificNormalKa.TabStop = false;
            // 
            // picSpecificFinisherDong
            // 
            picSpecificFinisherDong.Location = new Point(186, 51);
            picSpecificFinisherDong.Name = "picSpecificFinisherDong";
            picSpecificFinisherDong.Size = new Size(42, 42);
            picSpecificFinisherDong.TabIndex = 12;
            picSpecificFinisherDong.TabStop = false;
            // 
            // picSpecificFinisherKa
            // 
            picSpecificFinisherKa.Location = new Point(229, 51);
            picSpecificFinisherKa.Name = "picSpecificFinisherKa";
            picSpecificFinisherKa.Size = new Size(42, 42);
            picSpecificFinisherKa.TabIndex = 11;
            picSpecificFinisherKa.TabStop = false;
            // 
            // lblSpecificGridLine
            // 
            lblSpecificGridLine.BackColor = Color.White;
            lblSpecificGridLine.Location = new Point(158, 7);
            lblSpecificGridLine.Name = "lblSpecificGridLine";
            lblSpecificGridLine.Size = new Size(157, 87);
            lblSpecificGridLine.TabIndex = 17;
            lblSpecificGridLine.Text = " ";
            // 
            // lblSpecificFinisher
            // 
            lblSpecificFinisher.BackColor = SystemColors.WindowText;
            lblSpecificFinisher.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            lblSpecificFinisher.ForeColor = SystemColors.Window;
            lblSpecificFinisher.Location = new Point(159, 51);
            lblSpecificFinisher.Name = "lblSpecificFinisher";
            lblSpecificFinisher.Size = new Size(26, 42);
            lblSpecificFinisher.TabIndex = 9;
            lblSpecificFinisher.Text = "大音符";
            lblSpecificFinisher.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblSpecificNormal
            // 
            lblSpecificNormal.BackColor = SystemColors.WindowText;
            lblSpecificNormal.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 128);
            lblSpecificNormal.ForeColor = SystemColors.Window;
            lblSpecificNormal.Location = new Point(159, 8);
            lblSpecificNormal.Name = "lblSpecificNormal";
            lblSpecificNormal.Size = new Size(26, 42);
            lblSpecificNormal.TabIndex = 7;
            lblSpecificNormal.Text = "通常";
            lblSpecificNormal.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // picSpecificNormalSlider
            // 
            picSpecificNormalSlider.Location = new Point(272, 8);
            picSpecificNormalSlider.Name = "picSpecificNormalSlider";
            picSpecificNormalSlider.Size = new Size(42, 42);
            picSpecificNormalSlider.TabIndex = 10;
            picSpecificNormalSlider.TabStop = false;
            // 
            // picSpecificFinisherSlider
            // 
            picSpecificFinisherSlider.Location = new Point(272, 51);
            picSpecificFinisherSlider.Name = "picSpecificFinisherSlider";
            picSpecificFinisherSlider.Size = new Size(42, 42);
            picSpecificFinisherSlider.TabIndex = 13;
            picSpecificFinisherSlider.TabStop = false;
            // 
            // lblSpecificGridLine2
            // 
            lblSpecificGridLine2.BackColor = Color.White;
            lblSpecificGridLine2.Location = new Point(315, 7);
            lblSpecificGridLine2.Name = "lblSpecificGridLine2";
            lblSpecificGridLine2.Size = new Size(43, 44);
            lblSpecificGridLine2.TabIndex = 14;
            lblSpecificGridLine2.Text = " ";
            // 
            // picSpecificNormalSpinner
            // 
            picSpecificNormalSpinner.Location = new Point(315, 8);
            picSpecificNormalSpinner.Name = "picSpecificNormalSpinner";
            picSpecificNormalSpinner.Size = new Size(42, 42);
            picSpecificNormalSpinner.TabIndex = 8;
            picSpecificNormalSpinner.TabStop = false;
            // 
            // tabExecuteType
            // 
            tabExecuteType.Controls.Add(tabApplyPage);
            tabExecuteType.Controls.Add(tabRemovePage);
            tabExecuteType.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabExecuteType.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            tabExecuteType.ItemSize = new Size(195, 40);
            tabExecuteType.Location = new Point(4, 303);
            tabExecuteType.Name = "tabExecuteType";
            tabExecuteType.SelectedIndex = 0;
            tabExecuteType.Size = new Size(393, 337);
            tabExecuteType.SizeMode = TabSizeMode.Fixed;
            tabExecuteType.TabIndex = 3;
            tabExecuteType.TabStop = false;
            tabExecuteType.DrawItem += tabExecuteType_DrawItem;
            tabExecuteType.SelectedIndexChanged += tabExecuteType_SelectedIndexChanged;
            // 
            // tabApplyPage
            // 
            tabApplyPage.BackColor = Color.FromArgb(0, 64, 64);
            tabApplyPage.BorderStyle = BorderStyle.FixedSingle;
            tabApplyPage.Controls.Add(btnApply);
            tabApplyPage.Controls.Add(chkEnableKiai);
            tabApplyPage.Controls.Add(tabSetType);
            tabApplyPage.Controls.Add(chkEnableOffset);
            tabApplyPage.Controls.Add(txtOffset);
            tabApplyPage.Controls.Add(lblMiliSecond);
            tabApplyPage.ForeColor = Color.DarkCyan;
            tabApplyPage.Location = new Point(4, 44);
            tabApplyPage.Name = "tabApplyPage";
            tabApplyPage.Padding = new Padding(3);
            tabApplyPage.RightToLeft = RightToLeft.No;
            tabApplyPage.Size = new Size(385, 289);
            tabApplyPage.TabIndex = 0;
            tabApplyPage.Text = "適用";
            // 
            // chkEnableKiai
            // 
            chkEnableKiai.AutoSize = true;
            chkEnableKiai.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            chkEnableKiai.ForeColor = Color.White;
            chkEnableKiai.Location = new Point(16, 14);
            chkEnableKiai.Name = "chkEnableKiai";
            chkEnableKiai.Size = new Size(47, 21);
            chkEnableKiai.TabIndex = 1;
            chkEnableKiai.TabStop = false;
            chkEnableKiai.Text = "kiai";
            chkEnableKiai.UseVisualStyleBackColor = true;
            chkEnableKiai.CheckedChanged += chkEnableKiai_CheckedChanged;
            // 
            // tabSetType
            // 
            tabSetType.Controls.Add(tabHitObjectsPage);
            tabSetType.Controls.Add(tabBeatSnap);
            tabSetType.Controls.Add(tabGreenLine);
            tabSetType.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabSetType.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            tabSetType.ImeMode = ImeMode.NoControl;
            tabSetType.ItemSize = new Size(126, 40);
            tabSetType.Location = new Point(0, 50);
            tabSetType.Name = "tabSetType";
            tabSetType.SelectedIndex = 0;
            tabSetType.Size = new Size(383, 168);
            tabSetType.SizeMode = TabSizeMode.Fixed;
            tabSetType.TabIndex = 2;
            tabSetType.TabStop = false;
            tabSetType.DrawItem += tabSetType_DrawItem;
            tabSetType.SelectedIndexChanged += tabSetType_SelectedIndexChanged;
            // 
            // tabHitObjectsPage
            // 
            tabHitObjectsPage.BackColor = Color.FromArgb(0, 64, 64);
            tabHitObjectsPage.BorderStyle = BorderStyle.FixedSingle;
            tabHitObjectsPage.Controls.Add(rdoOnlyOffNotes);
            tabHitObjectsPage.Controls.Add(rdoOnlyOnNotes);
            tabHitObjectsPage.Controls.Add(pnlSpecificGroup);
            tabHitObjectsPage.Controls.Add(lblSpecificNormal);
            tabHitObjectsPage.Controls.Add(picSpecificNormalSpinner);
            tabHitObjectsPage.Controls.Add(lblSpecificFinisher);
            tabHitObjectsPage.Controls.Add(picSpecificNormalSlider);
            tabHitObjectsPage.Controls.Add(picSpecificFinisherKa);
            tabHitObjectsPage.Controls.Add(picSpecificFinisherDong);
            tabHitObjectsPage.Controls.Add(picSpecificFinisherSlider);
            tabHitObjectsPage.Controls.Add(lblSpecificGridLine2);
            tabHitObjectsPage.Controls.Add(picSpecificNormalKa);
            tabHitObjectsPage.Controls.Add(picSpecificNormalDong);
            tabHitObjectsPage.Controls.Add(lblSpecificGridLine);
            tabHitObjectsPage.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            tabHitObjectsPage.ForeColor = Color.DarkCyan;
            tabHitObjectsPage.Location = new Point(4, 44);
            tabHitObjectsPage.Name = "tabHitObjectsPage";
            tabHitObjectsPage.Padding = new Padding(3);
            tabHitObjectsPage.RightToLeft = RightToLeft.No;
            tabHitObjectsPage.Size = new Size(375, 120);
            tabHitObjectsPage.TabIndex = 0;
            tabHitObjectsPage.Text = "Objectsのみ";
            // 
            // rdoOnlyOffNotes
            // 
            rdoOnlyOffNotes.AutoSize = true;
            rdoOnlyOffNotes.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoOnlyOffNotes.ForeColor = Color.White;
            rdoOnlyOffNotes.Location = new Point(158, 30);
            rdoOnlyOffNotes.Name = "rdoOnlyOffNotes";
            rdoOnlyOffNotes.Size = new Size(97, 19);
            rdoOnlyOffNotes.TabIndex = 0;
            rdoOnlyOffNotes.Text = "小節線上以外";
            rdoOnlyOffNotes.UseVisualStyleBackColor = true;
            // 
            // rdoOnlyOnNotes
            // 
            rdoOnlyOnNotes.AutoSize = true;
            rdoOnlyOnNotes.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoOnlyOnNotes.ForeColor = Color.White;
            rdoOnlyOnNotes.Location = new Point(158, 8);
            rdoOnlyOnNotes.Name = "rdoOnlyOnNotes";
            rdoOnlyOnNotes.Size = new Size(94, 19);
            rdoOnlyOnNotes.TabIndex = 1;
            rdoOnlyOnNotes.Text = "小節線上のみ";
            rdoOnlyOnNotes.UseVisualStyleBackColor = true;
            // 
            // pnlSpecificGroup
            // 
            pnlSpecificGroup.Controls.Add(rdoAllHitObjects);
            pnlSpecificGroup.Controls.Add(rdoOnlyBarline);
            pnlSpecificGroup.Controls.Add(rdoOnlyBookMark);
            pnlSpecificGroup.Controls.Add(rdoOnlySpecificHitObject);
            pnlSpecificGroup.Location = new Point(6, 6);
            pnlSpecificGroup.Name = "pnlSpecificGroup";
            pnlSpecificGroup.Size = new Size(136, 123);
            pnlSpecificGroup.TabIndex = 0;
            // 
            // rdoAllHitObjects
            // 
            rdoAllHitObjects.AutoSize = true;
            rdoAllHitObjects.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoAllHitObjects.ForeColor = Color.White;
            rdoAllHitObjects.Location = new Point(4, 3);
            rdoAllHitObjects.Name = "rdoAllHitObjects";
            rdoAllHitObjects.Size = new Size(115, 19);
            rdoAllHitObjects.TabIndex = 0;
            rdoAllHitObjects.Text = "すべてのHitObject";
            rdoAllHitObjects.UseVisualStyleBackColor = true;
            rdoAllHitObjects.CheckedChanged += rdoAllHitObjects_CheckedChanged;
            // 
            // rdoOnlyBarline
            // 
            rdoOnlyBarline.AutoSize = true;
            rdoOnlyBarline.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoOnlyBarline.ForeColor = Color.White;
            rdoOnlyBarline.Location = new Point(4, 28);
            rdoOnlyBarline.Name = "rdoOnlyBarline";
            rdoOnlyBarline.Size = new Size(61, 19);
            rdoOnlyBarline.TabIndex = 1;
            rdoOnlyBarline.Text = "小節線";
            rdoOnlyBarline.UseVisualStyleBackColor = true;
            rdoOnlyBarline.CheckedChanged += rdoOnlyBarline_CheckedChanged;
            // 
            // rdoOnlyBookMark
            // 
            rdoOnlyBookMark.AutoSize = true;
            rdoOnlyBookMark.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoOnlyBookMark.ForeColor = Color.White;
            rdoOnlyBookMark.Location = new Point(4, 53);
            rdoOnlyBookMark.Name = "rdoOnlyBookMark";
            rdoOnlyBookMark.Size = new Size(79, 19);
            rdoOnlyBookMark.TabIndex = 2;
            rdoOnlyBookMark.Text = "BookMark";
            rdoOnlyBookMark.UseVisualStyleBackColor = true;
            rdoOnlyBookMark.CheckedChanged += rdoOnlyBookMark_CheckedChanged;
            // 
            // rdoOnlySpecificHitObject
            // 
            rdoOnlySpecificHitObject.AutoSize = true;
            rdoOnlySpecificHitObject.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoOnlySpecificHitObject.ForeColor = Color.White;
            rdoOnlySpecificHitObject.Location = new Point(4, 78);
            rdoOnlySpecificHitObject.Name = "rdoOnlySpecificHitObject";
            rdoOnlySpecificHitObject.Size = new Size(132, 19);
            rdoOnlySpecificHitObject.TabIndex = 3;
            rdoOnlySpecificHitObject.Text = "特定のオブジェクトのみ";
            rdoOnlySpecificHitObject.UseVisualStyleBackColor = true;
            rdoOnlySpecificHitObject.CheckedChanged += rdoOnlySpecificHitObject_CheckedChanged;
            // 
            // tabBeatSnap
            // 
            tabBeatSnap.BackColor = Color.FromArgb(0, 64, 64);
            tabBeatSnap.BorderStyle = BorderStyle.FixedSingle;
            tabBeatSnap.Controls.Add(lblBeatSnaps);
            tabBeatSnap.Controls.Add(lblBeatSnap);
            tabBeatSnap.Controls.Add(txtBeatSnap);
            tabBeatSnap.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            tabBeatSnap.ForeColor = Color.DarkCyan;
            tabBeatSnap.Location = new Point(4, 44);
            tabBeatSnap.Name = "tabBeatSnap";
            tabBeatSnap.Padding = new Padding(3);
            tabBeatSnap.RightToLeft = RightToLeft.No;
            tabBeatSnap.Size = new Size(375, 120);
            tabBeatSnap.TabIndex = 1;
            tabBeatSnap.Text = "BeatSnap";
            // 
            // lblBeatSnaps
            // 
            lblBeatSnaps.AutoSize = true;
            lblBeatSnaps.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            lblBeatSnaps.ForeColor = Color.White;
            lblBeatSnaps.Location = new Point(11, 18);
            lblBeatSnaps.Name = "lblBeatSnaps";
            lblBeatSnaps.Size = new Size(100, 15);
            lblBeatSnaps.TabIndex = 3;
            lblBeatSnaps.Text = "Beat Snap Divisor";
            // 
            // tabGreenLine
            // 
            tabGreenLine.BackColor = Color.FromArgb(0, 64, 64);
            tabGreenLine.BorderStyle = BorderStyle.FixedSingle;
            tabGreenLine.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            tabGreenLine.ImeMode = ImeMode.NoControl;
            tabGreenLine.Location = new Point(4, 44);
            tabGreenLine.Name = "tabGreenLine";
            tabGreenLine.Padding = new Padding(3);
            tabGreenLine.Size = new Size(375, 120);
            tabGreenLine.TabIndex = 2;
            tabGreenLine.Text = "Inherited Point";
            // 
            // tabRemovePage
            // 
            tabRemovePage.BackColor = Color.FromArgb(0, 64, 64);
            tabRemovePage.BorderStyle = BorderStyle.FixedSingle;
            tabRemovePage.Controls.Add(chkEnableStartOffset);
            tabRemovePage.Controls.Add(txtStartOffset);
            tabRemovePage.Controls.Add(lbllblMiliSecondRemove);
            tabRemovePage.Controls.Add(btnRemove);
            tabRemovePage.ForeColor = Color.DarkCyan;
            tabRemovePage.Location = new Point(4, 44);
            tabRemovePage.Name = "tabRemovePage";
            tabRemovePage.Padding = new Padding(3);
            tabRemovePage.Size = new Size(385, 289);
            tabRemovePage.TabIndex = 1;
            tabRemovePage.Text = "削除";
            // 
            // chkEnableStartOffset
            // 
            chkEnableStartOffset.AutoSize = true;
            chkEnableStartOffset.Checked = true;
            chkEnableStartOffset.CheckState = CheckState.Checked;
            chkEnableStartOffset.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            chkEnableStartOffset.ForeColor = Color.White;
            chkEnableStartOffset.Location = new Point(184, 14);
            chkEnableStartOffset.Name = "chkEnableStartOffset";
            chkEnableStartOffset.Size = new Size(112, 21);
            chkEnableStartOffset.TabIndex = 0;
            chkEnableStartOffset.TabStop = false;
            chkEnableStartOffset.Text = "始点のオフセット";
            chkEnableStartOffset.UseVisualStyleBackColor = true;
            chkEnableStartOffset.CheckedChanged += chkEnableStartOffset_CheckedChanged;
            // 
            // txtStartOffset
            // 
            txtStartOffset.BackColor = SystemColors.Window;
            txtStartOffset.BorderStyle = BorderStyle.FixedSingle;
            txtStartOffset.Location = new Point(295, 12);
            txtStartOffset.Name = "txtStartOffset";
            txtStartOffset.Size = new Size(40, 27);
            txtStartOffset.TabIndex = 1;
            txtStartOffset.TabStop = false;
            txtStartOffset.TextChanged += txtStartOffset_TextChanged;
            // 
            // lbllblMiliSecondRemove
            // 
            lbllblMiliSecondRemove.AutoSize = true;
            lbllblMiliSecondRemove.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            lbllblMiliSecondRemove.ForeColor = Color.White;
            lbllblMiliSecondRemove.Location = new Point(340, 17);
            lbllblMiliSecondRemove.Name = "lbllblMiliSecondRemove";
            lbllblMiliSecondRemove.Size = new Size(23, 15);
            lbllblMiliSecondRemove.TabIndex = 9;
            lbllblMiliSecondRemove.Text = "ms";
            // 
            // chkApplyEndObject
            // 
            chkApplyEndObject.AutoSize = true;
            chkApplyEndObject.Checked = true;
            chkApplyEndObject.CheckState = CheckState.Checked;
            chkApplyEndObject.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            chkApplyEndObject.ForeColor = Color.White;
            chkApplyEndObject.Location = new Point(271, 161);
            chkApplyEndObject.Name = "chkApplyEndObject";
            chkApplyEndObject.Size = new Size(89, 21);
            chkApplyEndObject.TabIndex = 2;
            chkApplyEndObject.TabStop = false;
            chkApplyEndObject.Text = "終点に適用";
            chkApplyEndObject.UseVisualStyleBackColor = true;
            chkApplyEndObject.CheckedChanged += chkApplyEndObject_CheckedChanged;
            // 
            // chkApplyStartObject
            // 
            chkApplyStartObject.AutoSize = true;
            chkApplyStartObject.Checked = true;
            chkApplyStartObject.CheckState = CheckState.Checked;
            chkApplyStartObject.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            chkApplyStartObject.ForeColor = Color.White;
            chkApplyStartObject.Location = new Point(88, 161);
            chkApplyStartObject.Name = "chkApplyStartObject";
            chkApplyStartObject.Size = new Size(89, 21);
            chkApplyStartObject.TabIndex = 3;
            chkApplyStartObject.TabStop = false;
            chkApplyStartObject.Text = "始点に適用";
            chkApplyStartObject.UseVisualStyleBackColor = true;
            chkApplyStartObject.CheckedChanged += chkApplyStartObject_CheckedChanged;
            // 
            // lblCalculationType
            // 
            lblCalculationType.AutoSize = true;
            lblCalculationType.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblCalculationType.ForeColor = Color.White;
            lblCalculationType.Location = new Point(12, 189);
            lblCalculationType.Name = "lblCalculationType";
            lblCalculationType.Size = new Size(69, 20);
            lblCalculationType.TabIndex = 2;
            lblCalculationType.Text = "計算方法";
            // 
            // btnSetTimingFrom
            // 
            btnSetTimingFrom.FlatAppearance.BorderColor = Color.Cyan;
            btnSetTimingFrom.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSetTimingFrom.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSetTimingFrom.FlatStyle = FlatStyle.Flat;
            btnSetTimingFrom.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Bold);
            btnSetTimingFrom.ForeColor = SystemColors.Control;
            btnSetTimingFrom.ImageAlign = ContentAlignment.TopCenter;
            btnSetTimingFrom.Location = new Point(88, 32);
            btnSetTimingFrom.Name = "btnSetTimingFrom";
            btnSetTimingFrom.Size = new Size(120, 24);
            btnSetTimingFrom.TabIndex = 1;
            btnSetTimingFrom.TabStop = false;
            btnSetTimingFrom.Text = "Set Start Timing";
            btnSetTimingFrom.UseVisualStyleBackColor = true;
            btnSetTimingFrom.Click += btnSetTimingFrom_Click;
            // 
            // btnSetTimingTo
            // 
            btnSetTimingTo.FlatAppearance.BorderColor = Color.Cyan;
            btnSetTimingTo.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSetTimingTo.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSetTimingTo.FlatStyle = FlatStyle.Flat;
            btnSetTimingTo.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Bold);
            btnSetTimingTo.ForeColor = SystemColors.Control;
            btnSetTimingTo.ImageAlign = ContentAlignment.TopCenter;
            btnSetTimingTo.Location = new Point(271, 32);
            btnSetTimingTo.Name = "btnSetTimingTo";
            btnSetTimingTo.Size = new Size(120, 24);
            btnSetTimingTo.TabIndex = 0;
            btnSetTimingTo.TabStop = false;
            btnSetTimingTo.Text = "Set End Timing";
            btnSetTimingTo.UseVisualStyleBackColor = true;
            btnSetTimingTo.Click += btnSetTimingTo_Click;
            // 
            // pnlRelativeSvGroup
            // 
            pnlRelativeSvGroup.BorderStyle = BorderStyle.Fixed3D;
            pnlRelativeSvGroup.Controls.Add(txtRelativeBaseSv);
            pnlRelativeSvGroup.Controls.Add(lblRelativeBaseSv);
            pnlRelativeSvGroup.Controls.Add(rdoRelativeMultiply);
            pnlRelativeSvGroup.Controls.Add(rdoRelativeSum);
            pnlRelativeSvGroup.Location = new Point(131, 212);
            pnlRelativeSvGroup.Name = "pnlRelativeSvGroup";
            pnlRelativeSvGroup.Size = new Size(140, 76);
            pnlRelativeSvGroup.TabIndex = 11;
            // 
            // txtRelativeBaseSv
            // 
            txtRelativeBaseSv.BackColor = SystemColors.Window;
            txtRelativeBaseSv.BorderStyle = BorderStyle.FixedSingle;
            txtRelativeBaseSv.ForeColor = SystemColors.WindowText;
            txtRelativeBaseSv.Location = new Point(62, 24);
            txtRelativeBaseSv.Name = "txtRelativeBaseSv";
            txtRelativeBaseSv.Size = new Size(62, 23);
            txtRelativeBaseSv.TabIndex = 6;
            txtRelativeBaseSv.TabStop = false;
            txtRelativeBaseSv.Text = "0";
            txtRelativeBaseSv.TextAlign = HorizontalAlignment.Right;
            txtRelativeBaseSv.TextChanged += txtRelativeBaseSv_TextChanged;
            // 
            // lblRelativeBaseSv
            // 
            lblRelativeBaseSv.AutoSize = true;
            lblRelativeBaseSv.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblRelativeBaseSv.ForeColor = Color.White;
            lblRelativeBaseSv.Location = new Point(7, 28);
            lblRelativeBaseSv.Name = "lblRelativeBaseSv";
            lblRelativeBaseSv.Size = new Size(46, 15);
            lblRelativeBaseSv.TabIndex = 14;
            lblRelativeBaseSv.Text = "基準SV";
            // 
            // rdoRelativeMultiply
            // 
            rdoRelativeMultiply.Checked = true;
            rdoRelativeMultiply.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            rdoRelativeMultiply.ForeColor = Color.White;
            rdoRelativeMultiply.Location = new Point(7, 3);
            rdoRelativeMultiply.Name = "rdoRelativeMultiply";
            rdoRelativeMultiply.Size = new Size(121, 24);
            rdoRelativeMultiply.TabIndex = 0;
            rdoRelativeMultiply.TabStop = true;
            rdoRelativeMultiply.Text = "乗算";
            rdoRelativeMultiply.UseVisualStyleBackColor = true;
            rdoRelativeMultiply.CheckedChanged += rdoRelativeMultiply_CheckedChanged;
            // 
            // rdoRelativeSum
            // 
            rdoRelativeSum.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold);
            rdoRelativeSum.ForeColor = Color.White;
            rdoRelativeSum.Location = new Point(7, 45);
            rdoRelativeSum.Name = "rdoRelativeSum";
            rdoRelativeSum.Size = new Size(117, 24);
            rdoRelativeSum.TabIndex = 1;
            rdoRelativeSum.Text = "加算";
            rdoRelativeSum.UseVisualStyleBackColor = true;
            rdoRelativeSum.CheckedChanged += rdoRelativeSum_CheckedChanged;
            // 
            // chkRelative
            // 
            chkRelative.AutoSize = true;
            chkRelative.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            chkRelative.ForeColor = Color.White;
            chkRelative.Location = new Point(134, 188);
            chkRelative.Name = "chkRelative";
            chkRelative.Size = new Size(118, 24);
            chkRelative.TabIndex = 13;
            chkRelative.Text = "相対速度変化";
            chkRelative.UseVisualStyleBackColor = true;
            chkRelative.CheckedChanged += chkRelative_CheckedChanged;
            // 
            // btnViewSetting
            // 
            btnViewSetting.BackColor = Color.DarkCyan;
            btnViewSetting.FlatAppearance.BorderColor = Color.Cyan;
            btnViewSetting.FlatAppearance.BorderSize = 2;
            btnViewSetting.FlatStyle = FlatStyle.Flat;
            btnViewSetting.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnViewSetting.ForeColor = SystemColors.ControlLightLight;
            btnViewSetting.Location = new Point(210, 909);
            btnViewSetting.Name = "btnViewSetting";
            btnViewSetting.Size = new Size(181, 39);
            btnViewSetting.TabIndex = 14;
            btnViewSetting.TabStop = false;
            btnViewSetting.Text = "設定";
            btnViewSetting.UseVisualStyleBackColor = false;
            btnViewSetting.Click += btnViewSetting_Click;
            // 
            // chkEnableSvTo
            // 
            chkEnableSvTo.AutoSize = true;
            chkEnableSvTo.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            chkEnableSvTo.ForeColor = Color.White;
            chkEnableSvTo.Location = new Point(277, 216);
            chkEnableSvTo.Name = "chkEnableSvTo";
            chkEnableSvTo.Size = new Size(96, 19);
            chkEnableSvTo.TabIndex = 15;
            chkEnableSvTo.Text = "終点の有効化";
            chkEnableSvTo.UseVisualStyleBackColor = true;
            chkEnableSvTo.Visible = false;
            chkEnableSvTo.CheckedChanged += chkEnableSvTo_CheckedChanged;
            // 
            // btnSetSvTo
            // 
            btnSetSvTo.FlatAppearance.BorderColor = Color.Cyan;
            btnSetSvTo.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSetSvTo.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSetSvTo.FlatStyle = FlatStyle.Flat;
            btnSetSvTo.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Bold);
            btnSetSvTo.ForeColor = SystemColors.Control;
            btnSetSvTo.ImageAlign = ContentAlignment.TopCenter;
            btnSetSvTo.Location = new Point(271, 84);
            btnSetSvTo.Name = "btnSetSvTo";
            btnSetSvTo.Size = new Size(120, 24);
            btnSetSvTo.TabIndex = 16;
            btnSetSvTo.TabStop = false;
            btnSetSvTo.Text = "Set End SV";
            btnSetSvTo.UseVisualStyleBackColor = true;
            btnSetSvTo.Click += btnSetSvTo_Click;
            // 
            // btnSetSvFrom
            // 
            btnSetSvFrom.FlatAppearance.BorderColor = Color.Cyan;
            btnSetSvFrom.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSetSvFrom.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSetSvFrom.FlatStyle = FlatStyle.Flat;
            btnSetSvFrom.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Bold);
            btnSetSvFrom.ForeColor = SystemColors.Control;
            btnSetSvFrom.ImageAlign = ContentAlignment.TopCenter;
            btnSetSvFrom.Location = new Point(88, 84);
            btnSetSvFrom.Name = "btnSetSvFrom";
            btnSetSvFrom.Size = new Size(120, 24);
            btnSetSvFrom.TabIndex = 17;
            btnSetSvFrom.TabStop = false;
            btnSetSvFrom.Text = "Set Start SV";
            btnSetSvFrom.UseVisualStyleBackColor = true;
            btnSetSvFrom.Click += btnSetSvFrom_Click;
            // 
            // btnSetVolumeTo
            // 
            btnSetVolumeTo.FlatAppearance.BorderColor = Color.Cyan;
            btnSetVolumeTo.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSetVolumeTo.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSetVolumeTo.FlatStyle = FlatStyle.Flat;
            btnSetVolumeTo.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Bold);
            btnSetVolumeTo.ForeColor = SystemColors.Control;
            btnSetVolumeTo.ImageAlign = ContentAlignment.TopCenter;
            btnSetVolumeTo.Location = new Point(271, 135);
            btnSetVolumeTo.Name = "btnSetVolumeTo";
            btnSetVolumeTo.Size = new Size(120, 24);
            btnSetVolumeTo.TabIndex = 18;
            btnSetVolumeTo.TabStop = false;
            btnSetVolumeTo.Text = "Set End Volume";
            btnSetVolumeTo.UseVisualStyleBackColor = true;
            btnSetVolumeTo.Click += btnSetVolumeTo_Click;
            // 
            // btnSetVolumeFrom
            // 
            btnSetVolumeFrom.FlatAppearance.BorderColor = Color.Cyan;
            btnSetVolumeFrom.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSetVolumeFrom.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSetVolumeFrom.FlatStyle = FlatStyle.Flat;
            btnSetVolumeFrom.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Bold);
            btnSetVolumeFrom.ForeColor = SystemColors.Control;
            btnSetVolumeFrom.ImageAlign = ContentAlignment.TopCenter;
            btnSetVolumeFrom.Location = new Point(88, 135);
            btnSetVolumeFrom.Name = "btnSetVolumeFrom";
            btnSetVolumeFrom.Size = new Size(120, 24);
            btnSetVolumeFrom.TabIndex = 19;
            btnSetVolumeFrom.TabStop = false;
            btnSetVolumeFrom.Text = "Set Start Volume";
            btnSetVolumeFrom.UseVisualStyleBackColor = true;
            btnSetVolumeFrom.Click += btnSetVolumeFrom_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.BackColor = Color.FromArgb(0, 64, 64);
            menuStrip1.Items.AddRange(new ToolStripItem[] { sVEditorToolStripMenuItem, utilityToolStripMenuItem, timingPropertyToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(408, 24);
            menuStrip1.TabIndex = 20;
            menuStrip1.Text = "menuStrip1";
            // 
            // sVEditorToolStripMenuItem
            // 
            sVEditorToolStripMenuItem.BackColor = Color.FromArgb(0, 96, 96);
            sVEditorToolStripMenuItem.ForeColor = Color.White;
            sVEditorToolStripMenuItem.Name = "sVEditorToolStripMenuItem";
            sVEditorToolStripMenuItem.Size = new Size(66, 20);
            sVEditorToolStripMenuItem.Text = "SV Editor";
            sVEditorToolStripMenuItem.Click += sVEditorToolStripMenuItem_Click;
            // 
            // utilityToolStripMenuItem
            // 
            utilityToolStripMenuItem.BackColor = Color.FromArgb(0, 96, 96);
            utilityToolStripMenuItem.ForeColor = Color.White;
            utilityToolStripMenuItem.Name = "utilityToolStripMenuItem";
            utilityToolStripMenuItem.Size = new Size(50, 20);
            utilityToolStripMenuItem.Text = "Utility";
            utilityToolStripMenuItem.Click += utilityToolStripMenuItem_Click;
            // 
            // timingPropertyToolStripMenuItem
            // 
            timingPropertyToolStripMenuItem.BackColor = Color.FromArgb(0, 96, 96);
            timingPropertyToolStripMenuItem.ForeColor = Color.White;
            timingPropertyToolStripMenuItem.Name = "timingPropertyToolStripMenuItem";
            timingPropertyToolStripMenuItem.Size = new Size(103, 20);
            timingPropertyToolStripMenuItem.Text = "Timing Property";
            timingPropertyToolStripMenuItem.Click += timingPropertyToolStripMenuItem_Click;
            // 
            // pnlGroupSvEditor
            // 
            pnlGroupSvEditor.Controls.Add(btnSetVolumeTo);
            pnlGroupSvEditor.Controls.Add(btnSetVolumeFrom);
            pnlGroupSvEditor.Controls.Add(btnSetSvTo);
            pnlGroupSvEditor.Controls.Add(btnSetSvFrom);
            pnlGroupSvEditor.Controls.Add(chkEnableSvTo);
            pnlGroupSvEditor.Controls.Add(chkApplyEndObject);
            pnlGroupSvEditor.Controls.Add(chkRelative);
            pnlGroupSvEditor.Controls.Add(chkApplyStartObject);
            pnlGroupSvEditor.Controls.Add(pnlRelativeSvGroup);
            pnlGroupSvEditor.Controls.Add(btnSetTimingTo);
            pnlGroupSvEditor.Controls.Add(btnSetTimingFrom);
            pnlGroupSvEditor.Controls.Add(lblCalculationType);
            pnlGroupSvEditor.Controls.Add(tabExecuteType);
            pnlGroupSvEditor.Controls.Add(btnSwapVolume);
            pnlGroupSvEditor.Controls.Add(btnSwapSv);
            pnlGroupSvEditor.Controls.Add(btnSwapTiming);
            pnlGroupSvEditor.Controls.Add(pnlCalcurationTypeGroup);
            pnlGroupSvEditor.Controls.Add(chkEnableVolume);
            pnlGroupSvEditor.Controls.Add(chkEnableSv);
            pnlGroupSvEditor.Controls.Add(lblTiming);
            pnlGroupSvEditor.Controls.Add(txtVolumeTo);
            pnlGroupSvEditor.Controls.Add(txtVolumeFrom);
            pnlGroupSvEditor.Controls.Add(txtSvTo);
            pnlGroupSvEditor.Controls.Add(txtSvFrom);
            pnlGroupSvEditor.Controls.Add(txtTimingFrom);
            pnlGroupSvEditor.Controls.Add(txtTimingTo);
            pnlGroupSvEditor.Location = new Point(5, 253);
            pnlGroupSvEditor.Name = "pnlGroupSvEditor";
            pnlGroupSvEditor.Size = new Size(400, 649);
            pnlGroupSvEditor.TabIndex = 21;
            pnlGroupSvEditor.Visible = false;
            // 
            // pnlGroupUtility
            // 
            pnlGroupUtility.Controls.Add(label8);
            pnlGroupUtility.Controls.Add(txtTimingOffset);
            pnlGroupUtility.Controls.Add(btnApplyOffsetShifter);
            pnlGroupUtility.Controls.Add(lblOffsetShifter);
            pnlGroupUtility.Controls.Add(btnApplyResnap);
            pnlGroupUtility.Controls.Add(cmbResnapBeatSnap);
            pnlGroupUtility.Controls.Add(lblBeatSnapDivisor);
            pnlGroupUtility.Controls.Add(lblResnap);
            pnlGroupUtility.Controls.Add(btnApplySettingCopier);
            pnlGroupUtility.Controls.Add(lblSettingCopier);
            pnlGroupUtility.Controls.Add(cmbSettingCopier);
            pnlGroupUtility.Controls.Add(txtTags);
            pnlGroupUtility.Controls.Add(btnApplyTagEditor);
            pnlGroupUtility.Controls.Add(lblTagEditor);
            pnlGroupUtility.Controls.Add(btnApplyNotesPosition);
            pnlGroupUtility.Controls.Add(lblNotesPosition);
            pnlGroupUtility.Controls.Add(cmbNotesPosition);
            pnlGroupUtility.Controls.Add(btnApplyHitsoundOption);
            pnlGroupUtility.Controls.Add(lblHitsoundOption);
            pnlGroupUtility.Controls.Add(cmbHitsoundTool);
            pnlGroupUtility.Controls.Add(btnSetResnapTimingTo);
            pnlGroupUtility.Controls.Add(btnSetResnapTimingFrom);
            pnlGroupUtility.Controls.Add(btnSwapResnapTiming);
            pnlGroupUtility.Controls.Add(lblResnapTiming);
            pnlGroupUtility.Controls.Add(txtResnapTimingFrom);
            pnlGroupUtility.Controls.Add(txtResnapTimingTo);
            pnlGroupUtility.Location = new Point(5, 253);
            pnlGroupUtility.Name = "pnlGroupUtility";
            pnlGroupUtility.Size = new Size(400, 649);
            pnlGroupUtility.TabIndex = 22;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            label8.ForeColor = Color.White;
            label8.Location = new Point(252, 44);
            label8.Name = "label8";
            label8.Size = new Size(28, 20);
            label8.TabIndex = 42;
            label8.Text = "ms";
            // 
            // txtTimingOffset
            // 
            txtTimingOffset.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            txtTimingOffset.Location = new Point(13, 41);
            txtTimingOffset.Name = "txtTimingOffset";
            txtTimingOffset.RightToLeft = RightToLeft.No;
            txtTimingOffset.Size = new Size(233, 29);
            txtTimingOffset.TabIndex = 41;
            txtTimingOffset.TextAlign = HorizontalAlignment.Right;
            // 
            // btnApplyOffsetShifter
            // 
            btnApplyOffsetShifter.BackColor = Color.DarkCyan;
            btnApplyOffsetShifter.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyOffsetShifter.FlatAppearance.BorderSize = 2;
            btnApplyOffsetShifter.FlatStyle = FlatStyle.Flat;
            btnApplyOffsetShifter.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyOffsetShifter.ForeColor = SystemColors.ControlLightLight;
            btnApplyOffsetShifter.Location = new Point(306, 36);
            btnApplyOffsetShifter.Name = "btnApplyOffsetShifter";
            btnApplyOffsetShifter.Size = new Size(83, 38);
            btnApplyOffsetShifter.TabIndex = 40;
            btnApplyOffsetShifter.TabStop = false;
            btnApplyOffsetShifter.Text = "Apply to All";
            btnApplyOffsetShifter.UseVisualStyleBackColor = false;
            btnApplyOffsetShifter.Click += btnApplyOffset_Click;
            // 
            // lblOffsetShifter
            // 
            lblOffsetShifter.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblOffsetShifter.ForeColor = Color.White;
            lblOffsetShifter.Location = new Point(0, 10);
            lblOffsetShifter.Name = "lblOffsetShifter";
            lblOffsetShifter.Size = new Size(400, 20);
            lblOffsetShifter.TabIndex = 39;
            lblOffsetShifter.Text = "Offset";
            lblOffsetShifter.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnApplyResnap
            // 
            btnApplyResnap.BackColor = Color.DarkCyan;
            btnApplyResnap.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyResnap.FlatAppearance.BorderSize = 2;
            btnApplyResnap.FlatStyle = FlatStyle.Flat;
            btnApplyResnap.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyResnap.ForeColor = SystemColors.ControlLightLight;
            btnApplyResnap.Location = new Point(306, 595);
            btnApplyResnap.Name = "btnApplyResnap";
            btnApplyResnap.Size = new Size(83, 38);
            btnApplyResnap.TabIndex = 37;
            btnApplyResnap.TabStop = false;
            btnApplyResnap.Text = "Apply";
            btnApplyResnap.UseVisualStyleBackColor = false;
            btnApplyResnap.Click += btnApplyResnap_Click;
            // 
            // cmbResnapBeatSnap
            // 
            cmbResnapBeatSnap.DropDownHeight = 120;
            cmbResnapBeatSnap.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbResnapBeatSnap.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            cmbResnapBeatSnap.FormattingEnabled = true;
            cmbResnapBeatSnap.IntegralHeight = false;
            cmbResnapBeatSnap.Items.AddRange(new object[] { "1/24 (1/8, 1/12)", "1/36 (1/4, 1/6, 1/9)", "1/48 (1/12, 1/16)", "1/60 (1/4, 1/5, 1/12)" });
            cmbResnapBeatSnap.Location = new Point(148, 599);
            cmbResnapBeatSnap.Name = "cmbResnapBeatSnap";
            cmbResnapBeatSnap.Size = new Size(151, 29);
            cmbResnapBeatSnap.TabIndex = 36;
            cmbResnapBeatSnap.SelectedIndexChanged += cmbResnapBeatSnap_SelectedIndexChanged;
            // 
            // lblBeatSnapDivisor
            // 
            lblBeatSnapDivisor.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblBeatSnapDivisor.ForeColor = Color.White;
            lblBeatSnapDivisor.Location = new Point(17, 602);
            lblBeatSnapDivisor.Name = "lblBeatSnapDivisor";
            lblBeatSnapDivisor.Size = new Size(139, 20);
            lblBeatSnapDivisor.TabIndex = 35;
            lblBeatSnapDivisor.Text = "Beat Snap Divisor";
            // 
            // lblResnap
            // 
            lblResnap.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblResnap.ForeColor = Color.White;
            lblResnap.Location = new Point(0, 506);
            lblResnap.Name = "lblResnap";
            lblResnap.Size = new Size(400, 20);
            lblResnap.TabIndex = 34;
            lblResnap.Text = "Resnap";
            lblResnap.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnApplySettingCopier
            // 
            btnApplySettingCopier.BackColor = Color.DarkCyan;
            btnApplySettingCopier.FlatAppearance.BorderColor = Color.Cyan;
            btnApplySettingCopier.FlatAppearance.BorderSize = 2;
            btnApplySettingCopier.FlatStyle = FlatStyle.Flat;
            btnApplySettingCopier.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplySettingCopier.ForeColor = SystemColors.ControlLightLight;
            btnApplySettingCopier.Location = new Point(306, 459);
            btnApplySettingCopier.Name = "btnApplySettingCopier";
            btnApplySettingCopier.Size = new Size(83, 38);
            btnApplySettingCopier.TabIndex = 33;
            btnApplySettingCopier.TabStop = false;
            btnApplySettingCopier.Text = "Apply to All";
            btnApplySettingCopier.UseVisualStyleBackColor = false;
            btnApplySettingCopier.Click += btnApplySettingCopier_Click;
            // 
            // lblSettingCopier
            // 
            lblSettingCopier.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblSettingCopier.ForeColor = Color.White;
            lblSettingCopier.Location = new Point(0, 433);
            lblSettingCopier.Name = "lblSettingCopier";
            lblSettingCopier.Size = new Size(400, 20);
            lblSettingCopier.TabIndex = 32;
            lblSettingCopier.Text = "Setting Copier";
            lblSettingCopier.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbSettingCopier
            // 
            cmbSettingCopier.DropDownHeight = 120;
            cmbSettingCopier.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbSettingCopier.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            cmbSettingCopier.FormattingEnabled = true;
            cmbSettingCopier.IntegralHeight = false;
            cmbSettingCopier.Items.AddRange(new object[] { "Metadata", "BG Position", "Preview", "Timing Points" });
            cmbSettingCopier.Location = new Point(12, 464);
            cmbSettingCopier.Name = "cmbSettingCopier";
            cmbSettingCopier.Size = new Size(287, 29);
            cmbSettingCopier.TabIndex = 31;
            cmbSettingCopier.SelectedIndexChanged += cmbMetadataSetting_SelectedIndexChanged;
            // 
            // txtTags
            // 
            txtTags.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            txtTags.Location = new Point(12, 260);
            txtTags.Multiline = true;
            txtTags.Name = "txtTags";
            txtTags.ScrollBars = ScrollBars.Vertical;
            txtTags.Size = new Size(377, 114);
            txtTags.TabIndex = 30;
            txtTags.TextChanged += txtTags_TextChanged;
            txtTags.KeyPress += txtTags_KeyPress;
            // 
            // btnApplyTagEditor
            // 
            btnApplyTagEditor.BackColor = Color.DarkCyan;
            btnApplyTagEditor.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyTagEditor.FlatAppearance.BorderSize = 2;
            btnApplyTagEditor.FlatStyle = FlatStyle.Flat;
            btnApplyTagEditor.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyTagEditor.ForeColor = SystemColors.ControlLightLight;
            btnApplyTagEditor.Location = new Point(306, 386);
            btnApplyTagEditor.Name = "btnApplyTagEditor";
            btnApplyTagEditor.Size = new Size(83, 38);
            btnApplyTagEditor.TabIndex = 29;
            btnApplyTagEditor.TabStop = false;
            btnApplyTagEditor.Text = "Apply to All";
            btnApplyTagEditor.UseVisualStyleBackColor = false;
            btnApplyTagEditor.Click += btnApplyTags_Click;
            // 
            // lblTagEditor
            // 
            lblTagEditor.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblTagEditor.ForeColor = Color.White;
            lblTagEditor.Location = new Point(0, 229);
            lblTagEditor.Name = "lblTagEditor";
            lblTagEditor.Size = new Size(400, 20);
            lblTagEditor.TabIndex = 28;
            lblTagEditor.Text = "Tag Edit";
            lblTagEditor.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // btnApplyNotesPosition
            // 
            btnApplyNotesPosition.BackColor = Color.DarkCyan;
            btnApplyNotesPosition.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyNotesPosition.FlatAppearance.BorderSize = 2;
            btnApplyNotesPosition.FlatStyle = FlatStyle.Flat;
            btnApplyNotesPosition.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyNotesPosition.ForeColor = SystemColors.ControlLightLight;
            btnApplyNotesPosition.Location = new Point(306, 182);
            btnApplyNotesPosition.Name = "btnApplyNotesPosition";
            btnApplyNotesPosition.Size = new Size(83, 38);
            btnApplyNotesPosition.TabIndex = 26;
            btnApplyNotesPosition.TabStop = false;
            btnApplyNotesPosition.Text = "Apply";
            btnApplyNotesPosition.UseVisualStyleBackColor = false;
            btnApplyNotesPosition.Click += btnApplyNotesPosition_Click;
            // 
            // lblNotesPosition
            // 
            lblNotesPosition.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblNotesPosition.ForeColor = Color.White;
            lblNotesPosition.Location = new Point(0, 156);
            lblNotesPosition.Name = "lblNotesPosition";
            lblNotesPosition.Size = new Size(400, 20);
            lblNotesPosition.TabIndex = 25;
            lblNotesPosition.Text = "Notes Position";
            lblNotesPosition.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbNotesPosition
            // 
            cmbNotesPosition.DropDownHeight = 120;
            cmbNotesPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNotesPosition.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            cmbNotesPosition.FormattingEnabled = true;
            cmbNotesPosition.IntegralHeight = false;
            cmbNotesPosition.Items.AddRange(new object[] { "Center", "Sepatate", "Random" });
            cmbNotesPosition.Location = new Point(13, 187);
            cmbNotesPosition.Name = "cmbNotesPosition";
            cmbNotesPosition.Size = new Size(287, 29);
            cmbNotesPosition.TabIndex = 24;
            cmbNotesPosition.SelectedIndexChanged += cmbNotesPosition_SelectedIndexChanged;
            // 
            // btnApplyHitsoundOption
            // 
            btnApplyHitsoundOption.BackColor = Color.DarkCyan;
            btnApplyHitsoundOption.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyHitsoundOption.FlatAppearance.BorderSize = 2;
            btnApplyHitsoundOption.FlatStyle = FlatStyle.Flat;
            btnApplyHitsoundOption.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyHitsoundOption.ForeColor = SystemColors.ControlLightLight;
            btnApplyHitsoundOption.Location = new Point(306, 109);
            btnApplyHitsoundOption.Name = "btnApplyHitsoundOption";
            btnApplyHitsoundOption.Size = new Size(83, 38);
            btnApplyHitsoundOption.TabIndex = 23;
            btnApplyHitsoundOption.TabStop = false;
            btnApplyHitsoundOption.Text = "Apply";
            btnApplyHitsoundOption.UseVisualStyleBackColor = false;
            btnApplyHitsoundOption.Click += btnApplyHitsound_Click;
            // 
            // lblHitsoundOption
            // 
            lblHitsoundOption.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblHitsoundOption.ForeColor = Color.White;
            lblHitsoundOption.Location = new Point(0, 83);
            lblHitsoundOption.Name = "lblHitsoundOption";
            lblHitsoundOption.Size = new Size(400, 20);
            lblHitsoundOption.TabIndex = 20;
            lblHitsoundOption.Text = "Hitsound Tool";
            lblHitsoundOption.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cmbHitsoundTool
            // 
            cmbHitsoundTool.DropDownHeight = 120;
            cmbHitsoundTool.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHitsoundTool.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            cmbHitsoundTool.FormattingEnabled = true;
            cmbHitsoundTool.IntegralHeight = false;
            cmbHitsoundTool.Items.AddRange(new object[] { "Clap", "Whistle" });
            cmbHitsoundTool.Location = new Point(13, 114);
            cmbHitsoundTool.Name = "cmbHitsoundTool";
            cmbHitsoundTool.Size = new Size(287, 29);
            cmbHitsoundTool.TabIndex = 19;
            cmbHitsoundTool.SelectedIndexChanged += cmbHitsoundTool_SelectedIndexChanged;
            // 
            // btnSetResnapTimingTo
            // 
            btnSetResnapTimingTo.FlatAppearance.BorderColor = Color.Cyan;
            btnSetResnapTimingTo.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSetResnapTimingTo.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSetResnapTimingTo.FlatStyle = FlatStyle.Flat;
            btnSetResnapTimingTo.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Bold);
            btnSetResnapTimingTo.ForeColor = SystemColors.Control;
            btnSetResnapTimingTo.ImageAlign = ContentAlignment.TopCenter;
            btnSetResnapTimingTo.Location = new Point(270, 559);
            btnSetResnapTimingTo.Name = "btnSetResnapTimingTo";
            btnSetResnapTimingTo.Size = new Size(120, 24);
            btnSetResnapTimingTo.TabIndex = 13;
            btnSetResnapTimingTo.TabStop = false;
            btnSetResnapTimingTo.Text = "Set End Timing";
            btnSetResnapTimingTo.UseVisualStyleBackColor = true;
            btnSetResnapTimingTo.Click += btnSetResnapTimingTo_Click;
            // 
            // btnSetResnapTimingFrom
            // 
            btnSetResnapTimingFrom.FlatAppearance.BorderColor = Color.Cyan;
            btnSetResnapTimingFrom.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSetResnapTimingFrom.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSetResnapTimingFrom.FlatStyle = FlatStyle.Flat;
            btnSetResnapTimingFrom.Font = new Font("Yu Gothic UI", 8.25F, FontStyle.Bold);
            btnSetResnapTimingFrom.ForeColor = SystemColors.Control;
            btnSetResnapTimingFrom.ImageAlign = ContentAlignment.TopCenter;
            btnSetResnapTimingFrom.Location = new Point(87, 559);
            btnSetResnapTimingFrom.Name = "btnSetResnapTimingFrom";
            btnSetResnapTimingFrom.Size = new Size(120, 24);
            btnSetResnapTimingFrom.TabIndex = 14;
            btnSetResnapTimingFrom.TabStop = false;
            btnSetResnapTimingFrom.Text = "Set Start Timing";
            btnSetResnapTimingFrom.UseVisualStyleBackColor = true;
            btnSetResnapTimingFrom.Click += btnSetResnapTimingFrom_Click;
            // 
            // btnSwapResnapTiming
            // 
            btnSwapResnapTiming.FlatAppearance.BorderColor = Color.Cyan;
            btnSwapResnapTiming.FlatAppearance.MouseDownBackColor = Color.Transparent;
            btnSwapResnapTiming.FlatAppearance.MouseOverBackColor = Color.Transparent;
            btnSwapResnapTiming.FlatStyle = FlatStyle.Flat;
            btnSwapResnapTiming.ForeColor = Color.Cyan;
            btnSwapResnapTiming.Location = new Point(206, 537);
            btnSwapResnapTiming.Name = "btnSwapResnapTiming";
            btnSwapResnapTiming.Size = new Size(65, 23);
            btnSwapResnapTiming.TabIndex = 17;
            btnSwapResnapTiming.TabStop = false;
            btnSwapResnapTiming.Text = "⇔";
            btnSwapResnapTiming.UseVisualStyleBackColor = true;
            btnSwapResnapTiming.Click += btnSwapResnapTiming_Click;
            // 
            // lblResnapTiming
            // 
            lblResnapTiming.AutoSize = true;
            lblResnapTiming.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            lblResnapTiming.ForeColor = Color.White;
            lblResnapTiming.Location = new Point(17, 537);
            lblResnapTiming.Name = "lblResnapTiming";
            lblResnapTiming.Size = new Size(56, 20);
            lblResnapTiming.TabIndex = 18;
            lblResnapTiming.Text = "Timing";
            // 
            // txtResnapTimingFrom
            // 
            txtResnapTimingFrom.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtResnapTimingFrom.BorderStyle = BorderStyle.FixedSingle;
            txtResnapTimingFrom.ForeColor = SystemColors.WindowText;
            txtResnapTimingFrom.Location = new Point(87, 537);
            txtResnapTimingFrom.Name = "txtResnapTimingFrom";
            txtResnapTimingFrom.Size = new Size(120, 23);
            txtResnapTimingFrom.TabIndex = 15;
            txtResnapTimingFrom.TextAlign = HorizontalAlignment.Center;
            txtResnapTimingFrom.TextChanged += txtResnapTimingFrom_TextChanged;
            // 
            // txtResnapTimingTo
            // 
            txtResnapTimingTo.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            txtResnapTimingTo.BorderStyle = BorderStyle.FixedSingle;
            txtResnapTimingTo.Location = new Point(270, 537);
            txtResnapTimingTo.Name = "txtResnapTimingTo";
            txtResnapTimingTo.Size = new Size(120, 23);
            txtResnapTimingTo.TabIndex = 16;
            txtResnapTimingTo.TextAlign = HorizontalAlignment.Center;
            txtResnapTimingTo.TextChanged += txtResnapTimingTo_TextChanged;
            // 
            // btnBackup
            // 
            btnBackup.BackColor = Color.DarkCyan;
            btnBackup.FlatAppearance.BorderColor = Color.Cyan;
            btnBackup.FlatAppearance.BorderSize = 2;
            btnBackup.FlatStyle = FlatStyle.Flat;
            btnBackup.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnBackup.ForeColor = SystemColors.ControlLightLight;
            btnBackup.Location = new Point(18, 909);
            btnBackup.Name = "btnBackup";
            btnBackup.Size = new Size(181, 39);
            btnBackup.TabIndex = 23;
            btnBackup.TabStop = false;
            btnBackup.Text = "バックアップ";
            btnBackup.UseVisualStyleBackColor = false;
            btnBackup.Click += btnBackup_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 64, 64);
            ClientSize = new Size(408, 963);
            Controls.Add(btnBackup);
            Controls.Add(pnlGroupSvEditor);
            Controls.Add(btnViewSetting);
            Controls.Add(pnlBeatmapInfoGroup);
            Controls.Add(menuStrip1);
            Controls.Add(pnlGroupUtility);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            MainMenuStrip = menuStrip1;
            Name = "MainForm";
            Text = "osu_taiko_Mapping_Helper";
            Load += osu_taiko_Mapping_Helper_Load;
            Shown += osu_taiko_Mapping_Helper_Shown;
            KeyDown += osu_taiko_Mapping_Helper_KeyDown;
            pnlBeatmapInfoGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picDisplayBg).EndInit();
            pnlCalcurationTypeGroup.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picSpecificNormalDong).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificNormalKa).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificFinisherDong).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificFinisherKa).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificNormalSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificFinisherSlider).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSpecificNormalSpinner).EndInit();
            tabExecuteType.ResumeLayout(false);
            tabApplyPage.ResumeLayout(false);
            tabApplyPage.PerformLayout();
            tabSetType.ResumeLayout(false);
            tabHitObjectsPage.ResumeLayout(false);
            tabHitObjectsPage.PerformLayout();
            pnlSpecificGroup.ResumeLayout(false);
            pnlSpecificGroup.PerformLayout();
            tabBeatSnap.ResumeLayout(false);
            tabBeatSnap.PerformLayout();
            tabRemovePage.ResumeLayout(false);
            tabRemovePage.PerformLayout();
            pnlRelativeSvGroup.ResumeLayout(false);
            pnlRelativeSvGroup.PerformLayout();
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            pnlGroupSvEditor.ResumeLayout(false);
            pnlGroupSvEditor.PerformLayout();
            pnlGroupUtility.ResumeLayout(false);
            pnlGroupUtility.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Panel pnlBeatmapInfoGroup;
        private Label lblFileName;
        private TextBox txtTimingTo;
        private TextBox txtTimingFrom;
        private TextBox txtSvFrom;
        private TextBox txtSvTo;
        private TextBox txtVolumeFrom;
        private TextBox txtVolumeTo;
        private Label lblTiming;
        private CheckBox chkEnableSv;
        private RadioButton rdoArithmetic;
        private RadioButton rdoGeometric;
        private CheckBox chkEnableVolume;
        private Panel pnlCalcurationTypeGroup;
        private Button btnApply;
        private CheckBox chkEnableOffset;
        private TextBox txtOffset;
        private Label lblMiliSecond;
        private Button btnSwapTiming;
        private Button btnSwapSv;
        private Button btnSwapVolume;
        private Button btnRemove;
        private TextBox txtBeatSnap;
        private Label lblBeatSnap;
        private PictureBox picSpecificNormalDong;
        private PictureBox picSpecificNormalKa;
        private PictureBox picSpecificFinisherDong;
        private PictureBox picSpecificFinisherKa;
        private Label lblSpecificGridLine;
        private Label lblSpecificFinisher;
        private Label lblSpecificNormal;
        private PictureBox picDisplayBg;
        private PictureBox picSpecificNormalSlider;
        private PictureBox picSpecificFinisherSlider;
        private Label lblSpecificGridLine2;
        private PictureBox picSpecificNormalSpinner;
        private TabControl tabExecuteType;
        private TabPage tabApplyPage;
        private TabPage tabRemovePage;
        private TabControl tabSetType;
        private TabPage tabHitObjectsPage;
        private TabPage tabBeatSnap;
        private Label lblCalculationType;
        private Button btnSetTimingFrom;
        private Button btnSetTimingTo;
        private CheckBox chkEnableStartOffset;
        private TextBox txtStartOffset;
        private Label lbllblMiliSecondRemove;
        private CheckBox chkApplyStartObject;
        private CheckBox chkApplyEndObject;
        private RadioButton rdoOnlyOffNotes;
        private RadioButton rdoOnlyOnNotes;
        private Panel pnlSpecificGroup;
        private RadioButton rdoAllHitObjects;
        private RadioButton rdoOnlyBarline;
        private RadioButton rdoOnlyBookMark;
        private RadioButton rdoOnlySpecificHitObject;
        private Panel pnlRelativeSvGroup;
        private RadioButton rdoRelativeMultiply;
        private RadioButton rdoRelativeSum;
        private CheckBox chkRelative;
        private Label lblRelativeBaseSv;
        private TextBox txtRelativeBaseSv;
        private Button btnViewSetting;
        private CheckBox chkEnableSvTo;
        private CheckBox chkEnableKiai;
        private Button btnSetSvTo;
        private Button btnSetSvFrom;
        private Button btnSetVolumeTo;
        private Button btnSetVolumeFrom;
        private TabPage tabGreenLine;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem sVEditorToolStripMenuItem;
        private ToolStripMenuItem utilityToolStripMenuItem;
        private Panel pnlGroupSvEditor;
        private Panel pnlGroupUtility;
        private ComboBox cmbHitsoundTool;
        private Button btnSetResnapTimingTo;
        private Button btnSetResnapTimingFrom;
        private Button btnSwapResnapTiming;
        private Label lblResnapTiming;
        private TextBox txtResnapTimingFrom;
        private TextBox txtResnapTimingTo;
        private Button btnApplyNotesPosition;
        private Label lblNotesPosition;
        private ComboBox cmbNotesPosition;
        private Button btnApplyHitsoundOption;
        private Label lblHitsoundOption;
        private TextBox txtTags;
        private Button btnApplyTagEditor;
        private Label lblTagEditor;
        private Button btnApplySettingCopier;
        private Label lblSettingCopier;
        private ComboBox cmbSettingCopier;
        private Label lblResnap;
        private Button btnApplyResnap;
        private ComboBox cmbResnapBeatSnap;
        private Label lblBeatSnapDivisor;
        private Button btnBackup;
        private ToolStripMenuItem timingPropertyToolStripMenuItem;
        private Button btnApplyOffsetShifter;
        private Label lblOffsetShifter;
        private Label label8;
        private TextBox txtTimingOffset;
        private Label lblBeatSnaps;
    }
}