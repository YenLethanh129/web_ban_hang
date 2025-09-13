using System.Windows.Forms.DataVisualization.Charting;

namespace Dashboard.Winform
{
    partial class frmLandingDashboard
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLandingDashboard));
            ChartArea chartArea1 = new ChartArea();
            Legend legend1 = new Legend();
            Series series1 = new Series();
            Title title1 = new Title();
            ChartArea chartArea2 = new ChartArea();
            Legend legend2 = new Legend();
            Series series2 = new Series();
            Title title2 = new Title();
            mainTableLayout = new TableLayoutPanel();
            headerPanel = new Panel();
            dateControlsPanel = new Panel();
            lblEndDate = new Label();
            btnOkeCustomDate = new Button();
            lblStartDate = new Label();
            btnCustomDate = new Button();
            label4 = new Label();
            btnToday = new Button();
            btnLast7Days = new Button();
            btnLast30Days = new Button();
            btnOneYear = new Button();
            dtpStart = new DateTimePicker();
            dtpEnd = new DateTimePicker();
            summaryTableLayout = new TableLayoutPanel();
            panel1 = new Panel();
            lblPerOrdersIncre = new Label();
            pictureBox1 = new PictureBox();
            lblNumberOfOrders = new Label();
            label2 = new Label();
            panel3 = new Panel();
            lblPerRevenueIncre = new Label();
            pictureBox2 = new PictureBox();
            label5 = new Label();
            lblTotalOfRevenue = new Label();
            panel2 = new Panel();
            lblPerProfitIncre = new Label();
            pictureBox3 = new PictureBox();
            lblTotalOfProfit = new Label();
            label3 = new Label();
            bottomTableLayout = new TableLayoutPanel();
            chartTopProduct = new Chart();
            rightSectionTableLayout = new TableLayoutPanel();
            chartGrossFinacial = new Chart();
            bottomRightTableLayout = new TableLayoutPanel();
            panel4 = new Panel();
            pictureBox6 = new PictureBox();
            pictureBox4 = new PictureBox();
            pictureBox5 = new PictureBox();
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
            mainTableLayout.SuspendLayout();
            headerPanel.SuspendLayout();
            dateControlsPanel.SuspendLayout();
            summaryTableLayout.SuspendLayout();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            bottomTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartTopProduct).BeginInit();
            rightSectionTableLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chartGrossFinacial).BeginInit();
            bottomRightTableLayout.SuspendLayout();
            panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).BeginInit();
            panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvUnderstock).BeginInit();
            SuspendLayout();
            // 
            // mainTableLayout
            // 
            mainTableLayout.ColumnCount = 1;
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTableLayout.Controls.Add(headerPanel, 0, 0);
            mainTableLayout.Controls.Add(summaryTableLayout, 0, 1);
            mainTableLayout.Controls.Add(bottomTableLayout, 0, 2);
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Location = new Point(0, 0);
            mainTableLayout.Margin = new Padding(0);
            mainTableLayout.MinimumSize = new Size(980, 530);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.Padding = new Padding(10, 0, 10, 10);
            mainTableLayout.RowCount = 3;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            mainTableLayout.Size = new Size(980, 640);
            mainTableLayout.TabIndex = 0;
            // 
            // headerPanel
            // 
            headerPanel.Controls.Add(dateControlsPanel);
            headerPanel.Dock = DockStyle.Fill;
            headerPanel.Location = new Point(10, 0);
            headerPanel.Margin = new Padding(0);
            headerPanel.MinimumSize = new Size(960, 50);
            headerPanel.Name = "headerPanel";
            headerPanel.Size = new Size(960, 50);
            headerPanel.TabIndex = 0;
            // 
            // dateControlsPanel
            // 
            dateControlsPanel.Controls.Add(lblEndDate);
            dateControlsPanel.Controls.Add(btnOkeCustomDate);
            dateControlsPanel.Controls.Add(lblStartDate);
            dateControlsPanel.Controls.Add(btnCustomDate);
            dateControlsPanel.Controls.Add(label4);
            dateControlsPanel.Controls.Add(btnToday);
            dateControlsPanel.Controls.Add(btnLast7Days);
            dateControlsPanel.Controls.Add(btnLast30Days);
            dateControlsPanel.Controls.Add(btnOneYear);
            dateControlsPanel.Controls.Add(dtpStart);
            dateControlsPanel.Controls.Add(dtpEnd);
            dateControlsPanel.Dock = DockStyle.Right;
            dateControlsPanel.Location = new Point(3, 0);
            dateControlsPanel.MinimumSize = new Size(780, 50);
            dateControlsPanel.Name = "dateControlsPanel";
            dateControlsPanel.Size = new Size(957, 50);
            dateControlsPanel.TabIndex = 1;
            // 
            // lblEndDate
            // 
            lblEndDate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblEndDate.AutoSize = true;
            lblEndDate.Font = new Font("Microsoft Sans Serif", 10F);
            lblEndDate.ForeColor = Color.FromArgb(124, 141, 181);
            lblEndDate.Location = new Point(729, 3);
            lblEndDate.MinimumSize = new Size(87, 23);
            lblEndDate.Name = "lblEndDate";
            lblEndDate.RightToLeft = RightToLeft.No;
            lblEndDate.Size = new Size(87, 23);
            lblEndDate.TabIndex = 18;
            lblEndDate.Text = "09/05/2025";
            lblEndDate.TextAlign = ContentAlignment.MiddleLeft;
            lblEndDate.Click += LblEndDate_Click;
            // 
            // btnOkeCustomDate
            // 
            btnOkeCustomDate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnOkeCustomDate.BackColor = Color.FromArgb(241, 88, 127);
            btnOkeCustomDate.BackgroundImage = (Image)resources.GetObject("btnOkeCustomDate.BackgroundImage");
            btnOkeCustomDate.BackgroundImageLayout = ImageLayout.Stretch;
            btnOkeCustomDate.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnOkeCustomDate.FlatAppearance.BorderSize = 0;
            btnOkeCustomDate.FlatStyle = FlatStyle.Flat;
            btnOkeCustomDate.Location = new Point(555, 2);
            btnOkeCustomDate.Margin = new Padding(3, 2, 3, 2);
            btnOkeCustomDate.Name = "btnOkeCustomDate";
            btnOkeCustomDate.Size = new Size(35, 30);
            btnOkeCustomDate.TabIndex = 8;
            btnOkeCustomDate.UseVisualStyleBackColor = true;
            // 
            // lblStartDate
            // 
            lblStartDate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            lblStartDate.AutoSize = true;
            lblStartDate.Font = new Font("Microsoft Sans Serif", 10F);
            lblStartDate.ForeColor = Color.FromArgb(124, 141, 181);
            lblStartDate.Location = new Point(603, 4);
            lblStartDate.MinimumSize = new Size(87, 23);
            lblStartDate.Name = "lblStartDate";
            lblStartDate.RightToLeft = RightToLeft.No;
            lblStartDate.Size = new Size(87, 23);
            lblStartDate.TabIndex = 2;
            lblStartDate.Text = "09/05/2025";
            lblStartDate.TextAlign = ContentAlignment.MiddleRight;
            lblStartDate.Click += LblStartDate_Click;
            // 
            // btnCustomDate
            // 
            btnCustomDate.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnCustomDate.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnCustomDate.FlatStyle = FlatStyle.Flat;
            btnCustomDate.Font = new Font("Microsoft Sans Serif", 11F);
            btnCustomDate.ForeColor = Color.FromArgb(124, 141, 181);
            btnCustomDate.Location = new Point(444, 3);
            btnCustomDate.Margin = new Padding(3, 2, 3, 2);
            btnCustomDate.Name = "btnCustomDate";
            btnCustomDate.Size = new Size(105, 30);
            btnCustomDate.TabIndex = 7;
            btnCustomDate.Text = "Tùy chọn";
            btnCustomDate.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.ForeColor = Color.FromArgb(124, 141, 181);
            label4.Location = new Point(696, 7);
            label4.Name = "label4";
            label4.Size = new Size(28, 15);
            label4.TabIndex = 17;
            label4.Text = "đến";
            // 
            // btnToday
            // 
            btnToday.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnToday.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnToday.FlatStyle = FlatStyle.Flat;
            btnToday.Font = new Font("Microsoft Sans Serif", 11F);
            btnToday.ForeColor = Color.FromArgb(124, 141, 181);
            btnToday.Location = new Point(333, 3);
            btnToday.Margin = new Padding(3, 2, 3, 2);
            btnToday.Name = "btnToday";
            btnToday.Size = new Size(105, 30);
            btnToday.TabIndex = 6;
            btnToday.Text = "Hôm nay";
            btnToday.UseVisualStyleBackColor = true;
            // 
            // btnLast7Days
            // 
            btnLast7Days.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnLast7Days.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnLast7Days.FlatStyle = FlatStyle.Flat;
            btnLast7Days.Font = new Font("Microsoft Sans Serif", 11F);
            btnLast7Days.ForeColor = Color.FromArgb(124, 141, 181);
            btnLast7Days.Location = new Point(222, 3);
            btnLast7Days.Margin = new Padding(3, 2, 3, 2);
            btnLast7Days.Name = "btnLast7Days";
            btnLast7Days.Size = new Size(105, 30);
            btnLast7Days.TabIndex = 5;
            btnLast7Days.Text = "7 ngày qua";
            btnLast7Days.UseVisualStyleBackColor = true;
            // 
            // btnLast30Days
            // 
            btnLast30Days.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnLast30Days.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnLast30Days.FlatStyle = FlatStyle.Flat;
            btnLast30Days.Font = new Font("Microsoft Sans Serif", 11F);
            btnLast30Days.ForeColor = Color.FromArgb(124, 141, 181);
            btnLast30Days.Location = new Point(111, 3);
            btnLast30Days.Margin = new Padding(3, 2, 3, 2);
            btnLast30Days.Name = "btnLast30Days";
            btnLast30Days.Size = new Size(105, 30);
            btnLast30Days.TabIndex = 4;
            btnLast30Days.Text = "30 ngày qua";
            btnLast30Days.UseVisualStyleBackColor = true;
            // 
            // btnOneYear
            // 
            btnOneYear.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            btnOneYear.FlatAppearance.BorderColor = Color.FromArgb(107, 83, 255);
            btnOneYear.FlatStyle = FlatStyle.Flat;
            btnOneYear.Font = new Font("Microsoft Sans Serif", 11F);
            btnOneYear.ForeColor = Color.FromArgb(124, 141, 181);
            btnOneYear.Location = new Point(0, 3);
            btnOneYear.Margin = new Padding(3, 2, 3, 2);
            btnOneYear.Name = "btnOneYear";
            btnOneYear.Size = new Size(105, 30);
            btnOneYear.TabIndex = 3;
            btnOneYear.Text = "năm qua";
            btnOneYear.UseVisualStyleBackColor = true;
            // 
            // dtpStart
            // 
            dtpStart.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dtpStart.CustomFormat = "dd/MM/yyyy";
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.Location = new Point(603, 7);
            dtpStart.Name = "dtpStart";
            dtpStart.Size = new Size(87, 21);
            dtpStart.TabIndex = 1;
            dtpStart.ValueChanged += DtpStart_ValueChanged;
            // 
            // dtpEnd
            // 
            dtpEnd.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dtpEnd.CustomFormat = "dd/MM/yyyy";
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.Location = new Point(729, 6);
            dtpEnd.Name = "dtpEnd";
            dtpEnd.Size = new Size(87, 21);
            dtpEnd.TabIndex = 2;
            dtpEnd.ValueChanged += DtpEnd_ValueChanged;
            // 
            // summaryTableLayout
            // 
            summaryTableLayout.ColumnCount = 3;
            summaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            summaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33F));
            summaryTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.34F));
            summaryTableLayout.Controls.Add(panel1, 0, 0);
            summaryTableLayout.Controls.Add(panel3, 1, 0);
            summaryTableLayout.Controls.Add(panel2, 2, 0);
            summaryTableLayout.Dock = DockStyle.Fill;
            summaryTableLayout.Location = new Point(10, 50);
            summaryTableLayout.Margin = new Padding(0);
            summaryTableLayout.MinimumSize = new Size(900, 80);
            summaryTableLayout.Name = "summaryTableLayout";
            summaryTableLayout.RowCount = 1;
            summaryTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            summaryTableLayout.Size = new Size(960, 80);
            summaryTableLayout.TabIndex = 1;
            // 
            // panel1
            // 
            panel1.BackColor = Color.FromArgb(42, 45, 86);
            panel1.Controls.Add(lblPerOrdersIncre);
            panel1.Controls.Add(pictureBox1);
            panel1.Controls.Add(lblNumberOfOrders);
            panel1.Controls.Add(label2);
            panel1.Dock = DockStyle.Fill;
            panel1.Location = new Point(3, 3);
            panel1.MinimumSize = new Size(280, 70);
            panel1.Name = "panel1";
            panel1.Size = new Size(313, 74);
            panel1.TabIndex = 10;
            // 
            // lblPerOrdersIncre
            // 
            lblPerOrdersIncre.Anchor = AnchorStyles.Right;
            lblPerOrdersIncre.AutoSize = true;
            lblPerOrdersIncre.Font = new Font("Microsoft Sans Serif", 10F);
            lblPerOrdersIncre.ForeColor = Color.Violet;
            lblPerOrdersIncre.Location = new Point(230, 27);
            lblPerOrdersIncre.MinimumSize = new Size(80, 0);
            lblPerOrdersIncre.Name = "lblPerOrdersIncre";
            lblPerOrdersIncre.Size = new Size(80, 17);
            lblPerOrdersIncre.TabIndex = 3;
            lblPerOrdersIncre.Text = "15%";
            lblPerOrdersIncre.TextAlign = ContentAlignment.TopRight;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(23, 10);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(51, 50);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 2;
            pictureBox1.TabStop = false;
            // 
            // lblNumberOfOrders
            // 
            lblNumberOfOrders.AutoSize = true;
            lblNumberOfOrders.Font = new Font("Microsoft Sans Serif", 13F);
            lblNumberOfOrders.ForeColor = Color.WhiteSmoke;
            lblNumberOfOrders.Location = new Point(80, 31);
            lblNumberOfOrders.Name = "lblNumberOfOrders";
            lblNumberOfOrders.Size = new Size(50, 22);
            lblNumberOfOrders.TabIndex = 1;
            lblNumberOfOrders.Text = "1000";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label2.ForeColor = Color.FromArgb(124, 141, 181);
            label2.Location = new Point(78, 14);
            label2.Name = "label2";
            label2.Size = new Size(83, 16);
            label2.TabIndex = 0;
            label2.Text = "Số đơn hàng";
            // 
            // panel3
            // 
            panel3.BackColor = Color.FromArgb(42, 45, 86);
            panel3.Controls.Add(lblPerRevenueIncre);
            panel3.Controls.Add(pictureBox2);
            panel3.Controls.Add(label5);
            panel3.Controls.Add(lblTotalOfRevenue);
            panel3.Dock = DockStyle.Fill;
            panel3.Location = new Point(322, 3);
            panel3.MinimumSize = new Size(280, 70);
            panel3.Name = "panel3";
            panel3.Size = new Size(313, 74);
            panel3.TabIndex = 11;
            // 
            // lblPerRevenueIncre
            // 
            lblPerRevenueIncre.Anchor = AnchorStyles.Right;
            lblPerRevenueIncre.AutoSize = true;
            lblPerRevenueIncre.Font = new Font("Microsoft Sans Serif", 10F);
            lblPerRevenueIncre.ForeColor = Color.FromArgb(128, 255, 255);
            lblPerRevenueIncre.Location = new Point(230, 31);
            lblPerRevenueIncre.MinimumSize = new Size(80, 0);
            lblPerRevenueIncre.Name = "lblPerRevenueIncre";
            lblPerRevenueIncre.Size = new Size(80, 17);
            lblPerRevenueIncre.TabIndex = 5;
            lblPerRevenueIncre.Text = "15%";
            lblPerRevenueIncre.TextAlign = ContentAlignment.TopRight;
            // 
            // pictureBox2
            // 
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(23, 10);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(55, 50);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 3;
            pictureBox2.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Microsoft Sans Serif", 10F);
            label5.ForeColor = Color.FromArgb(124, 141, 181);
            label5.Location = new Point(83, 12);
            label5.Name = "label5";
            label5.Size = new Size(109, 17);
            label5.TabIndex = 0;
            label5.Text = "Tổng doanh thu";
            // 
            // lblTotalOfRevenue
            // 
            lblTotalOfRevenue.AutoSize = true;
            lblTotalOfRevenue.Font = new Font("Microsoft Sans Serif", 13F);
            lblTotalOfRevenue.ForeColor = Color.WhiteSmoke;
            lblTotalOfRevenue.Location = new Point(83, 31);
            lblTotalOfRevenue.Name = "lblTotalOfRevenue";
            lblTotalOfRevenue.Size = new Size(50, 22);
            lblTotalOfRevenue.TabIndex = 1;
            lblTotalOfRevenue.Text = "1000";
            // 
            // panel2
            // 
            panel2.BackColor = Color.FromArgb(42, 45, 86);
            panel2.Controls.Add(lblPerProfitIncre);
            panel2.Controls.Add(pictureBox3);
            panel2.Controls.Add(lblTotalOfProfit);
            panel2.Controls.Add(label3);
            panel2.Dock = DockStyle.Fill;
            panel2.Location = new Point(641, 3);
            panel2.MinimumSize = new Size(280, 70);
            panel2.Name = "panel2";
            panel2.Size = new Size(316, 74);
            panel2.TabIndex = 11;
            // 
            // lblPerProfitIncre
            // 
            lblPerProfitIncre.Anchor = AnchorStyles.Right;
            lblPerProfitIncre.AutoSize = true;
            lblPerProfitIncre.Font = new Font("Microsoft Sans Serif", 10F);
            lblPerProfitIncre.ForeColor = Color.FromArgb(128, 128, 255);
            lblPerProfitIncre.Location = new Point(233, 31);
            lblPerProfitIncre.MinimumSize = new Size(80, 0);
            lblPerProfitIncre.Name = "lblPerProfitIncre";
            lblPerProfitIncre.Size = new Size(80, 17);
            lblPerProfitIncre.TabIndex = 4;
            lblPerProfitIncre.Text = "15%";
            lblPerProfitIncre.TextAlign = ContentAlignment.TopRight;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(22, 10);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(59, 50);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 4;
            pictureBox3.TabStop = false;
            // 
            // lblTotalOfProfit
            // 
            lblTotalOfProfit.AutoSize = true;
            lblTotalOfProfit.Font = new Font("Microsoft Sans Serif", 13F);
            lblTotalOfProfit.ForeColor = Color.WhiteSmoke;
            lblTotalOfProfit.Location = new Point(88, 31);
            lblTotalOfProfit.Name = "lblTotalOfProfit";
            lblTotalOfProfit.Size = new Size(50, 22);
            lblTotalOfProfit.TabIndex = 1;
            lblTotalOfProfit.Text = "1000";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Microsoft Sans Serif", 10F);
            label3.ForeColor = Color.FromArgb(124, 141, 181);
            label3.Location = new Point(88, 14);
            label3.Name = "label3";
            label3.Size = new Size(103, 17);
            label3.TabIndex = 0;
            label3.Text = "Tổng lợi nhuận";
            // 
            // bottomTableLayout
            // 
            bottomTableLayout.ColumnCount = 2;
            bottomTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            bottomTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            bottomTableLayout.Controls.Add(chartTopProduct, 0, 0);
            bottomTableLayout.Controls.Add(rightSectionTableLayout, 1, 0);
            bottomTableLayout.Dock = DockStyle.Fill;
            bottomTableLayout.Location = new Point(10, 130);
            bottomTableLayout.Margin = new Padding(0);
            bottomTableLayout.MinimumSize = new Size(900, 350);
            bottomTableLayout.Name = "bottomTableLayout";
            bottomTableLayout.RowCount = 1;
            bottomTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomTableLayout.Size = new Size(960, 500);
            bottomTableLayout.TabIndex = 2;
            // 
            // chartTopProduct
            // 
            chartTopProduct.BackColor = Color.FromArgb(42, 45, 86);
            chartArea1.Name = "ChartArea1";
            chartTopProduct.ChartAreas.Add(chartArea1);
            chartTopProduct.Dock = DockStyle.Fill;
            legend1.Docking = Docking.Bottom;
            legend1.Name = "Legend1";
            chartTopProduct.Legends.Add(legend1);
            chartTopProduct.Location = new Point(3, 3);
            chartTopProduct.MinimumSize = new Size(200, 350);
            chartTopProduct.Name = "chartTopProduct";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = SeriesChartType.Doughnut;
            series1.Font = new Font("Microsoft Sans Serif", 12F);
            series1.IsValueShownAsLabel = true;
            series1.LabelForeColor = Color.White;
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            chartTopProduct.Series.Add(series1);
            chartTopProduct.Size = new Size(234, 494);
            chartTopProduct.TabIndex = 13;
            chartTopProduct.Text = "chart2";
            title1.Alignment = ContentAlignment.TopLeft;
            title1.Font = new Font("Microsoft Sans Serif", 12F);
            title1.ForeColor = Color.WhiteSmoke;
            title1.Name = "Title1";
            title1.Text = "Top 5 Sản Phẩm Bán Chạy";
            chartTopProduct.Titles.Add(title1);
            // 
            // rightSectionTableLayout
            // 
            rightSectionTableLayout.ColumnCount = 1;
            rightSectionTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            rightSectionTableLayout.Controls.Add(chartGrossFinacial, 0, 0);
            rightSectionTableLayout.Controls.Add(bottomRightTableLayout, 0, 1);
            rightSectionTableLayout.Dock = DockStyle.Fill;
            rightSectionTableLayout.Location = new Point(240, 0);
            rightSectionTableLayout.Margin = new Padding(0);
            rightSectionTableLayout.MinimumSize = new Size(650, 350);
            rightSectionTableLayout.Name = "rightSectionTableLayout";
            rightSectionTableLayout.RowCount = 2;
            rightSectionTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            rightSectionTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            rightSectionTableLayout.Size = new Size(720, 500);
            rightSectionTableLayout.TabIndex = 1;
            // 
            // chartGrossFinacial
            // 
            chartGrossFinacial.BackColor = Color.FromArgb(42, 45, 86);
            chartArea2.Name = "ChartArea1";
            chartGrossFinacial.ChartAreas.Add(chartArea2);
            chartGrossFinacial.Dock = DockStyle.Fill;
            legend2.Docking = Docking.Top;
            legend2.Name = "Legend1";
            chartGrossFinacial.Legends.Add(legend2);
            chartGrossFinacial.Location = new Point(3, 3);
            chartGrossFinacial.MinimumSize = new Size(500, 200);
            chartGrossFinacial.Name = "chartGrossFinacial";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            chartGrossFinacial.Series.Add(series2);
            chartGrossFinacial.Size = new Size(714, 294);
            chartGrossFinacial.TabIndex = 12;
            chartGrossFinacial.Text = "chart1";
            title2.Alignment = ContentAlignment.TopLeft;
            title2.Font = new Font("Microsoft Sans Serif", 12F);
            title2.ForeColor = Color.WhiteSmoke;
            title2.Name = "Title1";
            title2.Text = "Tổng Doanh Thu";
            chartGrossFinacial.Titles.Add(title2);
            // 
            // bottomRightTableLayout
            // 
            bottomRightTableLayout.ColumnCount = 2;
            bottomRightTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            bottomRightTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F));
            bottomRightTableLayout.Controls.Add(panel4, 0, 0);
            bottomRightTableLayout.Controls.Add(panel5, 1, 0);
            bottomRightTableLayout.Dock = DockStyle.Fill;
            bottomRightTableLayout.Location = new Point(0, 300);
            bottomRightTableLayout.Margin = new Padding(0);
            bottomRightTableLayout.MinimumSize = new Size(500, 130);
            bottomRightTableLayout.Name = "bottomRightTableLayout";
            bottomRightTableLayout.RowCount = 1;
            bottomRightTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            bottomRightTableLayout.Size = new Size(720, 200);
            bottomRightTableLayout.TabIndex = 1;
            // 
            // panel4
            // 
            panel4.BackColor = Color.FromArgb(42, 45, 86);
            panel4.Controls.Add(pictureBox6);
            panel4.Controls.Add(pictureBox4);
            panel4.Controls.Add(pictureBox5);
            panel4.Controls.Add(lblNumberOfProducts);
            panel4.Controls.Add(lblNumberOfSuppliers);
            panel4.Controls.Add(label11);
            panel4.Controls.Add(label9);
            panel4.Controls.Add(label7);
            panel4.Controls.Add(lblNumberOfCustomers);
            panel4.Controls.Add(label6);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(3, 3);
            panel4.MinimumSize = new Size(150, 130);
            panel4.Name = "panel4";
            panel4.Size = new Size(210, 194);
            panel4.TabIndex = 11;
            // 
            // pictureBox6
            // 
            pictureBox6.Image = (Image)resources.GetObject("pictureBox6.Image");
            pictureBox6.Location = new Point(8, 85);
            pictureBox6.Name = "pictureBox6";
            pictureBox6.Size = new Size(44, 40);
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.TabIndex = 17;
            pictureBox6.TabStop = false;
            // 
            // pictureBox4
            // 
            pictureBox4.Image = (Image)resources.GetObject("pictureBox4.Image");
            pictureBox4.Location = new Point(8, 142);
            pictureBox4.Name = "pictureBox4";
            pictureBox4.Size = new Size(44, 40);
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.TabIndex = 16;
            pictureBox4.TabStop = false;
            // 
            // pictureBox5
            // 
            pictureBox5.Image = (Image)resources.GetObject("pictureBox5.Image");
            pictureBox5.Location = new Point(8, 26);
            pictureBox5.Name = "pictureBox5";
            pictureBox5.Size = new Size(44, 40);
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.TabIndex = 7;
            pictureBox5.TabStop = false;
            // 
            // lblNumberOfProducts
            // 
            lblNumberOfProducts.AutoSize = true;
            lblNumberOfProducts.Font = new Font("Microsoft Sans Serif", 13F);
            lblNumberOfProducts.ForeColor = Color.WhiteSmoke;
            lblNumberOfProducts.Location = new Point(63, 159);
            lblNumberOfProducts.Name = "lblNumberOfProducts";
            lblNumberOfProducts.Size = new Size(50, 22);
            lblNumberOfProducts.TabIndex = 15;
            lblNumberOfProducts.Text = "1000";
            // 
            // lblNumberOfSuppliers
            // 
            lblNumberOfSuppliers.AutoSize = true;
            lblNumberOfSuppliers.Font = new Font("Microsoft Sans Serif", 13F);
            lblNumberOfSuppliers.ForeColor = Color.WhiteSmoke;
            lblNumberOfSuppliers.Location = new Point(63, 102);
            lblNumberOfSuppliers.Name = "lblNumberOfSuppliers";
            lblNumberOfSuppliers.Size = new Size(50, 22);
            lblNumberOfSuppliers.TabIndex = 4;
            lblNumberOfSuppliers.Text = "1000";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label11.ForeColor = Color.FromArgb(124, 141, 181);
            label11.Location = new Point(63, 142);
            label11.Name = "label11";
            label11.Size = new Size(68, 16);
            label11.TabIndex = 14;
            label11.Text = "Sản phẩm";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label9.ForeColor = Color.FromArgb(124, 141, 181);
            label9.Location = new Point(63, 85);
            label9.Name = "label9";
            label9.Size = new Size(90, 16);
            label9.TabIndex = 3;
            label9.Text = "Nhà cung cấp";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Microsoft Sans Serif", 12F);
            label7.ForeColor = Color.WhiteSmoke;
            label7.Location = new Point(3, 0);
            label7.Name = "label7";
            label7.Size = new Size(81, 20);
            label7.TabIndex = 2;
            label7.Text = "Doanh Số";
            // 
            // lblNumberOfCustomers
            // 
            lblNumberOfCustomers.AutoSize = true;
            lblNumberOfCustomers.Font = new Font("Microsoft Sans Serif", 13F);
            lblNumberOfCustomers.ForeColor = Color.WhiteSmoke;
            lblNumberOfCustomers.Location = new Point(63, 43);
            lblNumberOfCustomers.Name = "lblNumberOfCustomers";
            lblNumberOfCustomers.Size = new Size(50, 22);
            lblNumberOfCustomers.TabIndex = 1;
            lblNumberOfCustomers.Text = "1000";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            label6.ForeColor = Color.FromArgb(124, 141, 181);
            label6.Location = new Point(63, 26);
            label6.Name = "label6";
            label6.Size = new Size(77, 16);
            label6.TabIndex = 0;
            label6.Text = "Khách hàng";
            // 
            // panel5
            // 
            panel5.BackColor = Color.FromArgb(42, 45, 86);
            panel5.Controls.Add(dgvUnderstock);
            panel5.Controls.Add(label13);
            panel5.Dock = DockStyle.Fill;
            panel5.Location = new Point(219, 3);
            panel5.MinimumSize = new Size(320, 130);
            panel5.Name = "panel5";
            panel5.Size = new Size(498, 194);
            panel5.TabIndex = 16;
            // 
            // dgvUnderstock
            // 
            dgvUnderstock.AllowUserToAddRows = false;
            dgvUnderstock.AllowUserToDeleteRows = false;
            dgvUnderstock.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUnderstock.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dgvUnderstock.Dock = DockStyle.Fill;
            dgvUnderstock.Location = new Point(0, 30);
            dgvUnderstock.Margin = new Padding(20, 5, 10, 20);
            dgvUnderstock.Name = "dgvUnderstock";
            dgvUnderstock.ReadOnly = true;
            dgvUnderstock.RowHeadersWidth = 51;
            dgvUnderstock.Size = new Size(498, 164);
            dgvUnderstock.TabIndex = 3;
            // 
            // label13
            // 
            label13.Dock = DockStyle.Top;
            label13.Font = new Font("Microsoft Sans Serif", 12F);
            label13.ForeColor = Color.WhiteSmoke;
            label13.Location = new Point(0, 0);
            label13.Name = "label13";
            label13.Padding = new Padding(10, 5, 0, 5);
            label13.Size = new Size(498, 30);
            label13.TabIndex = 2;
            label13.Text = "Hàng sắp hết";
            label13.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // frmLandingDashboard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(24, 28, 63);
            ClientSize = new Size(980, 640);
            Controls.Add(mainTableLayout);
            Font = new Font("Microsoft Sans Serif", 9F);
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.None;
            MinimumSize = new Size(980, 640);
            Name = "frmLandingDashboard";
            Text = "  ";
            Load += MainDashboardForm_Load;
            mainTableLayout.ResumeLayout(false);
            headerPanel.ResumeLayout(false);
            dateControlsPanel.ResumeLayout(false);
            dateControlsPanel.PerformLayout();
            summaryTableLayout.ResumeLayout(false);
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            bottomTableLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartTopProduct).EndInit();
            rightSectionTableLayout.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chartGrossFinacial).EndInit();
            bottomRightTableLayout.ResumeLayout(false);
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox6).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox4).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox5).EndInit();
            panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvUnderstock).EndInit();
            ResumeLayout(false);



        }

        #endregion
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Button btnOneYear;
        private Button btnLast30Days;
        private Button btnLast7Days;
        private Button btnToday;
        private Button btnCustomDate;
        private Button btnOkeCustomDate;
        private Panel panel1;
        private Label lblNumberOfOrders;
        private Label label2;
        private Panel panel2;
        private Label lblTotalOfProfit;
        private Label label3;
        private Panel panel3;
        private Label label5;
        private Label lblTotalOfRevenue;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartGrossFinacial;
        private Chart chartTopProduct;
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
        private Label lblStartDate;
        private Label lblEndDate;
        private PictureBox pictureBox1;
        private PictureBox pictureBox3;
        private PictureBox pictureBox2;
        private Label lblPerOrdersIncre;
        private Label lblPerProfitIncre;
        private Label lblPerRevenueIncre;
        
        // TableLayoutPanel controls
        private TableLayoutPanel mainTableLayout;
        private Panel headerPanel;
        private Panel dateControlsPanel;
        private TableLayoutPanel summaryTableLayout;
        private TableLayoutPanel bottomTableLayout;
        private TableLayoutPanel rightSectionTableLayout;
        private TableLayoutPanel bottomRightTableLayout;
        private PictureBox pictureBox6;
        private PictureBox pictureBox4;
        private PictureBox pictureBox5;
    }
}