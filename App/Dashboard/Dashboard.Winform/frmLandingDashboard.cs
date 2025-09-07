using Dashboard.BussinessLogic.Services;
using Dashboard.DataAccess.Models.Entities;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Dashboard.Winform.Interfaces;
using Microsoft.Extensions.Logging;

namespace Dashboard.Winform
{
    public partial class FrmLandingDashboard : Form, IBlurLoadingServiceAware
    {
        private readonly ILandingDashboardPresenter _presenter;
        private readonly LandingDashboardModel _model;
        private readonly ILogger<FrmLandingDashboard>? _logger;
        private Button currentlySelectedButton = null!;
        
        // Centralized blur loading service
        private IBlurLoadingService? _blurLoadingService;

        //public FrmLandingDashboard() : this(null!, null!) { }

        private async void MainDashboardForm_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        #region Setup dashboard components
        public FrmLandingDashboard(ILogger<FrmLandingDashboard> logger, ILandingDashboardPresenter presenter)
        {
            InitializeComponent();
            _logger = logger;
            _presenter = presenter;

            dtpStart.Enabled = false;
            dtpEnd.Enabled = false;
            btnOkeCustomDate.Enabled = false;

            dtpStart.Value = DateTime.Today.AddDays(-30);
            dtpEnd.Value = DateTime.Today;

            btnLast7Days.Select();
            SetDateMenuButtonUI(btnLast7Days);

            _model = _presenter.Model;

            InitializeConstraint();
            InitializeEvents();
            SetupCharts();
            SetupUnderstockGrid();
            SetupDataBinding();
        }

        #region IBlurLoadingServiceAware Implementation
        
        /// <summary>
        /// Sets the blur loading service for centralized loading management
        /// </summary>
        /// <param name="blurLoadingService">The blur loading service instance</param>
        public void SetBlurLoadingService(IBlurLoadingService blurLoadingService)
        {
            _blurLoadingService = blurLoadingService;
            _logger?.LogInformation("Blur loading service has been set for FrmLandingDashboard");
        }
        
        #endregion

