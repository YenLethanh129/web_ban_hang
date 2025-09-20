using Dashboard.Common.Constants;
using Dashboard.Common.Utitlities;
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
        private readonly IImageUrlValidator _imageUrlValidator;

        // Image validation management
        private readonly ImageValidationViewModel _imageValidation;
        private string? _tempImagePath = null;
        private static readonly HttpClient _httpClient = new HttpClient();
        private bool _imagesModified = false;

        #endregion

        #region Properties
        public DialogResult Result { get; private set; }
        public ProductDetailViewModel Product => _product;
        #endregion

        #region Constructor
        public FrmProductDetails(
            IProductDetailPresenter presenter,
            IImageUrlValidator imageUrlValidator,
            ILogger<FrmProductDetails>? logger = null,
            long? productId = null,
            ProductDetailViewModel? product = null)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _imageUrlValidator = imageUrlValidator ?? throw new ArgumentNullException(nameof(imageUrlValidator));
            _logger = logger;
            _productId = productId;
            _product = product ?? new ProductDetailViewModel();
            _isEditMode = productId.HasValue;

            // Initialize image validation
            _imageValidation = new ImageValidationViewModel();

            InitializeComponent();
            InitializeFormSettings();
            SetupEventHandlers();
            SetupDataGridViews();
            SetupImageValidation();

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
                CleanupTempFiles();

                // Cleanup image validation events
                if (_imageValidation != null)
                {
                    _imageValidation.PropertyChanged -= ImageValidation_PropertyChanged;
                }

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

        private void SetupImageValidation()
        {
            txtImagePath.DataBindings.Clear();
            txtImagePath.DataBindings.Add("Text", _imageValidation, nameof(_imageValidation.ImageUrl), false, DataSourceUpdateMode.OnPropertyChanged);

            btnCheckUrl.DataBindings.Clear();
            btnCheckUrl.DataBindings.Add("Enabled", _imageValidation, nameof(_imageValidation.CanValidate), false, DataSourceUpdateMode.Never);

            _imageValidation.PropertyChanged += ImageValidation_PropertyChanged;

            if (_isEditMode && !string.IsNullOrEmpty(_product.ThumbnailPath))
            {
                _imageValidation.ImageUrl = _product.ThumbnailPath;
                _imageValidation.OriginalImageUrl = _product.ThumbnailPath;
                _imageValidation.SetValidationResult(true, "Ảnh hiện tại hợp lệ");
            }
            else
            {
                _imageValidation.SetValidationResult(true, "");
            }
        }

        private void ImageValidation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(_imageValidation.IsValid) ||
                e.PropertyName == nameof(_imageValidation.RequiresValidation) ||
                e.PropertyName == nameof(_imageValidation.IsValidating) ||
                e.PropertyName == nameof(_imageValidation.CanSave))
            {
                UpdateValidationUI();
                UpdateSaveButtonState();
            }

            if  (e.PropertyName == nameof(_imageValidation.ImageUrl))
            {
                // When the image textbox changes, require validation if the value differs from original
                try
                {
                    var newUrl = _imageValidation.ImageUrl?.Trim() ?? string.Empty;
                    var origUrl = _imageValidation.OriginalImageUrl?.Trim() ?? string.Empty;

                    if (!string.Equals(newUrl, origUrl, StringComparison.OrdinalIgnoreCase))
                    {
                        _imageValidation.ResetValidation();
                        _imagesModified = true;
                    }
                    else
                    {
                        _imageValidation.SetValidationResult(true, "Ảnh hiện tại hợp lệ");
                        _imagesModified = false;
                    }
                }
                catch
                {
                }

            }
        }

        private void UpdateValidationUI()
        {
            SafeInvokeOnUI(() =>
            {
                if (_imageValidation.IsValidating)
                {
                    lblValidationStatus.ForeColor = Color.Blue;
                    lblValidationStatus.Text = "Đang kiểm tra...";
                    btnCheckUrl.Text = "Đang kiểm tra...";
                }
                else if (_imageValidation.RequiresValidation)
                {
                    lblValidationStatus.ForeColor = Color.Orange;
                    lblValidationStatus.Text = "Cần kiểm tra URL trước khi lưu";
                    btnCheckUrl.Text = "Kiểm tra URL";
                }
                else if (_imageValidation.IsValidated)
                {
                    if (_imageValidation.IsValid)
                    {
                        lblValidationStatus.ForeColor = Color.Green;
                        lblValidationStatus.Text = _imageValidation.ValidationMessage;
                        btnCheckUrl.Text = "✓ Hợp lệ";
                    }
                    else
                    {
                        lblValidationStatus.ForeColor = Color.Red;
                        lblValidationStatus.Text = _imageValidation.ValidationMessage;
                        btnCheckUrl.Text = "Kiểm tra lại";
                    }
                }
                else
                {
                    lblValidationStatus.ForeColor = Color.Gray;
                    lblValidationStatus.Text = "";
                    btnCheckUrl.Text = "Kiểm tra URL";
                }
            });
        }

        private void UpdateSaveButtonState()
        {
            SafeInvokeOnUI(() =>
            {
                var imageOkToSave = !_imagesModified 
                    || (_imageValidation.IsValidated && _imageValidation.IsValid 
                    && !_imageValidation.IsValidating && !_imageValidation.RequiresValidation);
                btnSave.Enabled = imageOkToSave && !_isProcessingSave;
            });
        }

        private bool _isProcessingSave = false;

        private void UpdateFormMode()
        {
            if (_isEditMode)
            {
                Text = $"Chi tiết sản phẩm - {_product?.Name ?? "N/A"}";
                btnClose.Visible = true;
                btnSave.Text = "Lưu";
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
            this.Load += (s, e) => FrmProductDetails_Load(s!, e);

            btnSave.Click += (s, e) => BtnSave_Click(s!, e);
            btnCancel.Click += (s, e) => BtnCancel_Click(s!, e);
            btnClose.Click += (s, e) => BtnClose_Click(s!, e);

            btnAddImage.Click += (s, e) => BtnAddImage_Click(s!, e);
            btnDeleteImage.Click += (s, e) => BtnDeleteImage_Click(s!, e);

            btnCheckUrl.Click += async (s, e) => await BtnCheckUrl_ClickAsync(s!, e);

            btnAddRecipe.Click += (s, e) => BtnAddRecipe_Click(s!, e);
            btnEditRecipe.Click += (s, e) => BtnEditRecipe_Click(s!, e);
            btnDeleteRecipe.Click += (s, e) => BtnDeleteRecipe_Click(s!, e);
            btnViewRecipeDetails.Click += (s, e) => BtnViewRecipeDetails_Click(s!, e);

            btnUpload.Click += (s, e) => BtnUpload_Click(s!, e);
            btnRemove.Click += (s, e) => BtnRemove_Click(s!, e);

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

                await PopulateFormWithData();
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
            cbxCategory.Items.Clear();
            cbxCategory.Items.Add(new CategoryViewModel { Id = 1L, Name = "Đồ uống" });
            cbxCategory.Items.Add(new CategoryViewModel { Id = 2L, Name = "Đồ ăn" });
            cbxCategory.Items.Add(new CategoryViewModel { Id = 3L, Name = "Bánh kẹo" });
            cbxCategory.DisplayMember = "Name";
            cbxCategory.ValueMember = "Id";

            cbxTax.Items.Clear();
            cbxTax.Items.Add(new TaxViewModel { Id = 1L, Name = "VAT 10%", Rate = 10 });
            cbxTax.Items.Add(new TaxViewModel { Id = 2L, Name = "VAT 8%", Rate = 8 });
            cbxTax.Items.Add(new TaxViewModel { Id = 3L, Name = "VAT 5%", Rate = 5 });
            cbxTax.DisplayMember = "Name";
            cbxTax.ValueMember = "Id";
        }

        private void Presenter_OnProductSaved(object? sender, ProductDetailViewModel? vm)
        {
            // Use safe invoke to avoid calling BeginInvoke before handle created
            SafeInvokeOnUI(async () =>
            {
                try
                {
                    if (vm != null)
                    {
                        _product = vm;
                        await PopulateFormWithData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
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

        private async Task PopulateFormWithData()
        {
            if (_product == null) return;

            try
            {
                txtProductId.Text = _product.Id.ToString();
                txtName.Text = _product.Name ?? string.Empty;
                txtDescription.Text = _product.Description ?? string.Empty;
                numPrice.Value = _product.Price;
                chkIsActive.Checked = _product.IsActive;

                // Setup image validation with original image path
                var originalImagePath = _product.ThumbnailPath ?? string.Empty;
                _imageValidation.OriginalImageUrl = originalImagePath;
                _imageValidation.ImageUrl = originalImagePath;

                // For existing images, mark as validated
                if (!string.IsNullOrEmpty(originalImagePath))
                {
                    _imageValidation.SetValidationResult(true, "Ảnh hiện tại hợp lệ");
                }

                SetComboBoxSelection(cbxCategory, _product.CategoryId ?? 0);
                SetComboBoxSelection(cbxTax, _product.TaxId ?? 0);

                lblCreatedAt.Text = _product.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                lblUpdatedAt.Text = _product.UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";

                dgvProductImages.DataSource = _product.ProductImages;
                dgvRecipes.DataSource = _product.Recipes;

                await LoadThumbnailImageAsync();

                if (!_isEditMode)
                {
                    chkIsActive.Checked = true;
                    numPrice.Value = 0;
                }
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
            if (!_imageValidation.CanSave)
            {
                MessageBox.Show("Vui lòng kiểm tra URL hình ảnh trước khi lưu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                btnCheckUrl.Focus();
                return;
            }

            if (!ValidateFormAsync()) return;

            if (_isProcessingSave) return;
            _isProcessingSave = true;

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
                    _imageValidation.SaveChanges(); // Mark changes as saved
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
                _isProcessingSave = false;
            }
        }

        private async Task ProcessImageBeforeSaveAsync()
        {
            try
            {
                var currentThumb = _imageValidation.ImageUrl?.Trim();
                if (!string.IsNullOrEmpty(currentThumb) && _imageValidation.HasChanges)
                {
                    var savedThumbPath = await SaveImageToUploadsAsync(currentThumb);
                    if (!string.IsNullOrEmpty(savedThumbPath))
                    {
                        _product.ThumbnailPath = savedThumbPath;
                        _imageValidation.OriginalImageUrl = savedThumbPath;
                        _imageValidation.ImageUrl = savedThumbPath;
                    }
                    else
                    {
                        throw new Exception("Không thể lưu ảnh đại diện. Vui lòng kiểm tra lại đường dẫn/ kết nối mạng.");
                    }
                }

                if (_imagesModified && _product.ProductImages != null)
                {
                    var list = _product.ProductImages;
                    for (int i = 0; i < list.Count; i++)
                    {
                        var imgVm = list[i];
                        var imgPath = imgVm.ImageUrl?.Trim();

                        if (string.IsNullOrEmpty(imgPath))
                            continue;

                        if (IsInUploadsFolder(imgPath))
                            continue;

                        var saved = await SaveImageToUploadsAsync(imgPath);
                        if (!string.IsNullOrEmpty(saved))
                        {
                            imgVm.ImageUrl = saved;
                        }
                        else
                        {
                            throw new Exception($"Không thể lưu ảnh: {imgPath}");
                        }
                    }
                }

                _imagesModified = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi xử lý hình ảnh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private async Task<string> SaveImageToUploadsAsync(string imagePathOrUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePathOrUrl))
                    return string.Empty;

                if (IsInUploadsFolder(imagePathOrUrl))
                {
                    var uploadsDir = GetUploadsDirectory();
                    if (imagePathOrUrl.StartsWith(uploadsDir, StringComparison.OrdinalIgnoreCase))
                    {
                        var relative = imagePathOrUrl.Substring(uploadsDir.Length).TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                        return Path.Combine("Resources", "Uploads", relative).Replace(Path.DirectorySeparatorChar, '/');
                    }
                    return imagePathOrUrl;
                }

                if (Uri.TryCreate(imagePathOrUrl, UriKind.Absolute, out var uri) &&
                    (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps))
                {
                    var saved = await DownloadAndSaveImageAsync(imagePathOrUrl);
                    return saved ?? string.Empty;
                }

                if (File.Exists(imagePathOrUrl))
                {
                    var saved = CopyImageToUploads(imagePathOrUrl);
                    return saved ?? string.Empty;
                }

                var projectDir = Path.GetDirectoryName(Application.ExecutablePath);
                if (!string.IsNullOrEmpty(projectDir))
                {
                    var full = Path.Combine(projectDir, imagePathOrUrl);
                    if (File.Exists(full))
                    {
                        var saved = CopyImageToUploads(full);
                        return saved ?? string.Empty;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error saving image to uploads: {Image}", imagePathOrUrl);
                return string.Empty;
            }
        }
        private async Task BtnCheckUrl_ClickAsync(object sender, EventArgs e)
        {
            var url = _imageValidation.ImageUrl?.Trim();

            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Vui lòng nhập URL hoặc đường dẫn hình ảnh", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                _imageValidation.StartValidation();

                if (!await _imageUrlValidator.ValidateAsync(url))
                {
                    _imageValidation.SetValidationResult(false, "Không phải đường dẫn hình ảnh hợp lệ");
                    return;
                }

                await LoadThumbnailImageAsync();
                _imageValidation.SetValidationResult(true, "Đường dẫn hình ảnh hợp lệ!");

                if (!string.Equals(_imageValidation.ImageUrl, _imageValidation.OriginalImageUrl, StringComparison.OrdinalIgnoreCase))
                {
                    _imagesModified = true;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error validating image URL");
                _imageValidation.SetValidationResult(false, "Không thể xác thực hình ảnh");
            }
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
                    var tempDir = Path.GetTempPath();
                    var tempFileName = $"temp_image_{Guid.NewGuid()}{Path.GetExtension(openFileDialog.FileName)}";
                    _tempImagePath = Path.Combine(tempDir, tempFileName);

                    File.Copy(openFileDialog.FileName, _tempImagePath);
                    _imageValidation.ImageUrl = _tempImagePath;

                    _imageValidation.SetValidationResult(true, "File hình ảnh hợp lệ");

                    _imagesModified = true;
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
            _imageValidation.ImageUrl = string.Empty;
            CleanupTempFiles();
            _imageValidation.SetValidationResult(true, "");
            _imagesModified = true;
        }


        // Other event handlers remain the same...
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

                        // mark modified
                        _imagesModified = true;
                    }
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hình ảnh để xóa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Recipe management methods remain the same...
        private void BtnAddRecipe_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Chức năng thêm công thức sẽ được triển khai", "Thông báo",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                    _product.Recipes!.Remove(selectedRecipe);
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
        private bool ValidateFormAsync()
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

            return true;
        }

        private ProductDetailViewModel BuildProductFromForm()
        {
            var vm = _product ?? new ProductDetailViewModel();

            vm.Name = txtName.Text.Trim();
            vm.Description = txtDescription.Text.Trim();
            vm.Price = numPrice.Value;
            vm.IsActive = chkIsActive.Checked;

            vm.ThumbnailPath = _imageValidation.ImageUrl?.Trim();

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

            // Handle Tax selection
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

            vm.ProductImages ??= [];
            vm.Recipes ??= [];
            vm.ProductRecipes ??= [];

            vm.ImagesModified = _imagesModified;

            return vm;
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
                string uploadsDir = GetUploadsDirectory();
                Directory.CreateDirectory(uploadsDir);

                // Case 1: Local file
                if (File.Exists(imageUrl))
                {
                    var ext = Path.GetExtension(imageUrl);
                    if (string.IsNullOrEmpty(ext))
                        ext = ".jpg";

                    var fileName = $"{Guid.NewGuid()}{ext}";
                    var filePath = Path.Combine(uploadsDir, fileName);

                    File.Copy(imageUrl, filePath, overwrite: true);
                    return Path.Combine("Resources", "Uploads", fileName);
                }

                // Case 2: Remote URL
                if (!Uri.TryCreate(imageUrl, UriKind.Absolute, out var uri) ||
                    (uri.Scheme != Uri.UriSchemeHttp && uri.Scheme != Uri.UriSchemeHttps))
                {
                    throw new ArgumentException("Đường dẫn không hợp lệ hoặc không hỗ trợ.", nameof(imageUrl));
                }

                using var response = await _httpClient.GetAsync(uri, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType?.MediaType ?? "";
                var extension = GetExtensionFromContentType(contentType);
                if (string.IsNullOrEmpty(extension))
                {
                    extension = Path.GetExtension(uri.AbsolutePath);
                    if (string.IsNullOrEmpty(extension))
                        extension = ".jpg";
                }

                var newFileName = $"{Guid.NewGuid()}{extension}";
                var newFilePath = Path.Combine(uploadsDir, newFileName);

                using (var fs = File.Create(newFilePath))
                {
                    await response.Content.CopyToAsync(fs);
                }

                return Path.Combine("Resources", "Uploads", newFileName);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error downloading or copying image from: {ImageUrl}", imageUrl);
                throw new Exception($"Không thể tải hoặc sao chép ảnh: {ex.Message}");
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
            return path.StartsWith(uploadsDir, StringComparison.OrdinalIgnoreCase) ||
                   path.StartsWith(Path.Combine("Resources", "Uploads"), StringComparison.OrdinalIgnoreCase);
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

        private void SetComboBoxSelection(ComboBox comboBox, long id)
        {
            try
            {
                if (id <= 0)
                {
                    comboBox.SelectedIndex = -1;
                    return;
                }

                comboBox.SelectedValue = id;

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

        private async Task LoadThumbnailImageAsync()
        {
            var imagePath = _imageValidation.ImageUrl?.Trim();

            try
            {
                if (string.IsNullOrEmpty(imagePath))
                {
                    try { picThumbnail.Image = Properties.Resources.logoIcon; } catch { }
                    return;
                }

                // Handle URL
                if (await _imageUrlValidator.ValidateAsync(imagePath))
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
            btnSave.Enabled = !isLoading && _imageValidation.CanSave;
            btnCancel.Enabled = !isLoading;
            btnClose.Enabled = !isLoading;

            txtName.Enabled = !isLoading;
            txtDescription.Enabled = !isLoading;
            numPrice.Enabled = !isLoading;
            chkIsActive.Enabled = !isLoading;
            cbxCategory.Enabled = !isLoading;
            cbxTax.Enabled = !isLoading;
            btnUpload.Enabled = !isLoading;
            btnRemove.Enabled = !isLoading;
            btnCheckUrl.Enabled = !isLoading && _imageValidation.CanValidate;

            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }
        #endregion

        #region Thread-safe UI helpers

        /// <summary>
        /// Safely invokes an Action on the UI thread. If the control handle is not yet created,
        /// the action will be scheduled to run when HandleCreated fires.
        /// If the form is disposed or disposing, the action is ignored.
        /// </summary>
        private void SafeInvokeOnUI(Action action)
        {
            if (action == null) return;
            if (IsDisposed || Disposing) return;

            try
            {
                if (IsHandleCreated)
                {
                    if (InvokeRequired)
                        BeginInvoke(action);
                    else
                        action();
                }
                else
                {
                    void Handler(object? s, EventArgs e)
                    {
                        HandleCreated -= Handler;
                        try
                        {
                            if (!IsDisposed && IsHandleCreated)
                                BeginInvoke(action);
                        }
                        catch { /* swallow */ }
                    }
                    HandleCreated += Handler;
                }
            }
            catch
            {
                // ignore any exceptions from invoking
            }
        }

        /// <summary>
        /// Safely invokes an async Func on the UI thread.
        /// </summary>
        private void SafeInvokeOnUI(Func<Task> asyncAction)
        {
            if (asyncAction == null) return;
            SafeInvokeOnUI(() => { _ = asyncAction(); });
        }

        #endregion
    }
}