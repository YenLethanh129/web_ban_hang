using System.ComponentModel.DataAnnotations;

namespace Dashboard.BussinessLogic.Dtos.AuthDtos;

public class LoginInput
{
    [Required]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = null!;

    [Required]
    [StringLength(200)]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }
}

