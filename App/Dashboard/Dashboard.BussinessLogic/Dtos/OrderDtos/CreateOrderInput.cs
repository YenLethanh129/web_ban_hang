using System.ComponentModel.DataAnnotations;

namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public class CreateOrderInput
{
    [Required]
    public int CustomerId { get; set; }

    [Required]
    public int BranchId { get; set; }

    [Required]
    public List<CreateOrderDetailInput> OrderDetails { get; set; } = new();

    public decimal DiscountAmount { get; set; } = 0;
    public string? Notes { get; set; }

    [Required]
    public int PaymentMethodId { get; set; }
    public string? PaymentNotes { get; set; }
}
