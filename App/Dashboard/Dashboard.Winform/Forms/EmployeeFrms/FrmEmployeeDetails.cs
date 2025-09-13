using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels.EmployeeModels;
using Dashboard.Winform.Interfaces;

namespace Dashboard.Winform.Forms
{
    public partial class FrmEmployeeDetails : Form, IBlurLoadingServiceAware
    {
        #region Fields

        private readonly bool _isEditMode;
        private readonly long? _employeeId;
        private readonly EmployeeDetailViewModel _model;
        private readonly IEmployeeDetailsPresenter _presenter;
        private IBlurLoadingService? _blurLoadingService;

        #endregion

        #region Properties

        public DialogResult Result { get; private set; }
        public EmployeeDetailViewModel Employee => _model;

        #endregion

        #region Constructor

        public FrmEmployeeDetails(IEmployeeDetailsPresenter presenter, long? employeeId = null, EmployeeDetailViewModel? employee = null)
        {
            _employeeId = employeeId;
            _isEditMode = employeeId.HasValue;
            _presenter = presenter;
            _model = _presenter.Model;

            if (employee != null)
            {
                MapEmployeeToModel(employee);
            }

            InitializeComponent();
            InitializeFormSettings();
            SetupEventHandlers();
            SetupDataGridViews();
            BindModelToUI();

            if (_isEditMode)
            {
                Text = $"Chi tiết nhân viên - {_model.FullName}";
                btnClose.Visible = true;
                btnSave.Text = "Cập nhật";
            }
            else
            {
                Text = "Thêm nhân viên mới";
                btnClose.Visible = false;
                btnSave.Text = "Thêm";
            }

            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.DrawItem += (s, o) => TabControl_DrawItem(s!, o);
            ApplyDarkTheme();

            if (!_isEditMode)
            {
                _model.Status = "Active";
                _model.HireDate = DateTime.Today;
            }

            Load += (s, o) => FrmEmployeeDetails_Load(s!, o);
        }

        private void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (sender is not TabControl tabControl)
                return;
            TabPage tabPage = tabControl.TabPages[e.Index];
            Rectangle tabRect = tabControl.GetTabRect(e.Index);

            Color tabBackColor = Color.FromArgb(24, 28, 63);
            Color selectedTabBackColor = Color.FromArgb(42, 45, 86);
            Color tabTextColor = Color.FromArgb(124, 141, 181);
            Color selectedTabTextColor = Color.FromArgb(192, 255, 192);
            Color borderColor = Color.FromArgb(107, 83, 255);

            using (SolidBrush brush = new SolidBrush(e.Index == tabControl.SelectedIndex ? selectedTabBackColor : tabBackColor))
            {
                e.Graphics.FillRectangle(brush, tabRect);
            }

            if (e.Index == tabControl.SelectedIndex)
            {
                using Pen pen = new Pen(borderColor, 2);
                e.Graphics.DrawRectangle(pen, tabRect.X, tabRect.Y, tabRect.Width - 1, tabRect.Height - 1);
            }

            using SolidBrush textBrush = new(e.Index == tabControl.SelectedIndex ? selectedTabTextColor : tabTextColor);
            StringFormat stringFormat = new()
            {
                Alignment = StringAlignment.Center,
                LineAlignment = StringAlignment.Center
            };

            e.Graphics.DrawString(tabPage.Text, tabControl.Font, textBrush, tabRect, stringFormat);
        }

        private void ApplyDarkTheme()
        {
            BackColor = Color.FromArgb(24, 28, 63);

            tabControl.BackColor = Color.FromArgb(24, 28, 63);

            foreach (TabPage tabPage in tabControl.TabPages)
            {
                tabPage.BackColor = Color.FromArgb(42, 45, 86);
            }
        }

        #endregion

        #region IBlurLoadingServiceAware Implementation

        public void SetBlurLoadingService(IBlurLoadingService blurLoadingService)
        {
            _blurLoadingService = blurLoadingService;
        }

        #endregion

        #region Initialization Methods

        private void InitializeFormSettings()
        {
            this.Size = new Size(900, 700);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowIcon = false;
        }

