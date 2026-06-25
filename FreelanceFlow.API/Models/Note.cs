namespace FreelanceFlow.API.Models
{
    public class Note
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Every Note belongs to exactly one Client.
        public int ClientId { get; set; }
        public Client? Client { get; set; }
    }
}