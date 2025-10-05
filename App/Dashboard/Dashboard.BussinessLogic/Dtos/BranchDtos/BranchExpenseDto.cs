namespace Dashboard.BussinessLogic.Dtos.BranchDtos;

public class BranchExpenseDto
{
    public long Id { get; set; }                   // thêm để CRUD
    public long BranchId { get; set; }
    public string ExpenseType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }          // sửa nullable
    public string? PaymentCycle { get; set; }
    public string? Note { get; set; }
}