namespace Identity.Web.Api.Features.UserProfile.Edit
{
    public sealed record Command(int userId, CancellationToken ct);

}