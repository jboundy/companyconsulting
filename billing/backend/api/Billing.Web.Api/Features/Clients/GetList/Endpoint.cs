using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Clients.GetList
{
    public sealed class Endpoint(IMessageBus messageBus) : EndpointWithoutRequest<Response>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Get("clients");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await _messageBus.InvokeAsync<Response>(ct);
            await Send.OkAsync(response, ct);
        }
    }
}