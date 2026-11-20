using Microsoft.AspNetCore.Identity;
using SchoolSafetySystem.Models;

namespace SchoolSafetySystem.Data
{
    public static class DbSeeder
    {
        public static async Task SeedAsync(IServiceProvider services)
        {
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var db = services.GetRequiredService<ApplicationDbContext>();

            string[] roles = ["Administrator", "SafetyOfficer", "Reporter"];
            foreach (var role in roles)
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));

            // Seed admin
            const string adminEmail = "admin@school.com";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser { UserName = adminEmail, Email = adminEmail, FullName = "System Admin", EmailConfirmed = true };
                await userManager.CreateAsync(admin, "Admin@123");
                await userManager.AddToRoleAsync(admin, "Administrator");
            }

            // Seed categories
            if (!db.IncidentCategories.Any())
            {
                db.IncidentCategories.AddRange(
                    new IncidentCategory { Name = "Bullying", Description = "Physical or verbal bullying" },
                    new IncidentCategory { Name = "Theft", Description = "Stolen property" },
                    new IncidentCategory { Name = "Accident", Description = "Accidental injury" },
                    new IncidentCategory { Name = "Physical Violence", Description = "Physical altercation" },
                    new IncidentCategory { Name = "Vandalism", Description = "Property damage" }
                );
                await db.SaveChangesAsync();
            }

            // Seed locations
            if (!db.Locations.Any())
            {
                db.Locations.AddRange(
                    new Location { Building = "Main Block", Room = "Classroom A1" },
                    new Location { Building = "Main Block", Room = "Classroom A2" },
                    new Location { Building = "Sports Complex", Room = "Field" },
                    new Location { Building = "Library", Room = "Reading Hall" },
                    new Location { Building = "Cafeteria", Room = "Dining Area" }
                );
                await db.SaveChangesAsync();
            }
        }
    }
}
