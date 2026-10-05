using System.Security.Claims;
using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace OAuth.Server.Features.ExternalCallback
{
    public class Endpoint : EndpointWithoutRequest
    {
        private readonly SignInManager<DomainUser> _signInManager;
        private readonly UserManager<DomainUser> _userManager;

        public Endpoint(SignInManager<DomainUser> signInManager, UserManager<DomainUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public override void Configure()
        {
            Get("/api/auth/external-callback");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                await Send.ResponseAsync("Error loading external login information.", 400, ct);
                return;
            }

            var user = await _userManager.FindByLoginAsync(info.LoginProvider, info.ProviderKey);
            if (user == null)
            {
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);
                user = await _userManager.FindByEmailAsync(email!);

                if (user == null)
                {
                    user = new DomainUser { UserName = email, Email = email };
                    await _userManager.CreateAsync(user);
                }
                await _userManager.AddLoginAsync(user, info);
            }

            await _signInManager.SignInAsync(user, false);
            await Send.OkAsync(new { Message = "Authenticated successfully", User = user.Email }, ct);
        }
    }
}