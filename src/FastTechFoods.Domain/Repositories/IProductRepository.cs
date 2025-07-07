using FastTechFoods.Domain.Entities;

namespace FastTechFoods.Domain.Repositories;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Product>> SearchAsync(Guid? categoryId, string? search = null, CancellationToken cancellationToken = default);
    Task<Product> AddAsync(Product product, CancellationToken cancellationToken = default);
    Task <Product>UpdateAsync(Product product, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(Product product, CancellationToken cancellationToken = default);
}
