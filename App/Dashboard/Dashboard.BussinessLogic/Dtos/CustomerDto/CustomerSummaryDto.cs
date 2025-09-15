namespace Dashboard.BussinessLogic.Dtos.CustomerDto;

public class CustomerSummaryDto
{
    public int TotalCustomers { get; set; }
    public int ActiveCustomers { get; set; }
    public int InactiveCustomers { get; set; }
    public int NewCustomersThisMonth { get; set; }
    public decimal AverageOrderValue { get; set; }
    public decimal TotalCustomerSpending { get; set; }
    public IEnumerable<TopCustomerDto> TopCustomers { get; set; } = new List<TopCustomerDto>();
}

public class TopCustomerDto
{
    public long Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public int TotalOrders { get; set; }
    public decimal TotalSpent { get; set; }
    public DateTime? LastOrderDate { get; set; }
}