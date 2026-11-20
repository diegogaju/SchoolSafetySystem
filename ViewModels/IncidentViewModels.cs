using System.ComponentModel.DataAnnotations;
using SchoolSafetySystem.Models;

namespace SchoolSafetySystem.ViewModels
{
    public class IncidentCreateViewModel
    {
        [Required, StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required, Display(Name = "Incident Date")]
        public DateTime IncidentDate { get; set; } = DateTime.Today;

        [Required, Display(Name = "Location")]
        public int LocationId { get; set; }

        [Display(Name = "Categories")]
        public List<int> SelectedCategoryIds { get; set; } = new();

        // Witness
        [Display(Name = "Witness Name")]
        public string? WitnessName { get; set; }

        [Display(Name = "Witness Contact")]
        public string? WitnessContact { get; set; }

        [Display(Name = "Evidence File")]
        public IFormFile? EvidenceFile { get; set; }

        [Display(Name = "Additional Notes")]
        public string? Notes { get; set; }

        public List<Location> Locations { get; set; } = new();
        public List<IncidentCategory> Categories { get; set; } = new();
    }

    public class IncidentFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public int? CategoryId { get; set; }
        public int? LocationId { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public IncidentStatus? Status { get; set; }
        public List<Incident> Incidents { get; set; } = new();
        public List<Location> Locations { get; set; } = new();
        public List<IncidentCategory> Categories { get; set; } = new();
    }

    public class DashboardViewModel
    {
        public int TotalIncidents { get; set; }
        public int PendingCount { get; set; }
        public int UnderInvestigationCount { get; set; }
        public int ResolvedCount { get; set; }
        public List<Incident> RecentIncidents { get; set; } = new();
        public Dictionary<string, int> ByCategory { get; set; } = new();
    }
}
