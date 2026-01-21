using FluentAssertions;
using Xunit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace FocusFlow.Infrastructure.Tests.Repositories;

/// <summary>
/// Tests for BaseRepository CRUD operations with in-memory database
/// </summary>
public class BaseRepositoryTests : IDisposable
{
    private readonly FocusFlowDbContext _context;
    private readonly TestRepository _repository;
    private readonly MockCurrentUserService _currentUserService;
    private readonly Mock<ILogger<BaseRepository<Project>>> _mockLogger;

    public BaseRepositoryTests()
    {
        var options = new DbContextOptionsBuilder<FocusFlowDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new FocusFlowDbContext(options);
        _currentUserService = new MockCurrentUserService();
        _mockLogger = new Mock<ILogger<BaseRepository<Project>>>();
        _repository = new TestRepository(_context, _currentUserService, _mockLogger.Object);
    }

    [Fact]
    public async Task AddAsync_ShouldAddEntityWithAuditFields()
    {
        // Arrange
        var userId = 1L;
        _currentUserService.SetUserId(userId);

        var project = new Project
        {
            Name = "Test Project",
            Description = "Test Description",
            OwnerId = userId
        };

        // Act
        var result = await _repository.AddAsync(project, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().BeGreaterThan(0);
        result.Created.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        result.CreatedById.Should().Be(userId);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnEntity()
    {
        // Arrange
        var project = new Project
        {
            Name = "Test Project",
            Description = "Test Description",
            OwnerId = 1,
            Created = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };
        _context.Set<Project>().Add(project);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.GetAsync(project.Id, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result!.Id.Should().Be(project.Id);
        result.Name.Should().Be("Test Project");
    }

    [Fact]
    public async Task GetAsync_WithInvalidId_ShouldReturnNull()
    {
        // Act
        var result = await _repository.GetAsync(999, CancellationToken.None);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateEntityWithAuditFields()
    {
        // Arrange
        var userId = 1L;
        _currentUserService.SetUserId(userId);

        var project = new Project
        {
            Name = "Original Name",
            Description = "Original Description",
            OwnerId = userId,
            Created = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };
        _context.Set<Project>().Add(project);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear();

        // Act
        var entityToUpdate = await _context.Set<Project>().FindAsync(project.Id);
        entityToUpdate!.Name = "Updated Name";
        var result = await _repository.UpdateAsync(entityToUpdate, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var updated = await _context.Set<Project>().FindAsync(project.Id);
        updated!.Name.Should().Be("Updated Name");
        updated.LastUpdated.Should().NotBeNull();
        updated.LastUpdatedById.Should().Be(userId);
    }

    [Fact]
    public async Task DeleteAsync_ShouldRemoveEntity()
    {
        // Arrange
        var project = new Project
        {
            Name = "Test Project",
            Description = "Test Description",
            OwnerId = 1,
            Created = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };
        _context.Set<Project>().Add(project);
        await _context.SaveChangesAsync();
        _context.ChangeTracker.Clear(); // Clear tracked entities

        // Act
        var result = await _repository.DeleteAsync(project.Id, CancellationToken.None);

        // Assert
        result.Should().BeTrue();
        var deleted = await _context.Set<Project>().FindAsync(project.Id);
        deleted.Should().BeNull();
    }

    [Fact]
    public async Task AnyAsync_ShouldReturnTrueWhenEntityExists()
    {
        // Arrange
        var project = new Project
        {
            Name = "Test Project",
            Description = "Test Description",
            OwnerId = 1,
            Created = DateTime.UtcNow,
            LastUpdated = DateTime.UtcNow
        };
        _context.Set<Project>().Add(project);
        await _context.SaveChangesAsync();

        // Act
        var result = await _repository.AnyAsync(p => p.Name == "Test Project", CancellationToken.None);

        // Assert
        result.Should().BeTrue();
    }

    [Fact]
    public async Task AnyAsync_ShouldReturnFalseWhenEntityDoesNotExist()
    {
        // Act
        var result = await _repository.AnyAsync(p => p.Name == "Nonexistent", CancellationToken.None);

        // Assert
        result.Should().BeFalse();
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    // Helper classes for testing
    private class TestRepository : BaseRepository<Project>
    {
        public TestRepository(FocusFlowDbContext context, ICurrentUserService currentUserService, ILogger<BaseRepository<Project>> logger)
            : base(logger, context, currentUserService)
        {
        }
    }

    private class MockCurrentUserService : ICurrentUserService
    {
        private long? _userId;

        public void SetUserId(long? userId) => _userId = userId;
        public long? GetUserId() => _userId;
        public string? GetUserEmail() => "test@example.com";
        public string? GetUsername() => "testuser";
    }
}
