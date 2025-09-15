using System.Windows.Forms;

namespace Dashboard.Winform.Forms;

partial class FrmScheduleEditor
{
    private System.ComponentModel.IContainer components = null;
    private Label lblEmployee;
    private Label lblShiftDate;
    private Label lblStartTime;
    private Label lblEndTime;
    private Label lblStatus;
    private ComboBox cbxEmployee;
    private DateTimePicker dtpShiftDate;
    private DateTimePicker dtpStartTime;
    private DateTimePicker dtpEndTime;
    private ComboBox cbxStatus;
    private Button btnSave;
    private Button btnCancel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && (components != null))
        {
            components.Dispose();
        }
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        lblEmployee = new Label();
        lblShiftDate = new Label();
        lblStartTime = new Label();
        lblEndTime = new Label();
        lblStatus = new Label();
        cbxEmployee = new ComboBox();
        dtpShiftDate = new DateTimePicker();
        dtpStartTime = new DateTimePicker();
        dtpEndTime = new DateTimePicker();
        cbxStatus = new ComboBox();
        btnSave = new Button();
        btnCancel = new Button();

        SuspendLayout();

        // lblEmployee
        lblEmployee.AutoSize = true;
        lblEmployee.ForeColor = Color.White;
        lblEmployee.Location = new Point(20, 30);
        lblEmployee.Size = new Size(80, 15);
        lblEmployee.Text = "Nhân viên:";
        lblEmployee.Font = new Font("Segoe UI", 9F);

        // cbxEmployee
        cbxEmployee.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxEmployee.Location = new Point(120, 27);
        cbxEmployee.Size = new Size(280, 23);
        cbxEmployee.BackColor = Color.FromArgb(73, 75, 111);
        cbxEmployee.ForeColor = Color.White;
        cbxEmployee.FlatStyle = FlatStyle.Flat;

        // lblShiftDate
        lblShiftDate.AutoSize = true;
        lblShiftDate.ForeColor = Color.White;
        lblShiftDate.Location = new Point(20, 70);
        lblShiftDate.Size = new Size(80, 15);
        lblShiftDate.Text = "Ngày làm:";
        lblShiftDate.Font = new Font("Segoe UI", 9F);

        // dtpShiftDate
        dtpShiftDate.Location = new Point(120, 67);
        dtpShiftDate.Size = new Size(280, 23);
        dtpShiftDate.Format = DateTimePickerFormat.Short;

        // lblStartTime
        lblStartTime.AutoSize = true;
        lblStartTime.ForeColor = Color.White;
        lblStartTime.Location = new Point(20, 110);
        lblStartTime.Size = new Size(80, 15);
        lblStartTime.Text = "Giờ bắt đầu:";
        lblStartTime.Font = new Font("Segoe UI", 9F);

        // dtpStartTime
        dtpStartTime.Location = new Point(120, 107);
        dtpStartTime.Size = new Size(130, 23);
        dtpStartTime.Format = DateTimePickerFormat.Time;
        dtpStartTime.ShowUpDown = true;
        dtpStartTime.ValueChanged += dtpStartTime_ValueChanged;

        // lblEndTime
        lblEndTime.AutoSize = true;
        lblEndTime.ForeColor = Color.White;
        lblEndTime.Location = new Point(270, 110);
        lblEndTime.Size = new Size(80, 15);
        lblEndTime.Text = "Giờ kết thúc:";
        lblEndTime.Font = new Font("Segoe UI", 9F);

        // dtpEndTime
        dtpEndTime.Location = new Point(270, 107);
        dtpEndTime.Size = new Size(130, 23);
        dtpEndTime.Format = DateTimePickerFormat.Time;
        dtpEndTime.ShowUpDown = true;

        // lblStatus
        lblStatus.AutoSize = true;
        lblStatus.ForeColor = Color.White;
        lblStatus.Location = new Point(20, 150);
        lblStatus.Size = new Size(80, 15);
        lblStatus.Text = "Trạng thái:";
        lblStatus.Font = new Font("Segoe UI", 9F);

        // cbxStatus
        cbxStatus.DropDownStyle = ComboBoxStyle.DropDownList;
        cbxStatus.Location = new Point(120, 147);
        cbxStatus.Size = new Size(280, 23);
        cbxStatus.BackColor = Color.FromArgb(73, 75, 111);
        cbxStatus.ForeColor = Color.White;
        cbxStatus.FlatStyle = FlatStyle.Flat;

        // btnSave
        btnSave.BackColor = Color.FromArgb(0, 120, 215);
        btnSave.FlatStyle = FlatStyle.Flat;
        btnSave.ForeColor = Color.White;
        btnSave.Location = new Point(200, 200);
        btnSave.Size = new Size(90, 35);
        btnSave.Text = "Lưu";
        btnSave.UseVisualStyleBackColor = false;
        btnSave.Font = new Font("Segoe UI", 9F);
        btnSave.Click += btnSave_Click;

        // btnCancel
        btnCancel.BackColor = Color.FromArgb(73, 75, 111);
        btnCancel.FlatStyle = FlatStyle.Flat;
        btnCancel.ForeColor = Color.White;
        btnCancel.Location = new Point(310, 200);
        btnCancel.Size = new Size(90, 35);
        btnCancel.Text = "Hủy";
        btnCancel.UseVisualStyleBackColor = false;
        btnCancel.Font = new Font("Segoe UI", 9F);
        btnCancel.Click += btnCancel_Click;

        // FrmScheduleEditor
        ClientSize = new Size(450, 270);
        Controls.Add(lblEmployee);
        Controls.Add(cbxEmployee);
        Controls.Add(lblShiftDate);
        Controls.Add(dtpShiftDate);
        Controls.Add(lblStartTime);
        Controls.Add(dtpStartTime);
        Controls.Add(lblEndTime);
        Controls.Add(dtpEndTime);
        Controls.Add(lblStatus);
        Controls.Add(cbxStatus);
        Controls.Add(btnSave);
        Controls.Add(btnCancel);

        ResumeLayout(false);
        PerformLayout();
    }
}
