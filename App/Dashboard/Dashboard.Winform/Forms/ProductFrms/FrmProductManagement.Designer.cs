namespace Dashboard.Winform.Forms
{
    partial class FrmProductManagement
    {
        private TabControl tabControl;
        private TabPage tabProducts;
        private TabPage tabRecipes;
        private DataGridView dgvProducts;
        private DataGridView dgvRecipes;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeDgvListItem()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();

            tabControl = new TabControl();
            tabProducts = new TabPage();
            tabRecipes = new TabPage();
            dgvProducts = new DataGridView();
            dgvRecipes = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvProducts).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dgvRecipes).BeginInit();
            tabControl.SuspendLayout();
            tabProducts.SuspendLayout();
            tabRecipes.SuspendLayout();
            SuspendLayout();

            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabProducts);
            tabControl.Controls.Add(tabRecipes);
            tabControl.Dock = DockStyle.Fill;
            tabControl.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabControl.Font = new Font("Segoe UI", 9F);
            tabControl.ItemSize = new Size(120, 30);
            tabControl.Location = new Point(0, 0);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(954, 520);
            tabControl.SizeMode = TabSizeMode.Fixed;
            tabControl.TabIndex = 0;

            // 
            // tabProducts
            // 
            tabProducts.BackColor = Color.FromArgb(42, 45, 86);
            tabProducts.Controls.Add(dgvProducts);
            tabProducts.Location = new Point(4, 34);
            tabProducts.Name = "tabProducts";
            tabProducts.Padding = new Padding(3);
            tabProducts.Size = new Size(946, 482);
            tabProducts.TabIndex = 0;
            tabProducts.Text = "Sản phẩm";

            // 
            // tabRecipes
            // 
            tabRecipes.BackColor = Color.FromArgb(42, 45, 86);
            tabRecipes.Controls.Add(dgvRecipes);
            tabRecipes.Location = new Point(4, 34);
            tabRecipes.Name = "tabRecipes";
            tabRecipes.Padding = new Padding(3);
            tabRecipes.Size = new Size(946, 482);
            tabRecipes.TabIndex = 1;
            tabRecipes.Text = "Công thức";

            // 
            // dgvProducts
            // 
            dgvProducts.AllowUserToAddRows = false;
            dgvProducts.AllowUserToDeleteRows = false;
            dgvProducts.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(42, 45, 86);
            dgvProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvProducts.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvProducts.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvProducts.BorderStyle = BorderStyle.None;
            dgvProducts.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvProducts.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvProducts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvProducts.DefaultCellStyle = dataGridViewCellStyle3;
            dgvProducts.Dock = DockStyle.Fill;
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.GridColor = Color.FromArgb(73, 75, 111);
            dgvProducts.Location = new Point(3, 3);
            dgvProducts.MultiSelect = false;
            dgvProducts.Name = "dgvProducts";
            dgvProducts.ReadOnly = true;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProducts.Size = new Size(940, 476);
            dgvProducts.TabIndex = 0;

            // 
            // dgvRecipes
            // 
            dgvRecipes.AllowUserToAddRows = false;
            dgvRecipes.AllowUserToDeleteRows = false;
            dgvRecipes.AllowUserToResizeRows = false;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(42, 45, 86);
            dgvRecipes.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            dgvRecipes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRecipes.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvRecipes.BorderStyle = BorderStyle.None;
            dgvRecipes.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvRecipes.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.True;
            dgvRecipes.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            dgvRecipes.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = Color.White;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.False;
            dgvRecipes.DefaultCellStyle = dataGridViewCellStyle6;
            dgvRecipes.Dock = DockStyle.Fill;
            dgvRecipes.EnableHeadersVisualStyles = false;
            dgvRecipes.GridColor = Color.FromArgb(73, 75, 111);
            dgvRecipes.Location = new Point(3, 3);
            dgvRecipes.MultiSelect = false;
            dgvRecipes.Name = "dgvRecipes";
            dgvRecipes.ReadOnly = true;
            dgvRecipes.RowHeadersVisible = false;
            dgvRecipes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRecipes.Size = new Size(940, 476);
            dgvRecipes.TabIndex = 0;

            pnlContent.Controls.Add(tabControl);

            ((System.ComponentModel.ISupportInitialize)dgvProducts).EndInit();
            ((System.ComponentModel.ISupportInitialize)dgvRecipes).EndInit();
            tabProducts.ResumeLayout(false);
            tabRecipes.ResumeLayout(false);
            tabControl.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
