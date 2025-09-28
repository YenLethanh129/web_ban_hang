namespace Dashboard.BussinessLogic.Dtos.SupplierDtos;

public class UpdateSupplierInput
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? Note { get; set; }
}
