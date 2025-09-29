using Dashboard.Winform.Presenters;
using Dashboard.Winform.Helpers;
using Dashboard.Winform.ViewModels.InventoryModels;
using Dashboard.Winform.Events;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dashboard.Winform.Forms.BaseFrm;

namespace Dashboard.Winform.Forms;

public partial class FrmInventoryManagement : FrmBaseAuthForm
{
    private readonly ILogger<FrmInventoryManagement> _logger;
    private readonly IInventoryManagementPresenter _presenter;
    private readonly InventoryManagementModel _model;
    private readonly IServiceProvider _serviceProvider;
    private TaskCompletionSource<bool>? _dataLoadingCompletionSource;
    private System.Windows.Forms.Timer? _searchTimer;
    private bool _isInitialized = false;

    public FrmInventoryManagement(
        ILogger<FrmInventoryManagement> logger,
        IInventoryManagementPresenter presenter,
        IServiceProvider serviceProvider)
    {
        _logger = logger;
        _presenter = presenter;
        _model = (InventoryManagementModel)_presenter.Model;
        _serviceProvider = serviceProvider;

        InitializeComponent();

        TabControlHelper.SetupDarkTheme(tabControl);

        OverrideTextUI();
        SetupDataBindings();
        SetupDataGridViews();
        SetupEvents();
        InitializeSearchTimer();

        _presenter.OnDataLoaded += (s, e) =>
        {
            try
            {
                if (e is InventoryDataLoadedEventArgs args)
                {
                    if (InvokeRequired)
                    {
                        Invoke(new Action(() =>
                        {
                            try
                            {
                                UpdateCurrentTabData(args);
                                UpdatePaginationInfo();
                            }
                            catch (Exception ex)
                            {
                                ShowError($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
                            }
                        }));
                    }
                    else
                    {
                        UpdateCurrentTabData(args);
                        UpdatePaginationInfo();
                    }
                }
            }
            catch (Exception ex)
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(() => ShowError($"Lỗi xử lý dữ liệu: {ex.Message}")));
                }
                else
                {
                    ShowError($"Lỗi xử lý dữ liệu: {ex.Message}");
                }
            }
        };

        Load += FrmInventoryManagement_Load;

        FormClosed += (s, e) =>
        {
            TabControlHelper.CleanupDarkTheme(tabControl);
            _searchTimer?.Dispose();
        };
    }

    public async Task WaitForDataLoadingComplete()
    {
        if (_dataLoadingCompletionSource != null)
        {
            await _dataLoadingCompletionSource.Task;
        }
    }

    private void InitializeSearchTimer()
    {
        _searchTimer = new System.Windows.Forms.Timer
        {
            Interval = 500
        };
        _searchTimer.Tick += async (s, e) =>
        {
            _searchTimer.Stop();
            if (_isInitialized)
            {
                await PerformSearch();
                GetCurrentSearchBox().Focus();
            }
        };
    }

    private void OverrideTextUI()
    {
        Text = "Quản lý nhập xuất hàng hóa";

        // Tab 1 - Inventory Transactions
        lblTransactionFilter1.Text = "Loại giao dịch:";
        lblTransactionFilter2.Text = "Chi nhánh:";
        lblTransactionSearch.Text = "Tìm kiếm theo (Mã/Nguyên liệu):";
        btnViewTransactionDetails.Text = "Chi tiết";
        btnCreateTransaction.Text = "Tạo mới";

        // Tab 2 - Inventory Requests
        lblRequestFilter1.Text = "Trạng thái:";
        lblRequestFilter2.Text = "Chi nhánh:";
        lblRequestSearch.Text = "Tìm kiếm theo (Mã/Nguyên liệu):";
        btnViewRequestDetails.Text = "Chi tiết";
        btnCreateRequest.Text = "Tạo yêu cầu";
        btnApproveRequest.Text = "Duyệt";

        // Tab 3 - Transfer Execution
        lblTransferFilter1.Text = "Loại chuyển:";
        lblTransferFilter2.Text = "Chi nhánh:";
        lblTransferSearch.Text = "Tìm kiếm nguyên liệu:";
        btnImportExcel.Text = "Import Excel";
        btnExecuteTransfer.Text = "Thực hiện";
        btnCreateTransfer.Text = "Tạo phiếu";
    }

    private void SetupDataBindings()
    {
        // Transaction filters
        cbxTransactionFilter1.DisplayMember = "Name";
        cbxTransactionFilter1.ValueMember = "Id";

        cbxTransactionFilter2.DisplayMember = "Name";
        cbxTransactionFilter2.ValueMember = "Id";

        // Request filters
        cbxRequestFilter1.DataSource = _model.RequestStatuses ?? new BindingList<string>();
        if (cbxRequestFilter1.Items.Count > 0)
            cbxRequestFilter1.SelectedIndex = 0;

        cbxRequestFilter2.DisplayMember = "Name";
        cbxRequestFilter2.ValueMember = "Id";

        // Transfer filters
        cbxTransferFilter1.DataSource = _model.TransferTypes ?? new BindingList<string>();
        if (cbxTransferFilter1.Items.Count > 0)
            cbxTransferFilter1.SelectedIndex = 0;

        cbxTransferFilter2.DisplayMember = "Name";
        cbxTransferFilter2.ValueMember = "Id";
    }

    private void SetupDataGridViews()
    {
        SetupTransactionDataGridView();
        SetupRequestDataGridView();
        SetupTransferDataGridViews();
    }

    private void SetupTransactionDataGridView()
    {
        dgvTransactions.AutoGenerateColumns = false;
        dgvTransactions.Columns.Clear();

        dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryTransactionViewModel.TransactionCode),
            HeaderText = "Mã giao dịch",
            Width = 120,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryTransactionViewModel.TransactionType),
            HeaderText = "Loại",
            Width = 80
        });

        dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryTransactionViewModel.BranchName),
            HeaderText = "Chi nhánh",
            Width = 150
        });

        dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryTransactionViewModel.IngredientName),
            HeaderText = "Nguyên liệu",
            Width = 200
        });

        dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryTransactionViewModel.Quantity),
            HeaderText = "Số lượng",
            Width = 100,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "N2"
            }
        });

        dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryTransactionViewModel.Unit),
            HeaderText = "Đơn vị",
            Width = 80
        });

        dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryTransactionViewModel.MovementDate),
            HeaderText = "Ngày thực hiện",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle
            {
                Format = "dd/MM/yyyy HH:mm"
            }
        });

        dgvTransactions.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryTransactionViewModel.Notes),
            HeaderText = "Ghi chú",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        dgvTransactions.DataSource = _model.Transactions;
        dgvTransactions.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTransactions.MultiSelect = false;

        dgvTransactions.ColumnHeaderMouseClick += DgvTransactions_ColumnHeaderMouseClick;
        dgvTransactions.CellDoubleClick += (s, e) => DgvTransactions_CellDoubleClick(s!, e);
        dgvTransactions.SelectionChanged += (s, e) => btnViewTransactionDetails.Enabled = dgvTransactions.SelectedRows.Count > 0;
    }

    private void SetupRequestDataGridView()
    {
        dgvRequests.AutoGenerateColumns = false;
        dgvRequests.Columns.Clear();

        dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryRequestViewModel.RequestNumber),
            HeaderText = "Mã yêu cầu",
            Width = 120
        });

        dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryRequestViewModel.BranchName),
            HeaderText = "Chi nhánh",
            Width = 150
        });

        dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryRequestViewModel.RequestDate),
            HeaderText = "Ngày yêu cầu",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
        });

        dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryRequestViewModel.RequiredDate),
            HeaderText = "Ngày cần",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "dd/MM/yyyy" }
        });

        dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryRequestViewModel.Status),
            HeaderText = "Trạng thái",
            Width = 100
        });

        dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryRequestViewModel.TotalItems),
            HeaderText = "Số mặt hàng",
            Width = 100
        });

        dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryRequestViewModel.RequestedBy),
            HeaderText = "Người yêu cầu",
            Width = 150
        });

        dgvRequests.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(InventoryRequestViewModel.Note),
            HeaderText = "Ghi chú",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        dgvRequests.DataSource = _model.Requests;
        dgvRequests.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvRequests.MultiSelect = false;

        dgvRequests.ColumnHeaderMouseClick += DgvRequests_ColumnHeaderMouseClick;
        dgvRequests.CellDoubleClick += (s, e) => DgvRequests_CellDoubleClick(s!, e);
        dgvRequests.SelectionChanged += (s, e) =>
        {
            btnViewRequestDetails.Enabled = dgvRequests.SelectedRows.Count > 0;
            btnApproveRequest.Enabled = dgvRequests.SelectedRows.Count > 0 &&
                                       GetSelectedRequest()?.Status == "PENDING";
        };
    }

    private void SetupTransferDataGridViews()
    {
        // Available Ingredients DataGridView
        dgvAvailableIngredients.AutoGenerateColumns = false;
        dgvAvailableIngredients.Columns.Clear();

        dgvAvailableIngredients.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(IngredientInventoryViewModel.IngredientName),
            HeaderText = "Nguyên liệu",
            Width = 200
        });

        dgvAvailableIngredients.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(IngredientInventoryViewModel.AvailableQuantity),
            HeaderText = "Số lượng có sẵn",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
        });

        dgvAvailableIngredients.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(IngredientInventoryViewModel.Unit),
            HeaderText = "Đơn vị",
            Width = 80
        });

        dgvAvailableIngredients.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(IngredientInventoryViewModel.CategoryName),
            HeaderText = "Danh mục",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        dgvAvailableIngredients.DataSource = _model.AvailableIngredients;

        // Transfer List DataGridView
        dgvTransferList.AutoGenerateColumns = false;
        dgvTransferList.Columns.Clear();

        dgvTransferList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(TransferItemViewModel.IngredientName),
            HeaderText = "Nguyên liệu",
            Width = 200
        });

        dgvTransferList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(TransferItemViewModel.TransferQuantity),
            HeaderText = "Số lượng chuyển",
            Width = 120,
            DefaultCellStyle = new DataGridViewCellStyle { Format = "N2" }
        });

        dgvTransferList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(TransferItemViewModel.Unit),
            HeaderText = "Đơn vị",
            Width = 80
        });

        dgvTransferList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(TransferItemViewModel.ToBranchName),
            HeaderText = "Đến chi nhánh",
            Width = 150
        });

        dgvTransferList.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(TransferItemViewModel.Notes),
            HeaderText = "Ghi chú",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        dgvTransferList.DataSource = _model.TransferItems;

        dgvAvailableIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvTransferList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
    }

    private void SetupEvents()
    {
        // Transaction tab events
        cbxTransactionFilter1.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await ApplyTransactionFilter();
        };

        cbxTransactionFilter2.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await ApplyTransactionFilter();
        };

        tbxTransactionSearch.TextChanged += (s, e) =>
        {
            if (_isInitialized && tabControl.SelectedTab == tabInventoryTransactions)
            {
                _searchTimer?.Stop();
                _searchTimer?.Start();
            }
        };

        // Request tab events
        cbxRequestFilter1.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await ApplyRequestFilter();
        };

        cbxRequestFilter2.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await ApplyRequestFilter();
        };

        tbxRequestSearch.TextChanged += (s, e) =>
        {
            if (_isInitialized && tabControl.SelectedTab == tabInventoryRequests)
            {
                _searchTimer?.Stop();
                _searchTimer?.Start();
            }
        };

        // Transfer tab events
        cbxTransferFilter1.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await ApplyTransferFilter();
        };

        cbxTransferFilter2.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await ApplyTransferFilter();
        };

        tbxTransferSearch.TextChanged += (s, e) =>
        {
            if (_isInitialized && tabControl.SelectedTab == tabTransferExecution)
            {
                _searchTimer?.Stop();
                _searchTimer?.Start();
            }
        };

        // Button events
        btnViewTransactionDetails.Click += (s, e) => ViewTransactionDetails();
        btnCreateTransaction.Click += (s, e) => CreateTransaction();

        btnViewRequestDetails.Click += (s, e) => ViewRequestDetails();
        btnCreateRequest.Click += (s, e) => CreateRequest();
        btnApproveRequest.Click += async (s, e) => await ApproveRequest();

        btnImportExcel.Click += async (s, e) => await ImportExcel();
        btnExecuteTransfer.Click += async (s, e) => await ExecuteTransfer();
        btnCreateTransfer.Click += (s, e) => CreateTransfer();

        btnAddToTransfer.Click += (s, e) => AddToTransfer();
        btnRemoveFromTransfer.Click += (s, e) => RemoveFromTransfer();
        btnClearTransfer.Click += (s, e) => ClearTransfer();

        btnSaveTransfer.Click += async (s, e) => await SaveTransfer();
        btnCancelTransfer.Click += (s, e) => CancelTransfer();

        // Pagination events
        SetupPaginationEvents();

        // Tab change events
        tabControl.SelectedIndexChanged += async (s, e) => await TabControl_SelectedIndexChanged();

        SetupContextMenus();
    }

    private void SetupPaginationEvents()
    {
        // Transaction pagination
        btnTransactionNext.Click += async (s, e) => await GoToNextPage(InventoryTabType.Transactions);
        btnTransactionPrevious.Click += async (s, e) => await GoToPreviousPage(InventoryTabType.Transactions);
        cbxTransactionPageSize.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await UpdatePageSize(InventoryTabType.Transactions);
        };

        // Request pagination
        btnRequestNext.Click += async (s, e) => await GoToNextPage(InventoryTabType.Requests);
        btnRequestPrevious.Click += async (s, e) => await GoToPreviousPage(InventoryTabType.Requests);
        cbxRequestPageSize.SelectedIndexChanged += async (s, e) =>
        {
            if (_isInitialized) await UpdatePageSize(InventoryTabType.Requests);
        };
    }

    #region Event Handlers

    private async void FrmInventoryManagement_Load(object? sender, EventArgs e)
    {
        _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
        try
        {
            SetLoadingState(true);
            Console.WriteLine("Starting inventory data load...");

            await _presenter.LoadDataAsync(InventoryTabType.Transactions);

            Console.WriteLine($"Inventory data loaded. Transactions: {_model.Transactions?.Count ?? 0}");
            UpdatePaginationInfo();

            _isInitialized = true;
            _dataLoadingCompletionSource.SetResult(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in FrmInventoryManagement_Load: {ex.Message}");
            _dataLoadingCompletionSource.SetException(ex);
            ShowError($"Lỗi khi tải dữ liệu nhập xuất: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async Task TabControl_SelectedIndexChanged()
    {
        if (!_isInitialized) return;

        try
        {
            SetLoadingState(true);

            var currentTab = GetCurrentTabType();
            await _presenter.LoadDataAsync(currentTab);

            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi chuyển tab: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void UpdateCurrentTabData(InventoryDataLoadedEventArgs args)
    {
        switch (args.TabType)
        {
            case InventoryTabType.Transactions:
                _model.Transactions.Clear();
                foreach (var item in args.Transactions ?? new List<InventoryTransactionViewModel>())
                {
                    _model.Transactions.Add(item);
                }
                dgvTransactions.Refresh();
                break;

            case InventoryTabType.Requests:
                _model.Requests.Clear();
                foreach (var item in args.Requests ?? new List<InventoryRequestViewModel>())
                {
                    _model.Requests.Add(item);
                }
                dgvRequests.Refresh();
                break;

            case InventoryTabType.TransferExecution:
                _model.AvailableIngredients.Clear();
                foreach (var item in args.AvailableIngredients ?? new List<IngredientInventoryViewModel>())
                {
                    _model.AvailableIngredients.Add(item);
                }
                dgvAvailableIngredients.Refresh();
                break;
        }

        // Update filter data sources if provided
        if (args.Branches?.Count > 0)
        {
            UpdateBranchFilters(args.Branches);
        }
    }

    private void UpdateBranchFilters(List<BranchSimpleViewModel> branches)
    {
        var branchesWithAll = new List<BranchSimpleViewModel>
        {
            new() { Id = 0, Name = "Tất cả" }
        };
        branchesWithAll.AddRange(branches);

        cbxTransactionFilter2.DataSource = new List<BranchSimpleViewModel>(branchesWithAll);
        cbxRequestFilter2.DataSource = new List<BranchSimpleViewModel>(branchesWithAll);
        cbxTransferFilter2.DataSource = new List<BranchSimpleViewModel>(branchesWithAll);
    }

    #endregion

    #region Search and Filter Methods

    private async Task PerformSearch()
    {
        try
        {
            SetLoadingState(true);
            var currentTab = GetCurrentTabType();
            var searchText = GetCurrentSearchBox().Text;

            await _presenter.SearchAsync(currentTab, searchText);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi tìm kiếm: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async Task ApplyTransactionFilter()
    {
        try
        {
            SetLoadingState(true);
            var transactionType = cbxTransactionFilter1.SelectedItem?.ToString();
            var branchId = cbxTransactionFilter2.SelectedValue as long? ?? 0;

            await _presenter.FilterTransactionsAsync(transactionType, branchId);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi lọc giao dịch: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async Task ApplyRequestFilter()
    {
        try
        {
            SetLoadingState(true);
            var status = cbxRequestFilter1.SelectedItem?.ToString();
            var branchId = cbxRequestFilter2.SelectedValue as long? ?? 0;

            await _presenter.FilterRequestsAsync(status, branchId);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi lọc yêu cầu: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async Task ApplyTransferFilter()
    {
        try
        {
            SetLoadingState(true);
            var transferType = cbxTransferFilter1.SelectedItem?.ToString();
            var branchId = cbxTransferFilter2.SelectedValue as long? ?? 0;

            await _presenter.FilterTransfersAsync(transferType, branchId);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi lọc chuyển kho: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    #endregion

    #region Button Event Handlers

    private void ViewTransactionDetails()
    {
        var selectedTransaction = GetSelectedTransaction();
        if (selectedTransaction != null)
        {
            // Open transaction details form
            ShowInfo($"Xem chi tiết giao dịch: {selectedTransaction.TransactionCode}");
        }
        else
        {
            ShowInfo("Vui lòng chọn một giao dịch để xem chi tiết.");
        }
    }

    private void CreateTransaction()
    {
        // Open create transaction form
        ShowInfo("Tạo giao dịch mới - Chức năng sẽ được triển khai.");
    }

    private void ViewRequestDetails()
    {
        var selectedRequest = GetSelectedRequest();
        if (selectedRequest != null)
        {
            ShowInfo($"Xem chi tiết yêu cầu: {selectedRequest.RequestNumber}");
        }
        else
        {
            ShowInfo("Vui lòng chọn một yêu cầu để xem chi tiết.");
        }
    }

    private void CreateRequest()
    {
        ShowInfo("Tạo yêu cầu mới - Chức năng sẽ được triển khai.");
    }

    private async Task ApproveRequest()
    {
        var selectedRequest = GetSelectedRequest();
        if (selectedRequest == null)
        {
            ShowInfo("Vui lòng chọn một yêu cầu để duyệt.");
            return;
        }

        if (selectedRequest.Status != "PENDING")
        {
            ShowInfo("Chỉ có thể duyệt các yêu cầu đang chờ xử lý.");
            return;
        }

        if (ShowConfirmation($"Bạn có chắc chắn muốn duyệt yêu cầu {selectedRequest.RequestNumber}?") == DialogResult.Yes)
        {
            try
            {
                SetLoadingState(true);
                await _presenter.ApproveRequestAsync(selectedRequest.Id);
                ShowInfo("Duyệt yêu cầu thành công!");
                await _presenter.LoadDataAsync(InventoryTabType.Requests);
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi duyệt yêu cầu: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
    }

    private async Task ImportExcel()
    {
        var openFileDialog = new OpenFileDialog
        {
            Filter = "Excel Files|*.xlsx;*.xls;*.csv;*.txt",
            Title = "Chọn file Excel để import"
        };

        if (openFileDialog.ShowDialog() == DialogResult.OK)
        {
            try
            {
                SetLoadingState(true);
                var importedItems = await _presenter.ImportFromExcelAsync(openFileDialog.FileName);

                foreach (var item in importedItems)
                {
                    _model.TransferItems.Add(item);
                }

                ShowInfo($"Import thành công {importedItems.Count} mặt hàng từ Excel.");
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi import Excel: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
    }

    private async Task ExecuteTransfer()
    {
        if (_model.TransferItems.Count == 0)
        {
            ShowInfo("Không có mặt hàng nào để chuyển kho.");
            return;
        }

        if (ShowConfirmation($"Bạn có chắc chắn muốn thực hiện chuyển kho {_model.TransferItems.Count} mặt hàng?") == DialogResult.Yes)
        {
            try
            {
                SetLoadingState(true);
                await _presenter.ExecuteTransferAsync(_model.TransferItems.ToList());
                ShowInfo("Thực hiện chuyển kho thành công!");

                _model.TransferItems.Clear();
                await _presenter.LoadDataAsync(InventoryTabType.TransferExecution);
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi thực hiện chuyển kho: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        }
    }

    private void CreateTransfer()
    {
        ShowInfo("Tạo phiếu chuyển kho mới - Chức năng sẽ được triển khai.");
    }

    private void AddToTransfer()
    {
        var selectedIngredient = GetSelectedIngredient();
        if (selectedIngredient == null)
        {
            ShowInfo("Vui lòng chọn nguyên liệu để thêm vào danh sách chuyển kho.");
            return;
        }

        // Open quantity input dialog
        var quantityForm = new QuantityInputForm(selectedIngredient);
        if (quantityForm.ShowDialog() == DialogResult.OK)
        {
            var transferItem = new TransferItemViewModel
            {
                IngredientId = selectedIngredient.IngredientId,
                IngredientName = selectedIngredient.IngredientName,
                TransferQuantity = quantityForm.Quantity,
                Unit = selectedIngredient.Unit,
                ToBranchId = quantityForm.ToBranchId,
                ToBranchName = quantityForm.ToBranchName,
                Notes = quantityForm.Notes
            };

            _model.TransferItems.Add(transferItem);
        }
    }

    private void RemoveFromTransfer()
    {
        var selectedTransferItem = GetSelectedTransferItem();
        if (selectedTransferItem == null)
        {
            ShowInfo("Vui lòng chọn mặt hàng để xóa khỏi danh sách chuyển kho.");
            return;
        }

        _model.TransferItems.Remove(selectedTransferItem);
    }

    private void ClearTransfer()
    {
        if (_model.TransferItems.Count > 0)
        {
            if (ShowConfirmation("Bạn có chắc chắn muốn xóa toàn bộ danh sách chuyển kho?") == DialogResult.Yes)
            {
                _model.TransferItems.Clear();
            }
        }
    }

    private async Task SaveTransfer()
    {
        if (_model.TransferItems.Count == 0)
        {
            ShowInfo("Không có mặt hàng nào để lưu.");
            return;
        }

        try
        {
            SetLoadingState(true);
            await _presenter.SaveTransferAsync(_model.TransferItems.ToList());
            ShowInfo("Lưu phiếu chuyển kho thành công!");

            _model.TransferItems.Clear();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi lưu phiếu chuyển kho: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void CancelTransfer()
    {
        if (_model.TransferItems.Count > 0)
        {
            if (ShowConfirmation("Bạn có chắc chắn muốn hủy bỏ thao tác chuyển kho?") == DialogResult.Yes)
            {
                _model.TransferItems.Clear();
            }
        }
    }

    #endregion

    #region Helper Methods

    private InventoryTabType GetCurrentTabType()
    {
        return tabControl.SelectedTab switch
        {
            var tab when tab == tabInventoryTransactions => InventoryTabType.Transactions,
            var tab when tab == tabInventoryRequests => InventoryTabType.Requests,
            var tab when tab == tabTransferExecution => InventoryTabType.TransferExecution,
            _ => InventoryTabType.Transactions
        };
    }

    private TextBox GetCurrentSearchBox()
    {
        return GetCurrentTabType() switch
        {
            InventoryTabType.Transactions => tbxTransactionSearch,
            InventoryTabType.Requests => tbxRequestSearch,
            InventoryTabType.TransferExecution => tbxTransferSearch,
            _ => tbxTransactionSearch
        };
    }

    private InventoryTransactionViewModel? GetSelectedTransaction()
    {
        if (dgvTransactions.SelectedRows.Count > 0)
        {
            return dgvTransactions.SelectedRows[0].DataBoundItem as InventoryTransactionViewModel;
        }
        return null;
    }

    private InventoryRequestViewModel? GetSelectedRequest()
    {
        if (dgvRequests.SelectedRows.Count > 0)
        {
            return dgvRequests.SelectedRows[0].DataBoundItem as InventoryRequestViewModel;
        }
        return null;
    }

    private IngredientInventoryViewModel? GetSelectedIngredient()
    {
        if (dgvAvailableIngredients.SelectedRows.Count > 0)
        {
            return dgvAvailableIngredients.SelectedRows[0].DataBoundItem as IngredientInventoryViewModel;
        }
        return null;
    }

    private TransferItemViewModel? GetSelectedTransferItem()
    {
        if (dgvTransferList.SelectedRows.Count > 0)
        {
            return dgvTransferList.SelectedRows[0].DataBoundItem as TransferItemViewModel;
        }
        return null;
    }

    private void UpdatePaginationInfo()
    {
        var currentTab = GetCurrentTabType();

        switch (currentTab)
        {
            case InventoryTabType.Transactions:
                lblTransactionRecords.Text = $"Số lượng: {_model.TransactionTotalItems}";
                lblTransactionPageInfo.Text = $"Hiện trang {_model.TransactionCurrentPage} trên {_model.TransactionTotalPages}";
                btnTransactionPrevious.Enabled = _model.TransactionCurrentPage > 1;
                btnTransactionNext.Enabled = _model.TransactionCurrentPage < _model.TransactionTotalPages;
                break;

            case InventoryTabType.Requests:
                lblRequestRecords.Text = $"Số lượng: {_model.RequestTotalItems}";
                lblRequestPageInfo.Text = $"Hiện trang {_model.RequestCurrentPage} trên {_model.RequestTotalPages}";
                btnRequestPrevious.Enabled = _model.RequestCurrentPage > 1;
                btnRequestNext.Enabled = _model.RequestCurrentPage < _model.RequestTotalPages;
                break;
        }
    }

    private void SetLoadingState(bool isLoading)
    {
        // Disable all controls during loading
        foreach (Control control in tabControl.Controls)
        {
            control.Enabled = !isLoading;
        }

        // Set cursor
        this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
    }

    #endregion

    #region Pagination Methods

    private async Task GoToNextPage(InventoryTabType tabType)
    {
        try
        {
            SetLoadingState(true);
            await _presenter.GoToNextPageAsync(tabType);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi chuyển trang: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async Task GoToPreviousPage(InventoryTabType tabType)
    {
        try
        {
            SetLoadingState(true);
            await _presenter.GoToPreviousPageAsync(tabType);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi chuyển trang: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async Task UpdatePageSize(InventoryTabType tabType)
    {
        try
        {
            var pageSize = tabType switch
            {
                InventoryTabType.Transactions => int.TryParse(cbxTransactionPageSize.SelectedItem?.ToString(), out int transactionPageSize) ? transactionPageSize : 10,
                InventoryTabType.Requests => int.TryParse(cbxRequestPageSize.SelectedItem?.ToString(), out int requestPageSize) ? requestPageSize : 10,
                _ => 10
            };

            SetLoadingState(true);
            await _presenter.ChangePageSizeAsync(tabType, pageSize);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi thay đổi số bản ghi hiển thị: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    #endregion

    #region Sorting Methods

    private async void DgvTransactions_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        try
        {
            string sortBy = e.ColumnIndex switch
            {
                0 => "TransactionCode",
                1 => "TransactionType",
                2 => "BranchName",
                3 => "IngredientName",
                4 => "Quantity",
                5 => "Unit",
                6 => "MovementDate",
                _ => "MovementDate"
            };

            SetLoadingState(true);
            await _presenter.SortByAsync(InventoryTabType.Transactions, sortBy);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi sắp xếp: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void DgvRequests_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        try
        {
            string sortBy = e.ColumnIndex switch
            {
                0 => "RequestNumber",
                1 => "BranchName",
                2 => "RequestDate",
                3 => "RequiredDate",
                4 => "Status",
                5 => "TotalItems",
                6 => "RequestedBy",
                _ => "RequestDate"
            };

            SetLoadingState(true);
            await _presenter.SortByAsync(InventoryTabType.Requests, sortBy);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi sắp xếp: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private void DgvTransactions_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            ViewTransactionDetails();
        }
    }

    private void DgvRequests_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            ViewRequestDetails();
        }
    }

    #endregion

    #region Context Menus

    private void SetupContextMenus()
    {
        // Transaction context menu
        var transactionContextMenu = new ContextMenuStrip();
        transactionContextMenu.Items.Add("Xem chi tiết", null, (s, e) => ViewTransactionDetails());
        dgvTransactions.ContextMenuStrip = transactionContextMenu;

        // Request context menu
        var requestContextMenu = new ContextMenuStrip();
        requestContextMenu.Items.Add("Xem chi tiết", null, (s, e) => ViewRequestDetails());
        requestContextMenu.Items.Add("Duyệt yêu cầu", null, async (s, e) => await ApproveRequest());
        dgvRequests.ContextMenuStrip = requestContextMenu;

        // Transfer context menus
        var ingredientContextMenu = new ContextMenuStrip();
        ingredientContextMenu.Items.Add("Thêm vào chuyển kho", null, (s, e) => AddToTransfer());
        dgvAvailableIngredients.ContextMenuStrip = ingredientContextMenu;

        var transferContextMenu = new ContextMenuStrip();
        transferContextMenu.Items.Add("Xóa khỏi danh sách", null, (s, e) => RemoveFromTransfer());
        dgvTransferList.ContextMenuStrip = transferContextMenu;
    }

    #endregion

    #region Message Helpers

    private void ShowError(string message)
    {
        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void ShowInfo(string message)
    {
        MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private DialogResult ShowConfirmation(string message)
    {
        return MessageBox.Show(message, "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }

    #endregion

    private void panel8_Paint(object sender, PaintEventArgs e)
    {

    }
}

// Helper form for quantity input
public partial class QuantityInputForm : Form
{
    public decimal Quantity { get; private set; }
    public long ToBranchId { get; private set; }
    public string ToBranchName { get; private set; } = string.Empty;
    public string Notes { get; private set; } = string.Empty;

    private readonly IngredientInventoryViewModel _ingredient;
    private NumericUpDown numQuantity;
    private ComboBox cbxToBranch;
    private TextBox txtNotes;
    private Button btnOK;
    private Button btnCancel;

    public QuantityInputForm(IngredientInventoryViewModel ingredient)
    {
        _ingredient = ingredient;
        InitializeComponent();
        SetupForm();

        // TODO FIX THIS TO REAL form 
        numQuantity = new();
        cbxToBranch = new();
        txtNotes = new TextBox();
        btnOK = new Button();
        btnCancel = new Button();
    }

    private void InitializeComponent()
    {
        this.Text = "Nhập số lượng chuyển kho";
        this.Size = new Size(400, 300);
        this.StartPosition = FormStartPosition.CenterParent;
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;

        var lblIngredient = new Label
        {
            Text = $"Nguyên liệu: {_ingredient.IngredientName}",
            Location = new Point(20, 20),
            Size = new Size(350, 20)
        };

        var lblAvailable = new Label
        {
            Text = $"Số lượng có sẵn: {_ingredient.AvailableQuantity:N2} {_ingredient.Unit}",
            Location = new Point(20, 45),
            Size = new Size(350, 20)
        };

        var lblQuantity = new Label
        {
            Text = "Số lượng chuyển:",
            Location = new Point(20, 80),
            Size = new Size(100, 20)
        };

        numQuantity = new NumericUpDown
        {
            Location = new Point(130, 78),
            Size = new Size(100, 23),
            DecimalPlaces = 2,
            Maximum = _ingredient.AvailableQuantity,
            Minimum = 0.01m,
            Value = 1
        };

        var lblToBranch = new Label
        {
            Text = "Chuyển đến:",
            Location = new Point(20, 115),
            Size = new Size(100, 20)
        };

        cbxToBranch = new ComboBox
        {
            Location = new Point(130, 113),
            Size = new Size(200, 23),
            DropDownStyle = ComboBoxStyle.DropDownList
        };

        var lblNotes = new Label
        {
            Text = "Ghi chú:",
            Location = new Point(20, 150),
            Size = new Size(100, 20)
        };

        txtNotes = new TextBox
        {
            Location = new Point(20, 175),
            Size = new Size(350, 60),
            Multiline = true
        };

        btnOK = new Button
        {
            Text = "OK",
            Location = new Point(215, 250),
            Size = new Size(75, 23),
            DialogResult = DialogResult.OK
        };

        btnCancel = new Button
        {
            Text = "Cancel",
            Location = new Point(295, 250),
            Size = new Size(75, 23),
            DialogResult = DialogResult.Cancel
        };

        this.Controls.AddRange(new Control[] {
            lblIngredient, lblAvailable, lblQuantity, numQuantity,
            lblToBranch, cbxToBranch, lblNotes, txtNotes, btnOK, btnCancel
        });
    }

    private void SetupForm()
    {
        cbxToBranch.Items.Add("Chi nhánh 1");
        cbxToBranch.Items.Add("Chi nhánh 2");
        if (cbxToBranch.Items.Count > 0) cbxToBranch.SelectedIndex = 0;

        btnOK.Click += (s, e) =>
        {
            Quantity = numQuantity.Value;
            ToBranchId = 1;
            ToBranchName = cbxToBranch.SelectedItem?.ToString() ?? "";
            Notes = txtNotes.Text;
        };
    }
}