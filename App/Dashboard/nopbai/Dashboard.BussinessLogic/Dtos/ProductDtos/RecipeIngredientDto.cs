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
    }
}