namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class CreateSupplierInput
{
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
}
