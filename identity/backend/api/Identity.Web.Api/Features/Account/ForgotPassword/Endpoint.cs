using FastEndpoints;
using Identity.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Identity.Web.Api.Features.Account.ForgotPassword
{
    public class Endpoint(UserManager<DomainUser> userManager) : Endpoint<Request>
    {
        private readonly UserManager<DomainUser> _userManager = userManager;

        public override void Configure()
        {
            Post("account/forgotPassword");
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            var domainUser = await _userManager.FindByIdAsync(req.UserId);

            if (domainUser == null)
            {
                await Send.ResponseAsync("Unable to find user", 400, ct);
                return;
            }

            var result = await _userManager.GeneratePasswordResetTokenAsync(domainUser);

            if (result.Count() == 0)
            {
                await Send.ResponseAsync("Unable to generate forgot password link.", 400, ct);
                return;
            }

            await Send.OkAsync(null, ct);
        }
    }
}