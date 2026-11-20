using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSafetySystem.Data;
using SchoolSafetySystem.Models;

namespace SchoolSafetySystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class CategoriesController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoriesController(ApplicationDbContext db) => _db = db;

        public async Task<IActionResult> Index() => View(await _db.IncidentCategories.ToListAsync());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IncidentCategory model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.IncidentCategories.Add(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var cat = await _db.IncidentCategories.FindAsync(id);
            return cat == null ? NotFound() : View(cat);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(IncidentCategory model)
        {
            if (!ModelState.IsValid) return View(model);
            _db.IncidentCategories.Update(model);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.IncidentCategories.FindAsync(id);
            if (cat != null) _db.IncidentCategories.Remove(cat);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
