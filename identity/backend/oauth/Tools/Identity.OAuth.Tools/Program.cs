
using Identity.Domain.Models;
using Marten;
using Marten.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuth.Server.OIDC;
using OAuth.Server.Tools;
using OpenIddict.Abstractions;

namespace Identity.OAuth.Tools;

public class Program
{
    public static async Task Main(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        var connectionString = configuration.GetConnectionString("DefaultConnection");
        var services = new ServiceCollection();
        services.AddMarten(connectionString);
        services.AddIdentity<DomainUser, IdentityRole>(options =>
        {
            // Configure identity options if needed (e.g., password policies)
        })
        .AddMartenStores<DomainUser, IdentityRole>()
        .AddDefaultTokenProviders();

        services.AddOpenIddict()
            .AddCore(options =>
            {
                options.SetDefaultApplicationEntity<AppDetail>()
                       .SetDefaultAuthorizationEntity<AuthorizationDetail>()
                       .SetDefaultScopeEntity<ScopeDetail>()
                       .SetDefaultTokenEntity<TokenDetail>();

                options.Services.AddScoped<IOpenIddictApplicationStore<AppDetail>, AppStore>();
                options.Services.AddScoped<IOpenIddictAuthorizationStore<AuthorizationDetail>, AuthorizationStore>();
                options.Services.AddScoped<IOpenIddictScopeStore<ScopeDetail>, ScopeStore>();
                options.Services.AddScoped<IOpenIddictTokenStore<TokenDetail>, TokenStore>();
            })
            .AddServer(options =>
            {
                options.SetAuthorizationEndpointUris("/connect/authorize")
                       .SetTokenEndpointUris("/connect/token")
                       .SetUserInfoEndpointUris("/connect/userinfo")
                       .SetEndSessionEndpointUris("/connect/logout")
                       .SetRevocationEndpointUris("/connect/revocation");

                options.AllowAuthorizationCodeFlow()
                       .RequireProofKeyForCodeExchange()
                       .AllowRefreshTokenFlow()
                       .AllowClientCredentialsFlow();

                options.UseAspNetCore()
                       .EnableAuthorizationEndpointPassthrough()
                       .EnableTokenEndpointPassthrough()
                       .EnableUserInfoEndpointPassthrough()
                       .EnableEndSessionEndpointPassthrough();

                options.AddDevelopmentEncryptionCertificate()
                       .AddDevelopmentSigningCertificate();
            });


        var serviceProvider = services.BuildServiceProvider();

        try
        {
            var scopeManager = serviceProvider.GetRequiredService<IOpenIddictScopeManager>();
            var appManager = serviceProvider.GetRequiredService<IOpenIddictApplicationManager>();
            var userManager = serviceProvider.GetRequiredService<UserManager<DomainUser>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var seeder = new DatabaseSeeder(scopeManager, appManager, userManager, roleManager);
            Console.WriteLine("Starting database seeding...");
            await seeder.SeedAsync();
            Console.WriteLine("Database seeding completed successfully!");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"An error occurred while seeding the database: {ex.Message}");
            Console.ResetColor();
        }
    }
}