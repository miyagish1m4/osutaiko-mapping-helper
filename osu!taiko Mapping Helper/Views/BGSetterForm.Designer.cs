namespace osu_taiko_Mapping_Helper.Views
{
    partial class BGSetterForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BGSetterForm));
            picDisplayBg = new PictureBox();
            picTaikoBar = new PictureBox();
            pictureBox1 = new PictureBox();
            btnApply = new Button();
            lblCurrentPosY = new Label();
            ((System.ComponentModel.ISupportInitialize)picDisplayBg).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picTaikoBar).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // picDisplayBg
            // 
            picDisplayBg.Location = new Point(0, 0);
            picDisplayBg.Name = "picDisplayBg";
            picDisplayBg.Size = new Size(1280, 720);
            picDisplayBg.SizeMode = PictureBoxSizeMode.AutoSize;
            picDisplayBg.TabIndex = 1;
            picDisplayBg.TabStop = false;
            picDisplayBg.MouseDown += picDisplayBg_MouseDown;
            picDisplayBg.MouseMove += picDisplayBg_MouseMove;
            picDisplayBg.MouseUp += picDisplayBg_MouseUp;
            // 
            // picTaikoBar
            // 
            picTaikoBar.BackColor = Color.Black;
            picTaikoBar.Location = new Point(0, 0);
            picTaikoBar.Name = "picTaikoBar";
            picTaikoBar.Size = new Size(854, 252);
            picTaikoBar.TabIndex = 2;
            picTaikoBar.TabStop = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(0, 64, 64);
            pictureBox1.Location = new Point(0, 480);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(1280, 60);
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // btnApply
            // 
            btnApply.BackColor = Color.DarkCyan;
            btnApply.FlatAppearance.BorderColor = Color.Cyan;
            btnApply.FlatAppearance.BorderSize = 2;
            btnApply.FlatStyle = FlatStyle.Flat;
            btnApply.Font = new Font("Yu Gothic UI", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 128);
            btnApply.ForeColor = SystemColors.ControlLightLight;
            btnApply.Location = new Point(337, 490);
            btnApply.Name = "btnApply";
            btnApply.Size = new Size(181, 39);
            btnApply.TabIndex = 4;
            btnApply.TabStop = false;
            btnApply.Text = "実行";
            btnApply.UseVisualStyleBackColor = false;
            btnApply.Click += btnApply_Click;
            // 
            // lblCurrentPosY
            // 
            lblCurrentPosY.BackColor = Color.Black;
            lblCurrentPosY.Font = new Font("Yu Gothic UI", 24F, FontStyle.Bold, GraphicsUnit.Point, 128);
            lblCurrentPosY.ForeColor = Color.White;
            lblCurrentPosY.ImageAlign = ContentAlignment.BottomLeft;
            lblCurrentPosY.Location = new Point(0, 76);
            lblCurrentPosY.Name = "lblCurrentPosY";
            lblCurrentPosY.Size = new Size(854, 100);
            lblCurrentPosY.TabIndex = 8;
            lblCurrentPosY.Text = "y : ";
            lblCurrentPosY.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // BGSetterForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(854, 540);
            Controls.Add(lblCurrentPosY);
            Controls.Add(btnApply);
            Controls.Add(pictureBox1);
            Controls.Add(picTaikoBar);
            Controls.Add(picDisplayBg);
            Icon = (Icon)resources.GetObject("$this.Icon");
            KeyPreview = true;
            Name = "BGSetterForm";
            Text = "BGSetterForm";
            KeyDown += BGSetterForm_KeyDown;
            ((System.ComponentModel.ISupportInitialize)picDisplayBg).EndInit();
            ((System.ComponentModel.ISupportInitialize)picTaikoBar).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private PictureBox picDisplayBg;
        private PictureBox picTaikoBar;
        private PictureBox pictureBox1;
        private Button btnApply;
        private Label lblCurrentPosY;
    }
}