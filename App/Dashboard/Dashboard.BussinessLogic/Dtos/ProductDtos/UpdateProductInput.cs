namespace Dashboard.BussinessLogic.Dtos.ProductDtos;

public record UpdateProductInput(long Id, string? Description, decimal Price, int CategoryId, bool IsActive, long TaxId)
{
    public string Name { get; set; } = string.Empty;
    public List<string?> ImageUrls { get; set; } = new();
}