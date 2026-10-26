using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;

namespace OAuth.Server.Tools
{
    public class DatabaseSeeder
    {
        private readonly IOpenIddictScopeManager _scopeManager;
        private readonly IOpenIddictApplicationManager _appManager;
        private readonly UserManager<DomainUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseSeeder(
            IOpenIddictScopeManager scopeManager,
            IOpenIddictApplicationManager appManager,
            UserManager<DomainUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _scopeManager = scopeManager;
            _appManager = appManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedAsync()
        {
            if (await _scopeManager.FindByNameAsync("api_scope") == null)
            {
                await _scopeManager.CreateAsync(new OpenIddictScopeDescriptor
                {
                    Name = "api_scope",
                    DisplayName = "Access to Backend APIs",
                    Resources = { "resource_server_api" }
                });
            }

            if (await _appManager.FindByClientIdAsync("web-app") == null)
            {
                await _appManager.CreateAsync(new OpenIddictApplicationDescriptor
                {
                    ClientId = "web-app",
                    ClientSecret = "web-app-secret-key-12345",
                    DisplayName = "Portal Web Application",
                    RedirectUris = { new Uri("https://localhost:7023/signin-oidc"), new Uri("https://oauth.pstmn.io/v1/callback") },
                    PostLogoutRedirectUris = { new Uri("https://localhost:7023/signout-callback-oidc") },
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

            if (await _appManager.FindByClientIdAsync("admin") == null)
            {
                await _appManager.CreateAsync(new OpenIddictApplicationDescriptor
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

            string adminRole = "Administrator";
            if (!await _roleManager.RoleExistsAsync(adminRole))
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
            }

            string adminEmail = "admin@example.com";
            var adminUser = await _userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new DomainUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(adminUser, "P@ssword1!");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(adminUser, adminRole);
                }
            }
        }
    }
}
