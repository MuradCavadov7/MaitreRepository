using AutoMapper;
using Maitre.BL.DTOs.ProductDTOs;
using Maitre.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDTO, Product>()
           .ForMember(d => d.Id, o => o.Ignore())
           .ForMember(d => d.RestaurantId, o => o.Ignore())   
           .ForMember(d => d.IsAvailable, o => o.Ignore())
           .ForMember(d => d.Restaurant, o => o.Ignore());

        CreateMap<Product, GetProductDTO>();

        CreateMap<UpdateProductDTO, Product>()
            .ForMember(d => d.Id, o => o.Ignore())
            .ForMember(d => d.RestaurantId, o => o.Ignore())
            .ForMember(d => d.IsAvailable, o => o.Ignore())
            .ForMember(d => d.Restaurant, o => o.Ignore());

    }
}
