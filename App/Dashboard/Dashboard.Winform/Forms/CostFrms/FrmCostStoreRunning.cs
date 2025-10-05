using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using Dashboard.BussinessLogic.Dtos.BranchDtos;
using Dashboard.BussinessLogic.Services.BranchServices;
using Dashboard.BussinessLogic.Services.ReportServices;
using Dashboard.Winform.Interfaces;

namespace Dashboard.Winform.Forms.CostFrms
{
    public partial class FrmCostStoreRunning : Form, IBlurLoadingServiceAware
    {
        private readonly IBranchService _branchService;
        private IBlurLoadingService? _blurLoading;
        private readonly List<BranchDto> _branches = new();
        private readonly List<BranchExpenseDto> _expenses = new();
        private long? _selectedBranchId;
        private CancellationTokenSource? _loadCts;

        // Time filter properties
        private DateTime _fromDate;
        private DateTime _toDate;
        private TimeFilterType _currentFilter = TimeFilterType.OneMonth;

        public FrmCostStoreRunning(IBranchService branchService)
        {
            _branchService = branchService;
            InitializeComponent();
            ConfigureBranchListLayout();
            InitializeTimeFilters();
            WireEvents();
            this.FormClosed += (_, _) => CleanupResources();
        }

        public void SetBlurLoadingService(IBlurLoadingService blurLoadingService) => _blurLoading = blurLoadingService;

        private void CleanupResources()
        {
            try
            {
                _loadCts?.Cancel();
                _loadCts?.Dispose();
            }
            catch { /* ignore */ }
        }

        /// <summary>
        /// Khởi tạo bộ lọc thời gian - mặc định 1 tháng
        /// </summary>
        private void InitializeTimeFilters()
        {
            // Mặc định lấy 1 tháng gần nhất
            _toDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1); // Cuối ngày hôm nay
            _fromDate = _toDate.AddMonths(-1).Date; // 1 tháng trước

            // Cập nhật giao diện các nút thời gian
            UpdateTimeFilterButtons();
        }

        /// <summary>
        /// Cập nhật trạng thái hiển thị các nút thời gian
        /// </summary>
        private void UpdateTimeFilterButtons()
        {
            // Reset tất cả nút về trạng thái bình thường
            ResetTimeButtonStyles();

            // Highlight nút được chọn
            switch (_currentFilter)
            {
                case TimeFilterType.OneMonth:
                    HighlightButton(btnOption1M);
                    break;
                case TimeFilterType.ThreeMonths:
                    HighlightButton(btnOption3M);
                    break;
                case TimeFilterType.OneYear:
                    HighlightButton(btnOption1Y);
                    break;
                case TimeFilterType.Custom:
                    HighlightButton(btnOptionTime);
                    break;
            }
        }

        private void ResetTimeButtonStyles()
        {
            var buttons = new[] { btnOption1M, btnOption3M, btnOption1Y, btnOptionTime };
            foreach (var btn in buttons)
            {
                btn.ForeColor = Color.White;
            }
        }

        private void HighlightButton(Button button)
        {
            button.ForeColor = Color.White;
        }

        private void WireEvents()
        {
            Load += async (_, _) => await SafeRunAsync(LoadBranchesAsync, "Đang tải chi nhánh...");
            btnTotalBranch.Click += async (_, _) => await SafeRunAsync(LoadAllExpensesAsync, "Đang tải tất cả chi phí...");
            
            // Wire time filter events - XÓA TẤT CẢ LEGACY EVENT HANDLERS CŨ
            // Chỉ giữ lại async handlers
            btnOption1M.Click += async (_, _) => await OnTimeFilterChanged(TimeFilterType.OneMonth);
            btnOption3M.Click += async (_, _) => await OnTimeFilterChanged(TimeFilterType.ThreeMonths);
            btnOption1Y.Click += async (_, _) => await OnTimeFilterChanged(TimeFilterType.OneYear);
            btnOptionTime.Click += async (_, _) => await OnCustomTimeFilterClicked();
            
            pnAddCost.Visible = false; // Ẩn panel thêm chi phí tạm thời
        }

