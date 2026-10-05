using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Invoices.GetClientInvoices
{
    public sealed class Endpoint : Endpoint<Request, Response>
    {
        private IMessageBus _messageBus;

        public Endpoint(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        public override void Configure()
        {
            Get("invoices/client/{clientId}");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var response = await _messageBus.InvokeAsync<Response>(new Command(req.ClientId, ct));
            await Send.OkAsync(response, ct);
        }
    }
}