//using Dashboard.Winform.Forms.Base;
//using Dashboard.Winform.Presenters;
//using Dashboard.Winform.ViewModels.ScheduleModels;
//using Dashboard.Winform.ViewModels;
//using GanttChart;

//namespace Dashboard.Winform.Forms
//{
//    public partial class FrmScheduleManagement : BaseCrudForm
//    {
//        private readonly IScheduleManagementPresenter _presenter;
//        private DateTime _currentWeekStart;
//        private bool _isWeekViewActive = false;
//        private bool _isGanttViewActive = false;
//        private GanttControl _ganttChart;
//        private List<GanttTaskViewModel> _currentGanttTasks = new();

//        public FrmScheduleManagement(IScheduleManagementPresenter presenter) : base("Quản lý lịch làm việc")
//        {
//            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
//            _currentWeekStart = GetWeekStart(DateTime.Today);

//            InitializeScheduleComponents();
//            InitializeGanttChart();
//            InitializeEventHandlers();
//            SetupDataBindings();
//        }

//        private void InitializeGanttChart()
//        {
//            _ganttChart = new GanttControl
//            {
//                Dock = DockStyle.Fill,
//                BackColor = Color.FromArgb(42, 45, 86),
//                ForeColor = Color.White,
//                GridLineColor = Color.FromArgb(73, 75, 111),
//                HeaderBackColor = Color.FromArgb(73, 75, 111),
//                HeaderForeColor = Color.White,
//                TimelineBackColor = Color.FromArgb(42, 45, 86),
//                TimelineForeColor = Color.White,
//                SelectionBackColor = Color.FromArgb(0, 120, 215),
//                SelectionForeColor = Color.White,
//                TaskBackColor = Color.FromArgb(0, 120, 215),
//                TaskForeColor = Color.White,
//                TaskCompletedBackColor = Color.FromArgb(76, 175, 80),
//                RowHeaderWidth = 200,
//                TimelineHeight = 60,
//                ShowTaskNames = true,
//                ShowProgressBars = true,
//                ShowGrid = true,
//                AllowTaskEdit = true,
//                AllowTaskMove = true,
//                AllowTaskResize = true
//            };

//            // Add event handlers for Gantt chart
//            _ganttChart.TaskClick += OnGanttTaskClick;
//            _ganttChart.TaskDoubleClick += OnGanttTaskDoubleClick;
//            _ganttChart.TaskMoved += OnGanttTaskMoved;
//            _ganttChart.TaskResized += OnGanttTaskResized;

//            pnlGanttView.Controls.Add(_ganttChart);
//        }

//        private void InitializeEventHandlers()
//        {
//            // Filter events
//            cbxEmployeeFilter.SelectedValueChanged += async (s, e) => await FilterByEmployee();
//            cbxStatusFilter.SelectedValueChanged += async (s, e) => await FilterByStatus();
//            dtpStartDate.ValueChanged += async (s, e) => await FilterByDateRange();
//            dtpEndDate.ValueChanged += async (s, e) => await FilterByDateRange();

//            // Action button events
//            btnAddSchedule.Click += async (s, e) => await ShowAddScheduleDialog();
//            btnEditSchedule.Click += async (s, e) => await ShowEditScheduleDialog();
//            btnDeleteSchedule.Click += async (s, e) => await DeleteSelectedSchedule();
//            btnBulkEdit.Click += async (s, e) => await ShowBulkEditDialog();
//            btnExportSchedule.Click += async (s, e) => await ExportSchedule();

//            // View toggle events
//            btnWeekView.Click += (s, e) => ToggleToWeekView();
//            btnMonthView.Click += (s, e) => ToggleToMonthView();
//            btnGanttView.Click += (s, e) => ToggleToGanttView();

//            // Week navigation events
//            btnPrevWeek.Click += async (s, e) => await NavigateToPreviousWeek();
//            btnNextWeek.Click += async (s, e) => await NavigateToNextWeek();

