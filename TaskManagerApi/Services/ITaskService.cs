using TaskManagerApi.DTOs;

namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        Task<List<TaskResponseDTO>> GetAllTasks();

        Task<TaskResponseDTO> CreateTask(TaskCreateDTO dto);

        Task<TaskResponseDTO?> UpdateTask(int id, TaskUpdateDTO dto);

        Task<bool> DeleteTask(int id);
    }
}