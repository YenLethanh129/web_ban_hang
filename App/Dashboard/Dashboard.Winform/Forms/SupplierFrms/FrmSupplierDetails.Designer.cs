namespace Dashboard.Winform.Forms.SupplierFrm
{
    partial class FrmSupplierDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing && (components != null))
        //    {
        //        components.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            pnlMain = new Panel();
            lblSupplierId = new Label();
            txtSupplierId = new TextBox();
            lblName = new Label();
            txtName = new TextBox();
            lblPhone = new Label();
            txtPhone = new TextBox();
            lblEmail = new Label();
            txtEmail = new TextBox();
            lblAddress = new Label();
            txtAddress = new TextBox();
            lblNote = new Label();
            txtNote = new TextBox();
            lblCreatedAtLabel = new Label();
            lblCreatedAt = new Label();
            lblUpdatedAtLabel = new Label();
            lblUpdatedAt = new Label();
            pnlButtons = new Panel();
            btnSave = new Button();
            btnCancel = new Button();
            btnClose = new Button();
            pnlMain.SuspendLayout();
            pnlButtons.SuspendLayout();
            SuspendLayout();
            // 
            // pnlMain
            // 
            pnlMain.BackColor = Color.FromArgb(42, 45, 86);
            pnlMain.Controls.Add(lblSupplierId);
            pnlMain.Controls.Add(txtSupplierId);
            pnlMain.Controls.Add(lblName);
            pnlMain.Controls.Add(txtName);
            pnlMain.Controls.Add(lblPhone);
            pnlMain.Controls.Add(txtPhone);
            pnlMain.Controls.Add(lblEmail);
            pnlMain.Controls.Add(txtEmail);
            pnlMain.Controls.Add(lblAddress);
            pnlMain.Controls.Add(txtAddress);
            pnlMain.Controls.Add(lblNote);
            pnlMain.Controls.Add(txtNote);
            pnlMain.Controls.Add(lblCreatedAtLabel);
            pnlMain.Controls.Add(lblCreatedAt);
            pnlMain.Controls.Add(lblUpdatedAtLabel);
            pnlMain.Controls.Add(lblUpdatedAt);
            pnlMain.Dock = DockStyle.Fill;
            pnlMain.Location = new Point(10, 10);
            pnlMain.Name = "pnlMain";
            pnlMain.Padding = new Padding(20);
            pnlMain.Size = new Size(580, 486);
            pnlMain.TabIndex = 0;
            // 
            // lblSupplierId
            // 
            lblSupplierId.AutoSize = true;
            lblSupplierId.ForeColor = Color.FromArgb(124, 141, 181);
            lblSupplierId.Location = new Point(20, 25);
            lblSupplierId.Name = "lblSupplierId";
            lblSupplierId.Size = new Size(96, 15);
            lblSupplierId.TabIndex = 0;
            lblSupplierId.Text = "ID nhà cung cấp:";
            // 
            // txtSupplierId
            // 
            txtSupplierId.BackColor = Color.FromArgb(24, 28, 63);
            txtSupplierId.BorderStyle = BorderStyle.FixedSingle;
            txtSupplierId.ForeColor = Color.WhiteSmoke;
            txtSupplierId.Location = new Point(140, 22);
            txtSupplierId.Name = "txtSupplierId";
            txtSupplierId.ReadOnly = true;
            txtSupplierId.Size = new Size(120, 23);
            txtSupplierId.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.AutoSize = true;
            lblName.ForeColor = Color.FromArgb(124, 141, 181);
            lblName.Location = new Point(20, 65);
            lblName.Name = "lblName";
            lblName.Size = new Size(104, 15);
            lblName.TabIndex = 2;
            lblName.Text = "Tên nhà cung cấp:";
            // 
            // txtName
            // 
            txtName.BackColor = Color.FromArgb(24, 28, 63);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.WhiteSmoke;
            txtName.Location = new Point(140, 62);
            txtName.Name = "txtName";
            txtName.Size = new Size(400, 23);
            txtName.TabIndex = 3;
            // 
            // lblPhone
            // 
            lblPhone.AutoSize = true;
            lblPhone.ForeColor = Color.FromArgb(124, 141, 181);
            lblPhone.Location = new Point(20, 105);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(79, 15);
            lblPhone.TabIndex = 4;
            lblPhone.Text = "Số điện thoại:";
            // 
            // txtPhone
            // 
            txtPhone.BackColor = Color.FromArgb(24, 28, 63);
            txtPhone.BorderStyle = BorderStyle.FixedSingle;
            txtPhone.ForeColor = Color.WhiteSmoke;
            txtPhone.Location = new Point(140, 102);
            txtPhone.Name = "txtPhone";
            txtPhone.Size = new Size(200, 23);
            txtPhone.TabIndex = 5;
            // 
            // lblEmail
            // 
            lblEmail.AutoSize = true;
            lblEmail.ForeColor = Color.FromArgb(124, 141, 181);
            lblEmail.Location = new Point(20, 145);
            lblEmail.Name = "lblEmail";
            lblEmail.Size = new Size(39, 15);
            lblEmail.TabIndex = 6;
            lblEmail.Text = "Email:";
            // 
            // txtEmail
            // 
            txtEmail.BackColor = Color.FromArgb(24, 28, 63);
            txtEmail.BorderStyle = BorderStyle.FixedSingle;
            txtEmail.ForeColor = Color.WhiteSmoke;
            txtEmail.Location = new Point(140, 142);
            txtEmail.Name = "txtEmail";
            txtEmail.Size = new Size(300, 23);
            txtEmail.TabIndex = 7;
            // 
            // lblAddress
            // 
            lblAddress.AutoSize = true;
            lblAddress.ForeColor = Color.FromArgb(124, 141, 181);
            lblAddress.Location = new Point(20, 185);
            lblAddress.Name = "lblAddress";
            lblAddress.Size = new Size(46, 15);
            lblAddress.TabIndex = 8;
            lblAddress.Text = "Địa chỉ:";
            // 
            // txtAddress
            // 
            txtAddress.BackColor = Color.FromArgb(24, 28, 63);
            txtAddress.BorderStyle = BorderStyle.FixedSingle;
            txtAddress.ForeColor = Color.WhiteSmoke;
            txtAddress.Location = new Point(140, 182);
            txtAddress.Multiline = true;
            txtAddress.Name = "txtAddress";
            txtAddress.ScrollBars = ScrollBars.Vertical;
            txtAddress.Size = new Size(400, 80);
            txtAddress.TabIndex = 9;
            // 
            // lblNote
            // 
            lblNote.AutoSize = true;
            lblNote.ForeColor = Color.FromArgb(124, 141, 181);
            lblNote.Location = new Point(20, 285);
            lblNote.Name = "lblNote";
            lblNote.Size = new Size(52, 15);
            lblNote.TabIndex = 10;
            lblNote.Text = "Ghi chú:";
            // 
            // txtNote
            // 
            txtNote.BackColor = Color.FromArgb(24, 28, 63);
            txtNote.BorderStyle = BorderStyle.FixedSingle;
            txtNote.ForeColor = Color.WhiteSmoke;
            txtNote.Location = new Point(140, 282);
            txtNote.Multiline = true;
            txtNote.Name = "txtNote";
            txtNote.ScrollBars = ScrollBars.Vertical;
            txtNote.Size = new Size(400, 80);
            txtNote.TabIndex = 11;
            // 
            // lblCreatedAtLabel
            // 
            lblCreatedAtLabel.AutoSize = true;
            lblCreatedAtLabel.ForeColor = Color.FromArgb(124, 141, 181);
            lblCreatedAtLabel.Location = new Point(20, 385);
            lblCreatedAtLabel.Name = "lblCreatedAtLabel";
            lblCreatedAtLabel.Size = new Size(58, 15);
            lblCreatedAtLabel.TabIndex = 12;
            lblCreatedAtLabel.Text = "Ngày tạo:";
            // 
            // lblCreatedAt
            // 
            lblCreatedAt.AutoSize = true;
            lblCreatedAt.ForeColor = Color.FromArgb(255, 192, 128);
            lblCreatedAt.Location = new Point(140, 385);
            lblCreatedAt.Name = "lblCreatedAt";
            lblCreatedAt.Size = new Size(0, 15);
            lblCreatedAt.TabIndex = 13;
            // 
            // lblUpdatedAtLabel
            // 
            lblUpdatedAtLabel.AutoSize = true;
            lblUpdatedAtLabel.ForeColor = Color.FromArgb(124, 141, 181);
            lblUpdatedAtLabel.Location = new Point(20, 415);
            lblUpdatedAtLabel.Name = "lblUpdatedAtLabel";
            lblUpdatedAtLabel.Size = new Size(87, 15);
            lblUpdatedAtLabel.TabIndex = 14;
            lblUpdatedAtLabel.Text = "Ngày cập nhật:";
            // 
            // lblUpdatedAt
            // 
            lblUpdatedAt.AutoSize = true;
            lblUpdatedAt.ForeColor = Color.FromArgb(255, 192, 128);
            lblUpdatedAt.Location = new Point(140, 415);
            lblUpdatedAt.Name = "lblUpdatedAt";
            lblUpdatedAt.Size = new Size(0, 15);
            lblUpdatedAt.TabIndex = 15;
            // 
            // pnlButtons
            // 
            pnlButtons.BackColor = Color.FromArgb(24, 28, 63);
            pnlButtons.Controls.Add(btnSave);
            pnlButtons.Controls.Add(btnCancel);
            pnlButtons.Controls.Add(btnClose);
            pnlButtons.Dock = DockStyle.Bottom;
            pnlButtons.Location = new Point(10, 496);
            pnlButtons.Name = "pnlButtons";
            pnlButtons.Padding = new Padding(0, 8, 0, 8);
            pnlButtons.Size = new Size(580, 50);
            pnlButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnSave.DialogResult = DialogResult.OK;
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Microsoft Sans Serif", 10F);
            btnSave.ForeColor = Color.FromArgb(192, 255, 192);
            btnSave.Location = new Point(343, 14);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(70, 28);
            btnSave.TabIndex = 0;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnCancel.DialogResult = DialogResult.Cancel;
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(255, 193, 7);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Microsoft Sans Serif", 10F);
            btnCancel.ForeColor = Color.FromArgb(255, 193, 7);
            btnCancel.Location = new Point(422, 14);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(70, 28);
            btnCancel.TabIndex = 1;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.DialogResult = DialogResult.Cancel;
            btnClose.FlatAppearance.BorderColor = Color.FromArgb(255, 99, 132);
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Font = new Font("Microsoft Sans Serif", 10F);
            btnClose.ForeColor = Color.FromArgb(255, 99, 132);
            btnClose.Location = new Point(501, 14);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(70, 28);
            btnClose.TabIndex = 2;
            btnClose.Text = "Đóng";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // FrmSupplierDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(600, 556);
            Controls.Add(pnlMain);
            Controls.Add(pnlButtons);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmSupplierDetails";
            Padding = new Padding(10);
            ShowIcon = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết nhà cung cấp";
            pnlMain.ResumeLayout(false);
            pnlMain.PerformLayout();
            pnlButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnlMain;
        private Label lblSupplierId;
        private TextBox txtSupplierId;
        private Label lblName;
        private TextBox txtName;
        private Label lblPhone;
        private TextBox txtPhone;
        private Label lblEmail;
        private TextBox txtEmail;
        private Label lblAddress;
        private TextBox txtAddress;
        private Label lblNote;
        private TextBox txtNote;
        private Label lblCreatedAtLabel;
        private Label lblCreatedAt;
        private Label lblUpdatedAtLabel;
        private Label lblUpdatedAt;
        private Panel pnlButtons;
        private Button btnSave;
        private Button btnCancel;
        private Button btnClose;
    }
}