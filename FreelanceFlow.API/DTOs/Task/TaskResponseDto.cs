using FreelanceFlow.API.Models;

namespace FreelanceFlow.API.DTOs.Task
{
    public class TaskResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; }
        public Models.TaskStatus Status { get; set; }
        public decimal? EstimatedHours { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = string.Empty;
    }
}