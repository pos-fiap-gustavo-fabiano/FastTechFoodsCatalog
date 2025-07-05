using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Services;
using FastTechFoods.Domain.Entities;
using FastTechFoods.Domain.Repositories;
using Moq;
using Xunit;

namespace FastTechFoods.Application.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly ProductService _service;

    public ProductServiceTests()
    {
        _service = new ProductService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        // Arrange
        var products = new List<Product>
        {
            new("Burger","Tasty",10m,true,ProductType.Snack),
            new("Fries","Crispy",5m,true,ProductType.Dessert)
        };
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _service.GetAllAsync();

        // Assert
        Assert.Equal(products.Count, result.Count());
        Assert.Contains(result, p => p.Name == "Burger" && p.Description == "Tasty");
        Assert.Contains(result, p => p.Name == "Fries" && p.Description == "Crispy");
    }

    [Fact]
    public async Task GetByIdAsync_WhenNotFound_ReturnsNull()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.GetByIdAsync(Guid.NewGuid());

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task CreateAsync_CallsRepositoryAndReturnsDto()
    {
        // Arrange
        Product? captured = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
            .Callback<Product, CancellationToken>((p, _) => captured = p)
            .Returns(Task.CompletedTask);

        var request = new CreateProductRequest
        {
            Name = "Soda",
            Description = "Cola",
            Price = 3.5m,
            Availability = true,
            Type = ProductType.Drink
        };

        // Act
        var result = await _service.CreateAsync(request);

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(captured);
        Assert.Equal(request.Name, captured!.Name);
        Assert.Equal(request.Description, captured.Description);
        Assert.Equal(request.Price, captured.Price);
        Assert.Equal(request.Type, captured.Type);
        Assert.Equal(result.Name, request.Name);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductNotFound_ReturnsNull()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        var request = new UpdateProductRequest
        {
            Name = "Updated",
            Description = "Updated desc",
            Price = 2,
            Availability = true,
            Type = ProductType.Snack
        };

        // Act
        var result = await _service.UpdateAsync(Guid.NewGuid(), request);

        // Assert
        Assert.Null(result);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_ReturnsTrue()
    {
        // Arrange
        var product = new Product("Burger","Tasty",10m,true,ProductType.Snack);
        _repoMock.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _repoMock.Setup(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _service.DeleteAsync(product.Id);

        // Assert
        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }
}
