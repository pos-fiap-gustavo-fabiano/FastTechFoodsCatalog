using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FastTechFoods.Application.Services;
using FastTechFoods.Domain.Entities;
using FastTechFoods.Domain.Repositories;
using Moq;
using Xunit;

namespace FastTechFoods.Application.Tests.Services;

public class ProductServiceTests
{
    private readonly Mock<IProductRepository> _repoMock = new();
    private readonly Mock<ICategoryRepository> _categoryRepoMock = new();
    private readonly Mock<IBlobStorageService> _blobStorageMock = new();
    private readonly ProductService _service;
    private readonly Guid _categoryId1 = Guid.NewGuid();
    private readonly Guid _categoryId2 = Guid.NewGuid();

    public ProductServiceTests()
    {
        _service = new ProductService(_repoMock.Object,_categoryRepoMock.Object, _blobStorageMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        // Arrange
        var category1 = new Category("Snacks", "Delicious snacks");
        var category2 = new Category("Desserts", "Sweet desserts");
        
        var products = new List<Product>
        {
            new("Burger", "Tasty", 10m, true, "", _categoryId1),
            new("Fries", "Crispy", 5m, true, "", _categoryId2)
        };

        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(products);

        // Act
        var result = await _service.GetAllAsync(category1.Id,null, CancellationToken.None);

        // Assert
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
        var result = await _service.GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public async Task GetByIdAsync_WhenFound_ReturnsMappedDto()
    {
        // Arrange
        var category = new Category("Snacks", "Delicious snacks");
        var product = new Product("Burger", "Tasty", 10m, true, "", _categoryId1);
        
        _repoMock.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _service.GetByIdAsync(product.Id, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(product.Name, result.Name);
    }

    [Fact]
    public async Task CreateAsync_CallsRepositoryAndReturnsDto()
    {
        // Arrange
        Product? captured = null;
        _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()))
        .Callback<Product, CancellationToken>((p, _) => captured = p)
        .ReturnsAsync((Product p, CancellationToken _) => p);

        _categoryRepoMock.Setup(r => r.ExistsAsync(_categoryId1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var request = new CreateProductRequest
        {
            Name = "Soda",
            Description = "Cola",
            Price = 3.5m,
            Availability = true,
            CategoryId = _categoryId1
        };

        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(captured);
        Assert.Equal(request.Name, captured!.Name);
        Assert.Equal(request.Description, captured.Description);
        Assert.Equal(request.Price, captured.Price);
        Assert.Equal(request.CategoryId, captured.CategoryId);
        Assert.Equal(result.Name, request.Name);
    }

    [Fact]
    public async Task CreateAsync_WhenCategoryNotExists_ThrowsException()
    {
        // Arrange
        _categoryRepoMock.Setup(r => r.ExistsAsync(_categoryId1, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var request = new CreateProductRequest
        {
            Name = "Soda",
            Description = "Cola", 
            Price = 3.5m,
            Availability = true,
            CategoryId = _categoryId1
        };

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _service.CreateAsync(request, CancellationToken.None));
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
            CategoryId = _categoryId1
        };

        // Act
        var result = await _service.UpdateAsync(Guid.NewGuid(), request, CancellationToken.None);

        // Assert
        Assert.Null(result);
        _repoMock.Verify(r => r.UpdateAsync(It.IsAny<Product>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task UpdateAsync_WhenProductExists_UpdatesAndReturnsDto()
    {
        // Arrange
        var product = new Product("Burger", "Tasty", 10m, true, "", _categoryId1);
        _repoMock.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _repoMock.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        _categoryRepoMock.Setup(r => r.ExistsAsync(_categoryId2, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var request = new UpdateProductRequest
        {
            Name = "Updated Burger",
            Description = "Updated desc",
            Price = 12m,
            Availability = false,
            CategoryId = _categoryId2
        };

        // Act
        var result = await _service.UpdateAsync(product.Id, request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(request.Name, result.Name);
        Assert.Equal(request.CategoryId, result.CategoryId);
        _repoMock.Verify(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductExists_ReturnsTrue()
    {
        // Arrange
        var product = new Product("Burger", "Tasty", 10m, true, "", Guid.NewGuid());
        _repoMock.Setup(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _service.DeleteAsync(product.Id, CancellationToken.None);

        // Assert
        Assert.True(result);
        _repoMock.Verify(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_WhenProductNotExists_ReturnsFalse()
    {
        // Arrange
        var product = new Product("Burger", "Tasty", 10m, true, "", Guid.NewGuid());
        _repoMock.Setup(r => r.DeleteAsync(product, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        // Act
        var result = await _service.DeleteAsync(product.Id, CancellationToken.None);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public async Task UpdateAvailabilityAsync_WhenProductExists_UpdatesAvailability()
    {
        // Arrange
        var product = new Product("Burger", "Tasty", 10m, true, "", _categoryId1);
        _repoMock.Setup(r => r.GetByIdAsync(product.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);
        _repoMock.Setup(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()))
            .ReturnsAsync(product);

        // Act
        var result = await _service.UpdateAvailabilityAsync(product.Id, false, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Availability);
        _repoMock.Verify(r => r.UpdateAsync(product, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAvailabilityAsync_WhenProductNotExists_ReturnsNull()
    {
        // Arrange
        _repoMock.Setup(r => r.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Product?)null);

        // Act
        var result = await _service.UpdateAvailabilityAsync(Guid.NewGuid(), false, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
