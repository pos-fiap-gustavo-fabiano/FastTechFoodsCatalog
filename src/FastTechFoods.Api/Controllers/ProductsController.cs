using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FastTechFoodsOrder.Shared.Controllers;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.Api.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductsController : BaseController
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
            return ToActionResult(result);
        }

        // GET /api/products
        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            var result = await _productService.GetAllAsync(null, "", ct);
            return ToActionResult(result);
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
            return ToCreatedResult(result, nameof(GetProductById), new { id = result.Value?.Id });
        }

        // PUT /api/products/{id}
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductRequest request, [FromServices] IValidator<UpdateProductRequest> validator, CancellationToken ct)
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid)
                return BadRequest(validation.Errors);

            var result = await _productService.UpdateAsync(id, request, ct);
            return ToActionResult(result);
        }

        // PATCH /api/products/{id}/availability
        [HttpPatch("{id:guid}/availability")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> UpdateProductAvailability(Guid id, [FromBody] UpdateProductAvailabilityRequest request, CancellationToken ct)
        {
            var result = await _productService.UpdateAvailabilityAsync(id, request.Availability, ct);
            return ToActionResult(result);
        }

        // DELETE /api/products/{id}
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin, Manager")]
        public async Task<IActionResult> DeleteProduct(Guid id, CancellationToken ct)
        {
            var result = await _productService.DeleteAsync(id, ct);
            return ToNoContentResult(result);
        }
    }
}
