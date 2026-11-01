using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.DTOs.ProductDTOs;

public class TopProductDTO
{
    public int ProductId { get; set; }
    public string Name { get; set; } = default!;
    public int TotalQuantity { get; set; }
}
