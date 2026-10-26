using Billing.Domain.Models;
using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Products.Update
{
    public sealed class Endpoint(IMessageBus messageBus) : Endpoint<Request, Response, UpdateProductMapper>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Put("products");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            Product entity = Map.ToEntity(req);
            var response = await _messageBus.InvokeAsync<Response>(new Command(entity), ct);
            await Send.OkAsync(response, ct);
        }
    }
}