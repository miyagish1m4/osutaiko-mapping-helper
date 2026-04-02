namespace osu_taiko_Mapping_Helper.Views
{
    partial class DebugForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DebugForm));
            lblMaxHistoryCount = new Label();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            lblCurrentTiming = new Label();
            lblCurrentBpm = new Label();
            lblCurrentSv = new Label();
            lblCurrentVolume = new Label();
            lblVisualizeBpm = new Label();
            label5 = new Label();
            SuspendLayout();
            // 
            // lblMaxHistoryCount
            // 
            lblMaxHistoryCount.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblMaxHistoryCount.ForeColor = Color.White;
            lblMaxHistoryCount.ImageAlign = ContentAlignment.MiddleRight;
            lblMaxHistoryCount.Location = new Point(2, 20);
            lblMaxHistoryCount.Name = "lblMaxHistoryCount";
            lblMaxHistoryCount.Size = new Size(190, 25);
            lblMaxHistoryCount.TabIndex = 3;
            lblMaxHistoryCount.Text = "TIming : ";
            lblMaxHistoryCount.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            label1.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            label1.ForeColor = Color.White;
            label1.Location = new Point(2, 55);
            label1.Name = "label1";
            label1.Size = new Size(190, 25);
            label1.TabIndex = 4;
            label1.Text = "BPM : ";
            label1.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label2
            // 
            label2.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            label2.ForeColor = Color.White;
            label2.Location = new Point(2, 90);
            label2.Name = "label2";
            label2.Size = new Size(190, 25);
            label2.TabIndex = 5;
            label2.Text = "SV : ";
            label2.TextAlign = ContentAlignment.MiddleRight;
            // 
            // label3
            // 
            label3.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            label3.ForeColor = Color.White;
            label3.Location = new Point(2, 160);
            label3.Name = "label3";
            label3.Size = new Size(190, 25);
            label3.TabIndex = 6;
            label3.Text = "Volume : ";
            label3.TextAlign = ContentAlignment.MiddleRight;
            // 
            // lblCurrentTiming
            // 
            lblCurrentTiming.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblCurrentTiming.ForeColor = Color.White;
            lblCurrentTiming.ImageAlign = ContentAlignment.MiddleRight;
            lblCurrentTiming.Location = new Point(182, 20);
            lblCurrentTiming.Name = "lblCurrentTiming";
            lblCurrentTiming.Size = new Size(151, 25);
            lblCurrentTiming.TabIndex = 7;
            lblCurrentTiming.Text = "00:00:000";
            lblCurrentTiming.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCurrentBpm
            // 
            lblCurrentBpm.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblCurrentBpm.ForeColor = Color.White;
            lblCurrentBpm.ImageAlign = ContentAlignment.MiddleRight;
            lblCurrentBpm.Location = new Point(182, 55);
            lblCurrentBpm.Name = "lblCurrentBpm";
            lblCurrentBpm.Size = new Size(151, 25);
            lblCurrentBpm.TabIndex = 8;
            lblCurrentBpm.Text = "120";
            lblCurrentBpm.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCurrentSv
            // 
            lblCurrentSv.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblCurrentSv.ForeColor = Color.White;
            lblCurrentSv.ImageAlign = ContentAlignment.MiddleRight;
            lblCurrentSv.Location = new Point(182, 90);
            lblCurrentSv.Name = "lblCurrentSv";
            lblCurrentSv.Size = new Size(151, 25);
            lblCurrentSv.TabIndex = 9;
            lblCurrentSv.Text = "1";
            lblCurrentSv.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblCurrentVolume
            // 
            lblCurrentVolume.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblCurrentVolume.ForeColor = Color.White;
            lblCurrentVolume.ImageAlign = ContentAlignment.MiddleRight;
            lblCurrentVolume.Location = new Point(182, 160);
            lblCurrentVolume.Name = "lblCurrentVolume";
            lblCurrentVolume.Size = new Size(151, 25);
            lblCurrentVolume.TabIndex = 10;
            lblCurrentVolume.Text = "100";
            lblCurrentVolume.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblVisualizeBpm
            // 
            lblVisualizeBpm.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblVisualizeBpm.ForeColor = Color.White;
            lblVisualizeBpm.ImageAlign = ContentAlignment.MiddleRight;
            lblVisualizeBpm.Location = new Point(182, 125);
            lblVisualizeBpm.Name = "lblVisualizeBpm";
            lblVisualizeBpm.Size = new Size(151, 25);
            lblVisualizeBpm.TabIndex = 12;
            lblVisualizeBpm.Text = "1";
            lblVisualizeBpm.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            label5.Font = new Font("Yu Gothic UI", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            label5.ForeColor = Color.White;
            label5.Location = new Point(2, 125);
            label5.Name = "label5";
            label5.Size = new Size(190, 25);
            label5.TabIndex = 11;
            label5.Text = "Visualization BPM : ";
            label5.TextAlign = ContentAlignment.MiddleRight;
            // 
            // DebugForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 64, 64);
            ClientSize = new Size(334, 211);
            Controls.Add(lblVisualizeBpm);
            Controls.Add(label5);
            Controls.Add(lblCurrentVolume);
            Controls.Add(lblCurrentSv);
            Controls.Add(lblCurrentBpm);
            Controls.Add(lblCurrentTiming);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(lblMaxHistoryCount);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "DebugForm";
            Text = "DebugForm";
            Load += DebugForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label lblMaxHistoryCount;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label lblCurrentTiming;
        private Label lblCurrentBpm;
        private Label lblCurrentSv;
        private Label lblCurrentVolume;
        private Label lblVisualizeBpm;
        private Label label5;
    }
}