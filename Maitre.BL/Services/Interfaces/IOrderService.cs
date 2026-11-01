using Maitre.BL.DTOs.Commons;
using Maitre.BL.DTOs.OrderDTOs;
using Maitre.BL.DTOs.ProductDTOs;
using Maitre.Core.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Services.Interfaces;

public interface IOrderService
{
    Task<GetOrderDTO> CreateAsync(int restaurantId, CreateOrderDTO dto, CancellationToken ct = default);
    Task<GetOrderDTO> UpdateStatusAsync(int restaurantId, int orderId, UpdateOrderStatusDTO dto, CancellationToken ct = default);
    Task<PageDTO<GetOrderDTO>> GetAllAsync(int restaurantId, OrderStatus? status, int page, int pageSize, CancellationToken ct = default);
    Task<List<TopProductDTO>> GetTopProductsAsync(int restaurantId, CancellationToken ct = default);
    Task DeleteAsync(int restaurantId, int orderId, CancellationToken ct = default);
}
