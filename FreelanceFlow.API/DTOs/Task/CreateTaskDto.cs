using System.ComponentModel.DataAnnotations;
using FreelanceFlow.API.Models;

namespace FreelanceFlow.API.DTOs.Task
{
    public class CreateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        [Range(0, double.MaxValue)]
        public decimal? EstimatedHours { get; set; }

        [Required]
        public int ProjectId { get; set; }
    }
}