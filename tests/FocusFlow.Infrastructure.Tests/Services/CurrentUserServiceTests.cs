using FluentAssertions;
using Xunit;
using Moq;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace FocusFlow.Infrastructure.Tests.Services;

/// <summary>
/// Tests for CurrentUserService extracting user information from HTTP context
/// </summary>
public class CurrentUserServiceTests
{
    private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
    private readonly CurrentUserService _currentUserService;

    public CurrentUserServiceTests()
    {
        _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
        _currentUserService = new CurrentUserService(_mockHttpContextAccessor.Object);
    }

    [Fact]
    public void GetUserId_WithValidClaims_ShouldReturnUserId()
    {
        // Arrange
        var userId = 123L;
        var claims = new[]
        {
            new Claim("sub", userId.ToString()),
            new Claim(ClaimTypes.NameIdentifier, userId.ToString())
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _currentUserService.GetUserId();

        // Assert
        result.Should().Be(userId);
    }

    [Fact]
    public void GetUserId_WithoutClaims_ShouldReturnNull()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _currentUserService.GetUserId();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetUserId_WithNullHttpContext_ShouldReturnNull()
    {
        // Arrange
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns((HttpContext?)null);

        // Act
        var result = _currentUserService.GetUserId();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetUserId_WithInvalidUserIdFormat_ShouldReturnNull()
    {
        // Arrange
        var claims = new[]
        {
            new Claim("sub", "not-a-number")
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _currentUserService.GetUserId();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetUserEmail_WithValidClaims_ShouldReturnEmail()
    {
        // Arrange
        var email = "test@example.com";
        var claims = new[]
        {
            new Claim("email", email)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _currentUserService.GetUserEmail();

        // Assert
        result.Should().Be(email);
    }

    [Fact]
    public void GetUserEmail_WithoutClaims_ShouldReturnNull()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _currentUserService.GetUserEmail();

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void GetUsername_WithValidClaims_ShouldReturnUsername()
    {
        // Arrange
        var username = "testuser";
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, username)
        };
        var identity = new ClaimsIdentity(claims, "Test");
        var principal = new ClaimsPrincipal(identity);
        var httpContext = new DefaultHttpContext { User = principal };

        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _currentUserService.GetUsername();

        // Assert
        result.Should().Be(username);
    }

    [Fact]
    public void GetUsername_WithoutClaims_ShouldReturnNull()
    {
        // Arrange
        var httpContext = new DefaultHttpContext();
        _mockHttpContextAccessor.Setup(x => x.HttpContext).Returns(httpContext);

        // Act
        var result = _currentUserService.GetUsername();

        // Assert
        result.Should().BeNull();
    }
}
