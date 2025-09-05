using Dashboard.Common.Enums;

namespace Dashboard.BussinessLogic.Dtos.ReportDtos;

public class GetRevenueReportInput : DefaultInput
{
    public DateTime? FromDate { get; set; }
    public DateTime? ToDate { get; set; }
    public long? BranchId { get; set; }
    public ReportPeriodEnum Period { get; set; } = ReportPeriodEnum.Daily;
}
