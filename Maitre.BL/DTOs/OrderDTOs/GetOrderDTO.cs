using Maitre.BL.DTOs.OrderItemDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.DTOs.OrderDTOs;

public class GetOrderDTO
{
    public int Id { get; set; }
    public int RestaurantId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; } = default!;
    public DateTime CreatedTime { get; set; }
    public List<GetOrderItemDTO> Items { get; set; } = new();

}
