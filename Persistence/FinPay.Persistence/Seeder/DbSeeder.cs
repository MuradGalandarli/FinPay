using FinPay.Domain.Identity;
using FinPay.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace FinPay.Persistence.Seeder
{
    public class DbSeeder
    {
        public static async Task SeedData(IApplicationBuilder app)
        {
            // Create a scoped service provider to resolve dependencies
            using var scope = app.ApplicationServices.CreateScope();

            // resolve the logger service
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbSeeder>>();

            try
            {
                // resolve other dependencies
                var userManager = scope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetService<RoleManager<ApplicationRole>>();

                // Check if any users exist to prevent duplicate seeding
                if (userManager.Users.Any() == false)
                {
                    var user = new ApplicationUser
                    {
                        Name = "Admin",
                        UserName = "admin@gmail.com",
                        Email = "admin@gmail.com",
                        EmailConfirmed = true,
                        SecurityStamp = Guid.NewGuid().ToString()
                    };

                    // Create Admin role if it doesn't exist
                    if ((await roleManager.RoleExistsAsync(Role.Admin)) == false)
                    {
                        logger.LogInformation("Admin role is creating");
                        var roleResult = await roleManager
                          .CreateAsync(new ApplicationRole { Name = Role.Admin,Id = Guid.NewGuid().ToString()});

                        if (roleResult.Succeeded == false)
                        {
                            var roleErros = roleResult.Errors.Select(e => e.Description);
                            logger.LogError($"Failed to create admin role. Errors : {string.Join(",", roleErros)}");

                            return;
                        }
                        logger.LogInformation("Admin role is created");
                    }

                    // Attempt to create admin user
                    var createUserResult = await userManager
                          .CreateAsync(user: user, password: "Admin@123");

                    // Validate user creation
                    if (createUserResult.Succeeded == false)
                    {
                        var errors = createUserResult.Errors.Select(e => e.Description);
                        logger.LogError(
                            $"Failed to create admin user. Errors: {string.Join(", ", errors)}"
                        );
                        return;
                    }

                    // adding role to user
                    var addUserToRoleResult = await userManager
                                    .AddToRoleAsync(user: user, role: Role.Admin);

                    if (addUserToRoleResult.Succeeded == false)
                    {
                        var errors = addUserToRoleResult.Errors.Select(e => e.Description);
                        logger.LogError($"Failed to add admin role to user. Errors : {string.Join(",", errors)}");
                    }
                    logger.LogInformation("Admin user is created");
                }
            }

            catch (Exception ex)
            {
                logger.LogCritical(ex.Message);
            }

        }
    }
}
