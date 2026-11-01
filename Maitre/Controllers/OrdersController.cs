using Maitre.BL.DTOs.Commons;
using Maitre.BL.DTOs.OrderDTOs;
using Maitre.BL.DTOs.ProductDTOs;
using Maitre.BL.Services.Interfaces;
using Maitre.Core.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maitre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController(IOrderService _orderService) : ControllerBase
    {
        [HttpPost("orders")]
        public async Task<IActionResult> Create(
        int restaurantId, [FromBody] CreateOrderDTO dto, CancellationToken ct)
        {
            var result = await _orderService.CreateAsync(restaurantId, dto, ct);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPatch("orders/{orderId:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int restaurantId, int orderId, [FromBody] UpdateOrderStatusDTO dto, CancellationToken ct)
        {
            var result = await _orderService.UpdateStatusAsync(restaurantId, orderId, dto, ct);
            return Ok(result);
        }

        [HttpGet("orders")]
        public async Task<IActionResult> GetAll(
            int restaurantId,
            [FromQuery] OrderStatus? status,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            var result = await _orderService.GetAllAsync(restaurantId, status, page, pageSize, ct);
            return Ok(result);
        }

        [HttpGet("top-products")]
        public async Task<IActionResult> GetTopProducts(int restaurantId, CancellationToken ct)
        {
            var result = await _orderService.GetTopProductsAsync(restaurantId, ct);
            return Ok(result);
        }

        [HttpDelete("orders/{orderId:int}")]
        public async Task<IActionResult> Delete(int restaurantId, int orderId, CancellationToken ct)
        {
            await _orderService.DeleteAsync(restaurantId, orderId, ct);
            return NoContent();
        }
    }
}
