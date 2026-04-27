namespace osu_taiko_Mapping_Helper.Views
{
    partial class DialogForm
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
            btnOk = new Button();
            pnlBg = new Panel();
            lblMessage = new Label();
            picSystemIcon = new PictureBox();
            btnCancel = new Button();
            ((System.ComponentModel.ISupportInitialize)picSystemIcon).BeginInit();
            SuspendLayout();
            // 
            // btnOk
            // 
            btnOk.BackColor = Color.Transparent;
            btnOk.Location = new Point(177, 79);
            btnOk.Name = "btnOk";
            btnOk.Size = new Size(75, 23);
            btnOk.TabIndex = 1;
            btnOk.Text = "OK";
            btnOk.UseVisualStyleBackColor = false;
            btnOk.Click += btnOk_Click;
            // 
            // pnlBg
            // 
            pnlBg.BackColor = Color.FromArgb(250, 250, 250);
            pnlBg.Location = new Point(1, 72);
            pnlBg.Name = "pnlBg";
            pnlBg.Size = new Size(275, 37);
            pnlBg.TabIndex = 2;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Font = new Font("Yu Gothic UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 128);
            lblMessage.Location = new Point(68, 28);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(38, 15);
            lblMessage.TabIndex = 3;
            lblMessage.Text = "label1";
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picSystemIcon
            // 
            picSystemIcon.Image = Properties.Resources.systemicon_info;
            picSystemIcon.Location = new Point(21, 20);
            picSystemIcon.Name = "picSystemIcon";
            picSystemIcon.Size = new Size(32, 32);
            picSystemIcon.TabIndex = 4;
            picSystemIcon.TabStop = false;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.Transparent;
            btnCancel.Location = new Point(-100, 79);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(75, 23);
            btnCancel.TabIndex = 5;
            btnCancel.Text = "OK";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += btnCancel_Click;
            // 
            // DialogForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(267, 108);
            Controls.Add(btnCancel);
            Controls.Add(picSystemIcon);
            Controls.Add(lblMessage);
            Controls.Add(btnOk);
            Controls.Add(pnlBg);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "DialogForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Information";
            ((System.ComponentModel.ISupportInitialize)picSystemIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button btnOk;
        private Panel pnlBg;
        private Label lblMessage;
        private PictureBox picSystemIcon;
        private Button btnCancel;
    }
}