using System;
using System.Drawing;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms.CostFrms
{
    public partial class DateRangeDialog : Form
    {
        #region Properties and Fields
        public DateTime FromDate { get; private set; }
        public DateTime ToDate { get; private set; }

        private DateTimePicker? dtpFromDate;
        private DateTimePicker? dtpToDate;
        private Label? lblFromDate;
        private Label? lblToDate;
        private Button? btnOK;
        private Button? btnCancel;
        
        private bool _isInitializing = true;
        #endregion

        #region Constructor
        public DateRangeDialog(DateTime currentFromDate, DateTime currentToDate)
        {
            // Validate input dates
            if (currentFromDate > currentToDate)
            {
                throw new ArgumentException("From date cannot be greater than to date");
            }

            InitializeComponent();
            
            // Set initial values
            FromDate = currentFromDate;
            ToDate = currentToDate;
            
            // Set DateTimePicker values after initialization
            SetInitialValues(currentFromDate, currentToDate);
            
            _isInitializing = false;
        }
        #endregion

        #region Initialization Methods
        private void InitializeComponent()
        {
            // Initialize controls first
            this.dtpFromDate = new DateTimePicker();
            this.dtpToDate = new DateTimePicker();
            this.lblFromDate = new Label();
            this.lblToDate = new Label();
            this.btnOK = new Button();
            this.btnCancel = new Button();
            
            this.SuspendLayout();

            // Form properties
            this.Text = "Chọn khoảng thời gian";
            this.Size = new Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(42, 45, 86);
            this.ForeColor = Color.White;

            // lblFromDate
            this.lblFromDate.Text = "Từ ngày:";
            this.lblFromDate.Location = new Point(20, 30);
            this.lblFromDate.Size = new Size(70, 23);
            this.lblFromDate.TextAlign = ContentAlignment.MiddleLeft;
            this.lblFromDate.ForeColor = Color.White;

            // dtpFromDate
            this.dtpFromDate.Location = new Point(100, 30);
            this.dtpFromDate.Size = new Size(250, 27);
            this.dtpFromDate.Format = DateTimePickerFormat.Short;

            // lblToDate
            this.lblToDate.Text = "Đến ngày:";
            this.lblToDate.Location = new Point(20, 70);
            this.lblToDate.Size = new Size(70, 23);
            this.lblToDate.TextAlign = ContentAlignment.MiddleLeft;
            this.lblToDate.ForeColor = Color.White;

            // dtpToDate
            this.dtpToDate.Location = new Point(100, 70);
            this.dtpToDate.Size = new Size(250, 27);
            this.dtpToDate.Format = DateTimePickerFormat.Short;

            // btnOK
            this.btnOK.Text = "Đồng ý";
            this.btnOK.Location = new Point(195, 120);
            this.btnOK.Size = new Size(80, 32);
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.BackColor = Color.FromArgb(99, 108, 203);
            this.btnOK.ForeColor = Color.White;
            this.btnOK.FlatStyle = FlatStyle.Flat;

            // btnCancel
            this.btnCancel.Text = "Hủy bỏ";
            this.btnCancel.Location = new Point(285, 120);
            this.btnCancel.Size = new Size(80, 32);
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.BackColor = Color.FromArgb(64, 64, 64);
            this.btnCancel.ForeColor = Color.White;
            this.btnCancel.FlatStyle = FlatStyle.Flat;

            // Add event handlers
            this.dtpFromDate.ValueChanged += DtpFromDate_ValueChanged;
            this.dtpToDate.ValueChanged += DtpToDate_ValueChanged;
            this.btnOK.Click += BtnOK_Click;
            this.btnCancel.Click += BtnCancel_Click;

            // Add controls to form
            this.Controls.AddRange(new Control[]
            {
                this.lblFromDate,
                this.dtpFromDate,
                this.lblToDate,
                this.dtpToDate,
                this.btnOK,
                this.btnCancel
            });

            // Set form accept/cancel buttons
            this.AcceptButton = this.btnOK;
            this.CancelButton = this.btnCancel;

            this.ResumeLayout(false);
        }

        private void SetInitialValues(DateTime fromDate, DateTime toDate)
        {
            if (dtpFromDate == null || dtpToDate == null) return;

            try
            {
                // Validate and set from date
                if (fromDate >= dtpFromDate.MinDate && fromDate <= dtpFromDate.MaxDate)
                {
                    dtpFromDate.Value = fromDate;
                }
                else
                {
                    dtpFromDate.Value = dtpFromDate.MinDate > fromDate ? dtpFromDate.MinDate : dtpFromDate.MaxDate;
                }

                // Validate and set to date
                if (toDate >= dtpToDate.MinDate && toDate <= dtpToDate.MaxDate)
                {
                    dtpToDate.Value = toDate;
                }
                else
                {
                    dtpToDate.Value = dtpToDate.MinDate > toDate ? dtpToDate.MinDate : dtpToDate.MaxDate;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error setting initial values: {ex.Message}");
                // Set safe default values
                dtpFromDate.Value = DateTime.Today.AddDays(-30);
                dtpToDate.Value = DateTime.Today;
            }
        }
        #endregion

        #region Event Handlers
        private void DtpFromDate_ValueChanged(object? sender, EventArgs e)
        {
            if (_isInitializing || dtpFromDate == null || dtpToDate == null) return;

            try
            {
                // Auto-adjust ToDate if FromDate is greater
                if (dtpFromDate.Value > dtpToDate.Value)
                {
                    // Temporarily remove event to prevent recursion
                    dtpToDate.ValueChanged -= DtpToDate_ValueChanged;
                    dtpToDate.Value = dtpFromDate.Value;
                    dtpToDate.ValueChanged += DtpToDate_ValueChanged;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DtpFromDate_ValueChanged: {ex.Message}");
            }
        }

        private void DtpToDate_ValueChanged(object? sender, EventArgs e)
        {
            if (_isInitializing || dtpFromDate == null || dtpToDate == null) return;

            try
            {
                // Auto-adjust FromDate if ToDate is less
                if (dtpToDate.Value < dtpFromDate.Value)
                {
                    // Temporarily remove event to prevent recursion
                    dtpFromDate.ValueChanged -= DtpFromDate_ValueChanged;
                    dtpFromDate.Value = dtpToDate.Value;
                    dtpFromDate.ValueChanged += DtpFromDate_ValueChanged;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in DtpToDate_ValueChanged: {ex.Message}");
            }
        }

        private void BtnOK_Click(object? sender, EventArgs e)
        {
            if (dtpFromDate == null || dtpToDate == null) return;

            try
            {
                // Final validation
                if (dtpFromDate.Value > dtpToDate.Value)
                {
                    MessageBox.Show("Ngày bắt đầu không thể lớn hơn ngày kết thúc!", 
                        "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    dtpFromDate.Focus();
                    return;
                }

                // Check if date range is reasonable (not more than 5 years)
                var timeSpan = dtpToDate.Value - dtpFromDate.Value;
                if (timeSpan.TotalDays > 365 * 5)
                {
                    var result = MessageBox.Show(
                        "Khoảng thời gian rất lớn (hơn 5 năm). Điều này có thể ảnh hưởng đến hiệu suất.\n\nBạn có chắc chắn muốn tiếp tục?", 
                        "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    
                    if (result == DialogResult.No)
                    {
                        dtpToDate.Focus();
                        return;
                    }
                }

                // Set return values
                FromDate = dtpFromDate.Value.Date;
                ToDate = dtpToDate.Value.Date.AddDays(1).AddSeconds(-1); // End of day

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", 
                    "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
        #endregion

        #region Override Methods
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            
            try
            {
                // Set focus to first DateTimePicker
                dtpFromDate?.Focus();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in OnLoad: {ex.Message}");
            }
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            try
            {
                // Handle keyboard shortcuts
                switch (keyData)
                {
                    case Keys.Escape:
                        this.DialogResult = DialogResult.Cancel;
                        this.Close();
                        return true;
                    
                    case Keys.Enter:
                        BtnOK_Click(this, EventArgs.Empty);
                        return true;
                    
                    default:
                        return base.ProcessDialogKey(keyData);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in ProcessDialogKey: {ex.Message}");
                return base.ProcessDialogKey(keyData);
            }
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            
            // Center the dialog on parent
            if (this.Owner != null)
            {
                this.Location = new Point(
                    this.Owner.Location.X + (this.Owner.Width - this.Width) / 2,
                    this.Owner.Location.Y + (this.Owner.Height - this.Height) / 2);
            }
        }
        #endregion

        #region Dispose Method
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                try
                {
                    // Remove event handlers to prevent memory leaks
                    if (dtpFromDate != null)
                    {
                        dtpFromDate.ValueChanged -= DtpFromDate_ValueChanged;
                        dtpFromDate.Dispose();
                    }
                    
                    if (dtpToDate != null)
                    {
                        dtpToDate.ValueChanged -= DtpToDate_ValueChanged;
                        dtpToDate.Dispose();
                    }
                    
                    if (btnOK != null)
                    {
                        btnOK.Click -= BtnOK_Click;
                        btnOK.Dispose();
                    }
                    
                    if (btnCancel != null)
                    {
                        btnCancel.Click -= BtnCancel_Click;
                        btnCancel.Dispose();
                    }

                    // Dispose other controls
                    lblFromDate?.Dispose();
                    lblToDate?.Dispose();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error in Dispose: {ex.Message}");
                }
            }
            
            base.Dispose(disposing);
        }
        #endregion
    }
}