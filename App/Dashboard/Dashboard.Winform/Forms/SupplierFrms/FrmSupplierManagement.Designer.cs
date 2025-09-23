using Microsoft.AspNetCore.Http.HttpResults;

namespace Dashboard.Winform.Forms.SupplierFrm;

partial class FrmSupplierManagement
{
    private DataGridView dgvSuppliers;

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

        dgvSuppliers = new DataGridView();

        ((System.ComponentModel.ISupportInitialize)dgvSuppliers).BeginInit();
        SuspendLayout();

        // 
        // dgvSuppliers
        // 
        dgvSuppliers.AllowUserToAddRows = false;
        dgvSuppliers.AllowUserToDeleteRows = false;
        dgvSuppliers.AllowUserToResizeRows = false;
        dataGridViewCellStyle1.BackColor = Color.FromArgb(42, 45, 86);
        dgvSuppliers.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
        dgvSuppliers.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dgvSuppliers.BackgroundColor = Color.FromArgb(42, 45, 86);
        dgvSuppliers.BorderStyle = BorderStyle.None;
        dgvSuppliers.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
        dgvSuppliers.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
        dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle2.BackColor = Color.FromArgb(42, 45, 86);
        dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle2.ForeColor = Color.FromArgb(124, 141, 181);
        dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(42, 45, 86);
        dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(124, 141, 181);
        dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
        dgvSuppliers.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
        dgvSuppliers.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
        dataGridViewCellStyle3.BackColor = Color.FromArgb(42, 45, 86);
        dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
        dataGridViewCellStyle3.ForeColor = Color.White;
        dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
        dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
        dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
        dgvSuppliers.DefaultCellStyle = dataGridViewCellStyle3;
        dgvSuppliers.Dock = DockStyle.Fill;
        dgvSuppliers.EnableHeadersVisualStyles = false;
        dgvSuppliers.GridColor = Color.FromArgb(73, 75, 111);
        dgvSuppliers.Location = new Point(0, 0);
        dgvSuppliers.MultiSelect = false;
        dgvSuppliers.Name = "dgvSuppliers";
        dgvSuppliers.ReadOnly = true;
        dgvSuppliers.RowHeadersVisible = false;
        dgvSuppliers.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        dgvSuppliers.Size = new Size(954, 520);
        dgvSuppliers.TabIndex = 0;

        pnlContent.Controls.Add(dgvSuppliers);

        ((System.ComponentModel.ISupportInitialize)dgvSuppliers).EndInit();
        ResumeLayout(false);
    }
}