using Dashboard.Winform.Interfaces;
using Dashboard.Winform.Presenters;
using Dashboard.Winform.ViewModels;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms;

#region Non-Generic Base UI Management Form
public partial class FrmBaseManagement : Form, IBlurLoadingServiceAware
{
    protected IBlurLoadingService? _blurLoadingService;
    protected TaskCompletionSource<bool>? _dataLoadingCompletionSource;

    public FrmBaseManagement()
    {
        _dataLoadingCompletionSource = new TaskCompletionSource<bool>();
        InitializeComponent();
        InitializeEvents();
    }

    #region Setup components
    protected virtual void InitializeBaseComponents()
    {
        SetupCommonUI();
        SetupCommonEventHandlers();
        SetupCommonDropdowns();
        InitializeDerivedComponents();
    }
    private void SetupCommonUI()
    {
        Text = "Management Form";
    }
    private void SetupCommonDropdowns()
    {
        cbxNumbRecordsPerPage.Items.Clear();
        cbxNumbRecordsPerPage.Items.AddRange([10, 25, 50, 100]);
        cbxNumbRecordsPerPage.SelectedItem = 10;

        cbxOrderBy.Items.Clear();
        cbxOrderBy.Items.AddRange(["ID", "Name", "Created Date"]);
        if (cbxOrderBy.Items.Count > 0)
            cbxOrderBy.SelectedIndex = 0;
    }
    private void SetupCommonEventHandlers()
    {
        btnAdd.Click += (s, e) => BtnAdd_Click(s!, e);
        btnGetDetails.Click += (s, e) => BtnGetDetails_Click(s!, e);
        btnOrderBy.Click += (s, e) => BtnOrderBy_Click(s!, e);
        btnfilter1.Click += (s, e) => Btnfilter1_Click(s!, e);
        btnFilter2.Click += (s, e) => BtnFilter2_Click(s!, e);
        btnNumbOfRecordShowing.Click += (s, e) => BtnNumbOfRecordShowing_Click(s!, e);

        btnNext.Click += (s, e) => BtnNext_Click(s!, e);
        btnToday.Click += (s, e) => BtnToday_Click(s!, e);

        cbxNumbRecordsPerPage.SelectedIndexChanged += (s, e) => CbxNumbRecordsPerPage_SelectedIndexChanged(s!, e);
        cbxOrderBy.SelectedIndexChanged += (s, e) => CbxOrderBy_SelectedIndexChanged(s!, e);
    }
    protected virtual void InitializeDerivedComponents()
    {
        throw new NotImplementedException("Derived classes must implement InitializeDerivedComponents.");
    }

    protected void DgvListItems_RowPrePaint(object? sender, DataGridViewRowPrePaintEventArgs e)
    {
        DataGridView? grid = sender as DataGridView;
        if (grid == null || e.RowIndex < 0) return;

        var row = grid.Rows[e.RowIndex];

        if (row.Selected)
        {
            row.DefaultCellStyle.SelectionForeColor = row.DefaultCellStyle.ForeColor;
            row.DefaultCellStyle.SelectionBackColor = Color.FromArgb(73, 75, 111);
        }
    }
    #endregion

    public async Task WaitForDataLoadingComplete()
    {
        if (_dataLoadingCompletionSource != null)
        {
            await _dataLoadingCompletionSource.Task;
        }
    }

    #region Events Initialization
    protected virtual void InitializeEvents()
    {
        cbxOrderBy.SelectedIndexChanged += (s, o) => CbxOrderBySelectedIndexChanged(s!, o);
        cbxFilter1.SelectedIndexChanged += (s, o) => CbxFilterByGoodsStatusSelectedIndexChanged(s!, o);
        cbxFilter2.SelectedIndexChanged += (s, o) => CbxFilterByStockStatusSelectedIndexChanged(s!, o);
        cbxNumbRecordsPerPage.SelectedIndexChanged += (s, o) => CbxNumbRecordsPerPageSelectedIndexChanged(s!, o);
    }

    protected virtual void CbxNumbRecordsPerPageSelectedIndexChanged(object v, EventArgs o)
    {
        if (cbxNumbRecordsPerPage.SelectedItem != null)
            btnNumbOfRecordShowing.Text = cbxNumbRecordsPerPage.Text;
    }

    protected virtual void CbxFilterByStockStatusSelectedIndexChanged(object v, EventArgs o)
    {
        if (cbxFilter2.SelectedItem != null)
            btnFilter2.Text = cbxFilter2.Text;
    }

    protected virtual void CbxFilterByGoodsStatusSelectedIndexChanged(object s, EventArgs o)
    {
        if (cbxFilter1.SelectedItem != null)
            btnfilter1.Text = cbxFilter1.Text;
    }

