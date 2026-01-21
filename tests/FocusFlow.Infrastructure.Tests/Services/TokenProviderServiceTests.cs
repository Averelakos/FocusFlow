using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace FocusFlow.Infrastructure.Tests.Services;

/// <summary>
/// Tests for TokenProviderService JWT generation and validation
/// </summary>
public class TokenProviderServiceTests
{
    private readonly TokenProviderService _tokenProvider;
    private readonly IConfiguration _configuration;

    public TokenProviderServiceTests()
    {
        var configValues = new Dictionary<string, string>
        {
            { "Jwt:Secret", "ThisIsAVerySecureSecretKeyWithAtLeast32Characters!!" },
            { "Jwt:Issuer", "FocusFlowTestIssuer" },
            { "Jwt:Audience", "FocusFlowTestAudience" },
            { "Jwt:ExpirationInMinutes", "60" }
        };

        _configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(configValues!)
            .Build();

        _tokenProvider = new TokenProviderService(_configuration);
    }

    [Fact]
    public void GenerateAccessToken_ShouldReturnValidJwtToken()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            PasswordHash = new byte[64],
            PasswordSalt = new byte[128]
        };

        // Act
        var token = _tokenProvider.GenerateAccessToken(user);

        // Assert
        token.Should().NotBeNullOrEmpty();
        token.Split('.').Should().HaveCount(3); // JWT has 3 parts: header.payload.signature
    }

    [Fact]
    public void GenerateAccessToken_ShouldIncludeUserClaims()
    {
        // Arrange
        var user = new User
        {
            Id = 123,
            Username = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            PasswordHash = new byte[64],
            PasswordSalt = new byte[128]
        };

        // Act
        var token = _tokenProvider.GenerateAccessToken(user);
        var principal = _tokenProvider.ValidateToken(token);

        // Assert
        principal.Should().NotBeNull();
        principal!.Claims.Should().Contain(c => c.Type == ClaimTypes.NameIdentifier && c.Value == "123");
        principal.Claims.Should().Contain(c => c.Type == ClaimTypes.Email && c.Value == "test@example.com");
        principal.Claims.Should().Contain(c => c.Type == ClaimTypes.Name && c.Value == "testuser");
    }

    [Fact]
    public void ValidateToken_WithValidToken_ShouldReturnClaimsPrincipal()
    {
        // Arrange
        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            PasswordHash = new byte[64],
            PasswordSalt = new byte[128]
        };
        var token = _tokenProvider.GenerateAccessToken(user);

        // Act
        var result = _tokenProvider.ValidateToken(token);

        // Assert
        result.Should().NotBeNull();
        result!.Identity.Should().NotBeNull();
        result.Identity!.IsAuthenticated.Should().BeTrue();
    }

    [Fact]
    public void ValidateToken_WithInvalidToken_ShouldReturnNull()
    {
        // Arrange
        var invalidToken = "invalid.jwt.token";

        // Act
        var result = _tokenProvider.ValidateToken(invalidToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ValidateToken_WithExpiredToken_ShouldReturnNull()
    {
        // Arrange - Create a token with expired configuration
        var expiredConfigValues = new Dictionary<string, string>
        {
            { "Jwt:Secret", "ThisIsAVerySecureSecretKeyWithAtLeast32Characters!!" },
            { "Jwt:Issuer", "FocusFlowTestIssuer" },
            { "Jwt:Audience", "FocusFlowTestAudience" },
            { "Jwt:ExpirationInMinutes", "-1" } // Already expired
        };

        var expiredConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(expiredConfigValues!)
            .Build();

        var expiredTokenProvider = new TokenProviderService(expiredConfig);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            PasswordHash = new byte[64],
            PasswordSalt = new byte[128]
        };

        var expiredToken = expiredTokenProvider.GenerateAccessToken(user);

        // Act
        var result = _tokenProvider.ValidateToken(expiredToken);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void ValidateToken_WithWrongIssuer_ShouldReturnNull()
    {
        // Arrange - Create token with different issuer
        var differentConfigValues = new Dictionary<string, string>
        {
            { "Jwt:Secret", "ThisIsAVerySecureSecretKeyWithAtLeast32Characters!!" },
            { "Jwt:Issuer", "DifferentIssuer" },
            { "Jwt:Audience", "FocusFlowTestAudience" },
            { "Jwt:ExpirationInMinutes", "60" }
        };

        var differentConfig = new ConfigurationBuilder()
            .AddInMemoryCollection(differentConfigValues!)
            .Build();

        var differentTokenProvider = new TokenProviderService(differentConfig);

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            PasswordHash = new byte[64],
            PasswordSalt = new byte[128]
        };

        var tokenWithDifferentIssuer = differentTokenProvider.GenerateAccessToken(user);

        // Act
        var result = _tokenProvider.ValidateToken(tokenWithDifferentIssuer);

        // Assert
        result.Should().BeNull();
    }
}
