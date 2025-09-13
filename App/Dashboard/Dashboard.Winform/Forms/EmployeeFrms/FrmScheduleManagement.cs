//using Dashboard.Winform.Forms.Base;
//using Dashboard.Winform.Presenters;
//using Dashboard.Winform.ViewModels.ScheduleModels;
//using Dashboard.Winform.ViewModels;
//using Dashboard.Winform.Controls;

//namespace Dashboard.Winform.Forms
//{
//    public partial class FrmScheduleManagement : FrmBaseManagement
//    {
//        private readonly IScheduleManagementPresenter _presenter;
//        private DateTime _currentWeekStart;
//        private bool _isWeekViewActive = false;
//        private GanttChartControl? ganttChart;
//        private bool _useGanttChart = true;     

//        public FrmScheduleManagement(IScheduleManagementPresenter presenter) : base()
//        {
//            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
//            _currentWeekStart = GetWeekStart(DateTime.Today);

//            InitializeScheduleComponents();
//            if (_useGanttChart)
//            {
//                InitializeGanttChart();
//            }
//            InitializeEventHandlers();
//            SetupDataBindings();
//        }

//        private void InitializeGanttChart()
//        {
//            try
//            {
//                ganttChart = new GanttChartControl
//                {
//                    Dock = DockStyle.Fill,
//                    StartDate = _currentWeekStart,
//                    EndDate = _currentWeekStart.AddDays(6),
//                    Visible = false
//                };

//                ganttChart.TaskClicked += (s, o) => OnGanttTaskClicked(s!,o);
//                ganttChart.TimeSlotClicked += (s, o) => OnGanttTimeSlotClicked(s!,);

//                // Add to week view panel
//                pnlWeekView.Controls.Add(ganttChart);
//                ganttChart.BringToFront();
//            }
//            catch (Exception ex)
//            {
//                _useGanttChart = false;
//                ShowErrorMessage($"Không thể khởi tạo Gantt Chart: {ex.Message}");
//            }
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

//            // View toggle events
//            btnWeekView.Click += (s, e) => ToggleToWeekView();
//            btnMonthView.Click += (s, e) => ToggleToMonthView();

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

//        private void SetupDataGridColumns()
//        {
//            dgvSchedules.Columns.Clear();

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "EmployeeName",
//                HeaderText = "Nhân viên",
//                Name = "EmployeeName",
//                Width = 150
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "PositionName",
//                HeaderText = "Vị trí",
//                Name = "PositionName",
//                Width = 120
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "FormattedDate",
//                HeaderText = "Ngày",
//                Name = "FormattedDate",
//                Width = 100
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "WeekDay",
//                HeaderText = "Thứ",
//                Name = "WeekDay",
//                Width = 80
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "ShiftDuration",
//                HeaderText = "Ca làm việc",
//                Name = "ShiftDuration",
//                Width = 120
//            });

//            dgvSchedules.Columns.Add(new DataGridViewTextBoxColumn
//            {
//                DataPropertyName = "Status",
//                HeaderText = "Trạng thái",
//                Name = "Status",
//                Width = 100
//            });

//            // Hide ID columns
//            if (dgvSchedules.Columns.Contains("Id"))
//                dgvSchedules.Columns["Id"].Visible = false;
//            if (dgvSchedules.Columns.Contains("EmployeeId"))
//                dgvSchedules.Columns["EmployeeId"].Visible = false;
//        }

//        #region Event Handlers

//        private async Task FilterByEmployee()
//        {
//            try
//            {
//                var employeeId = cbxEmployeeFilter.SelectedValue as long?;
//                await _presenter.FilterByEmployeeAsync(employeeId);
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
//                {
//                    await LoadWeekView();
//                }
//                else
//                {
//                    await FilterByDateRange();
//                }
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
//        }

//        private void OnDataLoaded(object sender, EventArgs e)
//        {
//            UpdatePaginationInfo();
//            if (_isWeekViewActive)
//            {
//                _ = Task.Run(LoadWeekView);
//            }
//        }

//        private async void OnGanttTaskClicked(object sender, GanttTaskViewModel task)
//        {
//            await ShowEditScheduleDialog(ConvertGanttTaskToSchedule(task));
//        }

//        private void OnGanttTimeSlotClicked(object sender, DateTime dateTime)
//        {
//            var selectedTime = new TimeOnly(dateTime.Hour, dateTime.Minute);
//            ShowAddScheduleDialog(dateTime.Date, selectedTime);
//        }

//        private EmployeeScheduleViewModel ConvertGanttTaskToSchedule(GanttTaskViewModel ganttTask)
//        {
//            return new EmployeeScheduleViewModel
//            {
//                Id = ganttTask.Id,
//                EmployeeId = ganttTask.EmployeeId,
//                EmployeeName = ganttTask.EmployeeName,
//                ShiftDate = ganttTask.StartDate.Date,
//                StartTime = TimeOnly.FromDateTime(ganttTask.StartDate),
//                EndTime = TimeOnly.FromDateTime(ganttTask.EndDate),
//                Status = ganttTask.Status
//            };
//        }

//        #endregion

//        #region View Toggle Methods

//        private void ToggleToWeekView()
//        {
//            _isWeekViewActive = true;
//            pnlWeekView.Visible = true;
//            dgvSchedules.Visible = false;

//            btnWeekView.BackColor = Color.FromArgb(0, 120, 215);
//            btnMonthView.BackColor = Color.FromArgb(73, 75, 111);

//            if (_useGanttChart && ganttChart != null)
//            {
//                ganttChart.Visible = true;
//                tlpWeekView.Visible = false;
//            }
//            else
//            {
//                tlpWeekView.Visible = true;
//                if (ganttChart != null)
//                    ganttChart.Visible = false;
//            }

