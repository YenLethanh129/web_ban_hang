using Dashboard.Common.Constants;
using Dashboard.Winform.Helpers;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms
{
    public partial class FrmProductDetails : Form
    {
        #region Fields
        private bool _isEditMode;
        private long? _productId;
        private ProductDetailViewModel _product;
        private readonly IProductDetailPresenter _presenter;
        private bool _isDataLoaded = false;
        private readonly ILogger<FrmProductDetails>? _logger;

        // Image handling fields
        private bool _isUrlChanged = false;
        private bool _isUrlValidated = false;
        private string? _tempImagePath = null;
        private string? _originalImagePath = null;
        private static readonly HttpClient _httpClient = new HttpClient();
        #endregion

        #region Properties
        public DialogResult Result { get; private set; }
        public ProductDetailViewModel Product => _product;
        #endregion

        #region Constructor
        public FrmProductDetails(
            IProductDetailPresenter presenter,
            ILogger<FrmProductDetails>? logger = null,
            long? productId = null,
            ProductDetailViewModel? product = null)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _logger = logger;
            _productId = productId;
            _product = product ?? new ProductDetailViewModel();
            _isEditMode = productId.HasValue;

            InitializeComponent();
            InitializeFormSettings();
            SetupEventHandlers();
            SetupDataGridViews();

            _presenter.OnProductSaved += Presenter_OnProductSaved;

            UpdateFormMode();

            try { TabControlHelper.SetupDarkTheme(tabControl); } catch { }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try { TabControlHelper.CleanupDarkTheme(tabControl); } catch { }
                components?.Dispose();
                // Clean up temp files
                CleanupTempFiles();
                // Unsubscribe from presenter events
                if (_presenter != null)
                {
                    _presenter.OnProductSaved -= Presenter_OnProductSaved;
                }
            }
            base.Dispose(disposing);
        }

        public void SetInitData(long? productId, ProductDetailViewModel? product)
        {
            _productId = productId;
            _product = product ?? new ProductDetailViewModel();
            _isEditMode = productId.HasValue;
            UpdateFormMode();
        }

        #endregion

        #region Initialization Methods
        private void InitializeFormSettings()
        {
            Size = new Size(900, 700);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            BackColor = Color.FromArgb(24, 28, 63);
        }

        private void UpdateFormMode()
        {
            if (_isEditMode)
            {
                Text = $"Chi tiết sản phẩm - {_product?.Name ?? "N/A"}";
                btnClose.Visible = true;
                btnSave.Text = "Cập nhật";
            }
            else
            {
                Text = "Thêm sản phẩm mới";
                btnClose.Visible = false;
                btnSave.Text = "Thêm";
            }
        }

        private void SetupEventHandlers()
        {
            // Form load event
            this.Load += (s, e) => FrmProductDetails_Load(s!, e);

            // Form buttons
            btnSave.Click += (s, e) => BtnSave_Click(s!, e);
            btnCancel.Click += (s, e) => BtnCancel_Click(s!, e);
            btnClose.Click += (s, e) => BtnClose_Click(s!, e);

            // Image management buttons
            btnAddImage.Click += (s, e) => BtnAddImage_Click(s!, e);
            btnDeleteImage.Click += (s, e) => BtnDeleteImage_Click(s!, e);

            btnCheckUrl.Click += async (s, e) => await BtnCheckUrl_ClickAsync(s!, e);

            // Recipe management buttons
            btnAddRecipe.Click += (s, e) => BtnAddRecipe_Click(s!, e);
            btnEditRecipe.Click += (s, e) => BtnEditRecipe_Click(s!, e);
            btnDeleteRecipe.Click += (s, e) => BtnDeleteRecipe_Click(s!, e);
            btnViewRecipeDetails.Click += (s, e) => BtnViewRecipeDetails_Click(s!, e);

            // Image upload/remove buttons
            btnUpload.Click += (s, e) => BtnUpload_Click(s!, e);
            btnRemove.Click += (s, e) => BtnRemove_Click(s!, e);

            // Image path text changed
            txtImagePath.TextChanged += (s, e) => TxtImagePath_TextChanged(s!, e);

            // Validation events
            txtName.Validating += (s, e) => TxtName_Validating(s!, e);
            numPrice.Validating += (s, e) => NumPrice_Validating(s!, e);
        }

        private async void FrmProductDetails_Load(object sender, EventArgs e)
        {
            if (!_isDataLoaded)
            {
                await LoadInitialDataAsync();
                _isDataLoaded = true;
            }
        }

        private async Task LoadInitialDataAsync()
        {
            try
            {
                SetLoadingState(true);

                await LoadDropdownDataAsync();

                if (_isEditMode && _productId.HasValue)
                {
                    var vm = await _presenter.LoadProductAsync(_productId.Value);
                    if (vm != null)
                    {
                        _product = vm;
                    }
                }

                PopulateFormWithData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task LoadDropdownDataAsync()
        {
            try
            {
                var categories = await _presenter.LoadCategoriesAsync();
                cbxCategory.DataSource = null;
                cbxCategory.DisplayMember = "Name";
                cbxCategory.ValueMember = "Id";
                cbxCategory.DataSource = categories;

                var taxes = await _presenter.LoadTaxesAsync();
                cbxTax.DataSource = null;
                cbxTax.DisplayMember = "Name";
                cbxTax.ValueMember = "Id";
                cbxTax.DataSource = taxes;

                if (cbxCategory.Items.Count > 0 && cbxCategory.SelectedIndex == -1)
                    cbxCategory.SelectedIndex = 0;

                if (cbxTax.Items.Count > 0 && cbxTax.SelectedIndex == -1)
                    cbxTax.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error loading dropdown data");
                LoadFallbackDropdownData();
            }
        }

        private void LoadFallbackDropdownData()
        {
            // Fallback categories
            cbxCategory.Items.Clear();
            cbxCategory.Items.Add(new CategoryViewModel { Id = 1L, Name = "Đồ uống" });
            cbxCategory.Items.Add(new CategoryViewModel { Id = 2L, Name = "Đồ ăn" });
            cbxCategory.Items.Add(new CategoryViewModel { Id = 3L, Name = "Bánh kẹo" });
            cbxCategory.DisplayMember = "Name";
            cbxCategory.ValueMember = "Id";

            // Fallback taxes
            cbxTax.Items.Clear();
            cbxTax.Items.Add(new TaxViewModel { Id = 1L, Name = "VAT 10%", Rate = 10 });
            cbxTax.Items.Add(new TaxViewModel { Id = 2L, Name = "VAT 8%", Rate = 8 });
            cbxTax.Items.Add(new TaxViewModel { Id = 3L, Name = "VAT 5%", Rate = 5 });
            cbxTax.DisplayMember = "Name";
            cbxTax.ValueMember = "Id";
        }

        private void Presenter_OnProductSaved(object? sender, ProductDetailViewModel? vm)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() =>
                {
                    try
                    {
                        if (vm != null)
                        {
                            _product = vm;
                            PopulateFormWithData();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Lỗi",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }));
            }
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
        }

        private void PopulateFormWithData()
        {
            if (_product == null) return;

            try
            {
                txtProductId.Text = _product.Id.ToString();
                txtName.Text = _product.Name ?? string.Empty;
                txtDescription.Text = _product.Description ?? string.Empty;
                numPrice.Value = _product.Price;
                chkIsActive.Checked = _product.IsActive;

                // Store original image path
                _originalImagePath = _product.ThumbnailPath ?? "";
                txtImagePath.Text = _originalImagePath;

                SetComboBoxSelection(cbxCategory, _product.CategoryId ?? 0);
                SetComboBoxSelection(cbxTax, _product.TaxId ?? 0);

                lblCreatedAt.Text = _product.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                lblUpdatedAt.Text = _product.UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";

                dgvProductImages.DataSource = _product.ProductImages;
                dgvRecipes.DataSource = _product.Recipes;

                LoadThumbnailImage();

                if (!_isEditMode)
                {
                    chkIsActive.Checked = true;
                    numPrice.Value = 0;
                }

                // Reset URL validation flags
                _isUrlChanged = false;
                _isUrlValidated = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Event Handlers
        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            if (btnSave.Tag?.Equals("processing") == true) return;
            btnSave.Tag = "processing";

            try
            {
                SetLoadingState(true);

                await ProcessImageBeforeSaveAsync();

                _product = BuildProductFromForm();

                ProductDetailViewModel? result = null;
                if (_isEditMode)
                {
                    result = await _presenter.UpdateProductAsync(_product);
                }
                else
                {
                    result = await _presenter.CreateProductAsync(_product);
                }

                if (result != null)
                {
                    _product = result;
                    CleanupTempFiles();
                    Result = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Lưu sản phẩm thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
                btnSave.Tag = null;
            }
        }


        private async Task ProcessImageBeforeSaveAsync()
        {
            try
            {
                var currentPath = txtImagePath.Text.Trim();

                if (string.IsNullOrEmpty(currentPath))
                {
                    return;
                }

                // If it's a URL and already validated, download and save it
                if (IsValidImageUrl(currentPath) && _isUrlValidated)
                {
                    var savedPath = await DownloadAndSaveImageAsync(currentPath);
                    if (!string.IsNullOrEmpty(savedPath))
                    {
                        txtImagePath.Text = savedPath;
                    }
                }
                // If it's a temp file, move it to the uploads folder
                else if (!string.IsNullOrEmpty(_tempImagePath) && File.Exists(_tempImagePath))
                {
                    var savedPath = CopyImageToUploads(_tempImagePath);
                    if (!string.IsNullOrEmpty(savedPath))
                    {
                        txtImagePath.Text = savedPath;
                    }
                }
                // If it's a local file path (not in uploads), copy it there
                else if (File.Exists(currentPath) && !IsInUploadsFolder(currentPath))
                {
                    // Check if it's the same as original, if so skip
                    if (currentPath != _originalImagePath)
                    {
                        var savedPath = CopyImageToUploads(currentPath);
                        if (!string.IsNullOrEmpty(savedPath))
                        {
                            txtImagePath.Text = savedPath;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error processing image before save");
                throw new Exception($"Lỗi khi xử lý hình ảnh: {ex.Message}");
            }
        }

        private string CopyImageToUploads(string sourcePath)
        {
            try
            {
                var uploadsDir = GetUploadsDirectory();
                Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(sourcePath)}";
                var destinationPath = Path.Combine(uploadsDir, fileName);

                File.Copy(sourcePath, destinationPath, true);

                return Path.Combine("Resources", "Uploads", fileName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error copying image to uploads folder");
                return string.Empty;
            }
        }

        private async Task<string> DownloadAndSaveImageAsync(string imageUrl)
        {
            try
            {
                using var response = await _httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
                var extension = GetExtensionFromContentType(contentType);

                if (string.IsNullOrEmpty(extension))
                {
                    extension = Path.GetExtension(imageUrl) ?? ".jpg";
                }

                var uploadsDir = GetUploadsDirectory();
                Directory.CreateDirectory(uploadsDir);

                var fileName = $"{Guid.NewGuid()}{extension}";
                var filePath = Path.Combine(uploadsDir, fileName);

                using var fileStream = File.Create(filePath);
                await response.Content.CopyToAsync(fileStream);

                return Path.Combine("Resources", "Uploads", fileName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error downloading image from URL");
                throw new Exception($"Không thể tải ảnh từ URL: {ex.Message}");
            }
        }

        private string GetExtensionFromContentType(string contentType)
        {
            return contentType.ToLower() switch
            {
                "image/jpeg" => ".jpg",
                "image/jpg" => ".jpg",
                "image/png" => ".png",
                "image/gif" => ".gif",
                "image/bmp" => ".bmp",
                "image/webp" => ".webp",
                _ => ".jpg"
            };
        }

        private bool IsInUploadsFolder(string path)
        {
            var uploadsDir = GetUploadsDirectory();
            return path.StartsWith(uploadsDir, StringComparison.OrdinalIgnoreCase);
        }

        private string GetUploadsDirectory()
        {
            var projectDir = Path.GetDirectoryName(Application.ExecutablePath);
            if (string.IsNullOrEmpty(projectDir))
                throw new InvalidOperationException("Could not determine the application directory.");
            return Path.Combine(projectDir, "Resources", "Uploads");
        }

        private void CleanupTempFiles()
        {
            try
            {
                if (!string.IsNullOrEmpty(_tempImagePath) && File.Exists(_tempImagePath))
                {
                    File.Delete(_tempImagePath);
                    _tempImagePath = null;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error cleaning up temp files");
            }
        }

        private async Task BtnCheckUrl_ClickAsync(object sender, EventArgs e)
        {
            var url = txtImagePath.Text.Trim();

            if (string.IsNullOrEmpty(url))
            {
                var toastWarning = new FrmToastMessage(ToastType.WARNING, "Vui lòng nhập URL hình ảnh");
                toastWarning.Show();
                return;
            }

            if (!IsValidImageUrl(url))
            {
                var toastError = new FrmToastMessage(ToastType.ERROR, "URL không hợp lệ hoặc không phải hình ảnh");
                toastError.Show();
                _isUrlValidated = false;
                return;
            }

            try
            {
                btnCheckUrl.Enabled = false;
                btnCheckUrl.Text = "Đang kiểm tra...";

                using var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
                if (!IsImageContentType(contentType))
                {
                    var toastInvalid = new FrmToastMessage(ToastType.ERROR, "URL không trỏ đến một hình ảnh hợp lệ");
                    toastInvalid.Show();
                    _isUrlValidated = false;
                    return;
                }

                // Load image to preview
                LoadThumbnailImage();

                _isUrlValidated = true;
                _isUrlChanged = false;
                var toastValid = new FrmToastMessage(ToastType.WARNING, "URL hình ảnh hợp lệ!");
                toastValid.Show();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error validating image URL");
                var toastCatch = new FrmToastMessage(ToastType.ERROR, "URL không trỏ đến một hình ảnh hợp lệ");
                toastCatch.Show();
                _isUrlValidated = false;
            }
            finally
            {
                btnCheckUrl.Enabled = true;
                btnCheckUrl.Text = "Kiểm tra URL";
            }
        }

        private bool IsValidImageUrl(string url)
        {
            if (string.IsNullOrEmpty(url)) return false;

            // Check if it's a valid URL
            if (!Uri.TryCreate(url, UriKind.Absolute, out var uri))
                return false;

            // Check if it's HTTP or HTTPS
            if (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps)
                return false;

            // Check file extension
            var extension = Path.GetExtension(uri.LocalPath).ToLower();
            var validExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".webp" };

            return validExtensions.Contains(extension);
        }

        private bool IsImageContentType(string contentType)
        {
            var imageTypes = new[] { "image/jpeg", "image/jpg", "image/png", "image/gif", "image/bmp", "image/webp" };
            return imageTypes.Contains(contentType.ToLower());
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            CleanupTempFiles();
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            CleanupTempFiles();
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void BtnAddImage_Click(object sender, EventArgs e)
        {
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
                    if (selectedImage != null && _product.ProductImages != null)
                    {
                        _product.ProductImages.Remove(selectedImage);
                        dgvProductImages.Refresh();
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

                // TODO: Replace with proper FrmRecipeDetails initialization
                using var recipeForm = new FrmRecipeDetails(null, ConvertToRecipeDetailViewModel(newRecipe));
                var result = recipeForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    var updatedRecipe = ConvertToRecipeViewModel(recipeForm.Recipe);
                    _product.Recipes.Add(updatedRecipe);
                    dgvRecipes.Refresh();
                    MessageBox.Show("Thêm công thức thành công!", "Thông báo",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                MessageBox.Show("Chức năng thêm công thức sẽ được triển khai", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                MessageBox.Show("Chức năng chỉnh sửa công thức sẽ được triển khai", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    dgvRecipes.Refresh();
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
                MessageBox.Show("Chức năng xem chi tiết công thức sẽ được triển khai", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công thức để xem chi tiết",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TxtImagePath_TextChanged(object sender, EventArgs e)
        {
            _isUrlChanged = true;
            _isUrlValidated = false;
            LoadThumbnailImage();
        }

        private void BtnUpload_Click(object sender, EventArgs e)
        {
            using OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Title = "Chọn hình ảnh",
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures)
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Create temp copy
                    var tempDir = Path.GetTempPath();
                    var tempFileName = $"temp_image_{Guid.NewGuid()}{Path.GetExtension(openFileDialog.FileName)}";
                    _tempImagePath = Path.Combine(tempDir, tempFileName);

                    File.Copy(openFileDialog.FileName, _tempImagePath);
                    txtImagePath.Text = _tempImagePath;

                    _isUrlChanged = false;
                    _isUrlValidated = true; // Local file is considered validated
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi tải file: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnRemove_Click(object sender, EventArgs e)
        {
            txtImagePath.Text = "";
            CleanupTempFiles();
            _isUrlChanged = false;
            _isUrlValidated = true;
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

            if (_isUrlChanged && !_isUrlValidated)
            {
                var imagePath = txtImagePath.Text.Trim();
                if (!string.IsNullOrEmpty(imagePath) && IsValidImageUrl(imagePath))
                {
                    MessageBox.Show("Vui lòng kiểm tra URL hình ảnh bằng cách nhấn 'Kiểm tra URL'", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnCheckUrl.Focus();
                    return false;
                }
            }

            return true;
        }

        private ProductDetailViewModel BuildProductFromForm()
        {
            var vm = _product ?? new ProductDetailViewModel();

            vm.Name = txtName.Text.Trim();
            vm.Description = txtDescription.Text.Trim();
            vm.Price = numPrice.Value;
            vm.IsActive = chkIsActive.Checked;
            vm.ThumbnailPath = txtImagePath.Text.Trim();

            // Handle Category selection - Fixed selection logic
            if (cbxCategory.SelectedValue != null)
            {
                try
                {
                    vm.CategoryId = Convert.ToInt64(cbxCategory.SelectedValue);
                    vm.CategoryName = cbxCategory.Text;
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Error getting selected category");
                }
            }

            // Handle Tax selection - Fixed selection logic
            if (cbxTax.SelectedValue != null)
            {
                try
                {
                    vm.TaxId = Convert.ToInt64(cbxTax.SelectedValue);
                    vm.TaxName = cbxTax.Text;
                }
                catch (Exception ex)
                {
                    _logger?.LogWarning(ex, "Error getting selected tax");
                }
            }

            // Ensure collections are properly initialized
            vm.ProductImages ??= [];
            vm.Recipes ??= [];
            vm.ProductRecipes ??= [];

            return vm;
        }

        private void SetComboBoxSelection(ComboBox comboBox, long id)
        {
            try
            {
                if (id <= 0)
                {
                    comboBox.SelectedIndex = -1;
                    return;
                }

                // Use SelectedValue for proper binding
                comboBox.SelectedValue = id;

                // Fallback if SelectedValue doesn't work
                if (comboBox.SelectedIndex == -1)
                {
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        var item = comboBox.Items[i];
                        if (item == null) continue;

                        var idProperty = item.GetType().GetProperty("Id");
                        if (idProperty != null)
                        {
                            var itemId = idProperty.GetValue(item);
                            if (itemId != null && Convert.ToInt64(itemId) == id)
                            {
                                comboBox.SelectedIndex = i;
                                return;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error setting ComboBox selection for id {Id}", id);
                comboBox.SelectedIndex = -1;
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

        private void LoadThumbnailImage()
        {
            var imagePath = txtImagePath.Text.Trim();

            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    try { picThumbnail.Image = Properties.Resources.logoIcon; } catch { }
                    return;
                }

                // Handle URL
                if (IsValidImageUrl(imagePath))
                {
                    picThumbnail.LoadAsync(imagePath);
                    return;
                }

                // Handle local file path
                if (File.Exists(imagePath))
                {
                    using var fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
                    picThumbnail.Image = Image.FromStream(fs);
                    return;
                }

                // Handle relative path
                var projectDir = Path.GetDirectoryName(Application.ExecutablePath);
                if (string.IsNullOrEmpty(projectDir))
                    throw new InvalidOperationException("Could not determine the application directory.");
                var fullPath = Path.Combine(projectDir, imagePath);
                if (File.Exists(fullPath))
                {
                    using var fs = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
                    picThumbnail.Image = Image.FromStream(fs);
                    return;
                }

                // Fallback to default image
                try { picThumbnail.Image = Properties.Resources.logoIcon; } catch { }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error loading thumbnail image");
                try { picThumbnail.Image = Properties.Resources.logoIcon; } catch { }
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            btnSave.Enabled = !isLoading;
            btnCancel.Enabled = !isLoading;
            btnClose.Enabled = !isLoading;

            // Disable/enable other controls as needed
            txtName.Enabled = !isLoading;
            txtDescription.Enabled = !isLoading;
            numPrice.Enabled = !isLoading;
            chkIsActive.Enabled = !isLoading;
            cbxCategory.Enabled = !isLoading;
            cbxTax.Enabled = !isLoading;
            btnUpload.Enabled = !isLoading;
            btnRemove.Enabled = !isLoading;
            btnCheckUrl.Enabled = !isLoading;

            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void SafeInvoke(Action action)
        {
            if (InvokeRequired)
            {
                Invoke(action);
            }
            else
            {
                action();
            }
        }

        private RecipeDetailViewModel ConvertToRecipeDetailViewModel(RecipeViewModel r)
        {
            return new RecipeDetailViewModel
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description,
                ProductId = r.ProductId,
                ProductName = r.ProductName,
                ServingSize = r.ServingSize,
                Unit = r.Unit,
                IsActive = r.IsActive,
                Notes = r.Notes,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt
            };
        }

        private RecipeViewModel ConvertToRecipeViewModel(RecipeDetailViewModel rd)
        {
            return new RecipeViewModel
            {
                Id = rd.Id,
                Name = rd.Name,
                Description = rd.Description,
                ProductId = rd.ProductId,
                ProductName = rd.ProductName,
                ServingSize = rd.ServingSize,
                Unit = rd.Unit,
                IsActive = rd.IsActive,
                Notes = rd.Notes,
                CreatedAt = rd.CreatedAt,
                UpdatedAt = rd.UpdatedAt
            };
        }
        #endregion
    }
}