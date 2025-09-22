namespace Dashboard.BussinessLogic.Dtos.ProductDtos
{
    public class CreateRecipeInput
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public long ProductId { get; set; }
        public decimal ServingSize { get; set; } = 1;
        public string Unit { get; set; } = "portion";
        public bool IsActive { get; set; } = true;
        public string? Notes { get; set; }
    }
}