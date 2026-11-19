using FastEndpoints;
using Wolverine;

namespace Product.Web.Api.Features.Products.GetList
{
    public sealed class Endpoint(IMessageBus messageBus) : EndpointWithoutRequest<Response>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Get("products");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await _messageBus.InvokeAsync<Response>(ct);
            await Send.OkAsync(response, ct);
        }
    }
}