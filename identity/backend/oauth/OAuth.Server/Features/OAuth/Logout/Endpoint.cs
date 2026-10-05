using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Server.AspNetCore;

namespace OAuth.Server.Features.OAuth.Logout
{
    public class Endpoint : EndpointWithoutRequest
    {
        private readonly SignInManager<DomainUser> _signInManager;

        public Endpoint(SignInManager<DomainUser> signInManager) => _signInManager = signInManager;

        public override void Configure()
        {
            Get("/connect/logout");
            Post("/connect/logout");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await _signInManager.SignOutAsync();

            await Send.ResultAsync(Results.SignOut(
                authenticationSchemes: new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme }));
        }
    }
}