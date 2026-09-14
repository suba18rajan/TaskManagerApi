using AutoMapper;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Mappings;
using TaskManagerApi.Models;
using TaskManagerApi.Services;
using Xunit;

namespace TaskManagerApi.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            var mapperConfig =
                 new MapperConfiguration(
                     cfg =>
                     {
                         cfg.AddProfile<MappingProfile>();
                     },
                     NullLoggerFactory.Instance);
            _mapper =
                mapperConfig.CreateMapper();

            var options =
                new DbContextOptionsBuilder<AppDbContext>()
                    .UseInMemoryDatabase(
                        Guid.NewGuid().ToString())
                    .Options;

            _context =
                new AppDbContext(options);

            _taskService =
                new TaskService(
                    _context,
                    _mapper);
        }

        [Fact]
        public async Task CreateTask_Should_Create_New_Task()
        {
            var dto =
                new TaskCreateDTO
                {
                    Title = "Test Task"
                };

            var result =
                await _taskService.CreateTask(dto);

            result.Should().NotBeNull();
            result.Title.Should().Be("Test Task");
            result.IsCompleted.Should().BeFalse();
        }

        [Fact]
        public async Task GetAllTasks_Should_Return_Paged_Tasks()
        {
            _context.Tasks.AddRange(
                new TaskItem
                {
                    Title = "Task 1",
                    IsCompleted = false
                },
                new TaskItem
                {
                    Title = "Task 2",
                    IsCompleted = true
                });

            await _context.SaveChangesAsync();

            var result =
                await _taskService.GetAllTasks(
                    1,
                    10,
                    null,
                    null);

            result.Items.Should().HaveCount(2);
            result.TotalCount.Should().Be(2);
            result.TotalPages.Should().Be(1);
        }

        [Fact]
        public async Task GetAllTasks_Should_Filter_By_Search()
        {
            _context.Tasks.AddRange(
                new TaskItem
                {
                    Title = "Learn C#"
                },
                new TaskItem
                {
                    Title = "Learn Angular"
                });

            await _context.SaveChangesAsync();

            var result =
                await _taskService.GetAllTasks(
                    1,
                    10,
                    "Angular",
                    null);

            result.Items.Should().ContainSingle();

            result.Items[0].Title
                .Should()
                .Be("Learn Angular");
        }

        [Fact]
        public async Task GetAllTasks_Should_Filter_By_Status()
        {
            _context.Tasks.AddRange(
                new TaskItem
                {
                    Title = "Open",
                    IsCompleted = false
                },
                new TaskItem
                {
                    Title = "Done",
                    IsCompleted = true
                });

            await _context.SaveChangesAsync();

            var result =
                await _taskService.GetAllTasks(
                    1,
                    10,
                    null,
                    true);

            result.Items.Should().ContainSingle();

            result.Items[0].Title
                .Should()
                .Be("Done");
        }

        [Fact]
        public async Task UpdateTask_Should_Update_Existing_Task()
        {
            var task =
                new TaskItem
                {
                    Title = "Old Title",
                    IsCompleted = false
                };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            var dto =
                new TaskUpdateDTO
                {
                    Title = "Updated Title",
                    IsCompleted = true
                };

            var result =
                await _taskService.UpdateTask(
                    task.Id,
                    dto);

            result.Should().NotBeNull();
            result!.Title.Should().Be("Updated Title");
            result.IsCompleted.Should().BeTrue();
        }

        [Fact]
        public async Task DeleteTask_Should_Delete_Existing_Task()
        {
            var task =
                new TaskItem
                {
                    Title = "Delete Me"
                };

            _context.Tasks.Add(task);

            await _context.SaveChangesAsync();

            var result =
                await _taskService.DeleteTask(
                    task.Id);

            result.Should().BeTrue();

            var deleted =
                await _context.Tasks.FindAsync(
                    task.Id);

            deleted.Should().BeNull();
        }

        [Fact]
        public async Task UpdateTask_Should_Return_Null_When_Not_Found()
        {
            var dto =
                new TaskUpdateDTO
                {
                    Title = "Updated",
                    IsCompleted = true
                };

            var result =
                await _taskService.UpdateTask(
                    999,
                    dto);

            result.Should().BeNull();
        }

        [Fact]
        public async Task DeleteTask_Should_Return_False_When_Not_Found()
        {
            var result =
                await _taskService.DeleteTask(999);

            result.Should().BeFalse();
        }
    }
}