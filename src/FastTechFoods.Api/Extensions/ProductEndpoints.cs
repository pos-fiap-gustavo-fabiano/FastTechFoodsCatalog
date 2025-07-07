using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FluentValidation;

namespace FastTechFoods.Api.Extensions;

public static class ProductEndpoints
{
    public static void MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        // Menu endpoint
        app.MapGet("/api/menu", async (Guid? categoryId, string? search, IProductService service, CancellationToken ct) =>
            await service.GetAllAsync(categoryId, search, ct))
            .WithTags("Menu");

        var group = app.MapGroup("/api/products").WithTags("Products");

        // GET /api/products/{id}
        group.MapGet("/{id:guid}", async (Guid id, IProductService service, CancellationToken ct) =>
            await service.GetByIdAsync(id, ct) is { } product ? Results.Ok(product) : Results.NotFound());

        // POST /api/products (multipart form)
        group.MapPost("/", async (HttpRequest request, IProductService service, IValidator<CreateProductRequest> validator, CancellationToken ct) =>
        {
            var form = await request.ReadFormAsync(ct);
            var productRequest = new CreateProductRequest
            {
                Name = form["Name"]!,
                Description = form["Description"]!,
                Price = decimal.Parse(form["Price"]!),
                Availability = bool.Parse(form["Availability"]!),
                CategoryId = Guid.Parse(form["CategoryId"]!),
                Image = form.Files.FirstOrDefault()
            };

            var validation = await validator.ValidateAsync(productRequest, ct);
            if (!validation.IsValid) return Results.BadRequest(validation.Errors);

            var result = await service.CreateAsync(productRequest, ct);
            return Results.Created($"/api/products/{result.Id}", result);
        }).DisableAntiforgery();

        // PUT /api/products/{id}
        group.MapPut("/{id:guid}", async (Guid id, UpdateProductRequest request, IProductService service, IValidator<UpdateProductRequest> validator, CancellationToken ct) =>
        {
            var validation = await validator.ValidateAsync(request, ct);
            if (!validation.IsValid) return Results.BadRequest(validation.Errors);
            
            return await service.UpdateAsync(id, request, ct) is { } updated ? Results.Ok(updated) : Results.NotFound();
        });

        // PATCH /api/products/{id}/availability
        group.MapPatch("/{id:guid}/availability", async (Guid id, UpdateProductAvailabilityRequest request, IProductService service, CancellationToken ct) =>
            await service.UpdateAvailabilityAsync(id, request.Availability, ct) is { } updated ? Results.Ok(updated) : Results.NotFound());

        // DELETE /api/products/{id}
        group.MapDelete("/{id:guid}", async (Guid id, IProductService service, CancellationToken ct) =>
            await service.DeleteAsync(id, ct) ? Results.NoContent() : Results.NotFound());
    }
}