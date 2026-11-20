using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSafetySystem.Data;
using SchoolSafetySystem.Models;
using SchoolSafetySystem.ViewModels;

namespace SchoolSafetySystem.Controllers
{
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _db;

        public DashboardController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index()
        {
            var incidents = await _db.Incidents
                .Include(i => i.Location)
                .Include(i => i.IncidentCategoryMaps).ThenInclude(m => m.IncidentCategory)
                .ToListAsync();

            var vm = new DashboardViewModel
            {
                TotalIncidents = incidents.Count,
                PendingCount = incidents.Count(i => i.Status == IncidentStatus.Pending),
                UnderInvestigationCount = incidents.Count(i => i.Status == IncidentStatus.UnderInvestigation),
                ResolvedCount = incidents.Count(i => i.Status == IncidentStatus.Resolved),
                RecentIncidents = incidents.OrderByDescending(i => i.ReportedAt).Take(5).ToList(),
                ByCategory = incidents
                    .SelectMany(i => i.IncidentCategoryMaps)
                    .GroupBy(m => m.IncidentCategory!.Name)
                    .ToDictionary(g => g.Key, g => g.Count())
            };

            return View(vm);
        }
    }
}
