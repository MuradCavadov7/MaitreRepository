using Maitre.BL.DTOs.OrderItemDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.DTOs.OrderDTOs;

public class CreateOrderDTO
{
    public List<CreateOrderItemDTO> OrderItems { get; set; } = new();
}