//            // Calendar events
//            calendarView.DateSelected += async (s, e) => await OnCalendarDateSelected(e.Start);

//            // DataGridView events
//            dgvSchedules.SelectionChanged += OnScheduleSelectionChanged;
//            dgvSchedules.CellDoubleClick += async (s, e) => await ShowEditScheduleDialog();

//            // Presenter events
//            _presenter.OnDataLoaded += OnDataLoaded;
//        }

//        private void SetupDataBindings()
//        {
//            // Bind data sources
//            dgvSchedules.DataSource = _presenter.Model.Schedules;
//            cbxEmployeeFilter.DataSource = _presenter.Model.AvailableEmployees;
//            cbxEmployeeFilter.DisplayMember = "FullName";
//            cbxEmployeeFilter.ValueMember = "Id";

//            cbxStatusFilter.DataSource = _presenter.Model.ShiftStatuses;

//            // Set default values
//            dtpStartDate.Value = _currentWeekStart;
//            dtpEndDate.Value = _currentWeekStart.AddDays(6);
//        }

//        protected override async void OnLoad(EventArgs e)
//        {
//            base.OnLoad(e);
//            await LoadInitialData();
//            SetupDataGridColumns();
//            ToggleToWeekView(); // Start with week view
//        }

//        private async Task LoadInitialData()
//        {
//            try
//            {
//                ShowLoading(true);
//                await _presenter.LoadDataAsync();
//                await _presenter.GetAvailableEmployeesAsync();
//                await LoadStatistics();
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi tải dữ liệu: {ex.Message}");
//            }
//            finally
//            {
//                ShowLoading(false);
//            }
//        }

//        private async Task LoadStatistics()
//        {
//            try
//            {
//                var stats = await _presenter.GetScheduleStatisticsAsync(dtpStartDate.Value, dtpEndDate.Value);
//                UpdateStatisticsDisplay(stats);
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi tải thống kê: {ex.Message}");
//            }
//        }

//        private void UpdateStatisticsDisplay(ScheduleStatisticsViewModel stats)
//        {
//            lblTotalSchedules.Text = $"Tổng ca: {stats.TotalSchedules}";
//            lblScheduledCount.Text = $"Đã lên lịch: {stats.ScheduledCount}";
//            lblCompletedCount.Text = $"Hoàn thành: {stats.CompletedCount}";
//            lblAbsentCount.Text = $"Vắng mặt: {stats.AbsentCount}";
//            lblTotalHours.Text = $"Tổng giờ: {stats.FormattedTotalHours}";
//            lblAverageHours.Text = $"TB/ca: {stats.FormattedAverageShiftDuration}";

//            // Update progress bars
//            progressCompletion.Value = (int)stats.CompletionRate;
//            progressAbsence.Value = (int)stats.AbsenceRate;
//            lblCompletionRate.Text = stats.FormattedCompletionRate;
//            lblAbsenceRate.Text = stats.FormattedAbsenceRate;
//        }

//        #region View Toggle Methods

//        private async void ToggleToWeekView()
//        {
//            _isWeekViewActive = true;
//            _isGanttViewActive = false;

//            pnlWeekView.Visible = true;
//            dgvSchedules.Visible = false;
//            pnlGanttView.Visible = false;

//            btnWeekView.BackColor = Color.FromArgb(0, 120, 215);
//            btnMonthView.BackColor = Color.FromArgb(73, 75, 111);
//            btnGanttView.BackColor = Color.FromArgb(73, 75, 111);

//            await LoadWeekView();
//        }

//        private void ToggleToMonthView()
//        {
//            _isWeekViewActive = false;
//            _isGanttViewActive = false;

//            pnlWeekView.Visible = false;
//            dgvSchedules.Visible = true;
//            pnlGanttView.Visible = false;

