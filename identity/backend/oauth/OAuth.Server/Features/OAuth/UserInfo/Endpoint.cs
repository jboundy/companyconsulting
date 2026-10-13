using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Validation.AspNetCore;

namespace OAuth.Server.Features.OAuth.UserInfo
{
    public class Endpoint : EndpointWithoutRequest
    {
        private readonly UserManager<DomainUser> _userManager;

        public Endpoint(UserManager<DomainUser> userManager)
        {
            _userManager = userManager;
        }

        public override void Configure()
        {
            Get("/connect/userinfo");
            Post("/connect/userinfo");
            AuthSchemes(OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme);
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var userId = User.GetClaim(OpenIddictConstants.Claims.Subject);
            if (string.IsNullOrEmpty(userId))
            {
                await Send.ResultAsync(Results.Challenge(
                    authenticationSchemes: new[] { OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme }));
                return;
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                await Send.ResultAsync(Results.Challenge(
                    authenticationSchemes: new[] { OpenIddictValidationAspNetCoreDefaults.AuthenticationScheme }));
                return;
            }

            var claims = new Dictionary<string, object>
            {
                [OpenIddictConstants.Claims.Subject] = user.Id,
                [OpenIddictConstants.Claims.Email] = user.Email ?? string.Empty,
                [OpenIddictConstants.Claims.Username] = user.UserName ?? string.Empty,
            };

            await Send.OkAsync(claims, ct);
        }
    }
}