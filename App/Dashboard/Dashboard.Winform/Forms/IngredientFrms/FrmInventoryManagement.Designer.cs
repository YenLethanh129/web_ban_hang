// Designer: FrmInventoryManagement - provides 3 tabs (Transactions, Requests, TransferExecution)
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms
{
    partial class FrmInventoryManagement
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

        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            tabControl = new TabControl();
            tabInventoryTransactions = new TabPage();
            tableLayoutPanel1 = new TableLayoutPanel();
            pnlTransactionHeader = new TableLayoutPanel();
            panel1 = new Panel();
            btnTransactionFilter1 = new Button();
            lblTransactionFilter1 = new Label();
            cbxTransactionFilter1 = new ComboBox();
            panel2 = new Panel();
            btnTransactionFilter2 = new Button();
            lblTransactionFilter2 = new Label();
            cbxTransactionFilter2 = new ComboBox();
            panel3 = new Panel();
            tbxTransactionSearch = new TextBox();
            lblTransactionSearch = new Label();
            btnViewTransactionDetails = new Button();
            btnCreateTransaction = new Button();
            pnlTransactionContent = new Panel();
            dgvTransactions = new DataGridView();
            pnlTransactionPagination = new Panel();
            tableLayoutPanel2 = new TableLayoutPanel();
            panel4 = new Panel();
            lblTransactionRecords = new Label();
            lblTransactionPageSize = new Label();
            cbxTransactionPageSize = new ComboBox();
            panel5 = new Panel();
            btnTransactionPreviousPage = new Button();
            btnTransactionNextPage = new Button();
            lblTransactionTotalPage = new Label();
            btnTransactionPrevious = new Button();
            btnTransactionNext = new Button();
            lblTransactionPageInfo = new Label();
            tabInventoryRequests = new TabPage();
            tableLayoutPanel3 = new TableLayoutPanel();
            pnlRequestHeader = new TableLayoutPanel();
            lblTransferTitle = new Label();
            panel6 = new Panel();
            btnRequestFilter1 = new Button();
            lblRequestFilter1 = new Label();
            cbxRequestFilter1 = new ComboBox();
            panel7 = new Panel();
            lblRequestFilter2 = new Label();
            btnRequestFilter2 = new Button();
            cbxRequestFilter2 = new ComboBox();
            panel8 = new Panel();
            tbxRequestSearch = new TextBox();
            lblRequestSearch = new Label();
            btnCreateRequest = new Button();
            btnApproveRequest = new Button();
            btnViewRequestDetails = new Button();
            pnlRequestContent = new Panel();
            dgvRequests = new DataGridView();
            pnlRequestPagination = new Panel();
            tableLayoutPanel4 = new TableLayoutPanel();
            panel9 = new Panel();
            lblRequestRecords = new Label();
            lblRequestPageSize = new Label();
            cbxRequestPageSize = new ComboBox();
            panel10 = new Panel();
            btnRequestPrevious = new Button();
            btnRequestNext = new Button();
            lblRequestPageInfo = new Label();
            tabTransferExecution = new TabPage();
            tableLayoutPanel5 = new TableLayoutPanel();
            pnlTransferHeader = new TableLayoutPanel();
            label1 = new Label();
            panel11 = new Panel();
            btnTransferFilter1 = new Button();
            lblTransferFilter1 = new Label();
            cbxTransferFilter1 = new ComboBox();
            panel12 = new Panel();
            btnTransferFilter2 = new Button();
            lblTransferFilter2 = new Label();
            cbxTransferFilter2 = new ComboBox();
            panel13 = new Panel();
            tbxTransferSearch = new TextBox();
            lblTransferSearch = new Label();
            btnImportExcel = new Button();
            btnExecuteTransfer = new Button();
            btnCreateTransfer = new Button();
            pnlTransferExecution = new Panel();
            tableLayoutPanel6 = new TableLayoutPanel();
            groupBoxAvailableIngredients = new GroupBox();
            dgvAvailableIngredients = new DataGridView();
            pnlTransferButtons = new Panel();
            btnAddToTransfer = new Button();
            btnRemoveFromTransfer = new Button();
            btnClearTransfer = new Button();
            groupBoxTransferList = new GroupBox();
            dgvTransferList = new DataGridView();
            pnlTransferActions = new Panel();
            btnSaveTransfer = new Button();
            btnCancelTransfer = new Button();
            tabControl.SuspendLayout();
            tabInventoryTransactions.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            pnlTransactionHeader.SuspendLayout();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            pnlTransactionContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).BeginInit();
            pnlTransactionPagination.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            tabInventoryRequests.SuspendLayout();
            tableLayoutPanel3.SuspendLayout();
            pnlRequestHeader.SuspendLayout();
            panel6.SuspendLayout();
            panel7.SuspendLayout();
            panel8.SuspendLayout();
            pnlRequestContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRequests).BeginInit();
            pnlRequestPagination.SuspendLayout();
            tableLayoutPanel4.SuspendLayout();
            panel9.SuspendLayout();
            panel10.SuspendLayout();
            tabTransferExecution.SuspendLayout();
            tableLayoutPanel5.SuspendLayout();
            pnlTransferHeader.SuspendLayout();
            panel11.SuspendLayout();
            panel12.SuspendLayout();
            panel13.SuspendLayout();
            pnlTransferExecution.SuspendLayout();
            tableLayoutPanel6.SuspendLayout();
            groupBoxAvailableIngredients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAvailableIngredients).BeginInit();
            pnlTransferButtons.SuspendLayout();
            groupBoxTransferList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvTransferList).BeginInit();
            pnlTransferActions.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabInventoryTransactions);
            tabControl.Controls.Add(tabInventoryRequests);
            tabControl.Controls.Add(tabTransferExecution);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(10, 10);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(980, 620);
            tabControl.TabIndex = 0;
            // 
            // tabInventoryTransactions
            // 
            tabInventoryTransactions.BackColor = Color.FromArgb(24, 28, 63);
            tabInventoryTransactions.Controls.Add(tableLayoutPanel1);
            tabInventoryTransactions.Location = new Point(4, 24);
            tabInventoryTransactions.Name = "tabInventoryTransactions";
            tabInventoryTransactions.Size = new Size(972, 592);
            tabInventoryTransactions.TabIndex = 0;
            tabInventoryTransactions.Text = "Lịch sử nhập xuất";
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.BackColor = Color.FromArgb(24, 28, 63);
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(pnlTransactionHeader, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlTransactionContent, 0, 1);
            tableLayoutPanel1.Controls.Add(pnlTransactionPagination, 0, 2);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(0, 0);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(972, 592);
            tableLayoutPanel1.TabIndex = 0;
            // 
            // pnlTransactionHeader
            // 
            pnlTransactionHeader.ColumnCount = 5;
            pnlTransactionHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            pnlTransactionHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            pnlTransactionHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlTransactionHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            pnlTransactionHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 128F));
            pnlTransactionHeader.Controls.Add(panel1, 0, 0);
            pnlTransactionHeader.Controls.Add(panel2, 1, 0);
            pnlTransactionHeader.Controls.Add(panel3, 2, 0);
            pnlTransactionHeader.Controls.Add(btnViewTransactionDetails, 3, 0);
            pnlTransactionHeader.Controls.Add(btnCreateTransaction, 4, 0);
            pnlTransactionHeader.Dock = DockStyle.Fill;
            pnlTransactionHeader.Location = new Point(3, 3);
            pnlTransactionHeader.Name = "pnlTransactionHeader";
            pnlTransactionHeader.RowCount = 1;
            pnlTransactionHeader.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            pnlTransactionHeader.Size = new Size(966, 59);
            pnlTransactionHeader.TabIndex = 0;
            // 
            // panel1
            // 
            panel1.Controls.Add(btnTransactionFilter1);
            panel1.Controls.Add(lblTransactionFilter1);
            panel1.Controls.Add(cbxTransactionFilter1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(5, 5);
            panel1.Margin = new Padding(5);
            panel1.Name = "panel1";
            panel1.Size = new Size(136, 49);
            panel1.TabIndex = 3;
            // 
            // btnTransactionFilter1
            // 
            btnTransactionFilter1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnTransactionFilter1.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnTransactionFilter1.FlatStyle = FlatStyle.Flat;
            btnTransactionFilter1.Font = new Font("Microsoft Sans Serif", 9F);
            btnTransactionFilter1.ForeColor = Color.FromArgb(192, 255, 192);
            btnTransactionFilter1.Location = new Point(0, 22);
            btnTransactionFilter1.Name = "btnTransactionFilter1";
            btnTransactionFilter1.Size = new Size(136, 26);
            btnTransactionFilter1.TabIndex = 0;
            btnTransactionFilter1.Text = "Loại giao dịch";
            btnTransactionFilter1.TextAlign = ContentAlignment.TopLeft;
            btnTransactionFilter1.UseVisualStyleBackColor = true;
            // 
            // lblTransactionFilter1
            // 
            lblTransactionFilter1.AutoSize = true;
            lblTransactionFilter1.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransactionFilter1.Location = new Point(0, 3);
            lblTransactionFilter1.Name = "lblTransactionFilter1";
            lblTransactionFilter1.Size = new Size(84, 15);
            lblTransactionFilter1.TabIndex = 1;
            lblTransactionFilter1.Text = "Loại giao dịch:";
            // 
            // cbxTransactionFilter1
            // 
            cbxTransactionFilter1.BackColor = Color.FromArgb(42, 45, 86);
            cbxTransactionFilter1.Dock = DockStyle.Bottom;
            cbxTransactionFilter1.FlatStyle = FlatStyle.Flat;
            cbxTransactionFilter1.ForeColor = Color.WhiteSmoke;
            cbxTransactionFilter1.FormattingEnabled = true;
            cbxTransactionFilter1.Location = new Point(0, 26);
            cbxTransactionFilter1.Name = "cbxTransactionFilter1";
            cbxTransactionFilter1.Size = new Size(136, 23);
            cbxTransactionFilter1.TabIndex = 2;
            // 
            // panel2
            // 
            panel2.Controls.Add(btnTransactionFilter2);
            panel2.Controls.Add(lblTransactionFilter2);
            panel2.Controls.Add(cbxTransactionFilter2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(151, 5);
            panel2.Margin = new Padding(5);
            panel2.Name = "panel2";
            panel2.Size = new Size(136, 49);
            panel2.TabIndex = 4;
            // 
            // btnTransactionFilter2
            // 
            btnTransactionFilter2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnTransactionFilter2.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnTransactionFilter2.FlatStyle = FlatStyle.Flat;
            btnTransactionFilter2.Font = new Font("Microsoft Sans Serif", 9F);
            btnTransactionFilter2.ForeColor = Color.FromArgb(192, 255, 192);
            btnTransactionFilter2.Location = new Point(0, 22);
            btnTransactionFilter2.Name = "btnTransactionFilter2";
            btnTransactionFilter2.Size = new Size(136, 26);
            btnTransactionFilter2.TabIndex = 0;
            btnTransactionFilter2.Text = "Chi nhánh";
            btnTransactionFilter2.TextAlign = ContentAlignment.TopLeft;
            btnTransactionFilter2.UseVisualStyleBackColor = true;
            // 
            // lblTransactionFilter2
            // 
            lblTransactionFilter2.AutoSize = true;
            lblTransactionFilter2.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransactionFilter2.Location = new Point(0, 3);
            lblTransactionFilter2.Name = "lblTransactionFilter2";
            lblTransactionFilter2.Size = new Size(65, 15);
            lblTransactionFilter2.TabIndex = 1;
            lblTransactionFilter2.Text = "Chi nhánh:";
            // 
            // cbxTransactionFilter2
            // 
            cbxTransactionFilter2.BackColor = Color.FromArgb(42, 45, 86);
            cbxTransactionFilter2.Dock = DockStyle.Bottom;
            cbxTransactionFilter2.FlatStyle = FlatStyle.Flat;
            cbxTransactionFilter2.ForeColor = Color.WhiteSmoke;
            cbxTransactionFilter2.FormattingEnabled = true;
            cbxTransactionFilter2.Location = new Point(0, 26);
            cbxTransactionFilter2.Name = "cbxTransactionFilter2";
            cbxTransactionFilter2.Size = new Size(136, 23);
            cbxTransactionFilter2.TabIndex = 2;
            // 
            // panel3
            // 
            panel3.Controls.Add(tbxTransactionSearch);
            panel3.Controls.Add(lblTransactionSearch);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(297, 5);
            panel3.Margin = new Padding(5);
            panel3.Name = "panel3";
            panel3.Padding = new Padding(5, 0, 5, 5);
            panel3.Size = new Size(438, 49);
            panel3.TabIndex = 5;
            // 
            // tbxTransactionSearch
            // 
            tbxTransactionSearch.BackColor = Color.FromArgb(42, 45, 86);
            tbxTransactionSearch.BorderStyle = BorderStyle.None;
            tbxTransactionSearch.Dock = DockStyle.Bottom;
            tbxTransactionSearch.ForeColor = Color.FromArgb(192, 255, 192);
            tbxTransactionSearch.Location = new Point(5, 28);
            tbxTransactionSearch.Name = "tbxTransactionSearch";
            tbxTransactionSearch.PlaceholderText = "Mã phiếu/Nguyên liệu cần tìm";
            tbxTransactionSearch.Size = new Size(428, 16);
            tbxTransactionSearch.TabIndex = 0;
            // 
            // lblTransactionSearch
            // 
            lblTransactionSearch.AutoSize = true;
            lblTransactionSearch.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransactionSearch.Location = new Point(6, 3);
            lblTransactionSearch.Name = "lblTransactionSearch";
            lblTransactionSearch.Size = new Size(184, 15);
            lblTransactionSearch.TabIndex = 1;
            lblTransactionSearch.Text = "Tìm kiếm theo (Mã/Nguyên liệu):";
            // 
            // btnViewTransactionDetails
            // 
            btnViewTransactionDetails.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnViewTransactionDetails.FlatAppearance.BorderColor = Color.Fuchsia;
            btnViewTransactionDetails.FlatStyle = FlatStyle.Flat;
            btnViewTransactionDetails.Font = new Font("Microsoft Sans Serif", 11F);
            btnViewTransactionDetails.ForeColor = Color.FromArgb(192, 255, 192);
            btnViewTransactionDetails.Location = new Point(745, 5);
            btnViewTransactionDetails.Margin = new Padding(5);
            btnViewTransactionDetails.Name = "btnViewTransactionDetails";
            btnViewTransactionDetails.Size = new Size(88, 49);
            btnViewTransactionDetails.TabIndex = 0;
            btnViewTransactionDetails.Text = "Chi tiết";
            btnViewTransactionDetails.UseVisualStyleBackColor = true;
            // 
            // btnCreateTransaction
            // 
            btnCreateTransaction.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnCreateTransaction.FlatAppearance.BorderColor = Color.Cyan;
            btnCreateTransaction.FlatStyle = FlatStyle.Flat;
            btnCreateTransaction.Font = new Font("Microsoft Sans Serif", 11F);
            btnCreateTransaction.ForeColor = Color.FromArgb(255, 224, 192);
            btnCreateTransaction.Location = new Point(843, 5);
            btnCreateTransaction.Margin = new Padding(5);
            btnCreateTransaction.Name = "btnCreateTransaction";
            btnCreateTransaction.Size = new Size(118, 49);
            btnCreateTransaction.TabIndex = 1;
            btnCreateTransaction.Text = "Tạo mới";
            btnCreateTransaction.UseVisualStyleBackColor = true;
            // 
            // pnlTransactionContent
            // 
            pnlTransactionContent.BackColor = Color.FromArgb(42, 45, 86);
            pnlTransactionContent.Controls.Add(dgvTransactions);
            pnlTransactionContent.Dock = DockStyle.Fill;
            pnlTransactionContent.Location = new Point(3, 68);
            pnlTransactionContent.Name = "pnlTransactionContent";
            pnlTransactionContent.Size = new Size(966, 481);
            pnlTransactionContent.TabIndex = 1;
            // 
            // dgvTransactions
            // 
            dgvTransactions.AllowUserToAddRows = false;
            dgvTransactions.AllowUserToDeleteRows = false;
            dgvTransactions.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(42, 45, 86);
            dgvTransactions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvTransactions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvTransactions.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvTransactions.BorderStyle = BorderStyle.None;
            dgvTransactions.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvTransactions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvTransactions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvTransactions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvTransactions.DefaultCellStyle = dataGridViewCellStyle3;
            dgvTransactions.Dock = DockStyle.Fill;
            dgvTransactions.EnableHeadersVisualStyles = false;
            dgvTransactions.GridColor = Color.FromArgb(73, 75, 111);
            dgvTransactions.Location = new Point(0, 0);
            dgvTransactions.MultiSelect = false;
            dgvTransactions.Name = "dgvTransactions";
            dgvTransactions.ReadOnly = true;
            dgvTransactions.RowHeadersVisible = false;
            dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransactions.Size = new Size(966, 481);
            dgvTransactions.TabIndex = 0;
            // 
            // pnlTransactionPagination
            // 
            pnlTransactionPagination.Controls.Add(tableLayoutPanel2);
            pnlTransactionPagination.Dock = DockStyle.Fill;
            pnlTransactionPagination.Location = new Point(3, 555);
            pnlTransactionPagination.Name = "pnlTransactionPagination";
            pnlTransactionPagination.Size = new Size(966, 34);
            pnlTransactionPagination.TabIndex = 2;
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
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutPanel2.Size = new Size(966, 34);
            tableLayoutPanel2.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(lblTransactionRecords);
            panel4.Controls.Add(lblTransactionPageSize);
            panel4.Controls.Add(cbxTransactionPageSize);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 3);
            panel4.Name = "panel4";
            panel4.Size = new Size(477, 28);
            panel4.TabIndex = 0;
            // 
            // lblTransactionRecords
            // 
            lblTransactionRecords.AutoSize = true;
            lblTransactionRecords.Font = new Font("Segoe UI", 13F);
            lblTransactionRecords.ForeColor = Color.FromArgb(255, 192, 128);
            lblTransactionRecords.Location = new Point(5, 3);
            lblTransactionRecords.Name = "lblTransactionRecords";
            lblTransactionRecords.Size = new Size(104, 25);
            lblTransactionRecords.TabIndex = 0;
            lblTransactionRecords.Text = "Số lượng: 0";
            // 
            // lblTransactionPageSize
            // 
            lblTransactionPageSize.AutoSize = true;
            lblTransactionPageSize.Font = new Font("Segoe UI", 12F);
            lblTransactionPageSize.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransactionPageSize.Location = new Point(196, 6);
            lblTransactionPageSize.Name = "lblTransactionPageSize";
            lblTransactionPageSize.Size = new Size(64, 21);
            lblTransactionPageSize.TabIndex = 1;
            lblTransactionPageSize.Text = "Hiển thị";
            // 
            // cbxTransactionPageSize
            // 
            cbxTransactionPageSize.BackColor = Color.FromArgb(42, 45, 86);
            cbxTransactionPageSize.ForeColor = Color.White;
            cbxTransactionPageSize.FormattingEnabled = true;
            cbxTransactionPageSize.Items.AddRange(new object[] { "10", "25", "50", "100" });
            cbxTransactionPageSize.Location = new Point(266, 6);
            cbxTransactionPageSize.Name = "cbxTransactionPageSize";
            cbxTransactionPageSize.Size = new Size(60, 23);
            cbxTransactionPageSize.TabIndex = 2;
            // 
            // panel5
            // 
            panel5.Controls.Add(btnTransactionPreviousPage);
            panel5.Controls.Add(btnTransactionNextPage);
            panel5.Controls.Add(lblTransactionTotalPage);
            panel5.Controls.Add(btnTransactionPrevious);
            panel5.Controls.Add(btnTransactionNext);
            panel5.Controls.Add(lblTransactionPageInfo);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(486, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(477, 28);
            panel5.TabIndex = 1;
            // 
            // btnTransactionPreviousPage
            // 
            btnTransactionPreviousPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnTransactionPreviousPage.FlatAppearance.BorderColor = Color.Cyan;
            btnTransactionPreviousPage.FlatStyle = FlatStyle.Flat;
            btnTransactionPreviousPage.ForeColor = Color.FromArgb(255, 224, 192);
            btnTransactionPreviousPage.Location = new Point(287, 1);
            btnTransactionPreviousPage.Name = "btnTransactionPreviousPage";
            btnTransactionPreviousPage.Size = new Size(88, 27);
            btnTransactionPreviousPage.TabIndex = 3;
            btnTransactionPreviousPage.Text = "Trang trước";
            // 
            // btnTransactionNextPage
            // 
            btnTransactionNextPage.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnTransactionNextPage.FlatAppearance.BorderColor = Color.Fuchsia;
            btnTransactionNextPage.FlatStyle = FlatStyle.Flat;
            btnTransactionNextPage.ForeColor = Color.FromArgb(192, 255, 192);
            btnTransactionNextPage.Location = new Point(385, 1);
            btnTransactionNextPage.Name = "btnTransactionNextPage";
            btnTransactionNextPage.Size = new Size(89, 27);
            btnTransactionNextPage.TabIndex = 4;
            btnTransactionNextPage.Text = "Trang sau";
            // 
            // lblTransactionTotalPage
            // 
            lblTransactionTotalPage.AutoSize = true;
            lblTransactionTotalPage.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransactionTotalPage.Location = new Point(160, 7);
            lblTransactionTotalPage.Name = "lblTransactionTotalPage";
            lblTransactionTotalPage.Size = new Size(105, 15);
            lblTransactionTotalPage.TabIndex = 5;
            lblTransactionTotalPage.Text = "Hiện trang 1 trên 1";
            // 
            // btnTransactionPrevious
            // 
            btnTransactionPrevious.Anchor = AnchorStyles.Right;
            btnTransactionPrevious.FlatAppearance.BorderColor = Color.Cyan;
            btnTransactionPrevious.FlatStyle = FlatStyle.Flat;
            btnTransactionPrevious.Font = new Font("Microsoft Sans Serif", 10F);
            btnTransactionPrevious.ForeColor = Color.FromArgb(255, 224, 192);
            btnTransactionPrevious.Location = new Point(527, -36);
            btnTransactionPrevious.Name = "btnTransactionPrevious";
            btnTransactionPrevious.Size = new Size(106, 28);
            btnTransactionPrevious.TabIndex = 0;
            btnTransactionPrevious.Text = "Trang trước";
            btnTransactionPrevious.UseVisualStyleBackColor = true;
            // 
            // btnTransactionNext
            // 
            btnTransactionNext.Anchor = AnchorStyles.Right;
            btnTransactionNext.FlatAppearance.BorderColor = Color.Fuchsia;
            btnTransactionNext.FlatStyle = FlatStyle.Flat;
            btnTransactionNext.Font = new Font("Microsoft Sans Serif", 10F);
            btnTransactionNext.ForeColor = Color.FromArgb(192, 255, 192);
            btnTransactionNext.Location = new Point(639, -36);
            btnTransactionNext.Name = "btnTransactionNext";
            btnTransactionNext.Size = new Size(106, 28);
            btnTransactionNext.TabIndex = 1;
            btnTransactionNext.Text = "Trang sau";
            btnTransactionNext.UseVisualStyleBackColor = true;
            // 
            // lblTransactionPageInfo
            // 
            lblTransactionPageInfo.Anchor = AnchorStyles.Right;
            lblTransactionPageInfo.AutoSize = true;
            lblTransactionPageInfo.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransactionPageInfo.Location = new Point(377, -30);
            lblTransactionPageInfo.Name = "lblTransactionPageInfo";
            lblTransactionPageInfo.Size = new Size(105, 15);
            lblTransactionPageInfo.TabIndex = 2;
            lblTransactionPageInfo.Text = "Hiện trang 1 trên 1";
            // 
            // tabInventoryRequests
            // 
            tabInventoryRequests.Controls.Add(tableLayoutPanel3);
            tabInventoryRequests.Location = new Point(4, 24);
            tabInventoryRequests.Name = "tabInventoryRequests";
            tabInventoryRequests.Size = new Size(972, 592);
            tabInventoryRequests.TabIndex = 1;
            tabInventoryRequests.Text = "Yêu cầu";
            // 
            // tableLayoutPanel3
            // 
            tableLayoutPanel3.BackColor = Color.FromArgb(24, 28, 63);
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(pnlRequestHeader, 0, 0);
            tableLayoutPanel3.Controls.Add(pnlRequestContent, 0, 1);
            tableLayoutPanel3.Controls.Add(pnlRequestPagination, 0, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(0, 0);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 65F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel3.Size = new Size(972, 592);
            tableLayoutPanel3.TabIndex = 0;
            // 
            // pnlRequestHeader
            // 
            pnlRequestHeader.ColumnCount = 7;
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 102F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlRequestHeader.Controls.Add(lblTransferTitle, 0, 0);
            pnlRequestHeader.Controls.Add(panel6, 1, 0);
            pnlRequestHeader.Controls.Add(panel7, 2, 0);
            pnlRequestHeader.Controls.Add(panel8, 3, 0);
            pnlRequestHeader.Controls.Add(btnCreateRequest, 5, 0);
            pnlRequestHeader.Controls.Add(btnApproveRequest, 6, 0);
            pnlRequestHeader.Controls.Add(btnViewRequestDetails, 4, 0);
            pnlRequestHeader.Dock = DockStyle.Fill;
            pnlRequestHeader.Location = new Point(3, 3);
            pnlRequestHeader.Name = "pnlRequestHeader";
            pnlRequestHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            pnlRequestHeader.Size = new Size(966, 59);
            pnlRequestHeader.TabIndex = 0;
            // 
            // lblTransferTitle
            // 
            lblTransferTitle.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            lblTransferTitle.ForeColor = Color.FromArgb(255, 224, 192);
            lblTransferTitle.Location = new Point(3, 0);
            lblTransferTitle.Name = "lblTransferTitle";
            lblTransferTitle.Size = new Size(114, 58);
            lblTransferTitle.TabIndex = 2;
            lblTransferTitle.Text = "YÊU CẦU ";
            lblTransferTitle.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel6
            // 
            panel6.Controls.Add(btnRequestFilter1);
            panel6.Controls.Add(lblRequestFilter1);
            panel6.Controls.Add(cbxRequestFilter1);
            panel6.Dock = DockStyle.Fill;
            panel6.Location = new Point(123, 3);
            panel6.Name = "panel6";
            panel6.Size = new Size(92, 53);
            panel6.TabIndex = 0;
            // 
            // btnRequestFilter1
            // 
            btnRequestFilter1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnRequestFilter1.FlatStyle = FlatStyle.Flat;
            btnRequestFilter1.ForeColor = Color.FromArgb(192, 255, 192);
            btnRequestFilter1.Location = new Point(0, 26);
            btnRequestFilter1.Name = "btnRequestFilter1";
            btnRequestFilter1.Size = new Size(92, 26);
            btnRequestFilter1.TabIndex = 0;
            btnRequestFilter1.Text = "Trạng thái";
            btnRequestFilter1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblRequestFilter1
            // 
            lblRequestFilter1.AutoSize = true;
            lblRequestFilter1.ForeColor = Color.FromArgb(124, 141, 181);
            lblRequestFilter1.Location = new Point(3, 3);
            lblRequestFilter1.Name = "lblRequestFilter1";
            lblRequestFilter1.Size = new Size(63, 15);
            lblRequestFilter1.TabIndex = 1;
            lblRequestFilter1.Text = "Trạng thái:";
            // 
            // cbxRequestFilter1
            // 
            cbxRequestFilter1.BackColor = Color.FromArgb(42, 45, 86);
            cbxRequestFilter1.Dock = DockStyle.Bottom;
            cbxRequestFilter1.ForeColor = Color.WhiteSmoke;
            cbxRequestFilter1.Location = new Point(0, 30);
            cbxRequestFilter1.Name = "cbxRequestFilter1";
            cbxRequestFilter1.Size = new Size(92, 23);
            cbxRequestFilter1.TabIndex = 2;
            // 
            // panel7
            // 
            panel7.Controls.Add(lblRequestFilter2);
            panel7.Controls.Add(btnRequestFilter2);
            panel7.Controls.Add(cbxRequestFilter2);
            panel7.Dock = DockStyle.Fill;
            panel7.Location = new Point(221, 3);
            panel7.Name = "panel7";
            panel7.Size = new Size(96, 53);
            panel7.TabIndex = 1;
            // 
            // lblRequestFilter2
            // 
            lblRequestFilter2.ForeColor = Color.FromArgb(124, 141, 181);
            lblRequestFilter2.Location = new Point(3, 3);
            lblRequestFilter2.Name = "lblRequestFilter2";
            lblRequestFilter2.Size = new Size(90, 23);
            lblRequestFilter2.TabIndex = 1;
            lblRequestFilter2.Text = "Chi nhánh:";
            // 
            // btnRequestFilter2
            // 
            btnRequestFilter2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnRequestFilter2.FlatStyle = FlatStyle.Flat;
            btnRequestFilter2.ForeColor = Color.FromArgb(192, 255, 192);
            btnRequestFilter2.Location = new Point(0, 26);
            btnRequestFilter2.Name = "btnRequestFilter2";
            btnRequestFilter2.Size = new Size(96, 26);
            btnRequestFilter2.TabIndex = 0;
            btnRequestFilter2.Text = "Chi nhánh";
            btnRequestFilter2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cbxRequestFilter2
            // 
            cbxRequestFilter2.BackColor = Color.FromArgb(42, 45, 86);
            cbxRequestFilter2.Dock = DockStyle.Bottom;
            cbxRequestFilter2.ForeColor = Color.WhiteSmoke;
            cbxRequestFilter2.Location = new Point(0, 30);
            cbxRequestFilter2.Name = "cbxRequestFilter2";
            cbxRequestFilter2.Size = new Size(96, 23);
            cbxRequestFilter2.TabIndex = 2;
            // 
            // panel8
            // 
            panel8.Controls.Add(tbxRequestSearch);
            panel8.Controls.Add(lblRequestSearch);
            panel8.Dock = DockStyle.Fill;
            panel8.Location = new Point(323, 3);
            panel8.Name = "panel8";
            panel8.Padding = new Padding(5, 0, 5, 5);
            panel8.Size = new Size(340, 53);
            panel8.TabIndex = 2;
            panel8.Paint += panel8_Paint;
            // 
            // tbxRequestSearch
            // 
            tbxRequestSearch.BackColor = Color.FromArgb(42, 45, 86);
            tbxRequestSearch.BorderStyle = BorderStyle.None;
            tbxRequestSearch.Dock = DockStyle.Bottom;
            tbxRequestSearch.ForeColor = Color.FromArgb(192, 255, 192);
            tbxRequestSearch.Location = new Point(5, 32);
            tbxRequestSearch.Name = "tbxRequestSearch";
            tbxRequestSearch.PlaceholderText = "Mã yêu cầu/Nguyên liệu";
            tbxRequestSearch.Size = new Size(330, 16);
            tbxRequestSearch.TabIndex = 0;
            // 
            // lblRequestSearch
            // 
            lblRequestSearch.ForeColor = Color.FromArgb(124, 141, 181);
            lblRequestSearch.Location = new Point(0, 0);
            lblRequestSearch.Name = "lblRequestSearch";
            lblRequestSearch.Size = new Size(100, 23);
            lblRequestSearch.TabIndex = 1;
            lblRequestSearch.Text = "Tìm kiếm theo (Mã/Nguyên liệu):";
            // 
            // btnCreateRequest
            // 
            btnCreateRequest.Dock = DockStyle.Fill;
            btnCreateRequest.FlatStyle = FlatStyle.Flat;
            btnCreateRequest.ForeColor = Color.FromArgb(255, 224, 192);
            btnCreateRequest.Location = new Point(769, 3);
            btnCreateRequest.Name = "btnCreateRequest";
            btnCreateRequest.Size = new Size(94, 53);
            btnCreateRequest.TabIndex = 4;
            btnCreateRequest.Text = "Tạo yêu cầu";
            // 
            // btnApproveRequest
            // 
            btnApproveRequest.Dock = DockStyle.Fill;
            btnApproveRequest.FlatStyle = FlatStyle.Flat;
            btnApproveRequest.ForeColor = Color.FromArgb(192, 255, 192);
            btnApproveRequest.Location = new Point(869, 3);
            btnApproveRequest.Name = "btnApproveRequest";
            btnApproveRequest.Size = new Size(94, 53);
            btnApproveRequest.TabIndex = 5;
            btnApproveRequest.Text = "Duyệt";
            // 
            // btnViewRequestDetails
            // 
            btnViewRequestDetails.Dock = DockStyle.Fill;
            btnViewRequestDetails.FlatStyle = FlatStyle.Flat;
            btnViewRequestDetails.ForeColor = Color.FromArgb(192, 255, 192);
            btnViewRequestDetails.Location = new Point(669, 3);
            btnViewRequestDetails.Name = "btnViewRequestDetails";
            btnViewRequestDetails.Size = new Size(94, 53);
            btnViewRequestDetails.TabIndex = 3;
            btnViewRequestDetails.Text = "Chi tiết";
            // 
            // pnlRequestContent
            // 
            pnlRequestContent.BackColor = Color.FromArgb(42, 45, 86);
            pnlRequestContent.Controls.Add(dgvRequests);
            pnlRequestContent.Dock = DockStyle.Fill;
            pnlRequestContent.Location = new Point(3, 68);
            pnlRequestContent.Name = "pnlRequestContent";
            pnlRequestContent.Size = new Size(966, 481);
            pnlRequestContent.TabIndex = 1;
            // 
            // dgvRequests
            // 
            dgvRequests.AllowUserToAddRows = false;
            dgvRequests.AllowUserToDeleteRows = false;
            dgvRequests.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvRequests.BorderStyle = BorderStyle.None;
            dgvRequests.Dock = DockStyle.Fill;
            dgvRequests.Location = new Point(0, 0);
            dgvRequests.Name = "dgvRequests";
            dgvRequests.ReadOnly = true;
            dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRequests.Size = new Size(966, 481);
            dgvRequests.TabIndex = 0;
            // 
            // pnlRequestPagination
            // 
            pnlRequestPagination.Controls.Add(tableLayoutPanel4);
            pnlRequestPagination.Dock = DockStyle.Fill;
            pnlRequestPagination.Location = new Point(3, 555);
            pnlRequestPagination.Name = "pnlRequestPagination";
            pnlRequestPagination.Size = new Size(966, 34);
            pnlRequestPagination.TabIndex = 2;
            // 
            // tableLayoutPanel4
            // 
            tableLayoutPanel4.ColumnCount = 2;
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel4.Controls.Add(panel9, 0, 0);
            tableLayoutPanel4.Controls.Add(panel10, 1, 0);
            tableLayoutPanel4.Dock = DockStyle.Fill;
            tableLayoutPanel4.Location = new Point(0, 0);
            tableLayoutPanel4.Name = "tableLayoutPanel4";
            tableLayoutPanel4.RowCount = 1;
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel4.Size = new Size(966, 34);
            tableLayoutPanel4.TabIndex = 0;
            // 
            // panel9
            // 
            panel9.Controls.Add(lblRequestRecords);
            panel9.Controls.Add(lblRequestPageSize);
            panel9.Controls.Add(cbxRequestPageSize);
            panel9.Dock = DockStyle.Fill;
            panel9.Location = new Point(3, 3);
            panel9.Name = "panel9";
            panel9.Size = new Size(477, 28);
            panel9.TabIndex = 0;
            // 
            // lblRequestRecords
            // 
            lblRequestRecords.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblRequestRecords.AutoSize = true;
            lblRequestRecords.Font = new Font("Segoe UI", 13F);
            lblRequestRecords.ForeColor = Color.FromArgb(255, 192, 128);
            lblRequestRecords.Location = new Point(5, 3);
            lblRequestRecords.Name = "lblRequestRecords";
            lblRequestRecords.Size = new Size(104, 25);
            lblRequestRecords.TabIndex = 0;
            lblRequestRecords.Text = "Số lượng: 0";
            // 
            // lblRequestPageSize
            // 
            lblRequestPageSize.AutoSize = true;
            lblRequestPageSize.Font = new Font("Segoe UI", 12F);
            lblRequestPageSize.ForeColor = Color.FromArgb(124, 141, 181);
            lblRequestPageSize.Location = new Point(196, 6);
            lblRequestPageSize.Name = "lblRequestPageSize";
            lblRequestPageSize.Size = new Size(64, 21);
            lblRequestPageSize.TabIndex = 1;
            lblRequestPageSize.Text = "Hiển thị";
            // 
            // cbxRequestPageSize
            // 
            cbxRequestPageSize.BackColor = Color.FromArgb(42, 45, 86);
            cbxRequestPageSize.ForeColor = Color.White;
            cbxRequestPageSize.Items.AddRange(new object[] { "10", "25", "50", "100" });
            cbxRequestPageSize.Location = new Point(266, 6);
            cbxRequestPageSize.Name = "cbxRequestPageSize";
            cbxRequestPageSize.Size = new Size(60, 23);
            cbxRequestPageSize.TabIndex = 2;
            // 
            // panel10
            // 
            panel10.Controls.Add(btnRequestPrevious);
            panel10.Controls.Add(btnRequestNext);
            panel10.Controls.Add(lblRequestPageInfo);
            panel10.Dock = DockStyle.Fill;
            panel10.Location = new Point(486, 3);
            panel10.Name = "panel10";
            panel10.Size = new Size(477, 28);
            panel10.TabIndex = 1;
            // 
            // btnRequestPrevious
            // 
            btnRequestPrevious.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnRequestPrevious.FlatAppearance.BorderColor = Color.Cyan;
            btnRequestPrevious.FlatStyle = FlatStyle.Flat;
            btnRequestPrevious.ForeColor = Color.FromArgb(255, 224, 192);
            btnRequestPrevious.Location = new Point(287, 1);
            btnRequestPrevious.Name = "btnRequestPrevious";
            btnRequestPrevious.Size = new Size(88, 27);
            btnRequestPrevious.TabIndex = 0;
            btnRequestPrevious.Text = "Trang trước";
            // 
            // btnRequestNext
            // 
            btnRequestNext.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnRequestNext.FlatAppearance.BorderColor = Color.Fuchsia;
            btnRequestNext.FlatStyle = FlatStyle.Flat;
            btnRequestNext.ForeColor = Color.FromArgb(192, 255, 192);
            btnRequestNext.Location = new Point(385, 1);
            btnRequestNext.Name = "btnRequestNext";
            btnRequestNext.Size = new Size(89, 27);
            btnRequestNext.TabIndex = 1;
            btnRequestNext.Text = "Trang sau";
            // 
            // lblRequestPageInfo
            // 
            lblRequestPageInfo.AutoSize = true;
            lblRequestPageInfo.ForeColor = Color.FromArgb(124, 141, 181);
            lblRequestPageInfo.Location = new Point(160, 7);
            lblRequestPageInfo.Name = "lblRequestPageInfo";
            lblRequestPageInfo.Size = new Size(105, 15);
            lblRequestPageInfo.TabIndex = 2;
            lblRequestPageInfo.Text = "Hiện trang 1 trên 1";
            // 
            // tabTransferExecution
            // 
            tabTransferExecution.Controls.Add(tableLayoutPanel5);
            tabTransferExecution.Location = new Point(4, 24);
            tabTransferExecution.Name = "tabTransferExecution";
            tabTransferExecution.Size = new Size(972, 592);
            tabTransferExecution.TabIndex = 2;
            tabTransferExecution.Text = "Chuyển kho";
            // 
            // tableLayoutPanel5
            // 
            tableLayoutPanel5.BackColor = Color.FromArgb(24, 28, 63);
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(pnlTransferHeader, 0, 0);
            tableLayoutPanel5.Controls.Add(pnlTransferExecution, 0, 1);
            tableLayoutPanel5.Controls.Add(pnlTransferActions, 0, 2);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(0, 0);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 3;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel5.Size = new Size(972, 592);
            tableLayoutPanel5.TabIndex = 0;
            // 
            // pnlTransferHeader
            // 
            pnlTransferHeader.ColumnCount = 7;
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 100F));
            pnlTransferHeader.Controls.Add(label1, 0, 0);
            pnlTransferHeader.Controls.Add(panel11, 1, 0);
            pnlTransferHeader.Controls.Add(panel12, 2, 0);
            pnlTransferHeader.Controls.Add(panel13, 3, 0);
            pnlTransferHeader.Controls.Add(btnImportExcel, 4, 0);
            pnlTransferHeader.Controls.Add(btnExecuteTransfer, 5, 0);
            pnlTransferHeader.Controls.Add(btnCreateTransfer, 6, 0);
            pnlTransferHeader.Dock = DockStyle.Fill;
            pnlTransferHeader.Location = new Point(3, 3);
            pnlTransferHeader.Name = "pnlTransferHeader";
            pnlTransferHeader.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            pnlTransferHeader.Size = new Size(966, 58);
            pnlTransferHeader.TabIndex = 0;
            // 
            // label1
            // 
            label1.Font = new Font("Segoe UI", 15F, FontStyle.Bold);
            label1.ForeColor = Color.FromArgb(255, 224, 192);
            label1.Location = new Point(3, 0);
            label1.Name = "label1";
            label1.Size = new Size(114, 58);
            label1.TabIndex = 6;
            label1.Text = "CHUYỂN KHO";
            label1.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // panel11
            // 
            panel11.Controls.Add(btnTransferFilter1);
            panel11.Controls.Add(lblTransferFilter1);
            panel11.Controls.Add(cbxTransferFilter1);
            panel11.Dock = DockStyle.Fill;
            panel11.Location = new Point(123, 3);
            panel11.Name = "panel11";
            panel11.Size = new Size(94, 52);
            panel11.TabIndex = 0;
            // 
            // btnTransferFilter1
            // 
            btnTransferFilter1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnTransferFilter1.FlatStyle = FlatStyle.Flat;
            btnTransferFilter1.ForeColor = Color.FromArgb(192, 255, 192);
            btnTransferFilter1.Location = new Point(0, 25);
            btnTransferFilter1.Name = "btnTransferFilter1";
            btnTransferFilter1.Size = new Size(94, 26);
            btnTransferFilter1.TabIndex = 0;
            btnTransferFilter1.Text = "Loại chuyển";
            // 
            // lblTransferFilter1
            // 
            lblTransferFilter1.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransferFilter1.Location = new Point(0, 0);
            lblTransferFilter1.Name = "lblTransferFilter1";
            lblTransferFilter1.Size = new Size(100, 23);
            lblTransferFilter1.TabIndex = 1;
            lblTransferFilter1.Text = "Loại chuyển:";
            // 
            // cbxTransferFilter1
            // 
            cbxTransferFilter1.BackColor = Color.FromArgb(42, 45, 86);
            cbxTransferFilter1.Dock = DockStyle.Bottom;
            cbxTransferFilter1.ForeColor = Color.WhiteSmoke;
            cbxTransferFilter1.Location = new Point(0, 29);
            cbxTransferFilter1.Name = "cbxTransferFilter1";
            cbxTransferFilter1.Size = new Size(94, 23);
            cbxTransferFilter1.TabIndex = 2;
            // 
            // panel12
            // 
            panel12.Controls.Add(btnTransferFilter2);
            panel12.Controls.Add(lblTransferFilter2);
            panel12.Controls.Add(cbxTransferFilter2);
            panel12.Dock = DockStyle.Fill;
            panel12.Location = new Point(223, 3);
            panel12.Name = "panel12";
            panel12.Size = new Size(94, 52);
            panel12.TabIndex = 1;
            // 
            // btnTransferFilter2
            // 
            btnTransferFilter2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            btnTransferFilter2.FlatStyle = FlatStyle.Flat;
            btnTransferFilter2.ForeColor = Color.FromArgb(192, 255, 192);
            btnTransferFilter2.Location = new Point(0, 28);
            btnTransferFilter2.Name = "btnTransferFilter2";
            btnTransferFilter2.Size = new Size(94, 23);
            btnTransferFilter2.TabIndex = 0;
            btnTransferFilter2.Text = "Chi nhánh";
            // 
            // lblTransferFilter2
            // 
            lblTransferFilter2.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransferFilter2.Location = new Point(0, 0);
            lblTransferFilter2.Name = "lblTransferFilter2";
            lblTransferFilter2.Size = new Size(100, 23);
            lblTransferFilter2.TabIndex = 1;
            lblTransferFilter2.Text = "Chi nhánh:";
            // 
            // cbxTransferFilter2
            // 
            cbxTransferFilter2.BackColor = Color.FromArgb(42, 45, 86);
            cbxTransferFilter2.Dock = DockStyle.Bottom;
            cbxTransferFilter2.ForeColor = Color.WhiteSmoke;
            cbxTransferFilter2.Location = new Point(0, 29);
            cbxTransferFilter2.Name = "cbxTransferFilter2";
            cbxTransferFilter2.Size = new Size(94, 23);
            cbxTransferFilter2.TabIndex = 2;
            // 
            // panel13
            // 
            panel13.Controls.Add(tbxTransferSearch);
            panel13.Controls.Add(lblTransferSearch);
            panel13.Dock = DockStyle.Fill;
            panel13.Location = new Point(323, 3);
            panel13.Name = "panel13";
            panel13.Padding = new Padding(5, 0, 5, 5);
            panel13.Size = new Size(340, 52);
            panel13.TabIndex = 2;
            // 
            // tbxTransferSearch
            // 
            tbxTransferSearch.BackColor = Color.FromArgb(42, 45, 86);
            tbxTransferSearch.BorderStyle = BorderStyle.None;
            tbxTransferSearch.Dock = DockStyle.Bottom;
            tbxTransferSearch.ForeColor = Color.FromArgb(192, 255, 192);
            tbxTransferSearch.Location = new Point(5, 31);
            tbxTransferSearch.Name = "tbxTransferSearch";
            tbxTransferSearch.PlaceholderText = "Nguyên liệu cần chuyển";
            tbxTransferSearch.Size = new Size(330, 16);
            tbxTransferSearch.TabIndex = 0;
            // 
            // lblTransferSearch
            // 
            lblTransferSearch.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransferSearch.Location = new Point(0, 0);
            lblTransferSearch.Name = "lblTransferSearch";
            lblTransferSearch.Size = new Size(100, 23);
            lblTransferSearch.TabIndex = 1;
            lblTransferSearch.Text = "Tìm kiếm nguyên liệu:";
            // 
            // btnImportExcel
            // 
            btnImportExcel.Dock = DockStyle.Fill;
            btnImportExcel.FlatStyle = FlatStyle.Flat;
            btnImportExcel.ForeColor = Color.FromArgb(255, 192, 128);
            btnImportExcel.Location = new Point(669, 3);
            btnImportExcel.Name = "btnImportExcel";
            btnImportExcel.Size = new Size(94, 52);
            btnImportExcel.TabIndex = 3;
            btnImportExcel.Text = "Import Excel";
            // 
            // btnExecuteTransfer
            // 
            btnExecuteTransfer.Dock = DockStyle.Fill;
            btnExecuteTransfer.FlatStyle = FlatStyle.Flat;
            btnExecuteTransfer.ForeColor = Color.FromArgb(192, 255, 192);
            btnExecuteTransfer.Location = new Point(769, 3);
            btnExecuteTransfer.Name = "btnExecuteTransfer";
            btnExecuteTransfer.Size = new Size(94, 52);
            btnExecuteTransfer.TabIndex = 4;
            btnExecuteTransfer.Text = "Thực hiện";
            // 
            // btnCreateTransfer
            // 
            btnCreateTransfer.Dock = DockStyle.Fill;
            btnCreateTransfer.FlatStyle = FlatStyle.Flat;
            btnCreateTransfer.ForeColor = Color.FromArgb(255, 224, 192);
            btnCreateTransfer.Location = new Point(869, 3);
            btnCreateTransfer.Name = "btnCreateTransfer";
            btnCreateTransfer.Size = new Size(94, 52);
            btnCreateTransfer.TabIndex = 5;
            btnCreateTransfer.Text = "Tạo phiếu";
            // 
            // pnlTransferExecution
            // 
            pnlTransferExecution.Controls.Add(tableLayoutPanel6);
            pnlTransferExecution.Dock = DockStyle.Fill;
            pnlTransferExecution.Location = new Point(3, 67);
            pnlTransferExecution.Name = "pnlTransferExecution";
            pnlTransferExecution.Size = new Size(966, 462);
            pnlTransferExecution.TabIndex = 1;
            // 
            // tableLayoutPanel6
            // 
            tableLayoutPanel6.ColumnCount = 3;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel6.Controls.Add(groupBoxAvailableIngredients, 0, 0);
            tableLayoutPanel6.Controls.Add(pnlTransferButtons, 1, 0);
            tableLayoutPanel6.Controls.Add(groupBoxTransferList, 2, 0);
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.Location = new Point(0, 0);
            tableLayoutPanel6.Name = "tableLayoutPanel6";
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel6.Size = new Size(966, 462);
            tableLayoutPanel6.TabIndex = 0;
            // 
            // groupBoxAvailableIngredients
            // 
            groupBoxAvailableIngredients.Controls.Add(dgvAvailableIngredients);
            groupBoxAvailableIngredients.Dock = DockStyle.Fill;
            groupBoxAvailableIngredients.ForeColor = Color.White;
            groupBoxAvailableIngredients.Location = new Point(3, 3);
            groupBoxAvailableIngredients.Name = "groupBoxAvailableIngredients";
            groupBoxAvailableIngredients.Size = new Size(428, 456);
            groupBoxAvailableIngredients.TabIndex = 0;
            groupBoxAvailableIngredients.TabStop = false;
            groupBoxAvailableIngredients.Text = "Nguyên liệu có sẵn";
            // 
            // dgvAvailableIngredients
            // 
            dgvAvailableIngredients.AllowUserToAddRows = false;
            dgvAvailableIngredients.AllowUserToDeleteRows = false;
            dgvAvailableIngredients.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvAvailableIngredients.BorderStyle = BorderStyle.None;
            dgvAvailableIngredients.Dock = DockStyle.Fill;
            dgvAvailableIngredients.Location = new Point(3, 19);
            dgvAvailableIngredients.Name = "dgvAvailableIngredients";
            dgvAvailableIngredients.ReadOnly = true;
            dgvAvailableIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailableIngredients.Size = new Size(422, 434);
            dgvAvailableIngredients.TabIndex = 0;
            // 
            // pnlTransferButtons
            // 
            pnlTransferButtons.Controls.Add(btnAddToTransfer);
            pnlTransferButtons.Controls.Add(btnRemoveFromTransfer);
            pnlTransferButtons.Controls.Add(btnClearTransfer);
            pnlTransferButtons.Dock = DockStyle.Fill;
            pnlTransferButtons.Location = new Point(437, 3);
            pnlTransferButtons.Name = "pnlTransferButtons";
            pnlTransferButtons.Size = new Size(90, 456);
            pnlTransferButtons.TabIndex = 1;
            // 
            // btnAddToTransfer
            // 
            btnAddToTransfer.FlatStyle = FlatStyle.Flat;
            btnAddToTransfer.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnAddToTransfer.Location = new Point(0, 0);
            btnAddToTransfer.Name = "btnAddToTransfer";
            btnAddToTransfer.Size = new Size(75, 23);
            btnAddToTransfer.TabIndex = 0;
            btnAddToTransfer.Text = "→";
            // 
            // btnRemoveFromTransfer
            // 
            btnRemoveFromTransfer.FlatStyle = FlatStyle.Flat;
            btnRemoveFromTransfer.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnRemoveFromTransfer.Location = new Point(0, 0);
            btnRemoveFromTransfer.Name = "btnRemoveFromTransfer";
            btnRemoveFromTransfer.Size = new Size(75, 23);
            btnRemoveFromTransfer.TabIndex = 1;
            btnRemoveFromTransfer.Text = "←";
            // 
            // btnClearTransfer
            // 
            btnClearTransfer.FlatStyle = FlatStyle.Flat;
            btnClearTransfer.Location = new Point(0, 0);
            btnClearTransfer.Name = "btnClearTransfer";
            btnClearTransfer.Size = new Size(75, 23);
            btnClearTransfer.TabIndex = 2;
            btnClearTransfer.Text = "Clear";
            // 
            // groupBoxTransferList
            // 
            groupBoxTransferList.Controls.Add(dgvTransferList);
            groupBoxTransferList.Dock = DockStyle.Fill;
            groupBoxTransferList.ForeColor = Color.White;
            groupBoxTransferList.Location = new Point(533, 3);
            groupBoxTransferList.Name = "groupBoxTransferList";
            groupBoxTransferList.Size = new Size(430, 456);
            groupBoxTransferList.TabIndex = 2;
            groupBoxTransferList.TabStop = false;
            groupBoxTransferList.Text = "Danh sách chuyển kho";
            // 
            // dgvTransferList
            // 
            dgvTransferList.AllowUserToAddRows = false;
            dgvTransferList.AllowUserToDeleteRows = false;
            dgvTransferList.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvTransferList.BorderStyle = BorderStyle.None;
            dgvTransferList.Dock = DockStyle.Fill;
            dgvTransferList.Location = new Point(3, 19);
            dgvTransferList.Name = "dgvTransferList";
            dgvTransferList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransferList.Size = new Size(424, 434);
            dgvTransferList.TabIndex = 0;
            // 
            // pnlTransferActions
            // 
            pnlTransferActions.Controls.Add(btnSaveTransfer);
            pnlTransferActions.Controls.Add(btnCancelTransfer);
            pnlTransferActions.Dock = DockStyle.Fill;
            pnlTransferActions.Location = new Point(3, 535);
            pnlTransferActions.Name = "pnlTransferActions";
            pnlTransferActions.Size = new Size(966, 54);
            pnlTransferActions.TabIndex = 2;
            // 
            // btnSaveTransfer
            // 
            btnSaveTransfer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnSaveTransfer.FlatStyle = FlatStyle.Flat;
            btnSaveTransfer.ForeColor = Color.FromArgb(192, 255, 192);
            btnSaveTransfer.Location = new Point(850, 15);
            btnSaveTransfer.Name = "btnSaveTransfer";
            btnSaveTransfer.Size = new Size(94, 23);
            btnSaveTransfer.TabIndex = 1;
            btnSaveTransfer.Text = "Lưu phiếu";
            // 
            // btnCancelTransfer
            // 
            btnCancelTransfer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancelTransfer.FlatStyle = FlatStyle.Flat;
            btnCancelTransfer.ForeColor = Color.FromArgb(255, 128, 128);
            btnCancelTransfer.Location = new Point(747, 15);
            btnCancelTransfer.Name = "btnCancelTransfer";
            btnCancelTransfer.Size = new Size(97, 23);
            btnCancelTransfer.TabIndex = 2;
            btnCancelTransfer.Text = "Hủy";
            // 
            // FrmInventoryManagement
            // 
            ClientSize = new Size(1000, 640);
            Controls.Add(tabControl);
            Name = "FrmInventoryManagement";
            Padding = new Padding(10);
            Text = "Quản lý nhập xuất hàng hóa";
            tabControl.ResumeLayout(false);
            tabInventoryTransactions.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            pnlTransactionHeader.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            pnlTransactionContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTransactions).EndInit();
            pnlTransactionPagination.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            tabInventoryRequests.ResumeLayout(false);
            tableLayoutPanel3.ResumeLayout(false);
            pnlRequestHeader.ResumeLayout(false);
            panel6.ResumeLayout(false);
            panel6.PerformLayout();
            panel7.ResumeLayout(false);
            panel8.ResumeLayout(false);
            panel8.PerformLayout();
            pnlRequestContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRequests).EndInit();
            pnlRequestPagination.ResumeLayout(false);
            tableLayoutPanel4.ResumeLayout(false);
            panel9.ResumeLayout(false);
            panel9.PerformLayout();
            panel10.ResumeLayout(false);
            panel10.PerformLayout();
            tabTransferExecution.ResumeLayout(false);
            tableLayoutPanel5.ResumeLayout(false);
            pnlTransferHeader.ResumeLayout(false);
            panel11.ResumeLayout(false);
            panel12.ResumeLayout(false);
            panel13.ResumeLayout(false);
            panel13.PerformLayout();
            pnlTransferExecution.ResumeLayout(false);
            tableLayoutPanel6.ResumeLayout(false);
            groupBoxAvailableIngredients.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvAvailableIngredients).EndInit();
            pnlTransferButtons.ResumeLayout(false);
            groupBoxTransferList.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvTransferList).EndInit();
            pnlTransferActions.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        // Fields (all controls declared above; designer partial must match code-behind)
        private TabControl tabControl;
        private TabPage tabInventoryTransactions;
        private TabPage tabInventoryRequests;
        private TabPage tabTransferExecution;

        // Tab 1
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel pnlTransactionHeader;
        private Panel panel1;
        private Panel panel2;
        private Panel panel3;
        private Button btnTransactionFilter1;
        private Button btnTransactionFilter2;
        private Label lblTransactionFilter1;
        private Label lblTransactionFilter2;
        private Label lblTransactionSearch;
        private ComboBox cbxTransactionFilter1;
        private ComboBox cbxTransactionFilter2;
        private TextBox tbxTransactionSearch;
        private Button btnViewTransactionDetails;
        private Button btnCreateTransaction;
        private Panel pnlTransactionContent;
        private DataGridView dgvTransactions;
        private Panel pnlTransactionPagination;
        private TableLayoutPanel tableLayoutPanel2;

        // Tab 2
        private TableLayoutPanel tableLayoutPanel3;
        private TableLayoutPanel pnlRequestHeader;
        private Panel panel6;
        private Panel panel7;
        private Panel panel8;
        private Button btnRequestFilter1;
        private Button btnRequestFilter2;
        private Label lblRequestFilter1;
        private Label lblRequestFilter2;
        private Label lblRequestSearch;
        private ComboBox cbxRequestFilter1;
        private ComboBox cbxRequestFilter2;
        private TextBox tbxRequestSearch;
        private Button btnViewRequestDetails;
        private Button btnCreateRequest;
        private Button btnApproveRequest;
        private Panel pnlRequestContent;
        private DataGridView dgvRequests;
        private Panel pnlRequestPagination;
        private TableLayoutPanel tableLayoutPanel4;
        private Panel panel9;
        private Panel panel10;
        private Label lblRequestRecords;
        private Label lblRequestPageSize;
        private Label lblRequestPageInfo;
        private ComboBox cbxRequestPageSize;
        private Button btnRequestPrevious;
        private Button btnRequestNext;

        // Tab 3
        private TableLayoutPanel tableLayoutPanel5;
        private TableLayoutPanel pnlTransferHeader;
        private Panel panel11;
        private Panel panel12;
        private Panel panel13;
        private Button btnTransferFilter1;
        private Button btnTransferFilter2;
        private Label lblTransferFilter1;
        private Label lblTransferFilter2;
        private Label lblTransferSearch;
        private ComboBox cbxTransferFilter1;
        private ComboBox cbxTransferFilter2;
        private TextBox tbxTransferSearch;
        private Button btnImportExcel;
        private Button btnExecuteTransfer;
        private Button btnCreateTransfer;
        private Panel pnlTransferExecution;
        private TableLayoutPanel tableLayoutPanel6;
        private GroupBox groupBoxAvailableIngredients;
        private DataGridView dgvAvailableIngredients;
        private Panel pnlTransferButtons;
        private Button btnAddToTransfer;
        private Button btnRemoveFromTransfer;
        private Button btnClearTransfer;
        private GroupBox groupBoxTransferList;
        private DataGridView dgvTransferList;
        private Panel pnlTransferActions;
        private Label lblTransferTitle;
        private Button btnSaveTransfer;
        private Button btnCancelTransfer;
        private Label label1;
        private Panel panel4;
        private Panel panel5;
        private Button btnTransactionPrevious;
        private Button btnTransactionNext;
        private Label lblTransactionPageInfo;
        private Label lblTransactionRecords;
        private Label lblTransactionPageSize;
        private ComboBox cbxTransactionPageSize;
        private Button btnTransactionPreviousPage;
        private Button btnTransactionNextPage;
        private Label lblTransactionTotalPage;
    }
}