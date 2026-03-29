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
            chkEnableBeatSnap = new CheckBox();
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
            btnApplyResnap = new Button();
            cmbResnapBeatSnap = new ComboBox();
            label1 = new Label();
            label6 = new Label();
            btnApplyMetadata = new Button();
            label5 = new Label();
            cmbMetadataSetting = new ComboBox();
            txtTags = new TextBox();
            btnApplyTags = new Button();
            label4 = new Label();
            btnApplyNotesPosition = new Button();
            label3 = new Label();
            cmbNotesPosition = new ComboBox();
            btnApplyHitsound = new Button();
            label2 = new Label();
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
            pnlCalcurationTypeGroup.Size = new Size(128, 66);
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
            btnRemove.Location = new Point(96, 235);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(189, 39);
            btnRemove.TabIndex = 10;
            btnRemove.TabStop = false;
            btnRemove.Text = "実行";
            btnRemove.UseVisualStyleBackColor = false;
            btnRemove.Click += btnRemove_Click;
            // 
            // chkEnableBeatSnap
            // 
            chkEnableBeatSnap.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            chkEnableBeatSnap.ForeColor = Color.White;
            chkEnableBeatSnap.Location = new Point(6, 6);
            chkEnableBeatSnap.Name = "chkEnableBeatSnap";
            chkEnableBeatSnap.Size = new Size(124, 37);
            chkEnableBeatSnap.TabIndex = 0;
            chkEnableBeatSnap.TabStop = false;
            chkEnableBeatSnap.Text = "ビートスナップ間隔でSVを置く";
            chkEnableBeatSnap.UseVisualStyleBackColor = true;
            chkEnableBeatSnap.CheckedChanged += chkEnableBeatSnap_CheckedChanged;
            // 
            // txtBeatSnap
            // 
            txtBeatSnap.BackColor = SystemColors.WindowFrame;
            txtBeatSnap.BorderStyle = BorderStyle.FixedSingle;
            txtBeatSnap.Enabled = false;
            txtBeatSnap.Location = new Point(288, 14);
            txtBeatSnap.Name = "txtBeatSnap";
            txtBeatSnap.Size = new Size(40, 25);
            txtBeatSnap.TabIndex = 2;
            txtBeatSnap.TabStop = false;
            // 
            // lblBeatSnap
            // 
            lblBeatSnap.Font = new Font("Yu Gothic UI", 9F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblBeatSnap.ForeColor = Color.White;
            lblBeatSnap.Location = new Point(271, 17);
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
            tabExecuteType.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            tabExecuteType.ItemSize = new Size(195, 40);
            tabExecuteType.Location = new Point(3, 303);
            tabExecuteType.Name = "tabExecuteType";
            tabExecuteType.SelectedIndex = 0;
            tabExecuteType.Size = new Size(393, 337);
            tabExecuteType.SizeMode = TabSizeMode.Fixed;
            tabExecuteType.TabIndex = 3;
            tabExecuteType.TabStop = false;
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
            tabBeatSnap.Controls.Add(chkEnableBeatSnap);
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
            tabBeatSnap.Text = "BeatSnap間隔";
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
            pnlRelativeSvGroup.Location = new Point(143, 212);
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
            chkRelative.Location = new Point(146, 188);
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
            chkEnableSvTo.Location = new Point(289, 216);
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
            pnlGroupUtility.Controls.Add(btnApplyResnap);
            pnlGroupUtility.Controls.Add(cmbResnapBeatSnap);
            pnlGroupUtility.Controls.Add(label1);
            pnlGroupUtility.Controls.Add(label6);
            pnlGroupUtility.Controls.Add(btnApplyMetadata);
            pnlGroupUtility.Controls.Add(label5);
            pnlGroupUtility.Controls.Add(cmbMetadataSetting);
            pnlGroupUtility.Controls.Add(txtTags);
            pnlGroupUtility.Controls.Add(btnApplyTags);
            pnlGroupUtility.Controls.Add(label4);
            pnlGroupUtility.Controls.Add(btnApplyNotesPosition);
            pnlGroupUtility.Controls.Add(label3);
            pnlGroupUtility.Controls.Add(cmbNotesPosition);
            pnlGroupUtility.Controls.Add(btnApplyHitsound);
            pnlGroupUtility.Controls.Add(label2);
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
            // btnApplyResnap
            // 
            btnApplyResnap.BackColor = Color.DarkCyan;
            btnApplyResnap.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyResnap.FlatAppearance.BorderSize = 2;
            btnApplyResnap.FlatStyle = FlatStyle.Flat;
            btnApplyResnap.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyResnap.ForeColor = SystemColors.ControlLightLight;
            btnApplyResnap.Location = new Point(291, 575);
            btnApplyResnap.Name = "btnApplyResnap";
            btnApplyResnap.Size = new Size(99, 38);
            btnApplyResnap.TabIndex = 37;
            btnApplyResnap.TabStop = false;
            btnApplyResnap.Text = "Run";
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
            cmbResnapBeatSnap.Location = new Point(102, 579);
            cmbResnapBeatSnap.Name = "cmbResnapBeatSnap";
            cmbResnapBeatSnap.Size = new Size(179, 29);
            cmbResnapBeatSnap.TabIndex = 36;
            cmbResnapBeatSnap.SelectedIndexChanged += cmbResnapBeatSnap_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(17, 582);
            label1.Name = "label1";
            label1.Size = new Size(73, 20);
            label1.TabIndex = 35;
            label1.Text = "BeatSnap";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            label6.ForeColor = Color.White;
            label6.Location = new Point(171, 486);
            label6.Name = "label6";
            label6.Size = new Size(58, 20);
            label6.TabIndex = 34;
            label6.Text = "Resnap";
            // 
            // btnApplyMetadata
            // 
            btnApplyMetadata.BackColor = Color.DarkCyan;
            btnApplyMetadata.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyMetadata.FlatAppearance.BorderSize = 2;
            btnApplyMetadata.FlatStyle = FlatStyle.Flat;
            btnApplyMetadata.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyMetadata.ForeColor = SystemColors.ControlLightLight;
            btnApplyMetadata.Location = new Point(277, 439);
            btnApplyMetadata.Name = "btnApplyMetadata";
            btnApplyMetadata.Size = new Size(111, 38);
            btnApplyMetadata.TabIndex = 33;
            btnApplyMetadata.TabStop = false;
            btnApplyMetadata.Text = "Apply to All";
            btnApplyMetadata.UseVisualStyleBackColor = false;
            btnApplyMetadata.Click += btnApplyMetadata_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            label5.ForeColor = Color.White;
            label5.Location = new Point(137, 413);
            label5.Name = "label5";
            label5.Size = new Size(126, 20);
            label5.TabIndex = 32;
            label5.Text = "Metadata Setting";
            // 
            // cmbMetadataSetting
            // 
            cmbMetadataSetting.DropDownHeight = 120;
            cmbMetadataSetting.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbMetadataSetting.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            cmbMetadataSetting.FormattingEnabled = true;
            cmbMetadataSetting.IntegralHeight = false;
            cmbMetadataSetting.Items.AddRange(new object[] { "All", "Metadata", "BG Position", "Preview" });
            cmbMetadataSetting.Location = new Point(12, 444);
            cmbMetadataSetting.Name = "cmbMetadataSetting";
            cmbMetadataSetting.Size = new Size(253, 29);
            cmbMetadataSetting.TabIndex = 31;
            cmbMetadataSetting.SelectedIndexChanged += cmbMetadataSetting_SelectedIndexChanged;
            // 
            // txtTags
            // 
            txtTags.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            txtTags.Location = new Point(12, 187);
            txtTags.Multiline = true;
            txtTags.Name = "txtTags";
            txtTags.ScrollBars = ScrollBars.Vertical;
            txtTags.Size = new Size(377, 167);
            txtTags.TabIndex = 30;
            txtTags.TextChanged += txtTags_TextChanged;
            // 
            // btnApplyTags
            // 
            btnApplyTags.BackColor = Color.DarkCyan;
            btnApplyTags.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyTags.FlatAppearance.BorderSize = 2;
            btnApplyTags.FlatStyle = FlatStyle.Flat;
            btnApplyTags.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyTags.ForeColor = SystemColors.ControlLightLight;
            btnApplyTags.Location = new Point(277, 366);
            btnApplyTags.Name = "btnApplyTags";
            btnApplyTags.Size = new Size(111, 38);
            btnApplyTags.TabIndex = 29;
            btnApplyTags.TabStop = false;
            btnApplyTags.Text = "Apply to All";
            btnApplyTags.UseVisualStyleBackColor = false;
            btnApplyTags.Click += btnApplyTags_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            label4.ForeColor = Color.White;
            label4.Location = new Point(169, 156);
            label4.Name = "label4";
            label4.Size = new Size(62, 20);
            label4.TabIndex = 28;
            label4.Text = "Tag Edit";
            // 
            // btnApplyNotesPosition
            // 
            btnApplyNotesPosition.BackColor = Color.DarkCyan;
            btnApplyNotesPosition.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyNotesPosition.FlatAppearance.BorderSize = 2;
            btnApplyNotesPosition.FlatStyle = FlatStyle.Flat;
            btnApplyNotesPosition.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyNotesPosition.ForeColor = SystemColors.ControlLightLight;
            btnApplyNotesPosition.Location = new Point(290, 109);
            btnApplyNotesPosition.Name = "btnApplyNotesPosition";
            btnApplyNotesPosition.Size = new Size(99, 38);
            btnApplyNotesPosition.TabIndex = 26;
            btnApplyNotesPosition.TabStop = false;
            btnApplyNotesPosition.Text = "Run";
            btnApplyNotesPosition.UseVisualStyleBackColor = false;
            btnApplyNotesPosition.Click += btnApplyNotesPosition_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            label3.ForeColor = Color.White;
            label3.Location = new Point(146, 83);
            label3.Name = "label3";
            label3.Size = new Size(108, 20);
            label3.TabIndex = 25;
            label3.Text = "Notes Position";
            // 
            // cmbNotesPosition
            // 
            cmbNotesPosition.DropDownHeight = 120;
            cmbNotesPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbNotesPosition.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            cmbNotesPosition.FormattingEnabled = true;
            cmbNotesPosition.IntegralHeight = false;
            cmbNotesPosition.Items.AddRange(new object[] { "Center", "Sepatate", "Random" });
            cmbNotesPosition.Location = new Point(13, 114);
            cmbNotesPosition.Name = "cmbNotesPosition";
            cmbNotesPosition.Size = new Size(268, 29);
            cmbNotesPosition.TabIndex = 24;
            cmbNotesPosition.SelectedIndexChanged += cmbNotesPosition_SelectedIndexChanged;
            // 
            // btnApplyHitsound
            // 
            btnApplyHitsound.BackColor = Color.DarkCyan;
            btnApplyHitsound.FlatAppearance.BorderColor = Color.Cyan;
            btnApplyHitsound.FlatAppearance.BorderSize = 2;
            btnApplyHitsound.FlatStyle = FlatStyle.Flat;
            btnApplyHitsound.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApplyHitsound.ForeColor = SystemColors.ControlLightLight;
            btnApplyHitsound.Location = new Point(290, 36);
            btnApplyHitsound.Name = "btnApplyHitsound";
            btnApplyHitsound.Size = new Size(99, 38);
            btnApplyHitsound.TabIndex = 23;
            btnApplyHitsound.TabStop = false;
            btnApplyHitsound.Text = "Run";
            btnApplyHitsound.UseVisualStyleBackColor = false;
            btnApplyHitsound.Click += btnApplyHitsound_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold);
            label2.ForeColor = Color.White;
            label2.Location = new Point(148, 10);
            label2.Name = "label2";
            label2.Size = new Size(103, 20);
            label2.TabIndex = 20;
            label2.Text = "Hitsound Tool";
            // 
            // cmbHitsoundTool
            // 
            cmbHitsoundTool.DropDownHeight = 120;
            cmbHitsoundTool.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbHitsoundTool.Font = new Font("Yu Gothic UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 128);
            cmbHitsoundTool.FormattingEnabled = true;
            cmbHitsoundTool.IntegralHeight = false;
            cmbHitsoundTool.Items.AddRange(new object[] { "Clap", "Whistle" });
            cmbHitsoundTool.Location = new Point(13, 41);
            cmbHitsoundTool.Name = "cmbHitsoundTool";
            cmbHitsoundTool.Size = new Size(268, 29);
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
            btnSetResnapTimingTo.Location = new Point(270, 539);
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
            btnSetResnapTimingFrom.Location = new Point(87, 539);
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
            btnSwapResnapTiming.Location = new Point(206, 517);
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
            lblResnapTiming.Location = new Point(17, 517);
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
            txtResnapTimingFrom.Location = new Point(87, 517);
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
            txtResnapTimingTo.Location = new Point(270, 517);
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
        private CheckBox chkEnableBeatSnap;
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
        private Label label3;
        private ComboBox cmbNotesPosition;
        private Button btnApplyHitsound;
        private Label label2;
        private TextBox txtTags;
        private Button btnApplyTags;
        private Label label4;
        private Button btnApplyMetadata;
        private Label label5;
        private ComboBox cmbMetadataSetting;
        private Label label6;
        private Button btnApplyResnap;
        private ComboBox cmbResnapBeatSnap;
        private Label label1;
        private Button btnBackup;
        private ToolStripMenuItem timingPropertyToolStripMenuItem;
    }
}