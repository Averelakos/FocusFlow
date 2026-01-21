using Moq;
using FluentAssertions;
using Xunit;
using Microsoft.Extensions.Caching.Memory;

namespace FocusFlow.Application.Tests.Services;

/// <summary>
/// Tests for ProjectService CRUD operations and caching functionality
/// </summary>
public class ProjectServiceTests
{
    private readonly Mock<IProjectRepository> _mockProjectRepository;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IMemoryCache> _mockCache;
    private readonly ProjectService _projectService;

    public ProjectServiceTests()
    {
        _mockProjectRepository = new Mock<IProjectRepository>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockCache = new Mock<IMemoryCache>();
        _projectService = new ProjectService(_mockProjectRepository.Object, _mockCurrentUserService.Object, _mockCache.Object);
    }

    [Fact]
    public async Task GetProjectById_WithValidId_ShouldReturnProject()
    {
        // Arrange
        var projectId = 1L;
        var project = new Project
        {
            Id = projectId,
            Name = "Test Project",
            Description = "Test Description",
            OwnerId = 1,
            Owner = new User { Id = 1, Username = "testuser", Email = "test@test.com", FullName = "Test User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockProjectRepository.Setup(x => x.GetAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);

        // Act
        var result = await _projectService.GetProjectById(projectId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(projectId);
        result.Name.Should().Be("Test Project");
        result.Description.Should().Be("Test Description");
    }

    [Fact]
    public async Task GetProjectById_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var projectId = 999L;
        _mockProjectRepository.Setup(x => x.GetAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var act = async () => await _projectService.GetProjectById(projectId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"Project with id {projectId} not found");
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateProject()
    {
        // Arrange
        var userId = 1L;
        var createDto = new CreateProjectDto
        {
            Name = "New Project",
            Description = "New Description",
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow.AddMonths(3)
        };

        var createdProject = new Project
        {
            Id = 1,
            Name = createDto.Name,
            Description = createDto.Description,
            OwnerId = userId,
            StartDate = createDto.StartDate,
            EndDate = createDto.EndDate,
            Owner = new User { Id = userId, Username = "testuser", Email = "test@test.com", FullName = "Test User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);
        _mockProjectRepository.Setup(x => x.AddAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProject);
        _mockProjectRepository.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdProject);

        // Act
        var result = await _projectService.CreateAsync(createDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(createDto.Name);
        result.Description.Should().Be(createDto.Description);
        result.OwnerId.Should().Be(userId);
        _mockCache.Verify(x => x.Remove(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WithoutAuthentication_ShouldThrowUnauthorizedException()
    {
        // Arrange
        var createDto = new CreateProjectDto
        {
            Name = "New Project",
            Description = "New Description"
        };

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns((long?)null);

        // Act
        var act = async () => await _projectService.CreateAsync(createDto, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<UnauthorizedException>()
            .WithMessage("User not authenticated");
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateProject()
    {
        // Arrange
        var userId = 1L;
        var updateDto = new UpdateProjectDto
        {
            Id = 1,
            Name = "Updated Project",
            Description = "Updated Description"
        };

        var existingProject = new Project
        {
            Id = updateDto.Id,
            Name = "Old Name",
            Description = "Old Description",
            OwnerId = userId,
            Owner = new User { Id = userId, Username = "testuser", Email = "test@test.com", FullName = "Test User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);
        _mockProjectRepository.Setup(x => x.GetAsync(updateDto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProject);
        _mockProjectRepository.Setup(x => x.UpdateAsync(It.IsAny<Project>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _projectService.UpdateAsync(updateDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(updateDto.Name);
        result.Description.Should().Be(updateDto.Description);
        _mockCache.Verify(x => x.Remove(It.IsAny<object>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WithDifferentOwner_ShouldThrowForbiddenException()
    {
        // Arrange
        var userId = 1L;
        var updateDto = new UpdateProjectDto
        {
            Id = 1,
            Name = "Updated Project",
            Description = "Updated Description"
        };

        var existingProject = new Project
        {
            Id = updateDto.Id,
            Name = "Old Name",
            Description = "Old Description",
            OwnerId = 2, // Different owner
            Owner = new User { Id = 2, Username = "otheruser", Email = "other@test.com", FullName = "Other User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);
        _mockProjectRepository.Setup(x => x.GetAsync(updateDto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProject);

        // Act
        var act = async () => await _projectService.UpdateAsync(updateDto, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<ForbiddenException>()
            .WithMessage("You do not have permission to update this project");
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteProject()
    {
        // Arrange
        var userId = 1L;
        var projectId = 1L;
        var existingProject = new Project
        {
            Id = projectId,
            Name = "Project to Delete",
            OwnerId = userId,
            Owner = new User { Id = userId, Username = "testuser", Email = "test@test.com", FullName = "Test User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);
        _mockProjectRepository.Setup(x => x.GetAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingProject);
        _mockProjectRepository.Setup(x => x.DeleteAsync(projectId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _projectService.DeleteAsync(projectId, CancellationToken.None);

        // Assert
        _mockProjectRepository.Verify(x => x.DeleteAsync(projectId, It.IsAny<CancellationToken>()), Times.Once);
        _mockCache.Verify(x => x.Remove(It.IsAny<object>()), Times.Once);
    }
}
