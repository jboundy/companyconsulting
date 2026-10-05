using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Invoices.GetUnpaidInvoices
{
    public sealed class Endpoint : EndpointWithoutRequest<Response>
    {
        private IMessageBus _messageBus;

        public Endpoint(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        public override void Configure()
        {
            Get("invoice/unpaid");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await _messageBus.InvokeAsync<Response>(new Command(ct));
            await Send.OkAsync(response, ct);
        }
    }
}