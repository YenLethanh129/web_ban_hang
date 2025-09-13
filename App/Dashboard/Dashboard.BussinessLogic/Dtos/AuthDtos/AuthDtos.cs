namespace Dashboard.BussinessLogic.Dtos.AuthDtos;

public class UserDto
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public bool IsActive { get; set; }
    public long RoleId { get; set; }
    public string RoleName { get; set; } = string.Empty;
    public long? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public long? PositionId { get; set; }
    public long? BranchId { get; set; }
    public string? BranchName { get; set; }
    public List<string> Permissions { get; set; } = new();
    public DateTime CreatedAt { get; set; }
}

public class CreateUserInput
{
    public long? EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public long RoleId { get; set; }
    public string Password { get; set; } = string.Empty;
    public bool IsActive { get; set; } = true;
}

public class UpdateUserInput
{
    public long Id { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Address { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public long? RoleId { get; set; }
    public long? EmployeeId { get; set; }
    public bool? IsActive { get; set; }
}

public class GetUsersInput
{
    public string? SearchText { get; set; }
    public string? RoleName { get; set; }
    public long? RoleId { get; set; }
    public long? BranchId { get; set; }
    public bool? IsActive { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}

public class RoleDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public List<PermissionDto> Permissions { get; set; } = [];
    public int UserCount { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class PermissionDto
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Resource { get; set; } = string.Empty;
    public string Action { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

public class SessionDto
{
    public string SessionId { get; set; } = string.Empty;
    public long UserId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public long? EmployeeId { get; set; }
    public long? PositionId { get; set; }
    public long? BranchId { get; set; }
    public string? BranchName { get; set; }
    public List<string> Permissions { get; set; } = new();
    public DateTime LoginTime { get; set; }
    public bool IsExpired => DateTime.Now.Subtract(LoginTime).TotalHours > 8; // 8 hour timeout
}

public class AuthenticationResult
{
    public bool IsSuccess { get; set; }
    public string? ErrorMessage { get; set; }
    public SessionDto? Session { get; set; }
}
