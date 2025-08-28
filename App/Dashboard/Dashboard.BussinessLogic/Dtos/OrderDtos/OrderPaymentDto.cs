namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public record OrderPaymentDto(long Id, decimal Amount, DateTime PaymentDate, string? TransactionId, string? Notes)
{
    public string PaymentMethodName { get; set; } = string.Empty;
}
