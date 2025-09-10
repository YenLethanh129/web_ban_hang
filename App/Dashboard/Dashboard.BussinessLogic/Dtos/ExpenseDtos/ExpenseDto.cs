namespace Dashboard.BussinessLogic.Dtos.ExpenseDtos;

public class ExpenseDto
{
    public long Id { get; set; }
    public long BranchId { get; set; }
    public string BranchName { get; set; } = string.Empty;
    public string ExpenseType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime LastModified { get; set; }
}

public class CreateExpenseInput
{
    public long BranchId { get; set; }
    public string ExpenseType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class UpdateExpenseInput
{
    public string ExpenseType { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Amount { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

public class GetExpensesInput : DefaultInput
{
    public long? BranchId { get; set; }
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public string? ExpenseType { get; set; }
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }
}

public class ExpenseSummaryDto
{
    public long? BranchId { get; set; }
    public string? BranchName { get; set; }
    public int Month { get; set; }
    public int Year { get; set; }
    public string? ExpenseType { get; set; }
    public decimal? TotalAmount { get; set; }
    public int? ExpenseCount { get; set; }
}
