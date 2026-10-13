using Billing.Domain.Models;
using FastEndpoints;
using Wolverine;

namespace Billing.Web.Api.Features.Clients.Create
{
    public sealed class Endpoint(IMessageBus messageBus) : Endpoint<Request, Response, CreateClientMapper>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Post("client");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            Client entity = Map.ToEntity(req);
            var response = await _messageBus.InvokeAsync<Response>(new Command(entity), ct);
            await Send.OkAsync(response, ct);
        }
    }
}