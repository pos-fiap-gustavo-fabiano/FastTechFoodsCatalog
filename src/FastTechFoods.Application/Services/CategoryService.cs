using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FastTechFoods.Domain.Entities;

namespace FastTechFoods.Application.Services;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryService(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<List<CategoryDto>> GetAllAsync(CancellationToken ct)
    {
        var categories = await _categoryRepository.GetAllAsync(ct);
        
        return categories.Select(c => new CategoryDto(
            c.Id,
            c.Name,
            c.Description,
            c.IsActive,
            c.CreatedDate
        )).ToList();
    }

    public async Task<CategoryDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        
        if (category == null)
            return null;

        return new CategoryDto(
            category.Id,
            category.Name,
            category.Description,
            category.IsActive,
            category.CreatedDate
        );
    }

    public async Task<CategoryDto> CreateAsync(CreateCategoryRequest request, CancellationToken ct)
    {
        var category = new Category(
            request.Name,
            request.Description,
            request.IsActive
        );

        var createdCategory = await _categoryRepository.CreateAsync(category, ct);

        return new CategoryDto(
            createdCategory.Id,
            createdCategory.Name,
            createdCategory.Description,
            createdCategory.IsActive,
            createdCategory.CreatedDate
        );
    }

    public async Task<CategoryDto?> UpdateAsync(Guid id, CreateCategoryRequest request, CancellationToken ct)
    {
        var category = await _categoryRepository.GetByIdAsync(id, ct);
        
        if (category == null)
            return null;

        category.Update(request.Name, request.Description, request.IsActive);

        var updatedCategory = await _categoryRepository.UpdateAsync(category, ct);

        return new CategoryDto(
            updatedCategory.Id,
            updatedCategory.Name,
            updatedCategory.Description,
            updatedCategory.IsActive,
            updatedCategory.CreatedDate
        );
    }

    public async Task<bool> DeleteAsync(Guid id, CancellationToken ct)
    {
        return await _categoryRepository.DeleteAsync(id, ct);
    }
}