using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FastTechFoods.Domain.Entities;
using FastTechFoods.Domain.Repositories;
using FastTechFoodsOrder.Shared.Results;

namespace FastTechFoods.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _repository;
    private readonly ICategoryRepository _categoryRepository;
    private IBlobStorageService _blobStorageService;


    public ProductService(IProductRepository repository, ICategoryRepository categoryRepository, IBlobStorageService blobStorageService)
    {
        _repository = repository;
        _categoryRepository = categoryRepository;
        _blobStorageService = blobStorageService;
    }

    public async Task<Result<IEnumerable<ProductDto>>> GetAllAsync(Guid? categoryId, string? search = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var products = await _repository.SearchAsync(categoryId, search, cancellationToken);
            var productDtos = products.Select(ToDto);
            return Result<IEnumerable<ProductDto>>.Success(productDtos);
        }
        catch (Exception ex)
        {
            return Result<IEnumerable<ProductDto>>.Failure($"Error retrieving products: {ex.Message}", "INTERNAL_ERROR");
        }
    }

    public async Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                return Result<ProductDto>.Failure("Product not found", "PRODUCT_NOT_FOUND");
            
            return Result<ProductDto>.Success(ToDto(product));
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Failure($"Error retrieving product: {ex.Message}", "INTERNAL_ERROR");
        }
    }

    public async Task<Result<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            // Validar se a categoria existe
            var categoryExists = await _categoryRepository.ExistsAsync(request.CategoryId, cancellationToken);
            if (!categoryExists)
            {
                return Result<ProductDto>.Failure($"Category with ID {request.CategoryId} does not exist", "CATEGORY_NOT_FOUND");
            }

            string? imageUrl = null;

            if (request.Image != null)
            {
                imageUrl = await _blobStorageService.UploadImageAsync(request.Image, "fasttechfoods", cancellationToken);
            }
        
            var product = new Product(request.Name, request.Description, request.Price, request.Availability, imageUrl ?? "", request.CategoryId);
            await _repository.AddAsync(product, cancellationToken);
            return Result<ProductDto>.Success(ToDto(product));
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Failure($"Error creating product: {ex.Message}", "INTERNAL_ERROR");
        }
    }

    public async Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                return Result<ProductDto>.Failure("Product not found", "PRODUCT_NOT_FOUND");

            product.Update(request.Name, request.Description, request.Price, request.Availability, request.CategoryId);
            await _repository.UpdateAsync(product, cancellationToken);
            return Result<ProductDto>.Success(ToDto(product));
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Failure($"Error updating product: {ex.Message}", "INTERNAL_ERROR");
        }
    }

    public async Task<Result<ProductDto>> UpdateAvailabilityAsync(Guid id, bool availability, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                return Result<ProductDto>.Failure("Product not found", "PRODUCT_NOT_FOUND");

            product.SetAvailability(availability);
            await _repository.UpdateAsync(product, cancellationToken);
            return Result<ProductDto>.Success(ToDto(product));
        }
        catch (Exception ex)
        {
            return Result<ProductDto>.Failure($"Error updating product availability: {ex.Message}", "INTERNAL_ERROR");
        }
    }

    public async Task<Result> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            var product = await _repository.GetByIdAsync(id, cancellationToken);
            if (product is null)
                return Result.Failure("Product not found", "PRODUCT_NOT_FOUND");

            await _repository.DeleteAsync(product, cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            return Result.Failure($"Error deleting product: {ex.Message}", "INTERNAL_ERROR");
        }
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
