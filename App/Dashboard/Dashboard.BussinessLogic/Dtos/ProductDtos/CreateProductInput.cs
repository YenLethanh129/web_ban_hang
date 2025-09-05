namespace Dashboard.BussinessLogic.Dtos.ProductDtos;

public record CreateProductInput(string? Description, decimal Price, int CategoryId)
{
    public string Name { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
    public List<string> ImageUrls { get; set; } = [];
}
