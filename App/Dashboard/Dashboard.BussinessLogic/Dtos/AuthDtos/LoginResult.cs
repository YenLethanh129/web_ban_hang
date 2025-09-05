namespace Dashboard.BussinessLogic.Dtos.AuthDtos;

public class LoginResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public UserSessionDto? User { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiry { get; set; }
}
public class UserSessionDto
{
    public long Id { get; set; }
    public string PhoneNumber { get; set; } = null!;
    public string? Fullname { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; } = null!;
    public List<string> Permissions { get; set; } = [];
    public long? EmployeeId { get; set; }
    public bool IsActive { get; set; }
}
