using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSafetySystem.Data;
using SchoolSafetySystem.Models;

namespace SchoolSafetySystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class LocationsController : Controller
    {
        private readonly ApplicationDbContext _db;
        public LocationsController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index() => View(await _db.Locations.ToListAsync());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Location model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Locations.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var loc = await _db.Locations.FindAsync(id);
            return loc == null ? NotFound() : View(loc);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Location model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.Locations.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var loc = await _db.Locations.FindAsync(id);
            if (loc != null) _db.Locations.Remove(loc);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
