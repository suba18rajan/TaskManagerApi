using System.ComponentModel.DataAnnotations;

namespace TaskManagerApi.DTOs
{
    public class TaskCreateDTO
    {
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }
    }
}