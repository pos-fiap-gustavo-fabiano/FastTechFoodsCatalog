namespace FastTechFoods.Application.DTOs;

public record CategoryDto(
    Guid Id,
    string Name,
    string Description,
    bool IsActive,
    DateTime CreatedDate
);