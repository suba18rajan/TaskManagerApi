using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagerApi.Controllers;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;
using Xunit;

namespace TaskManagerApi.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<ILogger<TasksController>> _loggerMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _taskServiceMock = new Mock<ITaskService>();
            _loggerMock = new Mock<ILogger<TasksController>>();

            _controller = new TasksController(
                _taskServiceMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task GetTasks_Should_Return_Ok()
        {
            // Arrange
            var tasks = new List<TaskResponseDTO>
            {
                new TaskResponseDTO
                {
                    Id = 1,
                    Title = "Task 1",
                    IsCompleted = false
                },
                new TaskResponseDTO
                {
                    Id = 2,
                    Title = "Task 2",
                    IsCompleted = true
                }
            };

            _taskServiceMock
                .Setup(x => x.GetAllTasks())
                .ReturnsAsync(tasks);

            // Act
            var result = await _controller.GetTasks();

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(tasks);
        }

        [Fact]
        public async Task CreateTask_Should_Return_Ok()
        {
            // Arrange
            var dto = new TaskCreateDTO
            {
                Title = "New Task"
            };

            var createdTask = new TaskResponseDTO
            {
                Id = 1,
                Title = "New Task",
                IsCompleted = false
            };

            _taskServiceMock
                .Setup(x => x.CreateTask(dto))
                .ReturnsAsync(createdTask);

            // Act
            var result = await _controller.CreateTask(dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(createdTask);
        }

        [Fact]
        public async Task UpdateTask_Should_Return_Ok_When_Task_Exists()
        {
            // Arrange
            var dto = new TaskUpdateDTO
            {
                Title = "Updated Task",
                IsCompleted = true
            };

            var updatedTask = new TaskResponseDTO
            {
                Id = 1,
                Title = "Updated Task",
                IsCompleted = true
            };

            _taskServiceMock
                .Setup(x => x.UpdateTask(1, dto))
                .ReturnsAsync(updatedTask);

            // Act
            var result = await _controller.UpdateTask(1, dto);

            // Assert
            result.Should().BeOfType<OkObjectResult>();

            var okResult = result as OkObjectResult;

            okResult!.Value.Should().BeEquivalentTo(updatedTask);
        }

        [Fact]
        public async Task UpdateTask_Should_Return_NotFound_When_Task_Does_Not_Exist()
        {
            // Arrange
            var dto = new TaskUpdateDTO
            {
                Title = "Updated Task",
                IsCompleted = true
            };

            _taskServiceMock
                .Setup(x => x.UpdateTask(999, dto))
                .ReturnsAsync((TaskResponseDTO?)null);

            // Act
            var result = await _controller.UpdateTask(999, dto);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeleteTask_Should_Return_Ok_When_Task_Exists()
        {
            // Arrange
            _taskServiceMock
                .Setup(x => x.DeleteTask(1))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteTask(1);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteTask_Should_Return_NotFound_When_Task_Does_Not_Exist()
        {
            // Arrange
            _taskServiceMock
                .Setup(x => x.DeleteTask(999))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteTask(999);

            // Assert
            result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}