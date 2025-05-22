using Microsoft.AspNetCore.Http;
using Moq;
using Restaurants.Application.Common.Identity;
using Restaurants.Domain.Constants;
using System.Security.Claims;

namespace Restaurants.Application.Tests;
public class UserContextTests
{
    private readonly Mock<IHttpContextAccessor> _httpContextAccessorMock = new();
    private readonly UserContext _sut;

    public UserContextTests()
    {
        _sut = new UserContext(_httpContextAccessorMock.Object);
    }

    [Fact]
    public void GetCurrentUser_WithUserContextNotPresent_ShouldThrowInvalidOperationException()
    {
        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns((HttpContext)null);

        Action act = () => _sut.GetCurrentUser();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("User context is not present");
    }


    [Fact]
    public void GetCurrentUser_WithAuthenticatedUser_ShouldReturnCurrentUser()
    {
        var dateOfBirth = new DateOnly(2002, 9, 24);
        var nationality = "Egyptian";
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, "1"),
            new Claim(ClaimTypes.Email, "test@test,com"),
            new Claim(ClaimTypes.Role, UserRoles.Admin),
            new Claim(ClaimTypes.Role, UserRoles.User),
            new Claim("Nationality", nationality),
            new Claim("DateOfBirth", dateOfBirth.ToString("yyyy-MM-dd"))
        };

        var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "Test"));

        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext { User = user });

        var result = _sut.GetCurrentUser();

        result.Should().NotBeNull();
        result.Id.Should().Be("1");
        result.Email.Should().Be("test@test,com");
        result.Roles.Should().ContainInOrder(new[] { UserRoles.Admin, UserRoles.User });
        result.Nationality.Should().Be(nationality);
        result.DateOfBirth.Should().Be(dateOfBirth);
    }

    [Fact]
    public void GetCurrentUser_WithUnauthenticatedUser_ShouldReturnNull()
    {
        var user = new ClaimsPrincipal(new ClaimsIdentity());
        _httpContextAccessorMock.Setup(x => x.HttpContext)
            .Returns(new DefaultHttpContext { User = user });

        var result = _sut.GetCurrentUser();

        result.Should().BeNull();
    }




}
