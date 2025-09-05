namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class SupplierDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    // Summary information
    public int TotalOrders { get; set; }
    public decimal TotalAmount { get; set; }
    public int ActiveIngredients { get; set; }
}
