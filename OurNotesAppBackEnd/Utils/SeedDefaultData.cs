using Microsoft.AspNetCore.Identity;
using OurNotesAppBackEnd.Identity;

namespace OurNotesAppBackEnd.Utils;

public static class SeedDefaultData
{
    public static async Task CreateDefaultRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { Roles.Admin, Roles.User };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.ToString()))
            {
                await roleManager.CreateAsync(new IdentityRole(role.ToString()));
            }
        }
    }

    public static async Task CreateDefaultAdminUser(IServiceProvider serviceProvider, IConfiguration configuration)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        string? userName = configuration["UserCredentials:UserName"];
        string? userEmail = configuration["UserCredentials:UserEmail"]; 
        string? userPassword = configuration["UserCredentials:UserPassword"]; 
    
        if (await userManager.FindByEmailAsync(userEmail) == null)
        {
            var user = new AppUser()
            {
                UserName = userEmail,
                Email = userEmail
            };

            var userCreationResult = await userManager.CreateAsync(user, userPassword);
            if (!userCreationResult.Succeeded)
            {
                foreach (var error in userCreationResult.Errors)
                {
                    Console.WriteLine($"Error during User creation: {error.Description}");
                }

                return;
            }

            var addUserToRoleResult = await userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            if (!addUserToRoleResult.Succeeded)
            {
                foreach (var error in addUserToRoleResult.Errors)
                {
                    Console.WriteLine($"Error during User addition to the Role: {error.Description}");
                }
            }
        }
    }
}