using System.ComponentModel.DataAnnotations;

namespace Dashboard.BussinessLogic.Dtos.OrderDtos;

public class CreateOrderDetailInput
{
    [Required]
    public int ProductId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    public string? Notes { get; set; }
}