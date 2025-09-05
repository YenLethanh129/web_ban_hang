using Dashboard.BussinessLogic.Services;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform
{
    public partial class frmLandingDashboard : Form
    {
        private readonly ILandingDashboardPresenter _presenter;
        private readonly LandingDashboardModel _model;

        public frmLandingDashboard(ILandingDashboardPresenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            dtpStart.Enabled = false;
            dtpEnd.Enabled = false;
            btnOkeCustomDate.Enabled = false;
            dtpStart.Value = DateTime.Today.AddDays(-30);
            dtpEnd.Value = DateTime.Today;
            InitializeConstraint();
            btnCustomDate.Select();

            _model = _presenter.Model;


            InitializeEvents();
            SetupCharts();
            SetupDataBinding();
        }

        private void InitializeConstraint()
        {

            dtpStart.MaxDate = DateTime.Today;
            dtpEnd.MinDate = dtpStart.Value;
            dtpEnd.MaxDate = DateTime.Today;

            dtpStart.ValueChanged += dtpStart_ValueChanged;
            dtpEnd.ValueChanged += dtpEnd_ValueChanged;
        }
        private void InitializeEvents()
        {
            _presenter.OnDataLoaded += OnDataLoaded;

            btnToday.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today, DateTime.Today);
            btnLast7Days.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today.AddDays(-7), DateTime.Today);
            btnLast30Days.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today.AddDays(-30), DateTime.Today);
            btnThisMonth.Click += async (s, e) => await LoadDataForPeriod(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);
            btnCustomDate.Click += OnCustomDateClick;
            btnOkeCustomDate.Click += async (s, e) => await LoadDataForPeriod(dtpStart.Value, dtpEnd.Value);
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

            chartGrossFinacial.DataSource = _model.GrossRevenueList;
            
            // Configure Revenue series
            chartGrossFinacial.Series["Revenue"].XValueMember = "Date";
            chartGrossFinacial.Series["Revenue"].YValueMembers = "Revenue";
            chartGrossFinacial.Series["Revenue"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            
            // Configure Expense series
            chartGrossFinacial.Series["Expense"].XValueMember = "Date";
            chartGrossFinacial.Series["Expense"].YValueMembers = "Expense";
            chartGrossFinacial.Series["Expense"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            
            // Configure Profit series
            chartGrossFinacial.Series["Profit"].XValueMember = "Date";
            chartGrossFinacial.Series["Profit"].YValueMembers = "Profit";
            chartGrossFinacial.Series["Profit"].XValueType = System.Windows.Forms.DataVisualization.Charting.ChartValueType.DateTime;
            
            chartGrossFinacial.DataBind();

            chartTopProduct.DataSource = _model.TopProducts;
            chartTopProduct.Series["TopProducts"].XValueMember = "ProductName";
            chartTopProduct.Series["TopProducts"].YValueMembers = "QuantitySold";
            chartTopProduct.DataBind();


            if (dgvUnderstock != null)
            {
                SetupUnderstockGrid();
            }
        }

        private void SetupUnderstockGrid()
        {
            dgvUnderstock.AutoGenerateColumns = false;
            dgvUnderstock.Columns.Clear();
            dgvUnderstock.DataSource = _model.UnderstockProducts;
            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductId",
                HeaderText = "Mã nguyên liệu",
                Name = "ProductCode",
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

            dgvUnderstock.Refresh();

            // Style the grid
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnderstock.AllowUserToAddRows = false;
            dgvUnderstock.AllowUserToDeleteRows = false;
            dgvUnderstock.ReadOnly = true;
            dgvUnderstock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnderstock.MultiSelect = false;
            dgvUnderstock.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            dgvUnderstock.CellFormatting += DgvUnderstock_CellFormatting;
        }

        private void DgvUnderstock_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is DataGridView grid && e.RowIndex >= 0)
            {
                UnderstockProductViewModel? item = grid.Rows[e.RowIndex].DataBoundItem as UnderstockProductViewModel;
                if (item != null)
                {
                    if (item.IsCritical)
                    {
                        grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightCoral;
                        grid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkRed;
                    }
                    else if (item.IsLowStock)
                    {
                        grid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightYellow;
                        grid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.DarkOrange;
                    }
                }
            }
        }

        private async void MainDashboardForm_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void SetupCharts()
        {
            // Setup Financial Chart with 3 series
            chartGrossFinacial.Series.Clear();
            
            // Revenue Series (Blue)
            chartGrossFinacial.Series.Add("Revenue");
            chartGrossFinacial.Series["Revenue"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartGrossFinacial.Series["Revenue"].Color = Color.Blue;
            chartGrossFinacial.Series["Revenue"].BorderWidth = 2;
            chartGrossFinacial.Series["Revenue"].LegendText = "Doanh thu";
            
            // Expense Series (Red)
            chartGrossFinacial.Series.Add("Expense");
            chartGrossFinacial.Series["Expense"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartGrossFinacial.Series["Expense"].Color = Color.Red;
            chartGrossFinacial.Series["Expense"].BorderWidth = 2;
            chartGrossFinacial.Series["Expense"].LegendText = "Chi phí";
            
            // Profit Series (Green)
            chartGrossFinacial.Series.Add("Profit");
            chartGrossFinacial.Series["Profit"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
            chartGrossFinacial.Series["Profit"].Color = Color.Green;
            chartGrossFinacial.Series["Profit"].BorderWidth = 2;
            chartGrossFinacial.Series["Profit"].LegendText = "Lợi nhuận";

            // Configure Chart Areas
            if (chartGrossFinacial.ChartAreas.Count > 0)
            {
                var chartArea = chartGrossFinacial.ChartAreas[0];
                chartArea.AxisX.Title = "Ngày";
                chartArea.AxisY.Title = "Số tiền (VNĐ)";
                chartArea.AxisY.LabelStyle.Format = "#,##0 đ";

                // Configure X-axis for dates
                chartArea.AxisX.LabelStyle.Format = "dd/MM";
                chartArea.AxisX.IntervalType = System.Windows.Forms.DataVisualization.Charting.DateTimeIntervalType.Days;
                chartArea.AxisX.IntervalAutoMode = System.Windows.Forms.DataVisualization.Charting.IntervalAutoMode.VariableCount;

                // Configure Y-axis
                chartArea.AxisY.IsStartedFromZero = true;

                // Grid configuration
                chartArea.AxisX.MajorGrid.Enabled = true;
                chartArea.AxisY.MajorGrid.Enabled = true;
                chartArea.AxisX.MajorGrid.LineColor = Color.LightGray;
                chartArea.AxisY.MajorGrid.LineColor = Color.LightGray;
            }
            
            // Enable legend
            if (chartGrossFinacial.Legends.Count > 0)
            {
                chartGrossFinacial.Legends[0].Enabled = true;
                chartGrossFinacial.Legends[0].Docking = System.Windows.Forms.DataVisualization.Charting.Docking.Top;
            }

            // Setup Top Products Chart
            chartTopProduct.Series.Clear();
            chartTopProduct.Series.Add("TopProducts");
            chartTopProduct.Series["TopProducts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
        }

        private async Task LoadDataAsync()
        {
            ShowLoading(true);
            await _presenter.LoadDashboardDataAsync();
        }

        private async Task LoadDataForPeriod(DateTime startDate, DateTime endDate)
        {
            ShowLoading(true);
            await _presenter.LoadDashboardDataAsync(startDate, endDate);
            UpdateCharts();
            ShowLoading(false);
        }

        private void OnCustomDateClick(object? sender, EventArgs e)
        {
            dtpStart.Enabled = true;
            dtpEnd.Enabled = true;
            btnOkeCustomDate.Enabled = true;
        }

        private void UpdateCharts()
        {
            // Debug: Log data count
            System.Diagnostics.Debug.WriteLine($"GrossRevenueList count: {_model.GrossRevenueList?.Count ?? 0}");

            // Update financial chart with 3 series
            if (chartGrossFinacial.Series.Count >= 3 && _model.GrossRevenueList != null)
            {
                // Clear all series
                chartGrossFinacial.Series["Revenue"].Points.Clear();
                chartGrossFinacial.Series["Expense"].Points.Clear();
                chartGrossFinacial.Series["Profit"].Points.Clear();

                foreach (var data in _model.GrossRevenueList)
                {
                    System.Diagnostics.Debug.WriteLine($"Adding point: Date={data.Date:yyyy-MM-dd}, Revenue={data.Revenue}, Expense={data.Expense}, Profit={data.Profit}");
                    
                    // Add points to each series
                    chartGrossFinacial.Series["Revenue"].Points.AddXY(data.Date, data.Revenue);
                    chartGrossFinacial.Series["Expense"].Points.AddXY(data.Date, data.Expense);
                    chartGrossFinacial.Series["Profit"].Points.AddXY(data.Date, data.Profit);
                }

                // Refresh chart
                chartGrossFinacial.Invalidate();

                System.Diagnostics.Debug.WriteLine($"Chart series counts - Revenue: {chartGrossFinacial.Series["Revenue"].Points.Count}, Expense: {chartGrossFinacial.Series["Expense"].Points.Count}, Profit: {chartGrossFinacial.Series["Profit"].Points.Count}");
            }

            if (chartTopProduct.Series.Count > 0 && _model.TopProducts != null)
            {
                chartTopProduct.Series["TopProducts"].Points.Clear();
                foreach (var product in _model.TopProducts.Take(5))
                {
                    chartTopProduct.Series["TopProducts"].Points.AddXY(product.ProductName, product.QuantitySold);
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

            Refresh();

            if (dgvUnderstock != null && _model.UnderstockProducts != null)
            {
                dgvUnderstock.DataSource = null;
                dgvUnderstock.DataSource = _model.UnderstockProducts;
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

        private void ShowLoading(bool show)
        {
            // TODO: Add a loading indicator 
            //UseWaitCursor = show;

            //foreach (Control control in Controls)
            //{
            //    if (control != label1)
            //    {
            //        control.Enabled = !show;
            //    }
            //}
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

        private void dtpStart_ValueChanged(object? sender, EventArgs e)
        {
            dtpEnd.MinDate = dtpStart.Value;

            if (dtpEnd.Value < dtpStart.Value)
            {
                dtpEnd.Value = dtpStart.Value;
            }
        }
        private void dtpEnd_ValueChanged(object? sender, EventArgs e)
        {
            if (dtpEnd.Value < dtpStart.Value)
            {
                MessageBox.Show("Ngày kết thúc không được nhỏ hơn ngày bắt đầu!",
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEnd.Value = dtpStart.Value;
            }
        }
    }
}