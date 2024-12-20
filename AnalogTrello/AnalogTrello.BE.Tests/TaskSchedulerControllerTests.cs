using AnalogTrelloBE.Controllers;
using AnalogTrelloBE.Dto;
using AnalogTrelloBE.Interfaces.IRepository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TaskScheduler = AnalogTrelloBE.Models.TaskScheduler;

namespace AnalogTrello.BE.Tests;

public class TaskSchedulerControllerTests
{
    private readonly Mock<ITaskSchedulerRepository> _mockRepo;
    private readonly TaskSchedulerController _controller;

    public TaskSchedulerControllerTests()
    {
        _mockRepo = new Mock<ITaskSchedulerRepository>();
        _controller = new TaskSchedulerController(_mockRepo.Object);
    }

    [Fact]
    public async Task GetTasks_ShouldReturnSuccess_WhenTasksExist()
    {
        // Arrange
        var tasks = new[] { new TaskScheduler() { Id = 1, Title = "Test Task" } };
        _mockRepo.Setup(repo => repo.GetAllTasks()).ReturnsAsync(tasks);

        // Act
        var result = await _controller.GetTasks();

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(tasks, result.Result);
    }

    [Fact]
    public async Task GetTasks_ShouldReturnFailed_WhenNoTasksExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetAllTasks()).ReturnsAsync(new TaskScheduler[0]);

        // Act
        var result = await _controller.GetTasks();

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Not found", result.ErrorMessages);
    }

    [Fact]
    public async Task GetTask_ShouldReturnSuccess_WhenTaskExists()
    {
        // Arrange
        var task = new TaskScheduler { Id = 1, Title = "Test Task" };
        _mockRepo.Setup(repo => repo.GetTask(1)).ReturnsAsync(task);

        // Act
        var result = await _controller.GetTask(1);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(task, result.Result);
    }

    [Fact]
    public async Task GetTask_ShouldReturnFailed_WhenTaskDoesNotExist()
    {
        // Arrange
        _mockRepo.Setup(repo => repo.GetTask(1)).ReturnsAsync(new TaskScheduler());

        // Act
        var result = await _controller.GetTask(1);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.IsSuccess);
        Assert.Equal("Not found", result.ErrorMessages);
    }

    [Fact]
    public async Task CreateTask_ShouldReturnSuccess()
    {
        // Arrange
        var taskDto = new TaskSchedulerDto { Title = "New Task" };
        var createdTask = new TaskSchedulerDto { Id = 1, Title = "New Task" };
        _mockRepo.Setup(repo => repo.CreateTask(taskDto)).ReturnsAsync(createdTask);

        // Act
        var result = await _controller.CreateTask(taskDto);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.IsSuccess);
        Assert.Equal(createdTask, result.Result);
    }

    [Fact]
    public void UpdateTask_ShouldReturnNoContent()
    {
        // Arrange
        var taskDto = new TaskSchedulerDto { Id = 1, Title = "Updated Task" };

        // Act
        var result = _controller.UpdateTask(taskDto) as OkResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        _mockRepo.Verify(repo => repo.UpdateTask(taskDto), Times.Once);
    }

    [Fact]
    public void DeleteTask_ShouldReturnNoContent()
    {
        // Arrange
        long taskId = 1;

        // Act
        var result = _controller.DeleteTask(taskId) as OkResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(200, result.StatusCode);
        _mockRepo.Verify(repo => repo.DeleteTask(taskId), Times.Once);
    }
}