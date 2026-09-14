namespace TaskManagerApi.DTOs
{
    public class PagedTaskResponseDTO
    {
        public List<TaskResponseDTO> Items { get; set; } = new();

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int TotalPages { get; set; }
    }
}