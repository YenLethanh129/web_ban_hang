using System.ComponentModel.DataAnnotations;

namespace Dashboard.BussinessLogic.Dtos.AuthDtos;

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
