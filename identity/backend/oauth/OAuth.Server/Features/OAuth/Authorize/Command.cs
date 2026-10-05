using OAuth.Server.Features.OAuth.Authorize;

namespace OAuth.Server.Features.OAuth
{
    public record Command(Request req, CancellationToken ct);
}