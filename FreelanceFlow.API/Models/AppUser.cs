using Microsoft.AspNetCore.Identity;

namespace FreelanceFlow.API.Models
{
    public class AppUser:IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
    }
}
