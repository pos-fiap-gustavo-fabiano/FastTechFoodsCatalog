using FastTechFoods.Application.DTOs;
using FastTechFoodsOrder.Shared.Results;

namespace FastTechFoods.Application.Interfaces;

public interface IProductService
{
    Task<Result<IEnumerable<ProductDto>>> GetAllAsync(Guid? categoryId, string? search, CancellationToken ct);
    Task<Result<ProductDto>> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Result<ProductDto>> CreateAsync(CreateProductRequest request, CancellationToken ct);
    Task<Result<ProductDto>> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken ct);
    Task<Result<ProductDto>> UpdateAvailabilityAsync(Guid id, bool availability, CancellationToken ct);
    Task<Result> DeleteAsync(Guid id, CancellationToken ct);
}
