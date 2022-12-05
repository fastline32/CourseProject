using Api.Helpers;
using Core.Interfaces;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace Api.Extensions;

public static class ShopServiceCollectionExtension
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddScoped<ITypeRepository, TypeRepository>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICartService, CartService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddTransient<IEmailSender, EmailSender>();
        services.AddScoped<IInquiryHeaderRepository, InquiryHeaderRepository>();
        services.AddScoped<IInquiryDetailsRepository, InquiryDetailsRepository>();
        return services;
    }
}