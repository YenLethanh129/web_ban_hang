
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform.Forms
{
    public partial class FrmRecipeDetails : Form
    {
        #region Fields
        private readonly bool _isEditMode;
        private readonly long? _recipeId;
        private RecipeDetailViewModel _recipe;
        #endregion

        #region Properties
        public DialogResult Result { get; private set; }
        public RecipeDetailViewModel Recipe => _recipe;
        public FrmRecipeDetails(long? recipeId = null, RecipeDetailViewModel? recipe = null, bool? isReadOnly = false)
        {
            _recipeId = recipeId;
            _isEditMode = !(isReadOnly ?? false);
            _recipe = recipe ?? new RecipeDetailViewModel();

            InitializeComponent();
            InitializeFormSettings();
            LoadInitialData();
            SetupEventHandlers();
            SetupDataGridView();

            if (_isEditMode)
            {
                PopulateFormWithData();
                Text = $"Chi tiết công thức - {_recipe.Name}";
                btnClose.Visible = true;
                btnSave.Text = "Lưu";
            }
            else
            {
                Text = "Thêm công thức mới";
                btnClose.Visible = false;
                btnSave.Text = "Thêm";
            }

            ApplyDarkTheme();
        }
        #endregion

        #region Initialization Methods
        private void InitializeFormSettings()
        {
            this.Size = new Size(800, 600);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
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
            btnSave.Click += (s, e) => BtnSave_Click(s!, e);
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

        private void LoadInitialData()
        {
            // Load products
            cbxProduct.Items.Clear();
            cbxProduct.Items.Add(new { Id = 1, Name = "Cà phê đen" });
            cbxProduct.Items.Add(new { Id = 2, Name = "Cà phê sữa" });
            cbxProduct.Items.Add(new { Id = 3, Name = "Bánh mì thịt" });
            cbxProduct.DisplayMember = "Name";
            cbxProduct.ValueMember = "Id";

            // Set default values
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

        private void BtnAddIngredient_Click(object sender, EventArgs e)
        {
            // TODO: Implement add ingredient dialog
            MessageBox.Show("Chức năng thêm nguyên liệu sẽ được triển khai",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditIngredient_Click(object sender, EventArgs e)
        {
            if (dgvIngredients.SelectedRows.Count > 0)
            {
                // TODO: Implement edit ingredient dialog
                MessageBox.Show("Chức năng sửa nguyên liệu sẽ được triển khai",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa nguyên liệu này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // TODO: Implement delete ingredient logic
                    MessageBox.Show("Chức năng xóa nguyên liệu sẽ được triển khai",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nguyên liệu để xóa",
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
                MessageBox.Show("Tên công thức không được để trống", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void NumServingSize_Validating(object sender, CancelEventArgs e)
        {
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

            if (cbxProduct.SelectedItem != null)
            {
                dynamic productItem = cbxProduct.SelectedItem;
                _recipe.ProductId = productItem.Id;
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
        #endregion
    }
}
