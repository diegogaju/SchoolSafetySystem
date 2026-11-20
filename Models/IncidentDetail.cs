using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SchoolSafetySystem.Models
{
    public class IncidentDetail
    {
        public int Id { get; set; }

        public int IncidentId { get; set; }

        [ForeignKey(nameof(IncidentId))]
        public Incident? Incident { get; set; }

        [StringLength(200)]
        public string? WitnessName { get; set; }

        [StringLength(200)]
        public string? WitnessContact { get; set; }

        public string? EvidenceFilePath { get; set; }

        [StringLength(500)]
        public string? Notes { get; set; }
    }
}
