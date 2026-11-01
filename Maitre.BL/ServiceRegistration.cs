using Maitre.BL.Profiles;
using Maitre.BL.Services.Implements;
using Maitre.BL.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;

namespace Maitre.BL
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddBusinessServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddFluentValidationAutoValidation();
            services.AddAutoMapper(cfg =>
            {
                cfg.LicenseKey = configuration["AutoMapper:LicenseKey"];
            }, typeof(OrderProfile));

            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderService, OrderService>();

            return services;
        }
    }
}
