using System.ComponentModel.DataAnnotations;

namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public class UpdateOrderInput
{
    [Required]
    public long Id { get; set; }

    public int? OrderStatusId { get; set; }
    public int? PaymentStatusId { get; set; }
    public int? DeliveryStatusId { get; set; }
    public decimal? DiscountAmount { get; set; }
    public string? Notes { get; set; }

    public List<UpdateOrderDetailInput>? OrderDetails { get; set; }
}
