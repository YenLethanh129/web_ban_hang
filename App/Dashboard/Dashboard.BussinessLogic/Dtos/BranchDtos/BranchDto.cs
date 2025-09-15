namespace Dashboard.BussinessLogic.Dtos.BranchDtos;

public class BranchDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Manager { get; set; } = string.Empty;
}

public class PositionDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool NeedSchedule { get; set; }
}