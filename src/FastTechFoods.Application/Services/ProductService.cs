using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FastTechFoods.Domain.Entities;
using FastTechFoods.Domain.Repositories;

namespace FastTechFoods.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private IBlobStorageService _blobStorageService;


    public ProductService(IProductRepository repository, ICategoryRepository @object, IBlobStorageService blobStorageService)
    {
        _repository = repository;
        _blobStorageService = blobStorageService;
    }

    public async Task<IEnumerable<ProductDto>> GetAllAsync(Guid? categoryId, string? search = null, CancellationToken cancellationToken = default)
    {
        var products = await _repository.SearchAsync(categoryId, search, cancellationToken);
        return products.Select(ToDto);
    }

    public async Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        return product is null ? null : ToDto(product);
    }

    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        string? imageUrl = null;

        if (request.Image != null)
        {
            imageUrl = await _blobStorageService.UploadImageAsync(request.Image, "fasttechfoods", cancellationToken);

        }
        var product = new Product(request.Name, request.Description, request.Price,request.Availability, imageUrl, request.CategoryId);
        await _repository.AddAsync(product, cancellationToken);
        return ToDto(product);
    }

    public async Task<ProductDto?> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return null;

        product.Update(request.Name, request.Description, request.Price, request.Availability, request.CategoryId);
        await _repository.UpdateAsync(product, cancellationToken);
        return ToDto(product);
    }

    public async Task<ProductDto?> UpdateAvailabilityAsync(Guid id, bool availability, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return null;

        product.SetAvailability(availability);
        await _repository.UpdateAsync(product, cancellationToken);
        return ToDto(product);
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var product = await _repository.GetByIdAsync(id, cancellationToken);
        if (product is null)
            return false;

        await _repository.DeleteAsync(product, cancellationToken);
        return true;
    }

    private static ProductDto ToDto(Product product) => new(
        product.Id,
        product.Name,
        product.Description,
        product.Price,
        product.Availability,
        product.ImageUrl,
        product.CategoryId,
        product.CreatedDate);
}
