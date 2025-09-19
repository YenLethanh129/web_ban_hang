using Microsoft.Extensions.Logging;

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
            if (disposing)
            {
                try
                {
                    // Dispose custom resources
                    _blurLoadingOverlay?.Dispose();
                    activeForm?.Dispose();
                    _loadingStopwatch?.Stop();

                    // Dispose components
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error during disposal");
                    
                    // Still dispose components even if custom disposal fails
                    if (components != null)
                    {
                        components.Dispose();
                    }
                }
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
            iconSBUser = new PictureBox();
            btnSBUser = new Button();
            pnSBEmployee = new Panel();
            iconSBEmployee = new PictureBox();
            btnSBEmployee = new Button();
            pnSBCustomer = new Panel();
            iconSBCustomer = new PictureBox();
            btnSBAccount = new Button();
            pnSBGoods = new Panel();
            iconSBGoods = new PictureBox();
            btnSBGoods = new Button();
            pnSBProduct = new Panel();
            iconSBProduct = new PictureBox();
            btnSBProduct = new Button();
            pnSBSupplier = new Panel();
            iconSBSupplier = new PictureBox();
            btnSBSupplier = new Button();
            pnSBExit = new Panel();
            iconSBExit = new PictureBox();
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
            ((System.ComponentModel.ISupportInitialize)iconSBUser).BeginInit();
            pnSBEmployee.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSBEmployee).BeginInit();
            pnSBCustomer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSBCustomer).BeginInit();
            pnSBGoods.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSBGoods).BeginInit();
            pnSBProduct.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSBProduct).BeginInit();
            pnSBSupplier.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSBSupplier).BeginInit();
            pnSBExit.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)iconSBExit).BeginInit();
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
            fpnSideBar.Size = new Size(245, 767);
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
            pnUserManagement.Controls.Add(iconSBUser);
            pnUserManagement.Controls.Add(btnSBUser);
            pnUserManagement.Location = new Point(0, 0);
            pnUserManagement.Margin = new Padding(0);
            pnUserManagement.Name = "pnUserManagement";
            pnUserManagement.Size = new Size(246, 50);
            pnUserManagement.TabIndex = 4;
            // 
            // iconSBUser
            // 
            iconSBUser.BackColor = Color.FromArgb(54, 58, 105);
            iconSBUser.Image = (Image)resources.GetObject("iconSBUser.Image");
            iconSBUser.Location = new Point(14, 16);
            iconSBUser.Name = "iconSBUser";
            iconSBUser.Size = new Size(28, 25);
            iconSBUser.SizeMode = PictureBoxSizeMode.StretchImage;
            iconSBUser.TabIndex = 3;
            iconSBUser.TabStop = false;
            // 
            // btnSBUser
            // 
            btnSBUser.Anchor = AnchorStyles.None;
            btnSBUser.BackColor = Color.FromArgb(54, 58, 105);
            btnSBUser.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBUser.ForeColor = Color.Gainsboro;
            btnSBUser.Location = new Point(-110, -10);
            btnSBUser.Name = "btnSBUser";
            btnSBUser.Size = new Size(400, 70);
            btnSBUser.TabIndex = 2;
            btnSBUser.Text = "Nhân sự";
            btnSBUser.UseVisualStyleBackColor = false;
            btnSBUser.Click += OpenUserManagementContainer;
            // 
            // pnSBEmployee
            // 
            pnSBEmployee.Controls.Add(iconSBEmployee);
            pnSBEmployee.Controls.Add(btnSBEmployee);
            pnSBEmployee.Location = new Point(0, 50);
            pnSBEmployee.Margin = new Padding(0);
            pnSBEmployee.Name = "pnSBEmployee";
            pnSBEmployee.Size = new Size(246, 50);
            pnSBEmployee.TabIndex = 6;
            // 
            // iconSBEmployee
            // 
            iconSBEmployee.BackColor = Color.FromArgb(54, 58, 105);
            iconSBEmployee.Image = (Image)resources.GetObject("iconSBEmployee.Image");
            iconSBEmployee.Location = new Point(44, 16);
            iconSBEmployee.Name = "iconSBEmployee";
            iconSBEmployee.Size = new Size(28, 25);
            iconSBEmployee.SizeMode = PictureBoxSizeMode.StretchImage;
            iconSBEmployee.TabIndex = 3;
            iconSBEmployee.TabStop = false;
            // 
            // btnSBEmployee
            // 
            btnSBEmployee.BackColor = Color.FromArgb(54, 58, 105);
            btnSBEmployee.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBEmployee.ForeColor = Color.Gainsboro;
            btnSBEmployee.Location = new Point(-48, -7);
            btnSBEmployee.Name = "btnSBEmployee";
            btnSBEmployee.Size = new Size(336, 70);
            btnSBEmployee.TabIndex = 2;
            btnSBEmployee.Text = "Nhân sự";
            btnSBEmployee.UseVisualStyleBackColor = false;
            // 
            // pnSBCustomer
            // 
            pnSBCustomer.Controls.Add(iconSBCustomer);
            pnSBCustomer.Controls.Add(btnSBAccount);
            pnSBCustomer.Location = new Point(0, 100);
            pnSBCustomer.Margin = new Padding(0);
            pnSBCustomer.Name = "pnSBCustomer";
            pnSBCustomer.Size = new Size(246, 50);
            pnSBCustomer.TabIndex = 7;
            // 
            // iconSBCustomer
            // 
            iconSBCustomer.BackColor = Color.FromArgb(54, 58, 105);
            iconSBCustomer.Image = (Image)resources.GetObject("iconSBCustomer.Image");
            iconSBCustomer.Location = new Point(44, 16);
            iconSBCustomer.Name = "iconSBCustomer";
            iconSBCustomer.Size = new Size(28, 25);
            iconSBCustomer.SizeMode = PictureBoxSizeMode.StretchImage;
            iconSBCustomer.TabIndex = 3;
            iconSBCustomer.TabStop = false;
            // 
            // btnSBAccount
            // 
            btnSBAccount.BackColor = Color.FromArgb(54, 58, 105);
            btnSBAccount.Font = new Font("Microsoft Sans Serif", 11F);
            btnSBAccount.ForeColor = Color.Gainsboro;
            btnSBAccount.Location = new Point(-66, -7);
            btnSBAccount.Name = "btnSBAccount";
            btnSBAccount.Size = new Size(384, 70);
            btnSBAccount.TabIndex = 2;
            btnSBAccount.Text = "Tài khoản";
            btnSBAccount.UseVisualStyleBackColor = false;
            // 
            // pnSBGoods
            // 
            pnSBGoods.Controls.Add(iconSBGoods);
            pnSBGoods.Controls.Add(btnSBGoods);
            pnSBGoods.Location = new Point(0, 206);
            pnSBGoods.Margin = new Padding(0);
            pnSBGoods.Name = "pnSBGoods";
            pnSBGoods.Size = new Size(246, 50);
            pnSBGoods.TabIndex = 7;
            // 
            // iconSBGoods
            // 
            iconSBGoods.BackColor = Color.FromArgb(54, 58, 105);
            iconSBGoods.Image = (Image)resources.GetObject("iconSBGoods.Image");
            iconSBGoods.Location = new Point(14, 16);
            iconSBGoods.Name = "iconSBGoods";
            iconSBGoods.Size = new Size(28, 25);
            iconSBGoods.SizeMode = PictureBoxSizeMode.StretchImage;
            iconSBGoods.TabIndex = 3;
            iconSBGoods.TabStop = false;
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
            pnSBProduct.Controls.Add(iconSBProduct);
            pnSBProduct.Controls.Add(btnSBProduct);
            pnSBProduct.Location = new Point(0, 256);
            pnSBProduct.Margin = new Padding(0);
            pnSBProduct.Name = "pnSBProduct";
            pnSBProduct.Size = new Size(246, 50);
            pnSBProduct.TabIndex = 9;
            // 
            // iconSBProduct
            // 
            iconSBProduct.BackColor = Color.FromArgb(54, 58, 105);
            iconSBProduct.Image = (Image)resources.GetObject("iconSBProduct.Image");
            iconSBProduct.Location = new Point(14, 16);
            iconSBProduct.Name = "iconSBProduct";
            iconSBProduct.Size = new Size(28, 25);
            iconSBProduct.SizeMode = PictureBoxSizeMode.StretchImage;
            iconSBProduct.TabIndex = 3;
            iconSBProduct.TabStop = false;
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
            pnSBSupplier.Controls.Add(iconSBSupplier);
            pnSBSupplier.Controls.Add(btnSBSupplier);
            pnSBSupplier.Location = new Point(0, 306);
            pnSBSupplier.Margin = new Padding(0);
            pnSBSupplier.Name = "pnSBSupplier";
            pnSBSupplier.Size = new Size(246, 50);
            pnSBSupplier.TabIndex = 8;
            // 
            // iconSBSupplier
            // 
            iconSBSupplier.BackColor = Color.FromArgb(54, 58, 105);
            iconSBSupplier.Image = (Image)resources.GetObject("iconSBSupplier.Image");
            iconSBSupplier.Location = new Point(15, 16);
            iconSBSupplier.Name = "iconSBSupplier";
            iconSBSupplier.Size = new Size(28, 25);
            iconSBSupplier.SizeMode = PictureBoxSizeMode.StretchImage;
            iconSBSupplier.TabIndex = 3;
            iconSBSupplier.TabStop = false;
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
            pnSBExit.Controls.Add(iconSBExit);
            pnSBExit.Controls.Add(btnSBExit);
            pnSBExit.Location = new Point(0, 356);
            pnSBExit.Margin = new Padding(0);
            pnSBExit.Name = "pnSBExit";
            pnSBExit.Size = new Size(246, 50);
            pnSBExit.TabIndex = 10;
            // 
            // iconSBExit
            // 
            iconSBExit.BackColor = Color.FromArgb(54, 58, 105);
            iconSBExit.Image = (Image)resources.GetObject("iconSBExit.Image");
            iconSBExit.Location = new Point(16, 16);
            iconSBExit.Name = "iconSBExit";
            iconSBExit.Size = new Size(28, 25);
            iconSBExit.SizeMode = PictureBoxSizeMode.StretchImage;
            iconSBExit.TabIndex = 3;
            iconSBExit.TabStop = false;
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
            pnMainDisplayRegion.Location = new Point(245, 0);
            pnMainDisplayRegion.Name = "pnMainDisplayRegion";
            pnMainDisplayRegion.Size = new Size(995, 767);
            pnMainDisplayRegion.TabIndex = 2;
            // 
            // pnMainContainer
            // 
            pnMainContainer.Dock = DockStyle.Fill;
            pnMainContainer.Location = new Point(0, 29);
            pnMainContainer.Name = "pnMainContainer";
            pnMainContainer.Size = new Size(995, 738);
            pnMainContainer.TabIndex = 1;
            // 
            // pnHeaderTitle
            // 
            pnHeaderTitle.BackColor = Color.FromArgb(24, 28, 63);
            pnHeaderTitle.Controls.Add(metroControlBox1);
            pnHeaderTitle.Dock = DockStyle.Top;
            pnHeaderTitle.Location = new Point(0, 0);
            pnHeaderTitle.Name = "pnHeaderTitle";
            pnHeaderTitle.Size = new Size(995, 29);
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
            metroControlBox1.Location = new Point(895, 3);
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
            ClientSize = new Size(1240, 767);
            Controls.Add(pnMainDisplayRegion);
            Controls.Add(fpnSideBar);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            IsMdiContainer = true;
            MinimumSize = new Size(1240, 764);
            Name = "FrmBaseMdiWithSidePanel";
            Text = "Form1";
            Load += FrmBaseManagement_Load;
            fpnSideBar.ResumeLayout(false);
            pnSBHeader.ResumeLayout(false);
            pnSBHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picSideBarLogo).EndInit();
            ((System.ComponentModel.ISupportInitialize)picSideBarIcon).EndInit();
            pnSBLanding.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBlanding).EndInit();
            fpnUserManagementContainer.ResumeLayout(false);
            pnUserManagement.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBUser).EndInit();
            pnSBEmployee.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBEmployee).EndInit();
            pnSBCustomer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBCustomer).EndInit();
            pnSBGoods.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBGoods).EndInit();
            pnSBProduct.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBProduct).EndInit();
            pnSBSupplier.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBSupplier).EndInit();
            pnSBExit.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)iconSBExit).EndInit();
            pnMainDisplayRegion.ResumeLayout(false);
            pnHeaderTitle.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private FlowLayoutPanel fpnSideBar;
        private Panel pnUserManagement;
        private PictureBox iconSBUser;
        private Button btnSBUser;
        private Panel pnSBEmployee;
        private PictureBox iconSBEmployee;
        private Button btnSBEmployee;
        private FlowLayoutPanel fpnUserManagementContainer;
        private Panel pnSBLanding;
        private PictureBox iconSBlanding;
        private Button btnSBLanding;
        private Panel pnSBCustomer;
        private PictureBox iconSBCustomer;
        private Button btnSBAccount;
        private Panel pnSBGoods;
        private PictureBox iconSBGoods;
        private Button btnSBGoods;
        private Panel pnSBSupplier;
        private PictureBox iconSBSupplier;
        private Button btnSBSupplier;
        private Panel pnSBProduct;
        private PictureBox iconSBProduct;
        private Button btnSBProduct;
        private Panel pnSBExit;
        private PictureBox iconSBExit;
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