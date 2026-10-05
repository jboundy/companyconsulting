using Billing.Domain.Models;
using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.LineItems.UpdateLineItem
{
    public sealed class Endpoint : Endpoint<Request, Response, UpdateLineItemMapper>
    {
        private IMessageBus _messageBus;

        public Endpoint(IMessageBus messageBus)
        {
            _messageBus = messageBus;
        }
        public override void Configure()
        {
            Put("lineItems");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            LineItem entity = Map.ToEntity(req);
            var response = await _messageBus.InvokeAsync<Response>(new Command(entity, ct));
            await Send.OkAsync(response, ct);
        }
    }
}