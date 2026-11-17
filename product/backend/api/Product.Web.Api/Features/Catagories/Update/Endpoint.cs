using Product.Domain.Models;
using FastEndpoints;
using Wolverine;

namespace Product.Web.Api.Features.Categories.Update
{
    public sealed class Endpoint(IMessageBus messageBus) : Endpoint<Request, Response, UpdateCategoryMapper>
    {
        private IMessageBus _messageBus = messageBus;

        public override void Configure()
        {
            Put("categories");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            Category entity = Map.ToEntity(req);
            var response = await _messageBus.InvokeAsync<Response>(new Command(entity), ct);
            await Send.OkAsync(response, ct);
        }
    }
}