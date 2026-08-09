using Xunit;
using FluentAssertions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TaskManagerApi.Data;
using TaskManagerApi.Services;
using TaskManagerApi.Models;
using TaskManagerApi.DTOs;
using TaskManagerApi.Mappings;

namespace TaskManagerApi.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            _mapper = mapperConfig.CreateMapper();

            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new AppDbContext(options);

            _taskService = new TaskService(_context, _mapper);
        }

        [Fact]
        public async Task CreateTask_Should_Create_New_Task()
        {
            // Arrange
            var dto = new TaskCreateDTO
            {
                Title = "Test Task"
            };

            // Act
            var result = await _taskService.CreateTask(dto);

            // Assert
            result.Should().NotBeNull();
            result.Title.Should().Be("Test Task");
            result.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllTasks_Should_Return_All_Tasks()
        {
            // Arrange
            _context.Tasks.AddRange(
                new TaskItem { Title = "Task 1", IsCompleted = false },
                new TaskItem { Title = "Task 2", IsCompleted = true }
            );

            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.GetAllTasks();

            // Assert
            result.Should().NotBeNull();
            result.Should().HaveCount(2);
        }

        [Fact]
        public async Task UpdateTask_Should_Update_Existing_Task()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Old Title",
                IsCompleted = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            var dto = new TaskUpdateDTO
            {
                Title = "Updated Title",
                IsCompleted = true
            };

            // Act
            var result = await _taskService.UpdateTask(task.Id, dto);

            // Assert
            result.Should().NotBeNull();
            result!.Title.Should().Be("Updated Title");
            result.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTask_Should_Delete_Existing_Task()
        {
            // Arrange
            var task = new TaskItem
            {
                Title = "Task to Delete",
                IsCompleted = false
            };

            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();

            // Act
            var result = await _taskService.DeleteTask(task.Id);

            // Assert
            result.Should().BeTrue();

            var deletedTask = await _context.Tasks.FindAsync(task.Id);
            deletedTask.Should().BeNull();
        }

        [Fact]
        public async Task UpdateTask_Should_Return_Null_When_Task_Not_Found()
        {
            // Arrange
            var dto = new TaskUpdateDTO
            {
                Title = "Updated Title",
                IsCompleted = true
            };

            // Act
            var result = await _taskService.UpdateTask(999, dto);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTask_Should_Return_False_When_Task_Not_Found()
        {
            // Act
            var result = await _taskService.DeleteTask(999);

            // Assert
            result.Should().BeFalse();
        }
    }
}