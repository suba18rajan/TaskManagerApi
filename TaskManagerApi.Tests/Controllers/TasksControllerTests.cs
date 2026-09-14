using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using TaskManagerApi.Controllers;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Tests.Controllers
{
    public class TasksControllerTests
    {
        private readonly Mock<ITaskService> _taskServiceMock;
        private readonly Mock<ILogger<TasksController>> _loggerMock;
        private readonly TasksController _controller;

        public TasksControllerTests()
        {
            _taskServiceMock =
                new Mock<ITaskService>();

            _loggerMock =
                new Mock<ILogger<TasksController>>();

            _controller =
                new TasksController(
                    _taskServiceMock.Object,
                    _loggerMock.Object);
        }

        [Fact]
        public async Task GetTasks_Should_Return_Ok()
        {
            var resultData =
                new PagedTaskResponseDTO
                {
                    Items =
                        new List<TaskResponseDTO>
                        {
                            new()
                            {
                                Id = 1,
                                Title = "Task 1",
                                IsCompleted = false
                            }
                        },

                    PageNumber = 1,
                    PageSize = 10,
                    TotalCount = 1,
                    TotalPages = 1
                };

            _taskServiceMock
                .Setup(x =>
                    x.GetAllTasks(
                        1,
                        10,
                        null,
                        null))
                .ReturnsAsync(resultData);

            var result =
                await _controller.GetTasks();

            result.Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task CreateTask_Should_Return_Created()
        {
            var dto =
                new TaskCreateDTO
                {
                    Title = "New Task"
                };

            var createdTask =
                new TaskResponseDTO
                {
                    Id = 1,
                    Title = "New Task",
                    IsCompleted = false
                };

            _taskServiceMock
                .Setup(x => x.CreateTask(dto))
                .ReturnsAsync(createdTask);

            var result =
                await _controller.CreateTask(dto);

            result.Should()
                .BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task UpdateTask_Should_Return_Ok()
        {
            var dto =
                new TaskUpdateDTO
                {
                    Title = "Updated",
                    IsCompleted = true
                };

            var task =
                new TaskResponseDTO
                {
                    Id = 1,
                    Title = "Updated",
                    IsCompleted = true
                };

            _taskServiceMock
                .Setup(x =>
                    x.UpdateTask(1, dto))
                .ReturnsAsync(task);

            var result =
                await _controller.UpdateTask(
                    1,
                    dto);

            result.Should()
                .BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task UpdateTask_Should_Return_NotFound()
        {
            var dto =
                new TaskUpdateDTO
                {
                    Title = "Updated",
                    IsCompleted = true
                };

            _taskServiceMock
                .Setup(x =>
                    x.UpdateTask(
                        999,
                        dto))
                .ReturnsAsync(
                    (TaskResponseDTO?)null);

            var result =
                await _controller.UpdateTask(
                    999,
                    dto);

            result.Should()
                .BeOfType<NotFoundObjectResult>();
        }

        [Fact]
        public async Task DeleteTask_Should_Return_NoContent()
        {
            _taskServiceMock
                .Setup(x => x.DeleteTask(1))
                .ReturnsAsync(true);

            var result =
                await _controller.DeleteTask(1);

            result.Should()
                .BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteTask_Should_Return_NotFound()
        {
            _taskServiceMock
                .Setup(x => x.DeleteTask(999))
                .ReturnsAsync(false);

            var result =
                await _controller.DeleteTask(999);

            result.Should()
                .BeOfType<NotFoundObjectResult>();
        }
    }
}