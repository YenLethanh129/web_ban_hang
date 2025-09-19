using Dashboard.Winform.Events;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms
{
    public partial class FrmProductManagement : FrmBaseManagement<ProductManagementModel, ProductManagementPresenter>
    {
        #region Fields
        private readonly ProductManagementModel _model;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public FrmProductManagement(
            ILogger<FrmProductManagement> logger,
            IServiceProvider serviceProvider,
            ProductManagementPresenter productPresenter
        ) : base(logger, productPresenter)
        {
            _model = _presenter.Model;
            _serviceProvider = serviceProvider;

            InitializeBaseComponents();

            _presenter.OnDataLoaded += (s, e) =>
            {
                try
                {
                    if (e is ProductsLoadedEventArgs args)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                try
                                {
                                    ApplyProductsToModel(args.Products, args.TotalCount);
                                }
                                catch (Exception ex)
                                {
                                    ShowError($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
                                }
                            }));
                        }
                        else
                        {
                            ApplyProductsToModel(args.Products, args.TotalCount);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() => ShowError($"Lỗi xử lý dữ liệu: {ex.Message}")));
                    }
                    else
                    {
                        ShowError($"Lỗi xử lý dữ liệu: {ex.Message}");
                    }
                }
            };

            Load += FrmProductManagement_Load;
            OverrideTextUI();
            OverrideComboBoxItem();
            SetupDataBindings();
            SetupDgvListItem();
            FinalizeFormSetup();
            SetupContextMenu();
        }
        #endregion

        #region Override Base Components
        protected override void InitializeDerivedComponents()
        {
            InitializeDgvListItem();
        }

        private void OverrideTextUI()
        {
            lblFilter1.Text = "Trạng thái:";
            lblFilter2.Text = "Danh mục:";
            lblSearchString.Text = "Tìm kiếm theo (ID/tên sản phẩm):";
            Text = "Quản lý sản phẩm";
        }

        private void SetupDataBindings()
        {
            cbxFilter1.DataSource = _model.Statuses;
            cbxFilter1.SelectedItem = "All";

            cbxFilter2.DataSource = _model.Categories;
            cbxFilter2.DisplayMember = "Name";
            cbxFilter2.ValueMember = "Id";
            cbxFilter2.SelectedValue = 0;

            tbxFindString.DataBindings.Add(
                "Text", _model,
                nameof(_model.SearchText),
                false, DataSourceUpdateMode.OnPropertyChanged
            );
        }


        private void OverrideComboBoxItem()
        {
            cbxOrderBy.Items.Clear();
            cbxOrderBy.Items.AddRange(["ID", "Name", "Price", "Category", "SoldQuantity"]);
            if (cbxOrderBy.Items.Count > 0)
                cbxOrderBy.SelectedIndex = 0;
        }

        protected void SetupDgvListItem()
        {
            if (dgvProducts == null)
            {
                throw new InvalidOperationException("dgvProducts must be initialized before calling SetupDgvListItem()");
            }

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

            dgvProducts.DataSource = _model.Products;
            dgvProducts.Refresh();
        }
        #endregion

        #region Override Event Handlers Base Class

        protected override void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
        }

        protected override void Btnfilter1_Click(object sender, EventArgs e)
        {
            base.Btnfilter1_Click(sender, e);
        }

        protected override void BtnFilter2_Click(object sender, EventArgs e)
        {
            base.BtnFilter2_Click(sender, e);
        }

        protected override void BtnOrderBy_Click(object sender, EventArgs e)
        {
            base.BtnOrderBy_Click(sender, e);
        }

        protected override void BtnNumbOfRecordShowing_Click(object sender, EventArgs e)
        {
            base.BtnNumbOfRecordShowing_Click(sender, e);
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
            cbxFilter2.SelectedIndexChanged += (s, e) => ApplyCategoryFilter();
            cbxOrderBy.SelectedIndexChanged += (s, e) => ApplySorting();
        }

        protected override async Task TbxFindString_TextChanged(object? sender, EventArgs e)
        {
            var textBox = sender as TextBox;
            var searchText = textBox?.Text;
            if (string.IsNullOrEmpty(searchText))
                return;
            await _presenter.SearchAsync(searchText);
        }

        #endregion

        #region Product Management Specific Methods

        private async void PerformSearch()
        {
            try
            {
                SetLoadingState(true);
                await _presenter.SearchAsync(_model.SearchText);
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tìm kiếm: {ex.Message}");
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
                await _presenter.FilterByStatusAsync(selectedStatus!);
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lọc theo trạng thái: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void ApplyCategoryFilter()
        {
            try
            {
                if (cbxFilter2.SelectedValue == null) return;

                SetLoadingState(true);
                if (cbxFilter2.SelectedValue is long categoryId)
                {
                    await _presenter.FilterByCategoryAsync(categoryId);
                    UpdatePaginationInfo();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lọc theo danh mục: {ex.Message}");
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
                await _presenter.SortBy(sortBy);
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi sắp xếp: {ex.Message}");
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
                await _presenter.GoToNextPageAsync();
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi chuyển trang: {ex.Message}");
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
                await _presenter.GoToPreviousPageAsync();
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi chuyển trang: {ex.Message}");
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
                    await _presenter.ChangePageSizeAsync(pageSize);
                    UpdatePaginationInfo();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi thay đổi số bản ghi hiển thị: {ex.Message}");
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
                await _presenter.RefreshCacheAsync();
                UpdatePaginationInfo();
                ShowInfo("Dữ liệu đã được cập nhật!");
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi làm mới dữ liệu: {ex.Message}");
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
            if (_model != null)
            {
                lblNumberOfRecords.Text = $"Số lượng: {_model.TotalItems}";
                lblShowingAtPage.Text = $"Hiện trang {_model.CurrentPage} trên {_model.TotalPages}";


                btnToday.Enabled = _model.CurrentPage > 1;
                btnNext.Enabled = _model.CurrentPage < _model.TotalPages;
            }
        }

        private void ShowError(string message)
        {
            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ShowInfo(string message)
        {
            MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SetLoadingState(bool isLoading)
        {
            btnGetDetails.Enabled = !isLoading;
            btnAdd.Enabled = !isLoading;
            cbxFilter1.Enabled = !isLoading;
            cbxFilter2.Enabled = !isLoading;
            cbxOrderBy.Enabled = !isLoading;
            cbxNumbRecordsPerPage.Enabled = !isLoading;

            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
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

        private void ApplyProductsToModel(List<ProductViewModel> products, int totalCount)
        {
            _model.Products.Clear();
            foreach (var product in products)
            {
                _model.Products.Add(product);
            }

            _model.TotalItems = totalCount;
            UpdatePaginationInfo();
        }

        #endregion

        #region Event Handlers

        private async void FrmProductManagement_Load(object? sender, EventArgs e)
        {
            _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                SetLoadingState(true);

                await _presenter.LoadDataAsync(page: _model.CurrentPage, pageSize: _model.PageSize);
                
                cbxFilter1.SelectedItem = "All";

                if (cbxFilter2.Items.Count > 0)
                    cbxFilter2.SelectedIndex = 0;
                

                UpdatePaginationInfo();

                _dataLoadingCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                _dataLoadingCompletionSource.SetException(ex);
                ShowError($"Lỗi khi tải dữ liệu sản phẩm: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        #endregion

        #region Dialog Integration Methods

        private async void OpenProductDetailsDialog(ProductViewModel? selectedProduct = null)
        {
            try
            {
                SetLoadingState(true);

                ProductDetailViewModel? initialModel = null;

                if (selectedProduct != null)
                {
                    initialModel = new ProductDetailViewModel
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
                        ThumbnailPath = selectedProduct.Thumbnail,
                        CreatedAt = selectedProduct.CreatedAt,
                        UpdatedAt = selectedProduct.UpdatedAt,
                        ProductImages = new BindingList<ProductImageViewModel>(),
                        Recipes = new BindingList<RecipeViewModel>(),
                        ProductRecipes = new BindingList<ProductRecipeViewModel>()
                    };
                }

                using var detailForm = _serviceProvider.GetRequiredService<FrmProductDetails>();

                detailForm.SetInitData(selectedProduct?.Id, initialModel);

                var result = detailForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    var updatedProduct = detailForm.Product;

                    if (selectedProduct != null)
                    {
                        await HandleProductUpdate(updatedProduct);
                    }
                    else
                    {
                        await HandleProductAdd(updatedProduct);
                        ShowInfo("Thêm sản phẩm mới thành công!");
                    }

                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi {(selectedProduct != null ? "cập nhật" : "thêm")} sản phẩm: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }


        private async Task HandleProductAdd(ProductDetailViewModel product)
        {
            // TODO: Implement product add logic through presenter
            await Task.Delay(50);
            Console.WriteLine($"Thêm sản phẩm: {product.Name}, Giá: {product.Price}");
        }

        private async Task HandleProductUpdate(ProductDetailViewModel product)
        {
            // TODO: Implement product update logic through presenter
            await Task.Delay(50);
            Console.WriteLine($"Cập nhật sản phẩm ID {product.Id}: {product.Name}");
        }

        #endregion

        #region Override Event Handlers - Updated

        protected override void BtnAdd_Click(object sender, EventArgs e)
        {
            OpenProductDetailsDialog();
        }

        protected override void BtnGetDetails_Click(object sender, EventArgs e)
        {
            var selectedProduct = GetSelectedProduct();

            if (selectedProduct != null)
            {
                OpenProductDetailsDialog(selectedProduct);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một sản phẩm để xem chi tiết.",
                               "Thông báo",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
        }

        private void DgvListItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedProduct = GetSelectedProduct();
                if (selectedProduct != null)
                {
                    OpenProductDetailsDialog(selectedProduct);
                }
            }
        }

        #endregion

        #region Additional Helper Methods

        /// <summary>
        /// Setup thêm event cho DataGridView double-click
        /// </summary>
        private void SetupAdditionalEvents()
        {
            if (dgvProducts != null)
            {
                dgvProducts.CellDoubleClick += (s, o) => DgvListItems_CellDoubleClick(s!, o);

                dgvProducts.SelectionChanged += (s, e) =>
                {
                    btnGetDetails.Enabled = dgvProducts.SelectedRows.Count > 0;
                };
            }
        }

        private void FinalizeFormSetup()
        {
            SetupAdditionalEvents();

            if (btnGetDetails != null)
                btnGetDetails.Enabled = false;
        }

        #endregion

        #region Context Menu for Refresh

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Làm mới dữ liệu", null, (s, e) => RefreshData());
            dgvProducts.ContextMenuStrip = contextMenu;
        }

        #endregion
    }
}