//            btnMonthView.BackColor = Color.FromArgb(0, 120, 215);
//            btnWeekView.BackColor = Color.FromArgb(73, 75, 111);
//            btnGanttView.BackColor = Color.FromArgb(73, 75, 111);
//        }

//        private async void ToggleToGanttView()
//        {
//            _isWeekViewActive = false;
//            _isGanttViewActive = true;

//            pnlWeekView.Visible = false;
//            dgvSchedules.Visible = false;
//            pnlGanttView.Visible = true;

//            btnGanttView.BackColor = Color.FromArgb(0, 120, 215);
//            btnWeekView.BackColor = Color.FromArgb(73, 75, 111);
//            btnMonthView.BackColor = Color.FromArgb(73, 75, 111);

//            await LoadGanttView();
//        }

//        #endregion

//        #region Gantt Chart Methods

//        private async Task LoadGanttView()
//        {
//            try
//            {
//                var ganttTasks = await _presenter.GetGanttDataAsync(dtpStartDate.Value, dtpEndDate.Value);
//                _currentGanttTasks = ganttTasks;
//                UpdateGanttChart(ganttTasks);
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi tải biểu đồ Gantt: {ex.Message}");
//            }
//        }

//        private void UpdateGanttChart(List<GanttTaskViewModel> tasks)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(() => UpdateGanttChart(tasks));
//                return;
//            }

//            _ganttChart.ClearTasks();

//            var groupedTasks = tasks.GroupBy(t => t.EmployeeId)
//                                  .OrderBy(g => g.First().EmployeeName)
//                                  .ToList();

//            foreach (var employeeGroup in groupedTasks)
//            {
//                var employee = employeeGroup.First();
//                var employeeTasks = employeeGroup.OrderBy(t => t.StartDate).ToList();

//                // Add employee as a group
//                var groupTask = new GanttTask
//                {
//                    Id = employee.EmployeeId,
//                    Name = $"{employee.EmployeeName} ({employee.PositionName})",
//                    StartDate = employeeTasks.Min(t => t.StartDate),
//                    EndDate = employeeTasks.Max(t => t.EndDate),
//                    IsGroup = true,
//                    BackColor = Color.FromArgb(73, 75, 111),
//                    ForeColor = Color.White
//                };
//                _ganttChart.AddTask(groupTask);

//                // Add individual tasks
//                foreach (var task in employeeTasks)
//                {
//                    var ganttTask = new GanttTask
//                    {
//                        Id = task.Id,
//                        Name = task.TimeRange,
//                        StartDate = task.StartDate,
//                        EndDate = task.EndDate,
//                        Progress = task.Progress,
//                        BackColor = ColorTranslator.FromHtml(task.Color),
//                        ForeColor = Color.White,
//                        ParentId = employee.EmployeeId,
//                        Tag = task
//                    };
//                    _ganttChart.AddTask(ganttTask);
//                }
//            }

//            // Set timeline range
//            if (tasks.Any())
//            {
//                _ganttChart.TimelineStart = tasks.Min(t => t.StartDate).Date;
//                _ganttChart.TimelineEnd = tasks.Max(t => t.EndDate).Date.AddDays(1);
//            }

//            _ganttChart.Refresh();
//        }

//        private async void OnGanttTaskClick(object sender, GanttTaskEventArgs e)
//        {
//            if (e.Task.Tag is GanttTaskViewModel task)
//            {
//                // Highlight selected task
//                await SelectScheduleInGrid(task.Id);
//            }
//        }

//        private async void OnGanttTaskDoubleClick(object sender, GanttTaskEventArgs e)
//        {
//            if (e.Task.Tag is GanttTaskViewModel task)
//            {
//                var schedule = _presenter.Model.Schedules.FirstOrDefault(s => s.Id == task.Id);
//                if (schedule != null)
//                {
//                    await ShowEditScheduleDialog(schedule);
//                }
//            }
//        }

