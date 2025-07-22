using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /api/products/{id}
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductDto>> GetProductById(Guid id, CancellationToken ct)
        {
            var product = await _productService.GetByIdAsync(id, ct);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // GET /api/products/{id}
        [HttpGet]
        public async Task<ActionResult<ProductDto>> GetAll(CancellationToken ct)
        {
            var product = await _productService.GetAllAsync(null, "",ct);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        // POST /api/products
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<ActionResult<ProductDto>> CreateProduct([FromForm] CreateProductRequest request, [FromServices] IValidator<CreateProductRequest> validator, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var created = await _productService.CreateAsync(request, ct);
            return CreatedAtAction(nameof(GetProductById), new { id = created.Id }, created);
        }

        // PUT /api/products/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<ProductDto>> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request, [FromServices] IValidator<UpdateProductRequest> validator, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var updated = await _productService.UpdateAsync(id, request, ct);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        // PATCH /api/products/{id}/availability
        [HttpPatch("{id:guid}/availability")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<ActionResult<ProductDto>> UpdateProductAvailability(Guid id, [FromBody] UpdateProductAvailabilityRequest request, CancellationToken ct)
        {
            var updated = await _productService.UpdateAvailabilityAsync(id, request.Availability, ct);
            if (updated == null)
                return NotFound();
            return Ok(updated);
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken ct)
        {
            var deleted = await _productService.DeleteAsync(id, ct);
            if (!deleted)
                return NotFound();
            return NoContent();
        }
    }
}
