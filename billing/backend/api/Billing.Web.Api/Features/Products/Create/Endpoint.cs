using Billing.Domain.Models;
using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Products.Create
{
    public sealed class Endpoint(IMessageBus messageBus) : Endpoint<Request, Response, CreateProductMapper>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Post("products");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            Product entity = Map.ToEntity(req);
            var response = await _messageBus.InvokeAsync<Response>(new Command(entity), ct);
            await Send.OkAsync(response, ct);
        }
    }
}