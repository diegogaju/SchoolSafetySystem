using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolSafetySystem.Models;
using SchoolSafetySystem.ViewModels;

namespace SchoolSafetySystem.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UsersController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Index()
        {
            var users = await _userManager.Users.ToListAsync();
            var vms = new List<UserManageViewModel>();
            foreach (var u in users)
            {
                var roles = await _userManager.GetRolesAsync(u);
                vms.Add(new UserManageViewModel
                {
                    Id = u.Id,
                    FullName = u.FullName,
                    Email = u.Email!,
                    Role = roles.FirstOrDefault() ?? "None",
                    CreatedAt = u.CreatedAt
                });
            }
            return View(vms);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var roles = await _userManager.GetRolesAsync(user);
            ViewBag.AllRoles = _roleManager.Roles.Select(r => r.Name).ToList();
            ViewBag.CurrentRole = roles.FirstOrDefault();
            return View(new UserManageViewModel { Id = user.Id, FullName = user.FullName, Email = user.Email! });
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditRole(string id, string role)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null) return NotFound();
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);
            await _userManager.AddToRoleAsync(user, role);
            TempData["Success"] = "Role updated.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null) await _userManager.DeleteAsync(user);
            return RedirectToAction(nameof(Index));
        }
    }
}
