using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSafetySystem.Data;
using SchoolSafetySystem.Models;
using SchoolSafetySystem.ViewModels;

namespace SchoolSafetySystem.Controllers
{
    [Authorize]
    public class IncidentsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IWebHostEnvironment _env;

        public IncidentsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager, IWebHostEnvironment env)
        {
            _db = db;
            _userManager = userManager;
            _env = env;
        }

        public async Task<IActionResult> Index(IncidentFilterViewModel filter)
        {
            var query = _db.Incidents
                .Include(i => i.Reporter)
                .Include(i => i.Location)
                .Include(i => i.IncidentCategoryMaps).ThenInclude(m => m.IncidentCategory)
                .AsQueryable();

            if (!User.IsInRole("Administrator") && !User.IsInRole("SafetyOfficer"))
            {
                var userId = _userManager.GetUserId(User);
                query = query.Where(i => i.ReporterId == userId);
            }

            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
                query = query.Where(i => i.Title.Contains(filter.SearchTerm) || i.Description.Contains(filter.SearchTerm));

            if (filter.CategoryId.HasValue)
                query = query.Where(i => i.IncidentCategoryMaps.Any(m => m.IncidentCategoryId == filter.CategoryId));

            if (filter.LocationId.HasValue)
                query = query.Where(i => i.LocationId == filter.LocationId);

            if (filter.FromDate.HasValue)
                query = query.Where(i => i.IncidentDate >= filter.FromDate);

            if (filter.ToDate.HasValue)
                query = query.Where(i => i.IncidentDate <= filter.ToDate);

            if (filter.Status.HasValue)
                query = query.Where(i => i.Status == filter.Status);

            filter.Incidents = await query.OrderByDescending(i => i.ReportedAt).ToListAsync();
            filter.Locations = await _db.Locations.ToListAsync();
            filter.Categories = await _db.IncidentCategories.ToListAsync();

            return View(filter);
        }

        public async Task<IActionResult> Details(int id)
        {
            var incident = await _db.Incidents
                .Include(i => i.Reporter)
                .Include(i => i.Location)
                .Include(i => i.Details)
                .Include(i => i.IncidentCategoryMaps).ThenInclude(m => m.IncidentCategory)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (incident == null) return NotFound();

            if (!User.IsInRole("Administrator") && !User.IsInRole("SafetyOfficer"))
            {
                var userId = _userManager.GetUserId(User);
                if (incident.ReporterId != userId) return Forbid();
            }

            return View(incident);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var vm = new IncidentCreateViewModel
            {
                Locations = await _db.Locations.ToListAsync(),
                Categories = await _db.IncidentCategories.ToListAsync()
            };
            return View(vm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidentCreateViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                vm.Locations = await _db.Locations.ToListAsync();
                vm.Categories = await _db.IncidentCategories.ToListAsync();
                return View(vm);
            }

            var incident = new Incident
            {
                Title = vm.Title,
                Description = vm.Description,
                IncidentDate = vm.IncidentDate,
                LocationId = vm.LocationId,
                ReporterId = _userManager.GetUserId(User)!
            };

            _db.Incidents.Add(incident);
            await _db.SaveChangesAsync();

            foreach (var catId in vm.SelectedCategoryIds)
                _db.IncidentCategoryMaps.Add(new IncidentCategoryMap { IncidentId = incident.Id, IncidentCategoryId = catId });

            if (vm.WitnessName != null || vm.Notes != null || vm.EvidenceFile != null)
            {
                var detail = new IncidentDetail
                {
                    IncidentId = incident.Id,
                    WitnessName = vm.WitnessName,
                    WitnessContact = vm.WitnessContact,
                    Notes = vm.Notes
                };

                if (vm.EvidenceFile != null && vm.EvidenceFile.Length > 0)
                {
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(vm.EvidenceFile.FileName)}";
                    var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
                    using var stream = new FileStream(path, FileMode.Create);
                    await vm.EvidenceFile.CopyToAsync(stream);
                    detail.EvidenceFilePath = $"/uploads/{fileName}";
                }

                _db.IncidentDetails.Add(detail);
            }

            await _db.SaveChangesAsync();
            TempData["Success"] = "Incident reported successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpGet, Authorize(Roles = "Administrator,SafetyOfficer")]
        public async Task<IActionResult> UpdateStatus(int id)
        {
            var incident = await _db.Incidents.FindAsync(id);
            if (incident == null) return NotFound();
            return View(incident);
        }

        [HttpPost, ValidateAntiForgeryToken, Authorize(Roles = "Administrator,SafetyOfficer")]
        public async Task<IActionResult> UpdateStatus(int id, IncidentStatus status)
        {
            var incident = await _db.Incidents.FindAsync(id);
            if (incident == null) return NotFound();
            incident.Status = status;
            await _db.SaveChangesAsync();
            TempData["Success"] = "Status updated.";
            return RedirectToAction(nameof(Details), new { id });
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(int id)
        {
            var incident = await _db.Incidents.Include(i => i.Location).FirstOrDefaultAsync(i => i.Id == id);
            if (incident == null) return NotFound();
            return View(incident);
        }

        [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken, Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var incident = await _db.Incidents.FindAsync(id);
            if (incident != null) _db.Incidents.Remove(incident);
            await _db.SaveChangesAsync();
            TempData["Success"] = "Incident deleted.";
            return RedirectToAction(nameof(Index));
        }
    }
}
