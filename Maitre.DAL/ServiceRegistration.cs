using Maitre.Core.Repostories;
using Maitre.DAL.GenericRepositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Maitre.DAL;

public static class ServiceRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    { 
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IOrderRepository, OrderRepository>();
        services.AddScoped<IOrderItemRepository, OrderItemRepository>();

        return services;
    }
}
