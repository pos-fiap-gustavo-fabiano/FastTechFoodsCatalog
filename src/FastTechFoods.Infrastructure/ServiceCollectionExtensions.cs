using FastTechFoods.Application.Interfaces;
using FastTechFoods.Application.Validators;
using FastTechFoods.Application.Services;
using FastTechFoods.Domain.Repositories;
using FastTechFoods.Infrastructure.Data;
using FastTechFoods.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using FluentValidation;
using Azure.Storage.Blobs;
using FastTechFoods.Infrastructure.Services;

namespace FastTechFoods.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<ICategoryService, CategoryService>();
        services.AddScoped<IProductRepository, ProductRepository>();
        services.AddScoped<ICategoryRepository, CategoryRepository>();
        services.AddValidatorsFromAssemblyContaining<CreateProductRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<UpdateProductRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<CreateCategoryRequestValidator>();
        services.AddSingleton(x => new BlobServiceClient(Environment.GetEnvironmentVariable("BLOB_CONNECTION"))); // Use your blob storage connection string
    services.AddScoped<IBlobStorageService, BlobStorageService>();
        return services;
    }
}
