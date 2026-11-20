using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSafetySystem.Models
{
    public enum IncidentStatus { Pending, UnderInvestigation, Resolved }

    public class Incident
    {
        public int Id { get; set; }

        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public DateTime IncidentDate { get; set; }

        public DateTime ReportedAt { get; set; } = DateTime.UtcNow;

        public IncidentStatus Status { get; set; } = IncidentStatus.Pending;

        [Required]
        public string ReporterId { get; set; } = string.Empty;

        [ForeignKey(nameof(ReporterId))]
        public ApplicationUser? Reporter { get; set; }

        public int LocationId { get; set; }

        [ForeignKey(nameof(LocationId))]
        public Location? Location { get; set; }

        public ICollection<IncidentDetail> Details { get; set; } = new List<IncidentDetail>();
        public ICollection<IncidentCategoryMap> IncidentCategoryMaps { get; set; } = new List<IncidentCategoryMap>();
    }
}
