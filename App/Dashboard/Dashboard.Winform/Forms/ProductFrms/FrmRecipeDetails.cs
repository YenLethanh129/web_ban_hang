using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Dashboard.Winform.Helpers;
using Dashboard.Winform.Presenters.RecipePresenters;
using Dashboard.Winform.Forms.BaseFrm;

namespace Dashboard.Winform.Forms
{
    public partial class FrmRecipeDetails : FrmBaseAuthForm
    {
        public static class ServiceProviderHolder
        {
            public static IServiceProvider? Current;
        }

        #region Fields
        private readonly bool _isEditMode;
        private readonly bool _isReadOnly;
        private readonly long? _recipeId;
        private RecipeDetailViewModel _recipe;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<FrmRecipeDetails>? _logger;
        private readonly IRecipeDetailPresenter _presenter;
        private bool _isDataLoaded = false;

        // Lists for managing ingredients
        private BindingList<RecipeIngredientViewModel> _recipeIngredients = new();
        private BindingList<IngredientViewModel> _availableIngredients = new();
        private BindingList<IngredientViewModel> _filteredIngredients = new();
        #endregion

        #region Properties
        public DialogResult Result { get; private set; }
        public RecipeDetailViewModel Recipe => _recipe;
        #endregion

        #region Constructors
        public FrmRecipeDetails(
            IServiceProvider serviceProvider,
            IRecipeDetailPresenter presenter,
            ILogger<FrmRecipeDetails>? logger = null,
            long? recipeId = null,
            RecipeDetailViewModel? recipe = null,
            bool? isReadOnly = false)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _logger = logger;
            _recipeId = recipeId;
            _isReadOnly = isReadOnly ?? false;
            _isEditMode = recipeId.HasValue && !_isReadOnly;
            _recipe = recipe ?? new RecipeDetailViewModel();

            InitializeComponent();
            InitializeFormSettings();
            SetupEventHandlers();
            SetupDataGridViews();

            UpdateFormMode();
            ApplyDarkTheme();
            TabControlHelper.SetupDarkTheme(tabControl);

            _presenter.OnRecipeSaved += Presenter_OnRecipeSaved;
            _presenter.OnDataLoaded += Presenter_OnDataLoaded;
            _presenter.OnIngredientAdded += Presenter_OnIngredientAdded;
            _presenter.OnIngredientDeleted += Presenter_OnIngredientDeleted;

            Load += FrmRecipeDetails_Load;

            FormClosed += (s, e) =>
            {
                TabControlHelper.CleanupDarkTheme(tabControl);
            };
        }

        public FrmRecipeDetails(long? recipeId = null, RecipeDetailViewModel? recipe = null, bool? isReadOnly = false)
            : this(
                  ServiceProviderHolder.Current ?? throw new InvalidOperationException("ServiceProviderHolder.Current is null"),
                  ServiceProviderHolder.Current!.GetRequiredService<IRecipeDetailPresenter>(),
                  ServiceProviderHolder.Current!.GetService<ILogger<FrmRecipeDetails>>(),
                  recipeId,
                  recipe,
                  isReadOnly)
        {
        }
        #endregion

        #region Initialization Methods
        private void InitializeFormSettings()
        {
            this.Size = new Size(1100, 800);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
        }

        private void UpdateFormMode()
        {
            if (_isReadOnly)
            {
                Text = $"Xem công thức - {_recipe.Name}";
                btnSave.Visible = false;
                btnCancel.Text = "Đóng";
                SetControlsReadOnly(true);
            }
            else if (_isEditMode)
            {
                Text = $"Sửa công thức - {_recipe.Name}";
                btnSave.Text = "Lưu";
                SetControlsReadOnly(false);
            }
            else
            {
                Text = "Thêm công thức mới";
                btnSave.Text = "Thêm";
                SetControlsReadOnly(false);
            }
        }

