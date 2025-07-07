using FastTechFoods.Domain.Entities;

namespace FastTechFoods.Application.DTOs;

public record ProductDto(
    Guid Id,
    string Name,
    string Description,
    decimal Price,
    bool Availability,
    string? ImageUrl,
    Guid CategoryId,
    DateTime CreatedDate
);
