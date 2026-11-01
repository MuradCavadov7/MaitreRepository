using AutoMapper;
using Maitre.BL.DTOs.OrderDTOs;
using Maitre.BL.DTOs.OrderItemDTOs;
using Maitre.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.BL.Profiles;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        CreateMap<CreateOrderDTO, Order>();
        CreateMap<CreateOrderItemDTO, OrderItem>();

        CreateMap<OrderItem, GetOrderItemDTO>();

        CreateMap<Order, GetOrderDTO>()
            .ForMember(d => d.Status, o => o.MapFrom(s => s.Status.ToString()))
            .ForMember(d => d.TotalAmount, o => o.MapFrom(s => s.TotalPrice))
            .ForMember(d => d.Items, o => o.MapFrom(s => s.OrderItems));




    }
}
