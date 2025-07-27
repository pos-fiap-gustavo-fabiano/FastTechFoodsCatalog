using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FastTechFoodsOrder.Shared.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.Api.Controllers
{
    [ApiController]
    [Route("api/menu")]
    public class MenuController : BaseController
    {
        private readonly IProductService _productService;

        public MenuController(IProductService productService)
        {
            _productService = productService;
        }

        // GET /api/menu?categoryId=xxx&search=xxx
        [HttpGet]
        public async Task<IActionResult> GetMenu([FromQuery] Guid? categoryId, [FromQuery] string? search, CancellationToken ct)
        {
            var result = await _productService.GetAllAsync(categoryId, search, ct);
            return ToActionResult(result);
        }
    }
}