using TaskManagerApi.Models;


namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        List<TaskItem> GetAllTasks();
        TaskItem CreateTask(TaskItem task);
    }
}
