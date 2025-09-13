using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Winform.Events;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;

namespace Dashboard.Winform.Forms
{
    public partial class FrmProductManagement : FrmBaseManagement
    {
        #region Fields
        private readonly ILogger<FrmProductManagement> _logger;
        private readonly ProductManagementPresenter _productPresenter;
        private readonly RecipeManagementPresenter _recipePresenter;
        private readonly ProductManagementModel _productModel;
        private readonly RecipeManagementModel _recipeModel;
        #endregion

        #region Constructor
        public FrmProductManagement(
            ILogger<FrmProductManagement> logger,
            ProductManagementPresenter productPresenter,
            RecipeManagementPresenter recipePresenter
        ) : base()
        {
            _logger = logger;
            _productPresenter = productPresenter;
            _recipePresenter = recipePresenter;
            _productModel = _productPresenter.Model;
            _recipeModel = _recipePresenter.Model;

            InitializeDgvListItem();
            InitializeBaseComponents();

            //SetupPresenters();
            //OverrideTextUI();
            //OverrideComboBoxItems();
            //SetupDataBindings();
            SetupDataGridViews();
            SetupTabControl();
            FinalizeFormSetup();

            Load += FrmProductManagement_Load;
        }
        #endregion

        #region Override Base Components
        protected override void InitializeDerivedComponents()
        {
            // Components will be initialized in InitializeComponent()
        }
        #endregion

        #region Setup Methods
        private void SetupDataGridViews()
        {
            SetupProductDataGridView();
            SetupRecipeDataGridView();
        }

