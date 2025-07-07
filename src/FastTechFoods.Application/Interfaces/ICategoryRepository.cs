using FastTechFoods.Domain.Entities;

namespace FastTechFoods.Application.Interfaces;

public interface ICategoryRepository
{
    Task<List<Category>> GetAllAsync(CancellationToken ct);
    Task<Category?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<Category> CreateAsync(Category category, CancellationToken ct);
    Task<Category> UpdateAsync(Category category, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
    Task<bool> ExistsAsync(Guid id, CancellationToken ct);
}