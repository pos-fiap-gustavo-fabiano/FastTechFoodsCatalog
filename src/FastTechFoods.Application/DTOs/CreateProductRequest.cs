using FastTechFoods.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace FastTechFoods.Application.DTOs;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool Availability { get; set; }
    public Guid CategoryId { get; set; }
    public IFormFile? Image { get; set; }
}
