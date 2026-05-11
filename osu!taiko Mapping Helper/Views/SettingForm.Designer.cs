namespace osu_taiko_Mapping_Helper.Views
{
    partial class SettingForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingForm));
            lblLanguage = new Label();
            lblMaxBackupCount = new Label();
            txtMaxBackupCount = new TextBox();
            cmbLanguage = new ComboBox();
            btnSave = new Button();
            label1 = new Label();
            chkAdvanceMode = new CheckBox();
            pnlNotesKind = new Panel();
            rdoFinisherKat = new RadioButton();
            rdoFinisherDon = new RadioButton();
            rdoKat = new RadioButton();
            rdoDon = new RadioButton();
            lblSeparateSetting = new Label();
            grpSeparateSetting = new GroupBox();
            lblPisitionY = new Label();
            txtPositionY = new TextBox();
            lblPisitionX = new Label();
            txtPisitionX = new TextBox();
            chkUnicodeSupport = new CheckBox();
            lblUnicodeSupport = new Label();
            lblOffsetType = new Label();
            rdoHexaAndDuoOffset = new RadioButton();
            rdoMiliSecondOffset = new RadioButton();
            panel1 = new Panel();
            pnlNotesKind.SuspendLayout();
            grpSeparateSetting.SuspendLayout();
            SuspendLayout();
            // 
            // lblLanguage
            // 
            lblLanguage.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblLanguage.ForeColor = Color.White;
            lblLanguage.Location = new Point(12, 40);
            lblLanguage.Name = "lblLanguage";
            lblLanguage.Size = new Size(256, 23);
            lblLanguage.TabIndex = 0;
            lblLanguage.Text = "言語設定";
            lblLanguage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblMaxBackupCount
            // 
            lblMaxBackupCount.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblMaxBackupCount.ForeColor = Color.White;
            lblMaxBackupCount.Location = new Point(12, 100);
            lblMaxBackupCount.Name = "lblMaxBackupCount";
            lblMaxBackupCount.Size = new Size(256, 25);
            lblMaxBackupCount.TabIndex = 1;
            lblMaxBackupCount.Text = "バックアップの最大保持数";
            lblMaxBackupCount.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtMaxBackupCount
            // 
            txtMaxBackupCount.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            txtMaxBackupCount.Location = new Point(274, 100);
            txtMaxBackupCount.Name = "txtMaxBackupCount";
            txtMaxBackupCount.Size = new Size(180, 27);
            txtMaxBackupCount.TabIndex = 3;
            txtMaxBackupCount.TextAlign = HorizontalAlignment.Right;
            // 
            // cmbLanguage
            // 
            cmbLanguage.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbLanguage.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            cmbLanguage.FormattingEnabled = true;
            cmbLanguage.ImeMode = ImeMode.Disable;
            cmbLanguage.Location = new Point(274, 40);
            cmbLanguage.Name = "cmbLanguage";
            cmbLanguage.Size = new Size(180, 28);
            cmbLanguage.TabIndex = 5;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom;
            btnSave.BackColor = Color.DarkCyan;
            btnSave.FlatAppearance.BorderColor = Color.Cyan;
            btnSave.FlatAppearance.BorderSize = 2;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnSave.ForeColor = SystemColors.ControlLightLight;
            btnSave.ImageAlign = ContentAlignment.MiddleLeft;
            btnSave.Location = new Point(146, 482);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(189, 39);
            btnSave.TabIndex = 15;
            btnSave.TabStop = false;
            btnSave.Text = "保存";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += btnSave_Click;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Bottom;
            label1.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            label1.ForeColor = Color.White;
            label1.Location = new Point(12, 426);
            label1.Name = "label1";
            label1.Size = new Size(256, 25);
            label1.TabIndex = 16;
            label1.Text = "AdvanceMode";
            label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkAdvanceMode
            // 
            chkAdvanceMode.Anchor = AnchorStyles.Bottom;
            chkAdvanceMode.Appearance = Appearance.Button;
            chkAdvanceMode.BackColor = Color.White;
            chkAdvanceMode.Font = new Font("MS UI Gothic", 15F, FontStyle.Regular, GraphicsUnit.Point, 128);
            chkAdvanceMode.ForeColor = Color.Green;
            chkAdvanceMode.Location = new Point(273, 426);
            chkAdvanceMode.Name = "chkAdvanceMode";
            chkAdvanceMode.Size = new Size(25, 25);
            chkAdvanceMode.TabIndex = 17;
            chkAdvanceMode.TextAlign = ContentAlignment.MiddleCenter;
            chkAdvanceMode.UseVisualStyleBackColor = false;
            chkAdvanceMode.CheckedChanged += chkAdvanceMode_CheckedChanged;
            // 
            // pnlNotesKind
            // 
            pnlNotesKind.Controls.Add(rdoFinisherKat);
            pnlNotesKind.Controls.Add(rdoFinisherDon);
            pnlNotesKind.Controls.Add(rdoKat);
            pnlNotesKind.Controls.Add(rdoDon);
            pnlNotesKind.Location = new Point(1, 9);
            pnlNotesKind.Name = "pnlNotesKind";
            pnlNotesKind.Size = new Size(135, 105);
            pnlNotesKind.TabIndex = 18;
            // 
            // rdoFinisherKat
            // 
            rdoFinisherKat.AutoSize = true;
            rdoFinisherKat.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoFinisherKat.ForeColor = Color.White;
            rdoFinisherKat.Location = new Point(8, 78);
            rdoFinisherKat.Name = "rdoFinisherKat";
            rdoFinisherKat.Size = new Size(97, 21);
            rdoFinisherKat.TabIndex = 3;
            rdoFinisherKat.TabStop = true;
            rdoFinisherKat.Text = "Finisher Kat";
            rdoFinisherKat.UseVisualStyleBackColor = true;
            rdoFinisherKat.CheckedChanged += rdoFinisherKat_CheckedChanged;
            // 
            // rdoFinisherDon
            // 
            rdoFinisherDon.AutoSize = true;
            rdoFinisherDon.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoFinisherDon.ForeColor = Color.White;
            rdoFinisherDon.Location = new Point(8, 54);
            rdoFinisherDon.Name = "rdoFinisherDon";
            rdoFinisherDon.Size = new Size(102, 21);
            rdoFinisherDon.TabIndex = 2;
            rdoFinisherDon.TabStop = true;
            rdoFinisherDon.Text = "Finisher Don";
            rdoFinisherDon.UseVisualStyleBackColor = true;
            rdoFinisherDon.CheckedChanged += rdoFinisherDon_CheckedChanged;
            // 
            // rdoKat
            // 
            rdoKat.AutoSize = true;
            rdoKat.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoKat.ForeColor = Color.White;
            rdoKat.Location = new Point(8, 30);
            rdoKat.Name = "rdoKat";
            rdoKat.Size = new Size(46, 21);
            rdoKat.TabIndex = 1;
            rdoKat.TabStop = true;
            rdoKat.Text = "Kat";
            rdoKat.UseVisualStyleBackColor = true;
            rdoKat.CheckedChanged += rdoKat_CheckedChanged;
            // 
            // rdoDon
            // 
            rdoDon.AutoSize = true;
            rdoDon.Font = new Font("Yu Gothic UI", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoDon.ForeColor = Color.White;
            rdoDon.Location = new Point(8, 6);
            rdoDon.Name = "rdoDon";
            rdoDon.Size = new Size(51, 21);
            rdoDon.TabIndex = 0;
            rdoDon.TabStop = true;
            rdoDon.Text = "Don";
            rdoDon.UseVisualStyleBackColor = true;
            rdoDon.CheckedChanged += rdoDon_CheckedChanged;
            // 
            // lblSeparateSetting
            // 
            lblSeparateSetting.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblSeparateSetting.ForeColor = Color.White;
            lblSeparateSetting.Location = new Point(12, 244);
            lblSeparateSetting.Name = "lblSeparateSetting";
            lblSeparateSetting.Size = new Size(203, 25);
            lblSeparateSetting.TabIndex = 19;
            lblSeparateSetting.Text = "ノーツ配置設定";
            lblSeparateSetting.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // grpSeparateSetting
            // 
            grpSeparateSetting.Controls.Add(lblPisitionY);
            grpSeparateSetting.Controls.Add(pnlNotesKind);
            grpSeparateSetting.Controls.Add(txtPositionY);
            grpSeparateSetting.Controls.Add(lblPisitionX);
            grpSeparateSetting.Controls.Add(txtPisitionX);
            grpSeparateSetting.Location = new Point(215, 234);
            grpSeparateSetting.Name = "grpSeparateSetting";
            grpSeparateSetting.Size = new Size(239, 117);
            grpSeparateSetting.TabIndex = 20;
            grpSeparateSetting.TabStop = false;
            // 
            // lblPisitionY
            // 
            lblPisitionY.Anchor = AnchorStyles.Right;
            lblPisitionY.AutoSize = true;
            lblPisitionY.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblPisitionY.ForeColor = Color.White;
            lblPisitionY.Location = new Point(138, 65);
            lblPisitionY.Name = "lblPisitionY";
            lblPisitionY.Size = new Size(37, 25);
            lblPisitionY.TabIndex = 22;
            lblPisitionY.Text = "y : ";
            lblPisitionY.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtPositionY
            // 
            txtPositionY.Anchor = AnchorStyles.Right;
            txtPositionY.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            txtPositionY.Location = new Point(178, 66);
            txtPositionY.Name = "txtPositionY";
            txtPositionY.Size = new Size(50, 27);
            txtPositionY.TabIndex = 23;
            txtPositionY.TextAlign = HorizontalAlignment.Right;
            txtPositionY.TextChanged += txtPositionY_TextChanged;
            // 
            // lblPisitionX
            // 
            lblPisitionX.Anchor = AnchorStyles.Right;
            lblPisitionX.AutoSize = true;
            lblPisitionX.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblPisitionX.ForeColor = Color.White;
            lblPisitionX.Location = new Point(138, 27);
            lblPisitionX.Name = "lblPisitionX";
            lblPisitionX.Size = new Size(37, 25);
            lblPisitionX.TabIndex = 21;
            lblPisitionX.Text = "x : ";
            lblPisitionX.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // txtPisitionX
            // 
            txtPisitionX.Anchor = AnchorStyles.Right;
            txtPisitionX.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            txtPisitionX.Location = new Point(178, 28);
            txtPisitionX.Name = "txtPisitionX";
            txtPisitionX.Size = new Size(50, 27);
            txtPisitionX.TabIndex = 21;
            txtPisitionX.TextAlign = HorizontalAlignment.Right;
            txtPisitionX.TextChanged += txtPisitionX_TextChanged;
            // 
            // chkUnicodeSupport
            // 
            chkUnicodeSupport.Anchor = AnchorStyles.Bottom;
            chkUnicodeSupport.Appearance = Appearance.Button;
            chkUnicodeSupport.BackColor = Color.White;
            chkUnicodeSupport.Font = new Font("MS UI Gothic", 15F, FontStyle.Regular, GraphicsUnit.Point, 128);
            chkUnicodeSupport.ForeColor = Color.Green;
            chkUnicodeSupport.Location = new Point(273, 366);
            chkUnicodeSupport.Name = "chkUnicodeSupport";
            chkUnicodeSupport.Size = new Size(25, 25);
            chkUnicodeSupport.TabIndex = 22;
            chkUnicodeSupport.TextAlign = ContentAlignment.MiddleCenter;
            chkUnicodeSupport.UseVisualStyleBackColor = false;
            chkUnicodeSupport.CheckedChanged += chkUnicodeSupport_CheckedChanged;
            // 
            // lblUnicodeSupport
            // 
            lblUnicodeSupport.Anchor = AnchorStyles.Bottom;
            lblUnicodeSupport.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblUnicodeSupport.ForeColor = Color.White;
            lblUnicodeSupport.Location = new Point(12, 366);
            lblUnicodeSupport.Name = "lblUnicodeSupport";
            lblUnicodeSupport.Size = new Size(256, 25);
            lblUnicodeSupport.TabIndex = 21;
            lblUnicodeSupport.Text = "Unicode対応";
            lblUnicodeSupport.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblOffsetType
            // 
            lblOffsetType.Anchor = AnchorStyles.Bottom;
            lblOffsetType.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblOffsetType.ForeColor = Color.White;
            lblOffsetType.Location = new Point(12, 160);
            lblOffsetType.Name = "lblOffsetType";
            lblOffsetType.Size = new Size(256, 25);
            lblOffsetType.TabIndex = 23;
            lblOffsetType.Text = "Unicode対応";
            lblOffsetType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // rdoHexaAndDuoOffset
            // 
            rdoHexaAndDuoOffset.AutoSize = true;
            rdoHexaAndDuoOffset.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoHexaAndDuoOffset.ForeColor = Color.White;
            rdoHexaAndDuoOffset.Location = new Point(274, 160);
            rdoHexaAndDuoOffset.Name = "rdoHexaAndDuoOffset";
            rdoHexaAndDuoOffset.Size = new Size(135, 24);
            rdoHexaAndDuoOffset.TabIndex = 24;
            rdoHexaAndDuoOffset.TabStop = true;
            rdoHexaAndDuoOffset.Text = "1/16 (1/12) offset";
            rdoHexaAndDuoOffset.UseVisualStyleBackColor = true;
            rdoHexaAndDuoOffset.CheckedChanged += rdoHexaAndDuoOffset_CheckedChanged;
            // 
            // rdoMiliSecondOffset
            // 
            rdoMiliSecondOffset.AutoSize = true;
            rdoMiliSecondOffset.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            rdoMiliSecondOffset.ForeColor = Color.White;
            rdoMiliSecondOffset.Location = new Point(273, 190);
            rdoMiliSecondOffset.Name = "rdoMiliSecondOffset";
            rdoMiliSecondOffset.Size = new Size(88, 24);
            rdoMiliSecondOffset.TabIndex = 25;
            rdoMiliSecondOffset.TabStop = true;
            rdoMiliSecondOffset.Text = "ms offset";
            rdoMiliSecondOffset.UseVisualStyleBackColor = true;
            rdoMiliSecondOffset.CheckedChanged += rdoMiliSecondOffset_CheckedChanged;
            // 
            // panel1
            // 
            panel1.Location = new Point(268, 156);
            panel1.Name = "panel1";
            panel1.Size = new Size(186, 72);
            panel1.TabIndex = 26;
            // 
            // SettingForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = Color.FromArgb(0, 64, 64);
            ClientSize = new Size(466, 552);
            Controls.Add(rdoMiliSecondOffset);
            Controls.Add(rdoHexaAndDuoOffset);
            Controls.Add(lblOffsetType);
            Controls.Add(chkUnicodeSupport);
            Controls.Add(lblUnicodeSupport);
            Controls.Add(lblSeparateSetting);
            Controls.Add(chkAdvanceMode);
            Controls.Add(label1);
            Controls.Add(btnSave);
            Controls.Add(cmbLanguage);
            Controls.Add(txtMaxBackupCount);
            Controls.Add(lblMaxBackupCount);
            Controls.Add(lblLanguage);
            Controls.Add(grpSeparateSetting);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "SettingForm";
            Text = "Setting";
            Load += SettingForm_Load;
            pnlNotesKind.ResumeLayout(false);
            pnlNotesKind.PerformLayout();
            grpSeparateSetting.ResumeLayout(false);
            grpSeparateSetting.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblLanguage;
        private Label lblMaxBackupCount;
        private TextBox txtMaxBackupCount;
        private ComboBox cmbLanguage;
        private Button btnSave;
        private Label label1;
        private CheckBox chkAdvanceMode;
        private Panel pnlNotesKind;
        private Label lblSeparateSetting;
        private GroupBox grpSeparateSetting;
        private RadioButton rdoFinisherKat;
        private RadioButton rdoFinisherDon;
        private RadioButton rdoKat;
        private RadioButton rdoDon;
        private TextBox txtPisitionX;
        private Label lblPisitionY;
        private TextBox txtPositionY;
        private Label lblPisitionX;
        private CheckBox chkUnicodeSupport;
        private Label lblUnicodeSupport;
        private Label lblOffsetType;
        private RadioButton rdoHexaAndDuoOffset;
        private RadioButton rdoMiliSecondOffset;
        private Panel panel1;
    }
}