using System.ComponentModel.DataAnnotations;

namespace Dashboard.BussinessLogic.Dtos.CustomerDto;

public class UpdateCustomerInput
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [EmailAddress]
    [StringLength(100)]
    public string Email { get; set; } = string.Empty;

    [Required]
    [Phone]
    [StringLength(20)]
    public string PhoneNumber { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Address { get; set; }

    public DateTime? DateOfBirth { get; set; }

    public bool IsActive { get; set; }
}