//            _ = Task.Run(LoadWeekView);
//        }

//        private void ToggleToMonthView()
//        {
//            _isWeekViewActive = false;
//            pnlWeekView.Visible = false;
//            dgvSchedules.Visible = true;

//            btnMonthView.BackColor = Color.FromArgb(0, 120, 215);
//            btnWeekView.BackColor = Color.FromArgb(73, 75, 111);
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

//            // Update Gantt Chart if it exists
//            if (_useGanttChart && ganttChart != null)
//            {
//                _ = Task.Run(() => LoadGanttChart());
//            }
//            else
//            {
//                // Fallback to table layout view
//                UpdateTableWeekView(weeklySchedule);
//            }
//        }

//        private async Task LoadGanttChart()
//        {
//            try
//            {
//                var ganttData = await _presenter.GetGanttDataAsync(_currentWeekStart, _currentWeekStart.AddDays(6));

//                if (InvokeRequired)
//                {
//                    Invoke(() => UpdateGanttChart(ganttData));
//                }
//                else
//                {
//                    UpdateGanttChart(ganttData);
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi tải Gantt chart: {ex.Message}");
//            }
//        }

//        private void UpdateGanttChart(List<GanttTaskViewModel> ganttData)
//        {
//            if (ganttChart != null)
//            {
//                ganttChart.StartDate = _currentWeekStart;
//                ganttChart.EndDate = _currentWeekStart.AddDays(6);
//                ganttChart.Tasks = ganttData;
//            }
//        }

//        private void UpdateTableWeekView(WeeklyScheduleViewModel weeklySchedule)
//        {
//            // Clear existing controls
//            tlpWeekView.Controls.Clear();
//            tlpWeekView.RowStyles.Clear();
//            tlpWeekView.ColumnStyles.Clear();

//            // Setup column and row count
//            tlpWeekView.ColumnCount = 8; // Time column + 7 days
//            tlpWeekView.RowCount = 17; // Header + 16 hours (6 AM to 10 PM)

//            // Setup column styles
//            tlpWeekView.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80)); // Time column
//            for (int i = 1; i <= 7; i++)
//            {
//                tlpWeekView.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 14.28f)); // Day columns
//            }

//            // Setup row styles
//            tlpWeekView.RowStyles.Add(new RowStyle(SizeType.Absolute, 40)); // Header row
//            for (int i = 1; i <= 16; i++)
//            {
//                tlpWeekView.RowStyles.Add(new RowStyle(SizeType.Absolute, 30)); // Hour rows
//            }

//            // Add header row
//            AddWeekViewHeader();

//            // Add time slots and schedules
//            for (int hour = 6; hour < 22; hour++) // 6 AM to 10 PM
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
//                dayPanel.Click += (s, e) => OnTimeSlotClick(dayIndex, hour);
//                dayPanel.Cursor = Cursors.Hand;

//                tlpWeekView.Controls.Add(dayPanel, dayIndex + 1, row);
//            }
//        }

//        private void AddScheduleToTimeSlot(Panel timeSlotPanel, EmployeeScheduleViewModel schedule, int hour)
//        {
//            var scheduleLabel = new Label
//            {
//                Text = $"{schedule.EmployeeName}\n{schedule.StartTime:HH:mm}-{schedule.EndTime:HH:mm}",
//                BackColor = GetStatusColor(schedule.Status),
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

//        private Color GetStatusColor(string status)
//        {
//            return status switch
//            {
//                "SCHEDULED" => Color.FromArgb(0, 120, 215),
//                "COMPLETED" => Color.FromArgb(76, 175, 80),
//                "ABSENT" => Color.FromArgb(244, 67, 54),
//                "CANCELLED" => Color.FromArgb(158, 158, 158),
//                _ => Color.FromArgb(73, 75, 111)
//            };
//        }

//        private void OnTimeSlotClick(int dayIndex, int hour)
//        {
//            var selectedDate = _currentWeekStart.AddDays(dayIndex);
//            var selectedTime = new TimeOnly(hour, 0);

//            ShowAddScheduleDialog(selectedDate, selectedTime);
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
//            await LoadWeekView();
//        }

//        private async Task NavigateToNextWeek()
//        {
//            _currentWeekStart = _currentWeekStart.AddDays(7);
//            dtpStartDate.Value = _currentWeekStart;
//            dtpEndDate.Value = _currentWeekStart.AddDays(6);
//            await LoadWeekView();
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
//                    await RefreshData();
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
//                    await RefreshData();
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
//                    await RefreshData();
//                    ShowSuccessMessage("Xóa lịch làm việc thành công!");
//                }
//            }
//            catch (Exception ex)
//            {
//                ShowErrorMessage($"Lỗi khi xóa lịch: {ex.Message}");
//            }
//        }

//        private EmployeeScheduleViewModel GetSelectedSchedule()
//        {
//            if (dgvSchedules.SelectedRows.Count == 0)
//                return null;

//            return dgvSchedules.SelectedRows[0].DataBoundItem as EmployeeScheduleViewModel;
//        }

//        private async Task RefreshData()
//        {
//            await _presenter.FilterByDateRangeAsync(dtpStartDate.Value.Date, dtpEndDate.Value.Date);
//        }

//        #endregion

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

//        private void ShowErrorMessage(string message)
//        {
//            MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        }

//        private void ShowWarningMessage(string message)
//        {
//            MessageBox.Show(message, "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//        }

//        private void ShowSuccessMessage(string message)
//        {
//            MessageBox.Show(message, "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        #endregion

//        protected override void Dispose(bool disposing)
//        {
//            if (disposing)
//            {
//                components?.Dispose();
//            }
//            base.Dispose(disposing);
//        }
//    }
//}