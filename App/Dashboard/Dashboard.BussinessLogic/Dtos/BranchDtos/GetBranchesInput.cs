namespace Dashboard.BussinessLogic.Dtos.BranchDtos;

public class GetBranchesInput : DefaultInput
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Address { get; set; }
    public string? Phone { get; set; }
    public string? Manager { get; set; }
}
