namespace Dashboard.Winform.Forms
{
    partial class FrmIngredientDetails
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            mainPanel = new TableLayoutPanel();
            contentPanel = new Panel();
            contentTableLayout = new TableLayoutPanel();
            lblId = new Label();
            txtId = new TextBox();
            lblName = new Label();
            txtName = new TextBox();
            lblUnit = new Label();
            txtUnit = new TextBox();
            lblCategory = new Label();
            cbxCategory = new ComboBox();
            lblDescription = new Label();
            txtDescription = new TextBox();
            lblStatus = new Label();
            chkIsActive = new CheckBox();
            lblCreated = new Label();
            txtCreated = new TextBox();
            lblUpdated = new Label();
            txtUpdated = new TextBox();
            buttonPanel = new Panel();
            btnCancel = new Button();
            btnSave = new Button();
            errorProvider = new ErrorProvider(components);
            mainPanel.SuspendLayout();
            contentPanel.SuspendLayout();
            contentTableLayout.SuspendLayout();
            buttonPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)errorProvider).BeginInit();
            SuspendLayout();
            // 
            // mainPanel
            // 
            mainPanel.BackColor = Color.FromArgb(24, 28, 63);
            mainPanel.ColumnCount = 1;
            mainPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainPanel.Controls.Add(contentPanel, 0, 0);
            mainPanel.Controls.Add(buttonPanel, 0, 1);
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Location = new Point(0, 0);
            mainPanel.Name = "mainPanel";
            mainPanel.Padding = new Padding(20);
            mainPanel.RowCount = 2;
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 85F));
            mainPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 15F));
            mainPanel.Size = new Size(500, 600);
            mainPanel.TabIndex = 0;
            // 
            // contentPanel
            // 
            contentPanel.BackColor = Color.FromArgb(42, 45, 86);
            contentPanel.Controls.Add(contentTableLayout);
            contentPanel.Dock = DockStyle.Fill;
            contentPanel.Location = new Point(23, 23);
            contentPanel.Name = "contentPanel";
            contentPanel.Padding = new Padding(20);
            contentPanel.Size = new Size(454, 470);
            contentPanel.TabIndex = 0;
            // 
            // contentTableLayout
            // 
            contentTableLayout.BackColor = Color.Transparent;
            contentTableLayout.ColumnCount = 2;
            contentTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
            contentTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            contentTableLayout.Controls.Add(lblId, 0, 0);
            contentTableLayout.Controls.Add(txtId, 1, 0);
            contentTableLayout.Controls.Add(lblName, 0, 1);
            contentTableLayout.Controls.Add(txtName, 1, 1);
            contentTableLayout.Controls.Add(lblUnit, 0, 2);
            contentTableLayout.Controls.Add(txtUnit, 1, 2);
            contentTableLayout.Controls.Add(lblCategory, 0, 3);
            contentTableLayout.Controls.Add(cbxCategory, 1, 3);
            contentTableLayout.Controls.Add(txtDescription, 1, 4);
            contentTableLayout.Controls.Add(lblStatus, 0, 5);
            contentTableLayout.Controls.Add(chkIsActive, 1, 5);
            contentTableLayout.Controls.Add(lblCreated, 0, 6);
            contentTableLayout.Controls.Add(txtCreated, 1, 6);
            contentTableLayout.Controls.Add(lblUpdated, 0, 7);
            contentTableLayout.Controls.Add(txtUpdated, 1, 7);
            contentTableLayout.Controls.Add(lblDescription, 0, 4);
            contentTableLayout.Location = new Point(20, 20);
            contentTableLayout.Name = "contentTableLayout";
            contentTableLayout.RowCount = 8;
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            contentTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            contentTableLayout.Size = new Size(414, 430);
            contentTableLayout.TabIndex = 0;
            // 
            // lblId
            // 
            lblId.Anchor = AnchorStyles.Left;
            lblId.AutoSize = true;
            lblId.ForeColor = Color.FromArgb(124, 141, 181);
            lblId.Location = new Point(3, 17);
            lblId.Name = "lblId";
            lblId.Size = new Size(21, 15);
            lblId.TabIndex = 0;
            lblId.Text = "ID:";
            lblId.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtId
            // 
            txtId.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtId.BackColor = Color.FromArgb(42, 45, 86);
            txtId.BorderStyle = BorderStyle.FixedSingle;
            txtId.Enabled = false;
            txtId.ForeColor = Color.White;
            txtId.Location = new Point(123, 13);
            txtId.Name = "txtId";
            txtId.ReadOnly = true;
            txtId.Size = new Size(288, 23);
            txtId.TabIndex = 1;
            // 
            // lblName
            // 
            lblName.Anchor = AnchorStyles.Left;
            lblName.AutoSize = true;
            lblName.ForeColor = Color.FromArgb(124, 141, 181);
            lblName.Location = new Point(3, 67);
            lblName.Name = "lblName";
            lblName.Size = new Size(94, 15);
            lblName.TabIndex = 2;
            lblName.Text = "Tên nguyên liệu:";
            lblName.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            txtName.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtName.BackColor = Color.FromArgb(42, 45, 86);
            txtName.BorderStyle = BorderStyle.FixedSingle;
            txtName.ForeColor = Color.White;
            txtName.Location = new Point(123, 63);
            txtName.Name = "txtName";
            txtName.Size = new Size(288, 23);
            txtName.TabIndex = 2;
            // 
            // lblUnit
            // 
            lblUnit.Anchor = AnchorStyles.Left;
            lblUnit.AutoSize = true;
            lblUnit.ForeColor = Color.FromArgb(124, 141, 181);
            lblUnit.Location = new Point(3, 117);
            lblUnit.Name = "lblUnit";
            lblUnit.Size = new Size(44, 15);
            lblUnit.TabIndex = 4;
            lblUnit.Text = "Đơn vị:";
            lblUnit.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtUnit
            // 
            txtUnit.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtUnit.BackColor = Color.FromArgb(42, 45, 86);
            txtUnit.BorderStyle = BorderStyle.FixedSingle;
            txtUnit.ForeColor = Color.White;
            txtUnit.Location = new Point(123, 113);
            txtUnit.Name = "txtUnit";
            txtUnit.Size = new Size(288, 23);
            txtUnit.TabIndex = 3;
            // 
            // lblCategory
            // 
            lblCategory.Anchor = AnchorStyles.Left;
            lblCategory.AutoSize = true;
            lblCategory.ForeColor = Color.FromArgb(124, 141, 181);
            lblCategory.Location = new Point(3, 167);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(65, 15);
            lblCategory.TabIndex = 6;
            lblCategory.Text = "Danh mục:";
            lblCategory.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // cbxCategory
            // 
            cbxCategory.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            cbxCategory.BackColor = Color.FromArgb(42, 45, 86);
            cbxCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            cbxCategory.FlatStyle = FlatStyle.Flat;
            cbxCategory.ForeColor = Color.White;
            cbxCategory.FormattingEnabled = true;
            cbxCategory.Location = new Point(123, 163);
            cbxCategory.Name = "cbxCategory";
            cbxCategory.Size = new Size(288, 23);
            cbxCategory.TabIndex = 4;
            // 
            // lblDescription
            // 
            lblDescription.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            lblDescription.AutoSize = true;
            lblDescription.ForeColor = Color.FromArgb(124, 141, 181);
            lblDescription.Location = new Point(3, 200);
            lblDescription.Name = "lblDescription";
            lblDescription.Size = new Size(41, 80);
            lblDescription.TabIndex = 8;
            lblDescription.Text = "Mô tả:";
            lblDescription.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtDescription
            // 
            txtDescription.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtDescription.BackColor = Color.FromArgb(42, 45, 86);
            txtDescription.BorderStyle = BorderStyle.FixedSingle;
            txtDescription.ForeColor = Color.White;
            txtDescription.Location = new Point(123, 213);
            txtDescription.Multiline = true;
            txtDescription.Name = "txtDescription";
            txtDescription.ScrollBars = ScrollBars.Vertical;
            txtDescription.Size = new Size(288, 54);
            txtDescription.TabIndex = 5;
            // 
            // lblStatus
            // 
            lblStatus.Anchor = AnchorStyles.Left;
            lblStatus.AutoSize = true;
            lblStatus.ForeColor = Color.FromArgb(124, 141, 181);
            lblStatus.Location = new Point(3, 297);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(63, 15);
            lblStatus.TabIndex = 10;
            lblStatus.Text = "Trạng thái:";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // chkIsActive
            // 
            chkIsActive.Anchor = AnchorStyles.Left;
            chkIsActive.AutoSize = true;
            chkIsActive.ForeColor = Color.White;
            chkIsActive.Location = new Point(123, 295);
            chkIsActive.Name = "chkIsActive";
            chkIsActive.Size = new Size(83, 19);
            chkIsActive.TabIndex = 6;
            chkIsActive.Text = "Hoạt động";
            chkIsActive.UseVisualStyleBackColor = true;
            // 
            // lblCreated
            // 
            lblCreated.Anchor = AnchorStyles.Left;
            lblCreated.AutoSize = true;
            lblCreated.ForeColor = Color.FromArgb(124, 141, 181);
            lblCreated.Location = new Point(3, 347);
            lblCreated.Name = "lblCreated";
            lblCreated.Size = new Size(58, 15);
            lblCreated.TabIndex = 12;
            lblCreated.Text = "Ngày tạo:";
            lblCreated.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCreated
            // 
            txtCreated.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtCreated.BackColor = Color.FromArgb(42, 45, 86);
            txtCreated.BorderStyle = BorderStyle.FixedSingle;
            txtCreated.Enabled = false;
            txtCreated.ForeColor = Color.White;
            txtCreated.Location = new Point(123, 343);
            txtCreated.Name = "txtCreated";
            txtCreated.ReadOnly = true;
            txtCreated.Size = new Size(288, 23);
            txtCreated.TabIndex = 7;
            // 
            // lblUpdated
            // 
            lblUpdated.Anchor = AnchorStyles.Left;
            lblUpdated.AutoSize = true;
            lblUpdated.ForeColor = Color.FromArgb(124, 141, 181);
            lblUpdated.Location = new Point(3, 397);
            lblUpdated.Name = "lblUpdated";
            lblUpdated.Size = new Size(58, 15);
            lblUpdated.TabIndex = 14;
            lblUpdated.Text = "Lưu:";
            lblUpdated.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtUpdated
            // 
            txtUpdated.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            txtUpdated.BackColor = Color.FromArgb(42, 45, 86);
            txtUpdated.BorderStyle = BorderStyle.FixedSingle;
            txtUpdated.Enabled = false;
            txtUpdated.ForeColor = Color.White;
            txtUpdated.Location = new Point(123, 393);
            txtUpdated.Name = "txtUpdated";
            txtUpdated.ReadOnly = true;
            txtUpdated.Size = new Size(288, 23);
            txtUpdated.TabIndex = 8;
            // 
            // buttonPanel
            // 
            buttonPanel.BackColor = Color.FromArgb(24, 28, 63);
            buttonPanel.Controls.Add(btnCancel);
            buttonPanel.Controls.Add(btnSave);
            buttonPanel.Dock = DockStyle.Fill;
            buttonPanel.Location = new Point(23, 499);
            buttonPanel.Name = "buttonPanel";
            buttonPanel.Size = new Size(454, 78);
            buttonPanel.TabIndex = 1;
            // 
            // btnCancel
            // 
            btnCancel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnCancel.BackColor = Color.FromArgb(42, 45, 86);
            btnCancel.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.ForeColor = Color.FromArgb(255, 224, 192);
            btnCancel.Location = new Point(350, 20);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(100, 35);
            btnCancel.TabIndex = 10;
            btnCancel.Text = "Hủy";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // btnSave
            // 
            btnSave.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnSave.BackColor = Color.FromArgb(42, 45, 86);
            btnSave.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.ForeColor = Color.FromArgb(192, 255, 192);
            btnSave.Location = new Point(240, 20);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(100, 35);
            btnSave.TabIndex = 9;
            btnSave.Text = "Lưu";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // errorProvider
            // 
            errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
            errorProvider.ContainerControl = this;
            // 
            // FrmIngredientDetails
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(500, 600);
            Controls.Add(mainPanel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FrmIngredientDetails";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Chi tiết nguyên liệu";
            mainPanel.ResumeLayout(false);
            contentPanel.ResumeLayout(false);
            contentTableLayout.ResumeLayout(false);
            contentTableLayout.PerformLayout();
            buttonPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)errorProvider).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel mainPanel;
        private System.Windows.Forms.Panel contentPanel;
        private System.Windows.Forms.TableLayoutPanel contentTableLayout;
        private System.Windows.Forms.Label lblId;
        private System.Windows.Forms.TextBox txtId;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblUnit;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.ComboBox cbxCategory;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.CheckBox chkIsActive;
        private System.Windows.Forms.Label lblCreated;
        private System.Windows.Forms.TextBox txtCreated;
        private System.Windows.Forms.Label lblUpdated;
        private System.Windows.Forms.TextBox txtUpdated;
        private System.Windows.Forms.Panel buttonPanel;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private ErrorProvider errorProvider;
    }
}