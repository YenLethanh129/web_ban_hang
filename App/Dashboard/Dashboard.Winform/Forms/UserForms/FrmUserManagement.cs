using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;

namespace Dashboard.Winform
{
    public partial class frmUserManagement : Form
    {
        private readonly UserManagementViewModel _model;
        private readonly ILogger<frmUserManagement>? _logger;
        private Button currentlySelectedTabButton = null!;

        public frmUserManagement(ILogger<frmUserManagement> logger)
        {
            InitializeComponent();
            _logger = logger;
            _model = new UserManagementViewModel();

            InitializeTabButtons();
            SetupDataBinding();
            SetupGrids();
            SetupEvents();

            // Set default tab
            btnUsersTab.Select();
            SetTabButtonUI(btnUsersTab);
            ShowUsersPanel();
        }

        #region Tab Navigation
        private void InitializeTabButtons()
        {
            btnUsersTab.Click += (s, e) => SwitchTab(s!, pnlUsers);
            btnRolesTab.Click += (s, e) => SwitchTab(s!, pnlRoles);
            btnPermissionsTab.Click += (s, e) => SwitchTab(s!, pnlPermissions);
        }

        private void SwitchTab(object sender, Panel targetPanel)
        {
            SetTabButtonUI((Button)sender);

            // Hide all panels
            pnlUsers.Visible = false;
            pnlRoles.Visible = false;
            pnlPermissions.Visible = false;

            // Show target panel
            targetPanel.Visible = true;
        }

        private void SetTabButtonUI(Button button)
        {
            // Reset all buttons
            ResetTabButtons();

            // Set selected button style
            button.BackColor = Color.FromArgb(107, 83, 255);
            button.ForeColor = Color.White;
            currentlySelectedTabButton = button;
        }

        private void ResetTabButtons()
        {
            var tabButtons = new[] { btnUsersTab, btnRolesTab, btnPermissionsTab };
            foreach (var btn in tabButtons)
            {
                btn.BackColor = Color.FromArgb(42, 45, 86);
                btn.ForeColor = Color.FromArgb(124, 141, 181);
            }
        }

        private void ShowUsersPanel()
        {
            pnlUsers.Visible = true;
            pnlRoles.Visible = false;
            pnlPermissions.Visible = false;
        }
        #endregion

        #region Data Binding
        private void SetupDataBinding()
        {
            // Bind summary labels
            lblTotalUsers.DataBindings.Add("Text", _model, nameof(_model.TotalUsers));
            lblActiveUsers.DataBindings.Add("Text", _model, nameof(_model.ActiveUsers));
            lblInactiveUsers.DataBindings.Add("Text", _model, nameof(_model.InactiveUsers));
            lblTotalRoles.DataBindings.Add("Text", _model, nameof(_model.TotalRoles));

            // Bind search textbox
            txtSearch.DataBindings.Add("Text", _model, nameof(_model.SearchText));

            // Bind filter checkbox
            chkActiveOnly.DataBindings.Add("Checked", _model, nameof(_model.IsActiveFilter));

            // Bind grids
            dgvUsers.DataSource = _model;
            dgvUsers.DataMember = nameof(_model.FilteredUsers);

            dgvRoles.DataSource = _model;
            dgvRoles.DataMember = nameof(_model.Roles);

            dgvPermissions.DataSource = _model;
            dgvPermissions.DataMember = nameof(_model.Permissions);

            dgvRolePermissions.DataSource = _model;
            dgvRolePermissions.DataMember = nameof(_model.RolePermissions);
        }
        #endregion

        #region Setup Grids
        private void SetupGrids()
        {
            SetupUsersGrid();
            SetupRolesGrid();
            SetupPermissionsGrid();
            SetupRolePermissionsGrid();
        }

