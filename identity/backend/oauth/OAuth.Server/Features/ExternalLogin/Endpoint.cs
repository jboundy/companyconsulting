using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace OAuth.Server.Features.ExternalLogin
{
    public class Endpoint : Endpoint<Request>
    {
        private readonly SignInManager<DomainUser> _signInManager;

        public Endpoint(SignInManager<DomainUser> signInManager)
        {
            _signInManager = signInManager;
        }

        public override void Configure()
        {
            Get("/api/auth/external-login");
            AllowAnonymous();
        }

        public async override Task HandleAsync(Request req, CancellationToken ct)
        {
            var redirectUrl = "/api/auth/external-callback";
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(req.Provider, redirectUrl);
            await Send.ResultAsync(Results.Challenge(properties, [req.Provider]));
        }
    }
}