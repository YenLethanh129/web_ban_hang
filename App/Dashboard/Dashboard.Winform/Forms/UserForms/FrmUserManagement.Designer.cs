namespace Dashboard.Winform
{
    partial class frmUserManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            mainTableLayout = new TableLayoutPanel();
            headerPanel = new Panel();
            tabButtonsPanel = new Panel();
            btnPermissionsTab = new Button();
            btnRolesTab = new Button();
            btnUsersTab = new Button();
            summaryTableLayout = new TableLayoutPanel();
            panel1 = new Panel();
            pictureBox1 = new PictureBox();
            lblTotalUsers = new Label();
            label2 = new Label();
            panel2 = new Panel();
            pictureBox2 = new PictureBox();
            lblActiveUsers = new Label();
            label4 = new Label();
            panel3 = new Panel();
            pictureBox3 = new PictureBox();
            lblInactiveUsers = new Label();
            label6 = new Label();
            panel4 = new Panel();
            pictureBox4 = new PictureBox();
            lblTotalRoles = new Label();
            label8 = new Label();
            contentTableLayout = new TableLayoutPanel();
            pnlUsers = new Panel();
            usersTableLayout = new TableLayoutPanel();
            usersControlPanel = new Panel();
            btnRefresh = new Button();
            btnResetPassword = new Button();
            btnDeleteUser = new Button();
            btnEditUser = new Button();
            btnAddUser = new Button();
            chkActiveOnly = new CheckBox();
            txtSearch = new TextBox();
            label1 = new Label();
            dgvUsers = new DataGridView();
            pnlRoles = new Panel();
            rolesTableLayout = new TableLayoutPanel();
            rolesLeftPanel = new Panel();
            rolesControlPanel = new Panel();
            btnDeleteRole = new Button();
            btnEditRole = new Button();
            btnAddRole = new Button();
            label3 = new Label();
            dgvRoles = new DataGridView();
            rolesRightPanel = new Panel();
            rolePermissionsTableLayout = new TableLayoutPanel();
            rolePermissionsControlPanel = new Panel();
            btnSaveRolePermissions = new Button();
            label5 = new Label();
            dgvRolePermissions = new DataGridView();
            pnlPermissions = new Panel();
            permissionsTableLayout = new TableLayoutPanel();
            permissionsControlPanel = new Panel();
            label7 = new Label();
            dgvPermissions = new DataGridView();
            mainTableLayout.SuspendLayout();
            headerPanel.SuspendLayout();
            tabButtonsPanel.SuspendLayout();
            summaryTableLayout.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            contentTableLayout.SuspendLayout();
            pnlUsers.SuspendLayout();
            usersTableLayout.SuspendLayout();
            usersControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            pnlRoles.SuspendLayout();
            rolesTableLayout.SuspendLayout();
            rolesLeftPanel.SuspendLayout();
            rolesControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            rolesRightPanel.SuspendLayout();
            rolePermissionsTableLayout.SuspendLayout();
            rolePermissionsControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRolePermissions).BeginInit();
            pnlPermissions.SuspendLayout();
            permissionsTableLayout.SuspendLayout();
            permissionsControlPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPermissions).BeginInit();
            SuspendLayout();
            // 
            // mainTableLayout
            // 
            mainTableLayout.ColumnCount = 1;
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTableLayout.Controls.Add(headerPanel, 0, 0);
            mainTableLayout.Controls.Add(summaryTableLayout, 0, 1);
            mainTableLayout.Controls.Add(contentTableLayout, 0, 2);
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Location = new Point(0, 0);
            mainTableLayout.Margin = new Padding(0);
            mainTableLayout.MinimumSize = new Size(980, 640);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.Padding = new Padding(10, 0, 10, 10);
            mainTableLayout.RowCount = 3;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTableLayout.Size = new Size(980, 692);
            mainTableLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.Controls.Add(tabButtonsPanel);
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.Location = new Point(10, 0);
            headerPanel.Margin = new Padding(0);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(960, 50);
            headerPanel.TabIndex = 0;
            // 
            // tabButtonsPanel
            // 
            tabButtonsPanel.Controls.Add(btnPermissionsTab);
            tabButtonsPanel.Controls.Add(btnRolesTab);
            tabButtonsPanel.Controls.Add(btnUsersTab);
            tabButtonsPanel.Dock = DockStyle.Left;
            tabButtonsPanel.Location = new Point(0, 0);
            tabButtonsPanel.Name = "tabButtonsPanel";
            tabButtonsPanel.Size = new Size(400, 50);
            tabButtonsPanel.TabIndex = 0;
            // 
            // btnPermissionsTab
            // 
            btnPermissionsTab.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnPermissionsTab.FlatStyle = FlatStyle.Flat;
            btnPermissionsTab.Font = new Font("Microsoft Sans Serif", 11F);
            btnPermissionsTab.ForeColor = Color.FromArgb(124, 141, 181);
            btnPermissionsTab.Location = new Point(222, 10);
            btnPermissionsTab.Name = "btnPermissionsTab";
            btnPermissionsTab.Size = new Size(105, 30);
            btnPermissionsTab.TabIndex = 2;
            btnPermissionsTab.Text = "Quyền hạn";
            btnPermissionsTab.UseVisualStyleBackColor = true;
            // 
            // btnRolesTab
            // 
            btnRolesTab.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnRolesTab.FlatStyle = FlatStyle.Flat;
            btnRolesTab.Font = new Font("Microsoft Sans Serif", 11F);
            btnRolesTab.ForeColor = Color.FromArgb(124, 141, 181);
            btnRolesTab.Location = new Point(111, 10);
            btnRolesTab.Name = "btnRolesTab";
            btnRolesTab.Size = new Size(105, 30);
            btnRolesTab.TabIndex = 1;
            btnRolesTab.Text = "Vai trò";
            btnRolesTab.UseVisualStyleBackColor = true;
            // 
            // btnUsersTab
            // 
            btnUsersTab.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnUsersTab.FlatStyle = FlatStyle.Flat;
            btnUsersTab.Font = new Font("Microsoft Sans Serif", 11F);
            btnUsersTab.ForeColor = Color.FromArgb(124, 141, 181);
            btnUsersTab.Location = new Point(0, 10);
            btnUsersTab.Name = "btnUsersTab";
            btnUsersTab.Size = new Size(105, 30);
            btnUsersTab.TabIndex = 0;
            btnUsersTab.Text = "Người dùng";
            btnUsersTab.UseVisualStyleBackColor = true;
            // 
            // summaryTableLayout
            // 
            summaryTableLayout.ColumnCount = 4;
            summaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            summaryTableLayout.Controls.Add(panel1, 0, 0);
            summaryTableLayout.Controls.Add(panel2, 1, 0);
            summaryTableLayout.Controls.Add(panel3, 2, 0);
            summaryTableLayout.Controls.Add(panel4, 3, 0);
            summaryTableLayout.Dock = DockStyle.Fill;
            summaryTableLayout.Location = new Point(10, 50);
            summaryTableLayout.Margin = new Padding(0);
            summaryTableLayout.Name = "summaryTableLayout";
            summaryTableLayout.RowCount = 1;
            summaryTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            summaryTableLayout.Size = new Size(960, 80);
            summaryTableLayout.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(42, 45, 86);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(lblTotalUsers);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(234, 74);
            panel1.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(20, 15);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(40, 40);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // lblTotalUsers
            // 
            lblTotalUsers.AutoSize = true;
            lblTotalUsers.Font = new Font("Microsoft Sans Serif", 13F);
            lblTotalUsers.ForeColor = Color.WhiteSmoke;
            lblTotalUsers.Location = new Point(70, 30);
            lblTotalUsers.Name = "lblTotalUsers";
            lblTotalUsers.Size = new Size(20, 22);
            lblTotalUsers.TabIndex = 1;
            lblTotalUsers.Text = "0";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 10F);
            label2.ForeColor = Color.FromArgb(124, 141, 181);
            label2.Location = new Point(70, 15);
            label2.Name = "label2";
            label2.Size = new Size(116, 17);
            label2.TabIndex = 0;
            label2.Text = "Tổng người dùng";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(42, 45, 86);
            panel2.Controls.Add(pictureBox2);
            panel2.Controls.Add(lblActiveUsers);
            panel2.Controls.Add(label4);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(243, 3);
            panel2.Name = "panel2";
            panel2.Size = new Size(234, 74);
            panel2.TabIndex = 1;
            // 
            // pictureBox2
            // 
            pictureBox2.Location = new Point(20, 15);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(40, 40);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 2;
            pictureBox2.TabStop = false;
            // 
            // lblActiveUsers
            // 
            lblActiveUsers.AutoSize = true;
            lblActiveUsers.Font = new Font("Microsoft Sans Serif", 13F);
            lblActiveUsers.ForeColor = Color.WhiteSmoke;
            lblActiveUsers.Location = new Point(70, 30);
            lblActiveUsers.Name = "lblActiveUsers";
            lblActiveUsers.Size = new Size(20, 22);
            lblActiveUsers.TabIndex = 1;
            lblActiveUsers.Text = "0";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new Font("Microsoft Sans Serif", 10F);
            label4.ForeColor = Color.FromArgb(124, 141, 181);
            label4.Location = new Point(70, 15);
            label4.Name = "label4";
            label4.Size = new Size(110, 17);
            label4.TabIndex = 0;
            label4.Text = "Đang hoạt động";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(42, 45, 86);
            panel3.Controls.Add(pictureBox3);
            panel3.Controls.Add(lblInactiveUsers);
            panel3.Controls.Add(label6);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(483, 3);
            panel3.Name = "panel3";
            panel3.Size = new Size(234, 74);
            panel3.TabIndex = 2;
            // 
            // pictureBox3
            // 
            pictureBox3.Location = new Point(20, 15);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(40, 40);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 2;
            pictureBox3.TabStop = false;
            // 
            // lblInactiveUsers
            // 
            lblInactiveUsers.AutoSize = true;
            lblInactiveUsers.Font = new Font("Microsoft Sans Serif", 13F);
            lblInactiveUsers.ForeColor = Color.WhiteSmoke;
            lblInactiveUsers.Location = new Point(70, 30);
            lblInactiveUsers.Name = "lblInactiveUsers";
            lblInactiveUsers.Size = new Size(20, 22);
            lblInactiveUsers.TabIndex = 1;
            lblInactiveUsers.Text = "0";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 10F);
            label6.ForeColor = Color.FromArgb(124, 141, 181);
            label6.Location = new Point(70, 15);
            label6.Name = "label6";
            label6.Size = new Size(117, 17);
            label6.TabIndex = 0;
            label6.Text = "Không hoạt động";
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(42, 45, 86);
            panel4.Controls.Add(pictureBox4);
            panel4.Controls.Add(lblTotalRoles);
            panel4.Controls.Add(label8);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(723, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(234, 74);
            panel4.TabIndex = 3;
            // 
            // pictureBox4
            // 
            pictureBox4.Location = new Point(20, 15);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(40, 40);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 2;
            pictureBox4.TabStop = false;
            // 
            // lblTotalRoles
            // 
            lblTotalRoles.AutoSize = true;
            lblTotalRoles.Font = new Font("Microsoft Sans Serif", 13F);
            lblTotalRoles.ForeColor = Color.WhiteSmoke;
            lblTotalRoles.Location = new Point(70, 30);
            lblTotalRoles.Name = "lblTotalRoles";
            lblTotalRoles.Size = new Size(20, 22);
            lblTotalRoles.TabIndex = 1;
            lblTotalRoles.Text = "0";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Microsoft Sans Serif", 10F);
            label8.ForeColor = Color.FromArgb(124, 141, 181);
            label8.Location = new Point(70, 15);
            label8.Name = "label8";
            label8.Size = new Size(84, 17);
            label8.TabIndex = 0;
            label8.Text = "Tổng vai trò";
            // 
            // contentTableLayout
            // 
            contentTableLayout.ColumnCount = 1;
            contentTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            contentTableLayout.Controls.Add(pnlUsers, 0, 0);
            contentTableLayout.Controls.Add(pnlRoles, 0, 0);
            contentTableLayout.Controls.Add(pnlPermissions, 0, 0);
            contentTableLayout.Dock = DockStyle.Fill;
            contentTableLayout.Location = new Point(10, 130);
            contentTableLayout.Margin = new Padding(0);
            contentTableLayout.Name = "contentTableLayout";
            contentTableLayout.RowCount = 1;
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            contentTableLayout.Size = new Size(960, 552);
            contentTableLayout.TabIndex = 2;
            // 
            // pnlUsers
            // 
            pnlUsers.Controls.Add(usersTableLayout);
            pnlUsers.Dock = DockStyle.Fill;
            pnlUsers.Location = new Point(0, 532);
            pnlUsers.Margin = new Padding(0);
            pnlUsers.Name = "pnlUsers";
            pnlUsers.Size = new Size(960, 20);
            pnlUsers.TabIndex = 0;
            // 
            // usersTableLayout
            // 
            usersTableLayout.ColumnCount = 1;
            usersTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            usersTableLayout.Controls.Add(usersControlPanel, 0, 0);
            usersTableLayout.Controls.Add(dgvUsers, 0, 1);
            usersTableLayout.Dock = DockStyle.Fill;
            usersTableLayout.Location = new Point(0, 0);
            usersTableLayout.Margin = new Padding(0);
            usersTableLayout.Name = "usersTableLayout";
            usersTableLayout.RowCount = 2;
            usersTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            usersTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            usersTableLayout.Size = new Size(960, 20);
            usersTableLayout.TabIndex = 0;
            // 
            // usersControlPanel
            // 
            usersControlPanel.BackColor = Color.FromArgb(42, 45, 86);
            usersControlPanel.Controls.Add(btnRefresh);
            usersControlPanel.Controls.Add(btnResetPassword);
            usersControlPanel.Controls.Add(btnDeleteUser);
            usersControlPanel.Controls.Add(btnEditUser);
            usersControlPanel.Controls.Add(btnAddUser);
            usersControlPanel.Controls.Add(chkActiveOnly);
            usersControlPanel.Controls.Add(txtSearch);
            usersControlPanel.Controls.Add(label1);
            usersControlPanel.Dock = DockStyle.Fill;
            usersControlPanel.Location = new Point(0, 0);
            usersControlPanel.Margin = new Padding(0);
            usersControlPanel.Name = "usersControlPanel";
            usersControlPanel.Padding = new Padding(10, 5, 10, 5);
            usersControlPanel.Size = new Size(960, 60);
            usersControlPanel.TabIndex = 0;
            // 
            // btnRefresh
            // 
            btnRefresh.Anchor = AnchorStyles.Right;
            btnRefresh.BackColor = Color.FromArgb(107, 83, 255);
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.Font = new Font("Microsoft Sans Serif", 9F);
            btnRefresh.ForeColor = Color.White;
            btnRefresh.Location = new Point(870, 15);
            btnRefresh.Name = "btnRefresh";
            btnRefresh.Size = new Size(80, 30);
            btnRefresh.TabIndex = 7;
            btnRefresh.Text = "Làm mới";
            btnRefresh.UseVisualStyleBackColor = false;
            // 
            // btnResetPassword
            // 
            btnResetPassword.Anchor = AnchorStyles.Right;
            btnResetPassword.BackColor = Color.FromArgb(255, 193, 7);
            btnResetPassword.FlatAppearance.BorderSize = 0;
            btnResetPassword.FlatStyle = FlatStyle.Flat;
            btnResetPassword.Font = new Font("Microsoft Sans Serif", 9F);
            btnResetPassword.ForeColor = Color.White;
            btnResetPassword.Location = new Point(750, 15);
            btnResetPassword.Name = "btnResetPassword";
            btnResetPassword.Size = new Size(110, 30);
            btnResetPassword.TabIndex = 6;
            btnResetPassword.Text = "Reset mật khẩu";
            btnResetPassword.UseVisualStyleBackColor = false;
            // 
            // btnDeleteUser
            // 
            btnDeleteUser.Anchor = AnchorStyles.Right;
            btnDeleteUser.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteUser.FlatAppearance.BorderSize = 0;
            btnDeleteUser.FlatStyle = FlatStyle.Flat;
            btnDeleteUser.Font = new Font("Microsoft Sans Serif", 9F);
            btnDeleteUser.ForeColor = Color.White;
            btnDeleteUser.Location = new Point(680, 15);
            btnDeleteUser.Name = "btnDeleteUser";
            btnDeleteUser.Size = new Size(60, 30);
            btnDeleteUser.TabIndex = 5;
            btnDeleteUser.Text = "Xóa";
            btnDeleteUser.UseVisualStyleBackColor = false;
            // 
            // btnEditUser
            // 
            btnEditUser.Anchor = AnchorStyles.Right;
            btnEditUser.BackColor = Color.FromArgb(40, 167, 69);
            btnEditUser.FlatAppearance.BorderSize = 0;
            btnEditUser.FlatStyle = FlatStyle.Flat;
            btnEditUser.Font = new Font("Microsoft Sans Serif", 9F);
            btnEditUser.ForeColor = Color.White;
            btnEditUser.Location = new Point(610, 15);
            btnEditUser.Name = "btnEditUser";
            btnEditUser.Size = new Size(60, 30);
            btnEditUser.TabIndex = 4;
            btnEditUser.Text = "Sửa";
            btnEditUser.UseVisualStyleBackColor = false;
            // 
            // btnAddUser
            // 
            btnAddUser.Anchor = AnchorStyles.Right;
            btnAddUser.BackColor = Color.FromArgb(0, 123, 255);
            btnAddUser.FlatAppearance.BorderSize = 0;
            btnAddUser.FlatStyle = FlatStyle.Flat;
            btnAddUser.Font = new Font("Microsoft Sans Serif", 9F);
            btnAddUser.ForeColor = Color.White;
            btnAddUser.Location = new Point(540, 15);
            btnAddUser.Name = "btnAddUser";
            btnAddUser.Size = new Size(60, 30);
            btnAddUser.TabIndex = 3;
            btnAddUser.Text = "Thêm";
            btnAddUser.UseVisualStyleBackColor = false;
            // 
            // chkActiveOnly
            // 
            chkActiveOnly.AutoSize = true;
            chkActiveOnly.Checked = true;
            chkActiveOnly.CheckState = CheckState.Checked;
            chkActiveOnly.Font = new Font("Microsoft Sans Serif", 9F);
            chkActiveOnly.ForeColor = Color.FromArgb(124, 141, 181);
            chkActiveOnly.Location = new Point(350, 20);
            chkActiveOnly.Name = "chkActiveOnly";
            chkActiveOnly.Size = new Size(149, 19);
            chkActiveOnly.TabIndex = 2;
            chkActiveOnly.Text = "Chỉ hiển thị đang dùng";
            chkActiveOnly.UseVisualStyleBackColor = true;
            // 
            // txtSearch
            // 
            txtSearch.BackColor = Color.FromArgb(73, 75, 111);
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Microsoft Sans Serif", 10F);
            txtSearch.ForeColor = Color.White;
            txtSearch.Location = new Point(80, 18);
            txtSearch.Name = "txtSearch";
            txtSearch.PlaceholderText = "Tìm kiếm theo tên, SĐT, vai trò...";
            txtSearch.Size = new Size(250, 23);
            txtSearch.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold);
            label1.ForeColor = Color.WhiteSmoke;
            label1.Location = new Point(10, 20);
            label1.Name = "label1";
            label1.Size = new Size(77, 17);
            label1.TabIndex = 0;
            label1.Text = "Tìm kiếm:";
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.Location = new Point(0, 60);
            dgvUsers.Margin = new Padding(0);
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.Size = new Size(960, 1);
            dgvUsers.TabIndex = 1;
            // 
            // pnlRoles
            // 
            pnlRoles.Controls.Add(rolesTableLayout);
            pnlRoles.Dock = DockStyle.Fill;
            pnlRoles.Location = new Point(0, 512);
            pnlRoles.Margin = new Padding(0);
            pnlRoles.Name = "pnlRoles";
            pnlRoles.Size = new Size(960, 20);
            pnlRoles.TabIndex = 1;
            pnlRoles.Visible = false;
            // 
            // rolesTableLayout
            // 
            rolesTableLayout.ColumnCount = 2;
            rolesTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            rolesTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            rolesTableLayout.Controls.Add(rolesLeftPanel, 0, 0);
            rolesTableLayout.Controls.Add(rolesRightPanel, 1, 0);
            rolesTableLayout.Dock = DockStyle.Fill;
            rolesTableLayout.Location = new Point(0, 0);
            rolesTableLayout.Margin = new Padding(0);
            rolesTableLayout.Name = "rolesTableLayout";
            rolesTableLayout.RowCount = 1;
            rolesTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rolesTableLayout.Size = new Size(960, 20);
            rolesTableLayout.TabIndex = 0;
            // 
            // rolesLeftPanel
            // 
            rolesLeftPanel.Controls.Add(rolesControlPanel);
            rolesLeftPanel.Controls.Add(dgvRoles);
            rolesLeftPanel.Dock = DockStyle.Fill;
            rolesLeftPanel.Location = new Point(0, 0);
            rolesLeftPanel.Margin = new Padding(0, 0, 5, 0);
            rolesLeftPanel.Name = "rolesLeftPanel";
            rolesLeftPanel.Size = new Size(475, 20);
            rolesLeftPanel.TabIndex = 0;
            // 
            // rolesControlPanel
            // 
            rolesControlPanel.BackColor = Color.FromArgb(42, 45, 86);
            rolesControlPanel.Controls.Add(btnDeleteRole);
            rolesControlPanel.Controls.Add(btnEditRole);
            rolesControlPanel.Controls.Add(btnAddRole);
            rolesControlPanel.Controls.Add(label3);
            rolesControlPanel.Dock = DockStyle.Top;
            rolesControlPanel.Location = new Point(0, 0);
            rolesControlPanel.Margin = new Padding(0);
            rolesControlPanel.Name = "rolesControlPanel";
            rolesControlPanel.Padding = new Padding(10, 5, 10, 5);
            rolesControlPanel.Size = new Size(475, 60);
            rolesControlPanel.TabIndex = 0;
            // 
            // btnDeleteRole
            // 
            btnDeleteRole.Anchor = AnchorStyles.Right;
            btnDeleteRole.BackColor = Color.FromArgb(220, 53, 69);
            btnDeleteRole.FlatAppearance.BorderSize = 0;
            btnDeleteRole.FlatStyle = FlatStyle.Flat;
            btnDeleteRole.Font = new Font("Microsoft Sans Serif", 9F);
            btnDeleteRole.ForeColor = Color.White;
            btnDeleteRole.Location = new Point(405, 15);
            btnDeleteRole.Name = "btnDeleteRole";
            btnDeleteRole.Size = new Size(60, 30);
            btnDeleteRole.TabIndex = 3;
            btnDeleteRole.Text = "Xóa";
            btnDeleteRole.UseVisualStyleBackColor = false;
            // 
            // btnEditRole
            // 
            btnEditRole.Anchor = AnchorStyles.Right;
            btnEditRole.BackColor = Color.FromArgb(40, 167, 69);
            btnEditRole.FlatAppearance.BorderSize = 0;
            btnEditRole.FlatStyle = FlatStyle.Flat;
            btnEditRole.Font = new Font("Microsoft Sans Serif", 9F);
            btnEditRole.ForeColor = Color.White;
            btnEditRole.Location = new Point(335, 15);
            btnEditRole.Name = "btnEditRole";
            btnEditRole.Size = new Size(60, 30);
            btnEditRole.TabIndex = 2;
            btnEditRole.Text = "Sửa";
            btnEditRole.UseVisualStyleBackColor = false;
            // 
            // btnAddRole
            // 
            btnAddRole.Anchor = AnchorStyles.Right;
            btnAddRole.BackColor = Color.FromArgb(0, 123, 255);
            btnAddRole.FlatAppearance.BorderSize = 0;
            btnAddRole.FlatStyle = FlatStyle.Flat;
            btnAddRole.Font = new Font("Microsoft Sans Serif", 9F);
            btnAddRole.ForeColor = Color.White;
            btnAddRole.Location = new Point(265, 15);
            btnAddRole.Name = "btnAddRole";
            btnAddRole.Size = new Size(60, 30);
            btnAddRole.TabIndex = 1;
            btnAddRole.Text = "Thêm";
            btnAddRole.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            label3.ForeColor = Color.WhiteSmoke;
            label3.Location = new Point(10, 20);
            label3.Name = "label3";
            label3.Size = new Size(149, 20);
            label3.TabIndex = 0;
            label3.Text = "Danh sách vai trò";
            // 
            // dgvRoles
            // 
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.AllowUserToDeleteRows = false;
            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRoles.Dock = DockStyle.Fill;
            dgvRoles.Location = new Point(0, 0);
            dgvRoles.Margin = new Padding(0);
            dgvRoles.Name = "dgvRoles";
            dgvRoles.ReadOnly = true;
            dgvRoles.Size = new Size(475, 20);
            dgvRoles.TabIndex = 1;
            // 
            // rolesRightPanel
            // 
            rolesRightPanel.Controls.Add(rolePermissionsTableLayout);
            rolesRightPanel.Dock = DockStyle.Fill;
            rolesRightPanel.Location = new Point(485, 0);
            rolesRightPanel.Margin = new Padding(5, 0, 0, 0);
            rolesRightPanel.Name = "rolesRightPanel";
            rolesRightPanel.Size = new Size(475, 20);
            rolesRightPanel.TabIndex = 1;
            // 
            // rolePermissionsTableLayout
            // 
            rolePermissionsTableLayout.ColumnCount = 1;
            rolePermissionsTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rolePermissionsTableLayout.Controls.Add(rolePermissionsControlPanel, 0, 0);
            rolePermissionsTableLayout.Controls.Add(dgvRolePermissions, 0, 1);
            rolePermissionsTableLayout.Dock = DockStyle.Fill;
            rolePermissionsTableLayout.Location = new Point(0, 0);
            rolePermissionsTableLayout.Margin = new Padding(0);
            rolePermissionsTableLayout.Name = "rolePermissionsTableLayout";
            rolePermissionsTableLayout.RowCount = 2;
            rolePermissionsTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            rolePermissionsTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            rolePermissionsTableLayout.Size = new Size(475, 20);
            rolePermissionsTableLayout.TabIndex = 0;
            // 
            // rolePermissionsControlPanel
            // 
            rolePermissionsControlPanel.BackColor = Color.FromArgb(42, 45, 86);
            rolePermissionsControlPanel.Controls.Add(btnSaveRolePermissions);
            rolePermissionsControlPanel.Controls.Add(label5);
            rolePermissionsControlPanel.Dock = DockStyle.Fill;
            rolePermissionsControlPanel.Location = new Point(0, 0);
            rolePermissionsControlPanel.Margin = new Padding(0);
            rolePermissionsControlPanel.Name = "rolePermissionsControlPanel";
            rolePermissionsControlPanel.Padding = new Padding(10, 5, 10, 5);
            rolePermissionsControlPanel.Size = new Size(475, 60);
            rolePermissionsControlPanel.TabIndex = 0;
            // 
            // btnSaveRolePermissions
            // 
            btnSaveRolePermissions.Anchor = AnchorStyles.Right;
            btnSaveRolePermissions.BackColor = Color.FromArgb(40, 167, 69);
            btnSaveRolePermissions.FlatAppearance.BorderSize = 0;
            btnSaveRolePermissions.FlatStyle = FlatStyle.Flat;
            btnSaveRolePermissions.Font = new Font("Microsoft Sans Serif", 9F);
            btnSaveRolePermissions.ForeColor = Color.White;
            btnSaveRolePermissions.Location = new Point(395, 15);
            btnSaveRolePermissions.Name = "btnSaveRolePermissions";
            btnSaveRolePermissions.Size = new Size(70, 30);
            btnSaveRolePermissions.TabIndex = 1;
            btnSaveRolePermissions.Text = "Lưu";
            btnSaveRolePermissions.UseVisualStyleBackColor = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            label5.ForeColor = Color.WhiteSmoke;
            label5.Location = new Point(10, 20);
            label5.Name = "label5";
            label5.Size = new Size(183, 20);
            label5.TabIndex = 0;
            label5.Text = "Quyền hạn của vai trò";
            // 
            // dgvRolePermissions
            // 
            dgvRolePermissions.AllowUserToAddRows = false;
            dgvRolePermissions.AllowUserToDeleteRows = false;
            dgvRolePermissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRolePermissions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvRolePermissions.Dock = DockStyle.Fill;
            dgvRolePermissions.Location = new Point(0, 60);
            dgvRolePermissions.Margin = new Padding(0);
            dgvRolePermissions.Name = "dgvRolePermissions";
            dgvRolePermissions.ReadOnly = true;
            dgvRolePermissions.Size = new Size(475, 1);
            dgvRolePermissions.TabIndex = 1;
            // 
            // pnlPermissions
            // 
            pnlPermissions.Controls.Add(permissionsTableLayout);
            pnlPermissions.Dock = DockStyle.Fill;
            pnlPermissions.Location = new Point(0, 0);
            pnlPermissions.Margin = new Padding(0);
            pnlPermissions.Name = "pnlPermissions";
            pnlPermissions.Size = new Size(960, 512);
            pnlPermissions.TabIndex = 2;
            pnlPermissions.Visible = false;
            // 
            // permissionsTableLayout
            // 
            permissionsTableLayout.ColumnCount = 1;
            permissionsTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            permissionsTableLayout.Controls.Add(permissionsControlPanel, 0, 0);
            permissionsTableLayout.Controls.Add(dgvPermissions, 0, 1);
            permissionsTableLayout.Dock = DockStyle.Fill;
            permissionsTableLayout.Location = new Point(0, 0);
            permissionsTableLayout.Margin = new Padding(0);
            permissionsTableLayout.Name = "permissionsTableLayout";
            permissionsTableLayout.RowCount = 2;
            permissionsTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            permissionsTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            permissionsTableLayout.Size = new Size(960, 512);
            permissionsTableLayout.TabIndex = 0;
            // 
            // permissionsControlPanel
            // 
            permissionsControlPanel.BackColor = Color.FromArgb(42, 45, 86);
            permissionsControlPanel.Controls.Add(label7);
            permissionsControlPanel.Dock = DockStyle.Fill;
            permissionsControlPanel.Location = new Point(0, 0);
            permissionsControlPanel.Margin = new Padding(0);
            permissionsControlPanel.Name = "permissionsControlPanel";
            permissionsControlPanel.Padding = new Padding(10, 5, 10, 5);
            permissionsControlPanel.Size = new Size(960, 60);
            permissionsControlPanel.TabIndex = 0;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            label7.ForeColor = Color.WhiteSmoke;
            label7.Location = new Point(10, 20);
            label7.Name = "label7";
            label7.Size = new Size(183, 20);
            label7.TabIndex = 0;
            label7.Text = "Danh sách quyền hạn";
            // 
            // dgvPermissions
            // 
            dgvPermissions.AllowUserToAddRows = false;
            dgvPermissions.AllowUserToDeleteRows = false;
            dgvPermissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPermissions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvPermissions.Dock = DockStyle.Fill;
            dgvPermissions.Location = new Point(0, 60);
            dgvPermissions.Margin = new Padding(0);
            dgvPermissions.Name = "dgvPermissions";
            dgvPermissions.ReadOnly = true;
            dgvPermissions.Size = new Size(960, 452);
            dgvPermissions.TabIndex = 1;
            // 
            // frmUserManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(980, 692);
            Controls.Add(mainTableLayout);
            Font = new Font("Microsoft Sans Serif", 9F);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(980, 640);
            Name = "frmUserManagement";
            Text = "Quản lý người dùng";
            Load += FrmUserManagement_Load;
            mainTableLayout.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            tabButtonsPanel.ResumeLayout(false);
            summaryTableLayout.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            contentTableLayout.ResumeLayout(false);
            pnlUsers.ResumeLayout(false);
            usersTableLayout.ResumeLayout(false);
            usersControlPanel.ResumeLayout(false);
            usersControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            pnlRoles.ResumeLayout(false);
            rolesTableLayout.ResumeLayout(false);
            rolesLeftPanel.ResumeLayout(false);
            rolesControlPanel.ResumeLayout(false);
            rolesControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            rolesRightPanel.ResumeLayout(false);
            rolePermissionsTableLayout.ResumeLayout(false);
            rolePermissionsControlPanel.ResumeLayout(false);
            rolePermissionsControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRolePermissions).EndInit();
            pnlPermissions.ResumeLayout(false);
            permissionsTableLayout.ResumeLayout(false);
            permissionsControlPanel.ResumeLayout(false);
            permissionsControlPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvPermissions).EndInit();
            ResumeLayout(false);
        }

        #endregion

        // Main layout controls
        private TableLayoutPanel mainTableLayout;
        private Panel headerPanel;
        private Panel tabButtonsPanel;
        private TableLayoutPanel summaryTableLayout;
        private TableLayoutPanel contentTableLayout;

        // Tab buttons
        private Button btnUsersTab;
        private Button btnRolesTab;
        private Button btnPermissionsTab;

        // Summary panels
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private PictureBox pictureBox3;
        private PictureBox pictureBox4;
        private Label lblTotalUsers;
        private Label lblActiveUsers;
        private Label lblInactiveUsers;
        private Label lblTotalRoles;
        private Label label2;
        private Label label4;
        private Label label6;
        private Label label8;

        // Users panel
        private Panel pnlUsers;
        private TableLayoutPanel usersTableLayout;
        private Panel usersControlPanel;
        private DataGridView dgvUsers;
        private Label label1;
        private TextBox txtSearch;
        private CheckBox chkActiveOnly;
        private Button btnAddUser;
        private Button btnEditUser;
        private Button btnDeleteUser;
        private Button btnResetPassword;
        private Button btnRefresh;

        // Roles panel
        private Panel pnlRoles;
        private TableLayoutPanel rolesTableLayout;
        private Panel rolesLeftPanel;
        private Panel rolesRightPanel;
        private Panel rolesControlPanel;
        private Panel rolePermissionsControlPanel;
        private TableLayoutPanel rolePermissionsTableLayout;
        private DataGridView dgvRoles;
        private DataGridView dgvRolePermissions;
        private Label label3;
        private Label label5;
        private Button btnAddRole;
        private Button btnEditRole;
        private Button btnDeleteRole;
        private Button btnSaveRolePermissions;

        // Permissions panel
        private Panel pnlPermissions;
        private TableLayoutPanel permissionsTableLayout;
        private Panel permissionsControlPanel;
        private DataGridView dgvPermissions;
        private Label label7;
    }
}