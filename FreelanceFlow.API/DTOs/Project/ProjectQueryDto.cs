using FreelanceFlow.API.Models;

namespace FreelanceFlow.API.DTOs.Project
{
    public class ProjectQueryDto
    {
        public ProjectStatus? Status { get; set; }
        public int? ClientId { get; set; }
        public string? Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}