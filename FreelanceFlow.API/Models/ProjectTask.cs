namespace FreelanceFlow.API.Models
{
    public enum TaskPriority
    {
        Low,
        Medium,
        High
    }

    public enum TaskStatus
    {
        ToDo,
        InProgress,
        Completed
    }

    public class ProjectTask
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;
        public TaskStatus Status { get; set; } = TaskStatus.ToDo;
        public decimal? EstimatedHours { get; set; }

        // Every Task belongs to exactly one Project.
        public int ProjectId { get; set; }
        public Project? Project { get; set; }
    }
}