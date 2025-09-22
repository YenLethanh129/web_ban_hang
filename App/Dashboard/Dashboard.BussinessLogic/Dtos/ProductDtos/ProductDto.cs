namespace Dashboard.BussinessLogic.Dtos.ProductDtos;

public class ProductDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public long? CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public long? TaxId { get; set; }
    public string? TaxName { get; set; }
    public string? Thumbnail { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<ProductImageDto> Images { get; set; } = new();
    public int SoldQuantity { get; set; }
}


public class ProductDetailDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public long? CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public long? TaxId { get; set; }
    public string? TaxName { get; set; }
    public string? Thumbnail { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public List<ProductImageDto> Images { get; set; } = new();
    public List<RecipeDto> Recipes { get; set; } = new();
    public List<ProductRecipeDto> ProductRecipes { get; set; } = new();
}

public record ProductImageDto(long Id)
{
    public long? ProductId { get; set; }
    public string ImageUrl { get; set; } = string.Empty;
}

public class ProductRecipeDto
{
    public long Id { get; set; }
    public long ProductId { get; set; }
    public long IngredientId { get; set; }
    public string IngredientName { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class RecipeDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public decimal ServingSize { get; set; } = 1;
    public string Unit { get; set; } = "portion";
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<RecipeIngredientDto> RecipeIngredients { get; set; } = new();
}