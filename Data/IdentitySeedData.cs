using BlogApp.Models;
using Microsoft.AspNetCore.Identity;

namespace BlogApp.Data;

public class IdentitySeedData {
    public static async Task Initialize(ApplicationDbContext context,
        UserManager<BlogUser> userManager,
        RoleManager<IdentityRole> roleManager) {
        context.Database.EnsureCreated();

        string adminRole = "Admin";
        string contributorRole = "Contributor";
        string password4all = "P@$$w0rd";

        if (await roleManager.FindByNameAsync(adminRole) == null) {
            await roleManager.CreateAsync(new IdentityRole(adminRole));
        }

        if (await roleManager.FindByNameAsync(contributorRole) == null) {
            await roleManager.CreateAsync(new IdentityRole(contributorRole));
        }

        if (await userManager.FindByNameAsync("a@a.a") == null){
            var user = new BlogUser {
                UserName = "a@a.a",
                Email = "a@a.a",
                FirstName = "Adam",
                LastName = "Admin",
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded) {
                await userManager.AddPasswordAsync(user, password4all);
                await userManager.AddToRoleAsync(user, adminRole);
            }
        }

        if (await userManager.FindByNameAsync("c@c.c") == null) {
            var user = new BlogUser {
                UserName = "c@c.c",
                Email = "c@c.c",
                FirstName = "Charles",
                LastName = "Contributor",
            };

            var result = await userManager.CreateAsync(user);
            if (result.Succeeded) {
                await userManager.AddPasswordAsync(user, password4all);
                await userManager.AddToRoleAsync(user, contributorRole);
            }
        }
    }
}