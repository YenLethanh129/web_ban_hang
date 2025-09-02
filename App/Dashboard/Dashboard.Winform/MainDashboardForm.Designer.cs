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
            ChartArea chartArea3 = new ChartArea();
            Legend legend3 = new Legend();
            Series series3 = new Series();
            Title title3 = new Title();
            ChartArea chartArea4 = new ChartArea();
            Legend legend4 = new Legend();
            Series series4 = new Series();
            Title title4 = new Title();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
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
            label4 = new Label();
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
            label1.Location = new Point(14, 12);
            label1.Name = "label1";
            label1.Size = new Size(156, 40);
            label1.TabIndex = 0;
            label1.Text = "Dashboard";
            // 
            // dtpStart
            // 
            dtpStart.CustomFormat = "dd/MM/yyyy";
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.Location = new Point(174, 25);
            dtpStart.Margin = new Padding(3, 4, 3, 4);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(99, 27);
            dtpStart.TabIndex = 1;
            // 
            // dtpEnd
            // 
            dtpEnd.CustomFormat = "dd/MM/yyyy";
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.Location = new Point(321, 24);
            dtpEnd.Margin = new Padding(3, 4, 3, 4);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(99, 27);
            dtpEnd.TabIndex = 2;
            // 
            // btnThisMonth
            // 
            btnThisMonth.Location = new Point(976, 19);
            btnThisMonth.Name = "btnThisMonth";
            btnThisMonth.Size = new Size(120, 40);
            btnThisMonth.TabIndex = 3;
            btnThisMonth.Text = "Tháng này";
            btnThisMonth.UseVisualStyleBackColor = true;
            // 
            // btnLast30Days
            // 
            btnLast30Days.Location = new Point(850, 19);
            btnLast30Days.Name = "btnLast30Days";
            btnLast30Days.Size = new Size(120, 40);
            btnLast30Days.TabIndex = 4;
            btnLast30Days.Text = "30 ngày qua";
            btnLast30Days.UseVisualStyleBackColor = true;
            // 
            // Last7Days
            // 
            Last7Days.Location = new Point(725, 19);
            Last7Days.Name = "Last7Days";
            Last7Days.Size = new Size(120, 40);
            Last7Days.TabIndex = 5;
            Last7Days.Text = "7 ngày qua";
            Last7Days.UseVisualStyleBackColor = true;
            // 
            // btnToday
            // 
            btnToday.Location = new Point(599, 19);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(120, 40);
            btnToday.TabIndex = 6;
            btnToday.Text = "Hôm nay";
            btnToday.UseVisualStyleBackColor = true;
            // 
            // btnCustomDate
            // 
            btnCustomDate.Location = new Point(472, 19);
            btnCustomDate.Name = "btnCustomDate";
            btnCustomDate.Size = new Size(120, 40);
            btnCustomDate.TabIndex = 7;
            btnCustomDate.Text = "Tùy chọn";
            btnCustomDate.UseVisualStyleBackColor = true;
            // 
            // btnOkeCustomDate
            // 
            btnOkeCustomDate.Location = new Point(426, 19);
            btnOkeCustomDate.Name = "btnOkeCustomDate";
            btnOkeCustomDate.Size = new Size(40, 40);
            btnOkeCustomDate.TabIndex = 8;
            btnOkeCustomDate.Text = "Oke";
            btnOkeCustomDate.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(42, 45, 86);
            panel1.Controls.Add(lblNumberOfOrders);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(14, 76);
            panel1.Name = "panel1";
            panel1.Size = new Size(205, 68);
            panel1.TabIndex = 10;
            // 
            // lblNumberOfOrders
            // 
            lblNumberOfOrders.AutoSize = true;
            lblNumberOfOrders.Font = new Font("Segoe UI", 13F);
            lblNumberOfOrders.ForeColor = Color.WhiteSmoke;
            lblNumberOfOrders.Location = new Point(17, 23);
            lblNumberOfOrders.Name = "lblNumberOfOrders";
            lblNumberOfOrders.Size = new Size(61, 30);
            lblNumberOfOrders.TabIndex = 1;
            lblNumberOfOrders.Text = "1000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.WhiteSmoke;
            label2.Location = new Point(17, 0);
            label2.Name = "label2";
            label2.Size = new Size(108, 23);
            label2.TabIndex = 0;
            label2.Text = "Số đơn hàng";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(42, 45, 86);
            panel2.Controls.Add(label3);
            panel2.Controls.Add(lblTotalOfProfit);
            panel2.Location = new Point(590, 76);
            panel2.Name = "panel2";
            panel2.Size = new Size(506, 68);
            panel2.TabIndex = 11;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 13F);
            label3.ForeColor = Color.WhiteSmoke;
            label3.Location = new Point(33, 23);
            label3.Name = "label3";
            label3.Size = new Size(61, 30);
            label3.TabIndex = 1;
            label3.Text = "1000";
            // 
            // lblTotalOfProfit
            // 
            lblTotalOfProfit.AutoSize = true;
            lblTotalOfProfit.Font = new Font("Segoe UI", 10F);
            lblTotalOfProfit.ForeColor = Color.WhiteSmoke;
            lblTotalOfProfit.Location = new Point(33, 0);
            lblTotalOfProfit.Name = "lblTotalOfProfit";
            lblTotalOfProfit.Size = new Size(126, 23);
            lblTotalOfProfit.TabIndex = 0;
            lblTotalOfProfit.Text = "Tổng lợi nhuận";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(42, 45, 86);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(lblTotalOfRevenue);
            panel3.Location = new Point(225, 76);
            panel3.Name = "panel3";
            panel3.Size = new Size(358, 68);
            panel3.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 10F);
            label5.ForeColor = Color.WhiteSmoke;
            label5.Location = new Point(33, 0);
            label5.Name = "label5";
            label5.Size = new Size(134, 23);
            label5.TabIndex = 0;
            label5.Text = "Tổng doanh thu";
            // 
            // lblTotalOfRevenue
            // 
            lblTotalOfRevenue.AutoSize = true;
            lblTotalOfRevenue.Font = new Font("Segoe UI", 13F);
            lblTotalOfRevenue.ForeColor = Color.WhiteSmoke;
            lblTotalOfRevenue.Location = new Point(33, 25);
            lblTotalOfRevenue.Name = "lblTotalOfRevenue";
            lblTotalOfRevenue.Size = new Size(61, 30);
            lblTotalOfRevenue.TabIndex = 1;
            lblTotalOfRevenue.Text = "1000";
            // 
            // chart1
            // 
            chart1.BackColor = Color.FromArgb(42, 45, 86);
            chartArea3.Name = "ChartArea1";
            chart1.ChartAreas.Add(chartArea3);
            legend3.Docking = Docking.Top;
            legend3.Name = "Legend1";
            chart1.Legends.Add(legend3);
            chart1.Location = new Point(14, 151);
            chart1.Margin = new Padding(3, 4, 3, 4);
            chart1.Name = "chart1";
            series3.ChartArea = "ChartArea1";
            series3.Legend = "Legend1";
            series3.Name = "Series1";
            chart1.Series.Add(series3);
            chart1.Size = new Size(682, 336);
            chart1.TabIndex = 12;
            chart1.Text = "chart1";
            title3.Alignment = ContentAlignment.TopLeft;
            title3.Font = new Font("Microsoft Sans Serif", 12F);
            title3.ForeColor = Color.WhiteSmoke;
            title3.Name = "Title1";
            title3.Text = "Tổng Doanh Thu";
            chart1.Titles.Add(title3);
            // 
            // chart2
            // 
            chart2.BackColor = Color.FromArgb(42, 45, 86);
            chartArea4.Name = "ChartArea1";
            chart2.ChartAreas.Add(chartArea4);
            legend4.Docking = Docking.Bottom;
            legend4.Name = "Legend1";
            chart2.Legends.Add(legend4);
            chart2.Location = new Point(703, 151);
            chart2.Margin = new Padding(3, 4, 3, 4);
            chart2.Name = "chart2";
            series4.ChartArea = "ChartArea1";
            series4.ChartType = SeriesChartType.Doughnut;
            series4.Font = new Font("Microsoft Sans Serif", 12F);
            series4.IsValueShownAsLabel = true;
            series4.LabelForeColor = Color.White;
            series4.Legend = "Legend1";
            series4.Name = "Series1";
            chart2.Series.Add(series4);
            chart2.Size = new Size(393, 581);
            chart2.TabIndex = 13;
            chart2.Text = "chart2";
            title4.Alignment = ContentAlignment.TopLeft;
            title4.Font = new Font("Microsoft Sans Serif", 12F);
            title4.ForeColor = Color.WhiteSmoke;
            title4.Name = "Title1";
            title4.Text = "Top 5 Sản Phẩm Bán Chạy";
            chart2.Titles.Add(title4);
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
            panel4.Location = new Point(14, 493);
            panel4.Name = "panel4";
            panel4.Size = new Size(194, 239);
            panel4.TabIndex = 11;
            // 
            // lblNumberOfProducts
            // 
            lblNumberOfProducts.AutoSize = true;
            lblNumberOfProducts.Font = new Font("Segoe UI", 13F);
            lblNumberOfProducts.ForeColor = Color.WhiteSmoke;
            lblNumberOfProducts.Location = new Point(17, 192);
            lblNumberOfProducts.Name = "lblNumberOfProducts";
            lblNumberOfProducts.Size = new Size(61, 30);
            lblNumberOfProducts.TabIndex = 15;
            lblNumberOfProducts.Text = "1000";
            // 
            // lblNumberOfSuppliers
            // 
            lblNumberOfSuppliers.AutoSize = true;
            lblNumberOfSuppliers.Font = new Font("Segoe UI", 13F);
            lblNumberOfSuppliers.ForeColor = Color.WhiteSmoke;
            lblNumberOfSuppliers.Location = new Point(17, 124);
            lblNumberOfSuppliers.Name = "lblNumberOfSuppliers";
            lblNumberOfSuppliers.Size = new Size(61, 30);
            lblNumberOfSuppliers.TabIndex = 4;
            lblNumberOfSuppliers.Text = "1000";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.ForeColor = Color.WhiteSmoke;
            label11.Location = new Point(17, 169);
            label11.Name = "label11";
            label11.Size = new Size(87, 23);
            label11.TabIndex = 14;
            label11.Text = "Sản phẩm";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.WhiteSmoke;
            label9.Location = new Point(17, 101);
            label9.Name = "label9";
            label9.Size = new Size(117, 23);
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
            label7.Size = new Size(98, 28);
            label7.TabIndex = 2;
            label7.Text = "Doanh Số";
            // 
            // lblNumberOfCustomers
            // 
            lblNumberOfCustomers.AutoSize = true;
            lblNumberOfCustomers.Font = new Font("Segoe UI", 13F);
            lblNumberOfCustomers.ForeColor = Color.WhiteSmoke;
            lblNumberOfCustomers.Location = new Point(17, 56);
            lblNumberOfCustomers.Name = "lblNumberOfCustomers";
            lblNumberOfCustomers.Size = new Size(61, 30);
            lblNumberOfCustomers.TabIndex = 1;
            lblNumberOfCustomers.Text = "1000";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.WhiteSmoke;
            label6.Location = new Point(17, 33);
            label6.Name = "label6";
            label6.Size = new Size(101, 23);
            label6.TabIndex = 0;
            label6.Text = "Khách hàng";
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(42, 45, 86);
            panel5.Controls.Add(dgvUnderstock);
            panel5.Controls.Add(label13);
            panel5.Location = new Point(215, 493);
            panel5.Name = "panel5";
            panel5.Size = new Size(481, 239);
            panel5.TabIndex = 16;
            // 
            // dgvUnderstock
            // 
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvUnderstock.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvUnderstock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvUnderstock.DefaultCellStyle = dataGridViewCellStyle5;
            dgvUnderstock.Location = new Point(10, 33);
            dgvUnderstock.Margin = new Padding(3, 4, 3, 4);
            dgvUnderstock.Name = "dgvUnderstock";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 9F);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvUnderstock.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvUnderstock.RowHeadersWidth = 51;
            dgvUnderstock.Size = new Size(457, 200);
            dgvUnderstock.TabIndex = 3;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 12F);
            label13.ForeColor = Color.WhiteSmoke;
            label13.Location = new Point(3, 0);
            label13.Name = "label13";
            label13.Size = new Size(85, 28);
            label13.TabIndex = 2;
            label13.Text = "Tồn Kho";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.ForeColor = Color.White;
            label4.Location = new Point(279, 28);
            label4.Name = "label4";
            label4.Size = new Size(34, 20);
            label4.TabIndex = 17;
            label4.Text = "đến";
            // 
            // MainDashboardForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(1111, 744);
            Controls.Add(label4);
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
            Margin = new Padding(3, 4, 3, 4);
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
        private Label label4;
    }
}