//        private async void OnGanttTaskMoved(object sender, GanttTaskMovedEventArgs e)
//        {
//            if (e.Task.Tag is GanttTaskViewModel task)
//            {
//                try
//                {
//                    var newDate = e.NewStartDate.Date;
//                    var startTime = TimeOnly.FromDateTime(e.NewStartDate);
//                    var endTime = TimeOnly.FromDateTime(e.NewEndDate);

//                    await _presenter.UpdateScheduleAsync(task.Id, newDate, startTime, endTime, task.Status);
//                    ShowSuccessMessage("Cập nhật lịch làm việc thành công!");

//                    if (_isGanttViewActive)
//                        await LoadGanttView();
//                }
//                catch (Exception ex)
//                {
//                    ShowErrorMessage($"Lỗi khi cập nhật lịch: {ex.Message}");
//                    // Revert the change
//                    await LoadGanttView();
//                }
//            }
//        }

//        private async void OnGanttTaskResized(object sender, GanttTaskResizedEventArgs e)
//        {
//            if (e.Task.Tag is GanttTaskViewModel task)
//            {
//                try
//                {
//                    var newDate = e.NewStartDate.Date;
//                    var startTime = TimeOnly.FromDateTime(e.NewStartDate);
//                    var endTime = TimeOnly.FromDateTime(e.NewEndDate);

//                    await _presenter.UpdateScheduleAsync(task.Id, newDate, startTime, endTime, task.Status);
//                    ShowSuccessMessage("Cập nhật thời gian ca làm việc thành công!");

//                    if (_isGanttViewActive)
//                        await LoadGanttView();
//                }
//                catch (Exception ex)
//                {
//                    ShowErrorMessage($"Lỗi khi cập nhật thời gian: {ex.Message}");
//                    // Revert the change
//                    await LoadGanttView();
//                }
//            }
//        }

//        private async Task SelectScheduleInGrid(long scheduleId)
//        {
//            foreach (DataGridViewRow row in dgvSchedules.Rows)
//            {
//                if (row.DataBoundItem is EmployeeScheduleViewModel schedule && schedule.Id == scheduleId)
//                {
//                    dgvSchedules.ClearSelection();
//                    row.Selected = true;
//                    dgvSchedules.FirstDisplayedScrollingRowIndex = row.Index;
//                    break;
//                }
//            }
//        }

//        #endregion

//        #region Week View Methods

//        private async Task LoadWeekView()
//        {
//            try
//            {
//                var weeklySchedule = await _presenter.GetWeeklyScheduleAsync(_currentWeekStart);
//                UpdateWeekView(weeklySchedule);
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi tải lịch tuần: {ex.Message}");
//            }
//        }

//        private void UpdateWeekView(WeeklyScheduleViewModel weeklySchedule)
//        {
//            if (InvokeRequired)
//            {
//                Invoke(() => UpdateWeekView(weeklySchedule));
//                return;
//            }

//            lblWeekRange.Text = weeklySchedule.WeekRange;

//            // Clear existing controls
//            tlpWeekView.Controls.Clear();
//            tlpWeekView.RowCount = 18; // 17 hours (6 AM to 10 PM) + header
//            tlpWeekView.ColumnCount = 8; // 7 days + time column

//            // Add header row
//            AddWeekViewHeader();

//            // Add time slots and schedules
//            for (int hour = 6; hour < 23; hour++) // 6 AM to 10 PM
//            {
//                AddTimeSlotRow(hour, weeklySchedule.Days);
//            }
//        }

//        private void AddWeekViewHeader()
//        {
//            // Time column header
//            var timeHeader = new Label
//            {
//                Text = "Giờ",
//                BackColor = Color.FromArgb(73, 75, 111),
//                ForeColor = Color.White,
//                TextAlign = ContentAlignment.MiddleCenter,
//                Font = new Font("Segoe UI", 9F, FontStyle.Bold),
//                Dock = DockStyle.Fill
//            };
//            tlpWeekView.Controls.Add(timeHeader, 0, 0);

