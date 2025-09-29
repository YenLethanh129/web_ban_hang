namespace Dashboard.Winform.Forms
{
    partial class FrmRecipeDetails
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            tabControl = new TabControl();
            tabBasicInfo = new TabPage();
            pnlBasicInfo = new Panel();
            lblRecipeId = new Label();
            txtRecipeId = new TextBox();
            lblName = new Label();
            txtName = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblProduct = new Label();
            cbxProduct = new ComboBox();
            lblServingSize = new Label();
            numServingSize = new NumericUpDown();
            lblUnit = new Label();
            txtUnit = new TextBox();
            chkIsActive = new CheckBox();
            lblNotes = new Label();
            txtNotes = new TextBox();
            lblCreatedAtLabel = new Label();
            lblCreatedAt = new Label();
            lblUpdatedAtLabel = new Label();
            lblUpdatedAt = new Label();
            tabIngredients = new TabPage();
            pnlIngredients = new Panel();
            lblRecipeIngredients = new Label();
            dgvIngredients = new DataGridView();
            pnlIngredientButtons = new Panel();
            btnAddIngredient = new Button();
            btnEditIngredient = new Button();
            btnDeleteIngredient = new Button();
            pnlSearchArea = new Panel();
            lblAvailableIngredients = new Label();
            pnlSearchBox = new Panel();
            txtSearchIngredient = new TextBox();
            btnClearSearch = new Button();
            dgvAvailableIngredients = new DataGridView();
            pnlButtons = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            btnClose = new Button();
            tabControl.SuspendLayout();
            tabBasicInfo.SuspendLayout();
            pnlBasicInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numServingSize).BeginInit();
            tabIngredients.SuspendLayout();
            pnlIngredients.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIngredients).BeginInit();
            pnlIngredientButtons.SuspendLayout();
            pnlSearchArea.SuspendLayout();
            pnlSearchBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAvailableIngredients).BeginInit();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabBasicInfo);
            tabControl.Controls.Add(tabIngredients);
            tabControl.Dock = DockStyle.Fill;
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.Font = new Font("Segoe UI", 9F);
            tabControl.ItemSize = new Size(120, 40);
            tabControl.Location = new Point(10, 10);
            tabControl.Margin = new Padding(9);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1064, 716);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.TabIndex = 0;
            // 
            // tabBasicInfo
            // 
            tabBasicInfo.BackColor = Color.FromArgb(42, 45, 86);
            tabBasicInfo.Controls.Add(pnlBasicInfo);
            tabBasicInfo.Location = new Point(4, 44);
            tabBasicInfo.Name = "tabBasicInfo";
            tabBasicInfo.Padding = new Padding(3);
            tabBasicInfo.Size = new Size(1056, 668);
            tabBasicInfo.TabIndex = 0;
            tabBasicInfo.Text = "Thông tin cơ bản";
            // 
            // pnlBasicInfo
            // 
            pnlBasicInfo.BackColor = Color.FromArgb(42, 45, 86);
            pnlBasicInfo.Controls.Add(lblRecipeId);
            pnlBasicInfo.Controls.Add(txtRecipeId);
            pnlBasicInfo.Controls.Add(lblName);
            pnlBasicInfo.Controls.Add(txtName);
            pnlBasicInfo.Controls.Add(lblDescription);
            pnlBasicInfo.Controls.Add(txtDescription);
            pnlBasicInfo.Controls.Add(lblProduct);
            pnlBasicInfo.Controls.Add(cbxProduct);
            pnlBasicInfo.Controls.Add(lblServingSize);
            pnlBasicInfo.Controls.Add(numServingSize);
            pnlBasicInfo.Controls.Add(lblUnit);
            pnlBasicInfo.Controls.Add(txtUnit);
            pnlBasicInfo.Controls.Add(chkIsActive);
            pnlBasicInfo.Controls.Add(lblNotes);
            pnlBasicInfo.Controls.Add(txtNotes);
            pnlBasicInfo.Controls.Add(lblCreatedAtLabel);
            pnlBasicInfo.Controls.Add(lblCreatedAt);
            pnlBasicInfo.Controls.Add(lblUpdatedAtLabel);
            pnlBasicInfo.Controls.Add(lblUpdatedAt);
            pnlBasicInfo.Dock = DockStyle.Fill;
            pnlBasicInfo.Location = new Point(3, 3);
            pnlBasicInfo.Name = "pnlBasicInfo";
            pnlBasicInfo.Padding = new Padding(18, 19, 18, 19);
            pnlBasicInfo.Size = new Size(1050, 662);
            pnlBasicInfo.TabIndex = 0;
            // 
            // lblRecipeId
            // 
            lblRecipeId.AutoSize = true;
            lblRecipeId.ForeColor = Color.FromArgb(124, 141, 181);
            lblRecipeId.Location = new Point(18, 22);
            lblRecipeId.Name = "lblRecipeId";
            lblRecipeId.Size = new Size(82, 15);
            lblRecipeId.TabIndex = 0;
            lblRecipeId.Text = "ID công thức:";
            // 
            // txtRecipeId
            // 
            txtRecipeId.BackColor = Color.FromArgb(24, 28, 63);
            txtRecipeId.BorderStyle = BorderStyle.FixedSingle;
            txtRecipeId.ForeColor = Color.WhiteSmoke;
            txtRecipeId.Location = new Point(131, 19);
            txtRecipeId.Name = "txtRecipeId";
            txtRecipeId.ReadOnly = true;
            txtRecipeId.Size = new Size(176, 23);
            txtRecipeId.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.ForeColor = Color.FromArgb(124, 141, 181);
            lblName.Location = new Point(18, 54);
            lblName.Name = "lblName";
            lblName.Size = new Size(90, 15);
            lblName.TabIndex = 2;
            lblName.Text = "Tên công thức:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(24, 28, 63);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.WhiteSmoke;
            txtName.Location = new Point(131, 52);
            txtName.Name = "txtName";
            txtName.Size = new Size(350, 23);
            txtName.TabIndex = 3;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.ForeColor = Color.FromArgb(124, 141, 181);
            lblDescription.Location = new Point(18, 87);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(45, 15);
            lblDescription.TabIndex = 4;
            lblDescription.Text = "Mô tả:";
            // 
            // txtDescription
            // 
            txtDescription.BackColor = Color.FromArgb(24, 28, 63);
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.ForeColor = Color.WhiteSmoke;
            txtDescription.Location = new Point(131, 84);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(350, 60);
            txtDescription.TabIndex = 5;
            // 
            // lblProduct
            // 
            lblProduct.AutoSize = true;
            lblProduct.ForeColor = Color.FromArgb(124, 141, 181);
            lblProduct.Location = new Point(18, 158);
            lblProduct.Name = "lblProduct";
            lblProduct.Size = new Size(65, 15);
            lblProduct.TabIndex = 6;
            lblProduct.Text = "Sản phẩm:";
            // 
            // cbxProduct
            // 
            cbxProduct.BackColor = Color.FromArgb(24, 28, 63);
            cbxProduct.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxProduct.FlatStyle = FlatStyle.Flat;
            cbxProduct.ForeColor = Color.WhiteSmoke;
            cbxProduct.FormattingEnabled = true;
            cbxProduct.Location = new Point(131, 155);
            cbxProduct.Name = "cbxProduct";
            cbxProduct.Size = new Size(250, 23);
            cbxProduct.TabIndex = 7;
            // 
            // lblServingSize
            // 
            lblServingSize.AutoSize = true;
            lblServingSize.ForeColor = Color.FromArgb(124, 141, 181);
            lblServingSize.Location = new Point(18, 191);
            lblServingSize.Name = "lblServingSize";
            lblServingSize.Size = new Size(68, 15);
            lblServingSize.TabIndex = 8;
            lblServingSize.Text = "Khẩu phần:";
            // 
            // numServingSize
            // 
            numServingSize.BackColor = Color.FromArgb(24, 28, 63);
            numServingSize.BorderStyle = BorderStyle.FixedSingle;
            numServingSize.DecimalPlaces = 2;
            numServingSize.ForeColor = Color.WhiteSmoke;
            numServingSize.Location = new Point(131, 189);
            numServingSize.Maximum = new decimal(new int[] { 9999, 0, 0, 0 });
            numServingSize.Minimum = new decimal(new int[] { 1, 0, 0, 131072 });
            numServingSize.Name = "numServingSize";
            numServingSize.Size = new Size(120, 23);
            numServingSize.TabIndex = 9;
            numServingSize.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // lblUnit
            // 
            lblUnit.AutoSize = true;
            lblUnit.ForeColor = Color.FromArgb(124, 141, 181);
            lblUnit.Location = new Point(270, 191);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(48, 15);
            lblUnit.TabIndex = 10;
            lblUnit.Text = "Đơn vị:";
            // 
            // txtUnit
            // 
            txtUnit.BackColor = Color.FromArgb(24, 28, 63);
            txtUnit.BorderStyle = BorderStyle.FixedSingle;
            txtUnit.ForeColor = Color.WhiteSmoke;
            txtUnit.Location = new Point(324, 189);
            txtUnit.Name = "txtUnit";
            txtUnit.Size = new Size(100, 23);
            txtUnit.TabIndex = 11;
            txtUnit.Text = "portion";
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.Checked = true;
            chkIsActive.CheckState = CheckState.Checked;
            chkIsActive.ForeColor = Color.FromArgb(192, 255, 192);
            chkIsActive.Location = new Point(131, 225);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(117, 19);
            chkIsActive.TabIndex = 12;
            chkIsActive.Text = "Đang hoạt động";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // lblNotes
            // 
            lblNotes.AutoSize = true;
            lblNotes.ForeColor = Color.FromArgb(124, 141, 181);
            lblNotes.Location = new Point(18, 258);
            lblNotes.Name = "lblNotes";
            lblNotes.Size = new Size(54, 15);
            lblNotes.TabIndex = 13;
            lblNotes.Text = "Ghi chú:";
            // 
            // txtNotes
            // 
            txtNotes.BackColor = Color.FromArgb(24, 28, 63);
            txtNotes.BorderStyle = BorderStyle.FixedSingle;
            txtNotes.ForeColor = Color.WhiteSmoke;
            txtNotes.Location = new Point(131, 255);
            txtNotes.Multiline = true;
            txtNotes.Name = "txtNotes";
            txtNotes.ScrollBars = ScrollBars.Vertical;
            txtNotes.Size = new Size(350, 60);
            txtNotes.TabIndex = 14;
            // 
            // lblCreatedAtLabel
            // 
            lblCreatedAtLabel.AutoSize = true;
            lblCreatedAtLabel.ForeColor = Color.FromArgb(124, 141, 181);
            lblCreatedAtLabel.Location = new Point(18, 340);
            lblCreatedAtLabel.Name = "lblCreatedAtLabel";
            lblCreatedAtLabel.Size = new Size(58, 15);
            lblCreatedAtLabel.TabIndex = 15;
            lblCreatedAtLabel.Text = "Ngày tạo:";
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.AutoSize = true;
            lblCreatedAt.ForeColor = Color.FromArgb(255, 192, 128);
            lblCreatedAt.Location = new Point(131, 340);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Size = new Size(0, 15);
            lblCreatedAt.TabIndex = 16;
            // 
            // lblUpdatedAtLabel
            // 
            lblUpdatedAtLabel.AutoSize = true;
            lblUpdatedAtLabel.ForeColor = Color.FromArgb(124, 141, 181);
            lblUpdatedAtLabel.Location = new Point(18, 363);
            lblUpdatedAtLabel.Name = "lblUpdatedAtLabel";
            lblUpdatedAtLabel.Size = new Size(87, 15);
            lblUpdatedAtLabel.TabIndex = 17;
            lblUpdatedAtLabel.Text = "Ngày cập nhật:";
            // 
            // lblUpdatedAt
            // 
            lblUpdatedAt.AutoSize = true;
            lblUpdatedAt.ForeColor = Color.FromArgb(255, 192, 128);
            lblUpdatedAt.Location = new Point(131, 363);
            lblUpdatedAt.Name = "lblUpdatedAt";
            lblUpdatedAt.Size = new Size(0, 15);
            lblUpdatedAt.TabIndex = 18;
            // 
            // tabIngredients
            // 
            tabIngredients.BackColor = Color.FromArgb(42, 45, 86);
            tabIngredients.Controls.Add(pnlIngredients);
            tabIngredients.Location = new Point(4, 44);
            tabIngredients.Name = "tabIngredients";
            tabIngredients.Padding = new Padding(3);
            tabIngredients.Size = new Size(1056, 668);
            tabIngredients.TabIndex = 1;
            tabIngredients.Text = "Nguyên liệu";
            // 
            // pnlIngredients
            // 
            pnlIngredients.BackColor = Color.FromArgb(42, 45, 86);
            pnlIngredients.Controls.Add(lblRecipeIngredients);
            pnlIngredients.Controls.Add(dgvIngredients);
            pnlIngredients.Controls.Add(pnlIngredientButtons);
            pnlIngredients.Controls.Add(pnlSearchArea);
            pnlIngredients.Dock = DockStyle.Fill;
            pnlIngredients.Location = new Point(3, 3);
            pnlIngredients.Name = "pnlIngredients";
            pnlIngredients.Padding = new Padding(10);
            pnlIngredients.Size = new Size(1050, 662);
            pnlIngredients.TabIndex = 0;
            // 
            // lblRecipeIngredients
            // 
            lblRecipeIngredients.AutoSize = true;
            lblRecipeIngredients.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblRecipeIngredients.ForeColor = Color.FromArgb(124, 141, 181);
            lblRecipeIngredients.Location = new Point(13, 13);
            lblRecipeIngredients.Name = "lblRecipeIngredients";
            lblRecipeIngredients.Size = new Size(200, 19);
            lblRecipeIngredients.TabIndex = 0;
            lblRecipeIngredients.Text = "Nguyên liệu trong công thức";
            // 
            // dgvIngredients
            // 
            dgvIngredients.AllowUserToAddRows = false;
            dgvIngredients.AllowUserToDeleteRows = false;
            dgvIngredients.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            dgvIngredients.BackgroundColor = Color.FromArgb(24, 28, 63);
            dgvIngredients.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvIngredients.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvIngredients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvIngredients.DefaultCellStyle = dataGridViewCellStyle2;
            dgvIngredients.GridColor = Color.FromArgb(124, 141, 181);
            dgvIngredients.Location = new Point(13, 38);
            dgvIngredients.MultiSelect = false;
            dgvIngredients.Name = "dgvIngredients";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvIngredients.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvIngredients.RowHeadersWidth = 51;
            dgvIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngredients.Size = new Size(1024, 240);
            dgvIngredients.TabIndex = 1;
            // 
            // pnlIngredientButtons
            // 
            pnlIngredientButtons.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlIngredientButtons.BackColor = Color.FromArgb(42, 45, 86);
            pnlIngredientButtons.Controls.Add(btnAddIngredient);
            pnlIngredientButtons.Controls.Add(btnEditIngredient);
            pnlIngredientButtons.Controls.Add(btnDeleteIngredient);
            pnlIngredientButtons.Location = new Point(13, 284);
            pnlIngredientButtons.Name = "pnlIngredientButtons";
            pnlIngredientButtons.Size = new Size(1024, 38);
            pnlIngredientButtons.TabIndex = 2;
            // 
            // btnAddIngredient
            // 
            btnAddIngredient.FlatAppearance.BorderColor = Color.Cyan;
            btnAddIngredient.FlatStyle = FlatStyle.Flat;
            btnAddIngredient.ForeColor = Color.FromArgb(255, 224, 192);
            btnAddIngredient.Location = new Point(0, 5);
            btnAddIngredient.Name = "btnAddIngredient";
            btnAddIngredient.Size = new Size(100, 28);
            btnAddIngredient.TabIndex = 0;
            btnAddIngredient.Text = "↑ Thêm";
            btnAddIngredient.UseVisualStyleBackColor = true;
            // 
            // btnEditIngredient
            // 
            btnEditIngredient.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnEditIngredient.FlatStyle = FlatStyle.Flat;
            btnEditIngredient.ForeColor = Color.FromArgb(192, 255, 192);
            btnEditIngredient.Location = new Point(109, 5);
            btnEditIngredient.Name = "btnEditIngredient";
            btnEditIngredient.Size = new Size(100, 28);
            btnEditIngredient.TabIndex = 1;
            btnEditIngredient.Text = "Sửa";
            btnEditIngredient.UseVisualStyleBackColor = true;
            // 
            // btnDeleteIngredient
            // 
            btnDeleteIngredient.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnDeleteIngredient.FlatStyle = FlatStyle.Flat;
            btnDeleteIngredient.ForeColor = Color.FromArgb(255, 99, 132);
            btnDeleteIngredient.Location = new Point(218, 5);
            btnDeleteIngredient.Name = "btnDeleteIngredient";
            btnDeleteIngredient.Size = new Size(100, 28);
            btnDeleteIngredient.TabIndex = 2;
            btnDeleteIngredient.Text = "Xóa";
            btnDeleteIngredient.UseVisualStyleBackColor = true;
            // 
            // pnlSearchArea
            // 
            pnlSearchArea.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlSearchArea.BackColor = Color.FromArgb(42, 45, 86);
            pnlSearchArea.Controls.Add(lblAvailableIngredients);
            pnlSearchArea.Controls.Add(pnlSearchBox);
            pnlSearchArea.Controls.Add(dgvAvailableIngredients);
            pnlSearchArea.Location = new Point(13, 328);
            pnlSearchArea.Name = "pnlSearchArea";
            pnlSearchArea.Size = new Size(1024, 321);
            pnlSearchArea.TabIndex = 3;
            // 
            // lblAvailableIngredients
            // 
            lblAvailableIngredients.AutoSize = true;
            lblAvailableIngredients.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            lblAvailableIngredients.ForeColor = Color.FromArgb(124, 141, 181);
            lblAvailableIngredients.Location = new Point(3, 8);
            lblAvailableIngredients.Name = "lblAvailableIngredients";
            lblAvailableIngredients.Size = new Size(217, 19);
            lblAvailableIngredients.TabIndex = 0;
            lblAvailableIngredients.Text = "Danh sách nguyên liệu có sẵn";
            // 
            // pnlSearchBox
            // 
            pnlSearchBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            pnlSearchBox.BackColor = Color.FromArgb(42, 45, 86);
            pnlSearchBox.Controls.Add(txtSearchIngredient);
            pnlSearchBox.Controls.Add(btnClearSearch);
            pnlSearchBox.Location = new Point(3, 33);
            pnlSearchBox.Name = "pnlSearchBox";
            pnlSearchBox.Size = new Size(1018, 35);
            pnlSearchBox.TabIndex = 1;
            // 
            // txtSearchIngredient
            // 
            txtSearchIngredient.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            txtSearchIngredient.BackColor = Color.FromArgb(24, 28, 63);
            txtSearchIngredient.BorderStyle = BorderStyle.FixedSingle;
            txtSearchIngredient.Font = new Font("Segoe UI", 10F);
            txtSearchIngredient.ForeColor = Color.WhiteSmoke;
            txtSearchIngredient.Location = new Point(0, 5);
            txtSearchIngredient.Name = "txtSearchIngredient";
            txtSearchIngredient.PlaceholderText = "🔍 Tìm kiếm nguyên liệu theo tên hoặc danh mục...";
            txtSearchIngredient.Size = new Size(939, 25);
            txtSearchIngredient.TabIndex = 0;
            // 
            // btnClearSearch
            // 
            btnClearSearch.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClearSearch.FlatAppearance.BorderColor = Color.FromArgb(124, 141, 181);
            btnClearSearch.FlatStyle = FlatStyle.Flat;
            btnClearSearch.ForeColor = Color.FromArgb(124, 141, 181);
            btnClearSearch.Location = new Point(945, 4);
            btnClearSearch.Name = "btnClearSearch";
            btnClearSearch.Size = new Size(70, 27);
            btnClearSearch.TabIndex = 1;
            btnClearSearch.Text = "Xóa";
            btnClearSearch.UseVisualStyleBackColor = true;
            // 
            // dgvAvailableIngredients
            // 
            dgvAvailableIngredients.AllowUserToAddRows = false;
            dgvAvailableIngredients.AllowUserToDeleteRows = false;
            dgvAvailableIngredients.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAvailableIngredients.BackgroundColor = Color.FromArgb(24, 28, 63);
            dgvAvailableIngredients.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(66, 184, 131);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvAvailableIngredients.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvAvailableIngredients.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(66, 184, 131);
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvAvailableIngredients.DefaultCellStyle = dataGridViewCellStyle5;
            dgvAvailableIngredients.GridColor = Color.FromArgb(124, 141, 181);
            dgvAvailableIngredients.Location = new Point(3, 74);
            dgvAvailableIngredients.Name = "dgvAvailableIngredients";
            dgvAvailableIngredients.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvAvailableIngredients.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvAvailableIngredients.RowHeadersWidth = 51;
            dgvAvailableIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailableIngredients.Size = new Size(1018, 244);
            dgvAvailableIngredients.TabIndex = 2;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(24, 28, 63);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(10, 726);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 8, 0, 8);
            pnlButtons.Size = new Size(1064, 56);
            pnlButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.DialogResult = DialogResult.OK;
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft Sans Serif", 10F);
            btnSave.ForeColor = Color.FromArgb(192, 255, 192);
            btnSave.Location = new Point(827, 14);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(70, 28);
            btnSave.TabIndex = 0;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(255, 193, 7);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft Sans Serif", 10F);
            btnCancel.ForeColor = Color.FromArgb(255, 193, 7);
            btnCancel.Location = new Point(906, 14);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(70, 28);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Microsoft Sans Serif", 10F);
            btnClose.ForeColor = Color.FromArgb(255, 99, 132);
            btnClose.Location = new Point(985, 14);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(70, 28);
            btnClose.TabIndex = 2;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // FrmRecipeDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(1084, 792);
            Controls.Add(tabControl);
            Controls.Add(pnlButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmRecipeDetails";
            Padding = new Padding(10);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết công thức";
            tabControl.ResumeLayout(false);
            tabBasicInfo.ResumeLayout(false);
            pnlBasicInfo.ResumeLayout(false);
            pnlBasicInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)numServingSize).EndInit();
            tabIngredients.ResumeLayout(false);
            pnlIngredients.ResumeLayout(false);
            pnlIngredients.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvIngredients).EndInit();
            pnlIngredientButtons.ResumeLayout(false);
            pnlSearchArea.ResumeLayout(false);
            pnlSearchArea.PerformLayout();
            pnlSearchBox.ResumeLayout(false);
            pnlSearchBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAvailableIngredients).EndInit();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBasicInfo;
        private System.Windows.Forms.Panel pnlBasicInfo;
        private System.Windows.Forms.Label lblRecipeId;
        private System.Windows.Forms.TextBox txtRecipeId;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblProduct;
        private System.Windows.Forms.ComboBox cbxProduct;
        private System.Windows.Forms.Label lblServingSize;
        private System.Windows.Forms.NumericUpDown numServingSize;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.Label lblCreatedAtLabel;
        private System.Windows.Forms.Label lblCreatedAt;
        private System.Windows.Forms.Label lblUpdatedAtLabel;
        private System.Windows.Forms.Label lblUpdatedAt;
        private System.Windows.Forms.TabPage tabIngredients;
        private System.Windows.Forms.Panel pnlIngredients;
        private System.Windows.Forms.Label lblRecipeIngredients;
        private System.Windows.Forms.DataGridView dgvIngredients;
        private System.Windows.Forms.Panel pnlIngredientButtons;
        private System.Windows.Forms.Button btnAddIngredient;
        private System.Windows.Forms.Button btnEditIngredient;
        private System.Windows.Forms.Button btnDeleteIngredient;
        private System.Windows.Forms.Panel pnlSearchArea;
        private System.Windows.Forms.Label lblAvailableIngredients;
        private System.Windows.Forms.Panel pnlSearchBox;
        private System.Windows.Forms.TextBox txtSearchIngredient;
        private System.Windows.Forms.Button btnClearSearch;
        private System.Windows.Forms.DataGridView dgvAvailableIngredients;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
    }
}