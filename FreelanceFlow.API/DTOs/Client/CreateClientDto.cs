using System.ComponentModel.DataAnnotations;

namespace FreelanceFlow.API.DTOs.Client
{
    public class CreateClientDto
    {
        [Required]
        public string CompanyName { get; set; } = string.Empty;

        [Required]
        public string ContactName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        public string? Phone { get; set; }
    }
}