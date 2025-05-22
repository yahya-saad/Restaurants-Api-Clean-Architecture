using Microsoft.AspNetCore.Authorization;
using Restaurants.Application.Common.Identity;
using Restaurants.Domain.Entities;
using Restaurants.Domain.Interfaces;
using Restaurants.Infrastructure.Authorization.Requirement;
using System.Security.Claims;

namespace Restaurants.Infrastructure.Tests;
public class OwnsAtLeastRequirementHandlerTests
{
    private readonly Mock<IRestaurantsRepository> _restaurantsRepositoryMock;
    private readonly Mock<IUserContext> _userContextMock;
    private readonly OwnsAtLeastRequirementHandler _handler;

    public OwnsAtLeastRequirementHandlerTests()
    {
        _restaurantsRepositoryMock = new Mock<IRestaurantsRepository>();
        _userContextMock = new Mock<IUserContext>();
        _handler = new OwnsAtLeastRequirementHandler(_userContextMock.Object, _restaurantsRepositoryMock.Object);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserOwnsMultipleRestaurants_ShouldSucceed()
    {
        var currentUser = new CurrentUser("1", "email", [], null, null);

        _userContextMock
            .Setup(uc => uc.GetCurrentUser())
            .Returns(currentUser);

        var restaurants = new[] {
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = "2" }
        };

        _restaurantsRepositoryMock
            .Setup(repo => repo.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(restaurants);

        var requirement = new OwnsAtLeastRequirement(2);
        var context = new AuthorizationHandlerContext(new[] { requirement }, new ClaimsPrincipal(), null);

        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeTrue();
        context.HasFailed.Should().BeFalse();
    }

    [Fact]
    public async Task HandleRequirementAsync_UserOwnsMultipleRestaurants_ShouldFail()
    {
        var currentUser = new CurrentUser("1", "email", [], null, null);

        _userContextMock
            .Setup(uc => uc.GetCurrentUser())
            .Returns(currentUser);

        var restaurants = new[] {
            new Restaurant { OwnerId = currentUser.Id },
            new Restaurant { OwnerId = "2" }
        };

        _restaurantsRepositoryMock
            .Setup(repo => repo.GetAllAsync(CancellationToken.None))
            .ReturnsAsync(restaurants);

        var requirement = new OwnsAtLeastRequirement(2);
        var context = new AuthorizationHandlerContext(new[] { requirement }, new ClaimsPrincipal(), null);
        await _handler.HandleAsync(context);

        context.HasSucceeded.Should().BeFalse();
        context.HasFailed.Should().BeTrue();

    }
}



