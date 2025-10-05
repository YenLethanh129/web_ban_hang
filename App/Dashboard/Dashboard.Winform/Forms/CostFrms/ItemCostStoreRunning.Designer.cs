namespace Dashboard.Winform.Forms.CostFrms
{
    partial class ItemCostStoreRunning
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            panel1 = new Panel();
            txtNameCost = new Label();
            panel2 = new Panel();
            panel3 = new Panel();
            panel4 = new Panel();
            label2 = new Label();
            txtCost = new Label();
            label3 = new Label();
            button1 = new Button();
            panel1.SuspendLayout();
            panel2.SuspendLayout();
            panel3.SuspendLayout();
            panel4.SuspendLayout();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.Controls.Add(txtNameCost);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(200, 50);
            panel1.TabIndex = 0;
            // 
            // txtNameCost
            // 
            txtNameCost.AutoEllipsis = true;
            txtNameCost.Dock = DockStyle.Fill;
            txtNameCost.Font = new Font("Microsoft Sans Serif", 11F);
            txtNameCost.Location = new Point(0, 0);
            txtNameCost.Name = "txtNameCost";
            txtNameCost.Padding = new Padding(5, 0, 0, 0);
            txtNameCost.Size = new Size(200, 50);
            txtNameCost.TabIndex = 0;
            txtNameCost.Text = "name_cost";
            txtNameCost.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            panel2.Controls.Add(txtCost);
            panel2.Dock = DockStyle.Left;
            panel2.Location = new Point(200, 0);
            panel2.Name = "panel2";
            panel2.Size = new Size(100, 50);
            panel2.TabIndex = 0;
            // 
            // panel3
            // 
            panel3.Controls.Add(button1);
            panel3.Dock = DockStyle.Right;
            panel3.Location = new Point(550, 0);
            panel3.Name = "panel3";
            panel3.Size = new Size(100, 50);
            panel3.TabIndex = 0;
            // 
            // panel4
            // 
            panel4.Controls.Add(label3);
            panel4.Controls.Add(label2);
            panel4.Dock = DockStyle.Fill;
            panel4.Location = new Point(300, 0);
            panel4.Name = "panel4";
            panel4.Size = new Size(250, 50);
            panel4.TabIndex = 0;
            // 
            // label2
            // 
            label2.AutoEllipsis = true;
            label2.Font = new Font("Microsoft Sans Serif", 11F);
            label2.Location = new Point(-94, 0);
            label2.Name = "label2";
            label2.Padding = new Padding(5, 0, 0, 0);
            label2.Size = new Size(88, 50);
            label2.TabIndex = 0;
            label2.Text = "name_cost";
            label2.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // txtCost
            // 
            txtCost.Dock = DockStyle.Fill;
            txtCost.Font = new Font("Microsoft Sans Serif", 11F);
            txtCost.Location = new Point(0, 0);
            txtCost.Name = "txtCost";
            txtCost.Size = new Size(100, 50);
            txtCost.TabIndex = 0;
            txtCost.Text = "label3";
            txtCost.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            label3.Dock = DockStyle.Fill;
            label3.Font = new Font("Microsoft Sans Serif", 11F);
            label3.Location = new Point(0, 0);
            label3.Name = "label3";
            label3.Size = new Size(250, 50);
            label3.TabIndex = 0;
            label3.Text = "label3";
            label3.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            button1.Dock = DockStyle.Fill;
            button1.FlatAppearance.BorderSize = 0;
            button1.FlatStyle = FlatStyle.Flat;
            button1.Font = new Font("Microsoft Sans Serif", 11F);
            button1.Location = new Point(0, 0);
            button1.Margin = new Padding(0);
            button1.Name = "button1";
            button1.Size = new Size(100, 50);
            button1.TabIndex = 0;
            button1.Text = "Xem bill";
            button1.UseVisualStyleBackColor = true;
            // 
            // ItemCostStoreRunning
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(panel4);
            Controls.Add(panel3);
            Controls.Add(panel2);
            Controls.Add(panel1);
            Name = "ItemCostStoreRunning";
            Size = new Size(650, 50);
            panel1.ResumeLayout(false);
            panel2.ResumeLayout(false);
            panel3.ResumeLayout(false);
            panel4.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private Panel panel1;
        private Label txtNameCost;
        private Panel panel2;
        private Panel panel3;
        private Panel panel4;
        private Label label2;
        private Label txtCost;
        private Label label3;
        private Button button1;
    }
}
