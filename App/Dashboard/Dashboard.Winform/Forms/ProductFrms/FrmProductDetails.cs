using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dashboard.Winform.ViewModels;
using Dashboard.Winform.Helpers;

namespace Dashboard.Winform.Forms
{
    public partial class FrmProductDetails : Form
    {
        #region Fields
        private readonly bool _isEditMode;
        private readonly long? _productId;
        private ProductDetailViewModel _product;
        #endregion

        #region Properties
        public DialogResult Result { get; private set; }
        public ProductDetailViewModel Product => _product;
        #endregion

        #region Constructor
        public FrmProductDetails(long? productId = null, ProductDetailViewModel? product = null)
        {
            _productId = productId;
            _isEditMode = productId.HasValue;
            _product = product ?? new ProductDetailViewModel();

            InitializeComponent();
            InitializeFormSettings();
            LoadInitialData();
            SetupEventHandlers();
            SetupDataGridViews();

            if (_isEditMode)
            {
                PopulateFormWithData();
                Text = $"Chi tiết sản phẩm - {_product.Name}";
                btnClose.Visible = true;
                btnSave.Text = "Cập nhật";
            }
            else
            {
                Text = "Thêm sản phẩm mới";
                btnClose.Visible = false;
                btnSave.Text = "Thêm";
            }

            // Setup dark theme for TabControl using the helper
            TabControlHelper.SetupDarkTheme(tabControl);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                TabControlHelper.CleanupDarkTheme(tabControl);
                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Initialization Methods
        private void InitializeFormSettings()
        {
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
            this.BackColor = Color.FromArgb(24, 28, 63);
        }

        private void SetupEventHandlers()
        {
            // Form buttons
            btnSave.Click += (s, e) => BtnSave_Click(s!, e);
            btnCancel.Click += (s, e) => BtnCancel_Click(s!, e);
            btnClose.Click += (s, e) => BtnClose_Click(s!, e);

            // Image management buttons
            btnAddImage.Click += (s, e) => BtnAddImage_Click(s!, e);
            btnDeleteImage.Click += (s, e) => BtnDeleteImage_Click(s!, e);

            // Recipe management buttons
            btnAddRecipe.Click += (s, e) => BtnAddRecipe_Click(s!, e);
            btnEditRecipe.Click += (s, e) => BtnEditRecipe_Click(s!, e);
            btnDeleteRecipe.Click += (s, e) => BtnDeleteRecipe_Click(s!, e);
            //btnViewRecipeDetails.Click += (s, e) => BtnViewRecipeDetails_Click(s!, e);

            // Validation events
            txtName.Validating += (s, e) => TxtName_Validating(s!, e);
            numPrice.Validating += (s, e) => NumPrice_Validating(s!, e);
        }

        private void SetupDataGridViews()
        {
            SetupProductImagesDataGridView();
            SetupRecipesDataGridView();
        }

        private void SetupProductImagesDataGridView()
        {
            dgvProductImages.AutoGenerateColumns = false;
            dgvProductImages.Columns.Clear();

            dgvProductImages.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductImageViewModel.Id),
                HeaderText = "ID",
                Width = 60
            });

