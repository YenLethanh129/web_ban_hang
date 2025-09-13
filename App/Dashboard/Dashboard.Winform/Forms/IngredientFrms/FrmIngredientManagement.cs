using Dashboard.Winform.Events;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms
{
    public partial class FrmIngredientManagement
        : FrmBaseManagement<IngredientManagementModel, IngredientManagementPresenter>
    {
        private readonly IngredientManagementModel _model;
        private readonly IServiceProvider _serviceProvider;

        public FrmIngredientManagement(
            IServiceProvider serviceProvider,
            ILogger<FrmIngredientManagement> logger,
            IngredientManagementPresenter ingredientManagementPresenter
        ) : base(logger, ingredientManagementPresenter)
        {
            _model = _presenter.Model;
            _serviceProvider = serviceProvider;

            InitializeBaseComponents();

            _presenter.OnDataLoaded += (s, e) =>
            {
                try
                {
                    if (e is IngredientsLoadedEventArgs args)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                try
                                {
                                    ApplyIngredientsToModel(args.Ingredients);
                                }
                                catch (Exception ex)
                                {
                                    ShowError($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
                                }
                            }));
                        }
                        else
                        {
                            ApplyIngredientsToModel(args.Ingredients);
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

            Load += FrmIngredientManagement_Load;
            OverrideTextUI();
            OverrideComboBoxItem();
            SetupDataBindings();
            SetupDgvListItem();
            FinalizeFormSetup();
        }

        /// <summary>
        /// Override để khởi tạo components riêng của Ingredient Management
        /// </summary>
        protected override void InitializeDerivedComponents()
        {
            InitializeDgvListItem();
        }

        private void OverrideTextUI()
        {
            lblFilter1.Text = "Trạng thái:";
            lblFilter2.Text = "Danh mục:";
            lblSearchString.Text = "Tìm kiếm theo (ID/tên nguyên liệu):";
            Text = "Quản lý nguyên liệu";
        }

        private void SetupDataBindings()
        {
            cbxFilter1.DataSource = _model.Statuses;
            cbxFilter1.SelectedIndex = 0;

            cbxFilter2.DataSource = _model.Categories;
            cbxFilter2.DisplayMember = "Name";
            cbxFilter2.ValueMember = "Id";

            tbxFindString.DataBindings.Add(
                "Text", _model,
                nameof(_model.SearchText),
                false, DataSourceUpdateMode.OnPropertyChanged
            );
        }

        private void OverrideComboBoxItem()
        {
            cbxOrderBy.Items.Clear();
            cbxOrderBy.Items.AddRange(["ID", "Name", "Category", "Unit", "Status", "Created Date"]);
            if (cbxOrderBy.Items.Count > 0)
                cbxOrderBy.SelectedIndex = 0;
        }

        protected void SetupDgvListItem()
        {
            if (dgvListItems == null)
            {
                throw new InvalidOperationException("dgvListItems must be initialized before calling SetupDgvListItem()");
            }

            dgvListItems.AutoGenerateColumns = false;
            dgvListItems.Columns.Clear();

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.Id),
                HeaderText = "ID",
                Width = 50,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.Name),
                HeaderText = "Tên nguyên liệu",
                Width = 200
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.CategoryName),
                HeaderText = "Danh mục",
                Width = 120
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.Unit),
                HeaderText = "Đơn vị",
                Width = 80
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.Description),
                HeaderText = "Mô tả",
                Width = 150
            });

            dgvListItems.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.IsActive),
                HeaderText = "Hoạt động",
                Width = 80
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(IngredientViewModel.CreatedAt),
                HeaderText = "Ngày tạo",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvListItems.DataSource = _model.Ingredients;
            dgvListItems.Refresh();
        }

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

        #region Ingredient Management Specific Methods

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

        private IngredientViewModel? GetSelectedIngredient()
        {
            if (dgvListItems.SelectedRows.Count > 0)
            {
                var selectedRow = dgvListItems.SelectedRows[0];
                return selectedRow.DataBoundItem as IngredientViewModel;
            }
            return null;
        }
        #endregion

        #region Event Handlers

        private async void FrmIngredientManagement_Load(object? sender, EventArgs e)
        {
            _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                SetLoadingState(true);
                await _presenter.LoadDataAsync(page: _model.CurrentPage, pageSize: _model.PageSize);
                UpdatePaginationInfo();
                _dataLoadingCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                _dataLoadingCompletionSource.SetException(ex);
                ShowError($"Lỗi khi tải dữ liệu nguyên liệu: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void ApplyIngredientsToModel(List<IngredientViewModel> ingredients)
        {
            _model.Ingredients.Clear();
            foreach (var ingredient in ingredients)
            {
                _model.Ingredients.Add(ingredient);
            }

            UpdatePaginationInfo();
        }

        #endregion

        #region Context Menu for Refresh

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Làm mới dữ liệu", null, (s, e) => RefreshData());
            dgvListItems.ContextMenuStrip = contextMenu;
        }

        #endregion

        #region Dialog Integration Methods

        private async void OpenIngredientDetailsDialog(IngredientViewModel? selectedIngredient = null)
        {
            try
            {
                SetLoadingState(true);
                await Task.Delay(50);
                // TODO: Create IngredientDetailsPresenter when backend services are ready
                // var detailsPresenter = new IngredientDetailsPresenter(
                //     _serviceProvider.GetRequiredService<IIngredientManagementService>(),
                //     _serviceProvider.GetRequiredService<IIngredientCategoryService>(),
                //     _serviceProvider.GetRequiredService<ITaxService>(),
                //     _serviceProvider.GetRequiredService<IMapper>()
                // );

                long? ingredientId = selectedIngredient?.Id;
                IngredientDetailViewModel? initialModel = null;

                if (selectedIngredient != null)
                {
                    initialModel = new IngredientDetailViewModel
                    {
                        Id = selectedIngredient.Id,
                        Name = selectedIngredient.Name,
                        Unit = selectedIngredient.Unit,
                        CategoryId = selectedIngredient.CategoryId,
                        CategoryName = selectedIngredient.CategoryName,
                        Description = selectedIngredient.Description,
                        IsActive = selectedIngredient.IsActive,
                        TaxId = selectedIngredient.TaxId,
                        CreatedAt = selectedIngredient.CreatedAt,
                        UpdatedAt = selectedIngredient.UpdatedAt
                    };
                }

                // TODO: Create FrmIngredientDetails form
                // var detailForm = new FrmIngredientDetails(detailsPresenter, selectedIngredient?.Id, initialModel);

                // For now, show a placeholder dialog
                MessageBox.Show(selectedIngredient != null ?
                    $"Sẽ mở dialog chi tiết cho nguyên liệu: {selectedIngredient.Name}" :
                    "Sẽ mở dialog thêm nguyên liệu mới",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // TODO: Implement when detail form is created
                // var result = detailForm.ShowDialog(this);
                // if (result == DialogResult.OK)
                // {
                //     var updatedIngredient = detailForm.Ingredient;
                //     if (selectedIngredient != null)
                //     {
                //         await HandleIngredientUpdate(updatedIngredient);
                //         ShowInfo("Cập nhật thông tin nguyên liệu thành công!");
                //     }
                //     else
                //     {
                //         await HandleIngredientAdd(updatedIngredient);
                //         ShowInfo("Thêm nguyên liệu mới thành công!");
                //     }
                //     RefreshData();
                // }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi {(selectedIngredient != null ? "cập nhật" : "thêm")} nguyên liệu: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task HandleIngredientAdd(IngredientDetailViewModel ingredient)
        {
            await _presenter.AddIngredientAsync(
                ingredient.Name,
                ingredient.Unit,
                ingredient.CategoryId,
                ingredient.Description,
                ingredient.IsActive,
                ingredient.TaxId
            );

            var logInfo = $"Thêm nguyên liệu: {ingredient.Name}, Đơn vị: {ingredient.Unit}, Danh mục: {ingredient.CategoryName}";
            Console.WriteLine(logInfo);
        }

        private async Task HandleIngredientUpdate(IngredientDetailViewModel ingredient)
        {
            await _presenter.UpdateIngredientAsync(
                ingredient.Id,
                ingredient.Name,
                ingredient.Unit,
                ingredient.CategoryId,
                ingredient.Description,
                ingredient.IsActive,
                ingredient.TaxId
            );

            var logInfo = $"Cập nhật nguyên liệu ID {ingredient.Id}: {ingredient.Name}, Đơn vị: {ingredient.Unit}";
            Console.WriteLine(logInfo);
        }

        #endregion

        #region Override Event Handlers - Updated

        protected override void BtnAdd_Click(object sender, EventArgs e)
        {
            OpenIngredientDetailsDialog();
        }

        protected override void BtnGetDetails_Click(object sender, EventArgs e)
        {
            var selectedIngredient = GetSelectedIngredient();

            if (selectedIngredient != null)
            {
                OpenIngredientDetailsDialog(selectedIngredient);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một nguyên liệu để xem chi tiết.",
                               "Thông báo",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
        }

        private void DgvListItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedIngredient = GetSelectedIngredient();
                if (selectedIngredient != null)
                {
                    OpenIngredientDetailsDialog(selectedIngredient);
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
            if (dgvListItems != null)
            {
                dgvListItems.CellDoubleClick += (s, o) => DgvListItems_CellDoubleClick(s!, o);

                dgvListItems.SelectionChanged += (s, e) =>
                {
                    btnGetDetails.Enabled = dgvListItems.SelectedRows.Count > 0;
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
    }
}