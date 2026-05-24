using Microsoft.AspNetCore.Mvc;
using TaskManagerApi.Data;

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
    }
}