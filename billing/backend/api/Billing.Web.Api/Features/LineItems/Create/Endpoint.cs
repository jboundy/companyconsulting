using Billing.Domain.Models;
using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.LineItems.Create
{
    public sealed class Endpoint(IMessageBus messageBus) : Endpoint<Request, Response, CreateLineItemMapper>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Post("lineItems");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            List<LineItem> entities = Map.ToEntity(req);
            var response = await _messageBus.InvokeAsync<Response>(new Command(entities), ct);
            await Send.OkAsync(response, ct);
        }
    }
}