        private void SetControlsReadOnly(bool readOnly)
        {
            txtName.ReadOnly = readOnly;
            txtDescription.ReadOnly = readOnly;
            numServingSize.Enabled = !readOnly;
            txtUnit.ReadOnly = readOnly;
            chkIsActive.Enabled = !readOnly;
            txtNotes.ReadOnly = readOnly;
            cbxProduct.Enabled = !readOnly;

            btnAddIngredient.Enabled = !readOnly && _isEditMode; // Only enable if recipe exists
            btnEditIngredient.Enabled = !readOnly && _isEditMode;
            btnDeleteIngredient.Enabled = !readOnly && _isEditMode;
            txtSearchIngredient.ReadOnly = readOnly || !_isEditMode;
        }

        private void ApplyDarkTheme()
        {
            BackColor = Color.FromArgb(24, 28, 63);

            foreach (Control control in this.Controls)
            {
                if (control is Panel panel)
                {
                    panel.BackColor = Color.FromArgb(42, 45, 86);
                }
            }
        }

        private void SetupEventHandlers()
        {
            btnSave.Click += async (s, e) => await BtnSave_ClickAsync(s!, e);
            btnCancel.Click += (s, e) => BtnCancel_Click(s!, e);
            btnClose.Click += (s, e) => BtnClose_Click(s!, e);

            txtName.Validating += (s, e) => TxtName_Validating(s!, e);
            numServingSize.Validating += (s, e) => NumServingSize_Validating(s!, e);

            btnAddIngredient.Click += async (s, e) => await BtnAddIngredient_ClickAsync(s!, e);
            btnEditIngredient.Click += (s, e) => BtnEditIngredient_Click(s!, e);
            btnDeleteIngredient.Click += async (s, e) => await BtnDeleteIngredient_ClickAsync(s!, e);

            txtSearchIngredient.TextChanged += (s, e) => TxtSearchIngredient_TextChanged(s!, e);
            btnClearSearch.Click += (s, e) => BtnClearSearch_Click(s!, e);

            dgvIngredients.CellEndEdit += async (s, e) => await DgvIngredients_CellEndEdit(s!, e);
        }

