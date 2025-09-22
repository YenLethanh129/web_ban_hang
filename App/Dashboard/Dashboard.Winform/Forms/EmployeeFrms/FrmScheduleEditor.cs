using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels.ScheduleModels;

namespace Dashboard.Winform.Forms
{
    public partial class FrmScheduleEditor : Form
    {
        private readonly IScheduleManagementPresenter _presenter;
        private readonly EmployeeScheduleViewModel _existingSchedule;
        private bool _isEditMode;
        private readonly ScheduleManagementModel _model;

        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public TimeOnly SelectedTime { get; set; } = new TimeOnly(8, 0);

        public FrmScheduleEditor(IScheduleManagementPresenter presenter, EmployeeScheduleViewModel existingSchedule)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _model = (ScheduleManagementModel?)presenter.Model!;
            _existingSchedule = existingSchedule;
            _isEditMode = existingSchedule != null;

            InitializeComponent();
            SetupForm();
            LoadData();
        }

        private void SetupForm()
        {
            Text = _isEditMode ? "Sửa lịch làm việc" : "Thêm lịch làm việc";
            Size = new Size(450, 350);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            BackColor = Color.FromArgb(42, 45, 86);

            SetupDateTimeControls();
        }

        private void SetupDateTimeControls()
        {
            // Set time picker format
            dtpStartTime.Format = DateTimePickerFormat.Time;
            dtpStartTime.ShowUpDown = true;
            dtpEndTime.Format = DateTimePickerFormat.Time;
            dtpEndTime.ShowUpDown = true;

            // Set default values
            if (_isEditMode)
            {
                dtpShiftDate.Value = _existingSchedule.ShiftDate;
                dtpStartTime.Value = DateTime.Today.Add(_existingSchedule.StartTime.ToTimeSpan());
                dtpEndTime.Value = DateTime.Today.Add(_existingSchedule.EndTime.ToTimeSpan());
            }
            else
            {
                dtpShiftDate.Value = SelectedDate;
                dtpStartTime.Value = DateTime.Today.Add(SelectedTime.ToTimeSpan());
                dtpEndTime.Value = DateTime.Today.Add(SelectedTime.AddHours(8).ToTimeSpan());
            }
        }

        private async void LoadData()
        {
            try
            {
                // Load employees
                await _presenter.GetAvailableEmployeesAsync();

                cbxEmployee.DataSource = _model.AvailableEmployees;
                cbxEmployee.DisplayMember = "FullName";
                cbxEmployee.ValueMember = "Id";

                cbxStatus.DataSource = _model.ShiftStatuses
                    .Where(s => s != "All")
                    .ToList();

                if (_isEditMode)
                {
                    cbxEmployee.SelectedValue = _existingSchedule.EmployeeId;
                    cbxStatus.SelectedItem = _existingSchedule.Status;
                    cbxEmployee.Enabled = false; 
                }
                else
                {
                    cbxStatus.SelectedItem = "SCHEDULED";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInput())
                    return;

                var employeeId = (long)cbxEmployee.SelectedValue!;
                var shiftDate = dtpShiftDate.Value.Date;
                var startTime = TimeOnly.FromDateTime(dtpStartTime.Value);
                var endTime = TimeOnly.FromDateTime(dtpEndTime.Value);
                var status = cbxStatus.SelectedItem!.ToString();

                // Check for conflicts
                var hasConflict = await _presenter.CheckScheduleConflictAsync(
                    employeeId, shiftDate, startTime, endTime,
                    _isEditMode ? _existingSchedule.Id : null);

                if (hasConflict)
                {
                    MessageBox.Show(
                        "Nhân viên đã có lịch làm việc trong khoảng thời gian này!\nVui lòng chọn thời gian khác.",
                        "Xung đột lịch làm việc",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }

                if (_isEditMode)
                {
                    await _presenter.UpdateScheduleAsync(_existingSchedule.Id, shiftDate, startTime, endTime, status!);
                    MessageBox.Show("Lưu lịch làm việc thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    await _presenter.AddScheduleAsync(employeeId, shiftDate, startTime, endTime, status!);
                    MessageBox.Show("Thêm lịch làm việc thành công!", "Thành công",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            // Validate employee selection
            if (cbxEmployee.SelectedValue == null)
            {
                MessageBox.Show("Vui lòng chọn nhân viên.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxEmployee.Focus();
                return false;
            }

            // Validate time range
            var startTime = TimeOnly.FromDateTime(dtpStartTime.Value);
            var endTime = TimeOnly.FromDateTime(dtpEndTime.Value);

            if (startTime >= endTime)
            {
                MessageBox.Show("Thời gian kết thúc phải sau thời gian bắt đầu.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndTime.Focus();
                return false;
            }

            // Validate reasonable working hours
            var duration = endTime - startTime;
            if (duration.TotalHours > 12)
            {
                MessageBox.Show("Ca làm việc không thể dài quá 12 tiếng.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndTime.Focus();
                return false;
            }

            if (duration.TotalHours < 0.5)
            {
                MessageBox.Show("Ca làm việc phải ít nhất 30 phút.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndTime.Focus();
                return false;
            }

            // Validate status selection
            if (cbxStatus.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn trạng thái.", "Thông báo",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbxStatus.Focus();
                return false;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void dtpStartTime_ValueChanged(object sender, EventArgs e)
        {
            // Auto-adjust end time when start time changes
            if (!_isEditMode)
            {
                var startTime = dtpStartTime.Value;
                dtpEndTime.Value = startTime.AddHours(8); // Default 8-hour shift
            }
        }
    }
}