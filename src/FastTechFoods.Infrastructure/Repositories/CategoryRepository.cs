using FastTechFoods.Application.Interfaces;
using FastTechFoods.Domain.Entities;
using FastTechFoods.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.Infrastructure.Repositories;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _context;

    public CategoryRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Category>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Categories
            .AsNoTracking()
            .Where(c => c.IsActive)
            .OrderBy(c => c.Name)
            .ToListAsync(ct);
    }

    public async Task<Category?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Categories
            .FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<Category> CreateAsync(Category category, CancellationToken ct)
    {
        await _context.Categories.AddAsync(category, ct);
        await _context.SaveChangesAsync(ct);
        return category;
    }

    public async Task<Category> UpdateAsync(Category category, CancellationToken ct)
    {
        _context.Categories.Update(category);
        await _context.SaveChangesAsync(ct);
        return category;
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        var category = await _context.Categories.FindAsync(new object[] { id }, ct);
        if (category == null)
            return false;

        // Verificar se há produtos vinculados a esta categoria
        var hasProducts = await _context.Products
            .AnyAsync(p => p.CategoryId == id, ct);

        if (hasProducts)
        {
            // Soft delete - apenas desativa a categoria
            category.SetActive(false);
            _context.Categories.Update(category);
        }
        else
        {
            // Hard delete se não há produtos vinculados
            _context.Categories.Remove(category);
        }

        await _context.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken ct)
    {
        return await _context.Categories
            .AnyAsync(c => c.Id == id && c.IsActive, ct);
    }
}