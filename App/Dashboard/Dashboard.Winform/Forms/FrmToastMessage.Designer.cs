namespace Dashboard.Winform.Forms
{
    partial class FrmToastMessage
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmToastMessage));
            toastBorder = new Panel();
            iconToastMessage = new ReaLTaiizor.Controls.HopePictureBox();
            lblType = new Label();
            lblMessage = new Label();
            toastTimer = new System.Windows.Forms.Timer(components);
            toastHide = new System.Windows.Forms.Timer(components);
            toastWaitTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)iconToastMessage).BeginInit();
            SuspendLayout();
            // 
            // toastBorder
            // 
            toastBorder.BackColor = Color.FromArgb(57, 155, 53);
            toastBorder.Location = new Point(-2, -3);
            toastBorder.Name = "toastBorder";
            toastBorder.Size = new Size(9, 66);
            toastBorder.TabIndex = 0;
            // 
            // iconToastMessage
            // 
            iconToastMessage.BackColor = Color.FromArgb(192, 196, 204);
            iconToastMessage.Image = (Image)resources.GetObject("iconToastMessage.Image");
            iconToastMessage.Location = new Point(21, 20);
            iconToastMessage.Name = "iconToastMessage";
            iconToastMessage.PixelOffsetType = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
            iconToastMessage.Size = new Size(20, 20);
            iconToastMessage.SizeMode = PictureBoxSizeMode.StretchImage;
            iconToastMessage.SmoothingType = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
            iconToastMessage.TabIndex = 1;
            iconToastMessage.TabStop = false;
            iconToastMessage.TextRenderingType = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            // 
            // lblType
            // 
            lblType.AutoSize = true;
            lblType.Font = new Font("Segoe UI Semibold", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            lblType.Location = new Point(49, 10);
            lblType.Name = "lblType";
            lblType.Size = new Size(32, 17);
            lblType.TabIndex = 2;
            lblType.Text = "Loại";
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Font = new Font("Segoe UI", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMessage.Location = new Point(49, 28);
            lblMessage.MinimumSize = new Size(240, 26);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(240, 26);
            lblMessage.TabIndex = 3;
            lblMessage.Text = "Nội dung";
            lblMessage.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // toastTimer
            // 
            toastTimer.Enabled = true;
            toastTimer.Interval = 10;
            toastTimer.Tick += toastTimer_Tick;
            // 
            // toastHide
            // 
            toastHide.Interval = 10;
            toastHide.Tick += toastHide_Tick;
            // 
            // toastWaitTimer
            // 
            toastWaitTimer.Interval = 10;
            // 
            // FrmToastMessage
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(298, 59);
            Controls.Add(lblMessage);
            Controls.Add(lblType);
            Controls.Add(iconToastMessage);
            Controls.Add(toastBorder);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmToastMessage";
            Text = "ToastMessage";
            Load += FrmToastMessage_Load;
            ((System.ComponentModel.ISupportInitialize)iconToastMessage).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel toastBorder;
        private ReaLTaiizor.Controls.HopePictureBox iconToastMessage;
        private Label lblType;
        private Label lblMessage;
        private System.Windows.Forms.Timer toastTimer;
        private System.Windows.Forms.Timer toastHide;
        private System.Windows.Forms.Timer toastWaitTimer;
    }
}