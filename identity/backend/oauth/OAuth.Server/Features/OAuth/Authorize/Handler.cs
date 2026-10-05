using System.Text.Json;
using System.Web;
using Microsoft.AspNetCore.DataProtection;
using Wolverine;

namespace OAuth.Server.Features.OAuth.Authorize
{
    public class Handler(IDataProtectionProvider dataProtectionProvider) : IWolverineHandler
    {
        public async Task<Response> Handle(Command command)
        {
            var protector = dataProtectionProvider.CreateProtector("oauth");
            var code = new AuthCode
            {
                ClientId = command.req.ClientId,
                CodeChallenge = command.req.CodeChallenge,
                CodeChallengeMethod = command.req.CodeChallengeMethod,
                RedirectUri = command.req.RedirectUri,
                Expiry = DateTime.UtcNow.AddMinutes(5)
            };

            var json = JsonSerializer.Serialize(code);
            var codeString = protector.Protect(json);

            var issuer = HttpUtility.UrlEncode("https://localhost:5001");
            var redirectTarget = $"{command.req.RedirectUri}?code={codeString}&state={command.req.State}&iss={issuer}";

            return new Response
            {
                RedirectTarget = redirectTarget
            };
        }
    }
}