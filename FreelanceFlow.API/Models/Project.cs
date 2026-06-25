namespace FreelanceFlow.API.Models
{
    public enum ProjectStatus
    {
        Active,
        OnHold,
        Completed,
        Cancelled
    }

    public class Project
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal Budget { get; set; }
        public ProjectStatus Status { get; set; } = ProjectStatus.Active;

        // Every Project belongs to exactly one Client.
        public int ClientId { get; set; }
        public Client? Client { get; set; }

        // A Project can have many Tasks and many Invoices.
        public ICollection<ProjectTask> Tasks { get; set; } = [];
        public ICollection<Invoice> Invoices { get; set; } = [];
    }
}