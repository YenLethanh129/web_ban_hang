using System;
using System.Windows.Forms;

namespace Dashboard.Winform.Forms.Base
{
    public partial class BaseCrudForm : Form
    {
        protected Panel pnlContent = null!;
        protected Panel pnlHeader = null!;
        protected Label lblTitle = null!;

        public BaseCrudForm(string title)
        {
            InitializeBaseComponents();
            SetTitle(title);
        }

        private void InitializeBaseComponents()
        {
            Size = new Size(1200, 800);
            StartPosition = FormStartPosition.CenterScreen;
            BackColor = Color.FromArgb(42, 45, 86);
            ForeColor = Color.White;

            // Header panel
            pnlHeader = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.FromArgb(32, 35, 76)
            };

            lblTitle = new Label
            {
                Text = "Form Title",
                Font = new Font("Segoe UI", 16F, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 15),
                AutoSize = true
            };

            pnlHeader.Controls.Add(lblTitle);

            // Content panel
            pnlContent = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(42, 45, 86),
                Padding = new Padding(10)
            };

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
        }

        protected void SetTitle(string title)
        {
            Text = title;
            lblTitle.Text = title;
        }
    }
}
