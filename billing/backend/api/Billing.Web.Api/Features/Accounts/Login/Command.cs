namespace Billing.Web.Api.Features.Accounts.Login
{
    public record Command(Request req, CancellationToken ct);

}