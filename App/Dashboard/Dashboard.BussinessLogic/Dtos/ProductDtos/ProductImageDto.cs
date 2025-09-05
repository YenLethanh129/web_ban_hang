namespace Dashboard.BussinessLogic.Dtos.ProductDtos;

public record ProductImageDto(long Id, bool IsPrimary)
{
    public string ImageUrl { get; set; } = string.Empty;
}