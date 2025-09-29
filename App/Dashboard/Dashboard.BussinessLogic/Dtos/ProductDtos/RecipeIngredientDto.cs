namespace Dashboard.BussinessLogic.Dtos.ProductDtos
{
    public class RecipeIngredientDto
    {
        public long Id { get; set; }
        public long RecipeId { get; set; }
        public long IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public decimal Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal? WastePercentage { get; set; } = 0;
        public decimal ActualQuantityNeeded => Quantity * (1 + (WastePercentage ?? 0) / 100);
        public string? Notes { get; set; }
        public bool IsOptional { get; set; } = false;
        public int SortOrder { get; set; } = 0;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class CreateRecipeIngredientInput
    {
        public long IngredientId { get; set; }
        public decimal Quantity { get; set; } = 1;
        public decimal? WastePercentage { get; set; } = 0;
        public string? Notes { get; set; }
        public bool IsOptional { get; set; } = false;
        public int SortOrder { get; set; } = 0;
    }

    public class UpdateRecipeIngredientInput
    {
        public long Id { get; set; }
        public decimal Quantity { get; set; }
        public decimal? WastePercentage { get; set; }
        public string? Notes { get; set; }
        public bool IsOptional { get; set; }
        public int SortOrder { get; set; }
    }
}