using System.Windows.Forms.DataVisualization.Charting;

namespace Dashboard.Winform
{
    partial class MainDashboardForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();
            Series series1 = new Series();
            Title title1 = new Title();
            ChartArea chartArea2 = new ChartArea();
            Legend legend2 = new Legend();
            Series series2 = new Series();
            Title title2 = new Title();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            label1 = new Label();
            dtpStart = new DateTimePicker();
            dtpEnd = new DateTimePicker();
            btnThisMonth = new Button();
            btnLast30Days = new Button();
            Last7Days = new Button();
            btnToday = new Button();
            btnCustomDate = new Button();
            btnOkeCustomDate = new Button();
            panel1 = new Panel();
            lblNumberOfOrders = new Label();
            label2 = new Label();
            panel2 = new Panel();
            label3 = new Label();
            lblTotalOfProfit = new Label();
            panel3 = new Panel();
            label5 = new Label();
            lblTotalOfRevenue = new Label();
            chart1 = new Chart();
            chart2 = new Chart();
            panel4 = new Panel();
            lblNumberOfProducts = new Label();
            lblNumberOfSuppliers = new Label();
            label11 = new Label();
            label9 = new Label();
            label7 = new Label();
            lblNumberOfCustomers = new Label();
            label6 = new Label();
            panel5 = new Panel();
            dgvUnderstock = new DataGridView();
            label13 = new Label();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chart1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)chart2).BeginInit();
            panel4.SuspendLayout();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnderstock).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 17.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label1.ForeColor = Color.WhiteSmoke;
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(126, 31);
            label1.TabIndex = 0;
            label1.Text = "Dashboard";
            // 
            // dtpStart
            // 
            dtpStart.CustomFormat = "dd/MM/yyyy";
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.Location = new Point(164, 17);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(87, 23);
            dtpStart.TabIndex = 1;
            // 
            // dtpEnd
            // 
            dtpEnd.CustomFormat = "dd/MM/yyyy";
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.Location = new Point(269, 17);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(87, 23);
            dtpEnd.TabIndex = 2;
            // 
            // btnThisMonth
            // 
            btnThisMonth.Location = new Point(854, 14);
            btnThisMonth.Margin = new Padding(3, 2, 3, 2);
            btnThisMonth.Name = "btnThisMonth";
            btnThisMonth.Size = new Size(105, 30);
            btnThisMonth.TabIndex = 3;
            btnThisMonth.Text = "Tháng này";
            btnThisMonth.UseVisualStyleBackColor = true;
            // 
            // btnLast30Days
            // 
            btnLast30Days.Location = new Point(744, 14);
            btnLast30Days.Margin = new Padding(3, 2, 3, 2);
            btnLast30Days.Name = "btnLast30Days";
            btnLast30Days.Size = new Size(105, 30);
            btnLast30Days.TabIndex = 4;
            btnLast30Days.Text = "30 ngày qua";
            btnLast30Days.UseVisualStyleBackColor = true;
            // 
            // Last7Days
            // 
            Last7Days.Location = new Point(634, 14);
            Last7Days.Margin = new Padding(3, 2, 3, 2);
            Last7Days.Name = "Last7Days";
            Last7Days.Size = new Size(105, 30);
            Last7Days.TabIndex = 5;
            Last7Days.Text = "7 ngày qua";
            Last7Days.UseVisualStyleBackColor = true;
            // 
            // btnToday
            // 
            btnToday.Location = new Point(524, 14);
            btnToday.Margin = new Padding(3, 2, 3, 2);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(105, 30);
            btnToday.TabIndex = 6;
            btnToday.Text = "Hôm nay";
            btnToday.UseVisualStyleBackColor = true;
            // 
            // btnCustomDate
            // 
            btnCustomDate.Location = new Point(413, 14);
            btnCustomDate.Margin = new Padding(3, 2, 3, 2);
            btnCustomDate.Name = "btnCustomDate";
            btnCustomDate.Size = new Size(105, 30);
            btnCustomDate.TabIndex = 7;
            btnCustomDate.Text = "Tùy chọn";
            btnCustomDate.UseVisualStyleBackColor = true;
            // 
            // btnOkeCustomDate
            // 
            btnOkeCustomDate.Location = new Point(373, 14);
            btnOkeCustomDate.Margin = new Padding(3, 2, 3, 2);
            btnOkeCustomDate.Name = "btnOkeCustomDate";
            btnOkeCustomDate.Size = new Size(35, 30);
            btnOkeCustomDate.TabIndex = 8;
            btnOkeCustomDate.Text = "Oke";
            btnOkeCustomDate.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(42, 45, 86);
            panel1.Controls.Add(lblNumberOfOrders);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(12, 57);
            panel1.Margin = new Padding(3, 2, 3, 2);
            panel1.Name = "panel1";
            panel1.Size = new Size(179, 51);
            panel1.TabIndex = 10;
            // 
            // lblNumberOfOrders
            // 
            lblNumberOfOrders.AutoSize = true;
            lblNumberOfOrders.Font = new Font("Segoe UI", 13F);
            lblNumberOfOrders.ForeColor = Color.WhiteSmoke;
            lblNumberOfOrders.Location = new Point(15, 17);
            lblNumberOfOrders.Name = "lblNumberOfOrders";
            lblNumberOfOrders.Size = new Size(52, 25);
            lblNumberOfOrders.TabIndex = 1;
            lblNumberOfOrders.Text = "1000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.WhiteSmoke;
            label2.Location = new Point(15, 0);
            label2.Name = "label2";
            label2.Size = new Size(83, 17);
            label2.TabIndex = 0;
            label2.Text = "Số đơn hàng";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(42, 45, 86);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(lblTotalOfProfit);
            panel2.Location = new Point(516, 57);
            panel2.Margin = new Padding(3, 2, 3, 2);
            panel2.Name = "panel2";
            panel2.Size = new Size(443, 51);
            panel2.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13F);
            label3.ForeColor = Color.WhiteSmoke;
            label3.Location = new Point(29, 17);
            label3.Name = "label3";
            label3.Size = new Size(52, 25);
            label3.TabIndex = 1;
            label3.Text = "1000";
            // 
            // lblTotalOfProfit
            // 
            lblTotalOfProfit.AutoSize = true;
            lblTotalOfProfit.Font = new Font("Segoe UI", 10F);
            lblTotalOfProfit.ForeColor = Color.WhiteSmoke;
            lblTotalOfProfit.Location = new Point(29, 0);
            lblTotalOfProfit.Name = "lblTotalOfProfit";
            lblTotalOfProfit.Size = new Size(101, 19);
            lblTotalOfProfit.TabIndex = 0;
            lblTotalOfProfit.Text = "Tổng lợi nhuận";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(42, 45, 86);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(lblTotalOfRevenue);
            panel3.Location = new Point(197, 57);
            panel3.Margin = new Padding(3, 2, 3, 2);
            panel3.Name = "panel3";
            panel3.Size = new Size(313, 51);
            panel3.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.ForeColor = Color.WhiteSmoke;
            label5.Location = new Point(29, 0);
            label5.Name = "label5";
            label5.Size = new Size(108, 19);
            label5.TabIndex = 0;
            label5.Text = "Tổng doanh thu";
            // 
            // lblTotalOfRevenue
            // 
            lblTotalOfRevenue.AutoSize = true;
            lblTotalOfRevenue.Font = new Font("Segoe UI", 13F);
            lblTotalOfRevenue.ForeColor = Color.WhiteSmoke;
            lblTotalOfRevenue.Location = new Point(29, 19);
            lblTotalOfRevenue.Name = "lblTotalOfRevenue";
            lblTotalOfRevenue.Size = new Size(52, 25);
            lblTotalOfRevenue.TabIndex = 1;
            lblTotalOfRevenue.Text = "1000";
            // 
            // chart1
            // 
            chart1.BackColor = Color.FromArgb(42, 45, 86);
            chartArea1.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea1);
            legend1.Docking = Docking.Top;
            legend1.Name = "Legend1";
            chart1.Legends.Add(legend1);
            chart1.Location = new Point(12, 113);
            chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chart1.Series.Add(series1);
            chart1.Size = new Size(597, 252);
            chart1.TabIndex = 12;
            chart1.Text = "chart1";
            title1.Alignment = ContentAlignment.TopLeft;
            title1.Font = new Font("Microsoft Sans Serif", 12F);
            title1.ForeColor = Color.WhiteSmoke;
            title1.Name = "Title1";
            title1.Text = "Tổng Doanh Thu";
            chart1.Titles.Add(title1);
            // 
            // chart2
            // 
            chart2.BackColor = Color.FromArgb(42, 45, 86);
            chartArea2.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea2);
            legend2.Docking = Docking.Bottom;
            legend2.Name = "Legend1";
            chart2.Legends.Add(legend2);
            chart2.Location = new Point(615, 113);
            chart2.Name = "chart2";
            series2.ChartArea = "ChartArea1";
            series2.ChartType = SeriesChartType.Doughnut;
            series2.Font = new Font("Microsoft Sans Serif", 12F);
            series2.IsValueShownAsLabel = true;
            series2.LabelForeColor = Color.White;
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chart2.Series.Add(series2);
            chart2.Size = new Size(344, 436);
            chart2.TabIndex = 13;
            chart2.Text = "chart2";
            title2.Alignment = ContentAlignment.TopLeft;
            title2.Font = new Font("Microsoft Sans Serif", 12F);
            title2.ForeColor = Color.WhiteSmoke;
            title2.Name = "Title1";
            title2.Text = "Top 5 Sản Phẩm Bán Chạy";
            chart2.Titles.Add(title2);
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(42, 45, 86);
            panel4.Controls.Add(lblNumberOfProducts);
            panel4.Controls.Add(lblNumberOfSuppliers);
            panel4.Controls.Add(label11);
            panel4.Controls.Add(label9);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(lblNumberOfCustomers);
            panel4.Controls.Add(label6);
            panel4.Location = new Point(12, 370);
            panel4.Margin = new Padding(3, 2, 3, 2);
            panel4.Name = "panel4";
            panel4.Size = new Size(170, 179);
            panel4.TabIndex = 11;
            // 
            // lblNumberOfProducts
            // 
            lblNumberOfProducts.AutoSize = true;
            lblNumberOfProducts.Font = new Font("Segoe UI", 13F);
            lblNumberOfProducts.ForeColor = Color.WhiteSmoke;
            lblNumberOfProducts.Location = new Point(15, 144);
            lblNumberOfProducts.Name = "lblNumberOfProducts";
            lblNumberOfProducts.Size = new Size(52, 25);
            lblNumberOfProducts.TabIndex = 15;
            lblNumberOfProducts.Text = "1000";
            // 
            // lblNumberOfSuppliers
            // 
            lblNumberOfSuppliers.AutoSize = true;
            lblNumberOfSuppliers.Font = new Font("Segoe UI", 13F);
            lblNumberOfSuppliers.ForeColor = Color.WhiteSmoke;
            lblNumberOfSuppliers.Location = new Point(15, 93);
            lblNumberOfSuppliers.Name = "lblNumberOfSuppliers";
            lblNumberOfSuppliers.Size = new Size(52, 25);
            lblNumberOfSuppliers.TabIndex = 4;
            lblNumberOfSuppliers.Text = "1000";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.ForeColor = Color.WhiteSmoke;
            label11.Location = new Point(15, 127);
            label11.Name = "label11";
            label11.Size = new Size(66, 17);
            label11.TabIndex = 14;
            label11.Text = "Sản phẩm";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.WhiteSmoke;
            label9.Location = new Point(15, 76);
            label9.Name = "label9";
            label9.Size = new Size(89, 17);
            label9.TabIndex = 3;
            label9.Text = "Nhà cung cấp";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 12F);
            label7.ForeColor = Color.WhiteSmoke;
            label7.Location = new Point(3, 0);
            label7.Name = "label7";
            label7.Size = new Size(78, 21);
            label7.TabIndex = 2;
            label7.Text = "Doanh Số";
            // 
            // lblNumberOfCustomers
            // 
            lblNumberOfCustomers.AutoSize = true;
            lblNumberOfCustomers.Font = new Font("Segoe UI", 13F);
            lblNumberOfCustomers.ForeColor = Color.WhiteSmoke;
            lblNumberOfCustomers.Location = new Point(15, 42);
            lblNumberOfCustomers.Name = "lblNumberOfCustomers";
            lblNumberOfCustomers.Size = new Size(52, 25);
            lblNumberOfCustomers.TabIndex = 1;
            lblNumberOfCustomers.Text = "1000";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.WhiteSmoke;
            label6.Location = new Point(15, 25);
            label6.Name = "label6";
            label6.Size = new Size(76, 17);
            label6.TabIndex = 0;
            label6.Text = "Khách hàng";
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(42, 45, 86);
            panel5.Controls.Add(dgvUnderstock);
            panel5.Controls.Add(label13);
            panel5.Location = new Point(188, 370);
            panel5.Margin = new Padding(3, 2, 3, 2);
            panel5.Name = "panel5";
            panel5.Size = new Size(421, 179);
            panel5.TabIndex = 16;
            // 
            // dgvUnderstock
            // 
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvUnderstock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvUnderstock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvUnderstock.DefaultCellStyle = dataGridViewCellStyle2;
            dgvUnderstock.Location = new Point(9, 25);
            dgvUnderstock.Name = "dgvUnderstock";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvUnderstock.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvUnderstock.Size = new Size(400, 150);
            dgvUnderstock.TabIndex = 3;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 12F);
            label13.ForeColor = Color.WhiteSmoke;
            label13.Location = new Point(3, 0);
            label13.Name = "label13";
            label13.Size = new Size(67, 21);
            label13.TabIndex = 2;
            label13.Text = "Tồn Kho";
            // 
            // MainDashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(972, 558);
            Controls.Add(panel5);
            Controls.Add(panel4);
            Controls.Add(chart2);
            Controls.Add(chart1);
            Controls.Add(panel2);
            Controls.Add(panel3);
            Controls.Add(panel1);
            Controls.Add(btnOkeCustomDate);
            Controls.Add(btnCustomDate);
            Controls.Add(btnToday);
            Controls.Add(Last7Days);
            Controls.Add(btnLast30Days);
            Controls.Add(btnThisMonth);
            Controls.Add(dtpEnd);
            Controls.Add(dtpStart);
            Controls.Add(label1);
            Font = new Font("Segoe UI", 9F);
            Name = "MainDashboardForm";
            Text = "Dashboard";
            Load += MainDashboardForm_Load;
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)chart1).EndInit();
            ((System.ComponentModel.ISupportInitialize)chart2).EndInit();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel5.ResumeLayout(false);
            panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnderstock).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Button btnThisMonth;
        private Button btnLast30Days;
        private Button Last7Days;
        private Button btnToday;
        private Button btnCustomDate;
        private Button btnOkeCustomDate;
        private Panel panel1;
        private Label lblNumberOfOrders;
        private Label label2;
        private Panel panel2;
        private Label label3;
        private Label lblTotalOfProfit;
        private Panel panel3;
        private Label label5;
        private Label lblTotalOfRevenue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private Chart chart2;
        private Panel panel4;
        private Label lblNumberOfCustomers;
        private Label label6;
        private Label label7;
        private Label lblNumberOfSuppliers;
        private Label label9;
        private Label lblNumberOfProducts;
        private Label label11;
        private Panel panel5;
        private DataGridView dgvUnderstock;
        private Label label13;
    }
}