        private void InitializeConstraint()
        {

            dtpStart.MaxDate = DateTime.Today;
            dtpEnd.MinDate = dtpStart.Value;
            dtpEnd.MaxDate = DateTime.Today;

            dtpStart.ValueChanged += DtpStart_ValueChanged;
            dtpEnd.ValueChanged += DtpEnd_ValueChanged;
        }
        private void InitializeEvents()
        {
            _presenter.OnDataLoaded += OnDataLoaded;
            btnToday.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today, DateTime.Today);
            btnLast7Days.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today.AddDays(-7), DateTime.Today);
            btnLast30Days.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today.AddDays(-30), DateTime.Today);
            btnOneYear.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today.AddYears(-1), DateTime.Today);
            btnOkeCustomDate.Click += async (s, e) => await LoadDataForPeriod(dtpStart.Value, dtpEnd.Value);
            btnToday.Click += (s, e) => SetDateMenuButtonUI(s!);
            btnLast7Days.Click += (s, e) => SetDateMenuButtonUI(s!);
            btnLast30Days.Click += (s, e) => SetDateMenuButtonUI(s!);
            btnOneYear.Click += (s, e) => SetDateMenuButtonUI(s!);
            btnCustomDate.Click += (s, e) => SetDateMenuButtonUI(s!);

        }
        private void SetupDataBinding()
        {
            lblTotalOfRevenue.DataBindings.Clear();
            lblTotalOfRevenue.DataBindings.Add("Text", _model, nameof(_model.TotalRevenueFormatted));

            lblTotalOfProfit.DataBindings.Clear();
            lblTotalOfProfit.DataBindings.Add("Text", _model, nameof(_model.NetProfitFormatted));

            lblNumberOfOrders.DataBindings.Clear();
            lblNumberOfOrders.DataBindings.Add("Text", _model, nameof(_model.TotalOrders));

            lblNumberOfCustomers.DataBindings.Clear();
            lblNumberOfCustomers.DataBindings.Add("Text", _model, nameof(_model.CustomerCount));

            lblNumberOfSuppliers.DataBindings.Clear();
            lblNumberOfSuppliers.DataBindings.Add("Text", _model, nameof(_model.SupplierCount));

            lblNumberOfProducts.DataBindings.Clear();
            lblNumberOfProducts.DataBindings.Add("Text", _model, nameof(_model.ProductCount));

            lblEndDate.DataBindings.Clear();
            lblEndDate.DataBindings.Add("Text", _model, nameof(_model.EndDateFormatted));

            lblPerOrdersIncre.DataBindings.Add("Text", _model, nameof(_model.OrdersGrowthFormatted));
            lblPerProfitIncre.DataBindings.Add("Text", _model, nameof(_model.ProfitGrowthFormatted));
            lblPerRevenueIncre.DataBindings.Add("Text", _model, nameof(_model.RevenueGrowthFormatted));

            chartGrossFinacial.DataSource = _model.GrossRevenueList;

            chartGrossFinacial.Series["Revenue"].XValueMember = "Date";
            chartGrossFinacial.Series["Revenue"].YValueMembers = "Revenue";
            chartGrossFinacial.Series["Revenue"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;

            chartGrossFinacial.Series["Expense"].XValueMember = "Date";
            chartGrossFinacial.Series["Expense"].YValueMembers = "Expense";
            chartGrossFinacial.Series["Expense"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;

            chartGrossFinacial.Series["Profit"].XValueMember = "Date";
            chartGrossFinacial.Series["Profit"].YValueMembers = "Profit";
            chartGrossFinacial.Series["Profit"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;

            chartGrossFinacial.DataBind();

            chartTopProduct.DataSource = _model.TopProducts;
            chartTopProduct.Series["TopProducts"].XValueMember = "ProductName";
            chartTopProduct.Series["TopProducts"].YValueMembers = "QuantitySold";
            chartTopProduct.DataBind();

            dgvUnderstock.DataSource = _model.UnderstockProducts;
        }
        private void SetupUnderstockGrid()
        {
            dgvUnderstock.AutoGenerateColumns = false;
            dgvUnderstock.Columns.Clear();

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductId",
                HeaderText = "Mã nguyên liệu",
                Name = "ProductId",
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên nguyên liệu",
                Name = "ProductName",
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CurrentStock",
                HeaderText = "Tồn kho",
                Name = "CurrentStock",
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SafetyStock",
                HeaderText = "Tồn tối thiểu",
                Name = "SafetyStock",
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StockStatus",
                HeaderText = "Trạng thái",
                Name = "StockStatus",
            });

            // Style the grid
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnderstock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnderstock.ReadOnly = true;
            dgvUnderstock.MultiSelect = false;
            dgvUnderstock.RowHeadersVisible = false;
            dgvUnderstock.AllowUserToAddRows = false;
            dgvUnderstock.AllowUserToDeleteRows = false;
            dgvUnderstock.AllowUserToResizeRows = false;
            dgvUnderstock.EnableHeadersVisualStyles = false;

            // Remove left row headers

            // Theme
            Color backColor = Color.FromArgb(42, 45, 86);
            dgvUnderstock.BackgroundColor = backColor;
            dgvUnderstock.DefaultCellStyle.BackColor = backColor;
            dgvUnderstock.DefaultCellStyle.ForeColor = Color.White;
            dgvUnderstock.AlternatingRowsDefaultCellStyle.BackColor = backColor;

            dgvUnderstock.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(42, 45, 86);
            dgvUnderstock.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(124, 141, 181);
            dgvUnderstock.ColumnHeadersDefaultCellStyle.SelectionBackColor = dgvUnderstock.ColumnHeadersDefaultCellStyle.BackColor;
            dgvUnderstock.ColumnHeadersDefaultCellStyle.SelectionForeColor = dgvUnderstock.ColumnHeadersDefaultCellStyle.ForeColor;

            dgvUnderstock.GridColor = Color.FromArgb(73, 75, 111);
            dgvUnderstock.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvUnderstock.BorderStyle = BorderStyle.None;
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnderstock.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgvUnderstock.RowPrePaint += DgvUnderstock_RowPrePaint;
            dgvUnderstock.CellFormatting += DgvUnderstock_CellFormatting;
        }

        private void DgvUnderstock_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is DataGridView grid && e.RowIndex >= 0)
            {
                if (grid.Rows[e.RowIndex].DataBoundItem is UnderstockProductViewModel item)
                {
                    if (item.IsCritical)
                    {
                        grid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                    }
                    else if (item.IsLowStock)
                    {
                        grid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkOrange;
                    }
                    else
                    {
                        grid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.LightGray;
                    }
                }
            }
        }
        private void DgvUnderstock_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
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

        private void SetupCharts()
        {
            chartGrossFinacial.Series.Clear();

            chartGrossFinacial.Series.Add("Revenue");
            chartGrossFinacial.Series["Revenue"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartGrossFinacial.Series["Revenue"].Color = Color.Tomato;
            chartGrossFinacial.Series["Revenue"].BorderWidth = 2;
            chartGrossFinacial.Series["Revenue"].LegendText = "Doanh thu";
            chartGrossFinacial.Series["Revenue"].MarkerSize = 10;
            chartGrossFinacial.Series["Revenue"].MarkerSize = 7;
            chartGrossFinacial.Series["Revenue"].MarkerStep = 1;
            chartGrossFinacial.Series["Revenue"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;

            chartGrossFinacial.Series.Add("Expense");
            chartGrossFinacial.Series["Expense"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartGrossFinacial.Series["Expense"].Color = Color.Violet;
            chartGrossFinacial.Series["Expense"].BorderWidth = 2;
            chartGrossFinacial.Series["Expense"].LegendText = "Chi phí";
            chartGrossFinacial.Series["Expense"].MarkerSize = 10;
            chartGrossFinacial.Series["Expense"].MarkerSize = 7;
            chartGrossFinacial.Series["Expense"].MarkerStep = 1;
            chartGrossFinacial.Series["Expense"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;


            chartGrossFinacial.Series.Add("Profit");
            chartGrossFinacial.Series["Profit"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Spline;
            chartGrossFinacial.Series["Profit"].Color = Color.Turquoise;
            chartGrossFinacial.Series["Profit"].BorderWidth = 2;
            chartGrossFinacial.Series["Profit"].LegendText = "Lợi nhuận";
            chartGrossFinacial.Series["Profit"].MarkerSize = 7;
            chartGrossFinacial.Series["Profit"].MarkerStep = 1;
            chartGrossFinacial.Series["Profit"].MarkerStyle = System.Windows.Forms.DataVisualization.Charting.MarkerStyle.Circle;


            if (chartGrossFinacial.ChartAreas.Count > 0)
            {
                var chartArea = chartGrossFinacial.ChartAreas[0];

                chartArea.BackColor = Color.FromArgb(42, 45, 86);

                chartArea.AxisY.LabelStyle.Format = "#,##0,K đ";
                chartArea.AxisY.LabelStyle.ForeColor = Color.FromArgb(192, 192, 255);
                chartArea.AxisY.LabelStyle.Font = new Font("Segoe UI", 11, FontStyle.Regular);

                chartArea.AxisX.LabelStyle.Format = "dd/MM";
                chartArea.AxisX.LabelStyle.ForeColor = Color.FromArgb(192, 192, 255);
                chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 11, FontStyle.Regular); 

                chartArea.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
                chartArea.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;
                chartArea.AxisX.IsMarginVisible = false;

                chartArea.AxisY.IsStartedFromZero = true;

                chartArea.AxisX.MajorGrid.Enabled = false;
                chartArea.AxisY.MajorGrid.Enabled = true;
                chartArea.AxisY.MajorGrid.LineColor = Color.FromArgb(73, 75, 111);

            }

            if (chartGrossFinacial.Legends.Count > 0)
            {
                chartGrossFinacial.Legends[0].Enabled = true;
                chartGrossFinacial.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
                chartGrossFinacial.Legends[0].BackColor = Color.FromArgb(42, 45, 86);
                chartGrossFinacial.Legends[0].ForeColor = Color.FromArgb(192, 192, 255);
                chartGrossFinacial.Legends[0].Font = new Font("Segoe UI", 11, FontStyle.Regular);
            }
            chartGrossFinacial.Series["Revenue"].Font = new Font("Segoe UI", 11, FontStyle.Regular);
            chartGrossFinacial.Series["Expense"].Font = new Font("Segoe UI", 11, FontStyle.Regular);
            chartGrossFinacial.Series["Profit"].Font = new Font("Segoe UI", 11, FontStyle.Regular);

            // Setup doughnut chart for top products
            chartTopProduct.Series.Clear();
            chartTopProduct.Series.Add("TopProducts");
            chartTopProduct.Series["TopProducts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
            chartTopProduct.BackColor = Color.FromArgb(42, 45, 86);
            chartTopProduct.ChartAreas[0].BackColor = Color.FromArgb(42, 45, 86);
            chartTopProduct.Legends[0].ForeColor = Color.FromArgb(192, 192, 255);
            chartTopProduct.Legends[0].BackColor = Color.FromArgb(42, 45, 86);
            chartTopProduct.Series[0].BorderWidth = 4;
            chartTopProduct.Series[0].Font = new Font("Segoe UI", 11, FontStyle.Regular);
            chartTopProduct.Series[0].BorderColor = Color.FromArgb(42, 45, 86);
            chartTopProduct.Series[0].BackSecondaryColor = Color.Violet;
            chartTopProduct.Series[0].BackGradientStyle = System.Windows.Forms.DataVisualization.Charting.GradientStyle.DiagonalLeft;
            chartTopProduct.Series[0].Palette = System.Windows.Forms.DataVisualization.Charting.ChartColorPalette.Pastel;
            chartTopProduct.Legends[0].Font = new Font("Segoe UI", 11, FontStyle.Regular);
        }
        #endregion
        private async Task LoadDataAsync()
        {
            if (_blurLoadingService != null)
            {
                await _blurLoadingService.ExecuteWithLoadingAsync(async () =>
                {
                    _logger?.LogInformation("Loading dashboard data...");
                    await _presenter.LoadDashboardDataAsync();
                    _logger?.LogInformation("Dashboard data loaded successfully");
                }, "Đang tải dữ liệu Dashboard...", true);
            }
            else
            {
                try
                {
                    _logger?.LogInformation("Loading dashboard data...");
                    await _presenter.LoadDashboardDataAsync();
                    _logger?.LogInformation("Dashboard data loaded successfully");
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error loading dashboard data");
                    MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private async Task LoadDataForPeriod(DateTime startDate, DateTime endDate)
        {
            if (_blurLoadingService != null)
            {
                await _blurLoadingService.ExecuteWithLoadingAsync(async () =>
                {
                    _logger?.LogInformation("Loading dashboard data for period {StartDate} to {EndDate}", startDate, endDate);
                    await _presenter.LoadDashboardDataAsync(startDate, endDate);
                    _logger?.LogInformation("Dashboard data for period loaded successfully");
                }, $"Đang tải dữ liệu từ {startDate:dd/MM/yyyy} đến {endDate:dd/MM/yyyy}...", true);
            }
            else
            {
                try
                {
                    _logger?.LogInformation("Loading dashboard data for period {StartDate} to {EndDate}", startDate, endDate);
                    await _presenter.LoadDashboardDataAsync(startDate, endDate);
                    _logger?.LogInformation("Dashboard data for period loaded successfully");
                }
                catch (Exception ex)
                {
                    _logger?.LogError(ex, "Error loading dashboard data for period");
                    MessageBox.Show($"Có lỗi xảy ra khi tải dữ liệu: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void SetDateMenuButtonUI(object button)
        {
            var btn = (Button)button;
            btn.BackColor = btnLast30Days.FlatAppearance.BorderColor;
            btn.ForeColor = Color.White;
            if (currentlySelectedButton != null && currentlySelectedButton != btn)
            {
                currentlySelectedButton.BackColor = BackColor;
                currentlySelectedButton.ForeColor = Color.FromArgb(124, 141, 181);
            }
            if (btn != btnCustomDate)
            {
                dtpStart.Enabled = false;
                dtpEnd.Enabled = false;
                btnOkeCustomDate.Enabled = false;
            }
            else
            {
                dtpStart.Enabled = true;
                dtpEnd.Enabled = true;
                btnOkeCustomDate.Enabled = true;
                lblStartDate.Cursor = Cursors.Default;
                lblEndDate.Cursor = Cursors.Default;
            }

            currentlySelectedButton = btn;
        }
        private void UpdateCharts()
        {
            if (chartGrossFinacial.Series.Count >= 3 && _model.GrossRevenueList != null)
            {
                chartGrossFinacial.Series["Revenue"].Points.Clear();
                chartGrossFinacial.Series["Expense"].Points.Clear();
                chartGrossFinacial.Series["Profit"].Points.Clear();

                foreach (var data in _model.GrossRevenueList)
                {
                    chartGrossFinacial.Series["Revenue"].Points.AddXY(data.Date, data.Revenue);
                    chartGrossFinacial.Series["Expense"].Points.AddXY(data.Date, data.Expense);
                    chartGrossFinacial.Series["Profit"].Points.AddXY(data.Date, data.Profit);
                }

                chartGrossFinacial.Invalidate();
            }

            if (chartTopProduct.Series.Count > 0 && _model.TopProducts != null)
            {
                chartTopProduct.Series["TopProducts"].Points.Clear();

                foreach (var product in _model.TopProducts.Take(5))
                {
                    var point = chartTopProduct.Series["TopProducts"].Points
                        .AddXY(product.ProductName, product.QuantitySold);
                    chartTopProduct.Series["TopProducts"].Points.Last().Label = product.QuantitySold.ToString();
                    chartTopProduct.Series["TopProducts"].Points.Last().LegendText = product.ProductName;
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

            UpdateCharts();
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
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                if (_presenter != null)
                {
                    _presenter.OnDataLoaded -= OnDataLoaded;
                }

                if (dgvUnderstock != null)
                {
                    dgvUnderstock.CellFormatting -= DgvUnderstock_CellFormatting;
                }
            }
            base.Dispose(disposing);
        }
        private void DtpStart_ValueChanged(object? sender, EventArgs e)
        {
            dtpEnd.MinDate = dtpStart.Value;

            if (dtpEnd.Value < dtpStart.Value)
            {
                dtpEnd.Value = dtpStart.Value;
            }
            lblStartDate.Text = dtpStart.Value.ToString("dd/MM/yyyy");
        }
        private void DtpEnd_ValueChanged(object? sender, EventArgs e)
        {

            if (dtpEnd.Value < dtpStart.Value)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEnd.Value = dtpStart.Value;
            }
            lblEndDate.Text = dtpEnd.Value.ToString("dd/MM/yyyy");
        }
        private void lblStartDate_Click(object sender, EventArgs e)
        {
            if (currentlySelectedButton == btnCustomDate)
            {
                dtpStart.Select();
                SendKeys.Send("%{DOWN}");
            }
        }
        private void lblEndDate_Click(object sender, EventArgs e)
        {
            if (currentlySelectedButton == btnCustomDate)
            {
                dtpEnd.Select();
                SendKeys.Send("%{DOWN}");
            }
        }

        private void lblNumberOfSuppliers_Click(object sender, EventArgs e)
        {

        }
    }
}