//            // Day headers
//            string[] dayNames = { "Thứ 2", "Thứ 3", "Thứ 4", "Thứ 5", "Thứ 6", "Thứ 7", "CN" };
//            for (int i = 0; i < 7; i++)
//            {
//                var dayHeader = new Label
//                {
//                    Text = $"{dayNames[i]}\n{_currentWeekStart.AddDays(i):dd/MM}",
//                    BackColor = Color.FromArgb(73, 75, 111),
//                    ForeColor = Color.White,
//                    TextAlign = ContentAlignment.MiddleCenter,
//                    Font = new Font("Segoe UI", 8F, FontStyle.Bold),
//                    Dock = DockStyle.Fill
//                };

//                if (_currentWeekStart.AddDays(i).Date == DateTime.Today)
//                {
//                    dayHeader.BackColor = Color.FromArgb(0, 120, 215);
//                }

//                tlpWeekView.Controls.Add(dayHeader, i + 1, 0);
//            }
//        }

//        private void AddTimeSlotRow(int hour, List<DayScheduleViewModel> days)
//        {
//            int row = hour - 5; // Start from row 1 (hour 6 -> row 1)

//            // Time label
//            var timeLabel = new Label
//            {
//                Text = $"{hour:00}:00",
//                BackColor = Color.FromArgb(42, 45, 86),
//                ForeColor = Color.White,
//                TextAlign = ContentAlignment.MiddleCenter,
//                Font = new Font("Segoe UI", 8F),
//                Dock = DockStyle.Fill
//            };
//            tlpWeekView.Controls.Add(timeLabel, 0, row);

//            // Day cells
//            for (int dayIndex = 0; dayIndex < 7; dayIndex++)
//            {
//                var dayPanel = new Panel
//                {
//                    BackColor = Color.FromArgb(42, 45, 86),
//                    Dock = DockStyle.Fill,
//                    Margin = new Padding(1)
//                };

//                // Add schedules for this time slot
//                if (dayIndex < days.Count)
//                {
//                    var daySchedules = days[dayIndex].Schedules
//                        .Where(s => s.StartTime.Hour <= hour && s.EndTime.Hour > hour)
//                        .ToList();

//                    foreach (var schedule in daySchedules)
//                    {
//                        AddScheduleToTimeSlot(dayPanel, schedule, hour);
//                    }
//                }

//                // Add click event for adding new schedule
//                var selectedDate = _currentWeekStart.AddDays(dayIndex);
//                var selectedHour = hour;
//                dayPanel.Click += async (s, e) => await OnTimeSlotClick(selectedDate, selectedHour);
//                dayPanel.Cursor = Cursors.Hand;

//                tlpWeekView.Controls.Add(dayPanel, dayIndex + 1, row);
//            }
//        }

//        private void AddScheduleToTimeSlot(Panel timeSlotPanel, EmployeeScheduleViewModel schedule, int hour)
//        {
//            var scheduleLabel = new Label
//            {
//                Text = $"{schedule.EmployeeName}\n{schedule.StartTime:HH:mm}-{schedule.EndTime:HH:mm}",
//                BackColor = schedule.StatusColor,
//                ForeColor = Color.White,
//                Font = new Font("Segoe UI", 7F),
//                TextAlign = ContentAlignment.MiddleCenter,
//                Dock = DockStyle.Fill,
//                Cursor = Cursors.Hand,
//                Tag = schedule
//            };

//            scheduleLabel.Click += async (s, e) => await OnScheduleClick(schedule);

//            timeSlotPanel.Controls.Add(scheduleLabel);
//        }

//        private async Task OnTimeSlotClick(DateTime selectedDate, int hour)
//        {
//            var selectedTime = new TimeOnly(hour, 0);
//            await ShowAddScheduleDialog(selectedDate, selectedTime);
//        }

//        private async Task OnScheduleClick(EmployeeScheduleViewModel schedule)
//        {
//            await ShowEditScheduleDialog(schedule);
//        }

