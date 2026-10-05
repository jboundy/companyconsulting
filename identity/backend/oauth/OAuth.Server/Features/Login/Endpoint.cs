using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace OAuth.Server.Features.Login
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
            Post("/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var result = await _signInManager.PasswordSignInAsync(
                req.Email, req.Password, true, true);

            if (!result.Succeeded)
            {
                await Send.ResponseAsync("Invalid username or password.", 400, ct);
                return;
            }

            var redirectUrl = !string.IsNullOrEmpty(req.ReturnUrl) ? req.ReturnUrl : "/";
            await Send.RedirectAsync(redirectUrl);
        }
    }
}