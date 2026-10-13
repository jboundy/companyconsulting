using Billing.Web.Api.Configuration;
using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Accounts.Login
{
    public sealed class Endpoint(IMessageBus messageBus, JwtSettings jwtSettings) : Endpoint<Request, Response>
    {
        private IMessageBus _messageBus = messageBus;
        private JwtSettings _jwtSettings = jwtSettings;

        public override void Configure()
        {
            Post("account/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            // Build the target login URL on your OAuth server, preserving the return URL if provided
            var loginUrl = $"{_jwtSettings.Authority}/login";
            // if (!string.IsNullOrEmpty(req.ReturnUrl))
            // {
            //     loginUrl += $"?returnUrl={Uri.EscapeDataString(req.ReturnUrl)}";
            // }

            // Issue an HTTP redirect to the OAuth server
            await Send.RedirectAsync(loginUrl);
        }
    }
}