//        private async Task NavigateToPreviousWeek()
//        {
//            _currentWeekStart = _currentWeekStart.AddDays(-7);
//            dtpStartDate.Value = _currentWeekStart;
//            dtpEndDate.Value = _currentWeekStart.AddDays(6);

//            if (_isWeekViewActive)
//                await LoadWeekView();
//            else if (_isGanttViewActive)
//                await LoadGanttView();

//            await LoadStatistics();
//        }

//        private async Task NavigateToNextWeek()
//        {
//            _currentWeekStart = _currentWeekStart.AddDays(7);
//            dtpStartDate.Value = _currentWeekStart;
//            dtpEndDate.Value = _currentWeekStart.AddDays(6);

//            if (_isWeekViewActive)
//                await LoadWeekView();
//            else if (_isGanttViewActive)
//                await LoadGanttView();

//            await LoadStatistics();
//        }

//        #endregion

//        #region Event Handlers

//        private async Task FilterByEmployee()
//        {
//            try
//            {
//                var employeeId = cbxEmployeeFilter.SelectedValue as long?;
//                await _presenter.FilterByEmployeeAsync(employeeId);

//                if (_isGanttViewActive)
//                    await LoadGanttView();

//                await LoadStatistics();
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi lọc theo nhân viên: {ex.Message}");
//            }
//        }

//        private async Task FilterByStatus()
//        {
//            try
//            {
//                var status = cbxStatusFilter.SelectedItem?.ToString();
//                await _presenter.FilterByStatusAsync(status);

//                if (_isGanttViewActive)
//                    await LoadGanttView();

//                await LoadStatistics();
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi lọc theo trạng thái: {ex.Message}");
//            }
//        }

//        private async Task FilterByDateRange()
//        {
//            try
//            {
//                await _presenter.FilterByDateRangeAsync(dtpStartDate.Value.Date, dtpEndDate.Value.Date);

//                if (_isGanttViewActive)
//                    await LoadGanttView();
//                else if (_isWeekViewActive)
//                {
//                    _currentWeekStart = GetWeekStart(dtpStartDate.Value);
//                    await LoadWeekView();
//                }

//                await LoadStatistics();
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi lọc theo ngày: {ex.Message}");
//            }
//        }

//        private async Task OnCalendarDateSelected(DateTime selectedDate)
//        {
//            try
//            {
//                _currentWeekStart = GetWeekStart(selectedDate);
//                dtpStartDate.Value = _currentWeekStart;
//                dtpEndDate.Value = _currentWeekStart.AddDays(6);

//                if (_isWeekViewActive)
//                    await LoadWeekView();
//                else if (_isGanttViewActive)
//                    await LoadGanttView();
//                else
//                    await FilterByDateRange();

//                await LoadStatistics();
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi chọn ngày: {ex.Message}");
//            }
//        }

//        private void OnScheduleSelectionChanged(object sender, EventArgs e)
//        {
//            bool hasSelection = dgvSchedules.SelectedRows.Count > 0;
//            btnEditSchedule.Enabled = hasSelection;
//            btnDeleteSchedule.Enabled = hasSelection;
//            btnBulkEdit.Enabled = dgvSchedules.SelectedRows.Count > 1;
//        }

//        private void OnDataLoaded(object sender, EventArgs e)
//        {
//            UpdatePaginationInfo();
//            if (_isWeekViewActive)
//            {
//                _ = Task.Run(LoadWeekView);
//            }
//            else if (_isGanttViewActive)
//            {
//                _ = Task.Run(LoadGanttView);
//            }
//        }

//        #endregion

//        #region CRUD Operations

//        private async Task ShowAddScheduleDialog(DateTime? selectedDate = null, TimeOnly? selectedTime = null)
//        {
//            try
//            {
//                var dialog = new FrmScheduleEditor(_presenter, null)
//                {
//                    SelectedDate = selectedDate ?? DateTime.Today,
//                    SelectedTime = selectedTime ?? new TimeOnly(8, 0)
//                };