        private void SetupUsersGrid()
        {
            dgvUsers.AutoGenerateColumns = false;
            dgvUsers.Columns.Clear();

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserViewModel.Id),
                HeaderText = "ID",
                Name = "Id",
                Width = 60,
                Visible = false
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserViewModel.Fullname),
                HeaderText = "Họ tên",
                Name = "Fullname",
                MinimumWidth = 150
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserViewModel.PhoneNumber),
                HeaderText = "Số điện thoại",
                Name = "PhoneNumber",
                Width = 120
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserViewModel.RoleName),
                HeaderText = "Vai trò",
                Name = "RoleName",
                Width = 100
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserViewModel.StatusText),
                HeaderText = "Trạng thái",
                Name = "StatusText",
                Width = 100
            });

            dgvUsers.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(UserViewModel.CreatedAtFormatted),
                HeaderText = "Ngày tạo",
                Name = "CreatedAtFormatted",
                Width = 120
            });

            ApplyGridTheme(dgvUsers);
        }

        private void SetupRolesGrid()
        {
            dgvRoles.AutoGenerateColumns = false;
            dgvRoles.Columns.Clear();

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.Id),
                HeaderText = "ID",
                Name = "Id",
                Width = 60,
                Visible = false
            });

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.Name),
                HeaderText = "Tên vai trò",
                Name = "Name",
                MinimumWidth = 150
            });

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.Description),
                HeaderText = "Mô tả",
                Name = "Description",
                MinimumWidth = 200
            });

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.UserCount),
                HeaderText = "Số người dùng",
                Name = "UserCount",
                Width = 120
            });

            dgvRoles.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(RoleViewModel.CreatedAtFormatted),
                HeaderText = "Ngày tạo",
                Name = "CreatedAtFormatted",
                Width = 120
            });

            ApplyGridTheme(dgvRoles);
        }

        private void SetupPermissionsGrid()
        {
            dgvPermissions.AutoGenerateColumns = false;
            dgvPermissions.Columns.Clear();

            dgvPermissions.Columns.Add(new DataGridViewCheckBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.IsAssignedToRole),
                HeaderText = "Chọn",
                Name = "IsAssignedToRole",
                Width = 60
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Name),
                HeaderText = "Tên quyền",
                Name = "Name",
                MinimumWidth = 150
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Resource),
                HeaderText = "Tài nguyên",
                Name = "Resource",
                Width = 100
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Action),
                HeaderText = "Hành động",
                Name = "Action",
                Width = 100
            });

            dgvPermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Description),
                HeaderText = "Mô tả",
                Name = "Description",
                MinimumWidth = 200
            });

            ApplyGridTheme(dgvPermissions);
        }

        private void SetupRolePermissionsGrid()
        {
            dgvRolePermissions.AutoGenerateColumns = false;
            dgvRolePermissions.Columns.Clear();

            dgvRolePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Name),
                HeaderText = "Tên quyền",
                Name = "Name",
                MinimumWidth = 150
            });

            dgvRolePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Resource),
                HeaderText = "Tài nguyên",
                Name = "Resource",
                Width = 100
            });

            dgvRolePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Action),
                HeaderText = "Hành động",
                Name = "Action",
                Width = 100
            });

            dgvRolePermissions.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PermissionViewModel.Description),
                HeaderText = "Mô tả",
                Name = "Description",
                MinimumWidth = 200
            });

            ApplyGridTheme(dgvRolePermissions);
        }

        private void ApplyGridTheme(DataGridView dgv)
        {
            Color backColor = Color.FromArgb(42, 45, 86);

            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.ReadOnly = true;
            dgv.MultiSelect = false;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.EnableHeadersVisualStyles = false;

            dgv.BackgroundColor = backColor;
            dgv.DefaultCellStyle.BackColor = backColor;
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = backColor;

            dgv.ColumnHeadersDefaultCellStyle.BackColor = backColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(124, 141, 181);
            dgv.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgv.ColumnHeadersDefaultCellStyle.BackColor;
            dgv.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgv.ColumnHeadersDefaultCellStyle.ForeColor;

            dgv.GridColor = Color.FromArgb(73, 75, 111);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            dgv.RowPrePaint += Dgv_RowPrePaint;
        }

        private void Dgv_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid == null || e.RowIndex < 0) return;

            var row = grid.Rows[e.RowIndex];
            if (row.Selected)
            {
                row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor;
                row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(73, 75, 111);
            }
        }
        #endregion

        #region Events
        private void SetupEvents()
        {
            dgvRoles.SelectionChanged += DgvRoles_SelectionChanged;
            dgvUsers.SelectionChanged += DgvUsers_SelectionChanged;

            btnAddUser.Click += BtnAddUser_Click;
            btnEditUser.Click += BtnEditUser_Click;
            btnDeleteUser.Click += BtnDeleteUser_Click;
            btnResetPassword.Click += BtnResetPassword_Click;

            btnAddRole.Click += BtnAddRole_Click;
            btnEditRole.Click += BtnEditRole_Click;
            btnDeleteRole.Click += BtnDeleteRole_Click;
            btnSaveRolePermissions.Click += BtnSaveRolePermissions_Click;

            btnRefresh.Click += BtnRefresh_Click;
        }

        private void DgvRoles_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvRoles.CurrentRow?.DataBoundItem is RoleViewModel selectedRole)
            {
                _model.SelectedRole = selectedRole;
            }
        }

        private void DgvUsers_SelectionChanged(object? sender, EventArgs e)
        {
            if (dgvUsers.CurrentRow?.DataBoundItem is UserViewModel selectedUser)
            {
                _model.SelectedUser = selectedUser;
            }
        }
        #endregion

        #region Button Events
        private void BtnAddUser_Click(object? sender, EventArgs e)
        {
            _logger?.LogInformation("Add user button clicked");
            // TODO: Open add user dialog
        }

        private void BtnEditUser_Click(object? sender, EventArgs e)
        {
            if (_model.SelectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng để chỉnh sửa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _logger?.LogInformation("Edit user {UserId}", _model.SelectedUser.Id);
            // TODO: Open edit user dialog
        }

        private void BtnDeleteUser_Click(object? sender, EventArgs e)
        {
            if (_model.SelectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng để xóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa người dùng '{_model.SelectedUser.Fullname}'?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _logger?.LogInformation("Delete user {UserId}", _model.SelectedUser.Id);
                // TODO: Delete user logic
            }
        }

        private void BtnResetPassword_Click(object? sender, EventArgs e)
        {
            if (_model.SelectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn người dùng để reset mật khẩu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn reset mật khẩu cho '{_model.SelectedUser.Fullname}'?",
                "Xác nhận reset", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                _logger?.LogInformation("Reset password for user {UserId}", _model.SelectedUser.Id);
                // TODO: Reset password logic
            }
        }

        private void BtnAddRole_Click(object? sender, EventArgs e)
        {
            _logger?.LogInformation("Add role button clicked");
            // TODO: Open add role dialog
        }

        private void BtnEditRole_Click(object? sender, EventArgs e)
        {
            if (_model.SelectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò để chỉnh sửa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _logger?.LogInformation("Edit role {RoleId}", _model.SelectedRole.Id);
            // TODO: Open edit role dialog
        }

        private void BtnDeleteRole_Click(object? sender, EventArgs e)
        {
            if (_model.SelectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò để xóa.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (_model.SelectedRole.UserCount > 0)
            {
                MessageBox.Show("Không thể xóa vai trò này vì đang có người dùng sử dụng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var result = MessageBox.Show($"Bạn có chắc chắn muốn xóa vai trò '{_model.SelectedRole.Name}'?",
                "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                _logger?.LogInformation("Delete role {RoleId}", _model.SelectedRole.Id);
                // TODO: Delete role logic
            }
        }

        private void BtnSaveRolePermissions_Click(object? sender, EventArgs e)
        {
            if (_model.SelectedRole == null)
            {
                MessageBox.Show("Vui lòng chọn vai trò để cập nhật quyền.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _logger?.LogInformation("Save permissions for role {RoleId}", _model.SelectedRole.Id);
            // TODO: Save role permissions logic
        }

        private void BtnRefresh_Click(object? sender, EventArgs e)
        {
            _logger?.LogInformation("Refresh data");
            LoadData();
        }
        #endregion

        #region Data Loading
        private async void LoadData()
        {
            try
            {
                _logger?.LogInformation("Loading user management data...");

                // TODO: Load data from your data service
                // This is where you would call your business logic layer
                // await _userService.LoadUsersAsync();
                // await _roleService.LoadRolesAsync();
                // await _permissionService.LoadPermissionsAsync();
                await Task.Delay(50);
                _logger?.LogInformation("User management data loaded successfully");
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Error loading user management data");
                MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmUserManagement_Load(object sender, EventArgs e)
        {
            LoadData();
        }
        #endregion

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                // Cleanup event subscriptions
                if (dgvUsers != null)
                {
                    dgvUsers.RowPrePaint -= Dgv_RowPrePaint;
                    dgvUsers.SelectionChanged -= DgvUsers_SelectionChanged;
                }

                if (dgvRoles != null)
                {
                    dgvRoles.RowPrePaint -= Dgv_RowPrePaint;
                    dgvRoles.SelectionChanged -= DgvRoles_SelectionChanged;
                }

                if (dgvPermissions != null)
                {
                    dgvPermissions.RowPrePaint -= Dgv_RowPrePaint;
                }

                if (dgvRolePermissions != null)
                {
                    dgvRolePermissions.RowPrePaint -= Dgv_RowPrePaint;
                }
            }
            base.Dispose(disposing);
        }
    }
}