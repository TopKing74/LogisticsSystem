using LogisticsSystem.Domain.Contracts;
using LogisticsSystem.Domain.Models;
using LogisticsSystem.Domain.Models.Identity;
using LogisticsSystem.Persistence.Contexts;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LogisticsSystem.Persistence.Seed;

public class ContextSeed(
    ApplicationDbContext context,
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<int>> roleManager) : IContextSeed
{
    public async Task SeedAsync()
    {
        // 1. Roles Seeding
        if (!await roleManager.Roles.AnyAsync())
        {
            await roleManager.CreateAsync(new IdentityRole<int>("Admin"));
            await roleManager.CreateAsync(new IdentityRole<int>("DeliveryAgent"));
            await roleManager.CreateAsync(new IdentityRole<int>("Customer"));
        }

        // 2. Admin Seeding
        if (await userManager.FindByEmailAsync("admin@logistics.com") is null)
        {
            var adminUser = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@logistics.com",
                EmailConfirmed = true,
                DisplayName = "Admin"
            };

            var result = await userManager.CreateAsync(adminUser, "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }

        // 3. Warehouses Seeding
        if (!await context.Warehouses.AnyAsync())
        {
            var warehouses = new List<Warehouse>
        {
            new() { Name = "Alexandria Hub", Address = "Port Said St, Alexandria, Egypt", capacity = 5000 },
            new() { Name = "Cairo Central", Address = "Tahrir Square, Cairo, Egypt", capacity = 10000 }
        };

            await context.Warehouses.AddRangeAsync(warehouses);
            await context.SaveChangesAsync();
        }
    }
}