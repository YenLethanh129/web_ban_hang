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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmProductDetails));
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            tabControl = new TabControl();
            tabBasicInfo = new TabPage();
            pnlBasicInfo = new Panel();
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
            dgvProductImages = new DataGridView();
            pnlImageButtons = new Panel();
            btnAddImage = new Button();
            btnDeleteImage = new Button();
            tabRecipes = new TabPage();
            pnlRecipesInfo = new Panel();
            dgvRecipes = new DataGridView();
            pnlRecipeButtons = new Panel();
            btnAddRecipe = new Button();
            btnEditRecipe = new Button();
            btnViewRecipeDetails = new Button();
            btnDeleteRecipe = new Button();
            pnlButtons = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            btnClose = new Button();
            lblValidationStatus = new Label();
            tabControl.SuspendLayout();
            tabBasicInfo.SuspendLayout();
            pnlBasicInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picThumbnail).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numPrice).BeginInit();
            tabImages.SuspendLayout();
            pnlImagesInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProductImages).BeginInit();
            pnlImageButtons.SuspendLayout();
            tabRecipes.SuspendLayout();
            pnlRecipesInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRecipes).BeginInit();
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
            pnlImagesInfo.Controls.Add(dgvProductImages);
            pnlImagesInfo.Controls.Add(pnlImageButtons);
            pnlImagesInfo.Dock = DockStyle.Fill;
            pnlImagesInfo.Location = new Point(3, 3);
            pnlImagesInfo.Name = "pnlImagesInfo";
            pnlImagesInfo.Padding = new Padding(18, 19, 18, 19);
            pnlImagesInfo.Size = new Size(774, 532);
            pnlImagesInfo.TabIndex = 0;
            // 
            // dgvProductImages
            // 
            dgvProductImages.AllowUserToAddRows = false;
            dgvProductImages.AllowUserToDeleteRows = false;
            dgvProductImages.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvProductImages.BackgroundColor = Color.FromArgb(24, 28, 63);
            dgvProductImages.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle1.ForeColor = Color.White;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvProductImages.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvProductImages.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle2.SelectionForeColor = Color.White;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvProductImages.DefaultCellStyle = dataGridViewCellStyle2;
            dgvProductImages.GridColor = Color.FromArgb(124, 141, 181);
            dgvProductImages.Location = new Point(18, 19);
            dgvProductImages.MultiSelect = false;
            dgvProductImages.Name = "dgvProductImages";
            dgvProductImages.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvProductImages.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvProductImages.RowHeadersWidth = 51;
            dgvProductImages.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProductImages.Size = new Size(739, 453);
            dgvProductImages.TabIndex = 0;
            // 
            // pnlImageButtons
            // 
            pnlImageButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlImageButtons.BackColor = Color.FromArgb(42, 45, 86);
            pnlImageButtons.Controls.Add(btnAddImage);
            pnlImageButtons.Controls.Add(btnDeleteImage);
            pnlImageButtons.Location = new Point(18, 481);
            pnlImageButtons.Name = "pnlImageButtons";
            pnlImageButtons.Size = new Size(739, 38);
            pnlImageButtons.TabIndex = 1;
            // 
            // btnAddImage
            // 
            btnAddImage.FlatAppearance.BorderColor = Color.Cyan;
            btnAddImage.FlatStyle = FlatStyle.Flat;
            btnAddImage.ForeColor = Color.FromArgb(255, 224, 192);
            btnAddImage.Location = new Point(0, 5);
            btnAddImage.Name = "btnAddImage";
            btnAddImage.Size = new Size(70, 28);
            btnAddImage.TabIndex = 0;
            btnAddImage.Text = "Thêm";
            btnAddImage.UseVisualStyleBackColor = true;
            // 
            // btnDeleteImage
            // 
            btnDeleteImage.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnDeleteImage.FlatStyle = FlatStyle.Flat;
            btnDeleteImage.ForeColor = Color.FromArgb(255, 99, 132);
            btnDeleteImage.Location = new Point(79, 5);
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
            pnlRecipesInfo.Controls.Add(dgvRecipes);
            pnlRecipesInfo.Controls.Add(pnlRecipeButtons);
            pnlRecipesInfo.Dock = DockStyle.Fill;
            pnlRecipesInfo.Location = new Point(3, 3);
            pnlRecipesInfo.Name = "pnlRecipesInfo";
            pnlRecipesInfo.Padding = new Padding(18, 19, 18, 19);
            pnlRecipesInfo.Size = new Size(774, 532);
            pnlRecipesInfo.TabIndex = 0;
            // 
            // dgvRecipes
            // 
            dgvRecipes.AllowUserToAddRows = false;
            dgvRecipes.AllowUserToDeleteRows = false;
            dgvRecipes.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvRecipes.BackgroundColor = Color.FromArgb(24, 28, 63);
            dgvRecipes.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle4.ForeColor = Color.White;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvRecipes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvRecipes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(107, 83, 255);
            dataGridViewCellStyle5.SelectionForeColor = Color.White;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvRecipes.DefaultCellStyle = dataGridViewCellStyle5;
            dgvRecipes.GridColor = Color.FromArgb(124, 141, 181);
            dgvRecipes.Location = new Point(18, 19);
            dgvRecipes.MultiSelect = false;
            dgvRecipes.Name = "dgvRecipes";
            dgvRecipes.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.WhiteSmoke;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvRecipes.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvRecipes.RowHeadersWidth = 51;
            dgvRecipes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecipes.Size = new Size(739, 453);
            dgvRecipes.TabIndex = 0;
            // 
            // pnlRecipeButtons
            // 
            pnlRecipeButtons.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pnlRecipeButtons.BackColor = Color.FromArgb(42, 45, 86);
            pnlRecipeButtons.Controls.Add(btnAddRecipe);
            pnlRecipeButtons.Controls.Add(btnEditRecipe);
            pnlRecipeButtons.Controls.Add(btnViewRecipeDetails);
            pnlRecipeButtons.Controls.Add(btnDeleteRecipe);
            pnlRecipeButtons.Location = new Point(18, 481);
            pnlRecipeButtons.Name = "pnlRecipeButtons";
            pnlRecipeButtons.Size = new Size(739, 38);
            pnlRecipeButtons.TabIndex = 1;
            // 
            // btnAddRecipe
            // 
            btnAddRecipe.FlatAppearance.BorderColor = Color.Cyan;
            btnAddRecipe.FlatStyle = FlatStyle.Flat;
            btnAddRecipe.ForeColor = Color.FromArgb(255, 224, 192);
            btnAddRecipe.Location = new Point(0, 5);
            btnAddRecipe.Name = "btnAddRecipe";
            btnAddRecipe.Size = new Size(70, 28);
            btnAddRecipe.TabIndex = 0;
            btnAddRecipe.Text = "Thêm";
            btnAddRecipe.UseVisualStyleBackColor = true;
            // 
            // btnEditRecipe
            // 
            btnEditRecipe.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnEditRecipe.FlatStyle = FlatStyle.Flat;
            btnEditRecipe.ForeColor = Color.FromArgb(192, 255, 192);
            btnEditRecipe.Location = new Point(79, 5);
            btnEditRecipe.Name = "btnEditRecipe";
            btnEditRecipe.Size = new Size(70, 28);
            btnEditRecipe.TabIndex = 1;
            btnEditRecipe.Text = "Sửa";
            btnEditRecipe.UseVisualStyleBackColor = true;
            // 
            // btnViewRecipeDetails
            // 
            btnViewRecipeDetails.FlatAppearance.BorderColor = Color.FromArgb(255, 193, 7);
            btnViewRecipeDetails.FlatStyle = FlatStyle.Flat;
            btnViewRecipeDetails.ForeColor = Color.FromArgb(255, 193, 7);
            btnViewRecipeDetails.Location = new Point(158, 5);
            btnViewRecipeDetails.Name = "btnViewRecipeDetails";
            btnViewRecipeDetails.Size = new Size(70, 28);
            btnViewRecipeDetails.TabIndex = 2;
            btnViewRecipeDetails.Text = "Chi tiết";
            btnViewRecipeDetails.UseVisualStyleBackColor = true;
            // 
            // btnDeleteRecipe
            // 
            btnDeleteRecipe.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnDeleteRecipe.FlatStyle = FlatStyle.Flat;
            btnDeleteRecipe.ForeColor = Color.FromArgb(255, 99, 132);
            btnDeleteRecipe.Location = new Point(237, 5);
            btnDeleteRecipe.Name = "btnDeleteRecipe";
            btnDeleteRecipe.Size = new Size(70, 28);
            btnDeleteRecipe.TabIndex = 3;
            btnDeleteRecipe.Text = "Xóa";
            btnDeleteRecipe.UseVisualStyleBackColor = true;
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
            // lblValidationStatus
            // 
            lblValidationStatus.AutoSize = true;
            lblValidationStatus.Location = new Point(131, 314);
            lblValidationStatus.Name = "lblValidationStatus";
            lblValidationStatus.Size = new Size(0, 15);
            lblValidationStatus.TabIndex = 27;
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
            ((System.ComponentModel.ISupportInitialize)dgvProductImages).EndInit();
            pnlImageButtons.ResumeLayout(false);
            tabRecipes.ResumeLayout(false);
            pnlRecipesInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRecipes).EndInit();
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
        private Button btnAddImage;
        private Button btnDeleteImage;
        private TabPage tabRecipes;
        private Panel pnlRecipesInfo;
        private DataGridView dgvRecipes;
        private Panel pnlRecipeButtons;
        private Button btnAddRecipe;
        private Button btnEditRecipe;
        private Button btnViewRecipeDetails;
        private Button btnDeleteRecipe;
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
    }
}