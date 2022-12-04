using Core.Interfaces;
using Infrastructure.Repositories;

namespace Api.Extensions;

public static class ShopServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartService, CartService>();
        return services;
    }
}