using FastEndpoints;
using Wolverine;

namespace Product.Web.Api.Features.Products.GetDetail
{
    public sealed class Endpoint(IMessageBus messageBus) : Endpoint<Request, Response>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Get("products/{productId}");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var response = await _messageBus.InvokeAsync<Response>(new Command(req.ProductId), ct);
            await Send.OkAsync(response, ct);
        }
    }
}