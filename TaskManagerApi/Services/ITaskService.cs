using TaskManagerApi.DTOs;

namespace TaskManagerApi.Services
{
    public interface ITaskService
    {
        Task<PagedTaskResponseDTO> GetAllTasks(
            int pageNumber,
            int pageSize,
            string? search,
            bool? isCompleted);

        Task<TaskResponseDTO?> GetTaskById(int id);

        Task<TaskResponseDTO> CreateTask(
            TaskCreateDTO dto);

        Task<TaskResponseDTO?> UpdateTask(
            int id,
            TaskUpdateDTO dto);

        Task<bool> DeleteTask(int id);
    }
}