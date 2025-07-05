using FastTechFoods.Application.Interfaces;
using FastTechFoods.Application.Validators;
using FastTechFoods.Application.Services;
using FastTechFoods.Domain.Repositories;
using FastTechFoods.Infrastructure.Data;
using FastTechFoods.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;

namespace FastTechFoods.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<IProductService, ProductService>();
        services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
        return services;
    }
}
