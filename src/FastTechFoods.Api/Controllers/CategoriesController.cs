using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.Api.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        // GET /api/categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CategoryDto>>> GetAllCategories(CancellationToken ct)
        {
            var categories = await _categoryService.GetAllAsync(ct);
            return Ok(categories);
        }

        // GET /api/categories/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDto>> GetCategoryById(Guid id, CancellationToken ct)
        {
            var category = await _categoryService.GetByIdAsync(id, ct);
            if (category == null)
                return NotFound();
            return Ok(category);
        }

        // POST /api/categories
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<CategoryDto>> CreateCategory([FromBody] CreateCategoryRequest request, [FromServices] IValidator<CreateCategoryRequest> validator, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var created = await _categoryService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetCategoryById), new { id = created.Id }, created);
        }

        // PUT /api/categories/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<CategoryDto>> UpdateCategory(Guid id, [FromBody] CreateCategoryRequest request, [FromServices] IValidator<CreateCategoryRequest> validator, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var updated = await _categoryService.UpdateAsync(id, request, ct);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        // DELETE /api/categories/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteCategory(Guid id, CancellationToken ct)
        {
            var deleted = await _categoryService.DeleteAsync(id, ct);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}