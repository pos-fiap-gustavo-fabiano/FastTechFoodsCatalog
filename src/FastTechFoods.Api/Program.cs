using FastTechFoods.Infrastructure;
using FastTechFoodsAuth.Security.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Adicionar Controllers
builder.Services.AddControllers();
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

var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING_DATABASE") ??
                       builder.Configuration.GetConnectionString("Default") ??
                       throw new InvalidOperationException("Connection string not found");
builder.Services.AddInfrastructure(connectionString, builder.Configuration);


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
app.UseFastTechFoodsInfrastructure();
app.UseFastTechFoodsSecurityAudit();
app.MapControllers();
app.UseFastTechFoodsInfrastructure();

app.Run();