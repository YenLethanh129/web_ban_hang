using Dashboard.Winform.Events;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels.RBACModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.ComponentModel;

namespace Dashboard.Winform.Forms
{
    public partial class FrmRolePermissionManagement : Form
    {
        private readonly IRolePermissionManagementPresenter _presenter;
        private readonly ILogger<FrmRolePermissionManagement> _logger;
        private readonly IServiceProvider _serviceProvider;
        private RolePermissionManagementModel _model;

        private RoleDetailViewModel? _currentEditingRole;
        private PermissionDetailViewModel? _currentEditingPermission;
        private RoleViewModel? _selectedRoleForAssignment;

        // Performance optimization: Use proper BindingSources
        private BindingSource _rolesBindingSource = new();
        private BindingSource _permissionsBindingSource = new();
        private BindingSource _availablePermissionsBindingSource = new();

        public FrmRolePermissionManagement(
            IRolePermissionManagementPresenter presenter,
            ILogger<FrmRolePermissionManagement> logger,
            IServiceProvider serviceProvider)
        {
            _presenter = presenter;
            _logger = logger;
            _serviceProvider = serviceProvider;
            _model = (RolePermissionManagementModel)_presenter.Model;

            InitializeComponent();
            SetupDataGridViews();
            SetupEventHandlers();
            SetupDataBinding(); // New method for proper data binding
            InitializeTabControl();
        }

        private void InitializeTabControl()
        {
            // Ensure only the list tab is enabled initially
            tabPageRoleEdit.Enabled = false;
            tabPagePermissionEdit.Enabled = false;
            tabPageRolePermissionAssign.Enabled = false;

            tabControlMain.SelectedTab = tabPageList;
        }

        private void SetupDataBinding()
        {
            // Bind DataGridViews to BindingSources for better performance
            dgvRoles.DataSource = _rolesBindingSource;
            dgvPermissions.DataSource = _permissionsBindingSource;
            dgvAvailablePermissions.DataSource = _availablePermissionsBindingSource;

            // Bind model properties to BindingSources
            _rolesBindingSource.DataSource = _model.Roles;
            _permissionsBindingSource.DataSource = _model.Permissions;

            // Listen to model property changes for UI updates
            _model.PropertyChanged += Model_PropertyChanged;
        }

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            // Update UI based on model property changes
            switch (e.PropertyName)
            {
                case nameof(_model.SelectedRole):
                    UpdateSelectedRoleForAssignment(_model.SelectedRole);
                    break;
                case nameof(_model.TotalItems):
                case nameof(_model.CurrentPage):
                case nameof(_model.TotalPages):
                    // Update pagination UI if you have one
                    break;
            }
        }

        private void UpdateSelectedRoleForAssignment(RoleViewModel? selectedRole)
        {
            _selectedRoleForAssignment = selectedRole;
            if (selectedRole != null)
            {
                lblSelectedRoleValue.Text = selectedRole.Name;
                tabPageRolePermissionAssign.Enabled = true;
            }
            else
            {
                lblSelectedRoleValue.Text = "[Chưa chọn role nào]";
                tabPageRolePermissionAssign.Enabled = false;
            }
        }

        private void SetupDataGridViews()
        {
            SetupRolesDataGridView();
            SetupPermissionsDataGridView();
            SetupAvailablePermissionsDataGridView();
        }

