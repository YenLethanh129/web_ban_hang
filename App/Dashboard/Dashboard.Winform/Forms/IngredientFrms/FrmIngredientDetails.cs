using Dashboard.Common.Constants;
using Dashboard.Winform.Attributes;
using Dashboard.Winform.Forms.BaseFrm;
using Dashboard.Winform.Presenters.IngredientPresenters;
using Dashboard.Winform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Dashboard.Winform.Forms
{
    [RequireRole("ADMIN")]
    [RequirePermission("PRODUCT_UPDATE")]
    public partial class FrmIngredientDetails : FrmBaseAuthForm
    {
        private readonly IIngredientDetailPresenter _presenter;
        private readonly IngredientDetailViewModel _viewModel;
        private readonly bool _isEditMode;

        public IngredientDetailViewModel? SavedIngredient { get; private set; }

        private IEnumerable<IngredientCategoryViewModel>? _categories;

        public FrmIngredientDetails(
                    IIngredientDetailPresenter presenter,
                    long? ingredientId = null,
                    IngredientDetailViewModel? initialModel = null)
        {
            _presenter = presenter;
            _isEditMode = ingredientId.HasValue && initialModel != null;

            _viewModel = initialModel ?? new IngredientDetailViewModel();

            InitializeComponent();
            SetupForm();
            SetupDataBindings();
            LoadData();
        }


        private void SetupForm()
        {
            Text = _isEditMode ? "Lưu nguyên liệu" : "Thêm nguyên liệu mới";

            if (!_isEditMode)
            {
                lblId.Visible = false;
                txtId.Visible = false;
                lblCreated.Visible = false;
                txtCreated.Visible = false;
                lblUpdated.Visible = false;
                txtUpdated.Visible = false;
            }
        }

        private void SetupDataBindings()
        {
            txtId.DataBindings.Clear();
            txtName.DataBindings.Clear();
            txtUnit.DataBindings.Clear();
            txtDescription.DataBindings.Clear();
            chkIsActive.DataBindings.Clear();
            txtCreated.DataBindings.Clear();
            txtUpdated.DataBindings.Clear();

            txtId.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Id), false, DataSourceUpdateMode.Never);
            txtName.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Name), false, DataSourceUpdateMode.OnPropertyChanged);
            txtUnit.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Unit), false, DataSourceUpdateMode.OnPropertyChanged);
            txtDescription.DataBindings.Add("Text", _viewModel, nameof(_viewModel.Description), false, DataSourceUpdateMode.OnPropertyChanged);
            chkIsActive.DataBindings.Add("Checked", _viewModel, nameof(_viewModel.IsActive), false, DataSourceUpdateMode.OnPropertyChanged);
            txtCreated.DataBindings.Add("Text", _viewModel, nameof(_viewModel.CreatedAtFormatted), false, DataSourceUpdateMode.Never);
            txtUpdated.DataBindings.Add("Text", _viewModel, nameof(_viewModel.UpdatedAtFormatted), false, DataSourceUpdateMode.Never);
        }

        public async Task SetCategories(IEnumerable<IngredientCategoryViewModel> categories)
        {
            _categories = categories ?? Array.Empty<IngredientCategoryViewModel>();
            if (IsHandleCreated)
            {
                await LoadCategories();
            }
        }


        private async void LoadData()
        {
            try
            {
                SetLoadingState(true);
                await LoadCategories();

                if (!_isEditMode)
                {
                    _viewModel.IsActive = true;
                    _viewModel.CreatedAt = DateTime.Now;
                    if (cbxCategory.Items.Count > 0)
                    {
                        cbxCategory.SelectedIndex = 0;
                    }
                }
                else if (cbxCategory.Items.Count > 0)
                {
                    var category = cbxCategory.Items.Cast<IngredientCategoryViewModel>()
                        .FirstOrDefault(c => c.Id == _viewModel.CategoryId);
                    if (category != null)
                    {
                        cbxCategory.SelectedItem = category;
                    }
                }
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

        private async Task LoadCategories()
        {
            try
            {
                List<IngredientCategoryViewModel> categories;

                if (_presenter != null)
                {
                    categories = await _presenter.LoadCategoriesAsync();
                }
                else
                {
                    // Use default categories if no presenter
                    categories = GetDefaultCategories();
                }

                cbxCategory.DataSource = categories;
                cbxCategory.DisplayMember = "Name";
                cbxCategory.ValueMember = "Id";
            }
            catch (Exception ex)
            {
                var categories = GetDefaultCategories();
                cbxCategory.DataSource = categories;
                cbxCategory.DisplayMember = "Name";
                cbxCategory.ValueMember = "Id";
                new FrmToastMessage(ToastType.ERROR, $"Lỗi khi tải danh mục: {ex.Message}").Show();
            }
        }

        private List<IngredientCategoryViewModel> GetDefaultCategories()
        {
            return new List<IngredientCategoryViewModel>
            {
                new() { Id = 1, Name = "Rau củ", Description = "Các loại rau củ quả" },
                new() { Id = 2, Name = "Thịt cá", Description = "Thịt và hải sản" },
                new() { Id = 3, Name = "Gia vị", Description = "Các loại gia vị" },
                new() { Id = 4, Name = "Ngũ cốc", Description = "Gạo, bột, ngũ cốc" }
            };
        }

        private async void BtnSave_Click(object? sender, EventArgs e)
        {
            if (!ValidateChildren())
            {
                new FrmToastMessage(ToastType.WARNING, "Vui lòng kiểm tra lại thông tin nhập vào.").Show();
                return;
            }
            try
            {
                SetLoadingState(true);

                if (cbxCategory.SelectedItem is IngredientCategoryViewModel selectedCategory)
                {
                    _viewModel.CategoryId = selectedCategory.Id;
                    _viewModel.CategoryName = selectedCategory.Name;
                }


                await _presenter.SaveIngredientAsync(_viewModel);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)                                                                
            {                                                                                  
                new FrmToastMessage(ToastType.ERROR, $"Lỗi khi lưu dữ liệu: {ex.Message}").Show();
            }
            finally
            {
                SetLoadingState(false);
            }
        }


        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void TxtName_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                errorProvider.SetError(txtName, "Tên nguyên liệu không được để trống");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtName, string.Empty);
            }
        }

        private void TxtUnit_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                errorProvider.SetError(txtUnit, "Đơn vị không được để trống");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(txtUnit, string.Empty);
            }
        }

        private void CbxCategory_Validating(object? sender, System.ComponentModel.CancelEventArgs e)
        {
            if (cbxCategory.SelectedItem == null)
            {
                errorProvider.SetError(cbxCategory, "Vui lòng chọn danh mục");
                e.Cancel = true;
            }
            else
            {
                errorProvider.SetError(cbxCategory, string.Empty);
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            btnSave.Enabled = !isLoading;
            btnCancel.Enabled = !isLoading;
            txtName.Enabled = !isLoading;
            txtUnit.Enabled = !isLoading;
            cbxCategory.Enabled = !isLoading;
            txtDescription.Enabled = !isLoading;
            chkIsActive.Enabled = !isLoading;

            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }
    }
}