using System.IdentityModel.Tokens.Jwt;
using FastEndpoints;

namespace Billing.Web.Api.Features.Accounts.Login
{
    public class Request
    {
        public required string Username { get; set; }
        public required string Password { get; set; }
    }

    public class Response
    {
        public required string Token { get; set; }
        public required DateTime ExpiresAt { get; set; }
        public string? RefreshToken { get; set; }
        public required UserDTO User { get; set; }
    }

    public class Validator : Validator<Request>
    {
        public Validator()
        {

        }
    }

    public class UserDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<string> Roles { get; set; } = [];
    }
}