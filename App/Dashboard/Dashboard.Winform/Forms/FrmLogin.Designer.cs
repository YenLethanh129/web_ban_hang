namespace Dashboard.Winform.Forms
{
    partial class FrmLogin
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmLogin));
            panel1 = new Panel();
            gradientPanel1 = new Dashboard.Winform.Controls.GradientPanel();
            label2 = new Label();
            pictureBox2 = new PictureBox();
            label1 = new Label();
            pictureBox1 = new PictureBox();
            panel2 = new Panel();
            btnLogin = new Button();
            btnExit = new Button();
            label4 = new Label();
            linkLabel1 = new LinkLabel();
            tbxPassword = new Dashboard.Winform.Controls.loginTextBox();
            tbxUsername = new Dashboard.Winform.Controls.loginTextBox();
            bigLabel1 = new ReaLTaiizor.Controls.BigLabel();
            panel1.SuspendLayout();
            gradientPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(gradientPanel1);
            panel1.Controls.Add(panel2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(736, 554);
            panel1.TabIndex = 0;
            // 
            // gradientPanel1
            // 
            gradientPanel1.Controls.Add(label2);
            gradientPanel1.Controls.Add(pictureBox2);
            gradientPanel1.Controls.Add(label1);
            gradientPanel1.Controls.Add(pictureBox1);
            gradientPanel1.Dock = DockStyle.Fill;
            gradientPanel1.gradientBottom = Color.FromArgb(210, 143, 80);
            gradientPanel1.gradientTop = Color.FromArgb(120, 38, 170);
            gradientPanel1.Location = new Point(0, 0);
            gradientPanel1.Name = "gradientPanel1";
            gradientPanel1.Size = new Size(403, 554);
            gradientPanel1.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Mistral", 14.25F);
            label2.ForeColor = Color.LemonChiffon;
            label2.Location = new Point(103, 349);
            label2.Name = "label2";
            label2.Size = new Size(207, 22);
            label2.TabIndex = 3;
            label2.Text = "Hệ thống quản lý cửa hàng F&B";
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.Transparent;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(145, 222);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(116, 107);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Century Gothic", 14.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.LemonChiffon;
            label1.Location = new Point(133, 171);
            label1.Name = "label1";
            label1.Size = new Size(138, 23);
            label1.TabIndex = 1;
            label1.Text = "CHÀO MỪNG ";
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(322, -3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(100, 557);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // panel2
            // 
            panel2.BackColor = Color.White;
            panel2.Controls.Add(btnLogin);
            panel2.Controls.Add(btnExit);
            panel2.Controls.Add(label4);
            panel2.Controls.Add(linkLabel1);
            panel2.Controls.Add(tbxPassword);
            panel2.Controls.Add(tbxUsername);
            panel2.Controls.Add(bigLabel1);
            panel2.Dock = DockStyle.Right;
            panel2.Location = new Point(403, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(333, 554);
            panel2.TabIndex = 0;
            // 
            // btnLogin
            // 
            btnLogin.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnLogin.BackColor = Color.FromArgb(210, 143, 80);
            btnLogin.FlatAppearance.BorderColor = Color.FromArgb(210, 143, 80);
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Microsoft Sans Serif", 11F);
            btnLogin.ForeColor = Color.White;
            btnLogin.Location = new Point(45, 400);
            btnLogin.Margin = new Padding(3, 2, 3, 2);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(105, 30);
            btnLogin.TabIndex = 9;
            btnLogin.Text = "Đăng nhập";
            btnLogin.UseVisualStyleBackColor = false;
            // 
            // btnExit
            // 
            btnExit.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnExit.FlatAppearance.BorderColor = Color.FromArgb(210, 143, 80);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.Font = new Font("Microsoft Sans Serif", 11F);
            btnExit.ForeColor = Color.FromArgb(80, 80, 80);
            btnExit.Location = new Point(166, 400);
            btnExit.Margin = new Padding(3, 2, 3, 2);
            btnExit.Name = "btnExit";
            btnExit.Size = new Size(105, 30);
            btnExit.TabIndex = 8;
            btnExit.Text = "Thoát";
            btnExit.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Palatino Linotype", 9.75F, FontStyle.Bold | FontStyle.Italic);
            label4.Location = new Point(45, 516);
            label4.Name = "label4";
            label4.Size = new Size(123, 18);
            label4.TabIndex = 5;
            label4.Text = "Cần hỗ trợ? Hotline:";
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Font = new Font("Palatino Linotype", 9.75F, FontStyle.Bold | FontStyle.Italic);
            linkLabel1.LinkColor = Color.Black;
            linkLabel1.Location = new Point(166, 516);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(111, 18);
            linkLabel1.TabIndex = 4;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "XXX-YYY-DuyDT";
            // 
            // tbxPassword
            // 
            tbxPassword.BackColor = Color.FromArgb(210, 143, 80);
            tbxPassword.isPassword = true;
            tbxPassword.label = "Mật khẩu";
            tbxPassword.Location = new Point(38, 302);
            tbxPassword.Name = "tbxPassword";
            tbxPassword.Padding = new Padding(0, 0, 0, 2);
            tbxPassword.Size = new Size(241, 50);
            tbxPassword.TabIndex = 2;
            tbxPassword.TextValue = "";
            // 
            // tbxUsername
            // 
            tbxUsername.BackColor = Color.FromArgb(210, 143, 80);
            tbxUsername.isPassword = false;
            tbxUsername.label = "Tên đăng nhập";
            tbxUsername.Location = new Point(38, 205);
            tbxUsername.Name = "tbxUsername";
            tbxUsername.Padding = new Padding(0, 0, 0, 2);
            tbxUsername.Size = new Size(241, 50);
            tbxUsername.TabIndex = 1;
            tbxUsername.TextValue = "";
            // 
            // bigLabel1
            // 
            bigLabel1.AutoSize = true;
            bigLabel1.BackColor = Color.Transparent;
            bigLabel1.Font = new Font("Segoe UI", 24.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bigLabel1.ForeColor = Color.FromArgb(80, 80, 80);
            bigLabel1.Location = new Point(45, 113);
            bigLabel1.Name = "bigLabel1";
            bigLabel1.Size = new Size(228, 45);
            bigLabel1.TabIndex = 0;
            bigLabel1.Text = "ĐĂNG NHẬP ";
            // 
            // FrmLogin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(736, 554);
            Controls.Add(panel1);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmLogin";
            Text = "Đăng nhập";
            panel1.ResumeLayout(false);
            gradientPanel1.ResumeLayout(false);
            gradientPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Controls.GradientPanel gradientPanel1;
        private Panel panel2;
        private PictureBox pictureBox1;
        private Label label1;
        private PictureBox pictureBox2;
        private Label label2;
        private ReaLTaiizor.Controls.BigLabel bigLabel1;
        private Controls.loginTextBox tbxUsername;
        private Label label4;
        private LinkLabel linkLabel1;
        private Button btnExit;
        private Controls.loginTextBox tbxPassword;
        private Button btnLogin;
    }
}