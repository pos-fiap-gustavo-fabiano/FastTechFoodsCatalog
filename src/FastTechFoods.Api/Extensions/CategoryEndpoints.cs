using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FluentValidation;

namespace FastTechFoods.Api.Extensions;

public static class CategoryEndpoints
{
    public static void MapCategoryEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/categories")
            .WithTags("Categories")
            .WithOpenApi();

        // GET /api/categories
        group.MapGet("/", async (
            ICategoryService service, 
            CancellationToken ct) =>
            {
                var categories = await service.GetAllAsync(ct);
                return Results.Ok(categories);
            })
            .WithName("GetAllCategories")
            .WithSummary("Listar todas as categorias")
            .WithDescription("Retorna lista completa de categorias disponíveis")
            .Produces<IEnumerable<CategoryDto>>(200);

        // GET /api/categories/{id}
        group.MapGet("/{id:guid}", async (
            Guid id, 
            ICategoryService service, 
            CancellationToken ct) =>
            {
                var category = await service.GetByIdAsync(id, ct);
                return category != null ? Results.Ok(category) : Results.NotFound();
            })
            .WithName("GetCategoryById")
            .WithSummary("Buscar categoria por ID")
            .WithDescription("Retorna uma categoria específica pelo seu ID")
            .Produces<CategoryDto>(200)
            .Produces(404);

        // POST /api/categories
        group.MapPost("/", async (
            CreateCategoryRequest request, 
            ICategoryService service, 
            IValidator<CreateCategoryRequest> validator, 
            CancellationToken ct) =>
            {
                var validation = await validator.ValidateAsync(request, ct);
                if (!validation.IsValid) 
                    return Results.BadRequest(validation.Errors);
                
                var result = await service.CreateAsync(request, ct);
                return Results.Created($"/api/categories/{result.Id}", result);
            })
            .WithName("CreateCategory")
            .WithSummary("Criar nova categoria")
            .WithDescription("Cria uma nova categoria de produtos")
            .Produces<CategoryDto>(201)
            .Produces(400);

        // PUT /api/categories/{id}
        group.MapPut("/{id:guid}", async (
            Guid id, 
            CreateCategoryRequest request, 
            ICategoryService service, 
            IValidator<CreateCategoryRequest> validator, 
            CancellationToken ct) =>
            {
                var validation = await validator.ValidateAsync(request, ct);
                if (!validation.IsValid) 
                    return Results.BadRequest(validation.Errors);
                
                var updated = await service.UpdateAsync(id, request, ct);
                return updated != null ? Results.Ok(updated) : Results.NotFound();
            })
            .WithName("UpdateCategory")
            .WithSummary("Atualizar categoria")
            .WithDescription("Atualiza uma categoria existente")
            .Produces<CategoryDto>(200)
            .Produces(400)
            .Produces(404);

        // DELETE /api/categories/{id}
        group.MapDelete("/{id:guid}", async (
            Guid id, 
            ICategoryService service, 
            CancellationToken ct) =>
            {
                var deleted = await service.DeleteAsync(id, ct);
                return deleted ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteCategory")
            .WithSummary("Deletar categoria")
            .WithDescription("Remove uma categoria do sistema")
            .Produces(204)
            .Produces(404);
    }
}