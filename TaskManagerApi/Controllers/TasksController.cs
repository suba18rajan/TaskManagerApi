using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly ILogger<TasksController> _logger;

        public TasksController(
            ITaskService taskService,
            ILogger<TasksController> logger)
        {
            _taskService = taskService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetTasks(
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] bool? isCompleted = null)
        {
            var result =
                await _taskService.GetAllTasks(
                    pageNumber,
                    pageSize,
                    search,
                    isCompleted);

            _logger.LogInformation(
                "Retrieved {Count} tasks.",
                result.Items.Count);

            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> GetTaskById(
            int id)
        {
            var task =
                await _taskService.GetTaskById(id);

            if (task == null)
            {
                _logger.LogWarning(
                    "Task {Id} not found.",
                    id);

                return NotFound("Task not found");
            }

            return Ok(task);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateTask(
            TaskCreateDTO dto)
        {
            var task =
                await _taskService.CreateTask(dto);

            return CreatedAtAction(
                nameof(GetTaskById),
                new { id = task.Id },
                task);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateTask(
            int id,
            TaskUpdateDTO dto)
        {
            var result =
                await _taskService.UpdateTask(
                    id,
                    dto);

            if (result == null)
                return NotFound("Task not found");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteTask(
            int id)
        {
            var deleted =
                await _taskService.DeleteTask(id);

            if (!deleted)
                return NotFound("Task not found");

            return NoContent();
        }
    }
}