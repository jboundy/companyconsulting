using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Invoices.GetDetail
{
    public sealed class Endpoint(IMessageBus messageBus) : Endpoint<Request, Response>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Get("invoices/{invoiceId}");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var response = await _messageBus.InvokeAsync<Response>(new Command(req.InvoiceId), ct);
            await Send.OkAsync(response, ct);
        }
    }
}