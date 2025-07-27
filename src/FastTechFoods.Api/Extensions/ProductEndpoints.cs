using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace FastTechFoods.Api.Extensions;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        // Menu endpoint
        app.MapGet("/api/menu", async (
            Guid? categoryId, 
            string? search, 
            IProductService service, 
            CancellationToken ct) =>
            {
                var products = await service.GetAllAsync(categoryId, search, ct);
                return Results.Ok(products);
            })
            .WithName("GetMenu")
            .WithTags("Menu")
            .WithSummary("Buscar produtos do menu")
            .WithDescription("Retorna lista de produtos com filtros opcionais por categoria e busca textual")
            .Produces<IEnumerable<ProductDto>>(200);

        var group = app.MapGroup("/api/products")
            .WithTags("Products")
            .WithOpenApi();

        // GET /api/products/{id}
        group.MapGet("/{id:guid}", async (
            Guid id, 
            IProductService service, 
            CancellationToken ct) =>
            {
                var product = await service.GetByIdAsync(id, ct);
                return product != null ? Results.Ok(product) : Results.NotFound();
            })
            .WithName("GetProductById")
            .WithSummary("Buscar produto por ID")
            .WithDescription("Retorna um produto específico pelo seu ID")
            .Produces<ProductDto>(200)
            .Produces(404);

        // POST /api/products (multipart form)
        group.MapPost("/", async (
            [FromForm] CreateProductRequest productRequest,
            IProductService service,
            IValidator<CreateProductRequest> validator,
            CancellationToken ct) =>
            {
                var validation = await validator.ValidateAsync(productRequest, ct);
                if (!validation.IsValid) 
                    return Results.BadRequest(validation.Errors);

                var result = await service.CreateAsync(productRequest, ct);
                if (result.IsFailure)
                    return Results.BadRequest(result.ErrorMessage);
                
                return Results.Created($"/api/products/{result.Value!.Id}", result.Value);
            })
            .WithName("CreateProduct")
            .WithSummary("Criar novo produto")
            .WithDescription("Cria um novo produto com upload de imagem (multipart/form-data)")
            .Accepts<CreateProductRequest>("multipart/form-data")
            .Produces<ProductDto>(201)
            .Produces(400)
            .DisableAntiforgery();

        // PUT /api/products/{id}
        group.MapPut("/{id:guid}", async (
            Guid id, 
            UpdateProductRequest request, 
            IProductService service, 
            IValidator<UpdateProductRequest> validator, 
            CancellationToken ct) =>
            {
                var validation = await validator.ValidateAsync(request, ct);
                if (!validation.IsValid) 
                    return Results.BadRequest(validation.Errors);
                
                var updated = await service.UpdateAsync(id, request, ct);
                return updated != null ? Results.Ok(updated) : Results.NotFound();
            })
            .WithName("UpdateProduct")
            .WithSummary("Atualizar produto")
            .WithDescription("Atualiza um produto existente")
            .Produces<ProductDto>(200)
            .Produces(400)
            .Produces(404);

        // PATCH /api/products/{id}/availability
        group.MapPatch("/{id:guid}/availability", async (
            Guid id, 
            UpdateProductAvailabilityRequest request, 
            IProductService service, 
            CancellationToken ct) =>
            {
                var updated = await service.UpdateAvailabilityAsync(id, request.Availability, ct);
                return updated != null ? Results.Ok(updated) : Results.NotFound();
            })
            .WithName("UpdateProductAvailability")
            .WithSummary("Atualizar disponibilidade do produto")
            .WithDescription("Atualiza apenas a disponibilidade de um produto")
            .Produces<ProductDto>(200)
            .Produces(404);

        // DELETE /api/products/{id}
        group.MapDelete("/{id:guid}", async (
            Guid id, 
            IProductService service, 
            CancellationToken ct) =>
            {
                var result = await service.DeleteAsync(id, ct);
                return result.IsSuccess ? Results.NoContent() : Results.NotFound();
            })
            .WithName("DeleteProduct")
            .WithSummary("Deletar produto")
            .WithDescription("Remove um produto do sistema")
            .Produces(204)
            .Produces(404);
    }
}