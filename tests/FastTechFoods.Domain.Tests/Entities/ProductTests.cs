using System;
using FastTechFoods.Domain.Entities;
using Xunit;

namespace FastTechFoods.Domain.Tests.Entities;

public class ProductTests
{
    [Fact]
    public void Constructor_SetsProperties()
    {
        var before = DateTime.UtcNow;

        var product = new Product("Burger", "Tasty", 10m, true, ProductType.Snack);

        Assert.Equal("Burger", product.Name);
        Assert.Equal("Tasty", product.Description);
        Assert.Equal(10m, product.Price);
        Assert.True(product.Availability);
        Assert.Equal(ProductType.Snack, product.Type);
        Assert.NotEqual(Guid.Empty, product.Id);
        Assert.InRange(product.CreatedDate, before, DateTime.UtcNow);
    }

    [Fact]
    public void Update_ChangesProperties()
    {
        var product = new Product("Burger", "Tasty", 10m, true, ProductType.Snack);

        product.Update("Fries", "Crispy", 5m, false, ProductType.Dessert);

        Assert.Equal("Fries", product.Name);
        Assert.Equal("Crispy", product.Description);
        Assert.Equal(5m, product.Price);
        Assert.False(product.Availability);
        Assert.Equal(ProductType.Dessert, product.Type);
    }

    [Fact]
    public void SetAvailability_ChangesAvailabilityOnly()
    {
        var product = new Product("Burger", "Tasty", 10m, true, ProductType.Snack);

        product.SetAvailability(false);

        Assert.False(product.Availability);
    }
}
