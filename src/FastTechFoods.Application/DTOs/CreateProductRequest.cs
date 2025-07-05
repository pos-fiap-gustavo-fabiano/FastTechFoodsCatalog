using FastTechFoods.Domain.Entities;

namespace FastTechFoods.Application.DTOs;

public class CreateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool Availability { get; set; }
    public ProductType Type { get; set; }
}
