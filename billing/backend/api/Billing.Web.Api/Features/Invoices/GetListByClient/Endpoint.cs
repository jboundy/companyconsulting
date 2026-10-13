using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Invoices.GetListByClient
{
    public sealed class Endpoint(IMessageBus messageBus) : Endpoint<Request, Response>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Get("invoices/client/{clientId}");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var response = await _messageBus.InvokeAsync<Response>(new Command(req.ClientId), ct);
            await Send.OkAsync(response, ct);
        }
    }
}