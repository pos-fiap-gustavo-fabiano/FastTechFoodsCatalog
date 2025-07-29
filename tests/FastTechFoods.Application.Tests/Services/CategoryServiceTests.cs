using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using FastTechFoods.Application.DTOs;
using FastTechFoods.Application.Interfaces;
using FastTechFoods.Application.Services;
using FastTechFoods.Domain.Entities;
using Moq;
using Xunit;

namespace FastTechFoods.Application.Tests.Services;

public class CategoryServiceTests
{
    private readonly Mock<ICategoryRepository> _repoMock = new();
    private readonly CategoryService _service;

    public CategoryServiceTests()
    {
        _service = new CategoryService(_repoMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        // Arrange
        var categories = new List<Category>
        {
            new("Snacks", "Delicious snacks"),
            new("Drinks", "Refreshing drinks")
        };
        
        _repoMock.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(categories);

        // Act
        var result = await _service.GetAllAsync(CancellationToken.None);

        // Assert
        Assert.Equal(categories.Count, result.Count);
        Assert.Contains(result, c => c.Name == "Snacks" && c.Description == "Delicious snacks");
        Assert.Contains(result, c => c.Name == "Drinks" && c.Description == "Refreshing drinks");
    }

    [Fact]
    public async Task CreateAsync_CallsRepositoryAndReturnsDto()
    {
        // Arrange
        Category captured = null;
        _repoMock.Setup(r => r.CreateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()))
            .Callback<Category, CancellationToken>((c, _) => captured = c)
            .ReturnsAsync((Category c, CancellationToken _) => c);

        var request = new CreateCategoryRequest
        {
            Name = "Beverages",
            Description = "Hot and cold drinks",
            IsActive = true
        };
        // Act
        var result = await _service.CreateAsync(request, CancellationToken.None);

        // Assert
        _repoMock.Verify(r => r.CreateAsync(It.IsAny<Category>(), It.IsAny<CancellationToken>()), Times.Once);
        Assert.NotNull(captured);
        Assert.Equal(request.Name, captured!.Name);
        Assert.Equal(request.Description, captured.Description);
        Assert.Equal(request.IsActive, captured.IsActive);
        Assert.Equal(result.Name, request.Name);
    }
}