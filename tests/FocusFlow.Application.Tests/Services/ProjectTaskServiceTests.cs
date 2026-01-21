using Moq;
using FluentAssertions;
using Xunit;
using Microsoft.AspNetCore.SignalR;
using FocusFlow.Application.Hubs;

namespace FocusFlow.Application.Tests.Services;

/// <summary>
/// Tests for ProjectTaskService CRUD operations and SignalR notifications
/// </summary>
public class ProjectTaskServiceTests
{
    private readonly Mock<IProjectTaskRepository> _mockTaskRepository;
    private readonly Mock<ICurrentUserService> _mockCurrentUserService;
    private readonly Mock<IHubContext<TaskHub>> _mockHubContext;
    private readonly Mock<IHubClients> _mockClients;
    private readonly Mock<IClientProxy> _mockClientProxy;
    private readonly ProjectTaskService _taskService;

    public ProjectTaskServiceTests()
    {
        _mockTaskRepository = new Mock<IProjectTaskRepository>();
        _mockCurrentUserService = new Mock<ICurrentUserService>();
        _mockHubContext = new Mock<IHubContext<TaskHub>>();
        _mockClients = new Mock<IHubClients>();
        _mockClientProxy = new Mock<IClientProxy>();

        _mockHubContext.Setup(x => x.Clients).Returns(_mockClients.Object);
        _mockClients.Setup(x => x.All).Returns(_mockClientProxy.Object);

        _taskService = new ProjectTaskService(
            _mockTaskRepository.Object,
            _mockCurrentUserService.Object,
            _mockHubContext.Object
        );
    }

