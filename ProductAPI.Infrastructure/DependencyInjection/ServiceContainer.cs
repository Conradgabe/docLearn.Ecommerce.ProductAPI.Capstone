using EcommerceSharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductAPI.Application.Interfaces;
using ProductAPI.Infrastructure.Data;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastrutureService(this IServiceCollection services, IConfiguration config)
        {
            // Add database connectivity and Authentication scheme
            SharedServiceContainer.AddSharedServices<ProductDBContext>(services, config, config["MySerilog:FileName"]!);

            // Create Dependency Injection for ProductDBContext
            services.AddScoped<IProduct, ProductRepository>();

            return services;
        }

        public static IApplicationBuilder UseInfrastructurePolicy(this IApplicationBuilder app)
        {
            /** 
               Register the middleware
               Global Exceptions: handles all external Error
               GateWay Blocks: blocks all external API calls
            **/

            SharedServiceContainer.UseSharedPolicies(app);

            return app;
        }
    }
}
