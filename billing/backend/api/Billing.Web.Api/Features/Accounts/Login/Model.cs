using FastEndpoints;

namespace Billing.Web.Api.Features.Accounts.Login
{
    public sealed class Request
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public sealed class Response
    {
        public required string Token { get; set; }
        public required DateTime ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
        public required UserDTO User { get; set; }
    }

    public sealed class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    public sealed class UserDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}