//                if (dialog.ShowDialog() == DialogResult.OK)
//                {
//                    await RefreshCurrentView();
//                    await LoadStatistics();
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi thêm lịch: {ex.Message}");
//            }
//        }

//        private async Task ShowEditScheduleDialog(EmployeeScheduleViewModel selectedSchedule = null)
//        {
//            try
//            {
//                var schedule = selectedSchedule ?? GetSelectedSchedule();
//                if (schedule == null)
//                {
//                    ShowWarningMessage("Vui lòng chọn một lịch làm việc để sửa.");
//                    return;
//                }

//                var dialog = new FrmScheduleEditor(_presenter, schedule);
//                if (dialog.ShowDialog() == DialogResult.OK)
//                {
//                    await RefreshCurrentView();
//                    await LoadStatistics();
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi sửa lịch: {ex.Message}");
//            }
//        }

//        private async Task DeleteSelectedSchedule()
//        {
//            try
//            {
//                var schedule = GetSelectedSchedule();
//                if (schedule == null)
//                {
//                    ShowWarningMessage("Vui lòng chọn một lịch làm việc để xóa.");
//                    return;
//                }

//                var result = MessageBox.Show(
//                    $"Bạn có chắc chắn muốn xóa lịch làm việc của {schedule.EmployeeName} vào {schedule.FormattedDate}?",
//                    "Xác nhận xóa",
//                    MessageBoxButtons.YesNo,
//                    MessageBoxIcon.Question);

//                if (result == DialogResult.Yes)
//                {
//                    await _presenter.DeleteScheduleAsync(schedule.Id);
//                    await RefreshCurrentView();
//                    await LoadStatistics();
//                    ShowSuccessMessage("Xóa lịch làm việc thành công!");
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi xóa lịch: {ex.Message}");
//            }
//        }

//        private async Task ShowBulkEditDialog()
//        {
//            try
//            {
//                var selectedSchedules = GetSelectedSchedules();
//                if (!selectedSchedules.Any())
//                {
//                    ShowWarningMessage("Vui lòng chọn ít nhất một lịch làm việc.");
//                    return;
//                }

//                var dialog = new FrmBulkScheduleEditor(_presenter, selectedSchedules);
//                if (dialog.ShowDialog() == DialogResult.OK)
//                {
//                    await RefreshCurrentView();
//                    await LoadStatistics();
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi chỉnh sửa hàng loạt: {ex.Message}");
//            }
//        }

//        private async Task ExportSchedule()
//        {
//            try
//            {
//                var saveFileDialog = new SaveFileDialog
//                {
//                    Filter = "Excel Files|*.xlsx|CSV Files|*.csv",
//                    DefaultExt = "xlsx",
//                    FileName = $"LichLamViec_{DateTime.Now:yyyyMMdd}"
//                };

//                if (saveFileDialog.ShowDialog() == DialogResult.OK)
//                {
//                    var schedules = _presenter.Model.Schedules.ToList();
//                    var filePath = saveFileDialog.FileName;

//                    if (filePath.EndsWith(".xlsx"))
//                    {
//                        ExportToExcel(schedules, filePath);
//                    }
//                    else
//                    {
//                        ExportToCSV(schedules, filePath);
//                    }

//                    ShowSuccessMessage($"Xuất dữ liệu thành công: {filePath}");
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi xuất dữ liệu: {ex.Message}");
//            }
//        }

//        private void ExportToCSV(List<EmployeeScheduleViewModel> schedules, string filePath)
//        {
//            using var writer = new StreamWriter(filePath, false, System.Text.Encoding.UTF8);

//            // Header
//            writer.WriteLine("Nhân viên,Vị trí,Ngày,Thứ,Ca làm việc,Trạng thái,Giờ bắt đầu,Giờ kết thúc,Số giờ");

