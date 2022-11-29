using Core.Interfaces;
using Infrastructure.Repositories;

namespace Api.Extensions;

public static class ShopServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        return services;
    }
}