using Maitre.BL.DTOs.ProductDTOs;
using Maitre.BL.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Maitre.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController(IProductService _productService) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Create(
        int restaurantId, [FromBody] CreateProductDTO dto, CancellationToken ct)
        {
            var result = await _productService.CreateAsync(restaurantId, dto, ct);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPatch("{productId:int}")]
        public async Task<IActionResult> Update(
            int restaurantId, int productId, [FromBody] UpdateProductDTO dto, CancellationToken ct)
        {
            var result = await _productService.UpdateAsync(restaurantId, productId, dto, ct);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            int restaurantId, [FromQuery] bool onlyAvailable = false, CancellationToken ct = default)
        {
            var result = await _productService.GetAllAsync(restaurantId, onlyAvailable, ct);
            return Ok(result);
        }

        [HttpDelete("{productId:int}")]
        public async Task<IActionResult> Delete(int restaurantId, int productId, CancellationToken ct)
        {
            await _productService.DeleteAsync(restaurantId, productId, ct);
            return NoContent();
        }
    }
}
