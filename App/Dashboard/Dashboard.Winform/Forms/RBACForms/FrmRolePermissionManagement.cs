using Dashboard.Winform.Attributes;
using Dashboard.Winform.Events;
using Dashboard.Winform.Forms.BaseFrm;
using Dashboard.Winform.Helpers;
using Dashboard.Winform.Interfaces;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels.RBACModels;
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
    [RequireRole("ADMIN")]
    public partial class FrmRolePermissionManagement : FrmBaseAuthForm, IBlurLoadingServiceAware
    {
        private readonly IRolePermissionManagementPresenter _presenter;
        private readonly ILogger<FrmRolePermissionManagement> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly RolePermissionManagementModel _model;
        private IBlurLoadingService? _blurLoadingService;

        private RoleDetailViewModel? _currentEditingRole;
        private PermissionDetailViewModel? _currentEditingPermission;
        private RoleViewModel? _selectedRoleForAssignment;

        private readonly BindingSource _rolesBindingSource = new();
        private readonly BindingSource _permissionsBindingSource = new();
        private BindingList<PermissionViewModel>? _availablePermissionsBindingList;
        private BindingList<PermissionViewModel>? _assignedPermissionsBindingList;

        private TaskCompletionSource<bool>? _dataLoadingCompletionSource;

        public FrmRolePermissionManagement(
            IRolePermissionManagementPresenter presenter,
            ILogger<FrmRolePermissionManagement> logger,
            IServiceProvider serviceProvider)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _model = (RolePermissionManagementModel)_presenter.Model;

            InitializeComponent();

            TabControlHelper.SetupDarkTheme(tabControlMain);
            tabControlMain.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControlMain.DrawItem += TabControlMain_DrawItem;

            foreach (TabPage tp in tabControlMain.TabPages)
            {
                tp.ForeColor = Color.White;
            }

            SetupDataGridViews();
            SetupEventHandlers();
            SetupDataBinding();
            InitializeTabControl();

            Load += FrmRolePermissionManagement_Load;
            FormClosed += (s, e) =>
            {
                TabControlHelper.CleanupDarkTheme(tabControlMain);
            };
        }

        public void SetBlurLoadingService(IBlurLoadingService blurLoadingService)
        {
            _blurLoadingService = blurLoadingService;
        }

        public async Task WaitForDataLoadingComplete()
        {
            if (_dataLoadingCompletionSource != null)
            {
                await _dataLoadingCompletionSource.Task;
            }
        }

        private void InitializeTabControl()
        {
            tabPageRoleEdit.Enabled = false;
            tabPagePermissionEdit.Enabled = false;
            tabPageRolePermissionAssign.Enabled = false;

            tabControlMain.SelectedTab = tabPageList;
        }

        private void SetupDataBinding()
        {
            try
            {
                dgvRoles.DataSource = _rolesBindingSource;
                dgvPermissions.DataSource = _permissionsBindingSource;

                // Link to model's BindingLists
                _rolesBindingSource.DataSource = _model.Roles;
                _permissionsBindingSource.DataSource = _model.Permissions;

                _model.PropertyChanged += Model_PropertyChanged;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting up data binding");
            }
        }

        private void Model_PropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            try
            {
                switch (e.PropertyName)
                {
                    case nameof(RolePermissionManagementModel.SelectedRole):
                        UpdateSelectedRoleForAssignment(_model.SelectedRole);
                        break;
                    case nameof(RolePermissionManagementModel.TotalItems):
                    case nameof(RolePermissionManagementModel.CurrentPage):
                    case nameof(RolePermissionManagementModel.TotalPages):
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error handling model property change");
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
            SetupAssignedPermissionsDataGridView();
        }

        private void SetupRolesDataGridView()
        {
            try
            {
                dgvRoles.AutoGenerateColumns = false;
                dgvRoles.Columns.Clear();

                ApplyDarkThemeToDataGridView(dgvRoles);

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

                dgvRoles.VirtualMode = false;
                dgvRoles.AllowUserToResizeRows = false;
                dgvRoles.RowHeadersVisible = false;
                dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvRoles.MultiSelect = false;
                dgvRoles.ReadOnly = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting up roles DataGridView");
            }
        }

        private void SetupPermissionsDataGridView()
        {
            try
            {
                dgvPermissions.AutoGenerateColumns = false;
                dgvPermissions.Columns.Clear();

                ApplyDarkThemeToDataGridView(dgvPermissions);

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
                    HeaderText = "Tên quyền",
                    Width = 180,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                });

                dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(PermissionViewModel.Resource),
                    HeaderText = "Tài nguyên",
                    Width = 120,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                });

                dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    DataPropertyName = nameof(PermissionViewModel.Action),
                    HeaderText = "Hành động",
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

                dgvPermissions.VirtualMode = false;
                dgvPermissions.AllowUserToResizeRows = false;
                dgvPermissions.RowHeadersVisible = false;
                dgvPermissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvPermissions.MultiSelect = false;
                dgvPermissions.ReadOnly = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting up permissions DataGridView");
            }
        }

        private void SetupAvailablePermissionsDataGridView()
        {
            try
            {
                dgvAvailablePermissions.AutoGenerateColumns = false;
                dgvAvailablePermissions.Columns.Clear();

                dgvAvailablePermissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

                ApplyDarkThemeToDataGridView(dgvAvailablePermissions);

                dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colAvailable_Id",
                    DataPropertyName = nameof(PermissionViewModel.Id),
                    HeaderText = "ID",
                    ValueType = typeof(long),
                    Width = 60,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    ReadOnly = true
                });

                dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colAvailable_Name",
                    DataPropertyName = nameof(PermissionViewModel.Name),
                    HeaderText = "Tên quyền",
                    ValueType = typeof(string),
                    Width = 150,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                    ReadOnly = true
                });

                dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colAvailable_Resource",
                    DataPropertyName = nameof(PermissionViewModel.Resource),
                    HeaderText = "Tài nguyên",
                    ValueType = typeof(string),
                    Width = 80,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    ReadOnly = true
                });

                dgvAvailablePermissions.Columns.Add(new DataGridViewTextBoxColumn
                {
                    Name = "colAvailable_Action",
                    DataPropertyName = nameof(PermissionViewModel.Action),
                    HeaderText = "Hành động",
                    ValueType = typeof(string),
                    Width = 70,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                    ReadOnly = true
                });

                dgvAvailablePermissions.DefaultCellStyle.ForeColor = Color.White;
                dgvAvailablePermissions.DefaultCellStyle.BackColor = Color.FromArgb(42, 45, 86);
                dgvAvailablePermissions.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;

                dgvAvailablePermissions.VirtualMode = false;
                dgvAvailablePermissions.AllowUserToResizeRows = false;
                dgvAvailablePermissions.RowHeadersVisible = false;
                dgvAvailablePermissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                dgvAvailablePermissions.MultiSelect = true;
                dgvAvailablePermissions.ReadOnly = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting up available permissions DataGridView");
            }
        }


        private void SetupAssignedPermissionsDataGridView()
{
    try
    {
        dgvAssignedPermissions.AutoGenerateColumns = false;
        dgvAssignedPermissions.Columns.Clear();

        dgvAssignedPermissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

        ApplyDarkThemeToDataGridView(dgvAssignedPermissions);

        dgvAssignedPermissions.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colAssigned_Id",
            DataPropertyName = nameof(PermissionViewModel.Id),
            HeaderText = "ID",
            ValueType = typeof(long),
            Width = 60,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            ReadOnly = true
        });

        dgvAssignedPermissions.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colAssigned_Name",
            DataPropertyName = nameof(PermissionViewModel.Name),
            HeaderText = "Tên quyền",
            ValueType = typeof(string),
            Width = 150,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
            ReadOnly = true
        });

        dgvAssignedPermissions.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colAssigned_Resource",
            DataPropertyName = nameof(PermissionViewModel.Resource),
            HeaderText = "Tài nguyên",
            ValueType = typeof(string),
            Width = 80,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            ReadOnly = true
        });

        dgvAssignedPermissions.Columns.Add(new DataGridViewTextBoxColumn
        {
            Name = "colAssigned_Action",
            DataPropertyName = nameof(PermissionViewModel.Action),
            HeaderText = "Hành động",
            ValueType = typeof(string),
            Width = 70,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
            ReadOnly = true
        });

        dgvAssignedPermissions.DefaultCellStyle.ForeColor = Color.White;
        dgvAssignedPermissions.DefaultCellStyle.BackColor = Color.FromArgb(42, 45, 86);
        dgvAssignedPermissions.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;

        dgvAssignedPermissions.VirtualMode = false;
        dgvAssignedPermissions.AllowUserToResizeRows = false;
        dgvAssignedPermissions.RowHeadersVisible = false;
        dgvAssignedPermissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvAssignedPermissions.MultiSelect = true;
        dgvAssignedPermissions.ReadOnly = true;
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error setting up assigned permissions DataGridView");
    }
}

        private void ApplyDarkThemeToDataGridView(DataGridView dgv)
        {
            try
            {
                dgv.BackgroundColor = Color.FromArgb(42, 45, 86);
                dgv.BorderStyle = BorderStyle.None;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
                dgv.EnableHeadersVisualStyles = false;
                dgv.GridColor = Color.FromArgb(73, 75, 111);

                var headerStyle = new DataGridViewCellStyle
                {
                    Alignment = DataGridViewContentAlignment.MiddleLeft,
                    BackColor = Color.FromArgb(42, 45, 86),
                    Font = new Font("Segoe UI", 9F, FontStyle.Bold),
                    ForeColor = Color.FromArgb(124, 141, 181),
                    SelectionBackColor = Color.FromArgb(42, 45, 86),
                    SelectionForeColor = Color.FromArgb(124, 141, 181),
                    WrapMode = DataGridViewTriState.True
                };
                dgv.ColumnHeadersDefaultCellStyle = headerStyle;

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
                dgv.DefaultCellStyle = cellStyle;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error applying dark theme to DataGridView");
            }
        }

        private void SetupEventHandlers()
        {
            try
            {
                _presenter.OnRolesLoaded += OnRolesLoaded;
                _presenter.OnPermissionsLoaded += OnPermissionsLoaded;

                dgvRoles.CellDoubleClick += DgvRoles_CellDoubleClick;
                dgvPermissions.CellDoubleClick += DgvPermissions_CellDoubleClick;
                dgvRoles.SelectionChanged += DgvRoles_SelectionChanged;

                // make SelectedIndexChanged async to await LoadRolePermissionsForAssignmentAsync
                tabControlMain.SelectedIndexChanged += async (s, e) => await TabControlMain_SelectedIndexChangedAsync(s, e);

                btnSaveRole.Click += BtnSaveRole_Click;
                btnCancelRole.Click += BtnCancelRole_Click;
                btnSavePermission.Click += BtnSavePermission_Click;
                btnCancelPermission.Click += BtnCancelPermission_Click;

                btnAssignPermissions.Click += BtnAssignPermissions_Click;
                btnRemovePermissions.Click += BtnRemovePermissions_Click;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting up event handlers");
            }
        }

        private async void FrmRolePermissionManagement_Load(object? sender, EventArgs e)
        {
            _dataLoadingCompletionSource = new TaskCompletionSource<bool>();

            try
            {
                SetLoadingState(true);
                await _presenter.LoadDataAsync();

                _dataLoadingCompletionSource.SetResult(true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading role permission data");
                _dataLoadingCompletionSource.SetException(ex);
                ShowError($"Lỗi khi tải dữ liệu: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private async Task TabControlMain_SelectedIndexChangedAsync(object? sender, EventArgs e)
        {
            if (tabControlMain.SelectedTab == tabPageRolePermissionAssign && _selectedRoleForAssignment != null)
            {
                await LoadRolePermissionsForAssignmentAsync(_selectedRoleForAssignment.Id);
            }

            // Redraw tabs (owner-drawn) so selected text color updates immediately
            tabControlMain.Invalidate();
        }

        private async void BtnRemovePermissions_Click(object? sender, EventArgs e)
        {
            if (_selectedRoleForAssignment == null) return;

            try
            {
                if (dgvAssignedPermissions.SelectedRows.Count == 0)
                {
                    ShowInfo("Vui lòng chọn quyền để gỡ bỏ.");
                    return;
                }

                var result = ShowConfirmation($"Bạn có chắc chắn muốn gỡ bỏ {dgvAssignedPermissions.SelectedRows.Count} quyền đã chọn?");
                if (result != DialogResult.Yes) return;

                SetLoadingState(true);

                var permissionsToRemove = dgvAssignedPermissions.SelectedRows
                    .Cast<DataGridViewRow>()
                    .Where(row => row.DataBoundItem != null)
                    .Select(row => ((PermissionViewModel)row.DataBoundItem).Id)
                    .ToList();

                var currentAssigned = _assignedPermissionsBindingList?.Select(p => p.Id).ToList() ?? new List<long>();

                var remainingAssigned = currentAssigned.Except(permissionsToRemove).ToList();

                await _presenter.AssignPermissionsToRoleAsync(_selectedRoleForAssignment.Id, remainingAssigned);

                ShowInfo($"Đã gỡ {permissionsToRemove.Count} quyền khỏi role {_selectedRoleForAssignment.Name}");

                await LoadRolePermissionsForAssignmentAsync(_selectedRoleForAssignment.Id);

                await _presenter.LoadRolesAsync(forceRefresh: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing permissions");
                ShowError($"Lỗi khi gỡ quyền: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }

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
            try
            {
                ClearRoleEditForm();
                ClearPermissionEditForm();

                _currentEditingRole = null;
                _currentEditingPermission = null;

                tabPageRoleEdit.Enabled = false;
                tabPagePermissionEdit.Enabled = false;

                tabControlMain.SelectedTab = tabPageList;

                await _presenter.LoadRolesAsync(forceRefresh: true);
                await _presenter.LoadPermissionsAsync(forceRefresh: true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error returning to list tab");
                ShowError($"Lỗi khi quay lại tab danh sách: {ex.Message}");
            }
        }

        private void ClearRoleEditForm()
        {
            try
            {
                txtRoleName.Clear();
                txtRoleDescription.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error clearing role edit form");
            }
        }

        private void ClearPermissionEditForm()
        {
            try
            {
                txtPermissionName.Clear();
                txtPermissionDescription.Clear();
                txtPermissionResource.Clear();
                txtPermissionAction.Clear();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error clearing permission edit form");
            }
        }

        private RoleViewModel? GetSelectedRole()
        {
            try
            {
                return _rolesBindingSource.Current as RoleViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting selected role");
                return null;
            }
        }

        private PermissionViewModel? GetSelectedPermission()
        {
            try
            {
                return _permissionsBindingSource.Current as PermissionViewModel;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error getting selected permission");
                return null;
            }
        }

        private void SetLoadingState(bool isLoading)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => SetLoadingStateInternal(isLoading)));
                }
                else
                {
                    SetLoadingStateInternal(isLoading);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error setting loading state");
            }
        }

        private void SetLoadingStateInternal(bool isLoading)
        {
            tabControlMain.Enabled = !isLoading;
            Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void ShowError(string message)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)));
                }
                else
                {
                    MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error showing error message");
            }
        }

        private void ShowInfo(string message)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)));
                }
                else
                {
                    MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error showing info message");
            }
        }

        private DialogResult ShowConfirmation(string message)
        {
            try
            {
                if (InvokeRequired)
                {
                    return (DialogResult)Invoke(new Func<DialogResult>(() =>
                        MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question)));
                }
                else
                {
                    return MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error showing confirmation dialog");
                return DialogResult.No;
            }
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
            try
            {
                var roleContextMenu = new ContextMenuStrip();
                roleContextMenu.Items.Add("Thêm role mới", null, async (s, e) => await AddNewRole());
                roleContextMenu.Items.Add("Sửa role", null, async (s, e) =>
                {
                    var role = GetSelectedRole();
                    if (role != null) await OpenRoleEditTab(role.Id);
                });
                roleContextMenu.Items.Add("Xóa role", null, async (s, e) => await DeleteSelectedRole());
                roleContextMenu.Items.Add("-");
                roleContextMenu.Items.Add("Gán quyền", null, (s, e) =>
                {
                    if (_selectedRoleForAssignment != null)
                        tabControlMain.SelectedTab = tabPageRolePermissionAssign;
                });
                dgvRoles.ContextMenuStrip = roleContextMenu;

                var permissionContextMenu = new ContextMenuStrip();
                permissionContextMenu.Items.Add("Thêm permission mới", null, async (s, e) => await AddNewPermission());
                permissionContextMenu.Items.Add("Sửa permission", null, async (s, e) =>
                {
                    var permission = GetSelectedPermission();
                    if (permission != null) await OpenPermissionEditTab(permission.Id);
                });
                permissionContextMenu.Items.Add("Xóa permission", null, async (s, e) => await DeleteSelectedPermission());
                dgvPermissions.ContextMenuStrip = permissionContextMenu;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting up context menus");
            }
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

        #region Presenter Event Handlers & BindingSource Updates

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
                _rolesBindingSource.RaiseListChangedEvents = false;

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
                _permissionsBindingSource.RaiseListChangedEvents = false;

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

        #endregion

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
            try
            {
                var selectedRole = GetSelectedRole();
                _model.SelectedRole = selectedRole;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error handling role selection change");
            }
        }

        #endregion

        #region Tab Management & Load Role Permissions

        private async Task OpenRoleEditTab(long? roleId = null)
        {
            try
            {
                SetLoadingState(true);
                _currentEditingRole = await _presenter.CreateRoleDetailAsync(roleId);

                txtRoleName.Text = _currentEditingRole.Name;
                txtRoleDescription.Text = _currentEditingRole.Description ?? string.Empty;

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

                txtPermissionName.Text = _currentEditingPermission.Name;
                txtPermissionDescription.Text = _currentEditingPermission.Description ?? string.Empty;
                txtPermissionResource.Text = _currentEditingPermission.Resource;
                txtPermissionAction.Text = _currentEditingPermission.Action;

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

        private async Task LoadRolePermissionsForAssignmentAsync(long roleId)
        {
            try
            {
                SetLoadingState(true);

                var roleDetail = await _presenter.CreateRoleDetailAsync(roleId);

                var allPermissions = roleDetail.AllPermissions ?? new List<PermissionViewModel>();
                var assignedIds = new HashSet<long>(roleDetail.AssignedPermissionIds ?? new List<long>());

                var assignedPermissions = allPermissions
                    .Where(p => assignedIds.Contains(p.Id))
                    .OrderBy(p => p.Name)
                    .ToList();

                var availablePermissions = allPermissions
                    .Where(p => !assignedIds.Contains(p.Id))
                    .OrderBy(p => p.Name)
                    .ToList();

                // Create BindingLists
                var assignedBindingList = new BindingList<PermissionViewModel>(assignedPermissions);
                var availableBindingList = new BindingList<PermissionViewModel>(availablePermissions);

                // Keep references for later use (assign/remove)
                _assignedPermissionsBindingList = assignedBindingList;
                _availablePermissionsBindingList = availableBindingList;

                // Ensure UI thread when setting DataSource
                if (InvokeRequired)
                {
                    Invoke(new Action(() =>
                    {
                        dgvAssignedPermissions.DataSource = _assignedPermissionsBindingList;
                        dgvAvailablePermissions.DataSource = _availablePermissionsBindingList;
                        // Refresh to force redraw
                        dgvAssignedPermissions.Refresh();
                        dgvAvailablePermissions.Refresh();
                    }));
                }
                else
                {
                    dgvAssignedPermissions.DataSource = _assignedPermissionsBindingList;
                    dgvAvailablePermissions.DataSource = _availablePermissionsBindingList;
                    dgvAssignedPermissions.Refresh();
                    dgvAvailablePermissions.Refresh();
                }
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
                    ShowError("Vui lòng nhập Tên quyền.");
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
                if (dgvAvailablePermissions.SelectedRows.Count == 0)
                {
                    ShowInfo("Vui lòng chọn quyền để gán.");
                    return;
                }

                SetLoadingState(true);

                var permissionsToAssign = dgvAvailablePermissions.SelectedRows
                    .Cast<DataGridViewRow>()
                    .Where(row => row.DataBoundItem != null)
                    .Select(row => ((PermissionViewModel)row.DataBoundItem).Id)
                    .ToList();

                var assignedSource = _assignedPermissionsBindingList ?? (dgvAssignedPermissions.DataSource as BindingList<PermissionViewModel>);
                var currentAssigned = assignedSource?.Select(p => p.Id).ToList() ?? new List<long>();

                var allAssigned = currentAssigned.Concat(permissionsToAssign).Distinct().ToList();

                await _presenter.AssignPermissionsToRoleAsync(_selectedRoleForAssignment.Id, allAssigned);

                ShowInfo($"Đã gán {permissionsToAssign.Count} quyền cho role {_selectedRoleForAssignment.Name}");

                await LoadRolePermissionsForAssignmentAsync(_selectedRoleForAssignment.Id);

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
        #endregion

        /// <summary>
        /// Custom drawing for tab headers to ensure tab text is visible on dark theme.
        /// This fixes invisible tab text when default OS theme overrides colors.
        /// </summary>
        private void TabControlMain_DrawItem(object? sender, DrawItemEventArgs e)
        {
            try
            {
                var g = e.Graphics;
                var tab = tabControlMain.TabPages[e.Index];
                var tabRect = tabControlMain.GetTabRect(e.Index);

                // Slightly shrink rect so borders look nicer
                var drawRect = new Rectangle(tabRect.X + 2, tabRect.Y + 2, tabRect.Width - 4, tabRect.Height - 4);

                bool selected = (e.Index == tabControlMain.SelectedIndex);

                // Background color for tab headers (selected vs normal)
                Color backColor = selected ? Color.FromArgb(107, 83, 255) : Color.FromArgb(42, 45, 86);
                using (var bgBrush = new SolidBrush(backColor))
                {
                    g.FillRectangle(bgBrush, drawRect);
                }

                // Text color
                Color textColor = selected ? Color.White : Color.FromArgb(124, 141, 181);

                // Draw text centered
                var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                using (var textBrush = new SolidBrush(textColor))
                {
                    g.DrawString(tab.Text, tabControlMain.Font, textBrush, drawRect, sf);
                }

                // optional border
                using (var pen = new Pen(Color.FromArgb(73, 75, 111)))
                {
                    g.DrawRectangle(pen, drawRect);
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Error drawing tab control header");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }

            }
            base.Dispose(disposing);
        }
    }
}