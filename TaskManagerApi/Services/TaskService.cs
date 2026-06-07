using TaskManagerApi.Data;
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

        public List<TaskItem> GetAllTasks()
        {
            return _context.Tasks.ToList();
        }

        public TaskItem CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            _context.SaveChanges();

            return task;
        }

        public TaskItem? UpdateTask(int id, TaskItem updatedTask)
        {
            var existingTask = _context.Tasks.FirstOrDefault(x => x.Id == id);

            if (existingTask == null)
                return null;

            existingTask.Title = updatedTask.Title;
            existingTask.IsCompleted = updatedTask.IsCompleted;

            _context.SaveChanges();

            return existingTask;
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