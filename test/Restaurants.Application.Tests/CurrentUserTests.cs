using Restaurants.Application.Common.Identity;
using Restaurants.Domain.Constants;

namespace Restaurants.Application.Tests;
public class CurrentUserTests
{
    private readonly CurrentUser _sut = new CurrentUser("1", "test@test.com", new List<string> { UserRoles.Admin, UserRoles.User }, null, null);


    [Theory]
    [InlineData(UserRoles.Admin)]
    [InlineData(UserRoles.User)]
    public void IsInRole_WithMatchingRole_ShouldReturnTrue(string expected)
    {
        var result = _sut.IsInRole(expected);

        result.Should().BeTrue();
    }

    [Fact]
    public void IsInRole_WithUnMatchingRole_ShouldReturnFalse()
    {
        var result = _sut.IsInRole(UserRoles.Owner);

        result.Should().BeFalse();
    }

}
