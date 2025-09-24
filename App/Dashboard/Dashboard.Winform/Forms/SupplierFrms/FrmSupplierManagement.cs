using Dashboard.Winform.Events;
using Dashboard.Winform.Forms;
using Dashboard.Winform.Presenters.SupplierPresenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms.SupplierFrm;

public partial class FrmSupplierManagement : FrmBaseManagement<SupplierManagementModel, SupplierManagementPresenter>
{
    #region Fields
    private readonly SupplierManagementModel _model;
    private readonly IServiceProvider _serviceProvider;
    #endregion

    #region Constructor
    public FrmSupplierManagement(
        ILogger<FrmSupplierManagement> logger,
        IServiceProvider serviceProvider,
        SupplierManagementPresenter supplierPresenter
    ) : base(logger, supplierPresenter)
    {
        _model = _presenter.Model;
        _serviceProvider = serviceProvider;

        InitializeBaseComponents();

        _presenter.OnDataLoaded += (s, e) =>
        {
            try
            {
                if (e is SuppliersLoadedEventArgs args)
                {
                    SafeInvokeOnUI(() =>
                    {
                        try
                        {
                            ApplySuppliersToModel(
                                args.Suppliers.Cast<Dashboard.Winform.ViewModels.SupplierViewModel>().ToList(),
                                args.TotalCount
                            );
                        }
                        catch (Exception ex)
                        {
                            ShowError($"Lỗi khi cập nhật dữ liệu: {ex.Message}");
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                SafeInvokeOnUI(() => ShowError($"Lỗi xử lý dữ liệu: {ex.Message}"));
            }
        };

        Load += FrmSupplierManagement_Load;
        OverrideTextUI();
        OverrideComboBoxItem();
        SetupDataBindings();
        SetupDgvListItem();
        FinalizeFormSetup();
        SetupContextMenu();
    }
    #endregion

    #region Override Base Components
    protected override void InitializeDerivedComponents()
    {
        InitializeDgvListItem();
    }

    private void OverrideTextUI()
    {
        lblFilter1.Text = "Trạng thái:";
        lblFilter2.Text = "Loại nhà cung cấp:";
        lblSearchString.Text = "Tìm kiếm theo (ID/tên nhà cung cấp/email/SĐT):";
        Text = "Quản lý nhà cung cấp";
    }

    private void SetupDataBindings()
    {
        cbxFilter1.DataSource = _model.Statuses;
        cbxFilter1.SelectedItem = "All";

        cbxFilter2.DataSource = _model.SupplierTypes;
        cbxFilter2.DisplayMember = "Name";
        cbxFilter2.ValueMember = "Id";
        cbxFilter2.SelectedValue = 0;

        tbxFindString.DataBindings.Add(
            "Text", _model,
            nameof(_model.SearchText),
            false, DataSourceUpdateMode.OnPropertyChanged
        );
    }

    private void OverrideComboBoxItem()
    {
        cbxOrderBy.Items.Clear();
        cbxOrderBy.Items.AddRange(new[] { "ID", "Name", "Email", "Phone", "ContactPerson", "CreatedAt" });
        if (cbxOrderBy.Items.Count > 0)
            cbxOrderBy.SelectedIndex = 0;
    }

    protected void SetupDgvListItem()
    {
        if (dgvSuppliers == null)
        {
            throw new InvalidOperationException("dgvSuppliers must be initialized before calling SetupDgvListItem()");
        }

        dgvSuppliers.AutoGenerateColumns = false;
        dgvSuppliers.Columns.Clear();

        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SupplierViewModel.Id),
            HeaderText = "ID",
            Width = 60,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.None
        });

        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SupplierViewModel.Name),
            HeaderText = "Tên nhà cung cấp",
            Width = 180
        });

        //dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
        //{
        //    DataPropertyName = nameof(SupplierViewModel.ContactPerson),
        //    HeaderText = "Người liên hệ",
        //    Width = 150
        //});

        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SupplierViewModel.Email),
            HeaderText = "Email",
            Width = 180
        });

        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SupplierViewModel.Phone),
            HeaderText = "Số điện thoại",
            Width = 120
        });

        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SupplierViewModel.Address),
            HeaderText = "Địa chỉ",
            Width = 200
        });

        dgvSuppliers.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(SupplierViewModel.Note),
            HeaderText = "Ghi Chú",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        dgvSuppliers.DataSource = _model.Suppliers;
        dgvSuppliers.Refresh();

        // Attach column header click for sorting
        if (dgvSuppliers != null)
        {
            dgvSuppliers.ColumnHeaderMouseClick -= DgvSuppliers_ColumnHeaderMouseClick;
            dgvSuppliers.ColumnHeaderMouseClick += DgvSuppliers_ColumnHeaderMouseClick;
        }
    }

    private void DgvSuppliers_ColumnHeaderMouseClick(object? sender, DataGridViewCellMouseEventArgs e)
    {
        try
        {
            if (dgvSuppliers == null) return;
            var column = dgvSuppliers.Columns[e.ColumnIndex];
            var sortBy = column.DataPropertyName ?? column.Name;
            _ = Task.Run(async () =>
            {
                await _presenter.SortBy(sortBy);
                SafeInvokeOnUI(UpdatePaginationInfo);
            });
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi sắp xếp: {ex.Message}");
        }
    }
    #endregion

    #region Override Event Handlers Base Class

    protected override void BtnSearch_Click(object sender, EventArgs e)
    {
        PerformSearch();
    }

    protected override void Btnfilter1_Click(object sender, EventArgs e)
    {
        base.Btnfilter1_Click(sender, e);
    }

    protected override void BtnFilter2_Click(object sender, EventArgs e)
    {
        base.BtnFilter2_Click(sender, e);
    }

    protected override void BtnOrderBy_Click(object sender, EventArgs e)
    {
        base.BtnOrderBy_Click(sender, e);
    }

    protected override void BtnNumbOfRecordShowing_Click(object sender, EventArgs e)
    {
        base.BtnNumbOfRecordShowing_Click(sender, e);
    }

    protected override void BtnNext_Click(object sender, EventArgs e)
    {
        GoToNextPage();
    }

    protected override void BtnToday_Click(object sender, EventArgs e)
    {
        GoToPreviousPage();
    }

    protected override void CbxNumbRecordsPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdatePageSize();
    }

    protected override void InitializeEvents()
    {
        base.InitializeEvents();
        cbxFilter1.SelectedIndexChanged += (s, e) => ApplyStatusFilter();
        cbxFilter2.SelectedIndexChanged += (s, e) => ApplySupplierTypeFilter();
        cbxOrderBy.SelectedIndexChanged += (s, e) => ApplySorting();

        // Ensure search textbox uses debounced bound-search similar to other management forms
        tbxFindString.TextChanged += async (s, e) =>
        {
            // If control not ready or model not initialized, ignore
            if (_model == null) return;

            // If cleared, reset immediately and keep focus
            if (string.IsNullOrWhiteSpace(tbxFindString.Text))
            {
                try
                {
                    SetLoadingState(true);
                    _model.SearchText = string.Empty;
                    await _presenter.SearchAsync(_model.SearchText);
                    UpdatePaginationInfo();
                }
                catch (Exception ex)
                {
                    ShowError($"Lỗi khi reset tìm kiếm: {ex.Message}");
                }
                finally
                {
                    SetLoadingState(false);
                }

                try { tbxFindString.Focus(); } catch { }
                return;
            }

            // Otherwise start a small delay then perform search (debounce handled by presenter if needed)
            await Task.Delay(300);
            try
            {
                if (!string.IsNullOrWhiteSpace(tbxFindString.Text))
                {
                    SetLoadingState(true);
                    await _presenter.SearchAsync(tbxFindString.Text);
                    UpdatePaginationInfo();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Lỗi khi tìm kiếm: {ex.Message}");
            }
            finally
            {
                SetLoadingState(false);
            }
        };
    }

    protected override async Task TbxFindString_TextChanged(object? sender, EventArgs e)
    {
        var searchText = tbxFindString?.Text?.Trim() ?? string.Empty;

        await _presenter.SearchAsync(searchText);

        RefreshData();
    }

    #endregion

    #region Supplier Management Specific Methods

    private async void PerformSearch()
    {
        try
        {
            SetLoadingState(true);
            await _presenter.SearchAsync(_model.SearchText);
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

    private async void ApplyStatusFilter()
    {
        try
        {
            if (cbxFilter1.SelectedItem == null) return;

            SetLoadingState(true);
            var selectedStatus = cbxFilter1.SelectedItem.ToString();
            await _presenter.FilterByStatusAsync(selectedStatus!);
            UpdatePaginationInfo();
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi lọc theo trạng thái: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void ApplySupplierTypeFilter()
    {
        try
        {
            if (cbxFilter2.SelectedValue == null) return;

            SetLoadingState(true);
            if (cbxFilter2.SelectedValue is long supplierTypeId)
            {
                await _presenter.FilterBySupplierTypeAsync(supplierTypeId);
                UpdatePaginationInfo();
            }
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi lọc theo loại nhà cung cấp: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async void ApplySorting()
    {
        try
        {
            if (cbxOrderBy.SelectedItem == null) return;

            SetLoadingState(true);
            var sortBy = cbxOrderBy.SelectedItem.ToString();
            await _presenter.SortBy(sortBy);
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

    private async void GoToNextPage()
    {
        try
        {
            SetLoadingState(true);
            await _presenter.GoToNextPageAsync();
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

    private async void GoToPreviousPage()
    {
        try
        {
            SetLoadingState(true);
            await _presenter.GoToPreviousPageAsync();
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

    private async void UpdatePageSize()
    {
        try
        {
            if (cbxNumbRecordsPerPage.SelectedItem == null) return;

            SetLoadingState(true);
            if (int.TryParse(cbxNumbRecordsPerPage.SelectedItem.ToString(), out int pageSize))
            {
                await _presenter.ChangePageSizeAsync(pageSize);
                UpdatePaginationInfo();
            }
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

    private async void RefreshData()
    {
        try
        {
            SetLoadingState(true);
            await Task.Delay(200);
            await _presenter.RefreshCacheAsync();
            UpdatePaginationInfo();
            ShowInfo("Dữ liệu đã được cập nhật!");
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi làm mới dữ liệu: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    #endregion

    #region Helper Methods

    private void UpdatePaginationInfo()
    {
        if (_model != null)
        {
            lblNumberOfRecords.Text = $"Số lượng: {_model.TotalItems}";
            lblShowingAtPage.Text = $"Hiện trang {_model.CurrentPage} trên {_model.TotalPages}";

            btnToday.Enabled = _model.CurrentPage > 1;
            btnNext.Enabled = _model.CurrentPage < _model.TotalPages;
        }
    }

    private void ShowError(string message)
    {
        MessageBox.Show(message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    private void ShowInfo(string message)
    {
        MessageBox.Show(message, "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void SetLoadingState(bool isLoading)
    {
        btnGetDetails.Enabled = !isLoading;
        btnAdd.Enabled = !isLoading;
        cbxFilter1.Enabled = !isLoading;
        cbxFilter2.Enabled = !isLoading;
        cbxOrderBy.Enabled = !isLoading;
        cbxNumbRecordsPerPage.Enabled = !isLoading;

        this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
    }

    private SupplierViewModel? GetSelectedSupplier()
    {
        if (dgvSuppliers.SelectedRows.Count > 0)
        {
            var selectedRow = dgvSuppliers.SelectedRows[0];
            return selectedRow.DataBoundItem as SupplierViewModel;
        }
        return null;
    }

    private void ApplySuppliersToModel(List<SupplierViewModel> suppliers, int totalCount)
    {
        _model.Suppliers.Clear();
        foreach (var supplier in suppliers)
        {
            _model.Suppliers.Add(supplier);
        }

        _model.TotalItems = totalCount;
        UpdatePaginationInfo();
    }

    #endregion

    #region Event Handlers

    private async void FrmSupplierManagement_Load(object? sender, EventArgs e)
    {
        _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
        try
        {
            SetLoadingState(true);

            await _presenter.LoadDataAsync(page: _model.CurrentPage, pageSize: _model.PageSize);

            cbxFilter1.SelectedItem = "All";

            if (cbxFilter2.Items.Count > 0)
                cbxFilter2.SelectedIndex = 0;

            UpdatePaginationInfo();

            _dataLoadingCompletionSource.SetResult(true);
        }
        catch (Exception ex)
        {
            _dataLoadingCompletionSource.SetException(ex);
            ShowError($"Lỗi khi tải dữ liệu nhà cung cấp: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    #endregion

    #region Dialog Integration Methods

    private async void OpenSupplierDetailsDialog(SupplierViewModel? selectedSupplier = null)
    {
        try
        {
            SetLoadingState(true);

            SupplierDetailViewModel? initialModel = null;

            if (selectedSupplier != null)
            {
                initialModel = new SupplierDetailViewModel
                {
                    Id = selectedSupplier.Id,
                    Name = selectedSupplier.Name,
                    Email = selectedSupplier.Email,
                    Phone = selectedSupplier.Phone,
                    Address = selectedSupplier.Address,
                    IsActive = selectedSupplier.IsActive,
                    Note = selectedSupplier.Note,
                    CreatedAt = selectedSupplier.CreatedAt,
                    UpdatedAt = selectedSupplier.UpdatedAt,
                    SupplierProducts = new BindingList<SupplierProductViewModel>(),
                    PaymentTerms = new BindingList<PaymentTermViewModel>()
                };
            }

            using var detailForm = _serviceProvider.GetRequiredService<FrmSupplierDetails>();

            detailForm.SetInitData(selectedSupplier?.Id, initialModel);

            var result = detailForm.ShowDialog(this);

            if (result == DialogResult.OK)
            {
                var updatedSupplier = detailForm.Supplier;

                if (selectedSupplier != null)
                {
                    await HandleSupplierUpdate(updatedSupplier);
                }
                else
                {
                    //await HandleSupplierAdd(updatedSupplier);
                    await HandleSupplierAdd(updatedSupplier);
                    ShowInfo("Thêm nhà cung cấp mới thành công!");
                }

                RefreshData();
            }
        }
        catch (Exception ex)
        {
            ShowError($"Lỗi khi {(selectedSupplier != null ? "cập nhật" : "thêm")} nhà cung cấp: {ex.Message}");
        }
        finally
        {
            SetLoadingState(false);
        }
    }

    private async Task HandleSupplierAdd(SupplierDetailViewModel supplier)
    {
        // TODO: Implement supplier add logic through presenter
        await Task.Delay(50);
        Console.WriteLine($"Thêm nhà cung cấp: {supplier.Name}, Email: {supplier.Email}");
    }

    private async Task HandleSupplierUpdate(SupplierDetailViewModel supplier)
    {
        // TODO: Implement supplier update logic through presenter
        await Task.Delay(50);
        Console.WriteLine($"Lưu nhà cung cấp ID {supplier.Id}: {supplier.Name}");
    }

    #endregion

    #region Override Event Handlers - Updated

    protected override void BtnAdd_Click(object sender, EventArgs e)
    {
        OpenSupplierDetailsDialog();
    }

    protected override void BtnGetDetails_Click(object sender, EventArgs e)
    {
        var selectedSupplier = GetSelectedSupplier();

        if (selectedSupplier != null)
        {
            OpenSupplierDetailsDialog(selectedSupplier);
        }
        else
        {
            MessageBox.Show("Vui lòng chọn một nhà cung cấp để xem chi tiết.",
                           "Thông báo",
                           MessageBoxButtons.OK,
                           MessageBoxIcon.Information);
        }
    }

    private void DgvListItems_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex >= 0)
        {
            var selectedSupplier = GetSelectedSupplier();
            if (selectedSupplier != null)
            {
                OpenSupplierDetailsDialog(selectedSupplier);
            }
        }
    }

    #endregion

    #region Additional Helper Methods

    /// <summary>
    /// Setup thêm event cho DataGridView double-click
    /// </summary>
    private void SetupAdditionalEvents()
    {
        if (dgvSuppliers != null)
        {
            dgvSuppliers.CellDoubleClick += (s, o) => DgvListItems_CellDoubleClick(s!, o);

            dgvSuppliers.SelectionChanged += (s, e) =>
            {
                btnGetDetails.Enabled = dgvSuppliers.SelectedRows.Count > 0;
            };
        }
    }

    private void FinalizeFormSetup()
    {
        SetupAdditionalEvents();

        if (btnGetDetails != null)
            btnGetDetails.Enabled = false;
    }

    #endregion

    #region Context Menu for Refresh

    private void SetupContextMenu()
    {
        var contextMenu = new ContextMenuStrip();
        contextMenu.Items.Add("Làm mới dữ liệu", null, (s, e) => RefreshData());
        dgvSuppliers.ContextMenuStrip = contextMenu;
    }

    #endregion

    #region Thread-safe UI helpers

    /// <summary>
    /// Safely invokes an Action on the UI thread. If the control handle is not yet created,
    /// the action will be scheduled to run when HandleCreated fires.
    /// If the form is disposed or disposing, the action is ignored.
    /// </summary>
    private void SafeInvokeOnUI(Action action)
    {
        if (action == null) return;
        if (IsDisposed || Disposing) return;

        try
        {
            if (IsHandleCreated)
            {
                if (InvokeRequired)
                    BeginInvoke(action);
                else
                    action();
            }
            else
            {
                void Handler(object? s, EventArgs e)
                {
                    HandleCreated -= Handler;
                    try
                    {
                        if (!IsDisposed && IsHandleCreated)
                            BeginInvoke(action);
                    }
                    catch { /* swallow */ }
                }
                HandleCreated += Handler;
            }
        }
        catch
        {
            // ignore invocation exceptions
        }
    }

    #endregion
}