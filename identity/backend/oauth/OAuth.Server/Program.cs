using FastEndpoints;
using Identity.Domain.Models;
using Marten;
using Marten.AspNetIdentity;
using Microsoft.AspNetCore.Identity;
using OAuth.Server.OIDC;
using OpenIddict.Abstractions;
using Wolverine;

namespace OAuth.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Host.UseWolverine(opts =>
            {
                // Enables dynamic runtime compilation
                opts.UseRuntimeCompilation();
            });

        builder.Services.AddOpenApi();
        var connString = builder.Configuration.GetConnectionString("DefaultConnection")!;

        builder.Services.AddMarten(options =>
        {
            options.Connection(connString);
        });

        builder.Services.AddIdentity<DomainUser, IdentityRole>(options =>
        {
            // Configure password/user requirements if needed
        })
        .AddRoles<IdentityRole>()
        .AddMartenStores<DomainUser, IdentityRole>()
        .AddDefaultTokenProviders();

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultScheme = IdentityConstants.ApplicationScheme;
            options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddGoogle(googleOptions =>
        {
            googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"]!;
            googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"]!;
            googleOptions.SignInScheme = IdentityConstants.ExternalScheme;
        })
        .AddFacebook(facebookOptions =>
        {
            facebookOptions.AppId = builder.Configuration["Authentication:Facebook:AppId"]!;
            facebookOptions.AppSecret = builder.Configuration["Authentication:Facebook:AppSecret"]!;
            facebookOptions.SignInScheme = IdentityConstants.ExternalScheme;
        });

        builder.Services.AddOpenIddict()
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

        builder.Services.AddIdentity<DomainUser, IdentityRole>()
            .AddRoles<IdentityRole>()
            .AddMartenStores<DomainUser, IdentityRole>()
            .AddDefaultTokenProviders();

        builder.Services.AddAuthorization();

        builder.Services.AddFastEndpoints(options =>
        {
            options.Assemblies = new[] { typeof(Program).Assembly };
        });

        var app = builder.Build();
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseFastEndpoints();
        app.Run();
    }
}