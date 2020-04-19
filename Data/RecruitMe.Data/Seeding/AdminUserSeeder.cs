namespace RecruitMe.Data.Seeding
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    using RecruitMe.Common;
    using RecruitMe.Data.Models;

    public class AdminUserSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            UserManager<ApplicationUser> userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            RoleManager<ApplicationRole> roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            IConfiguration configManager = serviceProvider.GetRequiredService<IConfiguration>();

            await SeedUserAsync(userManager, configManager);
            await SeedRoleAsync(roleManager, GlobalConstants.AdministratorRoleName);
            await SeedUserToRoleAsync(dbContext, userManager, roleManager, configManager, GlobalConstants.AdministratorRoleName);
            await SeedUserToRoleAsync(dbContext, userManager, roleManager, configManager, GlobalConstants.EmployerRoleName);
            await SeedUserToRoleAsync(dbContext, userManager, roleManager, configManager, GlobalConstants.CandidateRoleName);
        }

        private static async Task SeedUserAsync(UserManager<ApplicationUser> userManager, IConfiguration configManager)
        {
            ApplicationUser user = await userManager.FindByNameAsync(configManager["DefaultAdminCredentials:Username"]);
            if (user == null)
            {
                IdentityResult result = await userManager.CreateAsync(
                     new ApplicationUser
                     {
                         Email = "admin@recruit.me",
                         UserName = "admin@recruit.me",
                         EmailConfirmed = true,
                     },
                     configManager["DefaultAdminCredentials:Password"]);

                ValidateResults(result);
            }
        }

        private static async Task SeedRoleAsync(RoleManager<ApplicationRole> roleManager, string roleName)
        {
            ApplicationRole role = await roleManager.FindByNameAsync(roleName);
            if (role == null)
            {
                IdentityResult result = await roleManager.CreateAsync(new ApplicationRole(roleName));
                ValidateResults(result);
            }
        }

        private static async Task SeedUserToRoleAsync(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, IConfiguration configManager, string roleName)
        {
            ApplicationUser user = await userManager.FindByNameAsync(configManager["DefaultAdminCredentials:Username"]);
            ApplicationRole role = await roleManager.FindByNameAsync(roleName);

            if (!dbContext.UserRoles.Any(ur => ur.RoleId == role.Id && ur.UserId == user.Id))
            {
                await dbContext.UserRoles.AddAsync(new IdentityUserRole<string> { UserId = user.Id, RoleId = role.Id });
            }
        }

        private static void ValidateResults(IdentityResult result)
        {
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
            }
        }
    }
}
