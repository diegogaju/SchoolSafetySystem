using Microsoft.AspNetCore.Identity;

namespace SchoolSafetySystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }
}
