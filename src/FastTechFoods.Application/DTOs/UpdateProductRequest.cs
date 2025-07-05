namespace FastTechFoods.Application.DTOs;

public class UpdateProductRequest
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool Availability { get; set; }
    public string Type { get; set; } = string.Empty;
}
