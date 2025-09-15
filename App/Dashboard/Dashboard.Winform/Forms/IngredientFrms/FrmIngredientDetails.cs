using Dashboard.Winform.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Dashboard.Winform.Forms
{
    public partial class FrmIngredientDetails : Form
    {
        public IngredientDetailViewModel? Ingredient { get; private set; }

        private readonly long? _ingredientId;
        private readonly IngredientDetailViewModel? _initialModel;
        private bool _isEditMode;

        public FrmIngredientDetails(long? ingredientId = null, IngredientDetailViewModel? initialModel = null)
        {
            _ingredientId = ingredientId;
            _initialModel = initialModel;
            _isEditMode = ingredientId.HasValue && initialModel != null;

            InitializeComponent();
            SetupForm();
            LoadData();
        }

        private void SetupForm()
        {
            Text = _isEditMode ? "Cập nhật nguyên liệu" : "Thêm nguyên liệu mới";

            // Hide ID fields if not in edit mode
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

        private void LoadData()
        {
            // Load categories (mock data for now)
            LoadCategories();

            // Load existing data if in edit mode
            if (_isEditMode && _initialModel != null)
            {
                LoadInitialData();
            }
            else
            {
                // Set default values for new ingredient
                chkIsActive.Checked = true;
                if (cbxCategory.Items.Count > 0)
                {
                    cbxCategory.SelectedIndex = 0;
                }
            }
        }

        private void LoadCategories()
        {
            var categories = new List<CategoryViewModel>
            {
                new() { Id = 1, Name = "Thịt" },
                new() { Id = 2, Name = "Rau củ" },
                new() { Id = 3, Name = "Gia vị" },
                new() { Id = 4, Name = "Hải sản" },
                new() { Id = 5, Name = "Đồ khô" }
            };

            cbxCategory.DataSource = categories;
            cbxCategory.DisplayMember = "Name";
            cbxCategory.ValueMember = "Id";
        }

        private void LoadInitialData()
        {
            if (_initialModel == null) return;

            txtId.Text = _initialModel.Id.ToString();
            txtName.Text = _initialModel.Name;
            txtUnit.Text = _initialModel.Unit;
            cbxCategory.SelectedValue = _initialModel.CategoryId;
            txtDescription.Text = _initialModel.Description ?? "";
            chkIsActive.Checked = _initialModel.IsActive;
            txtCreated.Text = _initialModel.CreatedAt.ToString("dd/MM/yyyy HH:mm");
            txtUpdated.Text = _initialModel.UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "Chưa cập nhật";
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    var ingredient = CreateIngredientFromInput();
                    Ingredient = ingredient;
                    DialogResult = DialogResult.OK;
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Vui lòng nhập tên nguyên liệu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtUnit.Text))
            {
                MessageBox.Show("Vui lòng nhập đơn vị tính.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUnit.Focus();
                return false;
            }

            if (cbxCategory.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn danh mục.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxCategory.Focus();
                return false;
            }

            return true;
        }

        private IngredientDetailViewModel CreateIngredientFromInput()
        {
            var selectedCategory = cbxCategory.SelectedItem as CategoryViewModel;

            var ingredient = new IngredientDetailViewModel
            {
                Id = _isEditMode ? _initialModel?.Id ?? 0 : 0,
                Name = txtName.Text.Trim(),
                Unit = txtUnit.Text.Trim(),
                CategoryId = selectedCategory?.Id ?? 1,
                CategoryName = selectedCategory?.Name ?? "",
                Description = txtDescription.Text.Trim(),
                IsActive = chkIsActive.Checked,
                CreatedAt = _isEditMode ? (_initialModel?.CreatedAt ?? DateTime.Now) : DateTime.Now,
                UpdatedAt = _isEditMode ? DateTime.Now : null
            };

            return ingredient;
        }
    }
}