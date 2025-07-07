using FastTechFoods.Api.Extensions;
using FastTechFoods.Infrastructure;
using FastTechFoodsAuth.Security.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✨ Configuração simplificada do Swagger com JWT usando a biblioteca
builder.Services.AddFastTechFoodsSwaggerWithJwt("FastTechFoods API", "v1", "API de produtos e menu para o sistema FastTechFoods");

// ✨ Configuração simplificada da autenticação JWT usando a biblioteca
builder.Services.AddFastTechFoodsJwtAuthentication(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

var connectionString = builder.Configuration.GetConnectionString("Default")!;
builder.Services.AddInfrastructure(connectionString);

var app = builder.Build();

// Migrate database
try
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<FastTechFoods.Infrastructure.Data.AppDbContext>();
    await db.Database.MigrateAsync();
}
catch (Exception ex)
{
    var logger = app.Services.GetService<ILogger<Program>>();
    logger?.LogError(ex, "An error occurred while migrating the database");
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AllowAll");

app.MapCategoryEndpoints();
app.MapProductEndpoints();

app.Run();