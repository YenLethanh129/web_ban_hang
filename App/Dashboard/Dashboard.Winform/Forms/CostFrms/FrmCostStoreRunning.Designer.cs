namespace Dashboard.Winform.Forms.CostFrms
{
    partial class FrmCostStoreRunning
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = new System.ComponentModel.Container();

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                components?.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmCostStoreRunning));
            pnListBranch = new Panel();
            fpnListBranch = new FlowLayoutPanel();
            btnTotalBranch = new Button();
            pnTitle = new Panel();
            txtTitle = new Button();
            pnNameBrach = new Panel();
            txtNameBranch = new Button();
            pnOptionTime = new Panel();
            btnOptionTime = new Button();
            btnOption1Y = new Button();
            btnOption3M = new Button();
            btnOption1M = new Button();
            pnListCost = new Panel();
            fpnListCost = new FlowLayoutPanel();
            pnTotalMoney = new Panel();
            button6 = new Button();
            button5 = new Button();
            pnAddCost = new Panel();
            pnListBranch.SuspendLayout();
            fpnListBranch.SuspendLayout();
            pnTitle.SuspendLayout();
            pnNameBrach.SuspendLayout();
            pnOptionTime.SuspendLayout();
            pnListCost.SuspendLayout();
            pnTotalMoney.SuspendLayout();
            SuspendLayout();
            // 
            // pnListBranch
            // 
            pnListBranch.Controls.Add(fpnListBranch);
            pnListBranch.Controls.Add(pnTitle);
            pnListBranch.Dock = DockStyle.Left;
            pnListBranch.Location = new Point(0, 0);
            pnListBranch.Margin = new Padding(0);
            pnListBranch.Name = "pnListBranch";
            pnListBranch.Size = new Size(250, 700);
            pnListBranch.TabIndex = 0;
            // 
            // fpnListBranch
            // 
            fpnListBranch.BackColor = Color.FromArgb(110, 140, 251);
            fpnListBranch.Controls.Add(btnTotalBranch);
            fpnListBranch.Dock = DockStyle.Fill;
            fpnListBranch.Location = new Point(0, 100);
            fpnListBranch.Margin = new Padding(0);
            fpnListBranch.Name = "fpnListBranch";
            fpnListBranch.Size = new Size(250, 600);
            fpnListBranch.TabIndex = 1;
            // 
            // btnTotalBranch
            // 
            btnTotalBranch.BackColor = Color.FromArgb(99, 108, 203);
            btnTotalBranch.Dock = DockStyle.Top;
            btnTotalBranch.FlatAppearance.BorderSize = 0;
            btnTotalBranch.FlatStyle = FlatStyle.Flat;
            btnTotalBranch.Font = new Font("Microsoft Sans Serif", 11F);
            btnTotalBranch.ForeColor = Color.WhiteSmoke;
            btnTotalBranch.Image = (Image)resources.GetObject("btnTotalBranch.Image");
            btnTotalBranch.ImageAlign = ContentAlignment.MiddleLeft;
            btnTotalBranch.Location = new Point(0, 0);
            btnTotalBranch.Margin = new Padding(0);
            btnTotalBranch.Name = "btnTotalBranch";
            btnTotalBranch.Padding = new Padding(10, 0, 0, 0);
            btnTotalBranch.Size = new Size(250, 50);
            btnTotalBranch.TabIndex = 0;
            btnTotalBranch.Text = "   Tổng Chi Nhánh";
            btnTotalBranch.TextAlign = ContentAlignment.MiddleRight;
            btnTotalBranch.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnTotalBranch.UseVisualStyleBackColor = false;
            // 
            // pnTitle
            // 
            pnTitle.Controls.Add(txtTitle);
            pnTitle.Dock = DockStyle.Top;
            pnTitle.Location = new Point(0, 0);
            pnTitle.Margin = new Padding(0);
            pnTitle.Name = "pnTitle";
            pnTitle.Size = new Size(250, 100);
            pnTitle.TabIndex = 0;
            // 
            // txtTitle
            // 
            txtTitle.BackColor = Color.FromArgb(110, 140, 251);
            txtTitle.Dock = DockStyle.Fill;
            txtTitle.Enabled = false;
            txtTitle.FlatAppearance.BorderSize = 0;
            txtTitle.FlatStyle = FlatStyle.Flat;
            txtTitle.Font = new Font("Microsoft Sans Serif", 14F);
            txtTitle.ForeColor = Color.WhiteSmoke;
            txtTitle.Location = new Point(0, 0);
            txtTitle.Margin = new Padding(0);
            txtTitle.Name = "txtTitle";
            txtTitle.Size = new Size(250, 100);
            txtTitle.TabIndex = 0;
            txtTitle.Text = "CHI NHÁNH";
            txtTitle.UseVisualStyleBackColor = false;
            // 
            // pnNameBrach
            // 
            pnNameBrach.Controls.Add(txtNameBranch);
            pnNameBrach.Dock = DockStyle.Top;
            pnNameBrach.Location = new Point(250, 0);
            pnNameBrach.Margin = new Padding(0);
            pnNameBrach.Name = "pnNameBrach";
            pnNameBrach.Size = new Size(650, 100);
            pnNameBrach.TabIndex = 1;
            // 
            // txtNameBranch
            // 
            txtNameBranch.BackColor = Color.FromArgb(80, 88, 156);
            txtNameBranch.Dock = DockStyle.Fill;
            txtNameBranch.Enabled = false;
            txtNameBranch.FlatAppearance.BorderSize = 0;
            txtNameBranch.FlatStyle = FlatStyle.Flat;
            txtNameBranch.Font = new Font("Microsoft Sans Serif", 14F);
            txtNameBranch.ForeColor = Color.WhiteSmoke;
            txtNameBranch.Location = new Point(0, 0);
            txtNameBranch.Margin = new Padding(0);
            txtNameBranch.Name = "txtNameBranch";
            txtNameBranch.Size = new Size(650, 100);
            txtNameBranch.TabIndex = 1;
            txtNameBranch.Text = "NAME_OF_BRANCH";
            txtNameBranch.UseVisualStyleBackColor = false;
            // 
            // pnOptionTime
            // 
            pnOptionTime.Controls.Add(btnOptionTime);
            pnOptionTime.Controls.Add(btnOption1Y);
            pnOptionTime.Controls.Add(btnOption3M);
            pnOptionTime.Controls.Add(btnOption1M);
            pnOptionTime.Dock = DockStyle.Top;
            pnOptionTime.Location = new Point(250, 100);
            pnOptionTime.Margin = new Padding(0);
            pnOptionTime.Name = "pnOptionTime";
            pnOptionTime.Size = new Size(650, 50);
            pnOptionTime.TabIndex = 2;
            // 
            // btnOptionTime
            // 
            btnOptionTime.BackColor = Color.FromArgb(79, 183, 179);
            btnOptionTime.Dock = DockStyle.Fill;
            btnOptionTime.FlatAppearance.BorderSize = 0;
            btnOptionTime.FlatStyle = FlatStyle.Flat;
            btnOptionTime.Font = new Font("Microsoft Sans Serif", 11F);
            btnOptionTime.ForeColor = Color.WhiteSmoke;
            btnOptionTime.Location = new Point(300, 0);
            btnOptionTime.Name = "btnOptionTime";
            btnOptionTime.Size = new Size(350, 50);
            btnOptionTime.TabIndex = 0;
            btnOptionTime.Text = "Tùy Chỉnh";
            btnOptionTime.UseVisualStyleBackColor = false;
            // 
            // btnOption1Y
            // 
            btnOption1Y.BackColor = Color.FromArgb(215, 143, 238);
            btnOption1Y.Dock = DockStyle.Left;
            btnOption1Y.FlatAppearance.BorderSize = 0;
            btnOption1Y.FlatStyle = FlatStyle.Flat;
            btnOption1Y.Font = new Font("Microsoft Sans Serif", 11F);
            btnOption1Y.ForeColor = Color.WhiteSmoke;
            btnOption1Y.Location = new Point(200, 0);
            btnOption1Y.Name = "btnOption1Y";
            btnOption1Y.Size = new Size(100, 50);
            btnOption1Y.TabIndex = 0;
            btnOption1Y.Text = "1 Năm";
            btnOption1Y.UseVisualStyleBackColor = false;
            // 
            // btnOption3M
            // 
            btnOption3M.BackColor = Color.FromArgb(145, 173, 200);
            btnOption3M.Dock = DockStyle.Left;
            btnOption3M.FlatAppearance.BorderSize = 0;
            btnOption3M.FlatStyle = FlatStyle.Flat;
            btnOption3M.Font = new Font("Microsoft Sans Serif", 11F);
            btnOption3M.ForeColor = Color.WhiteSmoke;
            btnOption3M.Location = new Point(100, 0);
            btnOption3M.Name = "btnOption3M";
            btnOption3M.Size = new Size(100, 50);
            btnOption3M.TabIndex = 0;
            btnOption3M.Text = "3 Tháng";
            btnOption3M.UseVisualStyleBackColor = false;
            // 
            // btnOption1M
            // 
            btnOption1M.BackColor = Color.FromArgb(100, 127, 188);
            btnOption1M.Dock = DockStyle.Left;
            btnOption1M.FlatAppearance.BorderSize = 0;
            btnOption1M.FlatStyle = FlatStyle.Flat;
            btnOption1M.Font = new Font("Microsoft Sans Serif", 11F);
            btnOption1M.ForeColor = Color.WhiteSmoke;
            btnOption1M.Location = new Point(0, 0);
            btnOption1M.Name = "btnOption1M";
            btnOption1M.Size = new Size(100, 50);
            btnOption1M.TabIndex = 0;
            btnOption1M.Text = "1 Tháng";
            btnOption1M.UseVisualStyleBackColor = false;
            // 
            // pnListCost
            // 
            pnListCost.BackColor = Color.FromArgb(100, 127, 188);
            pnListCost.Controls.Add(fpnListCost);
            pnListCost.Controls.Add(pnTotalMoney);
            pnListCost.Dock = DockStyle.Fill;
            pnListCost.Location = new Point(250, 150);
            pnListCost.Margin = new Padding(0);
            pnListCost.Name = "pnListCost";
            pnListCost.Size = new Size(650, 550);
            pnListCost.TabIndex = 3;
            // 
            // fpnListCost
            // 
            fpnListCost.Dock = DockStyle.Fill;
            fpnListCost.Location = new Point(0, 50);
            fpnListCost.Name = "fpnListCost";
            fpnListCost.Size = new Size(650, 500);
            fpnListCost.TabIndex = 1;
            // 
            // pnTotalMoney
            // 
            pnTotalMoney.Controls.Add(button6);
            pnTotalMoney.Controls.Add(button5);
            pnTotalMoney.Dock = DockStyle.Top;
            pnTotalMoney.Location = new Point(0, 0);
            pnTotalMoney.Name = "pnTotalMoney";
            pnTotalMoney.Size = new Size(650, 50);
            pnTotalMoney.TabIndex = 0;
            // 
            // button6
            // 
            button6.Anchor = AnchorStyles.Bottom;
            button6.BackColor = Color.White;
            button6.FlatAppearance.BorderSize = 0;
            button6.FlatStyle = FlatStyle.Flat;
            button6.Font = new Font("Microsoft Sans Serif", 11F);
            button6.Location = new Point(216, 6);
            button6.Margin = new Padding(0);
            button6.Name = "button6";
            button6.Size = new Size(215, 44);
            button6.TabIndex = 0;
            button6.Text = "0";
            button6.TextAlign = ContentAlignment.MiddleRight;
            button6.UseVisualStyleBackColor = false;
            // 
            // button5
            // 
            button5.Anchor = AnchorStyles.Bottom;
            button5.BackColor = Color.White;
            button5.FlatAppearance.BorderSize = 0;
            button5.FlatStyle = FlatStyle.Flat;
            button5.Font = new Font("Microsoft Sans Serif", 11F);
            button5.Location = new Point(88, 6);
            button5.Margin = new Padding(0);
            button5.Name = "button5";
            button5.Size = new Size(128, 44);
            button5.TabIndex = 0;
            button5.Text = "Tổng Tiền:";
            button5.TextAlign = ContentAlignment.MiddleLeft;
            button5.UseVisualStyleBackColor = false;
            // 
            // pnAddCost
            // 
            pnAddCost.Dock = DockStyle.Bottom;
            pnAddCost.Location = new Point(250, 450);
            pnAddCost.Margin = new Padding(0);
            pnAddCost.Name = "pnAddCost";
            pnAddCost.Size = new Size(650, 250);
            pnAddCost.TabIndex = 4;
            // 
            // FrmCostStoreRunning
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(900, 700);
            Controls.Add(pnAddCost);
            Controls.Add(pnListCost);
            Controls.Add(pnOptionTime);
            Controls.Add(pnNameBrach);
            Controls.Add(pnListBranch);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FrmCostStoreRunning";
            Text = "FrmCostStoreRunning";
            pnListBranch.ResumeLayout(false);
            fpnListBranch.ResumeLayout(false);
            pnTitle.ResumeLayout(false);
            pnNameBrach.ResumeLayout(false);
            pnOptionTime.ResumeLayout(false);
            pnListCost.ResumeLayout(false);
            pnTotalMoney.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel pnListBranch;
        private Panel pnNameBrach;
        private Panel pnOptionTime;
        private Panel pnListCost;
        private Panel pnAddCost;
        private Panel pnTitle;
        private Button txtTitle;
        private Button txtNameBranch;
        private FlowLayoutPanel fpnListBranch;
        private Button btnTotalBranch;
        private Button btnOption1M;
        private Button btnOptionTime;
        private Button btnOption1Y;
        private Button btnOption3M;
        private Panel pnTotalMoney;
        private Button button5;
        private FlowLayoutPanel fpnListCost;
        private Button button6;
    }
}