        /// <summary>
        /// Xử lý khi người dùng chọn bộ lọc thời gian
        /// </summary>
        private async Task OnTimeFilterChanged(TimeFilterType filterType)
        {
            _currentFilter = filterType;
            
            // Cập nhật khoảng thời gian
            var endDate = DateTime.Now.Date.AddDays(1).AddSeconds(-1);
            
            switch (filterType)
            {
                case TimeFilterType.OneMonth:
                    _fromDate = endDate.AddMonths(-1).Date;
                    _toDate = endDate;
                    pnListCost.BackColor = btnOption1M.BackColor;
                    break;
                case TimeFilterType.ThreeMonths:
                    _fromDate = endDate.AddMonths(-3).Date;
                    _toDate = endDate;
                    pnListCost.BackColor = btnOption3M.BackColor;
                    break;
                case TimeFilterType.OneYear:
                    _fromDate = endDate.AddYears(-1).Date;
                    _toDate = endDate;
                    pnListCost.BackColor = btnOption1Y.BackColor;
                    break;
            }

            UpdateTimeFilterButtons();
            await ReloadCurrentView();
        }

        /// <summary>
        /// Xử lý khi người dùng chọn thời gian tùy chỉnh
        /// </summary>
        private async Task OnCustomTimeFilterClicked()
        {
            try
            {
                using var dateRangeDialog = new DateRangeDialog(_fromDate, _toDate);
                
                if (dateRangeDialog.ShowDialog(this) == DialogResult.OK)
                {
                    _fromDate = dateRangeDialog.FromDate;
                    _toDate = dateRangeDialog.ToDate;
                    _currentFilter = TimeFilterType.Custom;
                    
                    UpdateTimeFilterButtons();
                    await ReloadCurrentView();
                }

                pnListCost.BackColor = btnOptionTime.BackColor;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi mở dialog chọn thời gian: {ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Tải lại dữ liệu hiện tại với bộ lọc thời gian mới
        /// </summary>
        private async Task ReloadCurrentView()
        {
            if (_selectedBranchId.HasValue)
            {
                await SafeRunAsync(() => LoadBranchExpenseAsync(_selectedBranchId.Value), "Đang tải chi phí chi nhánh...");
            }
            else
            {
                await SafeRunAsync(LoadAllExpensesAsync, "Đang tải tất cả chi phí...");
            }
        }

        #region Branch UI Layout
        private void ConfigureBranchListLayout()
        {
            // Cấu hình fpnListBranch
            fpnListBranch.AutoScroll = true;
            fpnListBranch.WrapContents = false;
            fpnListBranch.FlowDirection = FlowDirection.TopDown;
            fpnListBranch.Padding = new Padding(0);
            fpnListBranch.Margin = new Padding(0);
            EnableDoubleBuffer(fpnListBranch);

            // Cấu hình fpnListCost
            fpnListCost.AutoScroll = true;
            fpnListCost.WrapContents = false;
            fpnListCost.FlowDirection = FlowDirection.TopDown;
            fpnListCost.Padding = new Padding(5);
            fpnListCost.Margin = new Padding(0);
            EnableDoubleBuffer(fpnListCost);

            DesignBranchButton(btnTotalBranch);
            btnTotalBranch.Text = "Tất cả chi nhánh";
        }

        private void AddNewBranchToFlow(BranchDto branch)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => AddNewBranchToFlow(branch)));
                return;
            }

            var btn = new Button();
            DesignBranchButton(btn);

            btn.Text = branch.Name;
            btn.Tag = branch.Id;
            btn.Width = fpnListBranch.ClientSize.Width - 4;
            btn.Height = 50;
            btn.Margin = new Padding(0, 0, 0, 2);
            btn.TextAlign = ContentAlignment.MiddleLeft;
            btn.Click += async (_, _) => await OnBranchButtonClicked(branch.Id);

