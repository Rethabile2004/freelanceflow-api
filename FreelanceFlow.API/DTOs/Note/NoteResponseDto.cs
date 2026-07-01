namespace FreelanceFlow.API.DTOs.Note
{
    public class NoteResponseDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public int ClientId { get; set; }
    }
}