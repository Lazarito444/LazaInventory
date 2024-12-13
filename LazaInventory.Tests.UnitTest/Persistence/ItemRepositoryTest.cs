using System.Net;
using AutoMapper;
using FluentAssertions;
using LazaInventory.Core.Application.Dtos.Item;
using LazaInventory.Core.Application.Exceptions;
using LazaInventory.Core.Application.Interfaces.Repositories;
using LazaInventory.Core.Application.Interfaces.Services;
using LazaInventory.Core.Application.Services;
using LazaInventory.Core.Domain.Entities;
using Moq;

namespace LazaInventory.Tests.UnitTest.Persistence;

public class ItemServiceTest
{
    [Fact]
    public async Task GetAsync_ShouldReturnItem_WhenPassAnInt()
    {
        // Arrange
        Mock<IItemRepository> mock = new Mock<IItemRepository>();
        Mock<IMapper> mapperMock = new Mock<IMapper>();
        mock.Setup(repository => repository.GetAsync(1)).ReturnsAsync(new Item
        {
            Id = 1,
            Name = "Martillo",
            Price = 100,
            Stock = 20,
            MinimumStock = 10,
            ImageUrl = "NA",
            CreatedAt = DateTime.Today,
            CategoryId = 1
        });
        mapperMock.Setup(mapper => mapper.Map<Item>(It.IsAny<SaveItemDto>())).Returns(It.IsAny<Item>());
        IItemService itemService = new ItemService(mock.Object, mapperMock.Object);
        
        // Act
        Item? item = await itemService.GetAsync(1);
        
        // Assert
        item.Should().BeOfType<Item>().And.BeEquivalentTo(new Item
        {
            Id = 1,
            Name = "Martillo",
            Price = 100,
            Stock = 20,
            MinimumStock = 10,
            ImageUrl = "NA",
            CreatedAt = DateTime.Today,
            CategoryId = 1
        });
    }

    [Fact]
    public async Task GetAsync_ShouldThrowError_WhenItemDoesNotExist()
    {
        // Arrange
        Mock<IItemRepository> mock = new Mock<IItemRepository>();
        Mock<IMapper> mapperMock = new Mock<IMapper>();
        mock.Setup(repository => repository.GetAsync(10)).ThrowsAsync(new ApiException(HttpStatusCode.NotFound, "There's no item with ID '10'"));
        mapperMock.Setup(mapper => mapper.Map<Item>(It.IsAny<SaveItemDto>())).Returns(It.IsAny<Item>());
        IItemService itemService = new ItemService(mock.Object, mapperMock.Object);
        
        // Act & Assert
        await itemService.Invoking(async service => await service.GetAsync(10)).Should().ThrowAsync<ApiException>();
    }
}