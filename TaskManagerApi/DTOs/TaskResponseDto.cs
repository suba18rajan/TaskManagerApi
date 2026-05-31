namespace TaskManagerApi.DTOs
{
    public class TaskResponseDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
