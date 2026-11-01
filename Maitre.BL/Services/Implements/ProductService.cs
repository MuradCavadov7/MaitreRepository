using AutoMapper;
using Maitre.BL.DTOs.ProductDTOs;
using Maitre.BL.Exceptions.General;
using Maitre.BL.Services.Interfaces;
using Maitre.Core.Entities;
using Maitre.Core.Repostories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Services.Implements;

public class ProductService(IProductRepository _productRepository, IMapper _mapper) : IProductService
{ 
    public async Task<GetProductDTO> CreateAsync(int restaurantId, CreateProductDTO dto, CancellationToken ct = default)
    {
        var product = _mapper.Map<Core.Entities.Product>(dto);
        product.RestaurantId = restaurantId;
        product.IsAvailable = true;
        await _productRepository.AddAsync(product, ct);
        await _productRepository.SaveChangesAsync(ct);

        return _mapper.Map<GetProductDTO>(product);

    }

    public async Task DeleteAsync(int restaurantId, int productId, CancellationToken ct = default)
    {
        var product = _productRepository.GetByIdAsync(productId, ct).Result;
        if (product == null || product.RestaurantId != restaurantId)
        {
            throw new NotFoundException($"Product not found :{productId}.");
        }
        _productRepository.Remove(product);
        await _productRepository.SaveChangesAsync();
    }

    public async Task<List<GetProductDTO>> GetAllAsync(int restaurantId, bool onlyAvailable = false, CancellationToken ct = default)
    {
        var products = await _productRepository.GetAllAsync(
        p => p.RestaurantId == restaurantId && (!onlyAvailable || p.IsAvailable),
        ct: ct);

        return _mapper.Map<List<GetProductDTO>>(
            products.OrderBy(p => p.Category).ThenBy(p => p.Name).ToList());
    }

    public async Task<GetProductDTO> UpdateAsync(int restaurantId, int productId, UpdateProductDTO dto, CancellationToken ct = default)
    {
        var product = await _productRepository.GetByIdAsync(productId, ct);
        if (product == null || product.RestaurantId != restaurantId)
        {
            throw new NotFoundException($"Product not found :{productId}.");
        }
        _mapper.Map(dto, product);

        if (dto.Price.HasValue)
            product.Price = dto.Price.Value;

        if (dto.IsAvailable.HasValue)
            product.IsAvailable = dto.IsAvailable.Value;

        await _productRepository.SaveChangesAsync(ct);

        return _mapper.Map<GetProductDTO>(product);
    }
}
