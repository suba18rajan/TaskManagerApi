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
        [HttpGet]
        public IActionResult GetTasks()
        {
            return Ok(TaskData.Tasks);
        }

        [HttpPost]
        public IActionResult CreateTask(TaskItem newTask)
        {
            TaskData.Tasks.Add(newTask);

            return Ok(newTask);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTask(int id, TaskItem updatedTask)
        {
            var existingTask = TaskData.Tasks.FirstOrDefault(x => x.Id == id);

            if (existingTask == null)
            {
                return NotFound("Task not found");
            }

            existingTask.Title = updatedTask.Title;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            return Ok(existingTask);
        }
    }
}