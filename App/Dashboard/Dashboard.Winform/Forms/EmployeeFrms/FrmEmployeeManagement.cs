using AutoMapper;
using Dashboard.BussinessLogic.Services.BranchServices;
using Dashboard.BussinessLogic.Services.EmployeeServices;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.Winform.Events;
using Dashboard.Winform.ViewModels.EmployeeModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Common.Constants;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Presenters.EmployeePresenter;

namespace Dashboard.Winform.Forms
{
    public partial class FrmEmployeeManagement
        : FrmBaseManagement<EmployeeManagementModel, EmployeeManagementPresenter>
    {
        private readonly EmployeeManagementModel _model;
        private readonly IServiceProvider _serviceProvider;
        private bool _isInitialized = false;
        private System.Windows.Forms.Timer? _searchTimer;

        public FrmEmployeeManagement(
            IServiceProvider serviceProvider,
            ILogger<FrmEmployeeManagement> logger,
            EmployeeManagementPresenter employeeManagementPresenter
        ) : base(logger, employeeManagementPresenter)
        {
            _model = _presenter.Model;
            _serviceProvider = serviceProvider;

            InitializeBaseComponents();
                InitializeSearchTimer(); 
            _presenter.OnDataLoaded += (s, e) =>
            {
                try
                {
                    if (e is EmployeesLoadedEventArgs args)
                    {
                        if (InvokeRequired)
                        {
                            Invoke(new Action(() =>
                            {
                                try
                                {
                                    ApplyEmployeesToModel(args.Employees);
                                    SetupDgvListItem();
                                    UpdatePaginationInfo();
                                }
                                catch (Exception ex)
                                {
                                    ShowError($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
                                }
                            }));
                        }
                        else
                        {
                            ApplyEmployeesToModel(args.Employees);
                            SetupDgvListItem();
                            UpdatePaginationInfo();
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

            Load += FrmEmployeeManagement_Load;
            OverrideTextUI();
            OverrideComboBoxItem();
            SetupDataBindings();

            if (dgvListItems != null)
            {
                SetupDgvListItem();
            }

            FinalizeFormSetup();
            SetupContextMenu();

            // no timer disposal needed
        }


        /// <summary>
        /// Override để khởi tạo components riêng của Employee Management
        /// </summary>
        protected override void InitializeDerivedComponents()
        {
            InitializeDgvListItem();
        }

        private void OverrideTextUI()
        {
            lblFilter1.Text = "Trạng thái:";
            lblFilter2.Text = "Vai trò:";
            lblSearchString.Text = "Tìm kiếm theo (ID/tên nhân viên):";
            Text = "Quản lý nhân viên";
        }

        private void SetupDataBindings()
        {
            cbxFilter1.DataSource = _model.Statuses;
            cbxFilter1.SelectedIndex = 0;

            cbxFilter2.DisplayMember = "Name";
            cbxFilter2.ValueMember = "Id";

            tbxFindString.DataBindings.Clear();
        }

        private void OverrideComboBoxItem()
        {
            cbxOrderBy.Items.Clear();
            cbxOrderBy.Items.AddRange(["ID", "FullName", "position", "hiredate"]);
            if (cbxOrderBy.Items.Count > 0)
                cbxOrderBy.SelectedIndex = 0;
        }

        protected void SetupDgvListItem()
        {
            if (dgvListItems == null)
            {
                throw new InvalidOperationException("dgvListItems must be initialized before calling SetupDgvListItem()");
            }

            dgvListItems.ColumnHeaderMouseClick -= DgvListItems_ColumnHeaderMouseClick;
            dgvListItems.CellDoubleClick -= DgvListItems_CellDoubleClick;
            dgvListItems.SelectionChanged -= DgvListItems_SelectionChanged;
            dgvListItems.DataError -= DgvListItems_DataError;

            dgvListItems.AutoGenerateColumns = false;
            dgvListItems.Columns.Clear();

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "Id",
                DataPropertyName = nameof(EmployeeViewModel.Id),
                HeaderText = "ID",
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "BranchId",
                DataPropertyName = nameof(EmployeeViewModel.BranchId),
                HeaderText = "Mã chi nhánh",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "FullName",
                DataPropertyName = nameof(EmployeeViewModel.FullName),
                HeaderText = "Họ tên",
                Width = 200
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PositionName",
                DataPropertyName = nameof(EmployeeViewModel.PositionName),
                HeaderText = "Chức vụ",
                Width = 150
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                Name = "PhoneNumber",
                DataPropertyName = nameof(EmployeeViewModel.PhoneNumber),
                HeaderText = "Số điện thoại",
                Width = 130
            });

            dgvListItems.Columns.Add(new DataGridViewCheckBoxColumn
            {
                Name = "IsActive",
                DataPropertyName = nameof(EmployeeViewModel.IsActive),
                HeaderText = "Hoạt động",
                Width = 80
            });

            // Set DataSource
            dgvListItems.DataSource = _model.Employees;

            dgvListItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListItems.MultiSelect = false;

            dgvListItems.ColumnHeaderMouseClick += DgvListItems_ColumnHeaderMouseClick;
            dgvListItems.CellDoubleClick += DgvListItems_CellDoubleClick;
            dgvListItems.SelectionChanged += DgvListItems_SelectionChanged;
            dgvListItems.DataError += DgvListItems_DataError;

            dgvListItems.Refresh();
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
                if (_isInitialized) await ApplyPositionFilter();
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

        private void InitializeSearchTimer()
        {
            _searchTimer = new System.Windows.Forms.Timer
            {
                Interval = 300
            };
            _searchTimer.Tick += SearchTimer_Tick;
        }

        private async void SearchTimer_Tick(object? sender, EventArgs e)
        {
            _searchTimer?.Stop();

            var focusState = new
            {
                HasFocus = tbxFindString?.Focused ?? false,
                CursorPosition = tbxFindString?.SelectionStart ?? 0,
                Text = tbxFindString?.Text ?? string.Empty
            };

            try
            {
                var searchText = focusState.Text.Trim();

                await Task.Run(async () =>
                {
                    await _presenter.SearchAsync(searchText);
                });

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
        }

        private void RestoreFocusAndCursor(bool hadFocus, int cursorPosition)
        {
            if (hadFocus && tbxFindString != null && !tbxFindString.IsDisposed)
            {
                BeginInvoke(new Action(() =>
                {
                    try
                    {
                        tbxFindString.Focus();
                        if (cursorPosition <= tbxFindString.Text.Length)
                        {
                            tbxFindString.SelectionStart = cursorPosition;
                            tbxFindString.SelectionLength = 0;
                        }
                    }
                    catch (Exception ex)
                    {
                        new FrmToastMessage(ToastType.ERROR, $"Lỗi khi khôi phục con trỏ: {ex.Message}").Show();
                    }
                }));
            }
        }




        #endregion

        #region Employee Management Specific Methods

        private async Task ApplyStatusFilter()
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

        private async Task ApplyPositionFilter()
        {
            try
            {
                if (cbxFilter2.SelectedValue == null) return;

                SetLoadingState(true);
                if (cbxFilter2.SelectedValue is long positionId)
                {
                    await _presenter.FilterByPositionAsync(positionId);
                    UpdatePaginationInfo();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lọc theo vai trò: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task ApplySorting()
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
            // Early return if controls not initialized yet
            if (!AreControlsInitialized())
            {
                this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
                return;
            }

            // Safe to access all controls now
            dgvListItems.Enabled = !isLoading;
            btnGetDetails.Enabled = !isLoading && dgvListItems.SelectedRows.Count > 0;
            btnAdd.Enabled = !isLoading;
            cbxFilter1.Enabled = !isLoading;
            cbxFilter2.Enabled = !isLoading;
            cbxOrderBy.Enabled = !isLoading;
            cbxNumbRecordsPerPage.Enabled = !isLoading;
            tbxFindString.Enabled = !isLoading;

            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private EmployeeViewModel? GetSelectedEmployee()
        {
            try
            {
                if (dgvListItems.SelectedRows.Count > 0)
                {
                    var selectedRow = dgvListItems.SelectedRows[0];
                    return selectedRow.DataBoundItem as EmployeeViewModel;
                }
                return null;
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi lấy nhân viên được chọn: {ex.Message}");
                return null;
            }
        }

        #endregion

        #region Event Handlers

        private async void FrmEmployeeManagement_Load(object? sender, EventArgs e)
        {
            _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
            try
            {
                SetLoadingState(true);
                await _presenter.LoadDataAsync(page: _model.CurrentPage, pageSize: _model.PageSize);

                if (_model.Positions?.Count > 0 && cbxFilter2.DataSource == null)
                {
                    cbxFilter2.DataSource = _model.Positions;
                    if (cbxFilter2.Items.Count > 0)
                        cbxFilter2.SelectedIndex = 0;
                }

                UpdatePaginationInfo();
                _isInitialized = true;
                _dataLoadingCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                _dataLoadingCompletionSource.SetException(ex);
                ShowError($"Lỗi khi tải dữ liệu nhân viên: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void ApplyEmployeesToModel(List<EmployeeViewModel> employees)
        {
            try
            {
                _model.Employees.Clear();
                foreach (var emp in employees)
                {
                    _model.Employees.Add(emp);
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi cập nhật danh sách nhân viên: {ex.Message}");
            }
        }

        #endregion

        #region Context Menu for Refresh

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Làm mới dữ liệu", null, (s, e) => _ = Task.Run(async () =>
            {
                await Task.Delay(100);
                if (InvokeRequired)
                {
                    Invoke(new Action(async () => await RefreshDataSafely()));
                }
                else
                {
                    await RefreshDataSafely();
                }
            }));

            contextMenu.Items.Add("Xem chi tiết", null, (s, e) =>
            {
                var selectedEmployee = GetSelectedEmployee();
                if (selectedEmployee != null)
                {
                    OpenEmployeeDetailsDialog(selectedEmployee);
                }
                else
                {
                    ShowInfo("Vui lòng chọn một nhân viên để xem chi tiết.");
                }
            });

            dgvListItems.ContextMenuStrip = contextMenu;
        }

        #endregion

        #region Dialog Integration Methods

        private void OpenEmployeeDetailsDialog(EmployeeViewModel? selectedEmployee = null)
        {
            try
            {
                var presenter = _serviceProvider.GetRequiredService<IEmployeeDetailsPresenter>();

                long? employeeId = selectedEmployee?.Id;
                EmployeeDetailViewModel? employeeDetail = null;

                if (selectedEmployee != null)
                {
                    employeeDetail = new EmployeeDetailViewModel
                    {
                        Id = selectedEmployee.Id,
                        FullName = selectedEmployee.FullName,
                        Phone = selectedEmployee.PhoneNumber,
                        Email = selectedEmployee.Email,
                        HireDate = selectedEmployee.HireDate,
                        PositionId = selectedEmployee.PositionId,
                        BranchId = selectedEmployee.BranchId,
                        Status = selectedEmployee.IsActive ? "Active" : "Inactive"
                    };
                }

                using var form = new FrmEmployeeDetails(presenter, employeeId, employeeDetail);
                var result = form.ShowDialog();

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
                ShowError($"Lỗi khi mở form chi tiết nhân viên: {ex.Message}");
            }
        }

        private async Task RefreshDataSafely()
        {
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
            }
        }

        #endregion

        #region Override Event Handlers - Updated

        protected override void BtnAdd_Click(object sender, EventArgs e)
        {
            OpenEmployeeDetailsDialog();
        }

        protected override void BtnGetDetails_Click(object sender, EventArgs e)
        {
            var selectedEmployee = GetSelectedEmployee();

            if (selectedEmployee != null)
            {
                OpenEmployeeDetailsDialog(selectedEmployee);
            }
            else
            {
                new FrmToastMessage(ToastType.INFO, "Vui lòng chọn một nhân viên để xem chi tiết.").Show();
            }
        }

        // Fixed event handlers
        private void DgvListItems_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedEmployee = GetSelectedEmployee();
                if (selectedEmployee != null)
                {
                    OpenEmployeeDetailsDialog(selectedEmployee);
                }
            }
        }

        private void DgvListItems_SelectionChanged(object? sender, EventArgs e)
        {
            btnGetDetails.Enabled = dgvListItems.SelectedRows.Count > 0;
        }

        // Fixed column header click - similar to UserManagement pattern
        private async void DgvListItems_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (dgvListItems == null || e.ColumnIndex < 0 || e.ColumnIndex >= dgvListItems.Columns.Count)
                    return;

                var column = dgvListItems.Columns[e.ColumnIndex];

                // Map column index to sort field like in UserManagement
                string sortBy = e.ColumnIndex switch
                {
                    0 => "Id",
                    1 => "BranchId",
                    2 => "FullName",
                    3 => "PositionName",
                    4 => "PhoneNumber",
                    5 => "IsActive",
                    _ => "Id"
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

        private void DgvListItems_DataError(object? sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;

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