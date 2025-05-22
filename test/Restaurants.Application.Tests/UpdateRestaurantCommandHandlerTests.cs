using AutoMapper;
using Microsoft.Extensions.Logging;
using Moq;
using Restaurants.Application.Restaurants.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.DTOs;
using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Exceptions;
using Restaurants.Domain.Interfaces;

namespace Restaurants.Application.Tests;
public class UpdateRestaurantCommandHandlerTests
{
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly Mock<ILogger<UpdateRestaurantCommandHandler>> _loggerMock = new();
    private readonly Mock<IRestaurantsRepository> _restaurantRepositoryMock = new();
    private readonly Mock<IRestaurantAuthorizationService> _restaurantAuthorizationServiceMock = new();
    private readonly UpdateRestaurantCommandHandler _handler;
    public UpdateRestaurantCommandHandlerTests()
    {
        _handler = new UpdateRestaurantCommandHandler(
            _restaurantRepositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object,
            _restaurantAuthorizationServiceMock.Object);
    }

    [Fact]
    public void Handle_WithNonExistingRestaurant_ShouldThrowNotFoundException()
    {
        // Arrange
        var restaurantId = 1;
        var command = new UpdateRestaurantCommand(restaurantId, new UpdateRestaurantDto());

        _restaurantRepositoryMock
            .Setup(repo => repo.GetByIdAsync(restaurantId, CancellationToken.None, null))
            .ReturnsAsync((Restaurant)null!);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage($"Restaurant with id {restaurantId} not found.");
    }

    [Fact]
    public void Handle_WithUnauthorizedUser_ShouldThrowForbidException()
    {
        // Arrange
        var restaurantId = 1;
        var dto = new UpdateRestaurantDto();
        var command = new UpdateRestaurantCommand(restaurantId, dto);
        var restaurant = new Restaurant();

        _restaurantRepositoryMock
            .Setup(repo => repo.GetByIdAsync(restaurantId, CancellationToken.None, null))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock
            .Setup(service => service.Authorize(restaurant, ResourseOperation.Update))
            .Returns(false);

        // Act
        Func<Task> act = async () => await _handler.Handle(command, CancellationToken.None);

        // Assert
        act.Should()
            .ThrowAsync<ForbiddenException>()
            .WithMessage("Access is forbidden.");
    }

    [Fact]
    public async Task Handle_WhenCommandIsValid_ShouldUpdateRestaurant()
    {
        // Arrange
        var restaurantId = 1;
        var dto = new UpdateRestaurantDto();
        var command = new UpdateRestaurantCommand(restaurantId, dto);
        var restaurant = new Restaurant() { Id = restaurantId };

        _restaurantRepositoryMock
            .Setup(repo => repo.GetByIdAsync(restaurantId, CancellationToken.None, null))
            .ReturnsAsync(restaurant);

        _restaurantAuthorizationServiceMock
            .Setup(service => service.Authorize(restaurant, ResourseOperation.Update))
            .Returns(true);

        // Act
        await _handler.Handle(command, CancellationToken.None);

        // Assert
        _mapperMock.Verify(m => m.Map(dto, restaurant), Times.Once);
        _restaurantRepositoryMock.Verify(repo => repo.UpdateAsync(restaurant, CancellationToken.None), Times.Once);
    }
}
