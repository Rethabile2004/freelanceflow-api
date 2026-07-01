namespace FreelanceFlow.API.DTOs.Client
{
    public class ClientResponseDto
    {
        public int Id { get; set; }
        public string CompanyName { get; set; } = string.Empty;
        public string ContactName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public int ProjectCount { get; set; }
    }
}