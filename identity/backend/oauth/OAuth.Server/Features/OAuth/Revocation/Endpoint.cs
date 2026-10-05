using FastEndpoints;
using OpenIddict.Server.AspNetCore;

namespace OAuth.Server.Features.OAuth.Revocation
{
    public class Endpoint : EndpointWithoutRequest
    {
        public override void Configure()
        {
            Post("/connect/revocation");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            await Send.ResultAsync(Results.Forbid(
                authenticationSchemes: new[] { OpenIddictServerAspNetCoreDefaults.AuthenticationScheme }));
        }
    }
}