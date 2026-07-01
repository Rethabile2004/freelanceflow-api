using FreelanceFlow.API.Models;

namespace FreelanceFlow.API.DTOs.Task
{
    public class TaskQueryDto
    {
        public Models.TaskStatus? Status { get; set; }
        public TaskPriority? Priority { get; set; }
        public int? ProjectId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}