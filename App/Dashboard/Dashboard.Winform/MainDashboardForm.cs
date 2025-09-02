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
            lblTotalOfRevenue.DataBindings.Add("Text", _model, nameof(_model.TotalRevenueFormatted));

            lblNumberOfOrders.DataBindings.Clear();
            lblNumberOfOrders.DataBindings.Add("Text", _model, nameof(_model.TotalOrders));

            lblNumberOfCustomers.DataBindings.Clear();
            lblNumberOfCustomers.DataBindings.Add("Text", _model, "CustomerCount");

            lblNumberOfSuppliers.DataBindings.Clear();
            lblNumberOfSuppliers.DataBindings.Add("Text", _model, "SupplierCount");

            lblNumberOfProducts.DataBindings.Clear();
            lblNumberOfProducts.DataBindings.Add("Text", _model, "ProductCount");

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
                HeaderText = "Tên nguyên liệu",
                Name = "ProductName",
                Width = 180
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "Category",
                HeaderText = "Loại",
                Name = "Category",
                Width = 120
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "CurrentStock",
                HeaderText = "Tồn kho",
                Name = "CurrentStock",
                Width = 80
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "SafetyStock",
                HeaderText = "Tồn tối thiểu",
                Name = "SafetyStock",
                Width = 90
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "StockStatus",
                HeaderText = "Trạng thái",
                Name = "StockStatus",
                Width = 90
            });

            dgvUnderstock.Columns.Add(new DataGridViewTextBoxColumn
            {
                DataPropertyName = "UnitPriceFormatted",
                HeaderText = "Đơn giá",
                Name = "UnitPrice",
                Width = 100
            });

            // Style the grid
            dgvUnderstock.AllowUserToAddRows = false;
            dgvUnderstock.AllowUserToDeleteRows = false;
            dgvUnderstock.ReadOnly = true;
            dgvUnderstock.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvUnderstock.MultiSelect = false;
            dgvUnderstock.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            // Add event for row formatting
            dgvUnderstock.CellFormatting += DgvUnderstock_CellFormatting;
        }

        private void DgvUnderstock_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
        {
            if (sender is DataGridView grid && e.RowIndex >= 0)
            {
                var item = grid.Rows[e.RowIndex].DataBoundItem as UnderstockProductViewModel;
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
            chart1.Series.Clear();
            chart1.Series.Add("Revenue");
            chart1.Series["Revenue"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;

            chart2.Series.Clear();
            chart2.Series.Add("TopProducts");
            chart2.Series["TopProducts"].ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Doughnut;
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
    }
}