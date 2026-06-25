namespace FreelanceFlow.API.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }

        // Every Client belongs to exactly one freelancer
        public string UserId { get; set; } = string.Empty;
        public AppUser? User { get; set; }

        // A Client can have many Projects and many Notes.
        public ICollection<Project> Projects { get; set; } = [];
        public ICollection<Note> Notes { get; set; } = [];
    }
}