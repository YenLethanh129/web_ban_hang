namespace Dashboard.BussinessLogic.Dtos.ProductDtos
{
    // Input DTOs
    public class GetRecipesInput
    {
        public string? Name { get; set; }
        public long? ProductId { get; set; }
        public bool? IsActive { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}