//            // Data
//            foreach (var schedule in schedules)
//            {
//                writer.WriteLine($"\"{schedule.EmployeeName}\",\"{schedule.PositionName}\"," +
//                               $"\"{schedule.FormattedDate}\",\"{schedule.WeekDay}\"," +
//                               $"\"{schedule.ShiftDuration}\",\"{schedule.StatusDisplayText}\"," +
//                               $"{schedule.StartTime:HH:mm},{schedule.EndTime:HH:mm},{schedule.DurationInHours:F1}");
//            }
//        }

//        private void ExportToExcel(List<EmployeeScheduleViewModel> schedules, string filePath)
//        {
//            // This would require a library like ClosedXML or EPPlus
//            // For now, export as CSV with .xlsx extension
//            ExportToCSV(schedules, filePath);
//        }

//        private EmployeeScheduleViewModel GetSelectedSchedule()
//        {
//            if (dgvSchedules.SelectedRows.Count == 0)
//                return null;

//            return dgvSchedules.SelectedRows[0].DataBoundItem as EmployeeScheduleViewModel;
//        }

//        private List<EmployeeScheduleViewModel> GetSelectedSchedules()
//        {
//            return dgvSchedules.SelectedRows
//                .Cast<DataGridViewRow>()
//                .Select(row => row.DataBoundItem as EmployeeScheduleViewModel)
//                .Where(schedule => schedule != null)
//                .ToList();
//        }

//        private async Task RefreshCurrentView()
//        {
//            if (_isWeekViewActive)
//                await LoadWeekView();
//            else if (_isGanttViewActive)
//                await LoadGanttView();
//            else
//                await _presenter.FilterByDateRangeAsync(dtpStartDate.Value.Date, dtpEndDate.Value.Date);
//        }

//        #endregion

//        private void SetupDataGridColumns()
//        {
//            dgvSchedules.Columns.Clear();

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "EmployeeName",
//                HeaderText = "Nhân viên",
//                Name = "EmployeeName",
//                Width = 150,
//                ReadOnly = true
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "PositionName",
//                HeaderText = "Vị trí",
//                Name = "PositionName",
//                Width = 120,
//                ReadOnly = true
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "FormattedDate",
//                HeaderText = "Ngày",
//                Name = "FormattedDate",
//                Width = 100,
//                ReadOnly = true
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "WeekDay",
//                HeaderText = "Thứ",
//                Name = "WeekDay",
//                Width = 80,
//                ReadOnly = true
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "ShiftDuration",
//                HeaderText = "Ca làm việc",
//                Name = "ShiftDuration",
//                Width = 120,
//                ReadOnly = true
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "StatusDisplayText",
//                HeaderText = "Trạng thái",
//                Name = "StatusDisplayText",
//                Width = 100,
//                ReadOnly = true
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "DurationInHours",
//                HeaderText = "Số giờ",
//                Name = "DurationInHours",
//                Width = 80,
//                ReadOnly = true,
//                DefaultCellStyle = new DataGridViewCellStyle { Format = "F1" }
//            });

//            // Hide ID columns
//            if (dgvSchedules.Columns.Contains("Id"))
//                dgvSchedules.Columns["Id"].Visible = false;
//            if (dgvSchedules.Columns.Contains("EmployeeId"))
//                dgvSchedules.Columns["EmployeeId"].Visible = false;
//        }

//        #region Helper Methods

//        private DateTime GetWeekStart(DateTime date)
//        {
//            int daysFromMonday = ((int)date.DayOfWeek - 1 + 7) % 7;
//            return date.AddDays(-daysFromMonday).Date;
//        }

//        private void UpdatePaginationInfo()
//        {
//            // Update pagination info if needed
//            // This can be implemented based on your base form's pagination controls
//        }

//        #endregion

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                _presenter?.Dispose();
//                _ganttChart?.Dispose();
//                components?.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }