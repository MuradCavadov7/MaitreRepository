using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.DTOs.ProductDTOs;

public class CreateProductDTO
{
    public string Name { get; set; }  = default!;
    public decimal Price { get; set; }
    public string Category { get; set; } = default!;
}
