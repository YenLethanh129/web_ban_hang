using Dashboard.Common.Constants;
using Dashboard.Winform.Events;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Presenters.RecipePresenters;
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
    public partial class FrmRecipeManagement : FrmBaseManagement<RecipeManagementModel, IRecipeManagementPresenter>
    {
        #region Fields
        private readonly RecipeManagementModel _model;
        private readonly IServiceProvider _serviceProvider;
        #endregion

        #region Constructor
        public FrmRecipeManagement(
            ILogger<FrmRecipeManagement> logger,
            IServiceProvider serviceProvider,
            IRecipeManagementPresenter recipePresenter
        ) : base(logger, recipePresenter)
        {
            _model = (RecipeManagementModel)_presenter.Model;
            _serviceProvider = serviceProvider;

            InitializeComponent();
            InitializeBaseComponents();

            _presenter.OnDataLoaded += (s, e) =>
            {
                try
                {
                    if (e is RecipesLoadedEventArgs args)
                    {
                        SafeInvokeOnUI(() =>
                        {
                            try
                            {
                                ApplyRecipesToModel(args.Recipes, args.TotalCount);
                            }
                            catch (Exception ex)
                            {
                                ShowError($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
                            }
                        });
                    }
                }
                catch (Exception ex)
                {
                    SafeInvokeOnUI(() => ShowError($"Lỗi xử lý dữ liệu: {ex.Message}"));
                }
            };

            Load += FrmRecipeManagement_Load;
            OverrideTextUI();
            OverrideComboBoxItem();
            SetupDataBindings();
            SetupDgvListItem();
            FinalizeFormSetup();
            SetupContextMenu();
        }
        #endregion

        #region Override Base Components

        private void OverrideTextUI()
        {
            lblFilter1.Text = "Trạng thái:";
            lblFilter2.Text = "Sản phẩm:";
            lblSearchString.Text = "Tìm kiếm theo (ID/tên công thức):";
            Text = "Quản lý công thức";
        }

        private void SetupDataBindings()
        {
            cbxFilter1.DataSource = _model.Statuses;
            cbxFilter1.SelectedItem = "All";

            cbxFilter2.DataSource = _model.Products;
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
            cbxOrderBy.Items.AddRange(new[] { "ID", "Name", "Product", "ServingSize", "CreatedAt" });
            if (cbxOrderBy.Items.Count > 0)
                cbxOrderBy.SelectedIndex = 0;
        }

        protected void SetupDgvListItem()
        {
            if (dgvRecipes == null)
            {
                throw new InvalidOperationException("dgvRecipes must be initialized before calling SetupDgvListItem()");
            }

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
                DataPropertyName = nameof(RecipeViewModel.Description),
                HeaderText = "Mô tả",
                Width = 250
            });

            dgvRecipes.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RecipeViewModel.ProductName),
                HeaderText = "Sản phẩm",
                Width = 150
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

            dgvRecipes.DataSource = _model.Recipes;
            dgvRecipes.Refresh();

            dgvRecipes.ColumnHeaderMouseClick -= DgvRecipes_ColumnHeaderMouseClick;
            dgvRecipes.ColumnHeaderMouseClick += DgvRecipes_ColumnHeaderMouseClick;


            if (pnlContent?.Controls.Contains(dgvRecipes) == true)
            {
                pnlContent.Controls.Remove(dgvRecipes);
                pnlContent.Controls.Add(dgvRecipes);
                dgvRecipes.Dock = DockStyle.Fill;
            }
        }

        private void DgvRecipes_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (dgvRecipes == null) return;
                var column = dgvRecipes.Columns[e.ColumnIndex];
                var sortBy = column.DataPropertyName ?? column.Name;
                _ = Task.Run(async () =>
                {
                    await _presenter.SortBy(sortBy);
                    SafeInvokeOnUI(UpdatePaginationInfo);
                });
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi sắp xếp: {ex.Message}");
            }
        }
        #endregion

        #region Override Event Handlers Base Class

        protected override void BtnSearch_Click(object sender, EventArgs e)
        {
            PerformSearch();
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
            cbxFilter2.SelectedIndexChanged += (s, e) => ApplyProductFilter();
            cbxOrderBy.SelectedIndexChanged += (s, e) => ApplySorting();

            // Search textbox with debounce
            tbxFindString.TextChanged += async (s, e) =>
            {
                if (_model == null) return;

                if (string.IsNullOrWhiteSpace(tbxFindString.Text))
                {
                    try
                    {
                        SetLoadingState(true);
                        _model.SearchText = string.Empty;
                        await _presenter.SearchAsync(_model.SearchText);
                        UpdatePaginationInfo();
                    }
                    catch (Exception ex)
                    {
                        ShowError($"Lỗi khi reset tìm kiếm: {ex.Message}");
                    }
                    finally
                    {
                        SetLoadingState(false);
                    }

                    try { tbxFindString.Focus(); } catch { }
                    return;
                }

                await Task.Delay(300);
                try
                {
                    if (!string.IsNullOrWhiteSpace(tbxFindString.Text))
                    {
                        SetLoadingState(true);
                        await _presenter.SearchAsync(tbxFindString.Text);
                        UpdatePaginationInfo();
                    }
                }
                catch (Exception ex)
                {
                    ShowError($"Lỗi khi tìm kiếm: {ex.Message}");
                }
                finally
                {
                    SetLoadingState(false);
                }
            };
        }

        #endregion

        #region Recipe Management Specific Methods

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

        private async void ApplyProductFilter()
        {
            try
            {
                if (cbxFilter2.SelectedValue == null) return;

                SetLoadingState(true);
                if (cbxFilter2.SelectedValue is long productId)
                {
                    await _presenter.FilterByProductAsync(productId);
                    UpdatePaginationInfo();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lọc theo sản phẩm: {ex.Message}");
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
                await Task.Delay(200);
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

        private RecipeViewModel? GetSelectedRecipe()
        {
            if (dgvRecipes.SelectedRows.Count > 0)
            {
                var selectedRow = dgvRecipes.SelectedRows[0];
                return selectedRow.DataBoundItem as RecipeViewModel;
            }
            return null;
        }

        private void ApplyRecipesToModel(List<RecipeViewModel> recipes, int totalCount)
        {
            _model.Recipes.Clear();
            foreach (var recipe in recipes)
            {
                _model.Recipes.Add(recipe);
            }

            _model.TotalItems = totalCount;
            UpdatePaginationInfo();
        }

        #endregion

        #region Event Handlers

        private async void FrmRecipeManagement_Load(object? sender, EventArgs e)
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
                ShowError($"Lỗi khi tải dữ liệu công thức: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        #endregion

        #region Dialog Integration Methods

        private async void OpenRecipeDetailsDialog(RecipeViewModel? selectedRecipe = null)
        {
            try
            {
                SetLoadingState(true);

                RecipeDetailViewModel? initialModel = null;

                if (selectedRecipe != null)
                {
                    initialModel = new RecipeDetailViewModel
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
                }
                using var detailForm = new FrmRecipeDetails(selectedRecipe?.Id, initialModel);

                if (!await detailForm.CheckAuthorizationAsync())
                {
                    var warning = new FrmToastMessage(ToastType.WARNING, "Bạn không có quyền truy cập chức năng này!");
                    warning.Show();
                    detailForm.Dispose();
                    detailForm.BringToFront();
                    return;
                }

                var result = detailForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    var updatedRecipe = detailForm.Recipe;

                    if (selectedRecipe != null)
                    {
                        await HandleRecipeUpdate(updatedRecipe);
                    }
                    else
                    {
                        await HandleRecipeAdd(updatedRecipe);
                        ShowInfo("Thêm công thức mới thành công!");
                    }

                    RefreshData();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi {(selectedRecipe != null ? "cập nhật" : "thêm")} công thức: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task HandleRecipeAdd(RecipeDetailViewModel recipe)
        {
            await Task.Delay(50);
            Console.WriteLine($"Thêm công thức: {recipe.Name}, Sản phẩm: {recipe.ProductName}");
        }

        private async Task HandleRecipeUpdate(RecipeDetailViewModel recipe)
        {
            await Task.Delay(50);
            Console.WriteLine($"Lưu công thức ID {recipe.Id}: {recipe.Name}");
        }

        #endregion

        #region Override Event Handlers - Updated

        protected override void BtnAdd_ClickAsync(object sender, EventArgs e)
        {
            OpenRecipeDetailsDialog();
        }

        protected override void BtnGetDetails_Click(object sender, EventArgs e)
        {
            var selectedRecipe = GetSelectedRecipe();

            if (selectedRecipe != null)
            {
                OpenRecipeDetailsDialog(selectedRecipe);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một công thức để xem chi tiết.",
                               "Thông báo",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
        }

        private void DgvListItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRecipe = GetSelectedRecipe();
                if (selectedRecipe != null)
                {
                    OpenRecipeDetailsDialog(selectedRecipe);
                }
            }
        }

        #endregion

        #region Additional Helper Methods

        private void SetupAdditionalEvents()
        {
            if (dgvRecipes != null)
            {
                dgvRecipes.CellDoubleClick += (s, o) => DgvListItems_CellDoubleClick(s!, o);

                dgvRecipes.SelectionChanged += (s, e) =>
                {
                    btnGetDetails.Enabled = dgvRecipes.SelectedRows.Count > 0;
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
            dgvRecipes.ContextMenuStrip = contextMenu;
        }

        #endregion

        #region Thread-safe UI helpers

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
                // ignore invocation exceptions
            }
        }

        #endregion
    }
}