        private void SetupDataGridViews()
        {
            // DataGridView for recipe ingredients (top)
            dgvIngredients.AutoGenerateColumns = false;
            dgvIngredients.Columns.Clear();
            dgvIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngredients.MultiSelect = false;
            dgvIngredients.AllowUserToAddRows = false;
            dgvIngredients.ColumnHeadersVisible = false;

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.IngredientName),
                HeaderText = "Nguyên liệu",
                Width = 150,
                ReadOnly = true
            });

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.Quantity),
                HeaderText = "Số lượng",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" },
                ReadOnly = false
            });

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.Unit),
                HeaderText = "Đơn vị",
                Width = 80,
                ReadOnly = true
            });

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.WastePercentage),
                HeaderText = "Hao hụt (%)",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N1" },
                ReadOnly = false
            });

            dgvIngredients.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.IsOptional),
                HeaderText = "Tùy chọn",
                Width = 70,
                ReadOnly = false
            });

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.Notes),
                HeaderText = "Ghi chú",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = false
            });

            // DataGridView for available ingredients (bottom)
            dgvAvailableIngredients.AutoGenerateColumns = false;
            dgvAvailableIngredients.Columns.Clear();
            dgvAvailableIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailableIngredients.MultiSelect = true;
            dgvAvailableIngredients.ReadOnly = true;
            dgvAvailableIngredients.ColumnHeadersVisible = false;


            dgvAvailableIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.Name),
                HeaderText = "Tên nguyên liệu",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            });

            dgvAvailableIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.Unit),
                HeaderText = "Đơn vị",
                Width = 80
            });

            dgvAvailableIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.CategoryName),
                HeaderText = "Danh mục",
                Width = 120
            });

            dgvAvailableIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.StatusText),
                HeaderText = "Trạng thái",
                Width = 100
            });
        }

        private async void FrmRecipeDetails_Load(object? sender, EventArgs e)
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

                await LoadProductsAsync();
                await LoadAvailableIngredientsAsync();

                if (_isEditMode && _recipeId.HasValue)
                {
                    var recipe = await _presenter.LoadRecipeAsync(_recipeId.Value);
                    if (recipe != null)
                    {
                        _recipe = recipe;
                        PopulateFormWithData();
                    }
                }
                else if (!_isEditMode)
                {
                    SetDefaultValues();
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error loading initial data");
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task LoadProductsAsync()
        {
            try
            {
                var products = await _presenter.LoadProductsAsync();

                cbxProduct.DataSource = null;
                cbxProduct.DisplayMember = "Name";
                cbxProduct.ValueMember = "Id";
                cbxProduct.DataSource = products;

                if (cbxProduct.Items.Count > 0 && cbxProduct.SelectedIndex == -1)
                    cbxProduct.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error loading products");
                LoadFallbackProducts();
            }
        }

        private async Task LoadAvailableIngredientsAsync()
        {
            try
            {
                var ingredients = await _presenter.LoadIngredientsAsync();
                _availableIngredients = new BindingList<IngredientViewModel>(ingredients);
                _filteredIngredients = new BindingList<IngredientViewModel>(ingredients);

                dgvAvailableIngredients.DataSource = _filteredIngredients;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error loading available ingredients");
                MessageBox.Show($"Lỗi khi tải danh sách nguyên liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadFallbackProducts()
        {
            cbxProduct.Items.Clear();
            cbxProduct.Items.Add(new { Id = 1, Name = "Cà phê đen" });
            cbxProduct.Items.Add(new { Id = 2, Name = "Cà phê sữa" });
            cbxProduct.Items.Add(new { Id = 3, Name = "Bánh mì thịt" });
            cbxProduct.DisplayMember = "Name";
            cbxProduct.ValueMember = "Id";
        }

        private void SetDefaultValues()
        {
            chkIsActive.Checked = true;
            numServingSize.Value = 1;
            txtUnit.Text = "portion";
            _recipeIngredients = new BindingList<RecipeIngredientViewModel>();
            dgvIngredients.DataSource = _recipeIngredients;
        }

        private void PopulateFormWithData()
        {
            if (_recipe == null) return;

            try
            {
                txtRecipeId.Text = _recipe.Id.ToString();
                txtName.Text = _recipe.Name;
                txtDescription.Text = _recipe.Description ?? "";
                numServingSize.Value = _recipe.ServingSize;
                txtUnit.Text = _recipe.Unit;
                chkIsActive.Checked = _recipe.IsActive;
                txtNotes.Text = _recipe.Notes ?? "";

                SetComboBoxSelection(cbxProduct, _recipe.ProductId);

                lblCreatedAt.Text = _recipe.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                lblUpdatedAt.Text = _recipe.UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";

                _recipeIngredients = new BindingList<RecipeIngredientViewModel>(_recipe.RecipeIngredients.ToList());
                dgvIngredients.DataSource = _recipeIngredients;

                UpdateFormMode();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error populating form with data");
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Event Handlers
        private async Task BtnSave_ClickAsync(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            try
            {
                SetLoadingState(true);
                CollectFormData();

                RecipeDetailViewModel? result;
                if (_isEditMode)
                {
                    result = await _presenter.UpdateRecipeAsync(_recipe);
                }
                else
                {
                    result = await _presenter.CreateRecipeAsync(_recipe);
                }

                if (result != null)
                {
                    _recipe = result;
                    Result = DialogResult.OK;
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Lưu công thức thất bại.", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error saving recipe");
                MessageBox.Show($"Lỗi khi lưu công thức: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
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

        private async Task BtnAddIngredient_ClickAsync(object sender, EventArgs e)
        {
            if (_isReadOnly || !_isEditMode) return;

            if (!_recipeId.HasValue)
            {
                MessageBox.Show("Vui lòng lưu công thức trước khi thêm nguyên liệu",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            try
            {
                if (dgvAvailableIngredients.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Vui lòng chọn ít nhất một nguyên liệu để thêm",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                SetLoadingState(true);

                foreach (DataGridViewRow row in dgvAvailableIngredients.SelectedRows)
                {
                    var ingredient = row.DataBoundItem as IngredientViewModel;
                    if (ingredient == null) continue;

                    if (_recipeIngredients.Any(ri => ri.IngredientId == ingredient.Id))
                    {
                        MessageBox.Show($"Nguyên liệu '{ingredient.Name}' đã có trong công thức",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        continue;
                    }

                    // Add to database immediately
                    await _presenter.AddIngredientToRecipeAsync(_recipeId.Value, ingredient.Id, 1);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding ingredient");
                MessageBox.Show($"Lỗi khi thêm nguyên liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void BtnEditIngredient_Click(object sender, EventArgs e)
        {
            if (dgvIngredients.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedIngredient = dgvIngredients.SelectedRows[0].DataBoundItem as RecipeIngredientViewModel;
                    if (selectedIngredient != null)
                    {
                        MessageBox.Show($"Chỉnh sửa trực tiếp trong ô để cập nhật thông tin nguyên liệu '{selectedIngredient.IngredientName}'",
                            "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error editing ingredient");
                    MessageBox.Show($"Lỗi khi sửa nguyên liệu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nguyên liệu để chỉnh sửa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task BtnDeleteIngredient_ClickAsync(object sender, EventArgs e)
        {
            if (_isReadOnly || !_isEditMode) return;

            if (dgvIngredients.SelectedRows.Count > 0)
            {
                try
                {
                    var selectedIngredient = dgvIngredients.SelectedRows[0].DataBoundItem as RecipeIngredientViewModel;
                    if (selectedIngredient != null)
                    {
                        var result = MessageBox.Show(
                            $"Bạn có chắc chắn muốn xóa nguyên liệu '{selectedIngredient.IngredientName}'?",
                            "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                        if (result == DialogResult.Yes)
                        {
                            SetLoadingState(true);

                            // Delete from database immediately
                            var deleted = await _presenter.DeleteIngredientFromRecipeAsync(
                                _recipeId!.Value,
                                selectedIngredient.IngredientId);

                            if (!deleted)
                            {
                                MessageBox.Show("Không thể xóa nguyên liệu", "Lỗi",
                                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }

                            SetLoadingState(false);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error deleting ingredient");
                    MessageBox.Show($"Lỗi khi xóa nguyên liệu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    SetLoadingState(false);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nguyên liệu để xóa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private async Task DgvIngredients_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (_isReadOnly || !_isEditMode) return;

            try
            {
                var ingredient = dgvIngredients.Rows[e.RowIndex].DataBoundItem as RecipeIngredientViewModel;
                if (ingredient != null && ingredient.Id > 0)
                {
                    SetLoadingState(true);
                    await _presenter.UpdateRecipeIngredientAsync(ingredient);
                    SetLoadingState(false);
                }
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error updating ingredient");
                MessageBox.Show($"Lỗi khi cập nhật nguyên liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetLoadingState(false);
            }
        }

        private void TxtSearchIngredient_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var searchText = txtSearchIngredient.Text.Trim().ToLower();

                if (string.IsNullOrWhiteSpace(searchText))
                {
                    _filteredIngredients = new BindingList<IngredientViewModel>(_availableIngredients.ToList());
                }
                else
                {
                    var filtered = _availableIngredients
                        .Where(i => i.Name.ToLower().Contains(searchText) ||
                                   i.CategoryName.ToLower().Contains(searchText))
                        .ToList();
                    _filteredIngredients = new BindingList<IngredientViewModel>(filtered);
                }

                dgvAvailableIngredients.DataSource = _filteredIngredients;
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error filtering ingredients");
            }
        }

        private void BtnClearSearch_Click(object sender, EventArgs e)
        {
            txtSearchIngredient.Clear();
            txtSearchIngredient.Focus();
        }

        private void Presenter_OnRecipeSaved(object? sender, RecipeDetailViewModel? recipe)
        {
            if (recipe != null)
            {
                _recipe = recipe;
                SafeInvokeOnUI(() => PopulateFormWithData());
            }
        }

        private void Presenter_OnDataLoaded(object? sender, EventArgs e)
        {
            SafeInvokeOnUI(() => { });
        }

        private void Presenter_OnIngredientAdded(object? sender, RecipeIngredientViewModel ingredient)
        {
            SafeInvokeOnUI(() =>
            {
                _recipeIngredients.Add(ingredient);
                dgvIngredients.Refresh();
            });
        }

        private void Presenter_OnIngredientDeleted(object? sender, long ingredientId)
        {
            SafeInvokeOnUI(() =>
            {
                var item = _recipeIngredients.FirstOrDefault(ri => ri.IngredientId == ingredientId);
                if (item != null)
                {
                    _recipeIngredients.Remove(item);
                    dgvIngredients.Refresh();
                }
            });
        }
        #endregion

        #region Validation Event Handlers
        private void TxtName_Validating(object sender, CancelEventArgs e)
        {
            if (_isReadOnly) return;

            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox?.Text))
            {
                MessageBox.Show("Tên công thức không được để trống", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void NumServingSize_Validating(object sender, CancelEventArgs e)
        {
            if (_isReadOnly) return;

            var numericUpDown = sender as NumericUpDown;
            if (numericUpDown?.Value <= 0)
            {
                MessageBox.Show("Khẩu phần phải lớn hơn 0", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }
        #endregion

        #region Helper Methods
        private bool ValidateForm()
        {
            if (_isReadOnly) return true;

            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên công thức không được để trống", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            if (cbxProduct.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn sản phẩm", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbxProduct.Focus();
                return false;
            }

            if (numServingSize.Value <= 0)
            {
                MessageBox.Show("Khẩu phần phải lớn hơn 0", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                numServingSize.Focus();
                return false;
            }

            return true;
        }

        private void CollectFormData()
        {
            _recipe.Name = txtName.Text.Trim();
            _recipe.Description = txtDescription.Text.Trim();
            _recipe.ServingSize = numServingSize.Value;
            _recipe.Unit = txtUnit.Text.Trim();
            _recipe.IsActive = chkIsActive.Checked;
            _recipe.Notes = txtNotes.Text.Trim();

            if (cbxProduct.SelectedValue != null)
            {
                _recipe.ProductId = Convert.ToInt64(cbxProduct.SelectedValue);
                _recipe.ProductName = cbxProduct.Text;
            }
        }

        private void SetComboBoxSelection(ComboBox comboBox, long id)
        {
            try
            {
                comboBox.SelectedValue = id;

                if (comboBox.SelectedIndex == -1)
                {
                    for (int i = 0; i < comboBox.Items.Count; i++)
                    {
                        var item = comboBox.Items[i];
                        if (item != null)
                        {
                            var idProperty = item.GetType().GetProperty("Id");
                            if (idProperty != null)
                            {
                                var itemId = idProperty.GetValue(item);
                                if (itemId != null && Convert.ToInt64(itemId) == id)
                                {
                                    comboBox.SelectedIndex = i;
                                    break;
                                }
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

        private void SetLoadingState(bool isLoading)
        {
            btnSave.Enabled = !isLoading;
            btnCancel.Enabled = !isLoading;
            btnAddIngredient.Enabled = !isLoading && _isEditMode;
            btnDeleteIngredient.Enabled = !isLoading && _isEditMode;
            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void SafeInvokeOnUI(Action action)
        {
            if (action == null) return;
            if (IsDisposed || Disposing) return;

            try
            {
                if (InvokeRequired)
                    BeginInvoke(action);
                else
                    action();
            }
            catch { }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                if (_presenter != null)
                {
                    _presenter.OnRecipeSaved -= Presenter_OnRecipeSaved;
                    _presenter.OnDataLoaded -= Presenter_OnDataLoaded;
                    _presenter.OnIngredientAdded -= Presenter_OnIngredientAdded;
                    _presenter.OnIngredientDeleted -= Presenter_OnIngredientDeleted;
                }
            }
            base.Dispose(disposing);
        }
        #endregion
    }
}