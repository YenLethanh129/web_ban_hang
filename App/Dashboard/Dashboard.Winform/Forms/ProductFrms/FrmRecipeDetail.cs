using Dashboard.Winform.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms.ProductFrms
{
    public partial class FrmRecipeDetail : Form
    {
        #region Fields
        private readonly bool _isEditMode;
        private readonly bool _isReadOnly;
        private readonly long? _recipeId;
        private RecipeDetailViewModel _recipe = null!;
        #endregion

        #region Properties
        public DialogResult Result { get; private set; }
        public RecipeDetailViewModel Recipe => _recipe;
        #endregion

        public FrmRecipeDetail()
        {
            InitializeComponent();
        }
        #region Constructor
        public FrmRecipeDetail(long? recipeId = null, RecipeDetailViewModel? recipe = null, bool isReadOnly = false)
        {
            _recipeId = recipeId;
            _isEditMode = recipeId.HasValue;
            _isReadOnly = isReadOnly;
            _recipe = recipe ?? new RecipeDetailViewModel();

            InitializeComponent();
            InitializeFormSettings();
            LoadInitialData();

            if (_isReadOnly)
            {
                Text = $"Chi tiết công thức - {_recipe.Name}";
                SetReadOnlyMode();
            }
            else if (_isEditMode)
            {
                Text = $"Chỉnh sửa công thức - {_recipe.Name}";
            }
            else
            {
                Text = "Thêm công thức mới";
            }

            PopulateFormWithData();
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
        private void LoadInitialData()
        {
            // TODO: Load products, ingredients, etc.
            // This is a stub implementation
        }

        private void PopulateFormWithData()
        {
            if (_recipe == null) return;

            // TODO: Populate form fields with recipe data
            // This is a stub implementation
        }

        private void SetReadOnlyMode()
        {
            // TODO: Make all controls read-only
            // This is a stub implementation
        }
        #endregion

        #region Event Handlers
        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (_isReadOnly) return;

            try
            {
                // TODO: Validate and collect form data
                Result = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu công thức: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }
        #endregion
    }
}
