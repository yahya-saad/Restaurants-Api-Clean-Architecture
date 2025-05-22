using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Common.Identity;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Tests;

public class CreateRestaurantCommandHandlerTests
{
    private readonly Mock<ILogger<CreateRestaurantCommandHandler>> _loggerMock;
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly CreateRestaurantCommandHandler _handler;

    public CreateRestaurantCommandHandlerTests()
    {
        _loggerMock = new Mock<ILogger<CreateRestaurantCommandHandler>>();
        _restaurantRepositoryMock = new Mock<IRestaurantsRepository>();
        _userContextMock = new Mock<IUserContext>();
        _mapperMock = new Mock<IMapper>();

        _handler = new CreateRestaurantCommandHandler(
            _restaurantRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object,
            _userContextMock.Object);
    }

    private static CreateRestaurantDto GetSampleCreateRestaurantDto() => new()
    {
        Name = "Test Restaurant",
        Description = "Test Description",
        Category = "Fast Food",
        HasDelivery = true,
        ContractEmail = null,
        ContractNumber = null,
        Address = new Address
        {
            Street = "123 Test St",
            City = "Test City",
            PostalCode = "12345"
        }
    };

    [Fact]
    public async Task Handle_ValidCommand_ReturnsCreatedRestaurantId()
    {
        // Arrange
        var dto = GetSampleCreateRestaurantDto();
        var command = new CreateRestaurantCommand(dto);
        var expectedOwnerId = "owner-id";
        var restaurantEntity = new Restaurant();

        _mapperMock
            .Setup(m => m.Map<Restaurant>(dto))
            .Returns(restaurantEntity);

        _userContextMock
            .Setup(uc => uc.GetCurrentUser())
            .Returns(new CurrentUser(expectedOwnerId, "test@test.com", [], null, null));

        _restaurantRepositoryMock
            .Setup(repo => repo.AddAsync(It.IsAny<Restaurant>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.Should().Be(1);
        restaurantEntity.OwnerId.Should().Be(expectedOwnerId);
        _restaurantRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<Restaurant>(), CancellationToken.None), Times.Once);
    }
}
