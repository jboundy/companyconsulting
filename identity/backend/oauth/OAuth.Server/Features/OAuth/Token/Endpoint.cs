using System.Security.Claims;
using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace OAuth.Server.Features.OAuth.Token
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
            Post("/connect/token");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // 1. Extract the OpenIddict request validated by OpenIddict's middleware
            var request = HttpContext.GetOpenIddictServerRequest()
                ?? throw new InvalidOperationException("The OpenID Connect request cannot be retrieved.");

            // 2. Handle Authorization Code Flow
            if (request.IsAuthorizationCodeGrantType())
            {
                // Retrieve the ClaimsPrincipal attached to the authorization code
                var result = await HttpContext.AuthenticateAsync(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                var userId = result.Principal?.GetClaim(OpenIddictConstants.Claims.Subject);

                var user = await _userManager.FindByIdAsync(userId!);
                if (user == null)
                {
                    await Send.ResultAsync(Results.Challenge(
                        authenticationSchemes: new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme },
                        properties: new AuthenticationProperties(new Dictionary<string, string?>
                        {
                            ["grant"] = OpenIddictConstants.Errors.InvalidGrant,
                            ["description"] = "The token is no longer valid."
                        })));
                    return;
                }

                // Create identity & claims for the issued Access Token
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                identity.AddClaim(OpenIddictConstants.Claims.Subject, user.Id);
                identity.AddClaim(OpenIddictConstants.Claims.Email, user.Email ?? string.Empty);

                var claimsPrincipal = new ClaimsPrincipal(identity);

                // Set target destinations (ID Token vs Access Token)
                claimsPrincipal.SetScopes(request.GetScopes());

                // 3. Return SignIn -> OpenIddict intercepts this, generates JWT via Marten TokenStore, and outputs OIDC JSON
                await Send.ResultAsync(Results.SignIn(claimsPrincipal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme));
                return;
            }

            // 4. Handle Client Credentials Grant Type (App-to-App)
            if (request.IsClientCredentialsGrantType())
            {
                var identity = new ClaimsIdentity(OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
                identity.AddClaim(OpenIddictConstants.Claims.Subject, request.ClientId!);

                var claimsPrincipal = new ClaimsPrincipal(identity);
                claimsPrincipal.SetScopes(request.GetScopes());

                await Send.ResultAsync(Results.SignIn(claimsPrincipal, authenticationScheme: OpenIddictServerAspNetCoreDefaults.AuthenticationScheme));
                return;
            }

            throw new InvalidOperationException("The specified grant type is not supported.");
        }
    }
}