        private void SetupEventHandlers()
        {
            // Form buttons
            btnSave.Click += (s, o) => BtnSave_Click(s!, o);
            btnCancel.Click += (s, o) => BtnCancel_Click(s!, o);
            btnClose.Click += (s, o) => BtnClose_Click(s!, o);

            // Basic info events
            chkResignDate.CheckedChanged += (s, o) => ChkResignDate_CheckedChanged(s!, o);

            // Account info events
            chkHasAccount.CheckedChanged += (s, o) => ChkHasAccount_CheckedChanged(s!, o);

            // Salary management buttons
            btnAddSalary.Click += (s, o) => BtnAddSalary_Click(s!, o);
            btnEditSalary.Click += (s, o) => BtnEditSalary_Click(s!, o);
            btnDeleteSalary.Click += (s, o) => BtnDeleteSalary_Click(s!, o);

            // Validation events
            txtFullName.Validating += (s, o) => TxtFullName_Validating(s!, o);
            txtEmail.Validating += (s, o) => TxtEmail_Validating(s!, o);
            txtPhone.Validating += (s, o) => TxtPhone_Validating(s!, o);

            // Presenter events
            _presenter.OnDataLoaded += OnDataLoaded;
            _presenter.OnError += OnPresenterError;
        }

        private void SetupDataGridViews()
        {
            SetupSalaryDataGridView();
            SetupPayrollDataGridView();
        }