    [Fact]
    public async Task GetProjectTaskById_WithValidId_ShouldReturnTask()
    {
        // Arrange
        var taskId = 1L;
        var task = new ProjectTask
        {
            Id = taskId,
            Title = "Test Task",
            Description = "Test Description",
            ProjectId = 1,
            AssignedToId = 1,
            Status = ProjectTaskStatus.Todo,
            Priority = ProjectTaskPriority.Medium,
            Project = new Project { Id = 1, Name = "Test Project", OwnerId = 1, Owner = new User { Id = 1, Username = "test", Email = "test@test.com", FullName = "Test", PasswordHash = new byte[64], PasswordSalt = new byte[128] } },
            AssignedTo = new User { Id = 1, Username = "testuser", Email = "test@test.com", FullName = "Test User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockTaskRepository.Setup(x => x.GetAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(task);

        // Act
        var result = await _taskService.GetProjectTaskById(taskId, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(taskId);
        result.Title.Should().Be("Test Task");
        result.Status.Should().Be(ProjectTaskStatus.Todo);
    }

    [Fact]
    public async Task GetProjectTaskById_WithInvalidId_ShouldThrowNotFoundException()
    {
        // Arrange
        var taskId = 999L;
        _mockTaskRepository.Setup(x => x.GetAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((ProjectTask?)null);

        // Act
        var act = async () => await _taskService.GetProjectTaskById(taskId, CancellationToken.None);

        // Assert
        await act.Should().ThrowAsync<NotFoundException>()
            .WithMessage($"ProjectTask with id {taskId} not found");
    }

    [Fact]
    public async Task CreateAsync_WithValidData_ShouldCreateTaskAndNotify()
    {
        // Arrange
        var userId = 1L;
        var createDto = new CreateProjectTaskDto
        {
            Title = "New Task",
            Description = "New Description",
            ProjectId = 1,
            DueDate = DateTime.UtcNow.AddDays(7),
            Status = ProjectTaskStatus.Todo,
            Priority = ProjectTaskPriority.High
        };

        var createdTask = new ProjectTask
        {
            Id = 1,
            Title = createDto.Title,
            Description = createDto.Description,
            ProjectId = createDto.ProjectId,
            AssignedToId = userId,
            DueDate = createDto.DueDate,
            Status = createDto.Status,
            Priority = createDto.Priority,
            Project = new Project { Id = 1, Name = "Test Project", OwnerId = userId, Owner = new User { Id = userId, Username = "test", Email = "test@test.com", FullName = "Test", PasswordHash = new byte[64], PasswordSalt = new byte[128] } },
            AssignedTo = new User { Id = userId, Username = "testuser", Email = "test@test.com", FullName = "Test User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);
        _mockTaskRepository.Setup(x => x.AddAsync(It.IsAny<ProjectTask>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTask);
        _mockTaskRepository.Setup(x => x.GetAsync(It.IsAny<long>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(createdTask);

        // Act
        var result = await _taskService.CreateAsync(createDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(createDto.Title);
        result.Priority.Should().Be(ProjectTaskPriority.High);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync("TaskCreated", It.Is<object[]>(o => o.Length > 0), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateAsync_WithValidData_ShouldUpdateTaskAndNotify()
    {
        // Arrange
        var userId = 1L;
        var updateDto = new UpdateProjectTaskDto
        {
            Id = 1,
            Title = "Updated Task",
            Description = "Updated Description",
            Status = ProjectTaskStatus.InProgress,
            Priority = ProjectTaskPriority.Critical,
            DueDate = DateTime.UtcNow.AddDays(3)
        };

        var existingTask = new ProjectTask
        {
            Id = updateDto.Id,
            Title = "Old Title",
            Description = "Old Description",
            ProjectId = 1,
            AssignedToId = userId,
            Status = ProjectTaskStatus.Todo,
            Priority = ProjectTaskPriority.Low,
            Project = new Project { Id = 1, Name = "Test Project", OwnerId = userId, Owner = new User { Id = userId, Username = "test", Email = "test@test.com", FullName = "Test", PasswordHash = new byte[64], PasswordSalt = new byte[128] } },
            AssignedTo = new User { Id = userId, Username = "testuser", Email = "test@test.com", FullName = "Test User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);
        _mockTaskRepository.Setup(x => x.GetAsync(updateDto.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);
        _mockTaskRepository.Setup(x => x.UpdateAsync(It.IsAny<ProjectTask>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _taskService.UpdateAsync(updateDto, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(updateDto.Title);
        result.Status.Should().Be(ProjectTaskStatus.InProgress);
        result.Priority.Should().Be(ProjectTaskPriority.Critical);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync("TaskUpdated", It.Is<object[]>(o => o.Length > 0), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteAsync_WithValidId_ShouldDeleteTaskAndNotify()
    {
        // Arrange
        var userId = 1L;
        var taskId = 1L;
        var existingTask = new ProjectTask
        {
            Id = taskId,
            Title = "Task to Delete",
            ProjectId = 1,
            AssignedToId = userId,
            Project = new Project { Id = 1, Name = "Test Project", OwnerId = userId, Owner = new User { Id = userId, Username = "test", Email = "test@test.com", FullName = "Test", PasswordHash = new byte[64], PasswordSalt = new byte[128] } },
            AssignedTo = new User { Id = userId, Username = "testuser", Email = "test@test.com", FullName = "Test User", PasswordHash = new byte[64], PasswordSalt = new byte[128] }
        };

        _mockCurrentUserService.Setup(x => x.GetUserId()).Returns(userId);
        _mockTaskRepository.Setup(x => x.GetAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingTask);
        _mockTaskRepository.Setup(x => x.DeleteAsync(taskId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        await _taskService.DeleteAsync(taskId, CancellationToken.None);

        // Assert
        _mockTaskRepository.Verify(x => x.DeleteAsync(taskId, It.IsAny<CancellationToken>()), Times.Once);
        _mockClientProxy.Verify(
            x => x.SendCoreAsync("TaskDeleted", It.Is<object[]>(o => o.Length > 0), It.IsAny<CancellationToken>()),
            Times.Once
        );
    }

    [Fact]
    public void GetStatistics_ShouldReturnCorrectCounts()
    {
        // Arrange
        var tasks = new List<ProjectTask>
        {
            new ProjectTask { Id = 1, Status = ProjectTaskStatus.Todo, DueDate = DateTime.UtcNow.AddDays(5), ProjectId = 1, AssignedToId = 1, Project = new Project { Id = 1, Name = "P1", OwnerId = 1, Owner = new User { Id = 1, Username = "u", Email = "e@e.com", FullName = "U", PasswordHash = new byte[64], PasswordSalt = new byte[128] } }, AssignedTo = new User { Id = 1, Username = "u", Email = "e@e.com", FullName = "U", PasswordHash = new byte[64], PasswordSalt = new byte[128] } },
            new ProjectTask { Id = 2, Status = ProjectTaskStatus.InProgress, DueDate = DateTime.UtcNow.AddDays(3), ProjectId = 1, AssignedToId = 1, Project = new Project { Id = 1, Name = "P1", OwnerId = 1, Owner = new User { Id = 1, Username = "u", Email = "e@e.com", FullName = "U", PasswordHash = new byte[64], PasswordSalt = new byte[128] } }, AssignedTo = new User { Id = 1, Username = "u", Email = "e@e.com", FullName = "U", PasswordHash = new byte[64], PasswordSalt = new byte[128] } },
            new ProjectTask { Id = 3, Status = ProjectTaskStatus.Done, DueDate = DateTime.UtcNow.AddDays(-2), ProjectId = 1, AssignedToId = 1, Project = new Project { Id = 1, Name = "P1", OwnerId = 1, Owner = new User { Id = 1, Username = "u", Email = "e@e.com", FullName = "U", PasswordHash = new byte[64], PasswordSalt = new byte[128] } }, AssignedTo = new User { Id = 1, Username = "u", Email = "e@e.com", FullName = "U", PasswordHash = new byte[64], PasswordSalt = new byte[128] } },
            new ProjectTask { Id = 4, Status = ProjectTaskStatus.Todo, DueDate = DateTime.UtcNow.AddDays(-1), ProjectId = 1, AssignedToId = 1, Project = new Project { Id = 1, Name = "P1", OwnerId = 1, Owner = new User { Id = 1, Username = "u", Email = "e@e.com", FullName = "U", PasswordHash = new byte[64], PasswordSalt = new byte[128] } }, AssignedTo = new User { Id = 1, Username = "u", Email = "e@e.com", FullName = "U", PasswordHash = new byte[64], PasswordSalt = new byte[128] } }
        }.AsQueryable();

        _mockTaskRepository.Setup(x => x.Queryable()).Returns(tasks);

        // Act
        var result = _taskService.GetStatistics();

        // Assert
        result.Should().NotBeNull();
        result.TotalTasks.Should().Be(4);
        result.CompletedTasks.Should().Be(1);
        result.OverdueTasks.Should().Be(1); // Task 4 is overdue and not done
        result.InProgressTasks.Should().Be(1);
        result.TodoTasks.Should().Be(2);
    }
}
