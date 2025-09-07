namespace Dashboard.Winform
{
    partial class FrmBaseMdiWithSidePanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmBaseMdiWithSidePanel));
            fpnSideBar = new FlowLayoutPanel();
            pnSBHeader = new Panel();
            picSideBarLogo = new PictureBox();
            lblPanelSideBarTitle = new Label();
            picSideBarIcon = new PictureBox();
            pnSBLanding = new Panel();
            iconSBlanding = new PictureBox();
            btnSBLanding = new Button();
            fpnUserManagementContainer = new FlowLayoutPanel();
            pnUserManagement = new Panel();
            pictureBox3 = new PictureBox();
            btnSBUser = new Button();
            pnSBEmployee = new Panel();
            pictureBox5 = new PictureBox();
            btnSBEmployee = new Button();
            pnSBCustomer = new Panel();
            pictureBox2 = new PictureBox();
            btnSBCustomer = new Button();
            pnSBGoods = new Panel();
            pictureBox1 = new PictureBox();
            btnSBGoods = new Button();
            pnSBProduct = new Panel();
            pictureBox7 = new PictureBox();
            btnSBProduct = new Button();
            pnSBSupplier = new Panel();
            pictureBox6 = new PictureBox();
            btnSBSupplier = new Button();
            pnSBExit = new Panel();
            pictureBox8 = new PictureBox();
            btnSBExit = new Button();
            SBUserManagementTransition = new System.Windows.Forms.Timer(components);
            SBTransition = new System.Windows.Forms.Timer(components);
            pnMainDisplayRegion = new Panel();
            pnMainContainer = new Panel();
            pnHeaderTitle = new Panel();
            metroControlBox1 = new ReaLTaiizor.Controls.MetroControlBox();
            fpnSideBar.SuspendLayout();
            pnSBHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picSideBarLogo).BeginInit();
            ((System.ComponentModel.ISupportInitialize)picSideBarIcon).BeginInit();
            pnSBLanding.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSBlanding).BeginInit();
            fpnUserManagementContainer.SuspendLayout();
            pnUserManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            pnSBEmployee.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            pnSBCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            pnSBGoods.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            pnSBProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox7).BeginInit();
            pnSBSupplier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            pnSBExit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox8).BeginInit();
            pnMainDisplayRegion.SuspendLayout();
            pnHeaderTitle.SuspendLayout();
            SuspendLayout();
            // 
            // fpnSideBar
            // 
            fpnSideBar.BackColor = Color.FromArgb(54, 58, 105);
            fpnSideBar.Controls.Add(pnSBHeader);
            fpnSideBar.Controls.Add(pnSBLanding);
            fpnSideBar.Controls.Add(fpnUserManagementContainer);
            fpnSideBar.Controls.Add(pnSBGoods);
            fpnSideBar.Controls.Add(pnSBProduct);
            fpnSideBar.Controls.Add(pnSBSupplier);
            fpnSideBar.Controls.Add(pnSBExit);
            fpnSideBar.Dock = DockStyle.Left;
            fpnSideBar.Location = new Point(0, 0);
            fpnSideBar.Name = "fpnSideBar";
            fpnSideBar.Size = new Size(246, 767);
            fpnSideBar.TabIndex = 1;
            // 
            // pnSBHeader
            // 
            pnSBHeader.Anchor = AnchorStyles.Top;
            pnSBHeader.BackColor = Color.FromArgb(54, 58, 105);
            pnSBHeader.Controls.Add(picSideBarLogo);
            pnSBHeader.Controls.Add(lblPanelSideBarTitle);
            pnSBHeader.Controls.Add(picSideBarIcon);
            fpnSideBar.SetFlowBreak(pnSBHeader, true);
            pnSBHeader.Location = new Point(3, 3);
            pnSBHeader.Name = "pnSBHeader";
            pnSBHeader.Size = new Size(246, 100);
            pnSBHeader.TabIndex = 2;
            // 
            // picSideBarLogo
            // 
            picSideBarLogo.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picSideBarLogo.BackColor = Color.FromArgb(54, 58, 105);
            picSideBarLogo.Image = (Image)resources.GetObject("picSideBarLogo.Image");
            picSideBarLogo.Location = new Point(5, 33);
            picSideBarLogo.Margin = new Padding(13, 20, 3, 20);
            picSideBarLogo.Name = "picSideBarLogo";
            picSideBarLogo.Size = new Size(61, 47);
            picSideBarLogo.SizeMode = PictureBoxSizeMode.StretchImage;
            picSideBarLogo.TabIndex = 13;
            picSideBarLogo.TabStop = false;
            // 
            // lblPanelSideBarTitle
            // 
            lblPanelSideBarTitle.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lblPanelSideBarTitle.AutoSize = true;
            lblPanelSideBarTitle.BackColor = Color.FromArgb(54, 58, 105);
            lblPanelSideBarTitle.Font = new Font("Microsoft Sans Serif", 14F);
            lblPanelSideBarTitle.ForeColor = Color.WhiteSmoke;
            lblPanelSideBarTitle.Location = new Point(70, 35);
            lblPanelSideBarTitle.Margin = new Padding(0, 20, 3, 20);
            lblPanelSideBarTitle.MinimumSize = new Size(120, 45);
            lblPanelSideBarTitle.Name = "lblPanelSideBarTitle";
            lblPanelSideBarTitle.Size = new Size(120, 45);
            lblPanelSideBarTitle.TabIndex = 11;
            lblPanelSideBarTitle.Text = "Dashboard";
            lblPanelSideBarTitle.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // picSideBarIcon
            // 
            picSideBarIcon.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            picSideBarIcon.BackColor = Color.FromArgb(54, 58, 105);
            picSideBarIcon.Image = (Image)resources.GetObject("picSideBarIcon.Image");
            picSideBarIcon.Location = new Point(196, 35);
            picSideBarIcon.Margin = new Padding(0, 21, 0, 21);
            picSideBarIcon.Name = "picSideBarIcon";
            picSideBarIcon.Size = new Size(36, 40);
            picSideBarIcon.SizeMode = PictureBoxSizeMode.StretchImage;
            picSideBarIcon.TabIndex = 12;
            picSideBarIcon.TabStop = false;
            picSideBarIcon.Click += picSideBarIcon_Click;
            // 
            // pnSBLanding
            // 
            pnSBLanding.Controls.Add(iconSBlanding);
            pnSBLanding.Controls.Add(btnSBLanding);
            pnSBLanding.Location = new Point(0, 106);
            pnSBLanding.Margin = new Padding(0);
            pnSBLanding.Name = "pnSBLanding";
            pnSBLanding.Size = new Size(246, 50);
            pnSBLanding.TabIndex = 6;
            // 
            // iconSBlanding
            // 
            iconSBlanding.BackColor = Color.FromArgb(54, 58, 105);
            iconSBlanding.Image = (Image)resources.GetObject("iconSBlanding.Image");
            iconSBlanding.Location = new Point(14, 16);
            iconSBlanding.Name = "iconSBlanding";
            iconSBlanding.Size = new Size(28, 25);
            iconSBlanding.SizeMode = PictureBoxSizeMode.StretchImage;
            iconSBlanding.TabIndex = 3;
            iconSBlanding.TabStop = false;
            // 
            // btnSBLanding
            // 
            btnSBLanding.Anchor = AnchorStyles.None;
            btnSBLanding.BackColor = Color.FromArgb(54, 58, 105);
            btnSBLanding.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBLanding.ForeColor = Color.Gainsboro;
            btnSBLanding.Location = new Point(-71, -7);
            btnSBLanding.Name = "btnSBLanding";
            btnSBLanding.RightToLeft = RightToLeft.No;
            btnSBLanding.Size = new Size(330, 70);
            btnSBLanding.TabIndex = 2;
            btnSBLanding.Text = "Trang chủ";
            btnSBLanding.UseVisualStyleBackColor = false;
            btnSBLanding.Click += btnSBLanding_Click;
            // 
            // fpnUserManagementContainer
            // 
            fpnUserManagementContainer.BackColor = Color.FromArgb(54, 58, 105);
            fpnUserManagementContainer.Controls.Add(pnUserManagement);
            fpnUserManagementContainer.Controls.Add(pnSBEmployee);
            fpnUserManagementContainer.Controls.Add(pnSBCustomer);
            fpnUserManagementContainer.Location = new Point(0, 156);
            fpnUserManagementContainer.Margin = new Padding(0);
            fpnUserManagementContainer.Name = "fpnUserManagementContainer";
            fpnUserManagementContainer.Size = new Size(246, 50);
            fpnUserManagementContainer.TabIndex = 4;
            // 
            // pnUserManagement
            // 
            pnUserManagement.Controls.Add(pictureBox3);
            pnUserManagement.Controls.Add(btnSBUser);
            pnUserManagement.Location = new Point(0, 0);
            pnUserManagement.Margin = new Padding(0);
            pnUserManagement.Name = "pnUserManagement";
            pnUserManagement.Size = new Size(246, 50);
            pnUserManagement.TabIndex = 4;
            // 
            // pictureBox3
            // 
            pictureBox3.BackColor = Color.FromArgb(54, 58, 105);
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(14, 16);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(28, 25);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 3;
            pictureBox3.TabStop = false;
            // 
            // btnSBUser
            // 
            btnSBUser.Anchor = AnchorStyles.None;
            btnSBUser.BackColor = Color.FromArgb(54, 58, 105);
            btnSBUser.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBUser.ForeColor = Color.Gainsboro;
            btnSBUser.Location = new Point(-101, -10);
            btnSBUser.Name = "btnSBUser";
            btnSBUser.Size = new Size(401, 70);
            btnSBUser.TabIndex = 2;
            btnSBUser.Text = "Người dùng";
            btnSBUser.UseVisualStyleBackColor = false;
            btnSBUser.Click += BtnSBUser_Click;
            // 
            // pnSBEmployee
            // 
            pnSBEmployee.Controls.Add(pictureBox5);
            pnSBEmployee.Controls.Add(btnSBEmployee);
            pnSBEmployee.Location = new Point(0, 50);
            pnSBEmployee.Margin = new Padding(0);
            pnSBEmployee.Name = "pnSBEmployee";
            pnSBEmployee.Size = new Size(246, 50);
            pnSBEmployee.TabIndex = 6;
            // 
            // pictureBox5
            // 
            pictureBox5.BackColor = Color.FromArgb(54, 58, 105);
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(51, 16);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(28, 25);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 3;
            pictureBox5.TabStop = false;
            // 
            // btnSBEmployee
            // 
            btnSBEmployee.BackColor = Color.FromArgb(54, 58, 105);
            btnSBEmployee.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBEmployee.ForeColor = Color.Gainsboro;
            btnSBEmployee.Location = new Point(-38, -7);
            btnSBEmployee.Name = "btnSBEmployee";
            btnSBEmployee.Size = new Size(307, 70);
            btnSBEmployee.TabIndex = 2;
            btnSBEmployee.Text = "Nhân sự";
            btnSBEmployee.UseVisualStyleBackColor = false;
            // 
            // pnSBCustomer
            // 
            pnSBCustomer.Controls.Add(pictureBox2);
            pnSBCustomer.Controls.Add(btnSBCustomer);
            pnSBCustomer.Location = new Point(0, 100);
            pnSBCustomer.Margin = new Padding(0);
            pnSBCustomer.Name = "pnSBCustomer";
            pnSBCustomer.Size = new Size(246, 50);
            pnSBCustomer.TabIndex = 7;
            // 
            // pictureBox2
            // 
            pictureBox2.BackColor = Color.FromArgb(54, 58, 105);
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(51, 16);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(28, 25);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // btnSBCustomer
            // 
            btnSBCustomer.BackColor = Color.FromArgb(54, 58, 105);
            btnSBCustomer.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBCustomer.ForeColor = Color.Gainsboro;
            btnSBCustomer.Location = new Point(-25, -7);
            btnSBCustomer.Name = "btnSBCustomer";
            btnSBCustomer.Size = new Size(307, 70);
            btnSBCustomer.TabIndex = 2;
            btnSBCustomer.Text = "Khách hàng";
            btnSBCustomer.UseVisualStyleBackColor = false;
            // 
            // pnSBGoods
            // 
            pnSBGoods.Controls.Add(pictureBox1);
            pnSBGoods.Controls.Add(btnSBGoods);
            pnSBGoods.Location = new Point(0, 206);
            pnSBGoods.Margin = new Padding(0);
            pnSBGoods.Name = "pnSBGoods";
            pnSBGoods.Size = new Size(246, 50);
            pnSBGoods.TabIndex = 7;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.FromArgb(54, 58, 105);
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(14, 16);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(28, 25);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 3;
            pictureBox1.TabStop = false;
            // 
            // btnSBGoods
            // 
            btnSBGoods.Anchor = AnchorStyles.None;
            btnSBGoods.BackColor = Color.FromArgb(54, 58, 105);
            btnSBGoods.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBGoods.ForeColor = Color.Gainsboro;
            btnSBGoods.Location = new Point(-84, -6);
            btnSBGoods.Name = "btnSBGoods";
            btnSBGoods.Size = new Size(356, 70);
            btnSBGoods.TabIndex = 2;
            btnSBGoods.Text = "Hàng hóa";
            btnSBGoods.UseVisualStyleBackColor = false;
            // 
            // pnSBProduct
            // 
            pnSBProduct.Controls.Add(pictureBox7);
            pnSBProduct.Controls.Add(btnSBProduct);
            pnSBProduct.Location = new Point(0, 256);
            pnSBProduct.Margin = new Padding(0);
            pnSBProduct.Name = "pnSBProduct";
            pnSBProduct.Size = new Size(246, 50);
            pnSBProduct.TabIndex = 9;
            // 
            // pictureBox7
            // 
            pictureBox7.BackColor = Color.FromArgb(54, 58, 105);
            pictureBox7.Image = (Image)resources.GetObject("pictureBox7.Image");
            pictureBox7.Location = new Point(14, 16);
            pictureBox7.Name = "pictureBox7";
            pictureBox7.Size = new Size(28, 25);
            pictureBox7.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox7.TabIndex = 3;
            pictureBox7.TabStop = false;
            // 
            // btnSBProduct
            // 
            btnSBProduct.Anchor = AnchorStyles.None;
            btnSBProduct.BackColor = Color.FromArgb(54, 58, 105);
            btnSBProduct.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBProduct.ForeColor = Color.Gainsboro;
            btnSBProduct.Location = new Point(-125, -6);
            btnSBProduct.Name = "btnSBProduct";
            btnSBProduct.RightToLeft = RightToLeft.No;
            btnSBProduct.Size = new Size(440, 70);
            btnSBProduct.TabIndex = 2;
            btnSBProduct.Text = "Sản phẩm";
            btnSBProduct.UseVisualStyleBackColor = false;
            // 
            // pnSBSupplier
            // 
            pnSBSupplier.Controls.Add(pictureBox6);
            pnSBSupplier.Controls.Add(btnSBSupplier);
            pnSBSupplier.Location = new Point(0, 306);
            pnSBSupplier.Margin = new Padding(0);
            pnSBSupplier.Name = "pnSBSupplier";
            pnSBSupplier.Size = new Size(246, 50);
            pnSBSupplier.TabIndex = 8;
            // 
            // pictureBox6
            // 
            pictureBox6.BackColor = Color.FromArgb(54, 58, 105);
            pictureBox6.Image = (Image)resources.GetObject("pictureBox6.Image");
            pictureBox6.Location = new Point(15, 16);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(28, 25);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 3;
            pictureBox6.TabStop = false;
            // 
            // btnSBSupplier
            // 
            btnSBSupplier.Anchor = AnchorStyles.None;
            btnSBSupplier.BackColor = Color.FromArgb(54, 58, 105);
            btnSBSupplier.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBSupplier.ForeColor = Color.Gainsboro;
            btnSBSupplier.Location = new Point(-46, -7);
            btnSBSupplier.Name = "btnSBSupplier";
            btnSBSupplier.RightToLeft = RightToLeft.No;
            btnSBSupplier.Size = new Size(307, 70);
            btnSBSupplier.TabIndex = 2;
            btnSBSupplier.Text = "Nhà cung cấp";
            btnSBSupplier.UseVisualStyleBackColor = false;
            // 
            // pnSBExit
            // 
            pnSBExit.Controls.Add(pictureBox8);
            pnSBExit.Controls.Add(btnSBExit);
            pnSBExit.Location = new Point(0, 356);
            pnSBExit.Margin = new Padding(0);
            pnSBExit.Name = "pnSBExit";
            pnSBExit.Size = new Size(246, 50);
            pnSBExit.TabIndex = 10;
            // 
            // pictureBox8
            // 
            pictureBox8.BackColor = Color.FromArgb(54, 58, 105);
            pictureBox8.Image = (Image)resources.GetObject("pictureBox8.Image");
            pictureBox8.Location = new Point(16, 16);
            pictureBox8.Name = "pictureBox8";
            pictureBox8.Size = new Size(28, 25);
            pictureBox8.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox8.TabIndex = 3;
            pictureBox8.TabStop = false;
            // 
            // btnSBExit
            // 
            btnSBExit.Anchor = AnchorStyles.None;
            btnSBExit.BackColor = Color.FromArgb(54, 58, 105);
            btnSBExit.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBExit.ForeColor = Color.Gainsboro;
            btnSBExit.Location = new Point(-145, -7);
            btnSBExit.Name = "btnSBExit";
            btnSBExit.RightToLeft = RightToLeft.No;
            btnSBExit.Size = new Size(452, 70);
            btnSBExit.TabIndex = 2;
            btnSBExit.Text = "Thoát";
            btnSBExit.UseVisualStyleBackColor = false;
            // 
            // SBUserManagementTransition
            // 
            SBUserManagementTransition.Interval = 10;
            SBUserManagementTransition.Tick += SBUserManagementTransition_Tick;
            // 
            // SBTransition
            // 
            SBTransition.Interval = 10;
            SBTransition.Tick += SBTransition_Tick;
            // 
            // pnMainDisplayRegion
            // 
            pnMainDisplayRegion.BackColor = Color.FromArgb(24, 28, 63);
            pnMainDisplayRegion.Controls.Add(pnMainContainer);
            pnMainDisplayRegion.Controls.Add(pnHeaderTitle);
            pnMainDisplayRegion.Dock = DockStyle.Fill;
            pnMainDisplayRegion.Location = new Point(246, 0);
            pnMainDisplayRegion.Name = "pnMainDisplayRegion";
            pnMainDisplayRegion.Size = new Size(980, 767);
            pnMainDisplayRegion.TabIndex = 2;
            // 
            // pnMainContainer
            // 
            pnMainContainer.Dock = DockStyle.Fill;
            pnMainContainer.Location = new Point(0, 29);
            pnMainContainer.Name = "pnMainContainer";
            pnMainContainer.Size = new Size(980, 738);
            pnMainContainer.TabIndex = 1;
            // 
            // pnHeaderTitle
            // 
            pnHeaderTitle.BackColor = Color.FromArgb(24, 28, 63);
            pnHeaderTitle.Controls.Add(metroControlBox1);
            pnHeaderTitle.Dock = DockStyle.Top;
            pnHeaderTitle.Location = new Point(0, 0);
            pnHeaderTitle.Name = "pnHeaderTitle";
            pnHeaderTitle.Size = new Size(980, 29);
            pnHeaderTitle.TabIndex = 0;
            // 
            // metroControlBox1
            // 
            metroControlBox1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            metroControlBox1.CloseHoverBackColor = Color.FromArgb(183, 40, 40);
            metroControlBox1.CloseHoverForeColor = Color.White;
            metroControlBox1.CloseNormalForeColor = Color.Gray;
            metroControlBox1.DefaultLocation = ReaLTaiizor.Enum.Metro.LocationType.Normal;
            metroControlBox1.DisabledForeColor = Color.DimGray;
            metroControlBox1.IsDerivedStyle = true;
            metroControlBox1.Location = new Point(880, 3);
            metroControlBox1.MaximizeBox = true;
            metroControlBox1.MaximizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroControlBox1.MaximizeHoverForeColor = Color.Gray;
            metroControlBox1.MaximizeNormalForeColor = Color.Gray;
            metroControlBox1.MinimizeBox = true;
            metroControlBox1.MinimizeHoverBackColor = Color.FromArgb(238, 238, 238);
            metroControlBox1.MinimizeHoverForeColor = Color.Gray;
            metroControlBox1.MinimizeNormalForeColor = Color.Gray;
            metroControlBox1.Name = "metroControlBox1";
            metroControlBox1.Size = new Size(100, 25);
            metroControlBox1.Style = ReaLTaiizor.Enum.Metro.Style.Light;
            metroControlBox1.StyleManager = null;
            metroControlBox1.TabIndex = 0;
            metroControlBox1.Text = "metroControlBox1";
            metroControlBox1.ThemeAuthor = "Taiizor";
            metroControlBox1.ThemeName = "MetroLight";
            // 
            // FrmBaseMdiWithSidePanel
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1226, 767);
            Controls.Add(pnMainDisplayRegion);
            Controls.Add(fpnSideBar);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(1226, 767);
            Name = "FrmBaseMdiWithSidePanel";
            Text = "Form1";
            Load += Form1_Load;
            fpnSideBar.ResumeLayout(false);
            pnSBHeader.ResumeLayout(false);
            pnSBHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picSideBarLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSideBarIcon).EndInit();
            pnSBLanding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBlanding).EndInit();
            fpnUserManagementContainer.ResumeLayout(false);
            pnUserManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            pnSBEmployee.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            pnSBCustomer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            pnSBGoods.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            pnSBProduct.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox7).EndInit();
            pnSBSupplier.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            pnSBExit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox8).EndInit();
            pnMainDisplayRegion.ResumeLayout(false);
            pnHeaderTitle.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private FlowLayoutPanel fpnSideBar;
        private Panel pnUserManagement;
        private PictureBox pictureBox3;
        private Button btnSBUser;
        private Panel pnSBEmployee;
        private PictureBox pictureBox5;
        private Button btnSBEmployee;
        private FlowLayoutPanel fpnUserManagementContainer;
        private Panel pnSBLanding;
        private PictureBox iconSBlanding;
        private Button btnSBLanding;
        private Panel pnSBCustomer;
        private PictureBox pictureBox2;
        private Button btnSBCustomer;
        private Panel pnSBGoods;
        private PictureBox pictureBox1;
        private Button btnSBGoods;
        private Panel pnSBSupplier;
        private PictureBox pictureBox6;
        private Button btnSBSupplier;
        private Panel pnSBProduct;
        private PictureBox pictureBox7;
        private Button btnSBProduct;
        private Panel pnSBExit;
        private PictureBox pictureBox8;
        private Button btnSBExit;
        private System.Windows.Forms.Timer SBUserManagementTransition;
        private System.Windows.Forms.Timer SBTransition;
        private Panel pnSBHeader;
        private PictureBox picSideBarLogo;
        private Label lblPanelSideBarTitle;
        private PictureBox picSideBarIcon;
        private Panel pnMainDisplayRegion;
        private Panel pnMainContainer;
        private Panel pnHeaderTitle;
        private ReaLTaiizor.Controls.MetroControlBox metroControlBox1;
    }
}