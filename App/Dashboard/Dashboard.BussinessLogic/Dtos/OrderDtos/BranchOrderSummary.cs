namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record BranchOrderSummary(int BranchId, int OrderCount, decimal Revenue)
{
    public string BranchName { get; set; } = string.Empty;
}