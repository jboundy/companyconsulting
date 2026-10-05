using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using OpenIddict.Abstractions;

namespace OAuth.Server.Tools
{
    public static class DatabaseSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var scopeManager = scope.ServiceProvider.GetRequiredService<IOpenIddictScopeManager>();

            if (await scopeManager.FindByNameAsync("api_scope") == null)
            {
                await scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "api_scope",
                    DisplayName = "Access to Backend APIs",
                    Resources = { "resource_server_api" }
                });
            }
            var appManager = scope.ServiceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            if (await appManager.FindByClientIdAsync("web-app") == null)
            {
                await appManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "web-app",
                    ClientSecret = "web-app-secret-key-12345",
                    DisplayName = "Portal Web Application",
                    RedirectUris =
                {
                    new Uri("https://localhost:7023/signin-oidc"),
                    new Uri("https://oauth.pstmn.io/v1/callback")
                },
                    PostLogoutRedirectUris =
                {
                    new Uri("https://localhost:7023/signout-callback-oidc")
                },
                    Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Authorization,
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.Endpoints.EndSession,
                    OpenIddictConstants.Permissions.GrantTypes.AuthorizationCode,
                    OpenIddictConstants.Permissions.GrantTypes.RefreshToken,
                    OpenIddictConstants.Permissions.ResponseTypes.Code,
                    OpenIddictConstants.Permissions.Scopes.Email,
                    OpenIddictConstants.Permissions.Scopes.Profile,
                    OpenIddictConstants.Permissions.Scopes.Roles,
                    "api_scope"
                }
                });
            }

            if (await appManager.FindByClientIdAsync("admin") == null)
            {
                await appManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "admin",
                    ClientSecret = "admin-super-secret-key-9999",
                    DisplayName = "Admin Worker Service",
                    Permissions =
                {
                    OpenIddictConstants.Permissions.Endpoints.Token,
                    OpenIddictConstants.Permissions.GrantTypes.ClientCredentials,
                    "api_scope"
                }
                });
            }

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<DomainUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            string adminRole = "Administrator";
            if (!await roleManager.RoleExistsAsync(adminRole))
            {
                await roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            string adminEmail = "admin@example.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new DomainUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, "P@ssword123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}