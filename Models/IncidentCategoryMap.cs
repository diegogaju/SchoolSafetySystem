using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSafetySystem.Models
{
    public class IncidentCategoryMap
    {
        public int IncidentId { get; set; }

        [ForeignKey(nameof(IncidentId))]
        public Incident? Incident { get; set; }

        public int IncidentCategoryId { get; set; }

        [ForeignKey(nameof(IncidentCategoryId))]
        public IncidentCategory? IncidentCategory { get; set; }
    }
}
