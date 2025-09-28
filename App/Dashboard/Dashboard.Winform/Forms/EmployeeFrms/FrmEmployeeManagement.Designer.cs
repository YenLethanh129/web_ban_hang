namespace Dashboard.Winform.Forms
{
    partial class FrmEmployeeManagement
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        //private System.ComponentModel.IContainer components = null;

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

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeDgvListItem()
        {
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();

            dgvListItems = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)dgvListItems).BeginInit();
            SuspendLayout();

            // 
            // dgvListItems
            // 
            dgvListItems.AllowUserToAddRows = false;
            dgvListItems.AllowUserToDeleteRows = false;
            dgvListItems.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = Color.FromArgb(42, 45, 86);
            dgvListItems.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dgvListItems.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvListItems.BackgroundColor = Color.FromArgb(42, 45, 86);
            dgvListItems.BorderStyle = BorderStyle.None;
            dgvListItems.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvListItems.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.SelectionBackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle2.SelectionForeColor = Color.FromArgb(124, 141, 181);
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.True;
            dgvListItems.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            dgvListItems.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = Color.FromArgb(42, 45, 86);
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = Color.White;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.False;
            dgvListItems.DefaultCellStyle = dataGridViewCellStyle3;
            dgvListItems.Dock = DockStyle.Fill;
            dgvListItems.EnableHeadersVisualStyles = false;
            dgvListItems.GridColor = Color.FromArgb(73, 75, 111);
            dgvListItems.Location = new Point(0, 0);
            dgvListItems.MultiSelect = false;
            dgvListItems.Name = "dgvListItems";
            dgvListItems.ReadOnly = true;
            dgvListItems.RowHeadersVisible = false;
            dgvListItems.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvListItems.Size = new Size(954, 520);
            dgvListItems.TabIndex = 4;

            pnlContent.Controls.Add(dgvListItems);

            ((System.ComponentModel.ISupportInitialize)dgvListItems).EndInit();
            ResumeLayout(false);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        //private void InitializeComponent()
        //{
        //    this.components = new System.ComponentModel.Container();
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.ClientSize = new System.Drawing.Size(800, 450);
        //    this.Text = "FrmEmployeeManagement";
        //}

        #endregion

        protected DataGridView dgvListItems;
    }
}