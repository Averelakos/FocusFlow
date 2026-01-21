using Moq;
using FluentAssertions;
using Xunit;

namespace FocusFlow.Application.Tests.Services;

/// <summary>
/// Tests for AuthService authentication and registration functionality
/// </summary>
public class AuthServiceTests
{
    private readonly Mock<IUserRepository> _mockUserRepository;
    private readonly Mock<ITokenProviderService> _mockTokenProvider;
    private readonly AuthService _authService;

    public AuthServiceTests()
    {
        _mockUserRepository = new Mock<IUserRepository>();
        _mockTokenProvider = new Mock<ITokenProviderService>();
        _authService = new AuthService(_mockUserRepository.Object, _mockTokenProvider.Object);
    }

    [Fact]
    public async Task RegisterAsync_WithNewUser_ShouldReturnSuccessWithToken()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "testuser",
            Email = "test@example.com",
            Password = "password123",
            FullName = "Test User"
        };

        _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var expectedUser = new User
        {
            Id = 1,
            Username = registerDto.Username,
            Email = registerDto.Email,
            FullName = registerDto.FullName,
            PasswordHash = new byte[64],
            PasswordSalt = new byte[128]
        };

        _mockUserRepository.Setup(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(expectedUser);

        _mockTokenProvider.Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
            .Returns("test-jwt-token");

        // Act
        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Token.Should().Be("test-jwt-token");
        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task RegisterAsync_WithExistingUser_ShouldReturnFailure()
    {
        // Arrange
        var registerDto = new RegisterDto
        {
            Username = "existinguser",
            Email = "existing@example.com",
            Password = "password123",
            FullName = "Existing User"
        };

        _mockUserRepository.Setup(x => x.AnyAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _authService.RegisterAsync(registerDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("User already exists.");
        _mockUserRepository.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task LoginAsync_WithValidCredentials_ShouldReturnSuccessWithToken()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UsernameOrEmail = "testuser",
            Password = "password123"
        };

        var passwordHash = new byte[64];
        var passwordSalt = new byte[128];
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(loginDto.Password));
        }

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _mockUserRepository.Setup(x => x.GetByParameterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        _mockTokenProvider.Setup(x => x.GenerateAccessToken(It.IsAny<User>()))
            .Returns("test-jwt-token");

        // Act
        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeTrue();
        result.Token.Should().Be("test-jwt-token");
    }

    [Fact]
    public async Task LoginAsync_WithInvalidUsername_ShouldReturnFailure()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UsernameOrEmail = "nonexistent",
            Password = "password123"
        };

        _mockUserRepository.Setup(x => x.GetByParameterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((User?)null);

        // Act
        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("User not found.");
    }

    [Fact]
    public async Task LoginAsync_WithInvalidPassword_ShouldReturnFailure()
    {
        // Arrange
        var loginDto = new LoginDto
        {
            UsernameOrEmail = "testuser",
            Password = "wrongpassword"
        };

        var passwordHash = new byte[64];
        var passwordSalt = new byte[128];
        using (var hmac = new System.Security.Cryptography.HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes("correctpassword"));
        }

        var user = new User
        {
            Id = 1,
            Username = "testuser",
            Email = "test@example.com",
            FullName = "Test User",
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _mockUserRepository.Setup(x => x.GetByParameterAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(user);

        // Act
        var result = await _authService.LoginAsync(loginDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Success.Should().BeFalse();
        result.Message.Should().Be("Invalid password.");
    }
}
