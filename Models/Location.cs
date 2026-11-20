using System.ComponentModel.DataAnnotations;

namespace SchoolSafetySystem.Models
{
    public class Location
    {
        public int Id { get; set; }

        [Required, StringLength(100)]
        public string Building { get; set; } = string.Empty;

        [StringLength(100)]
        public string? Room { get; set; }

        public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    }
}
