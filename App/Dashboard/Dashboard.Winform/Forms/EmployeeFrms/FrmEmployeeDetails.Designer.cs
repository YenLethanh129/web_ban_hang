namespace Dashboard.Winform.Forms
{
    partial class FrmEmployeeDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl = new TabControl();
            tabBasicInfo = new TabPage();
            pnlBasicInfo = new Panel();
            lblEmployeeId = new Label();
            txtEmployeeId = new TextBox();
            lblFullName = new Label();
            txtFullName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblBranch = new Label();
            cbxBranch = new ComboBox();
            lblPosition = new Label();
            cbxPosition = new ComboBox();
            lblStatus = new Label();
            cbxStatus = new ComboBox();
            lblHireDate = new Label();
            dtpHireDate = new DateTimePicker();
            lblResignDate = new Label();
            chkResignDate = new CheckBox();
            dtpResignDate = new DateTimePicker();
            lblCreatedAtLabel = new Label();
            lblCreatedAt = new Label();
            lblUpdatedAtLabel = new Label();
            lblUpdatedAt = new Label();
            tabAccountInfo = new TabPage();
            pnlAccountInfo = new Panel();
            chkHasAccount = new CheckBox();
            grpAccountInfo = new GroupBox();
            lblUsername = new Label();
            txtUsername = new TextBox();
            lblRole = new Label();
            txtRole = new TextBox();
            pnlButtons = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            btnClose = new Button();
            tabControl.SuspendLayout();
            tabBasicInfo.SuspendLayout();
            pnlBasicInfo.SuspendLayout();
            tabAccountInfo.SuspendLayout();
            pnlAccountInfo.SuspendLayout();
            grpAccountInfo.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabBasicInfo);
            tabControl.Controls.Add(tabAccountInfo);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Font = new Font("Segoe UI", 9F);
            tabControl.ItemSize = new Size(120, 40);
            tabControl.Location = new Point(10, 10);
            tabControl.Margin = new Padding(9);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(788, 586);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.TabIndex = 0;
            // 
            // tabBasicInfo
            // 
            tabBasicInfo.BackColor = Color.FromArgb(42, 45, 86);
            tabBasicInfo.Controls.Add(pnlBasicInfo);
            tabBasicInfo.Location = new Point(4, 44);
            tabBasicInfo.Name = "tabBasicInfo";
            tabBasicInfo.Padding = new Padding(3);
            tabBasicInfo.Size = new Size(780, 538);
            tabBasicInfo.TabIndex = 0;
            tabBasicInfo.Text = "Thông tin cơ bản";
            // 
            // pnlBasicInfo
            // 
            pnlBasicInfo.BackColor = Color.FromArgb(42, 45, 86);
            pnlBasicInfo.Controls.Add(lblEmployeeId);
            pnlBasicInfo.Controls.Add(txtEmployeeId);
            pnlBasicInfo.Controls.Add(lblFullName);
            pnlBasicInfo.Controls.Add(txtFullName);
            pnlBasicInfo.Controls.Add(lblPhone);
            pnlBasicInfo.Controls.Add(txtPhone);
            pnlBasicInfo.Controls.Add(lblEmail);
            pnlBasicInfo.Controls.Add(txtEmail);
            pnlBasicInfo.Controls.Add(lblBranch);
            pnlBasicInfo.Controls.Add(cbxBranch);
            pnlBasicInfo.Controls.Add(lblPosition);
            pnlBasicInfo.Controls.Add(cbxPosition);
            pnlBasicInfo.Controls.Add(lblStatus);
            pnlBasicInfo.Controls.Add(cbxStatus);
            pnlBasicInfo.Controls.Add(lblHireDate);
            pnlBasicInfo.Controls.Add(dtpHireDate);
            pnlBasicInfo.Controls.Add(lblResignDate);
            pnlBasicInfo.Controls.Add(chkResignDate);
            pnlBasicInfo.Controls.Add(dtpResignDate);
            pnlBasicInfo.Controls.Add(lblCreatedAtLabel);
            pnlBasicInfo.Controls.Add(lblCreatedAt);
            pnlBasicInfo.Controls.Add(lblUpdatedAtLabel);
            pnlBasicInfo.Controls.Add(lblUpdatedAt);
            pnlBasicInfo.Dock = DockStyle.Fill;
            pnlBasicInfo.Location = new Point(3, 3);
            pnlBasicInfo.Name = "pnlBasicInfo";
            pnlBasicInfo.Padding = new Padding(18, 19, 18, 19);
            pnlBasicInfo.Size = new Size(774, 532);
            pnlBasicInfo.TabIndex = 0;
            // 
            // lblEmployeeId
            // 
            lblEmployeeId.AutoSize = true;
            lblEmployeeId.ForeColor = Color.FromArgb(124, 141, 181);
            lblEmployeeId.Location = new Point(18, 22);
            lblEmployeeId.Name = "lblEmployeeId";
            lblEmployeeId.Size = new Size(76, 15);
            lblEmployeeId.TabIndex = 0;
            lblEmployeeId.Text = "ID nhân viên:";
            // 
            // txtEmployeeId
            // 
            txtEmployeeId.BackColor = Color.FromArgb(24, 28, 63);
            txtEmployeeId.BorderStyle = BorderStyle.FixedSingle;
            txtEmployeeId.ForeColor = Color.WhiteSmoke;
            txtEmployeeId.Location = new Point(131, 19);
            txtEmployeeId.Name = "txtEmployeeId";
            txtEmployeeId.ReadOnly = true;
            txtEmployeeId.Size = new Size(176, 23);
            txtEmployeeId.TabIndex = 1;
            // 
            // lblFullName
            // 
            lblFullName.AutoSize = true;
            lblFullName.ForeColor = Color.FromArgb(124, 141, 181);
            lblFullName.Location = new Point(18, 54);
            lblFullName.Name = "lblFullName";
            lblFullName.Size = new Size(85, 15);
            lblFullName.TabIndex = 2;
            lblFullName.Text = "Họ tên đầy đủ:";
            // 
            // txtFullName
            // 
            txtFullName.BackColor = Color.FromArgb(24, 28, 63);
            txtFullName.BorderStyle = BorderStyle.FixedSingle;
            txtFullName.ForeColor = Color.WhiteSmoke;
            txtFullName.Location = new Point(131, 52);
            txtFullName.Name = "txtFullName";
            txtFullName.Size = new Size(263, 23);
            txtFullName.TabIndex = 3;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.ForeColor = Color.FromArgb(124, 141, 181);
            lblPhone.Location = new Point(18, 87);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(79, 15);
            lblPhone.TabIndex = 4;
            lblPhone.Text = "Số điện thoại:";
            // 
            // txtPhone
            // 
            txtPhone.BackColor = Color.FromArgb(24, 28, 63);
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.ForeColor = Color.WhiteSmoke;
            txtPhone.Location = new Point(131, 84);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(176, 23);
            txtPhone.TabIndex = 5;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.ForeColor = Color.FromArgb(124, 141, 181);
            lblEmail.Location = new Point(18, 120);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.FromArgb(24, 28, 63);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.ForeColor = Color.WhiteSmoke;
            txtEmail.Location = new Point(131, 117);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(263, 23);
            txtEmail.TabIndex = 7;
            // 
            // lblBranch
            // 
            lblBranch.AutoSize = true;
            lblBranch.ForeColor = Color.FromArgb(124, 141, 181);
            lblBranch.Location = new Point(18, 153);
            lblBranch.Name = "lblBranch";
            lblBranch.Size = new Size(65, 15);
            lblBranch.TabIndex = 8;
            lblBranch.Text = "Chi nhánh:";
            // 
            // cbxBranch
            // 
            cbxBranch.BackColor = Color.FromArgb(24, 28, 63);
            cbxBranch.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxBranch.FlatStyle = FlatStyle.Flat;
            cbxBranch.ForeColor = Color.WhiteSmoke;
            cbxBranch.FormattingEnabled = true;
            cbxBranch.Location = new Point(131, 150);
            cbxBranch.Name = "cbxBranch";
            cbxBranch.Size = new Size(219, 23);
            cbxBranch.TabIndex = 9;
            // 
            // lblPosition
            // 
            lblPosition.AutoSize = true;
            lblPosition.ForeColor = Color.FromArgb(124, 141, 181);
            lblPosition.Location = new Point(18, 186);
            lblPosition.Name = "lblPosition";
            lblPosition.Size = new Size(54, 15);
            lblPosition.TabIndex = 10;
            lblPosition.Text = "Chức vụ:";
            // 
            // cbxPosition
            // 
            cbxPosition.BackColor = Color.FromArgb(24, 28, 63);
            cbxPosition.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxPosition.FlatStyle = FlatStyle.Flat;
            cbxPosition.ForeColor = Color.WhiteSmoke;
            cbxPosition.FormattingEnabled = true;
            cbxPosition.Location = new Point(131, 183);
            cbxPosition.Name = "cbxPosition";
            cbxPosition.Size = new Size(176, 23);
            cbxPosition.TabIndex = 11;
            // 
            // lblStatus
            // 
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.FromArgb(124, 141, 181);
            lblStatus.Location = new Point(18, 218);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(63, 15);
            lblStatus.TabIndex = 12;
            lblStatus.Text = "Trạng thái:";
            // 
            // cbxStatus
            // 
            cbxStatus.BackColor = Color.FromArgb(24, 28, 63);
            cbxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxStatus.FlatStyle = FlatStyle.Flat;
            cbxStatus.ForeColor = Color.WhiteSmoke;
            cbxStatus.FormattingEnabled = true;
            cbxStatus.Items.AddRange(new object[] { "ACTIVE", "INACTIVE" });
            cbxStatus.Location = new Point(131, 216);
            cbxStatus.Name = "cbxStatus";
            cbxStatus.Size = new Size(132, 23);
            cbxStatus.TabIndex = 13;
            // 
            // lblHireDate
            // 
            lblHireDate.AutoSize = true;
            lblHireDate.ForeColor = Color.FromArgb(124, 141, 181);
            lblHireDate.Location = new Point(18, 251);
            lblHireDate.Name = "lblHireDate";
            lblHireDate.Size = new Size(102, 15);
            lblHireDate.TabIndex = 14;
            lblHireDate.Text = "Ngày tuyển dụng:";
            // 
            // dtpHireDate
            // 
            dtpHireDate.CalendarForeColor = Color.WhiteSmoke;
            dtpHireDate.CalendarMonthBackground = Color.FromArgb(24, 28, 63);
            dtpHireDate.Format = DateTimePickerFormat.Short;
            dtpHireDate.Location = new Point(131, 248);
            dtpHireDate.Name = "dtpHireDate";
            dtpHireDate.Size = new Size(176, 23);
            dtpHireDate.TabIndex = 15;
            // 
            // lblResignDate
            // 
            lblResignDate.AutoSize = true;
            lblResignDate.ForeColor = Color.FromArgb(124, 141, 181);
            lblResignDate.Location = new Point(18, 284);
            lblResignDate.Name = "lblResignDate";
            lblResignDate.Size = new Size(89, 15);
            lblResignDate.TabIndex = 16;
            lblResignDate.Text = "Ngày nghỉ việc:";
            // 
            // chkResignDate
            // 
            chkResignDate.AutoSize = true;
            chkResignDate.ForeColor = Color.FromArgb(124, 141, 181);
            chkResignDate.Location = new Point(131, 284);
            chkResignDate.Name = "chkResignDate";
            chkResignDate.Size = new Size(15, 14);
            chkResignDate.TabIndex = 17;
            chkResignDate.UseVisualStyleBackColor = true;
            // 
            // dtpResignDate
            // 
            dtpResignDate.CalendarForeColor = Color.WhiteSmoke;
            dtpResignDate.CalendarMonthBackground = Color.FromArgb(24, 28, 63);
            dtpResignDate.Enabled = false;
            dtpResignDate.Format = DateTimePickerFormat.Short;
            dtpResignDate.Location = new Point(158, 281);
            dtpResignDate.Name = "dtpResignDate";
            dtpResignDate.Size = new Size(176, 23);
            dtpResignDate.TabIndex = 18;
            // 
            // lblCreatedAtLabel
            // 
            lblCreatedAtLabel.AutoSize = true;
            lblCreatedAtLabel.ForeColor = Color.FromArgb(124, 141, 181);
            lblCreatedAtLabel.Location = new Point(18, 331);
            lblCreatedAtLabel.Name = "lblCreatedAtLabel";
            lblCreatedAtLabel.Size = new Size(58, 15);
            lblCreatedAtLabel.TabIndex = 19;
            lblCreatedAtLabel.Text = "Ngày tạo:";
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.AutoSize = true;
            lblCreatedAt.ForeColor = Color.FromArgb(255, 192, 128);
            lblCreatedAt.Location = new Point(131, 331);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Size = new Size(0, 15);
            lblCreatedAt.TabIndex = 20;
            // 
            // lblUpdatedAtLabel
            // 
            lblUpdatedAtLabel.AutoSize = true;
            lblUpdatedAtLabel.ForeColor = Color.FromArgb(124, 141, 181);
            lblUpdatedAtLabel.Location = new Point(18, 354);
            lblUpdatedAtLabel.Name = "lblUpdatedAtLabel";
            lblUpdatedAtLabel.Size = new Size(87, 15);
            lblUpdatedAtLabel.TabIndex = 21;
            lblUpdatedAtLabel.Text = "Ngày cập nhật:";
            // 
            // lblUpdatedAt
            // 
            lblUpdatedAt.AutoSize = true;
            lblUpdatedAt.ForeColor = Color.FromArgb(255, 192, 128);
            lblUpdatedAt.Location = new Point(131, 354);
            lblUpdatedAt.Name = "lblUpdatedAt";
            lblUpdatedAt.Size = new Size(0, 15);
            lblUpdatedAt.TabIndex = 22;
            // 
            // tabAccountInfo
            // 
            tabAccountInfo.BackColor = Color.FromArgb(42, 45, 86);
            tabAccountInfo.Controls.Add(pnlAccountInfo);
            tabAccountInfo.Location = new Point(4, 44);
            tabAccountInfo.Name = "tabAccountInfo";
            tabAccountInfo.Padding = new Padding(3);
            tabAccountInfo.Size = new Size(780, 538);
            tabAccountInfo.TabIndex = 1;
            tabAccountInfo.Text = "Thông tin tài khoản";
            // 
            // pnlAccountInfo
            // 
            pnlAccountInfo.BackColor = Color.FromArgb(42, 45, 86);
            pnlAccountInfo.Controls.Add(chkHasAccount);
            pnlAccountInfo.Controls.Add(grpAccountInfo);
            pnlAccountInfo.Dock = DockStyle.Fill;
            pnlAccountInfo.Location = new Point(3, 3);
            pnlAccountInfo.Name = "pnlAccountInfo";
            pnlAccountInfo.Padding = new Padding(18, 19, 18, 19);
            pnlAccountInfo.Size = new Size(774, 532);
            pnlAccountInfo.TabIndex = 0;
            // 
            // chkHasAccount
            // 
            chkHasAccount.AutoSize = true;
            chkHasAccount.ForeColor = Color.FromArgb(192, 255, 192);
            chkHasAccount.Location = new Point(18, 22);
            chkHasAccount.Name = "chkHasAccount";
            chkHasAccount.Size = new Size(153, 19);
            chkHasAccount.TabIndex = 0;
            chkHasAccount.Text = "Có tài khoản đăng nhập";
            chkHasAccount.UseVisualStyleBackColor = true;
            // 
            // grpAccountInfo
            // 
            grpAccountInfo.Controls.Add(lblUsername);
            grpAccountInfo.Controls.Add(txtUsername);
            grpAccountInfo.Controls.Add(lblRole);
            grpAccountInfo.Controls.Add(txtRole);
            grpAccountInfo.Enabled = false;
            grpAccountInfo.ForeColor = Color.FromArgb(124, 141, 181);
            grpAccountInfo.Location = new Point(18, 59);
            grpAccountInfo.Name = "grpAccountInfo";
            grpAccountInfo.Size = new Size(438, 188);
            grpAccountInfo.TabIndex = 1;
            grpAccountInfo.TabStop = false;
            grpAccountInfo.Text = "Chi tiết tài khoản";
            // 
            // lblUsername
            // 
            lblUsername.AutoSize = true;
            lblUsername.ForeColor = Color.FromArgb(124, 141, 181);
            lblUsername.Location = new Point(18, 31);
            lblUsername.Name = "lblUsername";
            lblUsername.Size = new Size(89, 15);
            lblUsername.TabIndex = 0;
            lblUsername.Text = "Tên đăng nhập:";
            // 
            // txtUsername
            // 
            txtUsername.BackColor = Color.FromArgb(24, 28, 63);
            txtUsername.BorderStyle = BorderStyle.FixedSingle;
            txtUsername.ForeColor = Color.WhiteSmoke;
            txtUsername.Location = new Point(131, 28);
            txtUsername.Name = "txtUsername";
            txtUsername.ReadOnly = true;
            txtUsername.Size = new Size(176, 23);
            txtUsername.TabIndex = 1;
            // 
            // lblRole
            // 
            lblRole.AutoSize = true;
            lblRole.ForeColor = Color.FromArgb(124, 141, 181);
            lblRole.Location = new Point(18, 64);
            lblRole.Name = "lblRole";
            lblRole.Size = new Size(43, 15);
            lblRole.TabIndex = 2;
            lblRole.Text = "Vai trò:";
            // 
            // txtRole
            // 
            txtRole.BackColor = Color.FromArgb(24, 28, 63);
            txtRole.BorderStyle = BorderStyle.FixedSingle;
            txtRole.ForeColor = Color.WhiteSmoke;
            txtRole.Location = new Point(131, 61);
            txtRole.Name = "txtRole";
            txtRole.ReadOnly = true;
            txtRole.Size = new Size(176, 23);
            txtRole.TabIndex = 3;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(24, 28, 63);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(10, 596);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 8, 0, 8);
            pnlButtons.Size = new Size(788, 56);
            pnlButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.DialogResult = DialogResult.OK;
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft Sans Serif", 10F);
            btnSave.ForeColor = Color.FromArgb(192, 255, 192);
            btnSave.Location = new Point(551, 14);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(70, 28);
            btnSave.TabIndex = 0;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(255, 193, 7);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft Sans Serif", 10F);
            btnCancel.ForeColor = Color.FromArgb(255, 193, 7);
            btnCancel.Location = new Point(630, 14);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(70, 28);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Microsoft Sans Serif", 10F);
            btnClose.ForeColor = Color.FromArgb(255, 99, 132);
            btnClose.Location = new Point(709, 14);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(70, 28);
            btnClose.TabIndex = 2;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // FrmEmployeeDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(808, 662);
            Controls.Add(tabControl);
            Controls.Add(pnlButtons);
            FormBorderStyle = FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmEmployeeDetails";
            Padding = new Padding(10);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết nhân viên";
            tabControl.ResumeLayout(false);
            tabBasicInfo.ResumeLayout(false);
            pnlBasicInfo.ResumeLayout(false);
            pnlBasicInfo.PerformLayout();
            tabAccountInfo.ResumeLayout(false);
            pnlAccountInfo.ResumeLayout(false);
            pnlAccountInfo.PerformLayout();
            grpAccountInfo.ResumeLayout(false);
            grpAccountInfo.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabBasicInfo;
        private System.Windows.Forms.Panel pnlBasicInfo;
        private System.Windows.Forms.Label lblEmployeeId;
        private System.Windows.Forms.TextBox txtEmployeeId;
        private System.Windows.Forms.Label lblFullName;
        private System.Windows.Forms.TextBox txtFullName;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblBranch;
        private System.Windows.Forms.ComboBox cbxBranch;
        private System.Windows.Forms.Label lblPosition;
        private System.Windows.Forms.ComboBox cbxPosition;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ComboBox cbxStatus;
        private System.Windows.Forms.Label lblHireDate;
        private System.Windows.Forms.DateTimePicker dtpHireDate;
        private System.Windows.Forms.Label lblResignDate;
        private System.Windows.Forms.CheckBox chkResignDate;
        private System.Windows.Forms.DateTimePicker dtpResignDate;
        private System.Windows.Forms.Label lblCreatedAtLabel;
        private System.Windows.Forms.Label lblCreatedAt;
        private System.Windows.Forms.Label lblUpdatedAtLabel;
        private System.Windows.Forms.Label lblUpdatedAt;
        private System.Windows.Forms.TabPage tabAccountInfo;
        private System.Windows.Forms.Panel pnlAccountInfo;
        private System.Windows.Forms.CheckBox chkHasAccount;
        private System.Windows.Forms.GroupBox grpAccountInfo;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Label lblRole;
        private System.Windows.Forms.TextBox txtRole;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnClose;
    }
}