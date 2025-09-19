using AutoMapper;
using Dashboard.BussinessLogic.Services.BranchServices;
using Dashboard.BussinessLogic.Services.EmployeeServices;
using Dashboard.BussinessLogic.Services.RBACServices;
using Dashboard.Winform.Events;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels.EmployeeModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms
{
    public partial class FrmEmployeeManagement
        : FrmBaseManagement<EmployeeManagementModel, EmployeeManagementPresenter>
    {
        private readonly EmployeeManagementModel _model;
        private readonly IServiceProvider _serviceProvider;
        public FrmEmployeeManagement(
            IServiceProvider serviceProvider,
            ILogger<FrmEmployeeManagement> logger,
            EmployeeManagementPresenter employeeManagementPresenter
        ) : base(logger, employeeManagementPresenter)
        {
            _model = _presenter.Model;
            _serviceProvider = serviceProvider;

            InitializeBaseComponents();

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
            SetupDgvListItem();
            FinalizeFormSetup();
            SetupContextMenu();
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

            cbxFilter2.DataSource = _model.Positions;

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

            dgvListItems.AutoGenerateColumns = false;
            dgvListItems.Columns.Clear();

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeViewModel.Id),
                HeaderText = "ID",
                Width = 20,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });
            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeViewModel.BranchId),
                HeaderText = "Mã chi nhánh",
                Width = 50,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeViewModel.FullName),
                HeaderText = "Họ tên",
                Width = 200
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeViewModel.PositionName),
                HeaderText = "Chức vụ",
                Width = 150
            });

            dgvListItems.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeViewModel.PhoneNumber),
                HeaderText = "Số điện thoại",
                Width = 130
            });

            dgvListItems.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(EmployeeViewModel.IsActive),
                HeaderText = "Hoạt động",
                Width = 80
            });

            dgvListItems.DataSource = _model.Employees;
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
            cbxFilter2.SelectedIndexChanged += (s, e) => ApplyPositionFilter();
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

        #region Employee Management Specific Methods

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

        private void OpenAddEmployeeDialog()
        {
            try
            {

                MessageBox.Show("Chức năng thêm nhân viên sẽ được triển khai",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi mở form thêm nhân viên: {ex.Message}");
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

        private async void ApplyPositionFilter()
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

        private EmployeeViewModel? GetSelectedEmployee()
        {
            if (dgvListItems.SelectedRows.Count > 0)
            {
                var selectedRow = dgvListItems.SelectedRows[0];
                return selectedRow.DataBoundItem as EmployeeViewModel;
            }
            return null;
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
                UpdatePaginationInfo();
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
            _model.Employees.Clear();
            foreach (var emp in employees)
            {
                _model.Employees.Add(emp);
            }

            UpdatePaginationInfo();
        }

        #endregion

        #region Context Menu for Refresh

        private void SetupContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            contextMenu.Items.Add("Làm mới dữ liệu", null, (s, e) => _ = Task.Run(async () => { await Task.Delay(100); if (InvokeRequired) { Invoke(new Action(async () => await RefreshDataSafely())); } else { await RefreshDataSafely(); } }));
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
                    // Again I just dunno why auto mapper not work here
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
                MessageBox.Show("Vui lòng chọn một nhân viên để xem chi tiết.",
                               "Thông báo",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Information);
            }
        }

        private void DgvListItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
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