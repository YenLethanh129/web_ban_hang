namespace Dashboard.Winform.Forms
{
    partial class FrmUserManagement
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            tabControl = new TabControl();
            tabUserList = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlHeader = new TableLayoutPanel();
            panel1 = new Panel();
            btnFilter1 = new Button();
            lblFilter1 = new Label();
            cbxFilter1 = new ComboBox();
            panel2 = new Panel();
            btnFilter2 = new Button();
            lblFilter2 = new Label();
            cbxFilter2 = new ComboBox();
            panel3 = new Panel();
            tbxFindString = new TextBox();
            lblSearchString = new Label();
            btnGetDetails = new Button();
            btnAdd = new Button();
            pnlContent = new Panel();
            dgvUsers = new DataGridView();
            pnlPagination = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel4 = new Panel();
            lblNumberOfRecords = new Label();
            lblShowNumberPerPage = new Label();
            cbxPageSize = new ComboBox();
            panel5 = new Panel();
            btnPrevious = new Button();
            btnNext = new Button();
            lblShowingAtPage = new Label();
            tabUserDetail = new TabPage();
            pnlUserDetail = new Panel();
            groupBoxUserInfo = new GroupBox();
            tableLayoutPanel3 = new TableLayoutPanel();
            lblUserId = new Label();
            tbxUserId = new TextBox();
            lblFullName = new Label();
            tbxUserName = new TextBox();
            lblPassword = new Label();
            tbxPassword = new TextBox();
            lblEmployee = new Label();
            cbxEmployee = new ComboBox();
            lblUserRole = new Label();
            cbxUserRole = new ComboBox();
            lblIsActive = new Label();
            chkIsActive = new CheckBox();
            pnlUserDetailButtons = new Panel();
            btnSaveUser = new Button();
            btnCancelUser = new Button();
            lblUserDetailTitle = new Label();
            tabRoleAssignment = new TabPage();
            pnlRoleAssignment = new Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            groupBoxAvailableRoles = new GroupBox();
            lstAvailableRoles = new ListBox();
            pnlRoleButtons = new Panel();
            btnAssignRole = new Button();
            btnRemoveRole = new Button();
            groupBoxUserRoles = new GroupBox();
            lstUserRoles = new ListBox();
            lblRoleAssignmentTitle = new Label();
            lblCurrentUserId = new Label();
            btnUpdate = new Button();
            tabControl.SuspendLayout();
            tabUserList.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            pnlHeader.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            pnlContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUsers).BeginInit();
            pnlPagination.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            tabUserDetail.SuspendLayout();
            pnlUserDetail.SuspendLayout();
            groupBoxUserInfo.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            pnlUserDetailButtons.SuspendLayout();
            tabRoleAssignment.SuspendLayout();
            pnlRoleAssignment.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            groupBoxAvailableRoles.SuspendLayout();
            pnlRoleButtons.SuspendLayout();
            groupBoxUserRoles.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabUserList);
            tabControl.Controls.Add(tabUserDetail);
            tabControl.Controls.Add(tabRoleAssignment);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(10, 10);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(980, 620);
            tabControl.TabIndex = 0;
            // 
            // tabUserList
            // 
            tabUserList.BackColor = Color.FromArgb(24, 28, 63);
            tabUserList.Controls.Add(tableLayoutPanel1);
            tabUserList.Location = new Point(4, 24);
            tabUserList.Name = "tabUserList";
            tabUserList.Padding = new Padding(3);
            tabUserList.Size = new Size(972, 592);
            tabUserList.TabIndex = 0;
            tabUserList.Text = "Danh sách người dùng";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(24, 28, 63);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(pnlHeader, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlContent, 0, 1);
            tableLayoutPanel1.Controls.Add(pnlPagination, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(966, 586);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlHeader
            // 
            pnlHeader.ColumnCount = 6;
            pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            pnlHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            pnlHeader.Controls.Add(panel1, 0, 0);
            pnlHeader.Controls.Add(panel2, 1, 0);
            pnlHeader.Controls.Add(panel3, 2, 0);
            pnlHeader.Controls.Add(btnGetDetails, 3, 0);
            pnlHeader.Controls.Add(btnAdd, 4, 0);
            pnlHeader.Controls.Add(btnUpdate, 5, 0);
            pnlHeader.Dock = DockStyle.Fill;
            pnlHeader.Location = new Point(3, 3);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.RowCount = 1;
            pnlHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlHeader.Size = new Size(960, 58);
            pnlHeader.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnFilter1);
            panel1.Controls.Add(lblFilter1);
            panel1.Controls.Add(cbxFilter1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(5, 5);
            panel1.Margin = new Padding(5);
            panel1.Name = "panel1";
            panel1.Size = new Size(136, 48);
            panel1.TabIndex = 3;
            // 
            // btnFilter1
            // 
            btnFilter1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnFilter1.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnFilter1.FlatStyle = FlatStyle.Flat;
            btnFilter1.Font = new Font("Microsoft Sans Serif", 9F);
            btnFilter1.ForeColor = Color.FromArgb(192, 255, 192);
            btnFilter1.Location = new Point(0, 24);
            btnFilter1.Name = "btnFilter1";
            btnFilter1.Size = new Size(136, 23);
            btnFilter1.TabIndex = 0;
            btnFilter1.Text = "Vai trò";
            btnFilter1.TextAlign = ContentAlignment.TopLeft;
            btnFilter1.UseVisualStyleBackColor = true;
            // 
            // lblFilter1
            // 
            lblFilter1.AutoSize = true;
            lblFilter1.ForeColor = Color.FromArgb(124, 141, 181);
            lblFilter1.Location = new Point(0, 3);
            lblFilter1.Name = "lblFilter1";
            lblFilter1.Size = new Size(43, 15);
            lblFilter1.TabIndex = 1;
            lblFilter1.Text = "Vai trò:";
            // 
            // cbxFilter1
            // 
            cbxFilter1.BackColor = Color.FromArgb(42, 45, 86);
            cbxFilter1.Dock = DockStyle.Bottom;
            cbxFilter1.FlatStyle = FlatStyle.Flat;
            cbxFilter1.ForeColor = Color.WhiteSmoke;
            cbxFilter1.FormattingEnabled = true;
            cbxFilter1.Location = new Point(0, 25);
            cbxFilter1.Name = "cbxFilter1";
            cbxFilter1.Size = new Size(136, 23);
            cbxFilter1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnFilter2);
            panel2.Controls.Add(lblFilter2);
            panel2.Controls.Add(cbxFilter2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(151, 5);
            panel2.Margin = new Padding(5);
            panel2.Name = "panel2";
            panel2.Size = new Size(136, 48);
            panel2.TabIndex = 4;
            // 
            // btnFilter2
            // 
            btnFilter2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnFilter2.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnFilter2.FlatStyle = FlatStyle.Flat;
            btnFilter2.Font = new Font("Microsoft Sans Serif", 9F);
            btnFilter2.ForeColor = Color.FromArgb(192, 255, 192);
            btnFilter2.Location = new Point(0, 24);
            btnFilter2.Name = "btnFilter2";
            btnFilter2.Size = new Size(136, 23);
            btnFilter2.TabIndex = 0;
            btnFilter2.Text = "Trạng thái";
            btnFilter2.TextAlign = ContentAlignment.TopLeft;
            btnFilter2.UseVisualStyleBackColor = true;
            // 
            // lblFilter2
            // 
            lblFilter2.AutoSize = true;
            lblFilter2.ForeColor = Color.FromArgb(124, 141, 181);
            lblFilter2.Location = new Point(0, 3);
            lblFilter2.Name = "lblFilter2";
            lblFilter2.Size = new Size(63, 15);
            lblFilter2.TabIndex = 1;
            lblFilter2.Text = "Trạng thái:";
            // 
            // cbxFilter2
            // 
            cbxFilter2.BackColor = Color.FromArgb(42, 45, 86);
            cbxFilter2.Dock = DockStyle.Bottom;
            cbxFilter2.FlatStyle = FlatStyle.Flat;
            cbxFilter2.ForeColor = Color.WhiteSmoke;
            cbxFilter2.FormattingEnabled = true;
            cbxFilter2.Location = new Point(0, 25);
            cbxFilter2.Name = "cbxFilter2";
            cbxFilter2.Size = new Size(136, 23);
            cbxFilter2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(tbxFindString);
            panel3.Controls.Add(lblSearchString);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(297, 5);
            panel3.Margin = new Padding(5);
            panel3.Name = "panel3";
            panel3.Padding = new Padding(5, 0, 5, 5);
            panel3.Size = new Size(364, 48);
            panel3.TabIndex = 5;
            // 
            // tbxFindString
            // 
            tbxFindString.BackColor = Color.FromArgb(42, 45, 86);
            tbxFindString.BorderStyle = BorderStyle.None;
            tbxFindString.Dock = DockStyle.Bottom;
            tbxFindString.ForeColor = Color.FromArgb(192, 255, 192);
            tbxFindString.Location = new Point(5, 27);
            tbxFindString.Name = "tbxFindString";
            tbxFindString.PlaceholderText = "ID/SĐT/Tên cần tìm";
            tbxFindString.Size = new Size(354, 16);
            tbxFindString.TabIndex = 0;
            // 
            // lblSearchString
            // 
            lblSearchString.AutoSize = true;
            lblSearchString.ForeColor = Color.FromArgb(124, 141, 181);
            lblSearchString.Location = new Point(1, 3);
            lblSearchString.Name = "lblSearchString";
            lblSearchString.Size = new Size(159, 15);
            lblSearchString.TabIndex = 1;
            lblSearchString.Text = "Tìm kiếm theo (ID/SĐT/Tên):";
            // 
            // btnGetDetails
            // 
            btnGetDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnGetDetails.FlatAppearance.BorderColor = Color.Fuchsia;
            btnGetDetails.FlatStyle = FlatStyle.Flat;
            btnGetDetails.Font = new Font("Microsoft Sans Serif", 11F);
            btnGetDetails.ForeColor = Color.FromArgb(192, 255, 192);
            btnGetDetails.Location = new Point(671, 5);
            btnGetDetails.Margin = new Padding(5);
            btnGetDetails.Name = "btnGetDetails";
            btnGetDetails.Size = new Size(88, 48);
            btnGetDetails.TabIndex = 0;
            btnGetDetails.Text = "Chi tiết";
            btnGetDetails.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.FlatAppearance.BorderColor = Color.Cyan;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Microsoft Sans Serif", 11F);
            btnAdd.ForeColor = Color.FromArgb(255, 224, 192);
            btnAdd.Location = new Point(769, 5);
            btnAdd.Margin = new Padding(5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(88, 48);
            btnAdd.TabIndex = 1;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.FromArgb(42, 45, 86);
            pnlContent.Controls.Add(dgvUsers);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(3, 67);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(960, 476);
            pnlContent.TabIndex = 1;
            // 
            // dgvUsers
            // 
            dgvUsers.AllowUserToAddRows = false;
            dgvUsers.AllowUserToDeleteRows = false;
            dgvUsers.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(42, 45, 86);
            dgvUsers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvUsers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsers.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvUsers.BorderStyle = BorderStyle.None;
            dgvUsers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUsers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvUsers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvUsers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvUsers.DefaultCellStyle = dataGridViewCellStyle3;
            dgvUsers.Dock = DockStyle.Fill;
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.GridColor = Color.FromArgb(73, 75, 111);
            dgvUsers.Location = new Point(0, 0);
            dgvUsers.MultiSelect = false;
            dgvUsers.Name = "dgvUsers";
            dgvUsers.ReadOnly = true;
            dgvUsers.RowHeadersVisible = false;
            dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUsers.Size = new Size(960, 476);
            dgvUsers.TabIndex = 0;
            // 
            // pnlPagination
            // 
            pnlPagination.Controls.Add(tableLayoutPanel2);
            pnlPagination.Dock = DockStyle.Fill;
            pnlPagination.Location = new Point(3, 549);
            pnlPagination.Name = "pnlPagination";
            pnlPagination.Size = new Size(960, 34);
            pnlPagination.TabIndex = 2;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(panel4, 0, 0);
            tableLayoutPanel2.Controls.Add(panel5, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(960, 34);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(lblNumberOfRecords);
            panel4.Controls.Add(lblShowNumberPerPage);
            panel4.Controls.Add(cbxPageSize);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(474, 28);
            panel4.TabIndex = 0;
            // 
            // lblNumberOfRecords
            // 
            lblNumberOfRecords.AutoSize = true;
            lblNumberOfRecords.Font = new Font("Segoe UI", 13F);
            lblNumberOfRecords.ForeColor = Color.FromArgb(255, 192, 128);
            lblNumberOfRecords.Location = new Point(5, 5);
            lblNumberOfRecords.Name = "lblNumberOfRecords";
            lblNumberOfRecords.Size = new Size(104, 25);
            lblNumberOfRecords.TabIndex = 0;
            lblNumberOfRecords.Text = "Số lượng: 0";
            // 
            // lblShowNumberPerPage
            // 
            lblShowNumberPerPage.AutoSize = true;
            lblShowNumberPerPage.Font = new Font("Segoe UI", 12F);
            lblShowNumberPerPage.ForeColor = Color.FromArgb(124, 141, 181);
            lblShowNumberPerPage.Location = new Point(200, 8);
            lblShowNumberPerPage.Name = "lblShowNumberPerPage";
            lblShowNumberPerPage.Size = new Size(64, 21);
            lblShowNumberPerPage.TabIndex = 1;
            lblShowNumberPerPage.Text = "Hiển thị";
            // 
            // cbxPageSize
            // 
            cbxPageSize.BackColor = Color.FromArgb(42, 45, 86);
            cbxPageSize.ForeColor = Color.White;
            cbxPageSize.FormattingEnabled = true;
            cbxPageSize.Items.AddRange(new object[] { "10", "25", "50", "100" });
            cbxPageSize.Location = new Point(270, 6);
            cbxPageSize.Name = "cbxPageSize";
            cbxPageSize.Size = new Size(60, 23);
            cbxPageSize.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnPrevious);
            panel5.Controls.Add(btnNext);
            panel5.Controls.Add(lblShowingAtPage);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(483, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(474, 28);
            panel5.TabIndex = 1;
            // 
            // btnPrevious
            // 
            btnPrevious.Anchor = AnchorStyles.Right;
            btnPrevious.FlatAppearance.BorderColor = Color.Cyan;
            btnPrevious.FlatStyle = FlatStyle.Flat;
            btnPrevious.Font = new Font("Microsoft Sans Serif", 10F);
            btnPrevious.ForeColor = Color.FromArgb(255, 224, 192);
            btnPrevious.Location = new Point(250, 0);
            btnPrevious.Name = "btnPrevious";
            btnPrevious.Size = new Size(106, 28);
            btnPrevious.TabIndex = 0;
            btnPrevious.Text = "Trang trước";
            btnPrevious.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Right;
            btnNext.FlatAppearance.BorderColor = Color.Fuchsia;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Microsoft Sans Serif", 10F);
            btnNext.ForeColor = Color.FromArgb(192, 255, 192);
            btnNext.Location = new Point(362, 0);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(106, 28);
            btnNext.TabIndex = 1;
            btnNext.Text = "Trang sau";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // lblShowingAtPage
            // 
            lblShowingAtPage.Anchor = AnchorStyles.Right;
            lblShowingAtPage.AutoSize = true;
            lblShowingAtPage.ForeColor = Color.FromArgb(124, 141, 181);
            lblShowingAtPage.Location = new Point(100, 6);
            lblShowingAtPage.Name = "lblShowingAtPage";
            lblShowingAtPage.Size = new Size(105, 15);
            lblShowingAtPage.TabIndex = 2;
            lblShowingAtPage.Text = "Hiện trang 1 trên 1";
            // 
            // tabUserDetail
            // 
            tabUserDetail.BackColor = Color.FromArgb(24, 28, 63);
            tabUserDetail.Controls.Add(pnlUserDetail);
            tabUserDetail.Location = new Point(4, 24);
            tabUserDetail.Name = "tabUserDetail";
            tabUserDetail.Size = new Size(972, 592);
            tabUserDetail.TabIndex = 1;
            tabUserDetail.Text = "Thêm/Sửa người dùng";
            // 
            // pnlUserDetail
            // 
            pnlUserDetail.Controls.Add(groupBoxUserInfo);
            pnlUserDetail.Controls.Add(pnlUserDetailButtons);
            pnlUserDetail.Controls.Add(lblUserDetailTitle);
            pnlUserDetail.Dock = DockStyle.Fill;
            pnlUserDetail.Location = new Point(0, 0);
            pnlUserDetail.Name = "pnlUserDetail";
            pnlUserDetail.Padding = new Padding(10);
            pnlUserDetail.Size = new Size(972, 592);
            pnlUserDetail.TabIndex = 0;
            // 
            // groupBoxUserInfo
            // 
            groupBoxUserInfo.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxUserInfo.Controls.Add(tableLayoutPanel3);
            groupBoxUserInfo.ForeColor = Color.White;
            groupBoxUserInfo.Location = new Point(13, 50);
            groupBoxUserInfo.Name = "groupBoxUserInfo";
            groupBoxUserInfo.Size = new Size(946, 450);
            groupBoxUserInfo.TabIndex = 0;
            groupBoxUserInfo.TabStop = false;
            groupBoxUserInfo.Text = "Thông tin người dùng";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.ColumnCount = 4;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel3.Controls.Add(lblUserId, 0, 0);
            tableLayoutPanel3.Controls.Add(tbxUserId, 1, 0);
            tableLayoutPanel3.Controls.Add(lblFullName, 0, 1);
            tableLayoutPanel3.Controls.Add(tbxUserName, 1, 1);
            tableLayoutPanel3.Controls.Add(lblPassword, 0, 2);
            tableLayoutPanel3.Controls.Add(tbxPassword, 1, 2);
            tableLayoutPanel3.Controls.Add(lblEmployee, 2, 2);
            tableLayoutPanel3.Controls.Add(cbxEmployee, 3, 2);
            tableLayoutPanel3.Controls.Add(lblUserRole, 0, 3);
            tableLayoutPanel3.Controls.Add(cbxUserRole, 1, 3);
            tableLayoutPanel3.Controls.Add(lblIsActive, 2, 3);
            tableLayoutPanel3.Controls.Add(chkIsActive, 3, 3);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 19);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 4;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutPanel3.Size = new Size(940, 428);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // lblUserId
            // 
            lblUserId.Anchor = AnchorStyles.Left;
            lblUserId.AutoSize = true;
            lblUserId.ForeColor = Color.FromArgb(124, 141, 181);
            lblUserId.Location = new Point(3, 17);
            lblUserId.Name = "lblUserId";
            lblUserId.Size = new Size(47, 15);
            lblUserId.TabIndex = 0;
            lblUserId.Text = "ID User:";
            // 
            // tbxUserId
            // 
            tbxUserId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tbxUserId.BackColor = Color.FromArgb(42, 45, 86);
            tbxUserId.ForeColor = Color.White;
            tbxUserId.Location = new Point(123, 13);
            tbxUserId.Name = "tbxUserId";
            tbxUserId.ReadOnly = true;
            tbxUserId.Size = new Size(344, 23);
            tbxUserId.TabIndex = 1;
            // 
            // lblFullName
            // 
            lblFullName.Anchor = AnchorStyles.Left;
            lblFullName.AutoSize = true;
            lblFullName.ForeColor = Color.FromArgb(124, 141, 181);
            lblFullName.Location = new Point(3, 67);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(63, 15);
            lblFullName.TabIndex = 4;
            lblFullName.Text = "Username:";
            // 
            // tbxUserName
            // 
            tbxUserName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tbxUserName.BackColor = Color.FromArgb(42, 45, 86);
            tbxUserName.ForeColor = Color.White;
            tbxUserName.Location = new Point(123, 63);
            tbxUserName.Name = "tbxUserName";
            tbxUserName.Size = new Size(344, 23);
            tbxUserName.TabIndex = 5;
            // 
            // lblPassword
            // 
            lblPassword.Anchor = AnchorStyles.Left;
            lblPassword.AutoSize = true;
            lblPassword.ForeColor = Color.FromArgb(124, 141, 181);
            lblPassword.Location = new Point(3, 117);
            lblPassword.Name = "lblPassword";
            lblPassword.Size = new Size(60, 15);
            lblPassword.TabIndex = 8;
            lblPassword.Text = "Mật khẩu:";
            // 
            // tbxPassword
            // 
            tbxPassword.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            tbxPassword.BackColor = Color.FromArgb(42, 45, 86);
            tbxPassword.ForeColor = Color.White;
            tbxPassword.Location = new Point(123, 113);
            tbxPassword.Name = "tbxPassword";
            tbxPassword.PasswordChar = '*';
            tbxPassword.Size = new Size(344, 23);
            tbxPassword.TabIndex = 9;
            // 
            // lblEmployee
            // 
            lblEmployee.Anchor = AnchorStyles.Left;
            lblEmployee.AutoSize = true;
            lblEmployee.ForeColor = Color.FromArgb(124, 141, 181);
            lblEmployee.Location = new Point(473, 117);
            lblEmployee.Name = "lblEmployee";
            lblEmployee.Size = new Size(64, 15);
            lblEmployee.TabIndex = 10;
            lblEmployee.Text = "Nhân viên:";
            // 
            // cbxEmployee
            // 
            cbxEmployee.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cbxEmployee.BackColor = Color.FromArgb(42, 45, 86);
            cbxEmployee.ForeColor = Color.White;
            cbxEmployee.FormattingEnabled = true;
            cbxEmployee.Location = new Point(593, 113);
            cbxEmployee.Name = "cbxEmployee";
            cbxEmployee.Size = new Size(344, 23);
            cbxEmployee.TabIndex = 11;
            // 
            // lblUserRole
            // 
            lblUserRole.Anchor = AnchorStyles.Left;
            lblUserRole.AutoSize = true;
            lblUserRole.ForeColor = Color.FromArgb(124, 141, 181);
            lblUserRole.Location = new Point(3, 281);
            lblUserRole.Name = "lblUserRole";
            lblUserRole.Size = new Size(43, 15);
            lblUserRole.TabIndex = 12;
            lblUserRole.Text = "Vai trò:";
            // 
            // cbxUserRole
            // 
            cbxUserRole.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cbxUserRole.BackColor = Color.FromArgb(42, 45, 86);
            cbxUserRole.ForeColor = Color.White;
            cbxUserRole.FormattingEnabled = true;
            cbxUserRole.Location = new Point(123, 277);
            cbxUserRole.Name = "cbxUserRole";
            cbxUserRole.Size = new Size(344, 23);
            cbxUserRole.TabIndex = 13;
            // 
            // lblIsActive
            // 
            lblIsActive.Anchor = AnchorStyles.Left;
            lblIsActive.AutoSize = true;
            lblIsActive.ForeColor = Color.FromArgb(124, 141, 181);
            lblIsActive.Location = new Point(473, 281);
            lblIsActive.Name = "lblIsActive";
            lblIsActive.Size = new Size(63, 15);
            lblIsActive.TabIndex = 14;
            lblIsActive.Text = "Trạng thái:";
            // 
            // chkIsActive
            // 
            chkIsActive.Anchor = AnchorStyles.Left;
            chkIsActive.AutoSize = true;
            chkIsActive.ForeColor = Color.White;
            chkIsActive.Location = new Point(593, 279);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(83, 19);
            chkIsActive.TabIndex = 15;
            chkIsActive.Text = "Hoạt động";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // pnlUserDetailButtons
            // 
            pnlUserDetailButtons.Controls.Add(btnSaveUser);
            pnlUserDetailButtons.Controls.Add(btnCancelUser);
            pnlUserDetailButtons.Dock = DockStyle.Bottom;
            pnlUserDetailButtons.Location = new Point(10, 542);
            pnlUserDetailButtons.Name = "pnlUserDetailButtons";
            pnlUserDetailButtons.Size = new Size(952, 40);
            pnlUserDetailButtons.TabIndex = 1;
            // 
            // btnSaveUser
            // 
            btnSaveUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveUser.FlatAppearance.BorderColor = Color.Lime;
            btnSaveUser.FlatStyle = FlatStyle.Flat;
            btnSaveUser.Font = new Font("Microsoft Sans Serif", 11F);
            btnSaveUser.ForeColor = Color.FromArgb(192, 255, 192);
            btnSaveUser.Location = new Point(760, 5);
            btnSaveUser.Name = "btnSaveUser";
            btnSaveUser.Size = new Size(90, 30);
            btnSaveUser.TabIndex = 0;
            btnSaveUser.Text = "Lưu";
            btnSaveUser.UseVisualStyleBackColor = true;
            // 
            // btnCancelUser
            // 
            btnCancelUser.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelUser.FlatAppearance.BorderColor = Color.Red;
            btnCancelUser.FlatStyle = FlatStyle.Flat;
            btnCancelUser.Font = new Font("Microsoft Sans Serif", 11F);
            btnCancelUser.ForeColor = Color.FromArgb(255, 128, 128);
            btnCancelUser.Location = new Point(856, 5);
            btnCancelUser.Name = "btnCancelUser";
            btnCancelUser.Size = new Size(90, 30);
            btnCancelUser.TabIndex = 1;
            btnCancelUser.Text = "Hủy";
            btnCancelUser.UseVisualStyleBackColor = true;
            // 
            // lblUserDetailTitle
            // 
            lblUserDetailTitle.AutoSize = true;
            lblUserDetailTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblUserDetailTitle.ForeColor = Color.FromArgb(255, 224, 192);
            lblUserDetailTitle.Location = new Point(13, 13);
            lblUserDetailTitle.Name = "lblUserDetailTitle";
            lblUserDetailTitle.Size = new Size(245, 30);
            lblUserDetailTitle.TabIndex = 2;
            lblUserDetailTitle.Text = "Thêm người dùng mới";
            // 
            // tabRoleAssignment
            // 
            tabRoleAssignment.BackColor = Color.FromArgb(24, 28, 63);
            tabRoleAssignment.Controls.Add(pnlRoleAssignment);
            tabRoleAssignment.Location = new Point(4, 24);
            tabRoleAssignment.Name = "tabRoleAssignment";
            tabRoleAssignment.Size = new Size(972, 592);
            tabRoleAssignment.TabIndex = 2;
            tabRoleAssignment.Text = "Gán vai trò";
            // 
            // pnlRoleAssignment
            // 
            pnlRoleAssignment.Controls.Add(tableLayoutPanel4);
            pnlRoleAssignment.Controls.Add(lblRoleAssignmentTitle);
            pnlRoleAssignment.Controls.Add(lblCurrentUserId);
            pnlRoleAssignment.Dock = DockStyle.Fill;
            pnlRoleAssignment.Location = new Point(0, 0);
            pnlRoleAssignment.Name = "pnlRoleAssignment";
            pnlRoleAssignment.Padding = new Padding(10);
            pnlRoleAssignment.Size = new Size(972, 592);
            pnlRoleAssignment.TabIndex = 0;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel4.ColumnCount = 3;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel4.Controls.Add(groupBoxAvailableRoles, 0, 0);
            tableLayoutPanel4.Controls.Add(pnlRoleButtons, 1, 0);
            tableLayoutPanel4.Controls.Add(groupBoxUserRoles, 2, 0);
            tableLayoutPanel4.Location = new Point(13, 50);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(946, 529);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // groupBoxAvailableRoles
            // 
            groupBoxAvailableRoles.Controls.Add(lstAvailableRoles);
            groupBoxAvailableRoles.Dock = DockStyle.Fill;
            groupBoxAvailableRoles.ForeColor = Color.White;
            groupBoxAvailableRoles.Location = new Point(3, 3);
            groupBoxAvailableRoles.Name = "groupBoxAvailableRoles";
            groupBoxAvailableRoles.Size = new Size(419, 523);
            groupBoxAvailableRoles.TabIndex = 0;
            groupBoxAvailableRoles.TabStop = false;
            groupBoxAvailableRoles.Text = "Vai trò có sẵn";
            // 
            // lstAvailableRoles
            // 
            lstAvailableRoles.BackColor = Color.FromArgb(42, 45, 86);
            lstAvailableRoles.BorderStyle = BorderStyle.None;
            lstAvailableRoles.Dock = DockStyle.Fill;
            lstAvailableRoles.ForeColor = Color.White;
            lstAvailableRoles.FormattingEnabled = true;
            lstAvailableRoles.ItemHeight = 15;
            lstAvailableRoles.Location = new Point(3, 19);
            lstAvailableRoles.Name = "lstAvailableRoles";
            lstAvailableRoles.SelectionMode = SelectionMode.MultiExtended;
            lstAvailableRoles.Size = new Size(413, 501);
            lstAvailableRoles.TabIndex = 0;
            // 
            // pnlRoleButtons
            // 
            pnlRoleButtons.Controls.Add(btnAssignRole);
            pnlRoleButtons.Controls.Add(btnRemoveRole);
            pnlRoleButtons.Dock = DockStyle.Fill;
            pnlRoleButtons.Location = new Point(428, 3);
            pnlRoleButtons.Name = "pnlRoleButtons";
            pnlRoleButtons.Size = new Size(88, 523);
            pnlRoleButtons.TabIndex = 1;
            // 
            // btnAssignRole
            // 
            btnAssignRole.Anchor = AnchorStyles.None;
            btnAssignRole.FlatAppearance.BorderColor = Color.Lime;
            btnAssignRole.FlatStyle = FlatStyle.Flat;
            btnAssignRole.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnAssignRole.ForeColor = Color.FromArgb(192, 255, 192);
            btnAssignRole.Location = new Point(19, 200);
            btnAssignRole.Name = "btnAssignRole";
            btnAssignRole.Size = new Size(50, 35);
            btnAssignRole.TabIndex = 0;
            btnAssignRole.Text = "→";
            btnAssignRole.UseVisualStyleBackColor = true;
            // 
            // btnRemoveRole
            // 
            btnRemoveRole.Anchor = AnchorStyles.None;
            btnRemoveRole.FlatAppearance.BorderColor = Color.Red;
            btnRemoveRole.FlatStyle = FlatStyle.Flat;
            btnRemoveRole.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnRemoveRole.ForeColor = Color.FromArgb(255, 128, 128);
            btnRemoveRole.Location = new Point(19, 260);
            btnRemoveRole.Name = "btnRemoveRole";
            btnRemoveRole.Size = new Size(50, 35);
            btnRemoveRole.TabIndex = 1;
            btnRemoveRole.Text = "←";
            btnRemoveRole.UseVisualStyleBackColor = true;
            // 
            // groupBoxUserRoles
            // 
            groupBoxUserRoles.Controls.Add(lstUserRoles);
            groupBoxUserRoles.Dock = DockStyle.Fill;
            groupBoxUserRoles.ForeColor = Color.White;
            groupBoxUserRoles.Location = new Point(522, 3);
            groupBoxUserRoles.Name = "groupBoxUserRoles";
            groupBoxUserRoles.Size = new Size(421, 523);
            groupBoxUserRoles.TabIndex = 2;
            groupBoxUserRoles.TabStop = false;
            groupBoxUserRoles.Text = "Vai trò đã gán";
            // 
            // lstUserRoles
            // 
            lstUserRoles.BackColor = Color.FromArgb(42, 45, 86);
            lstUserRoles.BorderStyle = BorderStyle.None;
            lstUserRoles.Dock = DockStyle.Fill;
            lstUserRoles.ForeColor = Color.White;
            lstUserRoles.FormattingEnabled = true;
            lstUserRoles.ItemHeight = 15;
            lstUserRoles.Location = new Point(3, 19);
            lstUserRoles.Name = "lstUserRoles";
            lstUserRoles.SelectionMode = SelectionMode.MultiExtended;
            lstUserRoles.Size = new Size(415, 501);
            lstUserRoles.TabIndex = 0;
            // 
            // lblRoleAssignmentTitle
            // 
            lblRoleAssignmentTitle.AutoSize = true;
            lblRoleAssignmentTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblRoleAssignmentTitle.ForeColor = Color.FromArgb(255, 224, 192);
            lblRoleAssignmentTitle.Location = new Point(13, 13);
            lblRoleAssignmentTitle.Name = "lblRoleAssignmentTitle";
            lblRoleAssignmentTitle.Size = new Size(298, 30);
            lblRoleAssignmentTitle.TabIndex = 1;
            lblRoleAssignmentTitle.Text = "Gán vai trò cho người dùng";
            // 
            // lblCurrentUserId
            // 
            lblCurrentUserId.AutoSize = true;
            lblCurrentUserId.ForeColor = Color.FromArgb(124, 141, 181);
            lblCurrentUserId.Location = new Point(13, 582);
            lblCurrentUserId.Name = "lblCurrentUserId";
            lblCurrentUserId.Size = new Size(13, 15);
            lblCurrentUserId.TabIndex = 2;
            lblCurrentUserId.Text = "0";
            lblCurrentUserId.Visible = false;
            // 
            // btnUpdate
            // 
            btnUpdate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnUpdate.FlatAppearance.BorderColor = Color.Orange;
            btnUpdate.FlatStyle = FlatStyle.Flat;
            btnUpdate.Font = new Font("Microsoft Sans Serif", 11F);
            btnUpdate.ForeColor = Color.FromArgb(255, 192, 128);
            btnUpdate.Location = new Point(867, 5);
            btnUpdate.Margin = new Padding(5);
            btnUpdate.Name = "btnUpdate";
            btnUpdate.Size = new Size(88, 48);
            btnUpdate.TabIndex = 2;
            btnUpdate.Text = "Sửa";
            btnUpdate.UseVisualStyleBackColor = true;
            // 
            // FrmUserManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(1000, 640);
            Controls.Add(tabControl);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(1000, 640);
            Name = "FrmUserManagement";
            Padding = new Padding(10);
            Text = "Quản lý người dùng";
            tabControl.ResumeLayout(false);
            tabUserList.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            pnlHeader.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            pnlContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUsers).EndInit();
            pnlPagination.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            tabUserDetail.ResumeLayout(false);
            pnlUserDetail.ResumeLayout(false);
            pnlUserDetail.PerformLayout();
            groupBoxUserInfo.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            tableLayoutPanel3.PerformLayout();
            pnlUserDetailButtons.ResumeLayout(false);
            tabRoleAssignment.ResumeLayout(false);
            pnlRoleAssignment.ResumeLayout(false);
            pnlRoleAssignment.PerformLayout();
            tableLayoutPanel4.ResumeLayout(false);
            groupBoxAvailableRoles.ResumeLayout(false);
            pnlRoleButtons.ResumeLayout(false);
            groupBoxUserRoles.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabUserList;
        private TabPage tabUserDetail;
        private TabPage tabRoleAssignment;

        // User List Tab
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel pnlHeader;
        private Panel pnlContent;
        private Panel pnlPagination;
        private DataGridView dgvUsers;

        // Header controls
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Button btnGetDetails;
        private Button btnAdd;
        private Button btnFilter1;
        private Button btnFilter2;
        private Label lblFilter1;
        private Label lblFilter2;
        private Label lblSearchString;
        private ComboBox cbxFilter1;
        private ComboBox cbxFilter2;
        private TextBox tbxFindString;

        // Pagination controls
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel4;
        private Panel panel5;
        private Label lblNumberOfRecords;
        private Label lblShowNumberPerPage;
        private Label lblShowingAtPage;
        private ComboBox cbxPageSize;
        private Button btnPrevious;
        private Button btnNext;

        // User Detail Tab
        private Panel pnlUserDetail;
        private GroupBox groupBoxUserInfo;
        private TableLayoutPanel tableLayoutPanel3;
        private Panel pnlUserDetailButtons;
        private Label lblUserDetailTitle;

        // User detail controls
        private Label lblUserId;
        private Label lblFullName;
        private Label lblPassword;
        private Label lblEmployee;
        private Label lblUserRole;
        private Label lblIsActive;
        private TextBox tbxUserId;
        private TextBox tbxUserName;
        private TextBox tbxPassword;
        private ComboBox cbxEmployee;
        private ComboBox cbxUserRole;
        private CheckBox chkIsActive;
        private Button btnSaveUser;
        private Button btnCancelUser;

        // Role Assignment Tab
        private Panel pnlRoleAssignment;
        private TableLayoutPanel tableLayoutPanel4;
        private GroupBox groupBoxAvailableRoles;
        private GroupBox groupBoxUserRoles;
        private Panel pnlRoleButtons;
        private ListBox lstAvailableRoles;
        private ListBox lstUserRoles;
        private Button btnAssignRole;
        private Button btnRemoveRole;
        private Label lblRoleAssignmentTitle;
        private Label lblCurrentUserId;
        private Button btnUpdate;
    }
}