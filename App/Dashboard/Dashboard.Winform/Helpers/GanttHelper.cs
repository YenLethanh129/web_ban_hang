//// First, you need to install the jakesee/GanttChart NuGet package:
//// Install-Package GanttChart
//// Or add this to your .csproj file:
//// <PackageReference Include="GanttChart" Version="1.0.0" />

//using GanttChart;
//using Dashboard.Winform.ViewModels.ScheduleModels;

//namespace Dashboard.Winform.Helpers
//{
//    public static class GanttHelper
//    {
//        public static GanttControl CreateScheduleGanttChart()
//        {
//            var ganttChart = new GanttControl
//            {
//                // Appearance
//                BackColor = Color.FromArgb(42, 45, 86),
//                ForeColor = Color.White,

//                // Grid settings
//                GridLineColor = Color.FromArgb(73, 75, 111),
//                //ShowGrid = true,

//                // Header settings
//                HeaderBackColor = Color.FromArgb(73, 75, 111),
//                HeaderForeColor = Color.White,
//                HeaderHeight = 60,

//                // Timeline settings
//                TimelineBackColor = Color.FromArgb(42, 45, 86),
//                TimelineForeColor = Color.White,

//                // Selection settings
//                SelectionBackColor = Color.FromArgb(0, 120, 215),
//                SelectionForeColor = Color.White,

//                // Task appearance
//                TaskBackColor = Color.FromArgb(0, 120, 215),
//                TaskForeColor = Color.White,
//                TaskCompletedBackColor = Color.FromArgb(76, 175, 80),

//                // Layout settings
//                RowHeaderWidth = 200,
//                ShowTaskNames = true,
//                ShowProgressBars = true,

//                // Interaction settings
//                AllowTaskEdit = true,
//                AllowTaskMove = true,
//                AllowTaskResize = true,

//                // Dock to fill container
//                Dock = DockStyle.Fill
//            };

//            return ganttChart;
//        }

//        public static GanttTask CreateTaskFromSchedule(EmployeeScheduleViewModel schedule, bool isGroup = false)
//        {
//            var task = new GanttTask
//            {
//                Id = schedule.Id,
//                Name = isGroup ? $"{schedule.EmployeeName} ({schedule.PositionName})" : schedule.ShiftDuration,
//                StartDate = schedule.ShiftDate.Add(schedule.StartTime.ToTimeSpan()),
//                EndDate = schedule.ShiftDate.Add(schedule.EndTime.ToTimeSpan()),
//                Progress = GetProgressFromStatus(schedule.Status),
//                BackColor = schedule.StatusColor,
//                ForeColor = Color.White,
//                IsGroup = isGroup,
//                Tag = schedule
//            };

//            return task;
//        }

//        public static GanttTask CreateGroupTask(string employeeName, string positionName, DateTime startDate, DateTime endDate, long employeeId)
//        {
//            return new GanttTask
//            {
//                Id = employeeId,
//                Name = $"{employeeName} ({positionName})",
//                StartDate = startDate,
//                EndDate = endDate,
//                IsGroup = true,
//                BackColor = Color.FromArgb(73, 75, 111),
//                ForeColor = Color.White,
//                Progress = 0
//            };
//        }

//        private static int GetProgressFromStatus(string status)
//        {
//            return status switch
//            {
//                "COMPLETED" => 100,
//                "SCHEDULED" => 0,
//                "ABSENT" => 0,
//                "CANCELLED" => 0,
//                _ => 0
//            };
//        }

//        public static void ConfigureTimelineScale(GanttControl ganttChart, DateTime startDate, DateTime endDate)
//        {
//            ganttChart.TimelineStart = startDate.Date;
//            ganttChart.TimelineEnd = endDate.Date.AddDays(1);

//            // Set appropriate scale based on date range
//            var daysDifference = (endDate - startDate).TotalDays;

//            if (daysDifference <= 7)
//            {
//                // Weekly view - show hours
//                ganttChart.TimelineScale = TimelineScale.Hour;
//                ganttChart.TimelineScaleSize = 24; // Show every hour
//            }
//            else if (daysDifference <= 31)
//            {
//                // Monthly view - show days
//                ganttChart.TimelineScale = TimelineScale.Day;
//                ganttChart.TimelineScaleSize = 1; // Show every day
//            }
//            else
//            {
//                // Longer periods - show weeks
//                ganttChart.TimelineScale = TimelineScale.Week;
//                ganttChart.TimelineScaleSize = 1; // Show every week
//            }
//        }

//        public static string GetStatusDisplayName(string status)
//        {
//            return status switch
//            {
//                "SCHEDULED" => "Đã lên lịch",
//                "COMPLETED" => "Hoàn thành",
//                "ABSENT" => "Vắng mặt",
//                "CANCELLED" => "Đã hủy",
//                _ => status
//            };
//        }
//    }

//    // Custom event args for Gantt chart events
//    public class GanttScheduleEventArgs : EventArgs
//    {
//        public EmployeeScheduleViewModel Schedule { get; set; }
//        public GanttTask Task { get; set; }

//        public GanttScheduleEventArgs(EmployeeScheduleViewModel schedule, GanttTask task)
//        {
//            Schedule = schedule;
//            Task = task;
//        }
//    }

//    // Extension methods for better integration
//    public static class GanttExtensions
//    {
//        public static void LoadSchedules(this GanttControl ganttChart, List<GanttTaskViewModel> tasks)
//        {
//            ganttChart.ClearTasks();

//            var groupedTasks = tasks.GroupBy(t => t.EmployeeId)
//                                  .OrderBy(g => g.First().EmployeeName)
//                                  .ToList();

//            foreach (var employeeGroup in groupedTasks)
//            {
//                var employee = employeeGroup.First();
//                var employeeTasks = employeeGroup.OrderBy(t => t.StartDate).ToList();

//                // Add employee as a group
//                var groupTask = GanttHelper.CreateGroupTask(
//                    employee.EmployeeName,
//                    employee.PositionName,
//                    employeeTasks.Min(t => t.StartDate),
//                    employeeTasks.Max(t => t.EndDate),
//                    employee.EmployeeId);

//                ganttChart.AddTask(groupTask);

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
//                    ganttChart.AddTask(ganttTask);
//                }
//            }

//            // Configure timeline
//            if (tasks.Any())
//            {
//                GanttHelper.ConfigureTimelineScale(ganttChart,
//                    tasks.Min(t => t.StartDate).Date,
//                    tasks.Max(t => t.EndDate).Date);
//            }

//            ganttChart.Refresh();
//        }

//        public static EmployeeScheduleViewModel GetSelectedSchedule(this GanttControl ganttChart)
//        {
//            var selectedTask = ganttChart.SelectedTask;
//            return selectedTask?.Tag as EmployeeScheduleViewModel;
//        }

//        public static List<EmployeeScheduleViewModel> GetSelectedSchedules(this GanttControl ganttChart)
//        {
//            return ganttChart.SelectedTasks
//                .Select(task => task.Tag as EmployeeScheduleViewModel)
//                .Where(schedule => schedule != null)
//                .ToList();
//        }
//    }
//}

