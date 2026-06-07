using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.DTOs;
using TaskManagerApi.Services;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(_taskService.GetAllTasks());
        }

        [HttpPost]
        public IActionResult CreateTask(TaskCreateDTO dto)
        {
            return Ok(_taskService.CreateTask(dto));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskUpdateDTO dto)
        {
            var result = _taskService.UpdateTask(id, dto);

            if (result == null)
                return NotFound("Task not found");

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var deleted = _taskService.DeleteTask(id);

            if (!deleted)
                return NotFound("Task not found");

            return Ok("Task deleted successfully");
        }
    }
}