        private void SetupSalaryDataGridView()
        {
            dgvSalaries.AutoGenerateColumns = false;
            dgvSalaries.Columns.Clear();

            dgvSalaries.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeSalaryViewModel.EffectiveDate),
                HeaderText = "Ngày hiệu lực",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
            });

            dgvSalaries.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeSalaryViewModel.BaseSalary),
                HeaderText = "Lương cơ bản",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvSalaries.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeSalaryViewModel.SalaryType),
                HeaderText = "Loại lương",
                Width = 100
            });

            dgvSalaries.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeSalaryViewModel.Allowance),
                HeaderText = "Phụ cấp",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvSalaries.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeSalaryViewModel.Bonus),
                HeaderText = "Thưởng",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvSalaries.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeSalaryViewModel.Penalty),
                HeaderText = "Khấu trừ",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvSalaries.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(EmployeeSalaryViewModel.TaxRate),
                HeaderText = "Thuế (%)",
                Width = 80,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "P2" }
            });
        }

        private void SetupPayrollDataGridView()
        {
            dgvPayrolls.AutoGenerateColumns = false;
            dgvPayrolls.Columns.Clear();

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PayrollViewModel.PayrollMonth),
                HeaderText = "Tháng/Năm",
                Width = 100
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PayrollViewModel.WorkingHours),
                HeaderText = "Giờ làm việc",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N1" }
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PayrollViewModel.BasicSalary),
                HeaderText = "Lương cơ bản",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PayrollViewModel.Allowances),
                HeaderText = "Phụ cấp",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PayrollViewModel.Bonuses),
                HeaderText = "Thưởng",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PayrollViewModel.Deductions),
                HeaderText = "Khấu trừ",
                Width = 100,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });

            dgvPayrolls.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(PayrollViewModel.TotalSalary),
                HeaderText = "Tổng lương",
                Width = 120,
                DefaultCellStyle = new DataGridViewCellStyle { Format = "N0" }
            });
        }

        private void MapEmployeeToModel(EmployeeDetailViewModel employee)
        {
            _model.Id = employee.Id;
            _model.FullName = employee.FullName;
            _model.Phone = employee.Phone;
            _model.Email = employee.Email;
            _model.HireDate = employee.HireDate;
            _model.ResignDate = employee.ResignDate;
            _model.Status = employee.Status;
            _model.PositionId = employee.PositionId;
            _model.PositionName = employee.PositionName;
            _model.CreatedAt = employee.CreatedAt;
            _model.UpdatedAt = employee.UpdatedAt;
            _model.BranchId = employee.BranchId;
            _model.BranchName = employee.BranchName;
            _model.BranchAddress = employee.BranchAddress;
            _model.BranchPhone = employee.BranchPhone;
            _model.BranchManager = employee.BranchManager;
            _model.HasAccount = employee.HasAccount;
            _model.PhoneAsUsername = employee.PhoneAsUsername;
            _model.Role = employee.Role;
            _model.Salaries = employee.Salaries;
            _model.Payrolls = employee.Payrolls;
            _model.Shifts = employee.Shifts;
        }

        #endregion

        #region Load data

        private async void FrmEmployeeDetails_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (_blurLoadingService != null)
            {
                await _blurLoadingService.ExecuteWithLoadingAsync(async () =>
                {
                    await _presenter.LoadLookupsAsync();
                    if (_isEditMode)
                    {
                        await _presenter.LoadEmployeeDetailsAsync(_employeeId!.Value);
                    }
                    else
                    {
                        _presenter.RaiseDataLoaded();
                    }
                }, "Đang tải dữ liệu nhân viên...", true);
            }
            else
            {
                try
                {
                    await _presenter.LoadLookupsAsync();
                    if (_isEditMode)
                    {
                        await _presenter.LoadEmployeeDetailsAsync(_employeeId!.Value);
                    }
                    else
                    {
                        _presenter.RaiseDataLoaded();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi load dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void OnDataLoaded(object? sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnDataLoaded(sender, e));
                return;
            }

            cbxBranch.DataSource = _model.ExistingBranches;
            cbxBranch.DisplayMember = nameof(BranchViewModel.Name);
            cbxBranch.ValueMember = nameof(BranchViewModel.Id);

            cbxPosition.DataSource = _model.ExistingPositions;
            cbxPosition.DisplayMember = nameof(PositionViewModel.Name);
            cbxPosition.ValueMember = nameof(PositionViewModel.Id);

            cbxStatus.DataSource = _model.Statuses;
        }

        private void OnPresenterError(object? sender, string errorMessage)
        {
            if (InvokeRequired)
            {
                Invoke(() => OnPresenterError(sender, errorMessage));
                return;
            }

            MessageBox.Show(errorMessage, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        #endregion

        #region Event Handlers

        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                try
                {
                    await _presenter.SaveEmployeeAsync(_model);
                    Result = DialogResult.OK;
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void ChkResignDate_CheckedChanged(object sender, EventArgs e)
        {
            dtpResignDate.Enabled = chkResignDate.Checked;
            if (!chkResignDate.Checked)
            {
                _model.ResignDate = null;
            }
        }

        private void ChkHasAccount_CheckedChanged(object sender, EventArgs e)
        {
            grpAccountInfo.Enabled = chkHasAccount.Checked;
            if (!chkHasAccount.Checked)
            {
                _model.PhoneAsUsername = "";
                _model.Role = "";
            }
        }

        private void BtnAddSalary_Click(object sender, EventArgs e)
        {
            // TODO: Implement add salary dialog
            MessageBox.Show("Chức năng thêm bản ghi lương sẽ được triển khai",
                "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnEditSalary_Click(object sender, EventArgs e)
        {
            if (dgvSalaries.SelectedRows.Count > 0)
            {
                // TODO: Implement edit salary dialog
                MessageBox.Show("Chức năng sửa bản ghi lương sẽ được triển khai",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bản ghi lương để chỉnh sửa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnDeleteSalary_Click(object sender, EventArgs e)
        {
            if (dgvSalaries.SelectedRows.Count > 0)
            {
                var result = MessageBox.Show(
                    "Bạn có chắc chắn muốn xóa bản ghi lương này?",
                    "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // TODO: Implement delete salary logic
                    MessageBox.Show("Chức năng xóa bản ghi lương sẽ được triển khai",
                        "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một bản ghi lương để xóa",
                    "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        #endregion

        #region Validation Event Handlers

        private void TxtFullName_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox?.Text))
            {
                MessageBox.Show("Họ tên không được để trống", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void TxtEmail_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(textBox?.Text))
            {
                if (!IsValidEmail(textBox.Text))
                {
                    MessageBox.Show("Email không đúng định dạng", "Lỗi nhập liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }

        private void TxtPhone_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            if (!string.IsNullOrWhiteSpace(textBox?.Text))
            {
                if (!IsValidPhoneNumber(textBox.Text))
                {
                    MessageBox.Show("Số điện thoại không đúng định dạng", "Lỗi nhập liệu",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;
                }
            }
        }

        #endregion

        #region Helper Methods

        private bool ValidateForm()
        {
            // Required field validation
            if (string.IsNullOrWhiteSpace(txtFullName.Text))
            {
                MessageBox.Show("Họ tên không được để trống", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFullName.Focus();
                return false;
            }

            if (cbxBranch.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chi nhánh", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbxBranch.Focus();
                return false;
            }

            if (cbxPosition.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn chức vụ", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbxPosition.Focus();
                return false;
            }

            if (cbxStatus.SelectedIndex == -1)
            {
                MessageBox.Show("Vui lòng chọn trạng thái", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbxStatus.Focus();
                return false;
            }

            // Email validation
            if (!string.IsNullOrWhiteSpace(txtEmail.Text) && !IsValidEmail(txtEmail.Text))
            {
                MessageBox.Show("Email không đúng định dạng", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return false;
            }

            // Phone validation
            if (!string.IsNullOrWhiteSpace(txtPhone.Text) && !IsValidPhoneNumber(txtPhone.Text))
            {
                MessageBox.Show("Số điện thoại không đúng định dạng", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtPhone.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private bool IsValidPhoneNumber(string phone)
        {
            phone = phone.Replace(" ", "").Replace("-", "").Replace(".", "");

            if (phone.Length < 10 || phone.Length > 11)
                return false;

            return phone.StartsWith("0") && phone.All(char.IsDigit);
        }

        private void BindModelToUI()
        {
            txtEmployeeId.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.Id), true, DataSourceUpdateMode.OnPropertyChanged);
            txtFullName.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.FullName), true, DataSourceUpdateMode.OnPropertyChanged);
            txtPhone.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.Phone), true, DataSourceUpdateMode.OnPropertyChanged);
            txtEmail.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.Email), true, DataSourceUpdateMode.OnPropertyChanged);

            cbxBranch.DataBindings.Add("SelectedValue", _model, nameof(EmployeeDetailViewModel.BranchId), true, DataSourceUpdateMode.OnPropertyChanged);
            cbxPosition.DataBindings.Add("SelectedValue", _model, nameof(EmployeeDetailViewModel.PositionId), true, DataSourceUpdateMode.OnPropertyChanged);
            cbxStatus.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.Status), true, DataSourceUpdateMode.OnPropertyChanged);

            dtpHireDate.DataBindings.Add("Value", _model, nameof(EmployeeDetailViewModel.HireDate), true, DataSourceUpdateMode.OnPropertyChanged);
            dtpResignDate.DataBindings.Add("Value", _model, nameof(EmployeeDetailViewModel.ResignDate), true, DataSourceUpdateMode.OnPropertyChanged);

            var resignDateBinding = new Binding("Checked", _model, nameof(EmployeeDetailViewModel.ResignDate));
            resignDateBinding.Format += (s, args) => args.Value = _model.ResignDate != null;
            resignDateBinding.Parse += (s, args) =>
            {
                if ((bool)args.Value!)
                    _model.ResignDate = dtpResignDate.Value;
                else
                    _model.ResignDate = null;
            };
            chkResignDate.DataBindings.Add(resignDateBinding);
            chkHasAccount.DataBindings.Add("Checked", _model, nameof(EmployeeDetailViewModel.HasAccount), true, DataSourceUpdateMode.OnPropertyChanged);

            txtUsername.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.PhoneAsUsername), true, DataSourceUpdateMode.OnPropertyChanged);
            txtRole.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.Role), true, DataSourceUpdateMode.OnPropertyChanged);

            dgvSalaries.DataSource = _model.Salaries;
            dgvPayrolls.DataSource = _model.Payrolls;

            lblCreatedAt.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.CreatedAt), true, DataSourceUpdateMode.Never, "N/A", "dd/MM/yyyy HH:mm");
            lblUpdatedAt.DataBindings.Add("Text", _model, nameof(EmployeeDetailViewModel.UpdatedAt), true, DataSourceUpdateMode.Never, "N/A", "dd/MM/yyyy HH:mm");
        }
        #endregion
    }
}