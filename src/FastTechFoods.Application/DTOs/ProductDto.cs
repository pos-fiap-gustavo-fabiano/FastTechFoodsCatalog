namespace FastTechFoods.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    bool Availability,
    string Type,
    DateTime CreatedDate);
