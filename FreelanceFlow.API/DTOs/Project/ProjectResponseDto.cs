using FreelanceFlow.API.Models;

namespace FreelanceFlow.API.DTOs.Project
{
    public class ProjectResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal Budget { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;
        public int ClientId { get; set; }
        public string ClientCompanyName { get; set; } = string.Empty;
        public int TaskCount { get; set; }
        public int InvoiceCount { get; set; }
    }
}