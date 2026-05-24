using TaskManagerApi.Models;

namespace TaskManagerApi.Data
{
    public static class TaskData
    {
        public static List<TaskItem> Tasks = new List<TaskItem>
        {
            new TaskItem
            {
                Id = 1,
                Title = "Learn .NET",
                IsCompleted = false
            },

            new TaskItem
            {
                Id = 2,
                Title = "Learn Git",
                IsCompleted = false
            }
        };
    }
}