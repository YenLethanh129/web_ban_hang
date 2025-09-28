using Dashboard.Common.Enums;

namespace Dashboard.BussinessLogic.Dtos.CustomerDto;

public class GetCustomersInput : DefaultInput
{
    public string? SearchTerm { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public bool? IsActive { get; set; }
    public bool IsDescending { get; set; } = false;
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public decimal? MinSpent { get; set; }
    public decimal? MaxSpent { get; set; }
    public SortByEnum? SortBy { get; set; }
}

