using System.ComponentModel.DataAnnotations;

namespace FreelanceFlow.API.DTOs.Note
{
    public class CreateNoteDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;

        [Required]
        public int ClientId { get; set; }
    }
}