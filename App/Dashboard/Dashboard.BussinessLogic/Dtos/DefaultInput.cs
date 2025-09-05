using Dashboard.Common.Enums;

namespace Dashboard.BussinessLogic.Dtos;

public class DefaultInput
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public string? SortByDefault { get; set; }
    public OrderByEnum? OrderByDefault { get; set; }
}
