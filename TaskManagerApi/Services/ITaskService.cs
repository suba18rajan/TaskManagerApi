using TaskManagerApi.DTOs;
using TaskManagerApi.Models;


namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        List<TaskResponseDTO> GetAllTasks();
        TaskResponseDTO CreateTask(TaskCreateDTO dto);
        TaskResponseDTO? UpdateTask(int id, TaskUpdateDTO dto);
        bool DeleteTask(int id);
    }
}
