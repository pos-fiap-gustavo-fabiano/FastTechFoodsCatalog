using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FastTechFoods.Domain.Entities;
using FastTechFoods.Infrastructure;
using Microsoft.EntityFrameworkCore;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddFluentValidationAutoValidation();

string connectionString = builder.Configuration.GetConnectionString("Default") ?? throw new ArgumentNullException("DbConnectionString");

builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<FastTechFoods.Infrastructure.Data.AppDbContext>();
    db.Database.Migrate();
}

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/api/menu", async (ProductType? type, string? search, IProductService service, CancellationToken ct) =>
    Results.Ok(await service.GetAllAsync(type, search, ct)));

app.MapGet("/api/products/{id}", async (Guid id, IProductService service, CancellationToken ct) =>
{
    var product = await service.GetByIdAsync(id, ct);
    return product is not null ? Results.Ok(product) : Results.NotFound();
});

app.MapPost("/api/products", async (CreateProductRequest request, IProductService service, IValidator<CreateProductRequest> validator, CancellationToken ct) =>
{
    var validation = await validator.ValidateAsync(request, ct);
    if (!validation.IsValid)
        return Results.BadRequest(validation.Errors);

    var result = await service.CreateAsync(request, ct);
    return Results.Created($"/api/products/{result.Id}", result);
});

app.MapPut("/api/products/{id}", async (Guid id, UpdateProductRequest request, IProductService service, IValidator<UpdateProductRequest> validator, CancellationToken ct) =>
{
    var validation = await validator.ValidateAsync(request, ct);
    if (!validation.IsValid)
        return Results.BadRequest(validation.Errors);

    var updated = await service.UpdateAsync(id, request, ct);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

app.MapPatch("/api/products/{id}/availability", async (Guid id, UpdateProductAvailabilityRequest request, IProductService service, CancellationToken ct) =>
{
    var updated = await service.UpdateAvailabilityAsync(id, request.Availability, ct);
    return updated is not null ? Results.Ok(updated) : Results.NotFound();
});

app.MapDelete("/api/products/{id}", async (Guid id, IProductService service, CancellationToken ct) =>
{
    var deleted = await service.DeleteAsync(id, ct);
    return deleted ? Results.NoContent() : Results.NotFound();
});

app.Run();
