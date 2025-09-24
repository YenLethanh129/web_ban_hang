using Dashboard.Winform.Presenters;
using Dashboard.Winform.Helpers;
using Dashboard.Winform.ViewModels.RBACModels;
using Dashboard.Winform.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Winform.Forms.BaseFrm;
using Dashboard.Winform.Attributes;

namespace Dashboard.Winform.Forms;

//[RequireRole("ADMIN")]
public partial class FrmUserManagement : Form
{
    private readonly ILogger<FrmUserManagement> _logger;
    private readonly IUserManagementPresenter _presenter;
    private readonly UserManagementModel _model;
    private readonly IServiceProvider _serviceProvider;
    private TaskCompletionSource<bool>? _dataLoadingCompletionSource;
    private System.Windows.Forms.Timer? _searchTimer;
    private bool _isInitialized = false;
    private BindingList<EmployeeSimpleViewModel> _allEmployees = new();
    private BindingList<EmployeeSimpleViewModel> _filteredEmployees = new();
    private bool _isUpdatingSelection = false;

    public FrmUserManagement(
        ILogger<FrmUserManagement> logger,
        IUserManagementPresenter presenter,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _presenter = presenter;
        _model = (UserManagementModel)_presenter.Model;
        _serviceProvider = serviceProvider;

        InitializeComponent();
        InitializeEmployeeControls();
        TabControlHelper.SetupDarkTheme(tabControl);

        OverrideTextUI();
        SetupDataBindings();
        SetupDgvUsers();
        SetupEvents();
        InitializeSearchTimer();

        _presenter.OnDataLoaded += (s, e) =>
        {
            try
            {
                if (e is UsersLoadedEventArgs args)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            try
                            {
                                UpdateUsersDataGridOnly(args.Users);
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
                        UpdateUsersDataGridOnly(args.Users);
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

        Load += FrmUserManagement_Load;

        FormClosed += (s, e) =>
        {
            TabControlHelper.CleanupDarkTheme(tabControl);
            _searchTimer?.Dispose();
        };
    }

    public async Task WaitForDataLoadingComplete()
    {
        if (_dataLoadingCompletionSource != null)
        {
            await _dataLoadingCompletionSource.Task;
        }
    }

    private void InitializeSearchTimer()
    {
        _searchTimer = new System.Windows.Forms.Timer
        {
            Interval = 300
        };
        _searchTimer.Tick += async (s, e) =>
        {
            _searchTimer.Stop();
            if (_isInitialized)
            {
                await PerformSearch();
                tbxFindString.Focus();
            }
        };
    }

    private void OverrideTextUI()
    {
        Text = "Quản lý người dùng";

        lblFilter1.Text = "Vai trò:";
        lblFilter2.Text = "Trạng thái:";
        lblSearchString.Text = "Tìm kiếm theo (ID/Tên đăng nhập):";

        btnAdd.Text = "Thêm User";
        btnGetDetails.Text = "Chi tiết";
    }

    private void SetupDataBindings()
    {
        cbxFilter1.DisplayMember = "Name";
        cbxFilter1.ValueMember = "Id";

        cbxFilter2.DataSource = _model.Statuses ?? new System.ComponentModel.BindingList<string>();
        if (cbxFilter2.Items.Count > 0)
            cbxFilter2.SelectedIndex = 0;
    }

    private void SetupDgvUsers()
    {
        dgvUsers.AutoGenerateColumns = false;
        dgvUsers.Columns.Clear();

        dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(UserViewModel.Id),
            HeaderText = "ID User",
            Width = 80,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(UserViewModel.Username),
            HeaderText = "Tên đăng nhập",
            Width = 120,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(UserViewModel.EmployeeName),
            HeaderText = "Tên nhân viên",
            Width = 200
        });

        dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(UserViewModel.RoleName),
            HeaderText = "Vai trò",
            Width = 150
        });

        dgvUsers.Columns.Add(new DataGridViewCheckBoxColumn
        {
            DataPropertyName = nameof(UserViewModel.IsActive),
            HeaderText = "Hoạt động",
            Width = 100
        });

        dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(UserViewModel.CreatedAt),
            HeaderText = "Ngày tạo",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "dd/MM/yyyy"
            }
        });

        dgvUsers.DataSource = _model.Users;
        dgvUsers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvUsers.MultiSelect = false;

        dgvUsers.ColumnHeaderMouseClick += DgvUsers_ColumnHeaderMouseClick;
        dgvUsers.CellDoubleClick += (s, o) => DgvUsers_CellDoubleClick(s!, o);
        dgvUsers.SelectionChanged += (s, e) => btnGetDetails.Enabled = dgvUsers.SelectedRows.Count > 0;
    }

    private void SetupEvents()
    {
        cbxFilter1.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await ApplyRoleFilter();
        };

        cbxFilter2.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await ApplyStatusFilter();
        };

        tbxFindString.TextChanged += (s, e) =>
        {
            if (_isInitialized)
            {
                _searchTimer?.Stop();
                _searchTimer?.Start();
            }
        };

        tbxPasswordAgain.TextChanged += (s, e) =>
        {
            if (tbxPassword.Text != tbxPasswordAgain.Text)
            {
                btnSaveUser.Enabled = false;
                tbxPasswordAgain.BackColor = Color.LightCoral;
                toolTip1.SetToolTip(tbxPasswordAgain, "Mật khẩu không khớp!");
            }
            else
            {
                btnSaveUser.Enabled = true;
                tbxPasswordAgain.BackColor = SystemColors.Window;
                toolTip1.SetToolTip(tbxPasswordAgain, string.Empty);
            }
        };

        btnAdd.Click += (s, o) => BtnAdd_Click(s!, o);
        btnGetDetails.Click += (s, o) => BtnGetDetails_Click(s!, o);

        btnNext.Click += async (s, e) => await GoToNextPage();
        btnPrevious.Click += async (s, e) => await GoToPreviousPage();
        cbxPageSize.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await UpdatePageSize();
        };

        tabControl.SelectedIndexChanged += (s, o) => TabControl_SelectedIndexChanged(s!, o);

        btnSaveUser.Click += async (s, e) => await SaveUser();
        btnCancelUser.Click += (s, o) => CancelUserEdit(s!, o);

        // Role Assignment Tab events
        btnAssignRole.Click += async (s, e) => await AssignRolesToUser();
        btnRemoveRole.Click += async (s, e) => await RemoveRoleFromUser();

        btnFilter1.Click += (s, o) => BtnFilter1_Click(s!, o);
        btnFilter2.Click += (s, o) => BtnFilter2_Click(s!, o);

        SetupContextMenu();
    }

    #region Event Handlers

    private void SetupEmployeeComboBox()
    {
        cbxEmployee.DisplayMember = "DisplayText";
        cbxEmployee.ValueMember = "Id";
        cbxEmployee.SelectedIndexChanged += CbxEmployee_SelectedIndexChanged;
    }

    private void DgvEmployeeList_SelectionChanged(object? sender, EventArgs e)
    {
        if (_isUpdatingSelection || dgvEmployeeList.SelectedRows.Count == 0)
            return;

        try
        {
            _isUpdatingSelection = true;

            var selectedEmployee = dgvEmployeeList.SelectedRows[0].DataBoundItem as EmployeeSimpleViewModel;
            if (selectedEmployee != null)
            {
                cbxEmployee.SelectedValue = selectedEmployee.Id;
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error updating employee selection from DataGridView");
        }
        finally
        {
            _isUpdatingSelection = false;
        }
    }

    private void CbxEmployee_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (_isUpdatingSelection || cbxEmployee.SelectedValue == null)
            return;

        try
        {
            _isUpdatingSelection = true;

            var selectedEmployeeId = (long)cbxEmployee.SelectedValue;

            for (int i = 0; i < dgvEmployeeList.Rows.Count; i++)
            {
                var employee = dgvEmployeeList.Rows[i].DataBoundItem as EmployeeSimpleViewModel;
                if (employee?.Id == selectedEmployeeId)
                {
                    dgvEmployeeList.ClearSelection();
                    dgvEmployeeList.Rows[i].Selected = true;
                    dgvEmployeeList.CurrentCell = dgvEmployeeList.Rows[i].Cells[0];

                    if (dgvEmployeeList.Rows[i].Displayed == false)
                    {
                        dgvEmployeeList.FirstDisplayedScrollingRowIndex = i;
                    }
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error updating employee selection from ComboBox");
        }
        finally
        {
            _isUpdatingSelection = false;
        }
    }

    private void SetupEmployeeSearch()
    {
        var searchTimer = new System.Windows.Forms.Timer
        {
            Interval = 300 
        };

        searchTimer.Tick += (s, e) =>
        {
            searchTimer.Stop();
            PerformEmployeeSearch();
        };

        tbxFindEmployeee.TextChanged += (s, e) =>
        {
            searchTimer.Stop();
            searchTimer.Start();
        };

        tbxFindEmployeee.Tag = searchTimer;
    }

    private void ClearUserDetailForm()
    {
        tbxUserId.Clear();
        tbxUserName.Clear();
        tbxPassword.Clear();
        tbxFindEmployeee.Clear();
        chkIsActive.Checked = true;

        _isUpdatingSelection = true;
        cbxEmployee.SelectedIndex = -1;
        dgvEmployeeList.ClearSelection();
        _isUpdatingSelection = false;

        if (cbxUserRole.Items.Count > 0)
            cbxUserRole.SelectedIndex = 0;
    }

    private void InitializeEmployeeControls()
    {
        SetupEmployeeSearch();
        tbxFindEmployeee.PlaceholderText = "Tìm kiếm nhân viên theo tên, SĐT, email...";
    }

    private void PerformEmployeeSearch()
    {
        try
        {
            var searchTerm = tbxFindEmployeee.Text?.Trim().ToLower() ?? string.Empty;

            _filteredEmployees.Clear();

            var filtered = string.IsNullOrWhiteSpace(searchTerm)
                ? _allEmployees.ToList()
                : _allEmployees.Where(emp =>
                    emp.FullName.ToLower().Contains(searchTerm) ||
                    (emp.PhoneNumber?.ToLower().Contains(searchTerm) ?? false) ||
                    (emp.Email?.ToLower().Contains(searchTerm) ?? false) ||
                    emp.PositionName.ToLower().Contains(searchTerm) ||
                    emp.BranchName.ToLower().Contains(searchTerm) ||
                    emp.Id.ToString().Contains(searchTerm)
                ).ToList();

            foreach (var emp in filtered)
            {
                _filteredEmployees.Add(emp);
            }

            cbxEmployee.DataSource = new BindingList<EmployeeSimpleViewModel>(_filteredEmployees.ToList());

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                _isUpdatingSelection = true;
                dgvEmployeeList.ClearSelection();
                cbxEmployee.SelectedIndex = -1;
                _isUpdatingSelection = false;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi tìm kiếm nhân viên: {ex.Message}");
        }
    }

    private async Task LoadEmployeesForSelection()
    {
        try
        {
            if (_allEmployees.Count == 0)
            {
                var employees = await _presenter.GetEmployeesAsync();
                _allEmployees.Clear();
                _filteredEmployees.Clear();

                foreach (var emp in employees)
                {
                    _allEmployees.Add(emp);
                    _filteredEmployees.Add(emp);
                }
            }

            if (dgvEmployeeList.Columns.Count == 0)
            {
                SetupEmployeeDataGridView();
            }

            SetupEmployeeComboBox();

            dgvEmployeeList.DataSource = _filteredEmployees;
            cbxEmployee.DataSource = new BindingList<EmployeeSimpleViewModel>(_filteredEmployees.ToList());
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi tải danh sách nhân viên: {ex.Message}");
        }
    }

    private void SetupEmployeeDataGridView()
    {
        dgvEmployeeList.AutoGenerateColumns = false;
        dgvEmployeeList.Columns.Clear();
        dgvEmployeeList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvEmployeeList.MultiSelect = false;
        dgvEmployeeList.ReadOnly = true;

        dgvEmployeeList.BackgroundColor = Color.FromArgb(42, 45, 86);
        dgvEmployeeList.BorderStyle = BorderStyle.None;
        dgvEmployeeList.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvEmployeeList.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dgvEmployeeList.EnableHeadersVisualStyles = false;
        dgvEmployeeList.GridColor = Color.FromArgb(73, 75, 111);

        var headerStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            BackColor = Color.FromArgb(42, 45, 86),
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.FromArgb(124, 141, 181),
            SelectionBackColor = Color.FromArgb(42, 45, 86),
            SelectionForeColor = Color.FromArgb(124, 141, 181),
            WrapMode = DataGridViewTriState.True
        };
        dgvEmployeeList.ColumnHeadersDefaultCellStyle = headerStyle;

        var cellStyle = new DataGridViewCellStyle
        {
            Alignment = DataGridViewContentAlignment.MiddleLeft,
            BackColor = Color.FromArgb(42, 45, 86),
            Font = new Font("Segoe UI", 9F),
            ForeColor = Color.White,
            SelectionBackColor = SystemColors.Highlight,
            SelectionForeColor = SystemColors.HighlightText,
            WrapMode = DataGridViewTriState.False
        };
        dgvEmployeeList.DefaultCellStyle = cellStyle;

        dgvEmployeeList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(EmployeeSimpleViewModel.Id),
            HeaderText = "ID",
            Width = 60,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvEmployeeList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(EmployeeSimpleViewModel.FullName),
            HeaderText = "Họ và tên",
            Width = 200,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        dgvEmployeeList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(EmployeeSimpleViewModel.PhoneNumber),
            HeaderText = "Số điện thoại",
            Width = 120,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvEmployeeList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(EmployeeSimpleViewModel.Email),
            HeaderText = "Email",
            Width = 180,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvEmployeeList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(EmployeeSimpleViewModel.PositionName),
            HeaderText = "Chức vụ",
            Width = 120,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvEmployeeList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(EmployeeSimpleViewModel.BranchName),
            HeaderText = "Chi nhánh",
            Width = 120,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvEmployeeList.SelectionChanged += DgvEmployeeList_SelectionChanged;
    }



    private async void FrmUserManagement_Load(object? sender, EventArgs e)
    {
        _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
        try
        {
            SetLoadingState(true);
            Console.WriteLine("Starting data load...");

            await _presenter.LoadDataAsync(page: _model.CurrentPage, pageSize: _model.PageSize);

            Console.WriteLine($"Data loaded. Users: {_model.Users?.Count ?? 0}, Roles: {_model.Roles?.Count ?? 0}");
            UpdatePaginationInfo();

            _isInitialized = true; // Đánh dấu form đã khởi tạo xong
            _dataLoadingCompletionSource.SetResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FrmUserManagement_Load: {ex.Message}");
            _dataLoadingCompletionSource.SetException(ex);
            ShowError($"Lỗi khi tải dữ liệu người dùng: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void UpdateUsersDataGridOnly(List<UserViewModel> users)
    {
        _model.Users.Clear();
        foreach (var user in users)
        {
            _model.Users.Add(user);
        }

        if (_model.Roles?.Count > 0 && cbxFilter1.DataSource == null)
        {
            var rolesWithAll = new List<RoleViewModel>
            {
                new() { Id = 0, Name = "Tất cả" }
            };
            rolesWithAll.AddRange(_model.Roles);

            cbxFilter1.DataSource = rolesWithAll;
            if (cbxFilter1.Items.Count > 0)
                cbxFilter1.SelectedIndex = 0;
        }

        if (cbxUserRole != null && _model.Roles?.Count > 0 && cbxUserRole.DataSource == null)
        {
            cbxUserRole.DataSource = new List<RoleViewModel>(_model.Roles);
            cbxUserRole.DisplayMember = "Name";
            cbxUserRole.ValueMember = "Id";
        }

        dgvUsers.Refresh();
    }

    private async void DgvUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        await LoadEmployeesForSelection();
        if (e.RowIndex >= 0)
        {
            var selectedUser = GetSelectedUser();
            if (selectedUser != null)
            {
                LoadUserForEdit(selectedUser);
                tabControl.SelectedTab = tabUserDetail;
            }
        }
    }

    private async void BtnAdd_Click(object sender, EventArgs e)
    {
        ClearUserDetailForm();
        await LoadEmployeesForSelection();
        tabControl.SelectedTab = tabUserDetail;
        lblUserDetailTitle.Text = "Thêm người dùng mới";
        btnSaveUser.Text = "Thêm";
    }
    private async void BtnUpdate_Click(object sender, EventArgs e)
    {
        await LoadEmployeesForSelection();
        var selectedUser = GetSelectedUser();
        if (selectedUser != null)
        {
            LoadUserForEdit(selectedUser);
            tabControl.SelectedTab = tabUserDetail;
        }
        else
        {
            ShowInfo("Vui lòng chọn một người dùng để cập nhật.");
        }
    }

    private void BtnGetDetails_Click(object sender, EventArgs e)
    {
        var selectedUser = GetSelectedUser();
        if (selectedUser != null)
        {
            tabControl.SelectedTab = tabUserDetail;
        }
        else
        {
            ShowInfo("Vui lòng chọn một người dùng để xem chi tiết.");
        }
    }

    private async void TabControl_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (tabControl.SelectedTab == tabRoleAssignment)
        {
            var selectedUser = GetSelectedUser();
            if (selectedUser != null)
            {
                LoadUserRoles(selectedUser);
            }
        }
        if (tabControl.SelectedTab == tabUserDetail)
        {
            await LoadEmployeesForSelection();
        }
    }

    private void BtnFilter1_Click(object sender, EventArgs e)
    {
        cbxFilter1.DroppedDown = true;
    }
    private void BtnFilter2_Click(object sender, EventArgs e)
    {
        cbxFilter2.DroppedDown = true;
    }
    #endregion

    #region User Management Methods - Improved Pattern

    private async Task PerformSearch()
    {
        try
        {
            SetLoadingState(true);
            _model.SearchText = tbxFindString.Text;
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

    private async Task ApplyRoleFilter()
    {
        try
        {
            if (cbxFilter1.SelectedValue == null) return;

            SetLoadingState(true);
            if (cbxFilter1.SelectedValue is long roleId)
            {
                await _presenter.FilterByRoleAsync(roleId);
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

    private async Task ApplyStatusFilter()
    {
        try
        {
            if (cbxFilter2.SelectedItem == null) return;

            SetLoadingState(true);
            var selectedStatus = cbxFilter2.SelectedItem.ToString();
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

    private async Task AutoRefreshData()
    {
        try
        {
            SetLoadingState(true);
            await _presenter.RefreshCacheAsync();
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    #endregion

    #region User Detail Methods

    private async void LoadUserForEdit(UserViewModel user)
    {
        try
        {
            lblUserDetailTitle.Text = $"Chỉnh sửa người dùng - {user.Username}";
            btnSaveUser.Text = "Lưu";

            tbxUserId.Text = user.Id.ToString();
            tbxUserName.Text = user.Username ?? string.Empty;
            chkIsActive.Checked = user.IsActive;

            await LoadEmployeesForSelection();

            if (user.EmployeeId != null)
            {
                _isUpdatingSelection = true;
                cbxEmployee.SelectedValue = user.EmployeeId.Value;
                _isUpdatingSelection = false;
            }
            else
            {
                _isUpdatingSelection = true;
                cbxEmployee.SelectedIndex = -1;
                dgvEmployeeList.ClearSelection();
                _isUpdatingSelection = false;
            }

            if (cbxUserRole.DataSource != null)
            {
                cbxUserRole.SelectedValue = user.RoleId;
            }
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi tải thông tin người dùng: {ex.Message}");
        }
    }


    private async Task SaveUser()
    {
        try
        {
            if (cbxUserRole.SelectedValue == null)
            {
                ShowError("Vui lòng chọn vai trò.");
                return;
            }

            SetLoadingState(true);

            var selectedEmployeeId = cbxEmployee.SelectedValue as long?;

            var userDetail = new UserDetailViewModel
            {
                Id = string.IsNullOrEmpty(tbxUserId.Text) ? 0 : long.Parse(tbxUserId.Text),
                Username = (tbxUserName.Text ?? string.Empty).Trim(),
                Password = tbxPassword.Text?.Trim(),
                IsActive = chkIsActive.Checked,
                EmployeeId = selectedEmployeeId,
                RoleId = (long)cbxUserRole.SelectedValue
            };

            if (userDetail.Id == 0)
            {
                await _presenter.AddUserAsync(userDetail);
                ShowInfo("Thêm người dùng thành công!");
            }
            else
            {
                await _presenter.UpdateUserAsync(userDetail);
                ShowInfo("Lưu người dùng thành công!");
            }

            tabControl.SelectedTab = tabUserList;
            ClearUserDetailForm();
            await AutoRefreshData();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi lưu người dùng: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void CancelUserEdit(object sender, EventArgs e)
    {
        tabControl.SelectedTab = tabUserList;
    }

    #endregion

    #region Role Assignment Methods

    private async void LoadUserRoles(UserViewModel user)
    {

        try
        {
            lblRoleAssignmentTitle.Text = $"Gán vai trò cho: {user.Username}";

            var userRole = await _presenter.GetUserRolesAsync(user.Id);
            lstUserRoles.Items.Clear();

            lstUserRoles.Items.Add(new RoleItem { Id = userRole.Id, Name = userRole.Name });

            var availableRoles = await _presenter.GetAvailableRolesAsync(user.Id);
            lstAvailableRoles.Items.Clear();
            foreach (var role in availableRoles)
            {
                lstAvailableRoles.Items.Add(new RoleItem { Id = role.Id, Name = role.Name });
            }

            lblCurrentUserId.Text = user.Id.ToString();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi tải vai trò người dùng: {ex.Message}");
        }
    }

    private async Task AssignRolesToUser()
    {
        try
        {
            if (lstAvailableRoles.SelectedItems.Count == 0)
            {
                ShowInfo("Vui lòng chọn vai trò để gán.");
                return;
            }

            var userId = long.Parse(lblCurrentUserId.Text);
            var selectedRoles = lstAvailableRoles.SelectedItems.Cast<RoleItem>().ToList();

            // Lưu lại user hiện tại được chọn
            var currentSelectedUser = GetSelectedUser();
            var currentUserId = currentSelectedUser?.Id;

            SetLoadingState(true);

            foreach (var role in selectedRoles)
            {
                await _presenter.AssignRoleToUserAsync(userId, role.Id);
            }

            ShowInfo("Gán vai trò thành công!");

            await AutoRefreshData();

            if (currentUserId.HasValue)
            {
                RestoreUserSelection(currentUserId.Value);
            }

            var selectedUser = GetSelectedUser();
            if (selectedUser != null)
            {
                LoadUserRoles(selectedUser);
            }
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi gán vai trò: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    // Thêm method helper để restore user selection
    private void RestoreUserSelection(long userId)
    {
        try
        {
            for (int i = 0; i < dgvUsers.Rows.Count; i++)
            {
                if (dgvUsers.Rows[i].DataBoundItem is UserViewModel user && user.Id == userId)
                {
                    dgvUsers.ClearSelection();
                    dgvUsers.Rows[i].Selected = true;
                    dgvUsers.CurrentCell = dgvUsers.Rows[i].Cells[0];
                    break;
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not restore user selection for userId: {UserId}", userId);
        }
    }

    // Sửa method RemoveRoleFromUser
    private async Task RemoveRoleFromUser()
    {
        try
        {
            if (lstUserRoles.SelectedItems.Count == 0)
            {
                ShowInfo("Vui lòng chọn vai trò để gỡ bỏ.");
                return;
            }

            var userId = long.Parse(lblCurrentUserId.Text);
            var selectedRoles = lstUserRoles.SelectedItems.Cast<RoleItem>().ToList();

            if (ShowConfirmation($"Bạn có chắc chắn muốn gỡ bỏ {selectedRoles.Count} vai trò đã chọn?") != DialogResult.Yes)
                return;

            var currentSelectedUser = GetSelectedUser();
            var currentUserId = currentSelectedUser?.Id;

            SetLoadingState(true);

            foreach (var role in selectedRoles)
            {
                await _presenter.RemoveRoleFromUserAsync(userId, role.Id);
            }

            ShowInfo("Gỡ bỏ vai trò thành công!");

            await AutoRefreshData();

            if (currentUserId.HasValue)
            {
                RestoreUserSelection(currentUserId.Value);
            }

            var selectedUser = GetSelectedUser();
            if (selectedUser != null)
            {
                LoadUserRoles(selectedUser);
            }
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi gỡ bỏ vai trò: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    #endregion

    #region Pagination Methods

    private async Task GoToNextPage()
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

    private async Task GoToPreviousPage()
    {
        try
        {
            SetLoadingState(true);
            await _presenter.GoToPreviousPageAsync();
            UpdatePaginationInfo();
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async Task UpdatePageSize()
    {
        try
        {
            if (cbxPageSize.SelectedItem == null) return;

            SetLoadingState(true);
            if (int.TryParse(cbxPageSize.SelectedItem.ToString(), out int pageSize))
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

    #region Sorting Methods

    private async void DgvUsers_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        try
        {
            string sortBy = e.ColumnIndex switch
            {
                0 => "Id",
                1 => "Username",
                2 => "EmployeeName",
                3 => "RoleName",
                4 => "IsActive",
                5 => "CreatedAt",
                _ => "Id"
            };

            SetLoadingState(true);
            await _presenter.SortByAsync(sortBy);
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

    #endregion

    #region Helper Methods

    private UserViewModel? GetSelectedUser()
    {
        if (dgvUsers.SelectedRows.Count > 0)
        {
            var selectedRow = dgvUsers.SelectedRows[0];
            return selectedRow.DataBoundItem as UserViewModel;
        }
        return null;
    }

    private void UpdatePaginationInfo()
    {
        if (_model != null)
        {
            lblNumberOfRecords.Text = $"Số lượng: {_model.TotalItems}";
            lblShowingAtPage.Text = $"Hiện trang {_model.CurrentPage} trên {_model.TotalPages}";

            btnPrevious.Enabled = _model.CurrentPage > 1;
            btnNext.Enabled = _model.CurrentPage < _model.TotalPages;
        }
    }

    private void SetLoadingState(bool isLoading)
    {
        // List tab controls
        dgvUsers.Enabled = !isLoading;
        btnAdd.Enabled = !isLoading;
        btnGetDetails.Enabled = !isLoading && dgvUsers.SelectedRows.Count > 0;

        // Filter controls
        cbxFilter1.Enabled = !isLoading;
        cbxFilter2.Enabled = !isLoading;
        tbxFindString.Enabled = !isLoading;

        // Pagination controls
        btnNext.Enabled = !isLoading && _model.CurrentPage < _model.TotalPages;
        btnPrevious.Enabled = !isLoading && _model.CurrentPage > 1;
        cbxPageSize.Enabled = !isLoading;

        // User detail controls
        if (pnlUserDetail != null)
            pnlUserDetail.Enabled = !isLoading;

        // Role assignment controls
        if (pnlRoleAssignment != null)
            pnlRoleAssignment.Enabled = !isLoading;

        this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void ShowInfo(string message)
    {
        MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private DialogResult ShowConfirmation(string message)
    {
        return MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    #endregion

    #region Helper Classes

    public class RoleItem
    {
        public long Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public override string ToString()
        {
            return Name;
        }
    }

    #endregion

    #region Context Menu

    private void SetupContextMenu()
    {
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Xem chi tiết", null, (s, e) =>
        {
            var user = GetSelectedUser();
            if (user != null)
            {
                LoadUserForEdit(user);
                tabControl.SelectedTab = tabUserDetail;
            }
        });
        dgvUsers.ContextMenuStrip = contextMenu;
    }

    #endregion

    protected override void Dispose(bool disposing)
    {
        if (disposing)
        {
            if (tbxFindEmployeee?.Tag is System.Windows.Forms.Timer timer)
            {
                timer?.Dispose();
            }

            if (components != null)
            {
                components.Dispose();
            }
        }
        base.Dispose(disposing);
    }
}