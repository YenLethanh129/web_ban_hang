using Dashboard.Winform.ViewModels.ScheduleModels;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Dashboard.Winform.Controls
{
    public partial class GanttChartControl : UserControl
    {
        private List<GanttTaskViewModel> _tasks = new List<GanttTaskViewModel>();
        private DateTime _startDate = DateTime.Today;
        private DateTime _endDate = DateTime.Today.AddDays(7);
        private int _rowHeight = 30;
        private int _headerHeight = 50;
        private int _employeeColumnWidth = 150;
        private int _dayWidth = 100;
        private VScrollBar _vScrollBar;
        private HScrollBar _hScrollBar;
        private int _scrollOffsetX = 0;
        private int _scrollOffsetY = 0;
        private ToolTip _toolTip;
        private GanttTaskViewModel? _hoveredTask = null; // Make nullable

        // Colors
        private readonly Color _headerColor = Color.FromArgb(73, 75, 111);
        private readonly Color _rowAlternateColor = Color.FromArgb(52, 55, 96);
        private readonly Color _rowColor = Color.FromArgb(42, 45, 86);
        private readonly Color _gridColor = Color.FromArgb(63, 66, 101);
        private readonly Color _textColor = Color.White;
        private readonly Color _weekendColor = Color.FromArgb(32, 35, 76);

        // Make events nullable
        public event EventHandler<GanttTaskViewModel>? TaskClicked;
        public event EventHandler<DateTime>? TimeSlotClicked;

        public List<GanttTaskViewModel> Tasks
        {
            get => _tasks;
            set
            {
                _tasks = value ?? new List<GanttTaskViewModel>();
                Invalidate();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                UpdateScrollBars();
                Invalidate();
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                UpdateScrollBars();
                Invalidate();
            }
        }

        public int RowHeight
        {
            get => _rowHeight;
            set
            {
                _rowHeight = Math.Max(20, value);
                UpdateScrollBars();
                Invalidate();
            }
        }

        public GanttChartControl()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.UserPaint |
                     ControlStyles.DoubleBuffer |
                     ControlStyles.ResizeRedraw, true);

            BackColor = _rowColor;

            // Initialize components to avoid nullable warnings
            _vScrollBar = new VScrollBar();
            _hScrollBar = new HScrollBar();
            _toolTip = new ToolTip();

            InitializeScrollBars();
            InitializeToolTip();
        }

        private void InitializeScrollBars()
        {
            // Re-initialize to ensure proper setup
            _vScrollBar = new VScrollBar
            {
                Dock = DockStyle.Right,
                Visible = false
            };
            _vScrollBar.Scroll += VScrollBar_Scroll;

            _hScrollBar = new HScrollBar
            {
                Dock = DockStyle.Bottom,
                Visible = false
            };
            _hScrollBar.Scroll += HScrollBar_Scroll;

            Controls.Add(_vScrollBar);
            Controls.Add(_hScrollBar);
        }

        private void InitializeToolTip()
        {
            _toolTip = new ToolTip
            {
                IsBalloon = true,
                ShowAlways = true
            };
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            UpdateScrollBars();
        }

        private void UpdateScrollBars()
        {
            var employees = GetUniqueEmployees();
            var totalHeight = _headerHeight + (employees.Count * _rowHeight);
            var totalWidth = _employeeColumnWidth + (int)(_endDate - _startDate).TotalDays * _dayWidth;

            // Vertical scroll bar
            if (totalHeight > Height && Height > 0)
            {
                _vScrollBar.Visible = true;
                _vScrollBar.Maximum = Math.Max(0, totalHeight - Height);
                _vScrollBar.LargeChange = Math.Max(1, Height);
                _vScrollBar.SmallChange = _rowHeight;
            }
            else
            {
                _vScrollBar.Visible = false;
                _scrollOffsetY = 0;
            }

            // Horizontal scroll bar
            var clientWidth = Width - (_vScrollBar.Visible ? _vScrollBar.Width : 0);
            if (totalWidth > clientWidth && clientWidth > 0)
            {
                _hScrollBar.Visible = true;
                _hScrollBar.Maximum = Math.Max(0, totalWidth - clientWidth);
                _hScrollBar.LargeChange = Math.Max(1, clientWidth);
                _hScrollBar.SmallChange = _dayWidth;
            }
            else
            {
                _hScrollBar.Visible = false;
                _scrollOffsetX = 0;
            }
        }

        private void VScrollBar_Scroll(object? sender, ScrollEventArgs e)
        {
            _scrollOffsetY = e.NewValue;
            Invalidate();
        }

        private void HScrollBar_Scroll(object? sender, ScrollEventArgs e)
        {
            _scrollOffsetX = e.NewValue;
            Invalidate();
        }

        private List<string> GetUniqueEmployees()
        {
            return _tasks?.Select(t => t.EmployeeName)
                          .Where(name => !string.IsNullOrEmpty(name))
                          .Distinct()
                          .OrderBy(name => name)
                          .ToList() ?? new List<string>();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;

            var clientRect = new Rectangle(0, 0, Width, Height);

            // Clear background
            using (var brush = new SolidBrush(_rowColor))
            {
                g.FillRectangle(brush, clientRect);
            }

            DrawHeader(g);
            DrawEmployeeColumn(g);
            DrawGrid(g);
            DrawTasks(g);
            DrawCurrentTimeLine(g);
        }

        private void DrawHeader(Graphics g)
        {
            var totalDays = Math.Max(1, (int)(_endDate - _startDate).TotalDays + 1);
            var headerRect = new Rectangle(-_scrollOffsetX, 0,
                _employeeColumnWidth + totalDays * _dayWidth, _headerHeight);

            using (var brush = new SolidBrush(_headerColor))
            {
                g.FillRectangle(brush, headerRect);
            }

            // Draw employee column header
            var employeeHeaderRect = new Rectangle(-_scrollOffsetX, 0, _employeeColumnWidth, _headerHeight);
            DrawHeaderText(g, "Nhân viên", employeeHeaderRect);

            // Draw date headers
            var currentDate = _startDate;
            var x = _employeeColumnWidth - _scrollOffsetX;

            while (currentDate <= _endDate)
            {
                var dayRect = new Rectangle(x, 0, _dayWidth, _headerHeight);
                var isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;

                if (isWeekend)
                {
                    using (var brush = new SolidBrush(_weekendColor))
                    {
                        g.FillRectangle(brush, dayRect);
                    }
                }

                var culture = new System.Globalization.CultureInfo("vi-VN");
                var dateText = $"{currentDate:dd/MM}\n{culture.DateTimeFormat.GetAbbreviatedDayName(currentDate.DayOfWeek)}";
                DrawHeaderText(g, dateText, dayRect);

                x += _dayWidth;
                currentDate = currentDate.AddDays(1);
            }

            // Draw header border
            using (var pen = new Pen(_gridColor))
            {
                var totalDaysWidth = totalDays * _dayWidth;
                g.DrawLine(pen, -_scrollOffsetX, _headerHeight,
                    _employeeColumnWidth + totalDaysWidth - _scrollOffsetX, _headerHeight);
            }
        }

        private void DrawEmployeeColumn(Graphics g)
        {
            var employees = GetUniqueEmployees();
            var y = _headerHeight - _scrollOffsetY;

            for (int i = 0; i < employees.Count; i++)
            {
                if (y > Height) break;
                if (y + _rowHeight < 0)
                {
                    y += _rowHeight;
                    continue;
                }

                var rowRect = new Rectangle(-_scrollOffsetX, y, _employeeColumnWidth, _rowHeight);
                var isAlternate = i % 2 == 1;

                using (var brush = new SolidBrush(isAlternate ? _rowAlternateColor : _rowColor))
                {
                    g.FillRectangle(brush, rowRect);
                }

                DrawEmployeeText(g, employees[i], rowRect);

                // Draw row separator
                using (var pen = new Pen(_gridColor))
                {
                    g.DrawLine(pen, -_scrollOffsetX, y + _rowHeight, _employeeColumnWidth - _scrollOffsetX, y + _rowHeight);
                }

                y += _rowHeight;
            }

            // Draw employee column border
            using (var pen = new Pen(_gridColor))
            {
                g.DrawLine(pen, _employeeColumnWidth - _scrollOffsetX, 0,
                    _employeeColumnWidth - _scrollOffsetX, Height);
            }
        }

        private void DrawGrid(Graphics g)
        {
            var employees = GetUniqueEmployees();
            var currentDate = _startDate;
            var x = _employeeColumnWidth - _scrollOffsetX;

            // Draw vertical grid lines (days)
            while (currentDate <= _endDate)
            {
                if (x > -_dayWidth && x < Width)
                {
                    var isWeekend = currentDate.DayOfWeek == DayOfWeek.Saturday || currentDate.DayOfWeek == DayOfWeek.Sunday;

                    if (isWeekend)
                    {
                        var weekendRect = new Rectangle(x, _headerHeight - _scrollOffsetY,
                            _dayWidth, Math.Max(0, employees.Count * _rowHeight));
                        using (var brush = new SolidBrush(Color.FromArgb(20, _weekendColor)))
                        {
                            g.FillRectangle(brush, weekendRect);
                        }
                    }

                    using (var pen = new Pen(_gridColor, 1))
                    {
                        g.DrawLine(pen, x, _headerHeight, x, Height);
                    }
                }

                x += _dayWidth;
                currentDate = currentDate.AddDays(1);
            }
        }

        private void DrawTasks(Graphics g)
        {
            var employees = GetUniqueEmployees();

            foreach (var task in _tasks)
            {
                if (task == null) continue;

                var employeeIndex = employees.IndexOf(task.EmployeeName ?? string.Empty);
                if (employeeIndex == -1) continue;

                var y = _headerHeight + (employeeIndex * _rowHeight) - _scrollOffsetY;
                if (y > Height || y + _rowHeight < 0) continue;

                var startX = _employeeColumnWidth + (int)(task.StartDate.Date - _startDate).TotalDays * _dayWidth;
                var endX = _employeeColumnWidth + (int)(task.EndDate.Date - _startDate).TotalDays * _dayWidth;

                // Adjust for time within the day
                var startTimeOffset = (int)(task.StartDate.TimeOfDay.TotalHours / 24.0 * _dayWidth);
                var endTimeOffset = (int)(task.EndDate.TimeOfDay.TotalHours / 24.0 * _dayWidth);

                startX += startTimeOffset;
                var width = Math.Max(10, endX + endTimeOffset - startX);

                startX -= _scrollOffsetX;

                if (startX > Width || startX + width < _employeeColumnWidth) continue;

                var taskRect = new Rectangle(startX, y + 2, width, _rowHeight - 4);
                DrawTask(g, task, taskRect);
            }
        }

        private void DrawTask(Graphics g, GanttTaskViewModel task, Rectangle taskRect)
        {
            Color taskColor;
            try
            {
                taskColor = ColorTranslator.FromHtml(task.Color ?? "#0078D4");
            }
            catch
            {
                taskColor = Color.FromArgb(0, 120, 215); // Default blue
            }

            var isHovered = _hoveredTask?.Id == task.Id;

            // Draw task background
            var backgroundColor = isHovered ?
                Color.FromArgb(Math.Min(255, taskColor.R + 30), Math.Min(255, taskColor.G + 30), Math.Min(255, taskColor.B + 30)) :
                taskColor;

            using (var brush = new SolidBrush(backgroundColor))
            {
                g.FillRectangle(brush, taskRect);
            }

            // Draw progress bar
            if (task.Progress > 0 && task.Progress <= 100)
            {
                var progressWidth = (int)(taskRect.Width * (task.Progress / 100.0));
                var progressRect = new Rectangle(taskRect.X, taskRect.Y, progressWidth, taskRect.Height);

                using (var brush = new SolidBrush(Color.FromArgb(100, Color.White)))
                {
                    g.FillRectangle(brush, progressRect);
                }
            }

            // Draw task border
            var borderColor = Color.FromArgb(
                Math.Max(0, taskColor.R - 40),
                Math.Max(0, taskColor.G - 40),
                Math.Max(0, taskColor.B - 40));

            using (var pen = new Pen(borderColor, 1))
            {
                g.DrawRectangle(pen, taskRect);
            }

            // Draw task text
            if (taskRect.Width > 50)
            {
                var taskText = $"{task.StartDate:HH:mm}-{task.EndDate:HH:mm}";
                if (taskRect.Width > 100 && !string.IsNullOrEmpty(task.EmployeeName))
                    taskText = $"{task.EmployeeName}\n{taskText}";

                using (var font = new Font("Segoe UI", 7f, FontStyle.Regular))
                using (var brush = new SolidBrush(Color.White))
                {
                    var textRect = new RectangleF(taskRect.X + 4, taskRect.Y + 2,
                        Math.Max(0, taskRect.Width - 8), Math.Max(0, taskRect.Height - 4));
                    var format = new StringFormat
                    {
                        Alignment = StringAlignment.Near,
                        LineAlignment = StringAlignment.Center,
                        Trimming = StringTrimming.EllipsisCharacter
                    };

                    g.DrawString(taskText, font, brush, textRect, format);
                }
            }

            // Store task bounds for click detection
            task.Tag = taskRect;
        }

        private void DrawCurrentTimeLine(Graphics g)
        {
            var now = DateTime.Now;
            if (now < _startDate || now > _endDate) return;

            var x = _employeeColumnWidth + (int)(now.Date - _startDate).TotalDays * _dayWidth;
            x += (int)(now.TimeOfDay.TotalHours / 24.0 * _dayWidth);
            x -= _scrollOffsetX;

            if (x < _employeeColumnWidth || x > Width) return;

            using (var pen = new Pen(Color.Red, 2))
            {
                g.DrawLine(pen, x, _headerHeight, x, Height);
            }

            // Draw time indicator
            using (var brush = new SolidBrush(Color.Red))
            using (var font = new Font("Segoe UI", 8f, FontStyle.Bold))
            {
                var timeText = now.ToString("HH:mm");
                var textSize = g.MeasureString(timeText, font);
                var textRect = new RectangleF(x - textSize.Width / 2, _headerHeight - 20, textSize.Width, textSize.Height);

                g.FillRectangle(brush, Rectangle.Round(textRect));
                g.DrawString(timeText, font, Brushes.White, textRect);
            }
        }

        private void DrawHeaderText(Graphics g, string text, Rectangle rect)
        {
            using (var font = new Font("Segoe UI", 8f, FontStyle.Bold))
            using (var brush = new SolidBrush(_textColor))
            {
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center
                };

                g.DrawString(text ?? string.Empty, font, brush, rect, format);
            }
        }

        private void DrawEmployeeText(Graphics g, string employeeName, Rectangle rect)
        {
            using (var font = new Font("Segoe UI", 8f))
            using (var brush = new SolidBrush(_textColor))
            {
                var format = new StringFormat
                {
                    Alignment = StringAlignment.Near,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                var textRect = new Rectangle(rect.X + 8, rect.Y, Math.Max(0, rect.Width - 16), rect.Height);
                g.DrawString(employeeName ?? string.Empty, font, brush, textRect, format);
            }
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            var previousHovered = _hoveredTask;
            _hoveredTask = GetTaskAtPoint(e.Location);

            if (_hoveredTask != previousHovered)
            {
                Cursor = _hoveredTask != null ? Cursors.Hand : Cursors.Default;

                if (_hoveredTask != null)
                {
                    var tooltip = $"{_hoveredTask.EmployeeName ?? "N/A"}\n" +
                                 $"{_hoveredTask.StartDate:dd/MM/yyyy HH:mm} - {_hoveredTask.EndDate:HH:mm}\n" +
                                 $"Trạng thái: {_hoveredTask.Status ?? "N/A"}\n" +
                                 $"Thời lượng: {_hoveredTask.FormattedDuration ?? "N/A"}";
                    _toolTip.SetToolTip(this, tooltip);
                }
                else
                {
                    _toolTip.RemoveAll();
                }

                Invalidate();
            }
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            var task = GetTaskAtPoint(e.Location);
            if (task != null)
            {
                TaskClicked?.Invoke(this, task);
            }
            else
            {
                var date = GetDateAtPoint(e.Location);
                if (date.HasValue)
                {
                    TimeSlotClicked?.Invoke(this, date.Value);
                }
            }
        }

        private GanttTaskViewModel? GetTaskAtPoint(Point point)
        {
            if (_tasks == null) return null;

            foreach (var task in _tasks)
            {
                if (task?.Tag is Rectangle rect && rect.Contains(point))
                {
                    return task;
                }
            }
            return null;
        }

        private DateTime? GetDateAtPoint(Point point)
        {
            if (point.Y < _headerHeight || point.X < _employeeColumnWidth) return null;

            var adjustedX = point.X + _scrollOffsetX - _employeeColumnWidth;
            var dayIndex = adjustedX / _dayWidth;

            var totalDays = (_endDate - _startDate).TotalDays;
            if (dayIndex < 0 || dayIndex >= totalDays) return null;

            var clickedDate = _startDate.AddDays(dayIndex);
            var hourOffset = (adjustedX % _dayWidth) / (double)_dayWidth * 24;

            return clickedDate.AddHours(hourOffset);
        }

        public void ScrollToDate(DateTime date)
        {
            if (date < _startDate || date > _endDate) return;

            var targetX = _employeeColumnWidth + (int)(date - _startDate).TotalDays * _dayWidth;
            var centerX = Width / 2;

            _scrollOffsetX = Math.Max(0, Math.Min(_hScrollBar.Maximum, targetX - centerX));
            if (_hScrollBar.Visible && _hScrollBar.Maximum > 0)
                _hScrollBar.Value = Math.Min(_scrollOffsetX, _hScrollBar.Maximum);

            Invalidate();
        }

        public void ScrollToEmployee(string employeeName)
        {
            if (string.IsNullOrEmpty(employeeName)) return;

            var employees = GetUniqueEmployees();
            var index = employees.IndexOf(employeeName);
            if (index == -1) return;

            var targetY = _headerHeight + index * _rowHeight;
            var centerY = Height / 2;

            _scrollOffsetY = Math.Max(0, Math.Min(_vScrollBar.Maximum, targetY - centerY));
            if (_vScrollBar.Visible && _vScrollBar.Maximum > 0)
                _vScrollBar.Value = Math.Min(_scrollOffsetY, _vScrollBar.Maximum);

            Invalidate();
        }
    }
}