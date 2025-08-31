using Dashboard.BussinessLogic.Services;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;

namespace Dashboard.Winform
{
    public partial class MainDashboardForm : Form
    {
        private readonly IDashboardPresenter _presenter;
        private MainDashboardModel _model;

        public MainDashboardForm(IDashboardPresenter presenter)
        {
            InitializeComponent();
            _presenter = presenter;
            _model = _presenter.Model;

            InitializeEvents();
            SetupDataBinding();
            SetupCharts();
        }

        private void InitializeEvents()
        {
            _presenter.OnError += OnPresenterError;
            _presenter.OnDataLoaded += OnDataLoaded;

            // Wire up button events
            btnToday.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today, DateTime.Today);
            Last7Days.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today.AddDays(-7), DateTime.Today);
            btnLast30Days.Click += async (s, e) => await LoadDataForPeriod(DateTime.Today.AddDays(-30), DateTime.Today);
            btnThisMonth.Click += async (s, e) => await LoadDataForPeriod(new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1), DateTime.Today);
            btnCustomDate.Click += OnCustomDateClick;
            btnOkeCustomDate.Click += async (s, e) => await LoadDataForPeriod(dtpStart.Value, dtpEnd.Value);
        }

        private void SetupDataBinding()
        {
            lblTotalOfRevenue.DataBindings.Clear();
            lblTotalOfRevenue.DataBindings.Add("Text", _model, nameof(_model.MonthlyRevenueFormatted));

            lblNumberOfOrders.DataBindings.Clear();
            lblNumberOfOrders.DataBindings.Add("Text", _model, nameof(_model.TotalOrders));

            lblNumberOfCustomers.DataBindings.Clear();
            lblNumberOfCustomers.DataBindings.Add("Text", _model, "CustomerCount");

            lblNumberOfSuppliers.DataBindings.Clear();
            lblNumberOfSuppliers.DataBindings.Add("Text", _model, "SupplierCount");

            lblNumberOfProducts.DataBindings.Clear();
            lblNumberOfProducts.DataBindings.Add("Text", _model, "ProductCount");

            // Setup understock grid
            if (dgvUnderstock != null)
            {
                SetupUnderstockGrid();
            }
        }

        private void SetupUnderstockGrid()
        {
            dgvUnderstock.AutoGenerateColumns = false;
            dgvUnderstock.Columns.Clear();

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "ProductName",
                HeaderText = "Tên sản phẩm",
                Name = "ProductName",
                Width = 200
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CurrentStock",
                HeaderText = "Tồn kho",
                Name = "CurrentStock",
                Width = 100
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "MinimumStock",
                HeaderText = "Tồn kho tối thiểu",
                Name = "MinimumStock",
                Width = 120
            });
        }

        private async void MainDashboardForm_Load(object sender, EventArgs e)
        {
            await LoadDataAsync();
        }

        private void SetupCharts()
        {
            chart1.Series.Clear();
            chart1.Series.Add("Revenue");
            chart1.Series["Revenue"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chart2.Series.Clear();
            chart2.Series.Add("TopProducts");
            chart2.Series["TopProducts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
        }

        private async Task LoadDataAsync()
        {
            try
            {
                ShowLoading(true);
                await _presenter.LoadDashboardDataAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occur while fetch data from database: {ex.Message}", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private async Task LoadDataForPeriod(DateTime startDate, DateTime endDate)
        {
            try
            {
                ShowLoading(true);
                await _presenter.LoadDashboardDataAsync(startDate, endDate);
                UpdateCharts();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                ShowLoading(false);
            }
        }

        private void OnCustomDateClick(object? sender, EventArgs e)
        {
            dtpStart.Visible = true;
            dtpEnd.Visible = true;
            btnOkeCustomDate.Visible = true;
        }

        private void UpdateCharts()
        {
            // Update revenue chart
            if (chart1.Series.Count > 0)
            {
                chart1.Series["Revenue"].Points.Clear();
                // Add data points based on your model data
                // This is just an example - you'll need to implement based on your data structure
            }

            // Update top products chart
            if (chart2.Series.Count > 0 && _model.TopProducts != null)
            {
                chart2.Series["TopProducts"].Points.Clear();
                foreach (var product in _model.TopProducts.Take(5))
                {
                    chart2.Series["TopProducts"].Points.AddXY(product.ProductName, product.QuantitySold);
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

            // Refresh the form
            Refresh();

            // Update understock grid
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
            this.UseWaitCursor = show;

            foreach (Control control in this.Controls)
            {
                if (control != label1) // Keep the title always enabled
                {
                    control.Enabled = !show;
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();

                // Unsubscribe from events
                if (_presenter != null)
                {
                    _presenter.OnError -= OnPresenterError;
                    _presenter.OnDataLoaded -= OnDataLoaded;
                }
            }
            base.Dispose(disposing);
        }
    }
}