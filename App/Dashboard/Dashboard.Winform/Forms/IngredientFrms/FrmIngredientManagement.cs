using Dashboard.BussinessLogic.Services.GoodsAndStockServcies;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Common.Constants;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Presenters.IngredientPresenters;

namespace Dashboard.Winform.Forms
{
    public partial class FrmIngredientManagement
        : FrmBaseManagement<IngredientManagementModel, IngredientManagementPresenter>
    {
        private readonly IngredientManagementModel _model;
        private readonly IServiceProvider _serviceProvider;
        private System.Windows.Forms.Timer? _searchTimer;
        private bool _isInitialized = false;
        private bool _isLoading = false;

        public FrmIngredientManagement(
            IServiceProvider serviceProvider,
            ILogger<FrmIngredientManagement> logger,
            IngredientManagementPresenter ingredientManagementPresenter
        ) : base(logger, ingredientManagementPresenter)
        {
            _model = _presenter.Model;
            _serviceProvider = serviceProvider;

            InitializeBaseComponents();
            InitializeSearchTimer();

            _presenter.OnDataLoaded += (s, e) =>
            {
                try
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            try
                            {
                                UpdateDataGridViewSafely();
                            }
                            catch (Exception ex)
                            {
                                ShowError($"Lỗi khi cập nhật giao diện: {ex.Message}");
                            }
                        }));
                    }
                    else
                    {
                        UpdateDataGridViewSafely();
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

            if (dgvListItems != null)
            {
                SetupDgvListItem();
            }

            FinalizeFormSetup();
            SetupContextMenu();
            SetupAdditionalEvents();

            // no timer disposal needed
        }


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

        private void InitializeSearchTimer()
        {
            _searchTimer = new System.Windows.Forms.Timer
            {
                Interval = 300
            };
            _searchTimer.Tick += SearchTimer_Tick;
        }


        private void SetupDataBindings()
        {
            cbxFilter1.DataSource = _model.Statuses;
            cbxFilter1.SelectedIndex = 0;

            cbxFilter2.DataSource = _model.Categories;
            cbxFilter2.DisplayMember = "Name";
            cbxFilter2.ValueMember = "Id";

            // Fix: bind textbox to model.SearchText so the presenter can read the latest value if needed
            tbxFindString.DataBindings.Clear();
            tbxFindString.DataBindings.Add("Text", _model, nameof(IngredientManagementModel.SearchText), false, DataSourceUpdateMode.OnPropertyChanged);
        }

        private void OverrideComboBoxItem()
        {
            cbxOrderBy.Items.Clear();
            // Fix: proper AddRange syntax for C#
            cbxOrderBy.Items.AddRange(new object[] { "ID", "Name", "Category", "Unit", "Status", "Created Date" });
            if (cbxOrderBy.Items.Count > 0)
                cbxOrderBy.SelectedIndex = 0;
        }

        protected void SetupDgvListItem()
        {
            if (dgvListItems == null)
            {
                throw new InvalidOperationException("dgvListItems must be initialized before calling SetupDgvListItem()");
            }

            dgvListItems.ColumnHeaderMouseClick -= DgvIngredient_ColumnHeaderMouseClick;
            dgvListItems.CellDoubleClick -= DgvListItems_CellDoubleClick;
            dgvListItems.SelectionChanged -= DgvListItems_SelectionChanged;
            dgvListItems.DataError -= DgvListItems_DataError;

            dgvListItems.AutoGenerateColumns = false;
            dgvListItems.Columns.Clear();

            dgvListItems.DataSource = null;

            try
            {
                dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Id",
                    DataPropertyName = "Id",
                    HeaderText = "ID",
                    Width = 50,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                });

                dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Name",
                    DataPropertyName = "Name",
                    HeaderText = "Tên nguyên liệu",
                    Width = 200
                });

                dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CategoryName",
                    DataPropertyName = "CategoryName",
                    HeaderText = "Danh mục",
                    Width = 120
                });

                dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Unit",
                    DataPropertyName = "Unit",
                    HeaderText = "Đơn vị",
                    Width = 80
                });

                dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "Description",
                    DataPropertyName = "Description",
                    HeaderText = "Mô tả",
                    Width = 150
                });

                dgvListItems.Columns.Add(new DataGridViewCheckBoxColumn
                {
                    Name = "IsActive",
                    DataPropertyName = "IsActive",
                    HeaderText = "Hoạt động",
                    Width = 80
                });

                dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "CreatedAt",
                    DataPropertyName = "CreatedAt",
                    HeaderText = "Ngày tạo",
                    Width = 100,
                    DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
                });

                dgvListItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvListItems.MultiSelect = false;

                if (_model?.Ingredients != null)
                {
                    dgvListItems.DataSource = _model.Ingredients;
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi thiết lập DataGridView: {ex.Message}");
                return;
            }

            dgvListItems.ColumnHeaderMouseClick += DgvIngredient_ColumnHeaderMouseClick;
            dgvListItems.CellDoubleClick += DgvListItems_CellDoubleClick;
            dgvListItems.SelectionChanged += DgvListItems_SelectionChanged;
            dgvListItems.DataError += DgvListItems_DataError;

            dgvListItems.Refresh();
        }


        private void UpdateDataGridViewSafely()
        {
            if (dgvListItems == null || _model?.Ingredients == null)
                return;

            try
            {
                dgvListItems.SuspendLayout();

                dgvListItems.DataSource = null;
                dgvListItems.DataSource = _model.Ingredients;

                UpdatePaginationInfo();
            }
            finally
            {
                dgvListItems.ResumeLayout();
            }
        }

        private async void DgvIngredient_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (dgvListItems == null || e.ColumnIndex < 0 || e.ColumnIndex >= dgvListItems.Columns.Count)
                    return;

                string sortBy = e.ColumnIndex switch
                {
                    0 => "id",
                    1 => "name",
                    2 => "category",
                    3 => "unit",
                    4 => "description",
                    5 => "isactive",
                    6 => "createdat",
                    _ => "id"
                };

                SetLoadingState(true);
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

        private async void SearchTimer_Tick(object? sender, EventArgs e)
        {
            _searchTimer?.Stop();

            if (_isLoading) return;

            var focusState = new
            {
                HasFocus = tbxFindString?.Focused ?? false,
                CursorPosition = tbxFindString?.SelectionStart ?? 0,
                Text = tbxFindString?.Text ?? string.Empty
            };

            try
            {
                _isLoading = true;
                var searchText = focusState.Text.Trim();

                await _presenter.SearchAsync(searchText);


                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        UpdatePaginationInfo();
                        RestoreFocusAndCursor(focusState.HasFocus, focusState.CursorPosition);
                    }));
                }
                else
                {
                    UpdatePaginationInfo();
                    RestoreFocusAndCursor(focusState.HasFocus, focusState.CursorPosition);
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    BeginInvoke(new Action(() =>
                    {
                        ShowError($"Lỗi khi tìm kiếm: {ex.Message}");
                        RestoreFocusAndCursor(focusState.HasFocus, focusState.CursorPosition);
                    }));
                }
                else
                {
                    ShowError($"Lỗi khi tìm kiếm: {ex.Message}");
                    RestoreFocusAndCursor(focusState.HasFocus, focusState.CursorPosition);
                }
            }
            finally
            {
                _isLoading = false;
            }
        }

        private void RestoreFocusAndCursor(bool hadFocus, int cursorPosition)
        {
            if (!hadFocus || tbxFindString == null || tbxFindString.IsDisposed)
                return;

            BeginInvoke((MethodInvoker)(() =>
            {
                try
                {
                    tbxFindString.Focus();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Focus restore error: {ex.Message}");
                }
            }));

            BeginInvoke((MethodInvoker)(() =>
            {
                try
                {
                    if (cursorPosition <= tbxFindString.Text.Length)
                    {
                        tbxFindString.SelectionStart = cursorPosition;
                        tbxFindString.SelectionLength = 0;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Cursor restore error: {ex.Message}");
                }
            }));
        }



        #region Override Event Handlers Base Class

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

            if (tbxFindString != null)
            {
                tbxFindString.TextChanged -= TbxFindString_TextChanged_Handler;
                tbxFindString.TextChanged += TbxFindString_TextChanged_Handler;
            }


            cbxFilter1.SelectedIndexChanged += async (s, e) =>
            {
                if (_isInitialized) await ApplyStatusFilter();
            };

            cbxFilter2.SelectedIndexChanged += async (s, e) =>
            {
                if (_isInitialized) await ApplyCategoryFilter();
            };

            cbxOrderBy.SelectedIndexChanged += async (s, e) =>
            {
                if (_isInitialized) await ApplySorting();
            };
        }

        private async void TbxFindString_TextChanged_Handler(object? sender, EventArgs e)
        {
            await TbxFindString_TextChanged(sender, e);
        }

        protected override Task TbxFindString_TextChanged(object? sender, EventArgs e)
        {
            if (!_isInitialized)
                return Task.CompletedTask;
            _searchTimer?.Stop();
            _searchTimer?.Start();
            return Task.CompletedTask;
        }


        #endregion

        #region Ingredient Management Specific Methods
        private async Task ApplyStatusFilter()
        {
            if (_isLoading) return;

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
                _isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task ApplyCategoryFilter()
        {
            if (_isLoading) return;

            try
            {
                if (cbxFilter2.SelectedValue == null) return;

                SetLoadingState(true);
                if (cbxFilter2.SelectedValue is long categoryId)
                {
                    await _presenter.FilterByCategoryAsync(categoryId);
                    UpdatePaginationInfo();
                }
                else if (long.TryParse(cbxFilter2.SelectedValue.ToString(), out var parsed))
                {
                    await _presenter.FilterByCategoryAsync(parsed);
                    UpdatePaginationInfo();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lọc theo danh mục: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                SetLoadingState(false);
            }
        }

        private async Task ApplySorting()
        {
            if (_isLoading) return;

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
                _isLoading = false;
                SetLoadingState(false);
            }
        }

        private async void GoToNextPage()
        {
            if (_isLoading) return;

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
                _isLoading = false;
                SetLoadingState(false);
            }
        }

        private async void GoToPreviousPage()
        {
            if (_isLoading) return;

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
                _isLoading = false;
                SetLoadingState(false);
            }
        }

        private async void UpdatePageSize()
        {
            if (_isLoading) return;
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
                _isLoading = false;
                SetLoadingState(false);
            }
        }

        private async void RefreshData()
        {
            if (_isLoading) return;
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
                _isLoading = false;
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
            new FrmToastMessage(ToastType.ERROR, message).Show();
        }

        private void ShowInfo(string message)
        {                                                                                      
            new FrmToastMessage(ToastType.INFO, message).Show();
        }

        private bool AreControlsInitialized()
        {
            return dgvListItems != null &&
                   btnGetDetails != null &&
                   btnAdd != null &&
                   cbxFilter1 != null &&
                   cbxFilter2 != null &&
                   cbxOrderBy != null &&
                   cbxNumbRecordsPerPage != null &&
                   tbxFindString != null;
        }

        private void SetLoadingState(bool isLoading)
        {
            if (!AreControlsInitialized())
            {
                this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
                return;
            }

            dgvListItems.Enabled = !isLoading;
            btnGetDetails.Enabled = !isLoading && dgvListItems.SelectedRows.Count > 0;
            btnAdd.Enabled = !isLoading;
            cbxFilter1.Enabled = !isLoading;
            cbxFilter2.Enabled = !isLoading;
            cbxOrderBy.Enabled = !isLoading;
            cbxNumbRecordsPerPage.Enabled = !isLoading;

            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private IngredientViewModel? GetSelectedIngredient()
        {
            try
            {
                if (dgvListItems.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvListItems.SelectedRows[0];
                    return selectedRow.DataBoundItem as IngredientViewModel;
                }
                return null;
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lấy nguyên liệu được chọn: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Event Handlers

        private async void FrmIngredientManagement_Load(object? sender, EventArgs e)
        {
            if (_isLoading) return;
            _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                _isLoading = true;
                SetLoadingState(true);
                await _presenter.LoadDataAsync(page: _model.CurrentPage, pageSize: _model.PageSize, forceRefresh: true);
                UpdatePaginationInfo();
                _isInitialized = true;
                _dataLoadingCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                _dataLoadingCompletionSource.SetException(ex);
                ShowError($"Lỗi khi tải dữ liệu nguyên liệu: {ex.Message}");
            }
            finally
            {
                _isLoading = false;
                SetLoadingState(false);
            }
        }

        private void ApplyIngredientsToModel(List<IngredientViewModel> ingredients)
        {
            try
            {
                _model.Ingredients.Clear();
                foreach (var ingredient in ingredients)
                {
                    _model.Ingredients.Add(ingredient);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi cập nhật danh sách nguyên liệu: {ex.Message}");
            }
        }

        #endregion

        #region Context Menu for Refresh

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Làm mới dữ liệu", null, (s, e) => RefreshData());
            contextMenu.Items.Add("Xem chi tiết", null, (s, e) =>
            {
                var selectedIngredient = GetSelectedIngredient();
                if (selectedIngredient != null)
                {
                    OpenIngredientDetailsDialog(selectedIngredient);
                }
                else
                {
                    ShowInfo("Vui lòng chọn một nguyên liệu để xem chi tiết.");
                }
            });
            dgvListItems.ContextMenuStrip = contextMenu;
        }

        #endregion

        #region Dialog Integration Methods

        private void OpenIngredientDetailsDialog(IngredientViewModel? selectedIngredient = null)
        {
            try
            {
                SetLoadingState(true);

                var presenter = _serviceProvider.GetRequiredService<IIngredientDetailPresenter>();

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

                var detailForm = new FrmIngredientDetails(presenter, selectedIngredient?.Id, initialModel);
                var result = detailForm.ShowDialog(this);

                if (result == DialogResult.OK)
                {
                    _ = Task.Run(async () =>
                    {
                        await Task.Delay(300);

                        if (InvokeRequired)
                        {
                            Invoke(new Action(async () => await RefreshDataSafely()));
                        }
                        else
                        {
                            await RefreshDataSafely();
                        }
                    });
                }
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

        private async Task RefreshDataSafely()
        {
            if (_isLoading) return;

            try
            {
                SetLoadingState(true);
                await _presenter.RefreshCacheAsync();
                UpdatePaginationInfo();
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi làm mới dữ liệu: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
                _isLoading = false;
            }
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
                new FrmToastMessage(ToastType.INFO, "Vui lòng chọn một nguyên liệu để xem chi tiết.").Show();
            }
        }

        private void DgvListItems_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
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

        private void DgvListItems_SelectionChanged(object? sender, EventArgs e)
        {
            if (btnGetDetails != null)
                btnGetDetails.Enabled = dgvListItems.SelectedRows.Count > 0;
        }

        private void DgvListItems_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true; // Prevent the default error dialog

            Console.WriteLine($"DataGridView Error - Column: {e.ColumnIndex}, Row: {e.RowIndex}, Exception: {e.Exception?.Message}");

            if (e.Exception != null)
            {
                ShowError($"Lỗi hiển thị dữ liệu tại dòng {e.RowIndex + 1}: {e.Exception.Message}");
            }
        }

        #endregion

        #region Additional Helper Methods

        private void SetupAdditionalEvents()
        {
            if (dgvListItems != null)
            {
                if (btnGetDetails != null)
                    btnGetDetails.Enabled = false;
            }
        }

        private void FinalizeFormSetup()
        {
            SetupAdditionalEvents();
        }

        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _searchTimer?.Stop();
                _searchTimer?.Dispose();
                _searchTimer = null;
            }
            base.Dispose(disposing);
        }
    }
}