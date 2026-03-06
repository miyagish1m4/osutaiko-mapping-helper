namespace osu_taiko_Mapping_Helper.Views
{
    partial class HistoryForm
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
            lblCreateDate = new Label();
            lblCreateDateData = new Label();
            SuspendLayout();
            // 
            // lblCreateDate
            // 
            lblCreateDate.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblCreateDate.ForeColor = SystemColors.ControlLight;
            lblCreateDate.Location = new Point(358, 9);
            lblCreateDate.Name = "lblCreateDate";
            lblCreateDate.Size = new Size(91, 23);
            lblCreateDate.TabIndex = 0;
            lblCreateDate.Text = "作成日時：";
            // 
            // lblCreateDateData
            // 
            lblCreateDateData.Font = new Font("Yu Gothic UI", 12F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblCreateDateData.ForeColor = SystemColors.Control;
            lblCreateDateData.Location = new Point(442, 9);
            lblCreateDateData.Name = "lblCreateDateData";
            lblCreateDateData.Size = new Size(197, 23);
            lblCreateDateData.TabIndex = 1;
            lblCreateDateData.Text = "yyyy/mm/dd HH:mm:ss:fff";
            // 
            // HistoryForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(0, 64, 64);
            ClientSize = new Size(654, 416);
            Controls.Add(lblCreateDateData);
            Controls.Add(lblCreateDate);
            Name = "HistoryForm";
            Text = "編集履歴";
            Load += HistoryForm_Load;
            ResumeLayout(false);
        }

        #endregion

        private Label lblCreateDate;
        private Label lblCreateDateData;
    }
}