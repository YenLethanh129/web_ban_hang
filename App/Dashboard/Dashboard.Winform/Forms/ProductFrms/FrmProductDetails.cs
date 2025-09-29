using Dashboard.BussinessLogic.Services.FileServices;
using Dashboard.Common.Constants;
using Dashboard.Common.Utitlities;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Forms.BaseFrm;
using Dashboard.Winform.Helpers;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.Presenters.ProductPresenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms
{
    public partial class FrmProductDetails : FrmBaseAuthForm
    {
        #region Fields
        private bool _isEditMode;
        private long? _productId;
        private ProductDetailViewModel _product;
        private readonly IProductDetailPresenter _presenter;
        private bool _isDataLoaded = false;
        private readonly ILogger<FrmProductDetails>? _logger;
        private readonly IImageUrlValidator _imageUrlValidator;

        private readonly ImageValidationViewModel _imageValidation;
        private string? _tempImagePath = null;
        private static readonly HttpClient _httpClient = new HttpClient();
        private bool _imagesModified = false;

        private readonly List<long> _deletedImageIds = new();

        private List<RecipeViewModel> _allAvailableRecipes = new();
        private List<RecipeViewModel> _assignedRecipes = new();
        private bool _recipesModified = false;

        #endregion

        #region Properties
        public DialogResult Result { get; private set; }
        public ProductDetailViewModel Product => _product;

        public readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public FrmProductDetails(
            IProductDetailPresenter presenter,
            IImageUrlValidator imageUrlValidator,
            IServiceProvider serviceProvider,
            ILogger<FrmProductDetails> logger,
            long? productId = null,
            ProductDetailViewModel? product = null)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
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

                CleanupTempFiles();

                if (_imageValidation != null)
                {
                    _imageValidation.PropertyChanged -= ImageValidation_PropertyChanged;
                }

                txtImagePath.TextChanged -= TxtImagePath_TextChanged;

                if (_presenter != null)
                {
                    _presenter.OnProductSaved -= Presenter_OnProductSaved;
                }

                components?.Dispose();
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
            try
            {
                txtImagePath.DataBindings.Clear();
                btnCheckUrl.DataBindings.Clear();

                _imageValidation.PropertyChanged -= ImageValidation_PropertyChanged;

                _imageValidation.PropertyChanged += ImageValidation_PropertyChanged;

                if (_isEditMode && !string.IsNullOrEmpty(_product.ThumbnailPath))
                {
                    _imageValidation.ImageUrl = _product.ThumbnailPath;
                    _imageValidation.OriginalImageUrl = _product.ThumbnailPath;
                    _imageValidation.SetValidationResult(true, "Ảnh hiện tại hợp lệ");
                }
                else
                {
                    _imageValidation.ImageUrl = string.Empty;
                    _imageValidation.OriginalImageUrl = string.Empty;
                    _imageValidation.SetValidationResult(true, "");
                }

                txtImagePath.Text = _imageValidation.ImageUrl ?? string.Empty;

                UpdateValidationUI();
                UpdateSaveButtonState();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error setting up image validation");
            }
        }
        private void ImageValidation_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            try
            {
                if (e.PropertyName == nameof(_imageValidation.IsValid) ||
                    e.PropertyName == nameof(_imageValidation.RequiresValidation) ||
                    e.PropertyName == nameof(_imageValidation.IsValidating) ||
                    e.PropertyName == nameof(_imageValidation.CanSave))
                {
                    UpdateValidationUI();
                    UpdateSaveButtonState();
                }

                if (e.PropertyName == nameof(_imageValidation.ImageUrl))
                {
                    SafeInvokeOnUI(() =>
                    {
                        if (txtImagePath.Text != (_imageValidation.ImageUrl ?? string.Empty))
                        {
                            txtImagePath.TextChanged -= (s, o) => TxtImagePath_TextChanged(s, o);
                            txtImagePath.Text = _imageValidation.ImageUrl ?? string.Empty;
                            txtImagePath.TextChanged += (s, o) => TxtImagePath_TextChanged(s, o);
                        }
                    });

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
                    catch (Exception ex)
                    {
                        _logger?.LogWarning(ex, "Error processing image URL change");
                    }
                }

                if (e.PropertyName == nameof(_imageValidation.CanValidate))
                {
                    SafeInvokeOnUI(() =>
                    {
                        btnCheckUrl.Enabled = _imageValidation.CanValidate;
                    });
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error in ImageValidation_PropertyChanged");
            }
        }

        private void TxtImagePath_TextChanged(object? sender, EventArgs e)
        {
            try
            {
                if (_imageValidation.ImageUrl != txtImagePath.Text)
                {
                    _imageValidation.ImageUrl = txtImagePath.Text;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogWarning(ex, "Error in TxtImagePath_TextChanged");
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
            Load += (s, e) => FrmProductDetails_Load(s!, e);

            btnSave.Click += (s, e) => BtnSave_Click(s!, e);
            btnCancel.Click += (s, e) => BtnCancel_Click(s!, e);
            btnClose.Click += (s, e) => BtnClose_Click(s!, e);

            btnViewImage.Click += (s, e) => BtnViewImage_Click(s!, e);
            btnDeleteImage.Click += (s, e) => BtnDeleteImage_Click(s!, e);

            btnCheckUrl.Click += async (s, e) => await BtnCheckUrl_ClickAsync(s!, e);

            btnAssignRecipe.Click += (s, e) => BtnAssignRecipe_Click(s!, e);
            btnUnassignRecipe.Click += (s, e) => BtnUnassignRecipe_Click(s!, e);
            btnCreateNewRecipe.Click += (s, e) => BtnCreateNewRecipe_Click(s!, e);
            btnDetailRecipe.Click += (s, e) => BtnEditRecipe_Click(s!, e);

            btnUpload.Click += (s, e) => BtnUpload_Click(s!, e);
            btnRemove.Click += (s, e) => BtnRemove_Click(s!, e);

            txtImagePath.TextChanged += TxtImagePath_TextChanged;

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

        private async Task RefreshProductImagesFromDatabase()
        {
            try
            {
                if (_isEditMode && _productId.HasValue)
                {
                    var productImages = await _presenter.GetProductImagesAsync(_productId.Value);
                    _product.ProductImages = new BindingList<ProductImageViewModel>(productImages);
                    RefreshProductImagesGrid();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error refreshing product images from database");
            }
        }

        private async Task LoadInitialDataAsync()
        {
            try
            {
                SetLoadingState(true);

                await LoadDropdownDataAsync();
                await LoadRecipesDataAsync();

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
                new FrmToastMessage(ToastType.ERROR, $"Lỗi khi tải dữ liệu: {ex.Message}").Show();
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

        private async Task LoadRecipesDataAsync()
        {
            try
            {
                // Load all available recipes
                _allAvailableRecipes = await _presenter.LoadAllRecipesAsync();

                // Setup available recipes ComboBox
                cbxAvailableRecipes.DataSource = null;
                cbxAvailableRecipes.DisplayMember = "Name";
                cbxAvailableRecipes.ValueMember = "Id";
                cbxAvailableRecipes.DataSource = _allAvailableRecipes;

                // Load assigned recipes if in edit mode
                if (_isEditMode && _productId.HasValue)
                {
                    _assignedRecipes = await _presenter.LoadRecipesByProductIdAsync(_productId.Value);
                    dgvAssignedRecipes.DataSource = _assignedRecipes;
                }
                else
                {
                    _assignedRecipes = new List<RecipeViewModel>();
                    dgvAssignedRecipes.DataSource = _assignedRecipes;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error loading recipes data");
                new FrmToastMessage(ToastType.WARNING, $"Lỗi khi tải dữ liệu công thức: {ex.Message}").Show();
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
                    new FrmToastMessage(ToastType.ERROR, $"Lỗi khi cập nhật dữ liệu: {ex.Message}").Show();
                }
            });
        }

        private void SetupDataGridViews()
        {
            SetupProductImagesDataGridView();
            SetupAssignedRecipesDataGridView();
        }

        private void SetupProductImagesDataGridView()
        {
            dgvProductImages.AutoGenerateColumns = false;
            dgvProductImages.Columns.Clear();
            dgvProductImages.RowHeadersVisible = false;

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
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvProductImages.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductImageViewModel.CreatedAt),
                HeaderText = "Ngày tạo",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });
        }

        private void SetupAssignedRecipesDataGridView()
        {
            dgvAssignedRecipes.AutoGenerateColumns = false;
            dgvAssignedRecipes.Columns.Clear();
            dgvAssignedRecipes.RowHeadersVisible = false;

            dgvAssignedRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Id),
                HeaderText = "ID",
                Width = 60
            });

            dgvAssignedRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Name),
                HeaderText = "Tên công thức",
                Width = 200
            });

            dgvAssignedRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Description),
                HeaderText = "Mô tả",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvAssignedRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.ServingSize),
                HeaderText = "Khẩu phần",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N1" }
            });

            dgvAssignedRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Unit),
                HeaderText = "Đơn vị",
                Width = 80
            });

            dgvAssignedRecipes.Columns.Add(new DataGridViewCheckBoxColumn
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

                var originalImagePath = _product.ThumbnailPath ?? string.Empty;
                _imageValidation.OriginalImageUrl = originalImagePath;
                _imageValidation.ImageUrl = originalImagePath;

                if (!string.IsNullOrEmpty(originalImagePath))
                {
                    _imageValidation.SetValidationResult(true, "Ảnh hiện tại hợp lệ");
                }

                SetComboBoxSelection(cbxCategory, _product.CategoryId ?? 0);
                SetComboBoxSelection(cbxTax, _product.TaxId ?? 0);

                lblCreatedAt.Text = _product.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                lblUpdatedAt.Text = _product.UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";

                _product.ProductImages ??= [];
                dgvProductImages.DataSource = new BindingList<ProductImageViewModel>(_product.ProductImages);

                if (_isEditMode && _productId.HasValue)
                {
                    await LoadRecipesDataAsync();
                }

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
                new FrmToastMessage(ToastType.WARNING, "Vui lòng kiểm tra URL hình ảnh trước khi lưu.").Show();
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
                    if (_recipesModified)
                    {
                        await SaveRecipeAssignments();
                    }
                }
                else
                {
                    result = await _presenter.CreateProductAsync(_product);
                    if (result != null && _assignedRecipes.Count != 0)
                    {
                        foreach (var recipe in _assignedRecipes)
                        {
                            await _presenter.AssignRecipeToProductAsync(result.Id, recipe.Id);
                        }
                    }
                }

                if (result != null)
                {
                    _product = result;
                    _imageValidation.SaveChanges();
                    _recipesModified = false;
                    _imagesModified = false;

                    _deletedImageIds.Clear();

                    CleanupTempFiles();
                    Result = DialogResult.OK;
                    Close();
                }
                else
                {
                    new FrmToastMessage(ToastType.ERROR, "Lưu sản phẩm thất bại.").Show();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error saving product");
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
                _isProcessingSave = false;
            }
        }

        private async Task SaveRecipeAssignments()
        {
            try
            {
                if (!_productId.HasValue) return;

                List<RecipeViewModel> currentAssigned;
                using (var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30)))
                {
                    try
                    {
                        currentAssigned = await Task.Run(async () =>
                            await _presenter.LoadRecipesByProductIdAsync(_productId.Value), cts.Token);
                    }
                    catch (OperationCanceledException)
                    {
                        throw new TimeoutException("Timeout loading current recipe assignments");
                    }
                }
                var toUnassign = currentAssigned
                    .Where(cr => !_assignedRecipes.Any(ar => ar.Id == cr.Id))
                    .ToList();
                foreach (var recipe in toUnassign)
                {
                    try
                    {
                        await _presenter.UnassignRecipeFromProductAsync(_productId.Value, recipe.Id);
                        await Task.Delay(50);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error unassigning recipe {RecipeId} from product {ProductId}",
                            recipe.Id, _productId.Value);
                    }
                }
                var toAssign = _assignedRecipes
                    .Where(ar => !currentAssigned.Any(cr => cr.Id == ar.Id))
                    .ToList();
                foreach (var recipe in toAssign)
                {
                    try
                    {
                        await _presenter.AssignRecipeToProductAsync(_productId.Value, recipe.Id);
                        await Task.Delay(50);
                    }
                    catch (Exception ex)
                    {
                        _logger?.LogError(ex, "Error assigning recipe {RecipeId} to product {ProductId}",
                            recipe.Id, _productId.Value);
                    }
                }

                _logger?.LogInformation("Recipe assignments saved: {UnassignedCount} unassigned, {AssignedCount} assigned",
                    toUnassign.Count, toAssign.Count);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error saving recipe assignments for product {ProductId}", _productId);
                throw new Exception($"Lỗi khi lưu gán công thức cho sản phẩm: {ex.Message}", ex);
            }
        }

        private async Task ProcessImageBeforeSaveAsync()
        {
            try
            {
                var currentThumb = _imageValidation.ImageUrl?.Trim();
                if (!string.IsNullOrEmpty(currentThumb) && _imageValidation.HasChanges)
                {
                    if (!IsAlreadyUploaded(currentThumb))
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
                            throw new Exception("Không thể lưu ảnh đại diện. Vui lòng kiểm tra lại đường dẫn/kết nối mạng.");
                        }
                    }
                }

                _imagesModified = false;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error processing images before save");
                throw;
            }
        }

        private bool IsAlreadyUploaded(string imagePath)
        {
            if (string.IsNullOrEmpty(imagePath)) return false;

            return imagePath.Contains("amazonaws.com") ||
                   imagePath.StartsWith("Resources/Uploads") ||
                   Uri.TryCreate(imagePath, UriKind.Absolute, out var uri) &&
                   (uri.Host.Contains("amazonaws.com") || uri.Host.Contains("your-cdn-domain.com"));
        }

        private async Task<string> SaveImageToUploadsAsync(string imagePathOrUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePathOrUrl))
                    return string.Empty;

                var imageUploadService = _serviceProvider.GetRequiredService<IImageUploadService>();
                var uploadedUrl = await imageUploadService.UploadImageAsync(imagePathOrUrl);

                return uploadedUrl;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error uploading image: {Image}", imagePathOrUrl);
                throw new Exception($"Không thể tải ảnh lên: {ex.Message}");
            }
        }


        private async Task BtnCheckUrl_ClickAsync(object sender, EventArgs e)
        {
            var url = _imageValidation.ImageUrl?.Trim();

            if (string.IsNullOrEmpty(url))
            {
                new FrmToastMessage(ToastType.WARNING, "Vui lòng nhập URL hoặc đường dẫn hình ảnh").Show();
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
                    new FrmToastMessage(ToastType.ERROR, $"Lỗi khi tải file: {ex.Message}").Show();
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

        private async void BtnViewImage_Click(object sender, EventArgs e)
        {
            if (dgvProductImages.SelectedRows.Count == 0)
            {
                new FrmToastMessage(ToastType.WARNING, "Vui lòng chọn một hình ảnh trong danh sách").Show();
                return;
            }

            var selectedImage = dgvProductImages.SelectedRows[0].DataBoundItem as ProductImageViewModel;
            if (selectedImage == null || string.IsNullOrWhiteSpace(selectedImage.ImageUrl))
            {
                new FrmToastMessage(ToastType.ERROR, "Hình ảnh không hợp lệ").Show();
                return;
            }

            try
            {
                txtImagePath.Text = selectedImage.ImageUrl;
                _imageValidation.ImageUrl = selectedImage.ImageUrl;

                await LoadThumbnailImageAsync();

                if (tabControl.TabPages.Contains(tabBasicInfo))
                {
                    tabControl.SelectedTab = tabBasicInfo;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Không thể xem ảnh: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnDeleteImage_Click(object sender, EventArgs e)
        {
            if (dgvProductImages.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa hình ảnh này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    if (dgvProductImages.SelectedRows[0].DataBoundItem is ProductImageViewModel selectedImage && _product.ProductImages != null)
                    {
                        try
                        {
                            SetLoadingState(true);

                            if (selectedImage.Id > 0 && _productId.HasValue)
                            {
                                bool deleteResult = await _presenter.DeleteImageAsync(_productId.Value, selectedImage.Id);

                                if (deleteResult)
                                {
                                    _product.ProductImages.Remove(selectedImage);
                                    RefreshProductImagesGrid();

                                    if (string.Equals(selectedImage.ImageUrl, _imageValidation.ImageUrl, StringComparison.OrdinalIgnoreCase))
                                    {
                                        _imageValidation.ImageUrl = string.Empty;
                                        _imageValidation.OriginalImageUrl = string.Empty;
                                        _imagesModified = true;
                                        await LoadThumbnailImageAsync();
                                    }

                                    new FrmToastMessage(ToastType.SUCCESS, "Xóa hình ảnh thành công!").Show();
                                }
                                else
                                {
                                    new FrmToastMessage(ToastType.ERROR, "Không thể xóa hình ảnh. Vui lòng thử lại.").Show();
                                }
                            }
                            else
                            {
                                _product.ProductImages.Remove(selectedImage);
                                RefreshProductImagesGrid();
                                _imagesModified = true;

                                new FrmToastMessage(ToastType.SUCCESS, "Đã xóa hình ảnh khỏi danh sách!").Show();
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger?.LogError(ex, "Error deleting image {ImageId}", selectedImage.Id);
                            new FrmToastMessage(ToastType.ERROR, $"Lỗi khi xóa hình ảnh: {ex.Message}").Show();
                            MessageBox.Show($"Lỗi khi xóa hình ảnh: {ex.Message}");
                        }
                        finally
                        {
                            SetLoadingState(false);
                        }
                    }
                }
                await Task.Delay(200);
                await RefreshProductImagesFromDatabase();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một hình ảnh để xóa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private void RefreshProductImagesGrid()
        {
            try
            {
                dgvProductImages.SuspendLayout();
                dgvProductImages.DataSource = null;
                var list = _product.ProductImages ?? [];
                dgvProductImages.DataSource = new BindingList<ProductImageViewModel>(list);
                dgvProductImages.ResumeLayout();
                dgvProductImages.Refresh();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error refreshing product images grid");
                dgvProductImages.DataSource = null;
            }
        }
        #endregion

        #region Recipe Management Event Handlers
        private void BtnAssignRecipe_Click(object sender, EventArgs e)
        {
            if (cbxAvailableRecipes.SelectedItem is RecipeViewModel selectedRecipe)
            {
                // Check if already assigned
                if (_assignedRecipes.Any(r => r.Id == selectedRecipe.Id))
                {
                    new FrmToastMessage(ToastType.WARNING, "Công thức này đã được gán cho sản phẩm.").Show();
                    return;
                }

                // Add to assigned list
                _assignedRecipes.Add(selectedRecipe);
                RefreshAssignedRecipesGrid();
                _recipesModified = true;
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công thức để gán.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnUnassignRecipe_Click(object sender, EventArgs e)
        {
            if (dgvAssignedRecipes.SelectedRows.Count > 0)
            {
                var selectedRecipe = dgvAssignedRecipes.SelectedRows[0].DataBoundItem as RecipeViewModel;
                if (selectedRecipe != null)
                {
                    var result = MessageBox.Show(
                        $"Bạn có chắc chắn muốn bỏ gán công thức '{selectedRecipe.Name}'?",
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (result == DialogResult.Yes)
                    {
                        _assignedRecipes.Remove(selectedRecipe);
                        RefreshAssignedRecipesGrid();
                        _recipesModified = true;
                    }
                }
            }
            else
            {
                new FrmToastMessage(ToastType.WARNING, "Vui lòng chọn một công thức đã gán để bỏ gán.").Show();
            }
        }

        private async void BtnCreateNewRecipe_Click(object sender, EventArgs e)
        {
            try
            {
                using var frmRecipe = new FrmRecipeDetails();
                if (!await frmRecipe.CheckAuthorizationAsync())
                {
                    var warning = new FrmToastMessage(ToastType.WARNING, "Bạn không có quyền truy cập chức năng này!");
                    warning.Show();
                    frmRecipe.Dispose();
                    frmRecipe.BringToFront();
                    return;
                }

                if (frmRecipe.ShowDialog() == DialogResult.OK)
                {
                    var newRecipe = frmRecipe.Recipe;
                    if (newRecipe != null)
                    {
                        var recipeVm = new RecipeViewModel
                        {
                            Id = newRecipe.Id,
                            Name = newRecipe.Name,
                            Description = newRecipe.Description,
                            ProductId = newRecipe.ProductId,
                            ProductName = newRecipe.ProductName,
                            ServingSize = newRecipe.ServingSize,
                            Unit = newRecipe.Unit,
                            IsActive = newRecipe.IsActive,
                            Notes = newRecipe.Notes,
                            CreatedAt = newRecipe.CreatedAt,
                            UpdatedAt = newRecipe.UpdatedAt
                        };

                        _allAvailableRecipes.Add(recipeVm);
                        RefreshAvailableRecipesComboBox();

                        MessageBox.Show("Tạo công thức mới thành công!", "Thành công",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error creating new recipe");
                MessageBox.Show($"Lỗi khi tạo công thức mới: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void BtnEditRecipe_Click(object sender, EventArgs e)
        {
            var selectedRecipe = GetSelectedAssignedRecipe();
            if (selectedRecipe != null)
            {
                try
                {
                    var recipeDetail = new RecipeDetailViewModel
                    {
                        Id = selectedRecipe.Id,
                        Name = selectedRecipe.Name,
                        Description = selectedRecipe.Description,
                        ProductId = selectedRecipe.ProductId,
                        ProductName = selectedRecipe.ProductName,
                        ServingSize = selectedRecipe.ServingSize,
                        Unit = selectedRecipe.Unit,
                        IsActive = selectedRecipe.IsActive,
                        Notes = selectedRecipe.Notes,
                        CreatedAt = selectedRecipe.CreatedAt,
                        UpdatedAt = selectedRecipe.UpdatedAt
                    };

                    using var frmRecipe = new FrmRecipeDetails(selectedRecipe.Id, recipeDetail);

                    if (!await frmRecipe.CheckAuthorizationAsync())
                    {
                        var warning = new FrmToastMessage(ToastType.WARNING, "Bạn không có quyền truy cập chức năng này!");
                        warning.Show();
                        frmRecipe.Dispose();
                        frmRecipe.BringToFront();
                        return;
                    }

                    if (frmRecipe.ShowDialog() == DialogResult.OK)
                    {
                        var updatedRecipe = frmRecipe.Recipe;
                        if (updatedRecipe != null)
                        {
                            // Update the recipe in assigned list
                            selectedRecipe.Name = updatedRecipe.Name;
                            selectedRecipe.Description = updatedRecipe.Description;
                            selectedRecipe.ServingSize = updatedRecipe.ServingSize;
                            selectedRecipe.Unit = updatedRecipe.Unit;
                            selectedRecipe.IsActive = updatedRecipe.IsActive;
                            selectedRecipe.Notes = updatedRecipe.Notes;
                            selectedRecipe.UpdatedAt = updatedRecipe.UpdatedAt;

                            RefreshAssignedRecipesGrid();
                            _recipesModified = true;

                            MessageBox.Show("Cập nhật công thức thành công!", "Thành công",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error editing recipe");
                    MessageBox.Show($"Lỗi khi sửa công thức: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công thức để chỉnh sửa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }


        private async void BtnViewRecipeDetails_Click(object sender, EventArgs e)
        {
            var selectedRecipe = GetSelectedAssignedRecipe();
            if (selectedRecipe != null)
            {
                try
                {
                    var recipeDetail = new RecipeDetailViewModel
                    {
                        Id = selectedRecipe.Id,
                        Name = selectedRecipe.Name,
                        Description = selectedRecipe.Description,
                        ProductId = selectedRecipe.ProductId,
                        ProductName = selectedRecipe.ProductName,
                        ServingSize = selectedRecipe.ServingSize,
                        Unit = selectedRecipe.Unit,
                        IsActive = selectedRecipe.IsActive,
                        Notes = selectedRecipe.Notes,
                        CreatedAt = selectedRecipe.CreatedAt,
                        UpdatedAt = selectedRecipe.UpdatedAt
                    };

                    using var frmRecipe = new FrmRecipeDetails(selectedRecipe.Id, recipeDetail, isReadOnly: true);
                    if (!await frmRecipe.CheckAuthorizationAsync())
                    {
                        var warning = new FrmToastMessage(ToastType.WARNING, "Bạn không có quyền truy cập chức năng này!");
                        warning.Show();
                        frmRecipe.Dispose();
                        frmRecipe.BringToFront();
                        return;
                    }
                    frmRecipe.ShowDialog();
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error viewing recipe details");
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
        private void RefreshAssignedRecipesGrid()
        {
            try
            {
                // Suspend layout to prevent flickering and binding issues
                dgvAssignedRecipes.SuspendLayout();

                // Clear the data source and reset
                dgvAssignedRecipes.DataSource = null;
                dgvAssignedRecipes.Rows.Clear();

                // Create a new BindingList to avoid reference issues
                var bindingList = new BindingList<RecipeViewModel>(_assignedRecipes);
                dgvAssignedRecipes.DataSource = bindingList;

                dgvAssignedRecipes.ResumeLayout();
                dgvAssignedRecipes.Refresh();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error refreshing assigned recipes grid");
                // Fallback: just clear the grid if binding fails
                dgvAssignedRecipes.DataSource = null;
                dgvAssignedRecipes.Rows.Clear();
            }
        }

        private void RefreshAvailableRecipesComboBox()
        {
            try
            {
                cbxAvailableRecipes.DataSource = null;
                cbxAvailableRecipes.Items.Clear();

                cbxAvailableRecipes.DisplayMember = "Name";
                cbxAvailableRecipes.ValueMember = "Id";

                // Create a new list to avoid reference issues
                var bindingList = new List<RecipeViewModel>(_allAvailableRecipes);
                cbxAvailableRecipes.DataSource = bindingList;

                cbxAvailableRecipes.Refresh();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error refreshing available recipes combo box");
                // Fallback: just clear the combo box
                cbxAvailableRecipes.DataSource = null;
                cbxAvailableRecipes.Items.Clear();
            }
        }

        private RecipeViewModel? GetSelectedAssignedRecipe()
        {
            if (dgvAssignedRecipes.SelectedRows.Count > 0)
            {
                return dgvAssignedRecipes.SelectedRows[0].DataBoundItem as RecipeViewModel;
            }
            return null;
        }

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
            vm.ProductImages = [];

            vm.Recipes ??= [];
            vm.Recipes = [];


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

            // Recipe controls
            btnAssignRecipe.Enabled = !isLoading;
            btnUnassignRecipe.Enabled = !isLoading;
            btnCreateNewRecipe.Enabled = !isLoading;
            btnDetailRecipe.Enabled = !isLoading;
            cbxAvailableRecipes.Enabled = !isLoading;

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