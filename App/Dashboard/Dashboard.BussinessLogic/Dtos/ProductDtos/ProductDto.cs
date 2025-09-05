namespace Dashboard.BussinessLogic.Dtos.ProductDtos;

public record ProductDto(long Id, string? Description, decimal Price, int CategoryId, bool IsActive, DateTime CreatedDate, DateTime? LastModifiedDate)
{
    public string Name { get; set; } = string.Empty;
    public string CategoryName { get; set; } = string.Empty;
    public List<ProductImageDto> Images { get; set; } = new();
}
