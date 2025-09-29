using ClosedXML.Excel;
using Dashboard.Winform.Forms.BaseFrm;
using Dashboard.Winform.Presenters.SupplierPresenters;
using Dashboard.Winform.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Sprache;
using System.ComponentModel;
using System.Text.RegularExpressions;

namespace Dashboard.Winform.Forms.SupplierFrm
{
    public partial class FrmSupplierDetails : FrmBaseAuthForm
    {
        #region Fields
        private bool _isEditMode;
        private long? _supplierId;
        private SupplierDetailViewModel _supplier;
        private readonly ISupplierDetailPresenter _presenter;
        private bool _isDataLoaded = false;
        private readonly ILogger<FrmSupplierDetails>? _logger;
        private bool _isProcessingSave = false;
        #endregion

        #region Properties
        public DialogResult Result { get; private set; }
        public SupplierDetailViewModel Supplier => _supplier;
        #endregion

        #region Constructor
        public FrmSupplierDetails(
            ISupplierDetailPresenter presenter,
            ILogger<FrmSupplierDetails>? logger = null,
            long? supplierId = null,
            SupplierDetailViewModel? supplier = null)
        {
            _presenter = presenter ?? throw new ArgumentNullException(nameof(presenter));
            _logger = logger;
            _supplierId = supplierId;
            _supplier = supplier ?? new SupplierDetailViewModel();
            _isEditMode = supplierId.HasValue;

            InitializeComponent();
            InitializeFormSettings();
            SetupEventHandlers();

            _presenter.OnSupplierSaved += Presenter_OnSupplierSaved;

            UpdateFormMode();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_presenter != null)
                {
                    _presenter.OnSupplierSaved -= Presenter_OnSupplierSaved;
                }

                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        public void SetInitData(long? supplierId, SupplierDetailViewModel? supplier)
        {
            _supplierId = supplierId;
            _supplier = supplier ?? new SupplierDetailViewModel();
            _isEditMode = supplierId.HasValue;
            UpdateFormMode();
        }
        #endregion

        #region Initialization Methods
        private void InitializeFormSettings()
        {
            Size = new Size(620, 600);
            StartPosition = FormStartPosition.CenterParent;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            ShowIcon = false;
            BackColor = Color.FromArgb(24, 28, 63);
        }

        private void UpdateFormMode()
        {
            if (_isEditMode)
            {
                Text = $"Chi tiết nhà cung cấp - {_supplier?.Name ?? "N/A"}";
                btnClose.Visible = true;
                btnSave.Text = "Lưu";
            }
            else
            {
                Text = "Thêm nhà cung cấp mới";
                btnClose.Visible = false;
                btnSave.Text = "Thêm";
            }
        }

        private void SetupEventHandlers()
        {
            Load += (s, e) => FrmSupplierDetails_Load(s!, e);

            btnSave.Click += (s, e) => BtnSave_Click(s!, e);
            btnCancel.Click += (s, e) => BtnCancel_Click(s!, e);
            btnClose.Click += (s, e) => BtnClose_Click(s!, e);

            // Validation handlers
            txtName.Validating += (s, e) => TxtName_Validating(s!, e);
            txtEmail.Validating += (s, e) => TxtEmail_Validating(s!, e);
            txtPhone.Validating += (s, e) => TxtPhone_Validating(s!, e);
        }

        private async void FrmSupplierDetails_Load(object sender, EventArgs e)
        {
            if (!_isDataLoaded)
            {
                await LoadInitialDataAsync();
                _isDataLoaded = true;
            }
        }

        private async Task LoadInitialDataAsync()
        {
            try
            {
                SetLoadingState(true);

                if (_isEditMode && _supplierId.HasValue)
                {
                    var vm = await _presenter.LoadSupplierAsync(_supplierId.Value);
                    if (vm != null)
                    {
                        _supplier = vm;
                    }
                }

                PopulateFormWithData();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi tải dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
            }
        }

