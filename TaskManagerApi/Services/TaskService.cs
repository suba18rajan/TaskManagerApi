using TaskManagerApi.Data;
using TaskManagerApi.DTOs;
using TaskManagerApi.Models;

namespace TaskManagerApi.Services
{
    public class TaskService : ITaskService
    {
        private readonly AppDbContext _context;

        public TaskService(AppDbContext context)
        {
            _context = context;
        }

        public List<TaskResponseDTO> GetAllTasks()
        {
            return _context.Tasks
                .Select(t => new TaskResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    IsCompleted = t.IsCompleted
                })
                .ToList();
        }

        public TaskResponseDTO CreateTask(TaskCreateDTO dto)
        {
            var task = new TaskItem
            {
                Title = dto.Title,
                IsCompleted = false
            };

            _context.Tasks.Add(task);
            _context.SaveChanges();

            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };
        }

        public TaskResponseDTO? UpdateTask(int id, TaskUpdateDTO dto)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.Id == id);

            if (task == null)
                return null;

            task.Title = dto.Title;
            task.IsCompleted = dto.IsCompleted;

            _context.SaveChanges();

            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                IsCompleted = task.IsCompleted
            };
        }

        public bool DeleteTask(int id)
        {
            var task = _context.Tasks.FirstOrDefault(x => x.Id == id);

            if (task == null)
                return false;

            _context.Tasks.Remove(task);
            _context.SaveChanges();

            return true;
        }
    }
}