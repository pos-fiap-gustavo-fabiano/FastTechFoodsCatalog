using FastTechFoods.Application.DTOs;

namespace FastTechFoods.Application.Interfaces;

public interface IProductService
{
    Task<IEnumerable<ProductDto>> GetAllAsync(Guid? categoryId,string? search, CancellationToken ct);
    Task<ProductDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken ct);
    Task<ProductDto?> UpdateAsync(Guid id, UpdateProductRequest request, CancellationToken ct);
    Task<ProductDto?> UpdateAvailabilityAsync(Guid id, bool availability, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}