        private void SetupRolesDataGridView()
        {
            dgvRoles.AutoGenerateColumns = false;
            dgvRoles.Columns.Clear();

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.Id),
                HeaderText = "ID",
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.Name),
                HeaderText = "Tên Role",
                Width = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.Description),
                HeaderText = "Mô tả",
                Width = 300,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.PermissionCount),
                HeaderText = "Số quyền",
                Width = 80,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.CreatedAt),
                HeaderText = "Ngày tạo",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            // Performance optimization: Disable unnecessary features
            dgvRoles.VirtualMode = false; // Set to true for very large datasets
            dgvRoles.AllowUserToResizeRows = false;
            dgvRoles.RowHeadersVisible = false;
        }

        private void SetupPermissionsDataGridView()
        {
            dgvPermissions.AutoGenerateColumns = false;
            dgvPermissions.Columns.Clear();

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Id),
                HeaderText = "ID",
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Name),
                HeaderText = "Tên Permission",
                Width = 180,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Resource),
                HeaderText = "Resource",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Action),
                HeaderText = "Action",
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Description),
                HeaderText = "Mô tả",
                Width = 250,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.CreatedAt),
                HeaderText = "Ngày tạo",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            // Performance optimization: Disable unnecessary features
            dgvPermissions.VirtualMode = false; // Set to true for very large datasets
            dgvPermissions.AllowUserToResizeRows = false;
            dgvPermissions.RowHeadersVisible = false;
        }

        private void SetupAvailablePermissionsDataGridView()
        {
            dgvAvailablePermissions.AutoGenerateColumns = false;
            dgvAvailablePermissions.Columns.Clear();

            // Add checkbox column for selection
            var checkBoxColumn = new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.IsAssigned),
                HeaderText = "Đã gán",
                Width = 70,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None
            };
            dgvAvailablePermissions.Columns.Add(checkBoxColumn);

            dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Id),
                HeaderText = "ID",
                Width = 60,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = true
            });

            dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Name),
                HeaderText = "Tên Permission",
                Width = 200,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = true
            });

            dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Resource),
                HeaderText = "Resource",
                Width = 120,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = true
            });

            dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Action),
                HeaderText = "Action",
                Width = 100,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                ReadOnly = true
            });

            dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Description),
                HeaderText = "Mô tả",
                Width = 250,
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });

            // Performance optimization
            dgvAvailablePermissions.VirtualMode = false;
            dgvAvailablePermissions.AllowUserToResizeRows = false;
            dgvAvailablePermissions.RowHeadersVisible = false;
        }

        private void SetupEventHandlers()
        {
            // Presenter events
            _presenter.OnRolesLoaded += OnRolesLoaded;
            _presenter.OnPermissionsLoaded += OnPermissionsLoaded;

            // DataGridView events
            dgvRoles.CellDoubleClick += DgvRoles_CellDoubleClick;
            dgvPermissions.CellDoubleClick += DgvPermissions_CellDoubleClick;
            dgvRoles.SelectionChanged += DgvRoles_SelectionChanged;

            // Tab control events
            tabControlMain.SelectedIndexChanged += TabControlMain_SelectedIndexChanged;

            // Edit form events
            btnSaveRole.Click += BtnSaveRole_Click;
            btnCancelRole.Click += BtnCancelRole_Click;
            btnSavePermission.Click += BtnSavePermission_Click;
            btnCancelPermission.Click += BtnCancelPermission_Click;

            // Assignment events
            btnAssignPermissions.Click += BtnAssignPermissions_Click;
            btnUnassignPermissions.Click += BtnUnassignPermissions_Click;

            // Form events
            Load += FrmRolePermissionManagement_Load;
        }

        private async void FrmRolePermissionManagement_Load(object? sender, EventArgs e)
        {
            try
            {
                SetLoadingState(true);
                await _presenter.LoadDataAsync(); // Load both roles and permissions
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading role permission data");
                ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void OnRolesLoaded(object? sender, RolesLoadedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateRolesBindingSource(e.Roles)));
            }
            else
            {
                UpdateRolesBindingSource(e.Roles);
            }
        }

        private void OnPermissionsLoaded(object? sender, PermissionsLoadedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdatePermissionsBindingSource(e.Permissions)));
            }
            else
            {
                UpdatePermissionsBindingSource(e.Permissions);
            }
        }

        private void UpdateRolesBindingSource(List<RoleViewModel> roles)
        {
            try
            {
                // Performance optimization: Use SuspendBinding to prevent multiple updates
                _rolesBindingSource.RaiseListChangedEvents = false;

                // Clear and repopulate more efficiently
                var bindingList = _rolesBindingSource.DataSource as BindingList<RoleViewModel>;
                if (bindingList == null)
                {
                    bindingList = new BindingList<RoleViewModel>();
                    _rolesBindingSource.DataSource = bindingList;
                }

                bindingList.Clear();
                foreach (var role in roles)
                {
                    bindingList.Add(role);
                }

                _rolesBindingSource.RaiseListChangedEvents = true;
                _rolesBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating roles binding source");
                ShowError($"Lỗi khi cập nhật dữ liệu roles: {ex.Message}");
            }
        }

        private void UpdatePermissionsBindingSource(List<PermissionViewModel> permissions)
        {
            try
            {
                // Performance optimization: Use SuspendBinding to prevent multiple updates
                _permissionsBindingSource.RaiseListChangedEvents = false;

                // Clear and repopulate more efficiently
                var bindingList = _permissionsBindingSource.DataSource as BindingList<PermissionViewModel>;
                if (bindingList == null)
                {
                    bindingList = new BindingList<PermissionViewModel>();
                    _permissionsBindingSource.DataSource = bindingList;
                }

                bindingList.Clear();
                foreach (var permission in permissions)
                {
                    bindingList.Add(permission);
                }

                _permissionsBindingSource.RaiseListChangedEvents = true;
                _permissionsBindingSource.ResetBindings(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating permissions binding source");
                ShowError($"Lỗi khi cập nhật dữ liệu permissions: {ex.Message}");
            }
        }

        #region DataGridView Events

        private async void DgvRoles_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedRole = GetSelectedRole();
                if (selectedRole != null)
                {
                    await OpenRoleEditTab(selectedRole.Id);
                }
            }
        }

        private async void DgvPermissions_CellDoubleClick(object? sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var selectedPermission = GetSelectedPermission();
                if (selectedPermission != null)
                {
                    await OpenPermissionEditTab(selectedPermission.Id);
                }
            }
        }

        private void DgvRoles_SelectionChanged(object? sender, EventArgs e)
        {
            var selectedRole = GetSelectedRole();
            _model.SelectedRole = selectedRole; // This will trigger property changed event
        }

        #endregion

        #region Tab Management

        private void TabControlMain_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab == tabPageRolePermissionAssign && _selectedRoleForAssignment != null)
            {
                LoadRolePermissionsForAssignmentAsync(_selectedRoleForAssignment.Id);
            }
        }

        private async Task OpenRoleEditTab(long? roleId = null)
        {
            try
            {
                SetLoadingState(true);
                _currentEditingRole = await _presenter.CreateRoleDetailAsync(roleId);

                // Populate form
                txtRoleName.Text = _currentEditingRole.Name;
                txtRoleDescription.Text = _currentEditingRole.Description ?? string.Empty;

                // Enable and switch to edit tab
                tabPageRoleEdit.Enabled = true;
                tabControlMain.SelectedTab = tabPageRoleEdit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error opening role edit tab");
                ShowError($"Lỗi khi mở tab chỉnh sửa role: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task OpenPermissionEditTab(long? permissionId = null)
        {
            try
            {
                SetLoadingState(true);
                _currentEditingPermission = await _presenter.CreatePermissionDetailAsync(permissionId);

                // Populate form
                txtPermissionName.Text = _currentEditingPermission.Name;
                txtPermissionDescription.Text = _currentEditingPermission.Description ?? string.Empty;
                txtPermissionResource.Text = _currentEditingPermission.Resource;
                txtPermissionAction.Text = _currentEditingPermission.Action;

                // Enable and switch to edit tab
                tabPagePermissionEdit.Enabled = true;
                tabControlMain.SelectedTab = tabPagePermissionEdit;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error opening permission edit tab");
                ShowError($"Lỗi khi mở tab chỉnh sửa permission: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void LoadRolePermissionsForAssignmentAsync(long roleId)
        {
            try
            {
                SetLoadingState(true);
                var rolePermissions = await _presenter.GetRolePermissionsAsync(roleId);
                var allPermissions = await _presenter.CreateRoleDetailAsync(roleId);

                // Mark permissions as assigned efficiently
                var assignedPermissionIds = new HashSet<long>(allPermissions.AssignedPermissionIds);
                foreach (var permission in allPermissions.AllPermissions)
                {
                    permission.IsAssigned = assignedPermissionIds.Contains(permission.Id);
                }

                // Use BindingList for better performance
                var permissionsBindingList = new BindingList<PermissionViewModel>(allPermissions.AllPermissions);
                _availablePermissionsBindingSource.DataSource = permissionsBindingList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading role permissions for assignment");
                ShowError($"Lỗi khi tải quyền của role: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        #endregion

        #region Button Events

        private async void BtnSaveRole_Click(object? sender, EventArgs e)
        {
            if (_currentEditingRole == null) return;

            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtRoleName.Text))
                {
                    ShowError("Vui lòng nhập tên role.");
                    txtRoleName.Focus();
                    return;
                }

                SetLoadingState(true);

                // Update model
                _currentEditingRole.Name = txtRoleName.Text.Trim();
                _currentEditingRole.Description = txtRoleDescription.Text.Trim();

                await _presenter.SaveRoleAsync(_currentEditingRole);

                ShowInfo("Lưu role thành công!");
                await ReturnToListTab();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving role");
                ShowError($"Lỗi khi lưu role: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void BtnCancelRole_Click(object? sender, EventArgs e)
        {
            await ReturnToListTab();
        }

        private async void BtnSavePermission_Click(object? sender, EventArgs e)
        {
            if (_currentEditingPermission == null) return;

            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtPermissionName.Text))
                {
                    ShowError("Vui lòng nhập tên permission.");
                    txtPermissionName.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPermissionResource.Text))
                {
                    ShowError("Vui lòng nhập resource.");
                    txtPermissionResource.Focus();
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPermissionAction.Text))
                {
                    ShowError("Vui lòng nhập action.");
                    txtPermissionAction.Focus();
                    return;
                }

                SetLoadingState(true);

                // Update model
                _currentEditingPermission.Name = txtPermissionName.Text.Trim();
                _currentEditingPermission.Description = txtPermissionDescription.Text.Trim();
                _currentEditingPermission.Resource = txtPermissionResource.Text.Trim();
                _currentEditingPermission.Action = txtPermissionAction.Text.Trim();

                await _presenter.SavePermissionAsync(_currentEditingPermission);

                ShowInfo("Lưu permission thành công!");
                await ReturnToListTab();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving permission");
                ShowError($"Lỗi khi lưu permission: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void BtnCancelPermission_Click(object? sender, EventArgs e)
        {
            await ReturnToListTab();
        }

        private async void BtnAssignPermissions_Click(object? sender, EventArgs e)
        {
            if (_selectedRoleForAssignment == null) return;

            try
            {
                SetLoadingState(true);

                var selectedPermissions = GetSelectedPermissionsFromGrid();
                var permissionsToAssign = selectedPermissions
                    .Where(p => !p.IsAssigned)
                    .Select(p => p.Id)
                    .ToList();

                if (!permissionsToAssign.Any())
                {
                    ShowInfo("Không có quyền nào được chọn để gán.");
                    return;
                }

                // Get currently assigned permissions and add new ones
                var currentAssigned = selectedPermissions
                    .Where(p => p.IsAssigned)
                    .Select(p => p.Id)
                    .ToList();

                var allAssigned = currentAssigned.Concat(permissionsToAssign).Distinct().ToList();

                await _presenter.AssignPermissionsToRoleAsync(_selectedRoleForAssignment.Id, allAssigned);

                ShowInfo($"Đã gán {permissionsToAssign.Count} quyền cho role {_selectedRoleForAssignment.Name}");
                LoadRolePermissionsForAssignmentAsync(_selectedRoleForAssignment.Id);

                // Refresh roles list to update permission count
                await _presenter.LoadRolesAsync(forceRefresh: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error assigning permissions");
                ShowError($"Lỗi khi gán quyền: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async void BtnUnassignPermissions_Click(object? sender, EventArgs e)
        {
            if (_selectedRoleForAssignment == null) return;

            try
            {
                SetLoadingState(true);

                var selectedPermissions = GetSelectedPermissionsFromGrid();
                var permissionsToUnassign = selectedPermissions
                    .Where(p => p.IsAssigned)
                    .Select(p => p.Id)
                    .ToList();

                if (!permissionsToUnassign.Any())
                {
                    ShowInfo("Không có quyền nào được chọn để gỡ bỏ.");
                    return;
                }

                // Get remaining assigned permissions
                var currentAssigned = selectedPermissions
                    .Where(p => p.IsAssigned)
                    .Select(p => p.Id)
                    .ToList();

                var remainingAssigned = currentAssigned.Except(permissionsToUnassign).ToList();

                await _presenter.AssignPermissionsToRoleAsync(_selectedRoleForAssignment.Id, remainingAssigned);

                ShowInfo($"Đã gỡ {permissionsToUnassign.Count} quyền khỏi role {_selectedRoleForAssignment.Name}");
                LoadRolePermissionsForAssignmentAsync(_selectedRoleForAssignment.Id);

                // Refresh roles list to update permission count
                await _presenter.LoadRolesAsync(forceRefresh: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error unassigning permissions");
                ShowError($"Lỗi khi gỡ quyền: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        #endregion

        #region Public Methods for External Access

        public async Task AddNewRole()
        {
            await OpenRoleEditTab();
        }

        public async Task AddNewPermission()
        {
            await OpenPermissionEditTab();
        }

        #endregion

        #region Helper Methods

        private async Task ReturnToListTab()
        {
            // Clear edit forms
            ClearRoleEditForm();
            ClearPermissionEditForm();

            // Reset editing objects
            _currentEditingRole = null;
            _currentEditingPermission = null;

            // Disable edit tabs
            tabPageRoleEdit.Enabled = false;
            tabPagePermissionEdit.Enabled = false;

            // Return to list tab
            tabControlMain.SelectedTab = tabPageList;

            // Refresh data
            await _presenter.LoadRolesAsync(forceRefresh: true);
            await _presenter.LoadPermissionsAsync(forceRefresh: true);
        }

        private void ClearRoleEditForm()
        {
            txtRoleName.Clear();
            txtRoleDescription.Clear();
        }

        private void ClearPermissionEditForm()
        {
            txtPermissionName.Clear();
            txtPermissionDescription.Clear();
            txtPermissionResource.Clear();
            txtPermissionAction.Clear();
        }

        private RoleViewModel? GetSelectedRole()
        {
            return _rolesBindingSource.Current as RoleViewModel;
        }

        private PermissionViewModel? GetSelectedPermission()
        {
            return _permissionsBindingSource.Current as PermissionViewModel;
        }

        private List<PermissionViewModel> GetSelectedPermissionsFromGrid()
        {
            var permissions = new List<PermissionViewModel>();

            if (_availablePermissionsBindingSource.DataSource is BindingList<PermissionViewModel> bindingList)
            {
                // Use HashSet for better performance on lookups
                var selectedIndices = new HashSet<int>();
                foreach (DataGridViewRow row in dgvAvailablePermissions.SelectedRows)
                {
                    selectedIndices.Add(row.Index);
                }

                for (int i = 0; i < bindingList.Count; i++)
                {
                    var permission = bindingList[i];
                    if (permission.IsAssigned || selectedIndices.Contains(i))
                    {
                        permissions.Add(permission);
                    }
                }
            }

            return permissions;
        }

        private void SetLoadingState(bool isLoading)
        {
            tabControlMain.Enabled = !isLoading;
            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
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

        #region Context Menu Support

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            SetupContextMenus();
        }

        private void SetupContextMenus()
        {
            // Context menu for roles grid
            var roleContextMenu = new ContextMenuStrip();
            roleContextMenu.Items.Add("Thêm role mới", null, async (s, e) => await AddNewRole());
            roleContextMenu.Items.Add("Sửa role", null, async (s, e) => {
                var role = GetSelectedRole();
                if (role != null) await OpenRoleEditTab(role.Id);
            });
            roleContextMenu.Items.Add("Xóa role", null, async (s, e) => await DeleteSelectedRole());
            roleContextMenu.Items.Add("-");
            roleContextMenu.Items.Add("Gán quyền", null, (s, e) => {
                if (_selectedRoleForAssignment != null)
                    tabControlMain.SelectedTab = tabPageRolePermissionAssign;
            });
            dgvRoles.ContextMenuStrip = roleContextMenu;

            // Context menu for permissions grid
            var permissionContextMenu = new ContextMenuStrip();
            permissionContextMenu.Items.Add("Thêm permission mới", null, async (s, e) => await AddNewPermission());
            permissionContextMenu.Items.Add("Sửa permission", null, async (s, e) => {
                var permission = GetSelectedPermission();
                if (permission != null) await OpenPermissionEditTab(permission.Id);
            });
            permissionContextMenu.Items.Add("Xóa permission", null, async (s, e) => await DeleteSelectedPermission());
            dgvPermissions.ContextMenuStrip = permissionContextMenu;
        }

        private async Task DeleteSelectedRole()
        {
            var selectedRole = GetSelectedRole();
            if (selectedRole == null)
            {
                ShowInfo("Vui lòng chọn role cần xóa.");
                return;
            }

            var result = ShowConfirmation($"Bạn có chắc muốn xóa role '{selectedRole.Name}'?");
            if (result == DialogResult.Yes)
            {
                try
                {
                    SetLoadingState(true);
                    await _presenter.DeleteRoleAsync(selectedRole.Id);
                    ShowInfo("Xóa role thành công!");
                    await _presenter.LoadRolesAsync(forceRefresh: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting role");
                    ShowError($"Lỗi khi xóa role: {ex.Message}");
                }
                finally
                {
                    SetLoadingState(false);
                }
            }
        }

        private async Task DeleteSelectedPermission()
        {
            var selectedPermission = GetSelectedPermission();
            if (selectedPermission == null)
            {
                ShowInfo("Vui lòng chọn permission cần xóa.");
                return;
            }

            var result = ShowConfirmation($"Bạn có chắc muốn xóa permission '{selectedPermission.Name}'?");
            if (result == DialogResult.Yes)
            {
                try
                {
                    SetLoadingState(true);
                    await _presenter.DeletePermissionAsync(selectedPermission.Id);
                    ShowInfo("Xóa permission thành công!");
                    await _presenter.LoadPermissionsAsync(forceRefresh: true);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error deleting permission");
                    ShowError($"Lỗi khi xóa permission: {ex.Message}");
                }
                finally
                {
                    SetLoadingState(false);
                }
            }
        }

        #endregion

        #region IDisposable

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

                // Dispose BindingSources
                _rolesBindingSource?.Dispose();
                _permissionsBindingSource?.Dispose();
                _availablePermissionsBindingSource?.Dispose();

                // Unsubscribe from events
                if (_presenter != null)
                {
                    _presenter.OnRolesLoaded -= OnRolesLoaded;
                    _presenter.OnPermissionsLoaded -= OnPermissionsLoaded;
                }

                if (_model != null)
                {
                    _model.PropertyChanged -= Model_PropertyChanged;
                }
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}