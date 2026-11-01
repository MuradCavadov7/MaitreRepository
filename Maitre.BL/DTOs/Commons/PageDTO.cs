using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.DTOs.Commons;

public class PageDTO<T>
{
    public List<T> Items { get; set; } = new();
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
}
