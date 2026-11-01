using Maitre.BL.DTOs.ProductDTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Services.Interfaces;

public interface IProductService
{
    Task<GetProductDTO> CreateAsync(int restaurantId, CreateProductDTO dto, CancellationToken ct = default);
    Task<GetProductDTO> UpdateAsync(int restaurantId, int productId, UpdateProductDTO dto, CancellationToken ct = default);
    Task<List<GetProductDTO>> GetAllAsync(int restaurantId, bool onlyAvailable = false, CancellationToken ct = default);
    Task DeleteAsync(int restaurantId, int productId, CancellationToken ct = default);

}
