using System.ComponentModel.DataAnnotations;
using FreelanceFlow.API.Models;

namespace FreelanceFlow.API.DTOs.Task
{
    public class UpdateTaskDto
    {
        [Required]
        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        public Models.TaskStatus Status { get; set; } = Models.TaskStatus.ToDo;

        [Range(0, double.MaxValue)]
        public decimal? EstimatedHours { get; set; }
    }
}