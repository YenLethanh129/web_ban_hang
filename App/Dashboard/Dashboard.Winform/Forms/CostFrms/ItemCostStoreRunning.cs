using System;
using System.Drawing;
using System.Windows.Forms;
using Dashboard.BussinessLogic.Dtos.BranchDtos;

namespace Dashboard.Winform.Forms.CostFrms
{
    public partial class ItemCostStoreRunning : UserControl
    {
        public BranchExpenseDto? Expense { get; private set; }

        public event EventHandler<BranchExpenseDto>? ViewBillRequested;

        public ItemCostStoreRunning()
        {
            InitializeComponent();
            button1.Click += (_, _) =>
            {
                if (Expense != null)
                    ViewBillRequested?.Invoke(this, Expense);
            };
            ApplyRowStyle();
        }

        public void SetData(BranchExpenseDto expense)
        {
            Expense = expense; // Lưu reference
            
            // Gán dữ liệu vào các control
            txtNameCost.Text = expense.ExpenseType;
            txtCost.Text = expense.Amount.ToString("N0");
            
            // Tạo mô tả chi tiết
            var description = BuildDescription(expense);
            label3.Text = description;
            
            // Lưu reference để dùng cho event ViewBill
            this.Tag = expense;
        }

        private static string BuildDescription(BranchExpenseDto expense)
        {
            var period = expense.EndDate.HasValue 
                ? $"{expense.StartDate:dd/MM} - {expense.EndDate:dd/MM}" 
                : $"{expense.StartDate:dd/MM/yyyy}";
                
            var cycle = !string.IsNullOrWhiteSpace(expense.PaymentCycle) 
                ? $" [{expense.PaymentCycle}]" 
                : "";
                
            var note = !string.IsNullOrWhiteSpace(expense.Note) 
                ? $" - {expense.Note}" 
                : "";
                
            return $"{period}{cycle}{note}";
        }

        // Event khi user click "Xem bill"
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.Tag is BranchExpenseDto expense)
            {
                ViewBillRequested?.Invoke(this, expense);
            }
        }

        private void ApplyRowStyle()
        {
            BackColor = Color.FromArgb(235, 239, 249);
            Padding = new Padding(0);
            Margin = new Padding(0, 0, 0, 1);
            txtNameCost.ForeColor = Color.FromArgb(40, 40, 40);
            txtCost.ForeColor = Color.FromArgb(60, 60, 60);
            label3.ForeColor = Color.FromArgb(90, 90, 90);

            button1.FlatAppearance.MouseOverBackColor = Color.FromArgb(210, 220, 240);
            button1.FlatAppearance.MouseDownBackColor = Color.FromArgb(200, 210, 230);
        }
            }
}
