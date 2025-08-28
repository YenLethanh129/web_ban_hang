namespace Dashboard.BussinessLogic.Dtos.ProductDtos;
public class GetProductsInput : DefaultInput
{
    public string? Name { get; init; }
    public int? CategoryId { get; init; }
    public bool? IsActive { get; init; }
    public decimal? MinPrice { get; init; }
    public decimal? MaxPrice { get; init; }
    public string? SortBy { get; init; }
    public bool IsDescending { get; init; } = false;

    public GetProductsInput(
        string? name = null,
        int? categoryId = null,
        bool? isActive = null,
        decimal? minPrice = null,
        decimal? maxPrice = null,
        string? sortBy = null)
    {
        Name = name;
        CategoryId = categoryId;
        IsActive = isActive;
        MinPrice = minPrice;
        MaxPrice = maxPrice;
        SortBy = sortBy;
    }
}
