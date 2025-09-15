using static ReaLTaiizor.Drawing.Poison.PoisonPaint;
using System.Drawing;

namespace Dashboard.Winform.Forms
{
    partial class FrmRolePermissionManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            tabControlMain = new TabControl();
            tabPageList = new TabPage();
            splitContainer = new SplitContainer();
            dgvRoles = new DataGridView();
            dgvPermissions = new DataGridView();
            tabPageRoleEdit = new TabPage();
            pnlRoleEdit = new Panel();
            lblRoleName = new Label();
            txtRoleName = new TextBox();
            lblRoleDescription = new Label();
            txtRoleDescription = new TextBox();
            btnSaveRole = new Button();
            btnCancelRole = new Button();
            tabPagePermissionEdit = new TabPage();
            pnlPermissionEdit = new Panel();
            lblPermissionName = new Label();
            txtPermissionName = new TextBox();
            lblPermissionDescription = new Label();
            txtPermissionDescription = new TextBox();
            lblPermissionResource = new Label();
            txtPermissionResource = new TextBox();
            lblPermissionAction = new Label();
            txtPermissionAction = new TextBox();
            btnSavePermission = new Button();
            btnCancelPermission = new Button();
            tabPageRolePermissionAssign = new TabPage();
            pnlRolePermissionAssign = new Panel();
            lblSelectedRole = new Label();
            lblSelectedRoleValue = new Label();
            dgvAvailablePermissions = new DataGridView();
            btnAssignPermissions = new Button();
            btnUnassignPermissions = new Button();
            tabControlMain.SuspendLayout();
            tabPageList.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer).BeginInit();
            splitContainer.Panel1.SuspendLayout();
            splitContainer.Panel2.SuspendLayout();
            splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvRoles).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvPermissions).BeginInit();
            tabPageRoleEdit.SuspendLayout();
            pnlRoleEdit.SuspendLayout();
            tabPagePermissionEdit.SuspendLayout();
            pnlPermissionEdit.SuspendLayout();
            tabPageRolePermissionAssign.SuspendLayout();
            pnlRolePermissionAssign.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAvailablePermissions).BeginInit();
            SuspendLayout();
            // 
            // tabControlMain
            // 
            tabControlMain.Appearance = TabAppearance.FlatButtons;
            tabControlMain.Controls.Add(tabPageList);
            tabControlMain.Controls.Add(tabPageRoleEdit);
            tabControlMain.Controls.Add(tabPagePermissionEdit);
            tabControlMain.Controls.Add(tabPageRolePermissionAssign);
            tabControlMain.Dock = DockStyle.Fill;
            tabControlMain.Location = new Point(10, 25);
            tabControlMain.Name = "tabControlMain";
            tabControlMain.SelectedIndex = 0;
            tabControlMain.Size = new Size(960, 590);
            tabControlMain.TabIndex = 0;
            // 
            // tabPageList
            // 
            tabPageList.BackColor = Color.FromArgb(42, 45, 86);
            tabPageList.Controls.Add(splitContainer);
            tabPageList.ForeColor = Color.White;
            tabPageList.Location = new Point(4, 27);
            tabPageList.Name = "tabPageList";
            tabPageList.Padding = new Padding(3);
            tabPageList.Size = new Size(952, 559);
            tabPageList.TabIndex = 0;
            tabPageList.Text = "Danh sách Role & Permission";
            // 
            // splitContainer
            // 
            splitContainer.BackColor = Color.FromArgb(42, 45, 86);
            splitContainer.Dock = DockStyle.Fill;
            splitContainer.Location = new Point(3, 3);
            splitContainer.Name = "splitContainer";
            splitContainer.Orientation = Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            splitContainer.Panel1.BackColor = Color.FromArgb(42, 45, 86);
            splitContainer.Panel1.Controls.Add(dgvRoles);
            // 
            // splitContainer.Panel2
            // 
            splitContainer.Panel2.BackColor = Color.FromArgb(42, 45, 86);
            splitContainer.Panel2.Controls.Add(dgvPermissions);
            splitContainer.Size = new Size(946, 553);
            splitContainer.SplitterDistance = 275;
            splitContainer.TabIndex = 0;
            // 
            // dgvRoles
            // 
            dgvRoles.AllowUserToAddRows = false;
            dgvRoles.AllowUserToDeleteRows = false;
            dgvRoles.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(42, 45, 86);
            dgvRoles.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvRoles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRoles.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvRoles.BorderStyle = BorderStyle.None;
            dgvRoles.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRoles.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvRoles.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvRoles.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvRoles.DefaultCellStyle = dataGridViewCellStyle3;
            dgvRoles.Dock = DockStyle.Fill;
            dgvRoles.EnableHeadersVisualStyles = false;
            dgvRoles.GridColor = Color.FromArgb(73, 75, 111);
            dgvRoles.Location = new Point(0, 0);
            dgvRoles.MultiSelect = false;
            dgvRoles.Name = "dgvRoles";
            dgvRoles.ReadOnly = true;
            dgvRoles.RowHeadersVisible = false;
            dgvRoles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRoles.Size = new Size(946, 275);
            dgvRoles.TabIndex = 0;
            // 
            // dgvPermissions
            // 
            dgvPermissions.AllowUserToAddRows = false;
            dgvPermissions.AllowUserToDeleteRows = false;
            dgvPermissions.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(42, 45, 86);
            dgvPermissions.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvPermissions.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPermissions.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvPermissions.BorderStyle = BorderStyle.None;
            dgvPermissions.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPermissions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvPermissions.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvPermissions.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.White;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvPermissions.DefaultCellStyle = dataGridViewCellStyle6;
            dgvPermissions.Dock = DockStyle.Fill;
            dgvPermissions.EnableHeadersVisualStyles = false;
            dgvPermissions.GridColor = Color.FromArgb(73, 75, 111);
            dgvPermissions.Location = new Point(0, 0);
            dgvPermissions.MultiSelect = false;
            dgvPermissions.Name = "dgvPermissions";
            dgvPermissions.ReadOnly = true;
            dgvPermissions.RowHeadersVisible = false;
            dgvPermissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPermissions.Size = new Size(946, 274);
            dgvPermissions.TabIndex = 1;
            // 
            // tabPageRoleEdit
            // 
            tabPageRoleEdit.BackColor = Color.FromArgb(42, 45, 86);
            tabPageRoleEdit.Controls.Add(pnlRoleEdit);
            tabPageRoleEdit.ForeColor = Color.White;
            tabPageRoleEdit.Location = new Point(4, 27);
            tabPageRoleEdit.Name = "tabPageRoleEdit";
            tabPageRoleEdit.Padding = new Padding(3);
            tabPageRoleEdit.Size = new Size(952, 559);
            tabPageRoleEdit.TabIndex = 1;
            tabPageRoleEdit.Text = "Thêm/Sửa Role";
            // 
            // pnlRoleEdit
            // 
            pnlRoleEdit.BackColor = Color.FromArgb(42, 45, 86);
            pnlRoleEdit.Controls.Add(lblRoleName);
            pnlRoleEdit.Controls.Add(txtRoleName);
            pnlRoleEdit.Controls.Add(lblRoleDescription);
            pnlRoleEdit.Controls.Add(txtRoleDescription);
            pnlRoleEdit.Controls.Add(btnSaveRole);
            pnlRoleEdit.Controls.Add(btnCancelRole);
            pnlRoleEdit.Dock = DockStyle.Fill;
            pnlRoleEdit.Location = new Point(3, 3);
            pnlRoleEdit.Name = "pnlRoleEdit";
            pnlRoleEdit.Padding = new Padding(20);
            pnlRoleEdit.Size = new Size(946, 553);
            pnlRoleEdit.TabIndex = 0;
            // 
            // lblRoleName
            // 
            lblRoleName.AutoSize = true;
            lblRoleName.Font = new Font("Segoe UI", 10F);
            lblRoleName.ForeColor = Color.FromArgb(124, 141, 181);
            lblRoleName.Location = new Point(23, 23);
            lblRoleName.Name = "lblRoleName";
            lblRoleName.Size = new Size(63, 19);
            lblRoleName.TabIndex = 0;
            lblRoleName.Text = "Tên Role:";
            // 
            // txtRoleName
            // 
            txtRoleName.BackColor = Color.FromArgb(24, 28, 63);
            txtRoleName.BorderStyle = BorderStyle.FixedSingle;
            txtRoleName.Font = new Font("Segoe UI", 10F);
            txtRoleName.ForeColor = Color.White;
            txtRoleName.Location = new Point(23, 45);
            txtRoleName.Name = "txtRoleName";
            txtRoleName.Size = new Size(400, 25);
            txtRoleName.TabIndex = 1;
            // 
            // lblRoleDescription
            // 
            lblRoleDescription.AutoSize = true;
            lblRoleDescription.Font = new Font("Segoe UI", 10F);
            lblRoleDescription.ForeColor = Color.FromArgb(124, 141, 181);
            lblRoleDescription.Location = new Point(23, 83);
            lblRoleDescription.Name = "lblRoleDescription";
            lblRoleDescription.Size = new Size(49, 19);
            lblRoleDescription.TabIndex = 2;
            lblRoleDescription.Text = "Mô tả:";
            // 
            // txtRoleDescription
            // 
            txtRoleDescription.BackColor = Color.FromArgb(24, 28, 63);
            txtRoleDescription.BorderStyle = BorderStyle.FixedSingle;
            txtRoleDescription.Font = new Font("Segoe UI", 10F);
            txtRoleDescription.ForeColor = Color.White;
            txtRoleDescription.Location = new Point(23, 105);
            txtRoleDescription.Multiline = true;
            txtRoleDescription.Name = "txtRoleDescription";
            txtRoleDescription.Size = new Size(400, 80);
            txtRoleDescription.TabIndex = 3;
            // 
            // btnSaveRole
            // 
            btnSaveRole.BackColor = Color.FromArgb(107, 83, 255);
            btnSaveRole.FlatStyle = FlatStyle.Flat;
            btnSaveRole.Font = new Font("Segoe UI", 10F);
            btnSaveRole.ForeColor = Color.White;
            btnSaveRole.Location = new Point(23, 205);
            btnSaveRole.Name = "btnSaveRole";
            btnSaveRole.Size = new Size(100, 35);
            btnSaveRole.TabIndex = 4;
            btnSaveRole.Text = "Lưu";
            btnSaveRole.UseVisualStyleBackColor = false;
            // 
            // btnCancelRole
            // 
            btnCancelRole.BackColor = Color.FromArgb(73, 75, 111);
            btnCancelRole.FlatStyle = FlatStyle.Flat;
            btnCancelRole.Font = new Font("Segoe UI", 10F);
            btnCancelRole.ForeColor = Color.White;
            btnCancelRole.Location = new Point(133, 205);
            btnCancelRole.Name = "btnCancelRole";
            btnCancelRole.Size = new Size(100, 35);
            btnCancelRole.TabIndex = 5;
            btnCancelRole.Text = "Hủy";
            btnCancelRole.UseVisualStyleBackColor = false;
            // 
            // tabPagePermissionEdit
            // 
            tabPagePermissionEdit.BackColor = Color.FromArgb(42, 45, 86);
            tabPagePermissionEdit.Controls.Add(pnlPermissionEdit);
            tabPagePermissionEdit.ForeColor = Color.White;
            tabPagePermissionEdit.Location = new Point(4, 27);
            tabPagePermissionEdit.Name = "tabPagePermissionEdit";
            tabPagePermissionEdit.Padding = new Padding(3);
            tabPagePermissionEdit.Size = new Size(952, 559);
            tabPagePermissionEdit.TabIndex = 2;
            tabPagePermissionEdit.Text = "Thêm/Sửa Permission";
            // 
            // pnlPermissionEdit
            // 
            pnlPermissionEdit.BackColor = Color.FromArgb(42, 45, 86);
            pnlPermissionEdit.Controls.Add(lblPermissionName);
            pnlPermissionEdit.Controls.Add(txtPermissionName);
            pnlPermissionEdit.Controls.Add(lblPermissionDescription);
            pnlPermissionEdit.Controls.Add(txtPermissionDescription);
            pnlPermissionEdit.Controls.Add(lblPermissionResource);
            pnlPermissionEdit.Controls.Add(txtPermissionResource);
            pnlPermissionEdit.Controls.Add(lblPermissionAction);
            pnlPermissionEdit.Controls.Add(txtPermissionAction);
            pnlPermissionEdit.Controls.Add(btnSavePermission);
            pnlPermissionEdit.Controls.Add(btnCancelPermission);
            pnlPermissionEdit.Dock = DockStyle.Fill;
            pnlPermissionEdit.Location = new Point(3, 3);
            pnlPermissionEdit.Name = "pnlPermissionEdit";
            pnlPermissionEdit.Padding = new Padding(20);
            pnlPermissionEdit.Size = new Size(946, 553);
            pnlPermissionEdit.TabIndex = 0;
            // 
            // lblPermissionName
            // 
            lblPermissionName.AutoSize = true;
            lblPermissionName.Font = new Font("Segoe UI", 10F);
            lblPermissionName.ForeColor = Color.FromArgb(124, 141, 181);
            lblPermissionName.Location = new Point(23, 23);
            lblPermissionName.Name = "lblPermissionName";
            lblPermissionName.Size = new Size(102, 19);
            lblPermissionName.TabIndex = 0;
            lblPermissionName.Text = "Tên Permission:";
            // 
            // txtPermissionName
            // 
            txtPermissionName.BackColor = Color.FromArgb(24, 28, 63);
            txtPermissionName.BorderStyle = BorderStyle.FixedSingle;
            txtPermissionName.Font = new Font("Segoe UI", 10F);
            txtPermissionName.ForeColor = Color.White;
            txtPermissionName.Location = new Point(23, 45);
            txtPermissionName.Name = "txtPermissionName";
            txtPermissionName.Size = new Size(400, 25);
            txtPermissionName.TabIndex = 1;
            // 
            // lblPermissionDescription
            // 
            lblPermissionDescription.AutoSize = true;
            lblPermissionDescription.Font = new Font("Segoe UI", 10F);
            lblPermissionDescription.ForeColor = Color.FromArgb(124, 141, 181);
            lblPermissionDescription.Location = new Point(23, 83);
            lblPermissionDescription.Name = "lblPermissionDescription";
            lblPermissionDescription.Size = new Size(49, 19);
            lblPermissionDescription.TabIndex = 2;
            lblPermissionDescription.Text = "Mô tả:";
            // 
            // txtPermissionDescription
            // 
            txtPermissionDescription.BackColor = Color.FromArgb(24, 28, 63);
            txtPermissionDescription.BorderStyle = BorderStyle.FixedSingle;
            txtPermissionDescription.Font = new Font("Segoe UI", 10F);
            txtPermissionDescription.ForeColor = Color.White;
            txtPermissionDescription.Location = new Point(23, 105);
            txtPermissionDescription.Multiline = true;
            txtPermissionDescription.Name = "txtPermissionDescription";
            txtPermissionDescription.Size = new Size(400, 60);
            txtPermissionDescription.TabIndex = 3;
            // 
            // lblPermissionResource
            // 
            lblPermissionResource.AutoSize = true;
            lblPermissionResource.Font = new Font("Segoe UI", 10F);
            lblPermissionResource.ForeColor = Color.FromArgb(124, 141, 181);
            lblPermissionResource.Location = new Point(23, 183);
            lblPermissionResource.Name = "lblPermissionResource";
            lblPermissionResource.Size = new Size(67, 19);
            lblPermissionResource.TabIndex = 4;
            lblPermissionResource.Text = "Resource:";
            // 
            // txtPermissionResource
            // 
            txtPermissionResource.BackColor = Color.FromArgb(24, 28, 63);
            txtPermissionResource.BorderStyle = BorderStyle.FixedSingle;
            txtPermissionResource.Font = new Font("Segoe UI", 10F);
            txtPermissionResource.ForeColor = Color.White;
            txtPermissionResource.Location = new Point(23, 205);
            txtPermissionResource.Name = "txtPermissionResource";
            txtPermissionResource.Size = new Size(195, 25);
            txtPermissionResource.TabIndex = 5;
            // 
            // lblPermissionAction
            // 
            lblPermissionAction.AutoSize = true;
            lblPermissionAction.Font = new Font("Segoe UI", 10F);
            lblPermissionAction.ForeColor = Color.FromArgb(124, 141, 181);
            lblPermissionAction.Location = new Point(228, 183);
            lblPermissionAction.Name = "lblPermissionAction";
            lblPermissionAction.Size = new Size(51, 19);
            lblPermissionAction.TabIndex = 6;
            lblPermissionAction.Text = "Action:";
            // 
            // txtPermissionAction
            // 
            txtPermissionAction.BackColor = Color.FromArgb(24, 28, 63);
            txtPermissionAction.BorderStyle = BorderStyle.FixedSingle;
            txtPermissionAction.Font = new Font("Segoe UI", 10F);
            txtPermissionAction.ForeColor = Color.White;
            txtPermissionAction.Location = new Point(228, 205);
            txtPermissionAction.Name = "txtPermissionAction";
            txtPermissionAction.Size = new Size(195, 25);
            txtPermissionAction.TabIndex = 7;
            // 
            // btnSavePermission
            // 
            btnSavePermission.BackColor = Color.FromArgb(107, 83, 255);
            btnSavePermission.FlatStyle = FlatStyle.Flat;
            btnSavePermission.Font = new Font("Segoe UI", 10F);
            btnSavePermission.ForeColor = Color.White;
            btnSavePermission.Location = new Point(23, 250);
            btnSavePermission.Name = "btnSavePermission";
            btnSavePermission.Size = new Size(100, 35);
            btnSavePermission.TabIndex = 8;
            btnSavePermission.Text = "Lưu";
            btnSavePermission.UseVisualStyleBackColor = false;
            // 
            // btnCancelPermission
            // 
            btnCancelPermission.BackColor = Color.FromArgb(73, 75, 111);
            btnCancelPermission.FlatStyle = FlatStyle.Flat;
            btnCancelPermission.Font = new Font("Segoe UI", 10F);
            btnCancelPermission.ForeColor = Color.White;
            btnCancelPermission.Location = new Point(133, 250);
            btnCancelPermission.Name = "btnCancelPermission";
            btnCancelPermission.Size = new Size(100, 35);
            btnCancelPermission.TabIndex = 9;
            btnCancelPermission.Text = "Hủy";
            btnCancelPermission.UseVisualStyleBackColor = false;
            // 
            // tabPageRolePermissionAssign
            // 
            tabPageRolePermissionAssign.BackColor = Color.FromArgb(42, 45, 86);
            tabPageRolePermissionAssign.Controls.Add(pnlRolePermissionAssign);
            tabPageRolePermissionAssign.ForeColor = Color.White;
            tabPageRolePermissionAssign.Location = new Point(4, 27);
            tabPageRolePermissionAssign.Name = "tabPageRolePermissionAssign";
            tabPageRolePermissionAssign.Padding = new Padding(3);
            tabPageRolePermissionAssign.Size = new Size(952, 559);
            tabPageRolePermissionAssign.TabIndex = 3;
            tabPageRolePermissionAssign.Text = "Gán quyền cho Role";
            // 
            // pnlRolePermissionAssign
            // 
            pnlRolePermissionAssign.BackColor = Color.FromArgb(42, 45, 86);
            pnlRolePermissionAssign.Controls.Add(lblSelectedRole);
            pnlRolePermissionAssign.Controls.Add(lblSelectedRoleValue);
            pnlRolePermissionAssign.Controls.Add(dgvAvailablePermissions);
            pnlRolePermissionAssign.Controls.Add(btnAssignPermissions);
            pnlRolePermissionAssign.Controls.Add(btnUnassignPermissions);
            pnlRolePermissionAssign.Dock = DockStyle.Fill;
            pnlRolePermissionAssign.Location = new Point(3, 3);
            pnlRolePermissionAssign.Name = "pnlRolePermissionAssign";
            pnlRolePermissionAssign.Padding = new Padding(20);
            pnlRolePermissionAssign.Size = new Size(946, 553);
            pnlRolePermissionAssign.TabIndex = 0;
            // 
            // lblSelectedRole
            // 
            lblSelectedRole.AutoSize = true;
            lblSelectedRole.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            lblSelectedRole.ForeColor = Color.FromArgb(124, 141, 181);
            lblSelectedRole.Location = new Point(23, 23);
            lblSelectedRole.Name = "lblSelectedRole";
            lblSelectedRole.Size = new Size(113, 21);
            lblSelectedRole.TabIndex = 0;
            lblSelectedRole.Text = "Role đã chọn:";
            // 
            // lblSelectedRoleValue
            // 
            lblSelectedRoleValue.AutoSize = true;
            lblSelectedRoleValue.Font = new Font("Segoe UI", 12F);
            lblSelectedRoleValue.ForeColor = Color.White;
            lblSelectedRoleValue.Location = new Point(137, 23);
            lblSelectedRoleValue.Name = "lblSelectedRoleValue";
            lblSelectedRoleValue.Size = new Size(155, 21);
            lblSelectedRoleValue.TabIndex = 1;
            lblSelectedRoleValue.Text = "[Chưa chọn role nào]";
            // 
            // dgvAvailablePermissions
            // 
            dgvAvailablePermissions.AllowUserToAddRows = false;
            dgvAvailablePermissions.AllowUserToDeleteRows = false;
            dgvAvailablePermissions.AllowUserToResizeRows = false;
            dgvAvailablePermissions.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvAvailablePermissions.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvAvailablePermissions.BorderStyle = BorderStyle.None;
            dgvAvailablePermissions.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvAvailablePermissions.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvAvailablePermissions.EnableHeadersVisualStyles = false;
            dgvAvailablePermissions.GridColor = Color.FromArgb(73, 75, 111);
            dgvAvailablePermissions.Location = new Point(23, 60);
            dgvAvailablePermissions.Name = "dgvAvailablePermissions";
            dgvAvailablePermissions.RowHeadersVisible = false;
            dgvAvailablePermissions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAvailablePermissions.Size = new Size(900, 400);
            dgvAvailablePermissions.TabIndex = 2;
            // 
            // btnAssignPermissions
            // 
            btnAssignPermissions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnAssignPermissions.BackColor = Color.FromArgb(107, 83, 255);
            btnAssignPermissions.FlatStyle = FlatStyle.Flat;
            btnAssignPermissions.Font = new Font("Segoe UI", 10F);
            btnAssignPermissions.ForeColor = Color.White;
            btnAssignPermissions.Location = new Point(23, 480);
            btnAssignPermissions.Name = "btnAssignPermissions";
            btnAssignPermissions.Size = new Size(178, 35);
            btnAssignPermissions.TabIndex = 3;
            btnAssignPermissions.Text = "Gán quyền được chọn";
            btnAssignPermissions.UseVisualStyleBackColor = false;
            // 
            // btnUnassignPermissions
            // 
            btnUnassignPermissions.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnUnassignPermissions.BackColor = Color.FromArgb(220, 53, 69);
            btnUnassignPermissions.FlatStyle = FlatStyle.Flat;
            btnUnassignPermissions.Font = new Font("Segoe UI", 10F);
            btnUnassignPermissions.ForeColor = Color.White;
            btnUnassignPermissions.Location = new Point(207, 480);
            btnUnassignPermissions.Name = "btnUnassignPermissions";
            btnUnassignPermissions.Size = new Size(167, 35);
            btnUnassignPermissions.TabIndex = 4;
            btnUnassignPermissions.Text = "Gỡ quyền được chọn";
            btnUnassignPermissions.UseVisualStyleBackColor = false;
            // 
            // FrmRolePermissionManagement
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(980, 640);
            Controls.Add(tabControlMain);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmRolePermissionManagement";
            Padding = new Padding(10, 25, 10, 25);
            Text = "Quản lý Role & Permission";
            tabControlMain.ResumeLayout(false);
            tabPageList.ResumeLayout(false);
            splitContainer.Panel1.ResumeLayout(false);
            splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer).EndInit();
            splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvRoles).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvPermissions).EndInit();
            tabPageRoleEdit.ResumeLayout(false);
            pnlRoleEdit.ResumeLayout(false);
            pnlRoleEdit.PerformLayout();
            tabPagePermissionEdit.ResumeLayout(false);
            pnlPermissionEdit.ResumeLayout(false);
            pnlPermissionEdit.PerformLayout();
            tabPageRolePermissionAssign.ResumeLayout(false);
            pnlRolePermissionAssign.ResumeLayout(false);
            pnlRolePermissionAssign.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAvailablePermissions).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private TabControl tabControlMain;
        private TabPage tabPageList;
        private SplitContainer splitContainer;
        private DataGridView dgvRoles;
        private DataGridView dgvPermissions;

        private TabPage tabPageRoleEdit;
        private Panel pnlRoleEdit;
        private Label lblRoleName;
        private TextBox txtRoleName;
        private Label lblRoleDescription;
        private TextBox txtRoleDescription;
        private Button btnSaveRole;
        private Button btnCancelRole;

        private TabPage tabPagePermissionEdit;
        private Panel pnlPermissionEdit;
        private Label lblPermissionName;
        private TextBox txtPermissionName;
        private Label lblPermissionDescription;
        private TextBox txtPermissionDescription;
        private Label lblPermissionResource;
        private TextBox txtPermissionResource;
        private Label lblPermissionAction;
        private TextBox txtPermissionAction;
        private Button btnSavePermission;
        private Button btnCancelPermission;

        private TabPage tabPageRolePermissionAssign;
        private Panel pnlRolePermissionAssign;
        private Label lblSelectedRole;
        private Label lblSelectedRoleValue;
        private DataGridView dgvAvailablePermissions;
        private Button btnAssignPermissions;
        private Button btnUnassignPermissions;
    }
}