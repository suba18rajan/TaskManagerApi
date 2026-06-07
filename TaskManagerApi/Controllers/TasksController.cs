using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using TaskManagerApi.Services;
using System.Linq;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;
        private readonly AppDbContext _context;
        public TasksController(ITaskService taskService, AppDbContext context)
        {
            _taskService = taskService;
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(_taskService.GetAllTasks());
        }

        [HttpPost]
        public IActionResult CreateTask(TaskItem newTask)
        {
            return Ok(_taskService.CreateTask(newTask));
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskItem updatedTask)
        {
            var result = _taskService.UpdateTask(id, updatedTask);

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