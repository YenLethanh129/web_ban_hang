namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record OrderDetailDto(long Id, int ProductId, int Quantity, decimal UnitPrice, decimal TotalPrice, string? Notes)
{
    public string ProductName { get; set; } = string.Empty;
}
