using System.Security.Claims;
using System.Web;
using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace OAuth.Server.Features.OAuth.Authorize
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
            Get("/connect/authorize");
            Post("/connect/authorize");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ApplicationScheme);
            if (!result.Succeeded || result.Principal == null)
            {
                var returnUrl = HttpContext.Request.GetEncodedUrl();
                await Send.RedirectAsync($"/login?returnUrl={HttpUtility.UrlEncode(returnUrl)}");
                return;
            }

            var user = await _userManager.GetUserAsync(result.Principal);
            if (user == null)
            {
                var returnUrl = HttpContext.Request.GetEncodedUrl();
                await Send.RedirectAsync($"/login?returnUrl={HttpUtility.UrlEncode(returnUrl)}");
                return;
            }

            // 4. Create the identity & claims to embed into the issued Authorization Code
            var identity = new ClaimsIdentity(
                authenticationType: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme,
                nameType: OpenIddictConstants.Claims.Name,
                roleType: OpenIddictConstants.Claims.Role);

            identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id);
            identity.AddClaim(OpenIddictConstants.Claims.Email, user.Email ?? string.Empty);
            identity.AddClaim(OpenIddictConstants.Claims.Name, user.UserName ?? string.Empty);

            var claimsPrincipal = new ClaimsPrincipal(identity);
            claimsPrincipal.SetScopes(request.GetScopes());
            await Send.ResultAsync(Results.SignIn(
                claimsPrincipal,
                properties: null,
                authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme));
        }
    }
}