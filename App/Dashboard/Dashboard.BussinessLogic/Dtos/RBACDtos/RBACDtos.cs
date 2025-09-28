using System.ComponentModel.DataAnnotations;

namespace Dashboard.BussinessLogic.Dtos.RBACDtos;

public class CreateUserInput
{
    public long EmployeeId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public long RoleId { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateUserInput
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string? Password { get; set; }
    public long? RoleId { get; set; }
    public bool? IsActive { get; set; }
    public long EmployeeId { get; set; }
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
    public DateTime? LastModified { get; set; }
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
    public string? Token { get; set; }
    public long? UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public List<string> Permissions { get; set; } = new();
    public DateTime Expiration { get; set; }
}

public class UserDto
{
    public long Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public bool IsActive { get; set; }
    public long RoleId { get; set; }
    public string? Password { get; set; } = string.Empty;
    public string RoleName { get; set; } = string.Empty;
    public string? EmployeeName { get; set; } = string.Empty;
    public long? EmployeeId { get; set; }
    public DateTime CreatedAt { get; set; }
}
public class ChangePasswordInput
{
    [Required]
    public long UserId { get; set; }

    [Required]
    public string CurrentPassword { get; set; } = null!;

    [Required]
    [StringLength(200, MinimumLength = 6)]
    public string NewPassword { get; set; } = null!;

    [Required]
    [Compare(nameof(NewPassword))]
    public string ConfirmPassword { get; set; } = null!;
}

public class LoginInput
{
    public string Username { get; set; } = null!;
    public string Password { get; set; } = null!;
}

public class LoginResult
{
    public string Username { get; set; } = string.Empty;
    public string Token { get; set; } = null!;
    public DateTime ExpirationDate { get; set; }
    public string Role { get; set; } = null!;
    public List<string> Permissions { get; set; } = new();
}
