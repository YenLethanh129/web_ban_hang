namespace Dashboard.Winform.Forms
{
    partial class FrmProductDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProductDetails));
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            toolTip1 = new ToolTip(components);
            tabControl = new TabControl();
            tabBasicInfo = new TabPage();
            pnlBasicInfo = new Panel();
            lblValidationStatus = new Label();
            btnCheckUrl = new Button();
            btnRemove = new Button();
            btnUpload = new Button();
            picThumbnail = new PictureBox();
            lblProductId = new Label();
            txtProductId = new TextBox();
            lblName = new Label();
            txtName = new TextBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblPrice = new Label();
            numPrice = new NumericUpDown();
            lblCategory = new Label();
            cbxCategory = new ComboBox();
            lblTax = new Label();
            cbxTax = new ComboBox();
            chkIsActive = new CheckBox();
            lblThumbnail = new Label();
            txtImagePath = new TextBox();
            lblCreatedAtLabel = new Label();
            lblCreatedAt = new Label();
            lblUpdatedAtLabel = new Label();
            lblUpdatedAt = new Label();
            tabImages = new TabPage();
            pnlImagesInfo = new Panel();
            tableLayoutPanel1 = new TableLayoutPanel();
            dgvProductImages = new DataGridView();
            pnlImageButtons = new Panel();
            btnViewImage = new Button();
            btnDeleteImage = new Button();
            tabRecipes = new TabPage();
            pnlRecipesInfo = new Panel();
            pnlRecipeManagement = new Panel();
            lblAvailableRecipes = new Label();
            cbxAvailableRecipes = new ComboBox();
            btnAssignRecipe = new Button();
            lblAssignedRecipes = new Label();
            dgvAssignedRecipes = new DataGridView();
            pnlRecipeButtons = new Panel();
            btnUnassignRecipe = new Button();
            btnCreateNewRecipe = new Button();
            btnDetailRecipe = new Button();
            pnlButtons = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            btnClose = new Button();
            tabControl.SuspendLayout();
            tabBasicInfo.SuspendLayout();
            pnlBasicInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            tabImages.SuspendLayout();
            pnlImagesInfo.SuspendLayout();
            tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductImages).BeginInit();
            pnlImageButtons.SuspendLayout();
            tabRecipes.SuspendLayout();
            pnlRecipesInfo.SuspendLayout();
            pnlRecipeManagement.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAssignedRecipes).BeginInit();
            pnlRecipeButtons.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabBasicInfo);
            tabControl.Controls.Add(tabImages);
            tabControl.Controls.Add(tabRecipes);
            tabControl.Dock = DockStyle.Fill;
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.Font = new Font("Segoe UI", 9F);
            tabControl.ItemSize = new Size(120, 40);
            tabControl.Location = new Point(10, 10);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(788, 586);
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
            tabBasicInfo.Size = new Size(780, 538);
            tabBasicInfo.TabIndex = 0;
            tabBasicInfo.Text = "Thông tin cơ bản";
            // 
            // pnlBasicInfo
            // 
            pnlBasicInfo.BackColor = Color.FromArgb(42, 45, 86);
            pnlBasicInfo.Controls.Add(lblValidationStatus);
            pnlBasicInfo.Controls.Add(btnCheckUrl);
            pnlBasicInfo.Controls.Add(btnRemove);
            pnlBasicInfo.Controls.Add(btnUpload);
            pnlBasicInfo.Controls.Add(picThumbnail);
            pnlBasicInfo.Controls.Add(lblProductId);
            pnlBasicInfo.Controls.Add(txtProductId);
            pnlBasicInfo.Controls.Add(lblName);
            pnlBasicInfo.Controls.Add(txtName);
            pnlBasicInfo.Controls.Add(lblDescription);
            pnlBasicInfo.Controls.Add(txtDescription);
            pnlBasicInfo.Controls.Add(lblPrice);
            pnlBasicInfo.Controls.Add(numPrice);
            pnlBasicInfo.Controls.Add(lblCategory);
            pnlBasicInfo.Controls.Add(cbxCategory);
            pnlBasicInfo.Controls.Add(lblTax);
            pnlBasicInfo.Controls.Add(cbxTax);
            pnlBasicInfo.Controls.Add(chkIsActive);
            pnlBasicInfo.Controls.Add(lblThumbnail);
            pnlBasicInfo.Controls.Add(txtImagePath);
            pnlBasicInfo.Controls.Add(lblCreatedAtLabel);
            pnlBasicInfo.Controls.Add(lblCreatedAt);
            pnlBasicInfo.Controls.Add(lblUpdatedAtLabel);
            pnlBasicInfo.Controls.Add(lblUpdatedAt);
            pnlBasicInfo.Dock = DockStyle.Fill;
            pnlBasicInfo.Location = new Point(3, 3);
            pnlBasicInfo.Name = "pnlBasicInfo";
            pnlBasicInfo.Padding = new Padding(18, 19, 18, 19);
            pnlBasicInfo.Size = new Size(774, 532);
            pnlBasicInfo.TabIndex = 0;
            // 
            // lblValidationStatus
            // 
            lblValidationStatus.AutoSize = true;
            lblValidationStatus.ForeColor = Color.Gray;
            lblValidationStatus.Location = new Point(131, 314);
            lblValidationStatus.Name = "lblValidationStatus";
            lblValidationStatus.Size = new Size(0, 15);
            lblValidationStatus.TabIndex = 27;
            // 
            // btnCheckUrl
            // 
            btnCheckUrl.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnCheckUrl.FlatStyle = FlatStyle.Flat;
            btnCheckUrl.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            btnCheckUrl.ForeColor = Color.FromArgb(192, 255, 192);
            btnCheckUrl.Location = new Point(350, 286);
            btnCheckUrl.MaximumSize = new Size(70, 25);
            btnCheckUrl.MinimumSize = new Size(70, 25);
            btnCheckUrl.Name = "btnCheckUrl";
            btnCheckUrl.Size = new Size(70, 25);
            btnCheckUrl.TabIndex = 26;
            btnCheckUrl.Text = "Kiểm tra";
            btnCheckUrl.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            btnRemove.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnRemove.FlatStyle = FlatStyle.Flat;
            btnRemove.Font = new Font("Microsoft Sans Serif", 10F);
            btnRemove.ForeColor = Color.FromArgb(255, 99, 132);
            btnRemove.Location = new Point(427, 315);
            btnRemove.MaximumSize = new Size(110, 28);
            btnRemove.MinimumSize = new Size(110, 28);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(110, 28);
            btnRemove.TabIndex = 25;
            btnRemove.Text = "Xóa";
            btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnUpload
            // 
            btnUpload.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnUpload.FlatStyle = FlatStyle.Flat;
            btnUpload.Font = new Font("Microsoft Sans Serif", 10F);
            btnUpload.ForeColor = Color.FromArgb(192, 255, 192);
            btnUpload.Location = new Point(427, 281);
            btnUpload.MaximumSize = new Size(110, 28);
            btnUpload.MinimumSize = new Size(110, 28);
            btnUpload.Name = "btnUpload";
            btnUpload.Size = new Size(110, 28);
            btnUpload.TabIndex = 24;
            btnUpload.Text = "Tải lên";
            btnUpload.UseVisualStyleBackColor = true;
            // 
            // picThumbnail
            // 
            picThumbnail.Image = (Image)resources.GetObject("picThumbnail.Image");
            picThumbnail.Location = new Point(427, 157);
            picThumbnail.MaximumSize = new Size(110, 110);
            picThumbnail.MinimumSize = new Size(110, 110);
            picThumbnail.Name = "picThumbnail";
            picThumbnail.Size = new Size(110, 110);
            picThumbnail.SizeMode = PictureBoxSizeMode.StretchImage;
            picThumbnail.TabIndex = 19;
            picThumbnail.TabStop = false;
            // 
            // lblProductId
            // 
            lblProductId.AutoSize = true;
            lblProductId.ForeColor = Color.FromArgb(124, 141, 181);
            lblProductId.Location = new Point(18, 22);
            lblProductId.Name = "lblProductId";
            lblProductId.Size = new Size(76, 15);
            lblProductId.TabIndex = 0;
            lblProductId.Text = "ID sản phẩm:";
            // 
            // txtProductId
            // 
            txtProductId.BackColor = Color.FromArgb(24, 28, 63);
            txtProductId.BorderStyle = BorderStyle.FixedSingle;
            txtProductId.ForeColor = Color.WhiteSmoke;
            txtProductId.Location = new Point(131, 19);
            txtProductId.Name = "txtProductId";
            txtProductId.ReadOnly = true;
            txtProductId.Size = new Size(176, 23);
            txtProductId.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.ForeColor = Color.FromArgb(124, 141, 181);
            lblName.Location = new Point(18, 54);
            lblName.Name = "lblName";
            lblName.Size = new Size(84, 15);
            lblName.TabIndex = 2;
            lblName.Text = "Tên sản phẩm:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(24, 28, 63);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.WhiteSmoke;
            txtName.Location = new Point(131, 52);
            txtName.Name = "txtName";
            txtName.Size = new Size(263, 23);
            txtName.TabIndex = 3;
            // 
            // lblDescription
            // 
            lblDescription.AutoSize = true;
            lblDescription.ForeColor = Color.FromArgb(124, 141, 181);
            lblDescription.Location = new Point(18, 87);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(41, 15);
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
            txtDescription.Size = new Size(406, 60);
            txtDescription.TabIndex = 5;
            // 
            // lblPrice
            // 
            lblPrice.AutoSize = true;
            lblPrice.ForeColor = Color.FromArgb(124, 141, 181);
            lblPrice.Location = new Point(18, 160);
            lblPrice.Name = "lblPrice";
            lblPrice.Size = new Size(27, 15);
            lblPrice.TabIndex = 6;
            lblPrice.Text = "Giá:";
            // 
            // numPrice
            // 
            numPrice.BackColor = Color.FromArgb(24, 28, 63);
            numPrice.BorderStyle = BorderStyle.FixedSingle;
            numPrice.ForeColor = Color.WhiteSmoke;
            numPrice.Location = new Point(131, 157);
            numPrice.Maximum = new decimal(new int[] { 999999999, 0, 0, 0 });
            numPrice.Name = "numPrice";
            numPrice.Size = new Size(176, 23);
            numPrice.TabIndex = 7;
            numPrice.ThousandsSeparator = true;
            // 
            // lblCategory
            // 
            lblCategory.AutoSize = true;
            lblCategory.ForeColor = Color.FromArgb(124, 141, 181);
            lblCategory.Location = new Point(18, 193);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(65, 15);
            lblCategory.TabIndex = 8;
            lblCategory.Text = "Danh mục:";
            // 
            // cbxCategory
            // 
            cbxCategory.BackColor = Color.FromArgb(24, 28, 63);
            cbxCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCategory.FlatStyle = FlatStyle.Flat;
            cbxCategory.ForeColor = Color.WhiteSmoke;
            cbxCategory.FormattingEnabled = true;
            cbxCategory.Location = new Point(131, 190);
            cbxCategory.Name = "cbxCategory";
            cbxCategory.Size = new Size(219, 23);
            cbxCategory.TabIndex = 9;
            // 
            // lblTax
            // 
            lblTax.AutoSize = true;
            lblTax.ForeColor = Color.FromArgb(124, 141, 181);
            lblTax.Location = new Point(18, 226);
            lblTax.Name = "lblTax";
            lblTax.Size = new Size(37, 15);
            lblTax.TabIndex = 10;
            lblTax.Text = "Thuế:";
            // 
            // cbxTax
            // 
            cbxTax.BackColor = Color.FromArgb(24, 28, 63);
            cbxTax.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxTax.FlatStyle = FlatStyle.Flat;
            cbxTax.ForeColor = Color.WhiteSmoke;
            cbxTax.FormattingEnabled = true;
            cbxTax.Location = new Point(131, 223);
            cbxTax.Name = "cbxTax";
            cbxTax.Size = new Size(176, 23);
            cbxTax.TabIndex = 11;
            // 
            // chkIsActive
            // 
            chkIsActive.AutoSize = true;
            chkIsActive.ForeColor = Color.FromArgb(192, 255, 192);
            chkIsActive.Location = new Point(131, 256);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(83, 19);
            chkIsActive.TabIndex = 12;
            chkIsActive.Text = "Hoạt động";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // lblThumbnail
            // 
            lblThumbnail.AutoSize = true;
            lblThumbnail.ForeColor = Color.FromArgb(124, 141, 181);
            lblThumbnail.Location = new Point(18, 289);
            lblThumbnail.Name = "lblThumbnail";
            lblThumbnail.Size = new Size(81, 15);
            lblThumbnail.TabIndex = 13;
            lblThumbnail.Text = "Hình đại diện:";
            // 
            // txtImagePath
            // 
            txtImagePath.BackColor = Color.FromArgb(24, 28, 63);
            txtImagePath.BorderStyle = BorderStyle.FixedSingle;
            txtImagePath.ForeColor = Color.WhiteSmoke;
            txtImagePath.Location = new Point(131, 286);
            txtImagePath.MinimumSize = new Size(0, 25);
            txtImagePath.Name = "txtImagePath";
            txtImagePath.Size = new Size(219, 25);
            txtImagePath.TabIndex = 14;
            // 
            // lblCreatedAtLabel
            // 
            lblCreatedAtLabel.AutoSize = true;
            lblCreatedAtLabel.ForeColor = Color.FromArgb(124, 141, 181);
            lblCreatedAtLabel.Location = new Point(18, 333);
            lblCreatedAtLabel.Name = "lblCreatedAtLabel";
            lblCreatedAtLabel.Size = new Size(58, 15);
            lblCreatedAtLabel.TabIndex = 15;
            lblCreatedAtLabel.Text = "Ngày tạo:";
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.AutoSize = true;
            lblCreatedAt.ForeColor = Color.FromArgb(255, 192, 128);
            lblCreatedAt.Location = new Point(131, 331);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Size = new Size(0, 15);
            lblCreatedAt.TabIndex = 16;
            // 
            // lblUpdatedAtLabel
            // 
            lblUpdatedAtLabel.AutoSize = true;
            lblUpdatedAtLabel.ForeColor = Color.FromArgb(124, 141, 181);
            lblUpdatedAtLabel.Location = new Point(18, 356);
            lblUpdatedAtLabel.Name = "lblUpdatedAtLabel";
            lblUpdatedAtLabel.Size = new Size(87, 15);
            lblUpdatedAtLabel.TabIndex = 17;
            lblUpdatedAtLabel.Text = "Ngày cập nhật:";
            // 
            // lblUpdatedAt
            // 
            lblUpdatedAt.AutoSize = true;
            lblUpdatedAt.ForeColor = Color.FromArgb(255, 192, 128);
            lblUpdatedAt.Location = new Point(131, 354);
            lblUpdatedAt.Name = "lblUpdatedAt";
            lblUpdatedAt.Size = new Size(0, 15);
            lblUpdatedAt.TabIndex = 18;
            // 
            // tabImages
            // 
            tabImages.BackColor = Color.FromArgb(42, 45, 86);
            tabImages.Controls.Add(pnlImagesInfo);
            tabImages.Location = new Point(4, 44);
            tabImages.Name = "tabImages";
            tabImages.Padding = new Padding(3);
            tabImages.Size = new Size(780, 538);
            tabImages.TabIndex = 1;
            tabImages.Text = "Hình ảnh";
            // 
            // pnlImagesInfo
            // 
            pnlImagesInfo.BackColor = Color.FromArgb(42, 45, 86);
            pnlImagesInfo.Controls.Add(tableLayoutPanel1);
            pnlImagesInfo.Dock = DockStyle.Fill;
            pnlImagesInfo.Location = new Point(3, 3);
            pnlImagesInfo.Name = "pnlImagesInfo";
            pnlImagesInfo.Padding = new Padding(18, 19, 18, 19);
            pnlImagesInfo.Size = new Size(774, 532);
            pnlImagesInfo.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 1;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.Controls.Add(dgvProductImages, 0, 0);
            tableLayoutPanel1.Controls.Add(pnlImageButtons, 0, 1);
            tableLayoutPanel1.Dock = DockStyle.Fill;
            tableLayoutPanel1.Location = new Point(18, 19);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutPanel1.Size = new Size(738, 494);
            tableLayoutPanel1.TabIndex = 2;
            // 
            // dgvProductImages
            // 
            dgvProductImages.AllowUserToAddRows = false;
            dgvProductImages.AllowUserToDeleteRows = false;
            dgvProductImages.BackgroundColor = Color.FromArgb(24, 28, 63);
            dgvProductImages.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle10.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle10.ForeColor = Color.White;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            dgvProductImages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            dgvProductImages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle11.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle11.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle11.SelectionBackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle11.SelectionForeColor = Color.White;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            dgvProductImages.DefaultCellStyle = dataGridViewCellStyle11;
            dgvProductImages.Dock = DockStyle.Fill;
            dgvProductImages.GridColor = Color.FromArgb(124, 141, 181);
            dgvProductImages.Location = new Point(3, 3);
            dgvProductImages.MultiSelect = false;
            dgvProductImages.Name = "dgvProductImages";
            dgvProductImages.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle12.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle12.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
            dgvProductImages.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            dgvProductImages.RowHeadersWidth = 51;
            dgvProductImages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductImages.Size = new Size(732, 444);
            dgvProductImages.TabIndex = 0;
            // 
            // pnlImageButtons
            // 
            pnlImageButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlImageButtons.BackColor = Color.FromArgb(42, 45, 86);
            pnlImageButtons.Controls.Add(btnViewImage);
            pnlImageButtons.Controls.Add(btnDeleteImage);
            pnlImageButtons.Location = new Point(0, 450);
            pnlImageButtons.Margin = new Padding(0);
            pnlImageButtons.Name = "pnlImageButtons";
            pnlImageButtons.Size = new Size(738, 44);
            pnlImageButtons.TabIndex = 1;
            // 
            // btnViewImage
            // 
            btnViewImage.FlatAppearance.BorderColor = Color.Cyan;
            btnViewImage.FlatStyle = FlatStyle.Flat;
            btnViewImage.ForeColor = Color.FromArgb(255, 224, 192);
            btnViewImage.Location = new Point(1, 6);
            btnViewImage.Name = "btnViewImage";
            btnViewImage.Size = new Size(70, 28);
            btnViewImage.TabIndex = 0;
            btnViewImage.Text = "Xem";
            btnViewImage.UseVisualStyleBackColor = true;
            // 
            // btnDeleteImage
            // 
            btnDeleteImage.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnDeleteImage.FlatStyle = FlatStyle.Flat;
            btnDeleteImage.ForeColor = Color.FromArgb(255, 99, 132);
            btnDeleteImage.Location = new Point(79, 7);
            btnDeleteImage.Name = "btnDeleteImage";
            btnDeleteImage.Size = new Size(70, 28);
            btnDeleteImage.TabIndex = 1;
            btnDeleteImage.Text = "Xóa";
            btnDeleteImage.UseVisualStyleBackColor = true;
            // 
            // tabRecipes
            // 
            tabRecipes.BackColor = Color.FromArgb(42, 45, 86);
            tabRecipes.Controls.Add(pnlRecipesInfo);
            tabRecipes.Location = new Point(4, 44);
            tabRecipes.Name = "tabRecipes";
            tabRecipes.Padding = new Padding(3);
            tabRecipes.Size = new Size(780, 538);
            tabRecipes.TabIndex = 2;
            tabRecipes.Text = "Công thức";
            // 
            // pnlRecipesInfo
            // 
            pnlRecipesInfo.BackColor = Color.FromArgb(42, 45, 86);
            pnlRecipesInfo.Controls.Add(pnlRecipeManagement);
            pnlRecipesInfo.Dock = DockStyle.Fill;
            pnlRecipesInfo.Location = new Point(3, 3);
            pnlRecipesInfo.Name = "pnlRecipesInfo";
            pnlRecipesInfo.Padding = new Padding(18, 19, 18, 19);
            pnlRecipesInfo.Size = new Size(774, 532);
            pnlRecipesInfo.TabIndex = 0;
            // 
            // pnlRecipeManagement
            // 
            pnlRecipeManagement.Controls.Add(lblAvailableRecipes);
            pnlRecipeManagement.Controls.Add(cbxAvailableRecipes);
            pnlRecipeManagement.Controls.Add(btnAssignRecipe);
            pnlRecipeManagement.Controls.Add(lblAssignedRecipes);
            pnlRecipeManagement.Controls.Add(dgvAssignedRecipes);
            pnlRecipeManagement.Controls.Add(pnlRecipeButtons);
            pnlRecipeManagement.Dock = DockStyle.Fill;
            pnlRecipeManagement.Location = new Point(18, 19);
            pnlRecipeManagement.Name = "pnlRecipeManagement";
            pnlRecipeManagement.Size = new Size(738, 494);
            pnlRecipeManagement.TabIndex = 0;
            // 
            // lblAvailableRecipes
            // 
            lblAvailableRecipes.AutoSize = true;
            lblAvailableRecipes.ForeColor = Color.FromArgb(124, 141, 181);
            lblAvailableRecipes.Location = new Point(0, 8);
            lblAvailableRecipes.Name = "lblAvailableRecipes";
            lblAvailableRecipes.Size = new Size(103, 15);
            lblAvailableRecipes.TabIndex = 0;
            lblAvailableRecipes.Text = "Công thức có sẵn:";
            // 
            // cbxAvailableRecipes
            // 
            cbxAvailableRecipes.BackColor = Color.FromArgb(24, 28, 63);
            cbxAvailableRecipes.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxAvailableRecipes.FlatStyle = FlatStyle.Flat;
            cbxAvailableRecipes.ForeColor = Color.WhiteSmoke;
            cbxAvailableRecipes.FormattingEnabled = true;
            cbxAvailableRecipes.Location = new Point(0, 26);
            cbxAvailableRecipes.Name = "cbxAvailableRecipes";
            cbxAvailableRecipes.Size = new Size(300, 23);
            cbxAvailableRecipes.TabIndex = 1;
            // 
            // btnAssignRecipe
            // 
            btnAssignRecipe.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnAssignRecipe.FlatStyle = FlatStyle.Flat;
            btnAssignRecipe.ForeColor = Color.FromArgb(192, 255, 192);
            btnAssignRecipe.Location = new Point(310, 26);
            btnAssignRecipe.Name = "btnAssignRecipe";
            btnAssignRecipe.Size = new Size(80, 23);
            btnAssignRecipe.TabIndex = 2;
            btnAssignRecipe.Text = "Gán";
            btnAssignRecipe.UseVisualStyleBackColor = true;
            // 
            // lblAssignedRecipes
            // 
            lblAssignedRecipes.AutoSize = true;
            lblAssignedRecipes.ForeColor = Color.FromArgb(124, 141, 181);
            lblAssignedRecipes.Location = new Point(0, 65);
            lblAssignedRecipes.Name = "lblAssignedRecipes";
            lblAssignedRecipes.Size = new Size(105, 15);
            lblAssignedRecipes.TabIndex = 3;
            lblAssignedRecipes.Text = "Công thức đã gán:";
            // 
            // dgvAssignedRecipes
            // 
            dgvAssignedRecipes.AllowUserToAddRows = false;
            dgvAssignedRecipes.AllowUserToDeleteRows = false;
            dgvAssignedRecipes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAssignedRecipes.BackgroundColor = Color.FromArgb(24, 28, 63);
            dgvAssignedRecipes.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle7.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle7.ForeColor = Color.White;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgvAssignedRecipes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgvAssignedRecipes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle8.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle8.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle8.SelectionBackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle8.SelectionForeColor = Color.White;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvAssignedRecipes.DefaultCellStyle = dataGridViewCellStyle8;
            dgvAssignedRecipes.GridColor = Color.FromArgb(124, 141, 181);
            dgvAssignedRecipes.Location = new Point(0, 83);
            dgvAssignedRecipes.MultiSelect = false;
            dgvAssignedRecipes.Name = "dgvAssignedRecipes";
            dgvAssignedRecipes.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle9.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle9.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dgvAssignedRecipes.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dgvAssignedRecipes.RowHeadersWidth = 51;
            dgvAssignedRecipes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAssignedRecipes.Size = new Size(738, 365);
            dgvAssignedRecipes.TabIndex = 4;
            // 
            // pnlRecipeButtons
            // 
            pnlRecipeButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlRecipeButtons.BackColor = Color.FromArgb(42, 45, 86);
            pnlRecipeButtons.Controls.Add(btnUnassignRecipe);
            pnlRecipeButtons.Controls.Add(btnCreateNewRecipe);
            pnlRecipeButtons.Controls.Add(btnDetailRecipe);
            pnlRecipeButtons.Location = new Point(0, 454);
            pnlRecipeButtons.Name = "pnlRecipeButtons";
            pnlRecipeButtons.Size = new Size(738, 40);
            pnlRecipeButtons.TabIndex = 5;
            // 
            // btnUnassignRecipe
            // 
            btnUnassignRecipe.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnUnassignRecipe.FlatStyle = FlatStyle.Flat;
            btnUnassignRecipe.ForeColor = Color.FromArgb(255, 99, 132);
            btnUnassignRecipe.Location = new Point(0, 6);
            btnUnassignRecipe.Name = "btnUnassignRecipe";
            btnUnassignRecipe.Size = new Size(80, 28);
            btnUnassignRecipe.TabIndex = 0;
            btnUnassignRecipe.Text = "Bỏ gán";
            btnUnassignRecipe.UseVisualStyleBackColor = true;
            // 
            // btnCreateNewRecipe
            // 
            btnCreateNewRecipe.FlatAppearance.BorderColor = Color.Cyan;
            btnCreateNewRecipe.FlatStyle = FlatStyle.Flat;
            btnCreateNewRecipe.ForeColor = Color.FromArgb(255, 224, 192);
            btnCreateNewRecipe.Location = new Point(90, 6);
            btnCreateNewRecipe.Name = "btnCreateNewRecipe";
            btnCreateNewRecipe.Size = new Size(100, 28);
            btnCreateNewRecipe.TabIndex = 1;
            btnCreateNewRecipe.Text = "Tạo mới";
            btnCreateNewRecipe.UseVisualStyleBackColor = true;
            // 
            // btnDetailRecipe
            // 
            btnDetailRecipe.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnDetailRecipe.FlatStyle = FlatStyle.Flat;
            btnDetailRecipe.ForeColor = Color.FromArgb(192, 255, 192);
            btnDetailRecipe.Location = new Point(200, 6);
            btnDetailRecipe.Name = "btnDetailRecipe";
            btnDetailRecipe.Size = new Size(86, 28);
            btnDetailRecipe.TabIndex = 2;
            btnDetailRecipe.Text = "Chi tiết";
            btnDetailRecipe.UseVisualStyleBackColor = true;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(24, 28, 63);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(10, 596);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 8, 0, 8);
            pnlButtons.Size = new Size(788, 56);
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
            btnSave.Location = new Point(551, 14);
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
            btnCancel.Location = new Point(630, 14);
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
            btnClose.Location = new Point(709, 14);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(70, 28);
            btnClose.TabIndex = 2;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // FrmProductDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(808, 662);
            Controls.Add(tabControl);
            Controls.Add(pnlButtons);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MaximumSize = new Size(808, 662);
            MinimizeBox = false;
            MinimumSize = new Size(808, 662);
            Name = "FrmProductDetails";
            Padding = new Padding(10);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết sản phẩm";
            tabControl.ResumeLayout(false);
            tabBasicInfo.ResumeLayout(false);
            pnlBasicInfo.ResumeLayout(false);
            pnlBasicInfo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).EndInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).EndInit();
            tabImages.ResumeLayout(false);
            pnlImagesInfo.ResumeLayout(false);
            tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProductImages).EndInit();
            pnlImageButtons.ResumeLayout(false);
            tabRecipes.ResumeLayout(false);
            pnlRecipesInfo.ResumeLayout(false);
            pnlRecipeManagement.ResumeLayout(false);
            pnlRecipeManagement.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAssignedRecipes).EndInit();
            pnlRecipeButtons.ResumeLayout(false);
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControl;
        private TabPage tabBasicInfo;
        private Panel pnlBasicInfo;
        private Label lblProductId;
        private TextBox txtProductId;
        private Label lblName;
        private TextBox txtName;
        private Label lblDescription;
        private TextBox txtDescription;
        private Label lblPrice;
        private NumericUpDown numPrice;
        private Label lblCategory;
        private ComboBox cbxCategory;
        private Label lblTax;
        private ComboBox cbxTax;
        private CheckBox chkIsActive;
        private Label lblThumbnail;
        private Label lblCreatedAtLabel;
        private Label lblCreatedAt;
        private Label lblUpdatedAtLabel;
        private Label lblUpdatedAt;
        private TabPage tabImages;
        private Panel pnlImagesInfo;
        private DataGridView dgvProductImages;
        private Panel pnlImageButtons;
        private Button btnViewImage;
        private Button btnDeleteImage;
        private TabPage tabRecipes;
        private Panel pnlRecipesInfo;
        private Panel pnlRecipeManagement;
        private Label lblAvailableRecipes;
        private ComboBox cbxAvailableRecipes;
        private Button btnAssignRecipe;
        private Label lblAssignedRecipes;
        private DataGridView dgvAssignedRecipes;
        private Panel pnlRecipeButtons;
        private Button btnUnassignRecipe;
        private Button btnCreateNewRecipe;
        private Button btnDetailRecipe;
        private Panel pnlButtons;
        private Button btnSave;
        private Button btnCancel;
        private Button btnClose;
        private PictureBox picThumbnail;
        private TextBox txtImagePath;
        private Button btnRemove;
        private Button btnUpload;
        private Button btnCheckUrl;
        private Label lblValidationStatus;
        private ToolTip toolTip1;
        private TableLayoutPanel tableLayoutPanel1;
    }
}