            fpnListBranch.Controls.Add(btn);
        }

        private void DesignBranchButton(Button btn)
        {
            btn.BackColor = Color.FromArgb(99, 108, 203);
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Font = new Font("Microsoft Sans Serif", 11F);
            btn.ForeColor = Color.WhiteSmoke;
            btn.ImageAlign = ContentAlignment.MiddleLeft;
            btn.Padding = new Padding(10, 0, 0, 0);
            btn.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void EnableDoubleBuffer(Control ctl)
        {
            try
            {
                ctl.GetType()
                    .GetProperty("DoubleBuffered", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic)
                    ?.SetValue(ctl, true);
            }
            catch { /* ignore */ }
        }

        private void RenderBranchButtons()
        {
            fpnListBranch.SuspendLayout();
            fpnListBranch.Controls.Clear();

            // Thêm nút "Tất cả chi nhánh"
            DesignBranchButton(btnTotalBranch);
            btnTotalBranch.Text = "Tất cả chi nhánh";
            btnTotalBranch.Width = fpnListBranch.ClientSize.Width - 4;
            btnTotalBranch.Height = 50;
            fpnListBranch.Controls.Add(btnTotalBranch);

            // Thêm các nút chi nhánh
            foreach (var branch in _branches)
            {
                AddNewBranchToFlow(branch);
            }

            fpnListBranch.ResumeLayout();

            // Setup resize event
            fpnListBranch.Resize += (_, _) =>
            {
                var w = fpnListBranch.ClientSize.Width - 4;
                foreach (var btn in fpnListBranch.Controls.OfType<Button>())
                    btn.Width = w;
            };
        }
        #endregion

        #region Branch Loading  
        private async Task LoadBranchesAsync()
        {
            _branches.Clear();
            var page = await _branchService.GetBranchesAsync(new GetBranchesInput { PageNumber = 1, PageSize = 200 });
            _branches.AddRange(page.Items);

            RenderBranchButtons();

            // Mặc định load tất cả chi phí với bộ lọc thời gian
            await LoadAllExpensesAsync();
        }

        private async Task LoadBranchExpenseAsync(long branchId)
        {
            _selectedBranchId = branchId;
            _loadCts?.Cancel();
            _loadCts = new CancellationTokenSource();

            // Lấy chi phí của chi nhánh với bộ lọc thời gian
            var rawExpenses = await GetFilteredBranchExpensesAsync(branchId);

            _expenses.Clear();
            _expenses.AddRange(rawExpenses);

            // Highlight selected branch và update UI
            var selectedBranch = _branches.FirstOrDefault(b => b.Id == branchId);
            txtNameBranch.Text = selectedBranch?.Name ?? "Chi nhánh không xác định";

            RenderExpensesToUI();
        }

        private async Task LoadAllExpensesAsync()
        {
            _selectedBranchId = null;
            _loadCts?.Cancel();
            _loadCts = new CancellationTokenSource();

            // Lấy tất cả chi phí với bộ lọc thời gian
            var rawExpenses = await GetFilteredAllExpensesAsync();

            _expenses.Clear();
            _expenses.AddRange(rawExpenses);

            txtNameBranch.Text = "Tất cả chi nhánh";

            RenderExpensesToUI();
        }

        /// <summary>
        /// Lấy chi phí của chi nhánh với bộ lọc thời gian
        /// </summary>
        private async Task<List<BranchExpenseDto>> GetFilteredBranchExpensesAsync(long branchId)
        {
            var allExpenses = await _branchService.GetBranchExpensesByBranchIdAsync(branchId);
            return FilterExpensesByDateRange(allExpenses);
        }

        /// <summary>
        /// Lấy tất cả chi phí với bộ lọc thời gian
        /// </summary>
        private async Task<List<BranchExpenseDto>> GetFilteredAllExpensesAsync()
        {
            var allExpenses = await _branchService.GetAllBranchExpensesAsync();
            return FilterExpensesByDateRange(allExpenses);
        }

        /// <summary>
        /// Lọc chi phí theo khoảng thời gian
        /// </summary>
        private List<BranchExpenseDto> FilterExpensesByDateRange(List<BranchExpenseDto> expenses)
        {
            return expenses.Where(expense =>
            {
                // Kiểm tra nếu chi phí nằm trong khoảng thời gian
                // Nếu có EndDate, kiểm tra overlap với khoảng thời gian
                if (expense.EndDate.HasValue)
                {
                    // Chi phí có khoảng thời gian, kiểm tra overlap
                    return expense.StartDate <= _toDate && expense.EndDate >= _fromDate;
                }
                else
                {
                    // Chi phí không có EndDate, chỉ kiểm tra StartDate
                    return expense.StartDate >= _fromDate && expense.StartDate <= _toDate;
                }
            }).ToList();
        }

        private void RenderExpensesToUI()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(RenderExpensesToUI));
                return;
            }

            fpnListCost.SuspendLayout();

            // Xóa tất cả ItemCostStoreRunning cũ
            var itemsToRemove = fpnListCost.Controls.OfType<ItemCostStoreRunning>().ToList();
            foreach (var item in itemsToRemove)
            {
                fpnListCost.Controls.Remove(item);
                item.Dispose();
            }

            decimal totalAmount = 0;

            // Thêm từng BranchExpense vào UI
            foreach (var expense in _expenses.OrderByDescending(e => e.StartDate))
            {
                var itemControl = new ItemCostStoreRunning
                {
                    Width = fpnListCost.ClientSize.Width - 25,
                    Margin = new Padding(3, 2, 3, 2)
                };

                // Set dữ liệu cho ItemCostStoreRunning
                itemControl.SetData(expense);

                fpnListCost.Controls.Add(itemControl);
                totalAmount += expense.Amount;
            }

            // Cập nhật tổng tiền
            UpdateTotalAmount(totalAmount);

            fpnListCost.ResumeLayout();
        }

        private void UpdateTotalAmount(decimal totalAmount)
        {
            if (button6 != null)
            {
                button6.Text = totalAmount.ToString("N0");
            }

            // Cập nhật title với thông tin thời gian
            var timeRangeText = GetTimeRangeDisplayText();
            this.Text = $"Chi phí vận hành - {_expenses.Count} khoản - Tổng: {totalAmount:N0} VNĐ ({timeRangeText})";
        }

        /// <summary>
        /// Lấy text hiển thị khoảng thời gian
        /// </summary>
        private string GetTimeRangeDisplayText()
        {
            return _currentFilter switch
            {
                TimeFilterType.OneMonth => "1 tháng gần nhất",
                TimeFilterType.ThreeMonths => "3 tháng gần nhất", 
                TimeFilterType.OneYear => "1 năm gần nhất",
                TimeFilterType.Custom => $"{_fromDate:dd/MM/yyyy} - {_toDate:dd/MM/yyyy}",
                _ => "Tất cả thời gian"
            };
        }

        private void fpnListCost_Resize(object sender, EventArgs e)
        {
            var newWidth = fpnListCost.ClientSize.Width - 25;

            foreach (var control in fpnListCost.Controls)
            {
                if (control is ItemCostStoreRunning item)
                {
                    item.Width = newWidth;
                }
            }
        }
        #endregion

        #region Helper Methods

        private async Task OnBranchButtonClicked(long branchId)
        {
            await SafeRunAsync(async () => await LoadBranchExpenseAsync(branchId), "Đang tải chi phí...");
        }

        private async Task SafeRunAsync(Func<Task> action, string message)
        {
            try
            {
                if (_blurLoading != null)
                {
                    await _blurLoading.ShowLoadingAsync(message);
                }
                else
                {
                    this.Cursor = Cursors.WaitCursor;
                }

                await action();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (_blurLoading != null)
                {
                    await _blurLoading.HideLoadingAsync();
                }
                else
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        #endregion
    }

    /// <summary>
    /// Enum định nghĩa các loại bộ lọc thời gian
    /// </summary>
    public enum TimeFilterType
    {
        OneMonth,
        ThreeMonths,
        OneYear,
        Custom
    }
}