            dgvProductImages.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductImageViewModel.ImageUrl),
                HeaderText = "URL hình ảnh",
                Width = 300
            });

            dgvProductImages.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductImageViewModel.CreatedAt),
                HeaderText = "Ngày tạo",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
        }

        private void SetupRecipesDataGridView()
        {
            dgvRecipes.AutoGenerateColumns = false;
            dgvRecipes.Columns.Clear();

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Id),
                HeaderText = "ID",
                Width = 60
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Name),
                HeaderText = "Tên công thức",
                Width = 200
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Description),
                HeaderText = "Mô tả",
                Width = 180
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.ServingSize),
                HeaderText = "Khẩu phần",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N1" }
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Unit),
                HeaderText = "Đơn vị",
                Width = 80
            });

            dgvRecipes.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.IsActive),
                HeaderText = "Hoạt động",
                Width = 80
            });

            dgvRecipes.DataSource = _product.Recipes;
        }

        private void LoadInitialData()
        {
            // Load categories
            cbxCategory.Items.Clear();
            cbxCategory.Items.Add(new { Id = 1, Name = "Đồ uống" });
            cbxCategory.Items.Add(new { Id = 2, Name = "Đồ ăn" });
            cbxCategory.Items.Add(new { Id = 3, Name = "Bánh kẹo" });
            cbxCategory.DisplayMember = "Name";
            cbxCategory.ValueMember = "Id";

            // Load taxes
            cbxTax.Items.Clear();
            cbxTax.Items.Add(new { Id = 1, Name = "VAT 10%", Rate = 10 });
            cbxTax.Items.Add(new { Id = 2, Name = "VAT 8%", Rate = 8 });
            cbxTax.Items.Add(new { Id = 3, Name = "VAT 5%", Rate = 5 });
            cbxTax.DisplayMember = "Name";
            cbxTax.ValueMember = "Id";

            // Set default values
            chkIsActive.Checked = true;
            numPrice.Value = 0;
        }

        private void PopulateFormWithData()
        {
            if (_product == null) return;

            try
            {
                // Basic info
                txtProductId.Text = _product.Id.ToString();
                txtName.Text = _product.Name;
                txtDescription.Text = _product.Description;
                numPrice.Value = _product.Price;
                chkIsActive.Checked = _product.IsActive;
                txtThumbnail.Text = _product.Thumbnail ?? "";

                // Set combo box selections
                SetComboBoxSelection(cbxCategory, _product.CategoryId ?? 0);
                SetComboBoxSelection(cbxTax, _product.TaxId ?? 0);

                // Audit fields
                lblCreatedAt.Text = _product.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                lblUpdatedAt.Text = _product.UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";

                // Load related data
                dgvProductImages.DataSource = _product.ProductImages;
                dgvRecipes.DataSource = _product.Recipes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Event Handlers
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    CollectFormData();
                    Result = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void BtnAddImage_Click(object sender, EventArgs e)
        {
            // TODO: Implement add image dialog
            MessageBox.Show("Chức năng thêm hình ảnh sẽ được triển khai",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnDeleteImage_Click(object sender, EventArgs e)
        {
            if (dgvProductImages.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa hình ảnh này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    var selectedImage = dgvProductImages.SelectedRows[0].DataBoundItem as ProductImageViewModel;
                    if (selectedImage != null)
                    {
                        _product.ProductImages.Remove(selectedImage);
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hình ảnh để xóa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnAddRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                var newRecipe = new RecipeViewModel
                {
                    ProductId = _product.Id,
                    ProductName = _product.Name,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };

                using var recipeForm = new FrmRecipeDetails(null, ConvertToRecipeDetailViewModel(newRecipe));
                var result = recipeForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    var updatedRecipe = ConvertToRecipeViewModel(recipeForm.Recipe);
                    _product.Recipes.Add(updatedRecipe);
                    MessageBox.Show("Thêm công thức thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm công thức: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnEditRecipe_Click(object sender, EventArgs e)
        {
            var selectedRecipe = GetSelectedRecipe();
            if (selectedRecipe != null)
            {
                try
                {
                    using var recipeForm = new FrmRecipeDetails(selectedRecipe.Id, ConvertToRecipeDetailViewModel(selectedRecipe));
                    var result = recipeForm.ShowDialog(this);

                    if (result == DialogResult.OK)
                    {
                        var updatedRecipe = ConvertToRecipeViewModel(recipeForm.Recipe);
                        var index = _product.Recipes.IndexOf(selectedRecipe);
                        if (index >= 0)
                        {
                            _product.Recipes[index] = updatedRecipe;
                        }
                        MessageBox.Show("Cập nhật công thức thành công!", "Thông báo",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi chỉnh sửa công thức: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công thức để chỉnh sửa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDeleteRecipe_Click(object sender, EventArgs e)
        {
            var selectedRecipe = GetSelectedRecipe();
            if (selectedRecipe != null)
            {
                var result = MessageBox.Show(
                    $"Bạn có chắc chắn muốn xóa công thức '{selectedRecipe.Name}'?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    _product.Recipes.Remove(selectedRecipe);
                    MessageBox.Show("Xóa công thức thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công thức để xóa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnViewRecipeDetails_Click(object sender, EventArgs e)
        {
            var selectedRecipe = GetSelectedRecipe();
            if (selectedRecipe != null)
            {
                try
                {
                    using var recipeForm = new FrmRecipeDetails(selectedRecipe.Id, ConvertToRecipeDetailViewModel(selectedRecipe), isReadOnly: true);
                    recipeForm.ShowDialog(this);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xem chi tiết công thức: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công thức để xem chi tiết",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Validation Event Handlers
        private void TxtName_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox?.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void NumPrice_Validating(object sender, CancelEventArgs e)
        {
            var numericUpDown = sender as NumericUpDown;
            if (numericUpDown?.Value <= 0)
            {
                MessageBox.Show("Giá sản phẩm phải lớn hơn 0", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }
        #endregion

        #region Helper Methods
        private bool ValidateForm()
        {
            // Required field validation
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên sản phẩm không được để trống", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (numPrice.Value <= 0)
            {
                MessageBox.Show("Giá sản phẩm phải lớn hơn 0", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                numPrice.Focus();
                return false;
            }

            if (cbxCategory.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn danh mục", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbxCategory.Focus();
                return false;
            }

            return true;
        }

        private void CollectFormData()
        {
            _product.Name = txtName.Text.Trim();
            _product.Description = txtDescription.Text.Trim();
            _product.Price = numPrice.Value;
            _product.IsActive = chkIsActive.Checked;
            _product.Thumbnail = txtThumbnail.Text.Trim();

            if (cbxCategory.SelectedItem != null)
            {
                dynamic categoryItem = cbxCategory.SelectedItem;
                _product.CategoryId = categoryItem.Id;
            }

            if (cbxTax.SelectedItem != null)
            {
                dynamic taxItem = cbxTax.SelectedItem;
                _product.TaxId = taxItem.Id;
            }
        }

        private void SetComboBoxSelection(ComboBox comboBox, long id)
        {
            for (int i = 0; i < comboBox.Items.Count; i++)
            {
                var item = comboBox.Items[i];
                if (item != null)
                {
                    dynamic dynItem = item;
                    if (dynItem.Id == id)
                    {
                        comboBox.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        private RecipeViewModel? GetSelectedRecipe()
        {
            if (dgvRecipes.SelectedRows.Count > 0)
            {
                return dgvRecipes.SelectedRows[0].DataBoundItem as RecipeViewModel;
            }
            return null;
        }

        private RecipeDetailViewModel ConvertToRecipeDetailViewModel(RecipeViewModel recipe)
        {
            return new RecipeDetailViewModel
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                ProductId = recipe.ProductId,
                ProductName = recipe.ProductName,
                ServingSize = recipe.ServingSize,
                Unit = recipe.Unit,
                IsActive = recipe.IsActive,
                Notes = recipe.Notes,
                CreatedAt = recipe.CreatedAt,
                UpdatedAt = recipe.UpdatedAt,
                RecipeIngredients = new BindingList<RecipeIngredientViewModel>()
            };
        }

        private RecipeViewModel ConvertToRecipeViewModel(RecipeDetailViewModel recipeDetail)
        {
            return new RecipeViewModel
            {
                Id = recipeDetail.Id,
                Name = recipeDetail.Name,
                Description = recipeDetail.Description,
                ProductId = recipeDetail.ProductId,
                ProductName = recipeDetail.ProductName,
                ServingSize = recipeDetail.ServingSize,
                Unit = recipeDetail.Unit,
                IsActive = recipeDetail.IsActive,
                Notes = recipeDetail.Notes,
                CreatedAt = recipeDetail.CreatedAt,
                UpdatedAt = recipeDetail.UpdatedAt
            };
        }
        #endregion
    }
}