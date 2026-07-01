using System.ComponentModel.DataAnnotations;
using FreelanceFlow.API.Models;

namespace FreelanceFlow.API.DTOs.Project
{
    public class UpdateProjectDto
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        public DateTime? DueDate { get; set; }

        [Range(0, double.MaxValue)]
        public decimal Budget { get; set; }

        [Required]
        public ProjectStatus Status { get; set; }
    }
}