using System.ComponentModel.DataAnnotations;

namespace FreelanceFlow.API.DTOs.Note
{
    public class UpdateNoteDto
    {
        [Required]
        public string Content { get; set; } = string.Empty;
    }
}