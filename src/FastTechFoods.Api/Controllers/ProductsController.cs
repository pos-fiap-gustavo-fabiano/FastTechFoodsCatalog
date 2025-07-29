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
        public async Task<IActionResult> GetProductById(Guid id, CancellationToken ct)
        {
            var result = await _productService.GetByIdAsync(id, ct);
            if (result.IsFailure)
            {
                if (result.ErrorCode == "PRODUCT_NOT_FOUND")
                    return NotFound(result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
            
            return Ok(result.Value);
        }

        // GET /api/products
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _productService.GetAllAsync(null, "", ct);
            if (result.IsFailure)
                return BadRequest(result.ErrorMessage);
                
            return Ok(result.Value);
        }

        // POST /api/products
        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> CreateProduct([FromForm] CreateProductRequest request, [FromServices] IValidator<CreateProductRequest> validator, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var result = await _productService.CreateAsync(request, ct);
            if (result.IsFailure)
                return BadRequest(result.ErrorMessage);
            
            return CreatedAtAction(nameof(GetProductById), new { id = result.Value!.Id }, result.Value);
        }

        // PUT /api/products/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromForm] UpdateProductRequest request, [FromServices] IValidator<UpdateProductRequest> validator, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var result = await _productService.UpdateAsync(id, request, ct);
            if (result.IsFailure)
            {
                if (result.ErrorCode == "PRODUCT_NOT_FOUND")
                    return NotFound(result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
            
            return Ok(result.Value);
        }

        // PATCH /api/products/{id}/availability
        [HttpPatch("{id:guid}/availability")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateProductAvailability(Guid id, [FromBody] UpdateProductAvailabilityRequest request, CancellationToken ct)
        {
            var result = await _productService.UpdateAvailabilityAsync(id, request.Availability, ct);
            if (result.IsFailure)
            {
                if (result.ErrorCode == "PRODUCT_NOT_FOUND")
                    return NotFound(result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
            
            return Ok(result.Value);
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken ct)
        {
            var result = await _productService.DeleteAsync(id, ct);
            if (result.IsFailure)
            {
                if (result.ErrorCode == "PRODUCT_NOT_FOUND")
                    return NotFound(result.ErrorMessage);
                return BadRequest(result.ErrorMessage);
            }
            
            return NoContent();
        }
    }
}
