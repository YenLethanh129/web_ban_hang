namespace Dashboard.BussinessLogic.Dtos.ProductDtos;

public class TaxDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal TaxRate { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
public record CreateTaxInput(string Name, decimal TaxRate, string? Description = null);
public record UpdateTaxInput(long Id, string Name, decimal TaxRate, string? Description = null);