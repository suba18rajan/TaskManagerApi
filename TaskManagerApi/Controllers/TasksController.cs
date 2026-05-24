using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;
using TaskManagerApi.Models;

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
    }
}