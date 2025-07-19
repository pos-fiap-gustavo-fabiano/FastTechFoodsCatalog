using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.Api.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class MenuController : ControllerBase
    {
        private readonly IProductService _productService;

        public MenuController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /api/menu?categoryId=xxx&search=xxx
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetMenu([FromQuery] Guid? categoryId, [FromQuery] string? search, CancellationToken ct)
        {
            var products = await _productService.GetAllAsync(categoryId, search, ct);
            return Ok(products);
        }
    }
}