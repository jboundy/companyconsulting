using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Accounts.Login
{
    public class Endpoint : Endpoint<Request, Response>
    {
        private IMessageBus _messageBus;

        public Endpoint(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        public override void Configure()
        {
            Post("account/login");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var oauthServerUrl = "https://localhost:5001"; // Or read from appsettings.json

            // Build the target login URL on your OAuth server, preserving the return URL if provided
            var loginUrl = $"{oauthServerUrl}/login";
            // if (!string.IsNullOrEmpty(req.ReturnUrl))
            // {
            //     loginUrl += $"?returnUrl={Uri.EscapeDataString(req.ReturnUrl)}";
            // }

            // Issue an HTTP redirect to the OAuth server
            await Send.RedirectAsync(loginUrl);
        }
    }
}