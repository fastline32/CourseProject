using Core.Data.EntryDbModels;
using Microsoft.AspNetCore.Identity;

namespace Core.Data;

public class SeedData
{
    public static async Task SeedRoles(RoleManager<IdentityRole> roleManager)
    {
        if (!roleManager.Roles.Any())
        {
            var roleAdmin = new IdentityRole {Name = "Admin"};
            var roleEditor = new IdentityRole {Name = "Editor"};
            var roleCustomer = new IdentityRole {Name = "Customer"};

            await roleManager.CreateAsync(roleAdmin);
            await roleManager.CreateAsync(roleEditor);
            await roleManager.CreateAsync(roleCustomer);
        }
    }

    public static async Task SeedAdmin(UserManager<ApplicationUser> userManager)
    {
        if (!userManager.Users.Any())
        {
            var adminUser = new ApplicationUser
            {
                DisplayName = "admin",
                Email = "admin@admin.com",
                EmailConfirmed = true,
                FullName = "Admin",
                PhoneNumber = "+359888000888"
            };
            await userManager.CreateAsync(adminUser, "Pa$$w0rd");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }
    }
}