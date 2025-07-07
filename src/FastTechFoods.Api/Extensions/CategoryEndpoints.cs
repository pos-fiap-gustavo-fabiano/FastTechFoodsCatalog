using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FluentValidation;

namespace FastTechFoods.Api.Extensions;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories").WithTags("Categories").RequireAuthorization();

        // GET /api/categories
        group.MapGet("/", async (ICategoryService service, CancellationToken ct) =>
            await service.GetAllAsync(ct));

        // GET /api/categories/{id}
        group.MapGet("/{id:guid}", async (Guid id, ICategoryService service, CancellationToken ct) =>
            await service.GetByIdAsync(id, ct) is { } category ? Results.Ok(category) : Results.NotFound());

        // POST /api/categories
        group.MapPost("/", async (CreateCategoryRequest request, ICategoryService service, IValidator<CreateCategoryRequest> validator, CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid) return Results.BadRequest(validation.Errors);
            
            var result = await service.CreateAsync(request, ct);
            return Results.Created($"/api/categories/{result.Id}", result);
        });

        // PUT /api/categories/{id}
        group.MapPut("/{id:guid}", async (Guid id, CreateCategoryRequest request, ICategoryService service, IValidator<CreateCategoryRequest> validator, CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid) return Results.BadRequest(validation.Errors);
            
            return await service.UpdateAsync(id, request, ct) is { } updated ? Results.Ok(updated) : Results.NotFound();
        });

        // DELETE /api/categories/{id}
        group.MapDelete("/{id:guid}", async (Guid id, ICategoryService service, CancellationToken ct) =>
            await service.DeleteAsync(id, ct) ? Results.NoContent() : Results.NotFound());
    }
}