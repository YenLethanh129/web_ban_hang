namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public class UpdateOrderDetailInput
{
    public int? Id { get; set; } 
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public string? Notes { get; set; }
    public bool IsDeleted { get; set; } = false;
}