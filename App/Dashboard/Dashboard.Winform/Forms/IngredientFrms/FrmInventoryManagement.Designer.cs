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
            tabInventoryRequests = new TabPage();
            tabTransferExecution = new TabPage();

            // Tab 1: Inventory Transactions
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
            btnTransactionPrevious = new Button();
            btnTransactionNext = new Button();
            lblTransactionPageInfo = new Label();

            // Tab 2: Inventory Requests (controls declared later)
            tableLayoutPanel3 = new TableLayoutPanel();
            pnlRequestHeader = new TableLayoutPanel();
            panel6 = new Panel();
            btnRequestFilter1 = new Button();
            lblRequestFilter1 = new Label();
            cbxRequestFilter1 = new ComboBox();
            panel7 = new Panel();
            btnRequestFilter2 = new Button();
            lblRequestFilter2 = new Label();
            cbxRequestFilter2 = new ComboBox();
            panel8 = new Panel();
            tbxRequestSearch = new TextBox();
            lblRequestSearch = new Label();
            btnViewRequestDetails = new Button();
            btnCreateRequest = new Button();
            btnApproveRequest = new Button();
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

            // Tab 3: Transfer Execution
            tableLayoutPanel5 = new TableLayoutPanel();
            pnlTransferHeader = new TableLayoutPanel();
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
            lblTransferTitle = new Label();
            btnSaveTransfer = new Button();
            btnCancelTransfer = new Button();

            // Initialize Tab Control
            tabControl.SuspendLayout();
            tabInventoryTransactions.SuspendLayout();
            tabInventoryRequests.SuspendLayout();
            tabTransferExecution.SuspendLayout();

            // 
            // tabControl
            // 
            tabControl.TabPages.AddRange(new TabPage[] {
                tabInventoryTransactions,
                tabInventoryRequests,
                tabTransferExecution
            });
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
            tabInventoryTransactions.Padding = new Padding(3);
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
            tableLayoutPanel1.Location = new Point(3, 3);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 3;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel1.Size = new Size(966, 586);
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
            pnlTransactionHeader.Size = new Size(960, 58);
            pnlTransactionHeader.TabIndex = 0;

            // 
            // panel1 (Transaction Type Filter)
            // 
            panel1.Controls.Add(btnTransactionFilter1);
            panel1.Controls.Add(lblTransactionFilter1);
            panel1.Controls.Add(cbxTransactionFilter1);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(5, 5);
            panel1.Margin = new Padding(5);
            panel1.Name = "panel1";
            panel1.Size = new Size(136, 48);
            panel1.TabIndex = 3;

            // 
            // btnTransactionFilter1
            // 
            btnTransactionFilter1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnTransactionFilter1.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnTransactionFilter1.FlatStyle = FlatStyle.Flat;
            btnTransactionFilter1.Font = new Font("Microsoft Sans Serif", 9F);
            btnTransactionFilter1.ForeColor = Color.FromArgb(192, 255, 192);
            btnTransactionFilter1.Location = new Point(0, 24);
            btnTransactionFilter1.Name = "btnTransactionFilter1";
            btnTransactionFilter1.Size = new Size(136, 23);
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
            lblTransactionFilter1.Size = new Size(82, 15);
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
            cbxTransactionFilter1.Location = new Point(0, 25);
            cbxTransactionFilter1.Name = "cbxTransactionFilter1";
            cbxTransactionFilter1.Size = new Size(136, 23);
            cbxTransactionFilter1.TabIndex = 2;

            // 
            // panel2 (Branch Filter)
            // 
            panel2.Controls.Add(btnTransactionFilter2);
            panel2.Controls.Add(lblTransactionFilter2);
            panel2.Controls.Add(cbxTransactionFilter2);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(151, 5);
            panel2.Margin = new Padding(5);
            panel2.Name = "panel2";
            panel2.Size = new Size(136, 48);
            panel2.TabIndex = 4;

            // 
            // btnTransactionFilter2
            // 
            btnTransactionFilter2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnTransactionFilter2.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnTransactionFilter2.FlatStyle = FlatStyle.Flat;
            btnTransactionFilter2.Font = new Font("Microsoft Sans Serif", 9F);
            btnTransactionFilter2.ForeColor = Color.FromArgb(192, 255, 192);
            btnTransactionFilter2.Location = new Point(0, 24);
            btnTransactionFilter2.Name = "btnTransactionFilter2";
            btnTransactionFilter2.Size = new Size(136, 23);
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
            lblTransactionFilter2.Size = new Size(66, 15);
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
            cbxTransactionFilter2.Location = new Point(0, 25);
            cbxTransactionFilter2.Name = "cbxTransactionFilter2";
            cbxTransactionFilter2.Size = new Size(136, 23);
            cbxTransactionFilter2.TabIndex = 2;

            // 
            // panel3 (Search)
            // 
            panel3.Controls.Add(tbxTransactionSearch);
            panel3.Controls.Add(lblTransactionSearch);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(297, 5);
            panel3.Margin = new Padding(5);
            panel3.Name = "panel3";
            panel3.Padding = new Padding(5, 0, 5, 5);
            panel3.Size = new Size(334, 48);
            panel3.TabIndex = 5;

            // 
            // tbxTransactionSearch
            // 
            tbxTransactionSearch.BackColor = Color.FromArgb(42, 45, 86);
            tbxTransactionSearch.BorderStyle = BorderStyle.None;
            tbxTransactionSearch.Dock = DockStyle.Bottom;
            tbxTransactionSearch.ForeColor = Color.FromArgb(192, 255, 192);
            tbxTransactionSearch.Location = new Point(5, 27);
            tbxTransactionSearch.Name = "tbxTransactionSearch";
            try { tbxTransactionSearch.PlaceholderText = "Mã phiếu/Nguyên liệu cần tìm"; } catch { }
            tbxTransactionSearch.Size = new Size(324, 16);
            tbxTransactionSearch.TabIndex = 0;

            // 
            // lblTransactionSearch
            // 
            lblTransactionSearch.AutoSize = true;
            lblTransactionSearch.ForeColor = Color.FromArgb(124, 141, 181);
            lblTransactionSearch.Location = new Point(1, 3);
            lblTransactionSearch.Name = "lblTransactionSearch";
            lblTransactionSearch.Size = new Size(189, 15);
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
            btnViewTransactionDetails.Location = new Point(641, 5);
            btnViewTransactionDetails.Margin = new Padding(5);
            btnViewTransactionDetails.Name = "btnViewTransactionDetails";
            btnViewTransactionDetails.Size = new Size(88, 48);
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
            btnCreateTransaction.Location = new Point(837, 5);
            btnCreateTransaction.Margin = new Padding(5);
            btnCreateTransaction.Name = "btnCreateTransaction";
            btnCreateTransaction.Size = new Size(118, 48);
            btnCreateTransaction.TabIndex = 1;
            btnCreateTransaction.Text = "Tạo mới";
            btnCreateTransaction.UseVisualStyleBackColor = true;

            // 
            // pnlTransactionContent
            // 
            pnlTransactionContent.BackColor = Color.FromArgb(42, 45, 86);
            pnlTransactionContent.Controls.Add(dgvTransactions);
            pnlTransactionContent.Dock = DockStyle.Fill;
            pnlTransactionContent.Location = new Point(3, 67);
            pnlTransactionContent.Name = "pnlTransactionContent";
            pnlTransactionContent.Size = new Size(960, 476);
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
            dgvTransactions.Size = new Size(960, 476);
            dgvTransactions.TabIndex = 0;

            // 
            // pnlTransactionPagination
            // 
            pnlTransactionPagination.Controls.Add(tableLayoutPanel2);
            pnlTransactionPagination.Dock = DockStyle.Fill;
            pnlTransactionPagination.Location = new Point(3, 549);
            pnlTransactionPagination.Name = "pnlTransactionPagination";
            pnlTransactionPagination.Size = new Size(960, 34);
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
            tableLayoutPanel2.Size = new Size(960, 34);
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
            panel4.Size = new Size(474, 28);
            panel4.TabIndex = 0;

            // 
            // lblTransactionRecords
            // 
            lblTransactionRecords.AutoSize = true;
            lblTransactionRecords.Font = new Font("Segoe UI", 13F);
            lblTransactionRecords.ForeColor = Color.FromArgb(255, 192, 128);
            lblTransactionRecords.Location = new Point(5, 5);
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
            lblTransactionPageSize.Location = new Point(200, 8);
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
            cbxTransactionPageSize.Location = new Point(270, 6);
            cbxTransactionPageSize.Name = "cbxTransactionPageSize";
            cbxTransactionPageSize.Size = new Size(60, 23);
            cbxTransactionPageSize.TabIndex = 2;

            // 
            // panel5
            // 
            panel5.Controls.Add(btnTransactionPrevious);
            panel5.Controls.Add(btnTransactionNext);
            panel5.Controls.Add(lblTransactionPageInfo);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(483, 3);
            panel5.Name = "panel5";
            panel5.Size = new Size(474, 28);
            panel5.TabIndex = 1;

            // 
            // btnTransactionPrevious
            // 
            btnTransactionPrevious.Anchor = AnchorStyles.Right;
            btnTransactionPrevious.FlatAppearance.BorderColor = Color.Cyan;
            btnTransactionPrevious.FlatStyle = FlatStyle.Flat;
            btnTransactionPrevious.Font = new Font("Microsoft Sans Serif", 10F);
            btnTransactionPrevious.ForeColor = Color.FromArgb(255, 224, 192);
            btnTransactionPrevious.Location = new Point(250, 0);
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
            btnTransactionNext.Location = new Point(362, 0);
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
            lblTransactionPageInfo.Location = new Point(100, 6);
            lblTransactionPageInfo.Name = "lblTransactionPageInfo";
            lblTransactionPageInfo.Size = new Size(105, 15);
            lblTransactionPageInfo.TabIndex = 2;
            lblTransactionPageInfo.Text = "Hiện trang 1 trên 1";

            // -----------------------
            // Tab 2: Requests layout (header/content/pagination)
            // -----------------------
            tableLayoutPanel3.BackColor = Color.FromArgb(24, 28, 63);
            tableLayoutPanel3.ColumnCount = 1;
            tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.Controls.Add(pnlRequestHeader, 0, 0);
            tableLayoutPanel3.Controls.Add(pnlRequestContent, 0, 1);
            tableLayoutPanel3.Controls.Add(pnlRequestPagination, 0, 2);
            tableLayoutPanel3.Dock = DockStyle.Fill;
            tableLayoutPanel3.Location = new Point(3, 3);
            tableLayoutPanel3.Name = "tableLayoutPanel3";
            tableLayoutPanel3.RowCount = 3;
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutPanel3.Size = new Size(966, 586);
            tableLayoutPanel3.TabIndex = 0;

            // Request Header
            pnlRequestHeader.ColumnCount = 6;
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            pnlRequestHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 98F));
            pnlRequestHeader.Controls.Add(panel6, 0, 0);
            pnlRequestHeader.Controls.Add(panel7, 1, 0);
            pnlRequestHeader.Controls.Add(panel8, 2, 0);
            pnlRequestHeader.Controls.Add(btnViewRequestDetails, 3, 0);
            pnlRequestHeader.Controls.Add(btnCreateRequest, 4, 0);
            pnlRequestHeader.Controls.Add(btnApproveRequest, 5, 0);
            pnlRequestHeader.Dock = DockStyle.Fill;

            // Panel 6 - Request Status Filter
            panel6.Controls.Add(btnRequestFilter1);
            panel6.Controls.Add(lblRequestFilter1);
            panel6.Controls.Add(cbxRequestFilter1);
            panel6.Dock = DockStyle.Fill;
            panel6.Margin = new Padding(5);
            panel6.Size = new Size(136, 48);

            btnRequestFilter1.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnRequestFilter1.FlatStyle = FlatStyle.Flat;
            btnRequestFilter1.ForeColor = Color.FromArgb(192, 255, 192);
            btnRequestFilter1.Text = "Trạng thái";

            lblRequestFilter1.AutoSize = true;
            lblRequestFilter1.ForeColor = Color.FromArgb(124, 141, 181);
            lblRequestFilter1.Text = "Trạng thái:";

            cbxRequestFilter1.BackColor = Color.FromArgb(42, 45, 86);
            cbxRequestFilter1.ForeColor = Color.WhiteSmoke;
            cbxRequestFilter1.Dock = DockStyle.Bottom;

            // Panel 7 - Branch Filter
            panel7.Controls.Add(btnRequestFilter2);
            panel7.Controls.Add(lblRequestFilter2);
            panel7.Controls.Add(cbxRequestFilter2);
            panel7.Dock = DockStyle.Fill;
            panel7.Margin = new Padding(5);

            btnRequestFilter2.Text = "Chi nhánh";
            btnRequestFilter2.FlatStyle = FlatStyle.Flat;
            btnRequestFilter2.ForeColor = Color.FromArgb(192, 255, 192);

            lblRequestFilter2.Text = "Chi nhánh:";
            lblRequestFilter2.ForeColor = Color.FromArgb(124, 141, 181);

            cbxRequestFilter2.BackColor = Color.FromArgb(42, 45, 86);
            cbxRequestFilter2.ForeColor = Color.WhiteSmoke;
            cbxRequestFilter2.Dock = DockStyle.Bottom;

            // Panel 8 - Search
            panel8.Controls.Add(tbxRequestSearch);
            panel8.Controls.Add(lblRequestSearch);
            panel8.Dock = DockStyle.Fill;
            panel8.Padding = new Padding(5, 0, 5, 5);

            tbxRequestSearch.BackColor = Color.FromArgb(42, 45, 86);
            tbxRequestSearch.BorderStyle = BorderStyle.None;
            tbxRequestSearch.ForeColor = Color.FromArgb(192, 255, 192);
            try { tbxRequestSearch.PlaceholderText = "Mã yêu cầu/Nguyên liệu"; } catch { }
            tbxRequestSearch.Dock = DockStyle.Bottom;

            lblRequestSearch.Text = "Tìm kiếm theo (Mã/Nguyên liệu):";
            lblRequestSearch.ForeColor = Color.FromArgb(124, 141, 181);

            // Buttons
            btnViewRequestDetails.Text = "Chi tiết";
            btnViewRequestDetails.FlatStyle = FlatStyle.Flat;
            btnViewRequestDetails.ForeColor = Color.FromArgb(192, 255, 192);

            btnCreateRequest.Text = "Tạo yêu cầu";
            btnCreateRequest.FlatStyle = FlatStyle.Flat;
            btnCreateRequest.ForeColor = Color.FromArgb(255, 224, 192);

            btnApproveRequest.Text = "Duyệt";
            btnApproveRequest.FlatStyle = FlatStyle.Flat;
            btnApproveRequest.ForeColor = Color.FromArgb(192, 255, 192);

            // Request Content
            pnlRequestContent.BackColor = Color.FromArgb(42, 45, 86);
            pnlRequestContent.Controls.Add(dgvRequests);
            pnlRequestContent.Dock = DockStyle.Fill;

            dgvRequests.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvRequests.BorderStyle = BorderStyle.None;
            dgvRequests.AllowUserToAddRows = false;
            dgvRequests.AllowUserToDeleteRows = false;
            dgvRequests.ReadOnly = true;
            dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRequests.Dock = DockStyle.Fill;

            // Request Pagination
            pnlRequestPagination.Controls.Add(tableLayoutPanel4);
            pnlRequestPagination.Dock = DockStyle.Fill;
            pnlRequestPagination.Location = new Point(3, 549);
            pnlRequestPagination.Name = "pnlRequestPagination";
            pnlRequestPagination.Size = new Size(960, 34);
            pnlRequestPagination.TabIndex = 2;

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
            tableLayoutPanel4.Size = new Size(960, 34);
            tableLayoutPanel4.TabIndex = 0;

            panel9.Controls.Add(lblRequestRecords);
            panel9.Controls.Add(lblRequestPageSize);
            panel9.Controls.Add(cbxRequestPageSize);
            panel9.Dock = DockStyle.Fill;

            lblRequestRecords.AutoSize = true;
            lblRequestRecords.Font = new Font("Segoe UI", 13F);
            lblRequestRecords.ForeColor = Color.FromArgb(255, 192, 128);
            lblRequestRecords.Text = "Số lượng: 0";

            lblRequestPageSize.AutoSize = true;
            lblRequestPageSize.Font = new Font("Segoe UI", 12F);
            lblRequestPageSize.ForeColor = Color.FromArgb(124, 141, 181);
            lblRequestPageSize.Text = "Hiển thị";

            cbxRequestPageSize.BackColor = Color.FromArgb(42, 45, 86);
            cbxRequestPageSize.ForeColor = Color.White;
            cbxRequestPageSize.Items.AddRange(new object[] { "10", "25", "50", "100" });

            panel10.Controls.Add(btnRequestPrevious);
            panel10.Controls.Add(btnRequestNext);
            panel10.Controls.Add(lblRequestPageInfo);
            panel10.Dock = DockStyle.Fill;

            btnRequestPrevious.FlatStyle = FlatStyle.Flat;
            btnRequestPrevious.FlatAppearance.BorderColor = Color.Cyan;
            btnRequestPrevious.ForeColor = Color.FromArgb(255, 224, 192);
            btnRequestPrevious.Text = "Trang trước";

            btnRequestNext.FlatStyle = FlatStyle.Flat;
            btnRequestNext.FlatAppearance.BorderColor = Color.Fuchsia;
            btnRequestNext.ForeColor = Color.FromArgb(192, 255, 192);
            btnRequestNext.Text = "Trang sau";

            lblRequestPageInfo.AutoSize = true;
            lblRequestPageInfo.ForeColor = Color.FromArgb(124, 141, 181);
            lblRequestPageInfo.Text = "Hiện trang 1 trên 1";

            // -----------------------
            // Tab 3: Transfer Execution layout
            // -----------------------
            tableLayoutPanel5.BackColor = Color.FromArgb(24, 28, 63);
            tableLayoutPanel5.ColumnCount = 1;
            tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.Controls.Add(pnlTransferHeader, 0, 0);
            tableLayoutPanel5.Controls.Add(pnlTransferExecution, 0, 1);
            tableLayoutPanel5.Controls.Add(pnlTransferActions, 0, 2);
            tableLayoutPanel5.Dock = DockStyle.Fill;
            tableLayoutPanel5.Location = new Point(3, 3);
            tableLayoutPanel5.Name = "tableLayoutPanel5";
            tableLayoutPanel5.RowCount = 3;
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 64F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutPanel5.Size = new Size(966, 586);
            tableLayoutPanel5.TabIndex = 0;

            pnlTransferHeader.ColumnCount = 6;
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            pnlTransferHeader.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 108F));
            pnlTransferHeader.Controls.Add(panel11, 0, 0);
            pnlTransferHeader.Controls.Add(panel12, 1, 0);
            pnlTransferHeader.Controls.Add(panel13, 2, 0);
            pnlTransferHeader.Controls.Add(btnImportExcel, 3, 0);
            pnlTransferHeader.Controls.Add(btnExecuteTransfer, 4, 0);
            pnlTransferHeader.Controls.Add(btnCreateTransfer, 5, 0);
            pnlTransferHeader.Dock = DockStyle.Fill;

            // Panel 11: transfer type
            panel11.Controls.Add(btnTransferFilter1);
            panel11.Controls.Add(lblTransferFilter1);
            panel11.Controls.Add(cbxTransferFilter1);
            panel11.Dock = DockStyle.Fill;
            btnTransferFilter1.Text = "Loại chuyển";
            btnTransferFilter1.FlatStyle = FlatStyle.Flat;
            btnTransferFilter1.ForeColor = Color.FromArgb(192, 255, 192);
            lblTransferFilter1.Text = "Loại chuyển:";
            lblTransferFilter1.ForeColor = Color.FromArgb(124, 141, 181);
            cbxTransferFilter1.BackColor = Color.FromArgb(42, 45, 86);
            cbxTransferFilter1.ForeColor = Color.WhiteSmoke;
            cbxTransferFilter1.Dock = DockStyle.Bottom;

            // Panel 12: branch
            panel12.Controls.Add(btnTransferFilter2);
            panel12.Controls.Add(lblTransferFilter2);
            panel12.Controls.Add(cbxTransferFilter2);
            panel12.Dock = DockStyle.Fill;
            btnTransferFilter2.Text = "Chi nhánh";
            btnTransferFilter2.FlatStyle = FlatStyle.Flat;
            btnTransferFilter2.ForeColor = Color.FromArgb(192, 255, 192);
            lblTransferFilter2.Text = "Chi nhánh:";
            lblTransferFilter2.ForeColor = Color.FromArgb(124, 141, 181);
            cbxTransferFilter2.BackColor = Color.FromArgb(42, 45, 86);
            cbxTransferFilter2.ForeColor = Color.WhiteSmoke;
            cbxTransferFilter2.Dock = DockStyle.Bottom;

            // Panel 13: search
            panel13.Controls.Add(tbxTransferSearch);
            panel13.Controls.Add(lblTransferSearch);
            panel13.Dock = DockStyle.Fill;
            panel13.Padding = new Padding(5, 0, 5, 5);
            tbxTransferSearch.BackColor = Color.FromArgb(42, 45, 86);
            tbxTransferSearch.BorderStyle = BorderStyle.None;
            tbxTransferSearch.ForeColor = Color.FromArgb(192, 255, 192);
            try { tbxTransferSearch.PlaceholderText = "Nguyên liệu cần chuyển"; } catch { }
            tbxTransferSearch.Dock = DockStyle.Bottom;
            lblTransferSearch.Text = "Tìm kiếm nguyên liệu:";
            lblTransferSearch.ForeColor = Color.FromArgb(124, 141, 181);

            btnImportExcel.FlatStyle = FlatStyle.Flat;
            btnImportExcel.ForeColor = Color.FromArgb(255, 192, 128);
            btnImportExcel.Text = "Import Excel";

            btnExecuteTransfer.FlatStyle = FlatStyle.Flat;
            btnExecuteTransfer.ForeColor = Color.FromArgb(192, 255, 192);
            btnExecuteTransfer.Text = "Thực hiện";

            btnCreateTransfer.FlatStyle = FlatStyle.Flat;
            btnCreateTransfer.ForeColor = Color.FromArgb(255, 224, 192);
            btnCreateTransfer.Text = "Tạo phiếu";

            // Transfer Execution content
            pnlTransferExecution.Dock = DockStyle.Fill;
            tableLayoutPanel6.ColumnCount = 3;
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10F));
            tableLayoutPanel6.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
            tableLayoutPanel6.Dock = DockStyle.Fill;
            tableLayoutPanel6.RowCount = 1;
            tableLayoutPanel6.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            groupBoxAvailableIngredients.Text = "Nguyên liệu có sẵn";
            groupBoxAvailableIngredients.ForeColor = Color.White;
            groupBoxAvailableIngredients.Dock = DockStyle.Fill;
            dgvAvailableIngredients.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvAvailableIngredients.BorderStyle = BorderStyle.None;
            dgvAvailableIngredients.AllowUserToAddRows = false;
            dgvAvailableIngredients.AllowUserToDeleteRows = false;
            dgvAvailableIngredients.ReadOnly = true;
            dgvAvailableIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailableIngredients.Dock = DockStyle.Fill;
            groupBoxAvailableIngredients.Controls.Add(dgvAvailableIngredients);

            pnlTransferButtons.Dock = DockStyle.Fill;
            btnAddToTransfer.FlatStyle = FlatStyle.Flat;
            btnAddToTransfer.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnAddToTransfer.Text = "→";
            btnRemoveFromTransfer.FlatStyle = FlatStyle.Flat;
            btnRemoveFromTransfer.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold);
            btnRemoveFromTransfer.Text = "←";
            btnClearTransfer.FlatStyle = FlatStyle.Flat;
            btnClearTransfer.Text = "Clear";
            pnlTransferButtons.Controls.Add(btnAddToTransfer);
            pnlTransferButtons.Controls.Add(btnRemoveFromTransfer);
            pnlTransferButtons.Controls.Add(btnClearTransfer);

            groupBoxTransferList.Text = "Danh sách chuyển kho";
            groupBoxTransferList.ForeColor = Color.White;
            groupBoxTransferList.Dock = DockStyle.Fill;
            dgvTransferList.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvTransferList.BorderStyle = BorderStyle.None;
            dgvTransferList.AllowUserToAddRows = false;
            dgvTransferList.AllowUserToDeleteRows = false;
            dgvTransferList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvTransferList.Dock = DockStyle.Fill;
            groupBoxTransferList.Controls.Add(dgvTransferList);

            tableLayoutPanel6.Controls.Add(groupBoxAvailableIngredients, 0, 0);
            tableLayoutPanel6.Controls.Add(pnlTransferButtons, 1, 0);
            tableLayoutPanel6.Controls.Add(groupBoxTransferList, 2, 0);

            pnlTransferExecution.Controls.Add(tableLayoutPanel6);

            // Transfer Actions / Footer
            pnlTransferActions.Dock = DockStyle.Fill;
            lblTransferTitle.Text = "Thực hiện chuyển kho";
            lblTransferTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblTransferTitle.ForeColor = Color.FromArgb(255, 224, 192);
            lblTransferTitle.Location = new Point(13, 13);
            btnSaveTransfer.FlatStyle = FlatStyle.Flat;
            btnSaveTransfer.Text = "Lưu phiếu";
            btnSaveTransfer.ForeColor = Color.FromArgb(192, 255, 192);
            btnCancelTransfer.FlatStyle = FlatStyle.Flat;
            btnCancelTransfer.Text = "Hủy";
            btnCancelTransfer.ForeColor = Color.FromArgb(255, 128, 128);
            pnlTransferActions.Controls.Add(lblTransferTitle);
            pnlTransferActions.Controls.Add(btnSaveTransfer);
            pnlTransferActions.Controls.Add(btnCancelTransfer);

            tableLayoutPanel5.Controls.Add(pnlTransferHeader, 0, 0);
            tableLayoutPanel5.Controls.Add(pnlTransferExecution, 0, 1);
            tableLayoutPanel5.Controls.Add(pnlTransferActions, 0, 2);

            tabInventoryRequests.Text = "Yêu cầu";
            tabInventoryRequests.Controls.Add(tableLayoutPanel3);

            tabTransferExecution.Text = "Chuyển kho";
            tabTransferExecution.Controls.Add(tableLayoutPanel5);

            // Add the main tabControl to the Form's controls so Designer and runtime can render it
            this.Controls.Add(tabControl);

            // Set some basic Form properties (designer/runtime expects these)
            this.ClientSize = new Size(1000, 640);
            this.Name = "FrmInventoryManagement";
            this.Text = "Quản lý nhập xuất hàng hóa";
            this.Padding = new Padding(10);

            // Resume layout for the Form
            tabControl.ResumeLayout(false);
            tabInventoryTransactions.ResumeLayout(false);
            tabInventoryRequests.ResumeLayout(false);
            tabTransferExecution.ResumeLayout(false);

            this.ResumeLayout(false);
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
        private Panel panel4;
        private Panel panel5;
        private Label lblTransactionRecords;
        private Label lblTransactionPageSize;
        private Label lblTransactionPageInfo;
        private ComboBox cbxTransactionPageSize;
        private Button btnTransactionPrevious;
        private Button btnTransactionNext;

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
    }
}