    protected virtual void CbxOrderBySelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbxOrderBy.SelectedItem != null)
            btnOrderBy.Text = cbxOrderBy.Text;
    }
    #endregion

    public void SetBlurLoadingService(IBlurLoadingService blurLoadingService)
    {
        _blurLoadingService = blurLoadingService;
    }

    #region Virtual Event Handlers - Override in derived classes

    protected virtual void BtnSearch_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException("Derived classes must implement BtnSearch_Click.");
    }

    protected virtual async Task TbxFindString_TextChanged(object sender, EventArgs e)
    {
        await Task.Delay(300);
    }
    protected virtual void BtnAdd_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException("Derived classes must implement BtnAdd_Click.");
    }
    protected virtual void BtnOrderBy_Click(object sender, EventArgs e)
    {
        cbxOrderBy.DroppedDown = true;
    }
    protected virtual void Btnfilter1_Click(object sender, EventArgs e)
    {
        cbxFilter1.DroppedDown = true;
    }
    protected virtual void BtnFilter2_Click(object sender, EventArgs e)
    {
        cbxFilter2.DroppedDown = true;
    }
    protected virtual void BtnNumbOfRecordShowing_Click(object sender, EventArgs e)
    {
        cbxNumbRecordsPerPage.DroppedDown = true;
    }
    protected virtual void BtnNext_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException("Derived classes must implement BtnNext_Click.");
    }
    protected virtual void BtnToday_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException("Derived classes must implement BtnToday_Click.");
    }
    protected virtual void CbxNumbRecordsPerPage_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbxNumbRecordsPerPage.SelectedItem != null)
            btnNumbOfRecordShowing.Text = cbxNumbRecordsPerPage.Text;
    }
    protected virtual void CbxOrderBy_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbxOrderBy.SelectedItem != null)
            btnOrderBy.Text = cbxOrderBy.Text;
        else btnOrderBy.Text = "Id";
    }
    protected virtual void CbxFilter1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbxFilter1.SelectedItem != null)
            btnfilter1.Text = cbxFilter1.Text;
        else btnfilter1.Text = "All";
    }
    protected virtual void CbxFilter2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (cbxFilter2.SelectedItem != null)
            btnFilter2.Text = cbxFilter2.Text;
        else btnFilter2.Text = "All";
    }
    protected virtual void BtnGetDetails_Click(object sender, EventArgs e)
    {
        throw new NotImplementedException("Derived classes must implement btnGetDetails_Click.");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Update records count display
    /// </summary>
    protected void UpdateRecordsDisplay(int totalRecords, int currentPage, int totalPages)
    {
        lblNumberOfRecords.Text = $"Số lượng: {totalRecords:N0}";
        lblShowingAtPage.Text = $"Hiện trang {currentPage} trên {totalPages}";
    }

    /// <summary>
    /// Enable/disable pagination buttons
    /// </summary>
    protected void UpdatePaginationButtons(bool canGoPrevious, bool canGoNext)
    {

        btnToday.Enabled = canGoPrevious;
        btnNext.Enabled = canGoNext;
    }
    /// <summary>
    /// Show loading state
    /// </summary>
    protected void ShowLoading(bool isLoading)
    {
        tbpnHeaderManagementSection.Enabled = !isLoading;
        pnlContent.Enabled = !isLoading;

        if (isLoading)
        {
            Cursor = Cursors.WaitCursor;
        }
        else
        {
            Cursor = Cursors.Default;
        }
    }
    /// <summary>
    /// Clear search and filters
    /// </summary>
    protected void ClearFilters()
    {
        tbxFindString.Clear();

        if (cbxFilter1.Items.Count > 0)
            cbxFilter1.SelectedIndex = 0;

        if (cbxFilter2.Items.Count > 0)
            cbxFilter2.SelectedIndex = 0;

        if (cbxOrderBy.Items.Count > 0)
            cbxOrderBy.SelectedIndex = 0;
    }
    /// <summary>
    /// Show error message
    /// </summary>
    protected void ShowErrorMessage(string message, string title = "Lỗi")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }

    /// <summary>
    /// Show success message
    /// </summary>
    protected void ShowSuccessMessage(string message, string title = "Thành công")
    {
        MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Information);
    }
    /// <summary>
    /// Show confirmation dialog
    /// </summary>
    protected DialogResult ShowConfirmation(string message, string title = "Xác nhận")
    {
        return MessageBox.Show(message, title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
    }
    #endregion

    #region Form Events

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
    }

    protected override void OnClosed(EventArgs e)
    {
        base.OnClosed(e);
    }

    #endregion
}
#endregion

#region Generic Base Management Form
public abstract class FrmBaseManagement<TModel, TPresenter> : FrmBaseManagement
    where TModel : class, IManagableModel
    where TPresenter : IManagementPresenter<TModel>
{
    protected readonly TPresenter _presenter;
    protected readonly ILogger _logger;

    protected FrmBaseManagement(ILogger logger, TPresenter presenter) : base()
    {
        _logger = logger;
        _presenter = presenter;
    }

    public virtual void OnDataLoaded(object? sender, EventArgs e) { throw new NotImplementedException(); }
}
#endregion
