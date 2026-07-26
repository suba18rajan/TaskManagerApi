using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
        public async Task<IActionResult> GetTasks()
        {
            _logger.LogInformation("Getting all tasks.");

            var tasks = await _taskService.GetAllTasks();

            _logger.LogInformation("Retrieved {Count} tasks.", tasks.Count);

            return Ok(tasks);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTask(TaskCreateDTO dto)
        {
            _logger.LogInformation("Creating a new task.");

            var task = await _taskService.CreateTask(dto);

            _logger.LogInformation("Task created successfully.");

            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(int id, TaskUpdateDTO dto)
        {
            _logger.LogInformation("Updating task with Id {Id}.", id);

            var result = await _taskService.UpdateTask(id, dto);

            if (result == null)
            {
                _logger.LogWarning("Task with Id {Id} not found.", id);
                return NotFound("Task not found");
            }

            _logger.LogInformation("Task with Id {Id} updated successfully.", id);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            _logger.LogInformation("Deleting task with Id {Id}.", id);

            var deleted = await _taskService.DeleteTask(id);

            if (!deleted)
            {
                _logger.LogWarning("Task with Id {Id} not found.", id);
                return NotFound("Task not found");
            }

            _logger.LogInformation("Task with Id {Id} deleted successfully.", id);

            return Ok("Task deleted successfully");
        }
    }
}