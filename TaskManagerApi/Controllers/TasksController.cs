using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Models;
using System.Linq;

namespace TaskManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TasksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TasksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(_context.Tasks.ToList());
        }

        [HttpPost]
        public IActionResult CreateTask(TaskItem newTask)
        {
            _context.Tasks.Add(newTask);
            _context.SaveChanges();

            return Ok(newTask);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskItem updatedTask)
        {
            var existingTask = _context.Tasks.FirstOrDefault(x => x.Id == id);

            if (existingTask == null)
            {
                return NotFound("Task not found");
            }

            existingTask.Title = updatedTask.Title;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            _context.SaveChanges();

            return Ok(existingTask);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.Id == id);

            if (task == null)
            {
                return NotFound("Task not found");
            }

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return Ok("Task deleted successfully");
        }
    }
}