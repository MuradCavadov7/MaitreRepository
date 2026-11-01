using AutoMapper;
using Maitre.BL.DTOs.Commons;
using Maitre.BL.DTOs.OrderDTOs;
using Maitre.BL.DTOs.ProductDTOs;
using Maitre.BL.Exceptions.General;
using Maitre.BL.Services.Interfaces;
using Maitre.Core.Entities;
using Maitre.Core.Enums;
using Maitre.Core.Repostories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Services.Implements
{
    public class OrderService(IOrderItemRepository _orderItemsRepository,IOrderRepository _orderRepository,IProductRepository _productRepository,IMapper _mapper) : IOrderService
    {
        private const int MaxPageSize = 50;
        private const int MaxQuantityPerProduct = 100;
        private const int TopProductsCount = 5;

        private static readonly Dictionary<OrderStatus, OrderStatus[]> AllowedTransitions = new()
        {
            [OrderStatus.Pending] = new[] { OrderStatus.Preparing, OrderStatus.Cancelled },
            [OrderStatus.Preparing] = new[] { OrderStatus.Ready },
            [OrderStatus.Ready] = new[] { OrderStatus.Completed },
            [OrderStatus.Completed] = Array.Empty<OrderStatus>(),
            [OrderStatus.Cancelled] = Array.Empty<OrderStatus>()
        };

        public async Task<GetOrderDTO> CreateAsync(int restaurantId, CreateOrderDTO dto, CancellationToken ct = default)
        {
            var sum = dto.OrderItems.GroupBy(oi => oi.ProductId)
                .Select(g => new { ProductId = g.Key, TotalQuantity = g.Sum(oi => oi.Quantity) })
                .ToList();
            if (sum.Any(item => item.TotalQuantity > MaxQuantityPerProduct))
            {
                throw new BadRequestException($"Total quantity for a product cannot exceed {MaxQuantityPerProduct}.");
            }
            var productIds = sum.Select(item => item.ProductId).ToList();
            var products = await _productRepository.GetAllAsync(
           p => productIds.Contains(p.Id) && p.RestaurantId == restaurantId,
           ct: ct);

            var missing = productIds.Except(products.Select(p => p.Id)).ToList();
            if (missing.Count > 0)
                throw new NotFoundException(
                    $"Bu restoranda məhsul tapılmadı: {string.Join(", ", missing)}");

            var unavailable = products.Where(p => !p.IsAvailable).ToList();
            if (unavailable.Count > 0)
                throw new ExistException(
                    $"Satışda olmayan məhsullar: {string.Join(", ", unavailable.Select(p => p.Name))}");

            var order = new Order
            {
                RestaurantId = restaurantId,
                Status = OrderStatus.Pending
            };

            foreach (var line in sum)
            {
                var product = products.First(p => p.Id == line.ProductId);
                order.OrderItems.Add(new OrderItem
                {
                    ProductId = product.Id,
                    ProductName = product.Name,
                    UnitPrice = product.Price,
                    Quantity = line.TotalQuantity
                });

            }
            order.TotalPrice = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);

            await _orderRepository.AddAsync(order, ct);
            await _orderRepository.SaveChangesAsync(ct);

            return _mapper.Map<GetOrderDTO>(order);
        }
        public async Task DeleteAsync(int restaurantId, int orderId, CancellationToken ct = default)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(restaurantId, orderId, ct);

            if (order is null)
                throw new NotFoundException($"Order not found: {orderId}");

            if (order.Status != OrderStatus.Cancelled)
                throw new ExistException(
                    $"Only cancelled orders can be deleted. Current status: {order.Status}.");

            _orderRepository.Remove(order); 
            await _orderRepository.SaveChangesAsync(ct);
        }

        public async Task<PageDTO<GetOrderDTO>> GetAllAsync(int restaurantId, OrderStatus? status, int page, int pageSize, CancellationToken ct = default)
        {
            if (page < 1)
                throw new BadRequestException("Page 1-dən kiçik ola bilməz.");

            if (pageSize < 1 || pageSize > MaxPageSize)
                throw new BadRequestException($"PageSize 1 ilə {MaxPageSize} arasında olmalıdır.");

            if (status.HasValue && !Enum.IsDefined(status.Value))
                throw new BadRequestException("Yanlış status dəyəri.");

            var (items, total) = await _orderRepository.GetPagedByRestaurantAsync(restaurantId, status, page, pageSize, ct);

            return new PageDTO<GetOrderDTO>
            {
                Items = _mapper.Map<List<GetOrderDTO>>(items),
                PageNumber = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<List<TopProductDTO>> GetTopProductsAsync(int restaurantId, CancellationToken ct = default)
        {
            var rows = await _orderItemsRepository.GetTopProductsAsync(restaurantId, TopProductsCount, ct);

            return rows.Select(r => new TopProductDTO
            {
                ProductId = r.ProductId,
                Name = r.ProductName,
                TotalQuantity = r.TotalQuantity
            }).ToList();
        }

        public async Task<GetOrderDTO> UpdateStatusAsync(int restaurantId, int orderId, UpdateOrderStatusDTO dto, CancellationToken ct = default)
        {
            var order = await _orderRepository.GetByIdWithItemsAsync(restaurantId, orderId, ct);

            if (order is null)
                throw new NotFoundException($"Order not found: {orderId}");

            if (!AllowedTransitions[order.Status].Contains(dto.Status))
                throw new ExistException(
                    $"Status {order.Status} → {dto.Status} can't be changed.");

            order.Status = dto.Status;
            await _orderRepository.SaveChangesAsync(ct);

            return _mapper.Map<GetOrderDTO>(order);
        }
    }
}
