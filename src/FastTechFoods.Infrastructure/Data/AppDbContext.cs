using FastTechFoods.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FastTechFoods.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(entity =>
        {
            entity.ToTable("Products");
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
            entity.Property(p => p.Description).IsRequired().HasMaxLength(500);
            entity.Property(p => p.Price).HasColumnType("decimal(10,2)");
            entity.Property(p => p.Availability).IsRequired();
            entity.Property(p => p.Type)
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(100);
            entity.Property(p => p.CreatedDate).IsRequired();
        });
    }
}