        private void Presenter_OnSupplierSaved(object? sender, SupplierDetailViewModel? vm)
        {
            SafeInvokeOnUI(() =>
            {
                try
                {
                    if (vm != null)
                    {
                        _supplier = vm;
                        PopulateFormWithData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi cập nhật dữ liệu: {ex.Message}", "Lỗi",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            });
        }

        private void PopulateFormWithData()
        {
            if (_supplier == null) return;

            try
            {
                txtSupplierId.Text = _supplier.Id.ToString();
                txtName.Text = _supplier.Name ?? string.Empty;
                txtPhone.Text = _supplier.Phone ?? string.Empty;
                txtEmail.Text = _supplier.Email ?? string.Empty;
                txtAddress.Text = _supplier.Address ?? string.Empty;
                txtNote.Text = _supplier.Note ?? string.Empty;

                lblCreatedAt.Text = _supplier.CreatedAt.ToString("dd/MM/yyyy HH:mm");
                lblUpdatedAt.Text = _supplier.UpdatedAt?.ToString("dd/MM/yyyy HH:mm") ?? "N/A";

                if (!_isEditMode)
                {
                    txtName.Text = string.Empty;
                    txtPhone.Text = string.Empty;
                    txtEmail.Text = string.Empty;
                    txtAddress.Text = string.Empty;
                    txtNote.Text = string.Empty;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi load dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Event Handlers
        private async void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateForm()) return;

            if (_isProcessingSave) return;
            _isProcessingSave = true;

            try
            {
                SetLoadingState(true);

                _supplier = BuildSupplierFromForm();

                SupplierDetailViewModel? result = null;
                if (_isEditMode)
                {
                    result = await _presenter.UpdateSupplierAsync(_supplier);
                }
                else
                {
                    result = await _presenter.CreateSupplierAsync(_supplier);
                }

                if (result != null)
                {
                    _supplier = result;
                    Result = DialogResult.OK;
                    Close();
                }
                else
                {
                    MessageBox.Show("Lưu nhà cung cấp thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi lưu dữ liệu: {ex.Message}", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetLoadingState(false);
                _isProcessingSave = false;
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Result = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region Validation Event Handlers
        private void TxtName_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            if (string.IsNullOrWhiteSpace(textBox?.Text))
            {
                MessageBox.Show("Tên nhà cung cấp không được để trống", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void TxtEmail_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            var email = textBox?.Text?.Trim();

            if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
            {
                MessageBox.Show("Định dạng email không hợp lệ", "Lỗi nhập liệu",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                e.Cancel = true;
            }
        }

        private void TxtPhone_Validating(object sender, CancelEventArgs e)
        {
            var textBox = sender as TextBox;
            var phone = textBox?.Text?.Trim();

            if (!string.IsNullOrEmpty(phone))
            {
                var trimmedPhone = phone.Trim();
                if (!Regex.IsMatch(trimmedPhone, @"^\d{6,20}$"))
                {
                    MessageBox.Show("Số điện thoại không hợp lệ, chỉ được nhập từ 6-20 chữ số", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = true;

                }
            }
        }
        #endregion

        #region Helper Methods
        private bool ValidateForm()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Tên nhà cung cấp không được để trống", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtName.Focus();
                return false;
            }

            var email = txtEmail.Text?.Trim();
            if (!string.IsNullOrEmpty(email) && !IsValidEmail(email))
            {
                MessageBox.Show("Định dạng email không hợp lệ", "Lỗi",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtEmail.Focus();
                return false;
            }

            return true;
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        private SupplierDetailViewModel BuildSupplierFromForm()
        {
            var vm = _supplier ?? new SupplierDetailViewModel();

            vm.Name = txtName.Text.Trim();
            vm.Phone = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim();
            vm.Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim();
            vm.Address = string.IsNullOrWhiteSpace(txtAddress.Text) ? null : txtAddress.Text.Trim();
            vm.Note = string.IsNullOrWhiteSpace(txtNote.Text) ? null : txtNote.Text.Trim();

            return vm;
        }

        private void SetLoadingState(bool isLoading)
        {
            btnSave.Enabled = !isLoading;
            btnCancel.Enabled = !isLoading;
            btnClose.Enabled = !isLoading;

            txtName.Enabled = !isLoading;
            txtPhone.Enabled = !isLoading;
            txtEmail.Enabled = !isLoading;
            txtAddress.Enabled = !isLoading;
            txtNote.Enabled = !isLoading;

            this.Cursor = isLoading ? Cursors.WaitCursor : Cursors.Default;
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
                // ignore any exceptions from invoking
            }
        }

        /// <summary>
        /// Safely invokes an async Func on the UI thread.
        /// </summary>
        private void SafeInvokeOnUI(Func<Task> asyncAction)
        {
            if (asyncAction == null) return;
            SafeInvokeOnUI(() => { _ = asyncAction(); });
        }
        #endregion
    }
}