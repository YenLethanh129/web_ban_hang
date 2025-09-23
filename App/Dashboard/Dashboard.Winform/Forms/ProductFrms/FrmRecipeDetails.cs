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

namespace Dashboard.Winform.Forms
{
    public partial class FrmRecipeDetails : Form
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
            SetupDataGridView();

            UpdateFormMode();
            ApplyDarkTheme();
            TabControlHelper.SetupDarkTheme(tabControl);

            // Setup presenter events
            _presenter.OnRecipeSaved += Presenter_OnRecipeSaved;
            _presenter.OnDataLoaded += Presenter_OnDataLoaded;

            Load += FrmRecipeDetails_Load;

            FormClosed += (s, e) =>
            {
                TabControlHelper.CleanupDarkTheme(tabControl);
            };
        }

        public FrmRecipeDetails(long? recipeId = null, RecipeDetailViewModel? recipe = null, bool? isReadOnly = false)
            : this(
                  ServiceProviderHolder.Current ?? throw new InvalidOperationException("ServiceProviderHolder.Current is null. Set FrmRecipeDetails.ServiceProviderHolder.Current = serviceProvider in Program.cs before using this constructor."),
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
            this.Size = new Size(900, 700);
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

                // Make all controls read-only
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

            btnAddIngredient.Enabled = !readOnly;
            btnEditIngredient.Enabled = !readOnly;
            btnDeleteIngredient.Enabled = !readOnly;
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

            btnAddIngredient.Click += (s, e) => BtnAddIngredient_Click(s!, e);
            btnEditIngredient.Click += (s, e) => BtnEditIngredient_Click(s!, e);
            btnDeleteIngredient.Click += (s, e) => BtnDeleteIngredient_Click(s!, e);
        }

        private void SetupDataGridView()
        {
            dgvIngredients.AutoGenerateColumns = false;
            dgvIngredients.Columns.Clear();

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.IngredientName),
                HeaderText = "Nguyên liệu",
                Width = 150
            });

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.Quantity),
                HeaderText = "Số lượng",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
            });

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.Unit),
                HeaderText = "Đơn vị",
                Width = 80
            });

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.WastePercentage),
                HeaderText = "Hao hụt (%)",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N1" }
            });

            dgvIngredients.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.IsOptional),
                HeaderText = "Tùy chọn",
                Width = 80
            });

            dgvIngredients.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeIngredientViewModel.Notes),
                HeaderText = "Ghi chú",
                Width = 150
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

                // Load products for combobox
                await LoadProductsAsync();

                // Load recipe data if in edit mode
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
                    // Set default values for new recipe
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

                dgvIngredients.DataSource = _recipe.RecipeIngredients;

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

        private void BtnAddIngredient_Click(object sender, EventArgs e)
        {
            try
            {
                // TODO: Open ingredient selection dialog
                // For now, show a placeholder message
                MessageBox.Show("Chức năng thêm nguyên liệu sẽ được triển khai sau",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error adding ingredient");
                MessageBox.Show($"Lỗi khi thêm nguyên liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                        // TODO: Open ingredient edit dialog
                        MessageBox.Show($"Chức năng sửa nguyên liệu '{selectedIngredient.IngredientName}' sẽ được triển khai sau",
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

        private void BtnDeleteIngredient_Click(object sender, EventArgs e)
        {
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
                            _recipe.RecipeIngredients.Remove(selectedIngredient);
                            dgvIngredients.Refresh();
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error deleting ingredient");
                    MessageBox.Show($"Lỗi khi xóa nguyên liệu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nguyên liệu để xóa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
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
            SafeInvokeOnUI(() =>
            {
                // Refresh UI if needed
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
            catch
            {
                // Ignore invocation exceptions
            }
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
                }

            }
            base.Dispose(disposing);
        }
        #endregion
    }
}