        private void SetupProductDataGridView()
        {
            dgvProducts.AutoGenerateColumns = false;
            dgvProducts.Columns.Clear();

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductViewModel.Id),
                HeaderText = "ID",
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductViewModel.Name),
                HeaderText = "Tên sản phẩm",
                Width = 200
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductViewModel.Description),
                HeaderText = "Mô tả",
                Width = 250
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductViewModel.Price),
                HeaderText = "Giá",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvProducts.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(ProductViewModel.CategoryName),
                HeaderText = "Danh mục",
                Width = 120
            });

            dgvProducts.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(ProductViewModel.IsActive),
                HeaderText = "Hoạt động",
                Width = 80
            });

            dgvProducts.DataSource = _productModel.Products;
            dgvProducts.RowPrePaint += DgvListItems_RowPrePaint;
        }

        private void SetupRecipeDataGridView()
        {
            dgvRecipes.AutoGenerateColumns = false;
            dgvRecipes.Columns.Clear();

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Id),
                HeaderText = "ID",
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.Name),
                HeaderText = "Tên công thức",
                Width = 200
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.ProductName),
                HeaderText = "Sản phẩm",
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

            dgvRecipes.DataSource = _recipeModel.Recipes;
            dgvRecipes.RowPrePaint += DgvListItems_RowPrePaint;
        }

        private void SetupTabControl()
        {
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            //tabControl.DrawItem += TabControl_DrawItem;
            tabControl.SelectedIndexChanged += TabControl_SelectedIndexChanged;
            ApplyDarkThemeToTabs();
        }

        private void ApplyDarkThemeToTabs()
        {
            tabControl.BackColor = Color.FromArgb(24, 28, 63);

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.BackColor = Color.FromArgb(42, 45, 86);
            }
        }
        #endregion

        #region Tab Control Events
        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tabControl)
                return;

            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabRect = tabControl.GetTabRect(e.Index);

            // Colors for dark theme
            Color tabBackColor = Color.FromArgb(24, 28, 63);
            Color selectedTabBackColor = Color.FromArgb(42, 45, 86);
            Color tabTextColor = Color.FromArgb(124, 141, 181);
            Color selectedTabTextColor = Color.FromArgb(192, 255, 192);
            Color borderColor = Color.FromArgb(107, 83, 255);

            using (SolidBrush brush = new SolidBrush(e.Index == tabControl.SelectedIndex ? selectedTabBackColor : tabBackColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            if (e.Index == tabControl.SelectedIndex)
            {
                using Pen pen = new Pen(borderColor, 2);
                e.Graphics.DrawRectangle(pen, tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);
            }

            using SolidBrush textBrush = new SolidBrush(e.Index == tabControl.SelectedIndex ? selectedTabTextColor : tabTextColor);
            StringFormat stringFormat = new StringFormat()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.DrawString(tabPage.Text, tabControl.Font, textBrush, tabRect, stringFormat);
        }

        private void TabControl_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                // Update filter combo box based on selected tab
                UpdateFilterComboBox();

                // Update search text binding
                UpdateSearchBinding();

                // Refresh pagination info
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi chuyển tab: {ex.Message}");
            }
        }

        private void UpdateFilterComboBox()
        {
            if (tabControl.SelectedIndex == 0) // Products tab
            {
                lblFilter2.Text = "Danh mục:";
                cbxFilter2.DataSource = null;
                cbxFilter2.Items.Clear();

                // TODO: Load categories from service
                cbxFilter2.Items.Add(new { Id = 0, Name = "All" });
                cbxFilter2.Items.Add(new { Id = 1, Name = "Đồ uống" });
                cbxFilter2.Items.Add(new { Id = 2, Name = "Đồ ăn" });
                cbxFilter2.Items.Add(new { Id = 3, Name = "Bánh kẹo" });
                cbxFilter2.DisplayMember = "Name";
                cbxFilter2.ValueMember = "Id";
                cbxFilter2.SelectedIndex = 0;
            }
            else // Recipes tab
            {
                lblFilter2.Text = "Sản phẩm:";
                cbxFilter2.DataSource = null;
                cbxFilter2.Items.Clear();

                // TODO: Load products from service
                cbxFilter2.Items.Add(new { Id = 0, Name = "All" });
                cbxFilter2.Items.Add(new { Id = 1, Name = "Cà phê đen" });
                cbxFilter2.Items.Add(new { Id = 2, Name = "Cà phê sữa" });
                cbxFilter2.Items.Add(new { Id = 3, Name = "Bánh mì thịt" });
                cbxFilter2.DisplayMember = "Name";
                cbxFilter2.ValueMember = "Id";
                cbxFilter2.SelectedIndex = 0;
            }
        }

        private void UpdateSearchBinding()
        {
            tbxFindString.DataBindings.Clear();

            if (tabControl.SelectedIndex == 0) // Products tab
            {
                tbxFindString.DataBindings.Add(
                    "Text", _productModel,
                    nameof(_productModel.SearchText),
                    false, DataSourceUpdateMode.OnPropertyChanged
                );
            }
            else // Recipes tab
            {
                tbxFindString.DataBindings.Add(
                    "Text", _recipeModel,
                    nameof(_recipeModel.SearchText),
                    false, DataSourceUpdateMode.OnPropertyChanged
                );
            }
        }
        #endregion

        #region Override Event Handlers
        protected override void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        protected override void BtnAdd_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
                OpenProductDetailsDialog();
            else
                OpenRecipeDetailsDialog();
        }

        protected override void BtnGetDetails_Click(object sender, EventArgs e)
        {
            if (tabControl.SelectedIndex == 0)
            {
                var selectedProduct = GetSelectedProduct();
                if (selectedProduct != null)
                    OpenProductDetailsDialog(selectedProduct);
                else
                    ShowErrorMessage("Vui lòng chọn một sản phẩm để xem chi tiết.");
            }
            else
            {
                var selectedRecipe = GetSelectedRecipe();
                if (selectedRecipe != null)
                    OpenRecipeDetailsDialog(selectedRecipe);
                else
                    ShowErrorMessage("Vui lòng chọn một công thức để xem chi tiết.");
            }
        }

        protected override void BtnNext_Click(object sender, EventArgs e)
        {
            GoToNextPage();
        }

        protected override void BtnToday_Click(object sender, EventArgs e)
        {
            GoToPreviousPage();
        }

        protected override void CbxNumbRecordsPerPage_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdatePageSize();
        }

        protected override void InitializeEvents()
        {
            base.InitializeEvents();

            cbxFilter1.SelectedIndexChanged += (s, e) => ApplyStatusFilter();
            cbxFilter2.SelectedIndexChanged += (s, e) => ApplySecondFilter();
            cbxOrderBy.SelectedIndexChanged += (s, e) => ApplySorting();
        }

        protected override async Task TbxFindString_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(300);

            var textBox = sender as TextBox;
            var searchText = textBox?.Text;
            if (string.IsNullOrEmpty(searchText))
                return;

            if (tabControl.SelectedIndex == 0)
                await _productPresenter.SearchAsync(searchText);
            else
                await _recipePresenter.SearchAsync(searchText);
        }
        #endregion

        #region Business Logic Methods
        private async void PerformSearch()
        {
            try
            {
                SetLoadingState(true);

                if (tabControl.SelectedIndex == 0)
                    await _productPresenter.SearchAsync(_productModel.SearchText);
                else
                    await _recipePresenter.SearchAsync(_recipeModel.SearchText);

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi tìm kiếm: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void ApplyStatusFilter()
        {
            try
            {
                if (cbxFilter1.SelectedItem == null) return;

                SetLoadingState(true);
                var selectedStatus = cbxFilter1.SelectedItem.ToString();

                if (tabControl.SelectedIndex == 0)
                    await _productPresenter.FilterByStatusAsync(selectedStatus!);
                else
                    await _recipePresenter.FilterByStatusAsync(selectedStatus!);

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi lọc theo trạng thái: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void ApplySecondFilter()
        {
            try
            {
                if (cbxFilter2.SelectedValue == null) return;

                SetLoadingState(true);

                if (cbxFilter2.SelectedValue is int id && id > 0)
                {
                    if (tabControl.SelectedIndex == 0)
                        await _productPresenter.FilterByCategoryAsync(id);
                    else
                        await _recipePresenter.FilterByProductAsync(id);
                }

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi lọc: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void ApplySorting()
        {
            try
            {
                if (cbxOrderBy.SelectedItem == null) return;

                SetLoadingState(true);
                var sortBy = cbxOrderBy.SelectedItem.ToString();

                if (tabControl.SelectedIndex == 0)
                    await _productPresenter.SortBy(sortBy);
                else
                    await _recipePresenter.SortBy(sortBy);

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi sắp xếp: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void GoToNextPage()
        {
            try
            {
                SetLoadingState(true);

                if (tabControl.SelectedIndex == 0)
                    await _productPresenter.GoToNextPageAsync();
                else
                    await _recipePresenter.GoToNextPageAsync();

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi chuyển trang: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void GoToPreviousPage()
        {
            try
            {
                SetLoadingState(true);

                if (tabControl.SelectedIndex == 0)
                    await _productPresenter.GoToPreviousPageAsync();
                else
                    await _recipePresenter.GoToPreviousPageAsync();

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi chuyển trang: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void UpdatePageSize()
        {
            try
            {
                if (cbxNumbRecordsPerPage.SelectedItem == null) return;

                SetLoadingState(true);
                if (int.TryParse(cbxNumbRecordsPerPage.SelectedItem.ToString(), out int pageSize))
                {
                    if (tabControl.SelectedIndex == 0)
                        await _productPresenter.ChangePageSizeAsync(pageSize);
                    else
                        await _recipePresenter.ChangePageSizeAsync(pageSize);
                }

                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi thay đổi số bản ghi hiển thị: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void RefreshData()
        {
            try
            {
                SetLoadingState(true);

                if (tabControl.SelectedIndex == 0)
                    await _productPresenter.RefreshCacheAsync();
                else
                    await _recipePresenter.RefreshCacheAsync();

                UpdatePaginationInfo();
                ShowSuccessMessage("Dữ liệu đã được cập nhật!");
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi làm mới dữ liệu: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        #endregion

        #region Helper Methods
        private void UpdatePaginationInfo()
        {
            if (tabControl.SelectedIndex == 0) // Products tab
            {
                UpdateRecordsDisplay(_productModel.TotalItems, _productModel.CurrentPage, _productModel.TotalPages);
                UpdatePaginationButtons(_productModel.CurrentPage > 1, _productModel.CurrentPage < _productModel.TotalPages);
            }
            else // Recipes tab
            {
                UpdateRecordsDisplay(_recipeModel.TotalItems, _recipeModel.CurrentPage, _recipeModel.TotalPages);
                UpdatePaginationButtons(_recipeModel.CurrentPage > 1, _recipeModel.CurrentPage < _recipeModel.TotalPages);
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            ShowLoading(isLoading);
        }

        private ProductViewModel? GetSelectedProduct()
        {
            if (dgvProducts.SelectedRows.Count > 0)
            {
                var selectedRow = dgvProducts.SelectedRows[0];
                return selectedRow.DataBoundItem as ProductViewModel;
            }
            return null;
        }

        private RecipeViewModel? GetSelectedRecipe()
        {
            if (dgvRecipes.SelectedRows.Count > 0)
            {
                var selectedRow = dgvRecipes.SelectedRows[0];
                return selectedRow.DataBoundItem as RecipeViewModel;
            }
            return null;
        }

        private void ApplyProductsToModel(List<ProductViewModel> products, int totalCount)
        {
            _productModel.Products.Clear();
            foreach (var product in products)
            {
                _productModel.Products.Add(product);
            }

            _productModel.TotalItems = totalCount;
            UpdatePaginationInfo();
        }

        private void ApplyRecipesToModel(List<RecipeViewModel> recipes, int totalCount)
        {
            _recipeModel.Recipes.Clear();
            foreach (var recipe in recipes)
            {
                _recipeModel.Recipes.Add(recipe);
            }

            _recipeModel.TotalItems = totalCount;
            UpdatePaginationInfo();
        }

        private void FinalizeFormSetup()
        {
            SetupAdditionalEvents();
            btnGetDetails.Enabled = false;
        }

        private void SetupAdditionalEvents()
        {
            if (dgvProducts != null)
            {
                dgvProducts.CellDoubleClick += (s, e) =>
                {
                    if (e.RowIndex >= 0)
                    {
                        var selectedProduct = GetSelectedProduct();
                        if (selectedProduct != null)
                            OpenProductDetailsDialog(selectedProduct);
                    }
                };

                dgvProducts.SelectionChanged += (s, e) =>
                {
                    if (tabControl.SelectedIndex == 0)
                        btnGetDetails.Enabled = dgvProducts.SelectedRows.Count > 0;
                };
            }

            if (dgvRecipes != null)
            {
                dgvRecipes.CellDoubleClick += (s, e) =>
                {
                    if (e.RowIndex >= 0)
                    {
                        var selectedRecipe = GetSelectedRecipe();
                        if (selectedRecipe != null)
                            OpenRecipeDetailsDialog(selectedRecipe);
                    }
                };

                dgvRecipes.SelectionChanged += (s, e) =>
                {
                    if (tabControl.SelectedIndex == 1)
                        btnGetDetails.Enabled = dgvRecipes.SelectedRows.Count > 0;
                };
            }
        }
        #endregion

        #region Dialog Methods
        private async void OpenProductDetailsDialog(ProductViewModel? selectedProduct = null)
        {
            try
            {
                SetLoadingState(true);

                ProductDetailViewModel? detailViewModel = null;

                if (selectedProduct != null)
                {
                    // TODO: Load product details from service
                    detailViewModel = new ProductDetailViewModel
                    {
                        Id = selectedProduct.Id,
                        Name = selectedProduct.Name,
                        Description = selectedProduct.Description,
                        Price = selectedProduct.Price,
                        IsActive = selectedProduct.IsActive,
                        CategoryId = selectedProduct.CategoryId,
                        CategoryName = selectedProduct.CategoryName,
                        TaxId = selectedProduct.TaxId,
                        TaxName = selectedProduct.TaxName,
                        Thumbnail = selectedProduct.Thumbnail,
                        CreatedAt = selectedProduct.CreatedAt,
                        UpdatedAt = selectedProduct.UpdatedAt,
                        ProductImages = new BindingList<ProductImageViewModel>(),
                        Recipes = new BindingList<RecipeViewModel>(),
                        ProductRecipes = new BindingList<ProductRecipeViewModel>()
                    };
                }

                using var detailForm = new FrmProductDetails(selectedProduct?.Id, detailViewModel);
                var result = detailForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    var updatedProduct = detailForm.Product;

                    if (selectedProduct != null)
                    {
                        await HandleProductUpdate(updatedProduct);
                        ShowSuccessMessage("Cập nhật sản phẩm thành công!");
                    }
                    else
                    {
                        await HandleProductAdd(updatedProduct);
                        ShowSuccessMessage("Thêm sản phẩm mới thành công!");
                    }

                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi {(selectedProduct != null ? "cập nhật" : "thêm")} sản phẩm: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void OpenRecipeDetailsDialog(RecipeViewModel? selectedRecipe = null)
        {
            try
            {
                SetLoadingState(true);

                RecipeDetailViewModel? detailViewModel = null;

                if (selectedRecipe != null)
                {
                    // TODO: Load recipe details from service
                    detailViewModel = new RecipeDetailViewModel
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
                        UpdatedAt = selectedRecipe.UpdatedAt,
                        RecipeIngredients = new BindingList<RecipeIngredientViewModel>()
                    };
                }

                using var detailForm = new FrmRecipeDetails(selectedRecipe?.Id, detailViewModel);
                var result = detailForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    var updatedRecipe = detailForm.Recipe;

                    if (selectedRecipe != null)
                    {
                        await HandleRecipeUpdate(updatedRecipe);
                        ShowSuccessMessage("Cập nhật công thức thành công!");
                    }
                    else
                    {
                        await HandleRecipeAdd(updatedRecipe);
                        ShowSuccessMessage("Thêm công thức mới thành công!");
                    }

                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage($"Lỗi khi {(selectedRecipe != null ? "cập nhật" : "thêm")} công thức: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task HandleProductAdd(ProductDetailViewModel product)
        {
            // TODO: Implement product add logic
            await Task.Delay(50);
            _logger.LogInformation("Added product: {ProductName}, Price: {Price}", product.Name, product.Price);
        }

        private async Task HandleProductUpdate(ProductDetailViewModel product)
        {
            // TODO: Implement product update logic
            await Task.Delay(50);
            _logger.LogInformation("Updated product ID {ProductId}: {ProductName}", product.Id, product.Name);
        }

        private async Task HandleRecipeAdd(RecipeDetailViewModel recipe)
        {
            // TODO: Implement recipe add logic
            await Task.Delay(50);
            _logger.LogInformation("Added recipe: {RecipeName} for product: {ProductName}", recipe.Name, recipe.ProductName);
        }

        private async Task HandleRecipeUpdate(RecipeDetailViewModel recipe)
        {
            // TODO: Implement recipe update logic
            await Task.Delay(50);
            _logger.LogInformation("Updated recipe ID {RecipeId}: {RecipeName}", recipe.Id, recipe.Name);
        }
        #endregion

        #region Form Events
        private async void FrmProductManagement_Load(object? sender, EventArgs e)
        {
            _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                SetLoadingState(true);

                // Load initial data for products tab
                await _productPresenter.LoadDataAsync(page: _productModel.CurrentPage, pageSize: _productModel.PageSize);

                // Preload recipes data
                await _recipePresenter.LoadDataAsync(page: _recipeModel.CurrentPage, pageSize: _recipeModel.PageSize);

                UpdatePaginationInfo();
                _dataLoadingCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                _dataLoadingCompletionSource.SetException(ex);
                ShowErrorMessage($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
        #endregion
    }
}