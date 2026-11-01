using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.DTOs.OrderItemDTOs;

public class GetOrderItemDTO
{
    public string ProductName { get; set; } = default!;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }
}
