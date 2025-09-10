namespace Dashboard.Winform.Forms
{
    partial class FrmBaseManagement
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
            tableLayoutPanel1 = new TableLayoutPanel();
            tbpnHeaderManagementSection = new TableLayoutPanel();
            btnSearch = new Button();
            btnAdd = new Button();
            panel3 = new Panel();
            btnFilterByStockStatus = new Button();
            lblFilter2 = new Label();
            cbxFilterByStockStatus = new ComboBox();
            panel2 = new Panel();
            btnfilterByGoodsStatus = new Button();
            lblFilter1 = new Label();
            cbxFilterByGoodsStatus = new ComboBox();
            panel1 = new Panel();
            btnOrderBy = new Button();
            lblSortBy = new Label();
            cbxOrderBy = new ComboBox();
            panel4 = new Panel();
            tbxFindString = new TextBox();
            lblSearchString = new Label();
            panel5 = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel6 = new Panel();
            btnNumbOfRecordShowing = new Button();
            lblNumberOfRecords = new Label();
            lblShowNumberPerPage = new Label();
            cbxNumbRecordsPerPage = new ComboBox();
            panel7 = new Panel();
            btnToday = new Button();
            btnNext = new Button();
            lblShowingAtPage = new Label();
            dgvListItem = new DataGridView();
            tableLayoutPanel1.SuspendLayout();
            tbpnHeaderManagementSection.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvListItem).BeginInit();
            SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(24, 28, 63);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(tbpnHeaderManagementSection, 0, 0);
            tableLayoutPanel1.Controls.Add(panel5, 0, 2);
            tableLayoutPanel1.Controls.Add(dgvListItem, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(10, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(960, 630);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // tbpnHeaderManagementSection
            // 
            tbpnHeaderManagementSection.ColumnCount = 6;
            tbpnHeaderManagementSection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            tbpnHeaderManagementSection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            tbpnHeaderManagementSection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            tbpnHeaderManagementSection.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tbpnHeaderManagementSection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            tbpnHeaderManagementSection.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            tbpnHeaderManagementSection.Controls.Add(btnSearch, 4, 0);
            tbpnHeaderManagementSection.Controls.Add(btnAdd, 5, 0);
            tbpnHeaderManagementSection.Controls.Add(panel3, 2, 0);
            tbpnHeaderManagementSection.Controls.Add(panel2, 1, 0);
            tbpnHeaderManagementSection.Controls.Add(panel1, 0, 0);
            tbpnHeaderManagementSection.Controls.Add(panel4, 3, 0);
            tbpnHeaderManagementSection.Dock = DockStyle.Top;
            tbpnHeaderManagementSection.Location = new Point(3, 3);
            tbpnHeaderManagementSection.Name = "tbpnHeaderManagementSection";
            tbpnHeaderManagementSection.RowCount = 1;
            tbpnHeaderManagementSection.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tbpnHeaderManagementSection.Size = new Size(954, 53);
            tbpnHeaderManagementSection.TabIndex = 5;
            // 
            // btnSearch
            // 
            btnSearch.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnSearch.FlatAppearance.BorderColor = Color.Fuchsia;
            btnSearch.FlatStyle = FlatStyle.Flat;
            btnSearch.Font = new Font("Microsoft Sans Serif", 11F);
            btnSearch.ForeColor = Color.FromArgb(192, 255, 192);
            btnSearch.Location = new Point(763, 5);
            btnSearch.Margin = new Padding(5);
            btnSearch.Name = "btnSearch";
            btnSearch.Size = new Size(88, 43);
            btnSearch.TabIndex = 12;
            btnSearch.Text = "Tìm kiếm";
            btnSearch.UseVisualStyleBackColor = true;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.FlatAppearance.BorderColor = Color.Cyan;
            btnAdd.FlatStyle = FlatStyle.Flat;
            btnAdd.Font = new Font("Microsoft Sans Serif", 11F);
            btnAdd.ForeColor = Color.FromArgb(255, 224, 192);
            btnAdd.Location = new Point(861, 5);
            btnAdd.Margin = new Padding(5);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(88, 43);
            btnAdd.TabIndex = 11;
            btnAdd.Text = "Thêm";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            panel3.Controls.Add(btnFilterByStockStatus);
            panel3.Controls.Add(lblFilter2);
            panel3.Controls.Add(cbxFilterByStockStatus);
            panel3.Location = new Point(297, 5);
            panel3.Margin = new Padding(5);
            panel3.Name = "panel3";
            panel3.Size = new Size(136, 42);
            panel3.TabIndex = 2;
            // 
            // btnFilterByStockStatus
            // 
            btnFilterByStockStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnFilterByStockStatus.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnFilterByStockStatus.FlatStyle = FlatStyle.Flat;
            btnFilterByStockStatus.Font = new Font("Microsoft Sans Serif", 9F);
            btnFilterByStockStatus.ForeColor = Color.FromArgb(192, 255, 192);
            btnFilterByStockStatus.Location = new Point(0, 18);
            btnFilterByStockStatus.Margin = new Padding(5);
            btnFilterByStockStatus.Name = "btnFilterByStockStatus";
            btnFilterByStockStatus.Size = new Size(136, 23);
            btnFilterByStockStatus.TabIndex = 16;
            btnFilterByStockStatus.Text = "Tìm kiếm";
            btnFilterByStockStatus.TextAlign = ContentAlignment.TopLeft;
            btnFilterByStockStatus.UseVisualStyleBackColor = true;
            // 
            // lblFilter2
            // 
            lblFilter2.AutoSize = true;
            lblFilter2.ForeColor = Color.FromArgb(124, 141, 181);
            lblFilter2.Location = new Point(0, 3);
            lblFilter2.Name = "lblFilter2";
            lblFilter2.Size = new Size(93, 15);
            lblFilter2.TabIndex = 4;
            lblFilter2.Text = "Trạng thái hàng:";
            // 
            // cbxFilterByStockStatus
            // 
            cbxFilterByStockStatus.BackColor = Color.FromArgb(42, 45, 86);
            cbxFilterByStockStatus.Dock = DockStyle.Bottom;
            cbxFilterByStockStatus.FlatStyle = FlatStyle.Flat;
            cbxFilterByStockStatus.ForeColor = Color.WhiteSmoke;
            cbxFilterByStockStatus.FormattingEnabled = true;
            cbxFilterByStockStatus.Location = new Point(0, 19);
            cbxFilterByStockStatus.Name = "cbxFilterByStockStatus";
            cbxFilterByStockStatus.Size = new Size(136, 23);
            cbxFilterByStockStatus.TabIndex = 3;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnfilterByGoodsStatus);
            panel2.Controls.Add(lblFilter1);
            panel2.Controls.Add(cbxFilterByGoodsStatus);
            panel2.Location = new Point(151, 5);
            panel2.Margin = new Padding(5);
            panel2.Name = "panel2";
            panel2.Size = new Size(136, 43);
            panel2.TabIndex = 1;
            // 
            // btnfilterByGoodsStatus
            // 
            btnfilterByGoodsStatus.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnfilterByGoodsStatus.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnfilterByGoodsStatus.FlatStyle = FlatStyle.Flat;
            btnfilterByGoodsStatus.Font = new Font("Microsoft Sans Serif", 9F);
            btnfilterByGoodsStatus.ForeColor = Color.FromArgb(192, 255, 192);
            btnfilterByGoodsStatus.Location = new Point(0, 19);
            btnfilterByGoodsStatus.Margin = new Padding(5);
            btnfilterByGoodsStatus.Name = "btnfilterByGoodsStatus";
            btnfilterByGoodsStatus.Size = new Size(136, 23);
            btnfilterByGoodsStatus.TabIndex = 15;
            btnfilterByGoodsStatus.Text = "Tìm kiếm";
            btnfilterByGoodsStatus.TextAlign = ContentAlignment.TopLeft;
            btnfilterByGoodsStatus.UseVisualStyleBackColor = true;
            // 
            // lblFilter1
            // 
            lblFilter1.AutoSize = true;
            lblFilter1.ForeColor = Color.FromArgb(124, 141, 181);
            lblFilter1.Location = new Point(0, 3);
            lblFilter1.Name = "lblFilter1";
            lblFilter1.Size = new Size(128, 15);
            lblFilter1.TabIndex = 4;
            lblFilter1.Text = "Trạng thái nguyên liệu:";
            // 
            // cbxFilterByGoodsStatus
            // 
            cbxFilterByGoodsStatus.BackColor = Color.FromArgb(42, 45, 86);
            cbxFilterByGoodsStatus.Dock = DockStyle.Bottom;
            cbxFilterByGoodsStatus.FlatStyle = FlatStyle.Flat;
            cbxFilterByGoodsStatus.ForeColor = Color.WhiteSmoke;
            cbxFilterByGoodsStatus.FormattingEnabled = true;
            cbxFilterByGoodsStatus.Location = new Point(0, 20);
            cbxFilterByGoodsStatus.Name = "cbxFilterByGoodsStatus";
            cbxFilterByGoodsStatus.Size = new Size(136, 23);
            cbxFilterByGoodsStatus.TabIndex = 3;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnOrderBy);
            panel1.Controls.Add(lblSortBy);
            panel1.Controls.Add(cbxOrderBy);
            panel1.Location = new Point(5, 5);
            panel1.Margin = new Padding(5);
            panel1.Name = "panel1";
            panel1.Size = new Size(136, 43);
            panel1.TabIndex = 0;
            // 
            // btnOrderBy
            // 
            btnOrderBy.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnOrderBy.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnOrderBy.FlatStyle = FlatStyle.Flat;
            btnOrderBy.Font = new Font("Microsoft Sans Serif", 9F);
            btnOrderBy.ForeColor = Color.FromArgb(192, 255, 192);
            btnOrderBy.Location = new Point(0, 19);
            btnOrderBy.Margin = new Padding(5);
            btnOrderBy.Name = "btnOrderBy";
            btnOrderBy.Size = new Size(136, 23);
            btnOrderBy.TabIndex = 14;
            btnOrderBy.Text = "Tìm kiếm";
            btnOrderBy.TextAlign = ContentAlignment.TopLeft;
            btnOrderBy.UseVisualStyleBackColor = true;
            // 
            // lblSortBy
            // 
            lblSortBy.AutoSize = true;
            lblSortBy.ForeColor = Color.FromArgb(124, 141, 181);
            lblSortBy.Location = new Point(0, 1);
            lblSortBy.Name = "lblSortBy";
            lblSortBy.Size = new Size(80, 15);
            lblSortBy.TabIndex = 4;
            lblSortBy.Text = "Sắp xếp theo: ";
            // 
            // cbxOrderBy
            // 
            cbxOrderBy.BackColor = Color.FromArgb(42, 45, 86);
            cbxOrderBy.Dock = DockStyle.Bottom;
            cbxOrderBy.FlatStyle = FlatStyle.Flat;
            cbxOrderBy.ForeColor = Color.WhiteSmoke;
            cbxOrderBy.FormattingEnabled = true;
            cbxOrderBy.Location = new Point(0, 20);
            cbxOrderBy.Name = "cbxOrderBy";
            cbxOrderBy.Size = new Size(136, 23);
            cbxOrderBy.TabIndex = 3;
            // 
            // panel4
            // 
            panel4.Controls.Add(tbxFindString);
            panel4.Controls.Add(lblSearchString);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(443, 5);
            panel4.Margin = new Padding(5);
            panel4.MinimumSize = new Size(310, 43);
            panel4.Name = "panel4";
            panel4.Padding = new Padding(5, 0, 5, 5);
            panel4.Size = new Size(310, 43);
            panel4.TabIndex = 3;
            // 
            // tbxFindString
            // 
            tbxFindString.BackColor = Color.FromArgb(42, 45, 86);
            tbxFindString.BorderStyle = BorderStyle.None;
            tbxFindString.Dock = DockStyle.Bottom;
            tbxFindString.ForeColor = Color.FromArgb(192, 255, 192);
            tbxFindString.Location = new Point(5, 22);
            tbxFindString.Margin = new Padding(5, 5, 10, 5);
            tbxFindString.Name = "tbxFindString";
            tbxFindString.PlaceholderText = "Tên/ID cần tìm ";
            tbxFindString.Size = new Size(300, 16);
            tbxFindString.TabIndex = 1;
            // 
            // lblSearchString
            // 
            lblSearchString.AutoSize = true;
            lblSearchString.ForeColor = Color.FromArgb(124, 141, 181);
            lblSearchString.Location = new Point(1, 3);
            lblSearchString.Margin = new Padding(0);
            lblSearchString.Name = "lblSearchString";
            lblSearchString.Size = new Size(162, 15);
            lblSearchString.TabIndex = 0;
            lblSearchString.Text = "Tìm kiếm theo (ID, tên hàng):";
            // 
            // panel5
            // 
            panel5.Controls.Add(tableLayoutPanel2);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(3, 593);
            panel5.Name = "panel5";
            panel5.Size = new Size(954, 34);
            panel5.TabIndex = 3;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.ColumnCount = 2;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel2.Controls.Add(panel6, 0, 0);
            tableLayoutPanel2.Controls.Add(panel7, 1, 0);
            tableLayoutPanel2.Dock = DockStyle.Fill;
            tableLayoutPanel2.Location = new Point(0, 0);
            tableLayoutPanel2.MaximumSize = new Size(0, 42);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 1;
            tableLayoutPanel2.RowStyles.Add(new RowStyle());
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(954, 34);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // panel6
            // 
            panel6.Controls.Add(btnNumbOfRecordShowing);
            panel6.Controls.Add(lblNumberOfRecords);
            panel6.Controls.Add(lblShowNumberPerPage);
            panel6.Controls.Add(cbxNumbRecordsPerPage);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(0, 0);
            panel6.Margin = new Padding(0);
            panel6.Name = "panel6";
            panel6.Size = new Size(477, 42);
            panel6.TabIndex = 0;
            // 
            // btnNumbOfRecordShowing
            // 
            btnNumbOfRecordShowing.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnNumbOfRecordShowing.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnNumbOfRecordShowing.FlatStyle = FlatStyle.Flat;
            btnNumbOfRecordShowing.Font = new Font("Microsoft Sans Serif", 9F);
            btnNumbOfRecordShowing.ForeColor = Color.FromArgb(192, 255, 192);
            btnNumbOfRecordShowing.Location = new Point(233, 7);
            btnNumbOfRecordShowing.Margin = new Padding(5);
            btnNumbOfRecordShowing.MaximumSize = new Size(50, 23);
            btnNumbOfRecordShowing.MinimumSize = new Size(50, 23);
            btnNumbOfRecordShowing.Name = "btnNumbOfRecordShowing";
            btnNumbOfRecordShowing.Size = new Size(50, 23);
            btnNumbOfRecordShowing.TabIndex = 17;
            btnNumbOfRecordShowing.Text = "50";
            btnNumbOfRecordShowing.UseVisualStyleBackColor = true;
            // 
            // lblNumberOfRecords
            // 
            lblNumberOfRecords.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblNumberOfRecords.AutoSize = true;
            lblNumberOfRecords.Font = new Font("Segoe UI", 13F);
            lblNumberOfRecords.ForeColor = Color.FromArgb(255, 192, 128);
            lblNumberOfRecords.Location = new Point(5, 6);
            lblNumberOfRecords.Margin = new Padding(5);
            lblNumberOfRecords.MinimumSize = new Size(150, 0);
            lblNumberOfRecords.Name = "lblNumberOfRecords";
            lblNumberOfRecords.Size = new Size(150, 25);
            lblNumberOfRecords.TabIndex = 5;
            lblNumberOfRecords.Text = "Số lượng: 5000";
            lblNumberOfRecords.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // lblShowNumberPerPage
            // 
            lblShowNumberPerPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblShowNumberPerPage.AutoSize = true;
            lblShowNumberPerPage.Font = new Font("Segoe UI", 12F);
            lblShowNumberPerPage.ForeColor = Color.FromArgb(124, 141, 181);
            lblShowNumberPerPage.Location = new Point(159, 9);
            lblShowNumberPerPage.Margin = new Padding(5);
            lblShowNumberPerPage.Name = "lblShowNumberPerPage";
            lblShowNumberPerPage.Size = new Size(64, 21);
            lblShowNumberPerPage.TabIndex = 6;
            lblShowNumberPerPage.Text = "Hiển thị";
            lblShowNumberPerPage.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // cbxNumbRecordsPerPage
            // 
            cbxNumbRecordsPerPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            cbxNumbRecordsPerPage.BackColor = Color.FromArgb(42, 45, 86);
            cbxNumbRecordsPerPage.FlatStyle = FlatStyle.System;
            cbxNumbRecordsPerPage.FormattingEnabled = true;
            cbxNumbRecordsPerPage.Location = new Point(233, 9);
            cbxNumbRecordsPerPage.Margin = new Padding(5);
            cbxNumbRecordsPerPage.MaximumSize = new Size(50, 0);
            cbxNumbRecordsPerPage.MinimumSize = new Size(50, 0);
            cbxNumbRecordsPerPage.Name = "cbxNumbRecordsPerPage";
            cbxNumbRecordsPerPage.Size = new Size(50, 23);
            cbxNumbRecordsPerPage.TabIndex = 7;
            // 
            // panel7
            // 
            panel7.Controls.Add(btnToday);
            panel7.Controls.Add(btnNext);
            panel7.Controls.Add(lblShowingAtPage);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(480, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(471, 36);
            panel7.TabIndex = 1;
            // 
            // btnToday
            // 
            btnToday.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnToday.FlatAppearance.BorderColor = Color.Cyan;
            btnToday.FlatStyle = FlatStyle.Flat;
            btnToday.Font = new Font("Microsoft Sans Serif", 10F);
            btnToday.ForeColor = Color.FromArgb(255, 224, 192);
            btnToday.Location = new Point(250, 3);
            btnToday.Margin = new Padding(3, 2, 3, 2);
            btnToday.MaximumSize = new Size(106, 28);
            btnToday.MinimumSize = new Size(106, 28);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(106, 28);
            btnToday.TabIndex = 11;
            btnToday.Text = "Trang trước";
            btnToday.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            btnNext.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnNext.FlatAppearance.BorderColor = Color.Fuchsia;
            btnNext.FlatStyle = FlatStyle.Flat;
            btnNext.Font = new Font("Microsoft Sans Serif", 10F);
            btnNext.ForeColor = Color.FromArgb(192, 255, 192);
            btnNext.Location = new Point(362, 3);
            btnNext.Margin = new Padding(3, 2, 3, 2);
            btnNext.MaximumSize = new Size(106, 28);
            btnNext.MinimumSize = new Size(106, 28);
            btnNext.Name = "btnNext";
            btnNext.Size = new Size(106, 28);
            btnNext.TabIndex = 10;
            btnNext.Text = "Trang sau";
            btnNext.UseVisualStyleBackColor = true;
            // 
            // lblShowingAtPage
            // 
            lblShowingAtPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            lblShowingAtPage.AutoSize = true;
            lblShowingAtPage.ForeColor = Color.FromArgb(124, 141, 181);
            lblShowingAtPage.Location = new Point(119, 11);
            lblShowingAtPage.Name = "lblShowingAtPage";
            lblShowingAtPage.Size = new Size(132, 15);
            lblShowingAtPage.TabIndex = 8;
            lblShowingAtPage.Text = "Hiện trang 340 trên 500 ";
            // 
            // dgvListItem
            // 
            dgvListItem.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvListItem.Dock = DockStyle.Fill;
            dgvListItem.Location = new Point(3, 67);
            dgvListItem.Name = "dgvListItem";
            dgvListItem.Size = new Size(954, 520);
            dgvListItem.TabIndex = 4;
            // 
            // FrmBaseManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(980, 640);
            Controls.Add(tableLayoutPanel1);
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(980, 640);
            Name = "FrmBaseManagement";
            Padding = new Padding(10, 0, 10, 10);
            Text = "Form1";
            tableLayoutPanel1.ResumeLayout(false);
            tbpnHeaderManagementSection.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel7.ResumeLayout(false);
            panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvListItem).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel tableLayoutPanel1;
        private Panel panel5;
        private DataGridView dgvListItem;
        private TableLayoutPanel tableLayoutPanel2;
        private Panel panel6;
        private Label lblNumberOfRecords;
        private Label lblShowNumberPerPage;
        private ComboBox cbxNumbRecordsPerPage;
        private Panel panel7;
        private Label lblShowingAtPage;
        private Button btnNext;
        private TableLayoutPanel tbpnHeaderManagementSection;
        private Button btnSearch;
        private Button btnAdd;
        private Panel panel3;
        private Label lblFilter2;
        private ComboBox cbxFilterByStockStatus;
        private Panel panel2;
        private Label lblFilter1;
        private ComboBox cbxFilterByGoodsStatus;
        private Panel panel1;
        private Label lblSortBy;
        private ComboBox cbxOrderBy;
        private Panel panel4;
        private TextBox tbxFindString;
        private Label lblSearchString;
        private Button btnOrderBy;
        private Button btnfilterByGoodsStatus;
        private Button btnFilterByStockStatus;
        private Button btnNumbOfRecordShowing;
        private Button btnToday;
    }
}